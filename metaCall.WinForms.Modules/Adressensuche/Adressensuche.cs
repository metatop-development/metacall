using System;
using System.Data;
using System.Windows.Forms;
using MaDaNet.Common.AppFrameWork.ApplicationModul;
using metatop.Applications.metaCall.BusinessLayer;
using metatop.Applications.metaCall.DataAccessLayer;
using metatop.Applications.metaCall.DataObjects;
using metatop.Applications.metaCall.ServiceAccessLayer;

namespace metatop.Applications.metaCall.WinForms.Modules
{
    [ModulIndex(666)]
    public partial class Adressensuche : UserControl, IModulMainControl
    {
        FormStateType formstate = new FormStateType();
        private DataTable callJobsDataTable = new DataTable();

        public enum FormStateType
        {
            ModeEdit,
            ModeView
        }

        private FormStateType FormState
        {
            get
            {
                return this.formstate;
            }

            set
            {
                this.formstate = value;
            }
        }

        public Adressensuche()
        {
            InitializeComponent();

            textBoxAdressenSuchfeld.KeyPress += (sndr, ev) =>
            {
                if (ev.KeyChar.Equals((char)13))
                {
                    this.FindCallJobs(this.textBoxAdressenSuchfeld.Text);
                    ev.Handled = true; // suppress default handling
                }
            };

            SetupCallJobsDataTable();
        }

        #region IModulMainControl Member

        public event ModulInfoMessageHandler StatusMessage;

        public event QueryPermissionHandler QueryPermisson;

        public event ModuleStateChangedHandler StateChanged;

        public void Initialize(IModulMainControl caller)
        {
        }

        public void UnloadModul(out bool canUnload)
        {
            canUnload = true;
        }

        public ToolStrip CreateToolStrip()
        {
            return new ToolStrip();
        }

        public ToolStripMenuItem[] CreateMainMenuItems()
        {
            return null;
        }

        #endregion

        private class Configuration : ModulConfigBase
        {
            public override StartUpMenuItem GetStartUpMenuItem()
            {
                return new StartUpMenuItem("Adressensuche", "Verwaltung");
            }

            public override bool HasStartupMenuItem
            {
                get { return true; }
            }

            public override bool HasMainMenuItems
            {
                get { return false; }
            }

            public override bool HasToolStrip
            {
                get { return false; }
            }
        }

        #region IModulMainControl Member

        public bool CanPauseApplication
        {
            get { return true; }
        }

        #endregion

        #region IModulMainControl Member

        public bool CanAccessModul(System.Security.Principal.IPrincipal principal)
        {
            return principal.IsInRole(MetaCallPrincipal.AdminRoleName);
        }

        #endregion

        private void Adressensuche_Load(object sender, EventArgs e)
        {
            EnableDataFields(true);
        }

        private void EnableDataFields(bool enable)
        {
            this.textBoxAdressenSuchfeld.Enabled = enable;
        }

