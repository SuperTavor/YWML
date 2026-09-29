namespace YWML.Src.Install
{
    public interface IModInstallTarget
    {
        Task InstallAsync(
            byte[] fa,
            string faName,
            Dictionary<string, string> rawFiles,
            Dictionary<string, string> exeFsFiles,
            IProgress<string> status,
            IProgress<int> percent,
            CancellationToken cancellationToken = default);
    }
}
