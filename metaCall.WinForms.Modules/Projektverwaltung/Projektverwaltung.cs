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
using CtrlSoft.Win.UI;

namespace metatop.Applications.metaCall.WinForms.Modules
{
    [ToolboxItem(false)]
    [ModulIndex(555)]
    public partial class Projektverwaltung : UserControl, IModulMainControl
    {
        /// <summary>
        /// Speichert die sich gerade in Arbeit befindlichen Projekte
        /// </summary>
        private Dictionary<Project, int> projectsInWork = new Dictionary<Project, int>();
        private List<mwProject> projectList;
        private List<ProjectInfo> projectInfoList;

        public Projektverwaltung()
        {
            InitializeComponent();

            Application.Idle += new EventHandler(Application_Idle);
            this.unusedTransferToolStripComboBox.LostFocus += new EventHandler(unusedTransferToolStripComboBox_LostFocus);
            this.unusedTransferToolStripComboBox.KeyDown += new KeyEventHandler(unusedTransferToolStripComboBox_KeyDown);
        }

        void unusedTransferToolStripComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Enter) ||
                 (e.KeyCode == Keys.Tab))
            {
                ToolStripComboBox unusedTransferComboBox = sender as ToolStripComboBox;
                if (unusedTransferComboBox == null)
                    return;

                this.contextMenuStrip1.Close(ToolStripDropDownCloseReason.ItemClicked);
            }
        }

        void unusedTransferToolStripComboBox_LostFocus(object sender, EventArgs e)
        {
            Point mouse = this.projectsTreeView.PointToClient(MousePosition);

            TreeViewHitTestInfo hitTestinfo = this.projectsTreeView.HitTest(MousePosition);

            ProjectInfoTreeNode node = hitTestinfo.Node as ProjectInfoTreeNode;
            node = this.projectsTreeView.SelectedNode as ProjectInfoTreeNode;
            if (node == null)
                return;

            Project projectSource = MetaCall.Business.Projects.Get(node.ProjectInfo);

            ToolStripComboBox unusedTransferComboBox = sender as ToolStripComboBox;

            if (unusedTransferComboBox == null)
                return;

            if (unusedTransferComboBox.SelectedItem != null)
            {
                int numberOfCallJobs;
                string msg;
                ProjectInfo projectTarget = unusedTransferToolStripComboBox.SelectedItem as ProjectInfo;
                numberOfCallJobs = MetaCall.Business.CallJobs.TransferUnusedCallJobsCount(projectSource.ProjectId);
                this.contextMenuStrip1.Close(ToolStripDropDownCloseReason.ItemClicked);
                if (numberOfCallJobs > 0)
                {
                    msg = String.Format("Im Projekt: {0} sind {1} Adressen nicht telefoniert oder 2x nicht erreicht " +
                    "worden. Möchten Sie diese Adressen in das Projekt: {2} übernehmen?", projectSource.Bezeichnung,
                    numberOfCallJobs.ToString(), projectTarget.Bezeichnung);

                    if (MessageBox.Show(msg, "unbenutzte Adressen übernehmen", MessageBoxButtons.YesNo, 
                        MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) ==
                            DialogResult.Yes)
                    {
                        MetaCall.Business.CallJobs.TransferUnusedCallJobs(projectSource.ProjectId,
                            projectTarget.ProjectId);
                        MessageBox.Show("Die Adressen wurden übernommen!",
                            "unbenutzte Adressen übernehmen",MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("In diesem Projekt sind keine unbenutzten Adressen vorhanden!",
                        "unbenutzte Adressen übernehmen", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                //MetaCall.Business.CallJobs.TransferUnusedCallJobs(projectSource.ProjectId, projectTarget.ProjectId);
            }
            else
            {
                this.contextMenuStrip1.Close(ToolStripDropDownCloseReason.ItemClicked);
                MessageBox.Show("Bitte wählen Sie einen Eintrag aus der Liste!");
            }

        }


        void Application_Idle(object sender, EventArgs e)
        {
            UpdateUI();
        }

        private void UpdateUI()
        {
            if (this.projectViewPanel.Controls.Count == 0)
                return;

            Control control = this.projectViewPanel.Controls[0];

            this.newProjectToolStripButton.Visible = (control is ProjectViewInfo);
            this.editProjectToolStripButton.Visible = (control is ProjectViewInfo);
            this.deleteToolStripButton.Visible = (control is ProjectViewInfo);
            this.saveEditToolStripButton.Visible = (control is ProjectViewBase || control is ProjectUnsuitableAddresses);
            this.cancelEditToolStripButton.Visible = (control is ProjectViewBase || control is ProjectUnsuitableAddresses);

            ProjectViewBase projectViewbase = control as ProjectViewBase;
            if (projectViewbase == null)
            {
                ProjectUnsuitableAddresses puaView = control as ProjectUnsuitableAddresses;
                if (puaView == null)
                {
                    this.projectsTreeView.Enabled = true;

                    ProjectInfoTreeNode node = this.projectsTreeView.SelectedNode as ProjectInfoTreeNode;

                    if (node != null)
                    {
                        this.editProjectToolStripButton.Enabled = true;

                        //ProjectStateInfoTreeNode state = node.Parent as ProjectStateInfoTreeNode;
                        //if (state.ProjectState.ProjectState == ProjectState.Finished)

                        if (node.ProjectInfo.State == ProjectState.Finished)
                        {
                            this.editProjectToolStripButton.Text = "Anzeigen";
                        }
                        else
                        {
                            this.editProjectToolStripButton.Text = "Bearbeiten";
                        }
                    }
                }
                else
                {
                    this.projectsTreeView.Enabled = false;
                    this.saveEditToolStripButton.Enabled = !puaView.IsInWork;
                    this.cancelEditToolStripButton.Enabled = !puaView.IsInWork;
                }
            }
            else
            {
                this.projectsTreeView.Enabled = false;
                this.saveEditToolStripButton.Enabled = !projectViewbase.IsInWork;
                this.cancelEditToolStripButton.Enabled = !projectViewbase.IsInWork;
            }
        }

        #region IModulMainControl Member

        public event ModulInfoMessageHandler StatusMessage;

        public event QueryPermissionHandler QueryPermisson;

        public event ModuleStateChangedHandler StateChanged;

        public void Initialize(IModulMainControl caller)
        {
            return;
        }

        public void UnloadModul(out bool canUnload)
        {
            canUnload = true;

            Application.Idle -= new EventHandler(this.Application_Idle);
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


        protected void OnStatusMessage(MaDaNet.Common.AppFrameWork.ApplicationModul.ModulInfoMessageEventArgs e)
        {
            if (StatusMessage != null)
                StatusMessage(this, e);
        }
        protected void OnModuleStateChanged(MaDaNet.Common.AppFrameWork.ApplicationModul.ModuleStateChangedEventArgs e)
        {
            if ((e.State == ModulState.NotInWork) &&
                (projectsInWork.Count > 0))
                return;

            if (StateChanged != null)
                StateChanged(this, e);
        }

        protected void OnQueryPermission(QueryPermissionEventArgs e)
        {
            if (QueryPermisson != null)
                QueryPermisson(this, e);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812")]
        private class Configuration : ModulConfigBase
        {
            public override StartUpMenuItem GetStartUpMenuItem()
            {
                //return base.GetStartUpMenuItem();
                return new StartUpMenuItem("Projekt", "Verwaltung");
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

        private void FillProjectsTreeView()
        {
            string[] projectStateNames = Enum.GetNames(typeof(ProjectState));

            this.projectsTreeView.BeginUpdate();

            if (this.projectsTreeView.Nodes.Count > 0)
                this.projectsTreeView.Nodes.Clear();

            
            foreach (ProjectStateInfo info in MetaCall.Business.Projects.ProjectStates.Values)
            {
                
                ProjectStateInfoTreeNode projectStateNode = new ProjectStateInfoTreeNode(info);

                CenterTreeNode[] centerTreeNodes = GetCenterTreeNodes(info);

                if(centerTreeNodes != null)
                    projectStateNode.Nodes.AddRange(centerTreeNodes);

                //projectStateNode.Nodes.AddRange(GetProjectInfoNodes(info));

                this.projectsTreeView.Nodes.Add(projectStateNode);
            }
            this.projectsTreeView.EndUpdate();
        }

        private CenterTreeNode[] GetCenterTreeNodes(ProjectStateInfo projectStateInfo)
        {
            List<CenterInfo> centers = MetaCall.Business.Centers.Centers;

            CenterTreeNode[] centerTreeNodes = new CenterTreeNode[centers.Count];

            for (int i = 0; i < centers.Count; i++)
            {
                CenterTreeNode centerTreeNode = new CenterTreeNode(centers[i]);
                centerTreeNode.Nodes.AddRange(GetProjectInfoNodes(projectStateInfo, centers[i].CenterId));
                centerTreeNodes[i] = centerTreeNode;
            }
            return centerTreeNodes;
        }

        private TreeNode[] GetProjectInfoNodes(ProjectStateInfo info, Guid centerId)
        {
            List<ProjectInfo> projects = MetaCall.Business.Projects.GetProjectsByProjectStateAndCenter(info.ProjectState, centerId);
            TreeNode[] nodes = new TreeNode[projects.Count];

            for (int i = 0; i < projects.Count; i++)
            {
                ProjectInfoActionTreeNode projectInfoNode = new ProjectInfoActionTreeNode(projects[i]);
                projectInfoNode.ContextMenuStrip = this.contextMenuStrip1;

                foreach (Project project in this.projectsInWork.Keys)
                {
                    if (project.ProjectId.Equals(projects[i].ProjectId) &&
                        this.projectsInWork[project] > 0)
                    {
                        projectInfoNode.StartAction();
                        projectInfoNode.ProgressPercentage =
                            this.projectsInWork[project];
                    }
                }
                nodes[i] = projectInfoNode;
            }
            return nodes;
        }

        #region IModulMainControl Member


        public bool CanPauseApplication
        {
            get
            {
                return true;
            }
        }

        #endregion

        #region IModulMainControl Member


        public bool CanAccessModul(System.Security.Principal.IPrincipal principal)
        {
            return principal.IsInRole(MetaCallPrincipal.AdminRoleName);
        }

        #endregion

        private void projectsTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //ProjectStateInfoTreeNode node = e.Node as ProjectStateInfoTreeNode;
            CenterTreeNode centerTreeNode = e.Node as CenterTreeNode;
            if (centerTreeNode != null)
            {
                ProjectStateInfoTreeNode projectStateInfoTreeNode = e.Node.Parent as ProjectStateInfoTreeNode;
                ShowProjectStateInfoPanel(projectStateInfoTreeNode.ProjectState.ProjectState, centerTreeNode.Center.CenterId);
            }
        }

        private void ShowProjectStateInfoPanel(ProjectState state, Guid centerId)
        {
            ModulState modulState = ModulState.NotInWork;

            this.projectViewPanel.SuspendLayout();
            this.projectViewPanel.Controls.Clear();

            ProjectViewInfo pviView = GetProjectViewInfoControl(state, centerId);

            pviView.Visible = true;
            pviView.Dock = DockStyle.Fill;
            this.projectViewPanel.Controls.Add(pviView);

            this.projectViewPanel.ResumeLayout();

            modulState = ModulState.NotInWork;
            
            UpdateUI();
            
            OnModuleStateChanged(new ModuleStateChangedEventArgs(modulState));            

        }

        private void EditProject(TreeNode treeNode)
        {
            ModulState modulState;

            //laden der View und anzeigen des Projects
            ProjectInfoActionTreeNode node = treeNode as ProjectInfoActionTreeNode;
            if (node == null)
                return;

            if (node.IsInAction)
                return;

            Project project = MetaCall.Business.Projects.Get(node.ProjectInfo);

            this.projectViewPanel.SuspendLayout();

            this.projectViewPanel.Controls.Clear();

            ProjectViewBase projectView = GetProjectViewControl(project);

            projectView.Visible = true;
            projectView.Dock = DockStyle.Fill;
            this.projectViewPanel.Controls.Add(projectView);

            this.projectViewPanel.ResumeLayout();
            modulState = ModulState.InWork;

            UpdateUI();

            OnModuleStateChanged(new ModuleStateChangedEventArgs(modulState));
        }

        private void CheckUnsuitableAddresses(TreeNode treeNode)
        {
            ModulState modulState;

            //laden der View und anzeigen des Projects
            ProjectInfoActionTreeNode node = treeNode as ProjectInfoActionTreeNode;
            if (node == null)
                return;

            if (node.IsInAction)
                return;

            Project project = MetaCall.Business.Projects.Get(node.ProjectInfo);

            this.projectViewPanel.SuspendLayout();

            this.projectViewPanel.Controls.Clear();

            //ProjectViewBase projectView = GetProjectViewControl(project);
            ProjectUnsuitableAddresses puaView = GetProjectUnsuitableAddressesControl(project);
            puaView.Visible = true;
            puaView.Dock = DockStyle.Fill;
            this.projectViewPanel.Controls.Add(puaView);

            //projectView.Visible = true;
            //projectView.Dock = DockStyle.Fill;
            //this.projectViewPanel.Controls.Add(projectView);

            this.projectViewPanel.ResumeLayout();
            modulState = ModulState.InWork;


            UpdateUI();

            OnModuleStateChanged(new ModuleStateChangedEventArgs(modulState));
        }

        private void ProjectViewInfo_Selected(object sender, ProjectViewInfoSelectedEventArgs e)
        {
            foreach (TreeNode nodeProjectsTree in projectsTreeView.Nodes)
            {
                if (nodeProjectsTree is ProjectStateInfoTreeNode)
                {
                    foreach (CenterTreeNode nodeCenter in nodeProjectsTree.Nodes)
                    {
                        if (nodeCenter is CenterTreeNode)
                        {
                            foreach (ProjectInfoTreeNode nodeProjectInfos in nodeCenter.Nodes)
                            {
                                if (nodeProjectInfos is ProjectInfoTreeNode)
                                {
                                    ProjectInfoTreeNode node = nodeProjectsTree as ProjectInfoTreeNode;

                                    if (nodeProjectInfos.ProjectInfo.ProjectId == e.ProjectId)
                                    {
                                        projectsTreeView.SelectedNode = nodeProjectInfos;
                                        EditProject(nodeProjectInfos);
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private ProjectUnsuitableAddresses GetProjectUnsuitableAddressesControl(Project project)
        {
            //mit JJ klären, warum das so gemacht ist?
            ProjectUnsuitableAddresses puaView = (ProjectUnsuitableAddresses)Activator.CreateInstance(typeof(ProjectUnsuitableAddresses), new Object[] { project });
            return puaView;
        }

        private ProjectViewInfo GetProjectViewInfoControl(ProjectState projectState, Guid centerId)
        {
            ProjectViewInfo pviView = (ProjectViewInfo)Activator.CreateInstance(typeof(ProjectViewInfo), new Object[] { projectState, centerId });

            pviView.ProjectViewInfoSelected += new EventHandler<ProjectViewInfoSelectedEventArgs>(ProjectViewInfo_Selected);

            return pviView;
        }

        private ProjectViewBase GetProjectViewControl(Project project)
        {
            //TODO: je nach Projektstatus muss evtl. eine andere View geladen werden
            ProjectCreatedView pcView = (ProjectCreatedView)Activator.CreateInstance(typeof(ProjectCreatedView), new object[] { project });
            return pcView;
        }

        private void NewProjectToolStripButton_Click(object sender, EventArgs e)
        {
            using (CreateProject createProject = new CreateProject())
            {
                if (createProject.ShowDialog() == DialogResult.OK)
                {
                    OnModuleStateChanged(new ModuleStateChangedEventArgs(ModulState.InWork));

                    mwProject mwProject = createProject.SelectedProject;
                    CenterInfo center = createProject.SelectedCenter;
                    try
                    {
                        OnModuleStateChanged(new ModuleStateChangedEventArgs(ModulState.InWork)); 
                        
                        Project newProject = MetaCall.Business.Projects.Create(mwProject, center);

                        ReloadProjectStateInfoNode(ProjectState.Created, center.CenterId);
                        
                        SelectProjectStateTreeNode(ProjectState.Created, newProject.Center);


                        ProjectInfoActionTreeNode actionNode = FindProjectNode(newProject) as  ProjectInfoActionTreeNode;
                        if (actionNode != null)
                        {
                            actionNode.StartAction();
                        }

                        this.addressTransferManager1.TransferAsync(newProject, createProject.Start, createProject.Stop);
                        this.projectsInWork.Add(newProject, 0);

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

        private void ProjektUebernahme_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            FillProjectsTreeView();

            if (this.projectsTreeView.Nodes.Count > 0)
                this.projectsTreeView.SelectedNode = this.projectsTreeView.Nodes[0];
        }

        private void saveEditToolStripButton_Click(object sender, EventArgs e)
        {
            Project project;

            if (this.projectViewPanel.Controls.Count == 0)
                return;


            ProjectViewBase projectView = this.projectViewPanel.Controls[0] as ProjectViewBase;
            if (projectView != null)
            {
                projectView.Save();
                project = projectView.CurrentProject;

                try
                {
                    MetaCall.Business.Projects.Update(project);

                }
                catch (Exception ex)
                {
                    bool rethrow = ExceptionPolicy.HandleException(ex, "UI Policy");
                    if (rethrow)
                        throw;
                }
            }
            else
            {
                ProjectUnsuitableAddresses puaView = this.projectViewPanel.Controls[0] as ProjectUnsuitableAddresses;
                project = puaView.CurrentProject;
                try
                {
                    puaView.UpdateAddresses();
                }
                catch (Exception ex)
                {
                    bool rethrow = ExceptionPolicy.HandleException(ex, "UI Policy");
                    if (rethrow)
                        throw;
                }
            }

            try
            {
                SelectProjectStateTreeNode(project.State, project.Center);
                ShowProjectStateInfoPanel(project.State, project.Center.CenterId);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "UI Policy");
                if (rethrow)
                    throw;
            }
        }

        private void SelectProjectStateTreeNode(ProjectState projectState, CenterInfo centerInfo)
        {

            foreach (ProjectStateInfoTreeNode node in this.projectsTreeView.Nodes)
            {
                if (node.ProjectState.ProjectState == projectState)
                {
                    if (node.Nodes.Count > 0)
                    {
                        if (!node.IsExpanded)
                            //        node.Expand();

                            foreach (CenterTreeNode centerNode in node.Nodes)
                            {
                                if (centerNode.Center.CenterId == centerInfo.CenterId)
                                {
                                    if (!node.IsExpanded)
                                        node.Expand();

                                    if (!centerNode.IsExpanded)
                                        centerNode.Expand();

                                    break;
                                }
                            }
                    }
                    break;
                }
            }
        }

        private void cancelEditToolStripButton_Click(object sender, EventArgs e)
        {
            if (this.projectViewPanel.Controls.Count == 0)
                return;

            Project project;

            ProjectViewBase projectView = this.projectViewPanel.Controls[0] as ProjectViewBase;
            if (projectView == null)
            {
                ProjectUnsuitableAddresses puaView = this.projectViewPanel.Controls[0] as ProjectUnsuitableAddresses;
                project = puaView.CurrentProject;
            }
            else
            {
                project = projectView.CurrentProject;
            }
            //Wählt und zeigt die aktuelle Node im Treeview an
            SelectProjectStateTreeNode(project.State, project.Center);
            //Zeigt die Übersicht der Projekte für das aktuelle Center an
            ShowProjectStateInfoPanel(project.State, project.Center.CenterId);
        }

        private void EditProjectToolStripButton_Click(object sender, EventArgs e)
        {
            ProjectViewInfo projectViewInfo = this.projectViewPanel.Controls[0] as ProjectViewInfo;
            if (projectViewInfo != null)
            {
                ProjectInfo project = projectViewInfo.SelectedProject;
                if (project != null)
                {

                    EditProject(this.projectsTreeView.SelectedNode);
                }
            }
        }

        private bool SelectProjectInfoTreeNode(ProjectInfo project, TreeNodeCollection nodes)
        {
            if (project == null)
                return false;

            if (nodes.Count == 0)
                return false;

            foreach (TreeNode currentNode in nodes)
            {
                //Aktuellen Node testen
                ProjectInfoTreeNode projectinfoTreeNode = currentNode as ProjectInfoTreeNode;
                if (projectinfoTreeNode != null)
                {
                    if (projectinfoTreeNode.ProjectInfo.ProjectId.Equals(project.ProjectId))
                    {
                        this.projectsTreeView.SelectedNode = currentNode;
                        return true;
                    }
                }

                //Rekursion falls childNodes vorhanden
                if (currentNode.Nodes.Count > 0)
                {
                    if (SelectProjectInfoTreeNode(project, currentNode.Nodes))
                        return true;
                }
            }
            return false;
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            Point mouse = this.projectsTreeView.PointToClient(MousePosition);

            ProjectInfoActionTreeNode projectTreeNode = this.projectsTreeView.GetNodeAt(mouse) as ProjectInfoActionTreeNode;

            e.Cancel = true;

            if (projectTreeNode != null)
            {
                if (projectTreeNode.IsInAction)
                    return;

                if (projectTreeNode.ProjectInfo.State != null)
                {
                    switch (projectTreeNode.ProjectInfo.State)
                    {
                        case ProjectState.NotSet:
                            break;
                        case ProjectState.Created:
                            this.changeProjectStateToolStripItem.Visible = false;
                            this.changeProjectStateToolStripItem.Enabled = false;
                            this.editToolStripMenuItem.Text = "Bearbeiten";
                            this.editToolStripMenuItem.Visible = true;
                            this.editToolStripMenuItem.Enabled = true;
                            this.neueAdressenHinzufügenToolStripMenuItem.Visible = true;
                            this.unusedTransferToolStripMenuItem.Visible = false;
                            this.unusedTransferToolStripComboBox.Visible = false;
                            this.unsuitableAddressesToolStripMenuItem.Visible = false;
                            this.resestBrokenAddressTransferToolStripMenuItem.Visible = true;
                            e.Cancel = false;
                            break;
                        case ProjectState.AssignedToTeam:
                            this.changeProjectStateToolStripItem.Visible = false;
                            //this.editToolStripMenuItem.Text = "Bearbeiten";
                            this.editToolStripMenuItem.Visible = false;
                            this.neueAdressenHinzufügenToolStripMenuItem.Visible = false;
                            this.unusedTransferToolStripMenuItem.Visible = false;
                            this.unusedTransferToolStripComboBox.Visible = false;
                            this.unsuitableAddressesToolStripMenuItem.Visible = false;
                            this.resestBrokenAddressTransferToolStripMenuItem.Visible = true;
                            e.Cancel = false;
                            break;
                        case ProjectState.ReleasedForPhone:
                            this.changeProjectStateToolStripItem.Visible = true;
                            this.changeProjectStateToolStripItem.Text = "Projekt abschließen";
                            this.changeProjectStateToolStripItem.Enabled = true;
                            this.editToolStripMenuItem.Visible = true;
                            this.editToolStripMenuItem.Text = "Bearbeiten"; 
                            this.editToolStripMenuItem.Enabled = true;
                            this.neueAdressenHinzufügenToolStripMenuItem.Visible = true;
                            this.unusedTransferToolStripMenuItem.Visible = false;
                            this.unusedTransferToolStripComboBox.Visible = false;
                            this.unsuitableAddressesToolStripMenuItem.Visible = true;
                            this.resestBrokenAddressTransferToolStripMenuItem.Visible = true;
                            e.Cancel = false;
                            break;
                        case ProjectState.Unknown4:
                            break;
                        case ProjectState.Unknown5:
                            break;
                        case ProjectState.Unknown6:
                            break;
                        case ProjectState.Unknown7:
                            break;
                        case ProjectState.Unknown8:
                            break;
                        case ProjectState.Unknown9:
                            break;
                        case ProjectState.Finished:
                            this.editToolStripMenuItem.Visible = true;
                            this.editToolStripMenuItem.Text = "Anzeigen";
                            this.editToolStripMenuItem.Enabled = true;
                            this.changeProjectStateToolStripItem.Visible = true;
                            this.changeProjectStateToolStripItem.Text = "Projekt zum telefonieren freigeben";
                            this.changeProjectStateToolStripItem.Enabled = true;
                            this.neueAdressenHinzufügenToolStripMenuItem.Visible = false;
                            this.unusedTransferToolStripMenuItem.Visible = true;
                            this.unusedTransferToolStripComboBox.Visible = true;
                            this.unsuitableAddressesToolStripMenuItem.Visible = true;
                            this.resestBrokenAddressTransferToolStripMenuItem.Visible = false;
                            FillUnusedTransferComboBox(projectTreeNode.Parent as CenterTreeNode);
                            e.Cancel = false;
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private void changeProjectStateToolStripItem_Click(object sender, EventArgs e)
        {
            ProjectInfoTreeNode projectTreeNode = this.selectedNode as ProjectInfoTreeNode;

            if (projectTreeNode == null)
                return;

            //Projekt vom Server laden 
            Project project = MetaCall.Business.Projects.Get(projectTreeNode.ProjectInfo);

            ProjectState newState = ProjectState.NotSet;
            ProjectState oldState = project.State;
            switch (project.State)
            {
                case ProjectState.NotSet:
                    break;
                case ProjectState.Created:
                    break;
                case ProjectState.AssignedToTeam:
                    break;
                case ProjectState.ReleasedForPhone:

                    newState = ProjectState.Finished;
                    break;
                case ProjectState.Unknown4:
                    break;
                case ProjectState.Unknown5:
                    break;
                case ProjectState.Unknown6:
                    break;
                case ProjectState.Unknown7:
                    break;
                case ProjectState.Unknown8:
                    break;
                case ProjectState.Unknown9:
                    break;
                case ProjectState.Finished:
                    newState = ProjectState.ReleasedForPhone;
                    break;
                default:
                    break;
            }

            //Projektstatus ändern
            if (newState != ProjectState.NotSet)
                try
                {
                    if (newState == ProjectState.Finished)
                    {
                        double percentage = MetaCall.Business.CallJobInfoUnsuitable.GetUnsuitableAddressPercentageByProject(project);
                        string meldung = String.Format("Sie haben {0:P} der ungeeigneten Adressen bestätigt.", percentage);
                        meldung = meldung + " Möchten Sie das Projekt abschließen?";
                        
                        DialogResult result =
                            MessageBox.Show(meldung,
                            "ungeeignete Adressen", MessageBoxButtons.YesNo);

                        if (result == DialogResult.No)
                            return;
                    }
                    project.State = newState;
                    MetaCall.Business.Projects.Update(project);
                    FillProjectsTreeView();
                    
                    SelectProjectStateTreeNode(oldState, project.Center);
                }
                catch (Exception ex)
                {
                    bool rethrow = ExceptionPolicy.HandleException(ex, "UI Policy");
                    if (rethrow)
                        throw;
                }
        }

        private TreeNode selectedNode;
        private void projectsTreeView_MouseDown(object sender, MouseEventArgs e)
        {
            this.selectedNode = this.projectsTreeView.GetNodeAt(e.Location);
            if (this.selectedNode != null)
                this.projectsTreeView.SelectedNode = this.selectedNode;
        }

        private void projectsTreeView_DoubleClick(object sender, EventArgs e)
        {
            Point mouse = this.projectsTreeView.PointToClient(MousePosition);

            TreeViewHitTestInfo hitTestinfo = this.projectsTreeView.HitTest(mouse);

            this.selectedNode = hitTestinfo.Node;

            EditProject(this.selectedNode);
        }

        private void neueAdressenHinzufügenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Point mouse = this.projectsTreeView.PointToClient(MousePosition);

            TreeViewHitTestInfo hitTestinfo = this.projectsTreeView.HitTest(MousePosition);

            ProjectInfoTreeNode node = hitTestinfo.Node as ProjectInfoTreeNode;
            node = this.projectsTreeView.SelectedNode as ProjectInfoTreeNode;
            if (node == null)
                return;

            Project project = MetaCall.Business.Projects.Get(node.ProjectInfo);

            using (AppendNewSponsors appendNewSponsorsDlg = new AppendNewSponsors())
            {
                if (appendNewSponsorsDlg.ShowDialog(this) == DialogResult.OK)
                {

                    OnModuleStateChanged(new ModuleStateChangedEventArgs(ModulState.InWork));

                    ProjectInfoActionTreeNode actionNode = node as ProjectInfoActionTreeNode;
                    if (actionNode != null)
                    {
                        actionNode.StartAction();
                    }

                    this.addressTransferManager1.TransferAsync(
                        project,
                        appendNewSponsorsDlg.StartDate,
                        appendNewSponsorsDlg.StopDate);

                    this.projectsInWork.Add(project,0);
                }
            }
        }

        private void addressTransferManager1_ProgressChanged(ProgressChangedEventArgs e)
        {
            TransferAddressesProgressChangedEventArgs progress = e as TransferAddressesProgressChangedEventArgs;

            if (progress != null)
            {
                ProjectInfoTreeNode projectinfoTreeNode = FindProjectNode(progress.Project);

                string msg = "";
                switch (progress.Step)
                {
                    case TransferAdressesSteps.RetrieveAddresses:
                        msg = "neue Adressen werden abgerufen";
                        break;
                    case TransferAdressesSteps.StoreAddresses:
                        msg = string.Format("Adressen werden gespeichert", progress.ProgressPercentage);
                        break;
                    case TransferAdressesSteps.AnalyseCallJobGroups:
                        msg = string.Format("Anrufgruppen werden analysiert");
                        break;
                    case TransferAdressesSteps.CreateCallJobs:
                        msg = string.Format("Anruf-Aufträge werden erstellt.");
                        break;
                    default:
                        break;
                }

                ProjectInfoActionTreeNode actionNode = projectinfoTreeNode as ProjectInfoActionTreeNode;
                if (actionNode != null)
                {
                    actionNode.ProgressPercentage = progress.ProgressPercentage;
                     actionNode.Step = progress.Step;

                    this.projectsInWork[progress.Project] = progress.ProgressPercentage;
                }
            }
        }

        private void addressTransferManager1_TransferCompleted(object sender, TransferCompletedEventArgs e)
        {
            string msg = null;
            if (!e.Cancelled && e.TotalAdressesTransfered > 0)
            {
                msg = string.Format("Für das Projekt {0} wurden {1} neue Adressen übernommen", e.Project.Bezeichnung, e.TotalAdressesTransfered);
            }
            else
            {
                msg = string.Format("Für das Projekt {0} stehen keine neuen Adressen für die Übernahme an", e.Project.Bezeichnung);
            }

            ProjectInfoActionTreeNode actionNode = FindProjectNode(e.Project) as ProjectInfoActionTreeNode;
            if (actionNode != null)
            {
                actionNode.StopAction();
                this.projectsInWork.Remove(e.Project);
                ReloadProjectStateInfoNode(ProjectState.Created, e.Project.Center.CenterId);
                ReloadProjectStateInfoNode(ProjectState.ReleasedForPhone, e.Project.Center.CenterId);
            }

            // Wenn nicht abgebrochen wurde wird eine Meldung angezeigt
            if (!e.Cancelled)
            {
                if (e.Error != null)
                {
                    bool rethrow = ExceptionPolicy.HandleException(e.Error, "UI Policy");
                    //Darf nicht erneut ausgelöst werden ... -> evtl. sind andere Threds noch nicht fertig
                    // - die Routine muss  für die Aufräumarbeiten durchlaufen werden
                }
                else
                {
                    MessageBox.Show(msg);
                }
            }
            OnModuleStateChanged(new ModuleStateChangedEventArgs(ModulState.NotInWork));
        }

        private ProjectInfoTreeNode FindProjectNode(Project project)
        {
            foreach (TreeNode node in this.projectsTreeView.Nodes)
            {
                if (node is ProjectStateInfoTreeNode)
                {
                    if (node.Nodes.Count > 0)
                    {
                        foreach (TreeNode centerNode in node.Nodes)
                        {
                            if (centerNode.Nodes.Count > 0)
                            {
                                foreach (TreeNode subNode in centerNode.Nodes)
                                {
                                    ProjectInfoTreeNode projectinfoTreeNode = subNode as ProjectInfoTreeNode;
                                    if (projectinfoTreeNode != null)
                                    {
                                        if (projectinfoTreeNode.ProjectInfo.ProjectId.Equals(project.ProjectId))
                                        {
                                            return projectinfoTreeNode;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return null;
        }

        private ProjectStateInfoTreeNode FindProjectStateNode(ProjectState projectState)
        {
            foreach (TreeNode node in this.projectsTreeView.Nodes)
            {
                if (node is ProjectStateInfoTreeNode)
                {
                    if (((ProjectStateInfoTreeNode)node).ProjectState.ProjectState == projectState)
                    {
                        return node as ProjectStateInfoTreeNode;
                    }
                }
            }
            return null;
        }

        private CenterTreeNode FindCenterNode(ProjectStateInfoTreeNode projectStateInfoTreeNode, Guid centerId)
        {
            foreach (TreeNode node in projectStateInfoTreeNode.Nodes)
            {
                if (node is CenterTreeNode)
                {
                    if (((CenterTreeNode)node).Center.CenterId == centerId)
                    {
                        return node as CenterTreeNode;
                    }
                }
            }
            return null;
        }

        private void ReloadProjectStateInfoNode(ProjectState projectState, Guid centerId)
        {
            ProjectStateInfoTreeNode stateNode = FindProjectStateNode(projectState);
            CenterTreeNode centerTreeNode = FindCenterNode(stateNode, centerId);

            if (centerTreeNode != null)
            {
                this.projectsTreeView.BeginUpdate();
                centerTreeNode.Nodes.Clear();
                centerTreeNode.Nodes.AddRange(GetProjectInfoNodes(stateNode.ProjectState, centerId));
                this.projectsTreeView.EndUpdate();
            }
        }

        private void projectsTreeView_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            ProjectInfoActionTreeNode actionNode = e.Node as ProjectInfoActionTreeNode;
            Brush backgroundBrush = new SolidBrush(((TreeView)this.projectsTreeView).BackColor);

            Rectangle nodeBounds = e.Bounds;
            nodeBounds.Offset(53, 0);
            nodeBounds = Rectangle.Inflate(nodeBounds, 53, 0);
            Font nodeFont = this.projectsTreeView.Font;
            Brush textBrush = new SolidBrush(Color.Black);

            if (actionNode != null)
            {
                int progressPercentage = actionNode.ProgressPercentage;

                e.Graphics.FillRectangle(backgroundBrush, nodeBounds);

                if (actionNode.IsInAction)
                {
                    Brush progressBarBrush = Brushes.Black;

                    switch (actionNode.Step)
                    {
                        case TransferAdressesSteps.RetrieveAddresses:
                            progressBarBrush = new SolidBrush(Color.White);
                            break;
                        case TransferAdressesSteps.StoreAddresses:
                            progressBarBrush = new SolidBrush(Color.Silver);
                            break;
                        case TransferAdressesSteps.AnalyseCallJobGroups:
                            progressBarBrush = new SolidBrush(Color.Yellow);
                            break;
                        case TransferAdressesSteps.CreateCallJobs:
                            progressBarBrush = new SolidBrush(Color.GreenYellow);
                            break;
                        default:
                            break;
                    }

                    if (nodeFont == null) nodeFont = ((TreeView)sender).Font;

                    Rectangle statusBarRect = new Rectangle(nodeBounds.Left + 2, nodeBounds.Top, 50, nodeBounds.Height);
                    Rectangle statusRect = new Rectangle(statusBarRect.Left, statusBarRect.Top, (int)(statusBarRect.Width * (actionNode.ProgressPercentage / 100f)), statusBarRect.Height);

                    //Statusanzeige
                    e.Graphics.FillRectangle(progressBarBrush, statusRect);
                    //Prozentsatz
                    e.Graphics.DrawString(actionNode.ProgressPercentage.ToString() + "%", nodeFont, Brushes.Black, statusBarRect);
                    //Projektbezeichnung
                    e.Graphics.DrawString(e.Node.Text, nodeFont, progressBarBrush, new PointF(statusBarRect.Right, statusBarRect.Top));

                    return;
                }
            }

            if ((e.State & TreeNodeStates.Selected) > 0)
            {
                textBrush = new SolidBrush(Color.White);
                backgroundBrush = new SolidBrush(ControlPaint.ContrastControlDark);
            }

            //Textformatierung bestimmen
            StringFormat strfmt = new StringFormat(StringFormatFlags.MeasureTrailingSpaces | StringFormatFlags.NoWrap);
            strfmt.Alignment = StringAlignment.Far;
            strfmt.LineAlignment = StringAlignment.Center;
            strfmt.Trimming = StringTrimming.None;

            //Größe der Textausmasse abrufen und auf die komplette Zeilenhöhe anpassen
            SizeF textSize = e.Graphics.MeasureString(e.Node.Text, nodeFont, new PointF(0, 0), strfmt);
            textSize.Height = nodeFont.Height+3;

            //Rechteck für die Ausgabe berechnen
            nodeBounds = new Rectangle(e.Bounds.Location, textSize.ToSize());
            nodeBounds.Inflate(3, 0);


            //Hintergrund leeren
            e.Graphics.FillRectangle(new SolidBrush(this.projectsTreeView.BackColor), nodeBounds);
            // Hintergrund neu zeichnen
            e.Graphics.FillRectangle(backgroundBrush, nodeBounds);

            if ((e.State & TreeNodeStates.Selected) > 0)
            {
                ControlPaint.DrawFocusRectangle(e.Graphics, nodeBounds);
            }

            //Projektbezeichnung
            e.Graphics.DrawString(e.Node.Text, nodeFont, textBrush, nodeBounds.Left + 3, nodeBounds.Top+1);
        }

        private class ProjectInfoActionTreeNode : ProjectInfoTreeNode
        {
            public ProjectInfoActionTreeNode(ProjectInfo projectInfo)
                : base(projectInfo)
            {

            }

            private bool isInAction;

            public bool IsInAction
            {
                get
                {
                    return this.isInAction;
                }
            }

            public void StartAction()
            {
                this.isInAction = true;
                this.progressPercentage = 0;

                UpdateNode();
            }

            public void StopAction()
            {
                this.isInAction = false;
                this.progressPercentage = 0;

                UpdateNode();
            }

            private int progressPercentage;

            public int ProgressPercentage
            {
                get { return progressPercentage; }
                set
                {
                    progressPercentage = value;
                    UpdateNode();
                }
            }

            private TransferAdressesSteps step;

            public TransferAdressesSteps Step
            {
                get { return step; }
                set { step = value; }
            }

            private void UpdateNode()
            {
                if (this.TreeView == null)
                    return;
                Rectangle nodeBounds = this.Bounds;

                this.TreeView.Invalidate(nodeBounds);
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ProjectInfoTreeNode node = this.projectsTreeView.SelectedNode as ProjectInfoTreeNode;
            if (node != null)
            {
                EditProject(node);
            }
        }

        private void FillUnusedTransferComboBox(CenterTreeNode centerTreeNode)
        {
            
            BindProjects(centerTreeNode.Center);
        }

        private void BindProjects(CenterInfo center)
        {
            try
            {
                this.projectInfoList = MetaCall.Business.Projects.GetProjectsByProjectStateAndCenter(ProjectState.ReleasedForPhone, center.CenterId);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "UI Policy");
                if (rethrow)
                    throw;
            }

            this.unusedTransferToolStripComboBox.Items.Clear();
            //this.unusedTransferToolStripComboBox.DisplayMember = "Bezeichnung";
            this.unusedTransferToolStripComboBox.Items.Clear();
            this.unusedTransferToolStripComboBox.SelectedItem = null;
            this.unusedTransferToolStripComboBox.Text = null;
            foreach (ProjectInfo project in this.projectInfoList)
            {
                ProjectInfoSpecial projectSpecial = new ProjectInfoSpecial();
                projectSpecial.Bezeichnung = project.Bezeichnung;
                projectSpecial.ProjectId = project.ProjectId;
                this.unusedTransferToolStripComboBox.Items.Add(projectSpecial);
            }
        }

        private void unusedTransferToolStripComboBox_DropDownClosed(object sender, EventArgs e)
        {
            ToolStripComboBox unusedTransferComboBox = sender as ToolStripComboBox;
            if (unusedTransferComboBox == null)
                return;

            this.contextMenuStrip1.Close(ToolStripDropDownCloseReason.ItemClicked);
        }

        private void unsuitableAddressesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProjectViewInfo projectViewInfo = this.projectViewPanel.Controls[0] as ProjectViewInfo;
            if (projectViewInfo != null)
            {
                ProjectInfo project = projectViewInfo.SelectedProject;
                if (project != null)
                {
                    CheckUnsuitableAddresses(this.projectsTreeView.SelectedNode);
                }
            }
        }

        private void resestBrokenAddressTransferToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int countFailureAddresses = 0;
            ProjectInfoActionTreeNode node = this.projectsTreeView.SelectedNode as ProjectInfoActionTreeNode;
            if (node != null)
            {
                ProjectInfo project = node.ProjectInfo;
                if (project != null)
                {
                    //CheckUnsuitableAddresses(this.projectsTreeView.SelectedNode);
                    countFailureAddresses = MetaCall.Business.Addresses.GetFailureByProject(project);
                    if (countFailureAddresses > 0)
                    {
                        string msg = String.Format("Achtung! Führen Sie diese Funktion nur aus, wenn Sie sicher "
                            + "sind, dass keine Adressen aktuell für diese Projekt übertragen werden!"
                            + "\n\nIm Projekt {0} werden nun {1} Adressenübertragungen zurückgesetzt!"
                            + "\n\nMöchten Sie fortfahren?", project.Bezeichnung, countFailureAddresses.ToString());
                        if (MessageBox.Show(msg, "Übertragung bereinigen", MessageBoxButtons.YesNo, 
                            MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                                MetaCall.Business.Addresses.DeleteFailureByProject(project);
                    }
                    else
                    {
                        MessageBox.Show("Für dieses Projekt sind keine Übertragungsfehler zu erkennen!");
                    }
                }
            }
        }
    }
    public class ProjectInfoSpecial : ProjectInfo
    {
        public ProjectInfoSpecial(): base()
        {
        }

        public override string ToString()
        {
            return base.Bezeichnung;
        }

    }

}


    


