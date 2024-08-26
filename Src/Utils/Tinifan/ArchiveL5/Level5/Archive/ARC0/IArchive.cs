using YWML.Src.Utils.Tinifan.ArchiveL5.Tools;

namespace YWML.Src.Utils.Tinifan.ArchiveL5.Level5.Archive.ARC0
{
    public interface IArchive
    {
        string Name { get; }

        VirtualDirectory Directory { get; set; }

        void Save(string path, ProgressBar progressBar);

        byte[] Save();

        IArchive Close();
    }
}
