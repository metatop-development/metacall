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
    [ModulIndex(999)]
    public partial class Wiedervorlageverwaltung : UserControl, IModulMainControl
    {
        private MetaCallPrincipal principal;

        private List<UserInfo> userInfos = new List<UserInfo>();
        private List<TeamInfo> teamInfos = new List<TeamInfo>();

        private enum SearchType
        {
            User,
            Project
        }

        private SearchType searchTypeSelected;

        private SearchType SearchTypeSelected
        {
            set 
            {
                if (searchTypeSelected != value)
                {
                    this.searchTypeSelected = value;
                    LoadFilterTreeView();
                    if (this.searchTypeSelected == SearchType.Project)
                    {
                        this.toolStripButtonProject.Enabled = false;
                        this.toolStripButtonAgent.Enabled = true;
                    }
                    else
                    {
                        this.toolStripButtonProject.Enabled = true;
                        this.toolStripButtonAgent.Enabled = false;
                    }
                }
                else
                {
                    searchTypeSelected = value;
                }
            }
            get { return searchTypeSelected; }
        }

        public Wiedervorlageverwaltung()
        {
            InitializeComponent();
            
            this.principal = System.Threading.Thread.CurrentPrincipal as MetaCallPrincipal;

            if (this.principal.IsInRole(MetaCallPrincipal.AdminRoleName))
            {
                //da nur Admins über kontextmenü User ändern dürfen erfolgt nur hier die Zuweisung
                this.userInfos = MetaCall.Business.Users.Users;
                //ebenso teams
                this.teamInfos = MetaCall.Business.Teams.Teams;
            }
            else
            {
                this.userInfos = null;
                this.teamInfos = null;
            }
            SearchTypeSelected = SearchType.Project;
            treeViewFilter_AfterSelect(null, null);
        }

        private void LoadFilterTreeView()
        {
            if (this.principal == null)
                throw new InvalidOperationException("CurrentPrincipal is not a type of metaCallPrincipal");

            List<CenterInfo> centers = new List<CenterInfo>();

            if (this.principal.IsInRole(MetaCallPrincipal.AdminRoleName))
            {
                centers = MetaCall.Business.Centers.Centers;
            }
            else
            {
                if (MetaCall.Business.Users.CurrentUser != null)
                    centers.Add(MetaCall.Business.Users.CurrentUser.Center);
            }

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

        private TeamTreeNode AddTeamNode(TeamInfo team)
        {
            TeamTreeNode teamNode = new TeamTreeNode(team);
            if (SearchTypeSelected == SearchType.User)
            {
                UserInfoTreeNode[] userNodes = CreateUserNodes(team);
                teamNode.Nodes.AddRange(userNodes);
            }
            else
            {
                ProjectInfoTreeNode[] projectNodes = CreateProjectNodes(team);
                teamNode.Nodes.AddRange(projectNodes);
            }
            return teamNode;
        }

        private CenterTreeNode AddCenterNode(CenterInfo center)
        {
            CenterTreeNode centerNode = new CenterTreeNode(center);
            if (searchTypeSelected == SearchType.Project)
            {
                ProjectInfoTreeNode[] projectNodes = CreateProjectNodes(center);
                centerNode.Nodes.AddRange(projectNodes);
            }
            else if (searchTypeSelected == SearchType.User)
            {
                TeamTreeNode[] teamNodes = CreateTeamNodes(center);
                centerNode.Nodes.AddRange(teamNodes);
            }
            return centerNode;
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
                    projectNodes[i].ContextMenuStrip = this.teamNodeContextMenuStrip;
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

        private ProjectInfoTreeNode[] CreateProjectNodes(CenterInfo center)
        {
            if (center == null)
                throw new ArgumentNullException("Center");


            try
            {
                List<ProjectInfo> project;
                if (this.principal.IsInRole(MetaCallPrincipal.AdminRoleName))
                {
                    project = MetaCall.Business.Projects.GetByCenter(center);
                }
                else
                {
                    TeamAssignInfo[] teams = this.principal.Identity.User.Teams;
                    TeamInfo teamInfo = new TeamInfo();
                    if (teams.Length > 0)
                    {
                        teamInfo = teams[0].Team;
                        project = MetaCall.Business.Projects.GetByTeam(teamInfo);
                    }
                    else
                    {
                        project = null;
                    }
                }

                ProjectInfoTreeNode[] projectNodes = new ProjectInfoTreeNode[project.Count];
                
                for (int i = 0; i < project.Count; i++)
                {
                    projectNodes[i] = new ProjectInfoTreeNode(project[i]);
                    projectNodes[i].ContextMenuStrip = this.teamNodeContextMenuStrip;
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

        private UserInfoTreeNode[] CreateUserNodes(TeamInfo team)
        {
            if (team == null)
                throw new ArgumentNullException("Team");

            try
            {
                List<UserInfo> user = new List<UserInfo>();

                if (this.principal.IsInRole(MetaCallPrincipal.AdminRoleName))
                {
                    user = MetaCall.Business.Teams.GetUsers(team);
                }
                else
                {
                    user.Add(MetaCall.Business.Users.GetUserInfo(MetaCall.Business.Users.CurrentUser));
                }

                UserInfoTreeNode[] userNodes = new UserInfoTreeNode[user.Count];

                for (int i = 0; i < user.Count; i++)
                {
                    userNodes[i] = new UserInfoTreeNode(user[i]);
                    userNodes[i].ContextMenuStrip = this.teamNodeContextMenuStrip;

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


        /// <summary>
        /// Liefert ein Array mit TeamInfoTreeNodes für das angegebenen Center
        /// </summary>
        /// <param name="center"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"/>
        private TeamTreeNode[] CreateTeamNodes(CenterInfo center)
        {
            if (center == null)
                throw new ArgumentNullException("center");


            try
            {
                List<TeamInfo> teams = new List<TeamInfo>();

                if (this.principal.IsInRole(MetaCallPrincipal.AdminRoleName))
                {
                    teams = MetaCall.Business.Teams.GetByCenter(center);
                }
                else
                {
                    teams = MetaCall.Business.Teams.GetTeamsByUser(MetaCall.Business.Users.CurrentUser);
                }
                TeamTreeNode[] teamNodes = new TeamTreeNode[teams.Count];

                for (int i = 0; i < teams.Count; i++)
                {
                    teamNodes[i] = AddTeamNode(teams[i]);
                    teamNodes[i].ContextMenuStrip = this.teamNodeContextMenuStrip;
                }

                return teamNodes;
            }
            catch(Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "UI Policy");
                if (rethrow)
                    throw;
                else
                    return new TeamTreeNode[0];
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
        }

        #region IModulMainControl Member

        public event ModulInfoMessageHandler StatusMessage;

        public event QueryPermissionHandler QueryPermisson;

        public event ModuleStateChangedHandler StateChanged;

        public void Initialize(IModulMainControl caller)
        {
            //throw new Exception("The method or operation is not implemented.");
        }

        public void UnloadModul(out bool canUnload)
        {
            //throw new Exception("The method or operation is not implemented.");
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
                return new StartUpMenuItem("Wiedervorlage", "Verwaltung");
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

        private void Wiedervorlageverwaltung_Load(object sender, EventArgs e)
        {

            if (DesignMode)
                return;

        }

        private ReminderViewInfo GetReminderViewInfoControl()
        {
            TreeNode node = treeViewFilter.SelectedNode;
            ReminderViewInfo rviView;
            if (node == null)
            {
                rviView = (ReminderViewInfo)Activator.CreateInstance(typeof(ReminderViewInfo), new Object[] { this.panel1.Width, this.panel1.Height });
            }
            else if (node.GetType() == typeof(TeamTreeNode))
            {
                //MessageBox.Show(((TeamTreeNode)node).Team.Bezeichnung);
                TeamInfo tInfo = (TeamInfo)((TeamTreeNode)node).Team;
                rviView = (ReminderViewInfo)Activator.CreateInstance(typeof(ReminderViewInfo), new Object[] { tInfo ,this.panel1.Width, this.panel1.Height });
            }
            else if (node.GetType() == typeof(ProjectInfoTreeNode))
            {
                if (this.principal.IsInRole(MetaCallPrincipal.AdminRoleName))
                {
                    rviView = (ReminderViewInfo)Activator.CreateInstance(typeof(ReminderViewInfo), new Object[] { ((ProjectInfoTreeNode)node).ProjectInfo, this.panel1.Width, this.panel1.Height });
                }
                else
                {
                    TeamAssignInfo[] teams = this.principal.Identity.User.Teams;
                    TeamInfo teamInfo = new TeamInfo();
                    UserInfo userInfo = MetaCall.Business.Users.GetUserInfo(MetaCall.Business.Users.CurrentUser);
                    if (teams.Length > 0)
                    {
                        teamInfo = teams[0].Team;
                    }

                    //TeamTreeNode teamInfoTreeNode = (TeamTreeNode)node.Parent;
                    rviView = (ReminderViewInfo)Activator.CreateInstance(typeof(ReminderViewInfo), new Object[] { userInfo, ((ProjectInfoTreeNode)node).ProjectInfo, teamInfo, this.panel1.Width, this.panel1.Height });
                }
            }
            else if (node.GetType() == typeof(UserInfoTreeNode))
            {
                rviView = (ReminderViewInfo)Activator.CreateInstance(typeof(ReminderViewInfo), new Object[] { ((UserInfoTreeNode)node).User, null, this.panel1.Width, this.panel1.Height });
            }
            else
            {
                rviView = (ReminderViewInfo)Activator.CreateInstance(typeof(ReminderViewInfo), new Object[] { this.panel1.Width, this.panel1.Height });
            }

            return rviView;
        }

        private void treeViewFilter_AfterSelect(object sender, TreeViewEventArgs e)
        {
            ReminderViewInfo rviView = GetReminderViewInfoControl();

            this.panel1.SuspendLayout();
            this.panel1.Controls.Clear();
            
            if (rviView != null)
            {
                this.panel1.Controls.Add(rviView);
                rviView.Visible = true;
                rviView.Dock = DockStyle.Fill;

                this.panel1.ResumeLayout();
            }
            
        }

        #region IModulMainControl Member


        public bool CanAccessModul(System.Security.Principal.IPrincipal principal)
        {
            if (this.principal.IsInRole(MetaCallPrincipal.AdminRoleName) || MetaCall.Business.Users.CurrentUser.ReminderEditPermit == true)
                return true;
            else
                return false;
        }

        #endregion

        private enum ChangeType
        {
            Agent,
            Project,
            Team
        }

        private void teamNodeContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            //Ein-/Ausschalten von Menüpunkten wenn User --> Admin ist
            if (this.principal.IsInRole(MetaCallPrincipal.AdminRoleName))
            {
                Point mouse = this.treeViewFilter.PointToClient(MousePosition);

                TeamTreeNode teamNode = this.treeViewFilter.GetNodeAt(mouse) as TeamTreeNode;

                this.teamNodeContextMenuStrip.Items.Clear();

                if (teamNode != null)
                {

                    this.teamNodeContextMenuStrip.Items.Add(GetUsersToolStripItem());
                    this.teamNodeContextMenuStrip.Items.Add(GetTeamsToolStripItem());

                    return;
                }

                ProjectInfoTreeNode projectNode = this.treeViewFilter.GetNodeAt(mouse) as ProjectInfoTreeNode;
                if (projectNode != null)
                {
                    this.teamNodeContextMenuStrip.Items.Add(GetUsersToolStripItem());
                    this.teamNodeContextMenuStrip.Items.Add(GetTeamsToolStripItem());

                    return;
                }

                UserInfoTreeNode userNode = this.treeViewFilter.GetNodeAt(mouse) as UserInfoTreeNode;
                if (userNode != null)
                {
                    this.teamNodeContextMenuStrip.Items.Add(GetUsersToolStripItem());
                    this.teamNodeContextMenuStrip.Items.Add(GetTeamsToolStripItem());

                    return;
                }
            }
        }

        private void treeViewFilter_MouseDown(object sender, MouseEventArgs e)
        {
            //Aktuellen Knoten selectieren wenn rechte Maustaste -> erforderlich damit 
            //anschließend das ToolStripMenuItem über selectedNode den aktuellen knoten abfragen kann
            if (e.Button == MouseButtons.Right)
            {
                TreeNode node = this.treeViewFilter.GetNodeAt(e.Location);

                if (node != null)
                {
                    this.treeViewFilter.SelectedNode = node;
                }
            }
       }

        private void RefreshTeamNode(TeamTreeNode teamNode, ProjectInfo projectToSelect)
        {
            if (teamNode == null)
                return;

            this.treeViewFilter.SuspendLayout();

            teamNode.Nodes.Clear();
            teamNode.Nodes.AddRange(CreateProjectNodes(teamNode.Team));

            if (projectToSelect != null)
            {
                foreach (ProjectInfoTreeNode node in teamNode.Nodes)
                {
                    if (node.ProjectInfo.ProjectId.Equals(projectToSelect.ProjectId))
                    {
                        this.treeViewFilter.SelectedNode = node;
                        this.treeViewFilter.Refresh();
                        break;
                    }
                }
            }

            this.treeViewFilter.ResumeLayout();
        }


        /// <summary>
        /// Aktualisiert einen einzelnen CenterKnoten und lädt die 
        /// zugehörigen Teams nach
        /// </summary>
        /// <param name="centerNode"></param>
        /// <param name="teamToSelect"></param>
        private void RefreshCenterNode(CenterTreeNode centerNode, TeamInfo teamToSelect)
        {
            if (centerNode == null)
                return;

            this.treeViewFilter.SuspendLayout();

            centerNode.Nodes.Clear();
            centerNode.Nodes.AddRange(CreateTeamNodes(centerNode.Center));

            //gewünschtes team als aktuellen Knoten auswählen
            if (teamToSelect != null)
            {
                foreach (TeamInfoTreeNode node in centerNode.Nodes)
                {
                    if (node.Team.TeamId.Equals(teamToSelect.TeamId))
                    {
                        this.treeViewFilter.SelectedNode = node;
                        this.treeViewFilter.Refresh();
                        break;
                    }
                }
            }

            this.treeViewFilter.ResumeLayout();
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

            return teamSelection;
        }


        private ToolStripItem GetUsersToolStripItem()
        {
            ToolStripMenuItem userSelection = new ToolStripMenuItem("Agent");
            userSelection.AutoSize = true;

            ToolStripMenuItem userMenuItem = new ToolStripMenuItem();

            //List<UserInfo> userInfos = MetaCall.Business.Users.Users;

            foreach (UserInfo userInfo in this.userInfos)
            {
                userMenuItem = new ToolStripMenuItem(userInfo.DisplayName);
                userMenuItem.Tag = userInfo;
                userMenuItem.CheckOnClick = false;
                userMenuItem.Click += new EventHandler(userMenuItem_Click);

                userSelection.DropDownItems.Add(userMenuItem);
            }

            return userSelection;
        }

        private enum EvokeUpdateTyp
        {
            nothing,
            Project,
            User,
            Team
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

            if (ctl is TreeView)
            {
                TreeNode selectedNode = ((TreeView)ctl).SelectedNode;

                if (selectedNode is UserInfoTreeNode)
                {
                    UserInfoTreeNode uitNode = (UserInfoTreeNode)selectedNode;
                    evokeUserInfo = uitNode.User;
                    msg = string.Format("Möchten Sie die Wiedervorlagen von {0} dem Agent {1} zuordnen?", evokeUserInfo.DisplayName.ToString(), selectedUserInfo.DisplayName.ToString());

                    evokeUpdateTyp = EvokeUpdateTyp.User;
                }
                else if (selectedNode is ProjectInfoTreeNode)
                {
                    ProjectInfoTreeNode pitNode = (ProjectInfoTreeNode)selectedNode;
                    evokeProjectInfo = pitNode.ProjectInfo;
                    msg = string.Format("Möchten Sie die Wiedervorlagen von dem Projekt '{0}' dem Agent '{1}' zuordnen?", evokeProjectInfo.Bezeichnung.ToString(), selectedUserInfo.DisplayName.ToString());
                    evokeUpdateTyp = EvokeUpdateTyp.Project;
                }
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

            if (ctl is TreeView)
            {
                TreeNode selectedNode = ((TreeView)ctl).SelectedNode;

                if (selectedNode is UserInfoTreeNode)
                {
                    UserInfoTreeNode uitNode = (UserInfoTreeNode)selectedNode;
                    evokeUserInfo = uitNode.User;
                    msg = string.Format("Möchten Sie die Wiedervorlagen von {0} dem Team {1} zuordnen?", evokeUserInfo.DisplayName.ToString(), selectedTeamInfo.Bezeichnung.ToString());

                    evokeUpdateTyp = EvokeUpdateTyp.User;
                    //Console.WriteLine(selectedNode.GetType().ToString());
                }
                else if (selectedNode is ProjectInfoTreeNode)
                {
                    ProjectInfoTreeNode pitNode = (ProjectInfoTreeNode)selectedNode;
                    evokeProjectInfo = pitNode.ProjectInfo;
                    msg = string.Format("Möchten Sie die Wiedervorlagen von dem Projekt '{0}' dem Team '{1}' zuordnen?", evokeProjectInfo.Bezeichnung.ToString(), selectedTeamInfo.Bezeichnung.ToString());
                    evokeUpdateTyp = EvokeUpdateTyp.Project;
                }
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
                    }
                }
            }
        }

        private void toolStripButtonProject_Click(object sender, EventArgs e)
        {
            SearchTypeSelected = SearchType.Project;            
        }

        private void toolStripButtonAgent_Click(object sender, EventArgs e)
        {
            SearchTypeSelected = SearchType.User;
        }

    }
}