        private void FindCallJobs(string adressenSuchbegriff)
        {
            if (string.IsNullOrWhiteSpace(adressenSuchbegriff) == true)
            {
                this.callJobsDataTable.Rows.Clear();
            }
            else
            {
                this.callJobsDataTable.Rows.Clear();
                
                foreach (var callJobInfoExtended in MetaCall.Business.CallJobs.GetListCallJobInfoExtendedByAddressSearch(adressenSuchbegriff))
                {
                    CallJob callJob = null;
                    object[] rowData = new object[]{
                        callJobInfoExtended.ProjectTerm,
                        callJobInfoExtended.ProjektJahr,
                        callJobInfoExtended.ProjektMonat,
                        callJobInfoExtended.CallJobGroupTerm,
                        callJobInfoExtended.Sponsor, // callJob.Sponsor.DisplaySortName,
                        callJobInfoExtended.Text1,
                        callJobInfoExtended.Street, // callJob.Sponsor.Strasse,
                        callJobInfoExtended.City, // callJob.Sponsor.DisplayResidence,
                        callJobInfoExtended.Fon, // callJob.Sponsor.TelefonNummer,
                        callJobInfoExtended.StartDate, // callJob.StartDate,
                        callJobInfoExtended.StopDate, // callJob.StopDate,
                        callJobInfoExtended.StateTerm, // callJobStateInfo.DisplayName,
                        callJobInfoExtended.LastResultDisplayName, // resultDisplayName,
                        callJobInfoExtended.IterationCounter, //callJob.IterationCounter,
                        callJobInfoExtended.DialModeTerm, // callJob.DialMode,
                        callJobInfoExtended.CallJobGroupTerm, // callJob.CallJobGroup == null ? null: callJob.CallJobGroup.DisplayName,
                        callJobInfoExtended.UserTerm, // callJob.User == null ? null: callJob.User.DisplayName,
                        callJobInfoExtended.LastOrderAgent, // lastOrderAgent,
                        callJobInfoExtended.LastContact, // lastContact,
                        callJobInfoExtended.LastContactAgent, //lastContactAgent,
                        callJobInfoExtended.AddressSafeActiv, // callJob.AddressSafeActiv,
                        callJobInfoExtended.QuantityOrders,
                        callJobInfoExtended.TotalAmountOrders,
                        callJobInfoExtended.CDSource,
                        callJobInfoExtended.RandomSorter,
                        callJobInfoExtended.CallJobId,
                        callJob}; // callJob

                        this.callJobsDataTable.Rows.Add(rowData);
                };            
                
                dataGridView1.DataSource = this.callJobsDataTable;
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            }
        }

