using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel;

using metatop.Applications.metaCall.DataObjects;
using metatop.Applications.metaCall.ServiceAccessLayer;
using System.Threading;
using System.Collections.Specialized;

namespace metatop.Applications.metaCall.BusinessLayer
{
    public class CallJobBusiness
    {
        private MetaCallBusiness metaCallBusiness;
      
        internal CallJobBusiness(MetaCallBusiness metaCallBusiness)
        {
            this.metaCallBusiness = metaCallBusiness;

            //Delegates für Asynchrone MethodenAufrufe initialisieren
            InitializeDelegates();
        }

        /// <summary>
        /// Gibt Adressen zum telefonieren frei und erstellt CallJobs für ein Projekt
        /// </summary>
        /// <param name="releaseInfoList"></param>
        public void CreateByAddressReleaseInfos(List<AddressReleaseInfo> releaseInfoList)
        {
            metaCallBusiness.ServiceAccess.CreateCallJobsByAddressReleaseInfos(releaseInfoList.ToArray());
        }

        /// <summary>
        /// Initialisiert die Delegates für Asynchrone MethodenAufrufe
        /// </summary>
        private void InitializeDelegates()
        {
            this.onGetCallJobsProgressReportDelegate = new SendOrPostCallback(GetCallJobsReportProgress);
            this.onGetCallJobsCompletedDelegate = new SendOrPostCallback(GetCallJobsCompletedMethod);
            this.getCallJobsCompletionMethodDelegate = new SendOrPostCallback(GetCallJobsCompletionMethod);
        }

        /// <summary>
        /// Liefert Liste mit CallJobresult-Instanzen zu einer Instanz der Klasse CallJob
        /// </summary>
        /// <param name="callJob"></param>
        /// <returns></returns>
        public List<CallJobResult> GetCallJobResults(CallJob callJob)
        {
            if (callJob == null)
                throw new ArgumentNullException("callJob");

            return new List<CallJobResult>(metaCallBusiness.ServiceAccess.GetCallJobResultsForCallJob(callJob));
        }

        /// <summary>
        /// liefert eine Auflistung der Auftragsverteilung pro Stunde
        /// </summary>
        /// <param name="centerId"></param>
        /// <param name="teamId"></param>
        /// <param name="userId"></param>
        /// <param name="projectId"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public List<SponsoringOrdersDistribution> GetSponsoringOrdersDistribution(
                        Guid? centerId,
                        Guid? teamId,
                        Guid? userId,
                        Guid? projectId,
                        DateTime from,
                        DateTime to)
        {
            return new List<SponsoringOrdersDistribution>(metaCallBusiness.ServiceAccess.GetSponsoringOrdersDistribution(
                        centerId,
                        teamId,
                        userId,
                        projectId,
                        from,
                        to));
        }

        public DateTime? GetLastAddressContact(int adressenPoolNummer, Guid projectId)
        {
            return metaCallBusiness.ServiceAccess.GetLastAddressContact(adressenPoolNummer, projectId);
        }


        /// <summary>
        /// Aktualisiert einen CallJob auf dem Server
        /// </summary>
        /// <param name="callJob"></param>
        public void UpdateCallJob(CallJob callJob)
        {
            if (callJob == null)
                throw new ArgumentNullException("callJob");

            metaCallBusiness.ServiceAccess.UpdateCallJob(callJob);
        }

        /// <summary>
        /// Aktualisiert den DialMode in allen Calljobs eines Projekts
        /// </summary>
        /// <param name="project"></param>
        /// <param name="dialMode"></param>
        public void UpdateCallJobsDialModeByProject(Project project, DialMode dialMode)
        {
            if (project == null)
                throw new ArgumentNullException("project");

            metaCallBusiness.ServiceAccess.UpdateCallJobsByProject(project, dialMode);
        }


        /// <summary>
        /// liefert einen neuen leeren Sponsoring CallJob
        /// </summary>
        /// <returns></returns>
        public CallJob GetNewSponsorinCallJob()
        {
            return this.metaCallBusiness.ServiceAccess.GetNewSponsoringCallJob();
        }

