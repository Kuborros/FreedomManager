using FreedomManager.Net.GitHub;
using System;
using System.Net;
using System.Text.Json;
using System.Windows.Forms;

namespace FreedomManager.Net
{
    public partial class ManagerUpdateInfoForm : Form
    {
        public ManagerUpdateInfoForm()
        {
            InitializeComponent();

            using (WebClient client = new WebClient())
            {
                client.Headers["Accept"] = "application/vnd.github+json";
                client.Headers["X-GitHub-Api-Version"] = "2022-11-28";
                client.Headers["user-agent"] = "FreedomManager";
                try
                {
                    string response = client.DownloadString(new Uri("https://api.github.com/repos/Kuborros/FreedomManager/releases/latest"));
                    GitHubRelease release = JsonSerializer.Deserialize<GitHubRelease>(response);
                    changelogRichTextBox.Text = release.body;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    changelogRichTextBox.Text = "Failed to obtain changelog.";
                }
            }
        }
    }
}
