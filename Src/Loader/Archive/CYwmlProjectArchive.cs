using YWML.Src.Loader.Archive.DataClasses;

namespace YWML.Src.Loader.Archive
{
    public class CYwmlProjectArchive : IDisposable
    {
        private const string PROJECT_FILE = "ywml.json";
        private const string INCLUDE_DIR = "include";

        private readonly IArchiveReader _reader;
        private bool _disposed;

        public string ProjectLevel { get; }

        public CYwmlProjectArchive(IArchiveReader reader)
        {
            _reader = reader;
            try
            {
                ProjectLevel = FindLevel(PROJECT_FILE);
            }
            catch
            {
                _reader.Dispose();
                throw;
            }
        }

        private string FindLevel(string fileName)
        {
            foreach (var entry in _reader.Entries)
            {
                var normalized = Normalize(entry.FullName);
                if (!normalized.Equals(fileName, StringComparison.OrdinalIgnoreCase)
                    && !normalized.EndsWith("/" + fileName, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                var level = GetDirectory(normalized);
                if (HasIncludeDirectory(level))
                {
                    return level;
                }
            }

            throw new InvalidDataException("YWML project with an 'include' directory was not found.");
        }

        private bool HasIncludeDirectory(string level)
        {
            var includePrefix = level.Length == 0 ? INCLUDE_DIR + "/" : level + "/" + INCLUDE_DIR + "/";
            var includeExact = includePrefix.TrimEnd('/');

            foreach (var entry in _reader.Entries)
            {
                var normalized = Normalize(entry.FullName);
                if (normalized.Equals(includeExact, StringComparison.OrdinalIgnoreCase)
                    || normalized.StartsWith(includePrefix, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        public void ExtractTo(string destinationRoot, IProgress<int>? progress = null)
        {
            if (_disposed) throw new ObjectDisposedException(nameof(CYwmlProjectArchive));

            destinationRoot = Path.GetFullPath(destinationRoot);
            Directory.CreateDirectory(destinationRoot);

            var levelPrefix = ProjectLevel.Length == 0 ? string.Empty : ProjectLevel + "/";

            long totalBytes = 0;
            foreach (var entry in _reader.Entries)
            {
                if (!entry.IsDirectory && TryGetRelativePath(entry, levelPrefix, out _))
                {
                    totalBytes += entry.Size;
                }
            }

            progress?.Report(0);
            long extractedBytes = 0;

            foreach (var entry in _reader.Entries)
            {
                if (!TryGetRelativePath(entry, levelPrefix, out var relative))
                {
                    continue;
                }

                var targetPath = ResolveSafePath(destinationRoot, relative);

                if (entry.IsDirectory)
                {
                    Directory.CreateDirectory(targetPath);
                    continue;
                }

                var targetDirectory = Path.GetDirectoryName(targetPath);
                if (!string.IsNullOrEmpty(targetDirectory))
                {
                    Directory.CreateDirectory(targetDirectory);
                }

                using (var entryStream = _reader.OpenEntry(entry))
                using (var fileStream = new FileStream(targetPath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    entryStream.CopyTo(fileStream);
                }

                extractedBytes += entry.Size;
                if (progress != null && totalBytes > 0)
                {
                    progress.Report((int)Math.Min(100, extractedBytes * 100 / totalBytes));
                }
            }

            progress?.Report(100);
        }

        private static bool TryGetRelativePath(CArchiveEntry entry, string levelPrefix, out string relative)
        {
            var normalized = Normalize(entry.FullName);

            if (levelPrefix.Length > 0
                && !normalized.StartsWith(levelPrefix, StringComparison.OrdinalIgnoreCase))
            {
                relative = string.Empty;
                return false;
            }

            relative = levelPrefix.Length == 0 ? normalized : normalized.Substring(levelPrefix.Length);
            return relative.Length > 0;
        }

        private static string Normalize(string path)
        {
            return path.Replace("\\", "/");
        }

        private static string GetDirectory(string normalizedPath)
        {
            var lastSlash = normalizedPath.LastIndexOf('/');
            return lastSlash < 0 ? string.Empty : normalizedPath.Substring(0, lastSlash);
        }

        private static string ResolveSafePath(string destinationRoot, string relative)
        {
            var combined = Path.GetFullPath(Path.Combine(destinationRoot, relative));
            var rootWithSeparator = destinationRoot.EndsWith(Path.DirectorySeparatorChar)
                ? destinationRoot
                : destinationRoot + Path.DirectorySeparatorChar;

            if (!combined.StartsWith(rootWithSeparator, StringComparison.OrdinalIgnoreCase)
                && !string.Equals(combined, destinationRoot, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidDataException($"Archive entry escapes the destination directory: {relative}");
            }

            return combined;
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _reader.Dispose();
            _disposed = true;
        }
    }
}
