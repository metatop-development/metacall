using System;
using System.Collections.Generic;
using System.Text;

using metatop.Applications.metaCall.ServiceAccessLayer;
using metatop.Applications.metaCall.DataObjects;

namespace metatop.Applications.metaCall.BusinessLayer
{
    public class BranchGroupTimeListBusiness
    {
        MetaCallBusiness metaCallBusiness;

        internal BranchGroupTimeListBusiness(MetaCallBusiness metaCallBusiness)
        {
            this.metaCallBusiness = metaCallBusiness;
        }

        /// <summary>
        /// Liefert eine Liste aller BranchGroupTimeList-Einträge mit der angegebenenen BranchGroupID
        /// </summary>
        /// <param name="BranchGroupID"></param>
        /// <returns></returns>
        public List<BranchGroupTimeList> BranchGroupTimeLists(Guid branchGroupID)
        {
            //Prüfen, ob sich ein Benutzer angemeldet hat
            if (!metaCallBusiness.Users.IsLoggedOn)
                throw new NoUserLoggedOnException();

            return new List<BranchGroupTimeList>(metaCallBusiness.ServiceAccess.GetBranchGroupTimeLists(branchGroupID));
        }

        /// <summary>
        /// Liefert einen BranchengruppeTimeList-Eintrag mit der angegebenen branchGroupTimeListID
        /// </summary>
        /// <param name="branchGroupTimeListID"></param>
        /// <returns></returns>
        public BranchGroupTimeList GetBranchGroupTimeList(Guid branchGroupTimeListID)
        {
            //Prüfen, ob sich ein Benutzer angemeldet hat
            if (!metaCallBusiness.Users.IsLoggedOn)
                throw new NoUserLoggedOnException();

            return this.metaCallBusiness.ServiceAccess.GetBranchGroupTimeList(branchGroupTimeListID);
        }
    }
}
