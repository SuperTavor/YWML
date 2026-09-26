using YWML.Src.Loader.Archive.DataClasses;

namespace YWML.Src.Loader.Archive
{
    public interface IArchiveReader : IDisposable
    {
        IReadOnlyList<CArchiveEntry> Entries { get; }

        Stream OpenEntry(CArchiveEntry entry);
    }
}
