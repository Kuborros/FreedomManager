using System;
using System.Text.Json.Serialization;

namespace FreedomManager.Net.GitHub
{
    internal class GitHubAsset
    {
        public string name { get; set; }
        public GitHubUploader uploader { get; set; }
        public string content_type { get; set; }
        public string state { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public string browser_download_url { get; set; }


        [JsonConstructor]
        public GitHubAsset(string name, GitHubUploader uploader, string content_type, string state, DateTime created_at, DateTime updated_at, string browser_download_url)
        {
            this.name = name;
            this.uploader = uploader;
            this.content_type = content_type;
            this.state = state;
            this.created_at = created_at;
            this.updated_at = updated_at;
            this.browser_download_url = browser_download_url;
        }
    }
}
