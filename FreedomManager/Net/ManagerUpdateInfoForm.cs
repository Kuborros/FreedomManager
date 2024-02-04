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
        }

        internal void loadFp2mmChangelog()
        {
            titleLabel.Text = "Mod Manager Update available!";

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

        internal void loadFp2libChangelog()
        {
            titleLabel.Text = "FP2Lib Update available!";

            using (WebClient client = new WebClient())
            {
                client.Headers["Accept"] = "application/vnd.github+json";
                client.Headers["X-GitHub-Api-Version"] = "2022-11-28";
                client.Headers["user-agent"] = "FreedomManager";
                try
                {
                    string response = client.DownloadString(new Uri("https://api.github.com/repos/Kuborros/FP2Lib/releases/latest"));
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

        private void updateButton_Click(object sender, EventArgs e)
        {
            Close();
            DialogResult = DialogResult.Yes;
        }
    }
}
