using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace metatop.Applications.metaCall.DataObjects
{
    public class GetCallJobsProgressChangedEventArgs : ProgressChangedEventArgs
    {
        public GetCallJobsProgressChangedEventArgs(
            CallJob callJob,
            int totalCount, 
            int current,
            int progressPercentage,
            object userState)
            : base(progressPercentage, userState)
        {
            this.callJob = callJob;
            this.totalCount = totalCount;
            this.current = current;
        }

        private CallJob callJob;
        public CallJob CallJob
        {
            get { return callJob; }
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
