namespace YWML.Src.Install
{
    public static class CModInstallBuilder
    {
        public static void Build(IInstallDestination destination, byte[] fa, string faName, Dictionary<string, string> rawFiles, Dictionary<string, string> exeFsFiles)
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

            foreach (var exeFsFile in exeFsFiles)
            {
                using var sourceStream = File.OpenRead(exeFsFile.Value);
                using var destinationStream = destination.Parent.OpenWrite(exeFsFile.Key);
                sourceStream.CopyTo(destinationStream);
            }
        }
    }
}
