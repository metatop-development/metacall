using System;
using System.Collections.Generic;
using System.Text;

using metatop.Applications.metaCall.DataObjects;
using metatop.Applications.metaCall.ServiceAccessLayer;

namespace metatop.Applications.metaCall.BusinessLayer
{
    public class CallJobGroupBusiness
    {
        private MetaCallBusiness metaCallBusiness;
        internal CallJobGroupBusiness(MetaCallBusiness metaCallBusiness)
        {
            this.metaCallBusiness = metaCallBusiness;
        }

        /// <summary>
        /// Erstellt eine neue CallJobGruppe auf dem Server.
        /// Die User und Teams-Listen werden mit 0 Elementen initielisiert
        /// </summary>
        /// <param name="project"></param>
        /// <param name="displayName"></param>
        /// <param name="description"></param>
        /// <param name="type"></param>
        /// <param name="ranking"></param>
        /// <returns></returns>

        public CallJobGroup Create(ProjectInfo project, 
            string displayName, 
            string description, 
            CallJobGroupType type,
            int ranking)
        {
            return Create(project, displayName, description, type, ranking, new UserInfo[0], new TeamInfo[0]);
        }

        /// <summary>
        /// Erstellt eine neue CallJobGruppe auf dem Server.
        /// </summary>
        /// <param name="project"></param>
        /// <param name="displayName"></param>
        /// <param name="description"></param>
        /// <param name="type"></param>
        /// <param name="ranking"></param>
        /// <param name="users"></param>
        /// <param name="teams"></param>
        /// <returns></returns>

        public CallJobGroup Create(ProjectInfo project, 
            string displayName, 
            string description, 
            CallJobGroupType type,
            int ranking, 
            UserInfo[] users,
            TeamInfo[] teams)
        {
            CallJobGroup callJobGroup = new CallJobGroup();
            callJobGroup.CallJobGroupId = Guid.NewGuid();
            callJobGroup.Project = project;
            callJobGroup.DisplayName = displayName;
            callJobGroup.Description = description;
            callJobGroup.Type = type;
            callJobGroup.Ranking = ranking;
            callJobGroup.Users = users;
            callJobGroup.Teams = teams;

            Create(callJobGroup);

            return callJobGroup;
        }
        
        /// <summary>
        /// Erstellt eine neue CallJobGruppe auf dem Server
        /// </summary>
        /// <param name="callJobGroup"></param>

        public void Create(CallJobGroup callJobGroup)
        {
            //TODO: Parameter prüfen
            if (callJobGroup.CallJobGroupId == Guid.Empty)
                throw new System.InvalidOperationException("CallJobGroupId could'nt be value Guid.Empty");

            if (string.IsNullOrEmpty(callJobGroup.DisplayName))
                throw new System.InvalidOperationException("DisplayName must be a string greather 0");

            if (string.IsNullOrEmpty(callJobGroup.Description))
                throw new System.InvalidOperationException("Description must be a string greather 0");

            if (callJobGroup.Teams == null)
                callJobGroup.Teams = new TeamInfo[0];

            if (callJobGroup.Users == null)
                callJobGroup.Users = new UserInfo[0];
            
            this.metaCallBusiness.ServiceAccess.CreateCallJobGroup(callJobGroup);
        }

        public void Update(CallJobGroup callJobGroup)
        {
            //TODO: Parameter prüfen
            if (callJobGroup.CallJobGroupId == Guid.Empty)
                throw new System.InvalidOperationException("CallJobGroupId could'nt be value Guid.Empty");

            if (string.IsNullOrEmpty(callJobGroup.DisplayName))
                throw new System.InvalidOperationException("DisplayName must be a string greather 0");

            if (string.IsNullOrEmpty(callJobGroup.Description))
                throw new System.InvalidOperationException("Description must be a string greather 0");

            if (callJobGroup.Teams == null)
                callJobGroup.Teams = new TeamInfo[0];

            if (callJobGroup.Users == null)
                callJobGroup.Users = new UserInfo[0];

            this.metaCallBusiness.ServiceAccess.UpdateCallJobGroup(callJobGroup);
        }

