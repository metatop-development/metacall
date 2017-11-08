using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using MaDaNet.Common.AppFrameWork.WinUI.Controls;
using MaDaNet.Common.AppFrameWork.WinUI.Controls.OutlookBar;
using MaDaNet.Common.AppFrameWork.ApplicationModul;

using metatop.Applications.metaCall.BusinessLayer;
using metatop.Applications.metaCall.DataObjects;
using System.Globalization;
using MaDaNet.Common.AppFrameWork.Activities;

namespace metatop.Applications.metaCall.WinForms.App
{
    public partial class MainForm : Form
    {
        #region ClassData

        //DataLayer.DataAccessLayer dataLayer;
        OutlookBar outlookbar1;

        System.Collections.Generic.Dictionary<object, ModulConfigBase> actionItems = new Dictionary<object, ModulConfigBase>();

        ModuleInfo currentModul;
        Pause pauseForm;
        // pauseAndUpdated wird verwendet, damit Application Idle nicht ständig
        // aktualisert, weil während der Pause nichts passiert und Application_Idle ständig 
        // aufgerufen wird
        private bool pauseAndUpdated = false;

        private bool closedByAutoShutDown = false;

        Training trainingForm;

        int activitiesTimer;
        int activitiesMax;

        messageFilter mouseMoveFilter = null;
        
        private ModulState moduleState = ModulState.Unavailable;

        #endregion

        #region Constructor

        public MainForm()
        {
            InitializeComponent();
                  
            CreateOutlookBar();

            this.MainMenu.ItemAdded += new ToolStripItemEventHandler(MainMenu_ItemAdded);
            this.Text = Application.ProductName;
            Application.Idle += new EventHandler(Application_Idle);

            //TODO: Benutzeran-/abmeldung abfragen           
            MetaCall.Business.PauseChanged += new EventHandler(Business_PauseChanged);
            MetaCall.Business.TrainingChanged += new EventHandler(Business_TrainingChanged);
            MetaCall.Business.ProjectChanged += new ProjectChangedEventHandler(Business_ProjectChanged);
            MetaCall.Business.LoggedOff += new EventHandler(Business_LogOff);

            this.timerActivities.Interval = 1000;
            this.timerActivities.Enabled = true;

            Setting setting = MetaCall.Business.Settings.GetSetting();
            
            this.activitiesMax = setting.ActivitiesMaxSec;
            this.activitiesTimer = 0;

            this.mouseMoveFilter = new messageFilter();
            this.mouseMoveFilter.GlobalMouseMove += new EventHandler(mouseMoveFilter_GlobalMouseMove);


            Application.AddMessageFilter(mouseMoveFilter);

        }

        void mouseMoveFilter_GlobalMouseMove(object sender, EventArgs e)
        {
            this.activitiesTimer = 0;
        }


        private class messageFilter : IMessageFilter
        {
            
            #region IMessageFilter Member

            public bool PreFilterMessage(ref Message m)
            {

                if (m.Msg == (int)WinUser.WM_LBUTTONDOWN)
                //if (m.Msg == (int)WinUser.WM_MOUSEMOVE)
                {
                    OnGlobalMouseMove();
                }
                //throw new Exception("The method or operation is not implemented.");
                return false;
            }

            #endregion

            public event EventHandler GlobalMouseMove;

            private void OnGlobalMouseMove()
            {
                if (GlobalMouseMove != null)
                    GlobalMouseMove(this, EventArgs.Empty);
            }

        }


        void Business_LogOff(object sender, EventArgs e)
        {
            if (this.currentModul != null)
            {
                UnloadModul(this.currentModul);
            }
        }

        void Business_ProjectChanged(object sender, ProjectChangedEventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Application.ProductName);
            //sb.AppendFormat(" ({0})", Application.ProductVersion);

            if (e.Project != null)
                sb.AppendFormat(" - {0}", e.Project.Bezeichnung);

            this.Text = sb.ToString();
        }

