using System;
using System.Collections.Generic;
using System.Text;

using metatop.Applications.metaCall.DataObjects;

namespace metatop.Applications.metaCall.BusinessLayer
{
    public delegate void ProjectChangedEventHandler(object sender , ProjectChangedEventArgs e);

    public class ProjectChangedEventArgs: EventArgs
    {
        public ProjectChangedEventArgs(ProjectInfo project)
        {
            this.project = project;
        }

        private ProjectInfo project;
        public ProjectInfo Project
        {
            get { return project; }
        }
	

    }
}
