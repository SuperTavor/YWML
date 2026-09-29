using YWML.Src.Net;
using YWML.Src.Updates.DataClasses;

namespace YWML.Src.Updates
{
    public sealed class CUpdateFlow
    {
        private readonly IUpdatePlatform _platform;
        private readonly CUpdateService _service;
        private readonly HttpMessageHandler? _handler;
        private SUpdateInfo? _pending;

        public CUpdateFlow(IUpdatePlatform platform, HttpMessageHandler? handler = null)
        {
            _platform = platform;
            _handler = handler;
            _service = new CUpdateService(handler);
        }

        public bool HasPending => _pending != null;

        public async Task CheckAsync(CancellationToken cancellationToken = default)
        {
            if (!_platform.IsConnected)
            {
                return;
            }

            _pending = await _service.CheckAsync(_platform.Platform, cancellationToken);
        }

        public async Task PresentPendingAsync(CancellationToken cancellationToken = default)
        {
            var update = _pending;
            if (update == null)
            {
                return;
            }

            _pending = null;

            var choice = await _platform.AskAsync(update);
            if (choice == SUpdateChoice.Never)
            {
                CUpdateService.Skip(update.Version);
                return;
            }

            if (choice != SUpdateChoice.InstallNow)
            {
                return;
            }

            var path = _platform.GetDownloadPath(update.AssetName);
            var directory = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var progress = _platform.BeginDownload(update);
            try
            {
                await CFileDownloader.DownloadAsync(update.DownloadUrl, path, progress, _handler);
            }
            finally
            {
                _platform.EndDownload();
            }

            if (!CUpdateService.VerifyChecksum(path, update.Digest))
            {
                _platform.ReportError("The downloaded update failed its integrity check and was discarded.");
                return;
            }

            await _platform.InstallAsync(update, path);
        }
    }
}
