using Microsoft.Win32;
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

namespace FreedomManager
{
    public partial class FreedomManager : Form
    {
        bool bepisPresent = false;
        bool fp2Found = false;
        bool melonPresent = false;
        bool exists = false;
        string rootDir = "";
        List<ModInfo> mods = new List<ModInfo>();

        public enum ArchiveType
        {
            BepinDir,
            PluginDir,
            MelonDir,
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

            treeView1.Nodes.Add("bepinmods", "Mods:");
            treeView1.Nodes.Add("bepindllmods", "Mods (Loose DLL):");

            if (bepisPresent)
            {
                mods.AddRange(DirectoryScan());
            }

            if (melonPresent)
            {
                treeView1.Nodes.Add("melonmods", "MelonLoader mods:");
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
        }

        public ArchiveType CheckArchive(String path)
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

        public List<ModInfo> DirectoryScan()
        {
            string dir = "BepInEx\\plugins";
            List<ModInfo> list = new List<ModInfo>();
            try
            {
                treeView1.Nodes[1].Nodes.Clear();
                treeView1.Nodes[0].Nodes.Clear();


                foreach (string f in Directory.GetFiles(dir))
                {

                    if (Path.GetExtension(f) == ".dll")
                    {
                        string modname = Path.GetFileNameWithoutExtension(f);
                        list.Add(new ModInfo(modname, ArchiveType.PluginDir));
                        treeView1.Nodes[1].Nodes.Add(modname, modname);
                        treeView1.Nodes[1].Expand();
                    }
                }
                foreach (string d in Directory.GetDirectories(dir))
                {
                    string modname = Path.GetFileName(d);
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
                                    modname = info.Name + " " + info.Version;
                                }
                                catch (Exception ex) { Console.WriteLine(ex); }
                            }
                        }
                        treeView1.Nodes[0].Nodes.Add(Path.GetFileName(d), modname);
                        treeView1.Nodes[0].Expand();
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
                treeView1.Nodes[2].Nodes.Clear();
                foreach (string f in Directory.GetFiles(dir))
                {

                    if (Path.GetExtension(f) == ".dll")
                    {
                        string modname = Path.GetFileNameWithoutExtension(f);
                        list.Add(new ModInfo(modname, ArchiveType.MelonDir));
                        treeView1.Nodes[2].Nodes.Add(modname, modname);
                        treeView1.Nodes[2].Expand();
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
                        if (document.RootElement.GetType().Equals(typeof(JsonObject))) {
                            MessageBox.Show(document.RootElement.GetProperty("Error").GetString());
                            return false;
                        }
                        JsonElement jName = document.RootElement[0];
                        name = jName.GetString();
                        JsonElement jUpdate = document.RootElement[1];
                        try { 
                        version = jUpdate[0].GetProperty("_sVersion").GetString();
                        } catch (Exception ex)
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
            if (modFileDialog.ShowDialog() == DialogResult.OK) {
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
                            DirectoryScan();
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
                            DirectoryScan();
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
                                DirectoryScan();
                                MelonScan();
                            } 
                            else
                            {
                                MessageBox.Show("MelonLoader mod unpacked!\n\nBut MelonLoader is not installed! Please install it before running Melon mods!",
                                Text, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                            }


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

        private void refresh_Click(object sender, EventArgs e)
        {
            DirectoryScan();
            if (melonPresent) MelonScan();
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
                DirectoryScan();
                treeView1.Nodes.RemoveByKey("melonmods");
                treeView1.Nodes.Add("melonmods", "MelonLoader mods:");
                MelonScan();
                melonButton.Text = "Uninstall MelonLoader Compat";
            }
            else
            {
                Directory.Delete("BepInEx\\plugins\\BepInEx.MelonLoader.Loader", true);
                //Directory.Delete("MLLoader", true);

                MessageBox.Show(this, "MelonLoader plugin uninstalled!\n\n" +
                "",
                Text, MessageBoxButtons.OK, MessageBoxIcon.Information);

                melonPresent = false;
                treeView1.Nodes.RemoveByKey("melonmods");
                melonButton.Text = "Install MelonLoader Compat";
                DirectoryScan();
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

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right && !(e.Node == treeView1.Nodes[0] || e.Node == treeView1.Nodes[1] || e.Node.Name == "melonmods"))
            {
                treeView1.SelectedNode = treeView1.GetNodeAt(e.X, e.Y); //Critical for other code, we need to know what node is selected by the user.
                contextMenuStrip1.Show(treeView1, e.Location);
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
            string node = treeView1.SelectedNode.Name;
            ModInfo modInfo;
            if (treeView1.SelectedNode.Parent == treeView1.Nodes[0] && File.Exists("BepInEx\\plugins\\" + node + "\\modinfo.json")) //Bepin mod with ModInfo
            {
                try
                {
                    modInfo = JsonSerializer.Deserialize<ModInfo>(File.ReadAllText("BepInEx\\plugins\\" + node + "\\modinfo.json"));
                }
                catch
                {
                    modInfo = new ModInfo(node, ArchiveType.BepinDir); //In case .json reading fails, fallback to no modinfo method
                }
            }
            else if (treeView1.SelectedNode.Parent == treeView1.Nodes[0]) //Bepin mod
            {
                modInfo = new ModInfo(node, ArchiveType.BepinDir);
            }
            else if (treeView1.SelectedNode.Parent == treeView1.Nodes[1]) //Loose DLL bepin
            {
                modInfo = new ModInfo(node, ArchiveType.DllDir);
            }
            else if (melonPresent)
            {
                if (treeView1.SelectedNode.Parent == treeView1.Nodes[2]) //Melon mod
                {
                    modInfo = new ModInfo(node, ArchiveType.MelonDir);
                }
                else return;
            }
            else return;

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
            string node = treeView1.SelectedNode.Name;

            try
            {
                DialogResult dialogResult = MessageBox.Show(this, "Do you want to remove \"" + node + "\"?", "Mod uninstall", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    if (treeView1.SelectedNode.Parent == treeView1.Nodes[0]) //Bepin mod
                    {
                        Directory.Delete("BepInEx\\plugins\\" + node, true);
                    }
                    else if (treeView1.SelectedNode.Parent == treeView1.Nodes[1]) //Loose DLL bepin
                    {
                        File.Delete("BepInEx\\plugins\\" + node + ".dll");
                    }
                    else if (melonPresent)
                    {
                        if (treeView1.SelectedNode.Parent == treeView1.Nodes[2]) //Melon mod
                        {
                            File.Delete("MLLoader\\mods\\" + node + ".dll");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            DirectoryScan();
        }
    }
}


