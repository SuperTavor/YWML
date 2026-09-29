using Microsoft.Maui.Devices;
using Microsoft.Maui.Storage;
using YWML.Android.Services;
using YWML.Src.ConfigManager;
using YWML.Src.Install;
using YWML.Src.Loader.Archive;
using YWML.Src.Loader.DataClasses;
using YWML.Src.RemoteInstall;
using YWML.Src.Utils.GeneralUtils;
using YWML.Src.Updates;
using YWML.Src.Warnings;

namespace YWML.Android.Pages
{
    public partial class LoadModsPage : ContentPage
    {
        private readonly CAppState _state = CAppState.Current;
        private readonly CArchiveModImporter _importer = new(CGeneralUtils.ZipModsStagingDir);
        private readonly CUpdateFlow _updateFlow;
        private bool _compact;
        private bool _densityCheckPending;
        private bool _isHome;
        private int _expandedModIndex = -1;

        public LoadModsPage()
        {
            InitializeComponent();

            _updateFlow = new CUpdateFlow(new CAndroidUpdatePlatform(this));

            ContentScroll.SizeChanged += (_, _) => CheckDensity();
            ContentStack.SizeChanged += (_, _) => CheckDensity();
        }

        private static Style IconButtonStyle => (Style)Application.Current!.Resources["IconButton"];
        private static Color CardSurface => (Color)Application.Current!.Resources["Secondary"];
        private static Color CardStroke => (Color)Application.Current!.Resources["Gray600"];

        protected override void OnAppearing()
        {
            base.OnAppearing();

            _isHome = true;

            if (_state.StartupError != null)
            {
                var error = _state.StartupError;
                _state.StartupError = null;
                _ = DisplayAlert("YWML", error, "OK");
            }

            RefreshGames();
            RenderMods();
            UpdateMode();

            _ = EnsureOnboardingAsync();
            _ = RunUpdateFlowAsync();
        }

        protected override void OnDisappearing()
        {
            _isHome = false;
            base.OnDisappearing();
        }

        private async Task RunUpdateFlowAsync()
        {
            if (!_state.UpdateChecked)
            {
                _state.UpdateChecked = true;
                await _updateFlow.CheckAsync();
            }

            //Only prompt once the user is back on the home page.
            if (_isHome)
            {
                await _updateFlow.PresentPendingAsync();
            }
        }

        private void OnGameChanged(object? sender, EventArgs e)
        {
            if (GamePicker.SelectedItem is string name)
            {
                CConfigManager.Cfg.LastUsedTargetGame = name;
                CConfigManager.UpdateConfig();
            }

            UpdateMode();
        }

        private void OnModeChanged(object? sender, CheckedChangedEventArgs e) => UpdateMode();

        private async void OnAddGame(object? sender, TappedEventArgs e)
        {
            await Shell.Current.GoToAsync("//extensions");
        }

        private async Task EnsureOnboardingAsync()
        {
            if (_state.UserFolder.IsSet || Preferences.Default.Get("onboarding_done", false))
            {
                return;
            }

            Preferences.Default.Set("onboarding_done", true);
            await Navigation.PushModalAsync(new OnboardingPage());
        }

        private void RefreshGames()
        {
            _state.GameCatalog.Refresh();
            var lastName = CConfigManager.Cfg.LastUsedTargetGame;

            GamePicker.Items.Clear();
            foreach (var name in _state.GameCatalog.Names)
            {
                if (_state.GameCatalog.Get(name)?.IsDisableAutoInstall == true)
                {
                    continue;
                }

                GamePicker.Items.Add(name);
            }

            if (!string.IsNullOrWhiteSpace(lastName) && GamePicker.Items.Contains(lastName))
            {
                GamePicker.SelectedItem = lastName;
            }
        }

        private string? SelectedId() => _state.GameCatalog.GetId(GamePicker.SelectedItem as string);

