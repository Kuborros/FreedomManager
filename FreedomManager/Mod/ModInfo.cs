using FreedomManager.Mod;
using System.Text.Json.Serialization;

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
        "GBID":"",
        "GitHub":"https://github.com/Author/Repo"
    }
    */


    public enum ModType
    {
        BEPINMOD,
        BEPINDLL,
        MELONMOD,
        LIBRARY,
        JSONNPC,
        STAGE,
        SPECIAL
    }


    public class ModInfo
    {
        public int ManifestVer { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string Version { get; set; }
        public string Loader { get; set; }
        public bool? HasAssets { get; set; }
        public int? GBID { get; set; }
        public ModType? Type { get; set; }
        public string Dirname { get; set; }
        public bool Enabled { get; set; }
        public bool HasIndex { get; set; }
        public string GitHub { get; set; }

        //V1 Json
        [JsonConstructor]
        public ModInfo(string name, string author, string version, string loader, bool? hasAssets, ModType? type, int? gBID, string gitHub)
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

            if (!type.HasValue || type == null)
            {
                Type = ModType.BEPINMOD;
            }
            else Type = type;

            if (!gBID.HasValue)
            {
                GBID = 0;
            }
            else GBID = gBID;
            GitHub = gitHub;
            Dirname = "invalid-directory-to-be-set";
            Enabled = true;
        }

        public ModInfo(string name, ModType archiveType) : this(name, "N/A", archiveType) { }

        public ModInfo(string name,string author, ModType archiveType)
        {
            Name = SpecialNames(name);
            Author = author;
            Version = "0.0.0";

            if (archiveType == ModType.BEPINMOD) Loader = "BepInEx";
            else if (archiveType == ModType.BEPINDLL) Loader = "BepInEx (DLL)";
            else if (archiveType == ModType.MELONMOD) Loader = "MelonLoader";
            else if (archiveType == ModType.JSONNPC) Loader = "NPC (JSON)";
            else if (archiveType == ModType.STAGE) Loader = "Stage";
            else Loader = "N/A";

            HasAssets = true;
            Type = archiveType;
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
