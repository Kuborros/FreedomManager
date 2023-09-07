using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FreedomManager.Net
{
    public partial class ModsUpdateInfoForm : Form
    {
        public ModsUpdateInfoForm()
        {

            InitializeComponent();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            changelogWebBrowser.DocumentText = "<html><body><h3>Test</h3><br>Aaaaa</body></html>";
        }
    }
}
