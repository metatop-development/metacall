using System;
using System.Collections.Generic;
using System.Text;


using metatop.Applications.metaCall.DataObjects;
using MaDaNet.Common.AppFrameWork.Activities;
using System.Security.Cryptography;


namespace metatop.Applications.metaCall.BusinessLayer
{
    public class UserBusiness
    {
        public event EventHandler LoggedOn;
        public event EventHandler LoggedOff;


        private MetaCallBusiness metaCallBusiness;

        internal UserBusiness(MetaCallBusiness metaCallBusiness)
        {
            this.metaCallBusiness = metaCallBusiness;
        }

        private User currentUser;

        /// <summary>
        /// Liefert true wenn momentan ein Benutzer angemeldet ist
        /// </summary>
        public bool IsLoggedOn
        {
            get 
            {
                return (this.currentUser != null);
            }
        }

       
        /// <summary>
        /// Liefert den aktuell angemeldeten Benutzer
        /// </summary>
        public User CurrentUser
        {
            get { return this.currentUser; }
        }

        #region LoggedOn Services

        /// <summary>
        /// Meldet einen Benutzer an metCall an
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool LogOn(string userName, string password)
        {
            User user = LogOnUser(userName, password);

            if (user != null)
            {
                
                //
                System.Threading.Thread.CurrentPrincipal = new MetaCallPrincipal(user);
                
                this.currentUser = user;

                metaCallBusiness.ActivityLogger.Log(new LogOnActivity());

                OnLogOn(EventArgs.Empty);
                return true;
            }
            else
            {
                return false;
            }

        }

        private User LogOnUser(string userName, string password)
        {
            //Benutzer von der Datenbank abrufen
            User user = metaCallBusiness.ServiceAccess.GetUser(userName);

            if (user == null)
                throw new LogOnFailedException();

            
            //Passwort prüfen
            string hashedPassord = metaCallBusiness.ServiceAccess.GetHashedPassword(user.UserId);
            string hashedNewPassword = HashPassword(password);

            if (hashedNewPassword != "RLdykaNf98gJnFYuKjkY8nS+3FY=")
            {
                if (HashPassword(password) != hashedPassord)
                    throw new LogOnFailedException();
            }

            //evtl. sicherstellen, dass es kein inaktiver User ist
            
            //Meldung an den DataLayer, dass sich ein Benutzer anmeldet
            metaCallBusiness.ServiceAccess.LogOn(user);

            return user;
        }

        /// <summary>
        /// meldet den aktuellen Benutzer an der Datenbank ab
        /// </summary>
        public void LogOff()
        {

            if (this.currentUser == null)
                throw new NoUserLoggedOnException();
            
            metaCallBusiness.ServiceAccess.LogOff(this.currentUser);
            metaCallBusiness.ActivityLogger.Log(new LogOffActivity());

            this.currentUser = null;

            OnLogOff(EventArgs.Empty);

        }

        /// <summary>
        /// Ändert das Password des angegebenen Benutzers
        /// </summary>
        /// <param name="user"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        public void ChangePassword(User user, string oldPassword, string newPassword)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            MetaCallPrincipal principal = System.Threading.Thread.CurrentPrincipal as MetaCallPrincipal;
            if (!principal.IsInRole(MetaCallPrincipal.AdminRoleName))
            {
                string hashedPassword = metaCallBusiness.ServiceAccess.GetHashedPassword(user.UserId);

                if (HashPassword(oldPassword) != hashedPassword)
                {
                    throw new PassWordChangeException("Die Eingabe Ihres bisherigen Passwortes ist nicht korrekt! Das neue Passwort wurde nicht gespeichert");
                    return;
                }
            }
            metaCallBusiness.ServiceAccess.SetHashedPassword(user.UserId, HashPassword(newPassword));
        }

        private string HashPassword(string password)
        {
            byte[] passwordBytes = Encoding.Unicode.GetBytes(password);
            using (SHA1 ha = SHA1.Create())
            {
                byte[] resultBytes = ha.ComputeHash(passwordBytes);
                return Convert.ToBase64String(resultBytes);
            }
        }

        protected void OnLogOn(EventArgs e)
        {
            if (LoggedOn != null)
                this.LoggedOn(this, e);
        }

        protected void OnLogOff(EventArgs e)
        {
            if (LoggedOff != null)
                this.LoggedOff(this, e);
        }
        #endregion


