using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace FreedomManager
{
    internal static class Program
    {
        public const string pipeName = "fp2-mod-manager";
        public const string protocol = "fp2mm:";
        private static readonly Mutex mutex = new Mutex(true, pipeName);

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {

            bool alreadyRunning;
            try { alreadyRunning = !mutex.WaitOne(0, true); }
            catch (AbandonedMutexException) { alreadyRunning = false; }

            if (args.Length > 1 && args[0] == "run-update")
            {

            }

            List<string> uris = args
                .Where(x => x.Length > protocol.Length && x.StartsWith(protocol, StringComparison.Ordinal))
                .ToList();

            if (uris.Count > 0 && alreadyRunning)
            {
                using (var pipe = new NamedPipeClientStream(".", pipeName, PipeDirection.Out))
                {
                    pipe.Connect(3600);

                    var writer = new StreamWriter(pipe);
                    foreach (string s in uris)
                    {
                        writer.WriteLine(s);
                    }
                    writer.Flush();
                }
            }

            if (alreadyRunning)
            {
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FreedomManager(uris));
        }
    }
}
