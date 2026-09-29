using Newtonsoft.Json;

namespace YWML.Src.Updates.DataClasses
{
    public sealed class SReleaseInfo
    {
        [JsonProperty("tag_name")]
        public string TagName { get; set; } = string.Empty;

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("assets")]
        public List<SReleaseAsset> Assets { get; set; } = new();
    }
}
