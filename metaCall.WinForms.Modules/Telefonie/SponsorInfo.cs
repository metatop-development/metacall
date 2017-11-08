using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using metatop.Applications.metaCall.DataObjects;
using System.Globalization;

namespace metatop.Applications.metaCall.WinForms.Modules.Telefonie
{
    [ToolboxItem(false)]
    public partial class SponsorInfo : ExpandableUserControl, IMessageFilter, IInitializeCall
    {
        private Sponsor sponsor;
        private ProjectInfo project;
        private CallJobGroup callJobGroup;
        private mwProject mwProject;
        private bool sponsorInfoIsOpen = false;

        private DataTable dtMwprojekt_SponsorOrderHistorie = new DataTable();

        public SponsorInfo()
        {
            InitializeComponent();

            this.dtMwprojekt_SponsorOrderHistorie.Locale = CultureInfo.CurrentUICulture;
            this.bindingSource1.DataSource = dtMwprojekt_SponsorOrderHistorie;

            SetupDataTableSponsorOrders();


            Application.AddMessageFilter(this);

        }

        public Branch Branch
        {
            set
            {
                if (this.sponsor != null)
                    this.sponsor.Branch = value;
            }
        }

        public BranchGroup BranchGroup
        {
            set
            {
                if (this.sponsor != null)
                    this.sponsor.BranchGroup = value;
            }
        }

        public string TelefonNummer
        {
            set
            {
                if (this.sponsor != null)
                    this.sponsor.TelefonNummer = value;
            }
        }

        public string Mobilnummer
        {
            set
            {
                if (this.sponsor != null)
                    this.sponsor.MobilNummer = value;
            }
        }

        public string Alternativnummer
        {
            set
            {
                if (this.sponsor != null)
                    this.sponsor.Additions.Phone3 = value;
            }
        }
        

        private void UpdateSponsorInformations()
        {

            if (this.sponsor != null)
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(this.UpdateSponsorInformations));
                    return;
                }

                //Abrufen der zugewiesenen GeoZone
                GeoZone geoZone = GeoZone.Unknown;
                if (this.project != null)
                {
                    geoZone = MetaCall.Business.Addresses.GetGeoZoneByProject(this.sponsor, this.project);
                }
                
                this.txtBezeichnung.Text = sponsor.DisplayName;

                this.lblText1.Text = sponsor.Text1;
                this.lblText2.Text = sponsor.Text2;
                this.lblStrasse.Text = sponsor.Strasse;
                this.lblDisplayResidence.Text = sponsor.DisplayResidence;
                this.lblContactDisplay.Text = sponsor.ContactPerson.DisplayName;

                this.lblTelefonNummer.Text = sponsor.TelefonNummer;
                this.lblFaxNummer.Text = sponsor.FaxNummer;
                this.lblMobilNummer.Text = sponsor.MobilNummer;

