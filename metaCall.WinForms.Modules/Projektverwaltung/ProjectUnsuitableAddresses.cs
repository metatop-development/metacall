using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using metatop.Applications.metaCall.DataObjects;

namespace metatop.Applications.metaCall.WinForms.Modules
{
    public partial class ProjectUnsuitableAddresses : UserControl
    {
        private Project project = null;
        private bool isInWork = false;
        private DataTable dtUnsuitableCallJobs = new DataTable();
        private Guid allUsersId = new Guid("9879A15E-DB1C-46C9-9DE4-74D34B3334C6");
        private Guid allReasonsId = new Guid("544AD687-5B8F-490B-9531-DC06AF0C0895");

        public ProjectUnsuitableAddresses()
        {
            InitializeComponent();
        }

        public ProjectUnsuitableAddresses(Project project)
            : this()
        {
            this.project = project;
            this.lblProject.Text = project.Bezeichnung;
            BindUserComboBox();
            BindReasonComboBox();

            this.bindingSourceUnsuitableCallJobs.DataSource = this.dtUnsuitableCallJobs;
            this.dataGridViewUnsuitableCallJobs.Columns.Clear();
            SetupDataTableUnsuitableCallJobs();
            // die Standard-Guids stammen aus den Stored Procedures
            // und sind dort mittels eines Union-Selects festgelegt
            List<CallJobUnsuitableInfo> callJobUnsuitableInfos = MetaCall.Business.CallJobInfoUnsuitable.GetListCallJobsUnsuitableInfoByProject(
                this.project, allUsersId,
                allReasonsId);
            foreach (CallJobUnsuitableInfo callJobUnsuitableInfo in callJobUnsuitableInfos)
            {
                DataTableUnsuitableCalljobsAdd(callJobUnsuitableInfo);
            }

            //DataTableHelper.FillGridView(this.dtUnsuitableCallJobs, this.dataGridViewUnsuitableCallJobs);


        }

        public bool IsInWork
        {
            get { return isInWork; }
        }

        public Project CurrentProject
        {
            get { return this.project; }
        }

        public void BindUserComboBox()
        {
            userComboBox.Items.Clear();
            CallJobUnsuitableInfoUser[] cuius = MetaCall.Business.CallJobInfoUnsuitable.GetListCallJobsUnsuitableInfoUsersByProject(
                this.project);
            foreach (CallJobUnsuitableInfoUser cuiu in cuius)
            {
                userComboBox.Items.Add(cuiu);
                if (cuiu.UserId.CompareTo(allUsersId) == 0)
                    userComboBox.SelectedItem = cuiu;
            }
            userComboBox.DisplayMember = "DisplayName";
        }

        public void BindReasonComboBox()
        {
            reasonComboBox.Items.Clear();
            CallJobUnsuitableInfoReason[] cuirs = MetaCall.Business.CallJobInfoUnsuitable.GetListCallJobsUnsuitableInfoReasonsByProject(
                this.project);
            foreach (CallJobUnsuitableInfoReason cuir in cuirs)
            {
                reasonComboBox.Items.Add(cuir);
                if (cuir.ContactTypesParticipationUnsuitableId.CompareTo(allReasonsId) == 0)
                    reasonComboBox.SelectedItem = cuir;
            }
            reasonComboBox.DisplayMember = "DisplayName";
        }

        private void userComboBox_NotInList(object sender, CancelEventArgs e)
        {
            MessageBox.Show("Sie können nur einen Eintrag aus der Liste wählen!", "Kein Element der Liste");
            e.Cancel = true;
        }

        private void reasonComboBox_NotInList(object sender, CancelEventArgs e)
        {
            MessageBox.Show("Sie können nur einen Eintrag aus der Liste wählen!", "Kein Element der Liste");
            e.Cancel = true;
        }

        private void DataTableUnsuitableCalljobsAdd(CallJobUnsuitableInfo cui)
        {
            this.bindingSourceUnsuitableCallJobs.SuspendBinding();

            try
            {
                Object[] objectData = new object[]{
                    cui.CallJobId, cui.Nachname, cui.Vorname,
                    cui.Text1, cui.PLZ, cui.Ort, cui.PhoneNumber, cui.Quelle,
                    cui.DisplayNameContactType, cui.DisplayNameUnsuitableType,
                    cui.Start, cui.Agent, cui.AdresseNichtGeeignet, 
                    cui.AddressId, cui.AdressenPoolNummer, 
                    cui.ContactTypesParticipationUnsuitableId,
                    cui.UserId};

                this.dtUnsuitableCallJobs.Rows.Add(objectData);
            }
            finally
            {
                this.bindingSourceUnsuitableCallJobs.ResumeBinding();
            }
        }

