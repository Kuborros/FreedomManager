using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace FreedomManager
{
    internal static class Program
    {
        private const string pipeName = "fp2-mod-manager";
        private static readonly Mutex mutex = new Mutex(true, pipeName);

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            bool alreadyRunning;
            try { alreadyRunning = !mutex.WaitOne(0, true); }
            catch (AbandonedMutexException) { alreadyRunning = false; }

            string[] args = Environment.GetCommandLineArgs();

            Application.Run(new FreedomManager(args,alreadyRunning));
        }
    }
}
