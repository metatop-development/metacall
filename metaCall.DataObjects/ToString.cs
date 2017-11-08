using System;
using System.Collections.Generic;
using System.Text;

namespace metatop.Applications.metaCall.DataObjects
{
    public partial class SecurityGroup
    {
        public override string ToString()
        {
            return this.DisplayName;
        }
    }

    public partial class mwUser
    {
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(this.nachnameField);

            if (!string.IsNullOrEmpty(this.vornameField))
            {
                sb.AppendFormat(", {0}", this.vornameField);
            }

            sb.AppendFormat("({0})", this.partnerNummerField);

            return sb.ToString();
        }
    }
    
    public partial class Team
    {
        public override string ToString()
        {
            return this.bezeichnungField;
        }
    }

    public partial class TeamInfo
    {
        public override string ToString()
        {
            return this.bezeichnungField;
        }
    }

    public partial class Branch
    {
        public override string ToString()
        {
            return this.Bezeichnung.ToString();
        }
    }

    public partial class BranchGroup
    {
        public override string ToString()
        {
            return this.BranchenGruppe.ToString();
        }
    }
}
