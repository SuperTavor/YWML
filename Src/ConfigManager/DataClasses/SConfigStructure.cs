using YWML.Src.Install.DataClasses;

namespace YWML.Src.ConfigManager.DataClasses
{
    public struct SConfigStructure
    {
        public string ExtensionLibraryURL { get; set; } 

        public bool IsUpdateFirstBoot { get; set; }

        public string FtpHost { get; set; }
        public int FtpPort { get; set; }

        public SInstallMode LastUsedInstallMode { get; set; }

        public SConfigStructure()
        {

        }
    }
}
