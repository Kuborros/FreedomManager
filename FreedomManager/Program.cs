using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace FreedomManager
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            using (var mutex = new Mutex(true, Assembly.GetExecutingAssembly().FullName, out bool createdNew))
            {
                if (createdNew)
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new FreedomManager(args));
                }
                else
                {
                    Process current = Process.GetCurrentProcess();
                    foreach (Process process in Process.GetProcessesByName(current.ProcessName))
                    {
                        if (process.Id != current.Id)
                        {
                            if (args.Length > 0)
                            {
                                // TODO tell other mod manager instance about arguments
                                //  right now users have to CLOSE the manager to get 1-click install to work...
                            }
                            else if (process.MainWindowHandle != IntPtr.Zero)
                            {
                                // make sure the other instance's window is visible...
                                ShowWindow(process.MainWindowHandle, SW_RESTORE);
                                // ...and FOCUS on it!
                                SetForegroundWindow(process.MainWindowHandle);
                            }
                            break;
                        }
                    }
                }
            }
        }

        #region Misc native methods
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        public const int SW_RESTORE = 9;

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);
        #endregion
    }
}
