using System.Windows.Forms;

namespace FreedomManager.Net
{
    partial class ModsUpdateInfoForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ModsUpdateInfoForm));
            this.updateSelectedButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.updatableListView = new System.Windows.Forms.ListView();
            this.nameColumnHeader = new System.Windows.Forms.ColumnHeader();
            this.versionColumnHeader = new System.Windows.Forms.ColumnHeader();
            this.cancelButton = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.changelogRichTextBox = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // updateSelectedButton
            // 
            this.updateSelectedButton.Location = new System.Drawing.Point(12, 264);
            this.updateSelectedButton.Name = "updateSelectedButton";
            this.updateSelectedButton.Size = new System.Drawing.Size(192, 23);
            this.updateSelectedButton.TabIndex = 2;
            this.updateSelectedButton.Text = "Update Selected";
            this.updateSelectedButton.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Mods:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(280, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Changelog:";
            // 
            // updatableListView
            // 
            this.updatableListView.CheckBoxes = true;
            this.updatableListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.nameColumnHeader,
            this.versionColumnHeader});
            this.updatableListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.updatableListView.HideSelection = false;
            this.updatableListView.Location = new System.Drawing.Point(12, 29);
            this.updatableListView.Name = "updatableListView";
            this.updatableListView.Size = new System.Drawing.Size(265, 225);
            this.updatableListView.TabIndex = 5;
            this.updatableListView.UseCompatibleStateImageBehavior = false;
            this.updatableListView.View = System.Windows.Forms.View.Details;
            this.updatableListView.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.updatableListView_ItemChecked);
            this.updatableListView.SelectedIndexChanged += new System.EventHandler(this.updatableListView_SelectedIndexChanged);
            this.updatableListView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.updatableListView_NodeMouseClick);
            // 
            // nameColumnHeader
            // 
            this.nameColumnHeader.Text = "Name";
            this.nameColumnHeader.Width = 188;
            // 
            // versionColumnHeader
            // 
            this.versionColumnHeader.Text = "New Version";
            this.versionColumnHeader.Width = 72;
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(545, 264);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 6;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // changelogRichTextBox
            // 
            this.changelogRichTextBox.AcceptsTab = true;
            this.changelogRichTextBox.Location = new System.Drawing.Point(283, 29);
            this.changelogRichTextBox.Name = "changelogRichTextBox";
            this.changelogRichTextBox.ReadOnly = true;
            this.changelogRichTextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.changelogRichTextBox.ShortcutsEnabled = false;
            this.changelogRichTextBox.Size = new System.Drawing.Size(337, 225);
            this.changelogRichTextBox.TabIndex = 7;
            this.changelogRichTextBox.Text = "";
            // 
            // ModsUpdateInfoForm
            // 
            this.AcceptButton = this.updateSelectedButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(632, 299);
            this.Controls.Add(this.changelogRichTextBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.updatableListView);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.updateSelectedButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "ModsUpdateInfoForm";
            this.Text = "Mod Updates";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListView updatableListView;
        private System.Windows.Forms.Button cancelButton;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.ColumnHeader nameColumnHeader;
        private System.Windows.Forms.ColumnHeader versionColumnHeader;
        private System.Windows.Forms.RichTextBox changelogRichTextBox;
        internal Button updateSelectedButton;
    }
}