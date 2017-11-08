using System;
using System.Collections.Generic;
using System.Text;

using metatop.Applications.metaCall.DataObjects;
using metatop.Applications.metaCall.ServiceAccessLayer;

namespace metatop.Applications.metaCall.BusinessLayer
{
    public class CallJobReminderBusiness
    {
        MetaCallBusiness metaCallBusiness;

        internal CallJobReminderBusiness(MetaCallBusiness metaCallBusiness)
        {
            this.metaCallBusiness = metaCallBusiness;
        }

        /// <summary>
        /// Erstellt einen User-Reminder aufgrund eines CallJobs
        /// </summary>
        /// <param name="callJob"></param>
        /// <param name="date"></param>
        /// <param name="mode"></param>
        /// <param name="tracking"></param>
        /// <param name="team"></param>
        /// <returns></returns>
        public CallJobReminder Create(CallJob callJob,
            DateTime dateStart, DateTime dateStop, CallJobReminderMode mode, CallJobReminderTracking tracking,
            UserInfo user, TeamInfo team)
        {
            //Parameterwerte prüfen
            if (user == null)
                throw new ArgumentNullException("user");

            if (team == null)
                throw new ArgumentNullException("team");

            if (callJob == null)
                throw new ArgumentNullException("callJob");

            CallJobReminder reminder = new CallJobReminder();
            reminder.CallJobReminderId = Guid.NewGuid();
            reminder.Address = callJob.Sponsor;
            reminder.CallJobResult = null;
            reminder.Project = callJob.Project;
            reminder.ReminderDateStart = dateStart;
            reminder.ReminderDateStop = dateStop;
            reminder.ReminderState = CallJobReminderState.Open;
            reminder.ReminderTracking = tracking;
            reminder.ReminderMode = mode;
            reminder.Team = team;
            reminder.User = user;
            reminder.DialMode = callJob.DialMode;

            Create(reminder);

            return reminder;
        }

        /// <summary>
        /// Erstellt einen TeamReminder
        /// </summary>
        /// <param name="address"></param>
        /// <param name="project"></param>
        /// <param name="date"></param>
        /// <param name="mode"></param>
        /// <param name="tracking"></param>
        /// <param name="team"></param>
        /// <returns></returns>
        public CallJobReminder Create(CallJob callJob,
            DateTime dateStart, DateTime dateStop, CallJobReminderMode mode, CallJobReminderTracking tracking,
            TeamInfo team)
        {
            //Parameterwerte prüfen
            if (team == null)
                throw new ArgumentNullException("team");

            if (callJob == null)
                throw new ArgumentNullException("callJob");

            CallJobReminder reminder = new CallJobReminder();
            reminder.CallJobReminderId = Guid.NewGuid();
            reminder.Address = callJob.Sponsor;
            reminder.CallJobResult = null;
            reminder.Project = callJob.Project;
            reminder.ReminderDateStart = dateStart;
            reminder.ReminderDateStop = dateStop;
            reminder.ReminderState = CallJobReminderState.Open;
            reminder.ReminderTracking = tracking;
            reminder.ReminderMode = mode;
            reminder.Team = team;
            reminder.User = null;
            reminder.DialMode = callJob.DialMode;

            Create(reminder);

            return reminder;
        }

        /// <summary>
        /// Erstellt einen Team-Reminder aufgrund eines CallJobReuslts
        /// </summary>
        /// <param name="address"></param>
        /// <param name="project"></param>
        /// <param name="date"></param>
        /// <param name="mode"></param>
        /// <param name="tracking"></param>
        /// <param name="team"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public CallJobReminder Create(CallJobResult result,
            DateTime dateStart, DateTime dateStop, CallJobReminderMode mode, CallJobReminderTracking tracking,
            TeamInfo team)
        {

            //Parameterwerte prüfen
            if (team == null)
                throw new ArgumentNullException("team");

            if (result == null)
                throw new ArgumentNullException("result");
            
            CallJobReminder reminder = new CallJobReminder();
            reminder.CallJobReminderId = Guid.NewGuid();
            reminder.Address = result.CallJob.Sponsor;
            reminder.CallJobResult =  this.metaCallBusiness.CallJobResults.GetResultInfo(result);
            reminder.Project = result.CallJob.Project;
            reminder.ReminderDateStart = dateStart;
            reminder.ReminderDateStop = dateStop;
            reminder.ReminderState = CallJobReminderState.Open;
            reminder.ReminderTracking = tracking;
            reminder.ReminderMode = mode;
            reminder.Team = team;
            reminder.User = null;
            reminder.DialMode = result.CallJob.DialMode;

            Create(reminder);

            return reminder;
        }

