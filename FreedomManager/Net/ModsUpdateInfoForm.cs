using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace FreedomManager.Net
{
    public partial class ModsUpdateInfoForm : Form
    {

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
                if (updatableListView.Items.Count > 0 && updatableListView.Items[0].Selected == false)
                {
                    updatableListView.Items[0].Selected = true;
                    changelogRichTextBox.Text = info.Description;
                }
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
                    ModUpdateInfo info = (ModUpdateInfo)listViewHitTestInfo.Item.Tag;
                    changelogRichTextBox.Text = info.Description;
                }
            }
        }
    }
}
