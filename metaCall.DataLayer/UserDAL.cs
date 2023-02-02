using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;

using metatop.Applications.metaCall.DataObjects;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Xml;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Logging;


namespace metatop.Applications.metaCall.DataAccessLayer
{
    public static class UserDAL
    {

        #region Stored Procedures
        private const string spUser_GetHashedPassword = "dbo.User_GetPassword";
        private const string spUser_SetHashedPassword = "dbo.User_SetPassword";
        private const string spUser_SetEMailPasswort = "User_SetEmailPassword";

        private const string spUser_Create = "dbo.User_Create";
        private const string spUser_Update = "dbo.User_Update";
        private const string spUser_Delete = "dbo.User_Delete";

        private const string spUser_GetSingle = "dbo.User_GetSingle";
        private const string spUser_GetByUserName = "dbo.User_GetByUserName";
        private const string spUser_GetAllActiveUsers = "dbo.User_GetAllActiveUsers";
        private const string spUser_GetListWithOutCenter = "dbo.User_GetListWithOutCenter";
        private const string spUser_GetListDeleted = "dbo.User_GetListDeleted";

        private const string spUser_LogOn = "dbo.User_LogOn";
        private const string spUser_LogOff = "dbo.User_LogOff";
        
        private const string spUser_GetTeams = "dbo.User_GetTeams";
        private const string spUser_GetTeamMitglieder = "dbo.User_GetListByTeam";

        private const string spUser_GetCenterAdmins = "dbo.Center_AdministratorenGet";
        private const string spUser_GetListByCenter = "dbo.User_GetListByCenter";
        private const string spCallJobGroup_GetUsers = "dbo.User_GetListByCallJobGroup";
        private const string spUser_WorktimeItemCreate = "dbo.User_WorktimeItemCreate";
        private const string spUser_WorktimeItemUpdate = "dbo.User_WorktimeItemUpdate";

        private const string spUserWorkTimes_GetListForReport = "dbo.UserWorkTimes_GetListForReport";

        private const string spUserCallJobActivities_GetListForReport = "dbo.UserCallJobActivity_GetListForReport";

        private const string spUser_CreateMetawareWorkTime = "dbo.User_CreateMetawareWorkTime";
        private const string spSystemInformation = "dbo.SystemInformation";

        private const string spUser_GetSignature = "dbo.User_GetSignature";
        private const string spUser_SetSignature = "dbo.User_SetSignature";

        private const string spUser_WorkTimeAdditions_GetByUser = "dbo.User_WorkTimeAdditions_GetByUser";
        private const string spUser_WorkTimeAdditions_GetSingle = "dbo.User_WorkTimeAdditions_GetSingle";
        private const string spUser_WorkTimeAdditions_Update = "dbo.User_WorkTimeAdditions_Update";
        private const string spUser_WorkTimeAdditions_Create = "dbo.User_WorkTimeAdditions_Create";
        private const string spUser_WorkTimeAdditions_Delete = "dbo.User_WorkTimeAdditions_Delete";
        private const string spUser_WorkTimeAdditions_Test = "dbo.mwUsers_WorkTimeTest";

        private const string spUser_WorkTimes_ALL_GetByUser = "dbo.User_WorkTimes_ALL_GetByUser";
        private const string spUser_WorkTimes_GROUP_GetByUser = "dbo.User_WorkTimes_GROUP_GetByUser";

        private const string spUser_KeyData_GetByUser = "dbo.User_KeyData_GetByUser";
        private const string spProject_KeyData_GetByUserAndProject = "dbo.Project_KeyData_GetByUserAndProject";
        
        private const string spUser_WorkTimes_DayList_GetByUser = "dbo.User_WorkTimes_DayList_GetByUser";

        private const string spUser_WorkTimeAdditionItems_GetAllByUser = "dbo.User_WorkTimeAdditionItems_GetAllByUser";
        private const string spUser_WorkTimeAdditionItem_GetSingle = "dbo.User_WorkTimeAdditionItems_GetSingle";

        private const string spmwUsers_AbrechnungCount = "dbo.mwUsers_AbrechnungCount";

        private const string spDomainUser_UsesDialer = "dbo.DomainUser_UsesDialer";
        private const string spDomainUser_GetLine = "dbo.DomainUser_GetLine";
        private const string spDomainUser_GetDialingCode = "dbo.DomainUser_GetDialingCode";
        #endregion

        #region Password Services
        public static string GetHashedPassword(Guid userId)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@UserId", userId);

            object password = SqlHelper.ExecuteScalar(spUser_GetHashedPassword, parameters);

            return (string) SqlHelper.GetNullableDBValue(password);

        }

        public static void SetHashedPassword(Guid userId, string password)
        {

            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@UserId", userId);
            parameters.Add("@password", password);

            SqlHelper.ExecuteStoredProc(spUser_SetHashedPassword, parameters);
        }

        public static void SetEMailPassword(Guid userId, string eMailPassword)
        {

            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@UserId", userId);
            parameters.Add("@eMailPassword", eMailPassword);

            SqlHelper.ExecuteStoredProc(spUser_SetEMailPasswort, parameters);
        }
        
        #endregion

        #region CRUD operations
        /// <summary>
        /// Create a new User on the Database
        /// </summary>
        /// <returns></returns>
        public static void CreateUser(User user, string password)
        {
            IDictionary<string, object> parameters = GetParameters(user);
            parameters.Add("@password", password);

            SqlHelper.ExecuteStoredProc(spUser_Create, parameters);

            ObjectCache.Add(user.UserId, user);
        }
        
