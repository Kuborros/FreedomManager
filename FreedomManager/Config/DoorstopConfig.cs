using IniParser.Model;
using IniParser.Parser;
using System;
using System.IO;

namespace FreedomManager.Config
{
    internal class DoorstopConfig
    {
        public static bool confWritten = true;
        //[UnityDoorstop]
        public bool DoorstopEnabled = true, RedirectOutputLog = false, EnableDebugger = false;
        public string DllSearchPathOverride = "";
        /*
        //[MultiFolderLoader]
        public string ModsbaseDir = "";
        public string DisabledModsListPath = "";
        private readonly bool EnableAdditionalDirectories = true;
        //[MultiFolderLoader_Workshop]
        public string WorkshopModsBaseDir = "";
        public string WorkshopDisabledModsListPath = "";
        */

        public DoorstopConfig()
        {

            //If there is no file, bepinex install is not present or broken.
            if (File.Exists("doorstop_config.ini"))
            {
                try
                {
                    if (!File.Exists("disabled_mods.list"))
                        File.Create("disabled_mods.list").Close();

                    Directory.CreateDirectory("Mods");
                    
                    var parser = new IniDataParser();
                    string basedir = Path.GetFullPath(".");
                    parser.Configuration.CommentString = "#";
                    IniData data = parser.Parse(File.ReadAllText("doorstop_config.ini"));

                    DoorstopEnabled = bool.Parse(data["General"]["enabled"]);
                    RedirectOutputLog = bool.Parse(data["General"]["redirect_output_log"]);
                    DllSearchPathOverride = data["UnityMono"]["dll_search_path_override"];
                    EnableDebugger = bool.Parse(data["UnityMono"]["debug_enabled"]);

                    //Used for multi folder loader. Currently unused due to steam workshop plans being abandoned by GT.
                    /*
                    ModsbaseDir = basedir + "\\Mods";
                    DisabledModsListPath = "disabled_mods.list";

                    if (FreedomManager.loaderHandler.runningUnderSteam)
                    {
                        WorkshopModsBaseDir = basedir.Replace("common\\Freedom Planet 2", "workshop\\content\\595500");
                        WorkshopDisabledModsListPath = "disabled_mods.list";
                    }
                    */
                    
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

                data["General"]["enabled"] = DoorstopEnabled.ToString();
                data["General"]["redirect_output_log"] = RedirectOutputLog.ToString();
                data["UnityMono"]["dll_search_path_override"] = DllSearchPathOverride;
                data["UnityMono"]["debug_enabled"] = EnableDebugger.ToString();

                /*
                data["MultiFolderLoader"]["baseDir"] = ModsbaseDir;
                data["MultiFolderLoader"]["disabledModsListPath"] = DisabledModsListPath;
                data["MultiFolderLoader"]["enableAdditionalDirectories"] = EnableAdditionalDirectories.ToString();

                if (FreedomManager.loaderHandler.runningUnderSteam)
                {
                    data["MultiFolderLoader_Workshop"]["baseDir"] = WorkshopModsBaseDir;
                    data["MultiFolderLoader_Workshop"]["disabledModsListPath"] = WorkshopDisabledModsListPath;
                }
                */
                File.WriteAllText("doorstop_config.ini", data.ToString());

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