        void Business_TrainingChanged(object sender, EventArgs e)
        {
            toolStripButtonTraining.Checked = MetaCall.Business.Training;

            if (MetaCall.Business.Training)
            {
                Screen screen = Screen.FromHandle(this.Handle);

                //Anzeigen des Training-Formulars
                trainingForm = new Training();
                trainingForm.StartPosition = FormStartPosition.Manual;

                if (screen != null)
                {
                    trainingForm.Location = new Point(
                        (screen.WorkingArea.Width / 2) - (trainingForm.Width / 2),
                        (screen.WorkingArea.Height / 2) - (trainingForm.Height / 2)
                        );
                }
                trainingForm.Show(this);

            }
            else
            {
                //Ausschalten des PauseFormulars
                if (trainingForm != null)
                    trainingForm.Close();
            }
        }     

        void Business_PauseChanged(object sender, EventArgs e)
        {
            toolStripButtonPause.Checked = MetaCall.Business.Pause;

            if (MetaCall.Business.Pause)
            {
                Screen screen = Screen.FromHandle(this.Handle);
                
                //Anzeigen des Pause-Formulars
                pauseForm = new Pause();
                pauseForm.StartPosition = FormStartPosition.Manual;

                if (screen != null)
                {
                    pauseForm.Location = new Point(
                        (screen.WorkingArea.Width / 2) - (pauseForm.Width / 2),
                        (screen.WorkingArea.Height / 2) - (pauseForm.Height / 2)
                        );
                }
                pauseForm.Show(this);

            }
            else
            {
                //Ausschalten des PauseFormulars
                if (pauseForm != null)
                    pauseForm.Close();
            }
        }

        void Application_Idle(object sender, EventArgs e)
        {
            //Wenn die Anwendung nichts zu tun hat, kann Sie die 
            // Benutzeröberfläche aktualisieren
            if (!MetaCall.Business.Pause)
            {
                pauseAndUpdated = false;
            }
            if (!pauseAndUpdated)
            {
                UpdateUI();
            }
        }
        #endregion


