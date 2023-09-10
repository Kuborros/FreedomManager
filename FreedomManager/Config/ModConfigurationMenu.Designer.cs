namespace FreedomManager.Config
{
    partial class ModConfigurationMenu
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ModConfigurationMenu));
            this.cancelButton = new System.Windows.Forms.Button();
            this.modConfigPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.modConfigMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.saveButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(301, 415);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // modConfigPropertyGrid
            // 
            this.modConfigPropertyGrid.CommandsVisibleIfAvailable = false;
            this.modConfigPropertyGrid.ContextMenuStrip = this.modConfigMenuStrip;
            this.modConfigPropertyGrid.Location = new System.Drawing.Point(12, 12);
            this.modConfigPropertyGrid.Name = "modConfigPropertyGrid";
            this.modConfigPropertyGrid.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.modConfigPropertyGrid.Size = new System.Drawing.Size(364, 397);
            this.modConfigPropertyGrid.TabIndex = 2;
            this.modConfigPropertyGrid.ToolbarVisible = false;
            // 
            // modConfigMenuStrip
            // 
            this.modConfigMenuStrip.Name = "modConfigMenuStrip";
            this.modConfigMenuStrip.Size = new System.Drawing.Size(61, 4);
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(12, 415);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 4;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            // 
            // ModConfigurationMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(388, 450);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.modConfigPropertyGrid);
            this.Controls.Add(this.cancelButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ModConfigurationMenu";
            this.Text = "Mod Configuration: ";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.PropertyGrid modConfigPropertyGrid;
        private System.Windows.Forms.ContextMenuStrip modConfigMenuStrip;
        private System.Windows.Forms.Button saveButton;
    }
}