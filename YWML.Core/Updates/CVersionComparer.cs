namespace YWML.Src.Updates
{
    public static class CVersionComparer
    {
        public static string Normalize(string tag)
        {
            var value = (tag ?? string.Empty).Trim();
            if (value.StartsWith("v", StringComparison.OrdinalIgnoreCase))
            {
                value = value[1..];
            }

            return value;
        }

        public static bool IsNewer(string candidateTag, string currentVersion)
        {
            if (!TryParse(Normalize(candidateTag), out var candidate) || !TryParse(Normalize(currentVersion), out var current))
            {
                return false;
            }

            return candidate > current;
        }

        private static bool TryParse(string value, out Version version)
        {
            version = new Version(0, 0);

            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            //Drop any pre-release/build suffix (e.g. 1.2.0-beta.1).
            var suffix = value.IndexOfAny(new[] { '-', '+' });
            var numeric = suffix >= 0 ? value[..suffix] : value;

            if (!Version.TryParse(numeric, out var parsed))
            {
                return false;
            }

            version = parsed;
            return true;
        }
    }
}
