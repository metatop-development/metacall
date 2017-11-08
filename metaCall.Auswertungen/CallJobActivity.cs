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

using CrystalDecisions.CrystalReports.Engine;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Globalization;


namespace metatop.Applications.metaCall.WinForms.Modules.Auswertungen
{
    [ModulIndex(1)]
    public partial class CallJobActivity : UserControl, IModulMainControl
    {
        public CallJobActivity()
        {
            InitializeComponent();
        }

        private List<UserCallJobActivityResults> results;
        private List<CenterInfo> centers;
        private List<TeamInfo> teams;
        private List<UserInfo> users;
        
        private void BindCenters()
        {
            this.centers = new List<CenterInfo>();

            if (System.Threading.Thread.CurrentPrincipal.IsInRole("Administrator"))
                centers.AddRange(MetaCall.Business.Centers.Centers);
            else
                centers.AddRange(MetaCall.Business.Centers.Centers);
            //TODO: Center auf die zulässigen Beschränken
            //centers = dataLayer.CenterDataLayer.GetCenters(dataLayer.CurrentUser);


            this.centerComboBox.Items.Clear();

            this.centerComboBox.Items.Add("Alle Center");
            foreach (CenterInfo center in centers)
            {
                centerComboBox.Items.Add(center.Bezeichnung);
            }

            centerComboBox.SelectedIndex = 0;
        }

        private void BindTeams(CenterInfo center)
        {
            //Administratoren dürfen auf alle teams des Centers zugreifen
            if (MetaCallPrincipal.Current.IsInRole(MetaCallPrincipal.AdminRoleName))
                this.teams = MetaCall.Business.Teams.GetByCenter(center);
            else
                if (MetaCallPrincipal.Current.IsInRole(MetaCallPrincipal.CenterAdminRoleName) &&
                    (MetaCall.Business.Centers.IsCenterAdmin(center, MetaCall.Business.Users.CurrentUser)))
                    this.teams = MetaCall.Business.Teams.GetByCenter(center);
                else
                    this.teams = MetaCall.Business.Teams.GetTeamsByUser(MetaCall.Business.Users.CurrentUser);



            this.teamComboBox.Items.Clear();
            this.teamComboBox.Items.Add("alle Teams");

            foreach (TeamInfo team in this.teams)
            {
                this.teamComboBox.Items.Add(team.Bezeichnung);
            }
            this.teamComboBox.SelectedIndex = 0;
        }

        private void BindUsers(CenterInfo center)
        {
            this.users = MetaCall.Business.Centers.GetUsers(center);

            this.userComboBox.Items.Clear();
            this.userComboBox.Items.Add("alle Mitarbeiter");
            foreach (UserInfo user in this.users)
            {
                this.userComboBox.Items.Add(user.DisplayName);                
            }

            this.userComboBox.SelectedIndex = 0;
        }

        private void BindUsers(TeamInfo team)
        {
            this.users = MetaCall.Business.Teams.GetUsers(team);

            this.userComboBox.Items.Clear();
            this.userComboBox.Items.Add("alle Mitarbeiter");
            foreach (UserInfo user in this.users)
            {
                this.userComboBox.Items.Add(user.DisplayName);
            }

            this.userComboBox.SelectedIndex = 0;
        }
        private void BindUsers()
        {
            this.users = MetaCall.Business.Users.Users;

            this.userComboBox.Items.Clear();
            this.userComboBox.Items.Add("alle Mitarbeiter");
            foreach (UserInfo user in this.users)
            {
                this.userComboBox.Items.Add(user.DisplayName);
            }

            this.userComboBox.SelectedIndex = 0;
        }

        #region IModulMainControl Member

        public event ModulInfoMessageHandler StatusMessage;

        public event QueryPermissionHandler QueryPermisson;

        public event ModuleStateChangedHandler StateChanged;

        public void Initialize(IModulMainControl caller)
        {
            //TODO: Implement method public void  Initialize(IModulMainControl caller)
            //throw new Exception("The method or operation is not implemented.");

            BindCenters();
            BindUsers();


            //Setzen des ZeitFilters
            int year  = DateTime.Now.Year;
            int month = DateTime.Now.Month;

            this.fromDateTimePicker.Value = new DateTime(year, month, 1);
            this.toDateTimePicker.Value = new DateTime(year, month, DateTime.DaysInMonth(year, month));

            this.monthMaskedTextBox.Text = string.Format("{0:00}{1:0000}", month, year);
            this.timeModeFromTo.Checked = true;
            SetTimeMode(null, EventArgs.Empty);

            UpdateUI();
            
        }

        public void UnloadModul(out bool canUnload)
        {
            //TODO: Implement method public void  UnloadModul(out bool canUnload)
            //throw new Exception("The method or operation is not implemented.");
            canUnload = true;
        }

        public ToolStrip CreateToolStrip()
        {
            //TODO: Implement method public ToolStrip  CreateToolStrip()
            //throw new Exception("The method or operation is not implemented.");
            return null;
        }

        public ToolStripMenuItem[] CreateMainMenuItems()
        {
            //TODO: Implement method public ToolStripMenuItem[]  CreateMainMenuItems()
            //throw new Exception("The method or operation is not implemented.");
            return null;
        }

        public bool CanPauseApplication
        {
            get { 
                return true;// throw new Exception("The method or operation is not implemented."); #
            }
        }

