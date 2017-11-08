using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using metatop.Applications.metaCall.BusinessLayer;
using metatop.Applications.metaCall.DataObjects;
using metatop.Applications.metaCall.BusinessLayer.Activities;

namespace metatop.Applications.metaCall.WinForms.Modules.Telefonie
{
    [ToolboxItem(false)]
    public partial class Projektanmeldung : UserControl
    {
        public event ProjectChangedEventHandler ProjectChanged;
        public event DurringChangedEventHandler DurringActivChanged;
        public event DurringLevelInfoChangedEventHandler DurringLevelInfoChanged;

        public event NewCustomerEventHandler NewCustomer;

        private bool updateComboBoxes = false;

        public Projektanmeldung()
        {
            InitializeComponent();

        }

        void Business_ProjectChanged(object sender, metatop.Applications.metaCall.BusinessLayer.ProjectChangedEventArgs e)
        {
            UpdateUI();
        }

        void Application_Idle(object sender, EventArgs e)
        {
            if (!MetaCall.Business.Pause)
            {
                UpdateUI();
            }
        }

        private void UpdateUI()
        {
            bool enableNewCustomer = (MetaCall.Business.Projects.Current != null &&
                MetaCall.Business.SponsoringCallManager.CallJobGroup != null && 
                MetaCall.Business.SponsoringCallManager.CallsAvailable &&
                !MetaCallPrincipal.Current.IsInRole(MetaCallPrincipal.AdminRoleName));


            if (MetaCall.Business.CallJobs.DurringActiv == true)
            {
                this.btnChangeCurrentProject.Enabled = false;
                //this.btnNewCustomer.Enabled = true;
                this.btnNewCustomer.Text = "Starten";
                this.cboCallJobGroups.Enabled = false;
                this.cboCenter.Enabled = false;
                this.cboTeam.Enabled = false;
                this.cboProjekt.Enabled = false;
                this.buttonDurring.Text = "Projekt";
            }
            else
            {
                this.btnNewCustomer.Text = "neuer Kunde";
                this.btnNewCustomer.Enabled = enableNewCustomer;
                this.buttonDurring.Text = "Mahnen";
                if (MetaCall.Business.Projects.Current != null)
                {
                    this.btnChangeCurrentProject.Text = "Abmelden";
                    this.btnChangeCurrentProject.Enabled = (cboProjekt.Items.Count > 0);
                    this.cboProjekt.Enabled = false;
                    this.cboCenter.Enabled = false;
                    this.cboTeam.Enabled = false;
                    // Wenn an einem Projekt angemeldet kann die Gruppe trotzdem geändert werden
                    this.cboCallJobGroups.Enabled = (cboCallJobGroups.Items.Count > 0);
                }
                else
                {
                    this.btnChangeCurrentProject.Text = "Anmelden";
                    this.btnChangeCurrentProject.Enabled = (cboProjekt.Items.Count > 0);
                    this.cboProjekt.Enabled = (cboProjekt.Items.Count > 1);
                    this.cboCenter.Enabled = (this.cboCenter.Items.Count > 1);
                    this.cboTeam.Enabled = (this.cboTeam.Items.Count > 1);
                    this.cboCallJobGroups.Enabled = (this.cboCallJobGroups.Items.Count > 1);
                }
            }
        }

        private void BindDurringLevels()
        {
            //DunningLevel ist veraltet und gibt es nicht mehr (DunningLevel war gruppiert: Anzahl der Mahnungsaktionen (Mahnstufe2)
            List<DurringLevelInfo> durringLevels = MetaCall.Business.CallJobs.getDurringLevelInfosByUser(MetaCall.Business.Users.CurrentUser); 

            //Die Abfrage wird nur noch verwendet, um festzustellen, ob es überhaupt Mahnungsjobs gibt
            if (durringLevels.Count != 0)
            {
                cboDurringLevel.Items.Clear();
                cboDurringLevel.Items.AddRange(new string[] { "<Alle Mahnungen>", "<neue Mahnungen>", "<bearbeitete Mahnungen>", "<Wiedervorlagen>" });

                this.btnNewCustomer.Enabled = true;
                this.cboDurringLevel.Enabled = true;
                this.cboDurringLevel.SelectedItem = "<Alle Mahnungen>";
            }
            else
            {
                cboDurringLevel.Items.Clear();
                cboDurringLevel.Text = "<keine Mahnungen vorhanden>";

                this.btnNewCustomer.Enabled = false;
                this.cboDurringLevel.Enabled = false;                
            }

        }