        public void Delete(CallJobGroup callJobGroup)
        {
            this.metaCallBusiness.ServiceAccess.DeleteCallJobGroup(callJobGroup.CallJobGroupId);
            callJobGroup.CallJobGroupId = Guid.Empty;
            callJobGroup.DisplayName = null;
            callJobGroup.Description = null;
            callJobGroup.Project = null;
            callJobGroup.Ranking = 0;
            callJobGroup.Teams = new TeamInfo[0];
            callJobGroup.Users = new UserInfo[0];
        }

        public CallJobGroup Get(Guid callJobGroupId)
        {
            return this.metaCallBusiness.ServiceAccess.GetCallJobGroup(callJobGroupId);
        }

        public List<CallJobGroup> Get(Project project)
        {
            return new List<CallJobGroup>(this.metaCallBusiness.ServiceAccess.GetCallJobGroupsByProject(project.ProjectId));
        }

        public List<CallJobGroup> Get(ProjectInfo project)
        {
            return new List<CallJobGroup>(this.metaCallBusiness.ServiceAccess.GetCallJobGroupsByProject(project.ProjectId));
        }

        public CallJobGroupInfo Get(CallJobGroup callJobGroup)
        {
            CallJobGroupInfo callJobgroupInfo = new CallJobGroupInfo();
            callJobgroupInfo.CallJobGroupId = callJobGroup.CallJobGroupId;
            callJobgroupInfo.DisplayName = callJobGroup.DisplayName;
            callJobgroupInfo.Ranking = callJobGroup.Ranking;
            callJobgroupInfo.Type= callJobGroup.Type;


            return callJobgroupInfo;

        }

        public List<CallJobGroupInfo> Get(Team team, Project project)
        {
            if (team == null) 
                return new List<CallJobGroupInfo>(this.metaCallBusiness.ServiceAccess.GetCallJobGroupInfosByUser(this.metaCallBusiness.Users.CurrentUser.UserId, null, project.ProjectId));
            else
                return new List<CallJobGroupInfo>(this.metaCallBusiness.ServiceAccess.GetCallJobGroupInfosByUser(this.metaCallBusiness.Users.CurrentUser.UserId, team.TeamId, project.ProjectId));

        }


        public List<CallJobGroupInfo> Get(UserInfo userInfo, TeamInfo teamInfo, ProjectInfo projectInfo)
        {

            if (teamInfo == null)
                return Get(userInfo, projectInfo);
            else
                return new List<CallJobGroupInfo>(
                    this.metaCallBusiness.ServiceAccess.GetCallJobGroupInfosByUser(
                    userInfo.UserId, 
                    teamInfo.TeamId, 
                    projectInfo.ProjectId));

        }
        
        public List<CallJobGroupInfo> Get(UserInfo userInfo, ProjectInfo projectInfo)
        {

            return new List<CallJobGroupInfo>(
                this.metaCallBusiness.ServiceAccess.GetCallJobGroupInfosByUser(
                userInfo.UserId,
                null,
                projectInfo.ProjectId));

        }


        public CallJobGroupTypeInfo GetCallJobGroupTypeInfo(CallJobGroupType callJobGroupType)
        {
            return this.metaCallBusiness.ServiceAccess.GetCallJobGroupTypeInfo(callJobGroupType);
        }

        public List<CallJobGroupTypeInfo> GetCallJobGroupTypeInfos()
        {
            return new List<CallJobGroupTypeInfo>(this.metaCallBusiness.ServiceAccess.GetCallJobGroupTypeInfos());
        }
        
        public CallJobGroupFactory GetCallJobGroupFactory()
        {
            return new CallJobGroupFactory(this.metaCallBusiness);
        }


        public List<CallJobGroupInfo> GetCallJobGroupInfo(ProjectInfo project)
        {
            if (project == null)
                throw new ArgumentNullException("project");

            return new List<CallJobGroupInfo>( this.metaCallBusiness.ServiceAccess.GetCallJobGroupInfosByProject(project.ProjectId));
        }

        public CallJobGroupInfo GetCallJobGroupInfo(Guid callJobGroupId)
        {
            if (callJobGroupId == Guid.Empty)
                throw new ArgumentNullException("CallJobGroupId");

            return this.metaCallBusiness.ServiceAccess.GetCallJobGroupInfo(callJobGroupId);
        }
    }
}
