using System;
using System.Collections.Generic;
using System.Text;

using MaDaNet.Common.AppFrameWork.Activities;
using metatop.Applications.metaCall.DataObjects;
using metatop.Applications.metaCall.BusinessLayer.Activities;


namespace metatop.Applications.metaCall.BusinessLayer
{
    public class SecondaryTListener : ActivityListenerBase
    {
        MetaCallBusiness metacallBusiness;
        User user;
        WorkTimeItem currentItem;

        bool secondaryTimer;

        public SecondaryTListener(MetaCallBusiness metacallBusiness)
        {
            this.metacallBusiness = metacallBusiness;
        
        }

        public override void Save()
        {
           base.Save();


           if (currentItem != null)
           {
               currentItem.Stop = CloseUpActivity.Date;
               currentItem.StopActivityId = CloseUpActivity.ActivityId;
               TimeSpan? duration = currentItem.Stop - currentItem.Start;
               currentItem.Duration = duration.HasValue ? (double?)duration.Value.TotalSeconds : null;
               metacallBusiness.ServiceAccess.UpdateWorkTimeItem(currentItem);
           }
        }

        private void SaveStartItem()
        {
            currentItem = new WorkTimeItem();
            currentItem.WorkTimeId = Guid.NewGuid();
            currentItem.WorkTimeItemType = "Nebenzeit";
            currentItem.User = metacallBusiness.Users.GetUserInfo(this.user);
            currentItem.Start = StartUpActivity.Date;
            currentItem.StartActivityId = StartUpActivity.ActivityId;


            metacallBusiness.ServiceAccess.CreateWorkTimeItem(currentItem);
        }
        
        public new void Reset()
        {
            base.Reset();

            this.user = null;
            this.currentItem = null;
        }

        public override void ActivityRaised(ActivityLoggedEventArgs e)
        {

            ActivityBase activity = e.Activity;
            /* Start */
            if (activity.GetType() == typeof(LogOnActivity) ||
                activity.GetType() == typeof(ProjectLogOff) ||
                (activity.GetType() == typeof(DurringChanged) && !((DurringChanged)activity).DurringActive) ||
                activity.GetType() == typeof(StopPause) ||
                activity.GetType() == typeof(StopTraining))   
            {

                if ((activity.GetType() == typeof(StopPause) || activity.GetType() == typeof(StopTraining)) && this.secondaryTimer == true)
                {
                    return;
                }

                if (metacallBusiness.Users.CurrentUser == null)
                    return;

                this.secondaryTimer = true;
                this.Reset();
                this.Start();
                this.StartUpActivity = activity;

                //Referenz auf den aktuellen Benutzer halten
                this.user = metacallBusiness.Users.CurrentUser;
                SaveStartItem();

                return;
            }
            /* Stop */
            if ((activity.GetType() == typeof(ProjectLogOn)) ||
                    (activity.GetType() == typeof(StartPause)) ||
                    (activity.GetType() == typeof(StartTraining)) ||
                    (activity.GetType() == typeof(DurringChanged) && ((DurringChanged)activity).DurringActive) ||
                    (activity.GetType() == typeof(LogOffActivity)) ||
                    (activity.GetType() == typeof(NewCustomer)))
            {
                if ((activity.GetType() == typeof(StartPause) || activity.GetType() == typeof(StartTraining)) && this.secondaryTimer == false)
                {
                    return;
                }

                this.secondaryTimer = true;

                this.Stop();
                this.CloseUpActivity = activity;
                this.Save();
                this.Reset();

                return;
            }
        }
    }
}
