using Newtonsoft.Json;
using YWML.Src.ConfigManager;
using YWML.Src.ExtensionLibrary;
using YWML.Src.ExtensionLibrary.DataClasses;
using YWML.Src.Install;
using YWML.Src.Install.DataClasses;
using YWML.Src.Loader;
using YWML.Src.Loader.Archive;
using YWML.Src.Loader.DataClasses;
using YWML.Src.RemoteInstall;
using YWML.Src.RemoteInstall.DataClasses;
using YWML.Src.Utils.GeneralUtils;

namespace YWML.Src.Forms.LoadForm
{
    public partial class LoadForm : Form
    {
        private CExtensionLibrary _lib;
        private Dictionary<string, string> _installDirs;
        private Dictionary<string, string> _nameToId = new();
        private Dictionary<string, string> _modNameToPath = new();
        private SFtpConnectionInfo? _ftpConnectionInfo;
        private readonly IFtpTransport _ftpTransport = new CFtpTransport();
        private readonly List<string> _archiveStagingDirs = new();
        private Font _installPathNormalFont;
        private Font _installPathItalicFont;
        private Color _installPathNormalForeColor;
        private bool _suppressInstallPathUpdate;
        private const string WRONG_STRUCT_MSG = "YWML project configuration exists (ywml.json), but is not structured correctly: ";
        private const string ADD_GAME_ITEM = "Can't find your game/region? Add it from here";
        private const string REMOTE_DISABLED_MSG = "Disabled for remote install";

        public LoadForm()
        {
            InitializeComponent();
            RefreshModListButtonsState();
            _lib = new CExtensionLibrary();
            modsTreeView.ShowNodeToolTips = true;
            this.FormClosing += LoadForm_FormClosing;

            _installPathNormalFont = modInstallPathTextBox.Font;
            _installPathItalicFont = new Font("Consolas", 8F, FontStyle.Italic);
            _installPathNormalForeColor = modInstallPathTextBox.ForeColor;

            if (File.Exists(CGeneralUtils.DefaultInstallationDirectoriesPath))
            {
                _installDirs = JsonConvert.DeserializeObject<Dictionary<string, string>>(
                    File.ReadAllText(CGeneralUtils.DefaultInstallationDirectoriesPath)) ?? new();
            }
            else
            {
                _installDirs = new();
            }

            if (CConfigManager.Cfg.LastUsedInstallMode == SInstallMode.Local)
            {
                localModeRadio.Checked = true;
            }
            else
            {
                remoteModeRadio.Checked = true;
            }

            RefreshInstalledExtensions();
            UpdateInstallControlsForMode();
        }

        private bool IsRemoteMode => remoteModeRadio.Checked;

        private void RefreshModListButtonsState()
        {
            bool hasSelection = modsTreeView.SelectedNode != null;

            removeSelectedModBtn.Enabled = hasSelection;
            moveUpSelectedModBtn.Enabled = hasSelection && modsTreeView.SelectedNode.Index > 0;
            moveDownSelectedModBtn.Enabled = hasSelection && modsTreeView.SelectedNode.Index < modsTreeView.Nodes.Count - 1;
        }

        public void LoadForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            File.WriteAllText(
                CGeneralUtils.DefaultInstallationDirectoriesPath,
                JsonConvert.SerializeObject(_installDirs));

            CConfigManager.Cfg.LastUsedInstallMode = IsRemoteMode ? SInstallMode.Remote : SInstallMode.Local;
            CConfigManager.UpdateConfig();

            foreach (var stagingDir in _archiveStagingDirs)
            {
                try
                {
                    if (Directory.Exists(stagingDir))
                    {
                        Directory.Delete(stagingDir, true);
                    }
                }
                catch
                {
                    
                }
            }
        }

        private void installModeRadio_CheckedChanged(object sender, EventArgs e)
        {
            UpdateInstallControlsForMode();
        }

        private void UpdateInstallControlsForMode()
        {
            var isRemote = IsRemoteMode;
            var selectedExt = GetSelectedExt();
            var autoDetectDisabledForGame = selectedExt != null && selectedExt.IsDisableAutoInstall;

            browseBtn.Enabled = CInstallModeRules.IsLocalControlEnabled(isRemote);
            autoInstallDirBtn.Enabled = CInstallModeRules.IsAutoDetectEnabled(isRemote, autoDetectDisabledForGame);
            modInstallPathTextBox.Enabled = CInstallModeRules.IsLocalControlEnabled(isRemote);

            _suppressInstallPathUpdate = true;
            if (isRemote)
            {
                modInstallPathTextBox.Font = _installPathItalicFont;
                modInstallPathTextBox.ForeColor = SystemColors.GrayText;
                modInstallPathTextBox.Text = REMOTE_DISABLED_MSG;
            }
            else
            {
                modInstallPathTextBox.Font = _installPathNormalFont;
                modInstallPathTextBox.ForeColor = _installPathNormalForeColor;
                modInstallPathTextBox.Text = GetSelectedInstallDir();
            }
            _suppressInstallPathUpdate = false;
        }

