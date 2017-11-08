using System;
using System.Collections.Generic;
using System.Text;

using metatop.Applications.metaCall.ServiceAccessLayer;
using metatop.Applications.metaCall.DataObjects;
using System.IO;
using System.Xml.Serialization;

namespace metatop.Applications.metaCall.BusinessLayer
{
    public partial class MetaCallBusiness
    {
        private ServiceAccess serviceAccess;
        
        public MetaCallBusiness()
        {
            
            this.serviceAccess = new ServiceAccess();
            
            this.users = new UserBusiness(this);
            this.projects = new ProjectBusiness(this);
            this.centers = new CenterBusiness(this);
            this.teams = new TeamBusiness(this);
            this.teamProjectAssign = new TeamProjectAssign(this);
            this.addresses = new AddressBusiness(this);
            this.sponsoringCallManager = new SponsoringCallManager(this);
            this.callJobReminders = new CallJobReminderBusiness(this);
            this.contactType = new ContactTypeBusiness(this);
            this.contactTypesParticipation = new ContactTypesParticipationBusiness(this);
            this.contactTypesParticipationCancellation = new ContactTypesParticipationCancellationBusiness(this);
            this.contactTypesParticipationUnsuitable = new ContactTypesParticipationUnsuitableBusiness(this);
            this.mwprojekt_SponsorPacket = new mwProjekt_SponsorPacketBusiness(this);
            this.branch = new BranchBusiness(this);
            this.branchGroup = new BranchGroupBusiness(this);
            this.branchGroupTimeList = new BranchGroupTimeListBusiness(this);
            this.dialer = new DialerBusiness(this);
            this.activityLogger = new metatop.Applications.metaCall.BusinessLayer.Activities.metaCallLogger(this);
            this.thankingsFormsProject = new ThankingsFormsProjectBusiness(this);
            this.callJobResults = new CallJobResultBusiness(this);
            this.mwprojekt_ProjekOrderHistorie = new mwProjekt_ProjektOrderHistorieBusiness(this);
            this.mwprojekt_SponsorOrderHistorie = new mwProjekt_SponsorOrderHistorieBusiness(this);
            this.callJobs = new CallJobBusiness(this);
            this.callJobsInfoExtended = new CallJobInfoBusiness(this);
            this.callJobInfoUnsuitable = new CallJobInfoUnsuitableBusiness(this);
            this.calljobPhoneEvents = new CallJobPhoneEventBusiness(this);
            this.securityGroups = new SecurityGroupBusiness(this);
            this.callJobGroups = new CallJobGroupBusiness(this);
            this.settings = new SettingBusiness(this);
            this.projectDocuments = new ProjectDocumentBusiness(this);
            this.documentHistory = new DocumentHistoryBusiness(this);
            this.recoveries = new RecoveriesBusiness(this);
            this.mwprojektBusiness = new mwProjektBusiness(this);
            this.trainingGrund = new TrainingGrundBusiness(this);
            this.contactReport = new ContactReportBusiness(this);
            this.phoneTimesReport = new PhoneTimesReportBusiness(this);
            this.projectReport = new ProjectReportBusiness(this);
            this.countryPhoneNumber = new CountryPhoneNumberBusiness(this);
            this.sponsorPacketBusiness = new List<mwProjekt_SponsorPacket>();
            this.encryptionBusiness = new StringEncryptionBusiness(this);
          //  this.settings.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(settings_PropertyChanged);


            RegisterEvents();
            RegisterListeners();
        }

        private void RegisterEvents()
        {
            this.projects.ProjectChanged += new ProjectChangedEventHandler(projects_ProjectChanged);
            this.dialer.Connected += new DialingEventHandler(dialer_Connected);
            this.dialer.WantConnect += new DialingEventHandler(dialer_WantConnect);
            this.dialer.HangedUp += new DialingEventHandler(dialer_HangedUp);
            this.users.LoggedOn += new EventHandler(users_LoggedOn);
            this.users.LoggedOff += new EventHandler(users_LoggedOff);
        }

        void users_LoggedOff(object sender, EventArgs e)
        {
            OnLoggedOff();
        }

        void users_LoggedOn(object sender, EventArgs e)
        {
            OnLoggedOn();
        }

        void dialer_HangedUp(object sender, DialingEventArgs e)
        {
            System.Threading.Thread.CurrentPrincipal = new MetaCallPrincipal(this.Users.CurrentUser);
            this.ActivityLogger.Log(new Activities.HangUp());
        }

