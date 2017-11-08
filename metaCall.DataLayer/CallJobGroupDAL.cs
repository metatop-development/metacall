using System;
using System.Collections.Generic;
using System.Text;

using System.Data;

using metatop.Applications.metaCall.DataObjects;
using System.Xml;

namespace metatop.Applications.metaCall.DataAccessLayer
{
    public static class CallJobGroupDAL
    {

        #region StoredProcedures
        private static string spCallJobGroup_Create = "dbo.CallJobGroup_Create";
        private static string spCallJobGroup_Update = "dbo.CallJobGroup_Update";
        private static string spCallJobGroup_Delete = "dbo.CallJobGroup_Delete";
        private static string spCallJobGroup_GetSingle = "dbo.CallJobGroup_GetSingle";
        private static string spCallJobGroup_GetAllByProject = "dbo.CallJobGroup_GetAllByProject";
        private static string spCallJobGroup_GetAllByUser = "dbo.CallJobGroup_GetAllByUser";

        private static string spCallJobGroupType_GetSingle = "dbo.CallJobGroupType_GetSingle";
        private static string spCallJobGroupType_GetAll = "dbo.CallJobGroupType_GetAll";

        #endregion

        public static void CreateCallJobGroup(CallJobGroup callJobGroup)
        {
            IDictionary<string, object> parameters = GetParameters(callJobGroup);
            SqlHelper.ExecuteStoredProc(spCallJobGroup_Create, parameters);
        }

        public static void UpdateCallJobGroup(CallJobGroup callJobGroup)
        {
            IDictionary<string, object> parameters = GetParameters(callJobGroup);
            SqlHelper.ExecuteStoredProc(spCallJobGroup_Update, parameters);
        }

        public static void DeleteCallJobGroup(Guid callJobGroupId)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@CallJobGroupId", callJobGroupId);

            SqlHelper.ExecuteStoredProc(spCallJobGroup_Delete, parameters);
        }

        public static CallJobGroup GetCallJobGroup(Guid? callJobGroupId)
        {
            if (!callJobGroupId.HasValue)
                return null;


            CallJobGroup callJobGroup = ObjectCache.Get<CallJobGroup>(callJobGroupId.Value);

            if (callJobGroup != null)
                return callJobGroup;
            
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@CallJobGroupId", callJobGroupId.Value);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spCallJobGroup_GetSingle, parameters);

            if (dataTable.Rows.Count < 1)
                return null;
            else
                return ConvertToCallJobGroup(dataTable.Rows[0]);
        }

