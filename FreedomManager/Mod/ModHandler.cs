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
    public class ModHandler
    {
        public enum ArchiveType
        {
            BepinDir,
            PluginDir,
            MelonDir,
            DllDir,
            None
        }

        //private readonly string dirEnabled = "BepInEx\\plugins";
        //private readonly string dirDisabled = "BepInEx\\plugins-disabled";
        //private readonly string dirEnabledM = "MLLoader\\Mods";
        //private readonly string dirDisabledM = "MLLoader\\Mods-disabled";


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
                modList.AddRange(DirectoryScan());
            }

            if (FreedomManager.loaderHandler.melonInstalled)
            {
                modList.AddRange(MelonScan());
            }
        }

        private ArchiveType CheckArchive(String path)
        {
            if (File.Exists(path) && (Path.GetExtension(path) == ".zip" || Path.GetExtension(path) == ".rar"))
            {
                using (Stream stream = File.OpenRead(path))
                using (var reader = ReaderFactory.Open(stream))
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
            else if (File.Exists(path) && Path.GetExtension(path) == ".7z")
            {
                using (Stream stream = File.OpenRead(path))
                using (var reader = SevenZipArchive.Open(stream))
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
            return ArchiveType.None;
        }

        public bool EnableDisableMod(ModInfo info)
        {
            try
            {
                Directory.CreateDirectory("BepInEx\\plugins-disabled");
                Directory.CreateDirectory("MLLoader\\Mods-disabled");

                string sourceDir;
                string destDir;
                if (info.Enabled)
                {
                    if (info.ArchiveType != ArchiveType.MelonDir)
                    {
                        sourceDir = "BepInEx\\plugins\\";
                        destDir = "BepInEx\\plugins-disabled\\";
                    }
                    else
                    {
                        sourceDir = "MLLoader\\Mods\\";
                        destDir = "MLLoader\\Mods-disabled\\";
                    }
                }
                else
                {
                    if (info.ArchiveType != ArchiveType.MelonDir)
                    {
                        sourceDir = "BepInEx\\plugins-disabled\\";
                        destDir = "BepInEx\\plugins\\";
                    }
                    else
                    {
                        sourceDir = "MLLoader\\Mods-disabled\\";
                        destDir = "MLLoader\\Mods\\";
                    }
                }
                switch (info.ArchiveType)
                {
                    case ArchiveType.BepinDir:
                    case ArchiveType.PluginDir:
                        Directory.Move(sourceDir + info.Dirname, destDir + info.Dirname);
                        return !info.Enabled;
                    case ArchiveType.DllDir:
                    case ArchiveType.MelonDir:
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
            try
            {
                DialogResult dialogResult = MessageBox.Show(FreedomManager.ActiveForm, "Do you want to remove \"" + modInfo.Name + "\"?", "Mod uninstall", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    if (modInfo.ArchiveType == ArchiveType.BepinDir || modInfo.ArchiveType == ArchiveType.PluginDir) //Bepin mod
                    {
                        if (modInfo.Enabled)
                            Directory.Delete("BepInEx\\plugins\\" + modInfo.Dirname, true);
                        else
                            Directory.Delete("BepInEx\\plugins-disabled\\" + modInfo.Dirname, true);
                    }
                    else if (modInfo.ArchiveType == ArchiveType.DllDir) //Loose DLL bepin
                    {
                        if (modInfo.Enabled)
                            File.Delete("BepInEx\\plugins\\" + modInfo.Dirname + ".dll");
                        else
                            File.Delete("BepInEx\\plugins-disabled\\" + modInfo.Dirname + ".dll");
                    }
                    if (modInfo.ArchiveType == ArchiveType.MelonDir) //Melon mod
                    {
                        if (modInfo.Enabled)
                            File.Delete("MLLoader\\mods\\" + modInfo.Dirname + ".dll");
                        else
                            File.Delete("MLLoader\\mods-disabled\\" + modInfo.Dirname + ".dll");
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
                case ArchiveType.MelonDir:
                    path = "MLLoader";
                    break;
                case ArchiveType.PluginDir:
                    path = "BepInEX";
                    break;
            }
            if (Path.GetExtension(file) != ".7z")
            {
                using (Stream stream = File.OpenRead(file))
                using (var reader = ReaderFactory.Open(stream))
                {
                    while (reader.MoveToNextEntry())
                    {
                        if (!reader.Entry.IsDirectory && reader.Entry.Key != "FP2.exe")
                        {
                            Console.WriteLine(reader.Entry.Key);
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
                            { //Try to copy any extra files from disabled mod. By specification none should have things there, but we should try anyways.
                                File.Copy(js, dirDisabled + "\\" + modname + "\\" + Path.GetFileName(js), false);
                            }
                            catch (Exception ex)
                            { //It should fail on files that alredy exist.
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

        private List<ModInfo> DirectoryScan()
        {
            //Enabled mods
            string dir = "BepInEx\\plugins";
            List<ModInfo> list = new List<ModInfo>();
            bool hasManifest;
            try
            {
                foreach (string f in Directory.GetFiles(dir))
                {

                    if (Path.GetExtension(f) == ".dll")
                    {
                        string modname = Path.GetFileNameWithoutExtension(f);
                        list.Add(new ModInfo(modname, ArchiveType.DllDir));
                    }
                }
                foreach (string d in Directory.GetDirectories(dir))
                {
                    string modname = Path.GetFileName(d);
                    hasManifest = false;
                    if (modname != "BepInEx.MelonLoader.Loader" && modname != "lib")
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
                            list.Add(new ModInfo(modname, ArchiveType.BepinDir));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            //Disabled Mods
            dir = "BepInEx\\plugins-disabled";
            try
            {
                foreach (string f in Directory.GetFiles(dir))
                {
                    if (Path.GetExtension(f) == ".dll")
                    {
                        string modname = Path.GetFileNameWithoutExtension(f);
                        ModInfo info = new ModInfo(modname, ArchiveType.DllDir)
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
                    if (modname != "BepInEx.MelonLoader.Loader")
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
                            ModInfo info = new ModInfo(modname, ArchiveType.BepinDir)
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
            string dir = "MLLoader\\Mods";
            List<ModInfo> list = new List<ModInfo>();
            try
            {
                foreach (string f in Directory.GetFiles(dir))
                {

                    if (Path.GetExtension(f) == ".dll")
                    {
                        string modname = Path.GetFileNameWithoutExtension(f);
                        list.Add(new ModInfo(modname, ArchiveType.MelonDir));
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
                        ModInfo info = new ModInfo(modname, ArchiveType.MelonDir)
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
    }
}
