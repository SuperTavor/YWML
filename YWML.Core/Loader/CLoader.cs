using YWML.Src.Loader.DataClasses;
using YWML.Src.Utils.Arc0Ex;
namespace YWML.Src.Loader
{
    public static class CLoader
    {
        private const string PROJECT_CONFIG_FILE = "ywml.json";

        public static SModLoadResult ModifyFA(IReadOnlyList<(string Path, SExeFsMode ExeFsMode)> modsLeastToMostImportant, string faToLoad)
        {
            var fs = new FileStream(faToLoad, FileMode.Open, FileAccess.ReadWrite);
            var arcEx = new CARC0Ex(fs);

            // store files for patching in the fa 
            var filesToPatch = new Dictionary<string, byte[]>();
            //Keep track of the base directory and the big file path so we can copy shit properly.
            var rawFiles = new Dictionary<string, string>();
            //ExeFS files that live one level above the romfs output.
            var exeFsFiles = new Dictionary<string, string>();

            foreach (var mod in modsLeastToMostImportant)
            {
                var modPath = mod.Path;
                var includePath = Path.Combine(modPath, "include");
                var codePath = Path.Combine(modPath, CExeFsFolder.Name);

                // add all files except those in "include" and "include_code"
                foreach (var f in Directory.EnumerateFiles(modPath, "*", SearchOption.AllDirectories)
                                .Where(file => !IsInsideDirectory(file, includePath) && !IsInsideDirectory(file, codePath)))
                {
                    rawFiles[f] = modPath;
                }

                // handle files inside the "include" folder
                if (Directory.Exists(includePath))
                {
                    foreach (var file in Directory.EnumerateFiles(includePath, "*", SearchOption.AllDirectories))
                    {
                        // change the path to skip everything before "include"
                        var relativePath = Path.GetRelativePath(includePath, file).Replace("\\", "/");
                        filesToPatch[relativePath] = File.ReadAllBytes(file);
                    }
                }
                else
                {
                    throw new DirectoryNotFoundException("Make sure you have an include folder.");
                }

                // handle files inside the "include_code" folder
                foreach (var file in CExeFsFolder.CollectFiles(modPath, mod.ExeFsMode))
                {
                    exeFsFiles[file.Key] = file.Value;
                }
            }

            foreach (var file in filesToPatch)
            {
                arcEx.AddOrReplace(file.Key, file.Value);
            }
            return new SModLoadResult
            {
                Archive = arcEx,
                RawFiles = rawFiles
                    .Where(x => !Path.GetFileName(x.Key).Equals(PROJECT_CONFIG_FILE, StringComparison.OrdinalIgnoreCase))
                    .ToDictionary(x => x.Key, x => x.Value),
                ExeFsFiles = exeFsFiles,
            };
        }

        private static bool IsInsideDirectory(string file, string directory)
        {
            var relative = Path.GetRelativePath(directory, file);
            return !relative.StartsWith("..") && !Path.IsPathRooted(relative);
        }

    }
}
