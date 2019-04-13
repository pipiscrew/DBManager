using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using System.Net;

namespace DBManager
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //
            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            //

            ////enable HTTPS - avoid "The request was aborted: Could not create SSL/TLS secure channel" - https://stackoverflow.com/a/51346252
            //ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);

            Application.Run(new MainForm());
        }

        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            if (e.Exception is NotImplementedException)
                General.Mes("This functionality not supported on this database type", MessageBoxIcon.Exclamation);
            else
            {
                ErrorReporter reporter = new ErrorReporter(e.Exception);
                reporter.ShowDialog();
            }
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            ErrorReporter reporter = new ErrorReporter((Exception)e.ExceptionObject);
            reporter.ShowDialog();
        }

    }
}
