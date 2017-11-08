using System;
using System.Collections.Generic;
using System.Text;

using metatop.Applications.metaCall.ServiceAccessLayer;
using metatop.Applications.metaCall.DataObjects;

namespace metatop.Applications.metaCall.BusinessLayer
{
    public class mwProjektBusiness
    {
        MetaCallBusiness metaCallBusiness;

        internal mwProjektBusiness(MetaCallBusiness metaCallBusiness)
        {
            this.metaCallBusiness = metaCallBusiness;
        }

        public List<ProjectOrders> GetAllProjectOrders(Guid userId, Guid projectId)
        {
            return new List<ProjectOrders>(metaCallBusiness.ServiceAccess.GetAllProjectOrders(userId, projectId));
        }

        public List<ProjectCounts> GetAllProjectCounts(Guid userId)
        {
            return new List<ProjectCounts>(metaCallBusiness.ServiceAccess.GetAllProjectCounts(userId));
        }

        public List<ProjectCounts> GetAllProjectCounts(Guid? userId, Guid projectId)
        {
            return new List<ProjectCounts>(metaCallBusiness.ServiceAccess.GetAllProjectCounts(userId, projectId));
        }

    }
}
