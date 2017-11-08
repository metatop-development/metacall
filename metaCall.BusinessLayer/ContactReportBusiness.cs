using System;
using System.Collections.Generic;
using System.Text;

using metatop.Applications.metaCall.DataObjects;
using metatop.Applications.metaCall.ServiceAccessLayer;
using System.Security.Permissions;

namespace metatop.Applications.metaCall.BusinessLayer
{
    public class ContactReportBusiness
    {

        MetaCallBusiness metaCallBusiness;
        internal ContactReportBusiness(MetaCallBusiness metaCallBusiness)
        {
            this.metaCallBusiness = metaCallBusiness;    
        }


        /// <summary>
        /// Liefert ein ContactReport anhand der CallJobId
        /// </summary>
        /// <param name="CallJobId"></param>
        /// <returns></returns>
        public List<ContactReport> GetContactReport(Guid callJobId)
        {
            return new List<ContactReport>(this.metaCallBusiness.ServiceAccess.GetContactReport(callJobId));
        }


    }
}
