using FreedomManager.Mod.Json;
using SharpCompress.Archives;
using SharpCompress.Archives.SevenZip;
using SharpCompress.Common;
using SharpCompress.Readers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;

namespace FreedomManager.Mod
{

    public enum ArchiveType
    {
        BepinDir,
        PluginDir,
        MelonDir,
        DllDir,
        WorkshopDir,
        None
    }

    public class ModHandler
    {

        public List<ModInfo> modList { get; }


        public ModHandler()
        {
            modList = new List<ModInfo>();
            UpdateModList();
        }

        public void UpdateModList()
        {
            DeduplicateMods();
            modList.Clear();
            if (FreedomManager.loaderHandler.bepinInstalled)
            {
                modList.AddRange(BepinScan("BepInEx\\plugins", ModType.BEPINMOD));
                modList.AddRange(BepinScan("BepInEx\\patchers", ModType.BEPINPATCHDIR));
            }

            if (FreedomManager.loaderHandler.melonInstalled)
            {
                modList.AddRange(MelonScan());
            }
            if (FreedomManager.loaderHandler.fp2libInstalled && FreedomManager.loaderHandler.bepinInstalled)
            {
                modList.AddRange(NPCScan());
            }
        }

        private ArchiveType CheckArchive(string path)
        {
            //GameBanana supports only 7z, zip, and rar files. With fp2mm being GB based mod manager, these are the only supported formats.
            //Additionaly, forcing usage of common compression methods makes it easier for end-users to manually install mods.
            if (File.Exists(path) && (Path.GetExtension(path) == ".zip" || Path.GetExtension(path) == ".rar"))
            {
                using (Stream stream = File.OpenRead(path))
                using (var reader = ReaderFactory.Open(stream))
                {
                    while (reader.MoveToNextEntry())
                    {
                        Console.WriteLine(reader.Entry.Key.ToLower());
                        if (reader.Entry.Key.ToLower().StartsWith("bepinex"))
                        {
                            return ArchiveType.BepinDir;
                        }
                        if (reader.Entry.Key.ToLower().StartsWith("plugins"))
                        {
                            return ArchiveType.PluginDir;
                        }
                        if (reader.Entry.Key.ToLower().StartsWith("mods"))
                        {
                            return ArchiveType.MelonDir;
                        }
                    }
                }
            }
            else if (File.Exists(path) && Path.GetExtension(path) == ".7z")
            {
                //SharpCompress doesn't handle 7z in it's default ReaderFactory
                using (Stream stream = File.OpenRead(path))
                using (var reader = SevenZipArchive.Open(stream))
                {
                    foreach (SevenZipArchiveEntry entry in reader.Entries)
                    {
                        Console.WriteLine(entry.Key.ToLower());
                        if (entry.Key.ToLower().StartsWith("bepinex"))
                        {
                            return ArchiveType.BepinDir;
                        }
                        if (entry.Key.ToLower().StartsWith("plugins"))
                        {
                            return ArchiveType.PluginDir;
                        }
                        if (entry.Key.ToLower().StartsWith("mods"))
                        {
                            return ArchiveType.MelonDir;
                        }
                    }
                }
            }
            return ArchiveType.None;
        }

