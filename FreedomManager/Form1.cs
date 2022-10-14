using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Windows.Forms;
using File = System.IO.File;

namespace FreedomManager
{
    public partial class FreedomManager : Form
    {
        bool bepisPresent = false;
        bool fp2Found = false;
        bool melonPresent = false;
        string rootDir = "";

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
            InitializeComponent();
            rootDir = typeof(FreedomManager).Assembly.Location.Replace("FreedomManager.exe", "");
            Directory.SetCurrentDirectory(rootDir);
            bepisPresent = File.Exists("winhttp.dll");
            fp2Found = File.Exists("FP2.exe");
            melonPresent = Directory.Exists("MLLoader");

            if (!fp2Found)
            {
                MessageBox.Show(this, "Freedom Planet 2 not Found!.\n\n" +
                "Please ensure the mod manager is in the main game directory.",
                Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                savePlay.Hide();
                setup.Hide();
                handlerButton.Hide();
                melonButton.Hide();
            }

            if (fp2Found && !bepisPresent)
            {
                MessageBox.Show(this, "BepInEx not Found!.\n\n" +
                        "Seems you dont have BepInEx installed - before you install any mods, install it by clicking on \"Install BepInEx\" button.",
                        Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else setup.Text = "Uninstall BepInEx";

            treeView1.Nodes.Add("Mods:");
            treeView1.Nodes.Add("Mods (Loose DLL):");

            if (bepisPresent)
            {
                DirectoryScan();
            }

            if (melonPresent)
            {
                treeView1.Nodes.Add("External loader mods (Melons):");
                melonButton.Text = "Uninstall MelonLoader Compat";
                MelonScan();
            }

            using (var current = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Classes\\" + "fp2mm"))
            {
                if (current != null) { handlerButton.Text = "Unregister URL handler"; }
            }

            try
            {
                string[] gblink = args[1].Replace("fp2mm:", string.Empty).Split(',');
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
            catch
            {
                Console.WriteLine("No arguments provided.");
            }

        }

        public ArchiveType CheckArchive(String path)
        {
            if (File.Exists(path) && Path.GetExtension(path) == ".zip")
            {
                ZipArchive zipArchive = ZipFile.OpenRead(path);
                foreach (ZipArchiveEntry zipArchiveEntry in zipArchive.Entries)
                {
                    if (zipArchiveEntry.FullName.ToLower() == "bepinex/")
                    {
                        zipArchive.Dispose();
                        return ArchiveType.BepinDir;
                    }
                    if (zipArchiveEntry.FullName.ToLower() == "plugins/")
                    {
                        zipArchive.Dispose();
                        return ArchiveType.PluginDir;
                    }
                    if (zipArchiveEntry.FullName.ToLower() == "mods/")
                    {
                        zipArchive.Dispose();
                        return ArchiveType.MelonDir;
                    }
                }
                zipArchive.Dispose();
            }
            return ArchiveType.None;
        }

        public void DirectoryScan()
        {
            String dir = "BepInEx\\plugins";
            try
            {
                treeView1.Nodes[1].Nodes.Clear();
                treeView1.Nodes[0].Nodes.Clear();
                foreach (string f in Directory.GetFiles(dir))
                {

                    if (Path.GetExtension(f) == ".dll")
                    {
                        treeView1.Nodes[1].Nodes.Add(Path.GetFileNameWithoutExtension(f));
                        treeView1.Nodes[1].Expand();
                    }
                }
                foreach (string d in Directory.GetDirectories(dir))
                {
                    treeView1.Nodes[0].Nodes.Add(Path.GetFileName(d));
                    treeView1.Nodes[0].Expand();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public void MelonScan()
        {
            String dir = "MLLoader\\Mods";
            try
            {
                treeView1.Nodes[2].Nodes.Clear();
                foreach (string f in Directory.GetFiles(dir))
                {

                    if (Path.GetExtension(f) == ".dll")
                    {
                        treeView1.Nodes[2].Nodes.Add(Path.GetFileNameWithoutExtension(f));
                        treeView1.Nodes[2].Expand();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public bool DownloadMod(Uri url, string name, string author)
        {
            DialogResult dialogResult = MessageBox.Show(this, "Do you want to install \"" + name.Replace("%20", " ") + "\" by: " + author + "?", "Mod install", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                WebClient client = new WebClient();
                client.DownloadFile(url, "tempmod.zip");
                InstallMod("tempmod.zip", CheckArchive("tempmod.zip"));
                File.Delete("tempmod.zip");
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
                    //TODO: Check download?
                    DeleteFilesPresentInZip("BepInEx.zip", ArchiveType.BepinDir);
                    ZipFile.ExtractToDirectory("BepInEx.zip", rootDir);
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
            modFileDialog.ShowDialog();
            string file = modFileDialog.FileName;
            InstallMod(file, CheckArchive(file));
        }

        private void InstallMod(string file, ArchiveType type)
        {
            switch (type)
            {
                case ArchiveType.BepinDir:
                    {
                        try
                        {
                            DeleteFilesPresentInZip(file, ArchiveType.BepinDir);
                            ZipFile.ExtractToDirectory(file, rootDir);
                            MessageBox.Show(this, "Mod Unpacked!.",
                            Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            DirectoryScan();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(this, "Unpacking failed!.\n\n" +
                            "Error info: " + ex.Message,
                            Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        break;
                    }
                case ArchiveType.PluginDir:
                    {
                        try
                        {
                            DeleteFilesPresentInZip(file, ArchiveType.PluginDir);
                            ZipFile.ExtractToDirectory(file, "BepInEx");
                            MessageBox.Show(this, "Mod Unpacked!.",
                            Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            DirectoryScan();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(this, "Unpacking failed!.\n\n" +
                            "Error info: " + ex.Message,
                            Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        break;
                    }
                case ArchiveType.MelonDir:
                    {
                        try
                        {
                            DeleteFilesPresentInZip(file, ArchiveType.MelonDir);
                            Directory.CreateDirectory("MLLoader");
                            ZipFile.ExtractToDirectory(file, "MLLoader");
                            MessageBox.Show(this, "Mod Unpacked!.",
                            Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            DirectoryScan();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(this, "Unpacking failed!.\n\n" +
                            "Error info: " + ex.Message,
                            Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        break;
                    }
                case ArchiveType.None:
                    {
                        MessageBox.Show(this, "Provided archive is invalid!.\n\n" +
                        "Please ensure the archive has proper directory structure, as well as containing a BepInEx plugin.",
                        Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }
                default:
                    {
                        MessageBox.Show(this, "Provided archive is invalid!.\n\n" +
                        "Please ensure the archive has proper directory structure, as well as containing a BepInEx plugin.",
                        Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }
            }
        }

        private void refresh_Click(object sender, EventArgs e)
        {
            DirectoryScan();
            melonPresent = Directory.Exists("MLLoader\\Mods");
            if (melonPresent) MelonScan();
        }

        private void DeleteFilesPresentInZip(String path, ArchiveType type)
        {
            ZipArchive zipArchive = ZipFile.OpenRead(path);
            foreach (ZipArchiveEntry zipArchiveEntry in zipArchive.Entries)
            {
                try
                {
                    switch (type)
                    {
                        case ArchiveType.BepinDir:
                            {
                                File.Delete(zipArchiveEntry.FullName);
                                break;
                            }
                        case ArchiveType.PluginDir:
                            {
                                File.Delete("BepInEx\\" + zipArchiveEntry.FullName);
                                break;
                            }
                        case ArchiveType.MelonDir:
                            {
                                File.Delete("MLLoader\\" + zipArchiveEntry.FullName);
                                break;
                            }
                        default:
                            {
                                break;
                            }

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            zipArchive.Dispose();
        }

        private void melonButton_Click(object sender, EventArgs e)
        {
            if (!melonPresent)
            {
                WebClient client = new WebClient();
                client.DownloadFile(new Uri("https://github.com/BepInEx/BepInEx.MelonLoader.Loader/releases/download/v2.0.0/BepInEx.MelonLoader.Loader.UnityMono_BepInEx5_2.0.0.zip"), "Melon.zip");
                DeleteFilesPresentInZip("Melon.zip", ArchiveType.BepinDir);
                ZipFile.ExtractToDirectory("Melon.zip", ".");
                File.Delete("Melon.zip");

                MessageBox.Show(this, "MelonLoader plugin installed!\n\n" +
                "Melon Loader mods can now be installed. Please be aware that MelonLoader can be heavy on the game.",
                Text, MessageBoxButtons.OK, MessageBoxIcon.Information);

                melonPresent = true;
                DirectoryScan();
                treeView1.Nodes.Add("External loader mods (Melons):");
                MelonScan();
                melonButton.Text = "Uninstall MelonLoader Compat";
            } else
            {
                Directory.Delete("BepInEx\\plugins\\BepInEx.MelonLoader.Loader",true);
                Directory.Delete("MLLoader", true);

                MessageBox.Show(this, "MelonLoader plugin uninstalled!\n\n" +
                "",
                Text, MessageBoxButtons.OK, MessageBoxIcon.Information);

                melonPresent = true;
                melonButton.Text = "Install MelonLoader Compat";
                DirectoryScan();
            }
        }

        private void handlerButton_Click(object sender, EventArgs e)
        {
            RegisterGameBananaProtocol();
        }

        void RegisterGameBananaProtocol()
        {
            using (var current = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Classes\\" + "fp2mm"))
            {
                if (current == null)
                {
                    using (var key = Registry.CurrentUser.CreateSubKey("SOFTWARE\\Classes\\" + "fp2mm"))
                    {
                        string applicationLocation = "FreedomManager.exe";

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
                else
                {
                    Registry.CurrentUser.DeleteSubKeyTree("SOFTWARE\\Classes\\" + "fp2mm");
                    MessageBox.Show("URL handler de-registered!.\n\n" +
                    "Gamebanana 1-Click support has been uninstalled.",
                    "URL Handler", MessageBoxButtons.OK);

                    handlerButton.Text = "Register URL handler";
                }
            }
        }


    }
}
