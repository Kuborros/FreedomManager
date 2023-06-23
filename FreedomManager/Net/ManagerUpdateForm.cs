using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FreedomManager.Net
{
    public partial class ManagerUpdateForm : Form
    {
        public ManagerUpdateForm()
        {
            InitializeComponent();
        }

        private void installButton_Click(object sender, EventArgs e)
        {
            
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void releaseLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.releaseLinkLabel.LinkVisited = true;
            Process.Start("explorer", "https://github.com/Kuborros/FreedomManager/releases/latest");
        }

        private void fp2libLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.fp2libLinkLabel.LinkVisited = true;
            Process.Start("explorer", "https://github.com/Kuborros/FP2Lib/releases/latest/");
        }
    }
}
