using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using metatop.Applications.metaCall.BusinessLayer;
using metatop.Applications.metaCall.DataObjects;

using MaDaNet.Common.AppFrameWork.Validation;

using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Globalization;

namespace metatop.Applications.metaCall.WinForms.Modules.Telefonie
{
    [ToolboxItem(false)]
    public partial class PhoneView : UserControl
    {
        public event PhoneViewEventHandler Save;
        public event PhoneViewEventHandler Cancel;
        //public event SponsorInfoChangeEventHandler SponsorInfoChanged;

        private UserInfo reminderUser;

        private Call call;

        private int nonConnectionTime;
        private int abortDialingTime;

        public PhoneView()
        {
            InitializeComponent();

            this.createContactTypeReminder1.ContactTypeChanged += new EventHandler<ContactTypeChangedEventArgs>(createContactTypeReminder1_ContactTypeChanged);
            this.createContactTypeDurringReminder1.ContactTypeChanged += new EventHandler<ContactTypeChangedEventArgs>(createContactTypeDurringReminder1_ContactTypeChanged);

            this.createCallNotice1.SponsorInfoChange += new EventHandler<SponsorInfoChangeEventArgs>(OnSponsorInfoChanged);
            this.createContactTypeReminder1.SponsorPhoneChanged += new SponsorPhoneChangeEventHandler(OnSponsorPhoneChanged);
            //SponsorInfoChanged += new SponsorInfoChangeEventHandler(OnSponsorInfoChanged);
            

            this.createReminderQuestion1.ReminderQuestionChanged += new EventHandler<ReminderQuestionChangeEventArgs>(createReminderQuestion1_ReminderQuestionChanged);
            this.createReminderDurringQuestion1.ReminderQuestionDurringChanged += new EventHandler<ReminderQuestionDurringChangeEventArgs>(createReminderDurringQuestion1_ReminderDurringQuestionChanged);
            Application.Idle += new EventHandler(Application_Idle);
            MetaCall.Business.Dialer.Connected += new DialingEventHandler(Dialer_Connected);
            MetaCall.Business.Dialer.HangedUp += new DialingEventHandler(Dialer_HangedUp);
            MetaCall.Business.Dialer.WantConnect += new DialingEventHandler(Dialer_WantConnect);
            //MetaCall.Business.Dialer.ExternalTimerStopped += new ExternalTimersStopEventHandler(Dialer_RealCallTimerStopped);

            Setting setting = MetaCall.Business.Settings.GetSetting();
            abortDialingTime = setting.AbortDialingTime;
            nonConnectionTime = 0;
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
            e.Control.MouseMove += new MouseEventHandler(ctl_MouseMove);

            base.OnControlAdded(e);
        }

        private void ctl_MouseMove(object sender, MouseEventArgs e)
        {
            Point mouse = PointToClient(new Point(Control.MousePosition.X, Control.MousePosition.Y));


            MouseEventArgs mea = new MouseEventArgs(Control.MouseButtons,
                e.Clicks, mouse.X, mouse.Y, e.Delta);

            this.OnMouseMove(mea);
        }

        private void Dialer_HangedUp(object sender, DialingEventArgs e)
        {

            if (this.InvokeRequired)
            {
                this.BeginInvoke(new EventHandler<DialingEventArgs>(this.Dialer_HangedUp), new object[] { sender, e });
                return;
            }
            this.RealCallTimer.Stop();
            nonConnectionTime = 0;
            this.dialButton.Text = "Wählen";
        }