        private void UpdateMode()
        {
            var canLocal = _state.UserFolder.IsSet;
            LocalMode.IsEnabled = canLocal;

            if (!canLocal && (LocalMode.IsChecked || !RemoteMode.IsChecked))
            {
                RemoteMode.IsChecked = true;
            }
        }

        private void CheckDensity()
        {
            if (!_densityCheckPending || _compact)
            {
                return;
            }

            if (ContentScroll.Height <= 0 || ContentStack.Height <= 0)
            {
                return;
            }

            _densityCheckPending = false;

            if (ContentStack.Height > ContentScroll.Height)
            {
                _compact = true;
                Dispatcher.Dispatch(RenderMods);
            }
        }

        private void RenderMods()
        {
            ModList.Children.Clear();
            var entries = _state.ModList.Entries;

            ModsCountLabel.Text = entries.Count == 1 ? "(1 mod)" : $"({entries.Count} mods)";
            EmptyHintLabel.IsVisible = entries.Count == 0;

            if (_expandedModIndex >= entries.Count)
            {
                _expandedModIndex = -1;
            }

            for (var i = 0; i < entries.Count; i++)
            {
                var index = i;
                var entry = entries[i];
                var showDetails = !_compact || index == _expandedModIndex;

                var nameLabel = new Label
                {
                    Text = entry.Project.DisplayName,
                    FontSize = 16,
                    FontAttributes = FontAttributes.Bold,
                    VerticalOptions = LayoutOptions.Center
                };

                if (_compact)
                {
                    var nameTap = new TapGestureRecognizer();
                    nameTap.Tapped += (_, _) => ToggleMod(index);
                    nameLabel.GestureRecognizers.Add(nameTap);
                }

                var up = new Button { Text = "\u25B2", IsEnabled = index > 0, Style = IconButtonStyle };
                up.Clicked += (_, _) => MoveMod(index, -1);
                var down = new Button { Text = "\u25BC", IsEnabled = index < entries.Count - 1, Style = IconButtonStyle };
                down.Clicked += (_, _) => MoveMod(index, 1);
                var remove = new Button { Text = "\u2715", Style = IconButtonStyle };
                remove.Clicked += (_, _) => RemoveMod(index);

                var controls = new HorizontalStackLayout { Spacing = 2, Children = { up, down, remove } };

                var header = new Grid
                {
                    ColumnDefinitions =
                    {
                        new ColumnDefinition(GridLength.Star),
                        new ColumnDefinition(GridLength.Auto)
                    }
                };
                header.Add(nameLabel, 0, 0);
                header.Add(controls, 1, 0);

                var content = new VerticalStackLayout { Spacing = 10 };
                content.Children.Add(header);

                if (showDetails)
                {
                    content.Children.Add(new Label
                    {
                        Text = $"by {entry.Project.DisplayAuthor} \u00B7 {entry.Project.DisplayVersion}",
                        FontSize = 12,
                        TextColor = Colors.Gray
                    });
                }

                ModList.Children.Add(new Border
                {
                    BackgroundColor = CardSurface,
                    Stroke = CardStroke,
                    StrokeThickness = 1,
                    StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle { CornerRadius = 12 },
                    Padding = 14,
                    Content = content
                });
            }

            _densityCheckPending = true;
        }

        private void ToggleMod(int index)
        {
            if (!_compact)
            {
                return;
            }

            _expandedModIndex = _expandedModIndex == index ? -1 : index;
            RenderMods();
        }

        private void MoveMod(int index, int direction)
        {
            if (direction < 0)
            {
                if (_state.ModList.MoveUp(index))
                {
                    _expandedModIndex = index - 1;
                }
            }
            else
            {
                if (_state.ModList.MoveDown(index))
                {
                    _expandedModIndex = index + 1;
                }
            }

            RenderMods();
        }

        private void RemoveMod(int index)
        {
            _state.ModList.RemoveAt(index);
            _compact = false;
            _expandedModIndex = -1;
            RenderMods();
        }

