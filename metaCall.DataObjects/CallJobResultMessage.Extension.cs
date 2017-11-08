using System;
using System.Collections.Generic;
using System.Text;

namespace metatop.Applications.metaCall.DataObjects
{
    public partial class CallJobResultMessage
    {
        private bool isCancelled ;

        private CallJobResultMessage(bool isCancelled)
        {
            this.isCancelled = isCancelled;
        }

        //StandardKonstruktor 
        public CallJobResultMessage() { }


        public static CallJobResultMessage CancelledResult = new CallJobResultMessage(true);
    }
}