        private void BindCenters()
        {
            List<CenterInfo> centers = new List<CenterInfo>();

            if (System.Threading.Thread.CurrentPrincipal.IsInRole("Administrator"))
                centers.AddRange(MetaCall.Business.Centers.Centers);
            else
                centers.AddRange(MetaCall.Business.Centers.Centers);
                //TODO: Center auf die zulässigen Beschränken
                //centers = dataLayer.CenterDataLayer.GetCenters(dataLayer.CurrentUser);


            this.cboCenter.DisplayMember = "Bezeichnung";
            this.cboCenter.DataSource = centers;

            if (cboCenter.SelectedIndex == -1)
            {
                cboCenter.Text = "<keine Center vorhanden>";
            }

            this.cboCenter.Enabled = (centers.Count > 1);
        }

        private void BindTeams(CenterInfo center) 
        {
            List<TeamInfo> teams ;
            

            //Administratoren dürfen auf alle teams des Centers zugreifen
            if (MetaCallPrincipal.Current.IsInRole(MetaCallPrincipal.AdminRoleName))
                teams = MetaCall.Business.Teams.GetByCenter(center);
            else
                if (MetaCallPrincipal.Current.IsInRole(MetaCallPrincipal.CenterAdminRoleName) &&
                    (MetaCall.Business.Centers.IsCenterAdmin(center, MetaCall.Business.Users.CurrentUser)))
                    teams = MetaCall.Business.Teams.GetByCenter(center);
            else
                teams = MetaCall.Business.Teams.GetTeamsByUser(MetaCall.Business.Users.CurrentUser);
            

            this.cboTeam.DisplayMember = "Bezeichnung";
            this.cboTeam.DataSource = teams;

            if (cboTeam.SelectedIndex == -1)
            {
                cboTeam.Text = "<keine Teams vorhanden";
            }

            this.cboTeam.Enabled = (teams.Count > 1);
        }

        private BindingList<ProjectInfoWithStatistics> projects = new BindingList<ProjectInfoWithStatistics>();

        private void BindProjects(TeamInfo team) 
        {
            List<ProjectInfo> projectInfos = MetaCall.Business.Projects.GetByTeam(team);

            this.projects.Clear();

            foreach (ProjectInfo projectInfo in projectInfos)
            {
                ProjectInfoWithStatistics projectWithStatistics = new ProjectInfoWithStatistics(projectInfo);
                if(projectWithStatistics.Statistics.Total > 0)
                    this.projects.Add(projectWithStatistics);
            }
            foreach (ProjectInfo projectInfo in projectInfos)
            {
                ProjectInfoWithStatistics projectWithStatistics = new ProjectInfoWithStatistics(projectInfo);
                if (projectWithStatistics.Statistics.Total == 0)
                    this.projects.Add(projectWithStatistics);
            }

            this.cboProjekt.DataSource = this.projects;
            cboProjekt_SelectedIndexChanged(null, null);

            if (cboProjekt.SelectedIndex == -1)
            {
                cboProjekt.Text = "<keine Projekte vorhanden>";
            }
            this.cboProjekt.Enabled = (this.projects.Count > 1);

        }

        private BindingList<CallJobGroupWithStatistics> callJobGroups = new BindingList<CallJobGroupWithStatistics>();