        /// <summary>
        /// Aktualisiert einen Benutzer auf der Datenbank
        /// </summary>
        /// <param name="user"></param>
        public static void UpdateUser(User user)
        {
            IDictionary<string, object> parameters = GetParameters(user);
            SqlHelper.ExecuteStoredProc(spUser_Update, parameters);

            ObjectCache.Add(user.UserId, user);
        }

        public static void DeleteUser(Guid userId)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@UserId", userId);
            parameters.Add("@CurrentUser", null);

            SqlHelper.ExecuteStoredProc(spUser_Delete, parameters);

            ObjectCache.Remove(userId);
        }
        #endregion

        #region SELECT operations

        public static User GetUser(Guid userId)
        {

            User user = null;

            user = ObjectCache.Get<User>(userId);

            if (user != null)
                return user;
            
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@UserId", userId);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spUser_GetSingle, parameters);

            if (dataTable.Rows.Count < 1)
                return null;
            else
                return ConvertToUser(dataTable.Rows[0]);
        }

        public static User GetUser(Guid? userId)
        {
            if (!userId.HasValue)
                return null;

            return GetUser(userId.Value);
        }

        public static User GetUser(string userName)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@UserName", userName);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spUser_GetByUserName, parameters);

            if (dataTable.Rows.Count < 1)
                return null;
            else
                return ConvertToUser(dataTable.Rows[0]);
        }

        public static UserInfo[] GetAllUsers()
        {
            DataTable dataTable = SqlHelper.ExecuteDataTable(spUser_GetAllActiveUsers);

            return ConvertToUserInfos(dataTable);

        }

        #region TeamMitglieder
        public static TeamMitglied[] GetUsersByTeam(Guid teamId)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@TeamId", teamId);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spUser_GetTeamMitglieder, parameters);

            return ConvertToTeamMitglieder(dataTable);
        }

        private static TeamMitglied[] ConvertToTeamMitglieder(DataTable dataTable)
        {
            TeamMitglied[] mitglieder = new TeamMitglied[dataTable.Rows.Count];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[i];
                mitglieder[i] = ConvertToTeamMitglieder(row);
            }

            return mitglieder;
        }

        private static TeamMitglied ConvertToTeamMitglieder(DataRow row)
        {
            TeamMitglied mitglied = new TeamMitglied();
            string vorname = (string)SqlHelper.GetNullableDBValue(row["Vorname"]);

            mitglied.UserId = (Guid)row["UserId"];
            mitglied.Vorname = vorname;
            mitglied.Nachname = (string)row["Nachname"];

            mitglied.IsTeamLeiter = (bool)row["isTeamleiter"];

            return mitglied;
        }
        #endregion

        #region CenterAdministratoren
        public static UserInfo[] GetCenterAdmins(Center center)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@CenterId", center.CenterId);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spUser_GetCenterAdmins, parameters);

            return ConvertToUserInfos(dataTable);
        }
        #endregion

        #endregion

        private static UserInfo[] ConvertToUserInfos(DataTable dataTable)
        {
            UserInfo[] userInfos = new UserInfo[dataTable.Rows.Count];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[i];
                userInfos[i] = ConvertToUserInfo(row);
            }

            return userInfos;

        }

        private static UserInfo ConvertToUserInfo(DataRow row)
        {
            
            UserInfo userInfo = new UserInfo();

            string vorname = (string) SqlHelper.GetNullableDBValue(row["Vorname"]);

            userInfo.UserId = (Guid)row["UserId"];
            userInfo.Vorname = vorname;
            userInfo.Nachname = (string)row["Nachname"];

            ObjectCache.Add(userInfo.UserId, userInfo, TimeSpan.FromMinutes(30));

            return userInfo;
        }

        private static User ConvertToUser(DataRow row)
        {
            User user = new User();

            string vorname = (string) SqlHelper.GetNullableDBValue(row["Vorname"]);

            user.UserId =  (Guid) row["UserId"];
            user.UserName = (string)row["username"];
            user.Vorname = vorname;
            user.Nachname = (string)row["Nachname"];
            user.IsMetaWareUser = (bool)row["IsMetawareUser"];
            user.IsDeleted= (bool)row["deleted"];
            user.ReminderEditPermit = (bool)row["ReminderEditPermit"];
            user.ProjectSearchPermit = (bool)row["ProjectSearchPermit"];
            user.WorkingTimeEditPermit = (bool)row["WorkingTimeEditPermit"];
            user.DialMode = (DialMode)row["DialMode"];
            user.Dunning = (bool)row["Dunning"];
            user.Teams = TeamDAL.GetTeamAssignsByUser(user.UserId);
            user.mwUser = mwUserDAL.GetmwUser(user.UserId);
            user.AnmeldungEmail = (string)SqlHelper.GetNullableDBValue(row["AnmeldungEmail"]);
            user.PasswordEmail = (string)SqlHelper.GetNullableDBValue(row["PasswordEmail"]);
            user.AdditionalInfo1 = (string)SqlHelper.GetNullableDBValue(row["AdditionalInfo1"]);
            user.AdditionalInfo2 = (string)SqlHelper.GetNullableDBValue(row["AdditionalInfo2"]);
            
            //Laden des Centers
            Guid? centerId = (Guid?) SqlHelper.GetNullableDBValue(row["CenterId"]);
            user.Center = CenterDAL.GetCenterInfo(centerId);

            //Laden des UserProfiles
            Guid? userProfileId = (Guid?)SqlHelper.GetNullableDBValue(row["userProfileId"]);
            user.UserProfile = UserProfileDAL.GetUserProfile(userProfileId);

            //Berechtigungsgruppen
            user.SecurityGroups = SecurityGroupDAL.GetSecurityGroupsForUser(user.UserId);

            ObjectCache.Add(user.UserId, user);

            return user;
        }

        public static void LogOnUser(User user)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@UserId", user.UserId);

            SqlHelper.ExecuteStoredProc(spUser_LogOn, parameters);
        }

        public static void LogOff(User user)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@UserId", user.UserId);

            SqlHelper.ExecuteStoredProc(spUser_LogOff, parameters);
        }

        #region WorkTimeItems
        public static void CreateWorkTimeItem(WorkTimeItem workTimeItem)
        {
            IDictionary<string, object> parameters = GetParameters(workTimeItem);
            SqlHelper.ExecuteStoredProc(spUser_WorktimeItemCreate, parameters);
            
        }

        public static void UpdateWorkTimeItem(WorkTimeItem workTimeItem)
        {
            IDictionary<string, object> parameters = GetParameters(workTimeItem);
            SqlHelper.ExecuteStoredProc(spUser_WorktimeItemUpdate, parameters);
        }

        private static IDictionary<string, object> GetParameters(WorkTimeItem workTimeItem)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@WorkTimeId", workTimeItem.WorkTimeId);
            parameters.Add("@UserId", workTimeItem.User.UserId);
            parameters.Add("@Start", workTimeItem.Start);
            parameters.Add("@Stop", workTimeItem.Stop);
            parameters.Add("@Duration", workTimeItem.Duration);
            parameters.Add("@WorktimeItemType", workTimeItem.WorkTimeItemType);
            parameters.Add("@StartActivityId", workTimeItem.StartActivityId);
            parameters.Add("@StopActivityId", workTimeItem.StopActivityId);

            return parameters;
        }

        #endregion


        #region WorkTimeAdditionItems

        public static WorkTimeAdditionItems[] WorkTimeAdditionItems_GetAllByUser(Guid userId, DateTime from, DateTime to)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@UserId", userId);
            parameters.Add("@From", from);
            parameters.Add("@To", to);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spUser_WorkTimeAdditionItems_GetAllByUser, parameters);

            return ConvertToWorkTimeAdditionItems(dataTable);
        }

        private static WorkTimeAdditionItems[] ConvertToWorkTimeAdditionItems(DataTable dataTable)
        {
            WorkTimeAdditionItems[] workTimeAdditionItems = new WorkTimeAdditionItems[dataTable.Rows.Count];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[i];
                workTimeAdditionItems[i] = ConvertToWorkTimeAdditionItem(row);
            }

            return workTimeAdditionItems;

        }

        private static WorkTimeAdditionItems ConvertToWorkTimeAdditionItem(DataRow row)
        {
            WorkTimeAdditionItems workTimeAdditionItem = new WorkTimeAdditionItems();

            workTimeAdditionItem.WorkTimeAdditionItemId = (Guid)row["WorkTimeAdditionItemId"];
            workTimeAdditionItem.Bezeichnung = (string)row["Bezeichnung"];
            workTimeAdditionItem.AllgemeineTaetigkeit = (Boolean)row["AllgemeineTaetigkeit"];
            workTimeAdditionItem.TaetigkeitArt = (int)(row["TaetigkeitArt"]);
            
            return workTimeAdditionItem;
        }

        public static WorkTimeAdditionItems WorkTimeAdditionItems_GetSingle(Guid workTimeAdditionItemId)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@WorkTimeAdditionItemId", workTimeAdditionItemId);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spUser_WorkTimeAdditionItem_GetSingle, parameters);

            if (dataTable.Rows.Count < 1)
                return null;
            else
                return ConvertToWorkTimeAdditionItem(dataTable.Rows[0]);

        }

        #endregion


        #region WorkTimeAdditions

        public static WorkTimeAdditions WorkTimeAdditions_GetSingle(Guid workTimeAdditionId)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@WorkTimeAdditionId", workTimeAdditionId);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spUser_WorkTimeAdditions_GetSingle, parameters);

            if (dataTable.Rows.Count < 1)
                return null;
            else
                return ConvertToWorkTimeAddition(dataTable.Rows[0]);

        }

        public static WorkTimeAdditions[] WorkTimeAdditions_GetByUser(Guid userId, DateTime from, DateTime to)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@UserId", userId);
            parameters.Add("@From", from);
            parameters.Add("@To", to);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spUser_WorkTimeAdditions_GetByUser, parameters);

            if (dataTable.Rows.Count < 1)
                return null;
            else
                return ConvertToWorkTimeAdditions(dataTable);

        }

        public static int mwWorkTime_Test(WorkTimeAdditions workTimeAddition)
        {
            int year = 0;
            int month = 0;
            int day = 0;

            if (workTimeAddition.Start != null)
            {
                DateTime wTimeStart = (DateTime)workTimeAddition.Start;
                year = wTimeStart.Year;
                month = wTimeStart.Month;
                day = wTimeStart.Day;
            }

            DateTime workTimeDate = new DateTime(
                                                    year,
                                                    month,
                                                    day
                                                );


            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@WorkTimeAdditionId", workTimeAddition.WorkTimeAdditionId);
            parameters.Add("@UserId", workTimeAddition.User.UserId);
            parameters.Add("@Arbeitsdatum", workTimeDate);
            parameters.Add("@ArbeitszeitVon", workTimeAddition.Start);
            parameters.Add("@ArbeitszeitBis", workTimeAddition.Stop);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spUser_WorkTimeAdditions_Test, parameters);

            return dataTable.Rows.Count;
        }

        public static User_KeyData User_KeyData_GetByUser(Guid userId, DateTime from, DateTime to)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@UserId", userId);
            parameters.Add("@From", from);
            parameters.Add("@To", to);

              DataTable dataTable = SqlHelper.ExecuteDataTable(spUser_KeyData_GetByUser, parameters);

            
            return ConvertToUser_KeyData(dataTable.Rows[0]);
        }

        public static User_KeyData Project_KeyData_GetByUserAndProject(Guid userId, Guid projectId)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@UserId", userId);
            parameters.Add("@ProjectId", projectId);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spProject_KeyData_GetByUserAndProject, parameters);

            return ConvertToUser_KeyData(dataTable.Rows[0]);
        }

        private static User_KeyData ConvertToUser_KeyData(DataRow row)
        {
            User_KeyData user_KeyData = new User_KeyData();

            user_KeyData.SumWorkTime = (double)(row["SumWorkTime"]);
            if (row["CountWorkDays"].ToString() != string.Empty)
                user_KeyData.CountWorkDays = (int)(row["CountWorkDays"]);
            else
                user_KeyData.CountWorkDays = 0;
            
            user_KeyData.AverageTimePerWorkDay = (double)(row["AverageTimePerWorkDay"]);
            user_KeyData.CountOrders = (double)(row["CountOrders"]);
            user_KeyData.SumPauseTime = (double)row["SumPauseTime"];
            user_KeyData.SumPresentTime = (double)row["SumPresentTime"];
            user_KeyData.SumSecondaryTime = (double)row["SumSecondaryTime"];
            user_KeyData.SumTrainingTimeConfirmed = (double)row["SumTrainingTimeConfirmed"];
            user_KeyData.SumTrainingTimeNotConfirmed = (double)row["SumTrainingTimeNotConfirmed"];
            user_KeyData.SumHolidayIllness = (double)row["SumHolidayIllness"];
            user_KeyData.SumPhoneTime = (double)row["SumPhoneTime"];
            user_KeyData.SumNumberOfCalls = (int)row["SumNumberOfCalls"];
            
            return user_KeyData;
        }

        public static WorkTimes[] WorkTimes_ALL_GetByUser(Guid userId, DateTime from, DateTime to)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@UserId", userId);
            parameters.Add("@From", from);
            parameters.Add("@To", to);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spUser_WorkTimes_ALL_GetByUser, parameters);

            return ConvertToWorkTimes(dataTable);
        }

        public static WorkDayList[] WorkDayList_GetByUser(Guid userId, DateTime from, DateTime to)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@UserId", userId);
            parameters.Add("@From", from);
            parameters.Add("@To", to);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spUser_WorkTimes_DayList_GetByUser, parameters);

            return ConvertToWorkDayLists(dataTable);

        }

        private static WorkDayList[] ConvertToWorkDayLists(DataTable dataTable)
        {
            WorkDayList[] workDayList = new WorkDayList[dataTable.Rows.Count];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[i];
                workDayList[i] = ConvertToWorkDayList(row);
            }
            return workDayList;
        }

        private static WorkDayList ConvertToWorkDayList(DataRow row)
        {
            WorkDayList workDayList = new WorkDayList();

            workDayList.WorkDay = (DateTime)(row["WorkDay"]);

            return workDayList;
        }

        public static WorkTimes[] WorkTimes_GROUP_GetByUser(Guid userId, DateTime from, DateTime to)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@UserId", userId);
            parameters.Add("@From", from);
            parameters.Add("@To", to);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spUser_WorkTimes_GROUP_GetByUser, parameters);

            return ConvertToWorkTimes(dataTable);
        }

        private static WorkTimes[] ConvertToWorkTimes(DataTable dataTable)
        {
            WorkTimes[] workTimes = new WorkTimes[dataTable.Rows.Count];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[i];
                workTimes[i] = ConvertToWorkTime(row);
            }
            return workTimes;
        }

        private static WorkTimes ConvertToWorkTime(DataRow row)
        {
            WorkTimes workTime = new WorkTimes();

            workTime.WorkTimesId = (Guid?)SqlHelper.GetNullableDBValue(row["WorkTimesId"]);
            workTime.User = GetUserInfo((Guid)row["UserId"]);
            workTime.WorkDate = (DateTime)row["WorkDate"];
            workTime.Start = (DateTime?)SqlHelper.GetNullableDBValue(row["Start"]);
            workTime.Stop = (DateTime?)SqlHelper.GetNullableDBValue(row["Stop"]);
            workTime.Duration = (double)SqlHelper.GetNullableDBValue(row["Duration"]);
            workTime.UploadMetaWare = (Boolean)row["UploadMetaWare"];
            workTime.RowObject = (string)row["RowObject"];
            workTime.RowDescription = (string)row["RowDescription"];
            workTime.Notice = (string)SqlHelper.GetNullableDBValue(row["Notice"]);

            return workTime;
        }


        private static WorkTimeAdditions[] ConvertToWorkTimeAdditions(DataTable dataTable)
        {
            WorkTimeAdditions[] workTimeAdditions = new WorkTimeAdditions[dataTable.Rows.Count];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[i];
                workTimeAdditions[i] = ConvertToWorkTimeAddition(row);
            }
            return workTimeAdditions;
        }

        private static WorkTimeAdditions ConvertToWorkTimeAddition(DataRow row)
        {
            WorkTimeAdditions workTimeAddition = new WorkTimeAdditions();
            
            workTimeAddition.WorkTimeAdditionId = (Guid)row["WorkTimeAdditionId"];
            workTimeAddition.User = GetUserInfo((Guid)row["UserId"]);
            workTimeAddition.Start = (DateTime)row["Start"];
            workTimeAddition.Stop = (DateTime?)SqlHelper.GetNullableDBValue(row["Stop"]);
            workTimeAddition.Duration = (double)SqlHelper.GetNullableDBValue(row["Duration"]);
            workTimeAddition.Confirmed = (Boolean)row["Confirmed"];
            workTimeAddition.WorkTimeAdditionItemType = (Guid)row["WorkTimeAdditionItemType"];
            workTimeAddition.Notice = (string)SqlHelper.GetNullableDBValue(row["Notice"]);

            return workTimeAddition;
        }

        public static void CreateWorkTimeAddition(WorkTimeAdditions workTimeAdditions)
        {
            IDictionary<string, object> parameters = GetParameters(workTimeAdditions);

            try
            {
                SqlHelper.ExecuteStoredProc(spUser_WorkTimeAdditions_Create, parameters);
            }
            catch (Exception ex)
            {
                Logger.Write(ex.Message, "CreateWorkTimeAddition--" + workTimeAdditions.WorkTimeAdditionId.ToString() , 99, 2, System.Diagnostics.TraceEventType.Transfer, "Fehler-MetaWare-Upload");                
            }

        }

        public static void DeleteWorkTimeAddition(WorkTimeAdditions workTimeAdditions)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@WorkTimeAdditionId", workTimeAdditions.WorkTimeAdditionId);

            try
            {
                SqlHelper.ExecuteStoredProc(spUser_WorkTimeAdditions_Delete, parameters);
            }
            catch (Exception ex)
            {
                Logger.Write(ex.Message, "DeleteWorkTimeAddition--" + workTimeAdditions.WorkTimeAdditionId.ToString(), 99, 2, System.Diagnostics.TraceEventType.Transfer, "Fehler-MetaWare-Upload");                
            }
        }

        public static void UpdateWorkTimeAddition(WorkTimeAdditions workTimeAdditions)
        {
            IDictionary<string, object> parameters = GetParameters(workTimeAdditions);

            try
            {
                SqlHelper.ExecuteStoredProc(spUser_WorkTimeAdditions_Update, parameters);
            }
            catch (Exception ex)
            {
                Logger.Write(ex.Message, "UpdateWorkTimeAddition--" + workTimeAdditions.WorkTimeAdditionId.ToString(), 99, 2, System.Diagnostics.TraceEventType.Transfer, "Fehler-MetaWare-Upload");                
            }
        }

        private static IDictionary<string, object> GetParameters(WorkTimeAdditions workTimeAdditions)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@WorkTimeAdditionId", workTimeAdditions.WorkTimeAdditionId);
            parameters.Add("@UserId", workTimeAdditions.User.UserId);
            parameters.Add("@Start", workTimeAdditions.Start);
            parameters.Add("@Stop", workTimeAdditions.Stop);
            parameters.Add("@Duration", workTimeAdditions.Duration);
            parameters.Add("@WorkTimeAdditionItemType", workTimeAdditions.WorkTimeAdditionItemType);
            parameters.Add("@Confirmed", workTimeAdditions.Confirmed);
            parameters.Add("@Notice", workTimeAdditions.Notice);

            return parameters;
        }



        #endregion


        #region UserInfo
        /// <summary>
        /// Liefert eine Liste mit UserInfo-Objekten die zu dem angegebenen Center gehören
        /// </summary>
        /// <param name="center"></param>
        /// <returns></returns>
        public static UserInfo[] GetUsersByCenter(Guid centerId)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@CenterId", centerId);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spUser_GetListByCenter, parameters);

            return ConvertToUserInfos(dataTable);
        }

        /// <summary>
        /// Liefert eine Liste aller Benutzer, die keine CenterZuordnung haben
        /// </summary>
        /// <returns></returns>
        public static UserInfo[] GetUsersWithOutCenter()
        {
            DataTable dataTable = SqlHelper.ExecuteDataTable(spUser_GetListWithOutCenter);

            return ConvertToUserInfos(dataTable);

        }

        /// <summary>
        /// Liefert eine Liste aller Benutzer, die "ausgeschieden", gelöscht sind
        /// </summary>
        /// <returns></returns>
        public static UserInfo[] GetUsersDeleted()
        {
            DataTable dataTable = SqlHelper.ExecuteDataTable(spUser_GetListDeleted);

            return ConvertToUserInfos(dataTable);

        }

        #endregion


        private static string GetUserProfileXml(User user)
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
                writer.WriteStartElement("UserProfiles");

                if (user.UserProfile != null)
                {
                    writer.WriteStartElement("UserProfile");
                    UserProfile userProfile = user.UserProfile;
                    writer.WriteAttributeString("UserProfileId", userProfile.UserProfileId.ToString());
                    writer.WriteAttributeString("Bezeichnung", userProfile.Bezeichnung);
                    writer.WriteEndElement();

                }
                writer.WriteEndElement();
            }

            return sb.ToString();
        }
        
        private static string GetTeamAssignXml(User user)
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
                writer.WriteStartElement("Teams");

                foreach (TeamAssignInfo assignInfo in user.Teams)
                {
                    writer.WriteStartElement("Team");
                    writer.WriteAttributeString("TeamId", assignInfo.Team.TeamId.ToString());
                    writer.WriteAttributeString("IsTeamLeiter", XmlConvert.ToString(assignInfo.IsTeamLeiter));
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
            }

            return sb.ToString();
        }
        
        private static string GetSecurityGroupsXml(User user)
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
                writer.WriteStartElement("SecurityGroups");

                foreach (SecurityGroup secGroup in user.SecurityGroups)
                {
                    writer.WriteStartElement("SecurityGroup");
                    writer.WriteAttributeString("SecurityGroupId", secGroup.SecurityGroupId.ToString());
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
            }

            return sb.ToString();
        }
        
        private static IDictionary<string, object> GetParameters(User user)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@UserId", user.UserId);
            parameters.Add("@UserName", user.UserName);
            parameters.Add("@Nachname", user.Nachname);
            parameters.Add("@Vorname", user.Vorname);
            parameters.Add("@ReminderEditPermit", user.ReminderEditPermit);
            parameters.Add("@ProjectSearchPermit", user.ProjectSearchPermit);
            parameters.Add("@WorkingTimeEditPermit", user.WorkingTimeEditPermit);
            parameters.Add("@CenterId", user.Center != null ? (Guid?)user.Center.CenterId : null);
            parameters.Add("@UserProfileXml", GetUserProfileXml(user));
            parameters.Add("@TeamAssignXml", GetTeamAssignXml(user));
            parameters.Add("@SecurityGroupsXml", GetSecurityGroupsXml(user));
            parameters.Add("@IsDeleted", user.IsDeleted);
            parameters.Add("@CurrentUser", Guid.NewGuid());
            parameters.Add("@DialMode", user.DialMode);
            parameters.Add("@Dunning", user.Dunning);
            parameters.Add("@AnmeldungEmail", user.AnmeldungEmail);
            parameters.Add("@PasswordEmail", user.PasswordEmail);
            parameters.Add("@AdditionalInfo1", user.AdditionalInfo1);
            parameters.Add("@AdditionalInfo2", user.AdditionalInfo2);

            return parameters;

        }
        
        public static UserInfo[] GetUsersByCallJobGroup(Guid callJobGroupId)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@callJobGroupId", callJobGroupId);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spCallJobGroup_GetUsers, parameters);

            return ConvertToUserInfos(dataTable);
        }

        public static Dictionary<string, string> GetSystemInformation()
        {
            DataTable dataTable = SqlHelper.ExecuteDataTable(spSystemInformation);

            Dictionary<string, string> result = new Dictionary<string, string>(dataTable.Columns.Count);

            foreach (DataColumn column in dataTable.Columns)
            {
                result.Add(column.ColumnName, dataTable.Rows[0][column].ToString());
            }


            return result;
        }

        #region Arbeitszeitberichte
        public static WorkTimeReportResults[] GetWorkTimeReportResults(Guid? centerId,
            Guid? teamId,
            Guid? userId,
            Guid? projectId,
            DateTime start,
            DateTime stop)
        {

            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@Start", start);
            parameters.Add("@Stop", stop);
            parameters.Add("@UserId", userId);
            parameters.Add("@CenterId", centerId);
            parameters.Add("@TeamId", teamId);
            parameters.Add("ProjectId", projectId);


            DataTable dataTable = SqlHelper.ExecuteDataTable(spUserWorkTimes_GetListForReport, parameters);

            return ConvertToWorkTimeReportResults(dataTable);


        }

        private static WorkTimeReportResults[] ConvertToWorkTimeReportResults(DataTable dataTable)
        {
            WorkTimeReportResults[] results = new WorkTimeReportResults[dataTable.Rows.Count];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[i];
                results[i] = ConvertToWorkTimeReportResult(row);
            }

            return results;
        }

        private static string ConvertToString(Object valueObject)
        {
            if (valueObject == null)
                return string.Empty;
            else
                return (string)valueObject;
        }

        private static string ConvertToDoubleString(Object valueObject)
        {
            if (valueObject == null)
            {
                return string.Empty;
            }
            else
            {
                return string.Format("{0:f2}", (double)valueObject / 60);
            }
        }

        private static WorkTimeReportResults ConvertToWorkTimeReportResult(DataRow row)
        {
            WorkTimeReportResults result = new WorkTimeReportResults();
            
            result.UserName = (string) row["Username"];
            result.Center = (string) SqlHelper.GetNullableDBValue(row["Center"]);
            result.Team = (string)SqlHelper.GetNullableDBValue(row["Team"]);
            result.Project = (string)SqlHelper.GetNullableDBValue(row["Project"]);

            result.Day = (string) row["Day"];
            result.Start = (string) row["Start"];
            result.Stop = ConvertToString(SqlHelper.GetNullableDBValue(row["Stop"]));

            /*
            result.WorkTime = ConvertToDoubleString(SqlHelper.GetNullableDBValue(row["WorkTime"]));
            result.PauseTime = ConvertToDoubleString(SqlHelper.GetNullableDBValue(row["PauseTime"]));
            result.TrainingTimeConfirmed = ConvertToDoubleString(SqlHelper.GetNullableDBValue(row["TrainingTimeConfirmed"]));
            result.TrainingTimeNotConfirmed = ConvertToDoubleString(SqlHelper.GetNullableDBValue(row["TrainingTimeNotConfirmed"]));
            result.SecondaryTime = ConvertToDoubleString(SqlHelper.GetNullableDBValue(row["SecondaryTime"]));
            result.PresentTime = ConvertToDoubleString(SqlHelper.GetNullableDBValue(row["PresentTime"]));
            */

            result.WorkTime = (double)row["WorkTime"];
            result.PauseTime = (double)row["PauseTime"];
            result.TrainingTimeConfirmed = (double)row["TrainingTimeConfirmed"];
            result.TrainingTimeNotConfirmed = (double)row["TrainingTimeNotConfirmed"];
            result.SecondaryTime = (double)row["SecondaryTime"];
            result.PresentTime = (double)row["PresentTime"];

            return result;

        }
        #endregion

        #region CallJob-Aktivitäten
        public static UserCallJobActivityResults[] GetUserCallJobActivityResults(Guid? centerId,
                    Guid? teamId,
                    Guid? userId,
                    Guid? projectId,
                    DateTime start,
                    DateTime stop)
        {

            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@Start", start);
            parameters.Add("@Stop", stop);
            parameters.Add("@UserId", userId);
            parameters.Add("@CenterId", centerId);
            parameters.Add("@TeamId", teamId);
            parameters.Add("ProjectId", projectId);


            DataTable dataTable = SqlHelper.ExecuteDataTable(spUserCallJobActivities_GetListForReport, parameters);

            return ConvertToUserCallJobActivityResults(dataTable);


        }

        private static UserCallJobActivityResults[] ConvertToUserCallJobActivityResults(DataTable dataTable)
        {
            UserCallJobActivityResults[] results = new UserCallJobActivityResults[dataTable.Rows.Count];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[i];
                results[i] = ConvertToUserCallJobActivityResult(row);
            }

            return results;
        }

        private static UserCallJobActivityResults ConvertToUserCallJobActivityResult(DataRow row)
        {
            UserCallJobActivityResults result = new UserCallJobActivityResults();
            result.UserName = (string)row["Username"];
            result.Center = (string)SqlHelper.GetNullableDBValue(row["Center"]);
            result.Team = (string)SqlHelper.GetNullableDBValue(row["Team"]);
            result.Project = (string)SqlHelper.GetNullableDBValue(row["Project"]);

            result.Day = (DateTime)row["Day"];
            result.Start = (string)row["Start"];
            result.Stop = (string)row["Stop"];
//            result.WorkTime = FormattingTimeSpan(TimeSpan.FromHours((double)row["WorkTime"]));
            //Anzeige in 100
            result.WorkTime = FormattingDoubleAsHour((double)row["WorkTime"]);

//            result.PauseTime = FormattingTimeSpan(TimeSpan.FromHours((double)row["PauseTime"]));
            //Anzeige in 100
            result.PauseTime = FormattingDoubleAsHour((double)row["PauseTime"]);
            if ((double)row["PauseTime"] != 0.0)
            {
                //result.AveragePauseTime = FormattingTimeSpan(TimeSpan.FromHours((double)row["WorkTime"] / (double)row["PauseTime"]));
                //Ändern auf 100 Ausgabe
                result.AveragePauseTime = FormattingDoubleAsHour((double)row["WorkTime"] / (double)row["PauseTime"]);
            }

            //result.PhoneTime = FormattingTimeSpan(TimeSpan.FromHours((double)row["PhoneTime"]));
            //Ändern auf 100 Ausgabe
            result.PhoneTime = FormattingDoubleAsHour((double)row["PhoneTime"]); 

            if ((int)row["TotalCalls"] != 0.0)
            {
                //result.AveragePhoneTime = FormattingTimeSpan(TimeSpan.FromHours((double)row["PhoneTime"] / (int)row["TotalCalls"]));
                //Ändern auf 100 Ausgabe
                result.AveragePhoneTime = FormattingDoubleAsHour((double)row["PhoneTime"] / (int)row["TotalCalls"]);
            }

            //result.PostEditingTime = FormattingTimeSpan(TimeSpan.FromHours((double)row["PostEditingTime"])); 
            //Ändern auf 100 Ausgabe
            result.PostEditingTime = FormattingDoubleAsHour((double)row["PostEditingTime"]); 

            if ((int)row["TotalCalls"] != 0.0)
            {
                //result.AveragePostEditingTime = FormattingTimeSpan(TimeSpan.FromHours((double)row["PostEditingTime"] / (int)row["TotalCalls"]));
                //Ändern auf 100 Ausgabe
                result.AveragePostEditingTime = FormattingDoubleAsHour((double)row["PostEditingTime"] / (int)row["TotalCalls"]);
            }

            result.TotalCalls = (int)row["TotalCalls"];
            result.Successfull = (int)row["Successfull"];
            result.UnSuccessfull = (int)row["Unsuccessfull"];
            result.UnCommitted = (int)row["UnCommitted"];
            result.Misc = (int)row["Misc"];

            return result;

        }
        #endregion

        private static string FormattingDoubleAsHour(Double doubleTime)
        {
            StringBuilder sb = new StringBuilder();
           // doubleTime = doubleTime / 3600;
            sb.AppendFormat("{0:F4}", doubleTime);

            return sb.ToString();
        }


        private static string FormattingTimeSpan(TimeSpan timeSpan)
        {
            StringBuilder sb = new StringBuilder();


            if (Math.Truncate(timeSpan.TotalSeconds) == 0)
                return null;

            sb.AppendFormat("{0:00}:{1:00}:{2:00}",
                Math.Truncate(timeSpan.TotalHours),
                timeSpan.Minutes,
                timeSpan.Seconds);

            return sb.ToString();
        }

        #region metaware2004-Arbeitszeiten
        public static void CreateMetawareArbeitszeit(
            int partnerNummer,
            int projektNummer,
            string bezeichnung,
            DateTime arbeitsdatum,
            DateTime arbeitszeitVon,
            DateTime arbeitszeitBis,
            ProjectLogOnTimeItem projectLogOnTimeItem)
        {
            projectLogOnTimeItem.UploadMetaWare = 1;
            try
            {
                /* Anfügen der Projektnebenzeit in metaWare-Arbeitszeit
                 * im Try-Block damit wenn speichern nicht funktioniert
                 * im Catch-Block reagiert werden kann.
                 */
                IDictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("@Partnernummer", partnerNummer);
                parameters.Add("@ProjektNummer", projektNummer);
                parameters.Add("@Bezeichnung", bezeichnung);
                parameters.Add("@Arbeitsdatum", arbeitsdatum);
                parameters.Add("@ArbeitszeitVon", arbeitszeitVon);
                parameters.Add("@ArbeitszeitBis", arbeitszeitBis);
                parameters.Add("@WorkTimeAdditionId", projectLogOnTimeItem.LogOnTimeId);

                SqlHelper.ExecuteStoredProc(spUser_CreateMetawareWorkTime, parameters);
            }
            catch(Exception ex)
            {
                /* Speichern in metaWare fehlgeschlagen,
                 * Flag wird gesetzt
                 */
                projectLogOnTimeItem.UploadMetaWare = 0;
                Logger.Write(ex.Message, "CreateMetaWareArbeitszeit--" + projectLogOnTimeItem.LogOnTimeId,99,2, System.Diagnostics.TraceEventType.Transfer,"Fehler-MetaWare-Upload");
            }
            finally
            {
                /* Speichern in metaCall
                 */
                metaCall.DataAccessLayer.ProjectDAL.CreateLogOnTimeItem(projectLogOnTimeItem);
            }
        }

        #endregion

        public static UserInfo GetUserInfo(Guid? userId)
        {
            if (!userId.HasValue)
                return null;

            UserInfo userInfo = null;

            userInfo = ObjectCache.Get<UserInfo>(userId.Value);

            if (userInfo != null)
                return userInfo;

            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@UserId", userId);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spUser_GetSingle, parameters);

            if (dataTable.Rows.Count < 1)
                return null;
            else
                return ConvertToUserInfo(dataTable.Rows[0]);
        }

        #region Abrechnungszeitraum

        public static Boolean WorkTimeEditable(Guid userId, DateTime fromDate)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@UserId", userId);
            parameters.Add("@FromDate", fromDate);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spmwUsers_AbrechnungCount, parameters);

            DataRow row = dataTable.Rows[0];

            int Anzahl = (int)row["Anzahl"];

            if (Anzahl < 2)
                return true;
            else
                return false;
        }
        #endregion

        #region Signatur
        public static UserSignature GetSignature(Guid userId)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@UserId", userId);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spUser_GetSignature, parameters);

            if (dataTable.Rows.Count < 1)
                return null;
            else
                return ConvertToUserSignature(dataTable.Rows[0]);
       }

        public static void SetSignature(Guid userId, string filename)
        {
            //Der in filename angegebenen Dateiname wird geprüft ob es sich um
            // evtl um einen namen auf einem Netzlaufwerk handelt.
            
            DriveInfo[] drives = DriveInfo.GetDrives();
             //drives[0].DriveType == DriveType.
            
            
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@UserId", userId);
            parameters.Add("@SignatureFile", filename);

            SqlHelper.ExecuteStoredProc(spUser_SetSignature, parameters);
        }

        private static UserSignature ConvertToUserSignature(DataRow row)
        {
            string filename = (string) SqlHelper.GetNullableDBValue(row["SignatureFile"]);
            
            if (string.IsNullOrEmpty(filename))
                return null;

            try
            {
                FileInfo file = new FileInfo(filename);
                    if (!file.Exists)
                        return null;

                    byte[] array = new byte[file.Length]; 
                    using (FileStream stream = file.OpenRead())
                    {
                        stream.Read(array, 0, array.Length);

                        stream.Close();
                    }

                    UserSignature userSignature = new UserSignature();
                    userSignature.FileName = file.FullName;
                    userSignature.Signature = array;
                    return userSignature;
                
            }
            catch
            {
                return null;
            }
        }

        public static bool DomainUser_UsesDialer(string domainUser)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@DomainUserName", domainUser);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spDomainUser_UsesDialer, parameters);

            if (dataTable.Rows.Count < 1)
                return false;
            else
                return (bool)dataTable.Rows[0][0];
        }

        public static string DomainUser_GetLine(string domainUser)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@DomainUser", domainUser);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spDomainUser_GetLine, parameters);

            if (dataTable.Rows.Count < 1)
                return string.Empty;
            else
            {
                string line;
                line = (string)SqlHelper.GetNullableDBValue(dataTable.Rows[0][0]);
                return line;
            }
        }

        public static string DomainUser_GetDialingCode(string domainUser)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@DomainUser", domainUser);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spDomainUser_GetDialingCode, parameters);

            if (dataTable.Rows.Count < 1)
                return string.Empty;
            else
            {
                string dialingCode;
                dialingCode = (string)SqlHelper.GetNullableDBValue(dataTable.Rows[0][0]);
                return dialingCode;
            }
        }
        #endregion
    }
}
