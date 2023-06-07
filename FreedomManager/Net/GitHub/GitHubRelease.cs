using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FreedomManager.Net.GitHub
{
    internal class GitHubRelease
    {
        public string tag_name { get; set; }
        public string name { get; set; }
        public string body { get; set; }
        public List<GitHubAsset> assets { get; set; }

        GitHubAsset latest { get; set; }
        public string downloadUrl { get; set; }
        public string filename { get; set; }


        [JsonConstructor]
        public GitHubRelease(List<GitHubAsset> assets, string tag_name, string name, string body)
        {
            this.assets = assets;
            this.name = name;
            this.body = body;
            this.tag_name = tag_name;

            latest = assets[0];

            downloadUrl = latest.browser_download_url;
            filename = latest.name;
        }
    }
}
