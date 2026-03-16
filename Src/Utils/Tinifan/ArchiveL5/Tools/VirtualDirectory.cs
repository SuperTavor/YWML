using System;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace YWML.Src.Utils.Tinifan.ArchiveL5.Tools
{
    /// <summary>
    /// Represents a file entry in the flat search index.
    /// </summary>
    public class FileIndexEntry
    {
        public string Name { get; set; }
        public string FullPath { get; set; }
        public SubMemoryStream StreamData { get; set; }
    }

    /// <summary>
    /// Represents a folder entry in the flat search index.
    /// </summary>
    public class FolderIndexEntry
    {
        public string Name { get; set; }
        public string FullPath { get; set; }
        public VirtualDirectory Directory { get; set; }
    }

    /// <summary>
    /// Represents a directory in a virtual, in-memory file system.
    /// It can contain files (as SubMemoryStream) and other VirtualDirectory instances as subfolders.
    /// </summary>
    public class VirtualDirectory
    {
        /// <summary>
        /// Gets or sets the name of the virtual directory.
        /// </summary>
        public string Name;

        /// <summary>
        /// Gets or sets the list of subdirectories contained within this directory.
        /// </summary>
        public List<VirtualDirectory> Folders;

        /// <summary>
        /// Gets or sets the dictionary of files contained within this directory.
        /// The key is the file name, and the value is a SubMemoryStream representing the file's content.
        /// </summary>
        public Dictionary<string, SubMemoryStream> Files;

        /// <summary>
        /// Gets or sets the color associated with this directory, often for UI display purposes.
        /// </summary>
        public Color Color = Color.Black;

        /// <summary>
        /// A cache for frequently accessed folders to improve performance.
        /// </summary>
        private Dictionary<string, VirtualDirectory> _folderCache;

        /// <summary>
        /// A lock object to ensure thread-safe operations on the folder cache.
        /// </summary>
        private readonly object _cacheLock = new object();

        // --- NEW FIELDS FOR THE SEARCH INDEX ---

        /// <summary>
        /// A flat list of all files for fast searching.
        /// </summary>
        private List<FileIndexEntry> _flatFileIndex;

        /// <summary>
        /// A flat list of all folders for fast searching.
        /// </summary>
        private List<FolderIndexEntry> _flatFolderIndex;

        /// <summary>
        /// A lock to ensure thread-safe operations on the search index.
        /// </summary>
        private readonly object _indexLock = new object();

        /// <summary>
        /// The "dirty flag". If true, the index is out-of-date and must be rebuilt before the next search.
        /// </summary>
        private bool _isIndexStale = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualDirectory"/> class with default values.
        /// </summary>
        public VirtualDirectory()
        {
            Folders = new List<VirtualDirectory>();
            Files = new Dictionary<string, SubMemoryStream>();
            _folderCache = new Dictionary<string, VirtualDirectory>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualDirectory"/> class with a specified name.
        /// </summary>
        /// <param name="name">The name of the directory.</param>
        public VirtualDirectory(string name)
        {
            Name = name;
            Folders = new List<VirtualDirectory>();
            Files = new Dictionary<string, SubMemoryStream>();
            _folderCache = new Dictionary<string, VirtualDirectory>();
        }

        /// <summary>
        /// Gets a direct subfolder by its name. Uses a cache for faster subsequent lookups.
        /// </summary>
        /// <param name="name">The name of the folder to retrieve.</param>
        /// <returns>The <see cref="VirtualDirectory"/> if found; otherwise, null.</returns>
        public VirtualDirectory GetFolder(string name)
        {
            if (_folderCache.TryGetValue(name, out VirtualDirectory cachedFolder))
            {
                return cachedFolder;
            }

            var folder = Folders.FirstOrDefault(f => f.Name == name);

            if (folder != null)
            {
                lock (_cacheLock)
                {
                    if (!_folderCache.ContainsKey(name))
                    {
                        _folderCache[name] = folder;
                    }
                }
            }

            return folder;
        }

        /// <summary>
        /// Retrieves a folder from a relative full path (e.g., "SubFolder1/SubFolder2").
        /// </summary>
        /// <param name="path">The relative path to the folder, using '/' as a separator.</param>
        /// <returns>The <see cref="VirtualDirectory"/> at the specified path.</returns>
        /// <exception cref="DirectoryNotFoundException">Thrown if the path does not exist.</exception>
        public VirtualDirectory GetFolderFromFullPath(string path)
        {
            if (string.IsNullOrEmpty(path) || path == "/")
                return this;

            var pathSplit = path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            var current = this;

            for (int i = 0; i < pathSplit.Length; i++)
            {
                current = current.GetFolder(pathSplit[i]);

                if (current == null)
                {
                    throw new DirectoryNotFoundException(path + " does not exist");
                }
            }

            return current;
        }

        /// <summary>
        /// Checks if a folder exists at the specified relative path.
        /// </summary>
        /// <param name="path">The relative path to check, using '/' as a separator.</param>
        /// <returns>True if the folder exists; otherwise, false.</returns>
        public bool IsFolderExists(string path)
        {
            if (string.IsNullOrEmpty(path))
                return true;

            var pathSplit = path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            var current = this;

            for (int i = 0; i < pathSplit.Length; i++)
            {
                current = current.GetFolder(pathSplit[i]);

                if (current == null)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Gets a list of all direct subfolders within the current directory.
        /// </summary>
        /// <returns>A new list containing all direct subfolders.</returns>
        public List<VirtualDirectory> GetAllFolders()
        {
            var allFolders = new List<VirtualDirectory>(Folders.Count);
            allFolders.AddRange(Folders);
            return allFolders;
        }

        /// <summary>
        /// Recursively gets all subfolders and returns them as a dictionary, with the full relative path as the key.
        /// </summary>
        /// <returns>A dictionary mapping full folder paths to <see cref="VirtualDirectory"/> instances.</returns>
        public Dictionary<string, VirtualDirectory> GetAllFoldersAsDictionnary()
        {
            var directories = new Dictionary<string, VirtualDirectory> { { Name + "/", this } };
            var stack = new Stack<(VirtualDirectory folder, string parentPath)>();

            foreach (var folder in Folders)
            {
                stack.Push((folder, Name + "/"));
            }

            while (stack.Count > 0)
            {
                var (currentFolder, parentPath) = stack.Pop();
                var currentPath = parentPath + currentFolder.Name + "/";

                if (!directories.ContainsKey(currentPath))
                {
                    directories.Add(currentPath, currentFolder);
                }

                foreach (var subFolder in currentFolder.Folders)
                {
                    stack.Push((subFolder, currentPath));
                }
            }

            return directories;
        }

        /// <summary>
        /// Retrieves the byte content of a file from a relative full path (e.g., "Folder/file.txt").
        /// </summary>
        /// <param name="path">The relative path to the file, using '/' as a separator.</param>
        /// <returns>A byte array representing the file's content.</returns>
        public byte[] GetFileFromFullPath(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentException("Path cannot be null or empty");

            var pathSplit = path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            var fileName = pathSplit[pathSplit.Length - 1];
            var current = this;

            for (int i = 0; i < pathSplit.Length - 1; i++)
            {
                current = current.GetFolder(pathSplit[i]);
                if (current == null)
                {
                    throw new DirectoryNotFoundException(path + " does not exist");
                }
            }

            if (current.Files.TryGetValue(fileName, out SubMemoryStream subMemoryStream))
            {
                if (subMemoryStream.ByteContent == null)
                {
                    subMemoryStream.Read();
                }
                return subMemoryStream.ByteContent;
            }
            else
            {
                throw new FileNotFoundException(fileName + " does not exist");
            }
        }

        /// <summary>
        /// Gets a SubMemoryStream from the specified full path.
        /// </summary>
        /// <param name="path">The full path of the file.</param>
        /// <returns>The file as a byte array.</returns>
        public SubMemoryStream GetSubMemoryStreamFromFullPath(string path)
        {
            var pathSplit = path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            var fileName = pathSplit[pathSplit.Length - 1];
            var current = this;

            // Get Path
            for (int i = 0; i < pathSplit.Length - 1; i++)
            {
                current = current.GetFolder(pathSplit[i]);

                if (current == null)
                {
                    throw new DirectoryNotFoundException(path + " not exist");
                }
            }

            if (current.Files.ContainsKey(fileName))
            {
                return current.Files[fileName];
            }
            else
            {
                throw new FileNotFoundException(fileName + " not exist");
            }
        }

        /// <summary>
        /// Recursively gets all files within this directory and its subdirectories.
        /// </summary>
        /// <returns>A dictionary mapping full file paths to <see cref="SubMemoryStream"/> instances.</returns>
        public IEnumerable<(string Path, SubMemoryStream Stream)> GetAllFiles()
        {
            var stack = new Stack<(VirtualDirectory folder, string path)>();
            stack.Push((this, Name));

            while (stack.Count > 0)
            {
                var (currentFolder, currentPath) = stack.Pop();

                if (currentFolder.Files != null)
                {
                    foreach (var file in currentFolder.Files)
                    {
                        yield return (currentPath + "/" + file.Key, file.Value);
                    }
                }

                if (currentFolder.Folders != null)
                {
                    foreach (var subFolder in currentFolder.Folders)
                    {
                        stack.Push((subFolder, currentPath + "/" + subFolder.Name));
                    }
                }
            }
        }

        /// <summary>
        /// Adds a file to the current directory. If a file with the same name exists, it is overwritten.
        /// </summary>
        public void AddFile(string name, SubMemoryStream data)
        {
            Files[name] = data;
            InvalidateIndex();
        }

        /// <summary>
        /// Adds a new, empty subfolder to the current directory.
        /// </summary>
        public void AddFolder(string name)
        {
            var newFolder = new VirtualDirectory(name);
            Folders.Add(newFolder);
            InvalidateCacheAndIndex();
        }

        /// <summary>
        /// Adds an existing <see cref="VirtualDirectory"/> instance as a subfolder.
        /// </summary>
        public void AddFolder(VirtualDirectory folder)
        {
            Folders.Add(folder);
            InvalidateCacheAndIndex();
        }

        /// <summary>
        /// Recursively calculates the total size in bytes of all files in this directory and its subdirectories.
        /// </summary>
        public long GetSize()
        {
            long size = 0;

            if (Files.Count > 100)
            {
                var fileSizes = new ConcurrentBag<long>();
                Parallel.ForEach(Files.Values, file => fileSizes.Add(file.ByteContent?.Length ?? file.Size));
                size = fileSizes.Sum();
            }
            else
            {
                foreach (var file in Files.Values)
                {
                    size += file.ByteContent?.Length ?? file.Size;
                }
            }

            if (Folders.Count > 1)
            {
                var folderSizes = new ConcurrentBag<long>();
                Parallel.ForEach(Folders, folder => folderSizes.Add(folder.GetSize()));
                size += folderSizes.Sum();
            }
            else
            {
                foreach (var folder in Folders)
                {
                    size += folder.GetSize();
                }
            }
            return size;
        }

        /// <summary>
        /// Reorganizes the entire directory structure.
        /// </summary>
        public void Reorganize()
        {
            var folders = GetAllFolders().OrderBy(x => x.Name).ToArray();

            foreach (var folderPath in folders)
            {
                var pathSplit = folderPath.Name.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                var currentPath = "";

                foreach (var folderName in pathSplit)
                {
                    currentPath = Path.Combine(currentPath, folderName) + "\\";
                    if (currentPath.Count(c => c == '\\') > 1 && GetFolder(currentPath.Replace("\\", "/")) == null)
                    {
                        AddFolder(currentPath.Replace("\\", "/"));
                    }
                }
            }

            folders = GetAllFolders().OrderBy(x => x.Name).ToArray();
            var result = new VirtualDirectory("");
            result.Files = new Dictionary<string, SubMemoryStream>(Files);

            foreach (var folder in folders.Where(x => !string.IsNullOrEmpty(x.Name)))
            {
                var path = folder.Name.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                var current = result;
                for (int i = 0; i < path.Length; i++)
                {
                    if (current.GetFolder(path[i]) == null)
                    {
                        var newFolder = new VirtualDirectory(path[i]) { Files = new Dictionary<string, SubMemoryStream>(folder.Files) };
                        current.AddFolder(newFolder);
                    }
                    current = current.GetFolder(path[i]);
                }
            }

            Files = result.Files;
            Folders = result.Folders;

            InvalidateCacheAndIndex();
        }

        /// <summary>
        /// Recursively sorts all folders and files alphabetically (case-insensitive).
        /// </summary>
        public void SortAlphabetically()
        {
            Folders.Sort((x, y) => StringComparer.OrdinalIgnoreCase.Compare(x.Name, y.Name));

            if (Folders.Count > 10)
            {
                Parallel.ForEach(Folders, folder => folder.SortAlphabetically());
            }
            else
            {
                foreach (var folder in Folders)
                {
                    folder.SortAlphabetically();
                }
            }

            if (Files.Count > 1)
            {
                Files = Files.OrderBy(file => file.Key, StringComparer.OrdinalIgnoreCase)
                               .ToDictionary(pair => pair.Key, pair => pair.Value);
            }
        }

        /// <summary>
        /// Prints the directory structure to the console, starting from the current directory.
        /// </summary>
        public void Print()
        {
            Print(this);
        }

        /// <summary>
        /// Helper method to print the directory structure recursively with indentation.
        /// </summary>
        public void Print(VirtualDirectory directory, int level = 0)
        {
            var indentation = new string('\t', level);
            Console.WriteLine($"{indentation}/{directory.Name}: ");
            foreach (var subDirectory in directory.Folders)
            {
                Print(subDirectory, level + 1);
            }
            foreach (var file in directory.Files)
            {
                var fileIndentation = new string('\t', level + 1);
                Console.WriteLine($"{fileIndentation}{file.Key}");
            }
        }

        /// <summary>
        /// Searches for directories whose names contain the specified search string, using the optimized index.
        /// </summary>
        public List<VirtualDirectory> SearchDirectories(string directoryName, string basePath = "/")
        {
            EnsureIndexIsReady();

            return _flatFolderIndex
                .Where(folder => folder.FullPath.StartsWith(basePath, StringComparison.OrdinalIgnoreCase) &&
                                 folder.Name.IndexOf(directoryName, StringComparison.OrdinalIgnoreCase) >= 0)
                .Select(entry => entry.Directory)
                .ToList();
        }

        /// <summary>
        /// Searches for files whose names contain the specified search string, using the optimized index.
        /// </summary>
        public List<KeyValuePair<string, SubMemoryStream>> SearchFiles(string fileName, string basePath = "/")
        {
            EnsureIndexIsReady();

            return _flatFileIndex
                .Where(file => file.FullPath.StartsWith(basePath, StringComparison.OrdinalIgnoreCase) &&
                               file.Name.IndexOf(fileName, StringComparison.OrdinalIgnoreCase) >= 0)
                .Select(entry => new KeyValuePair<string, SubMemoryStream>(entry.FullPath, entry.StreamData))
                .ToList();
        }

        /// <summary>
        /// Gets the full path of the current directory relative to a given root directory.
        /// </summary>
        public string GetFullPath(VirtualDirectory root)
        {
            return GetFullPath(root, this);
        }

        /// <summary>
        /// Finds the full path of a target directory by traversing from a starting directory.
        /// </summary>
        public string GetFullPath(VirtualDirectory currentDirectory, VirtualDirectory searchedDirectory)
        {
            var stack = new Stack<(VirtualDirectory dir, string path)>();
            stack.Push((currentDirectory, ""));
            while (stack.Count > 0)
            {
                var (current, currentPath) = stack.Pop();
                foreach (var directory in current.Folders)
                {
                    if (directory == searchedDirectory)
                    {
                        return string.IsNullOrEmpty(currentPath) ? directory.Name : currentPath + "/" + directory.Name;
                    }
                    var newPath = string.IsNullOrEmpty(currentPath) ? directory.Name : currentPath + "/" + directory.Name;
                    stack.Push((directory, newPath));
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Resets the color of this directory, its files, and all subdirectories recursively to black.
        /// </summary>
        public void ResetColor()
        {
            Color = Color.Black;

            if (Files.Count > 100)
            {
                Parallel.ForEach(Files.Values, file => file.Color = Color.Black);
            }
            else
            {
                foreach (var file in Files.Values)
                {
                    file.Color = Color.Black;
                }
            }

            if (Folders.Count > 10)
            {
                Parallel.ForEach(Folders, subFolder => subFolder.ResetColor());
            }
            else
            {
                foreach (var subFolder in Folders)
                {
                    subFolder.ResetColor();
                }
            }
        }

        /// <summary>
        /// Builds a flat index of all files and folders for extremely fast searching.
        /// This should be called once after loading or after any modification.
        /// </summary>
        /// <param name="forceRebuild">If true, rebuilds the index even if it is not marked as stale.</param>
        public void BuildSearchIndex(bool forceRebuild = false)
        {
            lock (_indexLock)
            {
                // If the index is not stale (up-to-date) and we are not forcing a rebuild, do nothing.
                if (!_isIndexStale && !forceRebuild && _flatFileIndex != null && _flatFolderIndex != null)
                {
                    return;
                }

                _flatFileIndex = new List<FileIndexEntry>();
                _flatFolderIndex = new List<FolderIndexEntry>();

                // Use an iterative stack-based traversal to avoid StackOverflowException on deep hierarchies.
                var stack = new Stack<(VirtualDirectory dir, string path)>();
                stack.Push((this, "/")); // Start at the root directory.

                while (stack.Count > 0)
                {
                    var (currentDir, currentPath) = stack.Pop();

                    // Index files in the current directory.
                    foreach (var file in currentDir.Files)
                    {
                        _flatFileIndex.Add(new FileIndexEntry
                        {
                            Name = file.Key,
                            FullPath = currentPath + file.Key,
                            StreamData = file.Value
                        });
                    }

                    // Index subdirectories and add them to the stack for traversal.
                    foreach (var subDir in currentDir.Folders)
                    {
                        string subDirPath = currentPath + subDir.Name + "/";
                        _flatFolderIndex.Add(new FolderIndexEntry
                        {
                            Name = subDir.Name,
                            FullPath = subDirPath,
                            Directory = subDir
                        });
                        stack.Push((subDir, subDirPath));
                    }
                }

                // The index is now up-to-date.
                _isIndexStale = false;
            }
        }

        /// <summary>
        /// A gatekeeper method that ensures the index is ready before a search is performed.
        /// If the index is stale, it triggers a rebuild.
        /// </summary>
        private void EnsureIndexIsReady()
        {
            // If the flag is set or the index has never been built, rebuild it.
            if (_isIndexStale || _flatFileIndex == null || _flatFolderIndex == null)
            {
                BuildSearchIndex(forceRebuild: true);
            }
        }

        /// <summary>
        /// Marks the search index as stale ("dirty"). Called whenever the directory structure changes.
        /// </summary>
        private void InvalidateIndex()
        {
            lock (_indexLock)
            {
                _isIndexStale = true;
            }
        }

        /// <summary>
        /// Invalidates both the folder cache and the search index.
        /// </summary>
        private void InvalidateCacheAndIndex()
        {
            lock (_cacheLock)
            {
                _folderCache.Clear();
            }
            InvalidateIndex();
        }
    }
}

