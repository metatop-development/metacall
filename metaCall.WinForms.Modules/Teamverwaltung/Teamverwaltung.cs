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
    [ModulIndex(98)]
    public partial class Teamverwaltung : UserControl, IModulMainControl
    {
        private DataTable dtTeams = new DataTable();
        
        public Teamverwaltung()
        {
            InitializeComponent();
            
            dtTeams.Locale = CultureInfo.CurrentUICulture;
            bindingSourceTeams.DataSource = dtTeams;

            SetupDataTable();

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

            TeamsWithoutCenterTreeNode teamsWithoutCenterTreeNode = new TeamsWithoutCenterTreeNode();
            treeViewFilter.Nodes.Add(teamsWithoutCenterTreeNode);

            TeamsDeletedTreeNode teamsDeletedTreeNode = new TeamsDeletedTreeNode();
            treeViewFilter.Nodes.Add(teamsDeletedTreeNode);

            treeViewFilter.EndUpdate();

        }

        private CenterTreeNode AddCenterNode(CenterInfo center)
        {
            CenterTreeNode centerNode = new CenterTreeNode(center);
            centerNode.ContextMenuStrip = this.centerNodeContextMenuStrip;
            return centerNode;
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
            return this.toolStrip1;
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
                return new StartUpMenuItem("Team", "Verwaltung");
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

        private void Teamverwaltung_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            LoadFilterTreeView();
        }

        private void SetupDataTable()
        {
            MetaCallPrincipal principal = System.Threading.Thread.CurrentPrincipal as MetaCallPrincipal;

            DataTableHelper.AddColumn(this.dtTeams, "Bezeichnung", "Bezeichnung", typeof(string));
            DataTableHelper.AddColumn(this.dtTeams, "Beschreibung", "Beschreibung", typeof(string));
            DataTableHelper.AddColumn(this.dtTeams, "Team", string.Empty, typeof(Team), MappingType.Hidden);
            DataTableHelper.FillGridView(this.dtTeams, this.dataGridViewTeams);

            dataGridViewTeams.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewTeams.RowHeadersVisible = true;
            dataGridViewTeams.ColumnHeadersVisible = true;
            dataGridViewTeams.AutoGenerateColumns = false;

            dataGridViewTeams.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewTeams.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewTeams.Columns[0].MinimumWidth = 200;
            dataGridViewTeams.Columns[1].MinimumWidth = 400;

        }

        private void LoadTeamsIntoDataTable()
        {
            TreeNode node = treeViewFilter.SelectedNode;
            List<TeamInfo> teams = new List<TeamInfo>();

            if (node.GetType() == typeof(CenterTreeNode))
            {
                teams = MetaCall.Business.Teams.GetByCenter(((CenterTreeNode)node).Center);
            }
            else if (node.GetType() == typeof(TeamsWithoutCenterTreeNode))
            {
                teams = MetaCall.Business.Teams.GetByWithoutCenter();
            }
            else if (node.GetType() == typeof(TeamsDeletedTreeNode))
            {
                teams = MetaCall.Business.Teams.GetByDeleted();
            }

            bindingSourceTeams.SuspendBinding();
            try
            {
                dtTeams.Rows.Clear();
                if (teams != null)
                {
                    foreach (TeamInfo teamInfo in teams)
                    {
                        Team team = MetaCall.Business.Teams.GetTeam(teamInfo);

                        object[] objectData = new object[]
                        {
                        team.Bezeichnung,
                        team.Beschreibung,
                        team
                        };

                        dtTeams.Rows.Add(objectData);
                    }
                }
            }
            finally
            {
                bindingSourceTeams.ResumeBinding();
            }
        }

        private void Reload()
        {
            LoadTeamsIntoDataTable();
        }


        private void treeViewFilter_AfterSelect(object sender, TreeViewEventArgs e)
        {
            LoadTeamsIntoDataTable();
        }

        private void TeamNew()
        {
            Team newTeam = new Team();
            newTeam.TeamId = Guid.NewGuid();

            using (TeamEdit dlg = new TeamEdit(newTeam))
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        MetaCall.Business.Teams.Create(newTeam);
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

        private void TeamUpdate()
        {
            if (this.SelectedTeam != null)
            {
                using (TeamEdit dlg = new TeamEdit(SelectedTeam))
                {
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            MetaCall.Business.Teams.Update(SelectedTeam);
                            LoadFilterTreeView();
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

        private void editToolStripButton_Click(object sender, EventArgs e)
        {
            TeamUpdate();
            Reload();
        }

        public Team SelectedTeam
        {
            get
            {
                if (dataGridViewTeams.CurrentRow == null)
                    return null;

                DataRowView currentRowView =
                    (DataRowView)dataGridViewTeams.CurrentRow.DataBoundItem;

                if (currentRowView == null || currentRowView.Row == null)
                    return null;

                return (Team)
                    currentRowView.Row.ItemArray[
                    currentRowView.Row.ItemArray.Length - 1];
            }
        }

        private void newToolStripButton_Click(object sender, EventArgs e)
        {
            TeamNew();
            Reload();
        }

        #region IModulMainControl Member

        public bool CanAccessModul(System.Security.Principal.IPrincipal principal)
        {
            return principal.IsInRole(MetaCallPrincipal.AdminRoleName);
        }

        #endregion

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
        }

        private void centerEditToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CenterTreeNode centerNode = this.treeViewFilter.SelectedNode as CenterTreeNode;

            if (centerNode == null)
                return;

            CenterInfo center = centerNode.Center;

            CenterUpdate(center);

            if (center != null)
            {
                LoadFilterTreeView();

                foreach (TreeNode node in this.treeViewFilter.Nodes)
                {
                    if (node is CenterTreeNode)
                    {
                        if (((CenterTreeNode)node).Center.CenterId.Equals(center.CenterId))
                        {
                            this.treeViewFilter.SelectedNode = node;
                            break;
                        }
                    }
                }
            }
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

        private void dataGridViewUsers_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                this.dataGridViewTeams.Rows[e.RowIndex].Selected = true;
                TeamUpdate();
            }
        }
    }
}
