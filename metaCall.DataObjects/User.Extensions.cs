using System;
using System.Collections.Generic;
using System.Text;

namespace metatop.Applications.metaCall.DataObjects
{
    partial class User
    {
        public string DisplayName
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(this.nachnameField);

                if (!string.IsNullOrEmpty(this.vornameField))
                    sb.AppendFormat(", {0}", this.vornameField);

                return sb.ToString();
            }
        }
    }

    public partial class UserInfo
    {
        public string DisplayName
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(this.nachnameField);

                if (!string.IsNullOrEmpty(this.vornameField))
                    sb.AppendFormat(", {0}", this.vornameField);

                return sb.ToString();
            }
        }
    
    }
}
