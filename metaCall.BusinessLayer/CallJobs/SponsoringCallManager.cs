using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using System.Data;
using System.Data.Common;
using System.Threading;
using metatop.Applications.metaCall.DataObjects;
using System.Collections.ObjectModel;

namespace metatop.Applications.metaCall.BusinessLayer
{
    [ToolboxItem(false)]
    public partial class SponsoringCallManager : Component
    {
        public event EventHandler CallJobsChanged;

        private MetaCallBusiness metaCallBusiness;

        private ProjectInfo currentProject;
        private User currentUser;
        private Call currentCall;
        private CallJobGroupInfo currentCallJobGroup;
        private int mahnstufe2;

        private int minCallJobs;

        private List<DurringInfo> durringInfo;
        private DurringInfo actDurringInfo;
        private List<Call> durringCallList = new List<Call>();
        private List<Call> sponsoringCallList = new List<Call>();
        private object syncRootSponsoringCalls = new object();
        private List<ReminderCall> reminderCallList = new List<ReminderCall>();
        private object syncRootReminderCalls = new object();

        public event DurringInfoChangedEventHandler DurringInfoChanged;
        public event DurringLevelInfoChangedEventHandler DurringLevelInfoChanged;

        protected void OnDurringLevelInfoChanged(DurringLevelInfoChangedEventArgs e)
        {
            if (DurringLevelInfoChanged != null)
                DurringLevelInfoChanged(this, e);
        }

        protected void OnDurringInfoChanged(DurringInfoChangedEventArgs e)
        {
            if (DurringInfoChanged != null)
                DurringInfoChanged(this, e);
        }

        public List<DurringInfo> DurringInfo
        {
            get { return this.durringInfo; }
        }

        public DurringInfo ActualDurringInfo
        {
            set { actDurringInfo = value; }
        }

        public ProjectInfo Project
        {
            get
            {
                return this.currentProject;
            }
        }

        public User User
        {
            get
            {
                return this.currentUser;
            }
        }

        public CallJobGroupInfo CallJobGroup
        {
            get
            {
                return this.currentCallJobGroup;
            }
        }


        #region SponsoringCalls
        private Timer sponsoringCallMonitor;
//        private Timer durringCallMonitor;
        private AutoResetEvent autoEvent = new AutoResetEvent(false);
        private TimeSpan sponsoringCallMonitorPeriod;
        #endregion

        #region ReminderCalls
        private Timer reminderCallMonitor;

        #endregion

        #region ExpirationTimer
        private Timer expirationTimer;
        #endregion

        private bool noMoreCallJobsAvailable;
        private DateTime lastCall;
        private int callsDone;

        private int numberOfCallsToRetrieve;
        private TimeSpan averageTimePerCall;
        private DateTime lastCallReceiveTime;

        private bool isRunning;

        private bool raiseEvents = true;

        private void metaCallBusiness_ProjectChanged(object sender, ProjectChangedEventArgs e)
        {
            Reset("Sponsoring");
        }

        private void metaCallBusiness_LogOff(object sender, EventArgs e)
        {
            if (metaCallBusiness.CallJobs.DurringActiv == true)
            {
                Reset("Durring");
            }
            else
            {
                Reset("Sponsoring");
            }
        }

        public bool IsRunning
        {
            get
            {
                return this.isRunning;
            }
        }

        /// <summary>
        /// Setzt die aktuellen Einsetllungen zurück, aktiviert die neue CallJobGruppe 
        /// und startet den SponsoringCallManager neu
        /// </summary>
        /// <param name="callJobGroup"></param>
        public void RestartWithNewCallJobGroup(CallJobGroupInfo callJobGroup)
        {
            //Neustart nur durchführen, wenn sich die CallJobGruppe auch unterscheidet
            if (callJobGroup.CallJobGroupId.Equals(this.currentCallJobGroup.CallJobGroupId))
                return;
            
            //Zwischensepichern von projekt und User
            ProjectInfo project = this.currentProject;
            User user = this.currentUser;

            //Reset durchführen
            Reset("Sponsoring");

            //Starten
            Start(user, project, callJobGroup);
        }

        private void Reset(string callJobArt)
        {
            if (this.isRunning)
            {
                if (callJobArt == "Durring")
                {
                    this.StopDurring();
                }
                else
                {
                    this.Stop();
                }
            }

            if (this.TotalCallsInBuffer > 0 )
                ClearCalls();

            this.currentUser = null;
            this.currentProject = null;
            this.currentCall = null;
            this.currentCallJobGroup = null;

            this.minCallJobs = 10;
            this.sponsoringCallMonitorPeriod = new TimeSpan(0, 0, 8);     //8 sec
            this.averageTimePerCall = new TimeSpan(0, 0, 10);     //10 sec. Angenommene Zeit zum bearbeiten eines Calls

            this.callsDone = 0;
            this.noMoreCallJobsAvailable = false;
            this.numberOfCallsToRetrieve = 0;

        }

        private void ClearCalls()
        {
            raiseEvents = false;
            
            while (sponsoringCallList.Count > 0)
            {
                CancelCall(sponsoringCallList[0]);
            }

            reminderCallList.Clear();

            if (this.currentUser != null)
                metaCallBusiness.ServiceAccess.ResetAllCallsForUser(this.currentUser);

            raiseEvents = true ;
            OnCallJobsChanged(EventArgs.Empty);
        
        }

        public SponsoringCallManager()
        {
            InitializeComponent();

            Reset("Sponsoring");
        }

