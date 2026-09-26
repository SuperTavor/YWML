using YWML.Src.ConfigManager;
using YWML.Src.ExtensionLibrary;
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
        private readonly CInstalledGameCatalog _catalog = new();
        private readonly CModList _modList = new();
        private readonly CInstallDirectoryStore _installDirectories = new(CGeneralUtils.DefaultInstallationDirectoriesPath);
        private readonly CArchiveModImporter _archiveImporter = new(CGeneralUtils.ZipModsStagingDir);
        private readonly CModInstaller _installer = new();
        private readonly IFtpTransport _ftpTransport = new CFtpTransport();
        private SFtpConnectionInfo? _ftpConnectionInfo;
        private Font _installPathNormalFont;
        private Font _installPathItalicFont;
        private Color _installPathNormalForeColor;
        private bool _suppressInstallPathUpdate;
        private string? _lastSelectedGameName;
        private const string WRONG_STRUCT_MSG = "YWML project configuration exists (ywml.json), but is not structured correctly: ";
        private const string ADD_GAME_ITEM = "Can't find your game/region? Add it from here";
        private const string REMOTE_DISABLED_MSG = "Disabled for remote install";

        public LoadForm()
        {
            InitializeComponent();
            RefreshModListButtonsState();
            modsTreeView.ShowNodeToolTips = true;
            this.FormClosing += LoadForm_FormClosing;

            _installPathNormalFont = modInstallPathTextBox.Font;
            _installPathItalicFont = new Font("Consolas", 8F, FontStyle.Italic);
            _installPathNormalForeColor = modInstallPathTextBox.ForeColor;

            _installDirectories.Load();

            if (CConfigManager.Cfg.LastUsedInstallMode == SInstallMode.Local)
            {
                localModeRadio.Checked = true;
            }
            else
            {
                remoteModeRadio.Checked = true;
            }

            RefreshInstalledExtensions();
            RestoreLastTargetGame();
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
            _installDirectories.Save();

            if (extensionComboBox.SelectedItem is string selectedGame)
            {
                CConfigManager.Cfg.LastUsedTargetGame = selectedGame;
            }

            CConfigManager.Cfg.LastUsedInstallMode = IsRemoteMode ? SInstallMode.Remote : SInstallMode.Local;
            CConfigManager.UpdateConfig();

            _archiveImporter.Cleanup();
        }

        private void RestoreLastTargetGame()
        {
            var lastName = CConfigManager.Cfg.LastUsedTargetGame;
            if (!string.IsNullOrWhiteSpace(lastName) && extensionComboBox.Items.Contains(lastName))
            {
                extensionComboBox.SelectedItem = lastName;
                _lastSelectedGameName = lastName;
            }
        }

        private void installModeRadio_CheckedChanged(object sender, EventArgs e)
        {
            UpdateInstallControlsForMode();
        }

        private void UpdateInstallControlsForMode()
        {
            var isRemote = IsRemoteMode;
            var selectedExt = _catalog.Get(extensionComboBox.SelectedItem as string);
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
            var id = _catalog.GetId(extensionComboBox.SelectedItem as string);
            return id != null ? _installDirectories.Get(id) ?? string.Empty : string.Empty;
        }

        private void SetLocalInstallDir(string path)
        {
            var id = _catalog.GetId(extensionComboBox.SelectedItem as string);
            if (id != null)
            {
                _installDirectories.Set(id, path);
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

            _catalog.Refresh();
            extensionComboBox.Items.Clear();

            foreach (var name in _catalog.Names)
            {
                extensionComboBox.Items.Add(name);
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
            var node = modsTreeView.SelectedNode;
            if (node == null) return;

            _modList.RemoveAt(node.Index);
            modsTreeView.Nodes.RemoveAt(node.Index);
            modsTreeView.SelectedNode = null;
            RefreshModListButtonsState();
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

                if (_lastSelectedGameName != null && extensionComboBox.Items.Contains(_lastSelectedGameName))
                {
                    extensionComboBox.SelectedItem = _lastSelectedGameName;
                }

                UpdateInstallControlsForMode();
                return;
            }

            if (selectedName != null)
            {
                _lastSelectedGameName = selectedName;
            }

            UpdateInstallControlsForMode();
        }

        private async void installBtn_Click(object sender, EventArgs e)
        {
            if (modsTreeView.Nodes.Count == 0)
            {
                MessageBox.Show("Please add at least one mod to the list to begin patching.");
                return;
            }

            var selectedId = _catalog.GetId(extensionComboBox.SelectedItem as string);
            var selectedExtension = _catalog.Get(extensionComboBox.SelectedItem as string);
            if (selectedId == null || selectedExtension == null)
            {
                MessageBox.Show("Please select a target game.");
                return;
            }

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
                var localDir = _installDirectories.Get(selectedId);
                if (string.IsNullOrWhiteSpace(localDir))
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
            var modPaths = _modList.GetPathsLeastToMostImportant();

            SetInstallingState(true);
            try
            {
                var status = new Progress<string>(message => installBtn.Text = message);
                var percent = new Progress<int>(percentage => installBtn.Text = $"Uploading... {percentage}%");
                await _installer.InstallAsync(faToLoad, selectedExtension.FAName, modPaths, target, status, percent);
                MessageBox.Show("Loaded all mods. Enjoy your game!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while installing mods:\n\n{ex.Message}");
            }
            finally
            {
                SetInstallingState(false);
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

                if (isMoveUp)
                {
                    _modList.MoveUp(index);
                }
                else
                {
                    _modList.MoveDown(index);
                }
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

            var id = _catalog.GetId(extensionComboBox.SelectedItem as string);
            if (id == null) return;
            _installDirectories.Set(id, modInstallPathTextBox.Text);
        }

        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
        }

        private void addByFolderItem_Click(object sender, EventArgs e)
        {
            var fbd = new FolderBrowserDialog();
            fbd.UseDescriptionForTitle = true;
            fbd.Description = "Select the mod folder you want to add";

            if (fbd.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            try
            {
                var project = CYwmlProjectReader.Read(fbd.SelectedPath);
                AddModToTree(project, fbd.SelectedPath);
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Invalid YWML project: make sure you have a project configuration file (ywml.json)");
            }
            catch (InvalidDataException ex)
            {
                MessageBox.Show(WRONG_STRUCT_MSG + ex.Message);
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

            using var loadingForm = new ArchiveLoadingForm(Path.GetFileName(archivePath));
            loadingForm.Show(this);
            Enabled = false;

            try
            {
                var progress = new Progress<int>(loadingForm.SetProgress);
                var imported = await Task.Run(() => _archiveImporter.Import(archivePath, progress));
                AddModToTree(imported.Project, imported.ProjectPath);
            }
            catch (NotSupportedException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show(WRONG_STRUCT_MSG + "ywml.json was not found after extraction");
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

        private void AddModToTree(CYwmlProject project, string projectPath)
        {
            var name = $"{project.Name}";
            _modList.Add(name, projectPath);

            var node = new TreeNode(name);
            node.ToolTipText = $"{project.Author}, {project.Version}";
            modsTreeView.Nodes.Add(node);
            modsTreeView.SelectedNode = null;
            RefreshModListButtonsState();
        }

        private void autoInstallDirBtn_Click(object sender, EventArgs e)
        {
            var selectedExt = _catalog.Get(extensionComboBox.SelectedItem as string);
            if (selectedExt == null)
            {
                MessageBox.Show("You must select a target game to generate an installation dir");
                return;
            }

            var f = new AutoInstallDirForm(selectedExt);
            f.ShowDialog();

            if (f.GeneratedInstallPath != null)
            {
                SetLocalInstallDir(f.GeneratedInstallPath);
            }
        }

        private void modsTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            RefreshModListButtonsState();
        }

        private void modePanel_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
