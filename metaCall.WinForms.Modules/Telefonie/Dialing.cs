using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using metatop.Applications.metaCall.DataObjects;
using metatop.Applications.metaCall.BusinessLayer;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace metatop.Applications.metaCall.WinForms.Modules.Telefonie
{
    public partial class Dialing : Form
    {
        private Call call;
        private int hangUpCounter;
        private bool isDialing;

        private Timer hangUpTimer;
        
        public Dialing()
        {
            InitializeComponent();
        }

        public Dialing(Call call)
        {

            InitializeComponent();
            this.call = call;
            this.hangUpTimer = new Timer();
            this.hangUpTimer.Enabled = false;
            this.hangUpTimer.Interval = 1000;
            this.hangUpTimer.Tick += new EventHandler(hangUpTimer_Tick);
            this.informationLabel.Text = string.Empty;

            MetaCall.Business.Dialer.Connected += new DialingEventHandler(Dialer_Connected);
            MetaCall.Business.Dialer.WantConnect += new DialingEventHandler(Dialer_WantConnect);
            MetaCall.Business.Dialer.HangedUp += new DialingEventHandler(Dialer_HangedUp);

            HandleCall(call);

            Application.Idle += new EventHandler(Application_Idle);
        }

        void Dialer_WantConnect(object sender, DialingEventArgs e)
        {

            if (this.InvokeRequired)
            {
                this.BeginInvoke(new EventHandler<DialingEventArgs>(this.Dialer_WantConnect), new object[] { sender, e });
                return;
            }

            //Wenn freizeichen da ... 
            if (e.State == DialStates.DialTone)
            {
                InitTimer();
                this.informationLabel.Text = string.Format("Anwahlversuch {0} ..." , e.PhoneNumber);
            }

            if (e.State == DialStates.RingBack)
            {
                InitTimer();
                this.informationLabel.Text = string.Format("Anwahlversuch {0} es klingelt ..." ,e.PhoneNumber);
            }

            
        }

        void hangUpTimer_Tick(object sender, EventArgs e)
        {
            if (this.hangUpCounter <= 0)
            {
                HangUp();
            }
            else
            {
                this.hangUpCounter--;
                UpdateUI();
            }

        }

        void Dialer_HangedUp(object sender, DialingEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new EventHandler<DialingEventArgs>(this.Dialer_HangedUp), new object[] { sender, e });
                return;
            }

            this.informationLabel.Text = "";
            this.isDialing = false;
            UpdateUI();
        }

        void Dialer_Connected(object sender, DialingEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new EventHandler<DialingEventArgs>(this.Dialer_Connected), new object[] { sender, e });
                return;
            }

            this.isDialing = false;
            this.DialogResult = DialogResult.OK;
            Close();
        }

        public string PhoneNumber
        {
            get
            {
                return this.phoneNumberTextBox.Text;
            }
        }

        void Application_Idle(object sender, EventArgs e)
        {
           // UpdateUI();
        }

        private void UpdateUI()
        {
            if (this.isDialing)
            {
                if (this.hangUpTimer.Enabled)
                    this.dialHangUpButton.Text = string.Format("auflegen nach {0} sek", this.hangUpCounter);
                else
                    this.dialHangUpButton.Text = "auflegen";
                
                this.dialHangUpButton.Enabled = true;
                this.cancelButton.Enabled = false;
            }
            else
            {
                this.dialHangUpButton.Text = "wählen";
                this.dialHangUpButton.Enabled = !string.IsNullOrEmpty(this.phoneNumberTextBox.Text);
                this.cancelButton.Enabled = true;
            }
        }

        private void HandleCall(Call call)
        {
            
            switch (call.DialMode)
            {
                case DialMode.ManualDialing:
                    this.phoneNumberTextBox.ReadOnly = false;
                    this.phoneNumberTextBox.Text = call.PhoneNumber;
                    this.dialHangUpButton.Text = "Wählen";
                    this.dialHangUpButton.Enabled = true;
                    break;
                case DialMode.AutoSoftwareDialing:
#if BETA || DEBUG                         
                    this.phoneNumberTextBox.ReadOnly = false;
#endif
                    this.phoneNumberTextBox.Text = call.PhoneNumber;
                    this.dialHangUpButton.Text = "Wählen";
                    this.dialHangUpButton.Enabled = true;
                    
#if !(BETA || DEBUG)
                        Dial();
#endif
                    break;
                case  DialMode.AutoDialingImmediately:
                    this.phoneNumberTextBox.ReadOnly = false;
                    this.phoneNumberTextBox.Text = call.PhoneNumber;

                    Dial();
                    break;
            }
        }

        private void Dial()
        {

            string phoneNumber = this.phoneNumberTextBox.Text;
            try
            {
                InitTimer();
                MetaCall.Business.Dialer.Dial(phoneNumber, this.call);
            }
            catch (LineNotConnectedException)
            {
                ResetTimer();
            }
            catch (LineNotFreeException)
            {
                ResetTimer();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "UI Policy");
                if (rethrow)
                    throw;
            }
            finally
            {
                this.isDialing = true;
                UpdateUI();
            }
        }

        private void InitTimer()
        {

            if (this.hangUpTimer.Enabled)
                this.hangUpTimer.Stop();

            this.hangUpCounter = 15;
            this.hangUpTimer.Start();
        }

        private void ResetTimer()
        {
            this.hangUpTimer.Stop();
        }

        private void HangUp()
        {

            try
            {
                MetaCall.Business.Dialer.HangUp();
            }
            catch (LineNotConnectedException)
            {
                ;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "UI Policy");
                if (rethrow)
                    throw;
            }
            finally
            {
                ResetTimer();
                this.isDialing = false;
                UpdateUI();
            }
        }

        private void dialHangUpButton_Click(object sender, EventArgs e)
        {
            if (isDialing)
                HangUp();
            else
                Dial();
        }

        private void Dialing_FormClosed(object sender, FormClosedEventArgs e)
        {

            MetaCall.Business.Dialer.HangedUp -= new DialingEventHandler(this.Dialer_HangedUp);
            MetaCall.Business.Dialer.WantConnect -= new DialingEventHandler(Dialer_WantConnect); 
            MetaCall.Business.Dialer.Connected -= new DialingEventHandler(this.Dialer_Connected);
            Application.Idle -= new EventHandler(this.Application_Idle);

        }

    }
}