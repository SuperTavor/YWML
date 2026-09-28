using SharpCompress.Archives;
using SharpCompress.Archives.SevenZip;
using SharpCompress.Readers;
using YWML.Src.Loader.Archive;
using YWML.Src.Loader.Archive.DataClasses;

namespace YWML.Src.Loader.SevenZip
{
    public class C7zArchiveReader : IArchiveReader
    {
        private readonly IArchive _archive;
        private readonly List<CArchiveEntry> _entries = new();
        private readonly Dictionary<string, IArchiveEntry> _archiveEntries = new(StringComparer.Ordinal);
        private bool _disposed;

        public IReadOnlyList<CArchiveEntry> Entries => _entries;

        public C7zArchiveReader(string path)
        {
            _archive = SevenZipArchive.OpenArchive(path, new ReaderOptions());
            try
            {
                foreach (var entry in _archive.Entries)
                {
                    var key = entry.Key ?? string.Empty;
                    _archiveEntries[key] = entry;
                    _entries.Add(new CArchiveEntry(key, entry.IsDirectory, entry.Size));
                }
            }
            catch
            {
                _archive.Dispose();
                _disposed = true;
                throw;
            }
        }

        public Stream OpenEntry(CArchiveEntry entry)
        {
            if (_disposed) throw new ObjectDisposedException(nameof(C7zArchiveReader));

            if (!_archiveEntries.TryGetValue(entry.FullName, out var archiveEntry))
            {
                throw new InvalidDataException($"Archive entry not found: {entry.FullName}");
            }

            return archiveEntry.OpenEntryStream();
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _archive.Dispose();
            _disposed = true;
        }
    }
}
