using System;
using System.Collections.Generic;
using System.Text;

using metatop.Applications.metaCall.DataObjects;

using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.Common;

using System.Xml;


namespace metatop.Applications.metaCall.DataAccessLayer
{
    public static class TeamDAL
    {
        #region stored Procedures
        private const string spTeam_Create = "dbo.Team_Create";
        private const string spTeam_Update = "dbo.Team_Update";
        //private const string spTeam_Delete = "dbo.Team_Delete";
        private const string spTeam_GetSingle = "dbo.Team_GetSingle";
        private const string spTeam_GetAllActiveTeams = "dbo.Team_GetAllActiveTeams";
        private const string spTeam_GetByUser = "dbo.Team_GetListByUser";
        private const string spTeam_GetByCenter = "dbo.Team_GetListByCenter";
        private const string spTeam_GetByWithoutCenter = "dbo.Team_GetListByWithoutCenter";
        private const string spTeam_GetByDeleted = "dbo.Team_GetListByDeleted";
        private const string spTeam_GetByProject = "dbo.Team_GetByProject";
        private const string spCallJobGroup_GetTeams = "dbo.Team_GetListByCallJobGroup";
        private const string spTeam_UpdateProjects = "dbo.Team_UpdateProjects";

        #endregion

        private static Team ConvertToTeam(DataRow row)
        {
            Team team = new Team();

            string beschreibung = (string) SqlHelper.GetNullableDBValue(row["Beschreibung"]);
            Guid? centerId = (Guid?)SqlHelper.GetNullableDBValue(row["CenterId"]);
            
            team.TeamId = (Guid)row["TeamId"];
            team.Bezeichnung = (string)row["Bezeichnung"];
            team.Beschreibung = beschreibung;
            team.IsDeleted = (bool)row["deleted"];

            if (centerId.HasValue)
                team.Center = CenterDAL.GetCenterInfo(centerId.Value);
            else
                team.Center = null;

            team.TeamMitglieder = UserDAL.GetUsersByTeam(team.TeamId);
            team.Projects = ProjectDAL.GetProjectByTeam(team.TeamId);
            

            return team;
        }

        private static Team[] ConvertToTeams(DataTable dataTable)
        {
            Team[] teams = new Team[dataTable.Rows.Count];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[i];
                teams[i] = ConvertToTeam(row);
            }

            return teams;
        }

        private static TeamInfo ConvertToTeamInfo(DataRow row)
        {
            TeamInfo teamInfo = new TeamInfo();

            string beschreibung = (string)SqlHelper.GetNullableDBValue(row["Beschreibung"]);

            teamInfo.TeamId = (Guid)row["TeamId"];
            teamInfo.Bezeichnung = (string)row["Bezeichnung"];
            teamInfo.Beschreibung = beschreibung;

            return teamInfo;
        }

        private static TeamInfo[] ConvertToTeamInfos(DataTable dataTable)
        {
            TeamInfo[] teamInfos = new TeamInfo[dataTable.Rows.Count];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[i];
                teamInfos[i] = ConvertToTeamInfo(row);
            }

