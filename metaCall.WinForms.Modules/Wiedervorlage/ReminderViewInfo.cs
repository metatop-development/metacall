using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using MaDaNet.Common.AppFrameWork.ApplicationModul;
using metatop.Applications.metaCall.BusinessLayer;
using metatop.Applications.metaCall.DataObjects;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Globalization;

namespace metatop.Applications.metaCall.WinForms.Modules
{
    [ToolboxItem(true)]
    public partial class ReminderViewInfo : UserControl
    {
        private DataTable dtReminders = new DataTable();
        private Dictionary<string, string> FilterHashTable = new Dictionary<string, string>();

        private MetaCallPrincipal principal;

        private UserInfo userInfo = new UserInfo();
        private ProjectInfo projectInfo = new ProjectInfo();
        private TeamInfo teamInfo = new TeamInfo();

        private List<UserInfo> userInfos = new List<UserInfo>();
        private List<TeamInfo> teamInfos = new List<TeamInfo>();

        private enum SearchType
        {
            User,
            Project,
            Team
        }

        private enum RescheduleType : int
        {
            Day,
            Hour,
            Minute
        }

        private SearchType searchTypeSelected;

        public int ReminderCount
        {
            get 
            {
                int OpenCounter =  0;
                foreach (DataRowView dRV in this.bindingSourceCallJobReminder)
                {
                    CallJobReminder cJR = (CallJobReminder)dRV["CallJobReminder"];
                    if (cJR.ReminderState == CallJobReminderState.Open)
                        OpenCounter++;
                }
                return OpenCounter;
            }
        }

        private SearchType SearchTypeSelected
        {
            set 
            {
                if (this.searchTypeSelected != value)
                {
                    this.searchTypeSelected = value;
                    if (this.searchTypeSelected == SearchType.Project)
                    {
                        dataGridViewReminders.Columns[1].Visible = false;
                        dataGridViewReminders.Columns[2].Visible = true;
                    }
                    else if (this.searchTypeSelected == SearchType.User)
                    {
                        dataGridViewReminders.Columns[1].Visible = true;
                        dataGridViewReminders.Columns[2].Visible = false;
                    }
                    else if (this.searchTypeSelected == SearchType.Team)
                    {
                        dataGridViewReminders.Columns[1].Visible = true;
                        dataGridViewReminders.Columns[2].Visible = false;
                    }
                }
            }
            get { return searchTypeSelected; }
        }

        public ReminderViewInfo()
        {
            InitializeComponent();
        }

        public ReminderViewInfo(UserInfo uInfo, ProjectInfo pInfo, TeamInfo tInfo, int width, int height): this()
        {
            this.Width = width;
            this.Height = height;
            SetupDataTable();
            SearchTypeSelected = SearchType.Project;
            this.projectInfo = pInfo;
            this.teamInfo = tInfo;
            this.userInfo = uInfo;
            SetupReminderViewInfo();
        }

        public ReminderViewInfo(ProjectInfo pInfo, int width, int height)
            : this()
        {
            this.Width = width;
            this.Height = height;
            SetupDataTable();
            SearchTypeSelected = SearchType.Project;
            this.projectInfo = pInfo;
            SetupReminderViewInfo();
        }

        public ReminderViewInfo(UserInfo uInfo, TeamInfo tInfo, int width, int height)
            : this()
        {
            this.Width = width;
            this.Height = height;
            SetupDataTable();
            SearchTypeSelected = SearchType.User;
            this.userInfo = uInfo;
            this.teamInfo = tInfo;
            SetupReminderViewInfo();
        }

        public ReminderViewInfo(TeamInfo tInfo, int width, int height): this()
        {
            this.Width = width;
            this.Height = height;
            SetupDataTable();
            SearchTypeSelected = SearchType.Team;
            this.teamInfo = tInfo;
            SetupReminderViewInfo();
        }

        public ReminderViewInfo(int width, int height)
            : this()
        {
            this.Width = width;
            this.Height = height;
            SetupDataTable();
            SearchTypeSelected = SearchType.Project;
            SetupReminderViewInfo();
        }

        private void SetupReminderViewInfo()
        {
            dtReminders.Locale = CultureInfo.CurrentUICulture;
            bindingSourceCallJobReminder.DataSource = dtReminders;
            this.principal = System.Threading.Thread.CurrentPrincipal as MetaCallPrincipal;

            if (this.principal.IsInRole(MetaCallPrincipal.AdminRoleName))
            {
                //neuen Reminder erstellen erlaubt
                this.newToolStripButton.Enabled = true;
                //da nur Admins über kontextmenü User ändern dürfen erfolgt nur hier die Zuweisung
                this.userInfos = MetaCall.Business.Users.Users;
                //ebenso teams
                this.teamInfos = MetaCall.Business.Teams.Teams;
            }
            else
            {
                //darf keinen neuen Reminder erstellen
                this.newToolStripButton.Enabled = false;
            }

//            SetupDataTable();
            LoadRemindersIntoDataTable();

        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            this.textBoxCountReminders.Left = this.panel1.Width - this.textBoxCountReminders.Width - 20;
        }

