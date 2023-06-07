using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FreedomManager.Config
{

    internal class ManagerConfig
    {
        private const string confFile = "fp2mmConfig.json";
        private bool configExists = false;

        //Json contents
        public bool autoUpdateManager { get; set; }
        public bool autoUpdateFP2Lib { get; set; }

        public ManagerConfig() 
        {
            configExists = File.Exists(confFile);
            readConfig();
        }

        /*
        {
        "autoUpdateManager":true,
        "autoUpdateFP2lib":true
        }
        */

        [JsonConstructor]
        public ManagerConfig(bool autoUpdateManager,bool autoUpdateFP2Lib)
        {
            this.autoUpdateFP2Lib = autoUpdateFP2Lib;
            this.autoUpdateManager = autoUpdateManager;
        }

        public void writeConfig()
        {
            string config = JsonSerializer.Serialize(this);
            File.WriteAllText(confFile, config);
            configExists = true;
        }

        public void readConfig()
        {
            if (configExists)
            {
                ManagerConfig conf = JsonSerializer.Deserialize<ManagerConfig>(File.ReadAllText(confFile));
                autoUpdateFP2Lib = conf.autoUpdateFP2Lib;
                autoUpdateManager = conf.autoUpdateManager;             
            }
            else 
            {
                autoUpdateManager = true;
                autoUpdateFP2Lib = true;
                writeConfig(); 
            }
            
        }
    }
}
