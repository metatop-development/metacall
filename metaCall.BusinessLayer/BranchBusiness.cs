using System;
using System.Collections.Generic;
using System.Text;

using metatop.Applications.metaCall.ServiceAccessLayer;
using metatop.Applications.metaCall.DataObjects;

namespace metatop.Applications.metaCall.BusinessLayer
{
    public class BranchBusiness
    {
        MetaCallBusiness metaCallBusiness;

        internal BranchBusiness(MetaCallBusiness metaCallBusiness)
        {
            this.metaCallBusiness = metaCallBusiness;
        }

        public List<Branch> Branches
        {
            get
            {
                //Prüfen, ob sich ein Benutzer angemeldet hat
                if (!metaCallBusiness.Users.IsLoggedOn)
                    throw new NoUserLoggedOnException();

                return new List<Branch>(metaCallBusiness.ServiceAccess.GetBranches());
            }
        }

        /// <summary>
        /// Liefert die Branche mit der angegebenen Brachennummer oder Branch.Unknown
        /// </summary>
        /// <param name="BranchNumber"></param>
        /// <returns></returns>
        public Branch GetBranch(int? BranchNumber)
        {
            if (!BranchNumber.HasValue)
                return Branch.Unknown;

            if (BranchNumber == 0)
            {
                return Branch.Unknown;
            }
            else
            {
                Branch branch = Branches.Find(
                    new Predicate<Branch>(delegate(Branch x) { return (x.Branchennummer == BranchNumber); }));
                return branch;
            }
        }

    }
}
