using YWML.Src.Updates.DataClasses;

namespace YWML.Android.Pages
{
    public partial class UpdatePromptPage : ContentPage
    {
        private readonly TaskCompletionSource<SUpdateChoice> _result = new();

        public UpdatePromptPage(SUpdateInfo update)
        {
            InitializeComponent();
            MessageLabel.Text = $"A new update is available! Do you want to install it right now?\n\nVersion {update.Version}";
        }

        public static async Task<SUpdateChoice> ShowAsync(Page host, SUpdateInfo update)
        {
            var page = new UpdatePromptPage(update);
            await host.Navigation.PushModalAsync(page, false);
            var choice = await page._result.Task;
            await host.Navigation.PopModalAsync(false);
            return choice;
        }

        private void OnInstall(object? sender, EventArgs e) => _result.TrySetResult(SUpdateChoice.InstallNow);

        private void OnLater(object? sender, EventArgs e) => _result.TrySetResult(SUpdateChoice.Later);

        private void OnNever(object? sender, EventArgs e) => _result.TrySetResult(SUpdateChoice.Never);
    }
}