        private void BindCallJobGroup(ProjectInfoWithStatistics projectInfo)
        {
            TeamInfo teamInfo = cboTeam.SelectedItem as TeamInfo;

            //ProjectInfoWithStatistics projectInfo = cboProjekt.SelectedItem as ProjectInfoWithStatistics;

            //List<CallJobGroup> callJobGroups = MetaCall.Business.CallJobGroups.Get(team, calljobGroup);
            List<CallJobGroupInfo> callJobGroups = new List<CallJobGroupInfo>();

            if (teamInfo != null && projectInfo != null)
            {
                if (MetaCallPrincipal.Current.IsInRole(MetaCallPrincipal.AdminRoleName) ||
                    MetaCallPrincipal.Current.IsInRole(MetaCallPrincipal.CenterAdminRoleName))
                {
                    callJobGroups = MetaCall.Business.CallJobGroups.GetCallJobGroupInfo(projectInfo.Project);
                }
                else
                {
                    callJobGroups = MetaCall.Business.CallJobGroups.Get(
                        MetaCall.Business.Users.GetUserInfo(MetaCall.Business.Users.CurrentUser),
                        teamInfo,
                        projectInfo.Project
                    );
                }
            }

            this.callJobGroups.Clear();
            
            foreach(CallJobGroupInfo callJobGroup in callJobGroups)
            {
                CallJobGroupWithStatistics infoWithStatistics = new CallJobGroupWithStatistics(callJobGroup);
                this.callJobGroups.Add(infoWithStatistics);
            }


            //this.cboCallJobGroups.DisplayMember = "DisplayName";
            this.cboCallJobGroups.DataSource = this.callJobGroups;

            if (cboCallJobGroups.Items.Count < 1)
            {
                cboCallJobGroups.Text = "<keine Gruppeneinteilung vorhanden>";
            }
            this.cboCallJobGroups.Enabled = (this.callJobGroups.Count > 1);

        }

        private void cboCenter_SelectedIndexChanged(object sender, EventArgs e)
        {
            //AddressTransferManager atm = new AddressTransferManager(this);

            //atm.UpdateCallJobGroups();

            CenterInfo center = cboCenter.SelectedItem as CenterInfo;

            if (center != null)
            {
                BindTeams(center);
            }
        }

        private void cboTeam_SelectedIndexChanged(object sender, EventArgs e)
        {
            TeamInfo team = cboTeam.SelectedItem as TeamInfo;

            if (team != null)
            {
                BindProjects(team);
            }
        }

        private void btnChangeCurrentProject_Click(object sender, EventArgs e)
        {
            /// Der Button hat eine Doppelfunktion
            /// Ist gerade kein Projekt angemeldet, so wird eine 
            /// Anmeldung durchgeführt.
            /// Ist gerade ein Projekt angemeldet, so 
            /// wird eine Abmeldung durchgeführt
            ProjectInfo selectedProject = null;
            CallJobGroupInfo callJobGroup = null;

            CallJobGroupWithStatistics callJobGroupStats = this.cboCallJobGroups.SelectedItem as CallJobGroupWithStatistics;

            if (callJobGroupStats != null)
                callJobGroup = callJobGroupStats.CallJobGroup;

            if (MetaCall.Business.Projects.Current != null)
            {
                //Abmelden und raus ...
                MetaCall.Business.Projects.LogOff();
                this.buttonDurring.Enabled = true;
            }
            else
            {
                ProjectInfoWithStatistics item = this.cboProjekt.SelectedItem as ProjectInfoWithStatistics;

                selectedProject = item.Project;
                this.buttonDurring.Enabled = false;
                MetaCall.Business.Projects.LogOn(selectedProject);

            }

            OnProjectChanged(new ProjectChangedEventArgs(selectedProject, callJobGroup));

        }

        protected void OnProjectChanged(ProjectChangedEventArgs e)
        {
            if (ProjectChanged != null)
                ProjectChanged(this, e);
        }

        protected void OnNewCustomer(NewCustomerEventArgs e)
        {
            if (NewCustomer != null)
                NewCustomer(this, e);
        }

        protected void OnDurringActivChanged(DurringChangedEventArgs e)
        {
            if (DurringActivChanged != null)
                UpdateUI();
                DurringActivChanged(this, e);
        }

        protected void OnDurringLevelInfoChanged(DurringLevelInfoChangedEventArgs e)
        {
            if (DurringLevelInfoChanged != null)
                DurringLevelInfoChanged(this, e);
        }

