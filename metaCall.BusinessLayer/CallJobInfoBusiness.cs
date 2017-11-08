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
    public class CallJobInfoBusiness
    {
        private MetaCallBusiness metaCallBusiness;

        internal CallJobInfoBusiness(MetaCallBusiness metaCallBusiness)
        {
            this.metaCallBusiness = metaCallBusiness;

            //Delegates für Asynchrone MethodenAufrufe initialisieren
            InitializeDelegates();
        }

        /// <summary>
        /// Initialisiert die Delegates für Asynchrone MethodenAufrufe
        /// </summary>
        private void InitializeDelegates()
        {
            this.onGetCallJobsInfoExtendedProgressReportDelegate = new SendOrPostCallback(GetCallJobsInfoExtendedReportProgress);
            this.onGetCallJobsInfoExtendedCompletedDelegate = new SendOrPostCallback(GetCallJobsInfoExtendedCompletedMethod);
            this.getCallJobsInfoExtendedCompletionMethodDelegate = new SendOrPostCallback(GetCallJobsInfoExtendedCompletionMethod);
        }

        #region Asynchrones Abrufen von CallJobs
        public event GetCallJobInfoExtendedProgressChangedEventHandler GetCallJobsInfoExtendedProgressChanged;
        public event GetCallJobInfoExtendedCompletedEventHandler GetCallJobsInfoExtendedCompleted;

        private SendOrPostCallback onGetCallJobsInfoExtendedProgressReportDelegate;
        private SendOrPostCallback onGetCallJobsInfoExtendedCompletedDelegate;
        private SendOrPostCallback getCallJobsInfoExtendedCompletionMethodDelegate;

        private delegate void GetCallJobsInfoExtendedWorkerEventHandler(Project project, AsyncOperation asynOp, SendOrPostCallback completionMethodDelegate);

        private GetCallJobsInfoExtendedWorkerEventHandler getCallJobsInfoExtendedWorkerDelegate;

        private HybridDictionary userTokenToLifeTime = new HybridDictionary();

                
        public void GetCallJobsInfoExtendedByProjectAsync(Project project, object taskId)
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


            getCallJobsInfoExtendedWorkerDelegate = new GetCallJobsInfoExtendedWorkerEventHandler(
                this.GetCallJobsInfoExtendedWorker);
            getCallJobsInfoExtendedWorkerDelegate.BeginInvoke(
                project,
                asyncOp,
                getCallJobsInfoExtendedCompletionMethodDelegate,
                null,
                null);


        }

        public void CancelGetCallJobsInfoExtendedAsync(object taskId)
        {
            throw new NotImplementedException();
        }


        private void GetCallJobsInfoExtendedWorker(Project project, AsyncOperation asynOp, 
            SendOrPostCallback completionMethodDelegate)
        {
            List<CallJobInfoExtended> callJobsInfoExt = new List<CallJobInfoExtended>();
            Exception e = null;

            try
            {
                callJobsInfoExt.AddRange(
                    this.metaCallBusiness.ServiceAccess.GetListCallJobInfoExtendedByProject(
                        project, asynOp, this.onGetCallJobsInfoExtendedProgressReportDelegate));
            }
            catch (Exception ex)
            {
                e = ex;
            }

            GetCallJobsInfoExtendedState getCallJobsInfoExtendedState = new GetCallJobsInfoExtendedState(
                project,
                callJobsInfoExt,
                e,
                asynOp);

            //Aufruf des Delegates dass wir fertig sind!!!!
            completionMethodDelegate(getCallJobsInfoExtendedState);
        }

        private void GetCallJobsInfoExtendedCompletedMethod(object operationState)
        {
              
            GetCallJobsInfoExtendedCompletedEventArgs e = operationState as GetCallJobsInfoExtendedCompletedEventArgs;

            OnGetCallJobsInfoExtendedCompleted(e);

        }

        private void OnGetCallJobsInfoExtendedCompleted(GetCallJobsInfoExtendedCompletedEventArgs e)
        {
            if (GetCallJobsInfoExtendedCompleted != null)
                GetCallJobsInfoExtendedCompleted(this, e);
        }

        private void GetCallJobsInfoExtendedReportProgress(object operationState)
        {
            
            GetCallJobInfoExtendedProgressChangedEventArgs e = operationState as GetCallJobInfoExtendedProgressChangedEventArgs;

            OnGetCallJobsInfoExtendedProgressChanged(e);
        }

        private void OnGetCallJobsInfoExtendedProgressChanged(GetCallJobInfoExtendedProgressChangedEventArgs e)
        {
            if (GetCallJobsInfoExtendedProgressChanged != null)
                GetCallJobsInfoExtendedProgressChanged(e);
        }

        private void GetCallJobsInfoExtendedCompletionMethod(object operationState)
        {
            GetCallJobsInfoExtendedState getCallJobsInfoExtendedState = operationState as GetCallJobsInfoExtendedState;

            AsyncOperation asyncOp = getCallJobsInfoExtendedState.AsyncOp;

            GetCallJobsInfoExtendedCompletedEventArgs e = new GetCallJobsInfoExtendedCompletedEventArgs(
                getCallJobsInfoExtendedState.Project,
                getCallJobsInfoExtendedState.CallJobsInfoExt,
                getCallJobsInfoExtendedState.Exception,
                false,
                asyncOp.UserSuppliedState);


            lock (userTokenToLifeTime.SyncRoot)
            {
                userTokenToLifeTime.Remove(asyncOp.UserSuppliedState);
            }

            asyncOp.PostOperationCompleted(onGetCallJobsInfoExtendedCompletedDelegate, e);

        }



        #endregion

        public delegate void GetCallJobInfoExtendedProgressChangedEventHandler(GetCallJobInfoExtendedProgressChangedEventArgs e);
        public delegate void GetCallJobInfoExtendedCompletedEventHandler(object sender, GetCallJobsInfoExtendedCompletedEventArgs e);

        public class GetCallJobsInfoExtendedCompletedEventArgs : AsyncCompletedEventArgs
        {

            public GetCallJobsInfoExtendedCompletedEventArgs(Project project, List<CallJobInfoExtended> callJobsInfoExt, Exception e, bool canceled, object state)
                : base(e, canceled, state)
            {
                this.project = project;
                this.callJobsInfoExt = callJobsInfoExt;
            }


            private Project project;
            public Project Project
            {
                get { return project; }
            }

            private List<CallJobInfoExtended> callJobsInfoExt;
            public List<CallJobInfoExtended> CallJobsInfoExt
            {
                get { return callJobsInfoExt; }
            }
        }

        internal class GetCallJobsInfoExtendedState
        {
            public Project Project = null;
            public List<CallJobInfoExtended> CallJobsInfoExt = null;
            public Exception Exception = null;
            public AsyncOperation AsyncOp = null;

            public GetCallJobsInfoExtendedState(
                Project project,
                List<CallJobInfoExtended> callJobsInfoExt,
                Exception ex,
                AsyncOperation asyncOp)
            {
                this.Project = project;
                this.CallJobsInfoExt = callJobsInfoExt;
                this.Exception = ex;
                this.AsyncOp = asyncOp;
            }

        }
    }
}
