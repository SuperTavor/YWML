using Android.Content;
using Android.Content.PM;
using Android.OS;
using Microsoft.Maui.Storage;
using YWML.Android.Pages;
using YWML.Src.Updates;
using YWML.Src.Updates.DataClasses;

namespace YWML.Android.Services
{
    public sealed class CAndroidUpdatePlatform : IUpdatePlatform
    {
        private const string APK_MIME = "application/vnd.android.package-archive";

        private readonly Page _host;
        private LoadingPage? _loadingPage;

        public CAndroidUpdatePlatform(Page host)
        {
            _host = host;
        }

        public SUpdatePlatform Platform => SUpdatePlatform.Android;

        public bool IsConnected =>
            Microsoft.Maui.Networking.Connectivity.Current.NetworkAccess == Microsoft.Maui.Networking.NetworkAccess.Internet;

        public Task<SUpdateChoice> AskAsync(SUpdateInfo update)
        {
            return UpdatePromptPage.ShowAsync(_host, update);
        }

        public string GetDownloadPath(string assetName)
        {
            return Path.Combine(FileSystem.CacheDirectory, "updates", assetName);
        }

        public IProgress<int>? BeginDownload(SUpdateInfo update)
        {
            _loadingPage = new LoadingPage("DOWNLOADING UPDATE", $"Downloading {update.AssetName}...");
            _ = _host.Navigation.PushModalAsync(_loadingPage, false);
            return new Progress<int>(_loadingPage.SetProgress);
        }

        public void EndDownload()
        {
            if (_loadingPage == null)
            {
                return;
            }

            _ = _host.Navigation.PopModalAsync(false);
            _loadingPage = null;
        }

        public void ReportError(string message)
        {
            _ = _host.DisplayAlert("YWML", message, "OK");
        }

        public Task InstallAsync(SUpdateInfo update, string downloadedFile)
        {
            var context = global::Android.App.Application.Context;
            var packageManager = context.PackageManager!;

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O && !packageManager.CanRequestPackageInstalls())
            {
                var settingsIntent = new Intent(global::Android.Provider.Settings.ActionManageUnknownAppSources,
                    global::Android.Net.Uri.Parse($"package:{context.PackageName}"));
                settingsIntent.AddFlags(ActivityFlags.NewTask);
                context.StartActivity(settingsIntent);
                ReportError("Allow YWML to install unknown apps, then try again.");
                return Task.CompletedTask;
            }

            var authority = $"{context.PackageName}.fileprovider";
            var uri = AndroidX.Core.Content.FileProvider.GetUriForFile(context, authority, new Java.IO.File(downloadedFile));

            var installIntent = new Intent(Intent.ActionView);
            installIntent.SetDataAndType(uri, APK_MIME);
            installIntent.AddFlags(ActivityFlags.GrantReadUriPermission | ActivityFlags.NewTask);
            context.StartActivity(installIntent);

            return Task.CompletedTask;
        }
    }
}
