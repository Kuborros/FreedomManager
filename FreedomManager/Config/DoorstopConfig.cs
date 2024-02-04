using IniParser.Model;
using IniParser.Parser;
using System;
using System.IO;

namespace FreedomManager.Config
{
    internal class DoorstopConfig
    {
        public readonly bool confWritten = true;
        //[UnityDoorstop]
        public bool DoorstopEnabled = true, RedirectOutputLog = false;
        public string DllSearchPathOverride = "";
        //[MultiFolderLoader]
        public string ModsbaseDir = "";
        public string DisabledModsListPath = "";
        private static bool EnableAdditionalDirectories = true;
        //[MultiFolderLoader_Workshop]
        public string WorkshopModsBaseDir = "";
        public string WorkshopDisabledModsListPath = "";


        public DoorstopConfig()
        {

            //If there is no file, bepinex install is not present or broken.
            if (File.Exists("doorstop_config.ini"))
            {
                try
                {
                    if (!File.Exists("disabled_mods.list"))
                        File.Create("disabled_mods.list").Close();

                    var parser = new IniDataParser();
                    string basedir = Path.GetFullPath(".");
                    parser.Configuration.CommentString = "#";
                    IniData data = parser.Parse(File.ReadAllText("doorstop_config.ini"));

                    DoorstopEnabled = bool.Parse(data["UnityDoorstop"]["enabled"]);
                    RedirectOutputLog = bool.Parse(data["UnityDoorstop"]["redirectOutputLog"]);
                    DllSearchPathOverride = data["UnityDoorstop"]["dllSearchPathOverride"];

                    ModsbaseDir = basedir + "\\FPMods";
                    DisabledModsListPath = "disabled_mods.list";

                    EnableAdditionalDirectories = bool.Parse(data["MultiFolderLoader"]["enableAdditionalDirectories"]);

                    if (FreedomManager.loaderHandler.runningUnderSteam)
                    {
                        WorkshopModsBaseDir = basedir.Replace("common\\Freedom Planet 2", "workshop\\content\\595500");
                        WorkshopDisabledModsListPath = "disabled_mods.list";
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    confWritten = false;
                }
            }
        }

        public void writeConfig()
        {
            try
            {
                var parser = new IniDataParser();
                parser.Configuration.CommentString = "#";
                IniData data = parser.Parse(File.ReadAllText("doorstop_config.ini"));

                data["UnityDoorstop"]["enabled"] = DoorstopEnabled.ToString();
                data["UnityDoorstop"]["redirectOutputLog"] = RedirectOutputLog.ToString();
                data["UnityDoorstop"]["dllSearchPathOverride"] = DllSearchPathOverride;

                data["MultiFolderLoader"]["baseDir"] = ModsbaseDir;
                data["MultiFolderLoader"]["disabledModsListPath"] = DisabledModsListPath;
                data["MultiFolderLoader"]["enableAdditionalDirectories"] = EnableAdditionalDirectories.ToString();

                if (FreedomManager.loaderHandler.runningUnderSteam)
                {
                    data["MultiFolderLoader_Workshop"]["baseDir"] = WorkshopModsBaseDir;
                    data["MultiFolderLoader_Workshop"]["disabledModsListPath"] = WorkshopDisabledModsListPath;
                }

                File.WriteAllText("doorstop_config.ini", data.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