        public ToolStrip CreateToolStrip()
        {
            return this.toolStripMenue;
        }

        public ToolStripMenuItem[] CreateMainMenuItems()
        {
            return null;
        }

        private void SetupDataTable()
        {
            DataTableHelper.AddColumn(this.dtReminders, "CallJobReminderId", "Id", typeof(Guid));
            DataTableHelper.AddColumn(this.dtReminders, "ProjectDisplayName", "Projekt", typeof(string));
            DataTableHelper.AddColumn(this.dtReminders, "UserDisplayName", "Agent", typeof(string));
            DataTableHelper.AddColumn(this.dtReminders, "TeamDisplayName", "Team", typeof(string));
            DataTableHelper.AddColumn(this.dtReminders, "SponsorDisplayName", "Sponsor", typeof(string));
            DataTableHelper.AddColumn(this.dtReminders, "ReminderArt", "Art", typeof(string));
            DataTableHelper.AddColumn(this.dtReminders, "ReminderTrackingDisplayName", "Termin-Art", typeof(string));
            DataTableHelper.AddColumn(this.dtReminders, "ReminderDateStart", "Von", typeof(string));
            DataTableHelper.AddColumn(this.dtReminders, "ReminderDateStop", "Bis", typeof(string));
            DataTableHelper.AddColumn(this.dtReminders, "ReminderStatus", "State", typeof(string));
            
            DataTableHelper.AddColumn(this.dtReminders, "CallJobReminder", string.Empty, typeof(CallJobReminder), MappingType.Hidden);

            DataTableHelper.FillGridView(this.dtReminders, this.dataGridViewReminders);

            dataGridViewReminders.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewReminders.RowHeadersVisible = true;
            dataGridViewReminders.ColumnHeadersVisible = true;
            dataGridViewReminders.AutoGenerateColumns = false;

            dataGridViewReminders.Columns[0].Visible = false;
            dataGridViewReminders.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewReminders.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewReminders.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewReminders.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewReminders.Columns[5].Width = 35;
            dataGridViewReminders.Columns[6].Width = 70;
            dataGridViewReminders.Columns[7].Width = 100;
            dataGridViewReminders.Columns[8].Width = 100;
            dataGridViewReminders.Columns[9].Width = 35;
        }

        private void LoadRemindersIntoDataTable()
        {
            List<CallJobReminderInfo> callJobReminderInfos = new List<CallJobReminderInfo>();

            if (SearchTypeSelected == SearchType.User)
            {
                if (this.principal.IsInRole(MetaCallPrincipal.AdminRoleName))
                {
                    callJobReminderInfos = MetaCall.Business.CallJobReminders.GetCallJobReminderInfoByUserAndProject(this.userInfo.UserId, null, null);
                }
                else
                {
                    callJobReminderInfos = MetaCall.Business.CallJobReminders.GetCallJobReminderInfoByUserAndProject(this.userInfo.UserId, null, null);
                }
            }
            else if (this.searchTypeSelected == SearchType.Project)
            {
                if (this.principal.IsInRole(MetaCallPrincipal.AdminRoleName))
                {
                    callJobReminderInfos = MetaCall.Business.CallJobReminders.GetCallJobReminderInfoByUserAndProject(null, this.projectInfo.ProjectId, null);
                }
                else
                {
                    callJobReminderInfos = MetaCall.Business.CallJobReminders.GetCallJobReminderInfoByUserAndProject(this.userInfo.UserId, this.projectInfo.ProjectId, this.teamInfo.TeamId);
                }

            }
            else if (this.searchTypeSelected == SearchType.Team)
            {
                callJobReminderInfos = MetaCall.Business.CallJobReminders.GetCallJobReminderInfoByUserAndProject(null, null, this.teamInfo.TeamId);
            }

            else
            {
                callJobReminderInfos = null;
            }

            bindingSourceCallJobReminder.SuspendBinding();
            try
            {
                dtReminders.Rows.Clear();

                if (callJobReminderInfos != null)
                {
                    foreach (CallJobReminderInfo reminderInfo in callJobReminderInfos)
                    {

                        CallJobReminder callJobReminder = MetaCall.Business.CallJobReminders.GetCallJobReminder(reminderInfo.CallJobReminderId);

                        object[] objectData = new object[]
                            {
                            reminderInfo.CallJobReminderId,
                            reminderInfo.ProjectDisplayName,
                            reminderInfo.UserDisplayName,
                            reminderInfo.TeamDisplayName,
                            reminderInfo.SponsorDisplayName,
                            reminderInfo.ReminderArt,
                            reminderInfo.ReminderTrackingDisplayName,
                            reminderInfo.DisplayStartDate,
                            reminderInfo.DisplayEndDate,
                            reminderInfo.ReminderStatus,
                            callJobReminder
                            };

                        dtReminders.Rows.Add(objectData);
                    }
                }
            }
            finally
            {
                bindingSourceCallJobReminder.ResumeBinding();
                dataGridViewReminders_SelectionChanged(null, null);
                
            }
        }

        private void Reload()
        {
            LoadRemindersIntoDataTable();
        }

