using System;
using System.Collections.Generic;
using System.Text;
using metatop.Applications.metaCall.DataObjects;

using System.Data;
using System.Xml;


namespace metatop.Applications.metaCall.DataAccessLayer
{
    public static class CallDAL
    {

        private const string spCall_GetNextCalls = "dbo.CallJobs_GetNextCalls";
        private const string spCall_CallJobDone = "dbo.CallJobs_CallDoneTest";
        private const string spCall_ResetCallsForUser = "dbo.User_ResetCalls";
        private const string spCall_GetNextReminderCalls = "dbo.CallJobReminder_GetCurrentRemindersByUser";
        private const string spCall_ReleaseCall = "dbo.CallJobs_ReleaseCall";
        private const string spCall_ReleaseCalls = "dbo.CallJobs_ReleaseCalls";
        private const string spCall_GetSingleCall = "dbo.CallJob_GetSingleCall";
        private const string spCall_CheckCallExists = "dbo.CallJob_CheckCallExists";

        /// <summary>
        /// Liefert die nächsten anstehenden SponsoringCalls
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static Call[] GetNextSponsoringCalls(CallRequestMessage message)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@ProjectId", message.Project.ProjectId);
            parameters.Add("@UserId", message.CurrentUser.UserId);
            parameters.Add("@NumberOfCallJobs", message.NumberOfCallJobs);
            parameters.Add("@ExpirationMinutes", message.ExpirationMinutes);
            parameters.Add("@CallJobIterationCounter", message.Project.IterationCounter);
            if (message.CallJobGroup == null)
                parameters.Add("@CallJobGroupId", null);
            else
                parameters.Add("@CallJobGroupId", message.CallJobGroup.CallJobGroupId);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spCall_GetNextCalls, parameters);

