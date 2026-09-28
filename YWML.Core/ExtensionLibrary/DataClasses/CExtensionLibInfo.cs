using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YWML.Src.ExtensionLibrary.DataClasses
{
    public class CExtensionLibInfo
    {
        public List<string> ExtensionCategories { get; set;  }
        public List<CExtension> ExtensionList { get; set; }
    }
}