        private void SetupDataTableUnsuitableCallJobs()
        {

            DataTableHelper.AddColumn(this.dtUnsuitableCallJobs, "CallJobId", "CallJobId", typeof(Guid));
            DataTableHelper.AddColumn(this.dtUnsuitableCallJobs, "Nachname", "Nachname", typeof(string));
            DataTableHelper.AddColumn(this.dtUnsuitableCallJobs, "Vorname", "Vorname", typeof(string));
            DataTableHelper.AddColumn(this.dtUnsuitableCallJobs, "Text1", "Text 1", typeof(string));
            DataTableHelper.AddColumn(this.dtUnsuitableCallJobs, "PLZ", "PLZ", typeof(string));
            DataTableHelper.AddColumn(this.dtUnsuitableCallJobs, "Ort", "Ort", typeof(string));
            DataTableHelper.AddColumn(this.dtUnsuitableCallJobs, "PhoneNumber", "Telefon", typeof(string));
            DataTableHelper.AddColumn(this.dtUnsuitableCallJobs, "Quelle", "Quelle", typeof(string));
            DataTableHelper.AddColumn(this.dtUnsuitableCallJobs, "DisplayNameContactType", "Kontaktart", typeof(string));
            DataTableHelper.AddColumn(this.dtUnsuitableCallJobs, "DisplayNameUnsuitableType", "Grund", typeof(string));
            DataTableHelper.AddColumn(this.dtUnsuitableCallJobs, "Start", "Anruf am", typeof(string));
            DataTableHelper.AddColumn(this.dtUnsuitableCallJobs, "Agent", "Agent", typeof(string));
            DataTableHelper.AddColumn(this.dtUnsuitableCallJobs, "AdresseNichtGeeignet", "Bestätigt", typeof(Boolean));
            DataTableHelper.AddColumn(this.dtUnsuitableCallJobs, "AddressId", "AddressId", typeof(Guid));
            DataTableHelper.AddColumn(this.dtUnsuitableCallJobs, "AdressenPoolNummer", "AdressenPoolNummer", typeof(int));
            DataTableHelper.AddColumn(this.dtUnsuitableCallJobs, "ContactTypesParticipationUnsuitableId", "ContactTypesParticipationUnsuitableId", typeof(Guid));
            DataTableHelper.AddColumn(this.dtUnsuitableCallJobs, "UserId", "UserId", typeof(Guid));

            DataTableHelper.FillGridView(this.dtUnsuitableCallJobs, this.dataGridViewUnsuitableCallJobs);

            //Weitere Formatierungen durchführen
            DataGridViewColumn column;
            column = this.dataGridViewUnsuitableCallJobs.Columns[0];
            column.Visible = false;

            //Nachname
            column = this.dataGridViewUnsuitableCallJobs.Columns[1];
            column.Width = 120;
            //column.Resizable = DataGridViewTriState.True;

            //Vorname
            column = this.dataGridViewUnsuitableCallJobs.Columns[2];
            //column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader;
            column.Width = 40;

            //Text1
            column = this.dataGridViewUnsuitableCallJobs.Columns[3];
            //column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader;
            column.Width = 80;

            //PLZ
            column = this.dataGridViewUnsuitableCallJobs.Columns[4];
            //column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader;
            column.Width = 30;
            column.Visible = false;

            //Ort            
            column = this.dataGridViewUnsuitableCallJobs.Columns[5];
            //column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader;
            column.Width = 100;
            column.Visible = false;

            //PhoneNumber
            column = this.dataGridViewUnsuitableCallJobs.Columns[6];
            //column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader;
            column.Width = 90;

            //Quelle
            column = this.dataGridViewUnsuitableCallJobs.Columns[7];
            //column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader;
            column.Width = 30;

            //Kontaktart
            column = this.dataGridViewUnsuitableCallJobs.Columns[8];
            //column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader;
            column.Width = 80;
            
            //Grund
            column = this.dataGridViewUnsuitableCallJobs.Columns[9];
            //column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader;
            column.Width = 100;

            //Anruf am
            column = this.dataGridViewUnsuitableCallJobs.Columns[10];
            //column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader;
            column.Width = 100;

            //Agent
            column = this.dataGridViewUnsuitableCallJobs.Columns[11];
            //column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader;
            column.Width = 80;

            //Bestätigt
            column = this.dataGridViewUnsuitableCallJobs.Columns[12];
            //column.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            column.Width = 50;

            //AddressId
            column = this.dataGridViewUnsuitableCallJobs.Columns[13];
            column.Visible = false;

            //AdressenPoolNummer
            column = this.dataGridViewUnsuitableCallJobs.Columns[14];
            column.Width = 100;
            column.Visible = false;

            column = this.dataGridViewUnsuitableCallJobs.Columns[15];
            column.Visible = false;

            column = this.dataGridViewUnsuitableCallJobs.Columns[16];
            column.Visible = false;
        }

