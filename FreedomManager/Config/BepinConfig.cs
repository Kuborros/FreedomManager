using IniParser.Model;
using IniParser.Parser;
using System;
using System.IO;

namespace FreedomManager
{
    internal class BepinConfig
    {
        public readonly bool confExists = true;
        //[Logging]
        public bool UnityLogListening = true, LogConsoleToUnityLog = false;
        //[Logging.Console]
        public bool ShowConsole = false, ConsolePreventClose = false;
        public string LogLevels = "Fatal, Error, Warning, Message, Info";
        //[Logging.Disk]
        public bool WriteUnityLog = true, AppendLog = false, FileLog = true;
        //[Harmony.Logger]
        public string HarmonyLogLevels = "Warn, Error";
        //[SplashScreen]
        public bool SplashEnabled = true, OnlyNoConsole = true;

        public BepinConfig()
        {

            //If there is no file, we default to nothing
            //This will gray out all the bepin options in the manager
            if (File.Exists("BepInEx\\config\\BepInEx.cfg"))
            {
                try
                {
                    var parser = new IniDataParser();
                    parser.Configuration.CommentString = "#";
                    IniData data = parser.Parse(File.ReadAllText("BepInEx\\config\\BepInEx.cfg"));

                    UnityLogListening = bool.Parse(data["Logging"]["UnityLogListening"]);
                    LogConsoleToUnityLog = bool.Parse(data["Logging"]["LogConsoleToUnityLog"]);

                    ShowConsole = bool.Parse(data["Logging.Console"]["Enabled"]);
                    ConsolePreventClose = bool.Parse(data["Logging.Console"]["PreventClose"]);
                    LogLevels = data["Logging.Console"]["LogLevels"];

                    WriteUnityLog = bool.Parse(data["Logging.Disk"]["WriteUnityLog"]);
                    AppendLog = bool.Parse(data["Logging.Disk"]["AppendLog"]);
                    FileLog = bool.Parse(data["Logging.Disk"]["Enabled"]);

                    HarmonyLogLevels = data["Harmony.Logger"]["LogChannels"];

                    SplashEnabled = bool.Parse(data["SplashScreen"]["Enabled"]);
                    OnlyNoConsole = bool.Parse(data["SplashScreen"]["OnlyNoConsole"]);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            else confExists = false;
        }

        public void writeConfig()
        {
            try
            {
                var parser = new IniDataParser();
                parser.Configuration.CommentString = "#";
                IniData data = parser.Parse(File.ReadAllText("BepInEx\\config\\BepInEx.cfg"));

                data["Logging"]["UnityLogListening"] = UnityLogListening.ToString();
                data["Logging"]["LogConsoleToUnityLog"] = LogConsoleToUnityLog.ToString();

                data["Logging.Console"]["Enabled"] = ShowConsole.ToString();
                data["Logging.Console"]["PreventClose"] = ConsolePreventClose.ToString();
                data["Logging.Console"]["LogLevels"] = LogLevels;

                data["Logging.Disk"]["WriteUnityLog"] = WriteUnityLog.ToString();
                data["Logging.Disk"]["AppendLog"] = AppendLog.ToString();
                data["Logging.Disk"]["Enabled"] = FileLog.ToString();

                data["Harmony.Logger"]["LogChannels"] = HarmonyLogLevels;

                data["SplashScreen"]["Enabled"] = SplashEnabled.ToString();
                data["SplashScreen"]["OnlyNoConsole"] = OnlyNoConsole.ToString();

                //We can technically construct the file ourselves, and BepInEx will accept it.
                //If somehow you delete the file after we loaded it, this will ensure we still save it and not crash horribly.
                if (!File.Exists("BepInEx\\config\\BepInEx.cfg"))
                {
                    Directory.CreateDirectory("BepInEx\\config");
                }
                File.WriteAllText("BepInEx\\config\\BepInEx.cfg", data.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
