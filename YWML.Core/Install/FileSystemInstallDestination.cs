namespace YWML.Src.Install
{
    public class FileSystemInstallDestination : IInstallDestination
    {
        private readonly string _root;

        public FileSystemInstallDestination(string root)
        {
            _root = root;
        }

        public Stream OpenWrite(string relativePath)
        {
            var path = Path.Combine(_root, relativePath);
            var directory = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }

            return new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.ReadWrite | FileShare.Delete);
        }
    }
}
