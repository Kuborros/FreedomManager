﻿using System;
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

    /*
    {
        "ManifestVer":2,
        "Name":"",
        "Author":"",
        "Version":"",
        "Loader":"",
        "GitHub":"https://github.com/Author/Repo"
    }
*/


    public enum ModType
    {
        BEPINMOD,
        BEPINDLL,
        BEPINPATCHDLL,
        BEPINPATCHDIR,
        MELONMOD,
        LIBRARY,
        JSONNPC,
        STAGE,
        SPECIAL
    }

    public enum ModLocation
    {
        WORKSHOP,
        MODSDIR,
        LEGACY
    }
    [Serializable]
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
        public ModLocation Location { get; set; }
        public string Dirname { get; set; }
        public bool Enabled { get; set; }
        public bool HasIndex { get; set; }
        public bool HideInUI { get; set; }
        public string GitHub { get; set; }

        internal bool InternalMod = false;

        //V1 Json
        [JsonConstructor]
        public ModInfo(string name, string author, string version, string loader, bool? hasAssets, ModType? type, int? gBID, string gitHub)
        {
            Name = SpecialNames(name);
            Author = author;
            Version = version;

            if (loader.ToLower() == "bepinex" || loader.ToLower() == "bepin")
            {
                Loader = "BepInEx";
                Location = ModLocation.LEGACY;
            }
            else if (loader.ToLower() == "melonloader" || loader.ToLower() == "melon") Loader = "MelonLoader";
            else Loader = "Unknown";

            if (!hasAssets.HasValue)
            {
                HasAssets = true;
            }
            else HasAssets = hasAssets;

            if (!type.HasValue)
            {
                Type = ModType.BEPINMOD;
            }
            else Type = type;

            if (!gBID.HasValue)
            {
                GBID = 0;
            }
            else GBID = gBID;
            Location = ModLocation.LEGACY;
            GitHub = gitHub;
            Dirname = "invalid-directory-to-be-set";
            Enabled = true;
        }

        public ModInfo(string name, ModType archiveType) : this(name, "N/A", archiveType) { }

        public ModInfo(string name, string author, ModType archiveType)
        {
            Name = SpecialNames(name);
            Author = author;
            Version = "0.0.0";

            if (archiveType == ModType.BEPINMOD) Loader = "BepInEx";
            else if (archiveType == ModType.BEPINDLL) Loader = "BepInEx (DLL)";
            else if (archiveType == ModType.BEPINPATCHDLL) Loader = "BepInEx (Patch)";
            else if (archiveType == ModType.BEPINPATCHDIR) Loader = "BepInEx (Patch)";
            else if (archiveType == ModType.MELONMOD) Loader = "MelonLoader";
            else if (archiveType == ModType.JSONNPC) Loader = "NPC (JSON)";
            else if (archiveType == ModType.STAGE) Loader = "Stage";
            else Loader = "N/A";

            Location = ModLocation.LEGACY;

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
