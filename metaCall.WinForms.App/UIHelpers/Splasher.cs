using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace metatop.Applications.metaCall.WinForms.App.UIHelpers
{
    public static class Splasher
    {
        static SplashForm MySplashForm;
        static Thread MySplashThread;
        static bool TopMost = true;

        //	internally used as a thread function - showing the form and
        //	starting the messageloop for it
        private static void ShowThread()
        {
            MySplashForm = new SplashForm();
            MySplashForm.StatusInfo = "";
            MySplashForm.TopMost = Splasher.TopMost;
            Application.Run(MySplashForm);
        }

        static public void Show(bool topMost)
        {

            Splasher.TopMost = topMost;
            Splasher.Show();

        }

        //	public Method to show the SplashForm
        static public void Show()
        {
            if (MySplashThread != null)
                return;

            MySplashThread = new Thread(new ThreadStart(Splasher.ShowThread));
            MySplashThread.IsBackground = true;
            //MySplashThread.TrySetApartmentState(ApartmentState.STA);
            MySplashThread.SetApartmentState(ApartmentState.STA);
            MySplashThread.Start();
            
            //Sleep the CurrentThread 
            Thread.Sleep(500);
        }

        //	public Method to hide the SplashForm
        static public void Close()
        {
            if (MySplashThread == null) return;
            if (MySplashForm == null) return;

            try
            {
                MySplashForm.Invoke(new MethodInvoker(MySplashForm.Close));
            }
            catch (Exception)
            {
                throw;
            }
            MySplashThread = null;
            MySplashForm = null;
        }

        //	public Method to set or get the loading Status
        static public string Status
        {
            set
            {
                if (MySplashForm == null)
                {
                    return;
                }

                MySplashForm.StatusInfo = value;
            }
            get
            {
                if (MySplashForm == null)
                {
                    throw new InvalidOperationException("Splash Form not on screen");
                }
                return MySplashForm.StatusInfo;
            }
        }
    }
}
