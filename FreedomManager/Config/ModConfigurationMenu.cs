﻿using IniParser.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
