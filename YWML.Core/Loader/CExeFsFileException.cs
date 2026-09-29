namespace YWML.Src.Loader
{
    public sealed class CExeFsFileException : Exception
    {
        public CExeFsFileException(IReadOnlyList<string> illegalFiles)
            : base(BuildMessage(illegalFiles))
        {
            IllegalFiles = illegalFiles;
        }

        public IReadOnlyList<string> IllegalFiles { get; }

        private static string BuildMessage(IReadOnlyList<string> illegalFiles)
        {
            return $"The {CExeFsFolder.Name} contains invalid exefs patches. Valid exefs patches only include {string.Join(" or ", CExeFsFolder.SupportedFileNames)} files. " +
                   $"Offending files: {string.Join(", ", illegalFiles)}.";
        }
    }
}