        public static CallJobGroupInfo[] GetCallJobGroupInfosByUser(Guid userId, Guid? teamId, Guid projectId)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@UserId", userId);
            parameters.Add("@TeamId", teamId);
            parameters.Add("@ProjectId", projectId);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spCallJobGroup_GetAllByUser, parameters);

            return ConvertToCallJobGroupInfos(dataTable);
        }
      
        private static IDictionary<string, object> GetParameters(CallJobGroup callJobGroup)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@CallJobGroupId", callJobGroup.CallJobGroupId);
            parameters.Add("@ProjectId", callJobGroup.Project.ProjectId);
            parameters.Add("@DisplayName", callJobGroup.DisplayName);
            parameters.Add("@Description", callJobGroup.Description);
            parameters.Add("@CallJobGroupType", (int)callJobGroup.Type);
            parameters.Add("@Ranking", callJobGroup.Ranking);
            parameters.Add("@Key", callJobGroup.Key);
            parameters.Add("@CallJobGroupUsersXml", GetCallJobGroupUsersXml(callJobGroup));
            parameters.Add("@CallJobGroupTeamsXml", GetCallJobGroupTeamsXml(callJobGroup));
            return parameters;
        }

        private static string GetCallJobGroupTeamsXml(CallJobGroup callJobGroup)
        {
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;

            using (XmlWriter writer = XmlWriter.Create(sb, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("Teams");

                foreach (TeamInfo teamInfo in callJobGroup.Teams)
                {
                    writer.WriteStartElement("Team");
                    writer.WriteAttributeString("TeamId", teamInfo.TeamId.ToString());
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
            }

            return sb.ToString();
        }

        private static string GetCallJobGroupUsersXml(CallJobGroup callJobGroup)
        {
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;

            using (XmlWriter writer = XmlWriter.Create(sb, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("Users");

                foreach (UserInfo userInfo in callJobGroup.Users)
                {
                    writer.WriteStartElement("User");
                    writer.WriteAttributeString("UserId", userInfo.UserId.ToString());
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
            }

            return sb.ToString();
        }

        public static CallJobGroup[] GetCallJobGroupsByProject(Guid projectId)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@ProjectId", projectId);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spCallJobGroup_GetAllByProject, parameters);

            return ConvertToCallJobGroups(dataTable);

        }

        public static CallJobGroupInfo[] GetCallJobGroupInfosByProject(Guid projectId)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@ProjectId", projectId);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spCallJobGroup_GetAllByProject, parameters);

            return ConvertToCallJobGroupInfos(dataTable);

        }
        
        private static CallJobGroup ConvertToCallJobGroup(DataRow row)
        {

            CallJobGroup callJobGroup = new CallJobGroup();
            callJobGroup.CallJobGroupId = (Guid)row["CallJobGroupId"];
            callJobGroup.Project = ProjectDAL.GetProjectInfo((Guid)row["ProjectId"]);
            callJobGroup.DisplayName = (string)row["DisplayName"];
            callJobGroup.Description = (string)row["Description"];
            callJobGroup.Type = (CallJobGroupType)row["CallJobGroupType"];
            callJobGroup.Ranking = (int) row["Ranking"];
            callJobGroup.Key = (string)row["Key"];
            callJobGroup.Count = (int)row["Count"];
            
            callJobGroup.Teams = TeamDAL.GetTeamsByCallJobGroup(callJobGroup.CallJobGroupId);
            callJobGroup.Users = UserDAL.GetUsersByCallJobGroup(callJobGroup.CallJobGroupId);

            ObjectCache.Add(callJobGroup.CallJobGroupId, callJobGroup, TimeSpan.FromSeconds(30));

            return callJobGroup;
        }

        private static CallJobGroup[] ConvertToCallJobGroups(DataTable dataTable)
        {
            CallJobGroup[] groups = new CallJobGroup[dataTable.Rows.Count];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[i];
                groups[i] = ConvertToCallJobGroup(row);
            }

            return groups;
        }

        private static CallJobGroupInfo ConvertToCallJobGroupInfo(DataRow row)
        {

            CallJobGroupInfo callJobGroup = new CallJobGroupInfo();
            callJobGroup.CallJobGroupId = (Guid)row["CallJobGroupId"];
            callJobGroup.DisplayName = (string)row["DisplayName"];
            callJobGroup.Type = (CallJobGroupType)row["CallJobGroupType"];
            callJobGroup.Ranking = (int)row["Ranking"];
            
            return callJobGroup;
        }

        private static CallJobGroupInfo[] ConvertToCallJobGroupInfos(DataTable dataTable)
        {
            CallJobGroupInfo[] groups = new CallJobGroupInfo[dataTable.Rows.Count];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[i];
                groups[i] = ConvertToCallJobGroupInfo(row);
            }

            return groups;
        }
        
        public static CallJobGroupTypeInfo[] GetCallJobGroupTypeInfos()
        {
            DataTable dataTable = SqlHelper.ExecuteDataTable(spCallJobGroupType_GetAll);
            return ConvertToCallJobGroupTypeInfos(dataTable);
        }

        public static CallJobGroupTypeInfo GetCallJobGroupTypeInfo(CallJobGroupType callJobGroupType)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@CallJobGroupTypeId", (int)callJobGroupType);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spCallJobGroupType_GetSingle, parameters);

            if (dataTable.Rows.Count < 0)
                return null;
            else
                return ConvertToCallJobGroupTypeInfo(dataTable.Rows[0]);
        }

        private static CallJobGroupTypeInfo[] ConvertToCallJobGroupTypeInfos(DataTable dataTable)
        {
            CallJobGroupTypeInfo[] infos = new CallJobGroupTypeInfo[dataTable.Rows.Count];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[i];
                infos[i] = ConvertToCallJobGroupTypeInfo(row);
            }

            return infos;
        }

        private static CallJobGroupTypeInfo ConvertToCallJobGroupTypeInfo(DataRow row)
        {
            CallJobGroupTypeInfo info = new CallJobGroupTypeInfo();

            info.CallJobGroupType = (CallJobGroupType)row["CallJobGroupTypeId"];
            info.DisplayName = (string)row["DisplayName"];
            info.Description = (string)row["Description"];
            info.DisplayNameTemplate =(string) SqlHelper.GetNullableDBValue(row["DisplayNameTemplate"]);
            info.Ranking = (int) row["Ranking"];

            return info;

        }

        public static CallJobGroupInfo GetCallJobGroupInfo(Guid? callJobGroupId)
        {
            if (!callJobGroupId.HasValue)
                return null;

            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@CallJobGroupId", callJobGroupId.Value);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spCallJobGroup_GetSingle, parameters);

            if (dataTable.Rows.Count < 1)
                return null;
            else
                return ConvertToCallJobGroupInfo(dataTable.Rows[0]);
        }
    }
}
