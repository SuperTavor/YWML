using YWML.Src.ExtensionLibrary;
using YWML.Src.Install;
using YWML.Src.Loader;
using YWML.Src.RemoteInstall;
using YWML.Src.RemoteInstall.DataClasses;

namespace YWML.Android.Services
{
    public sealed class CAppState
    {
        public static CAppState Current { get; } = new();

        public CExtensionLibrary ExtensionLibrary { get; } = new();
        public CInstalledGameCatalog GameCatalog { get; } = new();
        public CModList ModList { get; } = new();
        public CUserFolderStore UserFolder { get; } = new();
        public CExtensionInstallState ExtensionInstall { get; } = new();
        public IFtpTransport FtpTransport { get; } = new CFtpTransport();
        public CModInstaller Installer { get; } = new();

        public SFtpConnectionInfo? FtpConnection { get; set; }
        public string? StartupError { get; set; }
        public bool UpdateChecked { get; set; }
    }
}
