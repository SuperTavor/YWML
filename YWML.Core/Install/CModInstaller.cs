using YWML.Src.Loader;

namespace YWML.Src.Install
{
    public class CModInstaller
    {
        public async Task InstallAsync(
            string patchableFaPath,
            string faName,
            IReadOnlyList<string> modPathsLeastToMostImportant,
            IModInstallTarget target,
            IProgress<string> status,
            IProgress<int> percent)
        {
            var result = CLoader.ModifyFA(modPathsLeastToMostImportant, patchableFaPath);
            try
            {
                var modifiedFa = result.Archive.Save();
                await target.InstallAsync(modifiedFa, faName, result.RawFiles, status, percent);
            }
            finally
            {
                result.Archive.BaseStream.Close();
            }
        }
    }
}
