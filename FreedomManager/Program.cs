using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace FreedomManager
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string[] args = Environment.GetCommandLineArgs();

            Application.Run(new Form1(args));
        }
    }
}
