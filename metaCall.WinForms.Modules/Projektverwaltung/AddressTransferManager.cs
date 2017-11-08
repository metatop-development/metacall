using System;
using System.Collections.Generic;
using System.Text;

using metatop.Applications.metaCall.DataObjects;
using metatop.Applications.metaCall.BusinessLayer;

using System.ComponentModel;
using System.Threading;
using System.Collections.Specialized;

namespace metatop.Applications.metaCall.WinForms.Modules
{
    internal class AddressTransferManager  : Component
    {
        public event ProgressChangedEventHandler ProgressChanged;
        public event TransferCompletedEventHandler TransferCompleted;

        #region WaitCallBack-Delegates
        private SendOrPostCallback onProgressReportDelegate;
        private SendOrPostCallback onCompletedDelegate;
        private SendOrPostCallback completionMethodDelegate;

        private TransferWorkerEventHandler TransferWorkerDelegate;
        #endregion

        #region
        private HybridDictionary userStateToLifetime = new HybridDictionary();
        #endregion

        public bool IsRunning
        {
            get 
            {
                return (this.userStateToLifetime.Count > 0);
            }
        }
	
        public AddressTransferManager()
        {
            InitializeComponent();

            InitializeDelegates();
        }

        #region Erforderliche Member für Component
        public AddressTransferManager(IComponent container)
        {
            
            InitializeComponent();
            this.components.Add(container);

            InitializeDelegates();

        }

        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Komponenten-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }

        #endregion

        #endregion

        private void InitializeDelegates()
        {
            this.onProgressReportDelegate = new SendOrPostCallback(this.ReportProgress);
            this.onCompletedDelegate = new SendOrPostCallback(this.TransferAddressesCompleted);
            this.completionMethodDelegate = new SendOrPostCallback(this.CompletionMethod);
        }

        public void TransferAsync(Project project, DateTime startDate, DateTime stopDate)
        {
            if (project == null)
                throw new ArgumentNullException("calljobGroup");

            if (startDate == DateTime.MinValue)
                throw new ArgumentNullException("startDate");

            if (stopDate == DateTime.MinValue)
                throw new ArgumentNullException("stopDate");

            AsyncOperation asyncOp = AsyncOperationManager.CreateOperation(project.ProjectId);

            lock (this.userStateToLifetime.SyncRoot)
            {
                if (this.userStateToLifetime.Contains(project.ProjectId))
                {
                    throw new ArgumentException("The Project is already in work");
                }

                this.userStateToLifetime[project.ProjectId] = asyncOp;
            }

            this.TransferWorkerDelegate = new TransferWorkerEventHandler(this.TransferWorker);
            this.TransferWorkerDelegate.BeginInvoke(
                project, 
                startDate, 
                stopDate, 
                asyncOp, 
                this.completionMethodDelegate, 
                null, null);
        }

        public void Cancel(Project project)
        {
            lock (this.userStateToLifetime.SyncRoot)
            {
                object obj = this.userStateToLifetime[project.ProjectId];
                if (obj != null)
                {
                    AsyncOperation asyncOp = obj as AsyncOperation;

                    TransferCompletedEventArgs e = new TransferCompletedEventArgs(project, 0, null, true, asyncOp.UserSuppliedState);

                    asyncOp.PostOperationCompleted(this.onCompletedDelegate, e);
                }
            }
        }

        #region Ereignismethoden
        private void TransferAddressesCompleted(object userState)
        {
            TransferCompletedEventArgs e = userState as TransferCompletedEventArgs;

            OnTransferCompleted(e);
        }

        private void ReportProgress(object userState)
        {
            ProgressChangedEventArgs e = userState as ProgressChangedEventArgs;

            OnProgressChanged(e);
        }
        private void OnTransferCompleted(TransferCompletedEventArgs e)
        {
            if (TransferCompleted != null)
                TransferCompleted(this, e);
        }

        private void OnProgressChanged(ProgressChangedEventArgs e)
        {
            if (ProgressChanged != null)
                ProgressChanged(e);
        }
        #endregion

