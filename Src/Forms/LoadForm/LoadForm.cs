using Newtonsoft.Json;
using YWML.Src.Utils.GeneralUtils;
using YWML.Src.ExtensionLibrary;
using YWML.Src.Loader.DataClasses;
using YWML.Src.Loader;
using YWML.Src.ExtensionLibrary.DataClasses;

namespace YWML.Src.Forms.LoadForm
{
    public partial class LoadForm : Form
    {
        private CExtensionLibrary _lib;
        private Dictionary<string, string> _defaultInstallDirs;
        private Dictionary<string, string> _nameToId = new();
        private Dictionary<string, string> _modNameToPath = new();
        public LoadForm()
        {
            InitializeComponent();
            platComboBox.DataSource = Enum.GetValues(typeof(SPlatform));
            _lib = new CExtensionLibrary();
            _lib.LoadInstalledList();
            modsTreeView.ShowNodeToolTips = true;
            this.FormClosing += LoadForm_FormClosing;
            //Load the installed extensions into the comboBox
            foreach (var key in _lib.InstalledList.Keys)
            {
                _nameToId[_lib.InstalledList[key].Name] = key;
                extensionComboBox.Items.Add(_lib.InstalledList[key].Name);
            }
            if (!File.Exists(CGeneralUtils.DefaultInstallationDirectoriesPath))
            {
                _defaultInstallDirs = new();
            }
            else
            {
                _defaultInstallDirs = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(CGeneralUtils.DefaultInstallationDirectoriesPath));
            }


        }
        public void LoadForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            var defaultInstallDirsJson = JsonConvert.SerializeObject(_defaultInstallDirs);
            File.WriteAllText(CGeneralUtils.DefaultInstallationDirectoriesPath, defaultInstallDirsJson);
        }
        private string? ChooseFolder(string desc)
        {
            var fbd = new FolderBrowserDialog();
            fbd.UseDescriptionForTitle = true;
            fbd.Description = desc;
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                return fbd.SelectedPath;
            }
            else
            {
                return null;
            }
        }
        private void browseBtn_Click(object sender, EventArgs e)
        {
            if (extensionComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please select a game before choosing an installation directory");
                return;
            }
            var selFolder = ChooseFolder("Select the folder you want to install your mods to");
            if (selFolder == null) return;
            this.modInstallPathTextBox.Text = selFolder;
        }


        private void addModBtn_Click(object sender, EventArgs e)
        {
            const string WRONG_STRUCT_MSG = "YWML project configuration exists (ywml.json), but is not structured correctly: ";
            var fbd = new FolderBrowserDialog();
            fbd.UseDescriptionForTitle = true;
            fbd.Description = "Select the mod folder you want to add";
            string modItem = string.Empty;
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                var ywmlConfigPath = Path.Combine(fbd.SelectedPath, "ywml.json");
                if (!File.Exists(ywmlConfigPath))
                {
                    MessageBox.Show("Invalid YWML project: make sure you have a project configuration file (ywml.json)");
                    return;
                }
                else
                {
                    CYwmlProject ywmlProject = new();
                    try
                    {
                        ywmlProject = JsonConvert.DeserializeObject<CYwmlProject>(File.ReadAllText(ywmlConfigPath));
                    }
                    catch
                    {
                        MessageBox.Show(WRONG_STRUCT_MSG + "Wrong json format");
                        return;
                    }
                    if (ywmlProject == null)
                    {
                        MessageBox.Show(WRONG_STRUCT_MSG + "Wrong properties");
                        return;
                    }
                    modItem = $"{ywmlProject.Name}";
                    _modNameToPath[modItem] = fbd.SelectedPath;
                    TreeNode node = new TreeNode(modItem);
                    node.ToolTipText = $"{ywmlProject.Author}, {ywmlProject.Version}";
                    modsTreeView.Nodes.Add(node);
                }
            }
        }

        private void removeSelectedModBtn_Click(object sender, EventArgs e)
        {
            if (modsTreeView.SelectedNode != null) modsTreeView.Nodes.Remove(modsTreeView.SelectedNode);
        }

        private void extensionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_defaultInstallDirs.ContainsKey(_nameToId[extensionComboBox.SelectedItem as string]))
            {
                modInstallPathTextBox.Text = _defaultInstallDirs[_nameToId[extensionComboBox.SelectedItem as string]];
            }
            else
            {
                modInstallPathTextBox.Text = "";
            }
        }
        private CExtensionLibraryItem GetSelectedExt()
        {
            return _lib.InstalledList.Values.ToList().Find(match: e => e.Name == extensionComboBox.SelectedItem);
        }
        private void installBtn_Click(object sender, EventArgs e)
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

            //check if the mod path contains the title ID to warn the user it may be wrong
            var selectedExtension = _lib.InstalledList.Values.ToList().Find(match: e => e.Name == extensionComboBox.SelectedItem);
            if (!modInstallPathTextBox.Text.Contains(selectedExtension.TitleId))
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
            var faToLoad = Path.Combine(CGeneralUtils.ExtensionInstallDirectory, _nameToId[extensionComboBox.SelectedItem as string], "patchable.fa");
            var result = CLoader.ModifyFA(modsTreeView, _modNameToPath, faToLoad);
            var rawFiles = result.RawFiles;
            var modifiedFA = result.Archive.Save();
            var outputFaPath = Path.Combine(modInstallPathTextBox.Text, _lib.InstalledList[_nameToId[extensionComboBox.SelectedItem as string]].FAName);
            string? dir = Path.GetDirectoryName(outputFaPath);

            if (!string.IsNullOrEmpty(dir))
            {
                Directory.CreateDirectory(dir);
            }
            File.WriteAllBytes(outputFaPath, modifiedFA);
            foreach (var file in rawFiles)
            {
                var baseDirectory = file.Value;
                var filePath = file.Key;
                var outputPath = Path.Combine(modInstallPathTextBox.Text, Path.GetRelativePath(baseDirectory, filePath));
                Directory.CreateDirectory(Path.GetDirectoryName(outputPath));
                File.Copy(filePath, outputPath, true);
            }
            MessageBox.Show("Loaded all mods. Enjoy your game!");
            result.Archive.BaseStream.Close();
        }

        private void moveUpSelectedModBtn_Click(object sender, EventArgs e)
        {
            MoveSelectedNodePosition(true);
        }
        private void MoveSelectedNodePosition(bool isMoveUp)
        {
            if (modsTreeView.SelectedNode == null) return;

            TreeNode node = modsTreeView.SelectedNode;
            int index = node.Index;

            modsTreeView.Nodes.RemoveAt(index);
            if (isMoveUp)
            {
                modsTreeView.Nodes.Insert(index - 1, node);
            }
            else
            {
                modsTreeView.Nodes.Insert(index + 1, node);
            }

        }
        private void moveDownSelectedModBtn_Click(object sender, EventArgs e)
        {
            MoveSelectedNodePosition(false);
        }

        private void genModDirBtn_Click(object sender, EventArgs e)
        {
            string modInstallDir = string.Empty;
            if (extensionComboBox.Text == string.Empty)
            {
                MessageBox.Show("You must select a target game to generate an installation dir");
                return;
            }

            if (platComboBox.Text != "Modded3DS")
            {
                modInstallDir = $"C:/Users/{Environment.UserName}/AppData/Roaming/{platComboBox.Text}/load/mods/{GetSelectedExt().TitleId}";
            }
            else
            {
                MessageBox.Show("Your 3DS microSD drive must be inserted into your computer. Please insert it if you haven't already before proceeding.\n\nClick 'OK' to select your 3DS microSD card's drive when prompted to in the file explorer. (For example: When inserting my microSD card, it showed up as the D:/ drive. So I went to \"This PC\", selected my D:/ drive and pressed \"open\". ");
                var selFolder = ChooseFolder("Please choose your 3DS microSD card's drive");
                if (selFolder != null)
                {
                    //the last char removal is to remove the backslash at the end of the drive path (D:\)
                    if (selFolder.EndsWith("\\"))
                        selFolder = selFolder.Remove(selFolder.Length - 1);
                    modInstallDir = $"{selFolder}/luma/titles/{GetSelectedExt().TitleId}";
                }
                else
                {
                    MessageBox.Show("Operation was cancelled.");
                    return;
                }
            }
            modInstallPathTextBox.Text = modInstallDir+"/romfs";
        }

        private void LoadForm_Load(object sender, EventArgs e)
        {

        }

        private void modInstallPathTextBox_TextChanged(object sender, EventArgs e)
        {
            if(modInstallPathTextBox.Text != null)
                _defaultInstallDirs[_nameToId[extensionComboBox.SelectedItem as string]] = modInstallPathTextBox.Text;
        }
    }
}
