using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using metatop.Applications.metaCall.DataObjects;
using metatop.Applications.metaCall.BusinessLayer;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace metatop.Applications.metaCall.WinForms.Modules
{
    [ToolboxItem(false)]
    public partial class ProjectViewBase : UserControl
    {
        private Project project;
        private DialMode lastDialMode = DialMode.Unseeded;
        private Dictionary<string, string> FilterHashTable = new Dictionary<string,string>();

        private bool isInWork;
        public bool IsInWork
        {
            get { return isInWork; }
        }

        /// <summary>
        /// Das aktuelle Projekt
        /// </summary>
        public Project CurrentProject
        {
            get
            {
                return this.project;
            }
        }

        /// <summary>
        /// Parameterloser Konstruktor für Designer
        /// </summary>
        public ProjectViewBase()
        {
            InitializeComponent();
        }
        
        /// <summary>
        /// Konstruktor zum initialisieren mit einem neuen Projekt
        /// </summary>
        /// <param name="calljobGroup"></param>
        public ProjectViewBase(Project project): this()
        {
            this.progressBarCallJobsLoad.Visible = false;
            
            //Datenbindung für removedTeams-GridView
            this.removedTeamsDataTable.Locale = CultureInfo.CurrentUICulture;
            this.teamBindingSource.DataSource = this.removedTeamsDataTable;
            this.callJobsDataTable.Locale = CultureInfo.CurrentUICulture;
            this.callJobBindingSource.DataSource = this.callJobsDataTable;
            this.projectDocumentsTable.Locale = CultureInfo.CurrentUICulture;
            this.projectDocumentsBindingSource.DataSource = this.projectDocumentsTable;

            //Initialisierung der lokalen Listen
            this.project = project;
            this.storedTeams = new List<TeamInfo>(this.project.Teams);
            this.currentTeams = new List<TeamInfo>(this.project.Teams);
            this.currentCallJobGroups = new List<CallJobGroup>(this.project.CallJobGroups);
            this.storedCallJobGroups = new List<CallJobGroup>(this.project.CallJobGroups);

            //Einrichten der Datentabellen und des GridViews
            SetupRemovedTeamsDataTable();
            SetupAvailableTeamsDataTable();
            SetupTeamAssignDataGridView();
            SetupRemovedCallJobGroupsDataTable();
            SetupAvailableCallJobGroupsDataTable();
            SetupCallJobsDataTable();
            SetupProjectDocumentsDataTable();

            BindDialModes();
            BindCenters();

            //Projektdaten anzeigen
            FillControl();

            //bisheriger DialMode des Projekts - wird beim Speichern des Projekts verwendet
            lastDialMode = project.DialMode;

            Application.Idle += new EventHandler(Application_Idle);
        }

        private void Application_Idle(object sender, EventArgs e)
        {
            UpdateUI();
        }

        private void UpdateUI()
        {
            this.Enabled = (!isInWork);

            if (this.isInWork)
            {
                return;
            }

            if (this.project == null)
            {
                return;
            }

            bool isEditable = this.project.State != ProjectState.Finished;
            
            this.dialModeComboBox.Enabled = isEditable;
            this.centerComboBox.Enabled = isEditable;
            this.iterationCounterNnumericUpDown.Enabled = isEditable;
            this.dialPrefixNumberTextBox.Enabled = isEditable;
            this.AdditiveInfoTextBox.Enabled = isEditable;
            this.addCallJobGroupButton.Enabled = isEditable;
            this.deleteCallJobGroupButton.Enabled = isEditable;
            this.AddTeamAssign.Enabled = isEditable;
            this.RemoveTeamAssign.Enabled = isEditable;
           // this.DateTimePickerReminderMax.Enabled = isEditable;
            this.buttonLastProjectCall.Enabled = isEditable;
        }

        #region CallJobGroups
        /// <summary>
        /// Liste mit CalljObGruppen die bereits auf dem Server gespeichert sind
        /// </summary>
        private List<CallJobGroup> storedCallJobGroups = new List<CallJobGroup>();

        /// <summary>
        /// Liste der aktuellen CallJobGruppen
        /// </summary>
        private List<CallJobGroup> currentCallJobGroups = new List<CallJobGroup>();

        /// <summary>
        /// DataTable mit CallJobGruppen, die auf dem Server gespeichert wurden aber 
        /// vom Benutzer entfernt
        /// </summary>
        private DataTable removedCallJobGroupsDatatable = new DataTable();

        /// <summary>
        /// DataTable mit CallJobGruppen, die für eine Neuordnung verfügbar sind.
        /// </summary>
        private DataTable availableCallJobgroups = new DataTable();

        private void FillCallJobGroupsListBox()
        {
            this.callJobGroupsListBox.Items.Clear();
            this.callJobGroupsListBox.DisplayMember = "DisplayNameCount";
            this.callJobGroupsListBox.FormattingEnabled = true;

            foreach (CallJobGroup callJobGroup in this.currentCallJobGroups)
            {
                this.callJobGroupsListBox.Items.Add(callJobGroup);                
            }
        }
        private void callJobGroupsListBox_Format(object sender, ListControlConvertEventArgs e)
        {
            CallJobGroup callJobGroup = e.ListItem as CallJobGroup;
            //Ein Sternchen an die Bezeichnung anfügen, wenn es sich um ein neues Team handelt
            if (callJobGroup != null)
            {
                //Manuelle Gruppen mit einem M kennzeichnen
                if (callJobGroup.Type == CallJobGroupType.ManualList)
                {
                    e.Value += " (M)";
                }

                //neue Gruppen mit einem Sternchen kennzeichnen
                if (!storedCallJobGroups.Exists(new Predicate<CallJobGroup>(delegate(CallJobGroup callJobGroupItem)
                    {
                        return callJobGroupItem.CallJobGroupId.Equals(callJobGroup.CallJobGroupId);
                    }
                    )))
                {
                    e.Value += " *";
                }
            }
        }

        private void SetupRemovedCallJobGroupsDataTable()
        {
            DataColumn col1 = new DataColumn("CallJobGroupId", typeof(Guid));
            col1.Caption = "CallJobGroupId";
            col1.ColumnMapping = MappingType.Hidden;

            DataColumn col2 = new DataColumn("CallJobGroupDisplayName", typeof(string));
            col2.Caption = "CallJobgroup";
            col2.ColumnMapping = MappingType.Element;

            DataColumn col3 = new DataColumn("AssignCallJobGroupId", typeof(string));
            col3.Caption = "AssignCallJobGroupId";
            col3.ColumnMapping = MappingType.Element;

            this.removedCallJobGroupsDatatable.Columns.AddRange(new DataColumn[] { col1, col2, col3});
        }

        private void SetupAvailableCallJobGroupsDataTable()
        {
            DataColumn col1 = new DataColumn("AssignCallJobGroupId", typeof(string));
            col1.Caption = "AssignCallJobGroupsId";
            col1.ColumnMapping = MappingType.Element;

            DataColumn col2 = new DataColumn("CallJobGroupDisplayName", typeof(string));
            col2.Caption = "CallJobGroup";
            col2.ColumnMapping = MappingType.Element;

            this.availableCallJobgroups.Columns.AddRange(new DataColumn[] { col1, col2 });
        }

        /// <summary>
        /// Fügt der DataTable RemovedTeams ein Team hinzu, wenn dieses 
        /// bereits auf dem Server gespeichert ist.
        /// </summary>
        /// <param name="teamInfo">Team, das entfernt werden soll</param>
        private void RemovedTeamsDataTableAdd(TeamInfo teamInfo)
        {
            //Prüfen, ob das Team bereits auf dem Server gespeichert ist.
            if (this.storedTeams.Exists(new Predicate<TeamInfo>(
                delegate(TeamInfo storedTeamInfo)
                {
                    return storedTeamInfo.TeamId.Equals(teamInfo.TeamId);
                })))
            {
                this.teamBindingSource.SuspendBinding();

                try
                {
                    Object[] objectData = new object[]{
                    teamInfo.TeamId,
                    teamInfo.Bezeichnung,
                    string.Empty,
                    teamInfo};
                    this.removedTeamsDataTable.Rows.Add(objectData);
                }
                finally
                {
                    this.teamBindingSource.ResumeBinding();
                }
            }
            //Einblenden des GridViews wenn benötigt
            if (this.removedTeamsDataTable.Rows.Count > 0)
            {
                this.teamAssignDataGridView.Visible = true;
            }
        }

        /// <summary>
        /// durchsucht die DataTable RemovedTeams nach dem angegebenen Team 
        /// und entfernt dieses aus der Tabelle
        /// </summary>
        /// <param name="teamInfo"></param>
        private void RemovedCallJobGroupsDataTableDelete(CallJobGroup callJobGroup)
        {
            foreach (DataRow row in this.removedTeamsDataTable.Rows)
            {
                if (((Guid)row[0]).Equals(callJobGroup.CallJobGroupId))
                {
                    this.teamBindingSource.SuspendBinding();
                    try
                    {
                        row.Delete();
                        break;
                    }
                    finally
                    {
                        this.teamBindingSource.ResumeBinding();
                    }
                }
            }

            //Ausblenden des GridViews wenn nicht benötigt
            if (this.removedTeamsDataTable.Rows.Count == 0)
            {
                this.teamAssignDataGridView.Visible = false;
            }
        }

        private void editCallJobGroupButton_Click(object sender, EventArgs e)
        {
            if (this.callJobGroupsListBox.SelectedIndex < 0)
            {
                return;
            }

            CallJobGroup callJobGroup = this.callJobGroupsListBox.SelectedItem as CallJobGroup;

            using (CallJobGroupEdit callJobGroupEdit = new CallJobGroupEdit(callJobGroup, this.currentTeams))
            {
                if (callJobGroupEdit.ShowDialog() == DialogResult.OK)
                {
                    FillCallJobGroupsListBox();
                }
            }
        }

        private void addCallJobGroupButton_Click(object sender, EventArgs e)
        {
            CallJobGroup callJobGroup = new CallJobGroup();
            callJobGroup.CallJobGroupId = Guid.NewGuid();
            callJobGroup.Type = CallJobGroupType.ManualList;
            callJobGroup.Ranking = this.callJobGroupsListBox.Items.Count == 0 ? 1 : this.callJobGroupsListBox.Items.Count + 1;
            callJobGroup.Key = string.Format("manualKey {0}", callJobGroup.Ranking);
            callJobGroup.Teams = new TeamInfo[0];
            callJobGroup.Users = new UserInfo[0];

            using (CallJobGroupEdit callJobGroupEdit = new CallJobGroupEdit(callJobGroup, this.currentTeams))
            {
                if (callJobGroupEdit.ShowDialog() == DialogResult.OK)
                {
                    this.currentCallJobGroups.Add(callJobGroup);
                    FillCallJobGroupsListBox();
                }
            }
        }

        private void deleteCallJobGroupButton_Click(object sender, EventArgs e)
        {
            CallJobGroup callJobGroup = this.callJobGroupsListBox.SelectedItem as CallJobGroup;

            if (callJobGroup != null)
            {
                if (callJobGroup.Type != CallJobGroupType.ManualList)
                {
                    MessageBox.Show(this, "Sie können nur manuelle Gruppen entfernen!", Application.ProductName,
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                this.currentCallJobGroups.Remove(callJobGroup);
                FillCallJobGroupsListBox();
            }
        }

        private void callJobGroupsListBox_DoubleClick(object sender, EventArgs e)
        {
            int index = this.callJobGroupsListBox.IndexFromPoint(this.callJobGroupsListBox.PointToClient(MousePosition));

            if (index > -1)
            {
                this.callJobGroupsListBox.SelectedIndex = index;
                this.editCallJobGroupButton_Click(sender, e);
            }
        }
        #endregion

        #region CallJobs
        private DataTable callJobsDataTable = new DataTable();

        private void SetupCallJobsDataTable()
        {
            this.callJobsDataTable.CaseSensitive = false;
            
            DataTableHelper.AddColumn(this.callJobsDataTable, "Sponsor", "Sponsor", typeof(string));
            DataTableHelper.AddColumn(this.callJobsDataTable, "Text1", "Zusatz", typeof(string));
            DataTableHelper.AddColumn(this.callJobsDataTable, "Strasse", "Strasse", typeof(string));
            DataTableHelper.AddColumn(this.callJobsDataTable, "Wohnort", "Wohnort", typeof(string));
            DataTableHelper.AddColumn(this.callJobsDataTable, "Telefonnummer","Telefon", typeof(string));
            DataTableHelper.AddColumn(this.callJobsDataTable, "Start", "Start", typeof(string));
            DataTableHelper.AddColumn(this.callJobsDataTable, "Stop", "Stop", typeof(string));
            DataTableHelper.AddColumn(this.callJobsDataTable, "Status", "Status", typeof(string));
            DataTableHelper.AddColumn(this.callJobsDataTable, "Kontaktart", "Kontaktart", typeof(string));
            DataTableHelper.AddColumn(this.callJobsDataTable, "IterationCounter", "Anrufzähler", typeof(int));
            DataTableHelper.AddColumn(this.callJobsDataTable, "DialMode", "Wählmodus", typeof(string));
            DataTableHelper.AddColumn(this.callJobsDataTable, "CallJobGroup", "Anrufgruppe", typeof(string));
            DataTableHelper.AddColumn(this.callJobsDataTable, "User", "Benutzer", typeof(string));
            DataTableHelper.AddColumn(this.callJobsDataTable, "LastOrderAgent", "Letzter Auftrag", typeof(string));
            DataTableHelper.AddColumn(this.callJobsDataTable, "LastContact","Letzter Kontakt",typeof(string));
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
            //Sponsor
            column = this.dataGridView1.Columns[0];
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            column.FillWeight = 400;

            //Zusatz
            column = this.dataGridView1.Columns[1];
            column.Visible = false;
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            column.FillWeight = 300;

            //Strasse
            column = this.dataGridView1.Columns[2];
            column.Visible = true;
            //column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            column.FillWeight = 300;

            //Wohnort
            column = this.dataGridView1.Columns[3];
            column.Visible = true; 
            //column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            column.FillWeight = 300;

            //Telefon
            column = this.dataGridView1.Columns[4];
            column.Visible = false;

            //Start
            column = this.dataGridView1.Columns[5];
            column.Visible = false;

            //Stop
            column = this.dataGridView1.Columns[6];
            column.Visible = false;

            //Status
            column = this.dataGridView1.Columns[7];
            column.Visible = true;

            //Kontaktart
            column = this.dataGridView1.Columns[8];
            column.Visible = true;

            //IterationCounter
            column = this.dataGridView1.Columns[9];
            column.Visible = false;

            //DialMode
            column = this.dataGridView1.Columns[10];
            column.Visible = false; 

            //Anrufgruppe
            column = this.dataGridView1.Columns[11];
            column.Visible = true;
            column.FillWeight = 200;

            //Benutzer
            column = this.dataGridView1.Columns[12];
            column.Visible = true;
            column.FillWeight = 200;

            //LastOrderAgent
            column = this.dataGridView1.Columns[13];
            column.Visible = true;

            //LastContactDate Other Project
            column = this.dataGridView1.Columns[14];
            column.Visible = true;

            //LastContactAgent
            column = this.dataGridView1.Columns[15];
            column.Visible = true;

            //Address Activ
            column = this.dataGridView1.Columns[16];
            column.Visible = false;

            //QuantityOrders
            column = this.dataGridView1.Columns[17];
            column.Visible = false;
            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            column.DefaultCellStyle.Format = "#,##0.0";
            column.Width = 50;

            //TotalAmountOrders
            column = this.dataGridView1.Columns[18];
            column.Visible = false;
            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            column.DefaultCellStyle.Format = "#,##0.00";
            column.Width = 55;

            //Sorter
            column = this.dataGridView1.Columns[19];
            column.Visible = false;

            //CDSource
            column = this.dataGridView1.Columns[20];
            column.Visible = false;

            foreach (DataGridViewColumn col in this.dataGridView1.Columns)
            {
                //col.MinimumWidth = col.GetPreferredWidth(DataGridViewAutoSizeColumnMode.DisplayedCells, true);
                col.MinimumWidth = 10;
            }
        }

        private Guid taskId = Guid.Empty;
        private void LoadCallJobs()
        {
            try
            {
                this.isInWork = true;
                //Progressbar initialisieren
                this.progressBarCallJobsLoad.Minimum = 0;
                this.progressBarCallJobsLoad.Maximum = 100;
                this.progressBarCallJobsLoad.Value = 0;
                this.progressBarCallJobsLoad.Visible = true;

                //DataTable leeren
                this.callJobsDataTable.Rows.Clear();
                // Wen ein Filter gesetzt ist wird dieser beim neuladen entfernt
                if (this.callJobBindingSource.Filter != null)
                {
                    this.callJobBindingSource.RemoveFilter();
                }

                //Arbeit!!!
                MetaCall.Business.CallJobsInfoExtended.GetCallJobsInfoExtendedProgressChanged +=
                    new CallJobInfoBusiness.GetCallJobInfoExtendedProgressChangedEventHandler(CallJobsInfoExtended_GetCallJobsInfoExtendedProgressChanged);

                MetaCall.Business.CallJobsInfoExtended.GetCallJobsInfoExtendedCompleted += 
                    new CallJobInfoBusiness.GetCallJobInfoExtendedCompletedEventHandler(CallJobsInfoExtended_GetCallJobsInfoExtendedCompleted);

                //CallJobs
                //MetaCall.Business.CallJobs.GetCallJobsProgressChanged += new GetCallJobsProgressChangedEventHandler(CallJobs_GetCallJobsProgressChanged);
                //MetaCall.Business.CallJobs.GetCallJobsCompleted += new GetCallJobsCompletedEventHandler(CallJobs_GetCallJobsCompleted);

                taskId = Guid.NewGuid();
                MetaCall.Business.CallJobsInfoExtended.GetCallJobsInfoExtendedByProjectAsync(this.project, taskId);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "UI Policy");
                this.isInWork = false;
            }
        }

        private void loadCallJobsButton_Click(object sender, EventArgs e)
        {
            LoadCallJobs();
        }

        private void CallJobsInfoExtended_GetCallJobsInfoExtendedCompleted(object sender,
            CallJobInfoBusiness.GetCallJobsInfoExtendedCompletedEventArgs e)
        {
            //Fehler in GetCallJobs abfangen
            if (e.Error != null)
            {
                ExceptionPolicy.HandleException(e.Error, "UI Policy");
                return;
            }

            //Eventhandler entfernen
            CallJobInfoBusiness callJobInfoBusiness = sender as CallJobInfoBusiness;
            callJobInfoBusiness.GetCallJobsInfoExtendedCompleted -= new CallJobInfoBusiness.GetCallJobInfoExtendedCompletedEventHandler(this.CallJobsInfoExtended_GetCallJobsInfoExtendedCompleted);
            callJobInfoBusiness.GetCallJobsInfoExtendedProgressChanged -= new CallJobInfoBusiness.GetCallJobInfoExtendedProgressChangedEventHandler(this.CallJobsInfoExtended_GetCallJobsInfoExtendedProgressChanged);

            //jetzt haben wir CallJobs :-)

            List<CallJobInfoExtended> callJobsInfoExtended = e.CallJobsInfoExt;

            if (this.callJobsDataTable.Rows.Count > 0)
            {
                this.callJobsDataTable.AcceptChanges();
                this.dataGridView1.Sort(this.dataGridView1.Columns[0], ListSortDirection.Ascending);
                this.dataGridView1.Columns[0].Selected = true;
            }

            this.progressBarCallJobsLoad.Visible = false;
            this.taskId = Guid.Empty;
            dataGridView1_SelectionChanged(null, null);
            this.isInWork = false;
        }
        
        private void CallJobs_GetCallJobsCompleted(object sender, GetCallJobsCompletedEventArgs e)
        {
            //Fehler in GetCallJobs abfangen
            if (e.Error != null)
            {
                ExceptionPolicy.HandleException(e.Error, "UI Policy");
                return;
            }

            //Eventhandler entfernen
            CallJobBusiness callJobBusiness = sender as CallJobBusiness;
            callJobBusiness.GetCallJobsCompleted -= new GetCallJobsCompletedEventHandler(this.CallJobs_GetCallJobsCompleted);
            callJobBusiness.GetCallJobsProgressChanged -= new GetCallJobsProgressChangedEventHandler(this.CallJobs_GetCallJobsProgressChanged);

            //jetzt haben wir CallJobs :-)
            List<SponsoringCallJob> callJobs = e.CallJobs;

            if (this.callJobsDataTable.Rows.Count > 0)
            {
                this.callJobsDataTable.AcceptChanges();
                this.dataGridView1.Sort(this.dataGridView1.Columns[0], ListSortDirection.Ascending);
                this.dataGridView1.Columns[0].Selected = true;
            }

            this.progressBarCallJobsLoad.Visible = false;
            this.taskId = Guid.Empty;
            dataGridView1_SelectionChanged(null, null);
            this.isInWork = false;
        }

        private void CallJobsInfoExtended_GetCallJobsInfoExtendedProgressChanged(ProgressChangedEventArgs e)
        {
            if (e is GetCallJobInfoExtendedProgressChangedEventArgs)
            {
                GetCallJobInfoExtendedProgressChangedEventArgs getCallJobInfoExtendedProgressChangedEventArgs =
                    e as GetCallJobInfoExtendedProgressChangedEventArgs;

                CallJobInfoExtended callJobInfoExt = getCallJobInfoExtendedProgressChangedEventArgs.CallJobInfoExt;

                //DataBinding unterbrechen
                this.callJobBindingSource.SuspendBinding();

                try
                {
                    /*CallJobStateInfo callJobStateInfo = MetaCall.Business.CallJobs.GetCallJobState(callJob.State);

                    CallJobResult callJobResult = MetaCall.Business.CallJobResults.GetLastCallJobResultsByCallJobId(callJob);

                    List<mwProjekt_SponsorOrderHistorie> sponsorHistorie = MetaCall.Business.mwProjekt_SponsorOrderHistorie.GetAllmwProjekt_SponsorOrderHistorieLastAgent(callJob.Sponsor.AdressenPoolNummer);

                    DateTime? lastContactDate = MetaCall.Business.CallJobs.GetLastAddressContact(callJob.Sponsor.AdressenPoolNummer, this.project.ProjectId);

                    string lastContact;

                    if (lastContactDate == null)
                        lastContact = string.Empty;
                    else
                        lastContact = string.Format("{0:d}", lastContactDate);

                    string lastOrderAgent;

                    if (sponsorHistorie == null || sponsorHistorie.Count < 1)
                        lastOrderAgent = string.Empty;
                    else
                        lastOrderAgent = sponsorHistorie[0].Agent;

                    string resultDisplayName;

                    if (callJobResult == null)
                        resultDisplayName = string.Empty;
                    else
                        resultDisplayName = callJobResult.ContactType.DisplayName;
                    */

                    CallJob callJob = null;

                    object[] objectData = new object[]{
                        callJobInfoExt.Sponsor, // callJob.Sponsor.DisplaySortName,
                        callJobInfoExt.Text1,
                        callJobInfoExt.Street, // callJob.Sponsor.Strasse,
                        callJobInfoExt.City, // callJob.Sponsor.DisplayResidence,
                        callJobInfoExt.Fon, // callJob.Sponsor.TelefonNummer,
                        callJobInfoExt.StartDate, // callJob.StartDate,
                        callJobInfoExt.StopDate, // callJob.StopDate,
                        callJobInfoExt.StateTerm, // callJobStateInfo.DisplayName,
                        callJobInfoExt.LastResultDisplayName, // resultDisplayName,
                        callJobInfoExt.IterationCounter, //callJob.IterationCounter,
                        callJobInfoExt.DialModeTerm, // callJob.DialMode,
                        callJobInfoExt.CallJobGroupTerm, // callJob.CallJobGroup == null ? null: callJob.CallJobGroup.DisplayName,
                        callJobInfoExt.UserTerm, // callJob.User == null ? null: callJob.User.DisplayName,
                        callJobInfoExt.LastOrderAgent, // lastOrderAgent,
                        callJobInfoExt.LastContact, // lastContact,
                        callJobInfoExt.LastContactAgent, //lastContactAgent,
                        callJobInfoExt.AddressSafeActiv, // callJob.AddressSafeActiv,
                        callJobInfoExt.QuantityOrders,
                        callJobInfoExt.TotalAmountOrders,
                        callJobInfoExt.CDSource,
                        callJobInfoExt.RandomSorter,
                        callJobInfoExt.CallJobId,
                        callJob}; // callJob

                    this.callJobsDataTable.Rows.Add(objectData);
                }

                finally
                {
                    this.callJobBindingSource.ResumeBinding();
                }

                dataGridView1_SelectionChanged(null, null);
                string msg = "Anruf {0} von {1} ({2}%)";
                progressBarCallJobsLoad.Text =
                        string.Format(msg,
                        getCallJobInfoExtendedProgressChangedEventArgs.Current,
                        getCallJobInfoExtendedProgressChangedEventArgs.TotalCount,
                        getCallJobInfoExtendedProgressChangedEventArgs.ProgressPercentage);

            }

            this.progressBarCallJobsLoad.Value = e.ProgressPercentage;
        }

        private void CallJobs_GetCallJobsProgressChanged(ProgressChangedEventArgs e)
        {
            if (e is GetCallJobsProgressChangedEventArgs)
            { 
                GetCallJobsProgressChangedEventArgs getCallJobProgressChangedEventArgs = e as GetCallJobsProgressChangedEventArgs;
                CallJob callJob = getCallJobProgressChangedEventArgs.CallJob;
                
                //DataBinding unterbrechen
                this.callJobBindingSource.SuspendBinding();

                try
                {
                    CallJobStateInfo callJobStateInfo = MetaCall.Business.CallJobs.GetCallJobState(callJob.State);
                    CallJobResult callJobResult = MetaCall.Business.CallJobResults.GetLastCallJobResultsByCallJobId(callJob);
                    List<mwProjekt_SponsorOrderHistorie> sponsorHistorie = MetaCall.Business.mwProjekt_SponsorOrderHistorie.GetAllmwProjekt_SponsorOrderHistorieLastAgent(callJob.Sponsor.AdressenPoolNummer);
                    DateTime? lastContactDate = MetaCall.Business.CallJobs.GetLastAddressContact(callJob.Sponsor.AdressenPoolNummer, this.project.ProjectId);

                    string lastContact;

                    if (lastContactDate == null)
                    {
                        lastContact = string.Empty;
                    }
                    else
                    {
                        lastContact = string.Format("{0:d}", lastContactDate);
                    }

                    string lastOrderAgent;

                    if (sponsorHistorie == null || sponsorHistorie.Count < 1)
                    {
                        lastOrderAgent = string.Empty;
                    }
                    else
                    {
                        lastOrderAgent = sponsorHistorie[0].Agent;
                    }
                    
                    string resultDisplayName;

                    if (callJobResult == null)
                    {
                        resultDisplayName = string.Empty;
                    }
                    else
                    {
                        resultDisplayName = callJobResult.ContactType.DisplayName;
                    }

                    object[] objectData = new object[]{
                        callJob.Sponsor.DisplaySortName,
                        callJob.Sponsor.Strasse,
                        callJob.Sponsor.DisplayResidence,
                        callJob.Sponsor.TelefonNummer,
                        callJob.StartDate,
                        callJob.StopDate,
                        callJobStateInfo.DisplayName,
                        resultDisplayName,
                        callJob.IterationCounter,
                        callJob.DialMode,
                        callJob.CallJobGroup == null ? null: callJob.CallJobGroup.DisplayName,
                        callJob.User == null ? null: callJob.User.DisplayName,
                        lastOrderAgent,
                        lastContact,
                        callJob.AddressSafeActiv,
                        callJob};

                    this.callJobsDataTable.Rows.Add(objectData);
                }

                finally
                {
                    this.callJobBindingSource.ResumeBinding();
                }

                dataGridView1_SelectionChanged(null, null);
                string msg = "Anruf {0} von {1} ({2}%)";
                progressBarCallJobsLoad.Text =
                        string.Format(msg, 
                        getCallJobProgressChangedEventArgs.Current,
                        getCallJobProgressChangedEventArgs.TotalCount,
                        getCallJobProgressChangedEventArgs.ProgressPercentage);

            }

            this.progressBarCallJobsLoad.Value = e.ProgressPercentage;
        }

        private void ApplyCallJobFilter()
        {
            StringBuilder filter = new StringBuilder();

            foreach (string key in this.FilterHashTable.Keys)
            {
                Type dataType = this.callJobsDataTable.Columns[key].DataType;
                if (filter.Length > 0) filter.Append(" AND ");
                
                if (dataType == typeof(string))
                {
                    filter.AppendFormat("{0} like '%{1}%'", key, this.FilterHashTable[key]);
                }
                else if (dataType.IsEnum)
                {
                    filter.AppendFormat("{0} = {1}", key,  Convert.ChangeType(Enum.Parse(dataType, this.FilterHashTable[key]), Enum.GetUnderlyingType(dataType)));
                }
                else if (dataType == typeof(int))
                {
                    try
                    {
                        filter.AppendFormat("{0} = {1}", key, Convert.ChangeType(this.FilterHashTable[key], dataType));
                    }
                    catch
                    {
                        ;
                    }
                }
            }

            if (filter.Length > 0)
            {
                this.callJobBindingSource.Filter = filter.ToString();
            }
            else
            {
                this.callJobBindingSource.RemoveFilter();
            }
        }

        /// <summary>
        /// bearbeitet den CallJob der momentan in callJobBindingSource gewählt ist
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            EditCallJob();
        }

        private void EditCallJob()
        {
            DataRowView rowView = this.callJobBindingSource.Current as DataRowView;

            if (rowView != null)
            {
                CallJob callJob;
                if (this.dataGridView1.SelectedRows.Count > 1)
                {
                    callJob = MetaCall.Business.CallJobs.GetNewSponsorinCallJob();
                }
                else
                {
                    callJob = rowView.Row["CallJob"] as CallJob;
                    if (callJob == null)
                    {
                        callJob = MetaCall.Business.CallJobs.Get((Guid)rowView.Row["CallJobId"]);
                        rowView.Row["CallJob"] = callJob;
                    }
                }

                List<UserInfo> users = new List<UserInfo>();
                foreach (TeamInfo teamInfo in this.currentTeams)
                {
                    Team team = MetaCall.Business.Teams.GetTeam(teamInfo);
                    foreach (UserInfo userInfo in team.TeamMitglieder)
                    {
                        users.Add(userInfo);
                    }
                }

                using (CallJobEdit callJobEditDlg = new CallJobEdit(callJob, this.currentCallJobGroups, users))
                {
                    if (callJobEditDlg.ShowDialog(this) == DialogResult.OK)
                    {
                        if (this.dataGridView1.SelectedRows.Count == 1)
                        {
                            rowView.BeginEdit();
                            rowView["Start"] = callJob.StartDate.ToShortDateString();
                            rowView["Stop"] = callJob.StopDate.ToShortDateString();
                            rowView["IterationCounter"] = callJob.IterationCounter;
                            rowView["DialMode"] = callJob.DialMode;
                            CallJobStateInfo callJobStateInfo = MetaCall.Business.CallJobs.GetCallJobState(callJob.State);
                            rowView["Status"] = callJobStateInfo.DisplayName;
                            rowView["CallJobGroup"] = callJob.CallJobGroup.DisplayName;

                            if (callJob.User.UserId == Guid.Empty)
                            {
                                callJob.User = null;
                            }

                            rowView["User"] = callJob.User == null ? null : callJob.User.DisplayName;
                            rowView.EndEdit();
                        }
                        else
                        {
                            foreach (DataGridViewRow row in this.dataGridView1.SelectedRows)
                            {
                                DataRowView rowViewSelected = (DataRowView)row.DataBoundItem;
                                CallJob callJobCurrent = rowViewSelected.Row["CallJob"] as CallJob;

                                if (callJobCurrent == null)
                                {
                                    callJobCurrent = MetaCall.Business.CallJobs.Get((Guid)rowViewSelected.Row["CallJobId"]);
                                    rowViewSelected.Row["CallJob"] = callJobCurrent;
                                    //row["CallJob"] = callJobCurrent;
                                }

                                rowViewSelected.BeginEdit();

                                if (callJob.StartDate.Date != DateTime.MinValue.Date)
                                {
                                    rowViewSelected["Start"] = callJob.StartDate.ToShortDateString();
                                    callJobCurrent.StartDate = callJob.StartDate;
                                }

                                if (callJob.StopDate.Date != DateTime.MinValue.Date)
                                {
                                    rowViewSelected["Stop"] = callJob.StopDate.ToShortDateString();
                                    callJobCurrent.StopDate = callJob.StopDate;
                                }

                                if (callJob.IterationCounter != Int16.MaxValue)
                                {
                                    rowViewSelected["IterationCounter"] = callJob.IterationCounter;
                                    callJobCurrent.IterationCounter = callJob.IterationCounter;
                                }

                                if (callJob.DialMode != DialMode.Unseeded)
                                {
                                    rowViewSelected["DialMode"] = callJob.DialMode;
                                    callJobCurrent.DialMode = callJob.DialMode;
                                }

                                if (callJob.State != CallJobState.Unseeded)
                                {
                                    CallJobStateInfo callJobStateInfo = MetaCall.Business.CallJobs.GetCallJobState(callJob.State);
                                    rowViewSelected["Status"] = callJobStateInfo.DisplayName;
                                    callJobCurrent.State = callJob.State;
                                }

                                if (callJob.CallJobGroup != null)
                                {
                                    rowViewSelected["CallJobGroup"] = callJob.CallJobGroup.DisplayName;
                                    callJobCurrent.CallJobGroup = callJob.CallJobGroup;
                                }

                                if (callJob.User != null)
                                {
                                    if (callJob.User.UserId == Guid.Empty)
                                    {
                                        rowViewSelected["User"] = null;
                                        callJobCurrent.User = null;
                                    }
                                    else
                                    {
                                        rowViewSelected["User"] = callJob.User.DisplayName;
                                        callJobCurrent.User = callJob.User;
                                    }
                                }

                                rowViewSelected.EndEdit();
                            }
                        }
                    }
                }
            }
        }

        private void ClearFilterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.FilterHashTable.Clear();
            ApplyCallJobFilter();
        }

        private ToolStripItem GetFilterToolStripItem(DataGridViewColumn column)
        {
            if ((column.ValueType.IsEnum) ||
                (column.DataPropertyName == "CallJobGroup") ||
                (column.DataPropertyName == "Status") ||
                (column.DataPropertyName == "Kontaktart") ||
                (column.DataPropertyName == "User"))
            {
                ToolStripComboBox filterComboBox = new ToolStripComboBox("filterComboBox");
                filterComboBox.Tag = column;
                filterComboBox.DropDownClosed += new EventHandler(filterComboBox_DropDownClosed);
                filterComboBox.LostFocus += new EventHandler(filterComboBox_LostFocus);

                //Calljob-States
                if (column.DataPropertyName == "Status")
                {
                    foreach (CallJobStateInfo callJobStateinfo in MetaCall.Business.CallJobs.CallJobStates)
                    {
                        filterComboBox.Items.Add(callJobStateinfo.DisplayName);
                    }
                }

                //Kontaktarten
                if (column.DataPropertyName == "Kontaktart")
                {
                    foreach (ContactType contactTypes in MetaCall.Business.ContactType.ContactTypesSponsoringCallJob)
                    {
                        filterComboBox.Items.Add(contactTypes.DisplayName);
                    }
                }

                //DialModes
                if (column.ValueType == typeof(DialMode))
                {
                    filterComboBox.Items.AddRange(Enum.GetNames(column.ValueType));
                }

                //CallJobGroups
                if (column.DataPropertyName == "CallJobGroup")
                {
                    foreach (CallJobGroup callJobGroup in this.currentCallJobGroups)
                    {
                        filterComboBox.Items.Add(callJobGroup.DisplayName);
                    }
                }

                if (column.DataPropertyName == "User")
                {
                    filterComboBox.Sorted = true; 
                    foreach (TeamInfo teamInfo in this.currentTeams)
                    {
                        Team team = MetaCall.Business.Teams.GetTeam(teamInfo);
                        foreach(TeamMitglied teamMitglied in team.TeamMitglieder)
                        {
                            filterComboBox.Items.Add(teamMitglied.DisplayName);
                        }
                    }
                }

                //Vorbelegen eines bereits eingegebenen Filters
                if (this.FilterHashTable.ContainsKey(column.DataPropertyName) && this.FilterHashTable[column.DataPropertyName] != null)
                {
                    filterComboBox.SelectedItem = this.FilterHashTable[column.DataPropertyName];
                }
                else
                {
                    filterComboBox.Text = "<bitte wählen>";
                }

                filterComboBox.Size = new Size(200,20);

                return filterComboBox;
            }
            else
            {
                ToolStripTextBox filterTextBox = new ToolStripTextBox("filterTextBox");
                filterTextBox.Tag = column;
                filterTextBox.KeyDown += new KeyEventHandler(filterTextBox_KeyDown);

                //Vorbelegen eines bereits eingegebenen Filters
                if (this.FilterHashTable.ContainsKey(column.DataPropertyName) && this.FilterHashTable[column.DataPropertyName] != null)
                {
                    filterTextBox.Text = this.FilterHashTable[column.DataPropertyName];
                }
                else
                {
                    filterTextBox.Text = "<bitte wählen>";
                    filterTextBox.ForeColor = Color.Silver;
                }

                filterTextBox.GotFocus += new EventHandler(filterTextBox_GotFocus);

                return filterTextBox;
            }
        }

        private void filterComboBox_LostFocus(object sender, EventArgs e)
        {
            ToolStripComboBox filterComboBox = sender as ToolStripComboBox;

            if (filterComboBox == null)
            {
                return;
            }

            DataGridViewColumn column = filterComboBox.Tag as DataGridViewColumn;

            if (filterComboBox.SelectedItem != null)
            {
                if (!this.FilterHashTable.ContainsKey(column.DataPropertyName))
                {
                    this.FilterHashTable.Add(column.DataPropertyName, null);
                }

                this.FilterHashTable[column.DataPropertyName] = (string)filterComboBox.SelectedItem;
            }
            else
            {
                if (this.FilterHashTable.ContainsKey(column.DataPropertyName))
                {
                    this.FilterHashTable.Remove(column.DataPropertyName);
                }
            }

            ApplyCallJobFilter();
            this.callJobsContextMenuStrip.Close();
        }

        private void filterComboBox_GotFocus(object sender, EventArgs e)
        {
            ToolStripComboBox filterComboBox = sender as ToolStripComboBox;

            if (filterComboBox == null)
            {
                return;
            }

            filterComboBox.DroppedDown = true;
        }

        private void filterTextBox_GotFocus(object sender, EventArgs e)
        {
            ToolStripTextBox filterTextBox = sender as ToolStripTextBox;

            if (filterTextBox != null)
            {
                if (filterTextBox.ForeColor == Color.Silver)
                {
                    filterTextBox.Text = null;
                    filterTextBox.ForeColor = Control.DefaultForeColor;
                }

                filterTextBox.GotFocus -= new EventHandler(this.filterTextBox_GotFocus);
            }
        }

        private void filterComboBox_DropDownClosed(object sender, EventArgs e)
        {
            ToolStripComboBox filterComboBox = sender as ToolStripComboBox;

            if (filterComboBox == null)
            {
                return;
            }

            this.callJobsContextMenuStrip.Close(ToolStripDropDownCloseReason.ItemClicked);
        }

        private void filterTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Enter) || (e.KeyCode == Keys.Tab))
            {
                ToolStripTextBox filterTextBox = sender as ToolStripTextBox;
                if (filterTextBox == null)
                {
                    return;
                }

                DataGridViewColumn column = filterTextBox.Tag as DataGridViewColumn;

                if ((filterTextBox.Text != null) && (filterTextBox.Text.Length != 0))
                {
                    if (!this.FilterHashTable.ContainsKey(column.DataPropertyName))
                    {
                        this.FilterHashTable.Add(column.DataPropertyName, null);
                    }

                    this.FilterHashTable[column.DataPropertyName] = filterTextBox.Text;
                }
                else
                {
                    if (this.FilterHashTable.ContainsKey(column.DataPropertyName))
                    {
                        this.FilterHashTable.Remove(column.DataPropertyName);
                    }
                }

                ApplyCallJobFilter();

                this.callJobsContextMenuStrip.Close(ToolStripDropDownCloseReason.ItemClicked);
            }
        }

        private void callJobsContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            Point mouse = this.dataGridView1.PointToClient(MousePosition);
            DataGridView.HitTestInfo hitTestInfo = this.dataGridView1.HitTest(mouse.X, mouse.Y);
            bool isEditable = (this.project.State != ProjectState.Finished);

            //Zusammenstellen des FilterKontextMenues
            ToolStripMenuItem filterItem = new ToolStripMenuItem("Filter");
            filterItem.AutoSize = true;
            if (hitTestInfo.ColumnIndex > -1)
            {
                if (hitTestInfo.RowIndex > -1)
                {
                    //this.dataGridView1.CurrentCell = this.dataGridView1.Rows[hitTestInfo.RowIndex].Cells[hitTestInfo.ColumnIndex];
                }
                DataGridViewColumn column = this.dataGridView1.Columns[hitTestInfo.ColumnIndex];
                filterItem.DropDownItems.Add(new ToolStripLabel(string.Format("{0} filtern:", column.HeaderText)));
                filterItem.DropDownItems.Add(GetFilterToolStripItem(column));
            }

            if (this.callJobBindingSource.Filter != null)
            {
                if (filterItem.DropDownItems.Count > 0)
                {
                    filterItem.DropDownItems.Add("-");
                }

                ToolStripMenuItem clearButton = new ToolStripMenuItem("Filter löschen", null, this.ClearFilterToolStripMenuItem_Click, "clearFilter");
                clearButton.AutoSize = true;
                filterItem.DropDownItems.Add(clearButton);
            }

            //Zusammenstellen des Spaltenauswahl
            ToolStripMenuItem columnSelection = new ToolStripMenuItem("Spalten");
            columnSelection.AutoSize = true;

            foreach (DataGridViewColumn column in this.dataGridView1.Columns)
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

            //Zusammenstellen der CallJobGruppen
            ToolStripMenuItem callJobGroupSelection = new ToolStripMenuItem("Anrufgruppen");
            callJobGroupSelection.AutoSize = true;

            foreach (CallJobGroup callJobGroup in this.currentCallJobGroups)
            {
                ToolStripMenuItem callJobGroupMenuItem = new ToolStripMenuItem(callJobGroup.DisplayName);
                callJobGroupMenuItem.Tag = callJobGroup;
                callJobGroupMenuItem.CheckOnClick = false;
                callJobGroupMenuItem.Click += new EventHandler(callJobGroupMenuItem_Click);

                callJobGroupSelection.DropDownItems.Add(callJobGroupMenuItem);
            }

            ToolStripMenuItem userSelection = new ToolStripMenuItem("Benutzer");
            userSelection.AutoSize = true;

            ToolStripMenuItem userMenuItem = new ToolStripMenuItem("<keine Benutzerzuordnung>");
            userMenuItem.Tag = null;
            userMenuItem.CheckOnClick = false;
            userMenuItem.Click += new EventHandler(userMenuItem_Click);

            userSelection.DropDownItems.Add(userMenuItem);

            foreach (TeamInfo teamInfo in this.currentTeams)
            {
                Team team = MetaCall.Business.Teams.GetTeam(teamInfo);

                foreach (TeamMitglied user in team.TeamMitglieder)
                {
                    userMenuItem = new ToolStripMenuItem(user.DisplayName);
                    userMenuItem.Tag = user;
                    userMenuItem.CheckOnClick = false;
                    userMenuItem.Click += new EventHandler(userMenuItem_Click);

                    userSelection.DropDownItems.Add(userMenuItem);
                }
            }

            ToolStripMenuItem editCallJob;

            if (isEditable)
            {
                if (this.dataGridView1.SelectedRows.Count > 1)
                {
                    editCallJob = new ToolStripMenuItem("Auswahl bearbeiten");
                }
                else
                {
                    editCallJob = new ToolStripMenuItem("Bearbeiten");
                }
            }
            else
            {
                if (this.dataGridView1.SelectedRows.Count == 1)
                {
                    editCallJob = new ToolStripMenuItem("Anzeigen");
                }
                else
                {
                    editCallJob = null;
                }
            }

            if (editCallJob != null)
            {
                editCallJob.Click += new EventHandler(editCallJob_Click);
                editCallJob.Enabled = this.project.State != ProjectState.Finished;
            }

            ToolStripMenuItem addressSafeActiv = new ToolStripMenuItem("Adressenschutz");

            ToolStripMenuItem aktivieren = new ToolStripMenuItem("Aktivieren");
            aktivieren.Click += new EventHandler(Adressenschutz_aktivieren_Click);
            addressSafeActiv.DropDownItems.Add(aktivieren);

            ToolStripMenuItem deaktivieren = new ToolStripMenuItem("Deaktivieren");
            deaktivieren.Click += new EventHandler(Adressenschutz_deaktivieren_Click);
            addressSafeActiv.DropDownItems.Add(deaktivieren);
            this.callJobsContextMenuStrip.Items.Clear();

            ToolStripMenuItem selectedAll = new ToolStripMenuItem("Alle auswählen");
            selectedAll.Click += new EventHandler(alleAuswählenToolStripMenuItem_Click);
            this.callJobsContextMenuStrip.Items.Add(selectedAll);

            ToolStripMenuItem selectedNothing = new ToolStripMenuItem("Auswahl aufheben");
            selectedNothing.Click += new EventHandler(auswahlAufhebenToolStripMenuItem_Click);
            this.callJobsContextMenuStrip.Items.Add(selectedNothing);
            this.callJobsContextMenuStrip.Items.Add(addressSafeActiv);

            //Bearbeiten
            if (this.callJobBindingSource.Current != null)
            {
                this.callJobsContextMenuStrip.Items.Insert(0, editCallJob);
            }

            //CallJobGroups
            if (callJobGroupSelection.DropDownItems.Count > 0 && this.dataGridView1.SelectedRows.Count > 0)
            {
                this.callJobsContextMenuStrip.Items.Add(callJobGroupSelection);
            }

            if (userSelection.DropDownItems.Count > 0 && this.dataGridView1.SelectedRows.Count > 0)
            {
                this.callJobsContextMenuStrip.Items.Add(userSelection);
            }

            //Filter
            if (filterItem.DropDownItems.Count > 0)
            {
                this.callJobsContextMenuStrip.Items.Add(filterItem);
            }

            //Spaltenauswahl
            if (columnSelection.DropDownItems.Count > 0)
            {
                this.callJobsContextMenuStrip.Items.Add(columnSelection);
            }

            this.callJobsContextMenuStrip.AutoClose = true;
            this.callJobsContextMenuStrip.ShowCheckMargin = false;
            this.callJobsContextMenuStrip.ShowImageMargin = false;
            this.callJobsContextMenuStrip.ShowItemToolTips = true;

            e.Cancel = (this.callJobsContextMenuStrip.Items.Count < 1);
        }

        void Adressenschutz_aktivieren_Click(object sender, EventArgs e)
        {
            string msg;
            msg = "Möchten Sie für die gewählten Anrufaufträge den Anrufschutz aktivieren?";

            MessageBoxOptions options = CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft ? MessageBoxOptions.RtlReading : 0;
            if (MessageBox.Show(this, msg, Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button1, options) == DialogResult.Yes)
            {
                foreach (DataGridViewRow row in this.dataGridView1.SelectedRows)
                {
                    DataRowView dataRow = this.callJobBindingSource[row.Index] as DataRowView;

                    dataRow.BeginEdit();
                    CallJob callJob = dataRow["CallJob"] as CallJob;

                    if (callJob == null)
                    {
                        callJob = MetaCall.Business.CallJobs.Get((Guid)dataRow["CallJobId"]);
                        dataRow["CallJob"] = callJob;
                    }

                    callJob.AddressSafeActiv = true;
                    dataRow["AddressSafeActiv"] = true;
                    dataRow.EndEdit();
                }
            }
        }

        void Adressenschutz_deaktivieren_Click(object sender, EventArgs e)
        {
            string msg;
            msg = "Möchten Sie für die gewählten Anrufaufträge den Anrufschutz deaktivieren?";

            MessageBoxOptions options = CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft ? MessageBoxOptions.RtlReading : 0;
            if (MessageBox.Show(this, msg, Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button1, options) == DialogResult.Yes)
            {
                foreach (DataGridViewRow row in this.dataGridView1.SelectedRows)
                {
                    DataRowView dataRow = this.callJobBindingSource[row.Index] as DataRowView;

                    dataRow.BeginEdit();
                    CallJob callJob = dataRow["CallJob"] as CallJob;

                    if (callJob == null)
                    {
                        callJob = MetaCall.Business.CallJobs.Get((Guid)dataRow["CallJobId"]);
                        dataRow["CallJob"] = callJob;
                    }

                    callJob.AddressSafeActiv = false;
                    dataRow["AddressSafeActiv"] = false;
                    dataRow.EndEdit();
                }
            }
        }

        void userMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem userMenuItem = sender as ToolStripMenuItem;

            if (userMenuItem == null)
            {
                return;
            }

            UserInfo user = userMenuItem.Tag as UserInfo;
            string msg = string.Empty;

            if (user == null)
            {
                msg = "Möchten Sie die Benutzerzuordnung für die gewählten Anrufaufträge aufheben?";
            }
            else
            {
                msg = string.Format("Möchten Sie den gewählten Aunrufaufträgen den Benutzer {0} zuordnen?", user.DisplayName);
            }

            MessageBoxOptions options = CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft ? MessageBoxOptions.RtlReading : 0;
            if (MessageBox.Show(this, msg, Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button1, options) == DialogResult.Yes)
            {
                foreach (DataGridViewRow row in this.dataGridView1.SelectedRows)
                {
                    DataRowView dataRow = this.callJobBindingSource[row.Index] as DataRowView;

                    dataRow.BeginEdit();
                    CallJob callJob = dataRow["CallJob"] as CallJob;

                    if (callJob == null)
                    {
                        callJob = MetaCall.Business.CallJobs.Get((Guid)dataRow["CallJobId"]);
                        dataRow["CallJob"] = callJob;
                    }

                    callJob.User = user;
                    dataRow["user"] = callJob.User == null ? null:  callJob.User.DisplayName;
                    dataRow.EndEdit();
                }
            }
        }

        private void callJobGroupMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem callJobGroupMenuItem = sender as ToolStripMenuItem;

            if (callJobGroupMenuItem == null)
            {
                return;
            }

            CallJobGroup callJobGroup = callJobGroupMenuItem.Tag as CallJobGroup;
            
            string msg = "Möchten Sie die gewählten Anrufe der Anrufgruppe {0} zuordnen?";
            MessageBoxOptions options = CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft ? MessageBoxOptions.RtlReading : 0;

            if (MessageBox.Show(this, string.Format(msg, callJobGroup.DisplayName), Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button1, options) == DialogResult.Yes)
            {
                foreach (DataGridViewRow row in this.dataGridView1.SelectedRows)
                {
                    DataRowView dataRow = this.callJobBindingSource[row.Index] as DataRowView;

                    dataRow.BeginEdit();
                    CallJob callJob = dataRow["CallJob"] as CallJob;

                    if (callJob == null)
                    {
                        callJob = MetaCall.Business.CallJobs.Get((Guid)dataRow["CallJobId"]);
                        dataRow["CallJob"] = callJob;
                    }

                    callJob.CallJobGroup = MetaCall.Business.CallJobGroups.GetCallJobGroupInfo(callJobGroup.CallJobGroupId);
                    dataRow["CallJobGroup"] = callJob.CallJobGroup.DisplayName;
                    dataRow.EndEdit();
                }
            }
        }

        private void editCallJob_Click(object sender, EventArgs e)
        {
            if (this.callJobBindingSource.Current != null)
            {
                EditCallJob();
            }
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
        
        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            switch (e.ColumnIndex)
            {
                case 0:
                case 1:
                case 2:
                    break;

                case 3:
                case 4:
                    break;

                case 5:
                    DateTime value = DateTime.Parse((string)e.Value);

                    if (value.Date == DateTime.MinValue.Date)
                    {
                        e.Value = "N/A";
                    }
                    else if (value.Date == DateTime.MaxValue.Date)
                    {
                        e.Value = "unendlich";
                    }
                    else
                    {
                        e.Value = value.ToShortDateString();
                    }

                    break;

                case 6:
                    //CallJobState callJobState = (CallJobState) Enum.ToObject(typeof(CallJobState), (int)e.Value);
                    //CallJobStateInfo info = MetaCall.Business.CallJobs.GetCallJobState(callJobState);
                    //e.Value = info.DisplayName;
                    break;

                case 7:

                    break;

                case 8:
                    break;

                case 9:
                    break;

                default:
                    break;
            }
        }

        #endregion

        #region TeamHandling
        /// <summary>
        /// Liste mit Teams die bereits auf dem Server gespeichert sind
        /// </summary>
        private List<TeamInfo> storedTeams = new List<TeamInfo>();
        /// <summary>
        /// Liste der aktuell zugeordneten ProjektTeams
        /// </summary>
        private List<TeamInfo> currentTeams = new List<TeamInfo>();
        /// <summary>
        /// Verwaltet die Liste der Teams, die zwar auf dem Server gespeichert sind,
        /// vom Benutzer aber entfernt wurden
        /// </summary>
        private DataTable removedTeamsDataTable = new DataTable();
        /// <summary>
        /// verwaltet die Liste der Teams, die für die zuordnung zu entfernten Teams 
        /// verfügbar sind. Zusätzlich ist ein Eintrag "<nicht zugeordnet>" enthalten
        /// </summary>
        private DataTable availableTeamsDataTable = new DataTable();


        private void SetupRemovedTeamsDataTable()
        {
            DataColumn col1 = new DataColumn("TeamId", typeof(Guid));
            col1.Caption = "TeamId";
            col1.ColumnMapping = MappingType.Hidden;

            DataColumn col2 = new DataColumn("TeamDescription", typeof(string));
            col2.Caption = "Team";
            col2.ColumnMapping = MappingType.Element;

            DataColumn col3 = new DataColumn("AssignTeamId", typeof(string));
            col3.Caption = "AssignTeam";
            col3.ColumnMapping = MappingType.Element;

            DataColumn col4 = new DataColumn("RemovedTeam", typeof(TeamInfo));
            col4.Caption = string.Empty;
            col4.ColumnMapping = MappingType.Hidden;

            this.removedTeamsDataTable.Columns.AddRange(
                new DataColumn[] { col1, col2, col3, col4 });
        }

        private void SetupAvailableTeamsDataTable()
        {
            DataColumn col1 = new DataColumn("AssignTeamId", typeof(string));
            col1.Caption = "AssignTeamId";
            col1.ColumnMapping = MappingType.Element;

            DataColumn col2 = new DataColumn("TeamDescription", typeof(string));
            col2.Caption = "Team";
            col2.ColumnMapping = MappingType.Element;

            /*DataColumn col3 = new DataColumn("Team", typeof(Team));
             col3.Caption = string.Empty;
             col3.ColumnMapping = MappingType.Hidden;
              */
            this.availableTeamsDataTable.Columns.AddRange(new DataColumn[] { col1, col2 });
        }

        /// <summary>
        /// Fügt der DataTable RemovedTeams ein Team hinzu, wenn dieses 
        /// bereits auf dem Server gespeichert ist.
        /// </summary>
        /// <param name="teamInfo">Team, das entfernt werden soll</param>
        private void RemovedAvailableTeamsDataTableAdd(TeamInfo teamInfo)
        {
            //Prüfen, ob das Team bereits auf dem Server gespeichert ist.
            if (this.storedTeams.Exists(new Predicate<TeamInfo>(
                delegate(TeamInfo storedTeamInfo)
                {
                    return storedTeamInfo.TeamId.Equals(teamInfo.TeamId);
                })))
            {

                this.teamBindingSource.SuspendBinding();

                try
                {
                    Object[] objectData = new object[]{
                    teamInfo.TeamId,
                    teamInfo.Bezeichnung,
                    string.Empty,
                    teamInfo};

                    this.removedTeamsDataTable.Rows.Add(objectData);
                }
                finally
                {
                    this.teamBindingSource.ResumeBinding();
                }
            }
            //Einblenden des GridViews wenn benötigt
            if (this.removedTeamsDataTable.Rows.Count > 0)
            {
                this.teamAssignDataGridView.Visible = true;
            }

        }

        /// <summary>
        /// durchsucht die DataTable RemovedTeams nach dem angegebenen Team 
        /// und entfernt dieses aus der Tabelle
        /// </summary>
        /// <param name="teamInfo"></param>
        private void RemovedDataTableDelete(TeamInfo teamInfo)
        {
            foreach (DataRow row in this.removedTeamsDataTable.Rows)
            {
                if (((Guid)row[0]).Equals(teamInfo.TeamId))
                {
                    this.teamBindingSource.SuspendBinding();
                    try
                    {
                        row.Delete();
                        break;
                    }
                    finally
                    {
                        this.teamBindingSource.ResumeBinding();
                    }
                }
            }
            //Ausblenden des GridViews wenn nicht benötigt
            if (this.removedTeamsDataTable.Rows.Count == 0)
            {
                this.teamAssignDataGridView.Visible = false;
            }
        }
        
        private void FillTeamsListBox()
        {
            this.teamsListBox.Items.Clear();
            this.teamsListBox.DisplayMember = "Bezeichnung";
            this.teamsListBox.FormattingEnabled = true;


            this.availableTeamsDataTable.Clear();
            this.availableTeamsDataTable.Rows.Add(new object[] { string.Empty, "<nicht zugeordnet>" });


            foreach (TeamInfo teamInfo in this.currentTeams)
            {
                //Hinzufügen der Daten zur ListBox
                this.teamsListBox.Items.Add(teamInfo);

                //Hinzufügen der Daten zu den verfügbaren Teams
                this.availableTeamsDataTable.Rows.Add(new object[] { teamInfo.TeamId.ToString(), teamInfo.Bezeichnung });

            }
        }

        private void teamsListBox_Format(object sender, ListControlConvertEventArgs e)
        {
            TeamInfo teamInfo = e.ListItem as TeamInfo;
            //Ein Sternchen an die Bezeichnung anfügen, wenn es sich um ein neues Team handelt
            if (teamInfo != null)
            {
                if (!storedTeams.Exists(new Predicate<TeamInfo>(delegate(TeamInfo teamInfoItem)
                    {
                        return teamInfoItem.TeamId.Equals(teamInfo.TeamId);
                    }
                    )))
                {

                    e.Value += " *";
                }
            }
        }

        private void AddTeamAssign_Click(object sender, EventArgs e)
        {
            using (ProjectTeamAssign teamAssignDlg = new ProjectTeamAssign(this.project, this.currentTeams))
            {
                if (teamAssignDlg.ShowDialog(this) == DialogResult.OK)
                {
                    foreach (TeamInfo team in teamAssignDlg.SelectedTeams)
                    {
                        this.currentTeams.Add(team);

                        RemovedDataTableDelete(team);
                    }
                    FillTeamsListBox();
                }
            }
        }

        private void RemoveTeamAssign_Click(object sender, EventArgs e)
        {
            TeamInfo team = this.teamsListBox.SelectedItem as TeamInfo;

            if (team != null)
            {
                //Hinzufügen des Teams zu den entfernten Teams
                //this.removedTeams.Add(team);
                this.currentTeams.Remove(team);

                RemovedTeamsDataTableAdd(team);

                foreach (DataRow row in this.removedTeamsDataTable.Rows)
                {
                    if (row[2].Equals(team.TeamId.ToString()))
                    {
                        row.BeginEdit();
                        row[2] = string.Empty;
                        row.EndEdit();
                    }
                }
                FillTeamsListBox();
            }
        }

        private void SetupTeamAssignDataGridView()
        {
            DataGridViewTextBoxColumn column1 = new DataGridViewTextBoxColumn();
            column1.HeaderText = "entferntes Team";
            column1.ReadOnly = true;
            column1.ValueType = typeof(string);
            column1.DataPropertyName = "TeamDescription";


            DataGridViewComboBoxColumn column2 = new DataGridViewComboBoxColumn();
            column2.HeaderText = "neue Teamzuordnung";
            column2.DataPropertyName = "AssignTeamId";
            column2.DataSource = this.availableTeamsDataTable;
            column2.ValueMember = "AssignTeamId";
            column2.ValueType = typeof(string);
            column2.DisplayMember = "TeamDescription";
            column2.ReadOnly = false;

            //Alle Spalten löschen, damit aus der Entwurfszeit nicht mehr angezeigt wird
            this.teamAssignDataGridView.Columns.Clear();

            this.teamAssignDataGridView.Columns.Add(column1);
            this.teamAssignDataGridView.Columns.Add(column2);
        }

        private void teamAssignDataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        private void teamAssignDataGridView_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            ;
        }

        private void teamAssignDataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            Console.WriteLine("{0};{1}", e.ColumnIndex, e.RowIndex);
        }

        private void teamBindingSource_DataMemberChanged(object sender, EventArgs e)
        {
            Console.WriteLine("teamBindingSource_DataMemberChanged");
        }

        private void teamBindingSource_CurrentItemChanged(object sender, EventArgs e)
        {
            Console.WriteLine("teamBindingSource_CurrentItemChanged");
            Console.WriteLine(teamBindingSource.Current);

            DataRowView rowView = teamBindingSource.Current as DataRowView;


        }

        private void teamAssignDataGridView_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (this.teamAssignDataGridView.IsCurrentCellDirty)
            {
                this.teamAssignDataGridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private Guid teamAssignDataGridOldValue = Guid.Empty;

        private void teamAssignDataGridView_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {

            if (string.IsNullOrEmpty((string)this.teamAssignDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value))
                this.teamAssignDataGridOldValue = Guid.Empty;
            else
                this.teamAssignDataGridOldValue = new Guid((string)this.teamAssignDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
        }

        private void teamAssignDataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            string newValue = this.teamAssignDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value as string;

            TeamInfo newTeam = this.currentTeams.Find(new Predicate<TeamInfo>(delegate(TeamInfo teamInfo)
            {
                if (string.IsNullOrEmpty(newValue))
                    return false;
                else
                    return teamInfo.TeamId.Equals(new Guid(newValue));
            }));

            TeamInfo removedTeam = ((DataRowView)this.teamBindingSource.Current).Row[3] as TeamInfo;

            //wurde kein neues Team zugewiesen, wird das gespeicherte Team wieder eingesetzt
            if (newTeam == null)
                newTeam = removedTeam;

            //Alle CallJobGruppen durchlaufen und prüfen, ob 
            // eine CallJobgruppe das "alte" Team beinhaltet
            foreach (CallJobGroup callJobGroup in this.currentCallJobGroups)
            {
                List<TeamInfo> teams = new List<TeamInfo>(callJobGroup.Teams);

                TeamInfo oldCallJobGroupTeam = teams.Find(
                    new Predicate<TeamInfo>(delegate(TeamInfo teamInfo)
                {
                    return teamInfo.TeamId.Equals(removedTeam.TeamId) ||
                        teamInfo.TeamId.Equals(this.teamAssignDataGridOldValue);
                }));

                //Wenn nicht dann weitermachen
                if (oldCallJobGroupTeam == null)
                    continue;

                //Ansonsten wird das alte Team entfernt 
                teams.Remove(oldCallJobGroupTeam);

                //und das neue Team hinzugefügt
                if (newTeam != null)
                {
                    teams.Add(newTeam);
                }



                callJobGroup.Teams = new TeamInfo[teams.Count];
                teams.CopyTo(callJobGroup.Teams);
            }
        }

        #endregion

        #region ProjectDocuments
        private DataTable projectDocumentsTable = new DataTable();

        private void SetupProjectDocumentsDataTable()
        {
            DataTableHelper.AddColumn(this.projectDocumentsTable, "DisplayName", "Dokument", typeof(string));
            DataTableHelper.AddColumn(this.projectDocumentsTable, "PacketSelect", "PW", typeof(bool));
            DataTableHelper.AddColumn(this.projectDocumentsTable, "Category", "Katgorie", typeof(DocumentCategory));
            DataTableHelper.AddColumn(this.projectDocumentsTable, "ProjectDocument", "", typeof(ProjectDocument), MappingType.Hidden);

            DataTableHelper.FillGridView(this.projectDocumentsTable, this.projectDocumentsDataGridView);

            DataGridViewColumn column = this.projectDocumentsDataGridView.Columns[0];
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            DataGridViewColumn columnPW = this.projectDocumentsDataGridView.Columns[1];
            columnPW.AutoSizeMode =  DataGridViewAutoSizeColumnMode.ColumnHeader;
        }
        
        private void FillProjectDocumentsDataTable()
        {
            this.projectDocumentsBindingSource.SuspendBinding();

            try
            {
                ProjectInfo projectInfo = MetaCall.Business.Projects.Get(this.project);

                List<ProjectDocument> documents = MetaCall.Business.ProjectDocuments.GetDocumentsByProject(projectInfo);

                this.projectDocumentsTable.Clear();

                foreach (ProjectDocument document in documents)
                {
                    object[] data = new object[]{
                        document.DisplayName, 
                        document.PacketSelect,
                        document.Category, 
                        document};

                    this.projectDocumentsTable.Rows.Add(data);
                }
            }
            finally
            {
                this.projectDocumentsBindingSource.ResumeBinding();
            }
        }
        
        private void NewProjectDocument()
        {
            ProjectDocument document = new ProjectDocument();
            document.DateCreated = DateTime.Now;

            using (ProjectDocumentEdit documentDlg = new ProjectDocumentEdit(document))
            {
                if (documentDlg.ShowDialog(this) == DialogResult.OK)
                {

                    ProjectInfo projectInfo = MetaCall.Business.Projects.Get(this.project);

                    document = MetaCall.Business.ProjectDocuments.Create(
                        projectInfo,
                        document.DisplayName,
                        document.Category,
                        document.Filename,
                        document.PacketSelect);

                    FillProjectDocumentsDataTable();
                }
            }
        }
        
        private void EditProjectDocument()
        {
            DataRow row = this.projectDocumentsTable.Rows[this.projectDocumentsBindingSource.Position];
            ProjectDocument document = row["ProjectDocument"] as ProjectDocument;

            using (ProjectDocumentEdit documentDlg = new ProjectDocumentEdit(document))
            {
                if (documentDlg.ShowDialog(this) == DialogResult.OK)
                {
                    MetaCall.Business.ProjectDocuments.Update(document);

                    row["DisplayName"] = document.DisplayName;
                    row["PacketSelect"] = document.PacketSelect;
                    row["Category"] = document.Category;
                    row["ProjectDocument"] = document;
                }
            }
        }
        
        private void RemoveProjectDocument()
        {
            DataRow row = this.projectDocumentsTable.Rows[this.projectDocumentsBindingSource.Position];
            ProjectDocument document = row["ProjectDocument"] as ProjectDocument;


            MessageBoxOptions options = CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft ? MessageBoxOptions.RtlReading : 0;

            string msg = "Möchten Sie das Dokument {0} wircklich löschen?";
            msg = string.Format(msg, document.DisplayName);

            if (MessageBox.Show(this, msg, Application.ProductName, MessageBoxButtons.YesNo,
                MessageBoxIcon.Question, MessageBoxDefaultButton.Button2, options) == DialogResult.Yes)
            {
                this.projectDocumentsBindingSource.SuspendBinding();
                try
                {
                    MetaCall.Business.ProjectDocuments.Delete(document);

                    this.projectDocumentsTable.Rows.Remove(row);
                }
                finally
                {
                    this.projectDocumentsBindingSource.ResumeBinding();
                }

            }
        }
        
        #endregion

        protected void FillControl()
        {
            this.descriptionLabel.Text = this.project.Bezeichnung;

            if (this.project.mwProject != null)
            {
                mwProject mwProject = this.project.mwProject;
                this.projectNumberTextBox.Text = mwProject.Projektnummer.ToString();
                this.projectMonthYearTextBox.Text = string.Format("{0:00}/{1:0000}", mwProject.ProjektMonat, mwProject.ProjektJahr);
            }

            if (this.project.Customer != null)
            {
                Customer customer = this.project.Customer;
                this.customerStreetTextBox.Text = customer.Strasse;
                this.customerCityTextBox.Text = customer.DisplayResidence;
                if (customer.ContactPerson != null)
                    this.customerContactPersonTextBox.Text = customer.ContactPerson.DisplayName;
                else
                    this.customerContactPersonTextBox.Text = string.Empty;
            }

            this.centerTextBox.Text = this.project.Center.Bezeichnung;
            this.centerComboBox.Text = this.project.Center.Bezeichnung;
            this.iterationCounterNnumericUpDown.Value = this.project.IterationCounter;
            DialModeDescription dialModeDescription = new DialModeDescription();
            string description = dialModeDescription.TranslateToDescription(this.project.DialMode);

            if (description == null)
            {
                this.dialModeComboBox.SelectedItem = dialModeDescription.TranslateToDescription(DialMode.AutoDialingImmediately);
            }
            else
            {
                this.dialModeComboBox.SelectedItem = dialModeDescription.TranslateToDescription(this.project.DialMode);
            }

            this.dialPrefixNumberTextBox.Text = this.project.DialingPrefixNumber;
            this.AdditiveInfoTextBox.Text = this.project.Venue;
            this.checkBoxAddressSafeActiv.Checked = this.project.AddressSafeActiv;

            if (this.project.ReminderDateMax != null)
            {
                DateTime dtReminder = (DateTime)this.project.ReminderDateMax;
                this.DateTimePickerReminderMax.Value = dtReminder;
                this.DateTimePickerReminderMax.Checked = true;
            }
            else
            {
                this.DateTimePickerReminderMax.Checked = false;
            }

            this.praefixMailAttachmentTextBox.Text = this.project.PraefixMailAttachment;
            this.bezeichnungRechnungTextbox.Text = this.project.BezeichnungRechnung;

            //this.DateTimePickerReminderMax.Value = (DateTime)this.project.ReminderDateMax;

            //Füllen der Teams-Listbox
            FillTeamsListBox();

            //Füllen der CallJobGroups
            FillCallJobGroupsListBox();

            //Füllen der Dokumente
            FillProjectDocumentsDataTable();
        }

        private void BindCenters()
        {
            this.centerComboBox.Items.Clear();
            this.centerComboBox.DisplayMember = "Bezeichnung";
            List<CenterInfo> centers = MetaCall.Business.Centers.Centers;

            foreach (var centerInfo in centers)
            {
                this.centerComboBox.Items.Add(centerInfo);
            }
        }

        private void BindDialModes()
        {
            this.dialModeComboBox.Items.Clear();
            //foreach (string dialMode in Enum.GetNames(typeof(DialMode)))
            DialModeDescription dialModeDescription = new DialModeDescription();

            foreach (DialMode dialMode in Enum.GetValues(typeof(DialMode)))
            {
                if (dialMode != DialMode.Unseeded)
                {
                    this.dialModeComboBox.Items.Add(dialModeDescription.TranslateToDescription(dialMode));
                }
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            this.dataGridView1.Height = this.Height - this.dataGridView1.Top - this.Margin.Horizontal;
            this.dataGridView1.Width = this.Width - this.dataGridView1.Left -  this.Margin.Vertical;

            this.progressBarCallJobsLoad.Width = this.dataGridView1.Width - 110 - this.textBoxCountCallJobs.Width - this.Margin.Vertical;
            this.tabControl1.Width = this.Width - this.tabControl1.Left - this.Margin.Vertical;
            this.textBoxCountCallJobs.Left = this.dataGridView1.Left + this.dataGridView1.Width - this.textBoxCountCallJobs.Width;
        }

        /// <summary>
        /// speichert das aktuelle Projekt
        /// </summary>
        public void Save()
        {
            this.project.Center = (CenterInfo)this.centerComboBox.SelectedItem;
            this.project.AddressSafeActiv = this.checkBoxAddressSafeActiv.Checked;
            this.project.IterationCounter = (int) this.iterationCounterNnumericUpDown.Value;
            //this.project.DialMode = (DialMode)Enum.Parse(typeof(DialMode), (string)this.dialModeComboBox.SelectedItem);

            DialModeDescription dialModeDescription = new DialModeDescription();

            this.project.DialMode = dialModeDescription.TranslateToDialMode((string)this.dialModeComboBox.SelectedItem);
            this.project.DialingPrefixNumber = string.IsNullOrEmpty(this.dialPrefixNumberTextBox.Text) ? null : this.dialPrefixNumberTextBox.Text;
            this.project.Venue = this.AdditiveInfoTextBox.Text;
            if (this.DateTimePickerReminderMax.Checked == true)
            {
                this.project.ReminderDateMax = this.DateTimePickerReminderMax.Value.Date;
            }
            else
            {
                this.project.ReminderDateMax = null;
            }

            this.project.PraefixMailAttachment = string.IsNullOrEmpty(this.praefixMailAttachmentTextBox.Text) ? "Sponsoringangebot" : this.praefixMailAttachmentTextBox.Text;
            this.project.BezeichnungRechnung = this.bezeichnungRechnungTextbox.Text;

            project.Teams = new TeamInfo[this.currentTeams.Count];

            this.currentTeams.CopyTo(project.Teams);
            this.project.CallJobGroups = new CallJobGroup[this.currentCallJobGroups.Count];
            this.currentCallJobGroups.CopyTo(project.CallJobGroups);
            
            //Projekt speichern
            try
            {
                MetaCall.Business.Projects.Update(project);

                // falls der DialMode des Projekts geändert wurde, werden alle dazugehörigen
                // CallJobs aktualisiert
                if (project.DialMode != lastDialMode)
                {
                    if (project != null)
                    {
                        MetaCall.Business.CallJobs.UpdateCallJobsDialModeByProject(project, project.DialMode);
                    }
                }

                if (this.callJobsDataTable.Rows.Count > 0)
                {
                    DataTable changedCallJobs = this.callJobsDataTable.GetChanges();
                    if (changedCallJobs != null)
                    {
                        foreach (DataRow row in changedCallJobs.Rows)
                        {
                            if (row.RowState == DataRowState.Modified)
                            {
                                CallJob callJob = row["CallJob"] as CallJob;
                                MetaCall.Business.CallJobs.UpdateCallJob(callJob);
                            }
                        }
                    }
                }
            }
            catch 
            {
                throw ;
            }
        }

        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (Control.ModifierKeys == Keys.None && !this.dataGridView1.Rows[e.RowIndex].Selected)
                {
                    this.dataGridView1.ClearSelection();
                    this.dataGridView1.Rows[e.RowIndex].Selected = true;
                    this.callJobBindingSource.Position = e.RowIndex;
                }
            }
        }

        private void addProjectDocumentButton_Click(object sender, EventArgs e)
        {
            NewProjectDocument();
        }

        private void editProjectDocumentButton_Click(object sender, EventArgs e)
        {
            if (this.projectDocumentsBindingSource.Current == null)
            {
                MessageBox.Show("Sie müssen ein Dokument wählen um es bearbeiten zu können.");
                return;
            }

            EditProjectDocument();
        }
        
        private void projectDocumentsDataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
                DocumentCategory category = (DocumentCategory) e.Value;
                DocumentCategoryInfo info = MetaCall.Business.ProjectDocuments.GetDocumentCategoryInfo(category);

                if (info != null)
                {
                    e.Value = info.DisplayName;
                    e.FormattingApplied = true;
                }
            }
        }

        private void removeProjectDocumentButton_Click(object sender, EventArgs e)
        {
            if (this.projectDocumentsBindingSource.Current == null)
            {
                MessageBox.Show("Sie müssen ein Dokument wählen um es entfernen zu können.");
                return;
            }

            RemoveProjectDocument();
        }

        private void projectDocumentsDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                EditProjectDocument();
            }
        }

        private void buttonLastProjectCallHelp_Click(object sender, EventArgs e)
        {
            string msg = "Über den Button 'Letzter Anruf starten' werden alle Teamvorlagen geschlossen und alle offenen Adressen stehen für einen letztmaligen Anruf zu Verfügung!";

            MessageBoxOptions options = CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft ? MessageBoxOptions.RtlReading : 0;
            MessageBox.Show(this, msg, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, options);
        }

        private void buttonLastProjectCall_Click(object sender, EventArgs e)
        {
            string msg = "Möchten Sie nun das Projekt für die letzte Anrufrunde vorbereiten?";

            MessageBoxOptions options = CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft ? MessageBoxOptions.RtlReading : 0;
            if (MessageBox.Show(this, msg, Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, options) == DialogResult.Yes)
            {
                MetaCall.Business.Projects.SetLastCall(this.project);
                if (this.dataGridView1.RowCount > 0)
                {
                    loadCallJobsButton_Click(null, null);
                }
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            this.textBoxCountCallJobs.Text = string.Format("{0}/{1}", this.dataGridView1.RowCount, this.dataGridView1.SelectedRows.Count);
        }

        private void alleAuswählenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.dataGridView1.SelectAll();
        }

        private void auswahlAufhebenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.dataGridView1.ClearSelection();
        }

        private void buttonCallJobGroupsRefresh_Click(object sender, EventArgs e)
        {
            string msg = "Möchten Sie nun für alle Sponsoren dieses Projektes die Anrufgruppen aktualisieren?";

            MessageBoxOptions options = CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft ? MessageBoxOptions.RtlReading : 0;
            //if (MessageBox.Show(this, msg, Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, options) == DialogResult.Yes) ;
            DialogResult dR = MessageBox.Show(this, msg, Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, options);

            UpdateUI();

            if (dR == DialogResult.Yes)
            {
                this.isInWork = true;
                UpdateUI();
                //Progressbar initialisieren
                this.progressBarCallJobsLoad.Minimum = 0;
                this.progressBarCallJobsLoad.Maximum = 100;
                this.progressBarCallJobsLoad.Value = 0;
                this.progressBarCallJobsLoad.Visible = true;

                Project atmProject = MetaCall.Business.Projects.Get(this.project.ProjectId);
                atmProject.Sponsors = MetaCall.Business.Addresses.GetSponsorsByProject(atmProject).ToArray();

                AddressTransferManager aTM = new AddressTransferManager(this);
                aTM.ProgressChanged += new ProgressChangedEventHandler(aTM_ProgressChanged);
                aTM.TransferCompleted += new TransferCompletedEventHandler(aTM_TransferCompleted);
                aTM.UpdateCallJobGroups(atmProject);
            }
        }

        void aTM_TransferCompleted(object sender, TransferCompletedEventArgs e)
        {
            //Eventhandler entfernen
            AddressTransferManager aTM = sender as AddressTransferManager;
            aTM.ProgressChanged -= new ProgressChangedEventHandler(this.aTM_ProgressChanged);
            aTM.TransferCompleted += new TransferCompletedEventHandler(this.aTM_TransferCompleted);

            if (this.callJobsDataTable.Rows.Count > 0)
            {
                loadCallJobsButton_Click(null, null);
            }

            this.progressBarCallJobsLoad.Visible = false;
            this.taskId = Guid.Empty;
            this.isInWork = false;
        }

        void aTM_ProgressChanged(ProgressChangedEventArgs e)
        {
            if (e is TransferAddressesProgressChangedEventArgs)
            { 
                TransferAddressesProgressChangedEventArgs transferAddressesProgressChangedEventArgs = e as TransferAddressesProgressChangedEventArgs;

                string msg = "Anrufgruppe {0} von {1} ({2}%)";

                this.progressBarCallJobsLoad.Text =
                    string.Format(msg, 
                    transferAddressesProgressChangedEventArgs.Step,
                    transferAddressesProgressChangedEventArgs.Project.Sponsors.Length.ToString(),
                    transferAddressesProgressChangedEventArgs.ProgressPercentage);

                this.progressBarCallJobsLoad.Value = transferAddressesProgressChangedEventArgs.ProgressPercentage;
            }
        }

        private void buttonCallJobGroupsRefreshHelp_Click(object sender, EventArgs e)
        {
            string msg = "Über den Button 'Anrufgruppen aktualisieren' werden von allen Sponsoren dieses Projektes die Anrufgruppen aktualisiert. Eventuelle manuelle Änderungen gehen hierbei verloren!";

            MessageBoxOptions options = CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft ? MessageBoxOptions.RtlReading : 0;
            MessageBox.Show(this, msg, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, options);
        }

        private void DeleteCallJobGroupAssignsCompletely()
        {
            foreach (var currentCallJobGroup in currentCallJobGroups)
            {
                 currentCallJobGroup.Teams = new TeamInfo[0]{};
                 currentCallJobGroup.Users = new UserInfo[0]{};
            }
        }

        private void FillCallJobGroupAssignsCompletely()
        {
            foreach (var currentCallJobGroup in currentCallJobGroups)
            {
                int i = 0;
                currentCallJobGroup.Teams = new TeamInfo[project.Teams.Length];

                foreach (var teamInfo in project.Teams)
                {
                    currentCallJobGroup.Teams[i++] = teamInfo;

                    List<UserInfo> users = new List<UserInfo>();
                    Team team = MetaCall.Business.Teams.GetTeam(teamInfo);

                    foreach (UserInfo userInfo in team.TeamMitglieder)
                    {
                        users.Add(userInfo);
                    }

                    currentCallJobGroup.Users = new UserInfo[users.Count];
                    users.CopyTo(currentCallJobGroup.Users);
                }
            }
        }

        private void selectCallJobGroupButton_Click(object sender, EventArgs e)
        {
            const string msg = "Möchten Sie für alle Anrufgruppen die Zuordnung aller Teams und Agents setzen?";

            if (MessageBox.Show(this, msg, Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                FillCallJobGroupAssignsCompletely();
            }
        }

        private void delCallJobGroupsButton_Click(object sender, EventArgs e)
        {
            const string msg = "Möchten Sie die Zuordnung der Teams und Agents für alle Anrufgruppen löschen?";
            if (MessageBox.Show(this, msg, Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                DeleteCallJobGroupAssignsCompletely();
            }
        }

        private void editAllCallJobGroupsButton_Click(object sender, EventArgs e)
        {
            using (CallJobGroupEdit callJobGroupEdit = new CallJobGroupEdit(this.currentCallJobGroups, this.currentTeams))
            {
                if (callJobGroupEdit.ShowDialog() == DialogResult.OK)
                {
                    return;
                }
            }
        }
    }
}
