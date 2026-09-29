using System.Diagnostics;
using YWML.Src.Forms;
using YWML.Src.Forms.LoadForm;
using YWML.Src.Updates;
using YWML.Src.Utils.GeneralUtils;

namespace YWML
{
    public partial class MainForm : Form
    {
        private readonly CUpdateFlow _updateFlow;
        private bool _updateChecked;

        public MainForm()
        {
            InitializeComponent();
            this.verLabel.Text = CGeneralUtils.APP_VERSION;

            _updateFlow = new CUpdateFlow(new CDesktopUpdatePlatform(this));
            Shown += MainForm_Shown;
            Activated += MainForm_Activated;
        }

        private async void MainForm_Shown(object? sender, EventArgs e)
        {
            if (_updateChecked)
            {
                return;
            }

            _updateChecked = true;
            await _updateFlow.CheckAsync();
            await PresentUpdateIfIdleAsync();
        }

        private async void MainForm_Activated(object? sender, EventArgs e)
        {
            await PresentUpdateIfIdleAsync();
        }

        private async Task PresentUpdateIfIdleAsync()
        {
            //A modal dialog disables the main form; defer until we're back on the home screen.
            if (!Enabled || !_updateFlow.HasPending)
            {
                return;
            }

            await _updateFlow.PresentPendingAsync();
        }

        private void extLibBtn_Click(object sender, EventArgs e)
        {
            new ExtensionLibraryForm().ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new LoadForm().ShowDialog();
        }

        private void configOpenBtn_Click(object sender, EventArgs e)
        {
            var filePath = Path.GetFullPath(CGeneralUtils.WritableConfigPath); // ensures backslashes
            if (File.Exists(filePath))
            {
                var psi = new ProcessStartInfo
                {
                    FileName = "explorer.exe",
                    Arguments = $"/select,\"{filePath}\"",
                    UseShellExecute = true
                };
                Process.Start(psi);
            }
            else
            {
                MessageBox.Show("Config file is not found.");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var f = new MigrateModForm();
            f.ShowDialog();
        }
    }
}
