namespace FreedomManager
{
    partial class Form1
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
            this.savePlay = new System.Windows.Forms.Button();
            this.setup = new System.Windows.Forms.Button();
            this.exit = new System.Windows.Forms.Button();
            this.modInstall = new System.Windows.Forms.Button();
            this.modFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // savePlay
            // 
            this.savePlay.Location = new System.Drawing.Point(12, 598);
            this.savePlay.Name = "savePlay";
            this.savePlay.Size = new System.Drawing.Size(119, 23);
            this.savePlay.TabIndex = 1;
            this.savePlay.Text = "Save and Play";
            this.savePlay.UseVisualStyleBackColor = true;
            this.savePlay.Click += new System.EventHandler(this.savePlay_Click);
            // 
            // setup
            // 
            this.setup.Location = new System.Drawing.Point(137, 598);
            this.setup.Name = "setup";
            this.setup.Size = new System.Drawing.Size(281, 23);
            this.setup.TabIndex = 2;
            this.setup.Text = "Setup BepInEx";
            this.setup.UseVisualStyleBackColor = true;
            this.setup.Click += new System.EventHandler(this.setup_Click);
            // 
            // exit
            // 
            this.exit.Location = new System.Drawing.Point(424, 598);
            this.exit.Name = "exit";
            this.exit.Size = new System.Drawing.Size(75, 23);
            this.exit.TabIndex = 3;
            this.exit.Text = "Exit";
            this.exit.UseVisualStyleBackColor = true;
            this.exit.Click += new System.EventHandler(this.exit_Click);
            // 
            // modInstall
            // 
            this.modInstall.Location = new System.Drawing.Point(12, 525);
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
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(511, 633);
            this.Controls.Add(this.modInstall);
            this.Controls.Add(this.exit);
            this.Controls.Add(this.setup);
            this.Controls.Add(this.savePlay);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button savePlay;
        private System.Windows.Forms.Button setup;
        private System.Windows.Forms.Button exit;
        private System.Windows.Forms.Button modInstall;
        private System.Windows.Forms.OpenFileDialog modFileDialog;
    }
}

