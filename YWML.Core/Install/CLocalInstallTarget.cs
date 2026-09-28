namespace YWML.Src.Install
{
    public class CLocalInstallTarget : IModInstallTarget
    {
        private readonly IInstallDestination _destination;

        public CLocalInstallTarget(IInstallDestination destination)
        {
            _destination = destination;
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
            return Task.Run(() => CModInstallBuilder.Build(_destination, fa, faName, rawFiles), cancellationToken);
        }
    }
}
