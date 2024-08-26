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


            foreach (var ext in _lib.Extensions!)
            {
                var toAdd = $"{ext.Name} ({ext.FileSize} MiB)";
                if (_lib.InstalledList.ContainsKey(ext.Id))
                {
                    toAdd += " (Installed)";
                }
                extensionsTreeView.Nodes.Add(toAdd);
            }
        }

        private async void installBtn_Click(object sender, EventArgs e)
        {
            await _lib.Extensions![extensionsTreeView.SelectedNode.Index].InstallAsync([exitBtn, installBtn,uninstallBtn], statusLabel, percentageLabel, _lib.InstalledList);
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
            await _lib.Extensions![extensionsTreeView.SelectedNode.Index].UninstallAsync([exitBtn, installBtn, uninstallBtn], statusLabel, _lib.InstalledList);
            extensionsTreeView.SelectedNode.Text = extensionsTreeView.SelectedNode.Text.Substring(0, extensionsTreeView.SelectedNode.Text.Length - " (Installed)".Length);
            installBtn.Enabled = true;
            uninstallBtn.Enabled = false;
        }
    }
}
