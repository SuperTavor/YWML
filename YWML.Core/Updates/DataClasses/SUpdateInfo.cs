namespace YWML.Src.Updates.DataClasses
{
    public sealed class SUpdateInfo
    {
        public required string Version { get; init; }
        public required string AssetName { get; init; }
        public required string DownloadUrl { get; init; }
        public long Size { get; init; }
        public string? Digest { get; init; }
    }
}
