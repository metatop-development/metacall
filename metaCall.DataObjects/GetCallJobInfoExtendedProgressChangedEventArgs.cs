using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace metatop.Applications.metaCall.DataObjects
{
    public class GetCallJobInfoExtendedProgressChangedEventArgs : ProgressChangedEventArgs
    {
        public GetCallJobInfoExtendedProgressChangedEventArgs(
            CallJobInfoExtended callJobInfoExt,
            int totalCount,
            int current,
            int progressPercentage,
            object userState)
            : base(progressPercentage, userState)
        {
            this.callJobInfoExt = callJobInfoExt;
            this.totalCount = totalCount;
            this.current = current;
        }

        private CallJobInfoExtended callJobInfoExt;
        public CallJobInfoExtended CallJobInfoExt
        {
            get { return callJobInfoExt; }
        }

        private int totalCount;
        public int TotalCount
        {
            get { return totalCount; }
        }

        private int current;
        public int Current
        {
            get { return current; }
        }

    }

}
