

namespace FreedomManager.Net.GitHub
{
    internal class GitHubUploader
    {
        public string login { get; set; }
        public string avatar_url { get; set; }
        public string gravatar_id { get; set; }
        public string html_url { get; set; }
        public string type { get; set; }

        public GitHubUploader(string login, string avatar_url, string gravatar_id, string html_url, string type)
        {
            this.login = login;
            this.avatar_url = avatar_url;
            this.gravatar_id = gravatar_id;
            this.html_url = html_url;
            this.type = type;
        }
    }
}
