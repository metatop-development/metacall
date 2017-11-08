using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;

using metatop.Applications.metaCall.BusinessLayer;
using metatop.Applications.metaCall.DataObjects;



namespace metatop.Applications.metaCall.WinForms.Modules
{
    [ToolboxItem(false)]
    public partial class ProjectTeamAssign : Form
    {
        List<TeamInfo> currentTeams;
        List<TeamInfo> teamsList;
        Project project;
        
        public ProjectTeamAssign(Project project, List<TeamInfo> currentTeams)
        {
            InitializeComponent();

            this.project = project;
            this.currentTeams = currentTeams;
        }

        private void BindTeams()
        {
            if (this.project == null)
                return;

            this.allteamsListBox.DisplayMember = "Bezeichnung";

            //Abrufen des Centers, dem dieses Projekt zugeordnet ist
            CenterInfo center = this.project.Center;

            this.teamsList = MetaCall.Business.Teams.GetByCenter(center);

            foreach (TeamInfo team in teamsList)
            {
                if (!this.currentTeams.Exists(new Predicate<TeamInfo>(delegate(TeamInfo teamInfo)
                {
                    return teamInfo.TeamId.Equals(team.TeamId);
                }
                )))
                {
                    this.allteamsListBox.Items.Add(team);
                }
            }

        }

        private void ProjectTeamAssign_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                BindTeams();
            }
        }

        public List<TeamInfo> SelectedTeams
        {
            get
            {
                List<TeamInfo> teams = new List<TeamInfo>(this.allteamsListBox.SelectedItems.Count);

                foreach (TeamInfo team in this.allteamsListBox.SelectedItems)
                {
                    teams.Add(team);
                }
                return teams;
            }
        }
    }
}