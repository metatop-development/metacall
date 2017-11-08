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
    [ModulIndex(4)]
    public partial class Arbeitszeitübersicht : UserControl, IModulMainControl
    {
        private MetaCallPrincipal principal;
        private List<WorkTimeReportResults> results;

        public Arbeitszeitübersicht()
        {
            InitializeComponent();
            this.principal = System.Threading.Thread.CurrentPrincipal as MetaCallPrincipal;
            this.selectionPeriod1.SelectionChanged += new SelectionPeriodChangedEventHandler(selectionPeriod1_SelectionChanged);
        }

        void selectionPeriod1_SelectionChanged(SelectionPeriodEventArgs e)
        {
            //
            Reload();
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

                UsersWithOutCenterTreeNode usersWithOutCenterTreeNode = new UsersWithOutCenterTreeNode();
                treeViewFilter.Nodes.Add(usersWithOutCenterTreeNode);

                UsersDeletedTreeNode usersDeletedTreeNode = new UsersDeletedTreeNode();
                treeViewFilter.Nodes.Add(usersDeletedTreeNode);

                treeViewFilter.EndUpdate();
            }
            else
            {
                UserInfoTreeNode userInfoTreeNode = CreateUserNodes(MetaCall.Business.Users.CurrentUser);
                treeViewFilter.BeginUpdate();
                treeViewFilter.Nodes.Add(userInfoTreeNode);
                treeViewFilter.EndUpdate();
                treeViewFilter.SelectedNode = userInfoTreeNode;
            }

        }

        private CenterTreeNode AddCenterNode(CenterInfo center)
        {
            TeamInfoTreeNode[] teamNodes = CreateTeamNodes(center);
            CenterTreeNode centerNode = new CenterTreeNode(center);
            centerNode.Nodes.AddRange(teamNodes);
            centerNode.ContextMenuStrip = this.centerNodeContextMenuStrip;
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
                    teamNode.ContextMenuStrip = this.teamNodeContextMenuStrip;

                    teamNodes[i] = teamNode;
                }

                return teamNodes;
            }
            catch(Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "UI Policy");
                if (rethrow)
                    throw;
                else
                    return new TeamInfoTreeNode[0];
            }
        }

        /// <summary>
        /// Liefert ein UserInfoTreeNode für den angegebenen User zurück
        /// </summary>
        /// <param name="center"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"/>
        private UserInfoTreeNode CreateUserNodes(User user)
        {
            if (user == null)
                throw new ArgumentNullException("User");

            UserInfo userInfo = MetaCall.Business.Users.GetUserInfo(user);

            UserInfoTreeNode userNode = new UserInfoTreeNode(userInfo);

            return userNode;
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
                    userNodes[i] = new UserInfoTreeNode(users[i]);
                    //userNodes[i].ContextMenuStrip = this.userNodeContextMenuStrip;
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
            return null;
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
                return new StartUpMenuItem("Arbeitszeitübersicht", "Auswertungen");
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
                get { return true; }
            }
        }

        #region IModulMainControl Member


        public bool CanPauseApplication
        {
            get { return true; }
        }

        #endregion

        private void Arbeitseitübersicht_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            LoadFilterTreeView();
            this.selectionPeriod1.SelectionFrom = DateTime.Now;
            this.selectionPeriod1.SelectionTo = DateTime.Now;
        }

        private void Reload()
        {
            StringBuilder filterText = new StringBuilder();


            Guid? centerId = null;
            Guid? teamId = null;
            Guid? userId = null;
            Guid? projectId = null;

            DateTime from = this.selectionPeriod1.SelectionFrom;
            DateTime to = this.selectionPeriod1.SelectionTo;

            TreeNode node = treeViewFilter.SelectedNode;
            if (node != null)
            {
                if (node.GetType() == typeof(UserInfoTreeNode))
                {
                    userId = ((UserInfoTreeNode)node).User.UserId;
                    centerId = null;
                    teamId = null;
                }

                else if (node.GetType() == typeof(CenterTreeNode))
                {
                    userId = null;
                    centerId = ((CenterTreeNode)node).Center.CenterId;
                    teamId = null;
                }
                else if (node.GetType() == typeof(TeamInfoTreeNode))
                {
                    userId = null;
                    centerId = null;
                    teamId = ((TeamInfoTreeNode)node).Team.TeamId;
                }

                //Abrufen der Daten 
                try
                {

                    results = MetaCall.Business.Users.GetWorkTimeReportResults(
                        centerId, teamId, userId, projectId, from, to);

                    if (results.Count == 0)
                    {
                        /*
                        string msg = "Für den angegebenen Filter existieren keine Daten";
                        MessageBoxOptions options = CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft ?
                            MessageBoxOptions.RtlReading : 0;


                        MessageBox.Show(this, msg, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, options);
                        */

                        this.crystalReportViewer1.Visible = false;
                        return;
                    }
                    filterText.AppendFormat("vom {0:d} bis {1:d}", from, to);

                    ReportDocument report = new ReportDocument();
                    //Bericht instanzieren
                    report = new ReportDocument();
                    //Laden mit der entsprechenden Berichtsdatei
                    report.Load(@"Arbeitszeitübersicht.rpt", CrystalDecisions.Shared.OpenReportMethod.OpenReportByTempCopy);

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
                    this.crystalReportViewer1.Visible = true;
                }

                catch (Exception ex)
                {
                    bool rethrow = ExceptionPolicy.HandleException(ex, "UI Policy");
                    if (rethrow)
                        throw;
                }
            }
        }

        private string GetTeamsString(TeamAssignInfo[] teamInfo)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < teamInfo.Length; i++)
            {
                if (sb.Length > 0) sb.Append("; ");
                sb.Append(teamInfo[i].Team.Bezeichnung);
            }

            return sb.ToString();
        }

        private void treeViewFilter_AfterSelect(object sender, TreeViewEventArgs e)
        {
            Reload();
        }


        #region IModulMainControl Member

        public bool CanAccessModul(System.Security.Principal.IPrincipal principal)
        {
            if (this.principal.IsInRole(MetaCallPrincipal.AdminRoleName) )
                return true;
            else
                return false;

        }

        #endregion

        private void treeViewFilter_MouseDown(object sender, MouseEventArgs e)
        {
            //Aktuellen Knoten selectieren wenn rechte Maustaste -> erforderlich damit 
            //anschließend das ToolStripMenuItem über selöectedNode den aktuellen knoten abfragen kann
            if (e.Button == MouseButtons.Right)
            {
                TreeNode node = this.treeViewFilter.GetNodeAt(e.Location);

                if (node != null)
                {
                    this.treeViewFilter.SelectedNode = node;
                }
            }
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
    }
}
