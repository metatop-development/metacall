using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

using metatop.Applications.metaCall.DataObjects;


using System.Data;
using System.Data.Common;
using System.Threading;
using System.Collections.Specialized;


namespace metatop.Applications.metaCall.BusinessLayer
{
    [ToolboxItem(false)]
    public class TeamProjectAssign : Component
    {
        public event AssignedProjectsEventHandler AssignProjectCompleted;
        public event ProgressChangedEventHandler ProgressChanged;

        private HybridDictionary userStateToLifetime = new HybridDictionary();

        private MetaCallBusiness metaCallBusiness;

        private SendOrPostCallback onProgressReportDelegate;
        private SendOrPostCallback onCompletedDelegate;
        private SendOrPostCallback completionMethodDelegate;

        private IContainer components = null;

        public TeamProjectAssign(MetaCallBusiness metaCallBusiness)
        {
            this.metaCallBusiness = metaCallBusiness;

            InitializeComponent();

            InitializeDelegates();
        }

        public TeamProjectAssign(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

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

        public void AssignProjectAsync(Team team, Project project, object taskId)
        {
            if (team == null)
                throw new ArgumentNullException("team");

            if (project == null)
                throw new ArgumentNullException("project");


            AsyncOperation asyncOp = AsyncOperationManager.CreateOperation(taskId);


            lock (userStateToLifetime.SyncRoot)
            {
                if (userStateToLifetime.Contains(taskId))
                {
                    throw new ArgumentException("Task ID Parameter must be unique");
                }

                userStateToLifetime[taskId] = asyncOp;
            }


            workerDelegate = new WorkerEventHandler(AssignProjectWorker);
            workerDelegate.BeginInvoke(team, project, asyncOp, completionMethodDelegate, null, null);
        }
        public void AssignProjectAsyncCancel(object taskId)
        {
            lock (userStateToLifetime.SyncRoot)
            {
                object obj = userStateToLifetime[taskId];
                if (obj != null)
                {
                    AsyncOperation asyncOp = obj as AsyncOperation;

                    AssignProjectCompletedEventArgs e = new AssignProjectCompletedEventArgs(
                        null, true, null, asyncOp.UserSuppliedState);


                    asyncOp.PostOperationCompleted(onCompletedDelegate, e);
                }
            }
        }

        private void AssignProject(Team team, Project project, AsyncOperation asyncOp)
        {
            //return;

            ///* Einstellen des neuen Projektes */
            //AssignedProject assignedProject = new AssignedProject();
            //assignedProject.ProjectId = project.ProjectId;
            //assignedProject.mwProjektNummer = project.mwProject.Projektnummer;
            //assignedProject.Bezeichnung = project.Bezeichnung;
            //assignedProject.AssignId = Guid.NewGuid();
            //assignedProject.AssignDate = DateTime.Now;
            //assignedProject.AssignUserId = metaCallBusiness.Users.CurrentUser.UserId;

            //List<AssignedProject> projects;
            //if (team.Projects == null)
            //    projects = new List<AssignedProject>();
            //else
            //    projects = new List<AssignedProject>(team.Projects);

            //projects.Add(assignedProject);

            //team.Projects = projects.ToArray();

            ////TODO: Projektstatus muss aktualisiert werden
            //// Aufnehmen in ProjektInfo-Klasse

            //metaCallBusiness.Teams.Update(team);


            //Abrufen aller bestehenden Sponsoren
            project.Sponsors = metaCallBusiness.ServiceAccess.GetSponsorsByProject(project);

            //transferieren der anderen Sponsoren
            metaCallBusiness.ServiceAccess.TransferAddressPool(project);

            return;
        }

        private void AssignProjectWorker(Team team, Project project, AsyncOperation asyncOp, SendOrPostCallback completionMethodDelegate)
        {
            Exception e = null;
            
            try
            {
                this.AssignProject(team, project, asyncOp);
            }
            catch (Exception ex)
            {
                e = ex;
            }


            AssignProjectState state = new AssignProjectState(team, project, asyncOp, e);

            completionMethodDelegate(state);


        }
        
        /// <summary>
        /// Übernimmt den Adressenpool eines Projekts nach metaCall
        /// </summary>
        private void TransferAddressPool(Project project, DbTransaction transaction, AsyncOperation asyncOp)
        {

            //kopiert die Addressen aus metaware und 
            // fügt sie dem Projekt als Sponsoren hinzu
            metaCallBusiness.ServiceAccess.TransferAddressPool(project);

            return ;
        }

        protected virtual void InitializeDelegates()
        {
            this.onProgressReportDelegate += new SendOrPostCallback(ReportProgress);
            this.onCompletedDelegate += new SendOrPostCallback(AssignCompleted);
            this.completionMethodDelegate += new SendOrPostCallback(CompletionMethod);
        }

        protected void OnAssignProjectCompleted(AssignProjectCompletedEventArgs assignProjectCompletedEventArgs)
        {
            if (AssignProjectCompleted != null)
                AssignProjectCompleted(this, assignProjectCompletedEventArgs);
        }
        protected void OnProgressChanged(ProgressChangedEventArgs e)
        {
            if (ProgressChanged != null)
                ProgressChanged(this, e);
            
        }

        private delegate void WorkerEventHandler(Team team, Project project, AsyncOperation asyncOp, SendOrPostCallback completionMethodDelegate);
        private WorkerEventHandler workerDelegate;

        private void CompletionMethod(object assignProjectState)
        {
            AssignProjectState assignState = assignProjectState as AssignProjectState;
            AsyncOperation asyncOp = assignState.asyncOp;


            AssignProjectCompletedEventArgs e = new AssignProjectCompletedEventArgs(assignState.ex, false,  assignState.Project, asyncOp.UserSuppliedState);


            lock (this.userStateToLifetime.SyncRoot)
            {
                userStateToLifetime.Remove(asyncOp.UserSuppliedState);
            }

            asyncOp.PostOperationCompleted(onCompletedDelegate, e);
            
        }

        private void AssignCompleted(object operationState)
        {
            AssignProjectCompletedEventArgs e = operationState as AssignProjectCompletedEventArgs;

            OnAssignProjectCompleted(e);
        }
        private void ReportProgress(object state)
        {
            ProgressChangedEventArgs e = state as ProgressChangedEventArgs;


            lock (userStateToLifetime.SyncRoot)
            {

                OnProgressChanged(e);
            }
        }
    
    }

    public delegate void AssignedProjectsEventHandler (object sender, AssignProjectCompletedEventArgs e);
    public delegate void ProgressChangedEventHandler(object sender, ProgressChangedEventArgs e);

    public class AssignProjectCompletedEventArgs : AsyncCompletedEventArgs
    {
        Project project;

        public AssignProjectCompletedEventArgs(Exception error, bool cancelled, Project project, object obj):base(error, cancelled, obj)
        {
            this.project = project;
        }

        public Project Project
        {
            get { return this.project; }
        }

        
    }

    internal class AssignProjectState
    {
        public readonly Team Team;
        public readonly Project Project;
        public readonly AsyncOperation asyncOp;
        public readonly Exception ex;


        public AssignProjectState(
            Team team,
            Project project,
            AsyncOperation asyncOp,
            Exception ex)
        {
            this.Team = team;
            this.Project = project;
            this.asyncOp = asyncOp;
            this.ex = ex;
        }

    }

}
