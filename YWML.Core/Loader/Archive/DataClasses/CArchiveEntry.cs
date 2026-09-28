namespace YWML.Src.Loader.Archive.DataClasses
{
    public class CArchiveEntry
    {
        public string FullName { get; }
        public bool IsDirectory { get; }
        public long Size { get; }

        public CArchiveEntry(string fullName, bool isDirectory, long size)
        {
            FullName = fullName;
            IsDirectory = isDirectory;
            Size = size;
        }
    }
}