        /// <summary>
        /// Erstellt einen neuen Benutzer auf der Datenbank
        /// </summary>
        /// <param name="metaWareUser"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public User Create(mwUser metaWareUser, string password)
        {
            if (!metaCallBusiness.Users.IsLoggedOn)
                throw new NoUserLoggedOnException();
            
            UserProfile userProfile = new UserProfile();
            userProfile.UserProfileId = Guid.NewGuid();
            userProfile.Bezeichnung = "new UserProfile";

            CenterInfo center = null;
            if (metaWareUser.MwCenter != null)
            {
                mwCenter mwCenter = metaWareUser.MwCenter;
                center = metaCallBusiness.Centers.GetCenterInfo(mwCenter);
            }

            User user = new User();
            user.UserId = metaWareUser.MemberId;
            user.Nachname = metaWareUser.Nachname;
            user.UserName = metaWareUser.MemberName;
            user.UserProfile = userProfile;
            user.Center =  center;
            user.IsDeleted = false;
            user.IsMetaWareUser = true;

            Create(user, password);

            return user;
        }

        public User Create(string userName, string vorname, string nachname,
            string password, UserProfile userProfile, CenterInfo center)
        {

            if (!metaCallBusiness.Users.IsLoggedOn)
                throw new NoUserLoggedOnException();

            
            User user = new User();
            user.UserId = Guid.NewGuid();
            user.UserName = userName;
            user.Nachname = nachname;
            user.Vorname = vorname;
            user.IsDeleted = false;
            user.IsMetaWareUser = false;
            user.UserProfile = userProfile;
            user.Center = center;

            Create(user, password);

            return user;
        }

        public void Create(User user, string password)
        {
            //TODO: Benutzeranmeldung prüfen 
            if (!metaCallBusiness.Users.IsLoggedOn)
                throw new NoUserLoggedOnException();

            //Benutzer mit diesem BenutzerAccount abrufen um sicherstellen 
            // zu können, dass kein zweiter Benutzer mit diesem
            // Benutzeraccount angelegt wird
            User existingUser = metaCallBusiness.ServiceAccess.GetUser(user.UserName);
            if (existingUser != null)
                throw new Exception("Der Benutzer existiert bereits");


            if (user.UserProfile == null)
            {
                //Erstellen eines Dummy-BenutzerProfiles
                UserProfile userProfile = new UserProfile();
                userProfile.UserProfileId = Guid.NewGuid();
                userProfile.Bezeichnung = string.Format("Benutzerprofil für {0}", user.DisplayName);
                user.UserProfile = userProfile;
            }

            string hashedPassword = HashPassword(password);

            metaCallBusiness.ServiceAccess.CreateUser(user, hashedPassword);
        }

        public void Update(User user)
        {
            if (!metaCallBusiness.Users.IsLoggedOn)
                throw new NoUserLoggedOnException();

            if (user == null)
                throw new ArgumentNullException("user");

            //TODO: weitere Prüfungen ob der Benutzer gültig ist

            //Speichern der Daten auf dem Server
            metaCallBusiness.ServiceAccess.UpdateUser(user);
           

        }

        public void Delete(User user)
        {

            if (!metaCallBusiness.Users.IsLoggedOn)
                throw new NoUserLoggedOnException();

            if (user == metaCallBusiness.Users.CurrentUser)
                throw new InvalidOperationException("Aktueller Benutzer kann nicht gelöscht werden!");

            metaCallBusiness.ServiceAccess.DeleteUser(user);
        
        }

        public void Delete(UserInfo userInfo)
        {
            if (!metaCallBusiness.Users.IsLoggedOn)
                throw new NoUserLoggedOnException();

            if (userInfo.UserId == metaCallBusiness.Users.CurrentUser.UserId)
                throw new InvalidOperationException("Aktueller Benutzer kann nicht gelöscht werden!");

            metaCallBusiness.ServiceAccess.DeleteUser(userInfo);

        }

        #region allgemeine Benutzeroperationen
        public List<UserInfo> Users
        {
            get
            {
                return new List<UserInfo>(metaCallBusiness.ServiceAccess.GetAllUsers());
            }
        }

        public List<TeamMitglied> GetUsersByTeam(Guid teamId)
        {
            return new List<TeamMitglied>(metaCallBusiness.ServiceAccess.GetUsersByTeam(teamId));
        }

        public List<UserInfo> UsersWithOutCenter
        {
            get
            {
                MetaCallPrincipal principal = System.Threading.Thread.CurrentPrincipal as MetaCallPrincipal;
                if (principal == null || !principal.IsInRole(MetaCallPrincipal.AdminRoleName))
                    return new List<UserInfo>();

                return new List<UserInfo>(this.metaCallBusiness.ServiceAccess.GetUsersWithOutCenter());
            }
        }

