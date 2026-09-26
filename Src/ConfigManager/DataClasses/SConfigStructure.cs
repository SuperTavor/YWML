using YWML.Src.ExtensionLibrary.DataClasses;

namespace YWML.Src.ConfigManager.DataClasses
{
    public struct SConfigStructure
    {
        public string ExtensionLibraryURL { get; set; } 

        public bool IsUpdateFirstBoot { get; set; }

        public string FtpHost { get; set; }
        public int FtpPort { get; set; }
        public string FtpUserName { get; set; }

        public SConfigStructure()
        {

        }
    }
}