        private string GetSelectedInstallDir()
        {
            if (extensionComboBox.SelectedItem is not string selectedName)
            {
                return string.Empty;
            }

            return _installDirs.TryGetValue(_nameToId[selectedName], out var dir) ? dir : string.Empty;
        }

        private void SetLocalInstallDir(string path)
        {
            if (extensionComboBox.SelectedItem is string selectedName)
            {
                _installDirs[_nameToId[selectedName]] = path;
            }

            _suppressInstallPathUpdate = true;
            modInstallPathTextBox.Text = path;
            _suppressInstallPathUpdate = false;
        }

        private void browseBtn_Click(object sender, EventArgs e)
        {
            var selFolder = CGeneralUtils.ChooseFolder("Select the folder you want to install your mods to");
            if (selFolder == null) return;
            SetLocalInstallDir(selFolder);
        }

        private void SetInstallingState(bool installing)
        {
            installBtn.Enabled = !installing;
            installBtn.Text = installing ? "Installing..." : "INSTALL MODS";
        }

        private void RefreshInstalledExtensions()
        {
            var previouslySelected = extensionComboBox.SelectedItem as string;

            _lib.LoadInstalledList();
            _nameToId.Clear();
            extensionComboBox.Items.Clear();

            foreach (var key in _lib.InstalledList.Keys)
            {
                _nameToId[_lib.InstalledList[key].Name] = key;
                extensionComboBox.Items.Add(_lib.InstalledList[key].Name);
            }

            extensionComboBox.Items.Add(ADD_GAME_ITEM);

            if (previouslySelected != null
                && previouslySelected != ADD_GAME_ITEM
                && extensionComboBox.Items.Contains(previouslySelected))
            {
                extensionComboBox.SelectedItem = previouslySelected;
            }
        }

        private void addModBtn_Click(object sender, EventArgs e)
        {
            modsTreeView.SelectedNode = null;
            RefreshModListButtonsState();
            addModCtxMenu.Show(Cursor.Position);
        }

        private void removeSelectedModBtn_Click(object sender, EventArgs e)
        {
            if (modsTreeView.SelectedNode != null)
            {
                modsTreeView.Nodes.Remove(modsTreeView.SelectedNode);
                modsTreeView.SelectedNode = null; 
                RefreshModListButtonsState();
            }
        }

        private void extensionComboBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            var isEditPortion = (e.State & DrawItemState.ComboBoxEdit) == DrawItemState.ComboBoxEdit;

            if (e.Index < 0 || isEditPortion)
            {
                using var backgroundBrush = new SolidBrush(extensionComboBox.BackColor);
                e.Graphics.FillRectangle(backgroundBrush, e.Bounds);
            }
            else
            {
                e.DrawBackground();
            }

            if (e.Index < 0)
            {
                return;
            }

            var text = extensionComboBox.Items[e.Index]?.ToString() ?? string.Empty;
            var isAddItem = text == ADD_GAME_ITEM;
            var isHighlighted = !isEditPortion && (e.State & DrawItemState.Selected) == DrawItemState.Selected;
            var normalForeColor = extensionComboBox.BackColor.GetBrightness() < 0.5f ? Color.White : Color.Black;

            Color foreColor;
            if (isAddItem)
            {
                foreColor = isHighlighted ? SystemColors.HighlightText : Color.DeepSkyBlue;
            }
            else
            {
                foreColor = isHighlighted ? SystemColors.HighlightText : normalForeColor;
            }

            var flags = TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis;

            if (isAddItem)
            {
                using var boldFont = new Font(e.Font, FontStyle.Bold);
                TextRenderer.DrawText(e.Graphics, text, boldFont, e.Bounds, foreColor, flags);
            }
            else
            {
                TextRenderer.DrawText(e.Graphics, text, e.Font, e.Bounds, foreColor, flags);
            }