            return ConvertToCalls(dataTable);

        }

        public static Call CheckCallExists(CallJob callJob)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@CallJobId", callJob.CallJobId);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spCall_CheckCallExists, parameters);

            if (dataTable.Rows.Count < 1)
                return null;
            else
                return ConvertToCall(dataTable.Rows[0]);
        }

        public static ReminderCall CheckReminderCallExists(CallJob callJob)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@CallJobId", callJob.CallJobId);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spCall_CheckCallExists, parameters);

            if (dataTable.Rows.Count < 1)
                return null;
            else
                return ConvertToReminderCall(dataTable.Rows[0]);
        }

        public static Call GetSingleCall(CallJob callJob, UserInfo userInfo)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@CallJobId", callJob.CallJobId);
            parameters.Add("@UserId", userInfo.UserId);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spCall_GetSingleCall, parameters);

            if (dataTable.Rows.Count < 1)
                return null;
            else
                return ConvertToCall(dataTable.Rows[0]);
        }

        public static ReminderCall GetSingleReminderCall(CallJob callJob, UserInfo userInfo)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@CallJobId", callJob.CallJobId);
            parameters.Add("@UserId", userInfo.UserId);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spCall_GetSingleCall, parameters);

            if (dataTable.Rows.Count < 1)
                return null;
            else
                return ConvertToReminderCall(dataTable.Rows[0]);
        }        

        /// <summary>
        /// Liefert die nächsten anstehenden ReminderCalls
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ReminderCall[] GetNextReminderCalls(ReminderCallRequestMessage message)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@ProjectId", message.Project.ProjectId);
            parameters.Add("@UserId", message.User.UserId);
            parameters.Add("@ReminderDateRequest", message.ReminderRequestDate);
            parameters.Add("@MaxTeamReminders", message.MaxTeamReminders);
            parameters.Add("@ExpirationDate", message.ExpirationDate);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spCall_GetNextReminderCalls, parameters);
            //DataRow dataRow = dataTable.Rows[0];
            return ConvertToReminderCalls(dataTable);
        }
        
        private static CallJobReminder ConvertToCallJobReminder(DataRow row)
        {
            //Diese Routine holt einen CallJobReminder
            // ACHTUNG ... nicht gegen CallJobReminderDAL.ConvertCallJobreminder austauschen, da das 
            // Result andere Spaltennamen hat!!!!
            
            CallJobReminder reminder = new CallJobReminder();

            //Nullable-Values prüfen
            Guid? userId = (Guid?)SqlHelper.GetNullableDBValue(row["reminder_userId"]);
            Guid? teamId = (Guid?)SqlHelper.GetNullableDBValue(row["reminder_teamId"]);
            Guid? callJobResultId = (Guid?)SqlHelper.GetNullableDBValue(row["callJobResultId"]);

            reminder.CallJobReminderId = (Guid)row["CallJobReminderId"];
            reminder.Address = AddressDAL.GetAddress((Guid)row["Reminder_AddressId"]);
            reminder.Project = ProjectDAL.GetProjectInfo((Guid)row["Reminder_ProjectId"]);
            reminder.User = UserDAL.GetUserInfo(userId);
            reminder.Team = TeamDAL.GetTeamInfo(teamId);
            reminder.ReminderDateStart = (DateTime)row["ReminderDateStart"];
            reminder.ReminderDateStop = (DateTime)row["ReminderDateStop"];
            reminder.ReminderMode = (CallJobReminderMode)row["ReminderMode"];
            reminder.ReminderState = (CallJobReminderState)row["ReminderState"];
            reminder.ReminderTracking = (CallJobReminderTracking)row["ReminderTracking"];
            reminder.CallJobResult = CallJobResultDAL.GetCallJobResultInfo(callJobResultId);
            reminder.DialMode = (DialMode)row["DialMode"];

            return reminder;
        }

        private static ReminderCall[] ConvertToReminderCalls(DataTable dataTable)
        {

            ReminderCall[] calls = new ReminderCall[dataTable.Rows.Count];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[i];
                calls[i] = ConvertToReminderCall(row);

                calls[i].CallJobReminder = ConvertToCallJobReminder(row);
            }
            return calls;
        }

        /// <summary>
        /// Aktualisiert aufgrund der CallId und dem Status den 
        /// CallJab und löscht den Call aus der Liste
        /// </summary>
        /// <param name="callId">CallId des Calls</param>
        /// <param name="callJobState">Aktueller Statsu des CallJobs der gesetzt werden soll</param>
        public static void UpdateCall(Guid callId, CallJobState callJobState)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@callId", callId);
            parameters.Add("@State", callJobState);

            SqlHelper.ExecuteStoredProc(spCall_CallJobDone, parameters);
        }

        /// <summary>
        /// Löscht alle Calls in der Tabelle tblCalls ohne den Status der CallJobs oder Reminder zu verändern
        /// </summary>
        /// <param name="userId"></param>
        public static void ResetAllUserCalls(Guid userId)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@UserId", userId);

            SqlHelper.ExecuteStoredProc(spCall_ResetCallsForUser, parameters);
        }

        /// <summary>
        /// Entfernt einen Call aus der Tabelle tblCalls und gibt 
        /// ihn somit wieder frei für andere Benutzer
        /// </summary>
        /// <param name="callId"></param>
        public static void ReleaseCall(Guid callId)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@CallId", callId);

            SqlHelper.ExecuteStoredProc(spCall_ReleaseCall, parameters);
        }

        public static void ReleaseCalls(Call[] calls)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@CallXml", GetCallXml(calls));

            SqlHelper.ExecuteStoredProc(spCall_ReleaseCalls, parameters);

        }

        private static object GetCallXml(Call[] calls)
        {
            //Erstellt eine XML-Zeichenfolge der Art
            // <centerAdmins>
            //    <CenterAdmin UserId=".." />
            // </CenterAdmins>

            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;

            using (XmlWriter writer = XmlWriter.Create(sb, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("Calls");

                foreach (Call call in calls)
                {
                    writer.WriteStartElement("Call");
                    writer.WriteAttributeString("CallId", call.CallId.ToString());
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
            }

            return sb.ToString();
        }

        private  static Call[] ConvertToCalls(DataTable dataTable)
        {
            Call[] calls = new Call[dataTable.Rows.Count];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[i];
                calls[i] = ConvertToCall(row);
            }
            return calls;
        }

        private static Call ConvertToCall(DataRow row)
        {
            Call call = new Call();

            string phoneNumber = (string) SqlHelper.GetNullableDBValue(row["PhoneNumber"]);
            DateTime? callDate = (DateTime?)SqlHelper.GetNullableDBValue(row["CallDate"]);

            call.CallId = (Guid)row["CallId"];
            call.PhoneNumber = phoneNumber;
            call.CallDate = callDate;
            call.User = UserDAL.GetUserInfo((Guid)row["UserId"]);
            call.ExpirationDate = (DateTime)row["ExpirationDate"];
            call.DialMode = (DialMode)row["DialMode"];
            call.CallJob = CallJobDAL.GetCallJob((Guid)row["CallJobId"]);
            call.CallJobGroup = CallJobGroupDAL.GetCallJobGroup((Guid?)SqlHelper.GetNullableDBValue(row["CallJobGroupId"]));
            // Hier wird die Telefonnummer zugewiesen
            if (call.CallJob.Sponsor.TelefonNummer != "")
            {
                call.PhoneNumber = call.CallJob.Sponsor.TelefonNummer;
            }
            else
            {
                if (call.CallJob.Sponsor.MobilNummer != "")
                {
                    call.PhoneNumber = call.CallJob.Sponsor.MobilNummer;
                }
            }

            return call;
        }

        private static ReminderCall ConvertToReminderCall(DataRow row)
        {
            ReminderCall call = new ReminderCall();

            string phoneNumber = (string)SqlHelper.GetNullableDBValue(row["PhoneNumber"]);
            DateTime? callDate = (DateTime?)SqlHelper.GetNullableDBValue(row["CallDate"]);

            call.CallId = (Guid)row["CallId"];
            call.PhoneNumber = phoneNumber;
            call.CallDate = callDate;
            call.User = UserDAL.GetUserInfo((Guid)row["UserId"]);
            call.ExpirationDate = (DateTime)row["ExpirationDate"];
            call.DialMode = (DialMode)row["DialMode"];
            call.CallJob = CallJobDAL.GetCallJob((Guid)row["CallJobId"]);
            // Hier wird die Telefonnummer zugewiesen
            call.PhoneNumber = call.CallJob.Sponsor.TelefonNummer;
            call.CallJobGroup = CallJobGroupDAL.GetCallJobGroup((Guid?)SqlHelper.GetNullableDBValue(row["CallJobGroupId"]));

            return call;
        }
    }
}