        public bool EnableDisableMod(ModInfo info)
        {
            //There's no obvious easier or faster solution for this case. Zipping the mod was considered, but we would then add extra overhead when reading it's data.
            //Mods are just a single .dll and .json, so moving them around is fast and painless. AssetBundles in /mods, /mod_overrides, etc. can stay
            try
            {
                Directory.CreateDirectory("BepInEx\\plugins-disabled");
                Directory.CreateDirectory("BepInEx\\patchers-disabled");
                Directory.CreateDirectory("MLLoader\\Mods-disabled");

                string sourceDir;
                string destDir;

                if (info.Enabled)
                {
                    switch (info.Type)
                    {
                        case ModType.BEPINMOD:
                        case ModType.BEPINDLL:
                            sourceDir = "BepInEx\\plugins\\";
                            destDir = "BepInEx\\plugins-disabled\\";
                            break;
                        case ModType.MELONMOD:
                            sourceDir = "MLLoader\\Mods\\";
                            destDir = "MLLoader\\Mods-disabled\\";
                            break;
                        case ModType.BEPINPATCHDLL:
                        case ModType.BEPINPATCHDIR:
                            sourceDir = "BepInEx\\patchers\\";
                            destDir = "BepInEx\\patchers-disabled\\";
                            break;
                        default: return info.Enabled;
                    }
                }
                else
                {
                    switch (info.Type)
                    {
                        case ModType.BEPINMOD:
                        case ModType.BEPINDLL:
                            sourceDir = "BepInEx\\plugins-disabled\\";
                            destDir = "BepInEx\\plugins\\";
                            break;
                        case ModType.MELONMOD:
                            sourceDir = "MLLoader\\Mods-disabled\\";
                            destDir = "MLLoader\\Mods\\";
                            break;
                        case ModType.BEPINPATCHDLL:
                        case ModType.BEPINPATCHDIR:
                            sourceDir = "BepInEx\\patchers-disabled\\";
                            destDir = "BepInEx\\patchers\\";
                            break;
                        default: return info.Enabled;
                    }
                }
                switch (info.Type)
                {
                    case ModType.BEPINMOD:
                    case ModType.BEPINPATCHDIR:
                        Directory.Move(sourceDir + info.Dirname, destDir + info.Dirname);
                        return !info.Enabled;
                    case ModType.BEPINDLL:
                    case ModType.BEPINPATCHDLL:
                    case ModType.MELONMOD:
                        File.Move(sourceDir + info.Dirname + ".dll", destDir + info.Dirname + ".dll");
                        return !info.Enabled;
                }
                return info.Enabled;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return info.Enabled;
            }
        }

        public bool InstallMod(string filename, bool delete)
        {
            ArchiveType archiveType = CheckArchive(filename);
            if (archiveType != ArchiveType.None)
            {
                ExtractMod(filename, archiveType);
                if (delete) File.Delete(filename);
                return true;
            }
            return false;
        }

