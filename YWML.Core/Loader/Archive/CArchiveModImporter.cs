using YWML.Src.Loader.DataClasses;

namespace YWML.Src.Loader.Archive
{
    public class CArchiveModImporter
    {
        private readonly string _stagingRoot;
        private readonly List<string> _stagingDirs = new();

        public CArchiveModImporter(string stagingRoot)
        {
            _stagingRoot = stagingRoot;
        }

        public (string ProjectPath, CYwmlProject Project) Import(string archivePath, IProgress<int>? progress = null)
        {
            var stagingDir = Path.Combine(_stagingRoot, Guid.NewGuid().ToString("N"));

            using (var archive = new CYwmlProjectArchive(CArchiveReaderFactory.Open(archivePath)))
            {
                archive.ExtractTo(stagingDir, progress);
            }
            _stagingDirs.Add(stagingDir);

            var project = CYwmlProjectReader.Read(stagingDir);
            return (stagingDir, project);
        }

        public void Cleanup()
        {
            foreach (var stagingDir in _stagingDirs)
            {
                try
                {
                    if (Directory.Exists(stagingDir))
                    {
                        Directory.Delete(stagingDir, true);
                    }
                }
                catch
                {

                }
            }

            _stagingDirs.Clear();
        }
    }
}
