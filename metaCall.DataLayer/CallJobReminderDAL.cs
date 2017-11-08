using System;
using System.Collections.Generic;
using System.Text;

using metatop.Applications.metaCall.DataObjects;

using System.Data;
using System.Xml;


namespace metatop.Applications.metaCall.DataAccessLayer
{
    public static class CallJobReminderDAL
    {
        private const string spCallJobReminder_Update = "dbo.CallJobReminder_Update";
        private const string spCallJobreminder_Create = "dbo.CallJobReminder_Create";
        private const string spCallJobReminder_Delete = "dbo.CallJobReminder_Delete";
        private const string spCallJobReminder_GetSingle = "dbo.CallJobReminder_GetSingle";
        private const string spCallJobReminder_GetSingleByCallJob = "dbo.CallJobReminder_GetSingleByCallJob";
        private const string spCallJobReminderInfo_GetByUserAndProject = "dbo.CallJobReminder_GetAllByProjectOrUser";
        private const string spCallJobReminder_UpdateByReminderEdit = "dbo.CallJobReminder_UpdateByReminderEdit";

        public static void CreateCallJobReminder(CallJobReminder callJobReminder, User user)
        {
            IDictionary<string, object> parameters = GetParameters(callJobReminder, user);
            SqlHelper.ExecuteStoredProc(spCallJobreminder_Create, parameters);
        }

        public static void UpdateCallJobReminder(CallJobReminder callJobReminder)
        {
            IDictionary<string, object> parameters = GetParameters(callJobReminder, null);
            SqlHelper.ExecuteStoredProc(spCallJobReminder_Update, parameters);
        }

        public static void DeleteCallJobReminder(Guid callJobReminderId)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@CallJobReminderId", callJobReminderId);
            parameters.Add("@CurrentUser", Guid.NewGuid());

            SqlHelper.ExecuteStoredProc(spCallJobReminder_Delete, parameters);
        }

        public static void UpdateCallJobReminders(  int evokeUpdateTyp,
                                                    Guid? newTeamId,
                                                    Guid? newUserId,
                                                    Guid? findProjectId,
                                                    Guid? findUserId,
                                                    Guid? findCallJobReminderId)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@evokeUpdateTyp",evokeUpdateTyp);
            parameters.Add("@newTeamId", newTeamId);
            parameters.Add("@newUserId", newUserId);
            parameters.Add("@FindProjectId", findProjectId);
            parameters.Add("@FindUserId", findUserId);
            parameters.Add("@FindCallJobReminderId", findCallJobReminderId);

