﻿namespace FreedomManager
{
    partial class FreedomManager
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FreedomManager));
            this.savePlay = new System.Windows.Forms.Button();
            this.setup = new System.Windows.Forms.Button();
            this.exit = new System.Windows.Forms.Button();
            this.modInstall = new System.Windows.Forms.Button();
            this.modFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.refresh = new System.Windows.Forms.Button();
            this.melonButton = new System.Windows.Forms.Button();
            this.handlerButton = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.infoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.seeOnGameBananaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uninstallToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openModsFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bepInExToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.melonLoaderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.installModToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enableConsoleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gitHubWikiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gameBananaPageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkForUpdatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.contextMenuStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // savePlay
            // 
            this.savePlay.Location = new System.Drawing.Point(24, 554);
            this.savePlay.Name = "savePlay";
            this.savePlay.Size = new System.Drawing.Size(119, 23);
            this.savePlay.TabIndex = 1;
            this.savePlay.Text = "Play";
            this.savePlay.UseVisualStyleBackColor = true;
            this.savePlay.Click += new System.EventHandler(this.savePlay_Click);
            // 
            // setup
            // 
            this.setup.Location = new System.Drawing.Point(133, 419);
            this.setup.Name = "setup";
            this.setup.Size = new System.Drawing.Size(256, 70);
            this.setup.TabIndex = 2;
            this.setup.Text = "Install BepInEx";
            this.setup.UseVisualStyleBackColor = true;
            this.setup.Click += new System.EventHandler(this.setup_Click);
            // 
            // exit
            // 
            this.exit.Location = new System.Drawing.Point(411, 554);
            this.exit.Name = "exit";
            this.exit.Size = new System.Drawing.Size(78, 23);
            this.exit.TabIndex = 3;
            this.exit.Text = "Exit";
            this.exit.UseVisualStyleBackColor = true;
            this.exit.Click += new System.EventHandler(this.exit_Click);
            // 
            // modInstall
            // 
            this.modInstall.Location = new System.Drawing.Point(8, 457);
            this.modInstall.Name = "modInstall";
            this.modInstall.Size = new System.Drawing.Size(119, 32);
            this.modInstall.TabIndex = 4;
            this.modInstall.Text = "Install Mod";
            this.modInstall.UseVisualStyleBackColor = true;
            this.modInstall.Click += new System.EventHandler(this.modInstall_Click);
            // 
            // modFileDialog
            // 
            this.modFileDialog.DefaultExt = "zip";
            this.modFileDialog.Title = "Select mod to install.";
            // 
            // refresh
            // 
            this.refresh.Location = new System.Drawing.Point(8, 419);
            this.refresh.Name = "refresh";
            this.refresh.Size = new System.Drawing.Size(119, 32);
            this.refresh.TabIndex = 6;
            this.refresh.Text = "Refresh";
            this.refresh.UseVisualStyleBackColor = true;
            this.refresh.Click += new System.EventHandler(this.refresh_Click);
            // 
            // melonButton
            // 
            this.melonButton.Location = new System.Drawing.Point(395, 419);
            this.melonButton.Name = "melonButton";
            this.melonButton.Size = new System.Drawing.Size(78, 70);
            this.melonButton.TabIndex = 7;
            this.melonButton.Text = "Install MelonLoader Compat";
            this.melonButton.UseVisualStyleBackColor = true;
            this.melonButton.Click += new System.EventHandler(this.melonButton_Click);
            // 
            // handlerButton
            // 
            this.handlerButton.Location = new System.Drawing.Point(149, 554);
            this.handlerButton.Name = "handlerButton";
            this.handlerButton.Size = new System.Drawing.Size(256, 23);
            this.handlerButton.TabIndex = 8;
            this.handlerButton.Text = "Register URL handler";
            this.handlerButton.UseVisualStyleBackColor = true;
            this.handlerButton.Click += new System.EventHandler(this.handlerButton_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openFolderToolStripMenuItem,
            this.infoToolStripMenuItem,
            this.seeOnGameBananaToolStripMenuItem,
            this.uninstallToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(183, 92);
            // 
            // openFolderToolStripMenuItem
            // 
            this.openFolderToolStripMenuItem.Name = "openFolderToolStripMenuItem";
            this.openFolderToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.openFolderToolStripMenuItem.Text = "Open folder";
            this.openFolderToolStripMenuItem.Click += new System.EventHandler(this.openFolderToolStripMenuItem_Click);
            // 
            // infoToolStripMenuItem
            // 
            this.infoToolStripMenuItem.Name = "infoToolStripMenuItem";
            this.infoToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.infoToolStripMenuItem.Text = "Info";
            this.infoToolStripMenuItem.Click += new System.EventHandler(this.infoToolStripMenuItem_Click);
            // 
            // seeOnGameBananaToolStripMenuItem
            // 
            this.seeOnGameBananaToolStripMenuItem.Name = "seeOnGameBananaToolStripMenuItem";
            this.seeOnGameBananaToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.seeOnGameBananaToolStripMenuItem.Text = "See on GameBanana";
            this.seeOnGameBananaToolStripMenuItem.Click += new System.EventHandler(this.seeOnGameBananaToolStripMenuItem_Click);
            // 
            // uninstallToolStripMenuItem
            // 
            this.uninstallToolStripMenuItem.Name = "uninstallToolStripMenuItem";
            this.uninstallToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.uninstallToolStripMenuItem.Text = "Uninstall";
            this.uninstallToolStripMenuItem.Click += new System.EventHandler(this.uninstallToolStripMenuItem_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(506, 24);
            this.menuStrip1.TabIndex = 9;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openModsFolderToolStripMenuItem,
            this.installModToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openModsFolderToolStripMenuItem
            // 
            this.openModsFolderToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bepInExToolStripMenuItem,
            this.melonLoaderToolStripMenuItem});
            this.openModsFolderToolStripMenuItem.Name = "openModsFolderToolStripMenuItem";
            this.openModsFolderToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.openModsFolderToolStripMenuItem.Text = "Open Mods Folder";
            // 
            // bepInExToolStripMenuItem
            // 
            this.bepInExToolStripMenuItem.Name = "bepInExToolStripMenuItem";
            this.bepInExToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.bepInExToolStripMenuItem.Text = "BepInEx";
            this.bepInExToolStripMenuItem.Click += new System.EventHandler(this.bepInExToolStripMenuItem_Click);
            // 
            // melonLoaderToolStripMenuItem
            // 
            this.melonLoaderToolStripMenuItem.Name = "melonLoaderToolStripMenuItem";
            this.melonLoaderToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.melonLoaderToolStripMenuItem.Text = "MelonLoader";
            this.melonLoaderToolStripMenuItem.Click += new System.EventHandler(this.melonLoaderToolStripMenuItem_Click);
            // 
            // installModToolStripMenuItem
            // 
            this.installModToolStripMenuItem.Name = "installModToolStripMenuItem";
            this.installModToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.installModToolStripMenuItem.Text = "Install Mod";
            this.installModToolStripMenuItem.Click += new System.EventHandler(this.installModToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.enableConsoleToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // enableConsoleToolStripMenuItem
            // 
            this.enableConsoleToolStripMenuItem.Name = "enableConsoleToolStripMenuItem";
            this.enableConsoleToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.enableConsoleToolStripMenuItem.Text = "Enable Console";
            this.enableConsoleToolStripMenuItem.Click += new System.EventHandler(this.enableConsoleToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.gitHubWikiToolStripMenuItem,
            this.gameBananaPageToolStripMenuItem,
            this.checkForUpdatesToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // gitHubWikiToolStripMenuItem
            // 
            this.gitHubWikiToolStripMenuItem.Name = "gitHubWikiToolStripMenuItem";
            this.gitHubWikiToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.gitHubWikiToolStripMenuItem.Text = "GitHub Wiki";
            this.gitHubWikiToolStripMenuItem.Click += new System.EventHandler(this.gitHubWikiToolStripMenuItem_Click);
            // 
            // gameBananaPageToolStripMenuItem
            // 
            this.gameBananaPageToolStripMenuItem.Name = "gameBananaPageToolStripMenuItem";
            this.gameBananaPageToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.gameBananaPageToolStripMenuItem.Text = "GameBanana Page";
            this.gameBananaPageToolStripMenuItem.Click += new System.EventHandler(this.gameBananaPageToolStripMenuItem_Click);
            // 
            // checkForUpdatesToolStripMenuItem
            // 
            this.checkForUpdatesToolStripMenuItem.Name = "checkForUpdatesToolStripMenuItem";
            this.checkForUpdatesToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.checkForUpdatesToolStripMenuItem.Text = "Check for Updates";
            // 
            // listView1
            // 
            this.listView1.CheckBoxes = true;
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.listView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(8, 6);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(465, 407);
            this.listView1.Sorting = System.Windows.Forms.SortOrder.Descending;
            this.listView1.TabIndex = 10;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listView1_ItemChecked);
            this.listView1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listView1_NodeMouseClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 177;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Version";
            this.columnHeader2.Width = 71;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Author";
            this.columnHeader3.Width = 132;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Loader";
            this.columnHeader4.Width = 80;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 27);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(482, 521);
            this.tabControl1.TabIndex = 11;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.listView1);
            this.tabPage1.Controls.Add(this.modInstall);
            this.tabPage1.Controls.Add(this.refresh);
            this.tabPage1.Controls.Add(this.setup);
            this.tabPage1.Controls.Add(this.melonButton);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(474, 495);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Mods";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox3);
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(474, 495);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Configuration";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(8, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(460, 104);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // groupBox2
            // 
            this.groupBox2.Location = new System.Drawing.Point(8, 116);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(460, 113);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "groupBox2";
            // 
            // groupBox3
            // 
            this.groupBox3.Location = new System.Drawing.Point(8, 235);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(460, 100);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "groupBox3";
            // 
            // FreedomManager
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(506, 589);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.handlerButton);
            this.Controls.Add(this.exit);
            this.Controls.Add(this.savePlay);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FreedomManager";
            this.Text = "FreedomManager";
            this.contextMenuStrip1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button savePlay;
        private System.Windows.Forms.Button setup;
        private System.Windows.Forms.Button exit;
        private System.Windows.Forms.Button modInstall;
        private System.Windows.Forms.OpenFileDialog modFileDialog;
        private System.Windows.Forms.Button refresh;
        private System.Windows.Forms.Button melonButton;
        private System.Windows.Forms.Button handlerButton;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem infoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uninstallToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ToolStripMenuItem gitHubWikiToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gameBananaPageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem enableConsoleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openModsFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bepInExToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem melonLoaderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem installModToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem seeOnGameBananaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkForUpdatesToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}
