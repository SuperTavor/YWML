using Newtonsoft.Json;

namespace YWML.Src.Install
{
    public class CInstallDirectoryStore
    {
        private readonly string _path;
        private Dictionary<string, string> _directories = new();

        public CInstallDirectoryStore(string path)
        {
            _path = path;
        }

        public void Load()
        {
            _directories = File.Exists(_path)
                ? JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(_path)) ?? new()
                : new();
        }

        public void Save()
        {
            File.WriteAllText(_path, JsonConvert.SerializeObject(_directories));
        }

        public string? Get(string id)
        {
            return _directories.TryGetValue(id, out var directory) ? directory : null;
        }

        public void Set(string id, string path)
        {
            _directories[id] = path;
        }
    }
}
