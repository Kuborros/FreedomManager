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
            //TODO: Window with info here.
            if (!bepisPresent) {
                WebClient client = new WebClient();
                client.DownloadFile(new Uri("https://github.com/BepInEx/BepInEx/releases/download/v5.4.21/BepInEx_x86_5.4.21.0.zip"), "BepInEx.zip");
                //TODO: Check download?
                ZipFile.ExtractToDirectory("BepInEx.zip", ".");
                File.Delete("BepInEx.zip");
            } 
            else
            {
                File.Delete("winhttp.dll");
                bepisPresent = false;
            }
        }
    }
}