        private void Projektanmeldung_Load(object sender, EventArgs e)
        {
            ///Im DesignMode rausstringen, da keine 
            ///Datenzugriffe erfolgen können
            if (DesignMode)
                return;

            BindCenters();
            Application.Idle += new EventHandler(Application_Idle);
            MetaCall.Business.ProjectChanged += new metatop.Applications.metaCall.BusinessLayer.ProjectChangedEventHandler(Business_ProjectChanged);

            this.cboDurringLevel.Visible = false;
            this.labelDurringLevel.Visible = false;

            if (MetaCall.Business.Projects.Current != null)
                InitializeProject(MetaCall.Business.Projects.Current);

        }

        private void InitializeProject(ProjectInfo project)
        {

            if (MetaCall.Business.SponsoringCallManager.User.Center != null)
            {

                foreach (CenterInfo center in this.cboCenter.Items)
                {
                    if (center.CenterId.Equals(MetaCall.Business.SponsoringCallManager.User.Center.CenterId))
                    {
                        this.cboCenter.SelectedItem = center;
                        break;
                    }
                }
            }

            if (MetaCall.Business.SponsoringCallManager.User.Teams.Length > 0)
            {


                foreach (TeamInfo teamInfo in this.cboTeam.Items)
                {
                    if (teamInfo.TeamId.Equals(MetaCall.Business.SponsoringCallManager.User.Teams[0].Team.TeamId))
                    {
                        this.cboTeam.SelectedItem = teamInfo;
                        break;
                    }
                }
            }

            foreach (ProjectInfoWithStatistics projectinfo in this.cboProjekt.Items)
            {
                if (projectinfo.ProjectId.Equals(MetaCall.Business.SponsoringCallManager.Project.ProjectId))
                {
                    this.cboProjekt.SelectedItem = projectinfo;
                    break;
                }
            }

            foreach (CallJobGroupWithStatistics callJobGroup in this.cboCallJobGroups.Items)
            {
                if (callJobGroup.CallJobGroupId.Equals(MetaCall.Business.SponsoringCallManager.CallJobGroup.CallJobGroupId))
                {
                    this.cboCallJobGroups.SelectedItem = callJobGroup;
                    break;
                }
            }
        }

        /// <summary>
        /// Aktualisiert die Statistiken des im SponsoringCallManager gewählten Projekts
        /// </summary>
        /// <param name="manager"></param>
        public void UpdateStatistics(SponsoringCallManager manager)
        {
            if (manager == null)
                return;

            updateComboBoxes = true;
            try
            {
                if (manager.Project != null)
                {
                    for (int i = 0; i < projects.Count; i++)
                    {
                        if (this.projects[i].ProjectId.Equals(manager.Project.ProjectId))
                        {

                            this.projects[i].UpdateStatistics(
                                MetaCall.Business.Users.GetUserInfo(manager.User));

                            this.projects.ResetItem(i);
                            break;
                        }
                    }
                }

                if (manager.CallJobGroup != null)
                {
                    for (int i = 0; i < callJobGroups.Count; i++)
                    {
                        if (this.callJobGroups[i].CallJobGroupId.Equals(manager.CallJobGroup.CallJobGroupId))
                        {
                            this.callJobGroups[i].UpdateStatistics(
                                    MetaCall.Business.Users.GetUserInfo(manager.User));


                            this.callJobGroups.ResetItem(i);
                            break;
                        }
                    }
                }
            }
            finally
            {

                updateComboBoxes = false;
            }
        }

        private void btnNewCustomer_Click(object sender, EventArgs e)
        {
            ProjectInfoWithStatistics selectedProject = this.cboProjekt.SelectedItem as ProjectInfoWithStatistics;

            OnNewCustomer(new NewCustomerEventArgs(selectedProject.Project));
        }

        private void cboProjekt_SelectedIndexChanged(object sender, EventArgs e)
        {
            ProjectInfoWithStatistics projectInfo = cboProjekt.SelectedItem as ProjectInfoWithStatistics;

            if (!updateComboBoxes)
            {
                BindCallJobGroup(projectInfo);
            }
        }

        private void cboCallJobGroups_SelectionChangeCommitted(object sender, EventArgs e)
        {

            CallJobGroupWithStatistics callJobGroup = this.cboCallJobGroups.SelectedItem as CallJobGroupWithStatistics;
            
            if ((MetaCall.Business.SponsoringCallManager.IsRunning) && 
                (callJobGroup != null) && 
                (!updateComboBoxes))
            {
                MetaCall.Business.SponsoringCallManager.RestartWithNewCallJobGroup(callJobGroup.CallJobGroup);
            }
        }

