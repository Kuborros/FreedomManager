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
            this.openLogfileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.installModToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gitHubWikiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gameBananaPageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.customLaunchParamCheckBox = new System.Windows.Forms.CheckBox();
            this.LaunchParamsTextBox = new System.Windows.Forms.TextBox();
            this.checkForModUpdatesButton = new System.Windows.Forms.Button();
            this.updateCheckButton = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.modUpdateCheckBox = new System.Windows.Forms.CheckBox();
            this.fp2libVersionLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.managerVersionLabel = new System.Windows.Forms.Label();
            this.fp2libAutoUpdateCheckBox = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.managerAutoUpdateCheckBox = new System.Windows.Forms.CheckBox();
            this.fp2libGroupBox = new System.Windows.Forms.GroupBox();
            this.fancyJsonCheckBox = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.saveProfileComboBox = new System.Windows.Forms.ComboBox();
            this.saveRedirecCheckBox = new System.Windows.Forms.CheckBox();
            this.bepinGroupBox = new System.Windows.Forms.GroupBox();
            this.appendLogCheckBox = new System.Windows.Forms.CheckBox();
            this.unityFileCheckBox = new System.Windows.Forms.CheckBox();
            this.logfileCheckBox = new System.Windows.Forms.CheckBox();
            this.hideLogsCheckBox = new System.Windows.Forms.CheckBox();
            this.noConsoleCloseCheckBox = new System.Windows.Forms.CheckBox();
            this.enableConsoleCheckBox = new System.Windows.Forms.CheckBox();
            this.resPatchButton = new System.Windows.Forms.Button();
            this.fp2resComboBox = new System.Windows.Forms.ComboBox();
            this.fp2resCheckBox = new System.Windows.Forms.CheckBox();
            this.saveButton = new System.Windows.Forms.Button();
            this.helpProvider1 = new System.Windows.Forms.HelpProvider();
            this.contextMenuStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.fp2libGroupBox.SuspendLayout();
            this.bepinGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // savePlay
            // 
            this.savePlay.Location = new System.Drawing.Point(12, 554);
            this.savePlay.Name = "savePlay";
            this.savePlay.Size = new System.Drawing.Size(119, 23);
            this.savePlay.TabIndex = 1;
            this.savePlay.Text = "Save and Play";
            this.savePlay.UseVisualStyleBackColor = true;
            this.savePlay.Click += new System.EventHandler(this.savePlay_Click);
            // 
            // setup
            // 
            this.setup.Location = new System.Drawing.Point(128, 419);
            this.setup.Name = "setup";
            this.setup.Size = new System.Drawing.Size(249, 70);
            this.setup.TabIndex = 2;
            this.setup.Text = "Install BepInEx";
            this.setup.UseVisualStyleBackColor = true;
            this.setup.Click += new System.EventHandler(this.setup_Click);
            // 
            // exit
            // 
            this.exit.Location = new System.Drawing.Point(416, 554);
            this.exit.Name = "exit";
            this.exit.Size = new System.Drawing.Size(78, 23);
            this.exit.TabIndex = 3;
            this.exit.Text = "Exit";
            this.exit.UseVisualStyleBackColor = true;
            this.exit.Click += new System.EventHandler(this.exit_Click);
            // 
            // modInstall
            // 
            this.modInstall.Location = new System.Drawing.Point(3, 457);
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
            this.refresh.Location = new System.Drawing.Point(3, 419);
            this.refresh.Name = "refresh";
            this.refresh.Size = new System.Drawing.Size(119, 32);
            this.refresh.TabIndex = 6;
            this.refresh.Text = "Refresh";
            this.refresh.UseVisualStyleBackColor = true;
            this.refresh.Click += new System.EventHandler(this.refresh_Click);
            // 
            // melonButton
            // 
            this.melonButton.Location = new System.Drawing.Point(383, 419);
            this.melonButton.Name = "melonButton";
            this.melonButton.Size = new System.Drawing.Size(85, 70);
            this.melonButton.TabIndex = 7;
            this.melonButton.Text = "Install MelonLoader Compat";
            this.melonButton.UseVisualStyleBackColor = true;
            this.melonButton.Click += new System.EventHandler(this.melonButton_Click);
            // 
            // handlerButton
            // 
            this.handlerButton.Location = new System.Drawing.Point(8, 341);
            this.handlerButton.Name = "handlerButton";
            this.handlerButton.Size = new System.Drawing.Size(157, 74);
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
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
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
            this.openLogfileToolStripMenuItem,
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
            this.openModsFolderToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
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
            // openLogfileToolStripMenuItem
            // 
            this.openLogfileToolStripMenuItem.Name = "openLogfileToolStripMenuItem";
            this.openLogfileToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.openLogfileToolStripMenuItem.Text = "Open Logfile";
            this.openLogfileToolStripMenuItem.Click += new System.EventHandler(this.openLogfileToolStripMenuItem_Click);
            // 
            // installModToolStripMenuItem
            // 
            this.installModToolStripMenuItem.Name = "installModToolStripMenuItem";
            this.installModToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.installModToolStripMenuItem.Text = "Install Mod";
            this.installModToolStripMenuItem.Click += new System.EventHandler(this.installModToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.gitHubWikiToolStripMenuItem,
            this.gameBananaPageToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // gitHubWikiToolStripMenuItem
            // 
            this.gitHubWikiToolStripMenuItem.Name = "gitHubWikiToolStripMenuItem";
            this.gitHubWikiToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.gitHubWikiToolStripMenuItem.Text = "GitHub Wiki";
            this.gitHubWikiToolStripMenuItem.Click += new System.EventHandler(this.gitHubWikiToolStripMenuItem_Click);
            // 
            // gameBananaPageToolStripMenuItem
            // 
            this.gameBananaPageToolStripMenuItem.Name = "gameBananaPageToolStripMenuItem";
            this.gameBananaPageToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.gameBananaPageToolStripMenuItem.Text = "GameBanana Page";
            this.gameBananaPageToolStripMenuItem.Click += new System.EventHandler(this.gameBananaPageToolStripMenuItem_Click);
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
            this.listView1.Location = new System.Drawing.Point(3, 3);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(465, 410);
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
            this.columnHeader1.Width = 158;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Version";
            this.columnHeader2.Width = 59;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Author";
            this.columnHeader3.Width = 130;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Loader";
            this.columnHeader4.Width = 114;
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
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Controls.Add(this.checkForModUpdatesButton);
            this.tabPage2.Controls.Add(this.updateCheckButton);
            this.tabPage2.Controls.Add(this.groupBox3);
            this.tabPage2.Controls.Add(this.fp2libGroupBox);
            this.tabPage2.Controls.Add(this.bepinGroupBox);
            this.tabPage2.Controls.Add(this.handlerButton);
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
            this.groupBox1.Controls.Add(this.customLaunchParamCheckBox);
            this.groupBox1.Controls.Add(this.LaunchParamsTextBox);
            this.groupBox1.Location = new System.Drawing.Point(171, 341);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(297, 74);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Launch Parameters";
            // 
            // customLaunchParamCheckBox
            // 
            this.customLaunchParamCheckBox.AutoSize = true;
            this.customLaunchParamCheckBox.Location = new System.Drawing.Point(6, 19);
            this.customLaunchParamCheckBox.Name = "customLaunchParamCheckBox";
            this.customLaunchParamCheckBox.Size = new System.Drawing.Size(186, 17);
            this.customLaunchParamCheckBox.TabIndex = 1;
            this.customLaunchParamCheckBox.Text = "Enable custom launch parameters";
            this.customLaunchParamCheckBox.UseVisualStyleBackColor = true;
            this.customLaunchParamCheckBox.CheckedChanged += new System.EventHandler(this.customLaunchParamCheckBox_CheckedChanged);
            // 
            // LaunchParamsTextBox
            // 
            this.LaunchParamsTextBox.Location = new System.Drawing.Point(6, 42);
            this.LaunchParamsTextBox.Name = "LaunchParamsTextBox";
            this.LaunchParamsTextBox.Size = new System.Drawing.Size(285, 20);
            this.LaunchParamsTextBox.TabIndex = 0;
            // 
            // checkForModUpdatesButton
            // 
            this.checkForModUpdatesButton.Location = new System.Drawing.Point(311, 424);
            this.checkForModUpdatesButton.Name = "checkForModUpdatesButton";
            this.checkForModUpdatesButton.Size = new System.Drawing.Size(157, 65);
            this.checkForModUpdatesButton.TabIndex = 14;
            this.checkForModUpdatesButton.Text = "Check for Mod Updates";
            this.checkForModUpdatesButton.UseVisualStyleBackColor = true;
            this.checkForModUpdatesButton.Click += new System.EventHandler(this.checkForModUpdatesButton_Click);
            // 
            // updateCheckButton
            // 
            this.updateCheckButton.Location = new System.Drawing.Point(8, 424);
            this.updateCheckButton.Name = "updateCheckButton";
            this.updateCheckButton.Size = new System.Drawing.Size(157, 65);
            this.updateCheckButton.TabIndex = 12;
            this.updateCheckButton.Text = "Check for Manager Updates";
            this.updateCheckButton.UseVisualStyleBackColor = true;
            this.updateCheckButton.Click += new System.EventHandler(this.updateCheckButton_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.modUpdateCheckBox);
            this.groupBox3.Controls.Add(this.fp2libVersionLabel);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.managerVersionLabel);
            this.groupBox3.Controls.Add(this.fp2libAutoUpdateCheckBox);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.managerAutoUpdateCheckBox);
            this.groupBox3.Location = new System.Drawing.Point(8, 235);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(460, 100);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Automatic updates";
            // 
            // modUpdateCheckBox
            // 
            this.modUpdateCheckBox.AutoSize = true;
            this.modUpdateCheckBox.Location = new System.Drawing.Point(6, 42);
            this.modUpdateCheckBox.Name = "modUpdateCheckBox";
            this.modUpdateCheckBox.Size = new System.Drawing.Size(187, 17);
            this.modUpdateCheckBox.TabIndex = 19;
            this.modUpdateCheckBox.Text = "Check for Mod updates on startup";
            this.modUpdateCheckBox.UseVisualStyleBackColor = true;
            this.modUpdateCheckBox.CheckedChanged += new System.EventHandler(this.modUpdateCheckBox_CheckedChanged);
            // 
            // fp2libVersionLabel
            // 
            this.fp2libVersionLabel.AutoSize = true;
            this.fp2libVersionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(254)));
            this.fp2libVersionLabel.Location = new System.Drawing.Point(328, 66);
            this.fp2libVersionLabel.Name = "fp2libVersionLabel";
            this.fp2libVersionLabel.Size = new System.Drawing.Size(30, 13);
            this.fp2libVersionLabel.TabIndex = 18;
            this.fp2libVersionLabel.Text = "N/A";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(202, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(118, 13);
            this.label3.TabIndex = 17;
            this.label3.Text = "Current FP2Lib Version:";
            // 
            // managerVersionLabel
            // 
            this.managerVersionLabel.AutoSize = true;
            this.managerVersionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(254)));
            this.managerVersionLabel.Location = new System.Drawing.Point(142, 66);
            this.managerVersionLabel.Name = "managerVersionLabel";
            this.managerVersionLabel.Size = new System.Drawing.Size(36, 13);
            this.managerVersionLabel.TabIndex = 16;
            this.managerVersionLabel.Text = "2.0.0";
            // 
            // fp2libAutoUpdateCheckBox
            // 
            this.fp2libAutoUpdateCheckBox.AutoSize = true;
            this.fp2libAutoUpdateCheckBox.Location = new System.Drawing.Point(205, 42);
            this.fp2libAutoUpdateCheckBox.Name = "fp2libAutoUpdateCheckBox";
            this.fp2libAutoUpdateCheckBox.Size = new System.Drawing.Size(199, 17);
            this.fp2libAutoUpdateCheckBox.TabIndex = 14;
            this.fp2libAutoUpdateCheckBox.Text = "Check for FP2Lib updates on startup";
            this.fp2libAutoUpdateCheckBox.UseVisualStyleBackColor = true;
            this.fp2libAutoUpdateCheckBox.CheckedChanged += new System.EventHandler(this.fp2libAutoUpdateCheckBox_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 66);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(127, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "Current Manager Version:";
            // 
            // managerAutoUpdateCheckBox
            // 
            this.managerAutoUpdateCheckBox.AutoSize = true;
            this.managerAutoUpdateCheckBox.Location = new System.Drawing.Point(6, 19);
            this.managerAutoUpdateCheckBox.Name = "managerAutoUpdateCheckBox";
            this.managerAutoUpdateCheckBox.Size = new System.Drawing.Size(252, 17);
            this.managerAutoUpdateCheckBox.TabIndex = 13;
            this.managerAutoUpdateCheckBox.Text = "Check for Freedom Manager updates on startup";
            this.managerAutoUpdateCheckBox.UseVisualStyleBackColor = true;
            this.managerAutoUpdateCheckBox.CheckedChanged += new System.EventHandler(this.managerAutoUpdateCheckBox_CheckedChanged);
            // 
            // fp2libGroupBox
            // 
            this.fp2libGroupBox.Controls.Add(this.fancyJsonCheckBox);
            this.fp2libGroupBox.Controls.Add(this.label6);
            this.fp2libGroupBox.Controls.Add(this.label5);
            this.fp2libGroupBox.Controls.Add(this.saveProfileComboBox);
            this.fp2libGroupBox.Controls.Add(this.saveRedirecCheckBox);
            this.fp2libGroupBox.Location = new System.Drawing.Point(8, 101);
            this.fp2libGroupBox.Name = "fp2libGroupBox";
            this.fp2libGroupBox.Size = new System.Drawing.Size(460, 128);
            this.fp2libGroupBox.TabIndex = 0;
            this.fp2libGroupBox.TabStop = false;
            this.fp2libGroupBox.Text = "FP2Lib Settings";
            // 
            // fancyJsonCheckBox
            // 
            this.fancyJsonCheckBox.AutoSize = true;
            this.fancyJsonCheckBox.Location = new System.Drawing.Point(6, 42);
            this.fancyJsonCheckBox.Name = "fancyJsonCheckBox";
            this.fancyJsonCheckBox.Size = new System.Drawing.Size(117, 17);
            this.fancyJsonCheckBox.TabIndex = 6;
            this.fancyJsonCheckBox.Text = "Fancy JSON saves";
            this.fancyJsonCheckBox.UseVisualStyleBackColor = true;
            this.fancyJsonCheckBox.CheckedChanged += new System.EventHandler(this.fancyJsonCheckBox_CheckedChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 103);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(214, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Each profile allows for separate set of saves";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 63);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(39, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Profile:";
            // 
            // saveProfileComboBox
            // 
            this.saveProfileComboBox.FormattingEnabled = true;
            this.saveProfileComboBox.Items.AddRange(new object[] {
            "Default",
            "Profile 1",
            "Profile 2",
            "Profile 3",
            "Profile 4",
            "Profile 5",
            "Profile 6",
            "Profile 7",
            "Profile 8",
            "Profile 9"});
            this.saveProfileComboBox.Location = new System.Drawing.Point(6, 79);
            this.saveProfileComboBox.Name = "saveProfileComboBox";
            this.saveProfileComboBox.Size = new System.Drawing.Size(151, 21);
            this.saveProfileComboBox.TabIndex = 3;
            this.saveProfileComboBox.SelectedIndexChanged += new System.EventHandler(this.saveProfileComboBox_SelectedIndexChanged);
            // 
            // saveRedirecCheckBox
            // 
            this.saveRedirecCheckBox.AutoSize = true;
            this.saveRedirecCheckBox.Location = new System.Drawing.Point(6, 19);
            this.saveRedirecCheckBox.Name = "saveRedirecCheckBox";
            this.saveRedirecCheckBox.Size = new System.Drawing.Size(119, 17);
            this.saveRedirecCheckBox.TabIndex = 0;
            this.saveRedirecCheckBox.Text = "Save file redirection";
            this.saveRedirecCheckBox.UseVisualStyleBackColor = true;
            this.saveRedirecCheckBox.CheckedChanged += new System.EventHandler(this.saveRedirecCheckBox_CheckedChanged);
            // 
            // bepinGroupBox
            // 
            this.bepinGroupBox.Controls.Add(this.appendLogCheckBox);
            this.bepinGroupBox.Controls.Add(this.unityFileCheckBox);
            this.bepinGroupBox.Controls.Add(this.logfileCheckBox);
            this.bepinGroupBox.Controls.Add(this.hideLogsCheckBox);
            this.bepinGroupBox.Controls.Add(this.noConsoleCloseCheckBox);
            this.bepinGroupBox.Controls.Add(this.enableConsoleCheckBox);
            this.bepinGroupBox.Location = new System.Drawing.Point(8, 6);
            this.bepinGroupBox.Name = "bepinGroupBox";
            this.bepinGroupBox.Size = new System.Drawing.Size(460, 89);
            this.bepinGroupBox.TabIndex = 0;
            this.bepinGroupBox.TabStop = false;
            this.bepinGroupBox.Text = "BepInEx Settings";
            // 
            // appendLogCheckBox
            // 
            this.appendLogCheckBox.AutoSize = true;
            this.appendLogCheckBox.Location = new System.Drawing.Point(237, 65);
            this.appendLogCheckBox.Name = "appendLogCheckBox";
            this.appendLogCheckBox.Size = new System.Drawing.Size(126, 17);
            this.appendLogCheckBox.TabIndex = 5;
            this.appendLogCheckBox.Text = "Do not overwrite logs";
            this.appendLogCheckBox.UseVisualStyleBackColor = true;
            this.appendLogCheckBox.CheckedChanged += new System.EventHandler(this.appendLogCheckBox_CheckedChanged);
            // 
            // unityFileCheckBox
            // 
            this.unityFileCheckBox.AutoSize = true;
            this.unityFileCheckBox.Location = new System.Drawing.Point(237, 42);
            this.unityFileCheckBox.Name = "unityFileCheckBox";
            this.unityFileCheckBox.Size = new System.Drawing.Size(136, 17);
            this.unityFileCheckBox.TabIndex = 4;
            this.unityFileCheckBox.Text = "Add Unity logs to logfile";
            this.unityFileCheckBox.UseVisualStyleBackColor = true;
            this.unityFileCheckBox.CheckedChanged += new System.EventHandler(this.unityFileCheckBox_CheckedChanged);
            // 
            // logfileCheckBox
            // 
            this.logfileCheckBox.AutoSize = true;
            this.logfileCheckBox.Location = new System.Drawing.Point(237, 19);
            this.logfileCheckBox.Name = "logfileCheckBox";
            this.logfileCheckBox.Size = new System.Drawing.Size(96, 17);
            this.logfileCheckBox.TabIndex = 3;
            this.logfileCheckBox.Text = "Write log to file";
            this.logfileCheckBox.UseVisualStyleBackColor = true;
            this.logfileCheckBox.CheckedChanged += new System.EventHandler(this.logfileCheckBox_CheckedChanged);
            // 
            // hideLogsCheckBox
            // 
            this.hideLogsCheckBox.AutoSize = true;
            this.hideLogsCheckBox.Location = new System.Drawing.Point(6, 65);
            this.hideLogsCheckBox.Name = "hideLogsCheckBox";
            this.hideLogsCheckBox.Size = new System.Drawing.Size(102, 17);
            this.hideLogsCheckBox.TabIndex = 2;
            this.hideLogsCheckBox.Text = "Show Unity logs";
            this.hideLogsCheckBox.UseVisualStyleBackColor = true;
            this.hideLogsCheckBox.CheckedChanged += new System.EventHandler(this.hideLogsCheckBox_CheckedChanged);
            // 
            // noConsoleCloseCheckBox
            // 
            this.noConsoleCloseCheckBox.AutoSize = true;
            this.noConsoleCloseCheckBox.Location = new System.Drawing.Point(6, 42);
            this.noConsoleCloseCheckBox.Name = "noConsoleCloseCheckBox";
            this.noConsoleCloseCheckBox.Size = new System.Drawing.Size(151, 17);
            this.noConsoleCloseCheckBox.TabIndex = 1;
            this.noConsoleCloseCheckBox.Text = "Prevent closing of console";
            this.noConsoleCloseCheckBox.UseVisualStyleBackColor = true;
            this.noConsoleCloseCheckBox.CheckedChanged += new System.EventHandler(this.noConsoleCloseCheckBox_CheckedChanged);
            // 
            // enableConsoleCheckBox
            // 
            this.enableConsoleCheckBox.AutoSize = true;
            this.enableConsoleCheckBox.Location = new System.Drawing.Point(6, 19);
            this.enableConsoleCheckBox.Name = "enableConsoleCheckBox";
            this.enableConsoleCheckBox.Size = new System.Drawing.Size(100, 17);
            this.enableConsoleCheckBox.TabIndex = 0;
            this.enableConsoleCheckBox.Text = "Enable Console";
            this.enableConsoleCheckBox.UseVisualStyleBackColor = true;
            this.enableConsoleCheckBox.CheckedChanged += new System.EventHandler(this.enableConsoleCheckBox_CheckedChanged);
            // 
            // resPatchButton
            // 
            this.resPatchButton.Location = new System.Drawing.Point(0, 0);
            this.resPatchButton.Name = "resPatchButton";
            this.resPatchButton.Size = new System.Drawing.Size(75, 23);
            this.resPatchButton.TabIndex = 0;
            // 
            // fp2resComboBox
            // 
            this.fp2resComboBox.Location = new System.Drawing.Point(0, 0);
            this.fp2resComboBox.Name = "fp2resComboBox";
            this.fp2resComboBox.Size = new System.Drawing.Size(121, 21);
            this.fp2resComboBox.TabIndex = 0;
            // 
            // fp2resCheckBox
            // 
            this.fp2resCheckBox.Location = new System.Drawing.Point(0, 0);
            this.fp2resCheckBox.Name = "fp2resCheckBox";
            this.fp2resCheckBox.Size = new System.Drawing.Size(104, 24);
            this.fp2resCheckBox.TabIndex = 0;
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(137, 554);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(119, 23);
            this.saveButton.TabIndex = 12;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // FreedomManager
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(506, 589);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.menuStrip1);
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
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.fp2libGroupBox.ResumeLayout(false);
            this.fp2libGroupBox.PerformLayout();
            this.bepinGroupBox.ResumeLayout(false);
            this.bepinGroupBox.PerformLayout();
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
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ToolStripMenuItem gitHubWikiToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gameBananaPageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openModsFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bepInExToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem melonLoaderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem installModToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem seeOnGameBananaToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox fp2libGroupBox;
        private System.Windows.Forms.GroupBox bepinGroupBox;
        private System.Windows.Forms.CheckBox appendLogCheckBox;
        private System.Windows.Forms.CheckBox unityFileCheckBox;
        private System.Windows.Forms.CheckBox logfileCheckBox;
        private System.Windows.Forms.CheckBox hideLogsCheckBox;
        private System.Windows.Forms.CheckBox noConsoleCloseCheckBox;
        private System.Windows.Forms.CheckBox enableConsoleCheckBox;
        private System.Windows.Forms.Button updateCheckButton;
        private System.Windows.Forms.CheckBox managerAutoUpdateCheckBox;
        private System.Windows.Forms.Label managerVersionLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox fp2libAutoUpdateCheckBox;
        private System.Windows.Forms.Label fp2libVersionLabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button resPatchButton;
        private System.Windows.Forms.ComboBox fp2resComboBox;
        private System.Windows.Forms.CheckBox fp2resCheckBox;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.CheckBox saveRedirecCheckBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox saveProfileComboBox;
        private System.Windows.Forms.CheckBox fancyJsonCheckBox;
        private System.Windows.Forms.ToolStripMenuItem openLogfileToolStripMenuItem;
        private System.Windows.Forms.CheckBox modUpdateCheckBox;
        private System.Windows.Forms.Button checkForModUpdatesButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox customLaunchParamCheckBox;
        private System.Windows.Forms.TextBox LaunchParamsTextBox;
        private System.Windows.Forms.HelpProvider helpProvider1;
    }
}