        void dialer_WantConnect(object sender, DialingEventArgs e)
        {
            if (e.State == DialStates.DialTone)
            {
                System.Threading.Thread.CurrentPrincipal = new MetaCallPrincipal(this.Users.CurrentUser);
                this.ActivityLogger.Log(new Activities.Dial(e.Call));
            }
        }

        void dialer_Connected(object sender, DialingEventArgs e)
        {
            System.Threading.Thread.CurrentPrincipal = new MetaCallPrincipal(this.Users.CurrentUser);
            this.ActivityLogger.Log(new Activities.DialConnected(e.Call));
        }

        private void RegisterListeners()
        {
            //AppFrameWork.Activities.ActivityLogger.CurrentDatabase = this.serviceAccess.;
            this.ActivityLogger.AddActivityListener(new WTListener(this));
            this.ActivityLogger.AddActivityListener(new PausenListener(this));
            this.ActivityLogger.AddActivityListener(new ATListener(this));
            this.ActivityLogger.AddActivityListener(new PTListener(this));
            this.ActivityLogger.AddActivityListener(new DialTListener(this));
            this.ActivityLogger.AddActivityListener(new TTListener(this));
            this.ActivityLogger.AddActivityListener(new UTListener());
            this.activityLogger.AddActivityListener(new DTListener(this));
            this.ActivityLogger.AddActivityListener(new MetawareWorkTimeListener(this));
            this.activityLogger.AddActivityListener(new TrainingListener(this));
            this.activityLogger.AddActivityListener(new SecondaryTListener(this));
        }

        void projects_ProjectChanged(object sender, ProjectChangedEventArgs e)
        {
            ///Weiterreichen des Events
            OnProjectChanged(e);
        }

        private StringEncryptionBusiness encryptionBusiness;
        public StringEncryptionBusiness EncryptionBusiness
        {
            get { return this.encryptionBusiness; }
        }

        private string trainingGrundItem;
        public string TrainingGrundItem
        {
            get { return this.trainingGrundItem; }
            set { this.trainingGrundItem = value; }
        }

        private string trainingNotice;
        public string TrainingNotice
        {
            get { return this.trainingNotice; }
            set { this.trainingNotice = value; }
        }

        private bool training;
        /// <summary>
        /// Gibt an ob der Benutzer gerade in der Schulung ist oder legt dies fest.
        /// </summary>
        public bool Training
        {
            get { return training; }
            set {
                bool val = this.training;
                
                this.training = value;

                if (this.training)
                {
                    this.ActivityLogger.Log(new Activities.StartTraining());
                }
                else
                {
                    this.ActivityLogger.Log(new Activities.StopTraining(this.trainingGrundItem, this.trainingNotice));
                }

                if (val != this.training)
                    OnTrainingChanged(EventArgs.Empty);
            }
        }


        private bool pause;
        /// <summary>
        /// Gibt an ob der Benutzer gerade in Pause ist oder legt dies fest.
        /// </summary>
        public bool Pause
        {
            get { return pause; }
            set {
                bool val = this.pause;
                
                this.pause = value;

                if (this.pause)
                {
                    this.ActivityLogger.Log(new Activities.StartPause());
                }
                else
                {
                    this.ActivityLogger.Log(new Activities.StopPause());
                }

                if (val != this.pause)
                    OnPauseChanged(EventArgs.Empty);
            }
        }

        internal ServiceAccess ServiceAccess
        {
            get { return this.serviceAccess; }
        }

        private ProjectBusiness projects;

        public ProjectBusiness Projects
        {
            get { return projects; }
        }

        private UserBusiness users;
        public UserBusiness Users
        {
            get { return users; }
        }

        private CenterBusiness centers;
        public CenterBusiness Centers
        {
            get { return centers; }
        }

        private ContactReportBusiness contactReport;
        public ContactReportBusiness ContactReport
        {
            get { return contactReport; }
        }

        private PhoneTimesReportBusiness phoneTimesReport;
        public PhoneTimesReportBusiness PhoneTimesReport
        {
            get { return phoneTimesReport; }
        }

        private ProjectReportBusiness projectReport;
        public ProjectReportBusiness ProjectReport
        {
            get { return projectReport; }
        }

        private TeamBusiness teams;
        public TeamBusiness Teams
        {
            get { return teams; }
        }


        private TeamProjectAssign teamProjectAssign;
        public TeamProjectAssign TeamProjectAssignHelper
        {
            get { return teamProjectAssign; }
        }

        private AddressBusiness addresses;
        public AddressBusiness Addresses
        {
            get { return addresses; }
        }

        private SponsoringCallManager sponsoringCallManager;
        public SponsoringCallManager SponsoringCallManager
        {
            get { return sponsoringCallManager; }
        }