        public bool CanAccessModul(System.Security.Principal.IPrincipal principal)
        {
            return false;
            //return principal.IsInRole(MetaCallPrincipal.AdminRoleName);
        }

        #endregion

        private class Configuration: ModulConfigBase
        {

            public override StartUpMenuItem GetStartUpMenuItem()
            {
                return new StartUpMenuItem("Anrufaktivitäten", "Auswertungen");
            }

            public override bool HasStartupMenuItem
            {
                get {
                    return false;
                   }
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

        private TimeMode timeMode = TimeMode.FromTo;
        private enum TimeMode
        {
            FromTo,
            Month,
        }

        private void SetTimeMode(object sender, EventArgs e)
        {

            if (this.timeModeFromTo.Checked)
                this.timeMode = TimeMode.FromTo;
            else if (this.timeModeMonth.Checked)
                this.timeMode = TimeMode.Month;

            UpdateUI();

        }

        private void crystalReportViewer1_ReportRefresh(object source, CrystalDecisions.Windows.Forms.ViewerEventArgs e)
        {
            RefreshReport();
        }


        private void refreshButton_Click(object sender, EventArgs e)
        {
            RefreshReport();
        }

        private void centerComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (this.centerComboBox.SelectedIndex > 0)
            {
                CenterInfo center = this.centers[this.centerComboBox.SelectedIndex - 1];
                BindTeams(center);
                BindUsers(center);
            }
            else
            {
                BindUsers();
            }

            UpdateUI();
        }

        private void UpdateUI()
        {

            this.centerComboBox.Enabled = this.centerComboBox.Items.Count > 1;
            this.teamComboBox.Enabled = this.centerComboBox.SelectedIndex > 0 &&
                this.teamComboBox.Items.Count > 1;

            this.userComboBox.Enabled = this.userComboBox.Items.Count > 1;

            this.fromDateTimePicker.Enabled = (this.timeMode == TimeMode.FromTo);
            this.toDateTimePicker.Enabled = (this.timeMode == TimeMode.FromTo);
            this.monthMaskedTextBox.Enabled = (this.timeMode == TimeMode.Month);

        }
        
        private void RefreshReport()
        {

            StringBuilder filterText = new StringBuilder();


            Guid? centerId = null;
            Guid? teamId = null;
            Guid? userId = null;
            Guid? projectId = null;
            DateTime start;
            DateTime stop;


            if (this.centerComboBox.SelectedIndex > 0)
            {
                centerId = this.centers[this.centerComboBox.SelectedIndex - 1].CenterId;
            }

            if (this.teamComboBox.SelectedIndex > 0)
            {
                teamId = this.teams[this.teamComboBox.SelectedIndex - 1].TeamId;
            }

            if (this.userComboBox.SelectedIndex > 0)
            {
                userId = this.users[this.userComboBox.SelectedIndex - 1].UserId;
            }

            if (this.timeMode == TimeMode.FromTo)
            {
                start = this.fromDateTimePicker.Value;
                stop = this.toDateTimePicker.Value;
            }
            else
            {
                int month = int.Parse(this.monthMaskedTextBox.Text.Substring(0, 2));
                int year = int.Parse(this.monthMaskedTextBox.Text.Substring(3, 4));

                start = new DateTime(year, month, 1);
                stop = new DateTime(year, month, DateTime.DaysInMonth(year, month));
            }


            //Abrufen der Daten 
            try
            {

                results = MetaCall.Business.Users.GetUserCallJobActivityResults(
                    centerId, teamId, userId, projectId, start, stop);

                if (results.Count == 0)
                {
                    string msg = "Für den angegebenen Filter existieren keine Daten";
                    MessageBoxOptions options = CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft ?
                        MessageBoxOptions.RtlReading : 0;


                    MessageBox.Show(this, msg, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, options);

                    return;
                }
                filterText.AppendFormat("vom {0:d} bis {1:d}", start, stop);



                ReportDocument report = new ReportDocument();
                //Bericht instanzieren
                report = new ReportDocument();
                //Laden mit der entsprechenden Berichtsdatei
                report.Load(@"CallJobActivities.rpt", CrystalDecisions.Shared.OpenReportMethod.OpenReportByTempCopy);

                //Setzen der Filterüberschrift
                TextObject filterTextObject = report.ReportDefinition.ReportObjects["filterText"] as TextObject;
                if (filterTextObject != null)
                {
                    filterTextObject.Text = filterText.ToString();
                }

                //Datenherkunft initialisieren
                report.SetDataSource(results);

                //Zuweisen des Berichts zum BerichtsViewer
                this.crystalReportViewer1.ReportSource = report;
            }
            catch(Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "UI Policy");
                if (rethrow)
                    throw;
            }
        }

        private void teamComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (this.teamComboBox.SelectedIndex > 0)
            {
                TeamInfo team = this.teams[this.teamComboBox.SelectedIndex - 1];
                BindUsers(team);
            }

            UpdateUI();
        }

        private void fromDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            this.toDateTimePicker.MinDate = this.fromDateTimePicker.Value;
        }

        private void crystalReportViewer1_Error(object source, CrystalDecisions.Windows.Forms.ExceptionEventArgs e)
        {
            e.Handled = true;
            bool rethrow = ExceptionPolicy.HandleException(e.Exception, "UI Policy");
            if (rethrow)
                throw e.Exception;

        }
    }
}
