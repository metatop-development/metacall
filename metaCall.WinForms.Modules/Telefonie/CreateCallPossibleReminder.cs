using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Globalization;

using metatop.Applications.metaCall.BusinessLayer;
using metatop.Applications.metaCall.DataObjects;
using MaDaNet.Common.AppFrameWork.Validation;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace metatop.Applications.metaCall.WinForms.Modules.Telefonie
{
    [ToolboxItem(false)]
    public partial class CreateCallPossibleReminder : UserControl , ISupportInitialize, IInitializeCall
    {
        public event EventHandler ResultChanged;
        
        private DataTable dtContactTypesParticipation = new DataTable();
        private DataTable dtContactTypesParticipationCancellation = new DataTable();
        private DataTable dtMwProjekt_SponsorPacket = new DataTable();

        private List<ThankingsFormsProject> thankingsFormsProject;

        private GroupBox grpContactType;

        private int projektNummer;
        private int formHeight = 0;
        private int formHeightRest = 0;

        private string faxNoticeRemark = "";

        private string reminderAnswer;

        private Call call;

        /// <summary>
        /// 
        /// </summary>
        public Call Call
        {
            get { return call; }
            set
            {
                if (this.isInitializing) return;

                call = value;
                this.createReminder1.InitializeCall(call);
            }
        }

        /// <summary>
        /// gibt den Eintrag für das Notizfeld zurück
        /// </summary>
        public String FaxNoticeRemark
        {
            get { return this.faxNoticeRemark; }
        }


        /// <summary>
        /// Veröffentlichen des ReminderControls, da dies im PhoneView abgefragt wird
        /// </summary>
        /// <remarks>
        /// Hier muus die Remindereinstellung mittels eines Interfaces 
        /// bekannt gemacht werden, damit dieses zurückgeliefert werden kann
        /// </remarks>
        public CreateReminder ReminderControl
        {
            get{
                //TODO: Interface zurückliefern
                
                return this.createReminder1;
                }
        }

        public int FormHeight
        {
            get { return formHeight; }
        }

        public int FormHeightRest
        {
            set 
            { 
                formHeightRest = value;

             //   this.Height = formHeightRest;

                //Höhe der CHeckListBox & txtResult5 berechnen
                    Setting setting = MetaCall.Business.Settings.GetSetting();

                    if (setting.PaymentTargetVisible == true)
                    {
                        this.checkedListBoxThangingForm.Top = 117;
                    }
                    else
                    {
                        this.checkedListBoxThangingForm.Top = 90;
                    }

                int controlHeight = (formHeightRest - checkedListBoxThangingForm.Top - 0 - this.Margin.Horizontal) / 2 ;
                checkedListBoxThangingForm.Height = controlHeight;
                this.txtResult5.Top =   this.checkedListBoxThangingForm.Bottom + this.Margin.Horizontal;
                this.txtResult5.Height = controlHeight ;
                this.lblResult5.Top = this.txtResult5.Top + this.txtResult5.Margin.Top;

                grpContactType.Parent = this;
                grpContactType.Location = new Point(10, 0);
                grpContactType.Size = new Size(480, formHeightRest - 5);

                formHeight = txtResult5.Bottom + 35 + 52;
            }
        }

        [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden)]
        public string Projektnummer
        {
            set { this.projektNummer = Convert.ToInt32(value); }
        }

        public CreateCallPossibleReminder()
        {
            grpContactType = new GroupBox();
            InitializeComponent();
            dtContactTypesParticipation.Locale = System.Globalization.CultureInfo.InvariantCulture;
            dtContactTypesParticipationCancellation.Locale = System.Globalization.CultureInfo.InvariantCulture;
        }

        private void setupDataTableCTPC()
        {
            dtContactTypesParticipationCancellation = new DataTable("ContactTypesParticipationCancellation");

            AddColumn(dtContactTypesParticipationCancellation, "ContactTypesParticipationCancellationId", "ContactTypesParticipationCancellation", typeof(Guid));
            AddColumn(dtContactTypesParticipationCancellation, "DisplayName", "ContactTypesParticipationCancellationId", typeof(string));
            AddColumn(dtContactTypesParticipationCancellation, "ContactTypesParticipationCancellation", "", typeof(ContactTypesParticipationCancellation));

            dtContactTypesParticipationCancellation.Rows.Clear();
            foreach (ContactTypesParticipationCancellation contactTypesParticipationCancellation in MetaCall.Business.ContactTypesParticipationCancellation.ContactTypesParticipationsCancellation)
            {
                object[] rowData = new object[]{
                    contactTypesParticipationCancellation.ContactTypesParticipationCancellationId,
                    contactTypesParticipationCancellation.DisplayName,
                    contactTypesParticipationCancellation};

                dtContactTypesParticipationCancellation.Rows.Add(rowData);
            }
        }

        private void setupDataTableCTP()
        {
            dtContactTypesParticipation = new DataTable("ContactTypesParticipation");

            AddColumn(dtContactTypesParticipation, "ContactTypesParticipationId", "ContactTypesParticipation", typeof(Guid));
            AddColumn(dtContactTypesParticipation, "DisplayName", "ContactTypesParticipationId", typeof(string));
            AddColumn(dtContactTypesParticipation, "ContactTypesParticipation", "",typeof(ContactTypesParticipation));

            BindComboBox(dtContactTypesParticipation, this.cBResult0, 1, "ContactTypesParticipation");

        }

        private void FilldtContactTypesParticipation()
        {
            dtContactTypesParticipation.Rows.Clear();

            bool allowfax = this.ProjectDocumentsPresent(this.call.CallJob.Project);
            bool allowReminder = this.call.CallJob.Project.ReminderDateMax == null ? true :
                this.call.CallJob.Project.ReminderDateMax < System.DateTime.Today ? false : true;

            foreach (ContactTypesParticipation contactTypesParticipation in MetaCall.Business.ContactTypesParticipation.ContactTypesParticipations)
            {
                //kein Angebotsauswahl zulassen, wenn kein faxangebot hinterlegt ist.
                if (!allowfax &&
                    (contactTypesParticipation.ContactTypesParticipationId ==
                    ContactTypesParticipation.InteresseAngebot))
                {
                    continue;
                }

                if (!allowReminder &&
                    (contactTypesParticipation.ContactTypesParticipationId ==
                    ContactTypesParticipation.WiederVorlageId ||
                    contactTypesParticipation.ContactTypesParticipationId ==
                    ContactTypesParticipation.InteresseAngebot))
                {
                    continue;
                }

                object[] rowData = new object[]{
                    contactTypesParticipation.ContactTypesParticipationId,
                    contactTypesParticipation.DisplayName,
                    contactTypesParticipation};

                dtContactTypesParticipation.Rows.Add(rowData);
            }

            this.cBResult0.SelectedIndex = -1;
        }

        private void setupDataTableSponsorPacket()
        {
            dtMwProjekt_SponsorPacket = new DataTable("MwProjekt_SponsorPacket");

            AddColumn(dtMwProjekt_SponsorPacket, "ProjekteSponsorenpaketNummer", "ProjekteSponsorenpaketNummer", typeof(int));
            AddColumn(dtMwProjekt_SponsorPacket, "Bezeichnung", "Bezeichnung", typeof(string));
            AddColumn(dtMwProjekt_SponsorPacket, "BetragNetto", "BetragNetto", typeof(double));
            AddColumn(dtMwProjekt_SponsorPacket, "SponsorPaket", string.Empty, typeof(mwProjekt_SponsorPacket));

            //BindComboBox(dtMwProjekt_SponsorPacket, this.cBContactTypesParticipation, 1, "ProjekteSponsorenpaketNummer");

            dtMwProjekt_SponsorPacket.Rows.Clear();
            foreach (mwProjekt_SponsorPacket mwprojekt_SponsorPacket in MetaCall.Business.mwProjekt_SponsorPacket.mwProjekt_SponsorPackets(projektNummer))
            {
                object[] rowData = new object[]{
                    mwprojekt_SponsorPacket.ProjekteSponsorenpaketNummer,
                    mwprojekt_SponsorPacket.Bezeichnung,
                    mwprojekt_SponsorPacket.BetragNetto,
                    mwprojekt_SponsorPacket};

                dtMwProjekt_SponsorPacket.Rows.Add(rowData);
            }
        }

        private DataColumn AddColumn(DataTable target, string name, string caption, Type dataType)
        {
            DataColumn col = new DataColumn();
            col.ColumnName = name;
            col.Caption = caption;
            col.DataType = dataType;

            target.Columns.Add(col);

            return col;

        }

        private DataColumn AddColumn(DataTable target, string name, string caption, Type dataType, MappingType mappingType)
        {
            DataColumn col = AddColumn(target, name, caption, dataType);
            col.ColumnMapping = mappingType;

            return col;
        }

        private void BindComboBox(DataTable dataTable, ComboBox cbTarget, int displayColumnIndex, string valueMember)
        {
            cbTarget.DataSource = dataTable;
            cbTarget.DisplayMember = dataTable.Columns[displayColumnIndex].ColumnName;

            if (valueMember != "")
            {
                cbTarget.ValueMember = valueMember;
            }
            
        }

        private void CreateCallPossibleReminder_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            formHeight = checkedListBoxThangingForm.Top + checkedListBoxThangingForm.Height + 15;
            CleanResult();
            setupDataTableCTP();
        }

        private void AktionTeilnahme()
        {
            if (cBResult0.SelectedItem != null)
            {
                ContactTypesParticipation ctp = cBResult0.SelectedValue as ContactTypesParticipation;

                this.lblResult1.Top = 43;
                this.lblResult1.AutoSize = false;
                this.lblResult1.ForeColor = Color.Black;

                if (ctp.ContactTypesParticipationId.Equals(ContactTypesParticipation.JaId))
                {

                    mwProject mwProject = MetaCall.Business.Projects.GetMwProject(this.call.CallJob.Project);
                    
                    string advertisingTextDefault = MetaCall.Business.CallJobResults.GetDefaultAdvertisingText(mwProject.Language);
                    
                    //JA
                    CleanResult();
                    this.lblResult1.Text = "Ausrüstungspaket";
                    this.lblResult2.Text = "Anzahl";
                    this.lblResult4.Text = "Danksagungsformen";
                    this.lblResult5.Text = "Werbetext"; 
                    this.txtPreis.Text = "0";
                    this.lBPreisBez.Text = "Preis";
                    this.txtResult5.Text = advertisingTextDefault;


                    this.cBResult1.Size = new Size(210, 21); 
                    
                    this.lblResult1.Visible = true;
                    this.lblResult2.Visible = true;

                    this.lblResult4.Visible = true;
                    this.lblResult5.Visible = true;

                    this.txtAnzahl.Visible = true;
                    int StandardAnzahl = 1;
                    this.txtAnzahl.Text = StandardAnzahl.ToString();
                    this.txtResult5.Visible = true;

                    Setting setting = MetaCall.Business.Settings.GetSetting();
                    
                    if (setting.PaymentTargetVisible == true)
                    {
                        this.lblResult3.Text = "Zahlungsziel";
                        this.lblResult3.Visible = true;
                        this.DateTimePickerPaymentTarget.Checked = false;
                        this.DateTimePickerPaymentTarget.Visible = true;
                    }
                    else
                    {
                        this.lblResult3.Visible = false;
                        this.DateTimePickerPaymentTarget.Visible = false;
                    }
                    setupDataTableSponsorPacket();

                    BindComboBox(dtMwProjekt_SponsorPacket, this.cBResult1, 1, "ProjekteSponsorenpaketNummer");
                    this.cBResult1.Visible = true;

                    this.cBSet1.Visible = true;
                    this.cBSet2.Visible = true;
                    this.cBSet3.Visible = true;
                    this.cBSet4.Visible = true;
                    this.cBSet5.Visible = true;
                    this.cBSet6.Visible = true;

                    this.lBPreisBez.Visible = true;
                    this.txtPreis.Visible = true;

                    setupDataTableThangingForms();
                    this.checkedListBoxThangingForm.Visible = true;

                    this.sendFaxButton.Visible = this.ProjectDocumentsPresent(this.call.CallJob.Project);
                } 
                else if (ctp.ContactTypesParticipationId.Equals(ContactTypesParticipation.NeinId))
                {
                    //NEIN
                    CleanResult();
                    setupDataTableCTPC();
                    BindComboBox(dtContactTypesParticipationCancellation, this.cBResult1, 1, "ContactTypesParticipationCancellation");
                    this.lblResult1.Text = "Warum keine Teilnahme";
                    this.lblResult1.Visible = true;
                    this.cBResult1.SelectedIndex = -1;
                    this.cBResult1.Visible = true;
                    this.cBResult1.Size = new Size(210, 21);
                    this.secondCallDesiredGroupBox.Location = new Point(this.cBResult1.Left, this.cBResult1.Bottom + this.Margin.Horizontal);
                    this.secondCallDesiredGroupBox.Visible = true;

                } 
                else if (ctp.ContactTypesParticipationId.Equals(ContactTypesParticipation.InteresseAngebot))
                {
                    //Interesse Angebot
                    CleanResult();
                    /*this.lblResult1.Text = "Formular";
                    this.lblResult2.Text = "Versandart";
                    this.lblResult3.Text = "Rückruf";

                    this.lblResult1.Visible = true;
                    this.lblResult2.Visible = true;
                    this.lblResult3.Visible = true;

                    this.cBResult1.Visible = true;
                    this.cBResult2.Visible = true;
                    this.cBResult3.Visible = true;
                     * */
                    this.sendFaxButton.Visible = this.ProjectDocumentsPresent(this.call.CallJob.Project);
                    this.createReminder1.Visible = true;
                    this.createReminder1.ReminderDateStart = DateTime.Now;
                    this.lblResult1.Top = 40;
                    this.lblResult1.AutoSize = true;
                    this.lblResult1.Visible = true;
                    this.lblResult1.ForeColor = Color.Blue;
                } 
                else if (ctp.ContactTypesParticipationId.Equals(ContactTypesParticipation.ZweitAngebot))
                {
                    //Zweitanruf
                    CleanResult();
                }

                else if (ctp.ContactTypesParticipationId.Equals(ContactTypesParticipation.WiederVorlageId))
                {
                    //Wiedervorlage
                    CleanResult();
                    this.createReminder1.Visible = true;
                    this.createReminder1.ReminderDateStart = DateTime.Now;
                    this.lblResult1.Top = 40;
                    this.lblResult1.AutoSize = true;
                    this.lblResult1.Visible = true;
                    this.lblResult1.ForeColor = Color.Blue;
                }

                //{26D0D13E-04E4-46DF-AB57-A5F07C4B3E18}

                setupDataTableSponsorPacket();
            }
        }

        private void setupDataTableThangingForms()
        {
            thankingsFormsProject = MetaCall.Business.ThankingsFormsProject.GetThankingsFormsByProject(this.projektNummer);
           ((ListBox) checkedListBoxThangingForm).DisplayMember = "BedankungsformDe";
            checkedListBoxThangingForm.CheckOnClick = true;
            checkedListBoxThangingForm.Items.Clear();
            foreach (ThankingsFormsProject tFP in  thankingsFormsProject)
            {
                checkedListBoxThangingForm.Items.Add(tFP);
            }
            for (int i = 0; i < checkedListBoxThangingForm.Items.Count; i++)
            {
                checkedListBoxThangingForm.SetItemChecked(i, true);
            }
        }

        private void CleanResult()
        {
            this.lblResult1.Text = "";
            this.lblResult2.Text = "";
            this.lblResult3.Text = "";
            this.lblResult1.Visible = false;
            this.lblResult2.Visible = false;
            this.lblResult3.Visible = false;
            this.lblResult4.Visible = false;
            this.lblResult5.Visible = false;
            this.cBResult1.Visible = false;
            this.cBResult2.Visible = false;
            this.cBResult3.Visible = false;
            this.txtResult5.Visible = false;
            DataTable dtLeer = new DataTable();
            this.cBResult1.DataSource = dtLeer;
            this.cBResult2.DataSource = dtLeer;
            this.cBResult3.DataSource = dtLeer;
            this.cBResult1.Size = new Size(120, 21);
            this.cBResult2.Size = new Size(120, 21);
            this.cBResult3.Size = new Size(120, 21);
            this.txtAnzahl.Visible = false;

            this.cBSet1.Visible = false;
            this.cBSet2.Visible = false;
            this.cBSet3.Visible = false;
            this.cBSet4.Visible = false;
            this.cBSet5.Visible = false;
            this.cBSet6.Visible = false;
            
            this.lBPreisBez.Visible = false;
            this.txtPreis.Visible = false;

            this.checkedListBoxThangingForm.Visible = false;
            this.createReminder1.Visible = false;

            this.secondCallDesiredRadioButton.Checked = false;
            this.secondCallUndesiredRadioButton.Checked = false;

            this.secondCallDesiredGroupBox.Visible = false;
            this.sendFaxButton.Visible = false;

            this.DateTimePickerPaymentTarget.Visible = false;
        }

        private void cBResult0_SelectedIndexChanged(object sender, EventArgs e)
        {
            AktionTeilnahme();

            OnResultChanged(EventArgs.Empty);
        }

        protected void OnResultChanged(EventArgs e)
        {
            if (ResultChanged != null)
                ResultChanged(this, e);
        }

        private void cBSet1_Click(object sender, EventArgs e)
        {
            this.txtAnzahl.Text = Convert.ToString(1);
        }

        private void cBSet2_Click(object sender, EventArgs e)
        {
            this.txtAnzahl.Text = Convert.ToString(2);
        }

        private void cBSet3_Click(object sender, EventArgs e)
        {
            this.txtAnzahl.Text = Convert.ToString(3);
        }

        private void cBSet4_Click(object sender, EventArgs e)
        {
            this.txtAnzahl.Text = Convert.ToString(4);
        }

        private void cBSet5_Click(object sender, EventArgs e)
        {
            this.txtAnzahl.Text = Convert.ToString(5);
        }

        private void cBSet6_Click(object sender, EventArgs e)
        {
            this.txtAnzahl.Text = Convert.ToString(10);
        }

        #region Rückgabe der gesetzten Werte

        [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden)]
        public ContactTypesParticipation ContactTypeParticipation
        {
            get
            {
                if (this.isInitializing) return null;
                
                if (cBResult0.SelectedItem != null)
                    return (ContactTypesParticipation)dtContactTypesParticipation.Rows[cBResult0.SelectedIndex]["ContactTypesParticipation"];
                else
                    return null;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DateTime? PaymentTarget
        {
            get
            {
                if (this.isInitializing) return null;

                Setting setting = MetaCall.Business.Settings.GetSetting();

                if (setting.PaymentTargetVisible == false)
                    return null;

                if (this.DateTimePickerPaymentTarget.Checked == true)
                    return (DateTime)this.DateTimePickerPaymentTarget.Value;
                else
                    return null;
            }
        }

        [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden)]
        public mwProjekt_SponsorPacket SponsorPacket
        {
            get
            {
                if (this.isInitializing) return null;

                if (ContactTypeParticipation.ContactTypesParticipationId.Equals(ContactTypesParticipation.JaId))
                {
                    return (mwProjekt_SponsorPacket)dtMwProjekt_SponsorPacket.Rows[cBResult1.SelectedIndex]["SponsorPaket"];
                }
                else
                {
                    return null;
                }
            }
        }

        [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden)]
        public int SponsorPaketCount
        {
            get
            {
                if (this.isInitializing) return 0;

                if (ContactTypeParticipation.ContactTypesParticipationId.Equals(ContactTypesParticipation.JaId))
                {
                    //int count = 0;

                    return UIValidator.Validate<int>(this.txtAnzahl, this.lblResult2.Text);
                    
                    //if (!int.TryParse(this.txtAnzahl.Text, out count))
                    //{
                    //    throw new FormatException("Bitte geben Sie ein korrekte Anzahl an Sponsorenpaketen an.");
                    //}

                    //return count;
                }
                else
                {
                    return 0;
                }
            }
        }

        [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden)]
        public ThankingsFormsProject[] ThankingForms
        {
            get
            {
                if (this.isInitializing) return null;

                if (ContactTypeParticipation.ContactTypesParticipationId.Equals(ContactTypesParticipation.JaId))
                {
                    ThankingsFormsProject[] thankingForms = new ThankingsFormsProject[checkedListBoxThangingForm.CheckedItems.Count];

                    for (int i = 0; i < checkedListBoxThangingForm.CheckedItems.Count; i++)
                    {
                        thankingForms[i] = (ThankingsFormsProject)checkedListBoxThangingForm.CheckedItems[i];
                    }
                    return thankingForms;
                }
                else
                {
                    return new ThankingsFormsProject[0];
                }
            }
        }

        [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden)]
        public string AdvertisingText
        {
            get
            {
                if (this.isInitializing) return null;


                if (ContactTypeParticipation.ContactTypesParticipationId.Equals(ContactTypesParticipation.JaId))
                {

                    return this.txtResult5.Text;
                }
                else
                {
                    return null;
                }
            }
        }

        [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden)]
        public ContactTypesParticipationCancellation ContactTypesParticipationCancellation
        {
            get
            {
                if (this.isInitializing) return null;

                if (this.ContactTypeParticipation == null)
                    return null;

                if (ContactTypeParticipation.ContactTypesParticipationId.Equals(ContactTypesParticipation.NeinId))
                {
                    if (cBResult1.SelectedItem != null)
                    {

                        //ContactTypesParticipationCancellation cancellation = dtContactTypesParticipationCancellation.Rows[cBResult1.SelectedIndex]["ContactTypesParticipationCancellation"] as ContactTypesParticipationCancellation;

                        //if (cancellation != null)

                        
                        
                        return (ContactTypesParticipationCancellation)dtContactTypesParticipationCancellation.Rows[cBResult1.SelectedIndex]["ContactTypesParticipationCancellation"];
                    }
                    return null;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Liefert den Wert der Auswahl SecondCallDesired (Zweitanruf erwünscht)
        /// oder legt diesen fest.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SecondCallDesiredChoice SecondCallDesired
        {
            get
            {
                if (this.secondCallDesiredRadioButton.Checked)
                    return SecondCallDesiredChoice.Yes;

                if (this.secondCallUndesiredRadioButton.Checked)
                    return SecondCallDesiredChoice.No;

                return SecondCallDesiredChoice.Unset;
            }

            set
            {
                this.secondCallDesiredRadioButton.Checked = value == SecondCallDesiredChoice.Yes;
                this.secondCallDesiredRadioButton.Checked = value == SecondCallDesiredChoice.No;
            }
        }
        

        #endregion

        private void cBResult1_SelectedValueChanged(object sender, EventArgs e)
        {
            ContactTypesParticipation ctp = cBResult0.SelectedValue as ContactTypesParticipation;
            if (ctp != null)
            {
                if (ctp.ContactTypesParticipationId.Equals(ContactTypesParticipation.JaId))
                {
                    txtPreis.Text = string.Format("{0:0.00}", dtMwProjekt_SponsorPacket.Rows[cBResult1.SelectedIndex]["BetragNetto"]);
                }
            }

        }

        public string ReminderAnswer
        {
            get
            {
                return this.reminderAnswer;
            }

            set
            {
                this.reminderAnswer = value;
                this.lblResult1.Text = this.reminderAnswer;
            }
        }

        #region ISupportInitialize Member

        bool isInitializing;
        public void BeginInit()
        {
            this.isInitializing = true;
        }

        public void EndInit()
        {
            this.isInitializing = false;
        }

        #endregion

        private void createReminder1_ValueChanged(object sender, CreateReminderEventArgs e)
        {
            StringBuilder sb = new StringBuilder();

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

            if (sb.Length > 0)
            {
                this.ReminderAnswer = sb.ToString();
            }
            else
            {
                this.ReminderAnswer = string.Empty;
            }
        }


        #region IInitializeCall Member

        public void InitializeCall(Call call)
        {
            if (this.isInitializing) return;


            this.call = call;
            this.faxSend = false;
            FilldtContactTypesParticipation();
            this.faxNoticeRemark = string.Empty;
            this.cBResult0.SelectedIndex = -1;
            this.createReminder1.SetCurrentAgent();
            foreach (Control ctl in this.Controls)
            {
                IInitializeCall initializeCallControl = ctl as IInitializeCall;
                if (initializeCallControl != null)
                {
                    initializeCallControl.InitializeCall(call);
                }
            }
            CleanResult();
            
        }

        #endregion

        private bool faxSend;
        public bool FaxSend
        {
            get { return faxSend; }
        }
	

        private void sendFaxButton_Click(object sender, EventArgs e)
        {
            using (SendFaxDialog sendFaxDlg = new SendFaxDialog(this.call.CallJob, DocumentCategory.Faxangebot))
            {
                
                IDictionary<string, object> logInfos = new Dictionary<string, object>();

                logInfos.Add("user" , MetaCall.Business.Users.CurrentUser);
                logInfos.Add("CallJob", this.call.CallJob);
                logInfos.Add("Session", Environment.GetEnvironmentVariable("SESSIONNAME"));
                //logInfos.Add(MetaCall.Business.GetSystemInformation());

                LogEntry logEntry = new LogEntry("Der Faxdialog wurde instantiert", "Fax", 20, 2001, System.Diagnostics.TraceEventType.Information, "Faxmeldungen", logInfos);

                Logger.Write(logEntry);
                
                if (sendFaxDlg.ShowDialog(this) == DialogResult.OK)
                {

                    MessageBoxOptions options = CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft ?
                        MessageBoxOptions.RtlReading : 0;

                    this.Refresh();
                    this.Cursor = Cursors.WaitCursor;
                    
                    
                    ProjectDocument document = sendFaxDlg.SelectedDocument;
                    ProjectDocument emailTemplate = sendFaxDlg.SelectedEmailTemplate;
                    MetaCall.Business.SponsorPacketBusiness = sendFaxDlg.SponsorPacketsSelected;

                    string faxNumber = sendFaxDlg.FaxNumber;
                    string eMail = sendFaxDlg.EMail;
                    string betreff = sendFaxDlg.Betreff;
                    string briefAnrede = sendFaxDlg.Briefanrede;

                    if (document == null)
                    {
                        string msg = "Sie haben keine Vorlage ausgewählt!";

                        MessageBox.Show(msg, Application.ProductName,
                            MessageBoxButtons.OK, MessageBoxIcon.Information,
                            MessageBoxDefaultButton.Button1, options);

                        return;
                    }
                    try
                    {
                        StringBuilder sbFaxNotice = new StringBuilder();
                        //Sponsor aktualisieren und Fax versenden
                        Sponsor sponsor = this.call.CallJob.Sponsor;

                        sponsor.FaxNummer = faxNumber;
                        sponsor.EMail = eMail;


                        if (sendFaxDlg.MessageTransferMode == MessageTranferMode.Fax)
                        {
                            if (string.IsNullOrEmpty(faxNumber) )
                            {
                                string msg = "Sie haben keine Faxnummer angegeben!";

                                MessageBox.Show(msg, Application.ProductName,
                                    MessageBoxButtons.OK, MessageBoxIcon.Information,
                                    MessageBoxDefaultButton.Button1, options);

                                return;

                            }
                            else
                            {
                                MetaCall.Business.ProjectDocuments.Send(document, this.call.CallJob, SendProjectDocumentOptions.SendFax);
                                sbFaxNotice.AppendFormat("Es wurde am {0} um {1} das Fax {2} an die Nummer {3} versendet.",
                                                            string.Format("{0:d}", DateTime.Now),
                                                            string.Format("{0:t}", DateTime.Now),
                                                            document.DisplayName,
                                                            faxNumber);
                            }
                        }
                        else if (sendFaxDlg.MessageTransferMode == MessageTranferMode.EMail)
                        {
                            if (string.IsNullOrEmpty(betreff))
                            {
                                string msg = "Sie haben keinen Betreff angegeben!";

                                MessageBox.Show(msg, Application.ProductName,
                                    MessageBoxButtons.OK, MessageBoxIcon.Information,
                                    MessageBoxDefaultButton.Button1, options);

                                return;
                            }

                            if (string.IsNullOrEmpty(briefAnrede))
                            {
                                string msg = "Sie haben keine Briefanrede angegeben!";

                                MessageBox.Show(msg, Application.ProductName,
                                    MessageBoxButtons.OK, MessageBoxIcon.Information,
                                    MessageBoxDefaultButton.Button1, options);

                                return;
                            }

                            if (string.IsNullOrEmpty(eMail))
                            {
                                string msg = "Sie haben keine eMail-Adresse angegeben!";

                                MessageBox.Show(msg, Application.ProductName,
                                    MessageBoxButtons.OK, MessageBoxIcon.Information,
                                    MessageBoxDefaultButton.Button1, options);

                                return;
                            }
                            
                            if (emailTemplate == null)
                            {
                                string msg = "Sie haben keine eMail-Vorlage ausgewählt!";

                                MessageBox.Show(msg, Application.ProductName,
                                    MessageBoxButtons.OK, MessageBoxIcon.Information,
                                    MessageBoxDefaultButton.Button1, options);

                                return;
                                
                            }
 
                            MetaCall.Business.ProjectDocuments.Send(document, emailTemplate, this.call.CallJob, betreff, briefAnrede, SendProjectDocumentOptions.SendMail);
                            sbFaxNotice.AppendFormat("Es wurde am {0} um {1} das eMail {2} an die Adresse {3} versendet.",
                                                        string.Format("{0:d}", DateTime.Now),
                                                        string.Format("{0:t}", DateTime.Now),
                                                        document.DisplayName,
                                                        eMail);
                            
                        }
                        else if (sendFaxDlg.MessageTransferMode == MessageTranferMode.PrintOut)
                        {
                            MetaCall.Business.ProjectDocuments.Send(document, this.call.CallJob, SendProjectDocumentOptions.PrintOut, sendFaxDlg.Printer);
                            sbFaxNotice.AppendFormat("Es wurde am {0} um {1} das Dokument {2} gedruckt.",
                                                        string.Format("{0:d}", DateTime.Now),
                                                        string.Format("{0:t}", DateTime.Now),
                                                        document.DisplayName);
                        }
                        this.faxSend = true;
                        if (sbFaxNotice.Length > 0)
                        { 
                            this.faxNoticeRemark = sbFaxNotice.ToString();
                        }
                        else
                        {
                            this.faxNoticeRemark = string.Empty;
                        }
                    }
                    catch (Exception ex)
                    {
                        bool rethrow = ExceptionPolicy.HandleException(ex, "UI Policy");
                        if (rethrow)
                            throw;
                    }
                    finally
                    {
                        this.Cursor = Cursors.Default;
                    }
                }
            }
        }

        private bool ProjectDocumentsPresent(ProjectInfo project)
        {
            List<ProjectDocument> documents = MetaCall.Business.ProjectDocuments.GetDocumentsByProjectAndCategory(
                project, DocumentCategory.Faxangebot);

            return documents.Count > 0;
        }
    }

}
