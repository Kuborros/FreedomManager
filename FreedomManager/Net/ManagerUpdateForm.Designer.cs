namespace FreedomManager.Net
{
    partial class ManagerUpdateForm
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
            this.UpdateTitleLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.versionNumberLabel = new System.Windows.Forms.Label();
            this.releaseLinkLabel = new System.Windows.Forms.LinkLabel();
            this.installButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.fp2libVersionLabel = new System.Windows.Forms.Label();
            this.fp2libLinkLabel = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // UpdateTitleLabel
            // 
            this.UpdateTitleLabel.AutoSize = true;
            this.UpdateTitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(254)));
            this.UpdateTitleLabel.Location = new System.Drawing.Point(12, 9);
            this.UpdateTitleLabel.Name = "UpdateTitleLabel";
            this.UpdateTitleLabel.Size = new System.Drawing.Size(186, 17);
            this.UpdateTitleLabel.TabIndex = 1;
            this.UpdateTitleLabel.Text = "New Update is available!";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Mod Manager Version:";
            // 
            // versionNumberLabel
            // 
            this.versionNumberLabel.AutoSize = true;
            this.versionNumberLabel.Location = new System.Drawing.Point(132, 26);
            this.versionNumberLabel.Name = "versionNumberLabel";
            this.versionNumberLabel.Size = new System.Drawing.Size(31, 13);
            this.versionNumberLabel.TabIndex = 3;
            this.versionNumberLabel.Text = "0.0.0";
            this.versionNumberLabel.Click += new System.EventHandler(this.versionNumberLabel_Click);
            // 
            // releaseLinkLabel
            // 
            this.releaseLinkLabel.AutoSize = true;
            this.releaseLinkLabel.Location = new System.Drawing.Point(12, 39);
            this.releaseLinkLabel.Name = "releaseLinkLabel";
            this.releaseLinkLabel.Size = new System.Drawing.Size(80, 13);
            this.releaseLinkLabel.TabIndex = 4;
            this.releaseLinkLabel.TabStop = true;
            this.releaseLinkLabel.Text = "Github Release";
            // 
            // installButton
            // 
            this.installButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.installButton.Location = new System.Drawing.Point(12, 105);
            this.installButton.Name = "installButton";
            this.installButton.Size = new System.Drawing.Size(75, 23);
            this.installButton.TabIndex = 5;
            this.installButton.Text = "Install";
            this.installButton.UseVisualStyleBackColor = true;
            this.installButton.Click += new System.EventHandler(this.installButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(193, 105);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 6;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "FP2Lib Version:";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // fp2libVersionLabel
            // 
            this.fp2libVersionLabel.AutoSize = true;
            this.fp2libVersionLabel.Location = new System.Drawing.Point(132, 63);
            this.fp2libVersionLabel.Name = "fp2libVersionLabel";
            this.fp2libVersionLabel.Size = new System.Drawing.Size(31, 13);
            this.fp2libVersionLabel.TabIndex = 8;
            this.fp2libVersionLabel.Text = "0.0.0";
            // 
            // fp2libLinkLabel
            // 
            this.fp2libLinkLabel.AutoSize = true;
            this.fp2libLinkLabel.Location = new System.Drawing.Point(12, 76);
            this.fp2libLinkLabel.Name = "fp2libLinkLabel";
            this.fp2libLinkLabel.Size = new System.Drawing.Size(80, 13);
            this.fp2libLinkLabel.TabIndex = 9;
            this.fp2libLinkLabel.TabStop = true;
            this.fp2libLinkLabel.Text = "Github Release";
            // 
            // ManagerUpdateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(280, 140);
            this.Controls.Add(this.fp2libLinkLabel);
            this.Controls.Add(this.fp2libVersionLabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.installButton);
            this.Controls.Add(this.releaseLinkLabel);
            this.Controls.Add(this.versionNumberLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.UpdateTitleLabel);
            this.Name = "ManagerUpdateForm";
            this.Text = "Updater";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label UpdateTitleLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label versionNumberLabel;
        private System.Windows.Forms.LinkLabel releaseLinkLabel;
        private System.Windows.Forms.Button installButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label fp2libVersionLabel;
        private System.Windows.Forms.LinkLabel fp2libLinkLabel;
    }
}