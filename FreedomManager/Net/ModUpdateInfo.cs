using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreedomManager.Net
{
    internal class ModUpdateInfo
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }
        public string DownloadLink { get; set; }
        public bool DoUpdate { get; set; }
    }
}