        private void unsuitableContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            this.unsuitableContextMenuStrip.Items.Clear();

            ToolStripMenuItem selectAllAndConfirm = new ToolStripMenuItem("Alle auswählen + bestätigen");
            selectAllAndConfirm.Click += new EventHandler(selectAllAndConfirm_Click);
            this.unsuitableContextMenuStrip.Items.Add(selectAllAndConfirm);

            ToolStripSeparator separatorAlle = new ToolStripSeparator();
            separatorAlle.AutoSize = true;
            this.unsuitableContextMenuStrip.Items.Add(separatorAlle);

            ToolStripMenuItem selectedAll = new ToolStripMenuItem("Alle auswählen");
            selectedAll.Click += new EventHandler(selectedAll_Click);
            this.unsuitableContextMenuStrip.Items.Add(selectedAll);

            ToolStripMenuItem selectedNothing = new ToolStripMenuItem("Auswahl aufheben");
            selectedNothing.Click += new EventHandler(selectedNothing_Click);
            this.unsuitableContextMenuStrip.Items.Add(selectedNothing);

            ToolStripSeparator separatorAuswahl = new ToolStripSeparator();
            separatorAuswahl.AutoSize = true;
            this.unsuitableContextMenuStrip.Items.Add(separatorAuswahl);

            ToolStripMenuItem setDone = new ToolStripMenuItem("Bestätigen");
            setDone.Click += new EventHandler(setDone_Click);
            this.unsuitableContextMenuStrip.Items.Add(setDone);

            ToolStripMenuItem setUnDone = new ToolStripMenuItem("Bestätigen aufheben");
            setUnDone.Click += new EventHandler(setUnDone_Click);
            this.unsuitableContextMenuStrip.Items.Add(setUnDone);

            ToolStripSeparator separatorConfirm = new ToolStripSeparator();
            separatorConfirm.AutoSize = true;
            this.unsuitableContextMenuStrip.Items.Add(separatorConfirm);

            //Zusammenstellen des Spaltenauswahl
            ToolStripMenuItem columnSelection = new ToolStripMenuItem("Spalten");
            columnSelection.AutoSize = true;

            foreach (DataGridViewColumn column in this.dataGridViewUnsuitableCallJobs.Columns)
            {
                if (!(column.DataPropertyName == "CallJobId" || column.DataPropertyName == "ContactTypesParticipationUnsuitableId"
                    || column.DataPropertyName == "AddressId" || column.DataPropertyName == "UserId"))
                {
                    ToolStripMenuItem columnChecked = new ToolStripMenuItem(column.HeaderText);
                    columnChecked.AutoSize = true;
                    columnChecked.DisplayStyle = ToolStripItemDisplayStyle.Text;
                    columnChecked.CheckOnClick = true;
                    columnChecked.Checked = column.Visible;
                    columnChecked.Tag = column;
                    columnChecked.CheckedChanged += new EventHandler(columnChecked_CheckedChanged);
                    columnSelection.DropDownItems.Add(columnChecked);
                }
            }

            //Spaltenauswahl
            if (columnSelection.DropDownItems.Count > 0)
                this.unsuitableContextMenuStrip.Items.Add(columnSelection);

            this.unsuitableContextMenuStrip.AutoClose = true;
            this.unsuitableContextMenuStrip.ShowCheckMargin = false;
            this.unsuitableContextMenuStrip.ShowImageMargin = false;
            this.unsuitableContextMenuStrip.ShowItemToolTips = true;

            e.Cancel = (this.unsuitableContextMenuStrip.Items.Count < 1);

        }

        void selectAllAndConfirm_Click(object sender, EventArgs e)
        {
            selectedAll_Click(sender, e);
            setDone_Click(sender, e);
        }

