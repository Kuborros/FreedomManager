using FreedomManager.Net.GitHub;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FreedomManager.Net
{
    internal class ModUpdateHandler
    {

        public ModUpdateHandler() { }



        private async Task<ModUpdateInfo> getModUpdate(ModInfo mod)
        {
            string ghAuthor, ghRepo;

            using (WebClient client = new WebClient())
            {
                client.Headers["Accept"] = "application/vnd.github+json";
                client.Headers["X-GitHub-Api-Version"] = "2022-11-28";
                client.Headers["user-agent"] = "FreedomManager";
                try
                {
                    MatchCollection matches = Regex.Matches("", "(?:\\w*:\\/\\/github.com\\/)([\\w\\d]*)(?:\\/)([\\w\\d]*)");

                    ghAuthor = matches[0].Groups[1].Value;
                    ghRepo = matches[0].Groups[2].Value;

                    string response = await client.DownloadStringTaskAsync(new Uri(string.Format("https://api.github.com/repos/{0}/{1}/releases/latest",ghAuthor,ghRepo)));
                    GitHubRelease release = JsonSerializer.Deserialize<GitHubRelease>(response);

                    return new ModUpdateInfo();

                } catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return null;
                }
            } 
        }

        internal void getModsUpdates(List<ModInfo> mods)
        {

        }

    }
}