        public override bool PreProcessMessage(ref Message msg)
        {
            return base.PreProcessMessage(ref msg);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == (int)metaCall.WinForms.WinUser.WM_MOUSEMOVE)
            {
                this.activitiesTimer = 0;
            }
            base.WndProc(ref m);
        }

        /// <summary>
        /// Erstellt die Outlookbar für die Navigation auf der linken Seite
        /// </summary>
        private void CreateOutlookBar()
        {
            this.outlookbar1 = new OutlookBar();

            this.splitContainer1.SuspendLayout();
            this.outlookbar1.SuspendLayout();
            this.outlookbar1.BeginInit();

            this.splitContainer1.Panel1.Controls.Add(this.outlookbar1);
            //TODO: set some Styles
            this.outlookbar1.Dock = DockStyle.Fill;

            foreach (ModulConfigBase modul in ModulManager.Modules)
            {
                if (modul.HasStartupMenuItem)
                {

                    StartUpMenuItem startUpMenuItem = modul.GetStartUpMenuItem();

                    if (startUpMenuItem != null)
                    {

                        string category = startUpMenuItem.Category;
                        //find an existing Band with same category or create a new one
                        Band band = GetBand(category);

                        if (band == null) break;

                        //find an existing BandItem in the Band or create a new one
                        BandItem bandItem = GetBandItem(band, startUpMenuItem);

                        if (bandItem == null) break;


                        //Adding the BandItem / ModulConfigBase to The ActionItemsList
                        if (!this.actionItems.ContainsKey(bandItem))
                        {
                            this.actionItems.Add(bandItem, modul);
                        }
                    }
                }


            }

            this.outlookbar1.EndInit();
            this.splitContainer1.ResumeLayout();
            this.outlookbar1.ResumeLayout();

            // TODO: das Default-Modul initialisieren

        }

        /// <summary>
        /// Sucht nach einem Band für die Kategorie oder erstellte ein neues Band
        /// </summary>
        /// <param name="category"></param>
        /// <returns>liefert das gesuchte Band oder ein neues Band mit der übergebenen Kategorie</returns>
        private Band GetBand(string category)
        {
            // Durchhläuft alle bestehenden Bands und gibt das gefunden zurück
            foreach (Band band in outlookbar1.Bands)
            {
                if (band.Text == category) return band;
            }

            // wurde kein Band gefunden wird ein neues erstellt
            Band newBand = new Band();
            newBand.BeginInit();
            //newBand.BackColor = Color.SkyBlue;
            newBand.Text = category;
            newBand.EndInit();
            outlookbar1.Bands.Add(newBand);

            return newBand;
        }

        /// <summary>
        /// Sucht nach einem BandItem für das StartUpMenuItem oder erstellt ein neues BandItem
        /// </summary>
        /// <param name="band"></param>
        /// <param name="item"></param>
        /// <returns>liefert das gesuchte BandItem oder ein neues BandItem für das StartUpMenuItem</returns>
        private BandItem GetBandItem(Band band, StartUpMenuItem item)
        {
            //Durchläuft alle bestehenden BandItems und gibt das gefundene zurück
            foreach (BandItem bandItem in band.Items)
            {
                if (bandItem != null)
                    if (bandItem.Text == item.Text) return bandItem;
            }

            BandItem newItem = new BandItem(item.Text);
            if (item.Icon != null)
                newItem.Icon = item.Icon;

            //TODO: ToolTips für BandItems implementieren
            //if (item.ToolTip != null)
            //    newItem.ToolTip = item.ToolTip;

            //register an Eventhandler for the Click-Event
            newItem.Click += new EventHandler(this.BandItem_Click);

            band.Items.Add(newItem);

            return newItem;

        }

        /// <summary>
        /// Eventhandler für das BandItem.Click-Ereignis
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BandItem_Click(object sender, EventArgs e)
        {

            // in den ActionItems nach dem sender suchen
            if (this.actionItems.ContainsKey(sender))
            {
                ModulConfigBase modulConfiguration;

                if (this.actionItems.TryGetValue(sender, out modulConfiguration))
                {

                    ModuleInfo modul = new ModuleInfo();
                    modul.Configuration = modulConfiguration;

                    //Wenn das gewünschte Modul bereits gelagen ist wird nichts gemacht
                    if ((this.currentModul != null) &&
                        (this.currentModul.Configuration.Name == modul.Configuration.Name))
                    {
                                  if (splitContainer1.Panel2.Contains((Control) this.currentModul.Control))
                        return;
                    }

                    //Sichern des Controls für die Übergabe
                    IModulMainControl oldModuleControl = null;

                    if (this.currentModul != null)
                    {
                        oldModuleControl = this.currentModul.Control;
                        #region Aktuelles Modul entladen
                        UnloadModul(this.currentModul);
                        #endregion
                    }
                    #region neues Modul instanzieren und laden
                    LoadModul(modul, oldModuleControl);
                    #endregion
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Sender konnte nicht in den ActionItems gefunden werden");
            }
        }

        /// <summary>
        /// Lädt ein Anwendungsmodul
        /// </summary>
        /// <param name="modul"></param>
        /// <param name="mainControl"></param>
        private void LoadModul(ModuleInfo modul, IModulMainControl mainControl)
        {
            IModulMainControl mctl = modul.Control;

            if (!mctl.CanAccessModul(System.Threading.Thread.CurrentPrincipal))
            {
                MessageBox.Show("Sie haben für das gewählte Modul keine Berechtigung!");
                return;
            }
                

            mctl.Initialize(mainControl);
            mctl.StatusMessage += new ModulInfoMessageHandler(mctl_StatusMessage);
            mctl.StateChanged += new ModuleStateChangedHandler(mctl_StateChanged);
            mctl.QueryPermisson += new QueryPermissionHandler(mctl_QueryPermisson);


            ToolStripMenuItem[] menu = modul.MainMenuItems;
            if ((menu != null) &&
                (menu.Length > 0))
            {
                this.MainMenu.Items.AddRange(menu);

            }
            ToolStrip toolstrip = modul.Toolbar;
            if ((toolstrip != null) &&
                (toolstrip.Items.Count > 0))
            {
                ToolStripPanel panel = this.toolStripContainer1.TopToolStripPanel;

                toolstrip.LayoutStyle = ToolStripLayoutStyle.Flow;
                toolstrip.AutoSize = true;
                toolstrip.MinimumSize = new Size(toolstrip.Width, this.toolStrip1.Height);

                panel.SuspendLayout();
                //HACK: Damit dei Toolstrips in der richtigen Reihenfolge 
                // erscheinen müssen zuerst alle gelöscht und dann in umgekehrter Reihenfolge
                // hinzugefügt werden.
                panel.Controls.Clear();
                panel.Controls.Add(toolstrip);
                panel.Controls.Add(this.toolStrip1);

                panel.ResumeLayout(true);
            }

            Control ctl = mctl as Control;
            ctl.Dock = DockStyle.Fill;

            splitContainer1.Panel2.Controls.Clear();
            splitContainer1.Panel2.Controls.Add(ctl);

            this.currentModul = modul;
            this.PerformLayout();
        }

        /// <summary>
        /// Entlädt ein Anwendungsmodul
        /// </summary>
        /// <param name="mainControl"></param>
        /// <returns></returns>
        private void UnloadModul(ModuleInfo modulInfo)
        {
            
            if (this.splitContainer1.Panel2.Controls.Count > 0)
                this.splitContainer1.Panel2.Controls.RemoveAt(0);

            if (modulInfo != null)
            {
                IModulMainControl mainControl = modulInfo.Control;
                if (mainControl != null)
                {
                    bool canUnload;
                    mainControl.UnloadModul(out canUnload);
                    if (!canUnload) return;
                }

                //Löschen der HaupmenüEinträge und Symbollseiten
                if (modulInfo.Configuration.HasMainMenuItems)
                {
                    foreach (ToolStripMenuItem item in modulInfo.MainMenuItems)
                        this.MainMenu.Items.Remove(item);
                }

                if (modulInfo.Configuration.HasToolStrip)
                {
                    this.toolStripContainer1.TopToolStripPanel.Controls.Remove(modulInfo.Toolbar);
                }
                modulInfo.Control = null;

            }
            //return mainControl;
        }

        void mctl_QueryPermisson(object sender, QueryPermissionEventArgs e)
        {
            MessageBox.Show("Query Permission raised", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            throw new NotImplementedException();
        }

        void mctl_StateChanged(object sender, ModuleStateChangedEventArgs e)
        {
            this.moduleState = e.State;
            
            UpdateUI();
        }

        void mctl_StatusMessage(object sender, ModulInfoMessageEventArgs e)
        {
            this.toolStripStatusLabel1.Text = e.Message;
        }

        //aktualisiert das UserInterface
        private void UpdateUI()
        {


            //MenüItem AnAbmelden aktualisieren
            if (MetaCall.Business.Users.IsLoggedOn)
            {
                if (MetaCall.Business.Pause || MetaCall.Business.Training)
                {
                    pauseAndUpdated = true;
                    this.outlookbar1.Enabled = false;
                    this.mnuAnmeldung.Enabled = false;
                    this.changePasswordToolStripMenuItem.Enabled = false;
                    this.mnuQuit.Enabled = false;
                    this.toolStripButtonPause.Enabled = false;
                    this.toolStripButtonTraining.Enabled = false;
                }
                else
                {
                    if (moduleState == ModulState.InWork)
                    {
                        this.outlookbar1.Enabled = false;
                        this.mnuAnmeldung.Enabled = false;
                        this.changePasswordToolStripMenuItem.Enabled = false;
                        this.mnuQuit.Enabled = false;
                        if (currentModul != null)
                        {
                            this.toolStripButtonPause.Enabled = currentModul.Control.CanPauseApplication;
                            this.toolStripButtonTraining.Enabled = currentModul.Control.CanPauseApplication;
                        }
                        else
                        {
                            this.toolStripButtonPause.Enabled = false;
                            this.toolStripButtonTraining.Enabled = false;
                        }

                    }
                    else
                    {
                        this.outlookbar1.Enabled = true;
                        this.mnuAnmeldung.Enabled = true;
                        this.mnuQuit.Enabled = true;
                        this.changePasswordToolStripMenuItem.Enabled = true;
                        this.toolStripButtonPause.Enabled = true;
                        this.toolStripButtonTraining.Enabled = true;
                    }
                }
                                
                
                mnuAnmeldung.Text = "&Abmelden";
            }
            else
            {
                mnuAnmeldung.Text = "&Anmelden";
                this.toolStripButtonPause.Enabled = false;
                this.toolStripButtonTraining.Enabled = false;
                this.changePasswordToolStripMenuItem.Enabled = false;
            }

            splitContainer1.Enabled = (MetaCall.Business.Users.IsLoggedOn  && ! MetaCall.Business.Pause);

        }

        #region Form Methods

        protected override void OnLoad(EventArgs e)
        {
            //Activate the current Form because before the Splashscreen 
            // was closing
            this.Activate();
            base.OnLoad(e);

            UpdateUI();

            LogOn();

        }

        #endregion

        #region Login/Out Methods
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1300")]
        private void LogOn()
        {
            //MessageBox.Show("Dies ist eine Testversion. Bitte verwenden Sie diese Version nur nach Absprache mit der Centerleitung!",
            //    "Test-Version", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            LogOnServices.LogOnForm logOnForm = new metatop.Applications.metaCall.WinForms.App.LogOnServices.LogOnForm(MetaCall.Business.Users.LogOn);

            DialogResult result = DialogResult.Cancel;

            while ((!MetaCall.Business.Users.IsLoggedOn)
                && (logOnForm.LogOnCounter < LogOnServices.LogOnForm.MaxLogOnTrys))
            {
                result = logOnForm.ShowDialog();
                logOnForm.LogOnCounter++;
                if (result != DialogResult.OK) break;
            }

            if (result == DialogResult.OK)
            {
                if (!MetaCall.Business.Users.IsLoggedOn)
                {
                    MessageBox.Show("Ihre Anmeldung ist fehlgeschlagen!", null, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
            else
            {
                //Nur Meldung anzeigen, wenn der Benutzer auch angemeldet sein wollte
                //MessageBox.Show("LogIn failed", null, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }

            UpdateUI();
        }
        private void LogOut()
        {
            MetaCall.Business.Users.LogOff();
        }
        #endregion

        #region Menuhandlers
        private void MainMenu_ItemAdded(object sender, ToolStripItemEventArgs e)
        {
            //throw new Exception("The method or operation is not implemented.");
            // TODO: Prüfen ob das neu hinzugefügte Item die Userberechtigung besitzt
        }

        private void mnuAnmeldung_Click(object sender, EventArgs e)
        {
            if (MetaCall.Business.Users.IsLoggedOn)
            {
                LogOut();
                ActivitiesTimerStart = false;
            }
            else
            {
                LogOn();
                ActivitiesTimerStart = true;
            }

        }
        private void mnuQuit_Click(object sender, EventArgs e)
        {

                this.Close();
        }
        #endregion

        private class ModuleInfo
        {
            public ModuleInfo() { }

            ModulConfigBase configuration;

            public ModulConfigBase Configuration
            {
                get { return configuration; }
                set
                {
                    configuration = value;
                    this.InitModulObjects();
                }
            }

            private void InitModulObjects()
            {
                this.control = configuration.CreateMainControl();

                if (configuration.HasMainMenuItems)
                    this.mainMenuItems = this.control.CreateMainMenuItems();

                if (configuration.HasToolStrip)
                    this.toolbar = this.control.CreateToolStrip();
            }

            IModulMainControl control;

            public IModulMainControl Control
            {
                get {
                    if (this.control == null)
                        this.control = configuration.CreateMainControl();

                    return control; 
                }
                set { control = value; }
            }
            ToolStripMenuItem[] mainMenuItems;

            public ToolStripMenuItem[] MainMenuItems
            {
                get { return mainMenuItems; }
                set { mainMenuItems = value; }
            }
            ToolStrip toolbar;

            public ToolStrip Toolbar
            {
                get { return toolbar; }
                set { toolbar = value; }
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {

            if (MetaCall.Business.Users.IsLoggedOn)
                MetaCall.Business.Users.LogOff();

            if (this.mouseMoveFilter != null)
                Application.RemoveMessageFilter(this.mouseMoveFilter);

            MetaCall.Business.Dialer.ShutDown();
        }

        private void toolStripButtonPause_Click(object sender, EventArgs e)
        {
            //Pause aktivieren
            MetaCall.Business.Pause = true;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            string msg = Properties.Resources.WantReallyQuitQuestion;

            MessageBoxOptions options =
                CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft ?
                MessageBoxOptions.RtlReading :
                0;

            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (MetaCall.Business.Pause)
                {
                    e.Cancel = true;
                }
                else
                {
                    if (!closedByAutoShutDown)
                    {
                        DialogResult result = MessageBox.Show(this, msg,
                            Application.ProductName, MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question, MessageBoxDefaultButton.Button1,
                            options);

                        if (result == DialogResult.No)
                            e.Cancel = true;
                    }
                }

            }
            
            /* else
            {
                DialogResult result = MessageBox.Show(this, msg,
                    Application.ProductName, MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question, MessageBoxDefaultButton.Button1,
                    options);

                if (result == DialogResult.No)
                    e.Cancel = true;
            }
             */
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (AboutBox aboutBox = new AboutBox())
            {
                aboutBox.ShowDialog();
            }
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (LogOnServices.ChangePassWord changePasswordDlg = new metatop.Applications.metaCall.WinForms.App.LogOnServices.ChangePassWord(MetaCall.Business.Users.ChangePassword))
            {
                changePasswordDlg.ShowDialog(this);
            }
        }

        private void timerActivities_Tick(object sender, EventArgs e)
        {
            // Herunterzählen auch beim wählen aktivieren
            //if (MetaCall.Business.Pause != true && MetaCall.Business.Dialer.State == DialStates.Ready)
            if (MetaCall.Business.Pause != true && MetaCall.Business.Training != true)
                {
                if (MetaCall.Business.Users.CurrentUser == null || System.Threading.Thread.CurrentPrincipal.IsInRole(MetaCallPrincipal.AdminRoleName) || System.Threading.Thread.CurrentPrincipal.IsInRole(MetaCallPrincipal.CenterAdminRoleName))
                {
                    //kein hochzählen oder herunterfahren
                }
                else
                {
              //      #if !DEBUG
                        this.activitiesTimer++;
                //    #endif

                    this.toolStripStatusLabelCountdown.Text = string.Format("Countdown ({0:00})", this.activitiesMax - this.activitiesTimer);

                    if (this.activitiesTimer >= this.activitiesMax)
                    {
                        ActivitiesTimerStart = false;
                        using (LogOnServices.AutoShotDown autoShotDown = new metatop.Applications.metaCall.WinForms.App.LogOnServices.AutoShotDown())
                        {
                            if (DialogResult.Cancel == autoShotDown.ShowDialog(this))
                            {
                                // der modale Dialog wurde mit 'Abbrechen' geschlossen
                                ActivitiesTimerStart = true;
                                this.activitiesTimer = 0;
                            }
                            else
                            {
                                closedByAutoShutDown = true;
                                this.Close();
                                // der modale Dialog wurde mit 'OK' geschlossen 
                            }
                        }
                    }
                }
            }
        }

        public Boolean ActivitiesTimerStart
        {
            set 
            {
                this.timerActivities.Enabled = value;
            }
        }

        private void toolStripButtonTraining_Click(object sender, EventArgs e)
        {
            //Training aktivieren
            MetaCall.Business.Training = true;
        }
    }
}