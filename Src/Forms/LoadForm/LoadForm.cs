using Newtonsoft.Json;
using YWML.Src.Utils.GeneralUtils;
using YWML.Src.ExtensionLibrary;
using YWML.Src.Loader.DataClasses;
using YWML.Src.Loader;

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

        private void browseBtn_Click(object sender, EventArgs e)
        {
            if (extensionComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please select a game before choosing an installation directory");
                return;
            }
            var fbd = new FolderBrowserDialog();
            fbd.UseDescriptionForTitle = true;
            fbd.Description = "Select the folder you want to install your mods to";
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                this.modInstallPathTextBox.Text = fbd.SelectedPath;
                _defaultInstallDirs[_nameToId[extensionComboBox.SelectedItem as string]] = fbd.SelectedPath;
            }
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
            modsTreeView.Nodes.Remove(modsTreeView.SelectedNode);
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

        private void installBtn_Click(object sender, EventArgs e)
        {
            var faToLoad = Path.Combine(CGeneralUtils.ExtensionInstallDirectory, _nameToId[extensionComboBox.SelectedItem as string], "patchable.fa");
            var result = CLoader.ModifyFA(modsTreeView, _modNameToPath, faToLoad);
            var archive = result.Archive;
            var rawFiles = result.RawFiles;
            var modifiedFA = archive.Save();
            var outputFaPath = Path.Combine(modInstallPathTextBox.Text, _lib.InstalledList[_nameToId[extensionComboBox.SelectedItem as string]].FAName);
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
            archive.BaseStream.Close();
        }

        private void moveUpSelectedModBtn_Click(object sender, EventArgs e)
        {
            MoveSelectedNodePosition(modsTreeView.SelectedNode.PrevNode, true);
        }
        private void MoveSelectedNodePosition(TreeNode nodeToCheck, bool isMoveUp)
        {
            if (modsTreeView.SelectedNode == null) return;
            if (nodeToCheck == null) return;

            TreeNode node = modsTreeView.SelectedNode;
            int index = node.Index;

            modsTreeView.Nodes.RemoveAt(index);
            if(isMoveUp)
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
           MoveSelectedNodePosition(modsTreeView.SelectedNode.NextNode, false);
        }
    }
}