            if (!isEditPortion)
            {
                e.DrawFocusRectangle();
            }
        }

        private void extensionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedName = extensionComboBox.SelectedItem as string;

            if (selectedName == ADD_GAME_ITEM)
            {
                extensionComboBox.SelectedIndex = -1;
                new ExtensionLibraryForm().ShowDialog();
                RefreshInstalledExtensions();
                UpdateInstallControlsForMode();
                return;
            }

            UpdateInstallControlsForMode();
        }

        private CInstalledExtensionMetadata GetSelectedExt()
        {
            return _lib.InstalledList.Values.ToList().Find(match: e => e.Name == extensionComboBox.SelectedItem);
        }

        private async void installBtn_Click(object sender, EventArgs e)
        {
            if (modsTreeView.Nodes.Count == 0)
            {
                MessageBox.Show("Please add at least one mod to the list to begin patching.");
                return;
            }
            if (extensionComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please select a target game.");
                return;
            }

            var selectedId = _nameToId[extensionComboBox.SelectedItem as string];
            var selectedExtension = _lib.InstalledList[selectedId];

            IModInstallTarget target;
            if (IsRemoteMode)
            {
                if (_ftpConnectionInfo == null)
                {
                    var connectionForm = new FtpConnectionForm(selectedExtension.TitleId, _ftpConnectionInfo);
                    if (connectionForm.ShowDialog() != DialogResult.OK || connectionForm.ConnectionInfo == null)
                    {
                        return;
                    }
                    _ftpConnectionInfo = connectionForm.ConnectionInfo;
                }

                var remoteRoot = CRemotePath.GetModded3dsRomfsRoot(selectedExtension.TitleId);
                target = new CFtpInstallTarget(_ftpTransport, _ftpConnectionInfo.Value, remoteRoot);
            }
            else
            {
                if (!_installDirs.TryGetValue(selectedId, out var localDir) || string.IsNullOrWhiteSpace(localDir))
                {
                    MessageBox.Show("Please select a mod installation directory.");
                    return;
                }

                if (!localDir.Contains(selectedExtension.TitleId))
                {
                    DialogResult res = MessageBox.Show(
                        "Your selected mod installation directory DOES NOT contain your selected game's title ID.\nThis likely means this folder is NOT the correct mod installation directory. \n\nIf you are aware of this and know what you are doing, Continue. Else, Fix it.\n\nWould you like to continue?",
                        "YWML",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning,
                        MessageBoxDefaultButton.Button2
                    );

                    if (res == DialogResult.No)
                    {
                        return;
                    }
                }

                target = new CLocalInstallTarget(localDir);
            }

            var faToLoad = Path.Combine(CGeneralUtils.ExtensionInstallDirectory, selectedId, "patchable.fa");
            var result = CLoader.ModifyFA(modsTreeView, _modNameToPath, faToLoad);
            var rawFiles = result.RawFiles;
            var modifiedFA = result.Archive.Save();

            SetInstallingState(true);
            try
            {
                var status = new Progress<string>(message => installBtn.Text = message);
                var percent = new Progress<int>(percentage => installBtn.Text = $"Uploading... {percentage}%");
                await target.InstallAsync(modifiedFA, selectedExtension.FAName, rawFiles, status, percent);
                MessageBox.Show("Loaded all mods. Enjoy your game!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while installing mods:\n\n{ex.Message}");
            }
            finally
            {
                SetInstallingState(false);
                result.Archive.BaseStream.Close();
            }
        }

        private void moveUpSelectedModBtn_Click(object sender, EventArgs e)
        {
            MoveSelectedNodePosition(true);
        }

        private void moveDownSelectedModBtn_Click(object sender, EventArgs e)
        {
            MoveSelectedNodePosition(false);
        }

        private void MoveSelectedNodePosition(bool isMoveUp)
        {
            TreeNode node = modsTreeView.SelectedNode;
            if (node == null) return;

            int index = node.Index;
            int count = modsTreeView.Nodes.Count;

            bool canMoveUp = isMoveUp && index > 0;
            bool canMoveDown = !isMoveUp && index < count - 1;

            if (canMoveUp || canMoveDown)
            {
                modsTreeView.BeginUpdate();
                modsTreeView.Nodes.Remove(node);
                modsTreeView.Nodes.Insert(isMoveUp ? index - 1 : index + 1, node);
                modsTreeView.EndUpdate();
            }

            modsTreeView.Focus();
            modsTreeView.SelectedNode = node;
            node.EnsureVisible();
            RefreshModListButtonsState();
        }

        private void LoadForm_Load(object sender, EventArgs e)
        {
        }

        private void modInstallPathTextBox_TextChanged(object sender, EventArgs e)
        {
            if (_suppressInstallPathUpdate) return;
            if (IsRemoteMode) return;
            if (extensionComboBox.SelectedItem is not string selectedName) return;

            _installDirs[_nameToId[selectedName]] = modInstallPathTextBox.Text;
        }

        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
        }

        private void addByFolderItem_Click(object sender, EventArgs e)
        {
            var fbd = new FolderBrowserDialog();
            fbd.UseDescriptionForTitle = true;
            fbd.Description = "Select the mod folder you want to add";

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                var ywmlConfigPath = Path.Combine(fbd.SelectedPath, "ywml.json");
                if (!File.Exists(ywmlConfigPath))
                {
                    MessageBox.Show("Invalid YWML project: make sure you have a project configuration file (ywml.json)");
                    return;
                }

                var ywmlProject = TryReadProject(ywmlConfigPath);
                if (ywmlProject == null)
                {
                    return;
                }

                AddModToTree(ywmlProject, fbd.SelectedPath);
            }
        }

        private async void addByArchiveItem_Click(object sender, EventArgs e)
        {
            string archivePath;
            using (var ofd = new OpenFileDialog())
            {
                ofd.Title = "Select the mod archive you want to add";
                ofd.Filter = "YWML mod archive (*.zip;*.7z)|*.zip;*.7z";

                if (ofd.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                archivePath = ofd.FileName;
            }

            var stagingDir = Path.Combine(CGeneralUtils.ZipModsStagingDir, Guid.NewGuid().ToString("N"));
            using var loadingForm = new ArchiveLoadingForm(Path.GetFileName(archivePath));
            loadingForm.Show(this);
            Enabled = false;

            try
            {
                var progress = new Progress<int>(loadingForm.SetProgress);
                await Task.Run(() =>
                {
                    using var archive = new CYwmlProjectArchive(CArchiveReaderFactory.Open(archivePath));
                    archive.ExtractTo(stagingDir, progress);
                });

                _archiveStagingDirs.Add(stagingDir);

                var ywmlConfigPath = Path.Combine(stagingDir, "ywml.json");
                if (!File.Exists(ywmlConfigPath))
                {
                    MessageBox.Show(WRONG_STRUCT_MSG + "ywml.json was not found after extraction");
                    return;
                }

                var ywmlProject = TryReadProject(ywmlConfigPath);
                if (ywmlProject == null)
                {
                    return;
                }

                AddModToTree(ywmlProject, stagingDir);
            }
            catch (NotSupportedException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (InvalidDataException ex)
            {
                MessageBox.Show($"Invalid YWML project archive:\n\n{ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not read the archive:\n\n{ex.Message}");
            }
            finally
            {
                Enabled = true;
                loadingForm.Close();
            }
        }

        private CYwmlProject? TryReadProject(string ywmlConfigPath)
        {
            CYwmlProject? ywmlProject;
            try
            {
                ywmlProject = JsonConvert.DeserializeObject<CYwmlProject>(File.ReadAllText(ywmlConfigPath));
            }
            catch
            {
                MessageBox.Show(WRONG_STRUCT_MSG + "Wrong json format");
                return null;
            }

            if (ywmlProject == null)
            {
                MessageBox.Show(WRONG_STRUCT_MSG + "Wrong properties");
                return null;
            }

            return ywmlProject;
        }

        private void AddModToTree(CYwmlProject ywmlProject, string projectPath)
        {
            string modItem = $"{ywmlProject.Name}";
            _modNameToPath[modItem] = projectPath;

            TreeNode node = new TreeNode(modItem);
            node.ToolTipText = $"{ywmlProject.Author}, {ywmlProject.Version}";
            modsTreeView.Nodes.Add(node);
            modsTreeView.SelectedNode = null;
            RefreshModListButtonsState();
        }

        private void autoInstallDirBtn_Click(object sender, EventArgs e)
        {
            if (extensionComboBox.Text == string.Empty)
            {
                MessageBox.Show("You must select a target game to generate an installation dir");
            }
            else
            {
                var f = new AutoInstallDirForm(GetSelectedExt());
                f.ShowDialog();

                if (f.GeneratedInstallPath != null)
                {
                    SetLocalInstallDir(f.GeneratedInstallPath);
                }
            }
        }

        private void modsTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            RefreshModListButtonsState();
        }
    }
}
