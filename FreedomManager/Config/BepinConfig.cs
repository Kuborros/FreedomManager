using IniParser.Model;
using IniParser.Parser;
using System;
using System.IO;

namespace FreedomManager
{
    internal class BepinConfig
    {
        public bool confExists = true;
        //[Logging]
        public bool UnityLogListening = true, LogConsoleToUnityLog = false;
        //[Logging.Console]
        public bool ShowConsole = false, ConsolePreventClose = false;
        public string LogLevels = "Fatal, Error, Warning, Message";
        //[Logging.Disk]
        public bool WriteUnityLog = true,AppendLog = false,FileLog = true;

        public BepinConfig() {

            //If there is no file, we default to nothing
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
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            confExists = false;
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


                if (!File.Exists("BepInEx\\config\\BepInEx.cfg"))
                {
                    Directory.CreateDirectory("BepInEx\\config");
                }
                File.WriteAllText("BepInEx\\config\\BepInEx.cfg", data.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
