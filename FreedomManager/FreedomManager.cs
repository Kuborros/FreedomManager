using FreedomManager.Config;
using FreedomManager.Mod;
using FreedomManager.Net.GitHub;
using FreedomManager.Patches;
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
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static FreedomManager.Mod.ModHandler;

namespace FreedomManager
{
    public partial class FreedomManager : Form
    {
        readonly bool bepisPresent = false;
        readonly bool fp2Found = false;
        bool melonPresent = false;
        int columnIndex = 0;
        internal string tempname;

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
        static FP2LibConfig fP2LibConfig;
        static ManagerConfig managerConfig;
        static ResolutionPatchController resolutionPatchController;
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
                MessageBox.Show("Freedom Planet 2 not Found!.\n\n" +
                "Please ensure the mod manager is in the main game directory.",
                "",MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Environment.Exit(1);
            }

            if (fp2Found && !bepisPresent)
            {
                MessageBox.Show("BepInEx not Found!.\n\n" +
                        "Seems you dont have BepInEx installed - before you install any mods, install it by clicking on \"Install BepInEx\" button.",
                        "", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
            else setup.Text = "Uninstall BepInEx";

            if (melonPresent)
            {
                melonButton.Text = "Uninstall MelonLoader Compat";
            }

            using (var current = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Classes\\" + "fp2mm"))
            {
                if (current != null) { handlerButton.Text = "Unregister URL handler"; }
            }

            if (uris.Count > 0) handleGBUri(uris[0]);

            bepinConfig = new BepinConfig();
            fP2LibConfig = new FP2LibConfig();
            updateConfigUi();

            resolutionPatchController = new ResolutionPatchController();

            if (resolutionPatchController.enabled)
            {
                fp2resComboBox.SelectedIndex = Math.Min((int)resolutionPatchController.currentRes,9);
                fp2resCheckBox.Checked = true;
            }
            else
            {
                fp2resComboBox.SelectedIndex = 0;
                fp2resCheckBox.Checked = false;
            }

            RenderList(modHandler.modList);
            OneClickServer();

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

            managerVersionLabel.Text = Application.ProductVersion;

        }

        private void updateConfigUi()
        {
            fP2LibConfig = new FP2LibConfig();
            bepinConfig = new BepinConfig();

            if (bepinConfig.confExists)
            {
                enableConsoleCheckBox.Checked = bepinConfig.ShowConsole;
                noConsoleCloseCheckBox.Checked = bepinConfig.ConsolePreventClose;
                logfileCheckBox.Checked = bepinConfig.FileLog;
                hideLogsCheckBox.Checked = bepinConfig.UnityLogListening;
                unityFileCheckBox.Checked = bepinConfig.WriteUnityLog;
                appendLogCheckBox.Checked = bepinConfig.AppendLog;
            }
            else
            {
                bepinGroupBox.Enabled = false;
            }

            if (fP2LibConfig.configExists)
            {
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

            managerAutoUpdateCheckBox.Checked = managerConfig.autoUpdateManager;

            fp2libAutoUpdateCheckBox.Checked = managerConfig.autoUpdateFP2Lib;
            fp2libAutoUpdateCheckBox.Enabled = loaderHandler.fp2libInstalled;
            fp2libVersionLabel.Text = loaderHandler.fp2libVersion;
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

        private async void modDownloadWindow(string[] uri, UrlType type)
        {
            string name = "Unknown", author = "Unknown", gBananFileName = "";
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
                }
                dialogResult = MessageBox.Show("Do you want to install \"" + name + "\" by: " + author + " from GameBanana?", "Mod installation", MessageBoxButtons.YesNo, MessageBoxIcon.Question ,MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

            }
            else 
            {
                if (uri.Length == 3)
                {
                    name = uri[1];
                    author = uri[2];
                }

                string flavor = type == UrlType.GITHUB ? "GitHub" : "external site";

                dialogResult = MessageBox.Show("Do you want to install \"" + name + "\",  by: " + author + " from " + flavor + "?", "Mod installation", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }

            if (dialogResult == DialogResult.Yes)
            {
                switch (type) {
                    case UrlType.GBANANA:
                        AsyncModDownloadGbanana(new Uri(uri[0]), gBananFileName);
                        break;
                    case UrlType.GITHUB:
                        AsyncModDownloadGitHub(new Uri(uri[0]));
                        break;
                    default:
                        MessageBox.Show("Link points at unsupported service.", "Mod Download", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                }
            }
        }

        private async void CheckForUpdatesAsync(bool hideNoUpdates)
        {
            var check = await _updateManager.CheckForUpdatesAsync();

            if (!check.CanUpdate)
            {
                if (!hideNoUpdates) MessageBox.Show("There are no new Freedom Manager updates available.", "Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult dialogResult = MessageBox.Show("New Freedom Manager update is available!\n Version: " + check.LastVersion + "\n\n Would you like to install it now?", "Update", MessageBoxButtons.YesNo);

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

        private async void checkForFP2LibUpdatesAsync(bool hideNoUpdates)
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
                        DialogResult dialogResult = MessageBox.Show("New FP2Lib update is available!\n Version: " + release.tag_name + "\n\n Would you like to install it now?", "Update", MessageBoxButtons.YesNo);

                        if (dialogResult == DialogResult.Yes)
                            AsyncModDownloadGitHub(new Uri(release.downloadUrl));
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

        internal async void AsyncModDownloadGbanana(Uri url, string filename)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Headers["user-agent"] = "FreedomManager";
                    client.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
                    tempname = filename;

                    DownloadProgress progress = new DownloadProgress();
                    client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(progress.client_DownloadProgressChanged);
                    client.DownloadFileCompleted += new AsyncCompletedEventHandler(progress.client_DownloadFileCompleted);
                    client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
                    progress.Show();
                    await client.DownloadFileTaskAsync(url, filename);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Download failed!\n\n" +
                "Error info: " + ex.Message,
                Text, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                return;
            }
        }

        internal async void AsyncModDownloadGitHub(Uri url)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Headers["user-agent"] = "FreedomManager";
                    client.Headers["accept"] = "*/*";
                    client.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);

                    await client.OpenReadTaskAsync(url);
                    string filename = client.ResponseHeaders.Get("content-disposition").Split(';')[1].Trim().Replace("filename=", "");
                    tempname = filename;

                    DownloadProgress progress = new DownloadProgress
                    {
                        Text = "FP2Lib Update"
                    };

                    client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(progress.client_DownloadProgressChanged);
                    client.DownloadFileCompleted += new AsyncCompletedEventHandler(progress.client_DownloadFileCompleted);
                    client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
                    progress.Show();
                    await client.DownloadFileTaskAsync(url, filename);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Download failed!\n\n" +
                "Error info: " + ex.Message,
                Text, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                return;
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
            bepinConfig.writeConfig();
            managerConfig.writeConfig();
            fP2LibConfig.writeConfig();
            if (fp2Found) Process.Start("FP2.exe");
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
                }
                else
                {
                    MessageBox.Show(this, "BepInEx hook removed!.\n\n" +
                    "Mods will no longer be loaded.",
                    Text, MessageBoxButtons.OK, MessageBoxIcon.Information);

                    setup.Text = "Install BepInEx";

                }
                RenderList();
                checkForFP2LibUpdatesAsync(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Unpacking BepInEx failed!.\n\n" +
                "Error info: " + ex.Message,
                Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                "Do you still want to proceed?", "MelonLoader Installation", MessageBoxButtons.YesNo,MessageBoxIcon.Question);

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

        private void infoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ModInfo modInfo = (ModInfo)listView1.Items[columnIndex].Tag;

            StringBuilder builder = new StringBuilder();
            builder.Append("Name: ").AppendLine(modInfo.Name);
            builder.Append("Version: ").AppendLine(modInfo.Version);
            builder.Append("Author: ").AppendLine(modInfo.Author);
            builder.Append("Used Loader: ").AppendLine(modInfo.Loader);
            builder.Append("Uses extra files: ").AppendLine((bool)modInfo.HasAssets ? "Yes" : "No");

            MessageBox.Show(this, builder.ToString(), "Mod information", MessageBoxButtons.OK);
        }

        private void seeOnGameBananaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ModInfo modInfo = (ModInfo)listView1.Items[columnIndex].Tag;
            if (modInfo.GBID != null && modInfo.GBID != 0)
            {
                Process.Start("explorer", "https://gamebanana.com/mods/" + modInfo.GBID + "/");
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
                if (modInfo.ArchiveType == ArchiveType.BepinDir || modInfo.ArchiveType == ArchiveType.PluginDir) //Bepin mod
                {
                    if (modInfo.Enabled)
                        path = "BepInEx\\plugins\\" + modInfo.Dirname;
                    else
                        path = "BepInEx\\plugins-disabled\\" + modInfo.Dirname;
                }
                else if (modInfo.ArchiveType == ArchiveType.DllDir) //Loose DLL bepin
                {
                    if (modInfo.Enabled)
                        path = "BepInEx\\plugins";
                    else
                        path = "BepInEx\\plugins-disabled";
                }
                else if (melonPresent)
                {
                    if (modInfo.ArchiveType == ArchiveType.MelonDir) //Melon mod
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
            if (modInfo.GBID == 0 || modInfo.GBID == null)
            {
                contextMenuStrip1.Items[2].Enabled = false;
            }
            e.Cancel = false;
        }

        private void enableConsoleCheckBox_CheckedChanged(object sender, EventArgs e)
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
            bepinConfig.AppendLog = enableConsoleCheckBox.Checked;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            bepinConfig.writeConfig();
            managerConfig.writeConfig();
            fP2LibConfig.writeConfig();
        }

        private void fp2resCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (fp2resCheckBox.Checked)
            {
                resolutionPatchController.enabled = true;
                resolutionPatchController.setIntResolution((ResolutionPatchController.Resolution)fp2resComboBox.SelectedIndex);
            }
            else
            {
                resolutionPatchController.enabled = false;
                resolutionPatchController.setIntResolution(ResolutionPatchController.Resolution.x360);
            }
        }

        private void fp2resComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void resPatchButton_Click(object sender, EventArgs e)
        {
            if (resolutionPatchController.setIntResolution((ResolutionPatchController.Resolution)fp2resComboBox.SelectedIndex))
            {
                MessageBox.Show("Resolution patch successfull!",
                "Internal Resolution Patch", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Resolution patch failed!\n\n" +
                "Patch offset could not be located, did the game update recently?",
                "Internal Resolution Patch", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void updateCheckButton_Click(object sender, EventArgs e)
        {
            await Task.Run(() => CheckForUpdatesAsync(false));
            checkForFP2LibUpdatesAsync(false);
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
    }
}


