using System.Reflection;
using Tomlet;
using YWML.Src.ConfigManager.DataClasses;
using YWML.Src.Utils.GeneralUtils;

namespace YWML.Src.ConfigManager
{
    public static class CConfigManager
    {
        public static SConfigStructure Cfg;

        private static string ReadInitialConfigFile()
        {
            string resourceName = "YWML.config.toml";

            var assembly = Assembly.GetExecutingAssembly();
            using Stream stream = assembly.GetManifestResourceStream(resourceName);
            using StreamReader reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
        public static void Initialize()
        {
            //Deserialize 
            if (!File.Exists(CGeneralUtils.WritableConfigPath))
            {
                Cfg = TomletMain.To<SConfigStructure>(ReadInitialConfigFile());
            }
            else Cfg = TomletMain.To<SConfigStructure>(File.ReadAllText(CGeneralUtils.WritableConfigPath));

            if (Cfg.ExtensionLibraryURL == null || Cfg.ExtensionLibraryURL == "")
            {
                MessageBox.Show("Cannot find the extension library source URL in config.toml. Please reinstall the application or add your own source URL for custom extensions.");
                Environment.Exit(1);
            }

            if(!(Cfg.IsUpdateFirstBoot is bool))
            {
                MessageBox.Show("The IsUpdateFirstBoot variable in the config is corrupted. Please reinstall the app");
                Environment.Exit(1);
            }
        }

        public static void UpdateConfig()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(CGeneralUtils.WritableConfigPath));
            File.WriteAllText(CGeneralUtils.WritableConfigPath, TomletMain.TomlStringFrom(Cfg));
        }

    }
}