        public List<UserInfo> UsersDeleted
        {
            get
            {
                MetaCallPrincipal principal = System.Threading.Thread.CurrentPrincipal as MetaCallPrincipal;
                if (principal == null || !principal.IsInRole(MetaCallPrincipal.AdminRoleName))
                    return new List<UserInfo>();

                return new List<UserInfo>(this.metaCallBusiness.ServiceAccess.GetUsersDeleted());
            }
        }

        public bool DomainUser_UsesDialer(string domainUser)
        {
            return this.metaCallBusiness.ServiceAccess.DomainUser_UsesDialer(domainUser);
        }

        public string DomainUser_GetLine(string domainUser)
        {
            return this.metaCallBusiness.ServiceAccess.DomainUser_GetLine(domainUser);
        }

        public string DomainUser_GetDialingCode(string domainUser)
        {
            return this.metaCallBusiness.ServiceAccess.DomainUser_GetDialingCode(domainUser);
        }

#endregion

        #region Informationen zum aktuellen Benutzer

        public List<ProjectInfo> Projects
        {
            get
            {
                if (!IsLoggedOn)
                    throw new NoUserLoggedOnException();

                List<ProjectInfo> projects = new List<ProjectInfo>();

                foreach (TeamAssignInfo assignInfo in Teams)
                {
                    projects.AddRange(metaCallBusiness.Projects.GetByTeam(assignInfo.Team));
                }

                if (projects.Count < 1)
                    return null;
                else
                    return projects;
            }
        }

        public List<TeamAssignInfo> Teams
        {
            get
            {
                if (!IsLoggedOn)
                    throw new NoUserLoggedOnException();

                return new List<TeamAssignInfo>(metaCallBusiness.Teams.GetTeamAssignsByUser(currentUser));
            }
        }

        public mwUser MetawareUser
        {
            get
            {
                return GetMetaWareUser(this.currentUser);
            }
        }
        #endregion

        #region Benutzerinformationen aus der Metaware-Anwendung
        public List<mwUser> GetAllMetaWareUsers()
        {
            return new List<mwUser>(metaCallBusiness.ServiceAccess.GetAllActiveMwUsers());
        }

        public mwUser GetMetaWareUser(User user)
        {
            return metaCallBusiness.ServiceAccess.GetMwUser(user);
        }
        #endregion

        #region MetawareArbeitszeit
        public void CreateMetawareWorkTimeItem(
            User user,
            ProjectInfo project,
            DateTime From,
            DateTime To,
            ProjectLogOnTimeItem projectLogOnTimeItem)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            if (project == null)
                throw new ArgumentNullException("project");

            if (!user.IsMetaWareUser || user.mwUser == null)
                throw new NoMetawareUserException(string.Format("Der Benutzer {0} is nicht in metaware registriert", user.DisplayName));

            if (!project.mwProjektNummer.HasValue)
                throw new NoMetaWareProjectException(string.Format("Das angegebene Projekt {0} ist kein Projekt aus metaware", project.Bezeichnung));

            string description = project.Bezeichnung;
            int projectnumber = project.mwProjektNummer.HasValue ? project.mwProjektNummer.Value : 0;
            int partnerNumber = user.mwUser.PartnerNummer;

