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
            fp2libInstalled = File.Exists("BepInEx\\plugins\\lib\\fp2lib.dll");

            if (fp2libInstalled)
            {
                fp2libInfo = JsonSerializer.Deserialize<ModInfo>(File.ReadAllText("BepInEx\\plugins\\lib\\fp2lib.json"));
                fp2libInfo.Dirname = "lib";
                fp2libVersion = fp2libInfo.Version;
            } else
            {
                fp2libVersion = "Not Installed";
            }
        }

        internal bool installBepinLoader()
        {
            if (!bepinInstalled)
            {
                WebClient client = new WebClient();
                client.DownloadFile(new Uri("https://github.com/BepInEx/BepInEx/releases/download/v5.4.21/BepInEx_x86_5.4.21.0.zip"), "BepInEx.zip");
                FreedomManager.modHandler.InstallMod("BepInEx.zip", true);
                bepinInstalled = true;
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
                WebClient client = new WebClient();
                client.DownloadFile(new Uri("https://github.com/BepInEx/BepInEx.MelonLoader.Loader/releases/download/v2.0.0/BepInEx.MelonLoader.Loader.UnityMono_BepInEx5_2.0.0.zip"), "Melon.zip");
                FreedomManager.modHandler.InstallMod("Melon.zip",true);
                melonInstalled = true;
            }
            else
            {
                Directory.Delete("BepInEx\\plugins\\BepInEx.MelonLoader.Loader", true);
                melonInstalled = false;
            }
            return melonInstalled;
        }

        internal bool installFP2Lib()
        {
            if (!fp2libInstalled)
            {
                WebClient client = new WebClient();
                client.DownloadFile(new Uri("https://fp2mods.info/fp2lib/fp2lib.zip"), "Fp2lib.zip");
                FreedomManager.modHandler.InstallMod("Fp2lib.zip", true);
                fp2libInstalled = true;
                fp2libVersion = "Test";
            }
            else
            {
                File.Delete("BepInEx\\plugins\\libs\\fp2lib.dll");
                fp2libInstalled = false;
                fp2libVersion = "Not Installed";
            }
            return fp2libInstalled;
        }
    }
}
