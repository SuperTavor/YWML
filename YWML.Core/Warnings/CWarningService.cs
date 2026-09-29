using YWML.Src.Loader;
using YWML.Src.Warnings.DataClasses;

namespace YWML.Src.Warnings
{
    public static class CWarningService
    {
        public static SModWarning? CheckExeFs(string modPath)
        {
            try
            {
                CExeFsFolder.ValidateFiles(modPath);
                return null;
            }
            catch (CExeFsFileException ex)
            {
                return new SModWarning("warn: invalid files in include_code", ex.Message);
            }
        }
    }
}
