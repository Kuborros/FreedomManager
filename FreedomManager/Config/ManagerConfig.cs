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
        public bool autoUpdateMods { get; set; }
        public bool enableLaunchParams { get; set; }
        public string launchParams { get; set; }
        public bool forceNonSteam {  get; set; }

        public ManagerConfig()
        {
            configExists = File.Exists(confFile);
            readConfig();
        }

        [JsonConstructor]
        public ManagerConfig(bool autoUpdateManager, bool autoUpdateFP2Lib, bool autoUpdateMods, bool enableLaunchParams, string launchParams, bool forceNonSteam)
        {
            this.autoUpdateFP2Lib = autoUpdateFP2Lib;
            this.autoUpdateManager = autoUpdateManager;
            this.autoUpdateMods = autoUpdateMods;
            this.enableLaunchParams = enableLaunchParams;
            this.launchParams = launchParams;
            this.forceNonSteam = forceNonSteam;
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
                autoUpdateMods = conf.autoUpdateMods;
                enableLaunchParams = conf.enableLaunchParams;
                launchParams = conf.launchParams;
                forceNonSteam = conf.forceNonSteam;
            }
            else
            {
                //Default auto-updates to ON
                autoUpdateManager = true;
                autoUpdateFP2Lib = true;
                autoUpdateMods = true;
                enableLaunchParams = false;
                launchParams = "";
                forceNonSteam = false;
                writeConfig();
            }

        }
    }
}
