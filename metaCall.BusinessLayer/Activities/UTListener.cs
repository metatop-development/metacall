using System;
using System.Collections.Generic;
using System.Text;

using MaDaNet.Common.AppFrameWork.Activities;
using metatop.Applications.metaCall.DataObjects;
using metatop.Applications.metaCall.BusinessLayer.Activities;


namespace metatop.Applications.metaCall.BusinessLayer
{
    public class UTListener: ActivityListenerBase
    {

        bool pause;

        public UTListener() { }

        public override void Save()
        {
            base.Save();
        }

        public override void ActivityRaised(ActivityLoggedEventArgs e)
        {

            ActivityBase activity = e.Activity;
            /* Start */
            if ((activity.GetType() == typeof(LogOnActivity)) ||
                (activity.GetType() == typeof(DurringChanged) && !((DurringChanged)activity).DurringActive) ||
                (activity.GetType() == typeof(ProjectLogOff)))
            {
                //this.Reset();
                this.Start();
                this.StartUpActivity = activity;
                return;
            }

            /* Stop */
            if (this.IsRunning &&
                ((activity.GetType() == typeof(ProjectLogOn)) ||
                 ((activity.GetType() == typeof(DurringChanged)) && (((DurringChanged) activity).DurringActive)) ||
                 (activity.GetType() == typeof(LogOffActivity))))
            {
                this.Stop();
                this.CloseUpActivity = activity;
                this.Save();
                return;
            }

            /*Unterbrechungen */
            if (IsRunning && 
                (activity.GetType() == typeof(StartPause) ||
                activity.GetType() == typeof(StartTraining)))
            {
                this.Stop();
                pause = true;
                return;
            }

            if (pause && 
                (activity.GetType() == typeof(StopPause) ||
                activity.GetType() == typeof(StopTraining)))
            {
                this.Start();
                pause = false;
                return;
            }
        }
    }
}
