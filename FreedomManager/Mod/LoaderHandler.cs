using System;
using System.IO;
using System.Net;
using System.Text.Json;

namespace FreedomManager.Mod
{
    public class LoaderHandler
    {
        public bool bepinInstalled;
        public bool melonInstalled;
        public bool fp2Found;

        public bool fp2libInstalled;
        public string fp2libVersion;
        private ModInfo fp2libInfo;

        public LoaderHandler()
        {
            fp2Found = File.Exists("FP2.exe");
            bepinInstalled = File.Exists("winhttp.dll");
            melonInstalled = Directory.Exists("BepInEx\\plugins\\BepInEx.MelonLoader.Loader");
            fp2libInstalled = File.Exists("BepInEx\\plugins\\lib\\fp2lib.json");

            if (fp2libInstalled)
            {
                fp2libInfo = JsonSerializer.Deserialize<ModInfo>(File.ReadAllText("BepInEx\\plugins\\lib\\fp2lib.json"));
                fp2libInfo.Dirname = "lib";
                fp2libVersion = fp2libInfo.Version;
            }
            else
            {
                fp2libVersion = "Not Installed";
            }
        }

        internal bool installBepinLoader()
        {
            if (!bepinInstalled)
            {
                using (WebClient client = new WebClient()) {
                    client.DownloadFile(new Uri("https://github.com/BepInEx/BepInEx/releases/download/v5.4.22/BepInEx_x86_5.4.22.0.zip"), "BepInEx.zip");
                    FreedomManager.modHandler.InstallMod("BepInEx.zip", true);
                    bepinInstalled = true;
                }
            }
            else
            {
                File.Delete("winhttp.dll");
                bepinInstalled = false;
            }
            return bepinInstalled;
        }

        internal bool installMLLoader()
        {
            if (!melonInstalled)
            {
                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(new Uri("https://github.com/BepInEx/BepInEx.MelonLoader.Loader/releases/download/v2.0.0/BepInEx.MelonLoader.Loader.UnityMono_BepInEx5_2.0.0.zip"), "Melon.zip");
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