            return teamInfos;
        }

        public static void CreateTeam(Team team)
        {
            IDictionary<string, object> parameters = GetParameters(team);
            SqlHelper.ExecuteStoredProc(spTeam_Create, parameters);
        }

        //public static void DeleteTeam(Guid teamId)
        //{
        //    IDictionary<string, object> parameters = new Dictionary<string, object>();
        //    parameters.Add("@TeamId", teamId);
        //    SqlHelper.ExecuteStoredProc(spTeam_Delete, parameters);

        //}

        public static TeamInfo[] GetAllTeams()
        {
            DataTable dataTable = SqlHelper.ExecuteDataTable(spTeam_GetAllActiveTeams);
            return ConvertToTeamInfos(dataTable);
        }

        private static IDictionary<string, object> GetParameters(Team team)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@TeamId", team.TeamId);
            parameters.Add("@Bezeichnung", team.Bezeichnung);
            parameters.Add("@Beschreibung", team.Beschreibung);
            parameters.Add("@CurrentUser", Guid.NewGuid());
            parameters.Add("@deleted", team.IsDeleted);

            if (team.Center == null)
               parameters.Add("@CenterId", null);
            else
               parameters.Add("@CenterId", team.Center.CenterId);

           parameters.Add("@ProjectAssignXml", GetProjectsXml(team));
            
            return parameters;
        }

        public static Team GetTeam(Guid teamId)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@TeamId", teamId);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spTeam_GetSingle, parameters);

            return ConvertToTeam(dataTable.Rows[0]);
        }

        public static Team GetTeam(Guid? teamId)
        {
            if (!teamId.HasValue)
                return null;

            return GetTeam(teamId.Value);
        }

        public static TeamInfo GetTeamInfo(Guid teamId)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@TeamId", teamId);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spTeam_GetSingle, parameters);

            return ConvertToTeamInfo(dataTable.Rows[0]);
        }

        public static TeamInfo GetTeamInfo(Guid? teamId)
        {
            if (teamId.HasValue)
                return GetTeamInfo(teamId.Value);
            else
                return null;
        }

        public static TeamInfo[] GetTeamsByUser(Guid userId)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@userId", userId);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spTeam_GetByUser, parameters);

            return ConvertToTeamInfos(dataTable);
        }


        public static TeamAssignInfo[] GetTeamAssignsByUser(Guid userId)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@userId", userId);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spTeam_GetByUser, parameters);

            return ConvertToTeamAssignInfos(dataTable);

        }

        private static TeamAssignInfo[] ConvertToTeamAssignInfos(DataTable dataTable)
        {
            TeamAssignInfo[] assignInfos = new TeamAssignInfo[dataTable.Rows.Count];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[0];

                assignInfos[i] = ConvertToTeamAssignInfo(row);
            }

            return assignInfos;
        }

        private static TeamAssignInfo ConvertToTeamAssignInfo(DataRow row)
        {
            TeamAssignInfo assigninfo = new TeamAssignInfo();
            assigninfo.Team = ConvertToTeamInfo(row);
            assigninfo.IsTeamLeiter = (bool)row["IsTeamLeiter"];

            return assigninfo;
        }

        public static void UpdateTeam(Team team)
        {
            IDictionary<string, object> parameters = GetParameters(team);
            SqlHelper.ExecuteStoredProc(spTeam_Update, parameters);
        }

        public static void UpdateTeam(Team team, Project project)
        {
            IDictionary<string, object> parameters = GetParameters(team);
            SqlHelper.ExecuteStoredProc(spTeam_Update, parameters);
        }

        private static string GetProjectsXml(Team team)
        {
            //Erstellt eine XML-Zeichenfolge der Art
            // <centerAdmins>
            //    <CenterAdmin UserId=".." />
            // </CenterAdmins>

            if (team.Projects == null)
                return string.Empty;
            else
            {
                StringBuilder sb = new StringBuilder();
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.OmitXmlDeclaration = true;

                using (XmlWriter writer = XmlWriter.Create(sb, settings))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("Projects");

                    foreach (ProjectInfo project in team.Projects)
                    {
                        writer.WriteStartElement("Project");
                        writer.WriteAttributeString("ProjectId", project.ProjectId.ToString());
                        writer.WriteEndElement();
                    }

                    writer.WriteEndElement();
                }

                return sb.ToString();
            }
        }

        public static TeamInfo[] GetTeamsByCenter(Guid centerId)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@CenterId", centerId);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spTeam_GetByCenter, parameters);

            return ConvertToTeamInfos(dataTable);

        }

        public static TeamInfo[] GetTeamsByWithoutCenter()
        {
            DataTable dataTable = SqlHelper.ExecuteDataTable(spTeam_GetByWithoutCenter);

            if (dataTable == null)
                return null;
            else
                return ConvertToTeamInfos(dataTable);
        }

        public static TeamInfo[] GetTeamsByDeleted()
        {
            DataTable dataTable = SqlHelper.ExecuteDataTable(spTeam_GetByDeleted);

            return ConvertToTeamInfos(dataTable);
        }

        public static TeamInfo[] GetTeamsByProject(Guid projectId)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@ProjectId", projectId);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spTeam_GetByProject, parameters);

	        return ConvertToTeamInfos(dataTable);
        }

        public static TeamInfo[] GetTeamsByCallJobGroup(Guid callJobGroupId)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@CallJobGroupId", callJobGroupId);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spCallJobGroup_GetTeams, parameters);

            return ConvertToTeamInfos(dataTable);
        }
    }
}