        private void cboProjekt_Format(object sender, ListControlConvertEventArgs e)
        {
            ProjectInfo projectInfo = e.ListItem as ProjectInfo;

            if (projectInfo != null)
            {
                int resultType = 0;

                if (System.Threading.Thread.CurrentPrincipal.IsInRole(MetaCallPrincipal.AdminRoleName) || System.Threading.Thread.CurrentPrincipal.IsInRole(MetaCallPrincipal.CenterAdminRoleName))
                {
                    //Überprüfen ob aktueller User Admin oder Centerleiter isr
                    //Wenn ja werden alle Daten ausgewertet wenn nicht nur die persönlichen
                    resultType = 1;
                }

                if (MetaCall.Business.Users.CurrentUser != null)
                {
                    CallJobStatistics callJobStatistics = MetaCall.Business.CallJobs.GetStatistics(projectInfo, MetaCall.Business.Users.CurrentUser.UserId, resultType);
                    e.Value = "(" + callJobStatistics.Total + " / " + callJobStatistics.InWork + ") " + projectInfo.Bezeichnung;
                }
            }
        }

        private void cboCallJobGroups_Format(object sender, ListControlConvertEventArgs e)
        {
            CallJobGroupInfo callJobGroup = e.ListItem as CallJobGroupInfo;

            if (callJobGroup != null)
            {
                int resultType = 0;

                if (System.Threading.Thread.CurrentPrincipal.IsInRole(MetaCallPrincipal.AdminRoleName) || System.Threading.Thread.CurrentPrincipal.IsInRole(MetaCallPrincipal.CenterAdminRoleName))
                {
                    //Überprüfen ob aktueller User Admin oder Centerleiter isr
                    //Wenn ja werden alle Daten ausgewertet wenn nicht nur die persönlichen
                    resultType = 1;
                }

                if (MetaCall.Business.Users.CurrentUser != null)
                {
                    CallJobStatistics callJobStatistics = MetaCall.Business.CallJobs.GetStatistics(callJobGroup, MetaCall.Business.Users.CurrentUser.UserId, resultType);
                    e.Value = "(" + callJobStatistics.Total + " / " + callJobStatistics.InWork + ") " + callJobGroup.DisplayName;
                }
            }
        }

        private class ProjectInfoWithStatistics
        {
            private CallJobStatistics statistics;
            private ProjectInfo project;

            public ProjectInfoWithStatistics(ProjectInfo project)
            {
                this.project = project;
                UpdateStatistics(MetaCall.Business.Users.GetUserInfo(MetaCall.Business.Users.CurrentUser));
            }

            public ProjectInfo Project
            {
                get { return project; }
            }

            public CallJobStatistics Statistics
            {
                get { return this.statistics; }
            }

            public void UpdateStatistics(UserInfo user)
            {

                //Ergebnistyp bestimmen
                // 0 = nur Zahlen für den aktuellen User 
                // 1 = Gesamtzahlen
                int resultType =
                    System.Threading.Thread.CurrentPrincipal.IsInRole(MetaCallPrincipal.AdminRoleName) ||
                    System.Threading.Thread.CurrentPrincipal.IsInRole(MetaCallPrincipal.CenterAdminRoleName) ? 1 : 0;
                
                CallJobStatistics stats =
                    MetaCall.Business.CallJobs.GetStatistics(project, user.UserId, resultType);


                this.statistics = stats;

            }

            public override string ToString()
            {

                StringBuilder sb = new StringBuilder();

                if (this.project != null)
                {
                    sb.Append(project.Bezeichnung);

                    if (this.statistics != null)
                        sb.AppendFormat(" ({0}/{1})", this.statistics.Total, this.statistics.InWork);
                }
                else
                {
                    sb.Append("no calljobGroup loaded");
                }


                return sb.ToString();
            }

            public Guid ProjectId
            {
                get
                {
                    if (this.project != null)
                        return this.project.ProjectId;
                    else
                        return Guid.Empty;
                }
            }
        }

