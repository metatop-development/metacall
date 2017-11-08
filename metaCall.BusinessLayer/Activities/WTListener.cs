using System;
using System.Collections.Generic;
using System.Text;

using MaDaNet.Common.AppFrameWork.Activities;
using metatop.Applications.metaCall.DataObjects;
using metatop.Applications.metaCall.BusinessLayer.Activities;


namespace metatop.Applications.metaCall.BusinessLayer
{
    public class WTListener : ActivityListenerBase
    {
        MetaCallBusiness metacallBusiness;
        User user;
        WorkTimeItem currentItem;

        public WTListener(MetaCallBusiness metacallBusiness)
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
            currentItem.WorkTimeItemType = "Arbeitszeit";
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
            if (activity.GetType() == typeof(LogOnActivity))
            {
                this.Reset();
                this.Start();
                this.StartUpActivity = activity;

                //Referenz auf den aktuellen Benutzer halten
                this.user = metacallBusiness.Users.CurrentUser;
                SaveStartItem();

                return;
            }
            /* Stop */
            if (activity.GetType() == typeof(LogOffActivity))
            {
                this.Stop();
                this.CloseUpActivity = activity;
                this.Save();
                this.Reset();

                return;
            }

            ///* Unterbrechungen -> gibt's nicht*/
            //if (IsRunning && (activity.GetType() == typeof(StartPause)))
            //{
            //    this.Stop();
            //    return;
            //}

            //if (!IsRunning && (activity.GetType() == typeof(StopPause)))
            //{
            //    this.Start();
            //    return;
            //}

            //Sonderfall -> Zeit wurde angehalten und eine andere Aktivität passiert
            if (!IsRunning)
            {
                this.Start();
            }
        }
    }
}
