using System;
using System.Collections.Generic;
using System.Text;

using metatop.Applications.metaCall.DataObjects;

namespace metatop.Applications.metaCall.BusinessLayer
{
    public delegate void DurringChangedEventHandler(object sender, DurringChangedEventArgs e);

    public class DurringChangedEventArgs : EventArgs
    {
        public DurringChangedEventArgs(bool durringActiv)
        {
            this.durringActiv = durringActiv;
        }

        private bool durringActiv;
        public bool DurringAcitv
        {
            get { return durringActiv; }
        }


    }
}
