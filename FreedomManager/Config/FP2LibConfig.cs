using System.IO;

namespace FreedomManager.Config
{
    internal class FP2LibConfig
    {
        public bool configExists = false;
        public bool saveRedirectEnabled = false;
        public int saveRedirectProfile = 0;

        public FP2LibConfig() 
        {
            configExists = File.Exists("BepInEx\\config\\000.kuborro.fp2.lib.fp2lib.cfg");


        }
    }
}