        private void TransferWorker(Project project, DateTime startDate, DateTime stopDate, AsyncOperation asyncOp, SendOrPostCallback completionMethodDelegate)
        {
            Exception exception = null;
            int addressesTransferred = 0;

            try
            {
                 addressesTransferred =  TransferAddresses(project, startDate, stopDate, asyncOp);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            TransferAddressesState transferState = new TransferAddressesState(project, startDate, stopDate,addressesTransferred, asyncOp, exception);
            completionMethodDelegate(transferState);
        }

        private int TransferAddresses(Project project, DateTime startDate, DateTime stopDate, AsyncOperation asyncOp)
        {
            List<Sponsor> sponsors;
            List<AddressReleaseInfo> releaseInfos = null;
            ProgressChangedEventArgs e = null;            
            
            //Retrieve Addresses
            e = new TransferAddressesProgressChangedEventArgs(project, TransferAdressesSteps.RetrieveAddresses, 0, asyncOp.UserSuppliedState);
            asyncOp.Post(this.onProgressReportDelegate, e);
            sponsors = MetaCall.Business.Addresses.GetNewSponsorsForTransfer(project);

            releaseInfos = new List<AddressReleaseInfo>(sponsors.Count);

            //Store Addresses 
            foreach (Sponsor sponsor in sponsors)
            {
                int percentage = (int) (((float)sponsors.IndexOf(sponsor) / (float)sponsors.Count) * 100f);
                e = new TransferAddressesProgressChangedEventArgs(project, TransferAdressesSteps.StoreAddresses, percentage, asyncOp.UserSuppliedState);
                asyncOp.Post(this.onProgressReportDelegate, e);

                MetaCall.Business.Addresses.CreateSponsor(sponsor, project);
            }

            e = new TransferAddressesProgressChangedEventArgs(project, TransferAdressesSteps.AnalyseCallJobGroups, 0, asyncOp.UserSuppliedState);
            asyncOp.Post(this.onProgressReportDelegate, e);

            //Analyse CallJobsGroups 
            CallJobGroupFactory fact2 = MetaCall.Business.CallJobGroups.GetCallJobGroupFactory();
            fact2.CallJobGroupCreated += new CallJobGroupCreatedEventHandler(fact2_CallJobGroupCreated);
            fact2.AnalyseSponsorProgressChanged += new AnalyseSponsorProgressChangedEventHandler(fact2_AnalyseSponsorProgressChanged);

            fact2.Analyze(project, sponsors.ToArray(), releaseInfos, startDate, stopDate);


            if (project.State != ProjectState.ReleasedForPhone)
                project.State = ProjectState.ReleasedForPhone;

            MetaCall.Business.Projects.Update(project);

            e = new TransferAddressesProgressChangedEventArgs(project, TransferAdressesSteps.CreateCallJobs, 0, asyncOp.UserSuppliedState);
            asyncOp.Post(this.onProgressReportDelegate, e);

            //CreateCallJobs
            if (releaseInfos.Count > 0)
            {
                MetaCall.Business.CallJobs.CreateByAddressReleaseInfos(releaseInfos);
            }

            return sponsors.Count;
        }

        void fact2_AnalyseSponsorProgressChanged(object sender, AnalyseSponsorProgressChangedEventArgs e)
        {
            AsyncOperation asyncOp = this.userStateToLifetime[e.Project.ProjectId] as AsyncOperation;

            AddressReleaseInfo releaseInfo = new AddressReleaseInfo();
            releaseInfo.Project = e.Project;
            releaseInfo.Sponsor = e.Sponsor;
            releaseInfo.StartDate = e.StartDate;
            releaseInfo.StopDate = e.StopDate;
            releaseInfo.PreferredTeam = null;
            releaseInfo.PreferredUser = null;
            releaseInfo.CallJobGroup = e.CallJobGroup;
            releaseInfo.DoRelease = true;

            e.ReleaseInfos.Add(releaseInfo);

            if (asyncOp != null)
            {
                int percentage = e.ProgressPercentage;
                TransferAddressesProgressChangedEventArgs arg = new TransferAddressesProgressChangedEventArgs(e.Project, TransferAdressesSteps.AnalyseCallJobGroups, percentage, asyncOp.UserSuppliedState);
                asyncOp.Post(this.onProgressReportDelegate, arg);
            }
        }

        void fact2_CallJobGroupCreated(object sender, CallJobGroupCreatedEventArgs e)
        {

            List<CallJobGroup> callJobGroups = new List<CallJobGroup>(e.Project.CallJobGroups);

            callJobGroups.Add(e.CallJobGroup);

            e.Project.CallJobGroups = callJobGroups.ToArray();
        }

        private void CompletionMethod(object transferState)
        {
            TransferAddressesState transferAddressesState = transferState as TransferAddressesState;

            AsyncOperation asyncOp = transferAddressesState.AsyncOp;

            Project project = transferAddressesState.Project;

            Exception exception = transferAddressesState.Ex;

            TransferCompletedEventArgs e =
                new TransferCompletedEventArgs(
                    project,
                    transferAddressesState.TotalAddressesTransfered, 
                    exception,
                    false,
                    asyncOp.UserSuppliedState);

            lock (userStateToLifetime.SyncRoot)
            {
                userStateToLifetime.Remove(asyncOp.UserSuppliedState);
            }

            asyncOp.PostOperationCompleted(onCompletedDelegate, e);
        }


        private delegate void TransferWorkerEventHandler(Project project, DateTime startDate, DateTime stopDate, AsyncOperation asyncOp, SendOrPostCallback completionMethodDelegate);

        public void UpdateCallJobGroups()
        {
          //  Project project = MetaCall.Business.Projects.Get(new Guid("C0C7E7BF-F614-4E51-89D0-D11F4B7C41A4"));
          //  project.Sponsors = MetaCall.Business.Addresses.GetSponsorsByProject(project).ToArray();
          //  UpdateCallJobGroups(project);

        }

        public void UpdateCallJobGroups(Project project)
        {
            CallJobGroupFactory factoryForUpdate = MetaCall.Business.CallJobGroups.GetCallJobGroupFactory();
            factoryForUpdate.CallJobGroupCreated += new CallJobGroupCreatedEventHandler(fact2_CallJobGroupCreated);
            factoryForUpdate.AnalyseSponsorProgressChanged += new AnalyseSponsorProgressChangedEventHandler(factoryForUpdate_AnalyseSponsorProgressChanged);
            
            factoryForUpdate.Analyze(project, project.Sponsors, new List<AddressReleaseInfo>(), DateTime.MinValue, DateTime.MinValue);

            OnTransferCompleted(new TransferCompletedEventArgs(project, project.Sponsors.Length,null,false, null));
        }

        void factoryForUpdate_AnalyseSponsorProgressChanged(object sender, AnalyseSponsorProgressChangedEventArgs e)
        {

            CallJob callJob = MetaCall.Business.CallJobs.Get(e.Sponsor.AddressId, e.Project.ProjectId);
            if (callJob == null)
            {
                //raus oder fehler oder neuen CallJob erstellen 
                   throw new NotImplementedException("muss noch programmiert werden");
            }
            callJob.CallJobGroup = MetaCall.Business.CallJobGroups.Get(e.CallJobGroup);

            CallJobGroup calljobGroup = MetaCall.Business.CallJobGroups.Get(callJob.CallJobGroup.CallJobGroupId);

            if (calljobGroup == null)
            {
                //Project speichern
                MetaCall.Business.Projects.Update(e.Project);
                //MetaCall.Business.CallJobGroups.Create(callJob.CallJobGroup);
            }

            MetaCall.Business.CallJobs.UpdateCallJob(callJob);

            OnProgressChanged(new TransferAddressesProgressChangedEventArgs(e.Project, TransferAdressesSteps.AnalyseCallJobGroups, e.ProgressPercentage, null));
        }
    }

