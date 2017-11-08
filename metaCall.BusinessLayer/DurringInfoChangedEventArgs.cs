using System;
using System.Collections.Generic;
using System.Text;

using metatop.Applications.metaCall.DataObjects;

namespace metatop.Applications.metaCall.BusinessLayer
{
    public delegate void DurringInfoChangedEventHandler(object sender, DurringInfoChangedEventArgs e);

    public class DurringInfoChangedEventArgs : EventArgs
    {
        public DurringInfoChangedEventArgs(DurringInfo durringInfo)
        {
            this.durringInfo = durringInfo;
        }

        private DurringInfo durringInfo;
        public DurringInfo RemoveDurringInfo
        {
            get { return durringInfo; }
        }
    }
}
