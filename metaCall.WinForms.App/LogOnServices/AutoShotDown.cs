using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using metatop.Applications.metaCall.DataObjects;

namespace metatop.Applications.metaCall.WinForms.App.LogOnServices
{
    public partial class AutoShotDown : Form
    {
        //Konstante aus Settings holen

        int countdownSec ;
        int countdown ;

        private int Countdown
        {
            get { return countdown; }
            set
            { 
                this.countdown = value  ;
            }
        }

        public AutoShotDown()
        {
            InitializeComponent();

            //Timer-Interval auf eine Sekunde stellen
            this.timerShotDown.Interval = 1000 ;

            Setting setting = MetaCall.Business.Settings.GetSetting();

            this.countdownSec = setting.ShutDownCountdownSec;
            this.countdown = countdownSec;

            ProgressBarInitialisieren();
            // Enable timer.
            this.timerShotDown.Enabled = true;
        }

        private void ProgressBarInitialisieren()
        {
                //Progressbar initialisieren
                this.progressBarShutDownTime.Minimum = 0;
                this.progressBarShutDownTime.Maximum = countdownSec;
                this.progressBarShutDownTime.Value = countdownSec;
                this.progressBarShutDownTime.Visible = true;
        }

        public void ProgressBarAktualisieren()
        {
            if (Countdown < 0)
            {
                this.timerShotDown.Enabled = false;
                this.DialogResult = DialogResult.OK;
                this.Close();
                return;
            }
            this.progressBarShutDownTime.Value = Countdown;
        }

        private void timerShotDown_Tick(object sender, EventArgs e)
        {
            Countdown--;
            ProgressBarAktualisieren();
        }

    }
}