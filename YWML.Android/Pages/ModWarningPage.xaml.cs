using YWML.Src.Loader.DataClasses;
using YWML.Src.Warnings.DataClasses;

namespace YWML.Android.Pages
{
    public partial class ModWarningPage : ContentPage
    {
        private readonly TaskCompletionSource<SExeFsMode?> _result = new();

        public ModWarningPage(SModWarning warning)
        {
            InitializeComponent();
            TitleLabel.Text = warning.Title;
            MessageLabel.Text = warning.Message;
        }

        public static async Task<SExeFsMode?> ShowAsync(Page host, SModWarning warning)
        {
            var page = new ModWarningPage(warning);
            await host.Navigation.PushModalAsync(page, false);
            var choice = await page._result.Task;
            await host.Navigation.PopModalAsync(false);
            return choice;
        }

        private void OnContinueValid(object? sender, EventArgs e) => _result.TrySetResult(SExeFsMode.ValidOnly);

        private void OnContinueAll(object? sender, EventArgs e) => _result.TrySetResult(SExeFsMode.All);

        private void OnCancel(object? sender, EventArgs e) => _result.TrySetResult(null);
    }
}
