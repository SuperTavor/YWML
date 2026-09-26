namespace YWML.Src.Install
{
    public static class CInstallModeRules
    {
        public static bool IsLocalControlEnabled(bool isRemote)
        {
            return !isRemote;
        }

        public static bool IsAutoDetectEnabled(bool isRemote, bool autoDetectDisabledForGame)
        {
            return !isRemote && !autoDetectDisabledForGame;
        }
    }
}
