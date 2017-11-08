using System;
using System.Collections.Generic;
using System.Text;

using MaDaNet.Common.AppFrameWork.Activities;
using metatop.Applications.metaCall.DataObjects;
using metatop.Applications.metaCall.BusinessLayer.Activities;


namespace metatop.Applications.metaCall.BusinessLayer
{
    public class PausenListener: ActivityListenerBase
    {
        MetaCallBusiness metaCallBusiness;
        User user;
        WorkTimeItem currentItem;

        
        public PausenListener(MetaCallBusiness metaCallBusiness) 
        {
            this.metaCallBusiness = metaCallBusiness;
        }

        public override void Save()
        {
            base.Save();
            if (currentItem != null)
            {
                currentItem.Stop = CloseUpActivity.Date;
                currentItem.StopActivityId = CloseUpActivity.ActivityId;
                TimeSpan? duration = currentItem.Stop - currentItem.Start;
                currentItem.Duration = duration.HasValue ? (double?) duration.Value.TotalSeconds : null;
                
                metaCallBusiness.ServiceAccess.UpdateWorkTimeItem(currentItem);
            }

            this.currentItem = null;
            this.user = null;
        }

        private void SaveStartItem()
        {
            currentItem = new WorkTimeItem();
            currentItem.WorkTimeId = Guid.NewGuid();
            currentItem.WorkTimeItemType = "Pause";
            currentItem.User = metaCallBusiness.Users.GetUserInfo(this.user);
            currentItem.Start = StartUpActivity.Date;
            currentItem.StartActivityId = StartUpActivity.ActivityId;


            metaCallBusiness.ServiceAccess.CreateWorkTimeItem(currentItem);
        }
       
        public override void ActivityRaised(ActivityLoggedEventArgs e)
        {
            ActivityBase activity = e.Activity;

            /* Start */
            if (activity.GetType() == typeof(StartPause))
            {
                this.Start();
                this.StartUpActivity = activity;
                this.user = metaCallBusiness.Users.CurrentUser;
                this.SaveStartItem();
                return;
            }

            /* Stop */
            if (this.IsRunning)
            {
                this.Stop();
                this.CloseUpActivity = activity;
                this.Save();
                return;
            }
        }
    }
}
