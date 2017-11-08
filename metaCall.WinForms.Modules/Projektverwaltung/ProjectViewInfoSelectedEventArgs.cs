using System;
using System.Collections.Generic;
using System.Text;

using metatop.Applications.metaCall.DataObjects;

namespace metatop.Applications.metaCall.WinForms.Modules
{
    public class ProjectViewInfoSelectedEventArgs : EventArgs
    {
        public ProjectViewInfoSelectedEventArgs(Guid projectId)
        {
            this.projectId = projectId;
        }

        private Guid projectId;

        public Guid ProjectId
        {
            get { return projectId; }
        }
    }
}
