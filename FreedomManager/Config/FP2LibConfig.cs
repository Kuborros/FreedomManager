using IniParser.Model;
using IniParser.Parser;
using System;
using System.IO;

namespace FreedomManager.Config
{
    internal class FP2LibConfig
    {
        private const string confPath = "BepInEx\\config\\000.kuborro.libraries.fp2.fp2lib.cfg";

        public bool configExists = false;
        public bool saveRedirectEnabled = false;
        public bool saveFancyJson = false;
        public int saveRedirectProfile = 0;

        public FP2LibConfig() 
        {
            configExists = File.Exists(confPath);

            if (configExists)
            {
                try
                {
                    var parser = new IniDataParser();
                    parser.Configuration.CommentString = "#";
                    IniData data = parser.Parse(File.ReadAllText(confPath));

                    saveRedirectEnabled = bool.Parse(data["Save Redirection"]["Enabled"]);
                    saveFancyJson = bool.Parse(data["Save Redirection"]["Fancy Json"]);
                    saveRedirectProfile = int.Parse(data["Save Redirection"]["Profile"]);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        public void writeConfig()
        {
            if (configExists)
            {
                try
                {
                    var parser = new IniDataParser();
                    parser.Configuration.CommentString = "#";
                    IniData data = parser.Parse(File.ReadAllText(confPath));

                    data["Save Redirection"]["Enabled"] = saveRedirectEnabled.ToString();
                    data["Save Redirection"]["Fancy Json"] = saveFancyJson.ToString();
                    data["Save Redirection"]["Profile"] = saveRedirectProfile.ToString();

                    File.WriteAllText(confPath, data.ToString());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }
    }
}
