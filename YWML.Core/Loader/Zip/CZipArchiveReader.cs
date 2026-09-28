using System.IO.Compression;
using YWML.Src.Loader.Archive;
using YWML.Src.Loader.Archive.DataClasses;

namespace YWML.Src.Loader.Zip
{
    public class CZipArchiveReader : IArchiveReader
    {
        private readonly ZipArchive _archive;
        private readonly List<CArchiveEntry> _entries = new();
        private readonly Dictionary<string, ZipArchiveEntry> _zipEntries = new(StringComparer.Ordinal);
        private bool _disposed;

        public IReadOnlyList<CArchiveEntry> Entries => _entries;

        public CZipArchiveReader(string path)
        {
            _archive = ZipFile.OpenRead(path);
            try
            {
                foreach (var entry in _archive.Entries)
                {
                    var isDirectory = entry.FullName.EndsWith("/") || entry.FullName.EndsWith("\\");
                    _zipEntries[entry.FullName] = entry;
                    _entries.Add(new CArchiveEntry(entry.FullName, isDirectory, entry.Length));
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
            if (_disposed) throw new ObjectDisposedException(nameof(CZipArchiveReader));

            if (!_zipEntries.TryGetValue(entry.FullName, out var zipEntry))
            {
                throw new InvalidDataException($"Archive entry not found: {entry.FullName}");
            }

            return zipEntry.Open();
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
