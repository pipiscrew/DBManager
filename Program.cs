using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;

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
