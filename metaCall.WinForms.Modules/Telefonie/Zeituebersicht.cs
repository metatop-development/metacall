using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using metatop.Applications.metaCall.BusinessLayer;
using metatop.Applications.metaCall.DataObjects;

namespace metatop.Applications.metaCall.WinForms.Modules.Telefonie
{
    [ToolboxItem(false)]
    public partial class Zeituebersicht : UserControl, ISupportInitialize
    {

        WTListener wtListener;
        PTListener ptListener;
        PausenListener pausenListener;
        UTListener utListener;
        TTListener ttListener;
        ATListener atListener;
        DTListener dtListener;

        enum TimerFontStyle
        {
            Actively,
            InactivelyTime,
            InactivelyNoTime
        }
        
        public Zeituebersicht()
        {
            InitializeComponent();


            if (!initialize)
            {
                // Abrufen der Listener
                this.wtListener = MetaCall.Business.ActivityLogger.GetListener(typeof(WTListener)) as WTListener;
                this.ptListener = MetaCall.Business.ActivityLogger.GetListener(typeof(PTListener)) as PTListener;
                this.pausenListener = MetaCall.Business.ActivityLogger.GetListener(typeof(PausenListener)) as PausenListener;
                this.utListener = MetaCall.Business.ActivityLogger.GetListener(typeof(UTListener)) as UTListener;
                this.ttListener = MetaCall.Business.ActivityLogger.GetListener(typeof(TTListener)) as TTListener;
                this.atListener = MetaCall.Business.ActivityLogger.GetListener(typeof(ATListener)) as ATListener;
                this.dtListener = MetaCall.Business.ActivityLogger.GetListener(typeof(DTListener)) as DTListener;
            }

        }

        // blau --> System.Drawing.Color.CornflowerBlue
        

