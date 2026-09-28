namespace YWML.Android.Pages
{
    public partial class LoadingPage : ContentPage
    {
        public LoadingPage(string title, string status)
        {
            InitializeComponent();
            TitleLabel.Text = title;
            StatusLabel.Text = status;
        }

        public void SetStatus(string status)
        {
            StatusLabel.Text = status;
        }

        public void SetProgress(int percent)
        {
            if (!LoadingProgress.IsVisible)
            {
                Spinner.IsRunning = false;
                Spinner.IsVisible = false;
                LoadingProgress.IsVisible = true;
                PercentageLabel.IsVisible = true;
            }

            var value = Math.Clamp(percent, 0, 100);
            LoadingProgress.Progress = value / 100.0;
            PercentageLabel.Text = $"{value}%";
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}
