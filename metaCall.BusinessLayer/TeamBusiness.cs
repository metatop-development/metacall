using System;
using System.Collections.Generic;
using System.Text;

using metatop.Applications.metaCall.ServiceAccessLayer;
using metatop.Applications.metaCall.DataObjects;

namespace metatop.Applications.metaCall.BusinessLayer
{
    public class TeamBusiness
    {
        MetaCallBusiness metaCallBusiness;

        internal TeamBusiness(MetaCallBusiness metaCallBusiness)
        {
            this.metaCallBusiness = metaCallBusiness;
        }

        public void Create(Team team)
        {
            if (team == null)
                throw new ArgumentNullException();

            metaCallBusiness.ServiceAccess.CreateTeam(team);
        
        }

        public void Create(string bezeichnung, string Beschreibung, CenterInfo center)
        {
            Team team = new Team();
            team.TeamId = Guid.NewGuid();
            team.Bezeichnung = bezeichnung;
            team.Beschreibung = Beschreibung;
            team.Center = center;

            Create(team);
            
        }

        public void Update(Team team)
        {
            if (team == null)
                throw new ArgumentNullException("team");

            if (string.IsNullOrEmpty(team.Bezeichnung))
                throw new ArgumentException("team.Bezeichnung");

            if (string.IsNullOrEmpty(team.Beschreibung))
                throw new ArgumentException("team.Becshreibung");


            metaCallBusiness.ServiceAccess.UpdateTeam(team);
        }

        //public void Delete(Team team) 
        //{
        //    if (team == null)
        //        throw new ArgumentNullException("team");

        //    this.metaCallBusiness.ServiceAccess.DeleteTeam(team.TeamId);
        //}

        //public void Delete(TeamInfo team)
        //{
        //    if (team == null)
        //        throw new ArgumentNullException("team");

        //    this.metaCallBusiness.ServiceAccess.DeleteTeam(team.TeamId);
        //}

        /// <summary>
        /// Liefert die alle verfügbaren Teams
        /// Diese Routine darf nur von globalen Administratoren aufgerufen werden
        /// </summary>
        public List<TeamInfo> Teams
        {
            get
            {
                //Prüfen, ob sich ein Benutzer angemeldet hat
                if (!metaCallBusiness.Users.IsLoggedOn)
                    throw new NoUserLoggedOnException();
                
                //wenn der aktuelle Benutzer kein Admin ist wird eine Leere Liste zurückgegeben
                if (!MetaCallPrincipal.Current.IsInRole(MetaCallPrincipal.AdminRoleName))
                    return new List<TeamInfo>();
                
                return new List<TeamInfo>(metaCallBusiness.ServiceAccess.GetAllTeams());
            }
        }

        /// <summary>
        /// Liefert eine Liste mit den für den Benutzer verfügbaren Teams
        /// </summary>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public List<TeamInfo> GetTeamsByUser(User currentUser)
        {
            //Administratoren dürfen alle Teams sehen
            if (MetaCallPrincipal.Current.IsInRole(MetaCallPrincipal.AdminRoleName))
                return this.Teams;


            return new List<TeamInfo>(metaCallBusiness.ServiceAccess.GetTeamsByUser(currentUser));
        }


        public List<TeamAssignInfo> GetTeamAssignsByUser(User user)
        {
            return new List<TeamAssignInfo>(metaCallBusiness.ServiceAccess.GetTeamAssignsByUser(user));
        }
        public List<TeamInfo> GetByCenter(Center center)
        {
            return new List<TeamInfo>(metaCallBusiness.ServiceAccess.GetTeamsByCenter(center.CenterId));
        }

        public List<TeamInfo> GetByCenter(CenterInfo center)
        {
            return new List<TeamInfo>(metaCallBusiness.ServiceAccess.GetTeamsByCenter(center.CenterId));
        }

        public List<TeamInfo> GetByWithoutCenter()
        {
            TeamInfo[] teamInfo = metaCallBusiness.ServiceAccess.GetTeamsByWithoutCenter();

            if (teamInfo == null)
                return null;
            else
                return new List<TeamInfo>(metaCallBusiness.ServiceAccess.GetTeamsByWithoutCenter());

        }

        public List<TeamInfo> GetByDeleted()
        {
            return new List<TeamInfo>(metaCallBusiness.ServiceAccess.GetTeamsByDeleted());
        }

        //public List<Team> GetByProject(Project project)
        //{
        //    if (project == null)
        //        throw new ArgumentNullException("project");

        //    return metaCallBusiness.ServiceAccess.GetTeamsByProject(project.ProjectId);
        //}
        /// <summary>
        /// Liefert ein TeamInfo-Objekt aufgrund eines Team-Objekts
        /// </summary>
        /// <param name="team"></param>
        /// <returns></returns>
        /// <remarks>erforderlich für die Konvertierung</remarks>
        public TeamInfo GetTeamInfo(Team team)
        {
            TeamInfo teamInfo = new TeamInfo();
            teamInfo.TeamId = team.TeamId;
            teamInfo.Bezeichnung = team.Bezeichnung;
            teamInfo.Beschreibung = team.Beschreibung;
            return teamInfo;
        }

        public List<UserInfo> GetUsers(TeamInfo teamInfo)
        {
            if (teamInfo == null)
                throw new ArgumentNullException("teamInfo");

            return new List<UserInfo>(metaCallBusiness.ServiceAccess.GetUsersByTeam(teamInfo.TeamId));
        }
        
        public List<UserInfo> GetUsers(Team team)
        {
            if (team == null)
                throw new ArgumentNullException("team");

            return new List<UserInfo>(metaCallBusiness.ServiceAccess.GetUsersByTeam(team.TeamId));
        }


        public Team GetTeam(TeamInfo teamInfo)
        {

            if (teamInfo == null)
                throw new ArgumentNullException("teamInfo");

            return metaCallBusiness.ServiceAccess.GetTeam(teamInfo.TeamId);
        }



        public List<TeamInfo> GetByProject(ProjectInfo projectInfo)
        {

            if (projectInfo == null)
                throw new ArgumentNullException("projectInfo");

            return new List<TeamInfo>(
                this.metaCallBusiness.ServiceAccess.GetTeamsByProject(projectInfo.ProjectId));
        }
    }
}
