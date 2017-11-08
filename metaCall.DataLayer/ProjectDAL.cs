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
    public static class ProjectDAL
    {
        #region StoredProcedures
        private const string spProject_Create = "dbo.Project_Create";
        private const string spProject_Update = "dbo.Project_Update";
        private const string spProject_Delete = "dbo.Project_Delete";
        private const string spProject_GetSingle = "dbo.Project_GetSingle";
        private const string spProject_GetAllActiveProjects = "dbo.Project_GetListAllActive";

        private const string spProject_GetByCenter = "dbo.Project_GetListByCenter";
        private const string spProject_GetByTeam = "dbo.Project_GetListByTeam";
        private const string spProject_GetByUser = "dbo.Project_GetListByUser";
        private const string spProject_GetByUser_KeyData = "dbo.Project_GetListByUser_KeyData";
        private const string spProject_GetListByProjectState = "dbo.Project_GetListByProjectState";
        private const string spProject_GetListByProjectStateAndCenter = "dbo.Project_GetListByProjectStateAndCenter";
        private const string spProject_GetListByProjectStateAndUser = "dbo.Project_GetListByProjectStateAndUser";
        private const string spProject_GetListByProjectStateAndTeam = "dbo.Project_GetListByProjectStateAndTeam";
                                                                            
        private const string spProject_LogOnTimeItemCreate = "dbo.Project_LogOnTimeItemCreate";
        private const string spProject_LogOnTimeItemUpdate = "dbo.Project_LogOnTimeItemUpdate";

        private const string spProjectState_GetAll = "dbo.ProjectState_GetAll";
        private const string spProjectState_GetSingle = "dbo.ProjectState_GetSingle";

        private const string spProject_SetLastCall = "dbo.Project_SetLastCall";
        #endregion
       
        public static void CreateProject(Project project)
        {
            IDictionary<string, object> parameters = GetParameters(project);
            parameters.Add("@TargetVereinsAddressType", typeof(Customer).FullName);
            SqlHelper.ExecuteStoredProc(spProject_Create, parameters);

            //HACK: Projekt abrufen und Verein setzen
            Project newProject = GetProject(project.ProjectId);
            project.Customer = newProject.Customer;
        }

        /// <summary>
        /// Bereitet das Projekt für die letzte Anrufrunde vor
        /// Setzten der Team-Reminder auf erledigt
        /// Setzten der CallJobIterationsCounter auf Projektmax - 1
        /// Setzten der CallJobart auf weiterer Anruf wenn Reminder war
        /// </summary>
        /// <param name="project"></param>
        public static void SetLastCall(Project project)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@ProjectId", project.ProjectId);

            SqlHelper.ExecuteStoredProc(spProject_SetLastCall, parameters);
        }

        public static void UpdateProject(Project project)
        {
            
            //Update-Prozedur hat andere Parameter als Create
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@ProjectId", project.ProjectId);
            parameters.Add("@Bezeichnung", project.Bezeichnung);
            parameters.Add("@BezeichnungRechnung", project.BezeichnungRechnung);
            parameters.Add("@mwProjektnummer", project.mwProject != null ? (int?)project.mwProject.Projektnummer : null);
            parameters.Add("@Status", (int)project.State);
            parameters.Add("@deleted", project.isDeleted);
            parameters.Add("@IterationCounter", project.IterationCounter);
            parameters.Add("@CustomerAddressId", project.Customer != null ? (Guid?)project.Customer.AddressId : null);
            parameters.Add("@DialMode", (int)project.DialMode);
            parameters.Add("@TeamsXml", GetTeamsXml(project));
            parameters.Add("@CallJobGroupsXml", GetCallJobGroupsXml(project));
            parameters.Add("@ReminderDateMax", project.ReminderDateMax);
            parameters.Add("@AddressSafeActiv", project.AddressSafeActiv);
            if (project.Center == null)
                parameters.Add("@CenterId", null);
            else
                parameters.Add("@CenterId", project.Center.CenterId);
            parameters.Add("@DialingPrefixNumber", project.DialingPrefixNumber);
            parameters.Add("@Venue", project.Venue);
            parameters.Add("@PraefixMailAttachment", project.PraefixMailAttachment);
 
            SqlHelper.ExecuteStoredProc(spProject_Update, parameters);
        }

        private static string GetCallJobGroupsXml(Project project)
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
                writer.WriteStartElement("CallJobGroups");

                foreach (CallJobGroup callJobGroup in project.CallJobGroups)
                {
                    writer.WriteStartElement("CallJobGroup");
                    writer.WriteAttributeString("CallJobGroupId", callJobGroup.CallJobGroupId.ToString());
                    writer.WriteAttributeString("DisplayName", callJobGroup.DisplayName);
                    writer.WriteAttributeString("Description", callJobGroup.Description);
                    writer.WriteAttributeString("Key", callJobGroup.Key);
                    writer.WriteAttributeString("Ranking", callJobGroup.Ranking.ToString());
                    writer.WriteAttributeString("CallJobGroupType", ((int)callJobGroup.Type).ToString());
                    writer.WriteStartElement("Teams");
                    foreach (TeamInfo teamInfo in callJobGroup.Teams)
                    {
                        writer.WriteStartElement("Team");
                        writer.WriteAttributeString("CallJobGroupId", callJobGroup.CallJobGroupId.ToString());
                        writer.WriteAttributeString("TeamId", teamInfo.TeamId.ToString());
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                    writer.WriteStartElement("Users");
                    foreach (UserInfo userInfo in callJobGroup.Users)
                    {
                        writer.WriteStartElement("User");
                        writer.WriteAttributeString("CallJobGroupId", callJobGroup.CallJobGroupId.ToString());
                        writer.WriteAttributeString("UserId", userInfo.UserId.ToString());
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }

            return sb.ToString();
        }

        private static string GetTeamsXml(Project project)
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

                foreach (TeamInfo team in project.Teams)
                {
                    writer.WriteStartElement("Team");
                    writer.WriteAttributeString("TeamId", team.TeamId.ToString());
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
            }

            return sb.ToString();
        }

        public static void DeleteProject(Guid ProjectId)
        {
            IDictionary<string, object> parameters = new Dictionary<string,object>();
            parameters.Add("@ProjectId", ProjectId);

            SqlHelper.ExecuteStoredProc(spProject_Delete, parameters);
        }
        
        public static Project GetProject(Guid projectId)
        {

            IDictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@ProjectId", projectId);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spProject_GetSingle, parameters);

            if (dataTable.Rows.Count < 1)
                return null;
            else
                return ConvertToProject(dataTable.Rows[0]);
        }

        public static ProjectInfo[] GetAllProjects()
        {
            DataTable dataTable = SqlHelper.ExecuteDataTable(spProject_GetAllActiveProjects);
            return ConvertToProjectInfos(dataTable);
        }

        private static ProjectInfo[] ConvertToProjectInfos(DataTable dataTable)
        {
            ProjectInfo[] projectInfos = new ProjectInfo[dataTable.Rows.Count];

            if (dataTable.Columns.IndexOf("ProjectMonthYear") == -1)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    DataRow row = dataTable.Rows[i];
                    projectInfos[i] = ConvertToProjectInfo(row, false);
                }
            }
            else
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    DataRow row = dataTable.Rows[i];
                    projectInfos[i] = ConvertToProjectInfo(row, true);
                }
            }

            return projectInfos;
        
        }

        private static ProjectInfo ConvertToProjectInfo(DataRow row, bool projectMonthYear)
        {
            ProjectInfo projectInfo = new ProjectInfo();

            projectInfo.ProjectId = (Guid)row["ProjectId"];
            if (projectMonthYear)
                projectInfo.Bezeichnung = (string)row["Bezeichnung"] + " " + row["ProjectMonthYear"];
            else
                projectInfo.Bezeichnung = (string)row["Bezeichnung"];
            projectInfo.BezeichnungRechnung = (string)row["BezeichnungRechnung"];
            projectInfo.mwProjektNummer = (int?)row["mwProjektNummer"];
            projectInfo.IterationCounter = (int)row["IterationCounter"];
            projectInfo.State = (ProjectState)row["Status"];
            projectInfo.DialMode = (DialMode)row["DialMode"];
            projectInfo.DialingPrefixNumber = (string)SqlHelper.GetNullableDBValue(row["DialingPrefixNumber"]);
            projectInfo.Venue = (string)SqlHelper.GetNullableDBValue(row["Venue"]);
            projectInfo.CustomerAdressId = (Guid?)SqlHelper.GetNullableDBValue(row["CustomerAddressId"]);
            projectInfo.ReminderDateMax = (DateTime?)SqlHelper.GetNullableDBValue(row["ReminderDateMax"]);

            ObjectCache.Add(projectInfo.ProjectId, projectInfo, TimeSpan.FromSeconds(30));

            return projectInfo;
        }

        private static Project ConvertToProject(DataRow row)
        {

            Project project = new Project();

            project.ProjectId = (Guid) row["ProjectId"];
            project.Bezeichnung = (string) row["Bezeichnung"];
            project.BezeichnungRechnung = (string)row["BezeichnungRechnung"];
            project.State = (ProjectState) row["Status"];
            project.isDeleted = (bool) row["deleted"];
            project.IterationCounter = (int) row["IterationCounter"];
            project.DialMode = (DialMode)row["DialMode"];
            project.DialingPrefixNumber = (string)SqlHelper.GetNullableDBValue(row["DialingPrefixNumber"]);
            project.Venue = (string)SqlHelper.GetNullableDBValue(row["Venue"]);
            project.ReminderDateMax = (DateTime?)SqlHelper.GetNullableDBValue(row["ReminderDateMax"]);
            project.Center = CenterDAL.GetCenterInfo((Guid?) row["CenterId"]);
            project.AddressSafeActiv = (bool)row["AddressSafeActiv"];
            project.PraefixMailAttachment = (string) row["PraefixMailAttachment"];
            //Abrufen des Metaware-Projekts
            project.mwProject = mwProjectDAL.GetMwProject((int) row["mwProjektNummer"]);
            
            //Abrufen des Kunden (Verein o. Schule)
            Guid? customerAddressId = (Guid?)SqlHelper.GetNullableDBValue(row["CustomerAddressId"]);
            project.Customer = AddressDAL.GetCustomer(customerAddressId);
            //Abrufen der Teams
            project.Teams = TeamDAL.GetTeamsByProject(project.ProjectId);
            //Abrufen der CallJobGruppen
            project.CallJobGroups = CallJobGroupDAL.GetCallJobGroupsByProject(project.ProjectId);



            return project;
        }

        //ACHTUNG -> liefert nur die Parameter für CreateProject
        private static IDictionary<string, object> GetParameters(Project project)
        {
            //Prüfungen der project-Eigenschaften ob diese gültige Werte haben

            //Parameter erstellen
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@ProjectId", project.ProjectId);
            parameters.Add("@Bezeichnung", project.Bezeichnung);
            parameters.Add("@BezeichnungRechnung", project.BezeichnungRechnung);
            parameters.Add("@CurrentUser", Guid.NewGuid());
            parameters.Add("@ReminderDateMax", project.ReminderDateMax);
            parameters.Add("@State", project.State);
            if (project.mwProject == null )
                parameters.Add("@mwProjektNummer", 0);
            else
                parameters.Add("@mwProjektNummer", project.mwProject.Projektnummer);

            parameters.Add("@DialMode", project.DialMode);
            if (project.Center == null)
                parameters.Add("@CenterId", null);
            else
                parameters.Add("@CenterId", project.Center.CenterId);
            parameters.Add("@Venue", project.Venue);
            parameters.Add("@PraefixMailAttachment", project.PraefixMailAttachment);

            return parameters;
        }

        public static ProjectInfo[] GetProjectByTeam(Guid teamId)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@teamId", teamId);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spProject_GetByTeam, parameters);

            return ConvertToProjectInfos(dataTable);
        }

        public static ProjectInfo[] GetProjectByCenter(Guid centerId)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@centerId", centerId);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spProject_GetByCenter, parameters);

            return ConvertToProjectInfos(dataTable);
        }

        public static ProjectInfo[] GetProjectByUser(Guid userId)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@UserId", userId);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spProject_GetByUser, parameters);

            return ConvertToProjectInfos(dataTable);
        }

        public static ProjectInfo[] GetProjectByUser_KeyData(Guid userId)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@UserId", userId);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spProject_GetByUser_KeyData, parameters);

            return ConvertToProjectInfos(dataTable);
        }

        public static ProjectInfo GetProjectInfo(Guid projectId)
        {
            ProjectInfo projectInfo = null;

            projectInfo = ObjectCache.Get<ProjectInfo>(projectId);

            if (projectInfo != null)
                return projectInfo;
            
            IDictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@ProjectId", projectId);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spProject_GetSingle, parameters);

            if (dataTable.Rows.Count < 1)
                return null;
            else
            {
                return ConvertToProjectInfo(dataTable.Rows[0], (dataTable.Columns.IndexOf("ProjectMonthYear") != -1));
            }
        }

        public static ProjectInfo[] GetProjectInfosByProjectState(ProjectState state)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@ProjectState", state);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spProject_GetListByProjectState, parameters);

            return ConvertToProjectInfos(dataTable);
        }

        public static ProjectInfo[] GetProjectInfosByProjectStateAndCenter(ProjectState state, Guid centerId)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@ProjectState", state);
            parameters.Add("@CenterId", centerId);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spProject_GetListByProjectStateAndCenter, parameters);

            return ConvertToProjectInfos(dataTable);
        }

        public static ProjectInfo[] GetProjectInfosByProjectStateAndUser(ProjectState state, User user)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@ProjectState", state);
            parameters.Add("@UserId", user.UserId);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spProject_GetListByProjectStateAndUser, parameters);

            return ConvertToProjectInfos(dataTable);
        }

        public static ProjectInfo[] GetProjectInfosByProjectStateAndTeam(ProjectState state, Team team)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@ProjectState", state);
            parameters.Add("@TeamId", team.TeamId);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spProject_GetListByProjectStateAndTeam, parameters);

            return ConvertToProjectInfos(dataTable);
        }

        public static void CreateLogOnTimeItem(ProjectLogOnTimeItem projectLogOnTimeItem)
        {
            IDictionary<string, object> parameters = GetParameters(projectLogOnTimeItem);
            SqlHelper.ExecuteStoredProc(spProject_LogOnTimeItemCreate, parameters);
        }

        public static void UpdateLogOnTimeItem(ProjectLogOnTimeItem projectLogOnTimeItem)
        {
            IDictionary<string, object> parameters = GetParameters(projectLogOnTimeItem);
            SqlHelper.ExecuteStoredProc(spProject_LogOnTimeItemUpdate, parameters);
        }

        private static IDictionary<string, object> GetParameters(ProjectLogOnTimeItem projectLogOnTimeItem)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@LogOnTimeId", projectLogOnTimeItem.LogOnTimeId);
            parameters.Add("@UserId", projectLogOnTimeItem.User.UserId);
            parameters.Add("@ProjectId",projectLogOnTimeItem.Project.ProjectId) ;
            parameters.Add("@Start", projectLogOnTimeItem.Start);
            parameters.Add("@Stop", projectLogOnTimeItem.Stop);
            parameters.Add("@Duration", projectLogOnTimeItem.Duration);
            parameters.Add("@LogOnTimeItemType", projectLogOnTimeItem.LogOnTimeItemType);
            parameters.Add("@StartActivityId", projectLogOnTimeItem.StartActivityId);
            parameters.Add("@StopActivityId", projectLogOnTimeItem.StopActivityId);
            parameters.Add("@UploadMetaWare", projectLogOnTimeItem.UploadMetaWare);

            return parameters;
        }

        public static ProjectStateInfo GetProjectStateInfo(ProjectState projectState)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@ProjectStateId", projectState);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spProjectState_GetSingle, parameters);

            if (dataTable.Rows.Count < 1)
                return null;
            else
                return ConvertToProjectStateInfo(dataTable.Rows[0]);
        }

        public static ProjectStateInfo[] GetProjectStateInfos()
        {
            DataTable dataTable = SqlHelper.ExecuteDataTable(spProjectState_GetAll);

            return ConvertToProjectStateInfos(dataTable);
        }

        private static ProjectStateInfo[] ConvertToProjectStateInfos(DataTable dataTable)
        {
            ProjectStateInfo[] infos = new ProjectStateInfo[dataTable.Rows.Count];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[i];
                infos[i] = ConvertToProjectStateInfo(row);
            }

            return infos;
        }

        private static ProjectStateInfo ConvertToProjectStateInfo(DataRow row)
        {
            ProjectStateInfo info = new ProjectStateInfo();
            info.ProjectState = (ProjectState)row["ProjectStateId"];
            info.DisplayName = (string)row["DisplayName"];

            return info;
        }
    }
}
