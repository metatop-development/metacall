using System;
using System.Collections.Generic;
using System.Text;

using metatop.Applications.metaCall.DataObjects;
using metatop.Applications.metaCall.ServiceAccessLayer;
using System.Security.Permissions;

namespace metatop.Applications.metaCall.BusinessLayer
{
    public class PhoneTimesReportBusiness
    {

        MetaCallBusiness metaCallBusiness;
        internal PhoneTimesReportBusiness(MetaCallBusiness metaCallBusiness)
        {
            this.metaCallBusiness = metaCallBusiness;    
        }


        /// <summary>
        /// Liefert ein ContactReport anhand der CallJobId
        /// </summary>
        /// <param name="CallJobId"></param>
        /// <returns></returns>
        public List<PhoneTimesReport> GetPhoneTimesReport(Guid teamId, Guid userId, DateTime start, DateTime stop)
        {
            return new List<PhoneTimesReport>(this.metaCallBusiness.ServiceAccess.GetPhoneTimesReport(teamId, userId, start, stop));
        }


    }
}
