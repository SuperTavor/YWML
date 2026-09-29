namespace YWML.Src.Utils.GeneralUtils
{
    public static class CGeneralUtils
    {
        public const string APP_VERSION = "1.3.0";

        public const string REPO_URL = "https://github.com/SuperTavor/YWML";
        public const string WINDOWS_ASSET = "ywml_win_setup.exe";
        public const string ANDROID_ASSET = "com.ywml.android-Signed.apk";

        public static string LatestReleaseApiUrl => REPO_URL.Replace("github.com", "api.github.com/repos") + "/releases/latest";

        public static string YWMLDataDir { get; private set; } = string.Empty;

        public static string ExtensionInstallDirectory => Path.Combine(YWMLDataDir, "extensions_install");
        public static string ExtensionInstalledList => Path.Combine(ExtensionInstallDirectory, "installed_ext.json");
        public static string ExtensionLibraryCachePath => Path.Combine(YWMLDataDir, "extension_lib_cache");
        public static string TmpDirectory => Path.Combine(YWMLDataDir, "tmp");
        public static string WritableConfigPath => Path.Combine(YWMLDataDir, "config.toml");
        public static string DefaultInstallationDirectoriesPath => Path.Combine(YWMLDataDir, "default_install_dirs.json");
        public static string RemoteInstallStagingDir => Path.Combine(YWMLDataDir, "remote_install_staging");
        public static string ZipModsStagingDir => Path.Combine(YWMLDataDir, "zip_mods");

        public static void Initialize(string dataDir)
        {
            YWMLDataDir = dataDir;
        }
    }
}
