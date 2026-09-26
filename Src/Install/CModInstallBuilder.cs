namespace YWML.Src.Install
{
    public static class CModInstallBuilder
    {
        public static void Build(string root, byte[] fa, string faName, Dictionary<string, string> rawFiles)
        {
            Directory.CreateDirectory(root);

            var faOutputPath = Path.Combine(root, faName);
            var faDirectory = Path.GetDirectoryName(faOutputPath);
            if (!string.IsNullOrEmpty(faDirectory))
            {
                Directory.CreateDirectory(faDirectory);
            }
            File.WriteAllBytes(faOutputPath, fa);

            foreach (var rawFile in rawFiles)
            {
                var relativePath = Path.GetRelativePath(rawFile.Value, rawFile.Key);
                var outputPath = Path.Combine(root, relativePath);
                var outputDirectory = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                File.Copy(rawFile.Key, outputPath, true);
            }
        }
    }
}
