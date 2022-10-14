﻿using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Reflection.Emit;
using System.Threading;
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
            bepisPresent = File.Exists(rootDir + "winhttp.dll");
            fp2Found = File.Exists(rootDir + "FP2.exe");
            melonPresent = Directory.Exists(rootDir + "MLLoader\\Mods");

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
                MelonScan();
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
            String dir = rootDir + "BepInEx\\plugins";
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
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public void MelonScan()
        {
            String dir = rootDir + "MLLoader\\Mods";
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
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public bool DownloadMod(Uri url, string name, string author)
        {
            DialogResult dialogResult = MessageBox.Show(this, "Do you want to install " + name + " by: " + author + "?", "Mod install", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                    WebClient client = new WebClient();
                    client.DownloadFile(url, "tempmod.zip");
                    InstallMod(rootDir + "tempmod.zip", CheckArchive(rootDir + "tempmod.zip"));
                    File.Delete(rootDir + "tempmod.zip");
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
            if (!File.Exists(rootDir + "winhttp.dll"))
            {
                try
                {
                    WebClient client = new WebClient();
                    client.DownloadFile(new Uri("https://github.com/BepInEx/BepInEx/releases/download/v5.4.21/BepInEx_x86_5.4.21.0.zip"), rootDir + "BepInEx.zip");
                    //TODO: Check download?
                    DeleteFilesPresentInZip(rootDir + "BepInEx.zip", ArchiveType.BepinDir);
                    ZipFile.ExtractToDirectory(rootDir + "BepInEx.zip", rootDir);
                    File.Delete(rootDir + "BepInEx.zip");

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
                File.Delete(rootDir + "winhttp.dll");
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
                            ZipFile.ExtractToDirectory(file, rootDir + "BepInEx");
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
                            ZipFile.ExtractToDirectory(file, rootDir + "MLLoader\\Mods");
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
            melonPresent = Directory.Exists(rootDir + "MLLoader\\Mods");
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
                                File.Delete(rootDir + zipArchiveEntry.FullName);
                                break;
                            }
                        case ArchiveType.PluginDir:
                            {
                                File.Delete(rootDir + "BepInEx\\" + zipArchiveEntry.FullName);
                                break;
                            }
                        case ArchiveType.MelonDir:
                            {
                                File.Delete(rootDir + "MLLoader\\" + zipArchiveEntry.FullName);
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
                client.DownloadFile(new Uri("https://github.com/BepInEx/BepInEx.MelonLoader.Loader/releases/download/v2.0.0/BepInEx.MelonLoader.Loader.UnityMono_BepInEx5_2.0.0.zip"), rootDir + "Melon.zip");
                DeleteFilesPresentInZip(rootDir + "Melon.zip", ArchiveType.BepinDir);
                ZipFile.ExtractToDirectory(rootDir + "Melon.zip", ".");
                File.Delete(rootDir + "Melon.zip");

                MessageBox.Show(this, "MelonLoader plugin installed!.\n\n" +
                "Melon Loader mods can now be installed. Please be aware that MelonLoader can be heavy on the game.",
                Text, MessageBoxButtons.OK, MessageBoxIcon.Information);

                melonPresent = true;
                DirectoryScan();
                treeView1.Nodes.Add("External loader mods (Melons):");
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
                        string applicationLocation = rootDir + "FreedomManager.exe";

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
