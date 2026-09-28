using YWML.Src.ConfigManager;
using YWML.Src.RemoteInstall;
using YWML.Src.RemoteInstall.DataClasses;

namespace YWML.Src.Forms
{
    public partial class FtpConnectionForm : Form
    {
        private readonly string _titleId;

        public SFtpConnectionInfo? ConnectionInfo { get; private set; }
        public string? RemoteRoot { get; private set; }

        public FtpConnectionForm(string titleId, SFtpConnectionInfo? existingConnection = null)
        {
            _titleId = titleId;
            InitializeComponent();

            RemoteRoot = CRemotePath.GetModded3dsRomfsRoot(titleId);
            infoLabel.Text = $"Upload the mod directly to a modded 3DS running an FTP server (ftpd).\r\nTarget: {RemoteRoot}";

            var prefilled = existingConnection ?? GetSavedConnectionInfo();
            hostTextBox.Text = prefilled.Host;
            portNumeric.Value = CConfigManager.NormalizeFtpPort(prefilled.Port);
        }

        private static SFtpConnectionInfo GetSavedConnectionInfo()
        {
            return new SFtpConnectionInfo
            {
                Host = CConfigManager.Cfg.FtpHost ?? string.Empty,
                Port = CConfigManager.NormalizeFtpPort(CConfigManager.Cfg.FtpPort),
            };
        }

        private SFtpConnectionInfo BuildConnectionInfo()
        {
            return new SFtpConnectionInfo
            {
                Host = hostTextBox.Text.Trim(),
                Port = (int)portNumeric.Value,
            };
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(hostTextBox.Text))
            {
                MessageBox.Show("Please enter the FTP server address (your 3DS's IP).");
                return false;
            }
            return true;
        }

        private async void testBtn_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
            {
                return;
            }

            testBtn.Enabled = false;
            useBtn.Enabled = false;
            try
            {
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
                var transport = new CFtpTransport();
                await transport.ConnectAsync(BuildConnectionInfo(), cts.Token);
                await transport.DisconnectAsync();
                MessageBox.Show("Connection successful! You can now use remote install.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not connect to the FTP server.\n\n{ex.Message}");
            }
            finally
            {
                testBtn.Enabled = true;
                useBtn.Enabled = true;
            }
        }

        private void useBtn_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
            {
                return;
            }

            var connection = BuildConnectionInfo();

            if (saveInfoCheckBox.Checked)
            {
                CConfigManager.Cfg.FtpHost = connection.Host;
                CConfigManager.Cfg.FtpPort = connection.Port;
                CConfigManager.UpdateConfig();
            }

            ConnectionInfo = connection;
            RemoteRoot = CRemotePath.GetModded3dsRomfsRoot(_titleId);
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
