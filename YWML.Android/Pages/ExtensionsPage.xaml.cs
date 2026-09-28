using YWML.Android.Controls;
using YWML.Android.Services;
using YWML.Src.ExtensionLibrary.DataClasses;
using YWML.Src.Utils.GeneralUtils;

namespace YWML.Android.Pages
{
    public partial class ExtensionsPage : ContentPage
    {
        private const string InstalledCategory = "Installed";

        private readonly CAppState _state = CAppState.Current;
        private string? _expandedCategory = InstalledCategory;
        private bool _busy;

        public ExtensionsPage()
        {
            InitializeComponent();
            CExtensionInstallOverlay.Attach(this);
        }

        private static Style SmallButtonStyle => (Style)Application.Current!.Resources["SmallButton"];

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _ = LoadAsync();
        }

        private async Task LoadAsync()
        {
            StatusLabel.Text = "Loading extension library...";
            _state.ExtensionLibrary.LoadInstalledList();

            try
            {
                await Task.Run(() => _state.ExtensionLibrary.FetchData());
            }
            catch (Exception)
            {
                var useCache = await DisplayAlert("YWML", "Could not fetch the extension library. Use the cached copy?", "Yes", "No");
                if (!useCache || !File.Exists(CGeneralUtils.ExtensionLibraryCachePath))
                {
                    StatusLabel.Text = "Failed to load the extension library.";
                    return;
                }
                try
                {
                    _state.ExtensionLibrary.LoadCachedData();
                }
                catch (Exception ex)
                {
                    StatusLabel.Text = ex.Message;
                    return;
                }
            }

            StatusLabel.Text = string.Empty;
            Render();
        }

        private void Render()
        {
            ExtensionList.Children.Clear();

            AddCategory(InstalledCategory, GetInstalledExtensions());

            var info = _state.ExtensionLibrary.ExtensionInfo;
            if (info?.ExtensionList == null || info.ExtensionCategories == null)
            {
                return;
            }

            foreach (var category in info.ExtensionCategories)
            {
                var inCategory = info.ExtensionList.Where(ext => !ext.IsDisableAutoInstall && ext.Name.Contains(category)).ToList();
                if (inCategory.Count == 0)
                {
                    continue;
                }

                AddCategory(category, inCategory);
            }
        }

        private List<CExtension> GetInstalledExtensions()
        {
            var library = _state.ExtensionLibrary.ExtensionInfo?.ExtensionList;
            var result = new List<CExtension>();

            foreach (var pair in _state.ExtensionLibrary.InstalledList)
            {
                var ext = library?.FirstOrDefault(e => e.Id == pair.Key);
                if (ext == null)
                {
                    var metadata = pair.Value;
                    ext = new CExtension
                    {
                        Id = pair.Key,
                        Name = metadata.Name,
                        OgFAName = metadata.FAName,
                        TitleId = metadata.TitleId,
                        IsDisableAutoInstall = metadata.IsDisableAutoInstall
                    };
                }

                result.Add(ext);
            }

            return result;
        }

        private void AddCategory(string category, List<CExtension> extensions)
        {
            var expanded = category == _expandedCategory;

            var container = new VerticalStackLayout { Spacing = 8 };
            container.Children.Add(BuildCategoryHeader(category, extensions.Count, expanded));

            if (expanded)
            {
                var body = new VerticalStackLayout { Spacing = 8, Padding = new Thickness(20, 0, 0, 0) };

                if (extensions.Count == 0)
                {
                    body.Children.Add(new Label { Text = "No extensions installed.", FontSize = 12, TextColor = Colors.Gray });
                }
                else
                {
                    foreach (var ext in extensions)
                    {
                        body.Children.Add(BuildExtensionRow(ext, category == InstalledCategory));
                    }
                }

                container.Children.Add(body);
            }

            container.Children.Add(new BoxView { HeightRequest = 1, Color = Color.FromArgb("#2A2A2A") });
            ExtensionList.Children.Add(container);
        }

        private View BuildCategoryHeader(string category, int count, bool expanded)
        {
            var row = new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition(GridLength.Auto),
                    new ColumnDefinition(GridLength.Star)
                },
                Padding = new Thickness(0, 4)
            };

            var label = new HorizontalStackLayout { Spacing = 8, VerticalOptions = LayoutOptions.Center };
            label.Children.Add(new Label { Text = expanded ? "\u25BE" : "\u25B8", FontSize = 14, TextColor = Colors.Gray, VerticalOptions = LayoutOptions.Center });
            label.Children.Add(new Label { Text = category, FontSize = 16, FontAttributes = FontAttributes.Bold, VerticalOptions = LayoutOptions.Center });
            label.Children.Add(new Label { Text = count.ToString(), FontSize = 12, TextColor = Colors.Gray, VerticalOptions = LayoutOptions.Center });
            row.Add(label, 0, 0);

            var tap = new TapGestureRecognizer();
            tap.Tapped += (_, _) => ToggleCategory(category);
            row.GestureRecognizers.Add(tap);

            return row;
        }

        private View BuildExtensionRow(CExtension ext, bool inInstalledCategory)
        {
            var installed = _state.ExtensionLibrary.InstalledList.ContainsKey(ext.Id);

            var text = ext.Name;
            if (ext.FileSize > 0)
            {
                text += $" ({ext.FileSize} MiB)";
            }
            if (installed && !inInstalledCategory)
            {
                text += " (Installed)";
            }

            var name = new Label { Text = text, FontSize = 14, VerticalOptions = LayoutOptions.Center };

            var button = new Button
            {
                Text = installed ? "Uninstall" : "Install",
                Style = SmallButtonStyle,
                IsEnabled = !_busy
            };
            button.Clicked += async (_, _) => await ToggleAsync(ext, installed);

            var row = new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition(GridLength.Star),
                    new ColumnDefinition(GridLength.Auto)
                },
                ColumnSpacing = 8
            };
            row.Add(name, 0, 0);
            row.Add(button, 1, 0);

            return row;
        }

        private void ToggleCategory(string category)
        {
            _expandedCategory = _expandedCategory == category ? null : category;
            Render();
        }

        private async Task ToggleAsync(CExtension ext, bool installed)
        {
            if (_busy)
            {
                return;
            }

            _busy = true;
            var install = _state.ExtensionInstall;
            install.Start(installed ? "Uninstalling extension..." : "Installing extension...");
            Render();
            try
            {
                if (installed)
                {
                    await ext.UninstallAsync(_state.ExtensionLibrary.InstalledList);
                }
                else
                {
                    await ext.InstallAsync(
                        new Progress<string>(message => { StatusLabel.Text = message; install.SetStatus(message); }),
                        new Progress<int>(percentage => install.SetPercent(percentage)),
                        _state.ExtensionLibrary.InstalledList);
                }
                _state.ExtensionLibrary.SaveInstalledList();
            }
            catch (Exception ex)
            {
                await DisplayAlert("YWML", ex.Message, "OK");
            }
            finally
            {
                install.Finish();
                _busy = false;
                Render();
            }
        }
    }
}
