using YWML.Src.Utils.Arc0Ex;

namespace YWML.Src.Loader.DataClasses
{
    public sealed class SModLoadResult
    {
        public required CARC0Ex Archive { get; init; }
        public required Dictionary<string, string> RawFiles { get; init; }
        public required Dictionary<string, string> ExeFsFiles { get; init; }
    }
}
