using System;
using System.Collections.Generic;
using System.Text;

using MaDaNet.Common.AppFrameWork.Activities;
using metatop.Applications.metaCall.BusinessLayer.Activities;
using metatop.Applications.metaCall.DataObjects;


namespace metatop.Applications.metaCall.BusinessLayer
{
    public class ATListener: ActivityListenerBase
    {
        private bool newCustomer;

        MetaCallBusiness metacallBusiness;
        User user;
        CallJob callJob;
        CallJobActivityTimeItem currentItem;

        public ATListener(MetaCallBusiness metacallBusiness)
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
                metacallBusiness.ServiceAccess.UpdateCallJobActivityTimeItem(currentItem);
            }

        }

        private void SaveStartItem()
        {
            currentItem = new CallJobActivityTimeItem();
            currentItem.ActivityTimeId = Guid.NewGuid();
            currentItem.ActivityTimeItemType = "Nacharbeitszeit";
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

            /* neuer Eintrag, wenn ein neuer Kunde aufgerufen wird */
            if (activity.GetType() == typeof(NewCustomer) || 
                activity.GetType() == typeof(HangUp) ||
                activity.GetType() == typeof(StopPause) ||
                activity.GetType() == typeof(StopTraining))
            {
                if ((activity.GetType() == typeof(StopPause) || activity.GetType() == typeof(StopTraining)) && this.newCustomer == false)
                {
                    return;
                }
                CallJob currentCallJob;
                if (activity.GetType() == typeof(NewCustomer))
                {
                    currentCallJob = ((NewCustomer)activity).Call.CallJob;
                }
                else
                {
                    currentCallJob = this.callJob;
                }
                this.newCustomer = true;
                this.Reset();
                this.Start();
                this.user = metacallBusiness.Users.CurrentUser;
                this.callJob = currentCallJob;
                this.StartUpActivity = activity;
                SaveStartItem();
                return;
            }

            /* Eintrag speichern, wenn CallJob abgeschlossen wird */
            //if ((activity.GetType() == typeof(NewCustomer)) ||
            if ((activity.GetType() == typeof(CancelCustomer)) ||
                (activity.GetType() == typeof(SaveCustomer)) ||
                (activity.GetType() == typeof(Dial)) ||
                (activity.GetType() == typeof(StartPause)) ||
                (activity.GetType() == typeof(StartTraining)) ||
                (activity.GetType() == typeof(LogOffActivity)) ||
                (activity.GetType() == typeof(ProjectLogOn)) ||
                (activity.GetType() == typeof(ProjectLogOff))) 
            {
                if((activity.GetType() == typeof(StartPause) || activity.GetType() == typeof(StartTraining)) && this.newCustomer == false)
                {
                    return;
                }

                if ((activity.GetType() == typeof(CancelCustomer)) ||
                    (activity.GetType() == typeof(SaveCustomer)) ||
                    (activity.GetType() == typeof(LogOffActivity)) ||
                    (activity.GetType() == typeof(ProjectLogOff))) 
                {
                    this.newCustomer = false;
                }
                if (IsRunning)
                {
                    this.Stop();
                    this.CloseUpActivity = activity;
                    this.Save();
                }

                return;
            }
        }
    }
}
