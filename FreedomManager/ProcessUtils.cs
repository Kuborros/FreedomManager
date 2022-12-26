using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreedomManager
{
    public static class ProcessUtils
    {
        public static void OpenUrl(string url)
        {
            using (var proc = new Process())
            {
                proc.StartInfo.UseShellExecute = true;
                proc.StartInfo.Verb = "open";
                proc.StartInfo.FileName = url;
                proc.Start();
            }
        }
    }
}
