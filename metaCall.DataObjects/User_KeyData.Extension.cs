using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace metatop.Applications.metaCall.DataObjects
{
    partial class User_KeyData
    {
        public string AvergaeCountPerWorkDay
        {
            get
            {
                if (this.CountOrders == 0 || this.CountWorkDays == 0)
                    return string.Format("{0:f2}", 0);
                else
                    return string.Format("{0:f2}", this.CountOrders / this.CountWorkDays);
            }
        }
    }
}
