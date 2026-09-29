using System.Security.Cryptography;
using Newtonsoft.Json;
using YWML.Src.ConfigManager;
using YWML.Src.Updates.DataClasses;
using YWML.Src.Utils.GeneralUtils;

namespace YWML.Src.Updates
{
    public sealed class CUpdateService
    {
        private readonly HttpMessageHandler? _handler;

        public CUpdateService(HttpMessageHandler? handler = null)
        {
            _handler = handler;
        }

        public async Task<SUpdateInfo?> CheckAsync(SUpdatePlatform platform, CancellationToken cancellationToken = default)
        {
            var release = await FetchLatestReleaseAsync(cancellationToken);
            if (release == null)
            {
                return null;
            }

            if (!CVersionComparer.IsNewer(release.TagName, CGeneralUtils.APP_VERSION))
            {
                return null;
            }

            var version = CVersionComparer.Normalize(release.TagName);
            if (CConfigManager.Cfg.SkippedUpdateVersions?.Contains(version) == true)
            {
                return null;
            }

            var asset = SelectAsset(release.Assets, platform);
            if (asset == null)
            {
                return null;
            }

            return new SUpdateInfo
            {
                Version = version,
                AssetName = asset.Name,
                DownloadUrl = asset.DownloadUrl,
                Size = asset.Size,
                Digest = asset.Digest,
            };
        }

        public async Task<SReleaseInfo?> FetchLatestReleaseAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                using var client = _handler == null ? new HttpClient() : new HttpClient(_handler);
                client.DefaultRequestHeaders.UserAgent.ParseAdd("YWML");
                client.DefaultRequestHeaders.Accept.ParseAdd("application/vnd.github+json");

                var json = await client.GetStringAsync(CGeneralUtils.LatestReleaseApiUrl, cancellationToken);
                return JsonConvert.DeserializeObject<SReleaseInfo>(json);
            }
            catch
            {
                return null;
            }
        }

        public static SReleaseAsset? SelectAsset(IReadOnlyList<SReleaseAsset> assets, SUpdatePlatform platform)
        {
            var expected = platform == SUpdatePlatform.Windows ? CGeneralUtils.WINDOWS_ASSET : CGeneralUtils.ANDROID_ASSET;
            return assets.FirstOrDefault(asset => asset.Name.Equals(expected, StringComparison.OrdinalIgnoreCase));
        }

        public static bool VerifyChecksum(string filePath, string? digest)
        {
            if (string.IsNullOrWhiteSpace(digest))
            {
                return false;
            }

            var separator = digest.IndexOf(':');
            if (separator <= 0)
            {
                return false;
            }

            using var algorithm = CreateHashAlgorithm(digest[..separator]);
            if (algorithm == null)
            {
                return false;
            }

            try
            {
                var expected = Convert.FromHexString(digest[(separator + 1)..]);
                using var stream = File.OpenRead(filePath);
                var actual = algorithm.ComputeHash(stream);
                return CryptographicOperations.FixedTimeEquals(actual, expected);
            }
            catch
            {
                return false;
            }
        }

        public static void Skip(string version)
        {
            CConfigManager.Cfg.SkippedUpdateVersions ??= new List<string>();
            if (CConfigManager.Cfg.SkippedUpdateVersions.Contains(version))
            {
                return;
            }

            CConfigManager.Cfg.SkippedUpdateVersions.Add(version);
            CConfigManager.UpdateConfig();
        }

        private static HashAlgorithm? CreateHashAlgorithm(string name)
        {
            return name.Trim().ToLowerInvariant() switch
            {
                "sha1" => SHA1.Create(),
                "sha256" => SHA256.Create(),
                "sha512" => SHA512.Create(),
                _ => null,
            };
        }
    }
}