        private class CallJobGroupWithStatistics
        {
            private CallJobStatistics statistics;
            private CallJobGroupInfo callJobGroup;

            public CallJobGroupWithStatistics(CallJobGroupInfo CallJobGroup)
            {
                this.callJobGroup = CallJobGroup;
                UpdateStatistics(MetaCall.Business.Users.GetUserInfo(MetaCall.Business.Users.CurrentUser));
            }

            public CallJobGroupInfo CallJobGroup
            {
                get { return callJobGroup; }
            }

            public CallJobStatistics Statistics
            {
                get { return this.statistics; }
            }

            public void UpdateStatistics(UserInfo user)
            {

                //Ergebnistyp bestimmen
                // 0 = nur Zahlen für den aktuellen User 
                // 1 = Gesamtzahlen
                int resultType =
                    System.Threading.Thread.CurrentPrincipal.IsInRole(MetaCallPrincipal.AdminRoleName) ||
                    System.Threading.Thread.CurrentPrincipal.IsInRole(MetaCallPrincipal.CenterAdminRoleName) ? 1 : 0;

                CallJobStatistics stats =
                    MetaCall.Business.CallJobs.GetStatistics(callJobGroup, user.UserId, resultType);


                this.statistics = stats;

            }

            public override string ToString()
            {

                StringBuilder sb = new StringBuilder();

                if (this.callJobGroup != null)
                {
                    sb.Append(callJobGroup.DisplayName);

                    if (this.statistics != null)
                        sb.AppendFormat(" ({0}/{1})", this.statistics.Total, this.statistics.InWork);
                }
                else
                {
                    sb.Append("no calljobGroup loaded");
                }


                return sb.ToString();
            }

            public Guid CallJobGroupId
            {
                get
                {
                    if (this.callJobGroup != null)
                        return this.callJobGroup.CallJobGroupId;
                    else
                        return Guid.Empty;
                }
            }
        }

        private void buttonDurring_Click(object sender, EventArgs e)
        {
            /*
               Der Button hat eine Doppelfunktion. Er schaltet entweder die Mahnungen ein oder aus, 
               bzw. die Projektanmeldung wieder an oder aus.
             
            */

            if (MetaCall.Business.CallJobs.DurringActiv == true)
            {
                // Mahnen deaktivieren
                MetaCall.Business.CallJobs.DurringActiv = false;
                MetaCall.Business.CallJobs.Mahnstufe2 = -1;
                this.labelDurringLevel.Visible = false;
                this.cboDurringLevel.Visible = false;
                MetaCall.Business.SponsoringCallManager.StopDurring();

            }
            else
            {
                // Mahnen aktivieren
                MetaCall.Business.SponsoringCallManager.StartDurring(MetaCall.Business.Users.CurrentUser);
                BindDurringLevels();
                MetaCall.Business.CallJobs.DurringActiv = true;

                this.labelDurringLevel.Visible = true;
                this.cboDurringLevel.Visible = true;
            }
            OnDurringActivChanged(new DurringChangedEventArgs(MetaCall.Business.CallJobs.DurringActiv));
        }

        private void cboDurringLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            //"<Alle Mahnungen>", "<neue Mahnungen>", "<bearbeitete Mahnungen>", "<Wiedervorlagen>"
            switch (this.cboDurringLevel.SelectedItem.ToString())
            {
                case "<Alle Mahnungen>":
                    MetaCall.Business.CallJobs.Mahnstufe2 = 0; 
                    break;
                case "<neue Mahnungen>":
                    MetaCall.Business.CallJobs.Mahnstufe2 = 1;
                    break;
                case "<bearbeitete Mahnungen>":
                    MetaCall.Business.CallJobs.Mahnstufe2 = 2;
                    break;
                case "<Wiedervorlagen>":
                    MetaCall.Business.CallJobs.Mahnstufe2 = 3;
                    break;
                default:
                    MetaCall.Business.CallJobs.Mahnstufe2 = 0;
                    break;      

            }
            

            OnDurringLevelInfoChanged(new DurringLevelInfoChangedEventArgs(MetaCall.Business.CallJobs.Mahnstufe2));

        }
    }
}
