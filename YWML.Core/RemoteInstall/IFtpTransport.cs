using YWML.Src.RemoteInstall.DataClasses;

namespace YWML.Src.RemoteInstall
{
    public interface IFtpTransport
    {
        Task ConnectAsync(SFtpConnectionInfo info, CancellationToken cancellationToken = default);

        Task EnsureDirectoryAsync(string remotePath, CancellationToken cancellationToken = default);

        Task UploadFileAsync(string localPath, string remotePath, IProgress<int>? percent = null, CancellationToken cancellationToken = default);

        Task DisconnectAsync();
    }
}
