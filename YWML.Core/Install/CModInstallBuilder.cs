namespace YWML.Src.Install
{
    public static class CModInstallBuilder
    {
        public static void Build(string root, byte[] fa, string faName, Dictionary<string, string> rawFiles)
        {
            Build(new FileSystemInstallDestination(root), fa, faName, rawFiles);
        }

        public static void Build(IInstallDestination destination, byte[] fa, string faName, Dictionary<string, string> rawFiles)
        {
            using (var faStream = destination.OpenWrite(faName))
            {
                faStream.Write(fa, 0, fa.Length);
            }

            foreach (var rawFile in rawFiles)
            {
                var relativePath = Path.GetRelativePath(rawFile.Value, rawFile.Key);
                using var sourceStream = File.OpenRead(rawFile.Key);
                using var destinationStream = destination.OpenWrite(relativePath);
                sourceStream.CopyTo(destinationStream);
            }
        }
    }
}
