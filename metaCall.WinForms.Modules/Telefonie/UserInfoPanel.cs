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
    public partial class UserInfoPanel : UserControl
    {
        public event UserWantSpecialCallHandler UserWantSpecialCall;
        
        private DataTable dataTable = new DataTable();
        
        public UserInfoPanel()
        {
            InitializeComponent();

            dataTable.Locale = System.Globalization.CultureInfo.InvariantCulture;
            bindingSource1.DataSource = dataTable;

            SetupDataTable();
        }

        private void SetupDataTable()
        {
            DataTableHelper.AddColumn(dataTable, "Icon", string.Empty, typeof(Icon));
            DataTableHelper.AddColumn(dataTable, "Bezeichnung", "Projekt", typeof(string));
            DataTableHelper.AddColumn(dataTable, "Nachname", "Sponsor", typeof(string));
            DataTableHelper.AddColumn(dataTable, "Datum", "Datum/Zeitpunkt", typeof(string));
            DataTableHelper.AddColumn(dataTable, "Art", "Art", typeof(string));
            DataTableHelper.AddColumn(dataTable, "ReminderCall", string.Empty, typeof(ReminderCall), MappingType.Hidden);
            DataTableHelper.AddColumn(dataTable, "ReminderState", "ReminderState", typeof(ReminderState), MappingType.Hidden);


            DataTableHelper.FillGridView(dataTable, this.dataGridView1);

            //Formatieren der Spalten 
            this.dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;


        }

        private void LoadReminderCallsTable(ReminderCall[] totalCalls)
        {
            System.Diagnostics.Trace.WriteLine("LoadReminderCallsTable()");

            bindingSource1.SuspendBinding();
            try
            {
                dataTable.Rows.Clear();
                foreach (ReminderCall call in totalCalls)
                {
                    Icon icon;
                    ReminderState state;

                    if (call.CallJobReminder.ReminderDateStart < DateTime.Now)
                        if (call.CallJobReminder.ReminderDateStop < DateTime.Now)
                        {
                            icon = Properties.Resources.ReminderStateOutOfTime;
                            state = ReminderState.ReminderMustBeCalled;
                        }
                        else
                        {
                            icon = Properties.Resources.ReminderStateCanBeCalled;
                            state = ReminderState.ReminderCannBeCalled;
                        }
                    else
                    {
                        icon = Properties.Resources.ReminderStateInTime;
                        state = ReminderState.ReminderCoudNotCalled;
                    }

                    StringBuilder dateTimeInfo = new StringBuilder();
                    dateTimeInfo.AppendFormat("{0:d} {0:t}", call.CallJobReminder.ReminderDateStart);

                    if ((call.CallJobReminder.ReminderTracking == CallJobReminderTracking.OnlyTimeSpan) ||
                        (call.CallJobReminder.ReminderTracking == CallJobReminderTracking.DateAndTimeSpan))
                    {
                        dateTimeInfo.AppendFormat(" - {0:t}", call.CallJobReminder.ReminderDateStop);
                    }

                    string art = string.Empty;
                    if (call.CallJobReminder.User == null)
                    {
                        art = "T";
                    }
                    else
                    {
                        art = "P";
                    }


                    object[] rowData = new object[]{
                        icon,
                        call.CallJob.Project.Bezeichnung,
                        call.CallJob.Sponsor.DisplayName,
                        dateTimeInfo.ToString(),
                        art,
                        call, 
                        state
                    };

                    dataTable.Rows.Add(rowData);
                }
            }
            finally
            {
                bindingSource1.ResumeBinding();
            }
        }


        private void UserInfoPanel_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            MetaCallPrincipal principal = System.Threading.Thread.CurrentPrincipal as MetaCallPrincipal;

            MetaCall.Business.SponsoringCallManager.CallJobsChanged += new EventHandler(SponsoringCallManager_CallJobsChanged);
            MetaCall.Business.Projects.ProjectChanged += new metatop.Applications.metaCall.BusinessLayer.ProjectChangedEventHandler(projects_ProjectChanged);

            ReminderCall[] calls = new ReminderCall[MetaCall.Business.SponsoringCallManager.ReminderCalls.Count];
            MetaCall.Business.SponsoringCallManager.ReminderCalls.CopyTo(calls, 0);
            LoadReminderCallsTable(calls);

            this.searchTabPage.Controls.Clear();
            
            Label labelPermissionInfo = new Label();
            labelPermissionInfo.Parent = this.searchTabPage;
            labelPermissionInfo.Location = new Point(20, 20);
            labelPermissionInfo.Size = new Size(this.searchTabPage.Width - 40 , 40);
            if (principal.IsInRole(MetaCallPrincipal.AdminRoleName) || MetaCall.Business.Users.CurrentUser.ProjectSearchPermit == true)
            {
                labelPermissionInfo.Text = "Die Suche ist nur innerhalb eines Projekts möglich. \n" +
                    "Bitte melden Sie sich im Projekt an.";
            }
            else
            {
                labelPermissionInfo.Text = "Sie sind nicht für die Sponsorensuche berechtigt. \n" +
                    "Bitte wenden Sie sich an die Centerleitung!";
            }
            labelPermissionInfo.Visible = true;
            
        }

        void callJobSearchPanel_UserWantSpecialCall(object sender, UserWantSpecialCallEventArgs e)
        {
            OnUserWantSpecialCall(e);
        }

        void SponsoringCallManager_CallJobsChanged(object sender, EventArgs e)
        {

            ReminderCall[] calls = new ReminderCall[MetaCall.Business.SponsoringCallManager.ReminderCalls.Count];
            MetaCall.Business.SponsoringCallManager.ReminderCalls.CopyTo(calls, 0);
            if (dataGridView1.InvokeRequired)
                dataGridView1.Invoke(new LoadReminderInvoker(this.LoadReminderCallsTable), new object[] { calls });
            else
                LoadReminderCallsTable(calls);
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            return;
        }

        private delegate void LoadReminderInvoker(ReminderCall[] calls);
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //abrufen des Status der aktuellen Zeile
            ReminderState state = (ReminderState)dataTable.Rows[e.RowIndex]["ReminderState"];
            ReminderCall call = (ReminderCall)dataTable.Rows[e.RowIndex]["ReminderCall"];

            //Wenn der Reminder angerufen werden kann, so wird das Event ausgelöst.
          //  if (state != ReminderState.ReminderCoudNotCalled)
          //  {
                OnUserWantSpecialCall(new UserWantSpecialCallEventArgs(call));
          //  }

        }

        protected void OnUserWantSpecialCall(UserWantSpecialCallEventArgs e)
        {
            if (UserWantSpecialCall != null)
                UserWantSpecialCall(this, e);
        }
        
        private enum ReminderState
        {
            ReminderCannBeCalled,
            ReminderMustBeCalled,
            ReminderCoudNotCalled,
        }

        void projects_ProjectChanged(object sender, metatop.Applications.metaCall.BusinessLayer.ProjectChangedEventArgs e)
        {
            MetaCallPrincipal principal = System.Threading.Thread.CurrentPrincipal as MetaCallPrincipal;
            this.searchTabPage.Controls.Clear();
            if (MetaCall.Business.Users.CurrentUser != null)
            {
                if (principal.IsInRole(MetaCallPrincipal.AdminRoleName) || MetaCall.Business.Users.CurrentUser.ProjectSearchPermit == true)
                {
                    ProjectInfo project = e.Project;
                    if (project != null)
                    {
                        CallJobSearchPanel callJobSearchPanel = new CallJobSearchPanel();
                        callJobSearchPanel.Parent = this.searchTabPage;
                        callJobSearchPanel.Dock = DockStyle.Fill;
                        callJobSearchPanel.UserWantSpecialCall += new UserWantSpecialCallHandler(callJobSearchPanel_UserWantSpecialCall);
                    }
                    else
                    {
                        Label labelPermissionInfo = new Label();
                        labelPermissionInfo.Parent = this.searchTabPage;
                        labelPermissionInfo.Location = new Point(20, 20);
                        labelPermissionInfo.Size = new Size(this.searchTabPage.Width - 40, 40);
                        labelPermissionInfo.Text = "Die Suche ist nur innerhalb eines Projekts möglich. \n" +
                            "Bitte melden Sie sich im Projekt an.";
                        labelPermissionInfo.Visible = true;
                    }
                }
                else
                {
                    Label labelPermissionInfo = new Label();
                    labelPermissionInfo.Parent = this.searchTabPage;
                    labelPermissionInfo.Location = new Point(20, 20);
                    labelPermissionInfo.Size = new Size(this.searchTabPage.Width - 40, 40);
                    labelPermissionInfo.Text = "Sie sind nicht für die Sponsorensuche berechtigt. \n" +
                        "Bitte wenden Sie sich an die Centerleitung!";
                    labelPermissionInfo.Visible = true;
                }
            }
            else
            {
                Label labelPermissionInfo = new Label();
                labelPermissionInfo.Parent = this.searchTabPage;
                labelPermissionInfo.Location = new Point(20, 20);
                labelPermissionInfo.Size = new Size(this.searchTabPage.Width - 40, 40);
                labelPermissionInfo.Text = "Die Sponsorensuche ist erst nach der Anmeldung \n" +
                    "im System möglich!";
                labelPermissionInfo.Visible = true;
            }

        }
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == this.searchTabPage)
            {
            }
        }
       
    }


}
