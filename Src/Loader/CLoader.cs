using YWML.Src.Utils.Arc0Ex;
namespace YWML.Src.Loader
{
    public static class CLoader
    {
        public static (CARC0Ex Archive, Dictionary<string,string> RawFiles) ModifyFA(TreeView modsTreeView, Dictionary<string, string> modPaths, string faToLoad)
        {
            var fs = new FileStream(faToLoad, FileMode.Open, FileAccess.ReadWrite);
            CARC0Ex arcEx = new CARC0Ex(fs);

            // get the list of mod paths from least to most important
            var modPathsFromLeastImportant = modsTreeView.Nodes
                .Cast<TreeNode>()
                .Select(node => modPaths[node.Text].Replace("\\", "/"))
                .Reverse()
                .ToList();

            // store files for patching in the fa 
            var filesToPatch = new Dictionary<string, byte[]>();
            //Keep track of the base directory and the big file path so we can copy shit properly.
            var rawFiles = new Dictionary<string,string>();

            foreach (var modPath in modPathsFromLeastImportant)
            {
                // add all files except those in "include"
                foreach(var f in Directory.EnumerateFiles(modPath, "*", SearchOption.AllDirectories)
                                .Where(file => !file.Contains("include\\")))
                {
                    rawFiles[f] = modPath;
                }

                // handle files inside the "include" folder
                var includePath = Path.Combine(modPath, "include");
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
            }

            foreach(var file in filesToPatch)
            {
                arcEx.AddOrReplace(file.Key, file.Value);
            }
            return (arcEx, rawFiles
                .Where(x => !x.Key.Contains("ywml.json"))
                .ToDictionary(x => x.Key, x => x.Value));
        }


    }
}
