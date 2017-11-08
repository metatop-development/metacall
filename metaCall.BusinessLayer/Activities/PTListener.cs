using System;
using System.Collections.Generic;
using System.Text;

using MaDaNet.Common.AppFrameWork.Activities;
using metatop.Applications.metaCall.DataObjects;
using metatop.Applications.metaCall.BusinessLayer.Activities;

namespace metatop.Applications.metaCall.BusinessLayer
{
    public class PTListener: ActivityListenerBase
    {
        private bool pause;

        MetaCallBusiness metacallBusiness;
        User user;
        ProjectInfo project;
        ProjectLogOnTimeItem currentItem;

        public PTListener(MetaCallBusiness metacallBusiness)
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

                /* Projektlistener wird nicht so nicht mehr verwendet
                 * da Unterbrechungen durch Reminder nicht berücksichtigt werden
                 * MetawareWorkTimeListener speichert die Projektzeit ohne CallJob nach
                 * metaware und in die Tabelle
                metacallBusiness.ServiceAccess.UpdateProjectLogOnTimeItem(currentItem);
                 */
            }
        }

        private void SaveStartItem()
        {
            currentItem = new ProjectLogOnTimeItem();
            currentItem.LogOnTimeId = Guid.NewGuid();
            currentItem.LogOnTimeItemType = "Projektzeit";
            currentItem.User = metacallBusiness.Users.GetUserInfo(this.user);
            currentItem.Project = this.project;
            currentItem.Start = StartUpActivity.Date;
            currentItem.StartActivityId = StartUpActivity.ActivityId;

            /* siehe Save()
            metacallBusiness.ServiceAccess.CreateProjectLogOnTimeItem(currentItem);
             */
        }

        public override void ActivityRaised(ActivityLoggedEventArgs e)
        {
            ActivityBase activity = e.Activity;

            /* Start */
            if (activity.GetType() == typeof(ProjectLogOn))
            {
                this.Start();
                this.StartUpActivity = activity;
                this.user = metacallBusiness.Users.CurrentUser;
                this.project = ((ProjectLogOn)activity).project;
                SaveStartItem();
                return;
            }

            /* Stop */
            if ((activity.GetType() == typeof(ProjectLogOff)) ||
                (activity.GetType() == typeof(LogOffActivity)))
            {
                this.Stop();
                this.CloseUpActivity = activity;
                this.Save();
                this.Reset();
                return;
            }

            /* Unterbrechungen */
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

            if (this.pause && 
                (activity.GetType() == typeof(StopPause) ||
                 activity.GetType() == typeof(StopTraining)))
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
