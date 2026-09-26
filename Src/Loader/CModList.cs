namespace YWML.Src.Loader
{
    public class CModList
    {
        private readonly List<(string Name, string Path)> _entries = new();

        public int Count => _entries.Count;

        public IReadOnlyList<string> Names => _entries.Select(entry => entry.Name).ToList();

        public void Add(string name, string path)
        {
            _entries.Add((name, path));
        }

        public void RemoveAt(int index)
        {
            if (index >= 0 && index < _entries.Count)
            {
                _entries.RemoveAt(index);
            }
        }

        public bool MoveUp(int index)
        {
            if (index <= 0)
            {
                return false;
            }

            (_entries[index - 1], _entries[index]) = (_entries[index], _entries[index - 1]);
            return true;
        }

        public bool MoveDown(int index)
        {
            if (index < 0 || index >= _entries.Count - 1)
            {
                return false;
            }

            (_entries[index + 1], _entries[index]) = (_entries[index], _entries[index + 1]);
            return true;
        }

        public IReadOnlyList<string> GetPathsLeastToMostImportant()
        {
            return _entries.Select(entry => entry.Path).Reverse().ToList();
        }
    }
}
