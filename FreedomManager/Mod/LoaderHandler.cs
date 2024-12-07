using System;
using System.IO;
using System.Net;
using System.Text.Json;

namespace FreedomManager.Mod
{
    public class LoaderHandler
    {
        //These updates can cause breaking changes, so they are verified by hand.
        //TODO: Figure out some nicer way to push this without updating whole mod manager
        public static readonly Uri latestStableBepInEx5 = new Uri("https://api.github.com/repos/BepInEx/BepInEx/releases/153584734");
        public static readonly Uri latestStableBepInEx5File = new Uri("https://github.com/BepInEx/BepInEx/releases/download/v5.4.23.2/BepInEx_win_x86_5.4.23.2.zip");

        public bool bepinInstalled;
        public bool melonInstalled;
        public bool fp2Found;

        public bool fp2libInstalled;
        public string fp2libVersion;
        private ModInfo fp2libInfo;

        public bool bepinUtilsInstalled;
        public string bepinVersion;
        public string doorstopVersion;

        public bool bepinDevtoolsInstalled;
        public string bepinDevtoolsVersion;

        public bool runningUnderSteam;

        private string gamePath;

        public LoaderHandler()
        {
            fp2Found = File.Exists("FP2.exe");
            bepinInstalled = File.Exists("winhttp.dll");
            bepinUtilsInstalled = Directory.Exists("BepInEx\\plugins\\ConfigurationManager");
            bepinDevtoolsInstalled = File.Exists("BepInEx\\patchers\\DemystifyExceptions.dll");
            melonInstalled = Directory.Exists("BepInEx\\plugins\\BepInEx.MelonLoader.Loader");
            fp2libInstalled = File.Exists("BepInEx\\plugins\\lib\\fp2lib.json");

            gamePath = Path.GetFullPath("FP2.exe");
            runningUnderSteam = gamePath.Contains("steamapps");

            if (fp2libInstalled)
            {
                fp2libInfo = JsonSerializer.Deserialize<ModInfo>(File.ReadAllText("BepInEx\\plugins\\lib\\fp2lib.json"));
                fp2libInfo.Dirname = "lib";
                fp2libInfo.Type = ModType.LIBRARY;
                fp2libVersion = fp2libInfo.Version;
            }
            else
            {
                fp2libVersion = "Not Installed";
            }

            if (bepinInstalled)
            {
                if (File.Exists(".doorstop_version"))
                {
                    doorstopVersion = File.ReadAllText(".doorstop_version");
                }
                else
                {
                    //Doorstop version file was added in 5.4.23. If not present, we are on 5.4.22 or lower.
                    bepinVersion = "5.4.22.0";
                    doorstopVersion = "3.4.0";
                }
            }

        }

        internal bool installBepinLoader()
        {
            if (!bepinInstalled)
            {
                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(latestStableBepInEx5File, "BepInEx.zip");
                    FreedomManager.modHandler.InstallMod("BepInEx.zip", true);
                    bepinInstalled = true;
                    bepinVersion = "5.4.23.2";
                    
                }
                installBepinUtils(false);
            }
            else
            {
                File.Delete("winhttp.dll");
                bepinInstalled = false;
            }
            return bepinInstalled;
        }

        internal bool installBepinUtils(bool force)
        {
            if ((!bepinUtilsInstalled && bepinInstalled) || force)
            {
                /*
                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(new Uri("https://github.com/BepInEx/BepInEx.SplashScreen/releases/download/v2.2/BepInEx.SplashScreen_BepInEx5_v2.2.zip"), "BepInExSplash.zip");
                    FreedomManager.modHandler.InstallMod("BepInExSplash.zip", true);
                }
                */
                using (WebClient client = new WebClient())
                {
                    //Delete the old loose version
                    File.Delete("BepInEx\\plugins\\ConfigurationManager.dll");

                    client.DownloadFile(new Uri("https://github.com/BepInEx/BepInEx.ConfigurationManager/releases/download/v18.3/BepInEx.ConfigurationManager.BepInEx5_v18.3.zip"), "BepInExSplash.zip");
                    FreedomManager.modHandler.InstallMod("BepInExSplash.zip", true);

                    ModInfo configMan = new ModInfo("Configuration Manager","ManlyMarco",ModType.BEPINMOD);
                    configMan.Version = "v18.3";
                    configMan.InternalMod = true;
                    string jsonString = JsonSerializer.Serialize(configMan);
                    File.WriteAllText("BepInEx\\plugins\\ConfigurationManager\\modinfo.json", jsonString);
                }
                /*
                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(new Uri("https://github.com/BepInEx/BepInEx.MultiFolderLoader/releases/download/v1.3.1/BepInEx.MultiFolderLoader.dll"), "BepInEx.MultiFolderLoader.dll");
                    if (File.Exists("BepInEx\\patchers\\BepInEx.MultiFolderLoader.dll") && force) File.Delete("BepInEx\\patchers\\BepInEx.MultiFolderLoader.dll");
                    if (!File.Exists("BepInEx\\patchers\\BepInEx.MultiFolderLoader.dll"))
                    {
                        File.Move("BepInEx.MultiFolderLoader.dll", "BepInEx\\patchers\\BepInEx.MultiFolderLoader.dll");
                    }
                    if (File.Exists("BepInEx.MultiFolderLoader.dll")) File.Delete("BepInEx.MultiFolderLoader.dll");
                }
               */
                bepinUtilsInstalled = true;
            }
            return bepinUtilsInstalled;
        }

        internal bool installBepinDevTools()
        {
            if (!bepinDevtoolsInstalled)
            {
                /*
                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(new Uri("https://github.com/BepInEx/BepInEx.Debug/releases/download/r11/StartupProfiler_r11.zip"), "Profiler.zip");
                    FreedomManager.modHandler.InstallMod("Profiler.zip", true);
                }
                */
                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(new Uri("https://github.com/BepInEx/BepInEx.Debug/releases/download/r11/DemystifyExceptions_r11.zip"), "VerboseExceptions.zip");
                    FreedomManager.modHandler.InstallMod("VerboseExceptions.zip", true);
                }
                bepinDevtoolsInstalled = true;
            }
            return bepinDevtoolsInstalled;
        }

        internal bool installMLLoader()
        {
            if (!melonInstalled)
            {
                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(new Uri("https://github.com/BepInEx/BepInEx.MelonLoader.Loader/releases/download/v2.1.0/MLLoader-UnityMono-BepInEx5-v0.5.7.zip"), "Melon.zip");
                    FreedomManager.modHandler.InstallMod("Melon.zip", true);
                    melonInstalled = true;
                }
            }
            else
            {
                Directory.Delete("BepInEx\\plugins\\BepInEx.MelonLoader.Loader", true);
                melonInstalled = false;
            }
            return melonInstalled;
        }

        internal void checkFP2Lib()
        {
            if (File.Exists("BepInEx\\plugins\\lib\\fp2lib.json"))
            {
                fp2libInstalled = true;
                fp2libInfo = JsonSerializer.Deserialize<ModInfo>(File.ReadAllText("BepInEx\\plugins\\lib\\fp2lib.json"));
                fp2libInfo.Dirname = "lib";
                fp2libVersion = fp2libInfo.Version;
            }
            else
            {
                fp2libInstalled = false;
                fp2libVersion = "Not Installed";
            }
        }
    }
}
