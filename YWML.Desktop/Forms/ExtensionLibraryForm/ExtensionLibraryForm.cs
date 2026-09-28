using YWML.Src.ExtensionLibrary;
using YWML.Src.ConfigManager;
using YWML.Src.Utils.GeneralUtils;
using YWML.Src.ExtensionLibrary.DataClasses;

namespace YWML.Src.Forms
{
    public partial class ExtensionLibraryForm : Form
    {
        private const string InstalledCategory = "Installed";

        private CExtensionLibrary _lib;
        private bool _busy;

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
                var result = MessageBox.Show(
                    "An error occurred while fetching the latest extension library. Would you like to try using a cached extension library (It may be missing new extension or even straight up including broken links)?",
                    "Error",
                    MessageBoxButtons.YesNo);

                if (result != DialogResult.Yes)
                {
                    this.Close();
                    return;
                }

                if (!File.Exists(CGeneralUtils.ExtensionLibraryCachePath))
                {
                    MessageBox.Show("Sorry, there isn't a previously cached version of the extension library on this device.");
                    this.Close();
                    return;
                }

                MessageBox.Show("Cache available. Loading now");
                try
                {
                    _lib.LoadCachedData();
                }
                catch
                {
                    this.Close();
                    return;
                }
            }

            BuildTree();
        }

        private void BuildTree()
        {
            extensionsTreeView.BeginUpdate();
            extensionsTreeView.Nodes.Clear();

            //Installed category, expanded by default, listing every installed extension.
            var installedNode = extensionsTreeView.Nodes.Add(InstalledCategory, InstalledCategory);
            foreach (var ext in GetInstalledExtensions())
            {
                installedNode.Nodes.Add(CreateExtensionNode(ext, showInstalledSuffix: false));
            }
            installedNode.Expand();

            //Regular categories.
            foreach (var cat in _lib.ExtensionInfo.ExtensionCategories)
            {
                extensionsTreeView.Nodes.Add(cat, cat);
            }

            foreach (var ext in _lib.ExtensionInfo.ExtensionList)
            {
                var installed = _lib.InstalledList.ContainsKey(ext.Id);
                foreach (var cat in _lib.ExtensionInfo.ExtensionCategories)
                {
                    if (ext.Name.Contains(cat))
                    {
                        extensionsTreeView.Nodes[cat].Nodes.Add(CreateExtensionNode(ext, installed));
                    }
                }
            }

            extensionsTreeView.EndUpdate();
            UpdateButtonStates();
        }

        private List<CExtension> GetInstalledExtensions()
        {
            var library = _lib.ExtensionInfo?.ExtensionList;
            var result = new List<CExtension>();

            foreach (var pair in _lib.InstalledList)
            {
                var ext = library?.Find(e => e.Id == pair.Key);
                if (ext == null)
                {
                    var metadata = pair.Value;
                    ext = new CExtension
                    {
                        Id = pair.Key,
                        Name = metadata.Name,
                        OgFAName = metadata.FAName,
                        TitleId = metadata.TitleId,
                        IsDisableAutoInstall = metadata.IsDisableAutoInstall
                    };
                }

                result.Add(ext);
            }

            return result;
        }

        private static TreeNode CreateExtensionNode(CExtension ext, bool showInstalledSuffix)
        {
            var text = ext.FileSize > 0 ? $"{ext.Name} ({ext.FileSize} MiB)" : ext.Name;
            if (showInstalledSuffix)
            {
                text += " (Installed)";
            }

            return new TreeNode(text) { Tag = ext };
        }

        private async void installBtn_Click(object sender, EventArgs e)
        {
            if (extensionsTreeView.SelectedNode?.Tag is not CExtension toInstall)
            {
                return;
            }

            SetBusy(true);
            try
            {
                statusLabel.Text = "Downloading LZMA extension";
                percentageLabel.Text = string.Empty;
                await toInstall.InstallAsync(
                    new Progress<string>(message => statusLabel.Text = message),
                    new Progress<int>(percentage => percentageLabel.Text = $"{percentage}%"),
                    _lib.InstalledList);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not install the extension.\n\n{ex.Message}", "YWML", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            finally
            {
                SetBusy(false);
            }

            BuildTree();
        }

        private async void uninstallBtn_Click(object sender, EventArgs e)
        {
            if (extensionsTreeView.SelectedNode?.Tag is not CExtension toUninstall)
            {
                return;
            }

            percentageLabel.Text = string.Empty;
            SetBusy(true);
            try
            {
                await toUninstall.UninstallAsync(_lib.InstalledList);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not uninstall the extension.\n\n{ex.Message}", "YWML", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            finally
            {
                SetBusy(false);
            }

            BuildTree();
        }

        private void SetBusy(bool busy)
        {
            _busy = busy;
            UpdateButtonStates();
        }

        private void extensionsTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            UpdateButtonStates(e.Node);
        }

        private void UpdateButtonStates(TreeNode? node = null)
        {
            //A running download/uninstall always wins over selection-based state.
            if (_busy)
            {
                exitBtn.Enabled = false;
                installBtn.Enabled = false;
                uninstallBtn.Enabled = false;
                return;
            }

            exitBtn.Enabled = true;
            installBtn.Enabled = false;
            uninstallBtn.Enabled = false;

            node ??= extensionsTreeView.SelectedNode;

            //Category nodes (including "Installed") carry no extension, so no buttons.
            if (node?.Tag is not CExtension ext)
            {
                return;
            }

            if (_lib.InstalledList.ContainsKey(ext.Id))
            {
                uninstallBtn.Enabled = true;
            }
            else
            {
                installBtn.Enabled = true;
            }
        }

        public void ExtensionLibraryForm_Closing(object sender, FormClosingEventArgs e)
        {
            _lib.SaveInstalledList();
        }
        private void exitBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
