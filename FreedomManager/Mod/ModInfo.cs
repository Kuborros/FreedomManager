using FreedomManager.Mod;
using System.Text.Json.Serialization;
using static FreedomManager.Mod.ModHandler;

namespace FreedomManager
{
    /*
    {
	    "ManifestVer":1,
	    "Name":"",
	    "Author":"",
	    "Version":"",
	    "Loader":"",
	    "HasAssets":false,
        "GBID":""
    }
    */

    /*
    {
        "ManifestVer":2,
        "Name":"",
        "Author":"",
        "Version":"",
        "GameBananaID":"",
        "AssetDir":"",
        "NeedFP2Lib":""
        "GitHub":"https://github.com/Author/Repo",
        "IncompatibleMods": []
    }
    */
    public class ModInfo
    {
        public int ManifestVer { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string Version { get; set; }
        public string Loader { get; set; }
        public bool? HasAssets { get; set; }
        public int? GBID { get; set; }
        public ArchiveType? ArchiveType { get; set; }
        public string Dirname { get; set; }
        public bool Enabled { get; set; }
        public bool HasIndex { get; set; }
        public string GitHub { get; set; }


        //V1
        [JsonConstructor]
        public ModInfo(string name, string author, string version, string loader, bool? hasAssets, ArchiveType? archiveType, int? gBID)
        {
            Name = SpecialNames(name);
            Author = author;
            Version = version;

            if (loader.ToLower() == "bepinex" || loader.ToLower() == "bepin") Loader = "BepInEx";
            else if (loader.ToLower() == "melonloader" || loader.ToLower() == "melon") Loader = "MelonLoader";
            else Loader = "Unknown";

            if (!hasAssets.HasValue)
            {
                HasAssets = true;
            }
            else HasAssets = hasAssets;

            if (!archiveType.HasValue || archiveType == null)
            {
                ArchiveType = ModHandler.ArchiveType.BepinDir;
            }
            else ArchiveType = archiveType;

            if (!gBID.HasValue)
            {
                GBID = 0;
            }
            else GBID = gBID;
            Dirname = "invalid-directory-to-be-set";
            Enabled = true;
        }

        public ModInfo(string name, ArchiveType archiveType)
        {
            Name = SpecialNames(name);
            Author = "N/A";
            Version = "N/A";

            if (archiveType == ModHandler.ArchiveType.BepinDir) Loader = "BepInEx";
            else if (archiveType == ModHandler.ArchiveType.DllDir) Loader = "BepInEx (DLL)";
            else if (archiveType == ModHandler.ArchiveType.MelonDir) Loader = "MelonLoader";
            else Loader = "Unknown";

            HasAssets = true;
            ArchiveType = archiveType;
            GBID = 0;
            Dirname = name;
            Enabled = true;
        }

        private string SpecialNames(string name)
        {
            switch (name)
            {
                case "XUnity.ResourceRedirector":
                    return "Resource Redirector";
                case "sinai-dev-UnityExplorer":
                    return "Unity Explorer";
                case "ConfigurationManager":
                    return "Configuration Manager";
                default:
                    return name;
            }
        }
    }
}
