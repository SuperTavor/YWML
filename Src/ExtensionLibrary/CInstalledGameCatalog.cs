using YWML.Src.ExtensionLibrary.DataClasses;

namespace YWML.Src.ExtensionLibrary
{
    public class CInstalledGameCatalog
    {
        private readonly CExtensionLibrary _library = new();
        private readonly Dictionary<string, string> _nameToId = new();

        public void Refresh()
        {
            _library.LoadInstalledList();

            _nameToId.Clear();
            foreach (var key in _library.InstalledList.Keys)
            {
                _nameToId[_library.InstalledList[key].Name] = key;
            }
        }

        public IReadOnlyList<string> Names => _nameToId.Keys.ToList();

        public string? GetId(string? name)
        {
            if (name == null)
            {
                return null;
            }

            return _nameToId.TryGetValue(name, out var id) ? id : null;
        }

        public CInstalledExtensionMetadata? Get(string? name)
        {
            var id = GetId(name);
            if (id == null)
            {
                return null;
            }

            return _library.InstalledList.TryGetValue(id, out var metadata) ? metadata : null;
        }
    }
}
