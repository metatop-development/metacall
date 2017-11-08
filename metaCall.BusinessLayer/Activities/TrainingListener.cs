using System;
using System.Collections.Generic;
using System.Text;

using MaDaNet.Common.AppFrameWork.Activities;
using metatop.Applications.metaCall.DataObjects;
using metatop.Applications.metaCall.BusinessLayer.Activities;


namespace metatop.Applications.metaCall.BusinessLayer
{
    public class TrainingListener: ActivityListenerBase
    {
        MetaCallBusiness metaCallBusiness;
        User user;
        WorkTimeItem currentItem;
        WorkTimeAdditions workTimeAddition;

        public TrainingListener(MetaCallBusiness metaCallBusiness) 
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
                
                //metaCallBusiness.ServiceAccess.UpdateWorkTimeItem(currentItem);

                StopTraining stopTraining = CloseUpActivity as StopTraining;

                this.workTimeAddition.Notice = stopTraining.TrainingNotice;
                this.workTimeAddition.Duration = currentItem.Duration;
                //newWorkTimeAddition.Notice;
                this.workTimeAddition.Stop = currentItem.Stop;
                //this.workTimeAddition.Confirmed = true;

                metaCallBusiness.Users.UpdateWorkTimeAddition(this.workTimeAddition);
                this.workTimeAddition = null;
            }

            this.currentItem = null;
            this.user = null;

        }

        private void SaveStartItem()
        {
            currentItem = new WorkTimeItem();
            currentItem.WorkTimeId = Guid.NewGuid();
            currentItem.WorkTimeItemType = "Schulung";
            currentItem.User = metaCallBusiness.Users.GetUserInfo(this.user);
            currentItem.Start = StartUpActivity.Date;
            currentItem.StartActivityId = StartUpActivity.ActivityId;

           // metaCallBusiness.ServiceAccess.CreateWorkTimeItem(currentItem);

            this.workTimeAddition = new WorkTimeAdditions();

            this.workTimeAddition.Confirmed = false;
            this.workTimeAddition.Duration = 0;
            //this.workTimeAddition.Notice;
            this.workTimeAddition.Start = currentItem.Start;
            //newWorkTimeAddition.Stop;
            this.workTimeAddition.WorkTimeAdditionId = Guid.NewGuid();
            this.workTimeAddition.User = currentItem.User;
            // Schulung
            this.workTimeAddition.WorkTimeAdditionItemType = new Guid("5F0565D9-0BDC-447B-8211-FF00074CEA22");

            metaCallBusiness.Users.CreateWorkTimeAddition(this.workTimeAddition);
        }
       
        public override void ActivityRaised(ActivityLoggedEventArgs e)
        {
            ActivityBase activity = e.Activity;

            /* Start */
            if (activity.GetType() == typeof(StartTraining))
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
