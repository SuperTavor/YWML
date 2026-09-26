using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YWML.Src.ExtensionLibrary.DataClasses
{
    public class CInstalledExtensionMetadata
    {
        public string FAName { get; set; }
        public string Name { get; set; }
        public string TitleId { get; set; } 
        public bool IsDisableAutoInstall { get; set; }
    }
}
