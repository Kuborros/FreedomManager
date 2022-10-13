using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using static System.Net.WebRequestMethods;
using File = System.IO.File;

namespace FreedomManager
{
    public partial class Form1 : Form
    {
        bool bepisPresent = false;
        bool fp2Found = false;
        bool melonPresent = false;

        public enum ArchiveType
        {
            BepinDir,
            PluginDir,
            MelonDir,
            None
        }

        public Form1()
        {
            InitializeComponent();
            bepisPresent = File.Exists("winhttp.dll");
            fp2Found = File.Exists("FP2.exe");
            melonPresent = Directory.Exists("MLLoader\\Mods");

            if (!fp2Found)
            {
                MessageBox.Show(this, "Freedom Planet 2 not Found!.\n\n" +
                "Please ensure the mod manager is in the main game directory.",
                Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                savePlay.Hide();
                setup.Hide();
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
            treeView1.Nodes[1].Nodes.Clear();
            treeView1.Nodes[0].Nodes.Clear();
            try
            {
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
            String dir = "MLLoader\\Mods";
            treeView1.Nodes[2].Nodes.Clear();
            try
            {
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

        public bool DownloadMod(Uri url, string path, string name)
        {
            return false;
        }

        public ModInfo parseArchive(string path)
        {
            ModInfo info = new ModInfo();
            return info;
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
                    ZipFile.ExtractToDirectory("BepInEx.zip", ".");
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
            switch (CheckArchive(file))
            {
                case ArchiveType.BepinDir:
                    {
                        try
                        {
                            DeleteFilesPresentInZip(file, ArchiveType.BepinDir);
                            ZipFile.ExtractToDirectory(file, ".");
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
                            ZipFile.ExtractToDirectory(file, "MLLoader\\Mods");
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

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

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
                                File.Delete("BepInEx\\"+zipArchiveEntry.FullName);
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

        }

        private void handlerButton_Click(object sender, EventArgs e)
        {

        }
    }
}
