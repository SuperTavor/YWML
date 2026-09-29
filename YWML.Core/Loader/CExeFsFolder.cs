using YWML.Src.Loader.DataClasses;

namespace YWML.Src.Loader
{
    public static class CExeFsFolder
    {
        public const string Name = "include_code";

        public static readonly IReadOnlySet<string> SupportedFileNames =
            new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "code.ips", "code.bin" };

        public static bool IsSupported(string fileName)
        {
            return SupportedFileNames.Contains(fileName);
        }

        public static void ValidateFiles(string modPath)
        {
            var codeDir = Path.Combine(modPath, Name);
            if (!Directory.Exists(codeDir))
            {
                return;
            }

            var illegalFiles = new List<string>();
            foreach (var file in Directory.GetFiles(codeDir))
            {
                var fileName = Path.GetFileName(file);
                if (!IsSupported(fileName))
                {
                    illegalFiles.Add(fileName);
                }
            }

            if (illegalFiles.Count > 0)
            {
                throw new CExeFsFileException(illegalFiles);
            }
        }

        public static IReadOnlyDictionary<string, string> CollectFiles(string modPath, SExeFsMode mode)
        {
            var files = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            var codeDir = Path.Combine(modPath, Name);
            if (!Directory.Exists(codeDir))
            {
                return files;
            }

            if (mode == SExeFsMode.All)
            {
                foreach (var file in Directory.GetFiles(codeDir))
                {
                    files[Path.GetFileName(file)] = file;
                }

                return files;
            }

            foreach (var fileName in SupportedFileNames)
            {
                var file = Path.Combine(codeDir, fileName);
                if (File.Exists(file))
                {
                    files[fileName] = file;
                }
            }

            return files;
        }
    }
}
