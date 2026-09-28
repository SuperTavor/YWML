using YWML.Android.Services;
using YWML.Src.ConfigManager;
using YWML.Src.RemoteInstall;
using YWML.Src.RemoteInstall.DataClasses;

namespace YWML.Android.Pages
{
    public partial class FtpSetupPage : ContentPage
    {
        public FtpSetupPage(string titleId)
        {
            InitializeComponent();

            HostEntry.Text = CConfigManager.Cfg.FtpHost ?? string.Empty;
            PortEntry.Text = CConfigManager.NormalizeFtpPort(CConfigManager.Cfg.FtpPort).ToString();
        }

        private SFtpConnectionInfo Build()
        {
            var port = int.TryParse(PortEntry.Text, out var parsed) ? CConfigManager.NormalizeFtpPort(parsed) : SFtpConnectionInfo.DEFAULT_PORT;
            return new SFtpConnectionInfo { Host = HostEntry.Text?.Trim() ?? string.Empty, Port = port };
        }

        private async void OnTest(object? sender, EventArgs e)
        {
            var info = Build();
            if (string.IsNullOrWhiteSpace(info.Host))
            {
                await DisplayAlert("YWML", "Enter the server IP.", "OK");
                return;
            }

            try
            {
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
                var transport = new CFtpTransport();
                await transport.ConnectAsync(info, cts.Token);
                await transport.DisconnectAsync();
                await DisplayAlert("YWML", "Connection successful!", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("YWML", ex.Message, "OK");
            }
        }

        private async void OnUse(object? sender, EventArgs e)
        {
            var info = Build();
            if (string.IsNullOrWhiteSpace(info.Host))
            {
                await DisplayAlert("YWML", "Enter the server IP.", "OK");
                return;
            }

            CConfigManager.Cfg.FtpHost = info.Host;
            CConfigManager.Cfg.FtpPort = info.Port;
            CConfigManager.UpdateConfig();

            CAppState.Current.FtpConnection = info;
            await Navigation.PopModalAsync();
        }
    }
}
