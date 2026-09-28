using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YWML.Src.Loader.DataClasses
{
    public class CYwmlProject
    {
        public string? Name { get; set; }
        public string? Version { get; set;  }
        public string? Author { get; set; }

        public string DisplayName => string.IsNullOrWhiteSpace(Name) ? "Mod" : Name;
        public string DisplayVersion => string.IsNullOrWhiteSpace(Version) ? "v0.0" : Version;
        public string DisplayAuthor => string.IsNullOrWhiteSpace(Author) ? "John Doe" : Author;
    }

}