        private void SetupCallJobsDataTable()
        {
            this.callJobsDataTable.CaseSensitive = false;

            DataTableHelper.AddColumn(this.callJobsDataTable, "ProjectTerm", "Projekt", typeof(string));
            DataTableHelper.AddColumn(this.callJobsDataTable, "ProjektJahr", "Projektjahr", typeof(int));
            DataTableHelper.AddColumn(this.callJobsDataTable, "ProjektMonat", "Projektmonat", typeof(int));
            DataTableHelper.AddColumn(this.callJobsDataTable, "CallJobGroupTerm", "Anrufgruppe", typeof(string));
            DataTableHelper.AddColumn(this.callJobsDataTable, "Sponsor", "Sponsor", typeof(string));
            DataTableHelper.AddColumn(this.callJobsDataTable, "Text1", "Zusatz", typeof(string));
            DataTableHelper.AddColumn(this.callJobsDataTable, "Strasse", "Strasse", typeof(string));
            DataTableHelper.AddColumn(this.callJobsDataTable, "Wohnort", "Wohnort", typeof(string));
            DataTableHelper.AddColumn(this.callJobsDataTable, "Telefonnummer", "Telefon", typeof(string));
            DataTableHelper.AddColumn(this.callJobsDataTable, "Start", "Start", typeof(string));
            DataTableHelper.AddColumn(this.callJobsDataTable, "Stop", "Stop", typeof(string));
            DataTableHelper.AddColumn(this.callJobsDataTable, "Status", "Status", typeof(string));
            DataTableHelper.AddColumn(this.callJobsDataTable, "Kontaktart", "Kontaktart", typeof(string));
            DataTableHelper.AddColumn(this.callJobsDataTable, "IterationCounter", "Anrufzähler", typeof(int));
            DataTableHelper.AddColumn(this.callJobsDataTable, "DialMode", "Wählmodus", typeof(string));
            DataTableHelper.AddColumn(this.callJobsDataTable, "CallJobGroup", "Anrufgruppe", typeof(string));
            DataTableHelper.AddColumn(this.callJobsDataTable, "User", "Benutzer", typeof(string));
            DataTableHelper.AddColumn(this.callJobsDataTable, "LastOrderAgent", "Letzter Auftrag", typeof(string));
            DataTableHelper.AddColumn(this.callJobsDataTable, "LastContact", "Letzter Kontakt", typeof(string));
            DataTableHelper.AddColumn(this.callJobsDataTable, "LastContactAgent", "Letzter Mitarbeiter", typeof(string));
            DataTableHelper.AddColumn(this.callJobsDataTable, "AddressSafeActiv", "Kontaktschutz", typeof(Boolean));
            DataTableHelper.AddColumn(this.callJobsDataTable, "QuantitiyOrders", "Stückzahl", typeof(decimal));
            DataTableHelper.AddColumn(this.callJobsDataTable, "TotalAmountOrders", "Umsatz", typeof(decimal));
            DataTableHelper.AddColumn(this.callJobsDataTable, "CDSource", "CD-Quelle", typeof(string));
            DataTableHelper.AddColumn(this.callJobsDataTable, "RandomSorter", "Sorter", typeof(string));
            DataTableHelper.AddColumn(this.callJobsDataTable, "CallJobId", "CallJobId", typeof(Guid), MappingType.Hidden);
            DataTableHelper.AddColumn(this.callJobsDataTable, "CallJob", "CallJob", typeof(CallJob), MappingType.Hidden);

            DataTableHelper.FillGridView(this.callJobsDataTable, this.dataGridView1);
            //Weitere Formatierungen durchführen
            DataGridViewColumn column;

            //Projekt
            column = this.dataGridView1.Columns[0];
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            column.FillWeight = 600;

            //Projektjahr
            column = this.dataGridView1.Columns[1];
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            column.FillWeight = 100;

            //Projektmonat
            column = this.dataGridView1.Columns[2];
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            column.FillWeight = 100;

            //Anrufgruppe
            column = this.dataGridView1.Columns[3];
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            column.FillWeight = 600;

            //Sponsor
            column = this.dataGridView1.Columns[4];
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            column.FillWeight = 600;

            //Zusatz
            column = this.dataGridView1.Columns[5];
            column.Visible = false;
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            column.FillWeight = 300;

            //Strasse
            column = this.dataGridView1.Columns[6];
            column.Visible = true;
            //column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            column.FillWeight = 300;

            //Wohnort
            column = this.dataGridView1.Columns[7];
            column.Visible = true;
            //column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            column.FillWeight = 300;

            //Telefon
            column = this.dataGridView1.Columns[8];
            column.Visible = false;

            //Start
            column = this.dataGridView1.Columns[9];
            column.Visible = false;

            //Stop
            column = this.dataGridView1.Columns[10];
            column.Visible = false;

            //Status
            column = this.dataGridView1.Columns[11];
            column.Visible = true;

            //Kontaktart
            column = this.dataGridView1.Columns[12];
            column.Visible = true;

            //IterationCounter
            column = this.dataGridView1.Columns[13];
            column.Visible = false;

            //DialMode
            column = this.dataGridView1.Columns[14];
            column.Visible = false;

            //Anrufgruppe
            column = this.dataGridView1.Columns[15];
            column.Visible = false;
            column.FillWeight = 300;

            //Benutzer
            column = this.dataGridView1.Columns[16];
            column.Visible = true;
            column.FillWeight = 300;

            //LastOrderAgent
            column = this.dataGridView1.Columns[17];
            column.Visible = false;

            //LastContactDate Other Project
            column = this.dataGridView1.Columns[18];
            column.Visible = false;

            //LastContactAgent
            column = this.dataGridView1.Columns[19];
            column.Visible = false;

            //Address Activ
            column = this.dataGridView1.Columns[20];
            column.Visible = false;

            //QuantityOrders
            column = this.dataGridView1.Columns[21];
            column.Visible = false;
            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            column.DefaultCellStyle.Format = "#,##0.0";
            column.Width = 50;

            //TotalAmountOrders
            column = this.dataGridView1.Columns[22];
            column.Visible = false;
            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            column.DefaultCellStyle.Format = "#,##0.00";
            column.Width = 55;

            //Sorter
            column = this.dataGridView1.Columns[23];
            column.Visible = false;

            //CDSource
            column = this.dataGridView1.Columns[24];
            column.Visible = false;

            foreach (DataGridViewColumn col in this.dataGridView1.Columns)
            {
                col.MinimumWidth = 10;
            }
        }

        private void textBoxAdressenSuchfeld_Leave(object sender, EventArgs e)
        {
            this.FindCallJobs(this.textBoxAdressenSuchfeld.Text);
        }
    }
}