    public delegate void ProgressChangedEventHandler(ProgressChangedEventArgs e);

    public delegate void TransferCompletedEventHandler(object sender, TransferCompletedEventArgs e);

    public class TransferCompletedEventArgs : AsyncCompletedEventArgs
    {
       
        public TransferCompletedEventArgs(
            Project project ,
            int totalAddressesTransfered,
            Exception error, 
            bool cancelled, 
            object userState)
            : base(error, cancelled, userState)
        {
            if (project == null)
                throw new ArgumentNullException("calljobGroup");

            this.project = project;
            this.totalAdressesTransfered = totalAddressesTransfered;
        }

        private Project project;
        public Project Project
        {
            get {

                //RaiseExceptionIfNecessary();

                return project; 
            }
        }

        private int totalAdressesTransfered;
        public int TotalAdressesTransfered
        {
            get {

                //RaiseExceptionIfNecessary();    

                return totalAdressesTransfered; 
            
            }
        }
    }

    internal class TransferAddressesState
    {
        public readonly Project Project = null;
        public readonly DateTime StartDate = DateTime.MinValue;
        public readonly DateTime StopDate = DateTime.MinValue;
        public readonly int TotalAddressesTransfered = 0;
        public readonly AsyncOperation AsyncOp = null;
        public readonly Exception Ex = null;

        public TransferAddressesState(
            Project project,
            DateTime startDate,
            DateTime stopDate,
            int totalAddressesTransfered,
            AsyncOperation asyncOp,
            Exception ex)
        {
            this.Project = project;
            this.StartDate = startDate;
            this.StopDate = stopDate;
            this.TotalAddressesTransfered = totalAddressesTransfered;
            this.AsyncOp = asyncOp;
            this.Ex = ex;

        }
    }

    public class TransferAddressesProgressChangedEventArgs : ProgressChangedEventArgs
    {
        public TransferAddressesProgressChangedEventArgs(Project project, TransferAdressesSteps step, int progressPercentage, object userToken)
            : base(progressPercentage, userToken)
        {
            this.project = project;
            this.step = step;
        }

        private Project project;
        public Project Project
        {
            get { return project; }
        }

        private TransferAdressesSteps step;
        public TransferAdressesSteps Step
        {
            get { return step; }
        }
    }

    public enum TransferAdressesSteps
    {
        RetrieveAddresses,
        StoreAddresses,
        AnalyseCallJobGroups,
        CreateCallJobs,
    }

}
