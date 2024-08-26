namespace YWML.Src.Utils.GeneralUtils
{
    public static class CGeneralUtils
    {
        public const string APP_VERSION = "1.0.0";
        private static string ExeDir = AppDomain.CurrentDomain.BaseDirectory;
        private static string AppDataDir = Environment.ExpandEnvironmentVariables("%APPDATA%/YWML");
        public static string ExtensionInstallDirectory = Path.Combine(AppDataDir, "extensions_install");
        public static string ExtensionInstalledList = Path.Combine(ExtensionInstallDirectory,"installed_ext.json");
        public static string ExtensionLibraryCachePath = Path.Combine(AppDataDir, "extension_lib_cache");
        public static string TmpDirectory = Path.Combine(AppDataDir, "tmp");
        public static string ConfigFilePath = Path.Combine(ExeDir, "config.toml");
        public static string DefaultInstallationDirectoriesPath = Path.Combine(AppDataDir, "default_install_dirs.json");
    }
}
