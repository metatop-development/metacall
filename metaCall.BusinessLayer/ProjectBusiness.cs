using System;
using System.Collections.Generic;
using System.Text;

using metatop.Applications.metaCall.DataObjects;
using metatop.Applications.metaCall.ServiceAccessLayer;

namespace metatop.Applications.metaCall.BusinessLayer
{
    public class ProjectBusiness
    {
        public event ProjectChangedEventHandler ProjectChanged;
                
        private BusinessLayer.MetaCallBusiness metaCallBusiness;

        internal ProjectBusiness(BusinessLayer.MetaCallBusiness metaCallBusiness)
        {
            this.metaCallBusiness = metaCallBusiness;


            this.metaCallBusiness.LoggedOff += new EventHandler(metaCallBusiness_LoggedOff);
        }

        void metaCallBusiness_LoggedOff(object sender, EventArgs e)
        {
            //Wenn der Benutzer sich abmeldet muss die Projektanmeldung zurückgesetzt werden
            this.LogOff();
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
                throw new ArgumentNullException("Projekt");

            metaCallBusiness.ServiceAccess.SetLastCall(project);
        }

        private void Create(Project project)
        {
            //Prüfen ob der Benutzer angemeldet ist
            if (!metaCallBusiness.Users.IsLoggedOn)
                throw new NoUserLoggedOnException();

            metaCallBusiness.ServiceAccess.CreateProject(project);
        }

        /// <summary>
        /// erstellt aus einem metaware projekt ein metacall projekt
        /// </summary>
        /// <param name="mwProject"></param>
        /// <returns></returns>
        public Project Create(mwProject mwProject, CenterInfo center)
        {
            Project project = new Project();
            project.ProjectId = Guid.NewGuid();
            project.mwProject = mwProject;
            project.Bezeichnung = mwProject.Bezeichnung;
            project.BezeichnungRechnung = mwProject.BezeichnungRechnung;
            project.State = ProjectState.Created;
            //TODO: Standardwert aus Anwendungseinstellungen laden
            project.IterationCounter = 5;
            project.isDeleted = false;
            //TODO: Standardwert aus Anwendungseinstellungen laden
            project.DialMode = DialMode.AutoDialingImmediately;
            project.Teams = new TeamInfo[0];
            project.Sponsors = new Sponsor[0];
            project.CallJobGroups = new CallJobGroup[0];
            project.Center = center;
            project.ReminderDateMax = null;
            project.PraefixMailAttachment = "Sponsoringangebot";
            //TODO: verein abrufen
            //project.Customer = this.metaCallBusiness.ServiceAccess.getcu
            
            Create(project);

            return project;
        }

        public void Update(Project project)
        {
            if (!metaCallBusiness.Users.IsLoggedOn)
                throw new NoUserLoggedOnException();

            metaCallBusiness.ServiceAccess.UpdateProject(project);
        }

        public void Delete(Project project)
        {
            if (!metaCallBusiness.Users.IsLoggedOn)
                throw new NoUserLoggedOnException();
            
            metaCallBusiness.ServiceAccess.DeleteProject(project);
        }

        public void Delete(ProjectInfo projectinfo)
        {
            if (!metaCallBusiness.Users.IsLoggedOn)
                throw new NoUserLoggedOnException();

            metaCallBusiness.ServiceAccess.DeleteProject(projectinfo);
        }

        public List<ProjectInfo> GetByTeam(Team team)
        {

            if (team == null)
                throw new ArgumentNullException("team");

            return new List<ProjectInfo>(metaCallBusiness.ServiceAccess.GetProjectByTeam(team.TeamId));
        }

        public List<ProjectInfo> GetByTeam(TeamInfo teamInfo)
        {
            if (teamInfo == null)
                throw new ArgumentNullException("teamInfo");

            return new List<ProjectInfo>(metaCallBusiness.ServiceAccess.GetProjectByTeam(teamInfo.TeamId));

        }

        public List<ProjectInfo> GetByCenter(Center center)
        {
            if (center == null)
                throw new ArgumentNullException("center");

            return new List<ProjectInfo>(metaCallBusiness.ServiceAccess.GetProjectByCenter(center.CenterId));
        }

        public List<ProjectInfo> GetByCenter(CenterInfo centerInfo)
        {
            if (centerInfo == null)
                throw new ArgumentNullException("centerInfo");

            return new List<ProjectInfo>(metaCallBusiness.ServiceAccess.GetProjectByCenter(centerInfo.CenterId));

        }

        public List<ProjectInfo> GetByUser(Guid userId)
        {
            return new List<ProjectInfo>(metaCallBusiness.ServiceAccess.GetProjectByUser(userId));
        }

        public List<ProjectInfo> GetByUser_KeyData(Guid userId)
        {
            return new List<ProjectInfo>(metaCallBusiness.ServiceAccess.GetProjectByUser_KeyData(userId)        );
        }

