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
    [ModulIndex(2)]
    public partial class ProjektReport02Auswertung : UserControl, IModulMainControl
    {
        private MetaCallPrincipal principal;
        private ProjectReportDetail result;
        
        public ProjektReport02Auswertung()
        {
            InitializeComponent();
            this.principal = System.Threading.Thread.CurrentPrincipal as MetaCallPrincipal;
            LoadFilterTreeView();
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
                return new StartUpMenuItem("Projekt Report detailiert", "Auswertungen");
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

        private void LoadReport()
        {
            crystalReportViewer1.Visible = false;

            if (SelectedProjectId != null)
            {
                Guid projectId = (Guid)SelectedProjectId;
                if (btnAktuell.Checked == true)
                    result = MetaCall.Business.ProjectReport.GetProjectReportDetail(projectId, 1);
                else
                    result = MetaCall.Business.ProjectReport.GetProjectReportDetail(projectId, 0);
            }
            else
            {
                result = null;
            }

            if (result == null || result.Daten.Count == 0)
            {
                this.crystalReportViewer1.Visible = false;
                return;
            }

            ReportDocument report = new ReportDocument();
            report.Load(@"ProjektReport02.rpt", CrystalDecisions.Shared.OpenReportMethod.OpenReportByTempCopy);

            report.SetDataSource(result.Daten);
            report.Subreports[1].SetDataSource(result.Summen);
            report.Subreports[0].SetDataSource(result.GeoZonen);

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

    }
}
