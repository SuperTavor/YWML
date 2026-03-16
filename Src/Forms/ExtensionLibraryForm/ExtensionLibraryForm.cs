using YWML.Src.ExtensionLibrary;
using YWML.Src.ConfigManager;
using YWML.Src.Utils.GeneralUtils;
using Newtonsoft.Json;
using static System.Net.Mime.MediaTypeNames;
namespace YWML.Src.Forms
{
    public partial class ExtensionLibraryForm : Form
    {
        private CExtensionLibrary _lib;
        public ExtensionLibraryForm()
        {
            InitializeComponent();
            _lib = new CExtensionLibrary();
            this.FormClosing += ExtensionLibraryForm_Closing;
            uninstallBtn.Enabled = false;
        }

        private void ExtensionLibraryForm_Load(object sender, EventArgs e)
        {
            _lib.LoadInstalledList();
            try
            {
                _lib.FetchData();
            }
            catch (HttpRequestException)
            {
                this.Close();
            }
            //add category nodes
            foreach(var cat in _lib.ExtensionInfo.ExtensionCategories)
            {
                extensionsTreeView.Nodes.Add(cat,cat);
            }
            //add extension nodes to category nodes
            foreach (var ext in _lib.ExtensionInfo.ExtensionList)
            {
                var toAdd = $"{ext.Name} ({ext.FileSize} MiB)";
                if (_lib.InstalledList.ContainsKey(ext.Id))
                {
                    toAdd += " (Installed)";
                }
                foreach(var cat in _lib.ExtensionInfo.ExtensionCategories)
                {
                    if(ext.Name.Contains(cat))
                    {
                        extensionsTreeView.Nodes[cat].Nodes.Add(toAdd);
                    }
                }
            }
        }

        private async void installBtn_Click(object sender, EventArgs e)
        {
            await _lib.ExtensionInfo.ExtensionList.Find(ext => extensionsTreeView.SelectedNode.Text.Contains(ext.Name)).InstallAsync([exitBtn, installBtn,uninstallBtn], statusLabel, percentageLabel, _lib.InstalledList);
            extensionsTreeView.SelectedNode.Text += " (Installed)";
            installBtn.Enabled = false;
            uninstallBtn.Enabled = true;
        }
        public void ExtensionLibraryForm_Closing(object sender, FormClosingEventArgs e)
        {
            var installedListJson = JsonConvert.SerializeObject(_lib.InstalledList);
            Directory.CreateDirectory(Path.GetDirectoryName(CGeneralUtils.ExtensionInstalledList)!);
            File.WriteAllText(CGeneralUtils.ExtensionInstalledList, installedListJson);
        }
        private void exitBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void extensionsTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //check that selected node is not a category node
            foreach (var cat in _lib.ExtensionInfo.ExtensionCategories)
            {
                if (e.Node.Text == cat)
                {
                    uninstallBtn.Enabled = false;
                    installBtn.Enabled = false;
                    return;
                }
            }
            if (extensionsTreeView.SelectedNode.Text.Contains("Installed"))
            {
                uninstallBtn.Enabled = true;
                installBtn.Enabled = false;
            }
            else
            {
                installBtn.Enabled = true;
                uninstallBtn.Enabled = false;
            }
        }

        private async void uninstallBtn_Click(object sender, EventArgs e)
        {
            percentageLabel.Text = string.Empty;
            await _lib.ExtensionInfo.ExtensionList.Find(ext => extensionsTreeView.SelectedNode.Text.Contains(ext.Name)).UninstallAsync([exitBtn, installBtn, uninstallBtn], statusLabel, _lib.InstalledList);
            extensionsTreeView.SelectedNode.Text = extensionsTreeView.SelectedNode.Text.Substring(0, extensionsTreeView.SelectedNode.Text.Length - " (Installed)".Length);
            installBtn.Enabled = true;
            uninstallBtn.Enabled = false;
        }
    }
}
