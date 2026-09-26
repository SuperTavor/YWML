namespace YWML.Src.Install
{
    public class CLocalInstallTarget : IModInstallTarget
    {
        private readonly string _destinationRoot;

        public CLocalInstallTarget(string destinationRoot)
        {
            _destinationRoot = destinationRoot;
        }

        public Task InstallAsync(
            byte[] fa,
            string faName,
            Dictionary<string, string> rawFiles,
            IProgress<string> status,
            IProgress<int> percent,
            CancellationToken cancellationToken = default)
        {
            status.Report("Preparing files...");
            return Task.Run(() => CModInstallBuilder.Build(_destinationRoot, fa, faName, rawFiles), cancellationToken);
        }
    }
}
