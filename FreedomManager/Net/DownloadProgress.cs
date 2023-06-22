using System;
using System.ComponentModel;
using System.Net;
using System.Windows.Forms;

namespace FreedomManager
{
    public partial class DownloadProgress : Form
    {
        public DownloadProgress(string name)
        {
            InitializeComponent();
            modNameLabel.Text = name;
            CenterToParent();
        }

        private void DownloadProgress_Load(object sender, EventArgs e)
        {
        }

        public void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            this.BeginInvoke((MethodInvoker)delegate
            {
                double bytesIn = double.Parse(e.BytesReceived.ToString());
                double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
                double percentage = bytesIn / totalBytes * 100;
                progressBar1.Value = int.Parse(Math.Truncate(percentage).ToString());
            });
        }
        public void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            this.BeginInvoke((MethodInvoker)delegate
            {
                this.Hide();
            });
        }
    }
}
