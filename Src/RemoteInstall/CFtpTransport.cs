using System.Text;
using FluentFTP;
using YWML.Src.RemoteInstall.DataClasses;

namespace YWML.Src.RemoteInstall
{
    public class CFtpTransport : IFtpTransport
    {
        private AsyncFtpClient? _client;

        public async Task ConnectAsync(SFtpConnectionInfo info, CancellationToken cancellationToken = default)
        {
            _client = new AsyncFtpClient(info.Host, info.UserName, info.Password, info.Port);
            _client.Encoding = Encoding.Latin1;
            await _client.Connect(cancellationToken);
        }

        public async Task EnsureDirectoryAsync(string remotePath, CancellationToken cancellationToken = default)
        {
            if (_client == null)
            {
                throw new InvalidOperationException("FTP client is not connected.");
            }

            var parts = CRemotePath.ToPosix(remotePath).Split('/', StringSplitOptions.RemoveEmptyEntries);
            var current = "";
            foreach (var part in parts)
            {
                current += "/" + part;
                await _client.CreateDirectory(current, cancellationToken);
            }
        }

        public async Task UploadFileAsync(string localPath, string remotePath, IProgress<int>? percent = null, CancellationToken cancellationToken = default)
        {
            if (_client == null)
            {
                throw new InvalidOperationException("FTP client is not connected.");
            }

            var progress = percent == null ? null : new Progress<FtpProgress>(p => percent.Report((int)p.Progress));
            await _client.UploadFile(localPath, remotePath, FtpRemoteExists.Overwrite, true, FtpVerify.None, progress, cancellationToken);
        }

        public async Task DisconnectAsync()
        {
            if (_client != null)
            {
                await _client.Disconnect();
                _client.Dispose();
                _client = null;
            }
        }
    }
}
