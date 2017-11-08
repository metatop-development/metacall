using System;
using System.Collections.Generic;
using System.Text;

using metatop.Applications.metaCall.DataObjects;
using metatop.Applications.metaCall.ServiceAccessLayer;
using System.Security.Permissions;

namespace metatop.Applications.metaCall.BusinessLayer
{
    public class ProjectReportBusiness
    {

        MetaCallBusiness metaCallBusiness;
        internal ProjectReportBusiness(MetaCallBusiness metaCallBusiness)
        {
            this.metaCallBusiness = metaCallBusiness;    
        }

        /// <summary>
        /// Liefert ein ProjectReport anhand der projectId
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <returns></returns>
        public List<ProjectReport> GetProjectReport(Guid projectId)
        {
            return new List<ProjectReport>(this.metaCallBusiness.ServiceAccess.GetProjectReport(projectId));
        }

        /// <summary>
        /// Liefert ein ProjectReportDetail anhand der projectId und Art
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <param name="Art"></param>
        /// <returns></returns>
        public ProjectReportDetail GetProjectReportDetail(Guid projectId, int art)
        {
            return this.metaCallBusiness.ServiceAccess.GetProjectReportDetail(projectId, art);
        }
    }
}
