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
        public Form1()
        {
            InitializeComponent();
            bepisPresent = File.Exists("winhttp.dll");
            fp2Found = File.Exists("FP2.exe");

            if (!bepisPresent) {
            MessageBox.Show(this, "BepInEx not Found!.\n\n" +
                    "Seems you dont have BepInEx installed - before you install any mods, install it by clicking on \"Install BepInEx\" button.",
                    Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            } else setup.Text = "Uninstall BepInEx";

            if (!fp2Found) {
                MessageBox.Show(this, "Freedom Planet 2 not Found!.\n\n" +
                "Please ensure the mod manager is in the main game directory.",
                Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                savePlay.Hide(); 
            }

            treeView1.Nodes.Add("Mods:");
            treeView1.Nodes.Add("Mods (Loose DLL):");
            if (Directory.Exists("Mods"))
            {
                treeView1.Nodes.Add("External loader mods (Melons):");
            }

            if (bepisPresent)
            {
                DirectoryScan();
            }

        }

        public bool CheckArchive(String path)
        {
            bool hasBepDir = false;
            if (File.Exists(path) && Path.GetExtension(path) == ".zip")
            {
                ZipArchive zipArchive = ZipFile.OpenRead(path);
                foreach (ZipArchiveEntry zipArchiveEntry in zipArchive.Entries)
                {
                    if (zipArchiveEntry.Name == "BepInEx") hasBepDir = true;
                }
            }
            return hasBepDir;
        }

        public void DirectoryScan()
        {
            String dir = "BepInEx\\plugins";
            try
            {
                foreach (string f in Directory.GetFiles(dir))
                {
                    treeView1.Nodes[1].Nodes.Add(Path.GetFileName(f));
                    Console.WriteLine(f);
                }
                foreach (string d in Directory.GetDirectories(dir))
                {
                    treeView1.Nodes[0].Nodes.Add(Path.GetFileName(d));
                    Console.WriteLine(d);
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }   


        private void exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void savePlay_Click(object sender, EventArgs e)
        {
            if(fp2Found) Process.Start("FP2.exe");
        }

        private void setup_Click(object sender, EventArgs e)
        {
            if (!bepisPresent) {
                WebClient client = new WebClient();
                client.DownloadFile(new Uri("https://github.com/BepInEx/BepInEx/releases/download/v5.4.21/BepInEx_x86_5.4.21.0.zip"), "BepInEx.zip");
                //TODO: Check download?
                ZipFile.ExtractToDirectory("BepInEx.zip", ".");
                File.Delete("BepInEx.zip");

                MessageBox.Show(this, "BepInEx installed!.\n\n" +
                "The game is now ready for modding.",
                Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            } 
            else
            {
                File.Delete("winhttp.dll");
                bepisPresent = false;

                MessageBox.Show(this, "BepInEx hook removed!.\n\n" +
                "The mods will no longer be loaded.",
                Text, MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }

        private void modInstall_Click(object sender, EventArgs e)
        {
            modFileDialog.ShowDialog();
            string file = modFileDialog.FileName;
            if (CheckArchive(file))
            {
                ZipFile.ExtractToDirectory(file, ".");
                MessageBox.Show(this, "Mod Unpacked!.\n\n" +
                "Test Message.",
                Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                DirectoryScan();
            }
            else
            {
                MessageBox.Show(this, "Provided archive is invalid!.\n\n" +
                "Please ensure the archive has proper directory structure, as well as containing a BepInEx .",
                Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void refresh_Click(object sender, EventArgs e)
        {
            DirectoryScan();
        }
    }
}