        private string[] ReminderTrackingDisplayName = new string[6] { "Zeitpunkt", "Tag", "KW", "WT", "Zeitspanne", "Zeitspanne" };

        private void CallJobReminderUpdate()
        {
            if (this.selectedCallJobReminder != null)
            {
                DataRowView rowView = this.bindingSourceCallJobReminder.Current as DataRowView;

                using (ReminderEdit dlg = new ReminderEdit(selectedCallJobReminder))
                {
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            rowView.BeginEdit();
                            rowView["ProjectDisplayName"] = selectedCallJobReminder.Project.Bezeichnung;
                            if (selectedCallJobReminder.User != null)
                            {
                                rowView["UserDisplayName"] = selectedCallJobReminder.User.DisplayName;
                                rowView["ReminderArt"] = "Pers.";
                            }
                            else
                            {
                                rowView["UserDisplayName"] = "";
                                rowView["ReminderArt"] = "Team";
                            }

                            if (selectedCallJobReminder.Team != null)
                            {
                                rowView["TeamDisplayName"] = selectedCallJobReminder.Team.Bezeichnung;
                            }
                            else
                            {
                                rowView["TeamDisplayName"] = "";
                            }

                            rowView["SponsorDisplayName"] = selectedCallJobReminder.CallJob.Sponsor.DisplayName;

                            rowView["ReminderTrackingDisplayName"] = ReminderTrackingDisplayName[(int)selectedCallJobReminder.ReminderTracking];
                            if (selectedCallJobReminder.ReminderTracking == CallJobReminderTracking.DateAndTimeSpan || 
                                selectedCallJobReminder.ReminderTracking == CallJobReminderTracking.OnlyTimeSpan)
                            {
                                rowView["ReminderDateStart"] = selectedCallJobReminder.ReminderDateStart.ToString("t");
                                rowView["ReminderDateStop"] = selectedCallJobReminder.ReminderDateStop.ToString("t");
                            }
                            else if (selectedCallJobReminder.ReminderTracking == CallJobReminderTracking.Day)
                            {
                                rowView["ReminderDateStart"] = selectedCallJobReminder.ReminderDateStart.ToString("d");
                                rowView["ReminderDateStop"] = string.Empty;
                            }
                            else if (selectedCallJobReminder.ReminderTracking == CallJobReminderTracking.Week)
                            {
                                DateUtilitys.CalendarWeek calenderWeek = DateUtilitys.GetCalendarWeek(selectedCallJobReminder.ReminderDateStart);
                                rowView["ReminderDateStart"] = string.Format("{0} / {1}", calenderWeek.Week.ToString(), calenderWeek.Year.ToString());
                                rowView["ReminderDateStop"] = string.Empty;
                            }
                            else if (selectedCallJobReminder.ReminderTracking == CallJobReminderTracking.WeekDay)
                            {
                                rowView["ReminderDateStart"] = selectedCallJobReminder.ReminderDateStart.ToString("dddd");
                                rowView["ReminderDateStop"] = string.Empty;
                            }
                            else if (selectedCallJobReminder.ReminderTracking == CallJobReminderTracking.ExactDateAndTime)
                            {
                                rowView["ReminderDateStart"] = selectedCallJobReminder.ReminderDateStart.ToString("g");
                                rowView["ReminderDateStop"] = string.Empty; 
                            }

                            if (selectedCallJobReminder.ReminderState == CallJobReminderState.Finished)
                                rowView["ReminderStatus"] = "erl.";
                            else
                                rowView["ReminderStatus"] = "Offen";

                            rowView.EndEdit();

                            MetaCall.Business.CallJobReminders.Update(selectedCallJobReminder);
                        }
                        catch (Exception ex)
                        {
                            bool rethrow = ExceptionPolicy.HandleException(ex, "UI Policy");
                            if (rethrow)
                                throw;
                        }
                    }
                }
            }
        }

        private void CallJobReminderNew()
        {
            using (ReminderEdit dlg = new ReminderEdit(null))
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        MetaCall.Business.CallJobReminders.Create(dlg.SelectedCallJobReminder);
                    }
                    catch (Exception ex)
                    {
                        bool rethrow = ExceptionPolicy.HandleException(ex, "UI Policy");
                        if (rethrow)
                            throw;
                    }
                }
            }
        }

        private void editToolStripButton_Click(object sender, EventArgs e)
        {
            CallJobReminderUpdate();
            Reload();

        }

        public CallJobReminder selectedCallJobReminder
        {
            get
            {
                if (dataGridViewReminders.CurrentRow == null)
                    return null;

                DataRowView currentRowView =
                    (DataRowView)dataGridViewReminders.CurrentRow.DataBoundItem;

                if (currentRowView == null || currentRowView.Row == null)
                    return null;

                return (CallJobReminder)
                    currentRowView.Row.ItemArray[
                    currentRowView.Row.ItemArray.Length - 1];
            }
        }

        private enum ChangeType
        {
            Agent,
            Project,
            Team
        }


        private void contextMenuStripCallJobReminder_Opening(object sender, CancelEventArgs e)
        {

            this.contextMenuStripCallJobReminder.Items.Clear();

            if (this.principal.IsInRole(MetaCallPrincipal.AdminRoleName))
            {
                //nur wenn Admin kann er Reminder à Block manipulieren.
                ToolStripMenuItem selectedAll = new ToolStripMenuItem("Alle auswählen");
                selectedAll.Click += new EventHandler(CallJobReminderGridSelectAllToolStripMenuItem_Click);
                this.contextMenuStripCallJobReminder.Items.Add(selectedAll);

                ToolStripMenuItem selectedNothing = new ToolStripMenuItem("Auswahl aufheben");
                selectedNothing.Click += new EventHandler(CallJobReminderGridSelectNothingToolStripMenuItem_Click);
                this.contextMenuStripCallJobReminder.Items.Add(selectedNothing);

                this.contextMenuStripCallJobReminder.Items.Add(GetUsersToolStripItem());
                this.contextMenuStripCallJobReminder.Items.Add(GetTeamsToolStripItem());
            }

            ToolStripMenuItem rescheduleSelected = new ToolStripMenuItem("Termin verschieben");
            //rescheduleSelected.Click += new EventHandler(CallJobReminderRescheduleDaySelected_Click);
            rescheduleSelected.DropDownItems.Add(new ToolStripLabel("Bitte wählen!"));
            rescheduleSelected.DropDownItems.Add(GetRescheduleDayItem());
            rescheduleSelected.DropDownItems.Add(GetRescheduleHourItem());
            rescheduleSelected.DropDownItems.Add(GetRescheduleMinuteItem());
            this.contextMenuStripCallJobReminder.Items.Add(rescheduleSelected);

            ToolStripMenuItem setDone = new ToolStripMenuItem("Erledigt");
            setDone.Click += new EventHandler(CallJobReminderGetSetDoneToolStripMenuItem_Click);
            this.contextMenuStripCallJobReminder.Items.Add(setDone);

            //Zusammenstellen des Spaltenauswahl
            ToolStripMenuItem columnSelection = new ToolStripMenuItem("Spalten");
            columnSelection.AutoSize = true;

            foreach (DataGridViewColumn column in this.dataGridViewReminders.Columns)
            {
                ToolStripMenuItem columnChecked = new ToolStripMenuItem(column.HeaderText);
               if (column.HeaderText != "Id")
                {
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
                this.contextMenuStripCallJobReminder.Items.Add(columnSelection);

            Point mouse = this.dataGridViewReminders.PointToClient(MousePosition);

            DataGridView.HitTestInfo hitTestInfo = this.dataGridViewReminders.HitTest(mouse.X, mouse.Y);

            //Zusammenstellen des FilterKontextMenues
            ToolStripMenuItem filterItem = new ToolStripMenuItem("Filter");
            filterItem.AutoSize = true;
            if (hitTestInfo.ColumnIndex > -1)
            {
                DataGridViewColumn column = this.dataGridViewReminders.Columns[hitTestInfo.ColumnIndex];
                filterItem.DropDownItems.Add(new ToolStripLabel(string.Format("{0} filtern:", column.HeaderText)));
                filterItem.DropDownItems.Add(GetFilterToolStripItem(column));
            }

            if (this.bindingSourceCallJobReminder.Filter != null)
            {
                if (filterItem.DropDownItems.Count > 0) filterItem.DropDownItems.Add("-");
                ToolStripMenuItem clearButton = new ToolStripMenuItem("Filter löschen", null, this.ClearFilterToolStripMenuItem_Click, "clearFilter");
                clearButton.AutoSize = true;
                filterItem.DropDownItems.Add(clearButton);
            }

            //Filter
            if (filterItem.DropDownItems.Count > 0)
                this.contextMenuStripCallJobReminder.Items.Add(filterItem);

            this.contextMenuStripCallJobReminder.AutoClose = true;
            this.contextMenuStripCallJobReminder.ShowCheckMargin = false;
            this.contextMenuStripCallJobReminder.ShowImageMargin = false;
            this.contextMenuStripCallJobReminder.ShowItemToolTips = true;

            e.Cancel = (this.contextMenuStripCallJobReminder.Items.Count < 1);

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

        private ToolStripItem GetTeamsToolStripItem()
        {
            ToolStripMenuItem teamSelection = new ToolStripMenuItem("Team");
            teamSelection.AutoSize = true;

            ToolStripMenuItem teamMenuItem = new ToolStripMenuItem();

            foreach (TeamInfo teamInfo in this.teamInfos)
            {
                teamMenuItem = new ToolStripMenuItem(teamInfo.Bezeichnung);
                teamMenuItem.Tag = teamInfo;
                teamMenuItem.CheckOnClick = false;
                teamMenuItem.Click += new EventHandler(teamMenuItem_Click);

                teamSelection.DropDownItems.Add(teamMenuItem);
            }

            if (this.dataGridViewReminders.SelectedRows.Count < 1)
            {
                teamSelection.Enabled = false;
            }

            return teamSelection;
        }


        private ToolStripItem GetUsersToolStripItem()
        {
            ToolStripMenuItem userSelection = new ToolStripMenuItem("Agent");
            userSelection.AutoSize = true;

            ToolStripMenuItem userMenuItem = new ToolStripMenuItem();

            foreach (UserInfo userInfo in this.userInfos)
            {
                userMenuItem = new ToolStripMenuItem(userInfo.DisplayName);
                userMenuItem.Tag = userInfo;
                userMenuItem.CheckOnClick = false;
                userMenuItem.Click += new EventHandler(userMenuItem_Click);

                userSelection.DropDownItems.Add(userMenuItem);
            }

            if (this.dataGridViewReminders.SelectedRows.Count < 1)
            {
                userSelection.Enabled = false;
            }
            return userSelection;
        }

        private ToolStripItem GetRescheduleDayItem()
        {
            ToolStripTextBox dayTextBox = new ToolStripTextBox("dayTextBox");
            dayTextBox.KeyDown += new KeyEventHandler(DayTextBox_KeyDown);

            dayTextBox.Text = "<Tage (-/+)>";
            dayTextBox.ForeColor = Color.Silver;

            dayTextBox.GotFocus += new EventHandler(SendedTextBox_GotFocus);

            return dayTextBox;

        }

        private ToolStripItem GetRescheduleHourItem()
        {
            ToolStripTextBox hourTextBox = new ToolStripTextBox("hourTextBox");
            hourTextBox.KeyDown += new KeyEventHandler(HourTextBox_KeyDown);

            hourTextBox.Text = "<Stunden (-/+)>";
            hourTextBox.ForeColor = Color.Silver;

            hourTextBox.GotFocus += new EventHandler(SendedTextBox_GotFocus);

            return hourTextBox;

        }

        private ToolStripItem GetRescheduleMinuteItem()
        {
            ToolStripTextBox hourTextBox = new ToolStripTextBox("minuteTextBox");
            hourTextBox.KeyDown += new KeyEventHandler(MinuteTextBox_KeyDown);

            hourTextBox.Text = "<Minuten (-/+)>";
            hourTextBox.ForeColor = Color.Silver;

            hourTextBox.GotFocus += new EventHandler(SendedTextBox_GotFocus);

            return hourTextBox;

        }

        private ToolStripItem GetFilterToolStripItem(DataGridViewColumn column)
        {
            if ((column.ValueType.IsEnum) ||
                (column.DataPropertyName == "Sponsor") ||
                (column.DataPropertyName == "Projekt") ||
                (column.DataPropertyName == "Agent"))
            {
                ToolStripComboBox filterComboBox = new ToolStripComboBox("filterComboBox");
                filterComboBox.Tag = column;
                filterComboBox.DropDownClosed += new EventHandler(filterComboBox_DropDownClosed);
                filterComboBox.LostFocus += new EventHandler(filterComboBox_LostFocus);
                
                //Vorbelegen eines bereits eingegebenen Filters
                if (this.FilterHashTable.ContainsKey(column.DataPropertyName) &&
                        this.FilterHashTable[column.DataPropertyName] != null)
                {
                    filterComboBox.SelectedItem = this.FilterHashTable[column.DataPropertyName];
                }
                else
                {
                    filterComboBox.Text = "<bitte wählen>";
                }
                return filterComboBox;
            }
            else
            {
                ToolStripTextBox filterTextBox = new ToolStripTextBox("filterTextBox");
                filterTextBox.Tag = column;
                filterTextBox.KeyDown += new KeyEventHandler(filterTextBox_KeyDown);

                //Vorbelegen eines bereits eingegebenen Filters
                if (this.FilterHashTable.ContainsKey(column.DataPropertyName) &&
                        this.FilterHashTable[column.DataPropertyName] != null)
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

        private void filterComboBox_DropDownClosed(object sender, EventArgs e)
        {
            ToolStripComboBox filterComboBox = sender as ToolStripComboBox;

            if (filterComboBox == null)
                return;

            this.contextMenuStripCallJobReminder.Close(ToolStripDropDownCloseReason.ItemClicked);
        }

        private void filterTextBox_KeyDown(object sender, KeyEventArgs e)
        {
           if ((e.KeyCode == Keys.Enter) ||
                (e.KeyCode == Keys.Tab))
            {
                ToolStripTextBox filterTextBox = sender as ToolStripTextBox;
                if (filterTextBox == null)
                    return;

                DataGridViewColumn column = filterTextBox.Tag as DataGridViewColumn;

                if ((filterTextBox.Text != null) &&
                    (filterTextBox.Text.Length != 0))
                {
                    if (!this.FilterHashTable.ContainsKey(column.DataPropertyName))
                        this.FilterHashTable.Add(column.DataPropertyName, null);

                    this.FilterHashTable[column.DataPropertyName] = filterTextBox.Text;
                }
                else
                {
                    if (this.FilterHashTable.ContainsKey(column.DataPropertyName))
                        this.FilterHashTable.Remove(column.DataPropertyName);
                }

                ApplyCallJobFilter();

                this.contextMenuStripCallJobReminder.Close(ToolStripDropDownCloseReason.ItemClicked);
            }
        }

        private void filterComboBox_LostFocus(object sender, EventArgs e)
        {
            ToolStripComboBox filterComboBox = sender as ToolStripComboBox;

            if (filterComboBox == null)
                return;

            DataGridViewColumn column = filterComboBox.Tag as DataGridViewColumn;

            if (filterComboBox.SelectedItem != null)
            {
                if (!this.FilterHashTable.ContainsKey(column.DataPropertyName))
                    this.FilterHashTable.Add(column.DataPropertyName, null);

                this.FilterHashTable[column.DataPropertyName] = (string)filterComboBox.SelectedItem;
            }
            else
            {
                if (this.FilterHashTable.ContainsKey(column.DataPropertyName))
                    this.FilterHashTable.Remove(column.DataPropertyName);
            }

            ApplyCallJobFilter();

            this.contextMenuStripCallJobReminder.Close();
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

        private void DayTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Enter) ||
                 (e.KeyCode == Keys.Tab))
            {
                ToolStripTextBox dayTextBox = sender as ToolStripTextBox;
                if (dayTextBox == null)
                    return;
                try
                {
                    Convert.ToInt32(dayTextBox.Text);
                }
                catch (FormatException ex)
                {
                    string msg = "Bitte geben Sie nur ganzzahlige Werte ein. Ein Plus-/Minus-Zeichen " +
                        "stellen Sie bitte voran!";
                    MessageBox.Show(msg);
                    this.contextMenuStripCallJobReminder.Close(ToolStripDropDownCloseReason.ItemClicked);
                    return;
                }

                CallJobReminderReschedule(RescheduleType.Day, Convert.ToInt32(dayTextBox.Text));

                this.contextMenuStripCallJobReminder.Close(ToolStripDropDownCloseReason.ItemClicked);
            }
        }

        private void HourTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Enter) ||
                 (e.KeyCode == Keys.Tab))
            {
                ToolStripTextBox hourTextBox = sender as ToolStripTextBox;
                if (hourTextBox == null)
                    return;
                try
                {
                    Convert.ToInt32(hourTextBox.Text);
                }
                catch (FormatException ex)
                {
                    string msg = "Bitte geben Sie nur ganzzahlige Werte ein. Ein Plus-/Minus-Zeichen " +
                        "stellen Sie bitte voran!";
                    MessageBox.Show(msg);
                    this.contextMenuStripCallJobReminder.Close(ToolStripDropDownCloseReason.ItemClicked);
                    return;
                }
                CallJobReminderReschedule(RescheduleType.Hour, Convert.ToInt32(hourTextBox.Text));

                this.contextMenuStripCallJobReminder.Close(ToolStripDropDownCloseReason.ItemClicked);
            }
        }

        private void MinuteTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Enter) ||
                 (e.KeyCode == Keys.Tab))
            {
                ToolStripTextBox minuteTextBox = sender as ToolStripTextBox;
                if (minuteTextBox == null)
                    return;
                try
                {
                    Convert.ToInt32(minuteTextBox.Text);
                }
                catch (FormatException ex)
                {
                    string msg = "Bitte geben Sie nur ganzzahlige Werte ein. Ein Plus-/Minus-Zeichen " +
                        "stellen Sie bitte voran!";
                    MessageBox.Show(msg);
                    this.contextMenuStripCallJobReminder.Close(ToolStripDropDownCloseReason.ItemClicked);
                    return;
                }
                CallJobReminderReschedule(RescheduleType.Minute, Convert.ToInt32(minuteTextBox.Text));

                this.contextMenuStripCallJobReminder.Close(ToolStripDropDownCloseReason.ItemClicked);
            }
        }

        private void SendedTextBox_GotFocus(object sender, EventArgs e)
        {
            ToolStripTextBox sendedTextBox = sender as ToolStripTextBox;

            if (sendedTextBox != null)
            {
                if (sendedTextBox.ForeColor == Color.Silver)
                {
                    sendedTextBox.Text = null;
                    sendedTextBox.ForeColor = Control.DefaultForeColor;
                }
                sendedTextBox.GotFocus -= new EventHandler(this.SendedTextBox_GotFocus);
            }
        }

        private void ApplyCallJobFilter()
        {
            StringBuilder filter = new StringBuilder();

            foreach (string key in this.FilterHashTable.Keys)
            {
                Type dataType = this.dtReminders.Columns[key].DataType;
                if (filter.Length > 0) filter.Append(" AND ");

                if (dataType == typeof(string))
                {
                    filter.AppendFormat("{0} like '%{1}%'", key, this.FilterHashTable[key]);
                }
                else if (dataType.IsEnum)
                {

                    filter.AppendFormat("{0} = {1}", key, Convert.ChangeType(Enum.Parse(dataType, this.FilterHashTable[key]), Enum.GetUnderlyingType(dataType)));
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
                this.bindingSourceCallJobReminder.Filter = filter.ToString();
            else
                this.bindingSourceCallJobReminder.RemoveFilter();

        }


        private void ClearFilterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.FilterHashTable.Clear();
            ApplyCallJobFilter();
        }

        private void dataGridViewReminders_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                this.dataGridViewReminders.Rows[e.RowIndex].Selected = true;
                CallJobReminderUpdate();
            }
        }

        private void dataGridViewReminders_SelectionChanged(object sender, EventArgs e)
        {
            if (this.dataGridViewReminders.SelectedRows.Count > 1)
            {
                this.editToolStripButton.Enabled = false;
            }
            else
            {
                this.editToolStripButton.Enabled = true;
            }
            this.textBoxCountReminders.Text = string.Format("{0}/{1}", this.bindingSourceCallJobReminder.Count, this.dataGridViewReminders.SelectedRows.Count);

        }

        private void CallJobReminderGridSelectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Alle Zeilen selektieren
            this.dataGridViewReminders.SelectAll();
        }

        private void CallJobReminderGridSelectNothingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Selektion aufheben
            this.dataGridViewReminders.ClearSelection();
        }

        private enum EvokeUpdateTyp
        {
            nothing,
            Project,
            User,
            Team,
            DataGrid
        }

        void userMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem userMenuItem = sender as ToolStripMenuItem;

            EvokeUpdateTyp evokeUpdateTyp = EvokeUpdateTyp.nothing;

            if (userMenuItem == null)
                return;

            UserInfo selectedUserInfo = userMenuItem.Tag as UserInfo;

            Control ctl = this.ActiveControl;

            string msg = string.Empty;

            UserInfo evokeUserInfo = null;
            ProjectInfo evokeProjectInfo = null;

            if (ctl is DataGridView)
            {
                msg = string.Format("Möchten Sie die markierten Wiedervorlagen dem Agent {0} zuordnen?", selectedUserInfo.DisplayName);
                evokeUpdateTyp = EvokeUpdateTyp.DataGrid;
            }

            if (evokeUpdateTyp != EvokeUpdateTyp.nothing)
            {
                MessageBoxOptions options = CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft ? MessageBoxOptions.RtlReading : 0;
                if (MessageBox.Show(this, msg, Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button1, options) == DialogResult.Yes)
                {
                    switch (evokeUpdateTyp)
                    {
                        case EvokeUpdateTyp.Project:
                            MetaCall.Business.CallJobReminders.UpdateCallJobReminders((int)evokeUpdateTyp,
                                                                                      null,
                                                                                      selectedUserInfo.UserId,
                                                                                      evokeProjectInfo.ProjectId,
                                                                                      null,
                                                                                      null);
                            break;
                        case EvokeUpdateTyp.User:
                            MetaCall.Business.CallJobReminders.UpdateCallJobReminders((int)evokeUpdateTyp,
                                                                                      null,
                                                                                      selectedUserInfo.UserId,
                                                                                      null,
                                                                                      evokeUserInfo.UserId,
                                                                                      null);
                            break;
                        case EvokeUpdateTyp.DataGrid:
                            foreach (DataGridViewRow row in this.dataGridViewReminders.SelectedRows)
                            {

                                DataRowView currentRowView = (DataRowView)row.DataBoundItem;

                                CallJobReminder curCallJobReminder = (CallJobReminder)currentRowView.Row.ItemArray[currentRowView.Row.ItemArray.Length - 1];

                                MetaCall.Business.CallJobReminders.UpdateCallJobReminders((int)evokeUpdateTyp,
                                                                                          null,
                                                                                          selectedUserInfo.UserId,
                                                                                          null,
                                                                                          null,
                                                                                          curCallJobReminder.CallJobReminderId);
                            }
                            break;
                    }
                }
            }
        }

        void teamMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem teamMenuItem = sender as ToolStripMenuItem;

            EvokeUpdateTyp evokeUpdateTyp = EvokeUpdateTyp.nothing;

            if (teamMenuItem == null)
                return;


            TeamInfo selectedTeamInfo = teamMenuItem.Tag as TeamInfo;

            Control ctl = this.ActiveControl;

            string msg = string.Empty;

            ProjectInfo evokeProjectInfo = null;
            UserInfo evokeUserInfo = null;

            if (ctl is DataGridView)
            {
                msg = string.Format("Möchten Sie die markierten Wiedervorlagen dem Team {0} zuordnen?", selectedTeamInfo.Bezeichnung);
                evokeUpdateTyp = EvokeUpdateTyp.DataGrid;
            }

            if (evokeUpdateTyp != EvokeUpdateTyp.nothing)
            {
                MessageBoxOptions options = CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft ? MessageBoxOptions.RtlReading : 0;
                if (MessageBox.Show(this, msg, Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button1, options) == DialogResult.Yes)
                {
                    switch (evokeUpdateTyp)
                    {
                        case EvokeUpdateTyp.Project:
                            MetaCall.Business.CallJobReminders.UpdateCallJobReminders((int)evokeUpdateTyp,
                                                                                      selectedTeamInfo.TeamId,
                                                                                      null,
                                                                                      evokeProjectInfo.ProjectId,
                                                                                      null,
                                                                                      null);

                            break;
                        case EvokeUpdateTyp.User:
                            MetaCall.Business.CallJobReminders.UpdateCallJobReminders((int)evokeUpdateTyp,
                                                                                      selectedTeamInfo.TeamId,
                                                                                      null,
                                                                                      null,
                                                                                      evokeUserInfo.UserId,
                                                                                      null);

                            break;
                        case EvokeUpdateTyp.DataGrid:
                            foreach (DataGridViewRow row in this.dataGridViewReminders.SelectedRows)
                            {

                                DataRowView currentRowView = (DataRowView)row.DataBoundItem;

                                CallJobReminder curCallJobReminder = (CallJobReminder)currentRowView.Row.ItemArray[currentRowView.Row.ItemArray.Length - 1];


                                MetaCall.Business.CallJobReminders.UpdateCallJobReminders((int)evokeUpdateTyp,
                                                                                          selectedTeamInfo.TeamId,
                                                                                          null,
                                                                                          null,
                                                                                          null,
                                                                                          curCallJobReminder.CallJobReminderId);
                            }
                            break;
                    }
                }
            }
        }

        private void newToolStripButton_Click(object sender, EventArgs e)
        {
            CallJobReminderNew();
            Reload();
        }

        private void ReminderViewInfo_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;
        }

        private void CallJobReminderReschedule(RescheduleType rescheduleType, int zeit)
        {
            int zaehler = 0;
            Guid[] selectRows = new Guid[this.dataGridViewReminders.SelectedRows.Count];
            foreach (DataGridViewRow dGV in this.dataGridViewReminders.SelectedRows)
            {
                DataRowView currentRowView = (DataRowView)dGV.DataBoundItem;
                selectRows[zaehler++] = (Guid)currentRowView.Row.ItemArray[0];
                CallJobReminder curCallJobReminder = (CallJobReminder)currentRowView.Row.ItemArray[currentRowView.Row.ItemArray.Length - 1];
                System.DateTime maxDate = (System.DateTime)(curCallJobReminder.Project.ReminderDateMax.HasValue ? 
                    curCallJobReminder.Project.ReminderDateMax : System.DateTime.MaxValue.AddDays(-1));
                maxDate = maxDate.AddDays(1);
                switch (rescheduleType)
                {
                    case RescheduleType.Day:
                        if (System.Threading.Thread.CurrentPrincipal.IsInRole(MetaCallPrincipal.AdminRoleName) ||
                            curCallJobReminder.ReminderDateStart.AddDays(zeit) < maxDate)
                        {
                            curCallJobReminder.ReminderDateStart = curCallJobReminder.ReminderDateStart.AddDays(zeit);
                            curCallJobReminder.ReminderDateStop = curCallJobReminder.ReminderDateStop.AddDays(zeit);
                        }
                        break;
                    case RescheduleType.Hour:
                        if (System.Threading.Thread.CurrentPrincipal.IsInRole(MetaCallPrincipal.AdminRoleName) ||
                            curCallJobReminder.ReminderDateStart.AddHours(zeit) < maxDate)
                        {
                            curCallJobReminder.ReminderDateStart = curCallJobReminder.ReminderDateStart.AddHours(zeit);
                            curCallJobReminder.ReminderDateStop = curCallJobReminder.ReminderDateStop.AddHours(zeit);
                        }
                        break;
                    case RescheduleType.Minute:
                        if (System.Threading.Thread.CurrentPrincipal.IsInRole(MetaCallPrincipal.AdminRoleName) ||
                            curCallJobReminder.ReminderDateStart.AddMinutes(zeit) < maxDate)
                        {
                            curCallJobReminder.ReminderDateStart = curCallJobReminder.ReminderDateStart.AddMinutes(zeit);
                            curCallJobReminder.ReminderDateStop = curCallJobReminder.ReminderDateStop.AddMinutes(zeit);
                        }
                        break;
                }
                MetaCall.Business.CallJobReminders.Update(curCallJobReminder);
            }
            LoadRemindersIntoDataTable();
            //jetzt wieder alle bisherig markierten markieren
            this.dataGridViewReminders.ClearSelection();
            foreach (DataGridViewRow dGV in this.dataGridViewReminders.Rows)
            {
                DataRowView currentRowView = (DataRowView)dGV.DataBoundItem;
                foreach (Guid selectedRow in selectRows)
                {
                    if(selectedRow.Equals((Guid)currentRowView.Row.ItemArray[0]))
                    {
                        dGV.Selected = true;
                    }
                }
            }
        }

        private void CallJobReminderGetSetDoneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string msg = string.Format("Möchten Sie die markierten Wiedervorlagen als erledigt kennzeichnen?");

            MessageBoxOptions options = CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft ? MessageBoxOptions.RtlReading : 0;
            if (MessageBox.Show(this, msg, Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button1, options) == DialogResult.Yes)
            {
                foreach (DataGridViewRow dGV in this.dataGridViewReminders.SelectedRows)
                {

                    DataRowView currentRowView = (DataRowView)dGV.DataBoundItem;

                    CallJobReminder curCallJobReminder = (CallJobReminder)currentRowView.Row.ItemArray[currentRowView.Row.ItemArray.Length - 1];

                    curCallJobReminder.ReminderState = CallJobReminderState.Finished;

                    MetaCall.Business.CallJobReminders.Update(curCallJobReminder);
                }
                LoadRemindersIntoDataTable();
                }
        }
    }
}
