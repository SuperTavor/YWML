
using YWML.Src.Utils.Tinifan.ArchiveL5.Level5.Archive.ARC0;

namespace YWML.Src.FAMerger
{
    internal class CFAMerger
    {
        private string _modPath { get; set; }
        private string _romfsPath { get; set; }
        public ARC0[] ModFAHandles { get; set; }
        public List<(int FAIdx, string FilePath)> ChangedOrAddedFiles { get; set; }
        public CFAMerger(string modPath, string romfsFolder)
        {
            _modPath = modPath;
            _romfsPath = romfsFolder;
        }
        public int GetDiffs()
        {
            ChangedOrAddedFiles = [];

            var modFaFiles = Directory
            .GetFiles(_modPath, "*.fa")
            .Select(Path.GetFileName)
            .OrderBy(x => x)
            .ToArray();
            
            ModFAHandles = new ARC0[modFaFiles.Length];
            var romfsFaFiles = Directory
                .GetFiles(_romfsPath, "*.fa")
                .Select(Path.GetFileName)
                .OrderBy(x => x)
                .ToArray();

            var romfsSet = new HashSet<string>(romfsFaFiles);

            if (!modFaFiles.All(f => romfsSet.Contains(f)))
            {
                MessageBox.Show(
                    "The .FA files in the mod folder and the original RomFS folder do not match.\n" +
                    "Please make sure the original RomFS has all the FA files the mod uses.");
                return 1;
            }

            for (int i = 0; i < modFaFiles.Length; i++)
            {
                var file = modFaFiles[i];
                var modFaFs = new FileStream(Path.Combine(_modPath, file), FileMode.Open, FileAccess.Read);
                var romfsFaFs = new FileStream(Path.Combine(_romfsPath, file), FileMode.Open, FileAccess.Read);
                var modFaHandle = new ARC0(modFaFs);
                var ogFaHandle = new ARC0(romfsFaFs);

                try
                {
                    foreach (var patchFile in modFaHandle.Directory.GetAllFiles())
                    {
                        try
                        {
                            patchFile.Stream.Read();
                            var correspondingFile = ogFaHandle.Directory.GetFileFromFullPath(patchFile.Path);
                          
                            //Check Lengths first of all
                            if (patchFile.Stream.ByteContent.Length != correspondingFile.Length)
                            {
                                ChangedOrAddedFiles.Add((i, patchFile.Path));
                                continue;
                            }

                            if (!patchFile.Stream.ByteContent.SequenceEqual(correspondingFile))
                            {
                                ChangedOrAddedFiles.Add((i, patchFile.Path));
                            }
                        }
                        catch (FileNotFoundException)
                        {
                            ChangedOrAddedFiles.Add((i,patchFile.Path));
                        }
                        catch(DirectoryNotFoundException)
                        {
                           ChangedOrAddedFiles.Add((i, patchFile.Path));
                        }
                    }
                }
                finally
                {
                    ogFaHandle.Close();
                }

                ModFAHandles[i] = modFaHandle;
            }

            return 0;
        }

    }
}
