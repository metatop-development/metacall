using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using metatop.Applications.metaCall.BusinessLayer;
using metatop.Applications.metaCall.DataObjects;

namespace metatop.Applications.metaCall.WinForms.Modules
{
    [ToolboxItem(false)]
    public partial class ProjectCreatedView : ProjectViewBase
    {
        public event EventHandler AssignTeams;
        
        public ProjectCreatedView():base()
        {
            InitializeComponent();
        }

        public ProjectCreatedView(Project project):base(project)
        {
            InitializeComponent();
        }

        protected void OnAssignTeams(EventArgs e)
        {
            if (this.AssignTeams != null)
                this.AssignTeams(this, e);
        }

        private void assignTeamsButton_Click(object sender, EventArgs e)
        {
            OnAssignTeams(EventArgs.Empty);
        }
    }
}
