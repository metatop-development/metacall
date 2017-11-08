using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using metatop.Applications.metaCall.DataObjects;

namespace metatop.Applications.metaCall.WinForms.Modules.Telefonie
{
    [ToolboxItem(false)]
    public partial class CallJobSearchPanel : UserControl
    {

        public event UserWantSpecialCallHandler UserWantSpecialCall;
        
        private DataTable callJobsDataTable = new DataTable();
        
        public CallJobSearchPanel()
        {
            InitializeComponent();

            this.callJobsDataTable.Locale = CultureInfo.CurrentUICulture;
            
            this.bindingSource1.DataSource = this.callJobsDataTable;

            SetupDataCallJobsDataTable();
        }


        private void SetupDataCallJobsDataTable()
        {

            DataTableHelper.AddColumn(this.callJobsDataTable, "Sponsor", "Sponsor", typeof(string));
            DataTableHelper.AddColumn(this.callJobsDataTable, "Strasse", "Strasse", typeof(string));
            DataTableHelper.AddColumn(this.callJobsDataTable, "Ort", "Ort", typeof(string));
            DataTableHelper.AddColumn(this.callJobsDataTable, "CallJob", "CallJob", typeof(CallJob), MappingType.Hidden);

            DataTableHelper.FillGridView(this.callJobsDataTable, this.dataGridView1);


            //Einstellungen für das datagridview
            this.dataGridView1.RowHeadersVisible = false;

            DataGridViewColumn column = this.dataGridView1.Columns[0];
            column.FillWeight = 60;
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            column.MinimumWidth = column.GetPreferredWidth(DataGridViewAutoSizeColumnMode.ColumnHeader, true);

            column = this.dataGridView1.Columns[1];
            column.FillWeight = 20;
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            column.MinimumWidth = column.GetPreferredWidth(DataGridViewAutoSizeColumnMode.ColumnHeader, true);

            column = this.dataGridView1.Columns[2];
            column.FillWeight = 20;
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            column.MinimumWidth = column.GetPreferredWidth(DataGridViewAutoSizeColumnMode.ColumnHeader, true);

        }

        private void FillDataTable()
        {

            this.bindingSource1.SuspendBinding();

            try
            {
                ProjectInfo project = null;
                UserInfo user = MetaCall.Business.Users.GetUserInfo(MetaCall.Business.Users.CurrentUser);
                string expression = this.SearchExpression.Text;
                bool isAdminMode = System.Threading.Thread.CurrentPrincipal.IsInRole(metaCall.BusinessLayer.MetaCallPrincipal.AdminRoleName);


                this.callJobsDataTable.Clear();
                this.bindingSource1.Filter = null;

                //Wenn kein Filter gewählt wurde so wird 
                // auch keine Abfrage an den Server geschickt.
                if (expression == null ||
                    expression.Length < 1)
                    return;

                Cursor = Cursors.WaitCursor;

                if (MetaCall.Business.SponsoringCallManager.IsRunning)
                {
                    project = MetaCall.Business.SponsoringCallManager.Project;
                    user = MetaCall.Business.Users.GetUserInfo(MetaCall.Business.SponsoringCallManager.User);

                }

                bool excludeRefusals = this.checkBoxRefusals.Checked;
                List<CallJob> callJobs = MetaCall.Business.CallJobs.GetCallJobsByUserAndProject(user, project, expression, isAdminMode, excludeRefusals);


                foreach (CallJob callJob in callJobs)
                {
                    object[] objectData = new object[]{
                        callJob.Sponsor.DisplayName,
                        callJob.Sponsor.Strasse,
                        callJob.Sponsor.DisplayResidence,
                        callJob};

                    this.callJobsDataTable.Rows.Add(objectData);
                }
            }
            finally
            {
                this.bindingSource1.ResumeBinding();
                Cursor = Cursors.Default;
            }
        }

