using System;
using System.Collections.Generic;
using System.Windows.Forms;


using MaDaNet.Common.AppFrameWork.ApplicationModul;
using Microsoft.Practices.EnterpriseLibrary.Logging.Database.Configuration;
using System.Diagnostics;

namespace metatop.Applications.metaCall.WinForms.App
{
    static class Program
    {

        private static MainForm mainForm;

        //für Agents mit XPhoneServer [MTAThread] - DialerBusiness: #define UseTAPI nicht vergessen
        //für Verwaltung mit Kopierfunktion [STAThread]

        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            DoStartup(args);

        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801", MessageId="args")]
        static void DoStartup(string[] args)
        {
            //Enable VisualStyles is the first thing to do 
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


            //Register Global Application Eventhandler
            Application.ApplicationExit += new EventHandler(Application_ApplicationExit);
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            Application.ThreadExit += new EventHandler(Application_ThreadExit);

            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            //Show the Splashscreen 

//#if DEBUG
            UIHelpers.Splasher.Show(false);

//#else
//            UIHelpers.Splasher.Show(Properties.Settings.Default.ShowStartupScreen);
//#endif

            //InitializationSteps
            

            // Step {1}

            ShowLoadAppModulMessages("Loading ApplicationPanels ...");

            ModulManager.ModulSearchStatus += new ModulSearchStatusHandler(Program.ShowLoadAppModulMessages);
            ModulManager.LoadModules();

            // Step {2}
#if DEBUG
            ShowLoadAppModulMessages("Step 2 ...");
#endif

#if DEBUG
            ShowLoadAppModulMessages("Do some initial procedures ...");
#endif

#if DEBUG
            UIHelpers.Splasher.Close();
#if ONLINEDATA 
            MessageBox.Show("Achtung Sie arbeiten mit Echtdaten !!!");
#endif
#endif


            ShowLoadAppModulMessages("initialize XPhone-server resources and starting the mainform ...");
            Program.mainForm = new MainForm();

            //Schließen des Splashscreens wenn alle Initialisierungen durchgeführt wurden
            UIHelpers.Splasher.Close();

            if (Environment.CurrentDirectory.EndsWith("metacall-test"))
                MessageBox.Show("Achtung!\nSie arbeiten mit einer Test-Version!" +
                    "\nBitte verwenden Sie diese Version nur nach Abstimmung mit der Center-Administration!",
                    "Test-Version", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);

            Application.Run(mainForm);

        }

        static void ShowLoadAppModulMessages(string message)
        {
            UIHelpers.Splasher.Status = message;
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.ExceptionPolicy.HandleException((Exception)e.ExceptionObject, "UI Policy");
        }

        static void Application_ThreadExit(object sender, EventArgs e)
        {
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {

            MetaCall.Business.Dialer.ShutDown();
            Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.ExceptionPolicy.HandleException(e.Exception, "UI Policy");            
            Application.Exit();
        }

        static void Application_ApplicationExit(object sender, EventArgs e)
        {
        }
    }
}