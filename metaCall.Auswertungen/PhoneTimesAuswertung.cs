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

using CrystalDecisions.CrystalReports.Engine;

namespace metatop.Applications.metaCall.WinForms.Modules
{
    [ModulIndex(6)]
    public partial class PhoneTimesAuswertung : UserControl, IModulMainControl
    {
        private MetaCallPrincipal principal;
        private List<PhoneTimesReport> result;
        
        public PhoneTimesAuswertung()
        {
            InitializeComponent();
            this.principal = System.Threading.Thread.CurrentPrincipal as MetaCallPrincipal;
            LoadFilterTreeView();
            this.selectionPeriodStartStop.SelectionFrom = DateTime.Today;
            this.selectionPeriodStartStop.SelectionTo = DateTime.Today;
            this.selectionPeriodStartStop.SelectionChanged += new SelectionPeriodChangedEventHandler(selectionPeriodStartStop_SelectionChanged);
        }

        void selectionPeriodStartStop_SelectionChanged(SelectionPeriodEventArgs e)
        {
            //
            LoadReport();
        }

        #region IModulMainControl Member

        public bool CanAccessModul(System.Security.Principal.IPrincipal principal)
        {
            if (this.principal.IsInRole(MetaCallPrincipal.AdminRoleName))
                return true;
            else
                return false;
        }

        public bool CanPauseApplication
        {
            get { return true; }
        }

        public ToolStripMenuItem[] CreateMainMenuItems()
        {
            return null;
        }

        public ToolStrip CreateToolStrip()
        {
            return null;
        }

        public void Initialize(IModulMainControl caller)
        {
        }

        public event QueryPermissionHandler QueryPermisson;

        public event ModuleStateChangedHandler StateChanged;

        public event ModulInfoMessageHandler StatusMessage;

        public void UnloadModul(out bool canUnload)
        {
            canUnload = true;
        }

        #endregion

        private class Configuration : ModulConfigBase
        {
            public override StartUpMenuItem GetStartUpMenuItem()
            {
                return new StartUpMenuItem("Telefonzeiten", "Auswertungen");
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

        private void LoadFilterTreeView()
        {
            if (this.principal == null)
                throw new InvalidOperationException("CurrentPrincipal is not a type of metaCallPrincipal");

            if (this.principal.IsInRole(MetaCallPrincipal.AdminRoleName))
            {
                List<CenterInfo> centers = MetaCall.Business.Centers.Centers;

                treeViewFilter.BeginUpdate();

                if (treeViewFilter.Nodes.Count > 0)
                    treeViewFilter.Nodes.Clear();

                foreach (CenterInfo center in centers)
                {
                    TreeNode centerNode = AddCenterNode(center);

                    treeViewFilter.Nodes.Add(centerNode);
                }
                treeViewFilter.CollapseAll();

                if (treeViewFilter.Nodes.Count > 0)
                    treeViewFilter.SelectedNode = treeViewFilter.Nodes[0];

                treeViewFilter.EndUpdate();
            }
        }

        private CenterTreeNode AddCenterNode(CenterInfo center)
        {
            TeamInfoTreeNode[] teamNodes = CreateTeamNodes(center);
            CenterTreeNode centerNode = new CenterTreeNode(center);
            centerNode.Nodes.AddRange(teamNodes);
            return centerNode;
        }

        /// <summary>
        /// Liefert ein Array mit TeamInfoTreeNodes für das angegebenen Center
        /// </summary>
        /// <param name="center"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"/>
        private TeamInfoTreeNode[] CreateTeamNodes(CenterInfo center)
        {
            if (center == null)
                throw new ArgumentNullException("center");

            try
            {
                List<TeamInfo> teams = MetaCall.Business.Teams.GetByCenter(center);

                TeamInfoTreeNode[] teamNodes = new TeamInfoTreeNode[teams.Count];

                for (int i = 0; i < teams.Count; i++)
                {
                    TeamInfoTreeNode teamNode = new TeamInfoTreeNode(teams[i]);
                    UserInfoTreeNode[] userNodes = CreateUserNodes(teams[i]);
                    teamNode.Nodes.AddRange(userNodes);
                    //ProjectInfoTreeNode[] projectNodes = CreateProjectNodes(teams[i]);
                    //teamNode.Nodes.AddRange(projectNodes);
                    teamNodes[i] = teamNode;
                }

                return teamNodes;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "UI Policy");
                if (rethrow)
                    throw;
                else
                    return new TeamInfoTreeNode[0];
            }
        }

        /// <summary>
        /// Liefert ein Array mit UserInfoTreeNodes für das angegebenen Team
        /// </summary>
        /// <param name="center"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"/>
        private UserInfoTreeNode[] CreateUserNodes(TeamInfo team)
        {
            if (team == null)
                throw new ArgumentNullException("Team");

            try
            {
                List<UserInfo> users = new List<UserInfo>();
                List<TeamMitglied> teamMitglied = MetaCall.Business.Users.GetUsersByTeam(team.TeamId);

                foreach (TeamMitglied tMitglied in teamMitglied)
                {
                    User user = MetaCall.Business.Users.GetUser(tMitglied.UserId);
                    users.Add(MetaCall.Business.Users.GetUserInfo(user));
                }

                UserInfoTreeNode[] userNodes = new UserInfoTreeNode[users.Count];

                for (int i = 0; i < users.Count; i++)
                {
                    UserInfoTreeNode userInfoTreeNode = new UserInfoTreeNode(users[i]);

                    //ProjectInfoTreeNode[] projectInfoTreeNode = CreateProjectNodes(users[i]);
                    //userInfoTreeNode.Nodes.AddRange(projectInfoTreeNode);
                    userNodes[i] = userInfoTreeNode;

                }
                return userNodes;

            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "UI Policy");
                if (rethrow)
                    throw;
                else
                    return new UserInfoTreeNode[0];
            }
        }
        
