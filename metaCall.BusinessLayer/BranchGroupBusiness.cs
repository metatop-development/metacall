using System;
using System.Collections.Generic;
using System.Text;

using metatop.Applications.metaCall.ServiceAccessLayer;
using metatop.Applications.metaCall.DataObjects;

namespace metatop.Applications.metaCall.BusinessLayer
{
    public class BranchGroupBusiness
    {
        MetaCallBusiness metaCallBusiness;

        internal BranchGroupBusiness(MetaCallBusiness metaCallBusiness)
        {
            this.metaCallBusiness = metaCallBusiness;
        }

        /// <summary>
        /// Liefert eine Liste aller Branchengruppen
        /// </summary>
        /// <returns>
        public List<BranchGroup> BranchGroups
        {
            get
            {
                //Prüfen, ob sich ein Benutzer angemeldet hat
                if (!metaCallBusiness.Users.IsLoggedOn)
                    throw new NoUserLoggedOnException();

                return new List<BranchGroup>(metaCallBusiness.ServiceAccess.GetBranchGroups());
            }
        }

        /// <summary>
        /// Liefert eine Branchengruppe mit der angegebenen BranchGroupID
        /// </summary>
        /// <param name="BranchGroupID"></param>
        /// <returns></returns>
        public BranchGroup GetBranchGroup(Guid branchGroupID)
        {
            //Prüfen, ob sich ein Benutzer angemeldet hat
            if (!metaCallBusiness.Users.IsLoggedOn)
                throw new NoUserLoggedOnException();

            return this.metaCallBusiness.ServiceAccess.GetBranchGroup(branchGroupID);
        }

    }
}
