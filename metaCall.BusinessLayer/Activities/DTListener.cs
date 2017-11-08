using System;
using System.Collections.Generic;
using System.Text;

using MaDaNet.Common.AppFrameWork.Activities;
using metatop.Applications.metaCall.DataObjects;
using metatop.Applications.metaCall.BusinessLayer.Activities;


namespace metatop.Applications.metaCall.BusinessLayer
{
    public class DTListener : ActivityListenerBase
    {
        MetaCallBusiness metacallBusiness;
        User user;
        WorkTimeItem currentItem;
        private bool pause;

        public DTListener(MetaCallBusiness metacallBusiness)
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
            if (this.user == null)
                return;

            if (StartUpActivity == null)
                return;
            
            currentItem = new WorkTimeItem();
            currentItem.WorkTimeId = Guid.NewGuid();
            currentItem.WorkTimeItemType = "Mahnungszeit";
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
            /* Wenn Durring Active auf true wechselt */
            if (activity.GetType() == typeof(DurringChanged))
            {
                if (((DurringChanged)activity).DurringActive)
                {

                    this.Start();
                    this.StartUpActivity = activity;
                    this.user = metacallBusiness.Users.CurrentUser;
                    //                this.project = ((ProjectLogOn)activity).project;
                    SaveStartItem();
                    return;
                }

            }

            /* Stop */
            /* Wenn Durring Active auf false wechselt */
            if (IsRunning && 
                (activity.GetType() == typeof(DurringChanged)))
            {
                if (!((DurringChanged)activity).DurringActive)
                {

                    this.Stop();
                    this.CloseUpActivity = activity;
                    //this.SaveStartItem();
                    this.Save();

                    return;
                }
            }

            /* Unterbrechungen */
            if (IsRunning &&
                (activity.GetType() == typeof(StartPause) ||
                activity.GetType() == typeof(StartTraining)))
            {
                pause = true;
                this.Stop();
                this.CloseUpActivity = activity;
                this.SaveStartItem();
                this.Save();
                return;
            }

            if (pause &&
                (activity.GetType() == typeof(StopPause) ||
                activity.GetType() == typeof(StopTraining)))
            {
                pause = false;
                this.Start();
                this.StartUpActivity = activity;
                SaveStartItem();
                return;
            }
        }
    }
}
