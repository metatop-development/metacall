using System;
using System.Collections.Generic;
using System.Text;

using MaDaNet.Common.AppFrameWork.Activities;
using metatop.Applications.metaCall.DataObjects;
using metatop.Applications.metaCall.BusinessLayer.Activities;

namespace metatop.Applications.metaCall.BusinessLayer
{
    internal class MetawareWorkTimeListener: ActivityListenerBase
    {
        private bool pause;
        User user;
        ProjectInfo project;

        MetaCallBusiness metaCallBusiness;


        public MetawareWorkTimeListener(MetaCallBusiness metaCallBusiness)
        {

            this.metaCallBusiness = metaCallBusiness;
        }

        public override void Save()
        {
            base.Save();

            if ((this.user != null) && 
                (this.project != null))
            {

                if (System.Threading.Thread.CurrentPrincipal.IsInRole(metaCall.BusinessLayer.MetaCallPrincipal.AdminRoleName) == true)
                    return;

                try
                {
                    ProjectLogOnTimeItem currentItem;
                    currentItem = new ProjectLogOnTimeItem();
                    currentItem.LogOnTimeId = Guid.NewGuid();
                    currentItem.LogOnTimeItemType = "Projektzeit";
                    currentItem.User = metaCallBusiness.Users.GetUserInfo(this.user);
                    currentItem.Start = this.StartTime;
                    currentItem.Stop = this.StopTime;
                    currentItem.Project = this.project;
                    currentItem.StartActivityId = StartUpActivity.ActivityId;
                    currentItem.Stop = CloseUpActivity.Date;
                    currentItem.StopActivityId = CloseUpActivity.ActivityId;
                    TimeSpan? duration = currentItem.Stop - currentItem.Start;
                    currentItem.Duration = duration.HasValue ? (double?)duration.Value.TotalSeconds : null;

                    this.metaCallBusiness.Users.CreateMetawareWorkTimeItem(
                        this.user,
                        this.project,
                        this.StartTime,
                        this.StopTime,
                        currentItem);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
        
        public override void ActivityRaised(ActivityLoggedEventArgs e)
        {
            ActivityBase activity = e.Activity;

            /* 
             * Start 
             * Timer soll nur anlaufen wenn man sich am Projektangemeldet hat
             * oder wenn man die Telefonie verlassen hat
            */
            if ((activity.GetType() == typeof(ProjectLogOn)) ||
                (activity.GetType() == typeof(CancelCustomer)) )
            {
                if (this.IsRunning)
                {
                    this.Stop(); 
                    this.CloseUpActivity = activity;
                    this.Save();
                }
                
                this.Reset();
                this.Start();

                this.StartUpActivity = activity;
                //Referenz auf den aktuellen Benutzer halten
                this.user = this.metaCallBusiness.Users.CurrentUser;
                ProjectLogOn projectLogOn = activity as ProjectLogOn;
                if (projectLogOn != null)
                {
                    this.project = projectLogOn.project;
                }

                NewCustomer newCustomer = activity as NewCustomer;
                if (newCustomer != null)
                {
                    this.project = newCustomer.Call.CallJob.Project;
                }

                return;
            }

            /* Stop 
             * Timer soll stoppen wenn man sich vom Projekt abgemeldet hat 
             * oder wenn Telefoniert wird
            */
            if ((activity.GetType() == typeof(ProjectLogOff)) ||
                (activity.GetType() == typeof(NewCustomer)) ||
                (activity.GetType() == typeof(LogOffActivity)))
            {
                if (this.IsRunning)
                {
                    this.Stop();
                    this.CloseUpActivity = activity; 
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
                if (Duration != TimeSpan.Zero && Duration.TotalSeconds >= 1)
                {
                    //this.SaveStartItem();
                    this.Save();
                }
                return;
            }

            if (pause &&
                (activity.GetType() == typeof(StopPause) ||
                activity.GetType() == typeof(StopTraining)))
            {
                pause = false;
                this.Start();
                this.StartUpActivity = activity;
                //SaveStartItem();
                return;
            }
        }
    }
}
