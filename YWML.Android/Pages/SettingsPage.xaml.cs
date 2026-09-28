using YWML.Android.Services;
using YWML.Src.ConfigManager;
using YWML.Src.Utils.GeneralUtils;

namespace YWML.Android.Pages
{
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();

            HostEntry.Text = CConfigManager.Cfg.FtpHost ?? string.Empty;
            PortEntry.Text = CConfigManager.NormalizeFtpPort(CConfigManager.Cfg.FtpPort).ToString();
            VersionLabel.Text = $"YWML {CGeneralUtils.APP_VERSION}";
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            UpdateUserFolderLabel();
        }

        private void UpdateUserFolderLabel()
        {
            var path = CAppState.Current.UserFolder.DisplayPath;
            UserFolderLabel.Text = string.IsNullOrEmpty(path) ? "User folder: not set" : $"User folder: {path}";
        }

        private async void OnChooseFolder(object? sender, EventArgs e)
        {
            var uri = await CAndroidFolderPicker.PickAsync();
            if (uri != null)
            {
                CAppState.Current.UserFolder.Set(uri);
                UpdateUserFolderLabel();
            }
        }

        private void OnClearFolder(object? sender, EventArgs e)
        {
            CAppState.Current.UserFolder.Clear();
            UpdateUserFolderLabel();
        }

        private async void OnShowSetup(object? sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new OnboardingPage());
        }

        private async void OnSaveFtp(object? sender, EventArgs e)
        {
            var port = int.TryParse(PortEntry.Text, out var parsed) ? CConfigManager.NormalizeFtpPort(parsed) : 5000;
            CConfigManager.Cfg.FtpHost = HostEntry.Text?.Trim() ?? string.Empty;
            CConfigManager.Cfg.FtpPort = port;
            CConfigManager.UpdateConfig();
            await DisplayAlert("YWML", "Saved.", "OK");
        }
    }
}