        private void TimerStyle(Label labelDelivery, TimerFontStyle timerFontStyle)
        {
            if (timerFontStyle == TimerFontStyle.Actively)
            {
                labelDelivery.Font = new Font(labelDelivery.Font, FontStyle.Bold );
                labelDelivery.ForeColor = System.Drawing.Color.CornflowerBlue;
            }
            else if (timerFontStyle == TimerFontStyle.InactivelyNoTime)
            {
                labelDelivery.Font = new Font(labelDelivery.Font, FontStyle.Regular);
                labelDelivery.ForeColor = System.Drawing.Color.LightGray;
            }
            else if (timerFontStyle == TimerFontStyle.InactivelyTime)
            {
                labelDelivery.Font = new Font(labelDelivery.Font, FontStyle.Regular);
                labelDelivery.ForeColor = System.Drawing.Color.Black;
            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (wtListener != null)
            {
                if (wtListener.IsRunning == true) 
                {
                    TimerStyle(this.lblArbeitszeit, TimerFontStyle.Actively);
                }
                else if ((wtListener.IsRunning == false) && (wtListener.Elapsed.Milliseconds > 0)) 
                {
                    TimerStyle(this.lblArbeitszeit, TimerFontStyle.InactivelyTime);
                }
                else if ((wtListener.IsRunning == false) && (wtListener.Elapsed.Milliseconds == 0))
                {
                    TimerStyle(this.lblArbeitszeit, TimerFontStyle.InactivelyNoTime);
                }
                this.lblArbeitszeit.Text = FormatTimeSpan(wtListener.Elapsed);
            }

            if (dtListener != null)
            {
                if (dtListener.IsRunning == true)
                {
                    TimerStyle(this.lblMahnzeit, TimerFontStyle.Actively);
                }
                else if ((dtListener.IsRunning == false) && (dtListener.Elapsed.Milliseconds > 0))
                {
                    TimerStyle(this.lblMahnzeit, TimerFontStyle.InactivelyTime);
                }
                else if ((dtListener.IsRunning == false) && (dtListener.Elapsed.Milliseconds == 0))
                {
                    TimerStyle(this.lblMahnzeit, TimerFontStyle.InactivelyNoTime);
                }
                this.lblMahnzeit.Text = FormatTimeSpan(dtListener.Elapsed);
            }


            if (ptListener != null)
            {
                if (ptListener.IsRunning == true)
                {
                    TimerStyle(this.lblProjektzeit, TimerFontStyle.Actively);
                }
                else if ((ptListener.IsRunning == false) && (ptListener.Elapsed.Milliseconds > 0))
                {
                    TimerStyle(this.lblProjektzeit, TimerFontStyle.InactivelyTime);
                }
                else if ((ptListener.IsRunning == false) && (ptListener.Elapsed.Milliseconds == 0))
                {
                    TimerStyle(this.lblProjektzeit, TimerFontStyle.InactivelyNoTime);
                }

                this.lblProjektzeit.Text = FormatTimeSpan(ptListener.Elapsed);
            }

            if (pausenListener != null)
            {
                if (pausenListener.IsRunning == true)
                {
                    TimerStyle(this.lblPausen, TimerFontStyle.Actively);
                }
                else if ((pausenListener.IsRunning == false) && (pausenListener.Elapsed.Milliseconds > 0))
                {
                    TimerStyle(this.lblPausen, TimerFontStyle.InactivelyTime);
                }
                else if ((pausenListener.IsRunning == false) && (pausenListener.Elapsed.Milliseconds == 0))
                {
                    TimerStyle(this.lblPausen, TimerFontStyle.InactivelyNoTime);
                }

                
                this.lblPausen.Text = FormatTimeSpan(pausenListener.Elapsed);
            }

            if (utListener != null)
            {
                if (utListener.IsRunning == true)
                {
                    TimerStyle(this.lblUnbestimmt, TimerFontStyle.Actively);
                }
                else if ((utListener.IsRunning == false) && (utListener.Elapsed.Milliseconds > 0))
                {
                    TimerStyle(this.lblUnbestimmt, TimerFontStyle.InactivelyTime);
                }
                else if ((utListener.IsRunning == false) && (utListener.Elapsed.Milliseconds == 0))
                {
                    TimerStyle(this.lblUnbestimmt, TimerFontStyle.InactivelyNoTime);
                }

                this.lblUnbestimmt.Text = FormatTimeSpan(this.utListener.Elapsed);
            }

            if (ttListener != null)
            {
                if (ttListener != null)
                {
                    if (ttListener.IsRunning == true)
                    {
                        TimerStyle(this.lblTelefonzeit, TimerFontStyle.Actively);
                    }
                    else if ((ttListener.IsRunning == false) && (ttListener.Elapsed.Milliseconds > 0))
                    {
                        TimerStyle(this.lblTelefonzeit, TimerFontStyle.InactivelyTime);
                    }
                    else if ((ttListener.IsRunning == false) && (ttListener.Elapsed.Milliseconds == 0))
                    {
                        TimerStyle(this.lblTelefonzeit, TimerFontStyle.InactivelyNoTime);
                    }

                    this.lblTelefonzeit.Text = FormatTimeSpan(this.utListener.Elapsed);
                }

                this.lblTelefonzeit.Text = FormatTimeSpan(this.ttListener.Elapsed);
            }

            if (atListener != null)
            {
                if (atListener.IsRunning == true)
                {
                    TimerStyle(this.lblNacharbeit, TimerFontStyle.Actively);
                }
                else if ((atListener.IsRunning == false) && (atListener.Elapsed.Milliseconds > 0))
                {
                    TimerStyle(this.lblNacharbeit, TimerFontStyle.InactivelyTime);
                }
                else if ((atListener.IsRunning == false) && (atListener.Elapsed.Milliseconds == 0))
                {
                    TimerStyle(this.lblNacharbeit, TimerFontStyle.InactivelyNoTime);
                }

                this.lblNacharbeit.Text = FormatTimeSpan(this.utListener.Elapsed);
            }

            this.lblNacharbeit.Text = FormatTimeSpan(this.atListener.Elapsed);
        }


        private string FormatTimeSpan(TimeSpan timespan)
        {

            string formatString = "{0}:{1:00}:{2:00}";

            return string.Format(System.Globalization.CultureInfo.InvariantCulture, formatString, (int)timespan.TotalHours, timespan.Minutes, timespan.Seconds);
        }

        private void Zeituebersicht_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;
            try
            {
                //Starten des Zeitgebers
                timer1.Interval = 500;
                timer1.Start();
            }
            catch (Exception){}

        }



        #region ISupportInitialize Member

        bool initialize = true;
        public void BeginInit()
        {
            initialize = true;
        }

        public void EndInit()
        {
            initialize = false;

            if (!DesignMode)
            {
                // Abrufen der Listener
                this.wtListener = MetaCall.Business.ActivityLogger.GetListener(typeof(WTListener)) as WTListener;
                this.ptListener = MetaCall.Business.ActivityLogger.GetListener(typeof(PTListener)) as PTListener;
                this.pausenListener = MetaCall.Business.ActivityLogger.GetListener(typeof(PausenListener)) as PausenListener;
                this.utListener = MetaCall.Business.ActivityLogger.GetListener(typeof(UTListener)) as UTListener;
                this.ttListener = MetaCall.Business.ActivityLogger.GetListener(typeof(TTListener)) as TTListener;
                this.atListener = MetaCall.Business.ActivityLogger.GetListener(typeof(ATListener)) as ATListener;
                this.dtListener = MetaCall.Business.ActivityLogger.GetListener(typeof(DTListener)) as DTListener;
            }
        }

        #endregion

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
