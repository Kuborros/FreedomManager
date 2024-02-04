using System.Windows.Forms;

namespace FreedomManager.Config
{
    public partial class ModConfigurationMenu : Form
    {

        ModConfig config;
        internal ModConfigurationMenu(ModConfig config)
        {
            InitializeComponent();
            this.config = config;

            modConfigPropertyGrid.SelectedObject = config;



        }
    }
}