        void setUnDone_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in this.dataGridViewUnsuitableCallJobs.SelectedRows)
            {
                DataRowView dataRow = this.bindingSourceUnsuitableCallJobs[row.Index] as DataRowView;

                dataRow.BeginEdit();
                dataRow["AdresseNichtGeeignet"] = false;
                dataRow.EndEdit();
            }
        }

        void setDone_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in this.dataGridViewUnsuitableCallJobs.SelectedRows)
            {
                DataRowView dataRow = this.bindingSourceUnsuitableCallJobs[row.Index] as DataRowView;

                dataRow.BeginEdit();
                dataRow["AdresseNichtGeeignet"] = true;
                dataRow.EndEdit();
            } 
        }

        void selectedNothing_Click(object sender, EventArgs e)
        {
            this.dataGridViewUnsuitableCallJobs.ClearSelection();
        }

        void selectedAll_Click(object sender, EventArgs e)
        {
            this.dataGridViewUnsuitableCallJobs.SelectAll();
        }

        private void columnChecked_CheckedChanged(object sender, EventArgs e)
        {
            ToolStripMenuItem columnChecked = sender as ToolStripMenuItem;

            if (columnChecked != null)
            {
                DataGridViewColumn column = columnChecked.Tag as DataGridViewColumn;
                if (column != null)
                {
                    column.Visible = columnChecked.Checked;
                }
            }
        }


        private void Filter()
        {
            string filterUser = string.Empty;
            string filterReason = string.Empty;
            string filter = string.Empty;
            Guid userId;
            Guid contactTypesParticipationUnsuitableId;

            CallJobUnsuitableInfoUser cuiu = (CallJobUnsuitableInfoUser)userComboBox.SelectedItem;
            CallJobUnsuitableInfoReason cuir = (CallJobUnsuitableInfoReason)reasonComboBox.SelectedItem;

            if (cuiu == null) 
                userId = allUsersId;
            else
                userId = cuiu.UserId;

            if (cuir == null)
                contactTypesParticipationUnsuitableId = allReasonsId;
            else
                contactTypesParticipationUnsuitableId = cuir.ContactTypesParticipationUnsuitableId;

            if (userId == allUsersId && contactTypesParticipationUnsuitableId == allReasonsId)
            {
                this.bindingSourceUnsuitableCallJobs.RemoveFilter();
                return;
            }

            if (userId != allUsersId)
            {
                filter = "UserId = '{0}'";
                filter = string.Format(filter, userId.ToString());
            }
            
            if (contactTypesParticipationUnsuitableId != allReasonsId &&
                filter != string.Empty)
            {
                filter = filter + " AND ";
            }

            if (contactTypesParticipationUnsuitableId != allReasonsId)
            {
                filter = filter + "contactTypesParticipationUnsuitableId = '{0}'";
                filter = string.Format(filter, contactTypesParticipationUnsuitableId.ToString());
            }
            this.bindingSourceUnsuitableCallJobs.Filter = filter;
        }

        private void userComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            Filter();
        }

        private void reasonComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            Filter();
        }

        public void UpdateAddresses()
        {
            int i = 0;
            this.bindingSourceUnsuitableCallJobs.RemoveFilter();
            
            CallJobUnsuitableAddressChanges[] callJobAddressChanges =
                new CallJobUnsuitableAddressChanges[this.dataGridViewUnsuitableCallJobs.Rows.Count];
            
            foreach (DataGridViewRow row in this.dataGridViewUnsuitableCallJobs.Rows)
            {
                CallJobUnsuitableAddressChanges callJobAddressChange = new CallJobUnsuitableAddressChanges();

                DataRowView dataRow = this.bindingSourceUnsuitableCallJobs[row.Index] as DataRowView;
                callJobAddressChange.AdressenPoolNummer = 
                    (int)dataRow["AdressenPoolNummer"];
                callJobAddressChange.AdresseNichtGeeignet = 
                    (Boolean)dataRow["AdresseNichtGeeignet"];
                callJobAddressChange.ContactTypesParticipationUnsuitableId =
                    (Guid)dataRow["contactTypesParticipationUnsuitableId"];
                callJobAddressChanges[i++] = callJobAddressChange;
            }

            MetaCall.Business.CallJobInfoUnsuitable.UpdateCallJobsUnsuitableAddressChanges(
                callJobAddressChanges);
        }
    }
}
