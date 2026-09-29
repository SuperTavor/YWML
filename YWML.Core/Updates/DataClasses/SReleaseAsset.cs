using Newtonsoft.Json;

namespace YWML.Src.Updates.DataClasses
{
    public sealed class SReleaseAsset
    {
        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty("browser_download_url")]
        public string DownloadUrl { get; set; } = string.Empty;

        [JsonProperty("size")]
        public long Size { get; set; }

        [JsonProperty("digest")]
        public string? Digest { get; set; }
    }
}
