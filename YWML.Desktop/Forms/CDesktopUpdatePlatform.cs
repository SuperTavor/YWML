using System.Diagnostics;
using System.Net.NetworkInformation;
using YWML.Src.Updates;
using YWML.Src.Updates.DataClasses;

namespace YWML.Src.Forms
{
    public sealed class CDesktopUpdatePlatform : IUpdatePlatform
    {
        private readonly Form _owner;
        private ArchiveLoadingForm? _progressForm;

        public CDesktopUpdatePlatform(Form owner)
        {
            _owner = owner;
        }

        public SUpdatePlatform Platform => SUpdatePlatform.Windows;

        public bool IsConnected => NetworkInterface.GetIsNetworkAvailable();

        public Task<SUpdateChoice> AskAsync(SUpdateInfo update)
        {
            return Task.FromResult(UpdatePromptForm.Prompt(_owner, update));
        }

        public string GetDownloadPath(string assetName)
        {
            return Path.Combine(Path.GetTempPath(), "ywml_update", assetName);
        }

        public IProgress<int>? BeginDownload(SUpdateInfo update)
        {
            _progressForm = new ArchiveLoadingForm(update.AssetName);
            _progressForm.Show(_owner);
            return new Progress<int>(_progressForm.SetProgress);
        }

        public void EndDownload()
        {
            _progressForm?.Close();
            _progressForm?.Dispose();
            _progressForm = null;
        }

        public void ReportError(string message)
        {
            MessageBox.Show(message, "YWML");
        }

        public Task InstallAsync(SUpdateInfo update, string downloadedFile)
        {
            //The Inno Setup installer closes the running app itself (Restart Manager), so we don't exit here.
            var process = Process.Start(new ProcessStartInfo(downloadedFile) { UseShellExecute = true });
            if (process == null || process.HasExited)
            {
                ReportError("The update installer could not be started.");
            }

            return Task.CompletedTask;
        }
    }
}
