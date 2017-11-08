using System;
using System.Collections.Generic;
using System.Text;

using metatop.Applications.metaCall.DataObjects;

namespace metatop.Applications.metaCall.BusinessLayer
{
    public delegate void DurringLevelInfoChangedEventHandler(object sender, DurringLevelInfoChangedEventArgs e);

    public class DurringLevelInfoChangedEventArgs : EventArgs
    {
        public DurringLevelInfoChangedEventArgs(int durringLevel)
        {
            this.durringLevel = durringLevel;
        }

        private int durringLevel;
        public int DurringLevel
        {
            get { return durringLevel; }
        }
    }
}
