using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using metatop.Applications.metaCall.DataObjects;
using metatop.Applications.metaCall.DataAccessLayer;
using System.Threading;

namespace metatop.Applications.metaCall.ServiceAccessLayer
{
    #region DataAccessLayer Class
    /// <summary>
    /// Global DataLayer Class
    /// </summary>
    /// <remarks>
    /// implements the SingletonPattern. Only one instance can be exist 
    /// in the current AppDomain
    /// </remarks>
    public class ServiceAccess
    {
        #region Constructor
        public ServiceAccess()
        {
        }
        #endregion

        #region User
        public Dictionary<string, string> GetSystemInformation()
        {
            return UserDAL.GetSystemInformation();
        }

        public User GetUser(UserInfo userInfo)
        {
            if (userInfo == null)
            {
                throw new ArgumentNullException("userInfo");
            }

            return UserDAL.GetUser(userInfo.UserId);
        }

        public User GetUser(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentNullException("userId");
            }

            return UserDAL.GetUser(userId);
        }

        public User GetUser(string userName)
        {
            if (userName == null)
            {
                throw new ArgumentNullException("userName");
            }

            return UserDAL.GetUser(userName);
        }

        public UserInfo[] GetAllUsers()
        {
            return UserDAL.GetAllUsers();
        }

        public UserInfo[] GetUsersWithOutCenter()
        {
            return UserDAL.GetUsersWithOutCenter();
        }

        public UserInfo[] GetUsersDeleted()
        {
            return UserDAL.GetUsersDeleted();
        }

        public UserInfo[] GetUsersByCenter(Guid centerId)
        {
            if (centerId == Guid.Empty)
            {
                throw new ArgumentNullException("centerId");
            }

            return UserDAL.GetUsersByCenter(centerId);
        }

        public Boolean WorkTimeEditable(Guid userId, DateTime fromDate)
        {
            return UserDAL.WorkTimeEditable(userId, fromDate);
        }

        #region ContactReport

        public ContactReport[] GetContactReport(Guid callJobId)
        {
            return ContactReportDAL.GetContactReport(callJobId);
        }

        public bool DomainUser_UsesDialer(string domainUser)
        {
            return UserDAL.DomainUser_UsesDialer(domainUser);
        }

        public string DomainUser_GetLine(string domainUser)
        {
            return UserDAL.DomainUser_GetLine(domainUser);
        }
        #endregion

        #region PhoneTimesReport

        public PhoneTimesReport[] GetPhoneTimesReport(Guid teamId, Guid userId, DateTime start, DateTime stop)
        {
            return PhoneTimesReportDAL.GetPhoneTimesReport(teamId, userId, start, stop);
        }

        #endregion


        #region ProjectReport

        public ProjectReport[] GetProjectReport(Guid projectId)
        {
            return ProjectReportDAL.GetProjectReport(projectId);
        }

        public ProjectReportDetail GetProjectReportDetail(Guid projectId, int art)
        {
            return ProjectReportDAL.GetProjectReportDetail(projectId, art);
        }
        #endregion

        #region WorkTimeAdditions

        public WorkTimes[] WorkTimes_ALL_GetByUser(Guid userId, DateTime from, DateTime to)
        {
            return UserDAL.WorkTimes_ALL_GetByUser(userId, from, to);
        }

        public User_KeyData User_KeyData_GetByUser(Guid userId, DateTime from, DateTime to)
        {
            return UserDAL.User_KeyData_GetByUser(userId, from, to);
        }

        public User_KeyData Project_KeyData_GetByUserAndProject(Guid userId, Guid projectId)
        {
            return UserDAL.Project_KeyData_GetByUserAndProject(userId, projectId);
        }

        public WorkTimes[] WorkTimes_GROUP_GetByUser(Guid userId, DateTime from, DateTime to)
        {
            return UserDAL.WorkTimes_GROUP_GetByUser(userId, from, to);
        }

        public WorkDayList[] WorkDayList_GetByUser(Guid userId, DateTime from, DateTime to)
        {
            return UserDAL.WorkDayList_GetByUser(userId, from , to);
        }

        public WorkTimeAdditionItems[] WorkTimeAdditionItems_GetAllByUser(Guid userId, DateTime from, DateTime to)
        {
            return UserDAL.WorkTimeAdditionItems_GetAllByUser(userId, from, to);
        }

        public WorkTimeAdditionItems WorkTimeAdditionItems_GetSingle(Guid workTimeAdditionItemId)
        {
            return UserDAL.WorkTimeAdditionItems_GetSingle(workTimeAdditionItemId);
        }

        public WorkTimeAdditions WorkTimeAdditions_GetSingle(Guid workTimeAdditionId)
        {
            return UserDAL.WorkTimeAdditions_GetSingle(workTimeAdditionId);
        }

        public WorkTimeAdditions[] WorkTimeAdditions_GetByUser(Guid userId, DateTime from, DateTime to)
        {
            return UserDAL.WorkTimeAdditions_GetByUser(userId, from, to);
        }

        public int mwWorkTime_Test(WorkTimeAdditions workTimeAdditions)
        {
            return UserDAL.mwWorkTime_Test(workTimeAdditions);
        }

        public void DeleteWorkTimeAddition(WorkTimeAdditions workTimeAdditions)
        {
            if (workTimeAdditions == null)
            {
                throw new ArgumentNullException("workTimeAdditions");
            }

            UserDAL.DeleteWorkTimeAddition(workTimeAdditions);
        }

        public void UpdateWorkTimeAddition(WorkTimeAdditions workTimeAdditions)
        {
            if (workTimeAdditions == null)
            {
                throw new ArgumentNullException("workTimeAdditions");
            }

            UserDAL.UpdateWorkTimeAddition(workTimeAdditions);
        }

        public void CreateWorkTimeAddition(WorkTimeAdditions workTimeAdditions)
        {
            if (workTimeAdditions == null)
            {
                throw new ArgumentNullException("workTimeAdditions");
            }

            UserDAL.CreateWorkTimeAddition(workTimeAdditions);
        }

        #endregion

