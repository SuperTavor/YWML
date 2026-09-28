using System.Globalization;
using Microsoft.Maui.ApplicationModel;

namespace YWML.Android.Pages
{
    public sealed class SOnboardingStep
    {
        public string Image { get; init; } = string.Empty;
        public string Title { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public string LinkText { get; init; } = string.Empty;
        public string LinkUrl { get; init; } = string.Empty;
        public int ImageHeight { get; init; } = 220;

        public bool ShowImage => !string.IsNullOrEmpty(Image);
    }

    public sealed class COnboardingTextConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is not SOnboardingStep step)
            {
                return string.Empty;
            }

            var formatted = new FormattedString();

            if (string.IsNullOrEmpty(step.LinkText)
                || string.IsNullOrEmpty(step.LinkUrl)
                || !step.Description.Contains(step.LinkText, StringComparison.Ordinal))
            {
                formatted.Spans.Add(new Span { Text = step.Description });
                return formatted;
            }

            var index = step.Description.IndexOf(step.LinkText, StringComparison.Ordinal);
            if (index > 0)
            {
                formatted.Spans.Add(new Span { Text = step.Description[..index] });
            }

            var link = new Span
            {
                Text = step.LinkText,
                TextColor = (Color)Application.Current!.Resources["Link"],
                TextDecorations = TextDecorations.Underline
            };
            var tap = new TapGestureRecognizer();
            tap.Tapped += async (_, _) => await Launcher.Default.OpenAsync(step.LinkUrl);
            link.GestureRecognizers.Add(tap);
            formatted.Spans.Add(link);

            var end = index + step.LinkText.Length;
            if (end < step.Description.Length)
            {
                formatted.Spans.Add(new Span { Text = step.Description[end..] });
            }

            return formatted;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
