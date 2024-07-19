using FreedomManager.Net.GitHub;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FreedomManager.Net
{
    internal class ModUpdateHandler
    {

        public ModUpdateHandler() { }



        private async Task<ModUpdateInfo> getModUpdate(ModInfo mod)
        {
            string ghAuthor, ghRepo;

            //Skip mods with no GitHub link
            if (string.IsNullOrEmpty(mod.GitHub)) return null;

            using (WebClient client = new WebClient())
            {
                client.Headers["Accept"] = "application/vnd.github+json";
                client.Headers["X-GitHub-Api-Version"] = "2022-11-28";
                client.Headers["user-agent"] = "FreedomManager";
                try
                {
                    MatchCollection matches = Regex.Matches(mod.GitHub, "(?:\\w*:\\/\\/github.com\\/)([\\w\\d]*)(?:\\/)([\\w\\d]*)");

                    ghAuthor = matches[0].Groups[1].Value;
                    ghRepo = matches[0].Groups[2].Value;

                    string response = await client.DownloadStringTaskAsync(new Uri(string.Format("https://api.github.com/repos/{0}/{1}/releases/latest", ghAuthor, ghRepo)));
                    GitHubRelease release = JsonSerializer.Deserialize<GitHubRelease>(response);

                    Version local = new Version(mod.Version);

                    string remoteVersion = release.tag_name.Split('-')[0];
                    Version remote = new Version(remoteVersion);

                    //If local ver is higher or equal, it means no updates
                    if (local >= remote) return null;

                    ModUpdateInfo info = new ModUpdateInfo
                    {
                        Name = mod.Name,
                        Version = release.tag_name,
                        Description = release.body,
                        DownloadLink = release.downloadUrl,
                        DoUpdate = true
                    };
                    return info;

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        internal async Task<List<ModUpdateInfo>> getModsUpdates(List<ModInfo> mods)
        {
            List<ModUpdateInfo> updates = new List<ModUpdateInfo>();

            foreach (ModInfo mod in mods)
            {
                ModUpdateInfo info = await getModUpdate(mod);
                if (info != null)
                {
                    updates.Add(info);
                }
            }
            return updates;
        }
    }
}
