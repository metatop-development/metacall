using System;
using System.Collections.Generic;
using System.Text;

namespace metatop.Applications.metaCall.DataObjects
{
    partial class WorkDayList
    {
        public string DisplayDate
        {
            get
            {
                return string.Format("{0:d}", this.WorkDay);
            }
        }
    }
}
