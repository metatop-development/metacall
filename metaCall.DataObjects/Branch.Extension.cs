using System;
using System.Collections.Generic;
using System.Text;

namespace metatop.Applications.metaCall.DataObjects
{
    public partial class Branch : IEquatable<Branch>
    {
        private static Branch unknownBranch ;


        public static Branch Unknown
        {
            get
            {
                if (Branch.unknownBranch == null)
                {
                    Branch unknownBranch = new Branch();
                    unknownBranch.BranchId = Guid.Empty;
                    unknownBranch.Branchennummer = 0;
                    unknownBranch.Bezeichnung = "[Branche unbekannt]";

                    Branch.unknownBranch = unknownBranch;
                }

                return Branch.unknownBranch;
            }
        }

        #region IEquatable<Branch> Member

        public bool Equals(Branch other)
        {
            return this.Branchennummer.Equals(other.Branchennummer);
        }

        #endregion
    }
}
