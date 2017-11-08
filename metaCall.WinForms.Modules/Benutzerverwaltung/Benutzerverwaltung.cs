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
    [ModulIndex(99)]
    public partial class Benutzerverwaltung : UserControl, IModulMainControl
    {
        UserInfoView uiView;

        public Benutzerverwaltung()
        {
            InitializeComponent();
        }

        private void LoadFilterTreeView()
        {
            //Berechtigungen des Benutzers abfragen
            MetaCallPrincipal principal = System.Threading.Thread.CurrentPrincipal as MetaCallPrincipal;

            if (principal == null)
                throw new InvalidOperationException("CurrentPrincipal is not a type of metaCallPrincipal");

            List<CenterInfo> centers = MetaCall.Business.Centers.Centers;

            treeViewFilter.BeginUpdate();

            if (treeViewFilter.Nodes.Count > 0)
                treeViewFilter.Nodes.Clear();

            foreach (CenterInfo center in centers)
            {
                TreeNode centerNode = AddCenterNode(center);

                treeViewFilter.Nodes.Add(centerNode);
            }
            treeViewFilter.ExpandAll();

            if (treeViewFilter.Nodes.Count > 0)
                treeViewFilter.SelectedNode = treeViewFilter.Nodes[0];


            // Wen der aktuelle User noch Administrator ist, darf er Benutzer sehen, die 
            // keinem Center zugeordnet sind.
            UsersWithOutCenterTreeNode usersWithOutCenterTreeNode = new UsersWithOutCenterTreeNode();
            treeViewFilter.Nodes.Add(usersWithOutCenterTreeNode);

            UsersDeletedTreeNode usersDeletedTreeNode = new UsersDeletedTreeNode();
            treeViewFilter.Nodes.Add(usersDeletedTreeNode);

            treeViewFilter.EndUpdate();

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
                    teamNodes[i] = new TeamInfoTreeNode(teams[i]);
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
                    return new TeamInfoTreeNode[0];
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
                return new StartUpMenuItem("Benutzer", "Verwaltung");
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

        private void Benutzerverwaltung_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            LoadFilterTreeView();
        }

        private void Reload()
        {
            uiView = GetUserInfoViewControl();

            this.panelUserInfo.SuspendLayout();
            this.panelUserInfo.Controls.Clear();

            if (uiView != null)
            {
                this.panelUserInfo.Controls.Add(uiView);
                uiView.Visible = true;
                uiView.Dock = DockStyle.Fill;
                this.panelUserInfo.ResumeLayout();
            }
        }

        private UserInfoView GetUserInfoViewControl()
        {
            UserInfoView uiView;

            TreeNode node = treeViewFilter.SelectedNode;

            if (node.GetType() == typeof(CenterTreeNode))
            {
                //          users = MetaCall.Business.Centers.GetUsers(((CenterTreeNode)node).Center);
                Center center = MetaCall.Business.Centers.GetCenter(((CenterTreeNode)node).Center.CenterId);
                uiView = (UserInfoView)Activator.CreateInstance(typeof(UserInfoView), new Object[] { center });

            }
            else if (node.GetType() == typeof(TeamInfoTreeNode))
            {
                //            users = MetaCall.Business.Teams.GetUsers(((TeamInfoTreeNode)node).Team);
                Team team = MetaCall.Business.Teams.GetTeam(((TeamInfoTreeNode)node).Team);
                uiView = (UserInfoView)Activator.CreateInstance(typeof(UserInfoView), new Object[] { team });
            }
            else if (node.GetType() == typeof(UsersWithOutCenterTreeNode))
            {
                //              users = MetaCall.Business.Users.UsersWithOutCenter;

                UserTyp uTyp = UserTyp.WithoutCenter;
                uiView = (UserInfoView)Activator.CreateInstance(typeof(UserInfoView), new Object[] { uTyp });
            }
            else if (node.GetType() == typeof(UsersDeletedTreeNode))
            {
                //                users = MetaCall.Business.Users.UsersDeleted;
                UserTyp uTyp = UserTyp.Deleted;
                uiView = (UserInfoView)Activator.CreateInstance(typeof(UserInfoView), new Object[] { uTyp });
            }
            else
            {
                uiView = null;
            }

            return uiView;
        }

        private string GetRolesString(SecurityGroup[] securityGroup)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < securityGroup.Length; i++)
            {
                if (sb.Length > 0) sb.Append("; ");
                sb.Append(securityGroup[i].DisplayName);
            }

            return sb.ToString();
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

        private void UserNew()
        {
            User newUser = new User();
            newUser.UserId = Guid.NewGuid();
            newUser.SecurityGroups = new SecurityGroup[0];
            newUser.Teams = new TeamAssignInfo[0];

            using (UserForm dlg = new UserForm(newUser))
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string password = AskForPassword();
                        if (!string.IsNullOrEmpty(password))
                        {
                            MetaCall.Business.Users.Create(newUser, password);
                        }
                        else
                        {
                            MessageBox.Show("Der Benutzer konnte nicht erstellt werden, da Sie kein Passwort eingegeben haben!");
                        }
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

        private string AskForPassword()
        {
            using (EnterPasswordForNewUser dlg = new EnterPasswordForNewUser())
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    return dlg.Password;
                }
            }
            return null;
        }

        #region IModulMainControl Member

        public bool CanAccessModul(System.Security.Principal.IPrincipal principal)
        {
            return principal.IsInRole(MetaCallPrincipal.AdminRoleName);
        }

        #endregion

        private void teamNodeContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            //Ein-/Ausschalten von Menüpunkten
            Point mouse = this.treeViewFilter.PointToClient(MousePosition);

            TeamInfoTreeNode teamNode = this.treeViewFilter.GetNodeAt(mouse) as TeamInfoTreeNode;

            if (teamNode == null)
            {
                e.Cancel = true;
                return;
            }

            //TeamInfo team = teamNode.Team;
            //abrufen des kompletten Teams
            Team team = null;
            try
            {
                team = MetaCall.Business.Teams.GetTeam(teamNode.Team);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "UI Policy");
                //if (rethrow)
                //    throw;
                return;
            }
        }

        private void teamAddMenueToolStripItem_Click(object sender, EventArgs e)
        {
            TeamInfoTreeNode teamNode;
            CenterTreeNode centerNode;

            if (this.treeViewFilter.SelectedNode == null)
                return;

            teamNode = this.treeViewFilter.SelectedNode as TeamInfoTreeNode;
            if (teamNode == null)
                centerNode = this.treeViewFilter.SelectedNode as CenterTreeNode;
            else
                centerNode = teamNode.Parent as CenterTreeNode;

            if (centerNode == null)
                return;

            //Team erstellen
            Team team = TeamNew(centerNode.Center);
            TeamInfo teamInfo;

            if (team == null)
                return;

            try
            {
                teamInfo = MetaCall.Business.Teams.GetTeamInfo(team);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "UI Policy");
                if (rethrow)
                    throw;
                else
                    return;
            }

            //TeamNodes für dieses Center laden
            RefreshCenterNode(centerNode, teamInfo);
        }

        private Team TeamNew(CenterInfo center)
        {
            //neues team erstellen
            Team team = new Team();
            team.TeamId = Guid.NewGuid();
            team.Bezeichnung = "neues Team";
            team.Beschreibung= null;
            team.Center = center;
            team.Projects = new ProjectInfo[0];
            team.TeamMitglieder = new TeamMitglied[0];

            using (TeamEdit teamEditDlg = new TeamEdit(team))
            {
                if (teamEditDlg.ShowDialog(this) == DialogResult.OK)
                {
                    //Team auf der Datenbank erstellen
                    try
                    {
                        MetaCall.Business.Teams.Create(team);
                        return team;
                    }
                    catch (Exception ex)
                    {
                        bool rethrow = ExceptionPolicy.HandleException(ex, "UI Policy");
                        if (rethrow)
                            throw;
                        else
                            return null;
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        private void TeamUpdate(Team team)
        {
            using (TeamEdit teamEditDlg = new TeamEdit(team))
            {
                if (teamEditDlg.ShowDialog(this) == DialogResult.OK)
                {
                    try
                    {
                        MetaCall.Business.Teams.Update(team);
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

        private void teamEditToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TeamInfoTreeNode teamNode;

            if (this.treeViewFilter.SelectedNode == null)
                return;

            teamNode = this.treeViewFilter.SelectedNode as TeamInfoTreeNode;
            if (teamNode == null)
                return;
            
            ///Team vom Server abrufen
            Team team = MetaCall.Business.Teams.GetTeam(teamNode.Team);

            TeamUpdate(team);

            //TeamNodes für dieses Center laden
            CenterTreeNode centerNode = teamNode.Parent as CenterTreeNode;
            RefreshCenterNode(centerNode, teamNode.Team);
        }

        private void centerEditToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CenterTreeNode centerNode = this.treeViewFilter.SelectedNode as CenterTreeNode;

            if (centerNode == null)
                return;

            CenterInfo center = centerNode.Center;

            CenterUpdate(center);
        }

        private void CenterUpdate(CenterInfo centerinfo)
        {
            Center center = MetaCall.Business.Centers.GetCenter(centerinfo.CenterId);
            
            using (CenterEdit centerEditDlg = new CenterEdit(center, false))
            {
                if (centerEditDlg.ShowDialog(this) == DialogResult.OK)
                {
                    try
                    {
                        MetaCall.Business.Centers.Update(center);
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

        private Center CenterNew()
        {
            Center center = new Center();
            center.CenterId = Guid.NewGuid();
            center.Bezeichnung = "neues Center";
            center.Administratoren = new UserInfo[0];

            using (CenterEdit centerEditDlg = new CenterEdit(center, true))
            {
                if (centerEditDlg.ShowDialog(this) == DialogResult.OK)
                {
                    try
                    {
                        MetaCall.Business.Centers.Create(center);
                        return center;
                    }
                    catch (Exception ex)
                    {
                        bool rethrow = ExceptionPolicy.HandleException(ex, "UI Policy");
                        if (rethrow)
                            throw;
                        else
                            return null;
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        private void centerAddToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Center center = CenterNew();

            if (center != null)
            {
                LoadFilterTreeView();

                foreach (TreeNode node in this.treeViewFilter.Nodes)
                {
                    if (node is CenterTreeNode)
                    {
                        if (((CenterTreeNode) node).Center.CenterId.Equals(center.CenterId))
                        {
                            this.treeViewFilter.SelectedNode = node;
                            break;
                        }
                    }
                }
            }
        }

        private void centerNodeContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            //Ein-/Ausschalten von Menüpunkten
            Point mouse = this.treeViewFilter.PointToClient(MousePosition);

            CenterTreeNode centerNode = this.treeViewFilter.GetNodeAt(mouse) as CenterTreeNode;

            if (centerNode == null)
            {
                e.Cancel = true;
                return;
            }

            //TODO: Evtl. prüfen ob ein Menupunkt nicht zur Verfügung steht
            //abrufen des kompletten Centers
            //Center center = MetaCall.Business.Centers.GetCenter(centerNode.Center);
        }

        private void centerDeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CenterTreeNode centerNode = this.treeViewFilter.SelectedNode as CenterTreeNode;

            if (centerNode == null)
                return;

            CenterDelete(centerNode.Center);

            LoadFilterTreeView();
        }

        private void CenterDelete(CenterInfo center)
        {
            MessageBoxOptions options = CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft ?
                MessageBoxOptions.RtlReading : 0;

            string msg = string.Format(
                @"Möchten Sie das Center {0} wircklich löschen?

Alle Benutzer dieses Centers stehen weiterhin ohne Centerzuordnung zur Verfügung."
                , center.Bezeichnung);

            try
            {
                DialogResult result = MessageBox.Show(this, msg, Application.ProductName, MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question, MessageBoxDefaultButton.Button2, options);

                if (result == DialogResult.Yes)
                {
                    MetaCall.Business.Centers.Delete(center);
                }
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