        private void Dialer_Connected(object sender, DialingEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new EventHandler<DialingEventArgs>(this.Dialer_Connected), new object[] { sender, e });
                return;
            }
            this.RealCallTimer.Stop();
            nonConnectionTime = 0;
            this.dialButton.Text = "Auflegen";
        }

        void Dialer_WantConnect(object sender, DialingEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new EventHandler<DialingEventArgs>(this.Dialer_WantConnect), new object[] { sender, e });
                return;
            }
            this.dialButton.Text = "Auflegen";
        }

        //private void Dialer_RealCallTimerStopped(object sender, EventArgs e)
        //{
        //    if (this.RealCallTimer.Enabled == true)
        //    {
        //        this.RealCallTimer.Stop(); // = false;
        //        nonConnectionTime = 0;
        //    }
        //}

        private void Application_Idle(object sender, EventArgs e)
        {
            if (!MetaCall.Business.Pause)
            {
                DialStates dialstate = MetaCall.Business.Dialer.State;

                this.dialButton.Enabled = ((this.call != null) &&
                                           (dialstate == DialStates.Ready || dialstate == DialStates.Connected
                                           || dialstate == DialStates.DialTone || dialstate == DialStates.RingBack));
            }
        }

        private void createReminderQuestion1_ReminderQuestionChanged(object sender, ReminderQuestionChangeEventArgs e)
        {
            this.createReminderQuestion1.ReminderAnswer = string.Empty;
            switch (e.ReminderQuestion.ToString())
            {
                case "Ja":
                    //
                    SetCallJobReminder();
                    break;
                case "Nein":
                    //
                    createReminder1.Visible = false;
                    break;
            }
        }

        private void createReminderDurringQuestion1_ReminderDurringQuestionChanged(object sender, ReminderQuestionDurringChangeEventArgs e)
        {
            this.createReminderDurringQuestion1.ReminderAnswer = string.Empty;
            int differenceHeight;
            switch (e.ReminderQuestion.ToString())
            {
                case "Ja":
                    //
                    SetCallJobDurringReminder();

                    int ctdrHeight = 55;
                    differenceHeight = this.createContactTypeDurringReminder1.FormHeight; // -ctdrHeight;

                    this.createContactTypeDurringReminder1.Height = 0;
                    this.invoiceInfo1.Top = this.invoiceInfo1.Top - differenceHeight;
                    this.invoiceInfo1.Height = this.invoiceInfo1.Height - 50;
                    this.createReminderDurring1.Top = this.createReminderDurring1.Top - differenceHeight - 50;
                    this.lblUweDurring.Top = this.createReminderDurring1.Top;
                    this.createReminderDurringQuestion1.Top = this.createReminderDurringQuestion1.Top - differenceHeight - 50;
                    Point lastControlLeftBottomPosition = new Point(this.createReminderDurring1.Left - 10, this.createReminderDurring1.Bottom);
                    SetCreateNoticeControlPosition(lastControlLeftBottomPosition);

                    break;
                case "Nein":
                    //
                    createReminderDurring1.Visible = false;
                    lblUweDurring.Visible = false;

                    differenceHeight = this.createContactTypeDurringReminder1.FormHeight; // -this.createContactTypeDurringReminder1.Height; ;
                    this.createContactTypeDurringReminder1.Height = this.createContactTypeDurringReminder1.FormHeight;
                    this.invoiceInfo1.Top = this.invoiceInfo1.Top + differenceHeight;
                    this.invoiceInfo1.Height = this.invoiceInfo1.Height + 50;
                    this.createReminderDurring1.Top = this.createReminderDurring1.Top + differenceHeight + 50;
                    this.lblUweDurring.Top = this.createReminderDurring1.Top;

                    this.createReminderDurringQuestion1.Top = this.createReminderDurringQuestion1.Top + differenceHeight + 50;
                    Point lastBottomPosition = new Point(this.createReminderDurringQuestion1.Left - 10, this.createReminderDurringQuestion1.Bottom);
                    SetCreateNoticeControlPosition(lastBottomPosition);

                    break;
            }
        }

        private void createContactTypeDurringReminder1_ContactTypeChanged(object sender, ContactTypeChangedEventArgs e)
        {
            // MessageBox.Show(e.ContactType.DisplayName);
            switch (e.ContactType.DisplayName)
            {
                case "Nicht erreichbar":
                    SetCallJobDurringReminderQuestion();
                    break;
                case "Gespräch möglich":
                    SetCallJobDurringPossibleReminder();
                    break;
                case "Besetzt":
                    SetCallJobDurringReminderQuestion();
                    break;
                case "Anrufbeantworter":
                    SetCallJobDurringReminderQuestion();
                    break;
                case "keine Zahlung":
                    SetCallJobDurringClosure();
                    break;
            }
        }

        private void createContactTypeReminder1_ContactTypeChanged(object sender, ContactTypeChangedEventArgs e)
        {
            // MessageBox.Show(e.ContactType.DisplayName);
            switch (e.ContactType.DisplayName)
            {
                case "Nicht erreichbar":
                    SetCallJobReminderQuestion();
                    break;
                case "Adresse doppelt":
                    SetReminderVisibleFalse();
                    createCallPossibleReminder1.Visible = false;
                    break;
                case "Gespräch möglich":
                    SetCallJobPossibleReminder();
                    break;
                case "Besetzt":
                    SetCallJobReminderQuestion();
                    break;
                case "Nummer falsch":
                    SetReminderVisibleFalse();
                    createCallPossibleReminder1.Visible = false;
                    break;
                case "Anrufbeantworter":
                    SetCallJobReminderQuestion();
                    break;
                case "Adresse nicht geeignet":
                    SetCallJobReminderUnsuitable();
                    break;
            }

            this.sponsorInfo1.TelefonNummer = this.createContactTypeReminder1.TelefonNummer;
            this.sponsorInfo1.Mobilnummer = this.createContactTypeReminder1.MobilNummer;
        }

        public void InitCall(Call call)
        {

            if (DesignMode) return;

            call.CallJob.Sponsor.PropertyChanged += new PropertyChangedEventHandler(Sponsor_PropertyChanged);


            Customer customer = MetaCall.Business.Addresses.GetCustomer(call.CallJob.Project);

            this.call = call;

            //Initialisierung der untergeordneten Steuerelemente
            foreach (Control ctl in this.Controls)
            {
                // Alle Steuerelemente die IInitializeCall unterstützen werden mit dem aktuellen 
                // Call initialisiert
                IInitializeCall initializeCallControl = ctl as IInitializeCall;
                if (initializeCallControl != null)
                {
                    initializeCallControl.InitializeCall(call);
                }


                // Alle Steuerelemente die IInitializeCustomer unterstützen werden mit dem aktuellen 
                // Customer initialisiert
                IInitializeCustomer initializeCustomerControl = ctl as IInitializeCustomer;
                if (initializeCustomerControl != null)
                {
                    initializeCustomerControl.InitializeCustomer(customer);
                }
            }
            
            if (MetaCall.Business.CallJobs.DurringActiv == true  || call.CallJob.GetType() == typeof(DurringCallJob))
            {
                this.createContactTypeDurringReminder1.Visible = true;
                this.createContactTypeReminder1.Visible = false;
                this.createCallNotice1.Visible = false;
                this.createCallDurringNotice1.Visible = true;
                this.invoiceInfo1.Visible = true;
            }
            else
            {
                this.createContactTypeDurringReminder1.Visible = false;
                this.createContactTypeReminder1.Visible = true;
                this.createCallNotice1.Visible = true;
                this.createCallDurringNotice1.Visible = false;
                this.invoiceInfo1.Visible = false;
            }

            this.createReminder1.Visible = false;
            this.createReminderDurring1.Visible = false;
            this.createReminderQuestion1.Visible = false;
            this.createCallPossibleReminder1.Visible = false;
            this.createReminderQuestion1.Visible = false;
            this.createReminderDurringQuestion1.Visible = false;
            this.createReminderUnsuitable1.Visible = false;
            this.createDurringClosure1.Visible = false;

            windowDesign();

            if (MetaCall.Business.Projects.Current == null ||
                !call.CallJob.Project.ProjectId.Equals(MetaCall.Business.Projects.Current.ProjectId))
            {
                if (MetaCall.Business.CallJobs.DurringActiv == false)
                    NotifyUserToCallAnotherProject();
            }

            //if (MetaCall.Business.CallJobs.DurringActiv == true)
            //{
            //    DurringCallJob dCJ = (DurringCallJob)call.CallJob;
            //    if (dCJ.Invoice.IstBezahlt == true)
            //    {
            //        //Nicht schön die Lösung da wieder neu gestartet werden muss.
            //        NotifyUserIfTheInvoiceNotPaid();
            //        MetaCall.Business.SponsoringCallManager.RemoveDurringCallFromList(call);
            //        OnCancel(new PhoneViewEventArgs(CallJobResultMessage.CancelledResult));
            //        return;
            //    }
            //}

            //Dunningteil gilt für WVs die während dem normalen Telefonieren
            //auftreten. In diesem Falls is DunningActive = false
            if (call.CallJob.GetType() == typeof(DurringCallJob) && MetaCall.Business.CallJobs.DurringActiv == false)
            {
                MetaCall.Business.ActivityLogger.Log(new BusinessLayer.Activities.DurringChanged(!MetaCall.Business.CallJobs.DurringActiv));
            }
            else
            {
                //Ereignis loggen
                MetaCall.Business.ActivityLogger.Log(new BusinessLayer.Activities.NewCustomer(this.call));
            }
            //Anwahl durchführen
            if (MetaCallPrincipal.Current.IsInRole(MetaCallPrincipal.AdminRoleName))
            {
                this.okButton.Visible = false;
            }
            else
            {
                this.okButton.Visible = true;
                if (MetaCallPrincipal.Current.Identity.User.DialMode == DialMode.AutoDialingImmediately)
                    Dial();
            }
        }

        /// <summary>
        /// Führt die Anwahl für den aktuellen Call durch
        /// </summary>
        public void Dial()
        {

            this.call.PhoneNumber = this.UsedPhoneNumber;

            //In der aktuellen Version wird kein Dialog angezeigt 
//#if !DEBUG && !BETA
            try
            {
                nonConnectionTime = 0;
                this.RealCallTimer.Start();
                MetaCall.Business.Dialer.Dial(this.UsedPhoneNumber, this.call);
                //return;
            }
            catch (LineNotFreeException ex)
            {
                System.Windows.Forms.MessageBox.Show("Es steht momentan keine freie Leitung " +
                    "zur Verfügung!", "keine freie Leitung");
                this.HangUp();
            }
            catch (LineNotConnectedException ex)
            {
                System.Windows.Forms.MessageBox.Show("Es kann keine Leitung gefunden werden!",
                    "Leitung verloren ;-)");
                this.HangUp();
            }


//#endif

            /*using (Dialing dialingDlg = new Dialing(call))
            {
                if (dialingDlg.ShowDialog(this) == DialogResult.OK)
                {
                    //TODO: PhoneView vorbelegen
                    call.PhoneNumber = dialingDlg.PhoneNumber;
                    //this.UsedPhoneNumber = dialingDlg.PhoneNumber;
                    if (MetaCall.Business.CallJobs.DurringActiv == true)
                        this.createContactTypeDurringReminder1.DialerNummer = dialingDlg.PhoneNumber;
                    else
                        this.createContactTypeReminder1.DialerNummer = dialingDlg.PhoneNumber;
                }
                else
                {
                    //TODO: PhoneView vorbelegen
                    //Wenn abgebochen wurde wird das PhoneView 
                    //Wert nicht erreichbar vorbelegt mit 
                }
                // Formular schlißet sich selbst
            } */

        }

        private void HangUp()
        {
            this.RealCallTimer.Stop();
            nonConnectionTime = 0;
            MetaCall.Business.Dialer.HangUp();
        }

        private void Sponsor_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            try
            {
                if (e.PropertyName == "Branch")
                    this.createCallNotice1.SelectedBranch = this.call.CallJob.Sponsor.Branch;

                if (e.PropertyName == "BranchGroup")
                    this.createCallNotice1.SelectedBranchGroup = this.call.CallJob.Sponsor.BranchGroup;

                if (e.PropertyName == "TelefonNummer")
                {
                    if (this.createContactTypeReminder1.UsedPhoneNumber ==
                            this.createContactTypeReminder1.TelefonNummer)
                    {
                        this.createContactTypeReminder1.TelefonNummer = this.call.CallJob.Sponsor.TelefonNummer;
                        this.createContactTypeReminder1.UpdateCurrentNumber();
                        call.PhoneNumber = this.createContactTypeReminder1.UsedPhoneNumber;
                    }
                    else
                        this.createContactTypeReminder1.TelefonNummer = this.call.CallJob.Sponsor.TelefonNummer;
                }
                if (e.PropertyName == "MobilNummer")
                    if (this.createContactTypeReminder1.UsedPhoneNumber ==
                            this.createContactTypeReminder1.MobilNummer)
                    {
                        this.createContactTypeReminder1.MobilNummer = this.call.CallJob.Sponsor.MobilNummer;
                        this.createContactTypeReminder1.UpdateCurrentNumber();
                        call.PhoneNumber = this.createContactTypeReminder1.UsedPhoneNumber;
                    }
                    else
                        this.createContactTypeReminder1.MobilNummer = this.call.CallJob.Sponsor.MobilNummer;

                if (e.PropertyName == "Additions")
                    if (this.createContactTypeReminder1.UsedPhoneNumber ==
                            this.createContactTypeReminder1.AlternativNummer)
                    {
                        this.createContactTypeReminder1.AlternativNummer = this.call.CallJob.Sponsor.Additions.Phone3;
                        this.createContactTypeReminder1.UpdateCurrentNumber();
                        call.PhoneNumber = this.createContactTypeReminder1.UsedPhoneNumber;
                    }
                    else
                        this.createContactTypeReminder1.AlternativNummer = this.call.CallJob.Sponsor.Additions.Phone3;
            }
            catch
            {
                //Fehlerbehandlung bei der alle Fehler ignoriert werden
                ;
            }
        }

        private void OnSponsorPhoneChanged(object sender, SponsorPhoneChangeEventArgs e)
        {
            if (e.Sponsor != null)
            {
                this.sponsorInfo1.TelefonNummer = e.Sponsor.TelefonNummer;
                this.sponsorInfo1.Mobilnummer = e.Sponsor.MobilNummer;
                this.sponsorInfo1.Alternativnummer = e.Sponsor.Additions.Phone3;
            }
            else
            {
                this.sponsorInfo1.TelefonNummer = null;
                this.sponsorInfo1.Mobilnummer = null;
                this.sponsorInfo1.Alternativnummer = null;
            }
        }

        private void OnSponsorInfoChanged(object sender, SponsorInfoChangeEventArgs e)
        {
            if (e.Sponsor != null)
            {
                this.sponsorInfo1.Branch = e.Sponsor.Branch;
                this.sponsorInfo1.BranchGroup = e.Sponsor.BranchGroup;
            }
            else
            {
                this.sponsorInfo1.Branch = null;
                this.sponsorInfo1.BranchGroup = null;
            }
        }

        private void NotifyUserIfTheInvoiceNotPaid()
        {
            MessageBoxOptions options = CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft ?
                MessageBoxOptions.RtlReading : 0;

            string msg = "Die ausgewählte Rechnung ist mittlerweile bezahlt oder storniert worden.";

            MessageBox.Show(msg, Application.ProductName,
                MessageBoxButtons.OK, MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1, options);

        }

        private void NotifyUserToCallAnotherProject()
        {
            MessageBoxOptions options = CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft ?
                MessageBoxOptions.RtlReading : 0;

            string msg = "Bitte beachten Sie, dass ein anderes Projekt als das gerade angemeldete aufgerufen wurde.";

            MessageBox.Show(msg, Application.ProductName,
                MessageBoxButtons.OK, MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1, options);

        }

        private void okButton_Click(object sender, EventArgs e)
        {
            // Zusammenstellen der Informationen
            ContactType contactType = new ContactType();

            if (MetaCall.Business.CallJobs.DurringActiv == false  &&
                    call.CallJob.GetType() != typeof(DurringCallJob))
            {
                #region StandardCall

                // Achtung die einzelenen IF-Statements schließen teilweise
                // mit return ab!!!

                contactType = createContactTypeReminder1.SelectedItem;
                #region ContactType = NULL
                if (contactType == null)
                {
                    MessageBox.Show("Sie müssen eine Kontaktart auswählen um den Call speichern zu können", "Fehlerhafte Eingabe", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                #endregion

                #region ContactType = Gespräch möglich I
                if (contactType.ContactTypeId.Equals(ContactType.GespraechMoeglichId))
                {
                    if (createCallPossibleReminder1.ContactTypeParticipation == null)
                    {
                        MessageBox.Show(this, "Bitte treffen Sie eine Auswahl über die Teilnahme an der Aktion!",
                            Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, 0);
                        return;
                    }

                    if (this.createCallPossibleReminder1.FaxNoticeRemark != string.Empty)
                        if (this.createCallNotice1.Notice != "")
                            this.createCallNotice1.Notice = this.createCallNotice1.Notice + Environment.NewLine + this.createCallPossibleReminder1.FaxNoticeRemark;
                        else
                            this.createCallNotice1.Notice = this.createCallPossibleReminder1.FaxNoticeRemark;

                }
                #endregion

                #region ContactType = Adresse nicht geeignet
                if (contactType.ContactTypeId.Equals(ContactType.AdresseNichtGeeignetId))
                {
                    if (createReminderUnsuitable1.ContactTypeParticipationUnsuitable == null)
                    {
                        MessageBox.Show(this, "Bitte wählen Sie einen Grund, warum die Adresse nicht geeignet ist!", "Fehlerhafte Eingabe", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                #endregion

                #region ContactType = Gespräch möglich und Antwort = NEIN
                //Gespräch möglich und Antwort = NEIN
                if (contactType.ContactTypeId.Equals(ContactType.GespraechMoeglichId) &&
                    createCallPossibleReminder1.ContactTypeParticipation != null &&
                    createCallPossibleReminder1.ContactTypeParticipation.ContactTypesParticipationId == ContactTypesParticipation.NeinId)
                {
                    if (createCallPossibleReminder1.ContactTypesParticipationCancellation == null)
                    {
                        MessageBox.Show("Bitte wählen Sie einen Grund für die Absage!", "Fehlerhafte Eingabe", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (createCallPossibleReminder1.SecondCallDesired == SecondCallDesiredChoice.Unset)
                    {
                        MessageBox.Show("Bitte geben Sie an, ob der Sponsor einen Zweitanruf wünscht!");
                        return;
                    }
                }
                #endregion

                //Gespräch möglich  und Antwort -> Interesse Angebot
                if (contactType.ContactTypeId.Equals(ContactType.GespraechMoeglichId) &&
                    createCallPossibleReminder1.ContactTypeParticipation != null &&
                    createCallPossibleReminder1.ContactTypeParticipation.ContactTypesParticipationId == ContactTypesParticipation.InteresseAngebot)
                {
                    if (!createCallPossibleReminder1.FaxSend)
                    {
                        MessageBox.Show("Bitte versenden Sie das Fax!","Fax noch nicht gesendet");
                        return;
                    }
                }

                if (createCallNotice1.SelectedBranch == null)
                {
                    if (contactType.ContactTypeId.Equals(ContactType.GespraechMoeglichId) &&
                        createCallPossibleReminder1.ContactTypeParticipation != null &&
                        (createCallPossibleReminder1.ContactTypeParticipation.ContactTypesParticipationId == ContactTypesParticipation.JaId
                        || createCallPossibleReminder1.ContactTypeParticipation.ContactTypesParticipationId == ContactTypesParticipation.NeinId))

                    {
                        MessageBox.Show("Bitte wählen Sie eine Branche, um den Call speichern zu können!", "Fehlerhafte Eingabe", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                else if (this.createCallNotice1.SelectedBranch.Equals(Branch.Unknown))
                {
                    //Wenn Gespräch möglich und Antwort = JA muss eine Branche ausgewählt sein.
                    if (contactType.ContactTypeId.Equals(ContactType.GespraechMoeglichId) &&
                        createCallPossibleReminder1.ContactTypeParticipation != null &&
                        (createCallPossibleReminder1.ContactTypeParticipation.ContactTypesParticipationId == ContactTypesParticipation.JaId
                        || createCallPossibleReminder1.ContactTypeParticipation.ContactTypesParticipationId == ContactTypesParticipation.NeinId))
                    {

                        MessageBox.Show("Bitte wählen Sie eine Branche, um den Call speichern zu können!", "Fehlerhafte Eingabe", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    else
                    {
                        // MessageBox.Show("Sie haben eine unbekannte Branche ausgewählt!", "Fehlerhafte Eingabe", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    call.CallJob.Sponsor.Branch = createCallNotice1.SelectedBranch;
                    call.CallJob.Sponsor.BranchGroup = createCallNotice1.SelectedBranchGroup;
                }
                else
                {
                    call.CallJob.Sponsor.Branch = createCallNotice1.SelectedBranch;
                    call.CallJob.Sponsor.BranchGroup = createCallNotice1.SelectedBranchGroup;
                }

                if (contactType.ContactTypeId.Equals(ContactType.GespraechMoeglichId) &&
                    createCallPossibleReminder1.ContactTypeParticipation != null &&
                    createCallPossibleReminder1.ContactTypeParticipation.ContactTypesParticipationId == ContactTypesParticipation.JaId)
                {
                    List<string> wrongFields;
                    bool isValid = MetaCall.Business.Addresses.IsSponsorValidForOrdering(this.call.CallJob.Sponsor, out wrongFields);
                    if (!isValid)
                    {
                        mwProject mwp = MetaCall.Business.Projects.GetMwProject(call.CallJob.Project);
                        using (SponsorEdit sponsorEditDlg = new SponsorEdit(this.call.CallJob.Sponsor, mwp.Language, true))
                        {
                            if (!(sponsorEditDlg.ShowDialog(this) == DialogResult.OK))
                            {
                                return;
                            }
                        }
                    }
                }
                call.PhoneNumber = this.createContactTypeReminder1.UsedPhoneNumber;
                //call.CallJob.Sponsor.EMail = 
                #endregion
            }
            else
            {
                #region MahnungsCall
                //Modus Durring (Mahnungsaktionen telefonieren

                contactType = createContactTypeDurringReminder1.SelectedItem;
                if (contactType == null)
                {
                    MessageBox.Show("Sie müssen eine Kontaktart auswählen um den Call speichern zu können", "Fehlerhafte Eingabe", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (contactType.ContactTypeId.Equals(ContactType.DurringKeineZahlungId))
                {
                    //if (createDurringClosure1.ClosureReason == null)
                    //{
                    //    MessageBox.Show(this, "Bitte wählen Sie einen Grund, warum die Zahlung nicht erfolgt!", "Fehlerhafte Eingabe", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //    return;
                    //}
                }

                call.PhoneNumber = this.createContactTypeDurringReminder1.UsedPhoneNumber;
                #endregion
            }

            if (!string.IsNullOrEmpty(this.createContactTypeReminder1.TelefonNummer))
            {
                call.CallJob.Sponsor.Additions.Phone1 = this.createContactTypeReminder1.TelefonNummer;
                call.CallJob.Sponsor.TelefonNummer = this.createContactTypeReminder1.TelefonNummer;
            }

            if (!string.IsNullOrEmpty(this.createContactTypeReminder1.MobilNummer))
            {
                call.CallJob.Sponsor.Additions.Phone2 = this.createContactTypeReminder1.MobilNummer;
                call.CallJob.Sponsor.MobilNummer = this.createContactTypeReminder1.MobilNummer;
            }

            if (!string.IsNullOrEmpty(this.createContactTypeReminder1.AlternativNummer))
            {
                call.CallJob.Sponsor.Additions.Phone3 = this.createContactTypeReminder1.AlternativNummer;
            }

            //prüfen ob das Telefonat bereits abgeschlossen ist
            if (MetaCall.Business.Dialer.State != DialStates.Ready)
                this.HangUp();

            // Call aktualisieren 
            call.CallDate = DateTime.Now;
            //            call.PhoneNumber = this.createContactTypeReminder1.UsedPhoneNumber;

            try
            {
                CallJobResult result = null;

                //TODO: Hier muss eine andere Möglichkeit gefunden werden die unterschiedlichen Contacttypes zu unterscheiden
                if (contactType.ContactTypeId.Equals(ContactType.NichtErreichbarId))
                {
                    if (createReminderQuestion1.ReminderQuestion == "Ja")
                    {
                        this.reminderUser = this.createReminder1.ReminderUser;

                        result = GetCallJobReminderResult(call);
                    }
                    else
                    {
                        result = GetCallJobResult<CallJobResult>(call);
                    }
                }


                if (contactType.ContactTypeId.Equals(ContactType.DurringNichtErreichbarId))
                {
                    if (createReminderDurringQuestion1.ReminderQuestion == "Ja")
                    {
                        result = GetCallJobDurringReminderResult(call);
                    }
                    else
                    {
                        result = GetCallJobDurringResult<CallJobResult>(call);
                    }
                }
                else if (contactType.ContactTypeId.Equals(ContactType.AdresseDoppeltId))
                {
                    CallJobUnsuitableResult unsuitableResult = GetCallJobUnsuitableResult(call);
                    ContactTypesParticipationUnsuitable ctpUnsuitable = new ContactTypesParticipationUnsuitable();
                    ctpUnsuitable.ContactTypesParticipationUnsuitableId = new Guid("{7F500022-097C-447A-B02F-8F2B8BAA87DC}");
                    ctpUnsuitable.DisplayName = "Adresse doppelt";
                    unsuitableResult.ContactTypesParticipationUnsuitable = ctpUnsuitable;
                    result = unsuitableResult;
                    //result = GetCallJobResult<CallJobResult>(call);

                }
                else if (contactType.ContactTypeId.Equals(ContactType.AdresseNichtGeeignetId))
                {
                    result = GetCallJobUnsuitableResult(call);

                }
                else if (contactType.ContactTypeId.Equals(ContactType.GespraechMoeglichId))
                {
                    this.reminderUser =  this.createCallPossibleReminder1.ReminderControl.ReminderUser;
                    result = GetCallJobPossibleResult(call);
                }
                else if (contactType.ContactTypeId.Equals(ContactType.DurringGespraechMoeglichId))
                {
                    result = GetCallJobDurringPossibleResult(call);
                }

                else if (contactType.ContactTypeId.Equals(ContactType.BesetztId))
                {
                    if (createReminderQuestion1.ReminderQuestion == "Ja")
                    {
                        this.reminderUser = this.createReminder1.ReminderUser;
                        result = GetCallJobReminderResult(call);
                    }
                    else
                    {
                        result = GetCallJobResult<CallJobResult>(call);
                    }
                }
                else if (contactType.ContactTypeId.Equals(ContactType.DurringBesetztId))
                {
                    if (createReminderDurringQuestion1.ReminderQuestion == "Ja")
                    {
                        result = GetCallJobDurringReminderResult(call);
                    }
                    else
                    {
                        result = GetCallJobDurringResult<CallJobResult>(call);
                    }
                }
                else if (contactType.ContactTypeId.Equals(ContactType.NummerFalschId))
                {
                    CallJobUnsuitableResult unsuitableResult = GetCallJobUnsuitableResult(call);
                    ContactTypesParticipationUnsuitable ctpUnsuitable = new ContactTypesParticipationUnsuitable();
                    ctpUnsuitable.ContactTypesParticipationUnsuitableId = new Guid("{45D708DB-106B-4FDF-9002-9C1C3E38A605}");
                    ctpUnsuitable.DisplayName = "Nummer falsch";
                    unsuitableResult.ContactTypesParticipationUnsuitable = ctpUnsuitable;
                    result = unsuitableResult;
                    //result = GetCallJobResult<CallJobResult>(call);
                }
                else if (contactType.ContactTypeId.Equals(ContactType.AnrufbeantworterId))
                {
                    if (createReminderQuestion1.ReminderQuestion == "Ja")
                    {
                        this.reminderUser = this.createReminder1.ReminderUser;
                        result = GetCallJobReminderResult(call);
                    }
                    else
                    {
                        result = GetCallJobResult<CallJobResult>(call);
                    }
                }
                else if (contactType.ContactTypeId.Equals(ContactType.DurringAnrufbeantworterId))
                {
                    if (createReminderDurringQuestion1.ReminderQuestion == "Ja")
                    {
                        result = GetCallJobDurringReminderResult(call);
                    }
                    else
                    {
                        result = GetCallJobDurringResult<CallJobResult>(call);
                    }
                }
                else if (contactType.ContactTypeId.Equals(ContactType.DurringKeineZahlungId))
                {
                    if (createReminderDurringQuestion1.ReminderQuestion == "Ja")
                    {
                        result = GetCallJobDurringReminderResult(call);
                    }
                    else
                    {
                        result = GetCallJobDurringResult<CallJobResult>(call);
                    }
                }

                CallJobResultMessage resultMessage = new CallJobResultMessage();
                resultMessage.Call = call;
                resultMessage.CallJobResult = result;

                //Dunningteil gilt für WVs die während dem normalen Telefonieren
                //auftreten. In diesem Falls is DunningActive = false
                if (call.CallJob.GetType() == typeof(DurringCallJob) && MetaCall.Business.CallJobs.DurringActiv == false)
                {
                    MetaCall.Business.ActivityLogger.Log(new BusinessLayer.Activities.DurringChanged(MetaCall.Business.CallJobs.DurringActiv));
                }
                else
                {
                    //Ereignis loggen
                    MetaCall.Business.ActivityLogger.Log(new BusinessLayer.Activities.SaveCustomer());
                }

                OnSave(new PhoneViewEventArgs(resultMessage));
            }

            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "UI Policy");
                if (rethrow)
                    throw;
            }
        }

        private T GetCallJobDurringResult<T>(Call call) where T : CallJobResult
        {

            T result = Activator.CreateInstance<T>();
            result.CallJobResultId = Guid.NewGuid();
            result.ContactType = createContactTypeDurringReminder1.SelectedItem;
            result.Notice = this.createCallDurringNotice1.Notice;
            result.PhoneNumber = call.PhoneNumber;
            result.Start = call.CallDate.HasValue ? call.CallDate.Value : DateTime.Now;
            result.Stop = DateTime.Now;
            result.User = MetaCall.Business.Users.CurrentUser;

            return result;
        }

 
        private T GetCallJobResult<T>(Call call) where T : CallJobResult
        {
            Branch branch = this.createCallNotice1.SelectedBranch;

            T result = Activator.CreateInstance<T>();
            result.CallJobResultId = Guid.NewGuid();
            result.ContactType = createContactTypeReminder1.SelectedItem;
            result.Notice = this.createCallNotice1.Notice;
            result.Branch = branch;
            result.PhoneNumber = call.PhoneNumber;
            result.Start = call.CallDate.HasValue ? call.CallDate.Value : DateTime.Now;
            result.Stop = DateTime.Now;
            result.User = MetaCall.Business.Users.CurrentUser;

            return result;
        }

        private CallJobPossibleResult GetCallJobDurringPossibleResult(Call call)
        {
            CallJobPossibleResult result = GetCallJobDurringResult<CallJobPossibleResult>(call);
            ContactTypesParticipation cTP = new ContactTypesParticipation();
            cTP.ContactTypesParticipationId = ContactTypesParticipation.WiederVorlageId;
            cTP.DisplayName = "Wiedervorlage";
            result.ContactTypesParticipation = cTP;
            //result.ContactTypesParticipation = createCallPossibleReminder1.ContactTypeParticipation;
            //result.ContactTypesParticipationCancellation = createCallPossibleReminder1.ContactTypesParticipationCancellation;
            //result.mwProjekt_SponsorPacket = createCallPossibleReminder1.SponsorPacket;
            //result.SponsorPacketCount = createCallPossibleReminder1.SponsorPaketCount;
            //result.ThankingsFormsProject = createCallPossibleReminder1.ThankingForms;
            //result.AdvertisingText = createCallPossibleReminder1.AdvertisingText;
            //result.SecondCallDesired = createCallPossibleReminder1.SecondCallDesired;
            if (this.createReminderDurringQuestion1.ReminderQuestion == "Ja")
            {
                CallJobReminder reminder = GetReminderDurring(call, this.createReminderDurring1);
                reminder.CallJobResult = MetaCall.Business.CallJobResults.GetResultInfo(result);
                result.Reminder = reminder;

            }

            //TODO: guid ersetzen durch Programmlogik oder Konfiguration
            /*
            if (result.ContactTypesParticipation.ContactTypesParticipationId.Equals(ContactTypesParticipation.JaId) ||
                result.ContactTypesParticipation.ContactTypesParticipationId.Equals(ContactTypesParticipation.InteresseAngebot))
            {
                CallJobReminder reminder = GetReminder(call, this.createCallPossibleReminder1.ReminderControl);
                reminder.CallJobResult = MetaCall.Business.CallJobResults.GetResultInfo(result);
                result.Reminder = reminder;
            }
            */
            return result;
        }

        private CallJobPossibleResult GetCallJobPossibleResult(Call call)
        {
            CallJobPossibleResult result = GetCallJobResult<CallJobPossibleResult>(call);
            result.ContactTypesParticipation = createCallPossibleReminder1.ContactTypeParticipation;
            result.ContactTypesParticipationCancellation = createCallPossibleReminder1.ContactTypesParticipationCancellation;
            result.mwProjekt_SponsorPacket = createCallPossibleReminder1.SponsorPacket;
            result.SponsorPacketCount = createCallPossibleReminder1.SponsorPaketCount;
            result.ThankingsFormsProject = createCallPossibleReminder1.ThankingForms;
            result.AdvertisingText = createCallPossibleReminder1.AdvertisingText;
            result.SecondCallDesired = createCallPossibleReminder1.SecondCallDesired;
            result.PaymentTarget = createCallPossibleReminder1.PaymentTarget;

            //TODO: guid ersetzen durch Programmlogik oder Konfiguration
            if (result.ContactTypesParticipation.ContactTypesParticipationId.Equals(ContactTypesParticipation.WiederVorlageId) ||
                result.ContactTypesParticipation.ContactTypesParticipationId.Equals(ContactTypesParticipation.InteresseAngebot))
            {
                CallJobReminder reminder = GetReminder(call, this.createCallPossibleReminder1.ReminderControl);
                reminder.CallJobResult = MetaCall.Business.CallJobResults.GetResultInfo(result);
                result.Reminder = reminder;
            }
            return result;
        }

        private CallJobUnsuitableResult GetCallJobUnsuitableResult(Call call)
        {
            CallJobUnsuitableResult result = GetCallJobResult<CallJobUnsuitableResult>(call);

            result.ContactTypesParticipationUnsuitable = createReminderUnsuitable1.ContactTypeParticipationUnsuitable;

            return result;
        }

        private CallJobReminderResult GetCallJobReminderResult(Call call)
        {
            CallJobReminderResult result = GetCallJobResult<CallJobReminderResult>(call);
            CallJobReminder reminder = GetReminder(call, this.createReminder1);
            reminder.CallJobResult = MetaCall.Business.CallJobResults.GetResultInfo(result);
            result.CallJobReminder = reminder;

            return result;
        }

        private CallJobReminderResult GetCallJobDurringReminderResult(Call call)
        {
            CallJobReminderResult result = GetCallJobDurringResult<CallJobReminderResult>(call);
            CallJobReminder reminder = GetReminderDurring(call, this.createReminderDurring1);
            reminder.CallJobResult = MetaCall.Business.CallJobResults.GetResultInfo(result);
            result.CallJobReminder = reminder;

            return result;
        }

        private CallJobReminder GetReminderDurring(Call call, CreateReminderDurring userReminderControl)
        {
            CallJobReminder reminder = new CallJobReminder();
            reminder.CallJobReminderId = Guid.NewGuid();
            reminder.CallJob = call.CallJob;
            reminder.Address = call.CallJob.Sponsor;
            reminder.Project = call.CallJob.Project;
            reminder.CallJobResult = null;
            reminder.ReminderDateStart = userReminderControl.ReminderDateStart;
            reminder.ReminderDateStop = userReminderControl.ReminderDateStop;
            //TODO: Die Entscheidung ob ein TeamReminder das aktuelle Projekt unterbricht oder nicht 
            // muss noch mit metatop abgeklärt werden (08.09.2006)
            // Aktueller Stand ist, dass alle Reminder das Projekt unterbrechen
            reminder.ReminderMode = CallJobReminderMode.DisturbCurrentProject;
            reminder.ReminderTracking = userReminderControl.ReminderTracking;
            reminder.ReminderState = CallJobReminderState.Open;

            reminder.Team = MetaCall.Business.Users.CurrentUser.Teams[0].Team;
            if (!userReminderControl.IsTeamReminder)
            {
                reminder.User = MetaCall.Business.Users.GetUserInfo(MetaCall.Business.Users.CurrentUser);
            }
            reminder.DialMode = call.DialMode;
            return reminder;
        }

        private CallJobReminder GetReminder(Call call, CreateReminder userReminderControl)
        {
            CallJobReminder reminder = new CallJobReminder();
            reminder.CallJobReminderId = Guid.NewGuid();
            reminder.CallJob = call.CallJob;
            reminder.Address = call.CallJob.Sponsor;
            reminder.Project = call.CallJob.Project;
            reminder.CallJobResult = null;
            reminder.ReminderDateStart = userReminderControl.ReminderDateStart;
            reminder.ReminderDateStop = userReminderControl.ReminderDateStop;
            //TODO: Die Entscheidung ob ein TeamReminder das aktuelle Projekt unterbricht oder nicht 
            // muss noch mit metatop abgeklärt werden (08.09.2006)
            // Aktueller Stand ist, dass alle Reminder das Projekt unterbrechen
            reminder.ReminderMode = CallJobReminderMode.DisturbCurrentProject;
            reminder.ReminderTracking = userReminderControl.ReminderTracking;
            reminder.ReminderState = CallJobReminderState.Open;

            reminder.Team = MetaCall.Business.Users.CurrentUser.Teams[0].Team;
            if (!userReminderControl.IsTeamReminder)
            {
                reminder.User = this.reminderUser;
                //reminder.User = MetaCall.Business.Users.GetUserInfo(MetaCall.Business.Users.CurrentUser);
            }
            reminder.DialMode = call.DialMode;
            return reminder;
        }

        private void SetCallJobReminder()
        {
            SetReminderVisibleFalse();
            createReminderQuestion1.Visible = true;
            lblUwe.Visible = true;
            lblUweDurring.Visible = false;
            createReminder1.Visible = true;
            createReminder1.TeamReminderOnly();

        }

        private void SetCallJobDurringReminder()
        {
            //SetReminderVisibleFalse();
            //createReminderDurringQuestion1.Visible = true;
            //lblUwe.Visible = false;
            //lblUweDurring.Visible = true;

            // Da das Control keinen Rahmen besitzt, wird es um 10px nach rechts verschoben .
            // Dann ist das Look & Feel gleich wie bei den Controls mit Rand

            Point location = new Point(this.createReminderDurringQuestion1.Left + 10, this.createReminderDurringQuestion1.Bottom);
            int reminderHeight = Math.Max(this.createCallDurringNotice1.Top - location.Y,
                this.createReminderDurring1.MinimumSize.Height);
            Size size = new Size(Width - location.X, reminderHeight);

            createReminderDurring1.PersonalReminderOnly();
            createReminderDurring1.Location = location;
            createReminderDurring1.Size = size;
            createReminderDurring1.Visible = true;

            //Damit links des Reminders keine weiße Fläche entsteht wird ein Label eingeblendet.
            location.Offset(-10, 0);
            size.Width = 10;
            this.lblUweDurring.Location = location;
            this.lblUweDurring.Size = size;
            this.lblUweDurring.Visible = true;


            Point lastControlLeftBottomPosition = new Point(this.createReminderDurring1.Left - 10, this.createReminderDurring1.Bottom);
            SetCreateNoticeControlPosition(lastControlLeftBottomPosition);


        }

        private void SetCreateNoticeControlPosition(Point lastControlLeftBottomPosition)
        {
            //Anpassen des Notizfeldes
            Point noticeLocation = lastControlLeftBottomPosition;
            Size noticeSize = new Size(this.Width - lastControlLeftBottomPosition.X, (this.okButton.Top - 10) - lastControlLeftBottomPosition.Y);

            this.createCallDurringNotice1.Location = noticeLocation;
            this.createCallDurringNotice1.Size = noticeSize;

            this.createCallDurringNotice1.PerformLayout();
        }

        private void SetCallJobReminderQuestion()
        {
            SetReminderVisibleFalse();
            createCallPossibleReminder1.Visible = false;

            createReminderQuestion1.InitializeCall(this.call);

            createReminderQuestion1.Visible = true;
        }

        private void SetCallJobDurringReminderQuestion()
        {

            SetReminderVisibleFalse();

            //Positionieren und initialisieren des Controls
            Point location = new Point(this.createContactTypeDurringReminder1.Left, this.createContactTypeDurringReminder1.Bottom);
            createReminderDurringQuestion1.InitializeCall(this.call);
            createReminderDurringQuestion1.Location = location;
            createReminderDurringQuestion1.Visible = true;

        }

        private void SetCallJobDurringClosure()
        {
            SetReminderVisibleFalse();

            //Positionieren und initialisieren des Controls
            Point location = new Point(this.createContactTypeDurringReminder1.Left, this.createContactTypeDurringReminder1.Bottom);
            //createReminderDurringQuestion1.InitializeCall(this.call);
            createDurringClosure1.Location = location;
            createDurringClosure1.Visible = true;
           
        }

        private void SetCallJobDurringPossibleReminder()
        {
            SetReminderVisibleFalse();

            //Positionen der Controls anpassen 
            Point location = new Point(this.createContactTypeDurringReminder1.Left, this.createContactTypeDurringReminder1.Bottom);

            //Rechnungsinformationen einblenden und positionieren
            this.invoiceInfo1.Location = location;
            invoiceInfo1.Size = new Size(this.createContactTypeDurringReminder1.Width,
                invoiceInfo1.MinimumSize.Height);
            invoiceInfo1.Visible = true;

            //neue Position berechnen 
            location.Offset(0, invoiceInfo1.Height);

            //ReminderControl initialisieren und positionieren
            this.createReminderDurringQuestion1.InitializeCall(this.call);
            this.createReminderDurringQuestion1.Visible = true;
            this.createReminderDurringQuestion1.Location = location;

            SetCreateNoticeControlPosition(new Point(this.createReminderDurringQuestion1.Left, this.okButton.Top - this.createCallDurringNotice1.MaximumSize.Height));

        }

        private void SetCallJobPossibleReminder()
        {
            SetReminderVisibleFalse();
            this.createCallPossibleReminder1.Visible = true;
            this.createCallPossibleReminder1.Projektnummer =
                call.CallJob.Project.mwProjektNummer.HasValue ? call.CallJob.Project.mwProjektNummer.Value.ToString() : null;
        }

        private void SetReminderVisibleFalse()
        {
            createCallPossibleReminder1.Visible = false;
            createReminder1.Visible = false;
            createReminderDurring1.Visible = false;
            createReminderQuestion1.Visible = false;
            createReminderDurringQuestion1.Visible = false;
            createReminderUnsuitable1.Visible = false;
            createDurringClosure1.Visible = false;
            lblUwe.Visible = false;
            invoiceInfo1.Visible = false;
            lblUweDurring.Visible = false;
        }

        private void SetCallJobReminderUnsuitable()
        {
            createReminderUnsuitable1.Visible = true;
        }

        protected void OnSave(PhoneViewEventArgs e)
        {
            if (Save != null)
                Save(this, e);
        }

        protected void OnCancel(PhoneViewEventArgs e)
        {
            if (Cancel != null)
                Cancel(this, e);
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            //prüfen obn das Telefonat bereits abgeschlossen ist
            if (MetaCall.Business.Dialer.State != DialStates.Ready)
            {
                this.HangUp();
            }

            //Dunningteil gilt für WVs die während dem normalen Telefonieren
            //auftreten. In diesem Falls is DunningActive = false
            if (call.CallJob.GetType() == typeof(DurringCallJob) && MetaCall.Business.CallJobs.DurringActiv == false)
            {
                MetaCall.Business.ActivityLogger.Log(new BusinessLayer.Activities.DurringChanged(MetaCall.Business.CallJobs.DurringActiv));
            }
            else
            {
                //Ereignis loggen
                MetaCall.Business.ActivityLogger.Log(new BusinessLayer.Activities.CancelCustomer());
            }

            OnCancel(new PhoneViewEventArgs(CallJobResultMessage.CancelledResult));
        }

        private void windowDesign()
        {
            int windowFree = 10;
            int windowFreeParts = 5;

            int windowHeight;

            windowHeight = this.Height - (windowFree * windowFreeParts);

            sponsorInfo1.Top = (1 * windowFree);
            mwProjectInfo1.Top = (2 * windowFree) + sponsorInfo1.Height;
            customerInfo1.Top = mwProjectInfo1.Height + mwProjectInfo1.Top + windowFree;
            historieInfo1.Top = this.Height - windowFree - historieInfo1.Height;

            int rightPartWidth;
            int maxWidth;

            maxWidth = this.Width;

            rightPartWidth = maxWidth - sponsorInfo1.Width - 2 * windowFree;

            createContactTypeReminder1.Location = new Point(370, windowFree);
            createContactTypeDurringReminder1.Location = new Point(370, windowFree);
            createReminderQuestion1.Location = new Point(370, createContactTypeReminder1.Location.Y + createContactTypeReminder1.FormHeight);
            createReminderDurringQuestion1.Location = new Point(370, createContactTypeDurringReminder1.Location.Y + createContactTypeDurringReminder1.FormHeight);
            createReminder1.Location = new Point(380, createReminderQuestion1.Location.Y + createReminderQuestion1.FormHeight);
            createReminderDurring1.Location = new Point(380, createReminderDurringQuestion1.Location.Y + createReminderDurringQuestion1.FormHeight);
            createDurringClosure1.Location = new Point(380, createContactTypeDurringReminder1.Location.Y + createContactTypeDurringReminder1.FormHeight);
            createCallNotice1.Location = new Point(370, this.Height - windowFree - createCallNotice1.FormHeight - okButton.Height);
            //createCallDurringNotice1.Location = new Point(370, this.Height - windowFree - createCallDurringNotice1.FormHeight - okButton.Height);
            createCallPossibleReminder1.Location = new Point(370, createContactTypeReminder1.Location.Y + createContactTypeReminder1.FormHeight);
            createReminderUnsuitable1.Location = new Point(370, createContactTypeReminder1.Location.Y + createContactTypeReminder1.FormHeight);
            dialButton.Location = new Point(380, okButton.Top);

            createContactTypeReminder1.Size = new Size(rightPartWidth, createContactTypeReminder1.FormHeight);
            createContactTypeDurringReminder1.Size = new Size(rightPartWidth, createContactTypeDurringReminder1.FormHeight);
            createReminderQuestion1.Size = new Size(rightPartWidth, createReminderQuestion1.FormHeight);
            createReminderDurringQuestion1.Size = new Size(rightPartWidth, createReminderDurringQuestion1.FormHeight);
            createReminder1.Size = new Size(rightPartWidth, createReminder1.FormHeight);
            createReminderDurring1.Size = new Size(rightPartWidth, this.Height - createReminderDurring1.Top - okButton.Height - createCallDurringNotice1.Height - windowFree);
            lblUwe.Location = new Point(370, createReminderQuestion1.Location.Y + createReminderQuestion1.Height);
            lblUweDurring.Location = new Point(370, createReminderDurringQuestion1.Location.Y + createReminderDurringQuestion1.Height);
            lblUwe.Size = new Size(10, createReminder1.FormHeight);
            lblUweDurring.Size = new Size(10, createReminderDurring1.Height);
            createCallNotice1.Size = new Size(rightPartWidth, createCallNotice1.FormHeight);
            //createCallDurringNotice1.Size = new Size(rightPartWidth, createCallDurringNotice1.FormHeight);


            //SetCreateNoticeControlPosition(new Point(370, this.createReminderDurring1.Bottom));
            SetCreateNoticeControlPosition(new Point(this.createReminderDurringQuestion1.Left, this.okButton.Top - this.createCallDurringNotice1.MaximumSize.Height));

            invoiceInfo1.Location = new Point(370, createContactTypeDurringReminder1.Bottom);
            invoiceInfo1.Size = new Size(rightPartWidth, this.createCallDurringNotice1.Top - this.createContactTypeDurringReminder1.Bottom);

            //invoiceInfo1.FormHeightRest = invoiceInfo1.Height;

            if (MetaCall.Business.CallJobs.DurringActiv == true)
                createCallPossibleReminder1.FormHeightRest = this.Height - createCallDurringNotice1.Height - createCallPossibleReminder1.Top - windowFree - okButton.Height;
            else
                createCallPossibleReminder1.FormHeightRest = this.Height - createCallNotice1.Height - createCallPossibleReminder1.Top - windowFree - okButton.Height;

            createCallPossibleReminder1.Size = new Size(rightPartWidth, createCallPossibleReminder1.FormHeight);

            if (MetaCall.Business.CallJobs.DurringActiv == true)
                createReminderUnsuitable1.FormHeightRest = this.Height - createCallDurringNotice1.Height - createReminderUnsuitable1.Top - windowFree - okButton.Height;
            else
                createReminderUnsuitable1.FormHeightRest = this.Height - createCallNotice1.Height - createReminderUnsuitable1.Top - windowFree - okButton.Height;

            createReminderUnsuitable1.Size = new Size(rightPartWidth, createReminderUnsuitable1.FormHeight);

            if (MetaCall.Business.CallJobs.DurringActiv == true)
                createDurringClosure1.FormHeightRest = this.Height - createCallDurringNotice1.Height -
                                                       createDurringClosure1.Top - windowFree - okButton.Height;
            else
                createDurringClosure1.FormHeightRest = this.Height - createCallNotice1.Height -
                                                        createDurringClosure1.Top - windowFree - okButton.Height;

            createDurringClosure1.Size = new Size(rightPartWidth, createDurringClosure1.FormHeight);
            
        }

        private void dialButton_Click(object sender, EventArgs e)
        {
            if (MetaCall.Business.Dialer.State == DialStates.Ready)
            {
                Dial();
            }
            else
            {
                this.HangUp();
            }
        }

        public Call Call
        {
            get
            {
                return this.call;
            }
        }

        public string UsedPhoneNumber
        {
            get
            {
                return this.createContactTypeReminder1.UsedPhoneNumber;
            }
        }

        private void createReminderDurring1_ValueChanged(object sender, CreateReminderDurringEventArgs e)
        {
            StringBuilder sb = new StringBuilder();

            switch (e.CallJobReminderTracking)
            {
                case CallJobReminderTracking.ExactDateAndTime:
                    if (e.IsTeamReminder)
                    {
                        sb.AppendFormat("Der Sponsor wird Ihrem Team am {0} um {1} Uhr zum Mahnen angeboten",
                            e.Start.ToShortDateString(),
                            e.Start.ToShortTimeString());

                    }
                    else
                    {
                        sb.AppendFormat("Der Sponsor wird Ihnen am {0} um {1} zum Mahnen angeboten",
                            e.Start.ToShortDateString(),
                            e.Start.ToShortTimeString());
                    }
                    break;
                case CallJobReminderTracking.Day:
                    break;
                case CallJobReminderTracking.Week:
                    break;
                case CallJobReminderTracking.WeekDay:
                    break;
                case CallJobReminderTracking.OnlyTimeSpan:
                    if (e.IsTeamReminder)
                    {
                        sb.AppendFormat("Der Sponsor wird Ihrem Team täglich zwischen {0} und {1} Uhr zum Mahnen angeboten",
                            e.Start.ToShortTimeString(),
                            e.Stop.ToShortTimeString());

                    }
                    else
                    {
                        sb.AppendFormat("Der Sponsor wird Ihnen täglich zwischen {0} und {1} Uhr zum Mahnen angeboten",
                                                    e.Start.ToShortTimeString(),
                                                    e.Stop.ToShortTimeString());
                    }
                    break;
                case CallJobReminderTracking.DateAndTimeSpan:
                    break;
                default:
                    break;
            }

            if (sb.Length > 0)
            {
                this.createReminderDurringQuestion1.ReminderAnswer = sb.ToString();
            }
            else
            {
                this.createReminderDurringQuestion1.ReminderAnswer = string.Empty;
            }
        }

        private void createReminder1_ValueChanged(object sender, CreateReminderEventArgs e)
        {
            StringBuilder sb = new StringBuilder();

            if (this.createReminderQuestion1.ReminderQuestion != "Nein")
            {
                switch (e.CallJobReminderTracking)
                {
                    case CallJobReminderTracking.ExactDateAndTime:
                        if (e.IsTeamReminder)
                        {
                            sb.AppendFormat("Der Sponsor wird Ihrem Team am {0} um {1} Uhr als Wiedervorlage angeboten",
                                e.Start.ToShortDateString(),
                                e.Start.ToShortTimeString());

                        }
                        else
                        {
                            sb.AppendFormat("Der Sponsor wird Ihnen am {0} um {1} als Wiedervorlage angeboten",
                                e.Start.ToShortDateString(),
                                e.Start.ToShortTimeString());
                        }
                        break;
                    case CallJobReminderTracking.Day:
                        break;
                    case CallJobReminderTracking.Week:
                        break;
                    case CallJobReminderTracking.WeekDay:
                        break;
                    case CallJobReminderTracking.OnlyTimeSpan:
                        if (e.IsTeamReminder)
                        {
                            sb.AppendFormat("Der Sponsor wird Ihrem Team täglich zwischen {0} und {1} Uhr als Wiedervorlage angeboten",
                                e.Start.ToShortTimeString(),
                                e.Stop.ToShortTimeString());

                        }
                        else
                        {
                            sb.AppendFormat("Der Sponsor wird Ihnen täglich zwischen {0} und {1} Uhr als Wiedervorlage angeboten",
                                                        e.Start.ToShortTimeString(),
                                                        e.Stop.ToShortTimeString());
                        }
                        break;
                    case CallJobReminderTracking.DateAndTimeSpan:
                        break;
                    default:
                        break;
                }
            }

            if (sb.Length > 0)
            {
                this.createReminderQuestion1.ReminderAnswer = sb.ToString();
            }
            else
            {
                this.createReminderQuestion1.ReminderAnswer = string.Empty;
            }
        }

        private void createCallPossibleReminder1_ResultChanged(object sender, System.EventArgs e)
        {
            if (this.createCallPossibleReminder1.ContactTypeParticipation != null &&
                this.createCallPossibleReminder1.ContactTypeParticipation.ContactTypesParticipationId.Equals(ContactTypesParticipation.JaId))
                this.sponsorInfo1.CheckValidSponsorInformation = true;
            else
                this.sponsorInfo1.CheckValidSponsorInformation = false;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {

            Control ctl = this.GetChildAtPoint(e.Location,
                GetChildAtPointSkip.Disabled | GetChildAtPointSkip.Invisible | GetChildAtPointSkip.Transparent);

            if (ctl != null &&
                ctl is ExpandableUserControl)
            {

                ExpandableUserControl expCtl = ctl as ExpandableUserControl;

                if (!expCtl.ExpandedSize.Equals(expCtl.Size))
                {


                    foreach (Control formCtl in this.Controls)
                    {
                        ExpandableUserControl expandableUserControl = formCtl as ExpandableUserControl;

                        if (expandableUserControl != null)
                        {
                            if (expandableUserControl == ctl)
                            {
                                expandableUserControl.Expand();
                            }
                            else
                            {
                                expandableUserControl.Collapse();
                            }
                        }
                    }
                }

                SetAutoPanelLocation();
            }

            base.OnMouseMove(e);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            SetAutoPanelLocation();
        }

        private void SetAutoPanelLocation()
        {

            //Abstand zwischen den Controls und dem Rand
            int margin = 10;

            Point posTopAnchor = new Point(margin, margin);
            Point posBottomAnchor = new Point(margin, Height - margin);

            // Der Counter läuft rückwärts damit die Reihenfolge der Controls eingehalten 
            // und entsprechend der Z-Order angezeigt wird
            for (int i = this.Controls.Count; i > 0; i--)
            {
                ExpandableUserControl autoFocusPanel = this.Controls[i - 1] as ExpandableUserControl;

                if (autoFocusPanel != null)
                {
                    autoFocusPanel.Location = posTopAnchor;

                    //nächste Position berechnen 
                    posTopAnchor = new Point(margin, autoFocusPanel.Bottom + margin);
                }
            }
        }

        private void RealCallTimer_Tick(object sender, EventArgs e)
        {
            //momentan ist der Timer auf 1000 ms eingestellt
            nonConnectionTime++;

            DialStates xPhoneState = MetaCall.Business.Dialer.XPhoneState();
            if (!(xPhoneState == DialStates.Ready || xPhoneState == DialStates.Connected))
            {
                //string dState = xPhoneState.ToString();
                //System.Windows.Forms.MessageBox.Show("Test " + dState);

                
                if (nonConnectionTime > abortDialingTime)
                {
                    //Dialer_RealCallTimerStopped(this, new EventArgs());
                    try
                    {
                        this.HangUp();
                    }
                    catch (Exception ex)
                    {
                        string msg = ex.Message;
                        System.Windows.Forms.MessageBox.Show(msg,
                                "Fehler in Methode 'Ticker'");
                    }
                }
            }
            /*if (nonConnectionTime > abortDialingTime)
            {
                nonConnectionTime = 0;
            }*/
        }
    }

    public delegate void PhoneViewEventHandler(object sender, PhoneViewEventArgs e);

    public class PhoneViewEventArgs : EventArgs
    {
        private CallJobResultMessage callJobResultMessage;

        public PhoneViewEventArgs(CallJobResultMessage callJobResultMessage)
        {
            this.callJobResultMessage = callJobResultMessage;
        }

        public CallJobResultMessage CallJobResultMessage
        {
            get { return this.callJobResultMessage; }
        }

    }

    public delegate void SponsorInfoChangeEventHandler(object sender, SponsorInfoChangeEventArgs e);

    public class SponsorInfoChangeEventArgs : EventArgs
    {
        Sponsor sponsor;

        public SponsorInfoChangeEventArgs(Sponsor sponsor)
        {
            this.sponsor = sponsor;
            
        }

        public Sponsor Sponsor
        {
            get { return this.sponsor; }
        }
    }

    public delegate void SponsorPhoneChangeEventHandler(object sender, SponsorPhoneChangeEventArgs e);

    public class SponsorPhoneChangeEventArgs : EventArgs
    {
        Sponsor sponsor;

        public SponsorPhoneChangeEventArgs(Sponsor sponsor)
        {
            this.sponsor = sponsor;
        }

        public Sponsor Sponsor
        {
            get { return this.sponsor; }
        }
    }
}
