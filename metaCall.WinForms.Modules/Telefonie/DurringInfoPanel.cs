using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

using metatop.Applications.metaCall.BusinessLayer;
using metatop.Applications.metaCall.DataObjects;
using System.Collections.ObjectModel;

namespace metatop.Applications.metaCall.WinForms.Modules.Telefonie
{
    [ToolboxItem(false)]
    public partial class DurringInfoPanel : UserControl
    {

        public event UserWantSpecialCallHandler UserWantSpecialCall;
        private DataTable dataTable = new DataTable();

        public DurringInfoPanel()
        {
            InitializeComponent();

            dataTable.Locale = System.Globalization.CultureInfo.InvariantCulture;
            bindingSourceDurring.DataSource = dataTable;

            SetupDataTable();
        }

        public void DurringLevelInfoChanged()
        {
            User user = MetaCall.Business.Users.CurrentUser;
            int durringLevel = MetaCall.Business.CallJobs.Mahnstufe2;
            if (durringLevel != -1)
            {
                // laden mit der Mahnstufe
                MetaCall.Business.SponsoringCallManager.InizializeDurringInfo(user, durringLevel);

                LoadDurringInfoTable(MetaCall.Business.SponsoringCallManager.DurringInfo);
            }
            else
            {
                // leer laden
                bindingSourceDurring.SuspendBinding();
                dataTable.Rows.Clear();
                bindingSourceDurring.ResumeBinding();
            }
        }


        private void SetupDataTable()
        {
            DataTableHelper.AddColumn(dataTable, "CallJobId", "CallJobId",typeof(Guid), MappingType.Hidden);
            DataTableHelper.AddColumn(dataTable, "Rechnungsnummer", "Re.-Nr.", typeof(int));
            DataTableHelper.AddColumn(dataTable, "Rechnungsdatum", "Re.Dat.", typeof(DateTime));
            DataTableHelper.AddColumn(dataTable, "Mahnstufe2", "Akt.", typeof(int));
            DataTableHelper.AddColumn(dataTable, "Sponsortext", "Sponsor", typeof(string));
            DataTableHelper.AddColumn(dataTable, "Prio", "Prio", typeof (string));
            DataTableHelper.AddColumn(dataTable, "Aktionsdatum", "letzt. Akt.", typeof (DateTime));
            DataTableHelper.AddColumn(dataTable, "BearbeiterMahnungsaktion", "Bearb.", typeof (string));
            DataTableHelper.AddColumn(dataTable, "BemerkungMahnungsaktion", "Bem.", typeof (string));
            DataTableHelper.AddColumn(dataTable, "Wiedervorlage", "WV", typeof (DateTime));
            DataTableHelper.AddColumn(this.dataTable, "DurringInfo", "", typeof(DurringInfo), MappingType.Hidden);


            DataColumn[] PrimaryKeyColumns = new DataColumn[1];
            PrimaryKeyColumns[0] = dataTable.Columns["CallJobId"];
            dataTable.PrimaryKey = PrimaryKeyColumns;
            
/*
            e.AnsprechpartnerText;
            e.BetragOffen;
            e.Brutto;
            e.Mahnungsdatum;
            e.ProjekteRechnungsnummer;
            e.ProjektText;
            e.Stueckzahl;
            e.Telefonnumer;
*/         

            DataTableHelper.FillGridView(dataTable, this.dataGridViewDurring);

            //Formatieren der Spalten 
            //this.dataGridViewDurring.Columns[0].Visible = false;

            //this.dataGridViewDurring.Width = 1500;

            this.dataGridViewDurring.Columns[0].Width = 75;
            this.dataGridViewDurring.Columns[1].Width = 75;
            this.dataGridViewDurring.Columns[2].Width = 20;
            this.dataGridViewDurring.Columns[3].Width = 250;
            this.dataGridViewDurring.Columns[4].Width = 80;
            this.dataGridViewDurring.Columns[5].Width = 80;
            this.dataGridViewDurring.Columns[6].Width = 150;
            this.dataGridViewDurring.Columns[7].Width = 80;
            this.dataGridViewDurring.Columns[8].Width = 120;

            this.dataGridViewDurring.Columns[0].MinimumWidth = 75;
            this.dataGridViewDurring.Columns[1].MinimumWidth = 75;
            this.dataGridViewDurring.Columns[2].MinimumWidth = 20;
            this.dataGridViewDurring.Columns[3].MinimumWidth = 100;
            this.dataGridViewDurring.Columns[4].MinimumWidth = 80;
            this.dataGridViewDurring.Columns[5].MinimumWidth = 80;
            this.dataGridViewDurring.Columns[6].MinimumWidth = 80;
            this.dataGridViewDurring.Columns[7].MinimumWidth = 80;
            this.dataGridViewDurring.Columns[8].MinimumWidth = 120;

            //this.dataGridViewDurring.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            //this.dataGridViewDurring.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            //this.dataGridViewDurring.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            //this.dataGridViewDurring.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            //this.dataGridViewDurring.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            //this.dataGridViewDurring.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            //this.dataGridViewDurring.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            //this.dataGridViewDurring.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            //this.dataGridViewDurring.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;

            //foreach (DataGridViewColumn col in dataGridViewDurring.Columns)
            //    col.SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        private void LoadDurringInfoTable(List<DurringInfo> durringInfo)
        {
            System.Diagnostics.Trace.WriteLine("LoadDurringList()");

            bindingSourceDurring.SuspendBinding();
            try
            {
                dataTable.Rows.Clear();
                if (durringInfo != null)
                {

                    foreach (DurringInfo dI in durringInfo)
                    {
                        object[] rowData = new object[]{
                        dI.CallJobId,
                        dI.Rechnungsnummer,
                        dI.Rechnungsdatum,
                        dI.Mahnstufe2,
                        dI.Sponsortext, 
                        dI.Prio,
                        dI.Aktionsdatum,
                        dI.BearbeiterMahnungsaktion,
                        dI.BemerkungMahnungsaktion,
                        dI.Wiedervorlage,
                        dI
                    };

                        dataTable.Rows.Add(rowData);
                    }
                }
            }
            finally
            {
                bindingSourceDurring.ResumeBinding();
            }
        }

        private void DurringInfoPanel_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            MetaCall.Business.SponsoringCallManager.DurringInfoChanged += new DurringInfoChangedEventHandler(SponsoringCallManager_DurringInfoChanged);

            LoadDurringInfoTable(MetaCall.Business.SponsoringCallManager.DurringInfo);
        }

