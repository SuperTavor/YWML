using System.Net;
using System.Reflection;
using Tomlet;
using YWML.Src.ConfigManager.DataClasses;
using YWML.Src.Utils.GeneralUtils;

namespace YWML.Src.ConfigManager
{
    public static class CConfigManager
    {
        public static SConfigStructure Cfg;
        private static Dictionary<string, string> _extensionLibrary;
        public static void Initialize()
        {
            //Deserialize 
            if (!File.Exists(CGeneralUtils.ConfigFilePath))
            {
                MessageBox.Show("config.toml file not found. Either make one or reinstall the application for a default one to appear.");
                Environment.Exit(1);
            }
            Cfg = TomletMain.To<SConfigStructure>(File.ReadAllText(CGeneralUtils.ConfigFilePath));

            if (Cfg.ExtensionLibraryURL == null || Cfg.ExtensionLibraryURL == "")
            {
                MessageBox.Show("Invalid config.toml structure. Either make a new one or reinstall the application for a default one to appear.");
                Environment.Exit(1);
            }

        }

    }
}