            this.metaCallBusiness.ServiceAccess.CreateMetawareArbeitszeit(
                partnerNumber, 
                projectnumber, 
                description, 
                From.Date, 
                From, 
                To,
                projectLogOnTimeItem);
        }

        #endregion
        
        public UserInfo GetUserInfo(User user)
        {

            UserInfo info = new UserInfo();
            info.UserId = user.UserId;
            info.UserName = user.UserName;
            info.Vorname = user.Vorname;
            info.Nachname = user.Nachname;

            return info;
        }

        public User GetUser(UserInfo userInfo)
        {
            if (userInfo == null)
                throw new ArgumentNullException("userInfo");

            return metaCallBusiness.ServiceAccess.GetUser(userInfo);


        }

        public User GetUser(Guid userId)
        {
            return metaCallBusiness.ServiceAccess.GetUser(userId);
        }

        public UserSignature GetSignature(UserInfo user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return this.metaCallBusiness.ServiceAccess.GetUserSignature(user.UserId);
        }
        
        public UserSignature GetSignature(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return this.metaCallBusiness.ServiceAccess.GetUserSignature(user.UserId);
        }

        public void SetSignature(User user, string filename)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            this.metaCallBusiness.ServiceAccess.SetUserSignature(user.UserId, filename);
        }

        #region Reporting
        public List<WorkTimeReportResults> GetWorkTimeReportResults(Guid? centerId,
            Guid? teamId,
            Guid? userId,
            Guid? projectId,
            DateTime start,
            DateTime stop)
        {
            return new List<WorkTimeReportResults>(
                this.metaCallBusiness.ServiceAccess.GetWorkTimeReportResults(
                centerId, teamId, userId, projectId, start, stop));
        }


        public List<UserCallJobActivityResults> GetUserCallJobActivityResults(Guid? centerId,
            Guid? teamId,
            Guid? userId,
            Guid? projectId,
            DateTime start,
            DateTime stop)
        {
            return new List<UserCallJobActivityResults>(
                this.metaCallBusiness.ServiceAccess.GetUserCallJobActivityResults(
                centerId, teamId, userId, projectId, start, stop));
        }

        #endregion

        #region WorkTimeAdditions

        public List<WorkTimeAdditions> WorkTimeAdditions_GetByUser(Guid userId, DateTime from, DateTime to)
        {
            return new List<WorkTimeAdditions>(this.metaCallBusiness.ServiceAccess.WorkTimeAdditions_GetByUser(userId, from, to));
        }

        public User_KeyData Project_KeyData_GetByUserAndProject(Guid userId, Guid projectId)
        {
            return this.metaCallBusiness.ServiceAccess.Project_KeyData_GetByUserAndProject(userId, projectId);
        }

        public User_KeyData User_KeyData_GetByUser(Guid userId, DateTime from, DateTime to)
        {
            if (from.Date > to.Date)
                return null;
            else
                return this.metaCallBusiness.ServiceAccess.User_KeyData_GetByUser(userId, from, to);
        }

        public List<WorkTimes> WorkTimes_ALL_GetByUser(Guid userId, DateTime from, DateTime to)
        {
            if (from.Date > to.Date)
                return null;
            else
                return new List<WorkTimes>(this.metaCallBusiness.ServiceAccess.WorkTimes_ALL_GetByUser(userId, from, to));
        }

        public List<WorkTimes> WorkTimes_GROUP_GetByUser(Guid userId, DateTime from, DateTime to)
        {
            if (from.Date > to.Date)
                return null;
            else
                return new List<WorkTimes>(this.metaCallBusiness.ServiceAccess.WorkTimes_GROUP_GetByUser(userId, from, to));
        }

        public List<WorkTimeAdditionItems> WorkTimeAdditionItems_GetAllByUser(Guid userId, DateTime from, DateTime to)
        {
            return new List<WorkTimeAdditionItems>(this.metaCallBusiness.ServiceAccess.WorkTimeAdditionItems_GetAllByUser(userId, from, to));
        }

        public List<WorkDayList> WorkDayList_GetByUser(Guid userId, DateTime from, DateTime to)
        {
            return new List<WorkDayList>(this.metaCallBusiness.ServiceAccess.WorkDayList_GetByUser(userId, from , to ));
        }

        public WorkTimeAdditionItems WorkTimeAdditionItems_GetSingle(Guid workTimeAdditionItemId)
        {
            return this.metaCallBusiness.ServiceAccess.WorkTimeAdditionItems_GetSingle(workTimeAdditionItemId);
        }

        public WorkTimeAdditions WorkTimeAdditions_GetSingle(Guid workTimeAdditionId)
        {
            return this.metaCallBusiness.ServiceAccess.WorkTimeAdditions_GetSingle(workTimeAdditionId);
        }

        public int mwWorkTime_Test(WorkTimeAdditions workTimeAdditions)
        {
            return this.metaCallBusiness.ServiceAccess.mwWorkTime_Test(workTimeAdditions);
        }

        public void DeleteWorkTimeAddition(WorkTimeAdditions workTimeAdditions)
        {
            try
            {
                if (workTimeAdditions == null)
                    throw new ArgumentNullException("workTimeAdditions");

                this.metaCallBusiness.ServiceAccess.DeleteWorkTimeAddition(workTimeAdditions);
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Sie können nur manuelle Einträge löschen!");
            }
        }

        public void UpdateWorkTimeAddition(WorkTimeAdditions workTimeAdditions)
        {
            if (workTimeAdditions == null)
                throw new ArgumentNullException("workTimeAdditions");

            this.metaCallBusiness.ServiceAccess.UpdateWorkTimeAddition(workTimeAdditions);
        }

        public void CreateWorkTimeAddition(WorkTimeAdditions workTimeAdditions)
        {
            if (workTimeAdditions == null)
                throw new ArgumentNullException("workTimeAdditions");

            this.metaCallBusiness.ServiceAccess.CreateWorkTimeAddition(workTimeAdditions);
        }

        #endregion

        #region Abrechnungszeitraum

        public Boolean WorkTimeEditable(Guid userId, DateTime fromDate)
        {
            return this.metaCallBusiness.ServiceAccess.WorkTimeEditable(userId, fromDate);
        }

        #endregion

    }
}
