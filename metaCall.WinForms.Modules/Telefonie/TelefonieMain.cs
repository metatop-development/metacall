using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using MaDaNet.Common.AppFrameWork.ApplicationModul;
using metatop.Applications.metaCall.BusinessLayer;
using metatop.Applications.metaCall.DataObjects;

namespace metatop.Applications.metaCall.WinForms.Modules.Telefonie
{
    [DefaultModul(),
    ModulIndex(0),
    ToolboxItem(false)]
    public partial class TelefonieMain : UserControl, IModulMainControl
    {

        List<UserControl> userControls = new List<UserControl>();
        UserControl currentControl;
        bool phoneViewEventsRegistered = false;

        //private int abortDialingTime = 30;

        public TelefonieMain()
        {
            InitializeComponent();
        }

        #region IModulMainControl Member

        public event ModulInfoMessageHandler StatusMessage;

        private void OnStatusMessage(ModulInfoMessageEventArgs e)
        {

            if (StatusMessage != null)
                StatusMessage(this, e);

        }

        public event QueryPermissionHandler QueryPermisson;

        public event ModuleStateChangedHandler StateChanged;

        private void OnStateChanged(ModuleStateChangedEventArgs e)
        {

            if (StateChanged != null)
                StateChanged(this, e);
        }

        public void Initialize(IModulMainControl caller)
        {
            //Setting setting = MetaCall.Business.Settings.GetSetting();
            //abortDialingTime = setting.AbortDialingTime;

            ShowMainView();

            MainView mainView = currentControl as MainView;
            if (mainView != null)
            {
                mainView.ProjectChanged += new ProjectChangedEventHandler(mainView_ProjectChanged);
                mainView.NewCustomer += new NewCustomerEventHandler(mainView_NewCustomer);
                mainView.UserWantSpecialCall += new UserWantSpecialCallHandler(mainView_UserWantSpecialCall);
                MetaCall.Business.SponsoringCallManager.CallJobsChanged += new EventHandler(SponsoringCallManager_CallJobsChanged);
                MetaCall.Business.Dialer.Connected += new DialingEventHandler(Dialer_Connected);
                MetaCall.Business.Dialer.HangedUp += new DialingEventHandler(Dialer_HangedUp);
                MetaCall.Business.Dialer.WantConnect += new DialingEventHandler(Dialer_WantConnect);
            }

        }

        void Dialer_WantConnect(object sender, DialingEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new EventHandler<DialingEventArgs>(this.Dialer_WantConnect), new object[] { sender, e });
                return;
            } 
            StringBuilder sb = new StringBuilder();
            //sb.AppendFormat("Anwahlversuch mit der Nummer {0} (aut. Auflegen nach {1} sek)", e.PhoneNumber, abortDialingTime.ToString());
            sb.AppendFormat("Anwahlversuch mit der Nummer {0}", e.PhoneNumber);

            OnStatusMessage(new ModulInfoMessageEventArgs(sb.ToString()));

