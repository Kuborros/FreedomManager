using FreedomManager.Mod;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace FreedomManager.Net
{
    public partial class ModsUpdateInfoForm : Form
    {
        static int columnIndex;

        internal ModsUpdateInfoForm(List<ModUpdateInfo> updates)
        {
            InitializeComponent();

            updatableListView.FocusedItem = null;
            foreach (ModUpdateInfo info in updates)
            {
                ListViewItem item = new ListViewItem
                {
                    Tag = info,
                    Text = info.Name
                };
                item.SubItems.Add(info.Version);
                item.Checked = info.DoUpdate;
                updatableListView.Items.Add(item);
            }
        }


        private void updatableListView_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            updatableListView.BeginUpdate();
            if (updatableListView.FocusedItem != null)
            {
                ModUpdateInfo info = (ModUpdateInfo)e.Item.Tag;
                info.DoUpdate = e.Item.Checked;
            }
            updatableListView.EndUpdate();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void updatableListView_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void updatableListView_NodeMouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ListViewHitTestInfo listViewHitTestInfo = updatableListView.HitTest(e.X, e.Y);
                if (listViewHitTestInfo.Item != null)
                {
                    columnIndex = listViewHitTestInfo.Item.Index;
                    ModUpdateInfo info = (ModUpdateInfo)listViewHitTestInfo.Item.Tag;
                    DisplayHtml(info.Description);
                }
            }
        }

        private void DisplayHtml(string content)
        {
            changelogWebBrowser.Navigate("about:blank");
            try
            {
                changelogWebBrowser.Document?.Write(string.Empty);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            content = content.Replace(Environment.NewLine, "<br>");

            string html = " <!DOCTYPE html><html><head></head><style>body {font-family:'Verdana',sans-serif; font-size: 14px;}</style><body>" + content + "</body></html>";

            changelogWebBrowser.DocumentText = html;
        }
    }
}
