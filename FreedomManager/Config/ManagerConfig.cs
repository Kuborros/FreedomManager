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
        public bool autoUpdateManager { get; set; } = true;
        public bool autoUpdateFP2Lib { get; set; } = true;
        public bool autoUpdateMods { get; set; } = true;
        public bool autoUpdateBepin { get; set; } = true;
        public bool enableLaunchParams { get; set; }
        public string launchParams { get; set; }
        public bool forceNonSteam {  get; set; }
        //Internal version info data
        public string bepinexVersion { get; set; }

        public ManagerConfig()
        {
            configExists = File.Exists(confFile);
            readConfig();
        }

        [JsonConstructor]
        public ManagerConfig(bool autoUpdateManager, bool autoUpdateFP2Lib, bool autoUpdateMods, bool autoUpdateBepin, bool enableLaunchParams, string launchParams, bool forceNonSteam,string bepinexVersion)
        {
            this.autoUpdateFP2Lib = autoUpdateFP2Lib;
            this.autoUpdateManager = autoUpdateManager;
            this.autoUpdateMods = autoUpdateMods;
            this.autoUpdateBepin = autoUpdateBepin;
            this.enableLaunchParams = enableLaunchParams;
            this.launchParams = launchParams;
            this.forceNonSteam = forceNonSteam;
            this.bepinexVersion = bepinexVersion;
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
                autoUpdateBepin = conf.autoUpdateBepin;
                enableLaunchParams = conf.enableLaunchParams;
                launchParams = conf.launchParams;
                forceNonSteam = conf.forceNonSteam;
                bepinexVersion = conf.bepinexVersion;
            }
            else
            {
                //Default auto-updates to ON
                autoUpdateManager = true;
                autoUpdateFP2Lib = true;
                autoUpdateMods = true;
                autoUpdateBepin = true;
                enableLaunchParams = false;
                launchParams = "";
                forceNonSteam = false;
                bepinexVersion = "5.4.22.0";
                writeConfig();
            }

        }
    }
}