            //Symbolleiste aktualisieren
            toolStripButton1.Text = "Auflegen";
            toolStripButton1.Checked = true;
            toolStripButton1.Enabled = true;
        }

        void Dialer_HangedUp(object sender, DialingEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new EventHandler<DialingEventArgs>(this.Dialer_HangedUp), new object[] { sender, e });
                return;
            }
            OnStatusMessage(new ModulInfoMessageEventArgs("Verbindung beendet ..."));

            //Symbolleiste aktualisieren
            toolStripButton1.Text = "Wählen";
            toolStripButton1.Checked = false;
            toolStripButton1.Enabled = true;
        }

        void Dialer_Connected(object sender, DialingEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new EventHandler<DialingEventArgs>(this.Dialer_Connected), new object[] { sender, e });
                return;
            }

            OnStatusMessage(new ModulInfoMessageEventArgs("verbunden ..."));
            
            //Symbolleiste aktualisieren
            toolStripButton1.Text = "Auflegen";
            toolStripButton1.Checked = true;
            toolStripButton1.Enabled = true;
        }

        void mainView_UserWantSpecialCall(object sender, UserWantSpecialCallEventArgs e)
        {
            //Unterscheidung ob der Call ein normaler Call oder ein ReminderCall ist.
            if (e.Call.GetType() == typeof(ReminderCall))
            {
                ReminderCall reminderCall = (ReminderCall)e.Call;
                ShowPhoneView(reminderCall);
            }
            else
            {
                Call call = e.Call;
                ShowPhoneView(call);
            }

            return;            
        }

        private void ShowPhoneView(Call call)
        {
            ShowControl(typeof(PhoneView));

            PhoneView phoneView = currentControl as PhoneView;

            if (phoneView != null)
            {
                if (!phoneViewEventsRegistered)
                {
                    phoneView.Cancel += new PhoneViewEventHandler(phoneView_Cancel);
                    phoneView.Save += new PhoneViewEventHandler(phoneView_Save);

                    phoneViewEventsRegistered = true;
                }

                //PhoneView initialisieren
                phoneView.InitCall(call);

            }
        }

        private void ShowMainView()
        {
            //Wahlmöglichkeit ausschalten
            this.toolStripButton1.Enabled = false;

            ShowControl(typeof(MainView));

            MainView mainView = currentControl as MainView;

            if (mainView != null)
            {
                mainView.UpdateStatistics(MetaCall.Business.SponsoringCallManager);
            }

            

            OnStateChanged(new ModuleStateChangedEventArgs(ModulState.NotInWork));
        }
        
        void SponsoringCallManager_CallJobsChanged(object sender, EventArgs e)
        {
            if (InvokeRequired)
                this.Invoke(new MethodInvoker(this.UpdateCallJobCountInformation));
            else
                this.UpdateCallJobCountInformation();
        }

        void mainView_NewCustomer(object sender, NewCustomerEventArgs e)
        {
           
            OnStateChanged(new ModuleStateChangedEventArgs(ModulState.InWork));
            Call call;
            if (MetaCall.Business.CallJobs.DurringActiv == true)
            {
                //nächster Mahnungscalljobcall
                call = MetaCall.Business.SponsoringCallManager.GetNextDurringCall();
            }
            else
            {
                call = MetaCall.Business.SponsoringCallManager.GetNextSponsoringCall();
            }

            if (call == null)
            {
                if (MetaCall.Business.CallJobs.DurringActiv == true)
                {
                    MessageBox.Show("Sie haben alle Mahnungen dieser Mahnungsstufe telefoniert! Bitte wechseln Sie die Mahnstufe oder wechseln Sie zur Projektarbeit!!", "Keine Mahnungen mehr vorhanden!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("Sie haben alle Calls für dieses Projekt und diese Gruppe telefoniert. Bitte wählen Sie ein neues Projekt oder Gruppe!!", "Keine Calls mehr vorhanden", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                ShowMainView();
            }
            else
                ShowPhoneView(call);

            return;
        }

        void phoneView_Save(object sender, PhoneViewEventArgs e)
        {
            //TODO: COde für das speichern implementieren

            CallJobResultMessage message = e.CallJobResultMessage;
            MetaCall.Business.SponsoringCallManager.CallDone(message);

            //Neuen Kunden Abrufen
            mainView_NewCustomer(this, new NewCustomerEventArgs());



            //ShowMainView();

        }

        void phoneView_Cancel(object sender, PhoneViewEventArgs e)
        {
            //TODO: Code für das abbrechen implementieren

            ShowMainView();

        }

        void mainView_ProjectChanged(object sender, ProjectChangedEventArgs e)
        {

            if (e.Project != null)
            {
                MetaCall.Business.SponsoringCallManager.Start(MetaCall.Business.Users.CurrentUser, e.Project, e.CallJobGroup);
            }
            else
            {
                MetaCall.Business.SponsoringCallManager.Stop();
            }
            return;
        }

        private void ShowControl(Type controlType)
        {
            UserControl controlToShow = null;

            foreach (UserControl control in userControls)
            {
                if (control.GetType() == controlType)
                    controlToShow = control;
            }


            if (controlToShow == null)
            {
                controlToShow = (UserControl)Activator.CreateInstance(controlType);
                controlToShow.Parent = this;
                controlToShow.Dock = DockStyle.Fill;
                userControls.Add(controlToShow);
            }

            controlToShow.Visible = true;

            foreach (UserControl control in userControls)
            {
                if (control != controlToShow)
                    control.Visible = false;
            }
            controlToShow.Focus();

            currentControl = controlToShow;
        }

        public void UnloadModul(out bool canUnload)
        {
            
            //Wenn sich das Control momentan im Mahnmodus befindet wird dieser ausgeschaltet.
            if (MetaCall.Business.CallJobs.DurringActiv)
            {
                MetaCall.Business.SponsoringCallManager.StopDurring();
                MetaCall.Business.CallJobs.DurringActiv = false;
            }
            
            canUnload = true;
        }

        public ToolStrip CreateToolStrip()
        {
            return this.toolStrip1;
        }

        public ToolStripMenuItem[] CreateMainMenuItems()
        {
            return null;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812")]
        private class Configuration : ModulConfigBase
        {
            public override StartUpMenuItem GetStartUpMenuItem()
            {
                //return base.GetStartUpMenuItem();
                return new StartUpMenuItem("Telefonieren", "Telefoniebereich", Properties.Resources._6b);
            }

            public override bool HasStartupMenuItem
            {
                get { return true; }
            }

            public override bool HasMainMenuItems
            {
                get { return false; }
            }

            public override bool HasToolStrip
            {
                get { return true; }
            }
        }

        #endregion

        private void UpdateCallJobCountInformation()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("aktuelle CallJobs:{0}", MetaCall.Business.SponsoringCallManager.SponsoringCallsInBuffer);
            sb.Append(new string(' ', 10));
            sb.AppendFormat("Wiedervorlagen-CallJobs:{0}", MetaCall.Business.SponsoringCallManager.ReminderCallsInBuffer);

            OnStatusMessage(new ModulInfoMessageEventArgs(sb.ToString()));
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            ToolStripButton item = sender as ToolStripButton;
            if (item == null)
                return;

            if (item.CheckState == CheckState.Unchecked)
            {
                if (MetaCall.Business.Dialer.State != DialStates.Ready)
                    MetaCall.Business.Dialer.HangUp();
            }
            else
            {
                if (item.CheckState == CheckState.Checked)
                {
                    if (MetaCall.Business.Dialer.State == DialStates.Ready)
                    {
                        PhoneView phoneView = currentControl as PhoneView;
                        if (phoneView != null)
                        {
                            string phoneNumber = phoneView.UsedPhoneNumber;
                            if (!string.IsNullOrEmpty(phoneNumber))
                            {
                                phoneView.Dial();
                            }
                        }
                    }
                }
            }

        }


        #region IModulMainControl Member


        public bool CanPauseApplication
        {
            get {
                return (MetaCall.Business.Dialer.State == DialStates.Ready);
                }
        }

        #endregion

        #region IModulMainControl Member


        public bool CanAccessModul(System.Security.Principal.IPrincipal principal)
        {
            return true;
        }

        #endregion
    }

    public delegate void ProjectChangedEventHandler(object sender, ProjectChangedEventArgs e);
    public delegate void NewCustomerEventHandler(object sender, NewCustomerEventArgs e);
    public delegate void UserWantSpecialCallHandler(object sender, UserWantSpecialCallEventArgs e);
    public delegate void DurringChangedEventHandler(object sender, DurringChangedEventArgs e);

    public class DurringChangedEventArgs : EventArgs
    {
        private bool durringActiv;

        public DurringChangedEventArgs(bool durringActiv)
        {
            this.durringActiv = durringActiv;
        }

        public bool DurringAcitv
        {
            get { return this.durringActiv; }
        }
    }

    public class ProjectChangedEventArgs : EventArgs
    {
        private ProjectInfo project;
        private CallJobGroupInfo callJobGroup;

        public ProjectChangedEventArgs(ProjectInfo project, CallJobGroupInfo callJobGroup)

        {
            this.project = project;
            this.callJobGroup = callJobGroup;
        }

        public CallJobGroupInfo CallJobGroup
        {
            get { return callJobGroup; }
        }

        public ProjectInfo Project
        {
            get { return project; }
        }
    }

    public class NewCustomerEventArgs : EventArgs
    {
        public NewCustomerEventArgs()
        {
            this.project = null;
        }
        
        public NewCustomerEventArgs(ProjectInfo project)
        {
            this.project = project;
            this.callJobGroup = null;
        }

        public NewCustomerEventArgs(ProjectInfo project, CallJobGroupInfo callJobGroup):this(project)
        {
            this.callJobGroup = callJobGroup;
        }

        private ProjectInfo project;
        public ProjectInfo Project
        {
            get { return project; }
        }

        private CallJobGroupInfo callJobGroup;
        public CallJobGroupInfo CallJobGroup
        {
            get { return callJobGroup; }
        }
	
    }

    public class UserWantSpecialCallEventArgs : EventArgs
    {
        public UserWantSpecialCallEventArgs(Call call)
        {
            this.call = call;
        }
        
        private Call call;
        public Call Call
        {
            get { return call; }
        }
    }
}
