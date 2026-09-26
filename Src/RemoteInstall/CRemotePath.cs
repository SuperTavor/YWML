namespace YWML.Src.RemoteInstall
{
    public static class CRemotePath
    {
        public const string MODDED_3DS_TITLE_PREFIX = "/luma/titles";

        public static string GetModded3dsRomfsRoot(string titleId)
        {
            return Combine(MODDED_3DS_TITLE_PREFIX, $"{titleId}/romfs");
        }

        public static string ToPosix(string path)
        {
            return (path ?? string.Empty).Replace("\\", "/");
        }

        public static string Combine(string root, string relativePath)
        {
            var normalizedRoot = ToPosix(root).TrimEnd('/');
            var normalizedRelative = ToPosix(relativePath).TrimStart('/');

            if (normalizedRoot.Length == 0)
            {
                return "/" + normalizedRelative;
            }

            if (normalizedRelative.Length == 0)
            {
                return normalizedRoot;
            }

            return normalizedRoot + "/" + normalizedRelative;
        }

        public static string? GetDirectoryName(string remotePath)
        {
            var normalized = ToPosix(remotePath);
            var lastSlash = normalized.LastIndexOf('/');

            if (lastSlash < 0)
            {
                return null;
            }

            if (lastSlash == 0)
            {
                return "/";
            }

            return normalized.Substring(0, lastSlash);
        }
    }
}
