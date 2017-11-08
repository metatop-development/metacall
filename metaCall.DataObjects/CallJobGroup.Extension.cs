using System;
using System.Collections.Generic;
using System.Text;

namespace metatop.Applications.metaCall.DataObjects
{
    public partial class CallJobGroup
    {
        public string DisplayNameCount
        {
            get
            {
                return string.Format("{0} [{1}]",this.DisplayName,this.Count);
            }
        }
    }
}
