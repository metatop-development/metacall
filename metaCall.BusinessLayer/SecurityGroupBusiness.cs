using System;
using System.Collections.Generic;
using System.Text;

using metatop.Applications.metaCall.DataObjects;
using metatop.Applications.metaCall.ServiceAccessLayer;

namespace metatop.Applications.metaCall.BusinessLayer
{
    public class SecurityGroupBusiness
    {
        MetaCallBusiness metaCallBusiness;

        public SecurityGroupBusiness(MetaCallBusiness metaCallBusiness)
        {
            this.metaCallBusiness = metaCallBusiness;
        }

        public List<SecurityGroup> GetAllGroups()
        {
            return new List<SecurityGroup>(this.metaCallBusiness.ServiceAccess.GetAllSecurityGroups());
        }

    }
}
