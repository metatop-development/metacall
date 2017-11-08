using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using metatop.Applications.metaCall.DataObjects;
using metatop.Applications.metaCall.BusinessLayer;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace metatop.Applications.metaCall.WinForms.Modules
{
    [ToolboxItem(true)]
    public partial class ProjectViewInfo : UserControl
    {
        public event EventHandler<ProjectViewInfoSelectedEventArgs> ProjectViewInfoSelected;

        /// <summary>
        /// DataTable für die Projekte
        /// </summary>
        private DataTable dataTableProjects = new DataTable();

        /// <summary>
        /// Parameterloser Konstruktor für Designer
        /// </summary>
        public ProjectViewInfo()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Konstruktor zum initialisieren mit einem neuen ProjektState
        /// </summary>
        /// <param name="State"></param>
        public ProjectViewInfo(ProjectState projectState):this()
        {

            this.bindingSourceProjects.DataSource = this.dataTableProjects;

            this.DataGridViewProjects.Columns.Clear();

            SetupDataTableProjects();

            List<ProjectInfo> projects = MetaCall.Business.Projects.GetProjectsByProjectState(projectState);

            foreach (ProjectInfo project in projects)
            {
                DataTableProjectsAdd(project);
            }
        }

        /// <summary>
        /// Konstruktor zum initialisieren mit einem ProjektState und einem Center
        /// </summary>
        /// <param name="projectState"></param>
        /// <param name="user"></param>
        public ProjectViewInfo(ProjectState projectState, Guid centerId)
            : this()
        {

            this.bindingSourceProjects.DataSource = this.dataTableProjects;

            this.DataGridViewProjects.Columns.Clear();

            SetupDataTableProjects();

            List<ProjectInfo> projects = MetaCall.Business.Projects.GetProjectsByProjectStateAndCenter(projectState, centerId);

            foreach (ProjectInfo project in projects)
            {
                DataTableProjectsAdd(project);
            }
        }

        /// <summary>
        /// Konstruktor zum initialisieren mit einem ProjektState und einem User
        /// </summary>
        /// <param name="projectState"></param>
        /// <param name="user"></param>
        public ProjectViewInfo(ProjectState projectState, User user): this()
        {

            this.bindingSourceProjects.DataSource = this.dataTableProjects;

            this.DataGridViewProjects.Columns.Clear();

            SetupDataTableProjects();

            List<ProjectInfo> projects = MetaCall.Business.Projects.GetProjectsByProjectStateAndUser(projectState, user);

            foreach (ProjectInfo project in projects)
            {
                DataTableProjectsAdd(project);
            }
        }

        /// <summary>
        /// Konstruktor zum initialisieren mit einem ProjektState und einem User
        /// </summary>
        /// <param name="projectState"></param>
        /// <param name="user"></param>
        public ProjectViewInfo(ProjectState projectState, Team team): this()
        {

            this.bindingSourceProjects.DataSource = this.dataTableProjects;

            this.DataGridViewProjects.Columns.Clear();

            SetupDataTableProjects();

            List<ProjectInfo> projects = MetaCall.Business.Projects.GetProjectsByProjectStateAndTeam(projectState, team);

            foreach (ProjectInfo project in projects)
            {
                DataTableProjectsAdd(project);
            }
        }

        private void SetupDataTableProjects()
        {
            
            DataTableHelper.AddColumn(this.dataTableProjects, "ProjectId", "ProjectId", typeof(Guid));
            DataTableHelper.AddColumn(this.dataTableProjects, "Bezeichnung", "Projekt", typeof(string));
            DataTableHelper.AddColumn(this.dataTableProjects, "Project", string.Empty, typeof(ProjectInfo), MappingType.Hidden);

            DataTableHelper.FillGridView(this.dataTableProjects, this.DataGridViewProjects);

            //Weitere Formatierungen durchführen
            DataGridViewColumn column;
            column = this.DataGridViewProjects.Columns[0];
            column.Visible = false;

            column = this.DataGridViewProjects.Columns[1];
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

        }

        private void DataTableProjectsAdd(ProjectInfo project)
        {
                this.bindingSourceProjects.SuspendBinding();

                try
                {
                    Object[] objectData = new object[]{
                    project.ProjectId,
                    project.Bezeichnung, 
                    project};

                    this.dataTableProjects.Rows.Add(objectData);
                }
                finally
                {
                    this.bindingSourceProjects.ResumeBinding();
                }
        }

        private void ProjectViewInfo_SizeChanged(object sender, EventArgs e)
        {
            //this.DataGridViewProjects.Height = this.Height - this.DataGridViewProjects.Top - 20;
            //this.DataGridViewProjects.Width = this.Width - this.DataGridViewProjects.Left - 20;
        }

        private void DataGridViewProjects_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Guid projectId;

            projectId = (Guid)this.DataGridViewProjects.Rows[e.RowIndex].Cells[0].Value;

            OnProjectInfoSelected(new ProjectViewInfoSelectedEventArgs(projectId));
        }

        public ProjectInfo SelectedProject
        {
            get
            {
                DataRowView row = this.bindingSourceProjects.Current as DataRowView;
                if (row != null)
                    return row.Row[2] as ProjectInfo;
                else
                    return null;
            }
        }

        protected void OnProjectInfoSelected(ProjectViewInfoSelectedEventArgs e)
        {
            if (ProjectViewInfoSelected != null)
                ProjectViewInfoSelected(this, e);
        }

        private void DataGridViewProjects_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            StringBuilder description = new StringBuilder();

            if (e.DesiredType == typeof(string))
            {
                int rowIndex = e.RowIndex;
                ProjectInfo projectInfo = ((DataRowView)this.bindingSourceProjects[rowIndex])["Project"] as ProjectInfo;


                if (projectInfo != null)
                {
                    description.Append(projectInfo.Bezeichnung);
                    try
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
                            CallJobStatistics statistics = MetaCall.Business.CallJobs.GetStatistics(projectInfo, MetaCall.Business.Users.CurrentUser.UserId, resultType);

                            if (statistics != null)
                            {
                                description.AppendFormat(" ({0}/{1})", statistics.Total, statistics.InWork);
                            }
                        }
                        else
                        {
                            description.Append(" (-/-)");
                        }
                    }
                    catch
                    {
                        description.Append(" (-/-)");
                    }
                }

            }

            e.Value = description.ToString() ;
            e.FormattingApplied = true;

        }

        private void Filter(string expression)
        {

            if (string.IsNullOrEmpty(expression))
            {
                this.bindingSourceProjects.RemoveFilter();
                return;
            }
            
            string filter = "[Bezeichnung] LIKE '*{0}*'";
            this.bindingSourceProjects.Filter = string.Format(filter, expression);
            
        }

        private void filterTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Filter(this.filterTextBox.Text);
            }
        }
    }
}
