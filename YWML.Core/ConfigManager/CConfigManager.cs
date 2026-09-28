using System.Reflection;
using Tomlet;
using YWML.Src.ConfigManager.DataClasses;
using YWML.Src.RemoteInstall.DataClasses;
using YWML.Src.Utils.GeneralUtils;

namespace YWML.Src.ConfigManager
{
    public static class CConfigManager
    {
        public static SConfigStructure Cfg;

        private static string ReadInitialConfigFile()
        {
            string resourceName = "YWML.Core.config.toml";

            var assembly = Assembly.GetExecutingAssembly();
            using Stream stream = assembly.GetManifestResourceStream(resourceName);
            using StreamReader reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }

        public static void Initialize()
        {
            Cfg = LoadOrReset();

            if (string.IsNullOrEmpty(Cfg.ExtensionLibraryURL))
            {
                throw new InvalidDataException("Cannot find the extension library source URL in config.toml. Please reinstall the application or add your own source URL for custom extensions.");
            }

            Cfg.FtpHost ??= string.Empty;
            Cfg.LastUsedTargetGame ??= string.Empty;
            Cfg.FtpPort = NormalizeFtpPort(Cfg.FtpPort);
        }

        //Loads the saved config, falling back to the defaults if it's missing or unreadable (e.g. written by an
        //older, incompatible version) so a stale config can never crash startup.
        private static SConfigStructure LoadOrReset()
        {
            if (File.Exists(CGeneralUtils.WritableConfigPath))
            {
                try
                {
                    return TomletMain.To<SConfigStructure>(File.ReadAllText(CGeneralUtils.WritableConfigPath));
                }
                catch
                {
                    var fallback = TomletMain.To<SConfigStructure>(ReadInitialConfigFile());
                    fallback.IsUpdateFirstBoot = false;
                    SaveConfig(fallback);
                    return fallback;
                }
            }

            return TomletMain.To<SConfigStructure>(ReadInitialConfigFile());
        }

        private static void SaveConfig(SConfigStructure config)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(CGeneralUtils.WritableConfigPath)!);
            File.WriteAllText(CGeneralUtils.WritableConfigPath, TomletMain.TomlStringFrom(config));
        }

        public static int NormalizeFtpPort(int port)
        {
            return port is > 0 and <= 65535 ? port : SFtpConnectionInfo.DEFAULT_PORT;
        }

        public static void UpdateConfig()
        {
            SaveConfig(Cfg);
        }

    }
}