        private CallJobPhoneEventBusiness calljobPhoneEvents;
        public CallJobPhoneEventBusiness CallJobPhoneEvents
        {
            get { return calljobPhoneEvents; }
        }

        private CallJobReminderBusiness callJobReminders;
        public CallJobReminderBusiness CallJobReminders
        {
            get { return callJobReminders; }
        }

        private ContactTypeBusiness contactType;
        public ContactTypeBusiness ContactType
        {
            get { return contactType; }
        }

        private ContactTypesParticipationBusiness contactTypesParticipation;
        public ContactTypesParticipationBusiness ContactTypesParticipation
        {
            get { return contactTypesParticipation; }
        }

        private ContactTypesParticipationCancellationBusiness contactTypesParticipationCancellation;
        public ContactTypesParticipationCancellationBusiness ContactTypesParticipationCancellation
        {
            get { return contactTypesParticipationCancellation; }
        }

        private ContactTypesParticipationUnsuitableBusiness contactTypesParticipationUnsuitable;
        public ContactTypesParticipationUnsuitableBusiness ContactTypesParticipationUnsuitable
        {
            get { return contactTypesParticipationUnsuitable; }
        }

        private mwProjekt_SponsorPacketBusiness mwprojekt_SponsorPacket;
        public mwProjekt_SponsorPacketBusiness mwProjekt_SponsorPacket
        {
            get { return mwprojekt_SponsorPacket; }
        }

        private ThankingsFormsProjectBusiness thankingsFormsProject;
        public ThankingsFormsProjectBusiness ThankingsFormsProject
        {
            get { return thankingsFormsProject; }
        }

        private mwProjekt_ProjektOrderHistorieBusiness mwprojekt_ProjekOrderHistorie;
        public mwProjekt_ProjektOrderHistorieBusiness mwProjekt_ProjekOrderHistorie
        {
            get { return mwprojekt_ProjekOrderHistorie; }
        }

        private mwProjekt_SponsorOrderHistorieBusiness mwprojekt_SponsorOrderHistorie;
        public mwProjekt_SponsorOrderHistorieBusiness mwProjekt_SponsorOrderHistorie
        {
            get { return mwprojekt_SponsorOrderHistorie; }
        }

        private RecoveriesBusiness recoveries;
        public RecoveriesBusiness Recoveries
        {
            get { return recoveries; }
        }

        private TrainingGrundBusiness trainingGrund;
        public TrainingGrundBusiness TrainingGrund
        {
            get { return trainingGrund; }
        }

        private CountryPhoneNumberBusiness countryPhoneNumber;
        public CountryPhoneNumberBusiness CountryPhoneNumber
        {
            get { return countryPhoneNumber; }
        }

        private BranchBusiness branch;
        public BranchBusiness Branch
        {
            get { return branch; }
        }

        private BranchGroupBusiness branchGroup;
        public BranchGroupBusiness BranchGroup
        {
            get { return branchGroup; }
        }

        private BranchGroupTimeListBusiness branchGroupTimeList;
        public BranchGroupTimeListBusiness BranchGroupTimeList
        {
            get { return branchGroupTimeList; }
        }


        private DialerBusiness dialer;
        public DialerBusiness Dialer
        {
            get { return dialer; }
        }

        private Activities.metaCallLogger activityLogger;
        public Activities.metaCallLogger ActivityLogger
        {
            get { return activityLogger; }
        }

        private CallJobResultBusiness callJobResults;
        public CallJobResultBusiness CallJobResults
        {
            get { return callJobResults; }
        }

        private CallJobBusiness callJobs;
        public CallJobBusiness CallJobs
        {
            get { return callJobs; }
        }

        

        private CallJobInfoBusiness callJobsInfoExtended;
        public CallJobInfoBusiness CallJobsInfoExtended
        {
            get { return callJobsInfoExtended; }
        }

        private CallJobInfoUnsuitableBusiness callJobInfoUnsuitable;
        public CallJobInfoUnsuitableBusiness CallJobInfoUnsuitable
        {
            get { return callJobInfoUnsuitable; }
        }
        
        private SecurityGroupBusiness securityGroups;
        public SecurityGroupBusiness SecurityGroups
        {
            get { return securityGroups; }
        }

        private CallJobGroupBusiness callJobGroups;
        public CallJobGroupBusiness CallJobGroups
        {
            get { return callJobGroups; }
        }

        private ProjectDocumentBusiness projectDocuments;
        public ProjectDocumentBusiness ProjectDocuments
        {
            get { return projectDocuments; }
        }

        private DocumentHistoryBusiness documentHistory;
        public DocumentHistoryBusiness DocumentHistory
        {
            get { return documentHistory; }
        }

