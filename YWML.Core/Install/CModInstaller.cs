using YWML.Src.Loader;
using YWML.Src.Loader.DataClasses;

namespace YWML.Src.Install
{
    public class CModInstaller
    {
        public async Task InstallAsync(
            string patchableFaPath,
            string faName,
            IReadOnlyList<(string Path, SExeFsMode ExeFsMode)> modsLeastToMostImportant,
            IModInstallTarget target,
            IProgress<string> status,
            IProgress<int> percent)
        {
            var result = CLoader.ModifyFA(modsLeastToMostImportant, patchableFaPath);
            try
            {
                var modifiedFa = result.Archive.Save();
                await target.InstallAsync(modifiedFa, faName, result.RawFiles, result.ExeFsFiles, status, percent);
            }
            finally
            {
                result.Archive.BaseStream.Close();
            }
        }
    }
}