        public void CreateUser(User user, string password)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            UserDAL.CreateUser(user, password);
        }

        public void UpdateUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            UserDAL.UpdateUser(user);
        }

        public void DeleteUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            UserDAL.DeleteUser(user.UserId);
        }

        public void DeleteUser(UserInfo userInfo)
        {
            if (userInfo == null)
            {
                throw new ArgumentNullException("userInfo");
            }

            UserDAL.DeleteUser(userInfo.UserId);
        }

        public string GetHashedPassword(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentNullException("userId");
            }

            return UserDAL.GetHashedPassword(userId);
        }
        
        public void SetHashedPassword(Guid userId, string hashedPassword)
        {
            UserDAL.SetHashedPassword(userId, hashedPassword);
        }

        public TeamMitglied[] GetUsersByTeam(Guid teamId)
        {
            if (teamId == Guid.Empty)
            {
                throw new ArgumentNullException("teamId");
            }

            return UserDAL.GetUsersByTeam(teamId);
        }


        public void LogOff(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            UserDAL.LogOff(user);
        }

        public void LogOn(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            UserDAL.LogOnUser(user);
        }

        public void CreateWorkTimeItem(WorkTimeItem workTimeItem)
        {
            if (workTimeItem == null)
            {
                throw new ArgumentNullException("workTimeItem");
            }

            UserDAL.CreateWorkTimeItem(workTimeItem);
        }

        public void UpdateWorkTimeItem(WorkTimeItem workTimeItem)
        {
            if (workTimeItem == null)
            {
                throw new ArgumentNullException("workTimeItem");
            }

            UserDAL.UpdateWorkTimeItem(workTimeItem);
        }

        public void CreateMetawareArbeitszeit(int partnerNummer, int projektNummer, string bezeichnung, DateTime arbeitsdatum, DateTime arbeitszeitVon, DateTime arbeitszeitBis, ProjectLogOnTimeItem projectLogOnTimeItem)
        {
            UserDAL.CreateMetawareArbeitszeit(partnerNummer, projektNummer, bezeichnung, arbeitsdatum, arbeitszeitVon, arbeitszeitBis, projectLogOnTimeItem);
        }

        #region Reporting
        public WorkTimeReportResults[] GetWorkTimeReportResults(Guid? centerId, Guid? teamId, Guid? userId, Guid? projectId, DateTime start, DateTime stop)
        {
            return UserDAL.GetWorkTimeReportResults(centerId, teamId, userId, projectId, start, stop);
        }

        public UserCallJobActivityResults[] GetUserCallJobActivityResults(Guid? centerId, Guid? teamId, Guid? userId, Guid? projectId, DateTime start, DateTime stop)
        {
            return UserDAL.GetUserCallJobActivityResults(centerId, teamId, userId, projectId, start, stop);
        }
        #endregion

        #region Signature
        public UserSignature GetUserSignature(Guid userId)
        {
            if (userId.Equals(Guid.Empty))
            {
                throw new ArgumentNullException("userId");
            }

            return UserDAL.GetSignature(userId);
        }

        public void SetUserSignature(Guid userId, string filename)
        {
            if (userId.Equals(Guid.Empty))
            {
                throw new ArgumentNullException("userId");
            }

            UserDAL.SetSignature(userId, filename);
        }
        #endregion
        #endregion

        #region SecurityGroups
        public metatop.Applications.metaCall.DataObjects.SecurityGroup[] GetAllSecurityGroups()
        {
            return SecurityGroupDAL.GetSecurityAllGroups();
        }
        #endregion

        #region TrainingGrund

        public TrainingGrund GetTrainingGrund(Guid trainingGrundId)
        {
            return TrainingGrundDAL.GetTrainingGrund(trainingGrundId);
        }

        public TrainingGrund[] GetAllTrainingGrund()
        {
            return TrainingGrundDAL.GetAllTrainingGrund();
        }

        #endregion

        #region Projects

        public Project GetProject(ProjectInfo projectInfo)
        {
            if (projectInfo == null)
            {
                throw new ArgumentNullException("projectinfo");
            }

            return ProjectDAL.GetProject(projectInfo.ProjectId);
        }

        public Project GetProject(Guid projectId)
        {
            if (projectId == null)
            {
                throw new ArgumentNullException("projectId");
            }

            return ProjectDAL.GetProject(projectId);
        }

        public ProjectInfo[] GetProjectByTeam(Guid teamId)
        {
            return ProjectDAL.GetProjectByTeam(teamId);
        }

        public ProjectInfo[] GetProjectByCenter(Guid centerId)
        {
            return ProjectDAL.GetProjectByCenter(centerId);
        }

        public ProjectInfo[] GetProjectByUser(Guid userId)
        {
            return ProjectDAL.GetProjectByUser(userId);
        }

        public ProjectInfo[] GetProjectByUser_KeyData(Guid userId)
        {
            return ProjectDAL.GetProjectByUser_KeyData(userId);
        }

        public void CreateProject(Project project)
        {
            if (project == null)
            {
                throw new ArgumentNullException("project");
            }

            ProjectDAL.CreateProject(project);
        }

        public void UpdateProject(Project project)
        {
            if (project == null)
            {
                throw new ArgumentNullException("project");
            }

            ProjectDAL.UpdateProject(project);
        }

        public void DeleteProject(Project project)
        {
            if (project == null)
            {
                throw new ArgumentNullException("project");
            }

            ProjectDAL.DeleteProject(project.ProjectId);
        }

        public void DeleteProject(ProjectInfo projectInfo)
        {
            if (projectInfo == null)
            {
                throw new ArgumentNullException("projectInfo");
            }

            ProjectDAL.DeleteProject(projectInfo.ProjectId);
        }

        public void CreateProjectLogOnTimeItem(ProjectLogOnTimeItem projectLogOnTimeItem)
        {
            if (projectLogOnTimeItem == null)
            {
                throw new ArgumentNullException("projectLogOnTimeItem");
            }

            ProjectDAL.CreateLogOnTimeItem(projectLogOnTimeItem);
        }

        public void UpdateProjectLogOnTimeItem(ProjectLogOnTimeItem projectLogOnTimeItem)
        {
            if (projectLogOnTimeItem == null)
            {
                throw new ArgumentNullException("projectLogOnTimeItem");
            }

            ProjectDAL.UpdateLogOnTimeItem(projectLogOnTimeItem);
        }

        /// <summary>
        /// Bereitet das Projekt für die letzte Anrufrunde vor
        /// Setzten der Team-Reminder auf erledigt
        /// Setzten der CallJobIterationsCounter auf Projektmax - 1
        /// Setzten der CallJobart auf weiterer Anruf wenn Reminder war
        /// </summary>
        /// <param name="project"></param>
        public void SetLastCall(Project project)
        {
            if (project == null)
            {
                throw new ArgumentNullException("Projekt");
            }

            ProjectDAL.SetLastCall(project);
        }

        public ProjectInfo[] GetProjectsByProjectState(ProjectState projectState)
        {
            return ProjectDAL.GetProjectInfosByProjectState(projectState);
        }

        public ProjectInfo[] GetProjectsByProjectStateAndCenter(ProjectState projectState, Guid centerId)
        {
            return ProjectDAL.GetProjectInfosByProjectStateAndCenter(projectState, centerId);
        }

        public ProjectInfo[] GetProjectsByProjectStateAndUser(ProjectState projectState, User user)
        {
            return ProjectDAL.GetProjectInfosByProjectStateAndUser(projectState, user);
        }

        public ProjectInfo[] GetProjectsByProjectStateAndTeam(ProjectState projectState, Team team)
        {
            return ProjectDAL.GetProjectInfosByProjectStateAndTeam(projectState, team);
        }

        public ProjectStateInfo GetProjectStateInfo(ProjectState projectState)
        {
            return ProjectDAL.GetProjectStateInfo(projectState);
        }

        public ProjectStateInfo[] GetProjectStateInfos()
        {
            return ProjectDAL.GetProjectStateInfos();
        }

        #endregion

        #region ProjectDocuments
        public void CreateProjectDocument(ProjectDocument document)
        {
            if (document == null)
            {
                throw new ArgumentNullException("document");
            }

            ProjectDocumentDAL.CreateDocument(document);
        }

        public void UpdateProjectDocument(ProjectDocument document)
        {
            if (document == null)
            {
                throw new ArgumentNullException("document");
            }

            ProjectDocumentDAL.UpdateDocument(document);
        }

        public void DeleteProjectDocument(Guid documentId)
        {
            if (documentId == Guid.Empty)
            {
                throw new ArgumentNullException("documentId");
            }

            ProjectDocumentDAL.DeleteDocument(documentId);
        }

        public ProjectDocument GetProjectDocument(Guid documentId)
        {
            if (documentId == Guid.Empty)
            {
                throw new ArgumentNullException("documentId");
            }

            return ProjectDocumentDAL.GetProjectDocument(documentId);
        }

        public ProjectDocument[] GetProjectDocumentsByProject(Guid projectid)
        {
            if (projectid == Guid.Empty)
            {
                throw new ArgumentNullException("projectId");
            }

            return ProjectDocumentDAL.GetProjectDocumentsByProject(projectid);
        }

        public ProjectDocument[] GetProjectDocumentsByProjectAndCategory(Guid projectId, DocumentCategory category)
        {
            if (projectId == Guid.Empty)
            {
                throw new ArgumentNullException("projectId");
            }

            return ProjectDocumentDAL.GetProjectDocumentsByProjectAndCategory(projectId, category);
        }

        public DocumentCategoryInfo GetDocuemntCategoryInfo(DocumentCategory category)
        {
            return ProjectDocumentDAL.GetDocumentCategoryInfo(category);
        }

        public DocumentCategoryInfo[] GetAllDocumentCategoryInfos()
        {
            return ProjectDocumentDAL.GetAllDocumentCategoryInfos();
        }
        #endregion

        #region Centers
        public void CreateCenter(Center center)
        {
            if (center == null)
            {
                throw new ArgumentNullException("center");
            }

            CenterDAL.CreateCenter(center);
        }

        public void UpdateCenter(Center center)
        {
            if (center == null)
            {
                throw new ArgumentNullException("center");
            }

            CenterDAL.UpdateCenter(center);
        }

        public void DeleteCenter(Guid centerId)
        {
            if (centerId == Guid.Empty)
            {
                throw new ArgumentNullException("centerId");
            }

            CenterDAL.DeleteCenter(centerId);
        }

        public Center GetCenter(Guid centerId)
        {
            if (centerId == Guid.Empty)
            {
                throw new ArgumentNullException("centerId");
            }

            return CenterDAL.GetCenter(centerId);
        }

        public Center GetCenter(mwCenter mwCenter)
        {
            if (mwCenter == null)
            {
                throw new ArgumentNullException("mwCenter");
            }

            return CenterDAL.GetCenter(mwCenter);
        }

        public CenterInfo[] GetAllCenters()
        {
            return CenterDAL.GetAllCenters();
        }

        public CenterInfo[] GetCentersForUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return CenterDAL.GetCenters(user);
        }

        #endregion

        #region Teams
        public void CreateTeam(Team team)
        {
            if (team == null)
            {
                throw new ArgumentNullException("team");
            }

            TeamDAL.CreateTeam(team);
        }

        public void UpdateTeam(Team team)
        {
            if (team == null)
            {
                throw new ArgumentNullException("team");
            }

            TeamDAL.UpdateTeam(team);
        }

        //public void DeleteTeam(Guid teamId)
        //{
        //    if (teamId == Guid.Empty)
        //        throw new ArgumentNullException("teamId");

        //    TeamDAL.DeleteTeam(teamId);
        //}

        public Team GetTeam(Guid teamId)
        {
            if (teamId == Guid.Empty)
            {
                throw new ArgumentNullException("teamId");
            }

            return TeamDAL.GetTeam(teamId);
        }

        public TeamInfo[] GetAllTeams()
        {
            return TeamDAL.GetAllTeams();
        }

        public TeamInfo[] GetTeamsByUser(User currentUser)
        {
            return TeamDAL.GetTeamsByUser(currentUser.UserId);
        }

        public TeamAssignInfo[] GetTeamAssignsByUser(User currentUser)
        {
            return TeamDAL.GetTeamAssignsByUser(currentUser.UserId);
        }

        public TeamInfo[] GetTeamsByCenter(Guid centerId)
        {
            if (centerId == Guid.Empty)
            {
                throw new ArgumentNullException("centerId");
            }

            return TeamDAL.GetTeamsByCenter(centerId);
        }

        public TeamInfo[] GetTeamsByWithoutCenter()
        {
            return TeamDAL.GetTeamsByWithoutCenter();
        }

        public TeamInfo[] GetTeamsByDeleted()
        {
            return TeamDAL.GetTeamsByDeleted();
        }

        public TeamInfo[] GetTeamsByProject(Guid projectId)
        {
            return TeamDAL.GetTeamsByProject(projectId);
        }
        #endregion

        #region mwUsers
        public mwUser GetMwUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return mwUserDAL.GetmwUser(user.UserId);
        }

        public mwUser GetMwUser(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentNullException("userId");
            }

            return mwUserDAL.GetmwUser(userId);
        }

        public mwUser[] GetAllActiveMwUsers()
        {
            return mwUserDAL.GetAllmwUsers();
        }
        #endregion

        #region mwCenters
        public mwCenter GetMwCenter(int centerNummer)
        {
            return mwCenterDAL.GetMwCenter(centerNummer);
        }

        public mwCenter GetMwCenter(Center center)
        {
            if (center == null)
            {
                throw new ArgumentNullException();
            }

            if (center.mwCenter == null)
            {
                return null;
            }
            else
            {
                return GetMwCenter(center.mwCenter.CenterNummer);
            }
        }

        public mwCenter[] GetAllActiveMetaWareCenters()
        {
            return mwCenterDAL.GetAllActiveMwCenters();
        }
        #endregion

        #region mwProjects
        public mwProject GetMwProject(int projektNummer)
        {
            if (projektNummer < 1)
            {
                throw new ArgumentNullException("projektNummer");
            }

            return mwProjectDAL.GetMwProject(projektNummer);
        }

        public mwProject[] GetAllMwProjectsForTransfer(CenterInfo center, int statusKennung)
        {
            if (center == null)
            {
                throw new ArgumentNullException("center");
            }

            if (statusKennung < 1)
            {
                throw new ArgumentOutOfRangeException("statusKennung must be greather than 0");
            }

            return mwProjectDAL.GetAllProjectsForTransfer(center, statusKennung);
        }
        #endregion

        #region Addresses
        public void TransferAddressPool(Project project)
        {
            if (project == null)
            {
                throw new ArgumentNullException("project");
            }

            AddressDAL.TransferAddressPool(project);
        }

        public int GetFailureByProject(ProjectInfo project)
        {
            if (project == null)
            {
                throw new ArgumentNullException("project");
            }

            return AddressDAL.GetFailureByProject(project);
        }

        public void DeleteFailureByProject(ProjectInfo project)
        {
            if (project == null)
            {
                throw new ArgumentNullException("project");
            }

            AddressDAL.DeleteFailureByProject(project);
        }

        public void BlockCallJobsWithMissingAddresses()
        {
            AddressDAL.BlockCallJobsWithMissingAddresses();
        }

        public Sponsor[] GetNewSponsorsForTransfer(Project project)
        {
            if (project == null)
            {
                throw new ArgumentNullException("project");
            }

            return AddressDAL.GetNewListForTransfer(project);
        }

        //Aktualisiert einen Sponsor auf dem Server
        public void UpdateSponsor(Sponsor sponsor)
        {
            if (sponsor == null)
            {
                throw new ArgumentNullException("sponsor");
            }

            AddressDAL.UpdateSponsor(sponsor);
        }

        public Sponsor[] GetSponsorsByProject(Project project)
        {
            if (project == null)
            {
                throw new ArgumentNullException("project");
            }

            return AddressDAL.GetSponsorsByProject(project.ProjectId);
        }

        public Sponsor[] GetSponsorsByProject(ProjectInfo projectInfo)
        {
            if (projectInfo == null)
            {
                throw new ArgumentNullException("projectInfo");
            }

            return AddressDAL.GetSponsorsByProject(projectInfo.ProjectId);
        }
        
        /// <summary>
        /// Liefert die GeoZone, die einem bestimmten Sponsor 
        /// bei einem Projekt zugeordnet wurde
        /// </summary>
        /// <param name="sponsor"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public GeoZone GetGeoZone(Sponsor sponsor, Guid projectId)
        {
            if (sponsor == null)
            {
                throw new ArgumentNullException("sponsor");
            }

            if (projectId == Guid.Empty)
            {
                throw new ArgumentNullException("projectId");
            }

            return AddressDAL.GetGeoZone(sponsor, projectId);
        }

        public SponsorOrderInfo[] GetAllSponsorOrderInfos(Sponsor sponsor, Guid projectId)
        {
            if (sponsor == null)
            {
                throw new ArgumentNullException("sponsor");
            }

            if (projectId == Guid.Empty)
            {
                throw new ArgumentNullException("projectid");
            }

            return AddressDAL.GetAllSponsorOrderInfos(sponsor, projectId);
        }

        /// <summary>
        /// Erstellt einen Sponsor auf dem Server
        /// </summary>
        /// <param name="sponsor"></param>
        /// <param name="project"></param>
        /// <remarks>
        /// Der Sponsor wird in der Addresstabelle hinterlegt und dem angegebenen Project zugeordnet
        /// </remarks>
        public void CreateSponsor(Sponsor sponsor, Project project)
        {
            AddressDAL.CreateSponsor(sponsor, project);
        }
        #endregion

        #region Calls & CallJobs

        /// <summary>
        /// liefert einen neuen leeren Sponsoring CallJob
        /// </summary>
        /// <returns></returns>
        public CallJob GetNewSponsoringCallJob()
        {
            return CallJobDAL.GetNewSponsoringCallJob();
        }

        /// <summary>
        /// Liefert einen einzelnen CallJob anhand der CallJobId vom Server
        /// </summary>
        /// <param name="callJobId"></param>
        /// <returns></returns>
        public CallJob GetCallJob(Guid callJobId)
        {
            if (callJobId == Guid.Empty)
            {
                throw new ArgumentNullException("callJobId");
            }

            return CallJobDAL.GetCallJob(callJobId);
        }

        /// <summary>
        /// Liefert einen einzelnen CallJob anhand der AddressId und der ProjectId vom Server
        /// </summary>
        /// <param name="addressId"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public CallJob GetCallJob(Guid addressId, Guid projectId)
        {
            if (addressId == Guid.Empty)
            {
                throw new ArgumentNullException("AddressId");
            }

            if (projectId == Guid.Empty)
            {
                throw new ArgumentNullException("ProjectId");
            }

            return CallJobDAL.GetCallJob(addressId, projectId);

        }

        /// <summary>
        /// Erstellt die CallJobs für ein Project anhand
        /// der AdressReleaseInfo-Liste
        /// </summary>
        /// <param name="infoList"></param>
        public void CreateCallJobsByAddressReleaseInfos(AddressReleaseInfo[] infoList)
        {
            if (infoList == null)
            {
                throw new ArgumentNullException("infoList");
            }

            CallJobDAL.CreateCallJobsByAddressReleaseInfos(infoList);
        }
        /// <summary>
        /// Aktualisiert einen CallJob auf dem Server
        /// Die Calls werden für den Benutzer reserviert und können nicht durch einen 
        /// anderen Benutzer angefordert werden
        /// </summary>
        /// <param name="callJob"></param>
        public void UpdateCallJob(CallJob callJob)
        {
            if (callJob == null)
            {
                throw new ArgumentNullException("callJob");
            }

            CallJobDAL.UpdateCallJob(callJob);
        }

        /// <summary>
        /// Aktualisiert den DialMode alle Calljobs eines Projekts
        /// </summary>
        /// <param name="project"></param>
        /// <param name="dialMode"></param>
        public void UpdateCallJobsByProject(Project project, DialMode dialMode)
        {
            if (project == null)
            {
                throw new ArgumentNullException("project");
            }

            CallJobDAL.UpdateCallJobsDialModeByProject(project, dialMode);
        }

        public void UpdateCallJobsUnsuitableAddressChanges(CallJobUnsuitableAddressChanges[] callJobAddressChanges)
        {
            if (callJobAddressChanges == null)
            {
                throw new ArgumentNullException("callJobAddressChanges");
            }

            CallJobUnsuitableInfoDAL.UpdateCallJobsUnsuitableAddressChanges(callJobAddressChanges);
        }

        public double GetUnsuitableAddressPercentageByProject(Project project)
        {
            if (project == null)
            {
                throw new ArgumentNullException("project");
            }

            return CallJobUnsuitableInfoDAL.GetUnsuitableAddressPercentageByProject(project);
        }

        /// <summary>
        /// Liefert die anstehenden ReminderCalls (Wiedervorlagen)
        /// Die Calls werden für den Benutzer reserviert und können nicht durch einen 
        /// anderen Benutzer angefordert werden
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public ReminderCall[] GetNextReminderCalls(ReminderCallRequestMessage message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            if (message.User == null)
            {
                throw new ArgumentNullException("message.User");
            }

            if (message.Project == null)
            {
                throw new ArgumentNullException("message.Project");
            }

            if (message.ReminderRequestDate == DateTime.MinValue)
            {
                throw new ArgumentOutOfRangeException("message.ReminderRequestDate");
            }

            if (message.MaxTeamReminders < 1)
            {
                throw new ArgumentOutOfRangeException("message.MaxTeamReminders");
            }

            return CallDAL.GetNextReminderCalls(message);
        }
        
        /// <summary>
        /// Liefert die anstehenden Calls (allgemeine Calls)
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public Call[] GetNextSponsoringCalls(CallRequestMessage message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            return CallDAL.GetNextSponsoringCalls(message);
        }

        public Call GetSingleCall(CallJob callJob, UserInfo userInfo)
        {
            return CallDAL.GetSingleCall(callJob, userInfo);
        }

        public ReminderCall GetSingleReminderCall(CallJob callJob, UserInfo userInfo)
        {
            return CallDAL.GetSingleReminderCall(callJob, userInfo);
        }

        public Call CheckCallExists(CallJob callJob)
        {
            return CallDAL.CheckCallExists(callJob);
        }

        public ReminderCall CheckReminderCallExists(CallJob callJob)
        {
            return CallDAL.CheckReminderCallExists(callJob);
        }

        /// <summary>
        /// aktualisiert den Status des CallJobs auf dem Server
        /// und entfernt den Call aus der Tabelle tblCalls
        /// </summary>
        /// <param name="callId"></param>
        /// <param name="callJobState"></param>
        public void UpdateCall(Guid callId, CallJobState callJobState)
        {
            if (callId == Guid.Empty)
            {
                throw new ArgumentNullException("call");
            }

            if (callJobState == CallJobState.Invalid)
            {
                throw new ArgumentException("callJobState cannot be Invalid");
            }

            CallDAL.UpdateCall(callId, callJobState);
        }
        /// <summary>
        /// Setzt die Call's für einen Benutzer zurück
        /// </summary>
        /// <param name="user"></param>
        public void ResetAllCallsForUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            CallDAL.ResetAllUserCalls(user.UserId);
        }
        /// <summary>
        /// Gibt einen Call auf dem Server wieder frei
        /// </summary>
        /// <param name="callId"></param>
        public void ReleaseCall(Guid callId)
        {
            if (callId == Guid.Empty)
            {
                throw new ArgumentNullException("callId");
            }

            CallDAL.ReleaseCall(callId);
        }

        public void ReleaseCalls(Call[] calls)
        {
            if (calls == null)
            {
                throw new ArgumentNullException("calls");
            }

            CallDAL.ReleaseCalls(calls);
        }

        public void CreateCallJobPhoneEvent(CallJobPhoneEvent phoneEvent)
        {
            if (phoneEvent == null)
            {
                throw new ArgumentNullException("callJobPhoneEvent");
            }

            CallJobPhoneEventDAL.CreateCallJobPhoneEvent(phoneEvent);
        }

        public void CreateCallJobActivityTimeItem(CallJobActivityTimeItem callJobActivityTimeItem)
        {
            if (callJobActivityTimeItem == null)
            {
                throw new ArgumentNullException("callJobActivityTimeItem");
            }

            CallJobDAL.CreateCallJobActivityTimeItem(callJobActivityTimeItem);
        }
        public void UpdateCallJobActivityTimeItem(CallJobActivityTimeItem callJobActivityTimeItem)
        {
            if (callJobActivityTimeItem == null)
            {
                throw new ArgumentNullException("callJobActivityTimeItem");
            }

            CallJobDAL.UpdateCallJobActivityTimeItem(callJobActivityTimeItem);
        }
        
        /// <summary>
        /// Liefert eine Liste von CallJobs für das angegebene Projekt (synchron)
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public SponsoringCallJob[] GetSponsoringCallJobsByProject(Project project)
        {
            if (project == null)
            {
                throw new ArgumentNullException("project");
            }

            return CallJobDAL.GetSponsoringCallJobsByProject(project);
        }

        /// <summary>
        /// Liefert true zurück wenn der Sponsor im Vorgängerprojekt ausgewählt hat das er dieses mal mitmachen würde.
        /// </summary>
        /// <param name="sponsor"></param>
        /// <param name="projectInfo"></param>
        /// <returns></returns>
        public Boolean GetTipAddressLastProject(Sponsor sponsor, ProjectInfo projectInfo)
        {
            if (sponsor == null)
            {
                throw new ArgumentNullException("Sponsor");
            }

            if (projectInfo == null)
            {
                throw new ArgumentNullException("Projektinfo");
            }

            return AddressDAL.GetTipAddressLastProject(sponsor, projectInfo);
        }


        /// <summary>
        /// Liefert true zurück wenn der Sponsor im AdressenPool als Tip-Adresse gekennzeichnet ist.
        /// </summary>
        /// <param name="AdressenPoolNummer"></param>
        /// <returns></returns>
        public Boolean GetAddress_IsTip(int adressenPoolNummer)
        {
            return AddressDAL.GetAddress_IsTip(adressenPoolNummer);
        }

        public string GetAddress_HistoryNotice(Guid addressId)
        {
            return AddressDAL.GetAddress_HistoryNotice(addressId);
        }

        /// <summary>
        /// Liefert eine Liste von Mahnungs-CallJobs für den angegebenen User (synchron)
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public DurringCallJob[] GetDurringCallJobsByUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("User");
            }

            return CallJobDAL.GetDurringCallJobsByUser(user);
        }

        /// <summary>
        /// Liefert eine Liste der anstehenden Mahnungen eines User
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public DurringInfo[] GetDurringInfoByUser(User user, int mahnstufe2)
        {
            if (user == null)
            {
                throw new ArgumentNullException("User");
            }

            return CallJobDAL.GetDurringInfosByUser(user, mahnstufe2);
        }

        /// <summary>
        /// Liefert eine Liste aller vorhandenen Mahnstufen eines User
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public DurringLevelInfo[] GetDurringLevelInfoByUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("User");
            }

            return CallJobDAL.GetDurringLevelInfoByUser(user);
        }

        /// <summary>
        /// Erstellen der aktuellen Durrings
        /// </summary>
        /// <param name="user"></param>
        public void DurringCreate(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("User");
            }

            CallJobDAL.DurringsCreate(user);
        }

        /// <summary>
        /// Liefert eine Liste von CallJobs für das angegebene Projekt (asynchron)
        /// </summary>
        /// <param name="project"></param>
        /// <param name="asyncOp"></param>
        /// <param name="reportProgressDelegate"></param>
        /// <returns></returns>
        public SponsoringCallJob[] GetSponsoringCallJobsByProject(Project project, AsyncOperation asyncOp, SendOrPostCallback reportProgressDelegate)
        {
            if (project == null)
            {
                throw new ArgumentNullException("project");
            }

            return CallJobDAL.GetSponsoringCallJobsByProject(project, asyncOp, reportProgressDelegate);
        }

        /// <summary>
        /// Liefert eine Liste von CallJobInfoExtended für das angegebene Projekt (asynchron)
        /// </summary>
        /// <param name="project"></param>
        /// <param name="asyncOp"></param>
        /// <param name="reportProgressDelegate"></param>
        /// <returns></returns>
        public CallJobInfoExtended[] GetListCallJobInfoExtendedByProject(Project project, AsyncOperation asyncOp, SendOrPostCallback reportProgressDelegate)
        {
            if (project == null)
            {
                throw new ArgumentNullException("project");
            }

            return CallJobInfoExtendedDAL.GetListCallJobInfoExtendedByProject(project, asyncOp, reportProgressDelegate);
        }

        /// <summary>
        /// Liefert Calljobs eines Projekts die als ungeeignet, Nummer falsch oder Adresse
        /// doppelt gekennzeichnet sind.
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public CallJobUnsuitableInfo[] GetListCallJobsUnsuitableInfoByProject(Project project, Guid userId, Guid contactTypeParticipationUnsuitableId)
        {
            if (project == null)
            {
                throw new ArgumentNullException("project");
            }

            return CallJobUnsuitableInfoDAL.GetListCallJobsUnsuitableInfoByProject(project, userId, contactTypeParticipationUnsuitableId);
        }

        /// <summary>
        /// Liefert UserIds eines Projekts für User die eine Adresse als ungeeignet gekennzeichnet
        /// haben
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public CallJobUnsuitableInfoUser[] GetListCallJobsUnsuitableInfoUsersByProject(Project project)
        {
            if (project == null)
            {
                throw new ArgumentNullException("project");
            }

            return CallJobUnsuitableInfoDAL.GetListCallJobsUnsuitableInfoUsersByProject(project);
        }

        /// <summary>
        /// Liefert contactTypeParticipationUnsuitableIds für Gründe die bei ungeeigneten Adressen des 
        /// Projekts angegeben wurden.
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public CallJobUnsuitableInfoReason[] GetListCallJobsUnsuitableInfoReasonsByProject(Project project)
        {
            if (project == null)
            {
                throw new ArgumentNullException("project");
            }

            return CallJobUnsuitableInfoDAL.GetListCallJobsUnsuitableInfoReasonsByProject(project);
        }

        /// <summary>
        /// Liefert ein Array mit CallJob-Instanzen des angegebenen Benutzer und des Projekts
        /// </summary>
        /// <param name="user"></param>
        /// <param name="project"></param>
        /// <param name="isAdminMode"></param>
        /// <returns></returns>
        public CallJob[] GetCallJobsByUserAndProject(UserInfo user, ProjectInfo project, string expression, bool isAdminMode)
        {
            return CallJobDAL.GetCallJobsByUserAndProject(user, project, expression, isAdminMode);
        }

        /// <summary>
        /// Liefert ein Array mit CallJob-Instanzen des angegebenen Benutzer und des Projekts
        /// </summary>
        /// <param name="user"></param>
        /// <param name="project"></param>
        /// <param name="isAdminMode"></param>
        /// <returns></returns>
        public CallJob[] GetCallJobsByUserAndProject(UserInfo user, ProjectInfo project, string expression, bool isAdminMode, bool excludeRefusals)
        {
            return CallJobDAL.GetCallJobsByUserAndProject(user, project, expression, isAdminMode, excludeRefusals);
        }

        /// <summary>
        /// Liefert eine Liste aller CallJobResults zu einem CallJob
        /// </summary>
        /// <param name="callJob"></param>
        /// <returns></returns>
        public CallJobResult[] GetCallJobResultsForCallJob(CallJob callJob)
        {
            if (callJob == null)
            {
                throw new ArgumentNullException("callJob");
            }

            return CallJobResultDAL.GetCallJobResultsByCallJobId(callJob.CallJobId);
        }

        /// <summary>
        /// Liefert das letzte CallJobResults zu einem CallJob
        /// </summary>
        /// <param name="callJob"></param>
        /// <returns></returns>
        public CallJobResult GetLastCallJobResultsByCallJobId(CallJob callJob)
        {
            if (callJob == null)
            {
                throw new ArgumentNullException("callJob");
            }

            return CallJobResultDAL.GetLastCallJobResultsByCallJobId(callJob.CallJobId);
        }

        /// <summary>
        /// Liefert eine Auflistung der Auftragsverteilung pro Stunde
        /// </summary>
        /// <param name="centerId"></param>
        /// <param name="teamId"></param>
        /// <param name="userId"></param>
        /// <param name="projectId"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public SponsoringOrdersDistribution[] GetSponsoringOrdersDistribution(Guid? centerId, Guid? teamId, Guid? userId, Guid? projectId, DateTime from, DateTime to)
        {
            return CallJobDAL.GetSponsoringOrdersDistribution(centerId, teamId, userId, projectId, from, to);
        }

        public DateTime? GetLastAddressContact(int adressenPoolNummer, Guid projectId)
        {
            return CallJobDAL.GetLastAddressContact(adressenPoolNummer, projectId);
        }

        public void CreateCallJobResult(CallJobResult result)
        {
            if (result == null)
            {
                throw new ArgumentNullException("result");
            }

            CallJobResultDAL.CreateCallJobResult(result);
        }

        public void CreateCallJobSponsoringOrder(CallJobPossibleResult result)
        {
            if (result == null)
            {
                throw new ArgumentNullException("result");
            }

            CallJobDAL.CreateSponsoringOrder(result);
        }

        public void CreateCallJobDurring(CallJobPossibleResult result)
        {
            if (result == null)
            {
                throw new ArgumentNullException("result");
            }

            CallJobDAL.CreateDurring(result);
        }

        public void CreateCallJobSponsoringCancellation(CallJobPossibleResult result)
        {
            if (result == null)
            {
                throw new ArgumentNullException("result");
            }

            CallJobDAL.CreateSponsoringCancellation(result);
        }

        public void CreateCallJobAddressUnsuitable(CallJobUnsuitableResult result)
        {
            if (result == null)
            {
                throw new ArgumentNullException("result");
            }

            CallJobDAL.CreateAddressUnsuitable(result);
        }

        public void UpdateCallJobResult(CallJobResult result)
        {
            if (result == null)
            {
                throw new ArgumentNullException("result");
            }

            CallJobResultDAL.UpdateCallJobResult(result);
        }

        public CallJobStatistics GetCallJobStatistics(ProjectInfo projectInfo,Guid? userId, int resultType)
        {
            if (projectInfo == null)
            {
                throw new ArgumentNullException("ProjectInfo");
            }

            return StatisticsDAL.GetCallJobStatistics(projectInfo, userId, resultType);
        }

        public CallJobStatistics GetCallJobStatistics(CallJobGroupInfo callJobGroup, Guid? userId, int resultType)
        {
            if (callJobGroup == null)
            {
                throw new ArgumentNullException("CallJobGroup");
            }

            return StatisticsDAL.GetCallJobStatistics(callJobGroup, userId, resultType );
        }

        public void TransferUnusedCallJobs(Guid sourceProjectId, Guid targetProjectId)
        {
            if (sourceProjectId == Guid.Empty)
            {
                throw new ArgumentNullException("sourceProjectId");
            }

            if (targetProjectId == Guid.Empty)
            {
                throw new ArgumentNullException("targetProjectId");
            }

            CallJobDAL.TransferUnusedCallJobs(sourceProjectId, targetProjectId);
        }

        public int TransferUnusedCallJobsCount(Guid sourceProjectId)
        {
            if (sourceProjectId == Guid.Empty)
            {
                throw new ArgumentNullException("sourceProjectId");
            }

            return CallJobDAL.TransferUnusedCallJobsCount(sourceProjectId);
        }

        #region CallJobStateInfos
        public CallJobStateInfo GetCallJobStateInfo(CallJobState callJobState)
        {
            return CallJobDAL.GetCallJobStateInfo(callJobState);
        }

        public CallJobStateInfo[] GetAllCallJobStateInfos()
        {
            return CallJobDAL.GetCallJobStateInfos();
        }
        #endregion

        #endregion

        #region CallJobGroups
        /// <summary>
        /// Erstellt eine CallJobgruppe auf dem Server
        /// </summary>
        /// <param name="callJobGroup"></param>
        public void CreateCallJobGroup(CallJobGroup callJobGroup)
        {
            if (callJobGroup == null)
            {
                throw new ArgumentNullException("callJobGroup");
            }

            CallJobGroupDAL.CreateCallJobGroup(callJobGroup);
        }

        /// <summary>
        /// Aktualisiert eine CallJobgruppe auf dem Server
        /// </summary>
        /// <param name="callJobGroup"></param>
        public void UpdateCallJobGroup(CallJobGroup callJobGroup)
        {
            if (callJobGroup == null)
            {
                throw new ArgumentNullException("callJobGroup");
            }

            CallJobGroupDAL.UpdateCallJobGroup(callJobGroup);
        }

        /// <summary>
        /// Löscht eine CallJobgruppe auf dem Server
        /// </summary>
        /// <param name="callJobGroup"></param>
        public void DeleteCallJobGroup(Guid callJobGroupId)
        {
            if (callJobGroupId == Guid.Empty)
            {
                throw new ArgumentNullException("callJobGroup");
            }

            CallJobGroupDAL.DeleteCallJobGroup(callJobGroupId);
        }

        /// <summary>
        /// Liefert eine CallJobGruppe mit der angegebenen CallJobGroupId
        /// </summary>
        /// <param name="callJobGroupId"></param>
        /// <returns></returns>
        public CallJobGroup GetCallJobGroup(Guid callJobGroupId)
        {
            if (callJobGroupId == Guid.Empty)
            {
                throw new ArgumentNullException("callJobGroupid");
            }

            return CallJobGroupDAL.GetCallJobGroup(callJobGroupId);
        }

        public CallJobGroup[] GetCallJobGroupsByProject(Guid projectId)
        {
            if (projectId == Guid.Empty)
            {
                throw new ArgumentNullException("projectId");
            }

            return CallJobGroupDAL.GetCallJobGroupsByProject(projectId);
        }

        public CallJobGroupInfo GetCallJobGroupInfo(Guid callJobGroupId)
        {
            if (callJobGroupId == Guid.Empty)
            {
                throw new ArgumentNullException("CallJobGroupId");
            }

            return CallJobGroupDAL.GetCallJobGroupInfo(callJobGroupId);
        }

        public CallJobGroupInfo[] GetCallJobGroupInfosByProject(Guid projectId)
        {
            if (projectId == Guid.Empty)
            {
                throw new ArgumentNullException("projectId");
            }

            return CallJobGroupDAL.GetCallJobGroupInfosByProject(projectId);
        }

        public CallJobGroupInfo[] GetCallJobGroupInfosByUser(Guid userId, Guid? teamId, Guid projectID)
        {
            return CallJobGroupDAL.GetCallJobGroupInfosByUser(userId, teamId, projectID);
        }

        public CallJobGroupTypeInfo GetCallJobGroupTypeInfo(CallJobGroupType callJobGroupType)
        {
            return CallJobGroupDAL.GetCallJobGroupTypeInfo(callJobGroupType);
        }

        public CallJobGroupTypeInfo[] GetCallJobGroupTypeInfos()
        {
            return CallJobGroupDAL.GetCallJobGroupTypeInfos();
        }

        #endregion

        #region CallJobReminders
        public void CreateCallJobReminder(CallJobReminder callJobReminder, User user)
        {
            if (callJobReminder == null)
            {
                throw new ArgumentNullException("callJobReminder");
            }

            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            CallJobReminderDAL.CreateCallJobReminder(callJobReminder, user);
        }

        public void UpdateCallJobReminder(CallJobReminder callJobReminder)
        {
            if (callJobReminder == null)
            {
                throw new ArgumentNullException("callJobReminder");
            }

            CallJobReminderDAL.UpdateCallJobReminder(callJobReminder);
        }

        public void UpdateCallJobReminders(int evokeUpdateTyp, Guid? newTeamId, Guid? newUserId, Guid? findProjectId, Guid? findUserId, Guid? findCallJobReminderId)
        {
            CallJobReminderDAL.UpdateCallJobReminders(evokeUpdateTyp, newTeamId, newUserId, findProjectId, findUserId, findCallJobReminderId);
        }

        public void DeleteCallJobReminder(CallJobReminder callJobReminder)
        {
            if (callJobReminder == null)
            {
                throw new ArgumentNullException("callJobReminder");
            }

            CallJobReminderDAL.DeleteCallJobReminder(callJobReminder.CallJobReminderId);
        }

        public CallJobReminderInfo[] GetCallJobReminderInfoByUserAndProject(Guid? userId, Guid? projectId, Guid? teamId)
        {
            return CallJobReminderDAL.GetCallJobReminderInfoByUserAndProject(userId, projectId, teamId);
        }

        public CallJobReminder GetCallJobReminder(Guid callJobReminderId)
        {
            return CallJobReminderDAL.GetCallJobReminder(callJobReminderId);
        }

        public CallJobReminder GetCallJobReminderByCallJob(Guid callJobId)
        {
            return CallJobReminderDAL.GetCallJobReminderByCallJob(callJobId);
        }

        #endregion

        #region ContactTypes

        public void CreateContactType(ContactType contactType)
        {
            if (contactType == null)
            {
                throw new ArgumentNullException("contactType");
            }

            ContactTypeDAL.CreateContactType(contactType);
        }

        public void UpdateContactType(ContactType contactType)
        {
            if (contactType == null)
            {
                throw new ArgumentNullException("contactType");
            }

            ContactTypeDAL.UpdateContactType(contactType);
        }

        public void DeleteContactType(Guid contactTypeId)
        {
            if (contactTypeId == Guid.Empty)
            {
                throw new ArgumentNullException("contactTypeId");
            }

            ContactTypeDAL.DeleteContactType(contactTypeId);
        }

        public ContactType GetContactType(Guid contactTypeId)
        {
            if (contactTypeId == Guid.Empty)
            {
                throw new ArgumentNullException("contactTypeId");
            }

            return ContactTypeDAL.GetContactType(contactTypeId);
        }

        public ContactType[] GetAllContactTypesSponsoringCallJob()
        {
            return ContactTypeDAL.GetAllContactTypesSponsoringCallJob();
        }

        public ContactType[] GetAllContactTypesDurringCallJob()
        {
            return ContactTypeDAL.GetAllContactTypesDurringCallJob();
        }

        #endregion

        #region ContactTypesParticipation

        public ContactTypesParticipation[] GetAllContactTypesParticipations()
        {
            return ContactTypesParticipationDAL.GetAllContactTypesParticipation();
        }

        #endregion

        #region ContactTypesParticipationCancellation

        public ContactTypesParticipationCancellation[] GetAllContactTypesParticipationCancellation()
        {
            return ContactTypesParticipationCancellationDAL.GetAllContactTypesParticipationCancellation();
        }

        #endregion

        #region ContactTypesParticipationUnsuitable

        public ContactTypesParticipationUnsuitable[] GetAllContactTypesParticipationUnsuitable()
        {
            return ContactTypesParticipationUnsuitableDAL.GetAllContactTypesParticipationUnsuitable();
        }

        #endregion

        #region mwProjekt_SponsorPacket

        public mwProjekt_SponsorPacket GetmwProjekt_SponsorPacket(int projekteSponsorenpaketNummer)
        {
            return mwProjekt_SponsorPacketDAL.GetmwProjekt_SponsorPacket(projekteSponsorenpaketNummer);
        }

        public mwProjekt_SponsorPacket[] GetAllmwProjekt_SponsorPacket(int projektnummer)
        {
            return mwProjekt_SponsorPacketDAL.GetAllmwProjekt_SponsorPackets(projektnummer);
        }

        #endregion

        #region ThankingsFormsProject

        public ThankingsFormsProject GetThankingsFormsProject(int id)
        {
            return ThankingsFormsProjectDAL.GetThankingFormProject(id);
        }

        public ThankingsFormsProject[] GetAllThankingsFormsProject(int projektnummer)
        {
            return ThankingsFormsProjectDAL.GetAllThankingsFormsProject(projektnummer);
        }

        #endregion

        #region mwProjekt_SponsorOrderHistorie

        public mwProjekt_SponsorOrderHistorie[] GetAllmwProjekt_SponsorOrderHistorie(int adressenPoolNummer)
        {
            return mwProjekt_SponsorOrderHistorieDAL.GetAllmwProjekt_SponsorOrderHistorie(adressenPoolNummer);
        }

        public mwProjekt_SponsorOrderHistorie[] GetAllmwProjekt_SponsorOrderHistorieLastAgent(int adressenPoolNummer)
        {
            return mwProjekt_SponsorOrderHistorieDAL.GetAllmwProjekt_SponsorOrderHistorieLastAgent(adressenPoolNummer);
        }

        #endregion

        #region mwProjekt_ProjekOrderHistorie

        public mwProjekt_ProjektOrderHistorie[] GetAllmwProjekt_ProjektOrderHistorie(int projektNummer)
        {
            return mwProjekt_ProjektOrderHistorieDAL.getAllmwProjekt_ProjektOrderHistorie(projektNummer);
        }

        public OrderHistory[] GetOrderHistoy_GetByUser(Guid userId, DateTime from, DateTime to)
        {
            return mwProjekt_ProjektOrderHistorieDAL.GetOrderHistoy_GetByUser(userId, from, to);
        }

        #endregion

        #region mwProject

        public ProjectOrders[] GetAllProjectOrders(Guid userId, Guid projectId)
        {
            return mwProjectDAL.GetAllProjectOrders(userId, projectId);
        }

        public ProjectCounts[] GetAllProjectCounts(Guid userId)
        {
            return mwProjectDAL.GetAllProjectCounts(userId);
        }

        public ProjectCounts[] GetAllProjectCounts(Guid? userId, Guid projectId)
        {
            return mwProjectDAL.GetAllProjectCounts(userId, projectId);
        }

        #endregion
        
        #region Branch

        public Branch[] GetBranches()
        {
            return BranchDAL.GetAllBranchs();
        }

        #endregion

        #region Settings

        public Setting GetSetting()
        {
            return SettingDAL.GetSettings();
        }

        public void UpdateSettings(Setting setting)
        {
            SettingDAL.UpdateSettings(setting);
        }

        #endregion


        #region BranchGroup

        public BranchGroup[] GetBranchGroups()
        {
            return BranchGroupDAL.GetAllBranchGroups();
        }

        public BranchGroup GetBranchGroup(Guid branchGroupID)
        {
            return BranchGroupDAL.GetBranchGroup(branchGroupID);
        }

        #endregion

        #region CountryPhoneNumber

        public CountryPhoneNumber GetCountryPhoneNumber(string land)
        {
            return CountryPhoneNumberDAL.GetCountryPhoneNumber(land);
        }

        #endregion

        #region BranchGroupTimeList

        public BranchGroupTimeList[] GetBranchGroupTimeLists(Guid branchGroupID)
        {
            return BranchGroupTimeListDAL.GetAllBranchGroupTimeLists(branchGroupID);
        }

        public BranchGroupTimeList GetBranchGroupTimeList(Guid branchGroupTimeListID)
        {
            return BranchGroupTimeListDAL.GetBranchGroupTimeList(branchGroupTimeListID);
        }

        #endregion

        #region Activites
        public void CreateActivity(MaDaNet.Common.AppFrameWork.Activities.ActivityBase activity, string contextInfo)
        {
            if (activity == null)
            {
                throw new ArgumentNullException("activity");
            }

            if (string.IsNullOrEmpty(contextInfo))
            {
                throw new ArgumentException("contextInfo musst be set", "contextInfo");
            }

            ActivityDAL.CreateActivity(activity, contextInfo);
        }
        #endregion


        #region Recoveries

        public RecoveriesSum GetRecoveriesSum_GetByUser(Guid userId, DateTime from, DateTime to, int vertriebabrechnungNummer, int mode)
        {
            return RecoveriesDAL.GetRecoveriesSum_GetByUser(userId, from, to, vertriebabrechnungNummer, mode);
        }

        public RecoveriesDetails[] GetRecoveriesDetails_GetByUser(Guid userId, DateTime from, DateTime to, int vertriebabrechnungNummer, int mode)
        {
            return RecoveriesDAL.GetRecoveriesDetails_GetByUser(userId, from, to, vertriebabrechnungNummer, mode);
        }

        public SalaryStatementNumbers[] GetSalaryStatementNumbers_GetByUser(Guid userId)
        {
            return RecoveriesDAL.GetSalaryStatementNumbers_GetByUser(userId);
        }

        #endregion

        public CenterInfo GetCenterInfo(mwCenter mwCenter)
        {
            if (mwCenter == null)
            {
                throw new ArgumentNullException("mwCenter");
            }

            return CenterDAL.GetCenterInfo(mwCenter);
        }

        public CenterInfo GetCenterInfo(Guid centerId)
        {
            return CenterDAL.GetCenterInfo(centerId);
        }

        public Customer GetCustomer(ProjectInfo projectInfo)
        {
            if (projectInfo == null)
            {
                throw new ArgumentNullException("projectInfo");
            }

            return AddressDAL.GetCustomer(projectInfo.CustomerAdressId);
        }

        #region DocumentHistory
        public void CreateDocumentHistoryItem(DocumentHistory documentHistoryItem)
        {
            if (documentHistoryItem == null)
            {
                throw new ArgumentNullException("documentHistoryItem");
            }

            DocumentHistoryDAL.CreateDocumentHistoryItem(documentHistoryItem);
        }

        public DocumentHistory[] GetDocumentHistoryItemsByCallJob(CallJob callJob)
        {
            if (callJob == null)
            {
                throw new ArgumentNullException("callJob");
            }

            return DocumentHistoryDAL.GetDocumentHistoryItems(callJob);
        }
        #endregion
    }
    #endregion

    #region Exceptions
    
    public class LogOnFailedException : ApplicationException
    {
        public LogOnFailedException() : base() { }

        public LogOnFailedException(string message) : base(message) { }

        public LogOnFailedException(string message, Exception innerException) : base(message, innerException) { }

        public LogOnFailedException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
    #endregion    
}