        private async void OnAddMod(object? sender, EventArgs e)
        {
            try
            {
                var options = new PickOptions
                {
                    PickerTitle = "Select the mod archive",
                    FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                    {
                        { DevicePlatform.Android, new[] { "application/zip", "application/x-7z-compressed", "application/octet-stream" } }
                    })
                };

                var pick = await FilePicker.Default.PickAsync(options);
                if (pick == null) return;

                var cachePath = Path.Combine(FileSystem.CacheDirectory, Guid.NewGuid().ToString("N") + Path.GetExtension(pick.FileName));
                await using (var source = await pick.OpenReadAsync())
                await using (var destination = File.Create(cachePath))
                {
                    await source.CopyToAsync(destination);
                }

                var loadingPage = new LoadingPage("LOADING ARCHIVE", $"Loading {pick.FileName}...");
                await Navigation.PushModalAsync(loadingPage, false);

                (string ProjectPath, CYwmlProject Project) imported;
                try
                {
                    var progress = new Progress<int>(loadingPage.SetProgress);
                    imported = await Task.Run(() => _importer.Import(cachePath, progress));
                }
                finally
                {
                    await Navigation.PopModalAsync(false);
                }

                var exeFsMode = await ResolveExeFsModeAsync(this, imported.ProjectPath);
                if (exeFsMode == null)
                {
                    return;
                }

                _state.ModList.Add(imported.Project, imported.ProjectPath, exeFsMode.Value);
                _compact = false;
                RenderMods();
            }
            catch (Exception ex)
            {
                await DisplayAlert("YWML", ex.Message, "OK");
            }
        }

        private static async Task<SExeFsMode?> ResolveExeFsModeAsync(Page host, string modPath)
        {
            var warning = CWarningService.CheckExeFs(modPath);
            if (warning == null)
            {
                return SExeFsMode.ValidOnly;
            }

            return await ModWarningPage.ShowAsync(host, warning);
        }

        private async void OnInstall(object? sender, EventArgs e)
        {
            var id = SelectedId();
            var ext = _state.GameCatalog.Get(GamePicker.SelectedItem as string);
            if (id == null || ext == null)
            {
                await DisplayAlert("YWML", "Please select a target game.", "OK");
                return;
            }
            if (_state.ModList.Count == 0)
            {
                await DisplayAlert("YWML", "Please add at least one mod.", "OK");
                return;
            }

            IModInstallTarget target;
            if (RemoteMode.IsChecked)
            {
                if (_state.FtpConnection == null)
                {
                    await Navigation.PushModalAsync(new FtpSetupPage(ext.TitleId));
                    return;
                }
                var remoteRoot = CRemotePath.GetModded3dsRomfsRoot(ext.TitleId);
                target = new CFtpInstallTarget(_state.FtpTransport, _state.FtpConnection.Value, remoteRoot);
            }
            else
            {
                if (!_state.UserFolder.IsSet)
                {
                    await DisplayAlert("YWML", "Set your emulator User folder in Settings first.", "OK");
                    return;
                }
                var installDestination = new SafInstallDestination(_state.UserFolder.TreeUri!, CUserFolderStore.ModsRelativeRoot(ext.TitleId));
                target = new CLocalInstallTarget(installDestination);
            }

            var faToLoad = Path.Combine(CGeneralUtils.ExtensionInstallDirectory, id, "patchable.fa");
            var mods = _state.ModList.GetModsLeastToMostImportant();

            var loadingPage = new LoadingPage("INSTALLING MODS", "Preparing files...");
            await Navigation.PushModalAsync(loadingPage, false);

            string? error = null;
            try
            {
                var status = new Progress<string>(loadingPage.SetStatus);
                var percent = new Progress<int>(loadingPage.SetProgress);
                await _state.Installer.InstallAsync(faToLoad, ext.FAName, mods, target, status, percent);
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }

            await Navigation.PopModalAsync(false);
            await DisplayAlert("YWML", error ?? "Loaded all mods. Enjoy your game!", "OK");
        }
    }
}
