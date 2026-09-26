using Newtonsoft.Json;
using YWML.Src.Install.DataClasses;

namespace YWML.Src.Install
{
    public static class CInstallTargetStore
    {
        public static Dictionary<string, SInstallTarget> Parse(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                return new();
            }

            try
            {
                var current = JsonConvert.DeserializeObject<Dictionary<string, SInstallTarget>>(json);
                if (current != null)
                {
                    return current;
                }
            }
            catch
            {
                
            }

            try
            {
                var legacy = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                if (legacy != null)
                {
                    return legacy.ToDictionary(
                        pair => pair.Key,
                        pair => new SInstallTarget { Path = pair.Value, IsRemote = false });
                }
            }
            catch
            {
                
            }

            return new();
        }

        public static string Serialize(Dictionary<string, SInstallTarget> targets)
        {
            return JsonConvert.SerializeObject(targets);
        }
    }
}