        private void ApplyFilter()
        {
            if (this.SearchExpression.TextLength == 0)
            {
                this.bindingSource1.Filter = null;
                return;
            }

            string[] expressions = this.SearchExpression.Text.Split(' ');
            StringBuilder sb = new StringBuilder();
            DataColumn prevColumn = null;

            foreach (DataColumn column in this.callJobsDataTable.Columns)
            {
                if (column.ColumnMapping != MappingType.Hidden)
                {
                    if (sb.Length > 0) sb.Append(" OR ");
                    sb.Append("(");
                    foreach (string expression in expressions)
                    {
                        if (sb.Length > 0 )
                            if (prevColumn == column)
                                sb.Append(" OR ");

                        if (column.DataType == typeof(string))
                        {
                            sb.AppendFormat("{0} LIKE '%{1}%'", column.ColumnName, expression);
                        }
                        else
                        {
                            sb.AppendFormat("{0} LIKE {1}", column.ColumnName, expression);
                        }

                        prevColumn = column;
                    }
                    sb.Append(")");
                }
            }

            this.bindingSource1.Filter = sb.ToString();
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            FillDataTable();
        }

        private void OnUserWantSpecialCall(UserWantSpecialCallEventArgs e)
        {
            if (UserWantSpecialCall != null)
                UserWantSpecialCall(this, e);
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                this.bindingSource1.Position = e.RowIndex;

                CallJob callJob = this.callJobsDataTable.Rows[e.RowIndex]["CallJob"] as CallJob;

                if (callJob != null)
                {
                    UserInfo user = MetaCall.Business.Users.GetUserInfo(MetaCall.Business.Users.CurrentUser);

                    if (MetaCall.Business.SponsoringCallManager.IsRunning)
                    {
                        user = MetaCall.Business.Users.GetUserInfo(MetaCall.Business.SponsoringCallManager.User);

                    }

                    if (callJob.User == null
                        || System.Threading.Thread.CurrentPrincipal.IsInRole(metaCall.BusinessLayer.MetaCallPrincipal.AdminRoleName)
                        || callJob.User.UserId == MetaCall.Business.Users.CurrentUser.UserId)
                    {
                        //TODO: Call auf der Datenbank erstellen !!!!                    
                        try
                        {
                            //Überprüfen ob der CallJob einen Reminder hat und dann muss dieser Übergeben werden
                            //wird nur in tblCallJobs geprüft -> nicht in Calls geladen
                            CallJobReminder callJobReminder = MetaCall.Business.CallJobReminders.GetCallJobReminderByCallJob(callJob.CallJobId);

                            //TODO Ändern
                            //ACHTUNG if und else legen den Call in der Tabelle an. 
                            //wenn er schon in der Tabelle ist wird in die catch-Anweisung gesprungen
                            //Sollte der Call aber für den Current User angelegt sein kann dieser ja benutzt werden.
                            if (callJobReminder == null)
                            {
                                Call call;
                                call = MetaCall.Business.SponsoringCallManager.CheckCallExists(callJob);

                                if (call == null)
                                    call = MetaCall.Business.SponsoringCallManager.GetSingleCall(callJob, user);

                                if (!System.Threading.Thread.CurrentPrincipal.IsInRole(metaCall.BusinessLayer.MetaCallPrincipal.AdminRoleName)
                                    && call.User.UserId != MetaCall.Business.Users.CurrentUser.UserId)
                                    MessageBox.Show("Anderer User!");
                                else
                                    OnUserWantSpecialCall(new UserWantSpecialCallEventArgs(call));
                            }
                            else
                            {
                                ReminderCall reminderCall;
                                
                                reminderCall = MetaCall.Business.SponsoringCallManager.CheckReminderCallExists(callJob);
                                
                                if (reminderCall == null)
                                    reminderCall = MetaCall.Business.SponsoringCallManager.GetSingleReminderCall(callJob, user);

                                if (!System.Threading.Thread.CurrentPrincipal.IsInRole(metaCall.BusinessLayer.MetaCallPrincipal.AdminRoleName)
                                    && reminderCall.User.UserId != MetaCall.Business.Users.CurrentUser.UserId)
                                    MessageBox.Show("Anderer User!");
                                else
                                    OnUserWantSpecialCall(new UserWantSpecialCallEventArgs(reminderCall));

                            }

                            // CallJob aus der Liste entfernen
                            this.bindingSource1.RemoveCurrent();
                        }

                        catch (BusinessLayer.CannotReceiveCallException)
                        {
                            MessageBox.Show("Der Anruf kann nicht vom Server abgerufen werden. Er ist vermutlich durch einen anderen Benutzer gesperrt.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Diese Adresse ist einem anderen Benutzer zugeordnet, und kann durch Sie nicht aufgerufen werden!");
                    }
                }
            }
        }

        private void SearchExpression_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode ==  Keys.Enter)
            {
                FillDataTable();
                e.Handled = true;
            }
        }
    }
}
