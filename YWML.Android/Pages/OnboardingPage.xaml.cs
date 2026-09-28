using YWML.Android.Services;

namespace YWML.Android.Pages
{
    public partial class OnboardingPage : ContentPage
    {
        private const string CancelMessage = "Are you sure you want to cancel the user folder selection? You can choose again from settings";

        private readonly SOnboardingStep[] _steps =
        {
            new()
            {
                Image = "onboarding_1.png",
                Title = "Welcome to YWML",
                Description = "The mod loader for the Yo-kai Watch series on 3DS."
            },
            new()
            {
                Image = "onboarding_blocked.png",
                Title = "Emulators from the play store are not supported.",
                Description = "The Play Store builds of Azahar and other emulators can't access the files YWML needs. Install from the emulator's own website instead."
            },
            new()
            {
                Image = "onboarding_search.png",
                Title = "Find your user folder",
                Description = "If your emulator doesn't show the screens in the next tabs, look up how to find the user folder for your emulator, or move to Azahar from their GitHub.",
                LinkText = "Azahar",
                LinkUrl = "https://azahar-emu.org/pages/download/"
            },
            new()
            {
                Image = "pointtosettings.png",
                ImageHeight = 400,
                Title = "Enter settings",
                Description = "Open your emulator and go to its settings."
            },
            new()
            {
                Image = "whereuserfolder.png",
                ImageHeight = 400,
                Title = "View your user folder",
                Description = "Open the option that shows your emulator's user folder, then select it here."
            }
        };

        public OnboardingPage()
        {
            InitializeComponent();
            Carousel.ItemsSource = _steps;
        }

        private void OnPositionChanged(object? sender, PositionChangedEventArgs e)
        {
            NextButton.Text = e.CurrentPosition >= _steps.Length - 1 ? "Select user folder" : "Next";
        }

        private async void OnNext(object? sender, EventArgs e)
        {
            if (Carousel.Position < _steps.Length - 1)
            {
                Carousel.Position++;
                return;
            }

            await RequestUserFolderAsync();
        }

        private async Task RequestUserFolderAsync()
        {
            while (true)
            {
                var uri = await CAndroidFolderPicker.PickAsync();
                if (uri != null)
                {
                    CAppState.Current.UserFolder.Set(uri);
                    ShowCongrats();
                    return;
                }

                var cancel = await DisplayAlertAsync("YWML", CancelMessage, "Yes", "No");
                if (cancel)
                {
                    await Navigation.PopModalAsync();
                    return;
                }
            }
        }

        private void ShowCongrats()
        {
            var done = new Button { Text = "Done" };
            done.Clicked += async (_, _) => await Navigation.PopModalAsync();

            Content = new VerticalStackLayout
            {
                Padding = 24,
                Spacing = 24,
                VerticalOptions = LayoutOptions.Center,
                Children =
                {
                    new Image { Source = "onboarding_3.png", Aspect = Aspect.AspectFit, HeightRequest = 160 },
                    new Label { Text = "Congrats!", FontSize = 28, FontFamily = "monospace", HorizontalOptions = LayoutOptions.Center },
                    new Label { Text = "Your user folder is set. You can now install mods.", FontSize = 14, TextColor = Colors.Gray, HorizontalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Center },
                    done
                }
            };
        }

        private async void OnSkip(object? sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}
