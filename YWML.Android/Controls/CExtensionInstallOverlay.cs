using Microsoft.Maui.Controls.Shapes;
using YWML.Android.Services;

namespace YWML.Android.Controls
{
    public sealed class CExtensionInstallOverlay : ContentView
    {
        private readonly Label _label = new() { FontSize = 12, TextColor = Colors.Gray };
        private readonly ProgressBar _progress = new();

        public CExtensionInstallOverlay()
        {
            var card = new Border
            {
                BackgroundColor = (Color)Application.Current!.Resources["Secondary"],
                Stroke = (Color)Application.Current!.Resources["Gray600"],
                StrokeThickness = 1,
                StrokeShape = new RoundRectangle { CornerRadius = 12 },
                Padding = new Thickness(16, 10),
                Margin = new Thickness(16),
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.End,
                Content = new VerticalStackLayout
                {
                    Spacing = 6,
                    WidthRequest = 220,
                    Children = { _label, _progress }
                }
            };

            Content = card;

            CAppState.Current.ExtensionInstall.Changed += Update;
            Update();
        }

        public static void Attach(ContentPage page)
        {
            var content = page.Content;
            page.Content = null;

            var root = new Grid();
            if (content != null)
            {
                root.Children.Add(content);
            }
            root.Children.Add(new CExtensionInstallOverlay());
            page.Content = root;
        }

        private void Update()
        {
            var state = CAppState.Current.ExtensionInstall;
            IsVisible = state.IsBusy;
            _label.Text = $"{state.Status} {state.Percent}%";
            _progress.Progress = state.Percent / 100.0;
        }
    }
}