        public SponsoringCallManager(MetaCallBusiness metaCallBusiness)
            : this()
        {
            this.metaCallBusiness = metaCallBusiness;
            this.metaCallBusiness.ProjectChanged += new ProjectChangedEventHandler(metaCallBusiness_ProjectChanged);
            this.metaCallBusiness.LoggedOff += new EventHandler(metaCallBusiness_LogOff);
        }

        private void InitCallJobMonitors()
        {
            if ((this.currentProject == null)
                || this.currentUser == null)
                return;

            this.sponsoringCallMonitor = new Timer(new TimerCallback(this.SponsoringCallMonitor_Tick), this.autoEvent, 10, (int) this.sponsoringCallMonitorPeriod.TotalMilliseconds);
            //auf das erste Abrufen Warten, bevor die Kontrolle zurückgegeben wird.
            //autoEvent.WaitOne();


            // Initialisieren des ReminderCallMonitor's
            // Alle zwei Minuten abrufen
            this.reminderCallMonitor = new Timer(new TimerCallback(this.ReminderCallMonitor_Tick), null, 0, 120000);

            //Initialisieren des Timers für das Aufräumen
            // Alle 30 sec prüfen
            this.expirationTimer = new Timer(new TimerCallback(this.ExpirationTimer_Tick), null, 100, 30000);
        }

        public void Start(User user, ProjectInfo project, CallJobGroupInfo callJobGroup)
        {
            if (isRunning)
                this.Reset("Sponsoring");

            this.currentUser = user;
            this.currentProject = project;
            this.currentCallJobGroup = callJobGroup;
            
            InitCallJobMonitors();

            this.isRunning = true;
        }

        private void InitDurringCallJobMonitors()
        {
            if (this.currentUser != null)
            {
                this.durringInfo = new List<DurringInfo>(this.metaCallBusiness.CallJobs.GetDurringInfosByUser(this.currentUser, this.mahnstufe2));

               // this.durringCallMonitor = new Timer(new TimerCallback(this.SponsoringCallMonitor_Tick), this.autoEvent, 10, (int)this.sponsoringCallMonitorPeriod.TotalMilliseconds);
                //auf das erste Abrufen Warten, bevor die Kontrolle zurückgegeben wird.
                //autoEvent.WaitOne();


                // Initialisieren des ReminderCallMonitor's
                // Alle zwei Minuten abrufen
                this.reminderCallMonitor = new Timer(new TimerCallback(this.ReminderCallMonitor_Tick), null, 0, 120000);

                //Initialisieren des Timers für das Aufräumen
                // Alle 30 sec prüfen
                this.expirationTimer = new Timer(new TimerCallback(this.ExpirationTimer_Tick), null, 100, 30000);
            }

        }

        public void InizializeDurringInfo(User user, int mahnStufe2)
        {
            this.currentUser = user;
            this.mahnstufe2 = mahnStufe2;

            if (durringInfo != null)
                this.durringInfo.RemoveRange(0,durringInfo.Count);

            //erstellen aktueller Durrings für diesen user
   //         this.metaCallBusiness.CallJobs.DurringCreate(user);

            this.durringInfo = new List<DurringInfo>(this.metaCallBusiness.CallJobs.GetDurringInfosByUser(this.currentUser, this.mahnstufe2));
        }

        public void StartDurring(User user)
        {
            if (isRunning)
                this.Reset("Durring");

            this.metaCallBusiness.CallJobs.DurringCreate(user);

            this.currentUser = user;
            this.isRunning = true;
        }

        public void StopDurring()
        {
            //Zeitgeber Durring stoppen

          //  durringCallMonitor.Change(Timeout.Infinite,Timeout.Infinite);
          //  reminderCallMonitor.Change(Timeout.Infinite, Timeout.Infinite);
          //  expirationTimer.Change(Timeout.Infinite, Timeout.Infinite);

            ClearUserCalls(currentUser);

            this.isRunning = false;
            this.Reset("Durring");

        }

        public void Stop()
        {
            //Zeitgeber stoppen
            sponsoringCallMonitor.Change(Timeout.Infinite, Timeout.Infinite);
            reminderCallMonitor.Change(Timeout.Infinite, Timeout.Infinite);
            expirationTimer.Change(Timeout.Infinite, Timeout.Infinite);
            
            ClearUserCalls(currentUser);

            this.isRunning = false;
        }