        private mwProjektBusiness mwprojektBusiness;
        public mwProjektBusiness mwProjektBusiness
        {
            get { return mwprojektBusiness; }
        }

        private List<mwProjekt_SponsorPacket> sponsorPacketBusiness;
        public List<mwProjekt_SponsorPacket> SponsorPacketBusiness
        {
            get { return sponsorPacketBusiness; }
            set 
            {
                sponsorPacketBusiness.Clear();
                sponsorPacketBusiness = value;
            }
        }

        #region Settings
        private SettingBusiness settings;
        DateTime lastSettingRecievedDate;

        /// <summary>
        /// Liefert die aktuellen Anwendungseinstellungen
        /// </summary>
        public SettingBusiness Settings
        {
            get
            {
                /*
                this.settings = SettingDAL.GetSettings();
                this.lastSettingRecievedDate = DateTime.Now;
                this.settings.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(settings_PropertyChanged);
                */
                return settings;
            }
        }
        /*
        private void settings_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            // Wenn sich an den Einstellungen was geändert hat müssen diese
            // auf die Datenbank gespeichert werden
            SettingDAL.UpdateSettings(settings);
            this.settings.PropertyChanged -= new System.ComponentModel.PropertyChangedEventHandler(this.settings_PropertyChanged);
            this.settings = null;
        }
        */
        #endregion

        //TODO: Events für An-/Abmeldung implementieren

        #region globale Events des BusinessLayer
        /// <summary>
        /// Meldet, dass sich ein Benutzer angemeldet hat
        /// </summary>
        public event EventHandler LoggedOn;
        protected void OnLoggedOn()
        {
            if (LoggedOn != null)
                LoggedOn(this, EventArgs.Empty);
        }
        /// <summary>
        /// Meldet, dass sich der aktuelle Benutzer abgemeldet hat
        /// </summary>
        public event EventHandler LoggedOff;
        protected void OnLoggedOff()
        {
            if (LoggedOff != null)
                LoggedOff(this, EventArgs.Empty);
        }
        /// <summary>
        /// Meldet, dass sich das aktuelle Projekt geändert hat
        /// </summary>
        public event ProjectChangedEventHandler ProjectChanged;
        protected void OnProjectChanged(ProjectChangedEventArgs e)
        {
            if (ProjectChanged != null)
                ProjectChanged(this, e);
        }

        public event DurringLevelInfoChangedEventHandler DurringLevelInfoChanged;
        protected void OnDurringLevelInfoChanged(DurringLevelInfoChangedEventArgs e)
        {
            if (DurringLevelInfoChanged != null)
                DurringLevelInfoChanged(this, e);
        }
        
        public event DurringInfoChangedEventHandler DurringInfoChanged;
        protected void OnDurringInfoChanged(DurringInfoChangedEventArgs e)
        {
            if (DurringInfoChanged != null)
                DurringInfoChanged(this, e);

        }

        public event DurringChangedEventHandler DurringChanged;
        protected void OnDurringChanged(DurringChangedEventArgs e)
        {
            if (DurringChanged != null)
                DurringChanged(this, e);

        }

        /// <summary>
        /// Meldet, dass sich die Training-Eigenschaft geändert hat
        /// </summary>
        public event EventHandler TrainingChanged;
        protected void OnTrainingChanged(EventArgs e)
        {
            if (TrainingChanged != null)
                TrainingChanged(this, e);
        }


        /// <summary>
        /// Meldet, dass sich die Pause-Eigenschaft geändert hat
        /// </summary>
        public event EventHandler PauseChanged;
        protected void OnPauseChanged(EventArgs e)
        {
            if (PauseChanged != null)
                PauseChanged(this, e);
        }
        #endregion

        public Dictionary<string, string> GetSystemInformation()
        {
            return this.serviceAccess.GetSystemInformation();

        }

        /// <summary>
        /// Erstellt eine tiefe Kopie eines objekts indem dieses Serialisiert und wieder deserialisiert wird.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectToCopy"></param>
        /// <returns></returns>
        public T GetDeepCopy<T>(T objectToCopy) where T : class, new()
        {

            T clonedObject = null;

            using (MemoryStream memoryStream = new MemoryStream())
            {
                try
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(objectToCopy.GetType());
                    xmlSerializer.Serialize(memoryStream, objectToCopy);

                    memoryStream.Seek(0, SeekOrigin.Begin);

                    clonedObject = (T)xmlSerializer.Deserialize(memoryStream);

                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("Cannot clone the object", ex);
                }
                finally
                {
                    memoryStream.Close();
                }

            }
            return clonedObject;
        }
    }
}