        void SponsoringCallManager_DurringInfoChanged(object sender, DurringInfoChangedEventArgs e)
        {
            DataRow dR;

            bindingSourceDurring.SuspendBinding();

            dR = dataTable.Rows.Find(e.RemoveDurringInfo.CallJobId);
            if (dR != null)
                dataTable.Rows.Remove(dR);

            bindingSourceDurring.ResumeBinding();
        }

        protected void OnUserWantSpecialCall(UserWantSpecialCallEventArgs e)
        {
            if (UserWantSpecialCall != null)
                UserWantSpecialCall(this, e);
        }

        private void dataGridViewDurring_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                UserInfo uInfo = MetaCall.Business.Users.GetUserInfo(MetaCall.Business.Users.CurrentUser);
                DurringInfo info = (DurringInfo) dataTable.Rows[e.RowIndex]["DurringInfo"];
                CallJob callJob = MetaCall.Business.CallJobs.Get(info.CallJobId);

                Call call;
                call = MetaCall.Business.SponsoringCallManager.CheckCallExists(callJob);

                if (call == null)
                    call = MetaCall.Business.SponsoringCallManager.GetSingleCall(callJob, uInfo);

                //Call call = new Call();
                //call.CallDate = System.DateTime.Now;
                //call.CallId = Guid.NewGuid();
                //call.CallJob = callJob;
                //call.DialMode = DialMode.AutoSoftwareDialing;
                //call.ExpirationDate = System.DateTime.Now.AddMinutes(10);
                //call.PhoneNumber = callJob.Sponsor.TelefonNummer;
                MetaCall.Business.SponsoringCallManager.ActualDurringInfo = info;
                OnUserWantSpecialCall(new UserWantSpecialCallEventArgs(call));
            }
        }

    }
}