        /// <summary>
        /// Erstellt einen User-Reminder aufgrund eines CallJobResults mit allen Angaben
        /// </summary>
        /// <param name="address"></param>
        /// <param name="project"></param>
        /// <param name="date"></param>
        /// <param name="mode"></param>
        /// <param name="tracking"></param>
        /// <param name="user"></param>
        /// <param name="team"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public CallJobReminder Create(CallJobResult result,
            DateTime dateStart, DateTime dateStop, CallJobReminderMode mode, CallJobReminderTracking tracking,
            UserInfo user, TeamInfo team)
        {
            //Parameterwerte prüfen
            if (user == null)
                throw new ArgumentNullException("user");

            if (team == null)
                throw new ArgumentNullException("team");

            if (result == null)
                throw new ArgumentNullException("result");
            
            //TODO: Berechtigungen prüfen

            CallJobReminder reminder = new CallJobReminder();
            reminder.CallJobReminderId = Guid.NewGuid();
            reminder.Address = result.CallJob.Sponsor;
            reminder.CallJobResult = this.metaCallBusiness.CallJobResults.GetResultInfo(result);
            reminder.Project = result.CallJob.Project;
            reminder.ReminderDateStart = dateStart;
            reminder.ReminderDateStop = dateStop;
            reminder.ReminderMode = mode;
            reminder.ReminderState = CallJobReminderState.Open;
            reminder.ReminderTracking = tracking;
            reminder.Team = team;
            reminder.User = user;
            reminder.DialMode = result.CallJob.DialMode;

            Create(reminder);
            return reminder;
        }

        public void Create(CallJobReminder callJobReminder)
        {
            //TODO: Werte prüfen

            if (callJobReminder.Address == null)
                throw new ArgumentNullException("callJobreminder.Address");

            if (callJobReminder.Project == null)
                throw new ArgumentNullException("callJobreminder.Project");

            if (callJobReminder.ReminderDateStart == DateTime.MinValue)
                throw new ArgumentOutOfRangeException("callJobReminder.ReminderDateStart");

            // Wenn es sich nicht um einen Reminderhandelt, bei dem eine Zeitspanne 
            // vorgegeben wurde, werden start und stop gleichgesetzt
            if (!(callJobReminder.ReminderTracking == CallJobReminderTracking.OnlyTimeSpan) &&
                !(callJobReminder.ReminderTracking == CallJobReminderTracking.DateAndTimeSpan))
                callJobReminder.ReminderDateStop = callJobReminder.ReminderDateStart;


            metaCallBusiness.ServiceAccess.CreateCallJobReminder(callJobReminder, metaCallBusiness.Users.CurrentUser);

        }

        public void Update(CallJobReminder callJobReminder)
        {
            //TODO: Berechtigungen prüfen

            metaCallBusiness.ServiceAccess.UpdateCallJobReminder(callJobReminder);
        }

        public void UpdateCallJobReminders(int evokeUpdateTyp,
                                           Guid? newTeamId,
                                           Guid? newUserId,
                                           Guid? findProjectId,
                                           Guid? findUserId,
                                           Guid? findCallJobReminderId)
        {

            metaCallBusiness.ServiceAccess.UpdateCallJobReminders(evokeUpdateTyp,
                                                      newTeamId,
                                                      newUserId,
                                                      findProjectId,
                                                      findUserId,
                                                      findCallJobReminderId);
        }

        public void Delete(CallJobReminder callJobReminder)
        {
            //TODO: Berechtigungen prüfen
            
            metaCallBusiness.ServiceAccess.DeleteCallJobReminder(callJobReminder);

            //TODO: CallJobReminder-Objekt leeren
        }

        public string GetTrackingDisplayName(string trackingName)
        {

            if (string.IsNullOrEmpty(trackingName) )
                return null;
            
            string resourceName = typeof(CallJobReminderTracking).Name + "_" + trackingName;

            return Strings.ResourceManager.GetString(resourceName);
            
        }

        public List<CallJobReminderInfo> GetCallJobReminderInfoByUserAndProject(Guid? userId, Guid? projectId, Guid? teamId)
        {
            return new List<CallJobReminderInfo>(
                this.metaCallBusiness.ServiceAccess.GetCallJobReminderInfoByUserAndProject(userId, projectId, teamId));
        }

        public CallJobReminder GetCallJobReminder(Guid callJobReminderId)
        {
            return metaCallBusiness.ServiceAccess.GetCallJobReminder(callJobReminderId);
        }

        public CallJobReminder GetCallJobReminderByCallJob(Guid callJobId)
        {
            return metaCallBusiness.ServiceAccess.GetCallJobReminderByCallJob(callJobId);
        }
    
    }
}