        public void UnInstallMod(ModInfo modInfo)
        {
            //TODO: Check if we can instead blindly delete both enabled and disabled dirs, in case deduplicator has missed something
            try
            {
                DialogResult dialogResult = MessageBox.Show(FreedomManager.ActiveForm, "Do you want to remove \"" + modInfo.Name + "\"?", "Mod uninstall", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    if (modInfo.Type == ModType.BEPINMOD) //Bepin mod
                    {
                        if (modInfo.Enabled)
                            Directory.Delete("BepInEx\\plugins\\" + modInfo.Dirname, true);
                        else
                            Directory.Delete("BepInEx\\plugins-disabled\\" + modInfo.Dirname, true);
                    }
                    else if (modInfo.Type == ModType.BEPINDLL) //Loose DLL bepin
                    {
                        if (modInfo.Enabled)
                            File.Delete("BepInEx\\plugins\\" + modInfo.Dirname + ".dll");
                        else
                            File.Delete("BepInEx\\plugins-disabled\\" + modInfo.Dirname + ".dll");
                    }
                    if (modInfo.Type == ModType.BEPINPATCHDIR) //Bepin patcher
                    {
                        if (modInfo.Enabled)
                            Directory.Delete("BepInEx\\patchers\\" + modInfo.Dirname, true);
                        else
                            Directory.Delete("BepInEx\\patchers-disabled\\" + modInfo.Dirname, true);
                    }
                    else if (modInfo.Type == ModType.BEPINPATCHDLL) //Loose DLL patch
                    {
                        if (modInfo.Enabled)
                            File.Delete("BepInEx\\patchers\\" + modInfo.Dirname + ".dll");
                        else
                            File.Delete("BepInEx\\patchers-disabled\\" + modInfo.Dirname + ".dll");
                    }
                    if (modInfo.Type == ModType.MELONMOD) //Melon mod
                    {
                        if (modInfo.Enabled)
                            File.Delete("MLLoader\\mods\\" + modInfo.Dirname + ".dll");
                        else
                            File.Delete("MLLoader\\mods-disabled\\" + modInfo.Dirname + ".dll");
                    }
                    if (modInfo.Type == ModType.JSONNPC)
                    {
                        File.Delete("BepInEx\\config\\NPCLibEzNPC\\" + modInfo.Dirname);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void ExtractMod(string file, ArchiveType type)
        {
            string path = ".";
            switch (type)
            {
                //Path magic so files go where they should
                case ArchiveType.MelonDir:
                    path = "MLLoader";
                    break;
                case ArchiveType.PluginDir:
                    path = "BepInEX";
                    break;
            }
            if (Path.GetExtension(file) != ".7z")
            {
                //All archives that aren't 7z can be handled like this. Due to GameBanana limitations (and program being Win specific) we can skip on extras like gzip compressed files.
                using (Stream stream = File.OpenRead(file))
                using (var reader = ReaderFactory.Open(stream))
                {
                    while (reader.MoveToNextEntry())
                    {
                        if (!reader.Entry.IsDirectory && reader.Entry.Key != "FP2.exe")
                        {
                            Console.WriteLine(reader.Entry.Key);

                            //TODO: Regex this \/, mods with modinfo should get a file index for better uninstall (to nuke extra assets and such..)
                            //BepInEx/plugins/Modname/modinfo.json
                            //Write to mod's dir with filename files.index, 

                            reader.WriteEntryToDirectory(path, new ExtractionOptions()
                            {
                                ExtractFullPath = true,
                                Overwrite = true
                            });
                        }
                    }
                }
            }
            else
            {
                //SharpCompress doesn't handle 7z in it's default ReaderFactory and it requires this separate construct
                using (Stream stream = File.OpenRead(file))
                using (var reader = SevenZipArchive.Open(stream))
                    foreach (SevenZipArchiveEntry entry in reader.Entries)
                    {
                        if (!entry.IsDirectory && entry.Key != "FP2.exe")
                        {
                            Console.WriteLine(entry.Key);
                            entry.WriteToDirectory(path, new ExtractionOptions()
                            {
                                ExtractFullPath = true,
                                Overwrite = true
                            });
                        }
                    }
            }
        }

        private void DeduplicateMods()
        {

            string dirEnabled = "BepInEx\\plugins";
            string dirDisabled = "BepInEx\\plugins-disabled";
            string dirEnabledM = "MLLoader\\Mods";
            string dirDisabledM = "MLLoader\\Mods-disabled";

            //Horrible hack. Ensures that any inconsistent half-disabled mods are cleaned up if we crash. With no dupes overhead is minimal even with 100s of mods.

            try
            {
                foreach (string f in Directory.GetFiles(dirEnabled))
                {

                    if (Path.GetExtension(f) == ".dll")
                    { //Loose .dll mods wont have any extra files, can nuke blindly.
                        string modname = Path.GetFileName(f);
                        if (File.Exists(dirDisabled + "\\" + modname))
                            File.Delete(dirDisabled + "\\" + modname);
                    }
                }
                foreach (string f in Directory.GetFiles(dirEnabledM))
                {

                    if (Path.GetExtension(f) == ".dll")
                    { //Loose .dll mods wont have any extra files, can nuke blindly. Melon edition.
                        string modname = Path.GetFileName(f);
                        if (File.Exists(dirDisabledM + "\\" + modname))
                            File.Delete(dirDisabledM + "\\" + modname);
                    }
                }
                foreach (string d in Directory.GetDirectories(dirEnabled))
                {
                    string modname = Path.GetFileName(d);
                    if (Directory.Exists(dirDisabled + "\\" + modname))
                    {
                        foreach (string js in Directory.GetFiles(dirDisabled + "\\" + modname))
                        {
                            try
                            {
                                //Try to copy any extra files from disabled mod. By specification none should have things there, but we should try anyways.
                                //*Please don't keep random stuff in your plugin folder, other places exist just for that*
                                File.Copy(js, dirDisabled + "\\" + modname + "\\" + Path.GetFileName(js), false);
                            }
                            catch (Exception ex)
                            { //It should fail on files that alredy exist. Fail is quiet since we assume files in enabled copy of the mod are the 'good' ones. Also, look above.
                                Console.WriteLine(ex.Message);
                            }
                            File.Delete(js);
                        }
                        Directory.Delete(dirDisabled + "\\" + modname);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private List<ModInfo> BepinScan(string dir, ModType type)
        {
            ModType dllType = ModType.BEPINDLL;
            ModType dirType = ModType.BEPINMOD;

            if (type == ModType.BEPINPATCHDIR)
            {
                dllType = ModType.BEPINPATCHDLL;
                dirType = ModType.BEPINPATCHDIR;
            }

            //Enabled mods
            List<ModInfo> list = new List<ModInfo>();
            bool hasManifest;
            try
            {
                foreach (string f in Directory.GetFiles(dir))
                {

                    if (Path.GetExtension(f) == ".dll")
                    {
                        string modname = Path.GetFileNameWithoutExtension(f);
                        if (modname != "BepInEx.MultiFolderLoader")
                            list.Add(new ModInfo(modname, dllType));
                    }
                }
                foreach (string d in Directory.GetDirectories(dir))
                {
                    string modname = Path.GetFileName(d);
                    hasManifest = false;
                    //FP2Lib and MelonLoader should not be listed. 
                    if (modname != "BepInEx.MelonLoader.Loader" && modname != "lib" && modname != "BepInEx.MultiFolderLoader" && modname != "BepInEx.SplashScreen")
                    {
                        foreach (string js in Directory.GetFiles(d))
                        {
                            if (Path.GetFileName(js) == "modinfo.json")
                            {
                                try
                                {
                                    ModInfo info = JsonSerializer.Deserialize<ModInfo>(File.ReadAllText(js));
                                    info.Dirname = modname;
                                    list.Add(info);
                                    hasManifest = true;
                                }
                                catch (Exception ex) { Console.WriteLine(ex); }
                            }
                        }
                        if (!hasManifest)
                        {
                            list.Add(new ModInfo(modname, dirType));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            //Disabled Mods
            dir += "-disabled";
            try
            {
                foreach (string f in Directory.GetFiles(dir))
                {
                    if (Path.GetExtension(f) == ".dll")
                    {
                        string modname = Path.GetFileNameWithoutExtension(f);
                        ModInfo info = new ModInfo(modname, dllType)
                        {
                            Enabled = false
                        };
                        list.Add(info);
                    }
                }
                foreach (string d in Directory.GetDirectories(dir))
                {
                    string modname = Path.GetFileName(d);
                    hasManifest = false;
                    //If this triggers, how did you manage to disable a hidden mod lol. Stays here just in case, the deduplicator will handle it on next run when ML is installed again.
                    if (modname != "BepInEx.MelonLoader.Loader" && modname != "lib" && modname != "BepInEx.MultiFolderLoader" && modname != "BepInEx.SplashScreen")
                    {
                        foreach (string js in Directory.GetFiles(d))
                        {
                            if (Path.GetFileName(js) == "modinfo.json")
                            {
                                try
                                {
                                    ModInfo info = JsonSerializer.Deserialize<ModInfo>(File.ReadAllText(js));
                                    info.Dirname = modname;
                                    info.Enabled = false;
                                    list.Add(info);
                                    hasManifest = true;
                                }
                                catch (Exception ex) { Console.WriteLine(ex); }
                            }
                        }
                        if (!hasManifest)
                        {
                            ModInfo info = new ModInfo(modname, dirType)
                            {
                                Enabled = false
                            };
                            list.Add(info);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


            return list;
        }

        public List<ModInfo> MelonScan()
        {
            //Separate, since we usually don't have melon mods around. No need to scan empty places.

            string dir = "MLLoader\\Mods";
            List<ModInfo> list = new List<ModInfo>();
            try
            {
                foreach (string f in Directory.GetFiles(dir))
                {

                    if (Path.GetExtension(f) == ".dll")
                    {
                        string modname = Path.GetFileNameWithoutExtension(f);
                        list.Add(new ModInfo(modname, ModType.MELONMOD));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            dir = "MLLoader\\Mods-disabled";
            try
            {
                foreach (string f in Directory.GetFiles(dir))
                {

                    if (Path.GetExtension(f) == ".dll")
                    {
                        string modname = Path.GetFileNameWithoutExtension(f);
                        ModInfo info = new ModInfo(modname, ModType.MELONMOD)
                        {
                            Enabled = false
                        };
                        list.Add(info);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return list;
        }

        public List<ModInfo> NPCScan()
        {
            string dir = "BepInEx\\config\\NPCLibEzNPC";
            List<ModInfo> list = new List<ModInfo>();
            try
            {
                if (Directory.Exists(dir))
                {
                    foreach (string f in Directory.GetFiles(dir))
                    {
                        if (Path.GetExtension(f) == ".json")
                        {
                            JsonNPC npc = JsonSerializer.Deserialize<JsonNPC>(File.ReadAllText(f));
                            ModInfo info = new ModInfo("NPC: " + npc.name, npc.author, ModType.JSONNPC);
                            info.Dirname = Path.GetFileName(f);
                            list.Add(info);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return list;
        }
    }
}