        private ProjectInfo currentProject;
        public ProjectInfo Current
        {
            get { return currentProject; }
        }

        protected void OnProjectChanged(ProjectChangedEventArgs e)
        {
            if (ProjectChanged != null)
                ProjectChanged(this, e);
        }

        /// <summary>
        /// meldet sich an einem Project an.
        /// </summary>
        /// <param name="project"></param>
        public void LogOn(ProjectInfo project)
        {
            ///Wenn bereits an einem Project angemeldet 
            ///zuerst die abmeldung durchführen
            if (this.currentProject != null)
                LogOff();
            
            this.currentProject = project;

            ///Activity auslösen
            metaCallBusiness.ActivityLogger.Log(new Activities.ProjectLogOn(project));

            ///Event für ProjectChanged auslösen
            OnProjectChanged(new ProjectChangedEventArgs(project));
        }

        /// <summary>
        /// meldet sich beim aktuellen Projekt ab
        /// </summary>
        public void LogOff()
        {
            if (this.currentProject == null)
                return;

            this.currentProject = null;

            metaCallBusiness.ActivityLogger.Log(new Activities.ProjectLogOff());

            //TODO: Evtl ersetzen durch ein anderes Event
            OnProjectChanged(new ProjectChangedEventArgs(null));
        }

        #region mwProjects
        public List<mwProject> GetMwProjectsForTransfer(CenterInfo center)
        {
            //TODO: Statuskennung in den Einstellungen hinterlegen
            return new List<mwProject>(metaCallBusiness.ServiceAccess.GetAllMwProjectsForTransfer(center, 2010));
        }
        #endregion

        /// <summary>
        /// Liefert ein Projekt-Objekt aufgrund einer ProjectInfo-Instanz
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public Project Get(ProjectInfo projectInfo)
        {

            return metaCallBusiness.ServiceAccess.GetProject(projectInfo);
        }

        public Project Get(Guid projectId)
        {
            return metaCallBusiness.ServiceAccess.GetProject(projectId);
        }

        public List<ProjectInfo> GetProjectsByProjectState(ProjectState projectState)
        {
            return new List<ProjectInfo>(this.metaCallBusiness.ServiceAccess.GetProjectsByProjectState(projectState));
        }

        public List<ProjectInfo> GetProjectsByProjectStateAndCenter(ProjectState projectState, Guid centerId)
        {
            return new List<ProjectInfo>(this.metaCallBusiness.ServiceAccess.GetProjectsByProjectStateAndCenter(projectState, centerId));
        }

        public List<ProjectInfo> GetProjectsByProjectStateAndUser(ProjectState projectState, User user)
        {
            return new List<ProjectInfo>(this.metaCallBusiness.ServiceAccess.GetProjectsByProjectStateAndUser(projectState, user));
        }

        public List<ProjectInfo> GetProjectsByProjectStateAndTeam(ProjectState projectState, Team team)
        {
            return new List<ProjectInfo>(this.metaCallBusiness.ServiceAccess.GetProjectsByProjectStateAndTeam(projectState, team));
        }

        public IDictionary<ProjectState, ProjectStateInfo> ProjectStates
        {
            get
            {
                IDictionary<ProjectState, ProjectStateInfo> infosDictionary = new Dictionary<ProjectState, ProjectStateInfo>();
                ProjectStateInfo[] infos = this.metaCallBusiness.ServiceAccess.GetProjectStateInfos();

                foreach (ProjectStateInfo info in infos)
                {
                    infosDictionary.Add(info.ProjectState, info);
                }

                return infosDictionary;
            }
        }

        /// <summary>
        /// Liefert ein Projektinfo-Objekt aufgrund einer Project-Instanz
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public ProjectInfo Get(Project project)
        {
            ProjectInfo info = new ProjectInfo();
            info.ProjectId = project.ProjectId;
            info.Bezeichnung = project.Bezeichnung;
            info.BezeichnungRechnung = project.BezeichnungRechnung;
            info.DialingPrefixNumber = project.DialingPrefixNumber;
            info.CustomerAdressId = project.Customer.AddressId;
            info.DialMode = project.DialMode;
            info.IterationCounter = project.IterationCounter;
            info.State = project.State;
            info.Venue = project.Venue;
            info.ReminderDateMax = project.ReminderDateMax;
            if (project.mwProject != null)
            {
                info.mwProjektNummer = project.mwProject.Projektnummer;
            }
            else
            {
                info.mwProjektNummer = 0;
            }

            return info;
        }

        public mwProject GetMwProject(ProjectInfo projectInfo)
        {
            if (projectInfo == null)
                throw new ArgumentNullException("projectInfo");

            if (!projectInfo.mwProjektNummer.HasValue)
                return null;

            return this.metaCallBusiness.ServiceAccess.GetMwProject(projectInfo.mwProjektNummer.Value);
        }


    }
}