        //entfernt alle Calls des Benutzers aus der Tabelle
        private void ClearUserCalls(User currentUser)
        {
            return;// throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// Liefert den nächsten DurringCall
        /// </summary>
        /// <returns></returns>
        public Call GetNextDurringCall()
        {
            System.Diagnostics.Trace.WriteLine("Der Client möchte einen neuen Durring Call");

            //DurringInfo dI = new DurringInfo();

            if (durringCallList.Count == 0)
            {
                if (this.durringInfo.Count > 0)
                {
                    actDurringInfo = this.durringInfo[0];

                    CallJob cJ = this.metaCallBusiness.ServiceAccess.GetCallJob(actDurringInfo.CallJobId);

                    UserInfo uI = metaCallBusiness.Users.GetUserInfo(this.currentUser);

                    durringCallList.Add(GetSingleCall(cJ, uI));
                    return durringCallList[0];
                }
                else
                {
                    return null;
                }

            }
            else
            {
                return durringCallList[0];
            }

        }

        /// <summary>
        /// Liefert den nächsten SponsorCall
        /// </summary>
        /// <returns></returns>
        public Call GetNextSponsoringCall()
        {
            System.Diagnostics.Trace.WriteLine("Der Client möchte neue CallJobs");

            //festhalten des Zeitpunkts, zu dem der Call vom Benutzer abgerufen wird
            this.lastCallReceiveTime = DateTime.Now;
            
            if (reminderCallList.Count > 0)
            {
                ReminderCall nextCall = GetNextReminderCall();

                if (nextCall != null)
                {
                    this.currentCall = nextCall;
                    return this.currentCall;
                }
            }

            if (sponsoringCallList.Count == 0 && !noMoreCallJobsAvailable)
            {
                System.Diagnostics.Trace.WriteLine("getNextCallJob wartet, bis der CallJobService die nächsten Daten holt");
                bool hasSignal = autoEvent.WaitOne(1000, false);

                if (!hasSignal) return null;
                
            }

            if (sponsoringCallList.Count > 0)
            {
                this.currentCall = sponsoringCallList[0];

                return currentCall;
            }

            return null;
        }

        private ReminderCall GetNextReminderCall()
        {
            if (reminderCallList.Count == 0)
                return null;

            //den nächsten ReminderCall abrufen
            ReminderCall nextCall = reminderCallList[0];
            CallJobReminder reminder = nextCall.CallJobReminder;

            // Wenn der aktuelle Reminder ansteht und das Projekt unterbrechen kann 
            // wird er zurückgeliefert ansonsten null
            if (reminder.ReminderDateStart <= DateTime.Now)
                if ((reminder.ReminderMode == CallJobReminderMode.DisturbCurrentProject) ||
                    (reminder.Project.ProjectId == this.currentProject.ProjectId))
                    return nextCall;
                else
                    return null;
            else
                return null;
        }

        /// <summary>
        /// Meldet, dass ein Call durchgeführt wurde
        /// </summary>
        /// <param name="callJobResultMessage"></param>
        public void CallDone(CallJobResultMessage callJobResultMessage)
        {

            //Prüfen der Parameter 
            if (callJobResultMessage == null)
                throw new ArgumentNullException("callJobResultMessage");

            if (callJobResultMessage.Call == null)
                throw new ArgumentNullException("callJobResultMessage.Call");

            if (callJobResultMessage.CallJobResult == null)
                throw new ArgumentNullException("callJobResultMessage.CallJobResult");


            // Berechnen, wieviel Zeit für die Bearbeitung benötigt wurde und 
            // anpassen der durchschnittlichen Bearbeitungszeit
            if (lastCallReceiveTime > DateTime.MinValue)
            {
                TimeSpan timeDueLastCall = DateTime.Now.Subtract(this.lastCallReceiveTime);
                if (this.averageTimePerCall != TimeSpan.Zero)
                {
                    // Durchschnittszeit berechnen
                    this.averageTimePerCall = new TimeSpan(averageTimePerCall.Add(timeDueLastCall).Ticks / 2);
                }
                else
                {
                    this.averageTimePerCall = timeDueLastCall;
                }
            }

            // Bearbeiten des Calls
            Call call = callJobResultMessage.Call;
            CallJobResult result = callJobResultMessage.CallJobResult;
            //Ermitteln des CallJobs
            //result.CallJob = metaCallBusiness.ServiceAccess.GetCallJobByProjectAndAddress(call.CallJob.Project.ProjectId, call.CallJob.Sponsor.AddressId);
            result.CallJob = call.CallJob;
            if (result.CallJob == null)
                throw new CallJobNotFoundException();


            bool reminderCreated = ((result.GetType() == typeof(CallJobReminderResult)) ||
                (result.GetType() == typeof(CallJobPossibleResult) && ((CallJobPossibleResult)result).Reminder != null));

            bool addCallAtEndOfList = false;

            CallJobState state = GetCallJobState(callJobResultMessage.CallJobResult, reminderCreated, out addCallAtEndOfList);
            CallJobResultCategory category = CallJobResultCategory.Miscellaneous;
            switch (state)
            {
                case CallJobState.Invalid:
                    category = CallJobResultCategory.Miscellaneous;
                    break;
                case CallJobState.FirstCall:
                    category = CallJobResultCategory.Miscellaneous;
                    break;
                case CallJobState.Waiting:
                    category = CallJobResultCategory.Miscellaneous;
                    break;
                case CallJobState.Cancelled:
                    category = CallJobResultCategory.Unsuccessfull;
                    break;
                case CallJobState.Ordered:
                    category = CallJobResultCategory.Successfull;
                    break;
                case CallJobState.ReminderCallJob:
                    category = CallJobResultCategory.Uncommitted;
                    break;
                case CallJobState.FurtherCall:
                    category = CallJobResultCategory.Uncommitted;
                    break;
                case CallJobState.Unsuitable:
                    category = CallJobResultCategory.Unsuccessfull;
                    break;
                case CallJobState.Durring:
                    category = CallJobResultCategory.Successfull;
                    break;
                case CallJobState.DurringDone:
                    category = CallJobResultCategory.Unsuccessfull;
                    break;
                default:
                    throw new UnknownCallJobResultCategoryException();
            }

            result.Category = category;

            if (result.User.IsMetaWareUser)
                result.mwUser = metaCallBusiness.Users.GetMetaWareUser(result.User);

            if (result.mwUser == null)
                throw new NoMetawareUserException();


            //Status des Calls auf dem Server aktualisieren und den Call aus der tblCalls entfernen
            if (!addCallAtEndOfList)
            {

                Call checkCall;
                checkCall = this.metaCallBusiness.SponsoringCallManager.CheckCallExists(call.CallJob);

                if (checkCall == null)
                {
                    //falls der Call auf dem Server schon wieder gelöscht ist, muss er wieder erstellt werden.
                    checkCall = this.metaCallBusiness.SponsoringCallManager.GetSingleCall(call.CallJob,
                                       this.metaCallBusiness.Users.GetUserInfo(this.metaCallBusiness.Users.CurrentUser));
                    call.CallId = checkCall.CallId;
                }

                metaCallBusiness.ServiceAccess.UpdateCall(call.CallId, state);
                //Der CallJob muss neu abgerufen werdenund synchronisiert werden, 
                // da sich Status und IterationCounter geändert haben 
                CallJob callJob = this.metaCallBusiness.CallJobs.Get(call.CallJob.CallJobId);
                if (callJob.State != call.CallJob.State)
                {
                    call.CallJob.State = callJob.State;
                }

                if (callJob.IterationCounter != call.CallJob.IterationCounter)
                {
                    call.CallJob.IterationCounter = callJob.IterationCounter;
                }
            }

            // Sollte bei den folgenden Datenbankoperationen ein Fehler auftreten, 
            // so müssen die nachfolgenden Schritte trotzdem durchgeführt werden
            // da der CallJob-Status schon gesetzt wurde

            try
            {
                //Sponsor aktualisieren
                // Wichtig -> das SponsorUpdate MUSS vor dem Create-CallJobResult 
                // durchgeführt werden, für einen neuen Auftrag die Sponsordaten aus dem 
                // AdressenPool gezogen werden.
                metaCallBusiness.Addresses.UpdateSponsor(result.CallJob.Sponsor);
            }
            catch { }

            try
            {
                // Wenn der Status Durring ist muss die Mahnstufe erhöht werden und die Notizen gespeichert werden.
                if (call.CallJob.GetType() == typeof (DurringCallJob) && 
                    result.ContactType.ContactTypeId.Equals(ContactType.DurringGespraechMoeglichId))
                {
                    //aktualisieren
                    metaCallBusiness.CallJobResults.CreateDurring((CallJobPossibleResult)result);
                }
            }
            catch { }

            //Speichern des CallJobResults
            try
            {
                metaCallBusiness.CallJobResults.Create(result);

                CallJobReminder reminder = null;
                if (result.GetType() == typeof(CallJobReminderResult))
                {
                    reminder = ((CallJobReminderResult)result).CallJobReminder;
                }
                if ((result.GetType() == typeof(CallJobPossibleResult)) &&
                    (((CallJobPossibleResult)result).Reminder != null))
                {
                    reminder = ((CallJobPossibleResult)result).Reminder;
                }

                //Prüfung auf Reminder und gegebenenfalls den CallJobReminder erstellen
                if (reminder != null)
                {
                    //Erstellen des Reminders
                    metaCallBusiness.CallJobReminders.Create(reminder);

                    //Wenn der Reminder noch am gleichen Tag erfolgen soll werden 
                    // neue Reminder abgeholt
                    //if(typeof(reminder.CallJob)
                    if (reminder.ReminderDateStart.Date == DateTime.Today && 
                        reminder.CallJob.GetType() != (typeof(DurringCallJob)))
                        GetNextReminderCallsFromDatabase();
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            

            // entfernen des Calls aus den lokalen Listen
            if (call is ReminderCall)
            {
                if (addCallAtEndOfList)
                    AddCallAtEndOfList<ReminderCall>(this.reminderCallList, (ReminderCall)call);
                else
                    RemoveReminderCallFromList((ReminderCall)call);
            }
            else
            {
                //Achtung --> Unterscheiden zwischen Sponsor und Durring Call
                if (addCallAtEndOfList)
                    AddCallAtEndOfList<Call>(this.sponsoringCallList, call);
                else
                    if (call.CallJob.GetType() == (typeof(DurringCallJob)))
                    {
                        //if (actDurringInfo == null)
                        //{
                        //    foreach (DurringInfo dInfo in durringInfo)
                        //    {
                        //        //Console.WriteLine(dInfo.CallJobId.ToString());
                        //        if (dInfo.CallJobId.CompareTo(call.CallJob.CallJobId) == 0)
                        //            actDurringInfo = dInfo;
                        //    }
                        //}
                        //durring call job
                        RemoveDurringCallFromList(call);
                    }
                    else
                    {
                        //sponsoring Call Job
                        RemoveSponsoringCallFromList(call);
                    }

            }

            //erhöhen des Call-Zählers
            callsDone++;

        }

        /// <summary>
        /// Setzt einen Call ans Ende der Liste
        /// </summary>
        /// <param name="list"></param>
        /// <param name="call"></param>
        private void AddCallAtEndOfList<T>(List<T> list, T call) where T: Call
        {
            lock (list)
            {
                list.Remove(call);

                call.ExpirationDate = call.ExpirationDate.AddMinutes(3);

                list.Add(call);
            }
        }

        private CallJobState GetCallJobState(CallJobResult callJobResult, bool reminderCreated, out bool addCallAtEndOfList)
        {
            //TODO: Hier muss eine andere Möglichkeit gefunden werden, den ContactType zu unterscheiden
            //string contactType = callJobResult.ContactType.DisplayName;
            ContactType contactType = callJobResult.ContactType;

            CallJobState state = CallJobState.Invalid;
            addCallAtEndOfList = false;



            if (contactType.ContactTypeId == ContactType.NichtErreichbarId)
            {
                if (reminderCreated)
                    state = CallJobState.ReminderCallJob;
                else
                {
                    state = CallJobState.FurtherCall;
                    addCallAtEndOfList = false;
                }
            }
            else if (contactType.ContactTypeId == ContactType.DurringNichtErreichbarId)
            {
                if (reminderCreated)
                    state = CallJobState.ReminderCallJob;
                else
                {
                    state = CallJobState.FurtherCall;
                    addCallAtEndOfList = false;
                }
            }
            else if (contactType.ContactTypeId == ContactType.AdresseDoppeltId)
            {
                //state = CallJobState.Waiting;
                state = CallJobState.Unsuitable;
            }
            else if (contactType.ContactTypeId == ContactType.GespraechMoeglichId)
            {
                if (reminderCreated)
                    state = CallJobState.ReminderCallJob;
                else
                {
                    CallJobPossibleResult result = callJobResult as CallJobPossibleResult;
                    Guid participationId = result.ContactTypesParticipation.ContactTypesParticipationId;

                    //TODO: Hier muss eine andere Unterscheidungsmöglichkeit gefunden werden
                    //Teilnahme an der Aktion (Ja/nein)
                    if (participationId.Equals(ContactTypesParticipation.JaId))
                        state = CallJobState.Ordered;
                    else
                        state = CallJobState.Cancelled;
                }
            }
            else if (contactType.ContactTypeId == ContactType.DurringGespraechMoeglichId)
            {
                if (reminderCreated)
                    state = CallJobState.ReminderCallJob;
                else
                {
                    CallJobPossibleResult result = callJobResult as CallJobPossibleResult;
                    state = CallJobState.Durring;
                }
            }

            else if (contactType.ContactTypeId == ContactType.BesetztId)
            {
                if (reminderCreated)
                    state = CallJobState.ReminderCallJob;
                else
                {
                    state = CallJobState.FurtherCall;
                    addCallAtEndOfList = false;
                }
            }
            else if (contactType.ContactTypeId == ContactType.DurringBesetztId)
            {
                if (reminderCreated)
                    state = CallJobState.ReminderCallJob;
                else
                {
                    state = CallJobState.FurtherCall;
                    addCallAtEndOfList = false;
                }
            }
            else if (contactType.ContactTypeId == ContactType.NummerFalschId)
            {
                //state = CallJobState.Waiting;
                state = CallJobState.Unsuitable;
            }
            else if (contactType.ContactTypeId == ContactType.AnrufbeantworterId)
            {
                if (reminderCreated)
                    state = CallJobState.ReminderCallJob;
                else
                {
                    state = CallJobState.FurtherCall;
                    addCallAtEndOfList = false;
                }
            }
            else if (contactType.ContactTypeId == ContactType.DurringAnrufbeantworterId)
            {
                if (reminderCreated)
                    state = CallJobState.ReminderCallJob;
                else
                {
                    state = CallJobState.FurtherCall;
                    addCallAtEndOfList = false;
                }
            }
            else if (contactType.ContactTypeId == ContactType.DurringKeineZahlungId)
            {
                if (reminderCreated)
                    state = CallJobState.ReminderCallJob;
                else
                {
                    state = CallJobState.DurringDone;
                    addCallAtEndOfList = false;
                }
            }
            else if (contactType.ContactTypeId == ContactType.AdresseNichtGeeignetId)
            {
                state = CallJobState.Unsuitable;
            }
            
            
            return state;            
        }

        private void RemoveReminderCallFromList(ReminderCall call)
        {
            lock (syncRootReminderCalls)
            {
                this.reminderCallList.Remove(call);
            }
            OnCallJobsChanged(EventArgs.Empty);
        }

        //Canceled einen CallJob
        private void CancelCall(Call call)
        {
            RemoveSponsoringCallFromList(call);
        }

        /// <summary>
        /// Entfernt einen DurringCall von der Liste
        /// </summary>
        /// <param name="call"></param>
        public void RemoveDurringCallFromList(Call call)
        {
            durringInfo.Remove(actDurringInfo);
            this.durringCallList.Remove(call);
            OnDurringInfoChanged(new DurringInfoChangedEventArgs(actDurringInfo));
        }
        
        /// <summary>
        /// Entfernt einen SponsoringCall von der Liste
        /// </summary>
        /// <param name="call"></param>
        private void RemoveSponsoringCallFromList(Call call)
        {
            lock (syncRootSponsoringCalls)
            {
                this.sponsoringCallList.Remove(call);
            }
            OnCallJobsChanged(EventArgs.Empty);
        }

        /// <summary>
        /// Fügt einen Call zu der Liste hinzu
        /// </summary>
        /// <param name="call"></param>
        private void AddSponsoringCallToList(Call call)
        {
            lock (syncRootSponsoringCalls)
            {
                //Debug.WriteLine(call.CallJob.CallJobId.ToString() + "###" + call.CallJob.IterationCounter);
                this.sponsoringCallList.Add(call);
            }
        }

        private int GetNextSponsoringCallsFromDatabase(int numberOfCallJobs, TimeSpan expirationDate )
        {
            
            CallRequestMessage message = new CallRequestMessage();
            message.ExpirationMinutes = (int)  expirationDate.TotalMinutes;
            message.NumberOfCallJobs = numberOfCallJobs;
            message.Project = this.currentProject;
            message.CurrentUser = this.currentUser;
            message.CallJobGroup = this.currentCallJobGroup;
     
            Call[] calls = metaCallBusiness.ServiceAccess.GetNextSponsoringCalls(message);

            foreach(Call call in calls)
            {
                AddSponsoringCallToList(call);
                //Den anderen Threads Gelegenheit zum arbeiten geben
                System.Threading.Thread.Sleep(0);
            }

            return calls.Length;
        }

        /// <summary>
        /// Ruft eine Liste aller aktuellen Reminder ab
        /// </summary>
        /// <returns></returns>
        private void GetNextReminderCallsFromDatabase()
        {

            User user = this.currentUser;
            ProjectInfo project = this.currentProject;
            //Reminders die heute anstehen
            DateTime reminderdate = DateTime.Today.Add(new TimeSpan(23,59,59));

            //TODO: diesen Wert entweder aus den Anwendungseinstellungen oder dem User-Profil holen
            int maxTeamReminders = 2;
            double expirationSeconds = Math.Max(averageTimePerCall.TotalSeconds * 1.5  , 120.0);

            if (user == null)
                throw new NoUserLoggedOnException();

            if (project == null)
                throw new NoProjectLoggedOnException();


            ReminderCallRequestMessage message = new ReminderCallRequestMessage();
            message.Project = project;
            message.User = user;
            message.ReminderRequestDate = reminderdate;
            message.MaxTeamReminders = maxTeamReminders;
            message.ExpirationDate = DateTime.Now.AddSeconds(expirationSeconds);
            message.CallJobGroup = currentCallJobGroup;

            ReminderCall[] newCalls = new ReminderCall[0];
            lock (syncRootReminderCalls)
            {
                //hinzufügen der neuen Reminder
                newCalls = metaCallBusiness.ServiceAccess.GetNextReminderCalls(message);
                //Vorsortieren
                Array.Sort(newCalls, this.CompareReminderCalls);
                //Einordnen der Reminder
                AddReminderCallsRanked(reminderCallList, newCalls);

                //CheckExpiration(this.reminderCallList, GetTeamReminders(this.reminderCallList));

            }
            if (newCalls.Length > 0)
            {
                OnCallJobsChanged(EventArgs.Empty);
            }

        }

        private List<ReminderCall> GetTeamReminders(List<ReminderCall> reminderCallList)
        {
            return reminderCallList.FindAll(new Predicate<ReminderCall>(delegate(ReminderCall x) { return (x.CallJobReminder.User == null); }));
        }
     
        private void SponsoringCallMonitor_Tick(object state)
        {
            //Berechnen, ob und wieviel Calls geholt werden müssen


            int callsReceived = 0;
            int availableJobs = 0;

            foreach (Call call in this.sponsoringCallList)
            {
                if (call.ExpirationDate > System.DateTime.Now)
                    availableJobs++;
            }

            lock (syncRootSponsoringCalls)
            {
                //System.Diagnostics.Trace.WriteLine(string.Format("SponsoringCallMonitor_Tick at {0}", DateTime.Now));

                AutoResetEvent autoEvent = (AutoResetEvent)state;

                //Neue Calljobs holen, wenn:
                // 1) die Anzahl der Calls die festgelegte mindestanzahl unterschritten hat
                // oder
                // 2) die durchschnittliche Bearbeitungszeit * die verbliebenen Calljobs kleiner als die Zeit bis zum nächsten Tick ist.
                bool receiveNewCallJobs = (
                    (availableJobs < this.minCallJobs) ||
                    (this.averageTimePerCall.TotalSeconds * this.TotalCallsInBuffer < this.sponsoringCallMonitorPeriod.TotalSeconds));


                if (receiveNewCallJobs)
                {
                    //Die Anzahl der abzuholenden CallJobs wird auf die Anzahl seit dem letzten Abholen gesetzt
                    int numberOfCallJobs = this.numberOfCallsToRetrieve;
                    if (callsDone == 0 )
                        numberOfCallJobs = this.minCallJobs;
                    else
                    {
                        if (callsDone <= this.numberOfCallsToRetrieve)
                            numberOfCallJobs = ++this.numberOfCallsToRetrieve;
                        else
                            numberOfCallJobs = --this.numberOfCallsToRetrieve;
                    }
                    //Die Verfallszeit wird auf das 1.5fache der durchschnittlichen Bearbeitungszeit gesetzt
                    TimeSpan expirationDate = new TimeSpan((long)(averageTimePerCall.Ticks * 1.5));

                    callsReceived = GetNextSponsoringCallsFromDatabase(numberOfCallJobs, expirationDate);
                    
                    //System.Diagnostics.Trace.WriteLine(string.Format("neue CallJobs wurden geholt (Anzahl {0}\t Zeitpunkt: {1})", numberOfCallJobs, DateTime.Now));

                    //Setzen der Statusvariablen
                    this.numberOfCallsToRetrieve = numberOfCallJobs;
                    lastCall = DateTime.Now;
                    callsDone = 0;
                }
                autoEvent.Set();
            }

            if (callsReceived > 0 )
                OnCallJobsChanged(EventArgs.Empty);

            //Entfernen der abgelaufenen Calls
            //CheckExpiration(this.sponsoringCallList, this.sponsoringCallList.AsReadOnly());

        }

        private void ReminderCallMonitor_Tick(object state)
        {
            System.Diagnostics.Trace.WriteLine(string.Format("ReminderCallMonitor_Tick at {0}", DateTime.Now));
            
            GetNextReminderCallsFromDatabase();
        }

        public bool CallsAvailable
        {
            get
            {
                if (this.sponsoringCallList.Count > 0)
                    return true;

                if (this.reminderCallList.Count > 0)
                {
                    //prüfen ob der erste Call in der Liste auch wircklich abgerufen wird
                    ReminderCall call = this.reminderCallList[0];
                    if (call.CallJobReminder.ReminderDateStart <= DateTime.Now)
                        return true;
                }

                return false;
            }
        }

        public int TotalCallsInBuffer
        {
            get { return this.sponsoringCallList.Count + this.reminderCallList.Count; }
        }

        public int SponsoringCallsInBuffer
        {
            get
            {
                return this.sponsoringCallList.Count;
            }
        }

        public int ReminderCallsInBuffer
        {
            get
            {
                return reminderCallList.Count;                    
            }
        }

        public ReadOnlyCollection<ReminderCall> ReminderCalls
        {
            get
            {
                return this.reminderCallList.AsReadOnly();
            }
        }

        /// <summary>
        /// Vergleicht das ReminderDateStart zweier ReminderCall-Objekte 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private int CompareReminderCalls(ReminderCall x, ReminderCall y)
        {
            CallJobReminder reminder1 = x.CallJobReminder;
            CallJobReminder reminder2 = y.CallJobReminder;

            //StartDatum vergleichen
            int resultStart = reminder1.ReminderDateStart.CompareTo(reminder2.ReminderDateStart);
            //StopDatum vergleichen
            int resultStop = reminder1.ReminderDateStop.CompareTo(reminder2.ReminderDateStop);


            int result = 0;
            if (resultStart == resultStop)
                result = resultStart;
            else if (resultStart > 0 && resultStop >= 0)
                result = resultStart;
            else if (resultStart > 0 && resultStop < 0)
                result = resultStart;
            else
                result = resultStop;

            return result;
        }

        private bool IsNewReminderCallAlreadyInList(List<ReminderCall> reminderCallList, ReminderCall call)
        {
            foreach (ReminderCall oldCall in reminderCallList)
            {
                if (oldCall.CallJob.CallJobId == call.CallJob.CallJobId)
                    return true;
            }
            return false;
        }

        private void AddReminderCallsRanked(List<ReminderCall> reminderCallList, ReminderCall[] newCalls)
        {
            if (newCalls == null)
                throw new ArgumentNullException("reminderCallList");


            //Alle neuen Calls durchlaufen und prüfen an welcher Stelle in der Liste 
            // der jewielige Call hinzugefügt werden kann
            for (int j = 0; j < newCalls.Length; j++)
            {
                ReminderCall call = newCalls[j];

                //eventuell wird die tblCalls auf dem SQL-Server nicht schnell genug aktualisiert und 
                //die Wiedervorlagen kämen wieder
                //if (IsNewReminderCallAlreadyInList(reminderCallList, call))
                //    continue;

                //ist die sortierte Liste leer, wird der Call an erster Stelle hinzugefügt
                if (reminderCallList.Count == 0)
                {
                    reminderCallList.Add(call);
                    continue;
                }

                // Ist die Liste nicht leer, wird in der sortierten Liste 
                // nach einem älteren Element gesucht und dann davor das 
                // neue Call-Objekt eingefügt, sofern die möglichkeit besteht, 
                // das aktuelle Projekt zu unterbrechen oder beide Projekte gleich sind
                for (int i = 0; i < reminderCallList.Count; i++)
                {
                    ReminderCall rankedCall = reminderCallList[i];
                    // <0 ^ call.Datum < rankedCall.Datum
                    // =0 ^ call.Datum = rankedCall.Datum
                    // >0 ^ call.Datum > rankedCall.Datum
                    int dateComparsion = CompareReminderCalls(rankedCall, call);
                    //prüfen ob ein Einfügen überhaupt möglich wäre
                    bool insertingAllowed = ((call.CallJobReminder.ReminderMode == CallJobReminderMode.DisturbCurrentProject) ||
                        (call.CallJob.Project.ProjectId.Equals(rankedCall.CallJob.Project.ProjectId)));

                    bool canBeWorked = false;// ((call.CallJobReminder.ReminderTracking == CallJobReminderTracking.OnlyTimeSpan) &&
                         //(call.CallJobReminder.ReminderDateStart < DateTime.Now));

                    // Wenn einfügen erlaubt und dateComparsion > 0 vor der aktuellen Position einfügen
                    if (insertingAllowed && 
                        ((dateComparsion > 0) ||
                        (dateComparsion < 0 && canBeWorked)))
                    {
                        reminderCallList.Insert(i, call);
                        break;
                    }

                    // Ansonsten
                    //reminderCallList.Add(call);
                    //break;
                }
                // Konnte der Call noch nicht hinzugefügt werden,
                // wird er am ende angefügt
                if (!reminderCallList.Contains(call))
                    reminderCallList.Add(call);

            }
        }
        
        protected void OnCallJobsChanged(EventArgs e)
        {
            if (CallJobsChanged != null && 
                raiseEvents)
                CallJobsChanged(this, e);
        }

        private void CheckExpiration(System.Collections.IList callList, System.Collections.IList callsToCheck)
        {
            List<Call> callToRemove = new List<Call>();


            foreach (Call call in callsToCheck)
            {
                if (call != this.currentCall
                    && call.ExpirationDate < DateTime.Now)
                {
                    callToRemove.Add(call);
                }
            }

            if (callToRemove.Count > 0)
            {
                this.metaCallBusiness.ServiceAccess.ReleaseCalls((Call[])callToRemove.ToArray());
            }
            
            foreach (Call call in callToRemove)
            {
                if (callList.Contains(call))
                {
                    callList.Remove(call);
                    //metaCallBusiness.ServiceAccess.ReleaseCall(call.CallId);
                }
            }

            if (callToRemove.Count > 0)
                OnCallJobsChanged(EventArgs.Empty);

        }

        private void ExpirationTimer_Tick(object state)
        {
            //System.Diagnostics.Trace.WriteLine(string.Format("ExpirationTimer_Tick at {0}", DateTime.Now));

            int countBeforeCheck;
            //prüfen der SponsoringCalls
            countBeforeCheck = this.sponsoringCallList.Count;
            CheckExpiration(this.sponsoringCallList, this.sponsoringCallList.AsReadOnly());
            if (countBeforeCheck != this.sponsoringCallList.Count)
                OnCallJobsChanged(EventArgs.Empty);

            //prüfen der ReminderCalls
            countBeforeCheck = this.reminderCallList.Count;
            CheckExpiration(this.reminderCallList, GetTeamReminders(this.reminderCallList));
            if (countBeforeCheck != this.reminderCallList.Count)
                OnCallJobsChanged(EventArgs.Empty);

            //prüfen der Reminder, die aufgrund der ReminderStopDatums verfallen sind.
            countBeforeCheck = this.reminderCallList.Count;
            RevokeRemindersByStopDate(this.reminderCallList);
            if (countBeforeCheck != this.reminderCallList.Count)
                OnCallJobsChanged(EventArgs.Empty);

        }

        private void RevokeRemindersByStopDate(List<ReminderCall> reminderCalls)
        {
            List<ReminderCall> remindersToRemove = new List<ReminderCall>();

            foreach (ReminderCall call in reminderCalls)
            {
                CallJobReminder reminder = call.CallJobReminder;
                if (reminder.ReminderTracking == CallJobReminderTracking.OnlyTimeSpan)
                {
                    if (reminder.ReminderDateStop.TimeOfDay < DateTime.Now.TimeOfDay)
                    {
                        remindersToRemove.Add(call);
                    }
                }
            }


            if (remindersToRemove.Count > 0)
            {
                this.metaCallBusiness.ServiceAccess.ReleaseCalls((Call[])remindersToRemove.ToArray());
            }

            foreach (ReminderCall call in remindersToRemove)
            {
                reminderCalls.Remove(call);
                //metaCallBusiness.ServiceAccess.ReleaseCall(call.CallId);
            }
        }

        public ReminderCall GetSingleReminderCall(CallJob callJob, UserInfo userInfo)
        {
            if (callJob == null)
                throw new ArgumentNullException("callJob");

            if (userInfo == null)
                throw new ArgumentNullException("userInfo");

            ReminderCall reminderCall = this.metaCallBusiness.ServiceAccess.GetSingleReminderCall(callJob, userInfo);

            if (reminderCall == null)
                throw new CannotReceiveCallException();

            if (reminderCall.CallJob.GetType() == (typeof(SponsoringCallJob)))
            {
                if (reminderCall == null)
                    throw new CannotReceiveCallException();

                RemoveReminderCallFromList(reminderCall);
            }
            return reminderCall;
        }

        public ReminderCall CheckReminderCallExists(CallJob callJob)
        {
            ReminderCall reminderCall = null;

            if (callJob == null)
                throw new ArgumentNullException("callJob");

            foreach(ReminderCall rc in reminderCallList)
            {
                if (rc.CallJob.CallJobId == callJob.CallJobId)
                {
                    reminderCall = rc;
                    break;
                }
            }

            if (reminderCall == null)
                reminderCall = this.metaCallBusiness.ServiceAccess.CheckReminderCallExists(callJob);

            if (reminderCall != null && reminderCall.CallJob.GetType() == (typeof(SponsoringCallJob)))
            {
                RemoveReminderCallFromList(reminderCall);
            }

            return reminderCall;
        }

        public Call GetSingleCall(CallJob callJob, UserInfo userInfo)
        {
            if (callJob == null)
                throw new ArgumentNullException("callJob");

            if (userInfo == null)
                throw new ArgumentNullException("userInfo");

            Call call = this.metaCallBusiness.ServiceAccess.GetSingleCall(callJob, userInfo);

            if (call == null)
                throw new CannotReceiveCallException();

            if (call.CallJob.GetType() == (typeof(SponsoringCallJob)))
            {
                if (call == null)
                    throw new CannotReceiveCallException();

                RemoveSponsoringCallFromList(call);
            }
            return call;

        }

        public Call CheckCallExists(CallJob callJob)
        {
            Call call = null;

            if (callJob == null)
                throw new ArgumentNullException("callJob");

            foreach (Call ca in sponsoringCallList)
            {
                if (ca.CallJob.CallJobId == callJob.CallJobId)
                {
                    call = ca;
                    break;
                }
            }

            if (call == null)
                call = this.metaCallBusiness.ServiceAccess.CheckCallExists(callJob);

            if (call != null && call.CallJob.GetType() == (typeof(SponsoringCallJob)))
            {
                RemoveSponsoringCallFromList(call);
            }
            return call;
        }
    
    }



}