        private ProjectInfoTreeNode[] CreateProjectNodes(TeamInfo team)
        {
            if (team == null)
                throw new ArgumentNullException("Team");


            try
            {
                List<ProjectInfo> project = MetaCall.Business.Projects.GetByTeam(team);

                ProjectInfoTreeNode[] projectNodes = new ProjectInfoTreeNode[project.Count];

                for (int i = 0; i < project.Count; i++)
                {
                    projectNodes[i] = new ProjectInfoTreeNode(project[i]);
                }

                return projectNodes;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "UI Policy");
                if (rethrow)
                    throw;
                else
                    return new ProjectInfoTreeNode[0];

            }

        }

/*        private CenterTreeNode AddCenterNode(CenterInfo center)
        {
            CenterTreeNode centerNode = new CenterTreeNode(center);
            ProjectInfoTreeNode[] projectNodes = CreateProjectNodes(center);
            centerNode.Nodes.AddRange(projectNodes);
            return centerNode;
        }

        private ProjectInfoTreeNode[] CreateProjectNodes(CenterInfo centerInfo)
        {
            if (centerInfo == null)
                throw new ArgumentNullException("CenterInfo");

            try
            {
                List<ProjectInfo> project = MetaCall.Business.Projects.GetByCenter(centerInfo);

                ProjectInfoTreeNode[] projectNodes = new ProjectInfoTreeNode[project.Count];

                for (int i = 0; i < project.Count; i++)
                {
                    projectNodes[i] = new ProjectInfoTreeNode(project[i]);
                }

                return projectNodes;
            }

            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "UI Policy");
                if (rethrow)
                    throw;
                else
                    return new ProjectInfoTreeNode[0];

            }

        }
*/
        private Guid? SelectedProjectId
        {
            get
            {
                ProjectInfo projectInfo = new ProjectInfo();
                TreeNode node = treeViewFilter.SelectedNode;
                if (node != null)
                {
                    if (node.GetType() == typeof(ProjectInfoTreeNode))
                    {
                        projectInfo = ((ProjectInfoTreeNode)node).ProjectInfo;
                    }
                    else
                    {
                        projectInfo = null;
                    }
                }

                if (projectInfo != null)
                    return projectInfo.ProjectId;
                else
                    return null;
            }
        }

        private Guid? SelectedTeamId
        {
            get
            {
                TeamInfo teamInfo = new TeamInfo();
                TreeNode node = treeViewFilter.SelectedNode;
                if (node != null)
                {
                    if (node.GetType() == typeof(TeamInfoTreeNode))
                    {
                        teamInfo = ((TeamInfoTreeNode)node).Team;
                    }
                    else
                    {
                        teamInfo = null;
                    }
                }

                if (teamInfo != null)
                    return teamInfo.TeamId;
                else
                    return null;
            }
        }

        private Guid? SelectedUserId
        {
            get
            {
                UserInfo userInfo = new UserInfo();
                TreeNode node = treeViewFilter.SelectedNode;
                if (node != null)
                {
                    if (node.GetType() == typeof(UserInfoTreeNode))
                    {
                        userInfo = ((UserInfoTreeNode)node).User;
                    }
                    else
                    {
                        userInfo = null;
                    }
                }

                if (userInfo != null)
                    return userInfo.UserId;
                else
                    return null;
            }
        }


        private void LoadReport()
        {
            crystalReportViewer1.Visible = false;

            if (SelectedTeamId != null | SelectedUserId != null)
            {
                Guid teamId = Guid.Empty;
                    if (SelectedTeamId != null)
                        teamId = (Guid)SelectedTeamId;
                Guid userId = Guid.Empty;
                    if (SelectedUserId != null)
                        userId = (Guid)SelectedUserId;
                
                DateTime start = this.selectionPeriodStartStop.SelectionFrom;
                DateTime stop = this.selectionPeriodStartStop.SelectionTo;

                if (btnAktuell.Checked == true)
                    result = MetaCall.Business.PhoneTimesReport.GetPhoneTimesReport(teamId, userId, start, stop);
                else
                    result = MetaCall.Business.PhoneTimesReport.GetPhoneTimesReport(teamId, userId, start, stop);
            }
            else
            {
                result = null;
            }

            if (result == null || result.Count == 0)
            {
                this.crystalReportViewer1.Visible = false;
                return;
            }

            ReportDocument report = new ReportDocument();
            report.Load(@"PhoneTimesReport.rpt", CrystalDecisions.Shared.OpenReportMethod.OpenReportByTempCopy);

            report.SetDataSource(result);

            this.crystalReportViewer1.ReportSource = report;
            this.crystalReportViewer1.Visible = true;
        }

        private void treeViewFilter_AfterSelect(object sender, TreeViewEventArgs e)
        {
            LoadReport();
        }

        private void btnAktuell_CheckedChanged(object sender, EventArgs e)
        {
            LoadReport();
        }

        private void PhoneTimesAuswertung_Load(object sender, EventArgs e)
        {
            this.selectionPeriodStartStop.SelectionFrom = LastWeekDay();
            this.selectionPeriodStartStop.SelectionTo = LastWeekDay();
        }

        private DateTime LastWeekDay()
        {
            DateTime dt = DateTime.Today.AddDays(-1);

            for (int i = 0; i < 4; i++)
            {
                if (IsWeekDay(dt))
                    return dt;
                else
                    dt.AddDays(-1);
            }

            return dt;
        }

        private bool IsWeekDay(DateTime dateTime)
        {
            return ((dateTime.DayOfWeek != DayOfWeek.Saturday) && (dateTime.DayOfWeek != DayOfWeek.Sunday));
        }

    }
}
