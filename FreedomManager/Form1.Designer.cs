namespace FreedomManager
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
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.infoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uninstallToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // savePlay
            // 
            this.savePlay.Location = new System.Drawing.Point(12, 441);
            this.savePlay.Name = "savePlay";
            this.savePlay.Size = new System.Drawing.Size(119, 23);
            this.savePlay.TabIndex = 1;
            this.savePlay.Text = "Save and Play";
            this.savePlay.UseVisualStyleBackColor = true;
            this.savePlay.Click += new System.EventHandler(this.savePlay_Click);
            // 
            // setup
            // 
            this.setup.Location = new System.Drawing.Point(137, 441);
            this.setup.Name = "setup";
            this.setup.Size = new System.Drawing.Size(281, 23);
            this.setup.TabIndex = 2;
            this.setup.Text = "Install BepInEx";
            this.setup.UseVisualStyleBackColor = true;
            this.setup.Click += new System.EventHandler(this.setup_Click);
            // 
            // exit
            // 
            this.exit.Location = new System.Drawing.Point(424, 441);
            this.exit.Name = "exit";
            this.exit.Size = new System.Drawing.Size(75, 23);
            this.exit.TabIndex = 3;
            this.exit.Text = "Exit";
            this.exit.UseVisualStyleBackColor = true;
            this.exit.Click += new System.EventHandler(this.exit_Click);
            // 
            // modInstall
            // 
            this.modInstall.Location = new System.Drawing.Point(12, 368);
            this.modInstall.Name = "modInstall";
            this.modInstall.Size = new System.Drawing.Size(119, 67);
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
            this.refresh.Location = new System.Drawing.Point(137, 368);
            this.refresh.Name = "refresh";
            this.refresh.Size = new System.Drawing.Size(116, 67);
            this.refresh.TabIndex = 6;
            this.refresh.Text = "Refresh";
            this.refresh.UseVisualStyleBackColor = true;
            this.refresh.Click += new System.EventHandler(this.refresh_Click);
            // 
            // melonButton
            // 
            this.melonButton.Location = new System.Drawing.Point(259, 368);
            this.melonButton.Name = "melonButton";
            this.melonButton.Size = new System.Drawing.Size(159, 67);
            this.melonButton.TabIndex = 7;
            this.melonButton.Text = "Install MelonLoader Compat";
            this.melonButton.UseVisualStyleBackColor = true;
            this.melonButton.Click += new System.EventHandler(this.melonButton_Click);
            // 
            // handlerButton
            // 
            this.handlerButton.Location = new System.Drawing.Point(424, 368);
            this.handlerButton.Name = "handlerButton";
            this.handlerButton.Size = new System.Drawing.Size(75, 67);
            this.handlerButton.TabIndex = 8;
            this.handlerButton.Text = "Register URL handler";
            this.handlerButton.UseVisualStyleBackColor = true;
            this.handlerButton.Click += new System.EventHandler(this.handlerButton_Click);
            // 
            // treeView1
            // 
            this.treeView1.HideSelection = false;
            this.treeView1.Location = new System.Drawing.Point(12, 12);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(487, 350);
            this.treeView1.TabIndex = 5;
            this.treeView1.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.infoToolStripMenuItem,
            this.uninstallToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(121, 48);
            // 
            // infoToolStripMenuItem
            // 
            this.infoToolStripMenuItem.Name = "infoToolStripMenuItem";
            this.infoToolStripMenuItem.Size = new System.Drawing.Size(120, 22);
            this.infoToolStripMenuItem.Text = "Info";
            this.infoToolStripMenuItem.Click += new System.EventHandler(this.infoToolStripMenuItem_Click);
            // 
            // uninstallToolStripMenuItem
            // 
            this.uninstallToolStripMenuItem.Name = "uninstallToolStripMenuItem";
            this.uninstallToolStripMenuItem.Size = new System.Drawing.Size(120, 22);
            this.uninstallToolStripMenuItem.Text = "Uninstall";
            this.uninstallToolStripMenuItem.Click += new System.EventHandler(this.uninstallToolStripMenuItem_Click);
            // 
            // FreedomManager
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(511, 475);
            this.Controls.Add(this.handlerButton);
            this.Controls.Add(this.melonButton);
            this.Controls.Add(this.refresh);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.modInstall);
            this.Controls.Add(this.exit);
            this.Controls.Add(this.setup);
            this.Controls.Add(this.savePlay);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FreedomManager";
            this.Text = "FreedomManager";
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

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
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem infoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uninstallToolStripMenuItem;
    }
}

