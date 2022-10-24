using Microsoft.Win32;
using Onova.Services;
using Onova;
using SharpCompress.Archives;
using SharpCompress.Archives.SevenZip;
using SharpCompress.Common;
using SharpCompress.Readers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Windows.Forms;
using File = System.IO.File;
using System.Xml.Linq;

namespace FreedomManager
{
    public partial class FreedomManager : Form
    {
        bool bepisPresent = false;
        bool fp2Found = false;
        bool melonPresent = false;
        bool exists = false;
        string rootDir = "";
        int columnIndex = 0;
        List<ModInfo> mods = new List<ModInfo>();

        private readonly IUpdateManager _updateManager = new UpdateManager(
            new GithubPackageResolver("Kuborros", "FreedomManager", "FreedomManager*.zip "),
            new ZipPackageExtractor());

        public enum ArchiveType
        {
            BepinDir,
            PluginDir,
            MelonDir,
            DllDir,
            None
        }

        public FreedomManager(string[] args)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            DragDrop += new DragEventHandler(FM_DragDrop);
            DragEnter += new DragEventHandler(FM_DragEnter);

            InitializeComponent();
            rootDir = typeof(FreedomManager).Assembly.Location.Replace(Path.GetFileName(System.Reflection.Assembly.GetEntryAssembly().Location), "");
            exists = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly().Location)).Count() > 1;
            Directory.SetCurrentDirectory(rootDir);
            bepisPresent = File.Exists("winhttp.dll");
            fp2Found = File.Exists("FP2.exe");
            melonPresent = Directory.Exists("BepInEx\\plugins\\BepInEx.MelonLoader.Loader");

            if (!fp2Found)
            {
                MessageBox.Show("Freedom Planet 2 not Found!.\n\n" +
                "Please ensure the mod manager is in the main game directory.",
                Text, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                savePlay.Hide();
                setup.Hide();
                handlerButton.Hide();
                melonButton.Hide();
            }

            if (fp2Found && !bepisPresent)
            {
                MessageBox.Show("BepInEx not Found!.\n\n" +
                        "Seems you dont have BepInEx installed - before you install any mods, install it by clicking on \"Install BepInEx\" button.",
                        Text, MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
            else setup.Text = "Uninstall BepInEx";

            if (bepisPresent)
            {
                mods.AddRange(DirectoryScan());
            }

            if (melonPresent)
            {
                melonButton.Text = "Uninstall MelonLoader Compat";
                mods.AddRange(MelonScan());
            }

            using (var current = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Classes\\" + "fp2mm"))
            {
                if (current != null) { handlerButton.Text = "Unregister URL handler"; }
            }

            try
            {
                string[] gblink = args[1].Replace("fp2mm://", string.Empty).Replace("fp2mm:", string.Empty).Split(',');
                if (gblink.Length == 1)
                {
                    DownloadMod(new Uri(gblink[0]), "Unknown", "Unknown");
                }
                if (gblink.Length == 1)
                {
                    DownloadMod(new Uri(gblink[0]), gblink[1], "Unknown");
                }
                if (gblink.Length == 3)
                {
                    DownloadMod(new Uri(gblink[0]), gblink[1], gblink[2]);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            if (exists)
            {
                if (args.Length < 2) MessageBox.Show("Only one instance can be running at the time!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Process.GetCurrentProcess().Kill();
            }

            if (bepisPresent && File.Exists("BepInEx\\config\\BepInEx.cfg"))
            {
                string[] lines = File.ReadAllLines("BepInEx\\config\\BepInEx.cfg");
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].Contains("Enabled ="))
                    {
                        if (lines[i].Contains("Enabled = true")) enableConsoleToolStripMenuItem.Checked = true;
                        break;
                    }
                }
            }
            RenderList(mods);
        }

        private void RenderList()
        {
            mods.Clear();
            if (bepisPresent)
            {
                mods.AddRange(DirectoryScan());
            }

            if (melonPresent)
            {
                mods.AddRange(MelonScan());
            }
            DeduplicateMods();
        }
        private void RenderList(List<ModInfo> modInfos)
        {
            listView1.Items.Clear();
            listView1.FocusedItem = null;
            foreach (ModInfo modInfo in modInfos)
            {
                ListViewItem item = new ListViewItem
                {
                    Tag = modInfo,
                    Text = modInfo.Name
                };
                item.SubItems.Add(modInfo.Version);
                item.SubItems.Add(modInfo.Author);
                item.SubItems.Add(modInfo.Loader);
                item.Checked = modInfo.Enabled;
                listView1.Items.Add(item);
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

            //Redoing the work, but we just possibly removed multiple duplicate mods. Needs to be rescanned.
            mods.Clear();
            if (bepisPresent)
            {
                mods.AddRange(DirectoryScan());
            }

            if (melonPresent)
            {
                mods.AddRange(MelonScan());
            }
            RenderList(mods);
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

        public bool DownloadMod(Uri url, string type, string id)
        {
            string name = "Unknown", author = "Unknown", version = "1.0.0";
            string uri = string.Format("https://api.gamebanana.com/Core/Item/Data?itemid={0}&itemtype={1}&fields=name,Updates().aGetLatestUpdates(),Credits().aAuthors()", id, type);

            try
            {
                using (WebClient client = new WebClient())
                {
                    string response = client.DownloadString(uri);
                    using (JsonDocument document = JsonDocument.Parse(response))
                    {
                        if (document.RootElement.GetType().Equals(typeof(JsonObject)))
                        {
                            MessageBox.Show(document.RootElement.GetProperty("Error").GetString());
                            return false;
                        }
                        JsonElement jName = document.RootElement[0];
                        name = jName.GetString();
                        JsonElement jUpdate = document.RootElement[1];
                        try
                        {
                            version = jUpdate[0].GetProperty("_sVersion").GetString();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        JsonElement jAuthor = document.RootElement[2];
                        author = jAuthor[0][0].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            DialogResult dialogResult = MessageBox.Show(this, "Do you want to install \"" + name + "\", version " + version + "  by: " + author + "?", "Mod install", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    using (WebClient client = new WebClient())
                    {
                        client.OpenRead(url);
                        string format = client.ResponseHeaders.Get("Content-Type").Split('/')[1];
                        string filename = "tempmod." + format;
                        client.DownloadFile(url, filename);
                        InstallMod(filename, CheckArchive(filename));
                        File.Delete(filename);
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else if (dialogResult == DialogResult.No)
            {
                return false;
            }
            return false;
        }

        private void exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void savePlay_Click(object sender, EventArgs e)
        {
            if (fp2Found) Process.Start("FP2.exe");
        }

        private void setup_Click(object sender, EventArgs e)
        {
            if (!File.Exists("winhttp.dll"))
            {
                try
                {
                    WebClient client = new WebClient();
                    client.DownloadFile(new Uri("https://github.com/BepInEx/BepInEx/releases/download/v5.4.21/BepInEx_x86_5.4.21.0.zip"), "BepInEx.zip");
                    ExtractMod("BepInEx.zip", ArchiveType.BepinDir);
                    File.Delete("BepInEx.zip");

                    MessageBox.Show(this, "BepInEx installed!.\n\n" +
                    "The game is now ready for modding.",
                    Text, MessageBoxButtons.OK, MessageBoxIcon.Information);

                    setup.Text = "Uninstall BepInEx";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Unpacking BepInEx failed!.\n\n" +
                    "Error info: " + ex.Message,
                    Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                File.Delete("winhttp.dll");
                bepisPresent = false;

                MessageBox.Show(this, "BepInEx hook removed!.\n\n" +
                "The mods will no longer be loaded.",
                Text, MessageBoxButtons.OK, MessageBoxIcon.Information);

                setup.Text = "Install BepInEx";

            }
        }

        private void modInstall_Click(object sender, EventArgs e)
        {
            if (modFileDialog.ShowDialog() == DialogResult.OK)
            {
                string file = modFileDialog.FileName;
                InstallMod(file, CheckArchive(file));
            }
        }

        private void InstallMod(string file, ArchiveType type)
        {
            switch (type)
            {
                case ArchiveType.BepinDir:
                    {
                        try
                        {
                            ExtractMod(file, ArchiveType.BepinDir);
                            MessageBox.Show("Mod unpacked!",
                            Text, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                            RenderList();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Unpacking failed!.\n\n" +
                            "Error info: " + ex.Message,
                            Text, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                        }
                        break;
                    }
                case ArchiveType.PluginDir:
                    {
                        try
                        {
                            ExtractMod(file, ArchiveType.PluginDir);
                            MessageBox.Show("Mod unpacked!",
                            Text, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                            RenderList();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(this, "Unpacking failed!\n\n" +
                            "Error info: " + ex.Message,
                            Text, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                        }
                        break;
                    }
                case ArchiveType.MelonDir:
                    {
                        try
                        {
                            Directory.CreateDirectory("MLLoader");
                            ExtractMod(file, ArchiveType.MelonDir);
                            if (melonPresent)
                            {
                                MessageBox.Show("MelonLoader mod unpacked!",
                                Text, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                            }
                            else
                            {
                                MessageBox.Show("MelonLoader mod unpacked!\n\nBut MelonLoader is not installed! Please install it before running Melon mods!",
                                Text, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                            }
                            RenderList();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Unpacking failed!\n\n" +
                            "Error info: " + ex.Message,
                            Text, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                        }
                        break;
                    }
                case ArchiveType.None:
                default:
                    {
                        MessageBox.Show("Provided archive is invalid!\n\n" +
                        "Please ensure the archive has proper directory structure, as well as contains a BepInEx/MelonLoader plugin.",
                        Text, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                        break;
                    }
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
                        if (!reader.Entry.IsDirectory)
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
                        if (!entry.IsDirectory)
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

        private bool EnableDisableMod(ModInfo info)
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
                //MessageBox.Show(ex.Message, "Error while changing mod status",MessageBoxButtons.OK,MessageBoxIcon.Error);
                //Disabled that as it would show if user spams the checkbox. More expected 'desync' behavior should propably stay
                return info.Enabled;
            }
        }


        private void refresh_Click(object sender, EventArgs e)
        {
            RenderList();
        }

        private void melonButton_Click(object sender, EventArgs e)
        {
            if (!melonPresent)
            {
                WebClient client = new WebClient();
                client.DownloadFile(new Uri("https://github.com/BepInEx/BepInEx.MelonLoader.Loader/releases/download/v2.0.0/BepInEx.MelonLoader.Loader.UnityMono_BepInEx5_2.0.0.zip"), "Melon.zip");
                ExtractMod("Melon.zip", ArchiveType.BepinDir);
                File.Delete("Melon.zip");

                MessageBox.Show(this, "MelonLoader plugin installed!\n\n" +
                "Melon Loader mods can now be installed. Please be aware that MelonLoader can be heavy on the game.",
                Text, MessageBoxButtons.OK, MessageBoxIcon.Information);

                melonPresent = true;
                RenderList();
                melonButton.Text = "Uninstall MelonLoader Compat";
            }
            else
            {
                Directory.Delete("BepInEx\\plugins\\BepInEx.MelonLoader.Loader", true);

                MessageBox.Show(this, "MelonLoader plugin uninstalled!\n\n" +
                "",
                Text, MessageBoxButtons.OK, MessageBoxIcon.Information);

                melonPresent = false;
                melonButton.Text = "Install MelonLoader Compat";
                RenderList();
            }
        }

        private void handlerButton_Click(object sender, EventArgs e)
        {
            RegisterGameBananaProtocol(true); //Separate method in case we want to run it ourselves from somewhere else.
        }

        void RegisterGameBananaProtocol(bool deleteIfPresent)
        {
            using (var current = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Classes\\" + "fp2mm"))
            {
                if (current == null)
                {
                    using (var key = Registry.CurrentUser.CreateSubKey("SOFTWARE\\Classes\\" + "fp2mm"))
                    {
                        string applicationLocation = typeof(FreedomManager).Assembly.Location;

                        Console.WriteLine(applicationLocation);

                        key.SetValue("", "URL: FreedomLoader");
                        key.SetValue("URL Protocol", "");

                        using (var defaultIcon = key.CreateSubKey("DefaultIcon"))
                        {
                            defaultIcon.SetValue("", applicationLocation + ",1");
                        }

                        using (var commandKey = key.CreateSubKey(@"shell\open\command"))
                        {
                            commandKey.SetValue("", "\"" + applicationLocation + "\" \"%1\"");
                        }
                    }
                    MessageBox.Show("URL handler registered!.\n\n" +
                    "Gamebanana 1-Click install is now available.",
                     "URL Handler", MessageBoxButtons.OK);

                    handlerButton.Text = "Unregister URL handler";
                }
                else if (deleteIfPresent)
                {
                    Registry.CurrentUser.DeleteSubKeyTree("SOFTWARE\\Classes\\" + "fp2mm");
                    MessageBox.Show("URL handler de-registered!.\n\n" +
                    "Gamebanana 1-Click support has been uninstalled.",
                    "URL Handler", MessageBoxButtons.OK);

                    handlerButton.Text = "Register URL handler";
                }
            }
        }
        private void listView1_NodeMouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ListViewHitTestInfo listViewHitTestInfo = listView1.HitTest(e.X, e.Y);
                if (listViewHitTestInfo.Item != null)
                {
                    columnIndex = listViewHitTestInfo.Item.Index;
                    contextMenuStrip1.Show(listView1, e.Location);
                }
            }
        }

        private void FM_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void FM_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                try
                {
                    InstallMod(files[0], CheckArchive(files[0])); //We deal with just one file, unpacking more at the time would add pointless complications.
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private void infoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ModInfo modInfo = (ModInfo)listView1.Items[columnIndex].Tag;

            StringBuilder builder = new StringBuilder();
            builder.Append("Name: ").AppendLine(modInfo.Name);
            builder.Append("Version: ").AppendLine(modInfo.Version);
            builder.Append("Author: ").AppendLine(modInfo.Author);
            builder.Append("Used Loader: ").AppendLine(modInfo.Loader);
            builder.Append("Uses mod_overrides: ").AppendLine((bool)modInfo.HasAssets ? "Yes" : "No");

            MessageBox.Show(this, builder.ToString(), "Mod information", MessageBoxButtons.OK);
        }

        private void uninstallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ModInfo modInfo = (ModInfo)listView1.Items[columnIndex].Tag;

            try
            {
                DialogResult dialogResult = MessageBox.Show(this, "Do you want to remove \"" + modInfo.Name + "\"?", "Mod uninstall", MessageBoxButtons.YesNo);
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
                    else if (melonPresent)
                    {
                        if (modInfo.ArchiveType == ArchiveType.MelonDir) //Melon mod
                        {
                            if (modInfo.Enabled)
                                File.Delete("MLLoader\\mods\\" + modInfo.Dirname + ".dll");
                            else
                                File.Delete("MLLoader\\mods-disabled\\" + modInfo.Dirname + ".dll");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            RenderList();
        }

        private void gitHubWikiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("explorer", "https://github.com/Kuborros/FreedomManager/wiki");
        }

        private void gameBananaPageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("explorer", "https://gamebanana.com/tools/10870");
        }

        private void enableConsoleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (enableConsoleToolStripMenuItem.Checked && File.Exists("BepInEx\\config\\BepInEx.cfg"))
            {
                string[] lines = File.ReadAllLines("BepInEx\\config\\BepInEx.cfg");
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].Contains("Enabled = true"))
                    {
                        lines[i] = "Enabled = false";
                        break;
                    }
                }
                File.WriteAllLines("BepInEx\\config\\BepInEx.cfg", lines);
                enableConsoleToolStripMenuItem.Checked = false;
            }
            else if (bepisPresent && File.Exists("BepInEx\\config\\BepInEx.cfg"))
            {
                string[] lines = File.ReadAllLines("BepInEx\\config\\BepInEx.cfg");
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].Contains("Enabled = false"))
                    {
                        lines[i] = "Enabled = true";
                        break;
                    }
                }
                File.WriteAllLines("BepInEx\\config\\BepInEx.cfg", lines);
                enableConsoleToolStripMenuItem.Checked = true;
            }
        }
        private void listView1_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            listView1.BeginUpdate();
            if (listView1.FocusedItem != null) //Prevents the initial checking of every item from firing an event
            {
                ModInfo info = (ModInfo)e.Item.Tag;
                e.Item.Checked = EnableDisableMod(info);
                RenderList();
            }
            listView1.EndUpdate();
        }

        private void openFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ModInfo modInfo = (ModInfo)listView1.Items[columnIndex].Tag;
            if (modInfo != null)
            {
                string path = "";
                if (modInfo.ArchiveType == ArchiveType.BepinDir || modInfo.ArchiveType == ArchiveType.PluginDir) //Bepin mod
                {
                    if (modInfo.Enabled)
                        path = "BepInEx\\plugins\\" + modInfo.Dirname;
                    else
                        path = "BepInEx\\plugins-disabled\\" + modInfo.Dirname;
                }
                else if (modInfo.ArchiveType == ArchiveType.DllDir) //Loose DLL bepin
                {
                    if (modInfo.Enabled)
                        path = "BepInEx\\plugins";
                    else
                        path = "BepInEx\\plugins-disabled";
                }
                else if (melonPresent)
                {
                    if (modInfo.ArchiveType == ArchiveType.MelonDir) //Melon mod
                    {
                        if (modInfo.Enabled)
                            path = "MLLoader\\mods";
                        else
                            path = "MLLoader\\mods-disabled";
                    }

                }
                Process.Start("explorer", Path.Combine(Path.GetFullPath("."), path));
            }
        }

        private void bepInExToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(Path.Combine(Path.GetFullPath("."), "BepInEx\\plugins")))
                Process.Start("explorer", Path.Combine(Path.GetFullPath("."), "BepInEx\\plugins"));
        }

        private void melonLoaderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(Path.Combine(Path.GetFullPath("."), "MLLoader\\mods")))
                Process.Start("explorer", Path.Combine(Path.GetFullPath("."), "MLLoader\\mods"));
        }

        private void installModToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (modFileDialog.ShowDialog() == DialogResult.OK)
            {
                string file = modFileDialog.FileName;
                InstallMod(file, CheckArchive(file));
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private async void checkForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var check = await _updateManager.CheckForUpdatesAsync();
            if (!check.CanUpdate)
            {
                MessageBox.Show(this,"There are no updates available.","Update check",MessageBoxButtons.OK,MessageBoxIcon.Information);
                return;
            } 
            else
            {
                DialogResult dialogResult = MessageBox.Show(this, "An update is available!\n\nDo you want to install it? (Manager will restart to do so)", "Update check", MessageBoxButtons.YesNo,MessageBoxIcon.Information);
                if (dialogResult == DialogResult.Yes)
                {
                    await _updateManager.PrepareUpdateAsync(check.LastVersion);
                    _updateManager.LaunchUpdater(check.LastVersion,true);
                    Application.Exit();
                }
            }
        }
    }
}


