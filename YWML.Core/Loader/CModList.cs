using YWML.Src.Loader.DataClasses;

namespace YWML.Src.Loader
{
    public class CModList
    {
        private readonly List<(CYwmlProject Project, string Path)> _entries = new();

        public int Count => _entries.Count;

        public IReadOnlyList<(CYwmlProject Project, string Path)> Entries => _entries;

        public void Add(CYwmlProject project, string path)
        {
            _entries.Add((project, path));
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
