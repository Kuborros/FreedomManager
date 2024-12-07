using FreedomManager.Config;
using FreedomManager.Mod;
using FreedomManager.Net;
using FreedomManager.Net.GitHub;
using Microsoft.Win32;
using Onova;
using Onova.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Net;
using System.Net.Cache;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FreedomManager
{
    public partial class FreedomManager : Form
    {
        readonly bool bepisPresent = false;
        readonly bool fp2Found = false;
        bool melonPresent = false;
        int columnIndex = 0;
        internal string tempname;
        internal List<ModUpdateInfo> modUpdates;

        private enum UrlType
        {
            GBANANA,
            GITHUB,
            GENERIC
        }

        private readonly IUpdateManager _updateManager = new UpdateManager(
            //new WebPackageResolver("https://fp2mods.info/fp2mm/versions.manifest"), //Cool but insecure - if i were to loose the domain anyone could spoof updates.
            new GithubPackageResolver("Kuborros", "FreedomManager", "FreedomManager*.zip"), //Lame but secure - needs _specific_ update name syntax to deem them worthy
            new ZipPackageExtractor());

        static BepinConfig bepinConfig;
        static DoorstopConfig doorstopConfig;
        static FP2LibConfig fP2LibConfig;
        static ManagerConfig managerConfig;
        static ModUpdateHandler modUpdateHandler;
        public static ModHandler modHandler;
        public static LoaderHandler loaderHandler;

        public FreedomManager(List<string> uris)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            DragDrop += new DragEventHandler(FM_DragDrop);
            DragEnter += new DragEventHandler(FM_DragEnter);

            InitializeComponent();

            loaderHandler = new LoaderHandler();
            modHandler = new ModHandler();
            managerConfig = new ManagerConfig();

            bepisPresent = loaderHandler.bepinInstalled;
            melonPresent = loaderHandler.melonInstalled;
            fp2Found = loaderHandler.fp2Found;

            if (!fp2Found)
            {
                //No FP2, no loader.
                using (Form tempform = new Form { TopMost = true })
                    MessageBox.Show(tempform,"Freedom Planet 2 not Found!.\n\n" +
                    "Please ensure the mod manager is in the main game directory.",
                    "", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                Environment.Exit(1);
            }

            if (fp2Found && !bepisPresent)
            {
                using (Form tempform = new Form { TopMost = true })
                    MessageBox.Show(tempform, "BepInEx not Found!.\n\n" +
                        "Seems you dont have BepInEx installed - before you install any mods, install it by clicking on \"Install BepInEx\" button.",
                        "", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);

                bepinVersionLabel.Text = "N/A";
            }
            else
            {
                setup.Text = "Uninstall BepInEx";
                if (managerConfig.bepinexVersion != null)
                {
                    loaderHandler.bepinVersion = managerConfig.bepinexVersion;
                }
                else
                {
                    loaderHandler.bepinVersion = "5.4.22.0";
                }
                bepinVersionLabel.Text = loaderHandler.bepinVersion;
            }

            if (melonPresent)
            {
                melonButton.Text = "Uninstall MelonLoader Compat";
            }

            bepInExToolStripMenuItem.Enabled = bepisPresent;
            melonLoaderToolStripMenuItem.Enabled = melonPresent;

            using (var current = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Classes\\" + "fp2mm"))
            {
                if (current != null) { handlerButton.Text = "Unregister URL handler"; }
            }

            if (uris.Count > 0) handleGBUri(uris[0]);

            bepinConfig = new BepinConfig();
            doorstopConfig = new DoorstopConfig();
            fP2LibConfig = new FP2LibConfig();

            if (loaderHandler.bepinInstalled && !loaderHandler.bepinUtilsInstalled)
            {
                loaderHandler.installBepinUtils(false);
            }
            updateConfigUi();

            RenderList(modHandler.modList);
            OneClickServer();

            modUpdateHandler = new ModUpdateHandler();

            if (managerConfig.autoUpdateManager)
            {
                Task.Run(() => CheckForUpdatesAsync(true));
            }
            if (managerConfig.autoUpdateFP2Lib)
            {
                if (loaderHandler.bepinInstalled)
                {
                    checkForFP2LibUpdatesAsync(true);
                }
            }
            if (managerConfig.autoUpdateMods)
            {
                checkForModUpdatesAsync(false);
            }
            if (managerConfig.autoUpdateBepin)
            {
                checkForBepInExUpdatesAsync(false);
            }

            managerVersionLabel.Text = Application.ProductVersion;
        }

        private void updateConfigUi()
        {
            fP2LibConfig = new FP2LibConfig();
            bepinConfig = new BepinConfig();

            bepInExToolStripMenuItem.Enabled = bepisPresent;
            reinstallSplashButton.Enabled = bepisPresent;
            melonLoaderToolStripMenuItem.Enabled = melonPresent;
            openLogfileToolStripMenuItem.Enabled = File.Exists(Path.Combine(Path.GetFullPath("."), "BepInEx\\LogOutput.log"));

            if (bepinConfig.confExists)
            {
                bepinGroupBox.Enabled = true;
                bepinConfgroupBox.Enabled = true;

                enableConsoleCheckBox.Checked = bepinConfig.ShowConsole;
                noConsoleCloseCheckBox.Checked = bepinConfig.ConsolePreventClose;
                logfileCheckBox.Checked = bepinConfig.FileLog;
                hideLogsCheckBox.Checked = bepinConfig.UnityLogListening;
                unityFileCheckBox.Checked = bepinConfig.WriteUnityLog;
                appendLogCheckBox.Checked = bepinConfig.AppendLog;
                splashEnableCheckBox.Checked = bepinConfig.SplashEnabled;
                splashWithConsoleCheckBox.Checked = !bepinConfig.OnlyNoConsole;
                logLevelTextBox.Text = bepinConfig.LogLevels;
                harmonyLogTextBox.Text = bepinConfig.HarmonyLogLevels;

                doorstopFileLogCheckBox.Checked = doorstopConfig.RedirectOutputLog;
            }
            else
            {
                bepinGroupBox.Enabled = false;
                bepinConfgroupBox.Enabled = false;
            }

            if (fP2LibConfig.configExists)
            {
                fp2libGroupBox.Enabled = true;
                if (fP2LibConfig.saveRedirectEnabled)
                {
                    saveRedirecCheckBox.Checked = true;
                    saveProfileComboBox.SelectedIndex = fP2LibConfig.saveRedirectProfile;
                }
                else
                {
                    saveProfileComboBox.SelectedIndex = 0;
                }

                fancyJsonCheckBox.Checked = fP2LibConfig.saveFancyJson;
            }
            else
            {
                fp2libGroupBox.Enabled = false;
            }

            if (loaderHandler.runningUnderSteam)
            {
                forceNonSteamCheckBox.Checked = managerConfig.forceNonSteam;
                if (forceNonSteamCheckBox.Checked) runningUnderSteamLabel.Text = "Forced Standalone";
                else runningUnderSteamLabel.Text = "Steam";
                forceNonSteamCheckBox.Enabled = true;
            }

            if (!loaderHandler.bepinUtilsInstalled)
            {
                splashInstalledOkLabel.Text = "Not installed!";
                reinstallSplashButton.Enabled = loaderHandler.bepinInstalled;
            }
            else splashInstalledOkLabel.Text = "Installed!";

            melonButton.Enabled = loaderHandler.bepinInstalled;
            //modUpdateCheckBox.Enabled = loaderHandler.bepinInstalled;

            managerAutoUpdateCheckBox.Checked = managerConfig.autoUpdateManager;
            bepinUpdateCheckbox.Checked = managerConfig.autoUpdateBepin;
            modUpdateCheckBox.Checked = managerConfig.autoUpdateMods;

            fp2libAutoUpdateCheckBox.Checked = managerConfig.autoUpdateFP2Lib;
            fp2libAutoUpdateCheckBox.Enabled = loaderHandler.fp2libInstalled;
            fp2libVersionLabel.Text = loaderHandler.fp2libVersion;

            customLaunchParamCheckBox.Checked = managerConfig.enableLaunchParams;
            LaunchParamsTextBox.Enabled = managerConfig.enableLaunchParams;
            LaunchParamsTextBox.Text = managerConfig.launchParams;

        }


        private void handleGBUri(string uri)
        {
            UrlType type;
            try
            {
                string[] gblink = uri.Replace("fp2mm://", string.Empty).Replace("fp2mm:", string.Empty).Split(',');
                if (gblink[0].Contains("gamebanana.com")) type = UrlType.GBANANA;
                else if (gblink[0].Contains("github.com")) type = UrlType.GITHUB;
                else type = UrlType.GENERIC;
                //Patch for random GB issue generating link
                if (type == UrlType.GBANANA)
                {
                    gblink[0] = gblink[0].Replace("https//", "https://");
                }

                modDownloadWindow(gblink, type);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private async void OneClickServer()
        {
            try
            {
                using (var pipeServer = new NamedPipeServerStream(Program.pipeName, PipeDirection.In))
                {
                    await pipeServer.WaitForConnectionAsync();

                    using (var sr = new StreamReader(pipeServer))
                    {
                        string temp;
                        while ((temp = sr.ReadLine()) != null)
                        {
                            Console.WriteLine("Received from client: {0}", temp);
                            handleGBUri(temp);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            OneClickServer();
        }

        private async Task modDownloadWindow(string[] uri, UrlType type)
        {
            string name = "Unknown", author = "Unknown", gBananFileName = "", gitHubFileName = "";
            DialogResult dialogResult;
            if (type == UrlType.GBANANA && uri.Length == 3)
            {
                string apiCall = string.Format("https://api.gamebanana.com/Core/Item/Data?itemid={0}&itemtype={1}&fields=name,Owner().name,Files().aFiles()", uri[2], uri[1]);

                try
                {
                    using (WebClient client = new WebClient())
                    {
                        string response = await client.DownloadStringTaskAsync(new Uri(apiCall));
                        using (JsonDocument document = JsonDocument.Parse(response))
                        {
                            if (document.RootElement.GetType().Equals(typeof(JsonObject)))
                            {
                                MessageBox.Show("Gamebanana api request returned an error:\n" + document.RootElement.GetProperty("error").GetString());
                                return;
                            }
                            JsonElement jName = document.RootElement[0];
                            name = jName.GetString();
                            JsonElement jAuthor = document.RootElement[1];
                            author = jAuthor.GetString();

                            string fileID = Regex.Match(uri[0], @"\d+").Value;

                            JsonElement jFiles = document.RootElement[2];
                            JsonElement jCurrFile = jFiles.GetProperty(fileID);
                            gBananFileName = jCurrFile.GetProperty("_sFile").GetString();

                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return;
                }
                //Cursed way to display window topmost - create a new form and make it a parent of the messagebox. Microsoft, why?
                using (Form tempform = new Form { TopMost = true })
                    dialogResult = MessageBox.Show(tempform, "Do you want to install \"" + name + "\" by: " + author + " from GameBanana?", "Mod installation", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

            }
            else if (type == UrlType.GITHUB)
            {
                //TODO: Replace this mess - use just repo and author name like updates do.
                try
                {
                    //Direct link to release
                    MatchCollection matches = Regex.Matches(uri[0], "(?:\\w*:\\/\\/github.com\\/)([\\w\\d]*)(?:\\/)([\\w\\d]*)(?:\\/[\\w\\d]*\\/[\\w\\d]*\\/[\\w\\d\\W][^\\/]*\\/)(\\S*)");
                    if (matches.Count > 0)
                    {
                        //Regex found a thingie
                        author = matches[0].Groups[1].Value;
                        name = matches[0].Groups[2].Value;
                        gitHubFileName = matches[0].Groups[3].Value;
                    }
                    else
                    {
                        MessageBox.Show("Invalid link.");
                        return;
                    }
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    //Regexes failed
                    Console.WriteLine(ex.Message);
                    return;
                }
                using (Form tempform = new Form { TopMost = true })
                    dialogResult = MessageBox.Show(tempform, "Do you want to install \"" + name + "\" by: " + author + " from GitHub?", "Mod installation", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            }
            else
            {
                if (uri.Length == 3)
                {
                    name = uri[1];
                    author = uri[2];
                }
                using (Form tempform = new Form { TopMost = true })
                    dialogResult = MessageBox.Show(tempform, "Do you want to install \"" + name + "\",  by: " + author + " from external site?", "Mod installation", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            }

            if (dialogResult == DialogResult.Yes)
            {
                Uri link;
                try { link = new Uri(uri[0]); }
                catch (UriFormatException)
                {
                    MessageBox.Show("The mod download link seems broken :( \nThere might be some issue on GameBanana side of things.", "Mod Download", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                switch (type)
                {
                    case UrlType.GBANANA:
                        await AsyncModDownloadGbanana(link, gBananFileName);
                        break;
                    case UrlType.GITHUB:
                        await AsyncModDownloadGitHub(link, gitHubFileName);
                        break;
                    default:
                        MessageBox.Show("Link points at unsupported service.", "Mod Download", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                }
            }
        }

        private async Task CheckForUpdatesAsync(bool hideNoUpdates)
        {
            try
            {
                var check = await _updateManager.CheckForUpdatesAsync();

                if (!check.CanUpdate)
                {
                    if (!hideNoUpdates) MessageBox.Show("There are no new Freedom Manager updates available.", "Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                using (ManagerUpdateInfoForm updateForm = new ManagerUpdateInfoForm())
                {
                    updateForm.loadFp2mmChangelog();
                    DialogResult dialogResult = updateForm.ShowDialog();

                    if (dialogResult == DialogResult.Yes)
                    {
                        bepinConfig.writeConfig();
                        managerConfig.writeConfig();
                        fP2LibConfig.writeConfig();
                        await _updateManager.PrepareUpdateAsync(check.LastVersion);

                        _updateManager.LaunchUpdater(check.LastVersion, true, "--post-update");
                        Application.Exit();
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex);
                if (!hideNoUpdates) MessageBox.Show("Failed downloading update information.", "Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private async Task checkForFP2LibUpdatesAsync(bool hideNoUpdates)
        {
            using (WebClient client = new WebClient())
            {
                client.Headers["Accept"] = "application/vnd.github+json";
                client.Headers["X-GitHub-Api-Version"] = "2022-11-28";
                client.Headers["user-agent"] = "FreedomManager";
                try
                {
                    string response = await client.DownloadStringTaskAsync(new Uri("https://api.github.com/repos/Kuborros/FP2Lib/releases/latest"));

                    GitHubRelease release = JsonSerializer.Deserialize<GitHubRelease>(response);

                    string localVersion = "0.0.0";
                    if (loaderHandler.fp2libInstalled)
                    {
                        localVersion = loaderHandler.fp2libVersion;
                    }
                    Version local = new Version(localVersion);

                    string remoteVersion = release.tag_name.Split('-')[0];
                    Version remote = new Version(remoteVersion);

                    if (remote > local)
                    {
                        using (ManagerUpdateInfoForm updateForm = new ManagerUpdateInfoForm())
                        {
                            updateForm.loadFp2libChangelog();
                            DialogResult dialogResult = updateForm.ShowDialog();

                            if (dialogResult == DialogResult.Yes)
                                await AsyncModDownloadGitHub(new Uri(release.downloadUrl), release.filename);
                        }
                    }
                    else
                    {
                        if (!hideNoUpdates) MessageBox.Show("There are no new FP2Lib updates available.", "Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        private async Task checkForBepInExUpdatesAsync(bool hideNoUpdates)
        {
            using (WebClient client = new WebClient())
            {
                client.Headers["Accept"] = "application/vnd.github+json";
                client.Headers["X-GitHub-Api-Version"] = "2022-11-28";
                client.Headers["user-agent"] = "FreedomManager";
                try
                {
                    string response = await client.DownloadStringTaskAsync(LoaderHandler.latestStableBepInEx5);

                    GitHubRelease release = JsonSerializer.Deserialize<GitHubRelease>(response);

                    string localVersion = "0.0.0";
                    if (loaderHandler.bepinInstalled)
                    {
                        localVersion = loaderHandler.bepinVersion.TrimStart('v');
                    }
                    Version local = new Version(localVersion);

                    string remoteVersion = release.tag_name.Split('-')[0].TrimStart('v');
                    Version remote = new Version(remoteVersion);

                    if (remote > local)
                    {
                        using (ManagerUpdateInfoForm updateForm = new ManagerUpdateInfoForm())
                        {
                            updateForm.loadBepInExChangelog();
                            DialogResult dialogResult = updateForm.ShowDialog();

                            if (dialogResult == DialogResult.Yes)
                                await AsyncModDownloadGitHub(LoaderHandler.latestStableBepInEx5File, "BepInEx_win_x86_5.4.23.2.zip");
                        }
                    }
                    else
                    {
                        if (!hideNoUpdates) MessageBox.Show("There are no new BepInEx5 updates available.", "Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        private async Task checkForModUpdatesAsync(bool showOnNoUpdates)
        {
            modUpdates = new List<ModUpdateInfo>();
            modUpdates = await modUpdateHandler.getModsUpdates(modHandler.modList);
            if (modUpdates.Count > 0)
            {
                using (ModsUpdateInfoForm modUpdateForm = new ModsUpdateInfoForm(modUpdates))
                {
                    modUpdateForm.updateSelectedButton.Click += new EventHandler(modUpdateInstall_Click);
                    modUpdateForm.ShowDialog();
                }
            }
            else if (showOnNoUpdates)
            {
                MessageBox.Show(this, "No new updates found",
                "Mod Updater", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        internal async Task AsyncModDownloadGbanana(Uri url, string filename)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Headers["user-agent"] = "FreedomManager";
                    client.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
                    tempname = filename;

                    using (DownloadProgress progress = new DownloadProgress(filename))
                    {
                        client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(progress.client_DownloadProgressChanged);
                        client.DownloadFileCompleted += new AsyncCompletedEventHandler(progress.client_DownloadFileCompleted);
                        client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
                        progress.Show();
                        await client.DownloadFileTaskAsync(url, filename);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Download failed!\n\n" +
                "Error info: " + ex.Message,
                "", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        internal async Task AsyncModDownloadGitHub(Uri url, string filename)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Headers["user-agent"] = "FreedomManager";
                    client.Headers["accept"] = "*/*";
                    client.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
                    tempname = filename;

                    using (DownloadProgress progress = new DownloadProgress(filename))
                    {
                        if (filename == "fp2lib.zip") progress.Text = "FP2Lib Update";
                        client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(progress.client_DownloadProgressChanged);
                        client.DownloadFileCompleted += new AsyncCompletedEventHandler(progress.client_DownloadFileCompleted);
                        client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
                        progress.Show();
                        await client.DownloadFileTaskAsync(url, filename);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Download failed!\n\n" +
                "Error info: " + ex.Message,
                "", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        private void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (modHandler.InstallMod(tempname, true))
            {
                RenderList();
                loaderHandler.checkFP2Lib();
                updateConfigUi();
            }
            else
            {
                MessageBox.Show("Mod unpacking failed!\n\n",
                Text, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        private void RenderList()
        {
            modHandler.UpdateModList();
            RenderList(modHandler.modList);
        }

        private void RenderList(List<ModInfo> modInfos)
        {
            listView1.Items.Clear();
            listView1.FocusedItem = null;
            foreach (ModInfo modInfo in modInfos)
            {
                ListViewItem item = new ListViewItem
                {
                    Tag = modInfo,
                    Text = modInfo.Name
                };
                item.SubItems.Add(modInfo.Version);
                item.SubItems.Add(modInfo.Author);
                item.SubItems.Add(modInfo.Loader);
                item.Checked = modInfo.Enabled;
                listView1.Items.Add(item);
            }
        }

        private void exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void savePlay_Click(object sender, EventArgs e)
        {
            saveButton_Click(sender, e);

            string parameters = "";
            if (customLaunchParamCheckBox.Checked)
            {
                parameters = LaunchParamsTextBox.Text;
            }
            //Launch trough steam if detected, otherwise run game exe directly for Itch.io users
            if (fp2Found && !loaderHandler.runningUnderSteam) Process.Start("FP2.exe", parameters);
            else if (fp2Found) Process.Start("explorer", "steam://rungameid/595500");
        }

        private void setup_Click(object sender, EventArgs e)
        {
            try
            {
                if (loaderHandler.installBepinLoader())
                {
                    MessageBox.Show(this, "BepInEx installed!.\n\n" +
                    "The game is now ready for modding.",
                    Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    setup.Text = "Uninstall BepInEx";
                    managerConfig.bepinexVersion = loaderHandler.bepinVersion;
                    bepinVersionLabel.Text = loaderHandler.bepinVersion;
                }
                else
                {
                    MessageBox.Show(this, "BepInEx hook removed!.\n\n" +
                    "Mods will no longer be loaded.",
                    Text, MessageBoxButtons.OK, MessageBoxIcon.Information);

                    setup.Text = "Install BepInEx";
                    managerConfig.bepinexVersion = "";
                    bepinVersionLabel.Text = "N/A";

                }
                RenderList();
                if (loaderHandler.bepinInstalled)
                    checkForFP2LibUpdatesAsync(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Unpacking BepInEx failed!.\n\n" +
                "Error info: " + ex.Message,
                Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            updateConfigUi();
        }

        private void modInstall_Click(object sender, EventArgs e)
        {
            if (modFileDialog.ShowDialog() == DialogResult.OK)
            {
                string file = modFileDialog.FileName;
                modHandler.InstallMod(file, false);
                RenderList();
            }
        }

        private void refresh_Click(object sender, EventArgs e)
        {
            RenderList();
            updateConfigUi();
        }
        private void melonButton_Click(object sender, EventArgs e)
        {

            DialogResult dialogResult = MessageBox.Show("Important!\n\n" +
                "MelonLoader compat is meant only to run mods requiring it.\n" +
                "You do NOT need it, unless you want to use these specific mods.\n" +
                "Bug reports for BepInEx mods where MelonLoader is installed will be ignored!\n\n" +
                "Do you still want to proceed?", "MelonLoader Installation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    if (loaderHandler.installMLLoader())
                    {
                        MessageBox.Show(this, "MelonLoader plugin installed!\n\n" +
                        "Melon Loader mods can now be installed. Please be aware that MelonLoader can be heavy on the game.",
                        Text, MessageBoxButtons.OK, MessageBoxIcon.Information);

                        melonPresent = true;
                        RenderList();
                        melonButton.Text = "Uninstall MelonLoader Compat";
                    }
                    else
                    {
                        MessageBox.Show(this, "MelonLoader plugin uninstalled!\n\n" +
                        "",
                        Text, MessageBoxButtons.OK, MessageBoxIcon.Information);

                        melonPresent = false;
                        melonButton.Text = "Install MelonLoader Compat";
                        RenderList();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Unpacking MelonLoader failed!.\n\n" +
                    "Error info: " + ex.Message,
                    Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                updateConfigUi();
            }
        }

        private void handlerButton_Click(object sender, EventArgs e)
        {
            RegisterGameBananaProtocol(true); //Separate method in case we want to run it ourselves from somewhere else.
        }

        void RegisterGameBananaProtocol(bool deleteIfPresent)
        {
            using (var current = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Classes\\" + "fp2mm"))
            {
                if (current == null)
                {
                    using (var key = Registry.CurrentUser.CreateSubKey("SOFTWARE\\Classes\\" + "fp2mm"))
                    {
                        string applicationLocation = typeof(FreedomManager).Assembly.Location;

                        Console.WriteLine(applicationLocation);

                        key.SetValue("", "URL: FreedomLoader");
                        key.SetValue("URL Protocol", "");

                        using (var defaultIcon = key.CreateSubKey("DefaultIcon"))
                        {
                            defaultIcon.SetValue("", applicationLocation + ",1");
                        }

                        using (var commandKey = key.CreateSubKey(@"shell\open\command"))
                        {
                            commandKey.SetValue("", "\"" + applicationLocation + "\" \"%1\"");
                        }
                    }
                    MessageBox.Show("URL handler registered!.\n\n" +
                    "Gamebanana 1-Click install is now available.",
                     "URL Handler", MessageBoxButtons.OK);

                    handlerButton.Text = "Unregister URL handler";
                }
                else if (deleteIfPresent)
                {
                    Registry.CurrentUser.DeleteSubKeyTree("SOFTWARE\\Classes\\" + "fp2mm");
                    MessageBox.Show("URL handler de-registered!.\n\n" +
                    "Gamebanana 1-Click support has been uninstalled.",
                    "URL Handler", MessageBoxButtons.OK);

                    handlerButton.Text = "Register URL handler";
                }
            }
        }
        private void listView1_NodeMouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ListViewHitTestInfo listViewHitTestInfo = listView1.HitTest(e.X, e.Y);
                if (listViewHitTestInfo.Item != null)
                {
                    columnIndex = listViewHitTestInfo.Item.Index;
                    contextMenuStrip1.Show(listView1, e.Location);
                }
            }
        }

        private void FM_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void FM_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                try
                {
                    modHandler.InstallMod(files[0], false);
                    RenderList();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private void seeOnGameBananaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ModInfo modInfo = (ModInfo)listView1.Items[columnIndex].Tag;
            if (modInfo.GitHub != null && modInfo.GitHub != "" && modInfo.GitHub.Contains("http"))
            {
                Process.Start("explorer", Uri.EscapeUriString(modInfo.GitHub));
            }
        }

        private void uninstallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ModInfo modInfo = (ModInfo)listView1.Items[columnIndex].Tag;
            modHandler.UnInstallMod(modInfo);
            RenderList();
        }

        private void gitHubWikiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("explorer", "https://github.com/Kuborros/FreedomManager/wiki");
        }

        private void gameBananaPageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("explorer", "https://gamebanana.com/tools/10870");
        }

        private void listView1_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            listView1.BeginUpdate();
            if (listView1.FocusedItem != null) //Prevents the initial checking of every item from firing an event
            {
                ModInfo info = (ModInfo)e.Item.Tag;
                if (info.Type == ModType.JSONNPC || info.Type == ModType.STAGE || info.Type == ModType.SPECIAL || info.InternalMod)
                {
                    //These types cannot be disabled. Winforms UI does not let you show it nicely without custom drawing, so we just force the option always on.
                    e.Item.Checked = true;
                    listView1.EndUpdate();
                    return;
                }
                e.Item.Checked = modHandler.EnableDisableMod(info);
                RenderList();
            }
            listView1.EndUpdate();
        }

        private void openFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ModInfo modInfo = (ModInfo)listView1.Items[columnIndex].Tag;
            if (modInfo != null)
            {
                string path = "";
                if (modInfo.Type == ModType.BEPINMOD) //Bepin mod
                {
                    if (modInfo.Enabled)
                        path = "BepInEx\\plugins\\" + modInfo.Dirname;
                    else
                        path = "BepInEx\\plugins-disabled\\" + modInfo.Dirname;
                }
                else if (modInfo.Type == ModType.BEPINDLL) //Loose DLL bepin
                {
                    if (modInfo.Enabled)
                        path = "BepInEx\\plugins";
                    else
                        path = "BepInEx\\plugins-disabled";
                }
                else if (modInfo.Type == ModType.JSONNPC) //JSON NPC
                {
                    path = "BepInEx\\config\\NPCLibEzNPC";
                }
                else if (melonPresent)
                {
                    if (modInfo.Type == ModType.MELONMOD) //Melon mod
                    {
                        if (modInfo.Enabled)
                            path = "MLLoader\\mods";
                        else
                            path = "MLLoader\\mods-disabled";
                    }

                }
                Process.Start("explorer", Path.Combine(Path.GetFullPath("."), path));
            }
        }

        private void bepInExToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(Path.Combine(Path.GetFullPath("."), "BepInEx\\plugins")))
                Process.Start("explorer", Path.Combine(Path.GetFullPath("."), "BepInEx\\plugins"));
        }

        private void melonLoaderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(Path.Combine(Path.GetFullPath("."), "MLLoader\\mods")))
                Process.Start("explorer", Path.Combine(Path.GetFullPath("."), "MLLoader\\mods"));
        }

        private void installModToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (modFileDialog.ShowDialog() == DialogResult.OK)
            {
                string file = modFileDialog.FileName;
                modHandler.InstallMod(file, false);
                RenderList();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            ModInfo modInfo = (ModInfo)listView1.Items[columnIndex].Tag;
            if (modInfo.GitHub == "" || modInfo.GitHub == null)
            {
                contextMenuStrip1.Items[1].Enabled = false;
            }
            e.Cancel = false;
        }

        private void enableConsoleCheckBox_CheckedChanged_1(object sender, EventArgs e)
        {
            bepinConfig.ShowConsole = enableConsoleCheckBox.Checked;
        }

        private void noConsoleCloseCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            bepinConfig.ConsolePreventClose = noConsoleCloseCheckBox.Checked;
        }

        private void hideLogsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            bepinConfig.UnityLogListening = hideLogsCheckBox.Checked;
        }

        private void logfileCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            bepinConfig.FileLog = logfileCheckBox.Checked;
        }

        private void unityFileCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            bepinConfig.WriteUnityLog = unityFileCheckBox.Checked;
        }

        private void appendLogCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            bepinConfig.AppendLog = appendLogCheckBox.Checked;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            managerConfig.launchParams = LaunchParamsTextBox.Text;
            bepinConfig.LogLevels = logLevelTextBox.Text;
            bepinConfig.HarmonyLogLevels = harmonyLogTextBox.Text;

            bepinConfig.writeConfig();
            doorstopConfig.writeConfig();
            managerConfig.writeConfig();
            fP2LibConfig.writeConfig();

            updateConfigUi();
        }

        private async void updateCheckButton_Click(object sender, EventArgs e)
        {
            await Task.Run(() => CheckForUpdatesAsync(false));
            await checkForFP2LibUpdatesAsync(true);
        }

        private void fp2libAutoUpdateCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            managerConfig.autoUpdateFP2Lib = fp2libAutoUpdateCheckBox.Checked;
        }

        private void managerAutoUpdateCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            managerConfig.autoUpdateManager = managerAutoUpdateCheckBox.Checked;
        }

        private void fancyJsonCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            fP2LibConfig.saveFancyJson = fancyJsonCheckBox.Checked;
        }

        private void saveRedirecCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            fP2LibConfig.saveRedirectEnabled = saveRedirecCheckBox.Checked;
        }

        private void saveProfileComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            fP2LibConfig.saveRedirectProfile = saveProfileComboBox.SelectedIndex;
        }

        private void openLogfileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (File.Exists(Path.Combine(Path.GetFullPath("."), "BepInEx\\LogOutput.log")))
                Process.Start("explorer", Path.Combine(Path.GetFullPath("."), "BepInEx\\LogOutput.log"));
        }

        private async void checkForModUpdatesButton_Click(object sender, EventArgs e)
        {
            await checkForModUpdatesAsync(true);
        }

        internal async void modUpdateInstall_Click(object sender, EventArgs e)
        {
            foreach (ModUpdateInfo modUpdate in modUpdates)
            {
                if (modUpdate.DoUpdate)
                {
                    await AsyncModDownloadGitHub(new Uri(modUpdate.DownloadLink), "modUpdate.zip");
                }
            }

            Button butt = (Button)sender;
            ModsUpdateInfoForm form = (ModsUpdateInfoForm)butt.Parent;

            MessageBox.Show("Mod updates installed!",
                "Mod Updater", MessageBoxButtons.OK, MessageBoxIcon.Information);

            form.Close();
            RenderList();
        }

        private void modUpdateCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            managerConfig.autoUpdateMods = modUpdateCheckBox.Checked;
        }

        private void customLaunchParamCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            managerConfig.enableLaunchParams = customLaunchParamCheckBox.Checked;
            LaunchParamsTextBox.Enabled = customLaunchParamCheckBox.Checked;
        }

        private void splashEnableCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            bepinConfig.SplashEnabled = splashEnableCheckBox.Checked;
        }

        private void splashWithConsoleCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            bepinConfig.OnlyNoConsole = !splashWithConsoleCheckBox.Checked;
        }

        private void forceNonSteamCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            loaderHandler.runningUnderSteam = !forceNonSteamCheckBox.Checked;
            managerConfig.forceNonSteam = forceNonSteamCheckBox.Checked;
            if (forceNonSteamCheckBox.Checked) runningUnderSteamLabel.Text = "Forced Standalone";
            else runningUnderSteamLabel.Text = "Steam";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            loaderHandler.installBepinUtils(true);
        }

        private void doorstopFileLogCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            doorstopConfig.RedirectOutputLog = doorstopFileLogCheckBox.Checked;
        }

        private void disableMultiFolderBox_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private async void bepinUpdateButton_Click(object sender, EventArgs e)
        {
            await checkForBepInExUpdatesAsync(false);
        }

        private void bepinUpdateCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            managerConfig.autoUpdateBepin = bepinUpdateCheckbox.Checked;
        }
    }
}