                this.lblBranche.Text = sponsor.Branch != null ? sponsor.Branch.Bezeichnung : string.Empty;
                this.lblBranchengruppe.Text = sponsor.BranchGroup != null ? sponsor.BranchGroup.BranchenGruppe : string.Empty;
                this.lblZone.Text = geoZone == GeoZone.Unknown ? string.Empty:  geoZone.Zone.ToString();
                if (callJobGroup != null)
                {
                    this.lBGruppe.Text = callJobGroup.DisplayName;
                }
                else
                {
                    this.lBGruppe.Text = "";
                }
            }
            else
            {
                this.txtBezeichnung.Text = string.Empty;
                this.lBGruppe.Text = string.Empty;
                this.lblText1.Text = string.Empty;
                this.lblText2.Text = string.Empty;
                this.lblStrasse.Text = string.Empty;
                this.lblDisplayResidence.Text = string.Empty;

                this.lblContactDisplay.Text = string.Empty;

                this.lblTelefonNummer.Text = string.Empty;
                this.lblFaxNummer.Text = string.Empty;
                this.lblMobilNummer.Text = string.Empty;
                this.lblBranche.Text = string.Empty;
                this.lblBranchengruppe.Text = string.Empty;
                this.lblZone.Text = string.Empty;
                this.lBGruppe.Text = string.Empty;
            }


            FillSponsorOrdersDataTable(this.sponsor);
        }


        void sponsor_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateSponsorInformations();
        }

        private void SetupDataTableSponsorOrders()
        {

            DataTableHelper.AddColumn(this.dtMwprojekt_SponsorOrderHistorie, "Projektbezeichnung", "Projektbezeichnung", typeof(string));
            DataTableHelper.AddColumn(this.dtMwprojekt_SponsorOrderHistorie, "Quelle", "Quelle", typeof(string));
            DataTableHelper.AddColumn(this.dtMwprojekt_SponsorOrderHistorie, "OrderDate", "OrderDate", typeof(string));
            DataTableHelper.AddColumn(this.dtMwprojekt_SponsorOrderHistorie, "Agent", "Agent", typeof(string));
            DataTableHelper.AddColumn(this.dtMwprojekt_SponsorOrderHistorie, "Stückzahl", "Stückzahl", typeof(decimal));
            DataTableHelper.AddColumn(this.dtMwprojekt_SponsorOrderHistorie, "Umsatz", "Umsatz", typeof(decimal));
            DataTableHelper.AddColumn(this.dtMwprojekt_SponsorOrderHistorie, "OrderState", "OrderState", typeof(string));

            

            dGVOrders.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dGVOrders.RowHeadersVisible = false;
            dGVOrders.ColumnHeadersVisible = false;
            dGVOrders.AutoGenerateColumns = false;

            DataTableHelper.FillGridView(this.dtMwprojekt_SponsorOrderHistorie,
                this.dGVOrders);


            dGVOrders.BackgroundColor = Color.Lavender;

            dGVOrders.Columns[0].Width = 270;
            dGVOrders.Columns[1].Width = 40;

        }

        private void FillSponsorOrdersDataTable(Sponsor sponsor)
        {
            this.bindingSource1.SuspendBinding();

            try
            {
                dtMwprojekt_SponsorOrderHistorie.Rows.Clear();
                if (sponsor != null)
                {
                    foreach (mwProjekt_SponsorOrderHistorie mwprojekt_SponsorOrderHistorie in MetaCall.Business.mwProjekt_SponsorOrderHistorie.GetmwProjekt_SponsorOrderHistorie(sponsor.AdressenPoolNummer))
                    {
                        object[] rowData = new object[]{
                            mwprojekt_SponsorOrderHistorie.Projektbezeichnung,
                            mwprojekt_SponsorOrderHistorie.Quelle, 
                            mwprojekt_SponsorOrderHistorie.OrderDate == DateTime.MinValue ? null : string.Format(@"{0:yyyy\/MM}",mwprojekt_SponsorOrderHistorie.OrderDate),
                            mwprojekt_SponsorOrderHistorie.Agent,
                            mwprojekt_SponsorOrderHistorie.Stueckzahl, 
                            mwprojekt_SponsorOrderHistorie.Umsatz,
                            mwprojekt_SponsorOrderHistorie.OrderState};

                        dtMwprojekt_SponsorOrderHistorie.Rows.Add(rowData);
                    }
                }
            }
            finally
            {
                this.bindingSource1.ResumeBinding();
            }
        }

        private bool checkValidSponsorInformation;

        /// <summary>
        /// Gibt an, dass das Control prüfen und anzeigen soll, 
        /// ob die Sponsorinformationen für einen Auftrag ausreichend sind,
        /// oder legt diesen Wert fest
        /// </summary>
        public bool CheckValidSponsorInformation
        {
            get { return checkValidSponsorInformation; }
            set { 
                checkValidSponsorInformation = value;

                SetSponsorValidation();
            }
        }

        private void SetSponsorValidation()
        {
            this.checkSponsorInformationTimer.Interval = 1500;
            this.checkSponsorInformationTimer.Enabled = this.checkValidSponsorInformation;

            if (!this.checkValidSponsorInformation)
                this.txtBezeichnung.ForeColor = this.ForeColor;
        }
	
        protected override void OnDoubleClick(EventArgs e)
        {
            if (sponsorInfoIsOpen == false)
            {
                sponsorInfoIsOpen = true;
                Language language = Language.German;
                if (this.mwProject != null)
                    language = mwProject.Language;

                using (SponsorEdit sponsorEditDlg = new SponsorEdit(this.sponsor, language, this.checkValidSponsorInformation))
                {
                    if (sponsorEditDlg.ShowDialog(this) == DialogResult.OK)
                    {
                        this.UpdateSponsorInformations();
                    }
                    sponsorInfoIsOpen = false;
                }
            }
        }

        #region IMessageFilter Member

        public bool PreFilterMessage(ref Message m)
        {
            //Fängt einen DoppelKlick auf das Control ab und behandelt das ereignis.
            //An die ClientControls wird kein Doppelklick mehr gesendet.
            if (m.Msg == ((int) WinUser.WM_LBUTTONDBLCLK))
            {
                //prüfen ob auf das Control geklickt wurde
                if (this.Bounds.Contains(
                    this.PointToClient(MousePosition)) && 
                    this.Visible)
                {
                OnDoubleClick(EventArgs.Empty);
                return true;
                }
            }
            return false;
        }

        #endregion

        public override bool PreProcessMessage(ref Message msg)
        {
            return base.PreProcessMessage(ref msg);
        }

        private void checkSponsorInformationTimer_Tick(object sender, EventArgs e)
        {

            if (this.sponsor != null)
            {
                List<string> wrongFields;
                bool isValid = MetaCall.Business.Addresses.IsSponsorValidForOrdering(this.sponsor, out wrongFields);
                if (!isValid && (this.txtBezeichnung.ForeColor == this.ForeColor))
                {
                    this.txtBezeichnung.ForeColor = Color.Red;
                }
                else
                {
                    this.txtBezeichnung.ForeColor = this.ForeColor;
                }
            }
        }

        #region IInitializeCall Member

        public void InitializeCall(Call call)
        {
            this.sponsor = call.CallJob.Sponsor;
            this.project = call.CallJob.Project;
            this.mwProject = MetaCall.Business.Projects.GetMwProject(call.CallJob.Project);
            this.callJobGroup = call.CallJobGroup;


            if (callJobGroup != null)
            {
                if (callJobGroup.Type == CallJobGroupType.TipAddress)
                {
                    this.labelTipInfo.Text = "Dies ist eine TIP-Adresse!";
                    this.labelTipInfo.Visible = true;
                }
                else
                {
                    this.labelTipInfo.Text = "Sponsor hat im letzten Projekt für dieses zugesagt!";
                    this.labelTipInfo.Visible = MetaCall.Business.Addresses.GetTipAddressLastProject(this.sponsor, this.project);
                }
            }
            else
            {
                this.labelTipInfo.Visible = false;
            }

            this.sponsor.PropertyChanged += new PropertyChangedEventHandler(sponsor_PropertyChanged);

            UpdateSponsorInformations();
        }

        #endregion
    }
}