            SqlHelper.ExecuteStoredProc(spCallJobReminder_UpdateByReminderEdit, parameters);
        }

        public static CallJobReminder GetCallJobReminder(Guid callJobReminderId)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@CallJobReminderId", callJobReminderId);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spCallJobReminder_GetSingle, parameters);

            if (dataTable.Rows.Count < 1)
                return null;
            else
                return ConvertToCallJobReminder(dataTable.Rows[0]);
        }

        public static CallJobReminder GetCallJobReminderByCallJob(Guid callJobId)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@CallJobId", callJobId);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spCallJobReminder_GetSingleByCallJob, parameters);

            if (dataTable.Rows.Count < 1)
                return null;
            else
                return ConvertToCallJobReminder(dataTable.Rows[0]);
        }

        private static CallJobReminder[] ConvertToCallJobReminders(DataTable dataTable)
        {

            CallJobReminder[] reminders = new CallJobReminder[dataTable.Rows.Count];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[i];
                reminders[i] = ConvertToCallJobReminder(row);
            }

            return reminders;
        }

        private static IDictionary<string, object> GetParameters(CallJobReminder callJobReminder, User user)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@CallJobReminderId", callJobReminder.CallJobReminderId);
            parameters.Add("@AddressId", callJobReminder.Address.AddressId );
            parameters.Add("@ProjectId", callJobReminder.Project.ProjectId);
            parameters.Add("@CallJobId", callJobReminder.CallJob.CallJobId);

            parameters.Add("@ReminderDateStart",callJobReminder.ReminderDateStart );
            parameters.Add("@ReminderDateStop", callJobReminder.ReminderDateStop);
            parameters.Add("@ReminderTracking", callJobReminder.ReminderTracking);
            parameters.Add("@ReminderState", callJobReminder.ReminderState);
            parameters.Add("@ReminderMode", callJobReminder.ReminderMode);
            if (user == null)
                parameters.Add("@CurrentUser", Guid.NewGuid());
            else
                parameters.Add("@CurrentUser", user.UserId);

            //Objekte die NULL enthalten können müssen geprüft werden
            if (callJobReminder.User == null)
                parameters.Add("@UserId", null);
            else
                parameters.Add("@UserId", callJobReminder.User.UserId);

            if (callJobReminder.Team == null)
                parameters.Add("@TeamId", null);
            else
                parameters.Add("@TeamId", callJobReminder.Team.TeamId);
            
            if (callJobReminder.CallJobResult == null)
                parameters.Add("@callJobResultId", null);
            else
                parameters.Add("@callJobResultId", callJobReminder.CallJobResult.CallJobResultId);

            parameters.Add("@DialMode", callJobReminder.DialMode);

            return parameters;
        }

        private static CallJobReminder ConvertToCallJobReminder(DataRow row)
        {
            CallJobReminder reminder = new CallJobReminder();
            
            //Nullable-Values prüfen
            Guid? userId = (Guid?) SqlHelper.GetNullableDBValue(row["userId"]);
            Guid? teamId = (Guid?) SqlHelper.GetNullableDBValue(row["teamId"]);
            Guid? callJobResultId = (Guid?) SqlHelper.GetNullableDBValue(row["callJobResultId"]);
            Guid? callJobId = (Guid?) SqlHelper.GetNullableDBValue(row["CallJobId"]);

            reminder.CallJobReminderId = (Guid)row["CallJobReminderId"];
            reminder.Address = AddressDAL.GetAddress((Guid)row["AddressId"]);
            reminder.Project = ProjectDAL.GetProjectInfo((Guid)row["ProjectId"]);
            if (userId != null)
                reminder.User = UserDAL.GetUserInfo(userId.Value);

            if (teamId != null)
                reminder.Team = TeamDAL.GetTeamInfo(teamId.Value);
            reminder.ReminderDateStart = (DateTime)row["ReminderDateStart"];
            reminder.ReminderDateStop = (DateTime)row["ReminderDateStop"];
            reminder.ReminderMode = (CallJobReminderMode)row["ReminderMode"];
            reminder.ReminderState = (CallJobReminderState)row["ReminderState"];
            reminder.ReminderTracking = (CallJobReminderTracking)row["ReminderTracking"];
            reminder.CallJobResult = CallJobResultDAL.GetCallJobResultInfo(callJobResultId);
            reminder.DialMode = (DialMode)row["DialMode"];
            
            if (callJobId != null)
                reminder.CallJob = (CallJob)CallJobDAL.GetCallJob(callJobId.Value);

            return reminder;
        }

        public static CallJobReminderInfo[] GetCallJobReminderInfoByUserAndProject(Guid? userId, Guid? projectId, Guid? teamId)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@UserId", userId);
            parameters.Add("@ProjectId", projectId);
            parameters.Add("@TeamId", teamId);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spCallJobReminderInfo_GetByUserAndProject, parameters);

            return ConvertToCallJobReminderInfos(dataTable);
        }

        private static CallJobReminderInfo ConvertToCallJobReminderInfo(DataRow row)
        {
            CallJobReminderInfo callJobReminderInfo = new CallJobReminderInfo();

            callJobReminderInfo.CallJobReminderId = (Guid)row["CallJobReminderId"];
            callJobReminderInfo.ProjectDisplayName = (string)row["Project"];
            callJobReminderInfo.UserDisplayName = (string)SqlHelper.GetNullableDBValue(row["Agent"]);
            callJobReminderInfo.SponsorDisplayName = (string)row["Sponsor"];
            callJobReminderInfo.ReminderArt = (string)row["ReminderArt"];
            callJobReminderInfo.ReminderTrackingDisplayName = (string)row["ReminderTrackingArt"];
            callJobReminderInfo.ReminderDateStart = (DateTime)row["ReminderDateStart"];
            callJobReminderInfo.ReminderDateStop = (DateTime)row["ReminderDateStop"];
            callJobReminderInfo.ReminderStatus = (string)row["ReminderStatus"];
            callJobReminderInfo.ReminderTracking = (CallJobReminderTracking)row["ReminderTracking"];
            callJobReminderInfo.TeamDisplayName = (string)SqlHelper.GetNullableDBValue(row["Team"]);
        
            return callJobReminderInfo;
        }

        private static CallJobReminderInfo[] ConvertToCallJobReminderInfos(DataTable dataTable)
        {
            CallJobReminderInfo[] callJobReminderInfos = new CallJobReminderInfo[dataTable.Rows.Count];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[i];
                callJobReminderInfos[i] = ConvertToCallJobReminderInfo(row);
            }

            return callJobReminderInfos;
        }

    }
}
