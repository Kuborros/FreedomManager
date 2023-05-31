using System.IO;

namespace FreedomManager.Config
{
    internal class ManagerConfig
    {
        private bool configExists = false;
        public bool autoUpdateManager = false;
        public bool autoUpdateFP2Lib = false;

        public ManagerConfig() 
        {
            configExists = File.Exists("config.json");
        }

        public void writeConfig()
        {

        }
    }
}