        /// <summary>
        /// Liefert eine Liste von CallJobs aufgrund einer Project-Instanz
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public List<SponsoringCallJob> GetSponsoringCallJobsByProject(Project project)
        {
            if (project == null)
                throw new ArgumentNullException("project");

            return new List<SponsoringCallJob>(this.metaCallBusiness.ServiceAccess.GetSponsoringCallJobsByProject(project));
        }

        /// <summary>
        /// Liefert eine Liste von DurringCallJobs von einem User
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public List<DurringCallJob> GetDurringCallJobsByUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException("User");

            return new List<DurringCallJob>(this.metaCallBusiness.ServiceAccess.GetDurringCallJobsByUser(user));
        }

        /// <summary>
        /// Liefert eine Liste aller Mahnungen für einen User
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<DurringInfo> GetDurringInfosByUser(User user, int mahnstufe2)
        {
            if (user == null)
                throw new ArgumentNullException("User");

            return new List<DurringInfo>(this.metaCallBusiness.ServiceAccess.GetDurringInfoByUser(user, mahnstufe2));
        }

        /// <summary>
        /// Liefert eine Liste aller Mahnstufen eines Users
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<DurringLevelInfo> getDurringLevelInfosByUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException("User");

            return new List<DurringLevelInfo>(this.metaCallBusiness.ServiceAccess.GetDurringLevelInfoByUser(user));
        }

        /// <summary>
        /// Erstellt neue Durrings
        /// </summary>
        /// <param name="user"></param>
        public void DurringCreate(User user)
        {
            if (user == null)
                throw new ArgumentNullException("User");

            this.metaCallBusiness.ServiceAccess.DurringCreate(user);
        }

        private bool durringActiv;
        public bool DurringActiv
        {
            get { return durringActiv; }
            set {

                bool raiseEvent = (this.durringActiv != value);
                
                durringActiv = value;

                if (raiseEvent)
                    this.metaCallBusiness.ActivityLogger.Log(new Activities.DurringChanged(this.durringActiv));
            
            }
        }

        private int mahnstufe2;
        public int Mahnstufe2
        {
            get { return mahnstufe2; }
            set { mahnstufe2 = value; }
        }

        /// <summary>
        /// Liefert eine Liste mit CallJobs des Benutzers und des Projects .. das Projekt darf null sein
        /// </summary>
        /// <param name="user"></param>
        /// <param name="project"></param>
        /// <returns></returns>
        public List<CallJob> GetCallJobsByUserAndProject(UserInfo user, ProjectInfo project, string expression, bool isAdminMode)
        {

            if (user == null)
                throw new ArgumentNullException("user");

            if (isAdminMode && 
                !System.Threading.Thread.CurrentPrincipal.IsInRole(MetaCallPrincipal.AdminRoleName))
                throw new System.Security.SecurityException("Cannot use Admin Mode without Admin-UserRole");
                

            return new List<CallJob>(this.metaCallBusiness.ServiceAccess.GetCallJobsByUserAndProject(user, project, expression, isAdminMode));

        }

        /// <summary>
        /// Liefert eine Liste mit CallJobs des Benutzers und des Projects .. das Projekt darf null sein
        /// Ablehnungen können ausgeschlossen werden
        /// </summary>
        /// <param name="user"></param>
        /// <param name="project"></param>
        /// <returns></returns>
        public List<CallJob> GetCallJobsByUserAndProject(UserInfo user, ProjectInfo project,
            string expression, bool isAdminMode, bool excludeRefusals)
        {

            if (user == null)
                throw new ArgumentNullException("user");

            if (isAdminMode &&
                !System.Threading.Thread.CurrentPrincipal.IsInRole(MetaCallPrincipal.AdminRoleName))
                throw new System.Security.SecurityException("Cannot use Admin Mode without Admin-UserRole");


            return new List<CallJob>(this.metaCallBusiness.ServiceAccess.GetCallJobsByUserAndProject(user, project, expression, isAdminMode, excludeRefusals));

        }

        /// <summary>
        /// Liefert einen einzelnen CallJob anhand der CallJobId vom Server
        /// </summary>
        /// <param name="callJobId"></param>
        /// <returns></returns>
        public CallJob Get(Guid callJobId)
        {

            if (callJobId == Guid.Empty)
                throw new ArgumentNullException("callJobId");

            return this.metaCallBusiness.ServiceAccess.GetCallJob(callJobId);
        }

        /// <summary>
        /// Liefert einen einzelnen CallJob anhand der Addressid und Projektid vom Server
        /// </summary>
        /// <param name="addressId"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public CallJob Get(Guid addressId, Guid projectId)
        {
            if (addressId == Guid.Empty)
                throw new ArgumentNullException("AddressId");

            if (projectId == Guid.Empty)
                throw new ArgumentNullException("ProjectId");

            return this.metaCallBusiness.ServiceAccess.GetCallJob(addressId, projectId);
        }


        public void TransferUnusedCallJobs(Guid sourceProjectId, Guid targetProjectId)
        {
            if (sourceProjectId == Guid.Empty)
                throw new ArgumentNullException("sourceProjectId");

            if (targetProjectId == Guid.Empty)
                throw new ArgumentNullException("targetProjectId");

            this.metaCallBusiness.ServiceAccess.TransferUnusedCallJobs(sourceProjectId, targetProjectId);
        }

        public int TransferUnusedCallJobsCount(Guid sourceProjectId)
        {
            if (sourceProjectId == Guid.Empty)
                throw new ArgumentNullException("sourceProjectId");

            return this.metaCallBusiness.ServiceAccess.TransferUnusedCallJobsCount(sourceProjectId);
        }

        #region Statistics
        /// <summary>
        /// Liefert eine Instanz der Klasse CallJobStatistics aufgrund eine ProjectInfo-Instanz.
        /// </summary>
        /// <param name="projectinfo"></param>
        /// <returns></returns>
        public CallJobStatistics GetStatistics(ProjectInfo projectinfo, Guid? userId, int resultType)
        {
            if (projectinfo == null)
                throw new ArgumentNullException("projectinfo");

            return this.metaCallBusiness.ServiceAccess.GetCallJobStatistics(projectinfo, userId, resultType);
        }

        /// <summary>
        /// Liefert eine Instanz der Klasse CallJobStatistics aufgrund einer CallJobGroup-Instanz
        /// </summary>
        /// <param name="callJobGroup"></param>
        /// <returns></returns>
        public CallJobStatistics GetStatistics(CallJobGroupInfo callJobGroup, Guid? userId, int resultType)
        {
            if (callJobGroup == null)
                throw new ArgumentNullException("callJobGroup");

            return this.metaCallBusiness.ServiceAccess.GetCallJobStatistics(callJobGroup, userId, resultType);
        }
        #endregion


        #region CallJobStateInfo
        /// <summary>
        /// Ruft eine Liste von CallJobStateInfo-Objekten ab
        /// </summary>
        public List<CallJobStateInfo> CallJobStates
        {
            get
            {
                return new List<CallJobStateInfo>(this.metaCallBusiness.ServiceAccess.GetAllCallJobStateInfos());
            }
        }

        /// <summary>
        /// Liefert eine Instanz der CallJobStateInfo-Klasse des angegebenen CallJobState-Wertes
        /// </summary>
        /// <param name="callJobState"></param>
        /// <returns></returns>
        public CallJobStateInfo GetCallJobState(CallJobState callJobState)
        {
            return this.metaCallBusiness.ServiceAccess.GetCallJobStateInfo(callJobState);
        }

        #endregion

        #region Asynchrones Abrufen von CallJobs
        public event GetCallJobsProgressChangedEventHandler GetCallJobsProgressChanged;
        public event GetCallJobsCompletedEventHandler GetCallJobsCompleted;

        private SendOrPostCallback onGetCallJobsProgressReportDelegate;
        private SendOrPostCallback onGetCallJobsCompletedDelegate;
        private SendOrPostCallback getCallJobsCompletionMethodDelegate;

        private delegate void GetCallJobsWorkerEventHandler(Project project, AsyncOperation asynOp, SendOrPostCallback completionMethodDelegate);

        private GetCallJobsWorkerEventHandler getCallJobsWorkerDelegate;

        private HybridDictionary userTokenToLifeTime = new HybridDictionary();


        public void GetCallJobsByProjectAsync(Project project, object taskId)
        {

            AsyncOperation asyncOp = AsyncOperationManager.CreateOperation(taskId);

            lock (userTokenToLifeTime.SyncRoot)
            {
                if (userTokenToLifeTime.Contains(taskId))
                {
                    throw new ArgumentException(
                          "Task ID parameter muss eindeutig sein",
                          "taskId");
                }

                userTokenToLifeTime[taskId] = asyncOp;
            }


            getCallJobsWorkerDelegate = new GetCallJobsWorkerEventHandler(this.GetCallJobsWorker);
            getCallJobsWorkerDelegate.BeginInvoke(
                project, 
                asyncOp, 
                getCallJobsCompletionMethodDelegate,
                null,
                null);


        }

        public void CancelGetCallJobsAsync(object taskId)
        {
            throw new NotImplementedException();
        }


        private void GetCallJobsWorker(Project project, AsyncOperation asynOp, SendOrPostCallback completionMethodDelegate)
        {
            List<SponsoringCallJob> callJobs = new List<SponsoringCallJob>();
            Exception e = null;

            try
            {
                callJobs.AddRange(
                    this.metaCallBusiness.ServiceAccess.GetSponsoringCallJobsByProject(
                        project, asynOp, this.onGetCallJobsProgressReportDelegate));
            }
            catch (Exception ex)
            {
                e = ex;
            }

            GetCallJobsState getCallJobsState = new GetCallJobsState(
                project,
                callJobs,
                e,
                asynOp);

            //Aufruf des Delegates dass wir fertig sind!!!!
            completionMethodDelegate(getCallJobsState);
        }

        private void GetCallJobsCompletedMethod(object operationState)
        {
            GetCallJobsCompletedEventArgs e = operationState as GetCallJobsCompletedEventArgs;

            OnGetCallJobsCompleted(e);
            
        }

        private void OnGetCallJobsCompleted(GetCallJobsCompletedEventArgs e)
        {
            if (GetCallJobsCompleted != null)
                GetCallJobsCompleted(this, e);
        }

        private void GetCallJobsReportProgress(object operationState)
        {
            GetCallJobsProgressChangedEventArgs e = operationState as GetCallJobsProgressChangedEventArgs;

            OnGetCallJobsProgressChanged(e);
        }

        private void OnGetCallJobsProgressChanged(GetCallJobsProgressChangedEventArgs e)
        {
            if (GetCallJobsProgressChanged != null)
                GetCallJobsProgressChanged(e);
        }

        private void GetCallJobsCompletionMethod(object operationState)
        {
            GetCallJobsState getCallJobsState = operationState as GetCallJobsState;

            AsyncOperation asyncOp = getCallJobsState.AsyncOp;

            GetCallJobsCompletedEventArgs e = new GetCallJobsCompletedEventArgs(
                getCallJobsState.Project,
                getCallJobsState.CallJobs,
                getCallJobsState.Exception,
                false,
                asyncOp.UserSuppliedState);


            lock (userTokenToLifeTime.SyncRoot)
            {
                userTokenToLifeTime.Remove(asyncOp.UserSuppliedState);
            }

            asyncOp.PostOperationCompleted(onGetCallJobsCompletedDelegate, e);

        }



        #endregion

    }

    public delegate void GetCallJobsProgressChangedEventHandler(GetCallJobsProgressChangedEventArgs e);
    public delegate void GetCallJobsCompletedEventHandler(object sender, GetCallJobsCompletedEventArgs e);

    public class GetCallJobsCompletedEventArgs : AsyncCompletedEventArgs
    {

        public GetCallJobsCompletedEventArgs(Project project, List<SponsoringCallJob> callJobs, Exception e, bool canceled, object state)
            : base(e, canceled, state)
        {
            this.project = project;
            this.callJobs = callJobs;
        }


        private Project project;
        public Project Project
        {
            get { return project; }
        }

        private List<SponsoringCallJob> callJobs;
        public List<SponsoringCallJob> CallJobs
        {
            get { return callJobs; }
        }
    }

    internal class GetCallJobsState
    {
        public Project Project = null;
        public List<SponsoringCallJob> CallJobs = null;
        public Exception Exception = null;
        public AsyncOperation AsyncOp = null;

        public GetCallJobsState(
            Project project,
            List<SponsoringCallJob> callJobs,
            Exception ex,
            AsyncOperation asyncOp)
        {
            this.Project = project;
            this.CallJobs = callJobs;
            this.Exception = ex;
            this.AsyncOp = asyncOp;
        }

    }

}
