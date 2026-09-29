using YWML.Src.RemoteInstall;
using YWML.Src.RemoteInstall.DataClasses;
using YWML.Src.Utils.GeneralUtils;

namespace YWML.Src.Install
{
    public class CFtpInstallTarget : IModInstallTarget
    {
        private const string ROMFS_STAGING_FOLDER = "romfs";

        private readonly IFtpTransport _transport;
        private readonly SFtpConnectionInfo _connectionInfo;
        private readonly string _remoteRoot;
        private readonly string _stagingRoot;

        public CFtpInstallTarget(IFtpTransport transport, SFtpConnectionInfo connectionInfo, string remoteRoot, string? stagingRoot = null)
        {
            _transport = transport;
            _connectionInfo = connectionInfo;
            _remoteRoot = remoteRoot;
            _stagingRoot = stagingRoot ?? CGeneralUtils.RemoteInstallStagingDir;
        }

        public async Task InstallAsync(
            byte[] fa,
            string faName,
            Dictionary<string, string> rawFiles,
            Dictionary<string, string> exeFsFiles,
            IProgress<string> status,
            IProgress<int> percent,
            CancellationToken cancellationToken = default)
        {
            DeleteStaging();

            try
            {
                status.Report("Preparing files...");

                //Stage the mod root so its parent (where ExeFS files go) maps to the remote parent directory.
                var romFsStaging = Path.Combine(_stagingRoot, ROMFS_STAGING_FOLDER);
                await Task.Run(() => CModInstallBuilder.Build(new FileSystemInstallDestination(romFsStaging), fa, faName, rawFiles, exeFsFiles), cancellationToken);

                status.Report("Connecting...");
                await _transport.ConnectAsync(_connectionInfo, cancellationToken);

                var remoteParentRoot = CRemotePath.GetDirectoryName(_remoteRoot) ?? _remoteRoot;
                await _transport.EnsureDirectoryAsync(remoteParentRoot, cancellationToken);

                status.Report("Uploading...");
                await UploadStagedTreeAsync(remoteParentRoot, percent, cancellationToken);
            }
            finally
            {
                try
                {
                    await _transport.DisconnectAsync();
                }
                catch
                {
                    
                }

                DeleteStaging();
            }
        }

        private async Task UploadStagedTreeAsync(string remoteParentRoot, IProgress<int> percent, CancellationToken cancellationToken)
        {
            var files = Directory.GetFiles(_stagingRoot, "*", SearchOption.AllDirectories);

            long totalBytes = 0;
            foreach (var file in files)
            {
                totalBytes += new FileInfo(file).Length;
            }

            long uploadedBytes = 0;
            foreach (var file in files)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var relativePath = Path.GetRelativePath(_stagingRoot, file);
                var remotePath = CRemotePath.Combine(remoteParentRoot, relativePath);

                var remoteDirectory = CRemotePath.GetDirectoryName(remotePath);
                if (!string.IsNullOrEmpty(remoteDirectory))
                {
                    await _transport.EnsureDirectoryAsync(remoteDirectory, cancellationToken);
                }

                var fileSize = new FileInfo(file).Length;
                var baseUploaded = uploadedBytes;
                var currentTotal = totalBytes;
                var fileProgress = new CProgress<int>(p =>
                    percent.Report(currentTotal == 0 ? 100 : (int)((baseUploaded + fileSize * p / 100) * 100 / currentTotal)));

                await _transport.UploadFileAsync(file, remotePath, fileProgress, cancellationToken);
                uploadedBytes += fileSize;
            }

            percent.Report(100);
        }

        private void DeleteStaging()
        {
            if (Directory.Exists(_stagingRoot))
            {
                Directory.Delete(_stagingRoot, true);
            }
        }
    }
}
