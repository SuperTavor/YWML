using System.Diagnostics;
using YWML.Src.Forms;
using YWML.Src.Forms.LoadForm;
using YWML.Src.Utils.GeneralUtils;

namespace YWML
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            this.verLabel.Text = CGeneralUtils.APP_VERSION;
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
