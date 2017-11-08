using System;
using System.Collections.Generic;
using System.Text;

using MaDaNet.Common.AppFrameWork.Activities;
using metatop.Applications.metaCall.DataObjects;
using metatop.Applications.metaCall.BusinessLayer.Activities;


namespace metatop.Applications.metaCall.BusinessLayer
{
    public class DialTListener: ActivityListenerBase
    {

        bool pause;

        MetaCallBusiness metacallBusiness;
        User user;
        CallJob callJob;
        CallJobActivityTimeItem currentItem;


        public DialTListener(MetaCallBusiness metacallBusiness)
        {
            this.metacallBusiness = metacallBusiness;
        }


        public override void Save()
        {
            base.Save();
            if (currentItem != null)
            {
                if (currentItem.Stop == null)
                {
                    currentItem.Stop = CloseUpActivity.Date;
                    currentItem.StopActivityId = CloseUpActivity.ActivityId;
                    TimeSpan? duration = currentItem.Stop - currentItem.Start;
                    currentItem.Duration = duration.HasValue ? (double?)duration.Value.TotalSeconds : null;
                    metacallBusiness.ServiceAccess.UpdateCallJobActivityTimeItem(currentItem);
                }
            }
        }

        private void SaveStartItem()
        {

            currentItem = new CallJobActivityTimeItem();
            currentItem.ActivityTimeId = Guid.NewGuid();
            currentItem.ActivityTimeItemType = "Anwahl";
            currentItem.User = metacallBusiness.Users.GetUserInfo(this.user);
            currentItem.CallJob = this.callJob;
            currentItem.Start = StartUpActivity.Date;
            currentItem.StartActivityId = StartUpActivity.ActivityId;

            metacallBusiness.ServiceAccess.CreateCallJobActivityTimeItem(currentItem);
        }

        public override void ActivityRaised(ActivityLoggedEventArgs e)
        {
            
            ActivityBase activity = e.Activity;

            if (metacallBusiness.CallJobs.DurringActiv == true)
                return;

            /* Start */
            if (activity.GetType() == typeof(Dial))
            {
                Call call = ((Dial) activity).Call;
                this.Start();
                this.user = metacallBusiness.Users.CurrentUser;
                this.callJob = call.CallJob;
                this.StartUpActivity = activity;
                SaveStartItem();
                return;
            }

            /* Stop */
            if (activity.GetType() == typeof(HangUp)||
                activity.GetType() == typeof(DialConnected))
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
                this.pause = true;
                this.CloseUpActivity = activity;
                this.Save();
                return;
            }

            if (!IsRunning && pause)
            {
                this.pause = false;
                this.Start();
                this.StartUpActivity = activity;
                this.SaveStartItem();
                return;
            }

        }
    }
}
