using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using metatop.Applications.metaCall.DataObjects;
using metatop.Applications.metaCall.BusinessLayer;

namespace metatop.Applications.metaCall.WinForms.Modules.Telefonie
{
    [ToolboxItem(false)]
    public partial class CreateReminderDurring : UserControl, ISupportInitialize, IInitializeCall 
    {
        /// <summary>
        /// Wird ausgelöst, wenn sich der Wert des Controls ändert
        /// </summary>
        [Description("Wird ausgelöst, wenn sich der Wert des Controls ändert"),
        Category("Action")]
        public event CreateReminderDurringHandler ValueChanged;

        
        private CallJobReminderTracking currentTracking = CallJobReminderTracking.ExactDateAndTime;

        TimeRange[] timeRanges = new TimeRange[]{
            new TimeRange("06:00 - 08:00"),
            new TimeRange("08:00 - 10:00"),
            new TimeRange("10:00 - 12:00"), 
            new TimeRange("12:00 - 14:00"),
            new TimeRange("14:00 - 16:00"),
            new TimeRange("16:00 - 18:00"),
            new TimeRange("18:00 - 20:00")
        };
        
        private Call call;
        public Call Call
        {
            get { return call; }
        }

        /// <summary>
        /// Setzt den Modus des Controls so, dass nur TeamReminder erstellt werden dürfen
        /// </summary>
        public void TeamReminderOnly()
        { 
            radioButton1.Enabled = false;
            radioButton2.Enabled = true;
            radioButton2.Checked = true;
                OnValueChanged(new CreateReminderDurringEventArgs(this.ReminderDateStart, 
                    this.ReminderDateStop, 
                    this.IsTeamReminder, this.ReminderTracking));
            
        }

        public void PersonalReminderOnly()
        {
            radioButton1.Enabled = true;
            radioButton1.Checked = true;
            radioButton2.Enabled = false;
            OnValueChanged(new CreateReminderDurringEventArgs(this.ReminderDateStart,
                this.ReminderDateStop,
                this.IsTeamReminder, this.ReminderTracking));

        }

        private void UpdateControls()
        {

            if (isInitialize) return;

            if (!MetaCall.Business.Users.IsLoggedOn)
                return;

            if (call == null)
                return;

            //Vorgabe-Datum für neue Wiedervorlage ermitteln und anzeigen
            this.dateTimePickerExt1.Value = DateTime.Now.AddMinutes(30);
            this.timeSpanComboBox.SelectedIndex = 0;

            this.radioButton2.Checked = true;
            this.radioButton1.Checked = false;

            //Team-Bezeichnung angeben
            //TeamInfo[] teams = call.CallJob.Project.Teams;

            this.lblTeam.Text = string.Empty; 
            if (MetaCall.Business.Users.CurrentUser.Teams.Length > 0)
            {
                TeamInfo team = MetaCall.Business.Users.CurrentUser.Teams[0].Team;

                if (team != null)
                {

                    //TODO: den Fall ermöglichen, dass ein Benutzer mehreren Teams zugeordnet sein kann
                    //foreach (TeamInfo teamInfo in teams)
                    {
                        if (this.lblTeam.Text.Length > 0) this.lblTeam.Text += "; ";
                        this.lblTeam.Text += string.Format("({0})", team.Bezeichnung);
                    }
                }
            }

        }

        public CreateReminderDurring()
        {
            InitializeComponent();
        }

        void btn_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton btn = sender as RadioButton;

            if (btn == null)
                return;

            this.currentTracking = (CallJobReminderTracking)btn.Tag;


            if (!this.isInitialize)
            {
                OnValueChanged(new CreateReminderDurringEventArgs(
                       this.reminderdateStart,
                       this.reminderDateStop,
                       this.IsTeamReminder,
                       this.ReminderTracking));
            }
        }

        public int FormHeight
        {
            get
            {
                return this.dateTimePickerExt1.Top + this.dateTimePickerExt1.Height + 10;
            }
        }

        private DateTime reminderDateStop;

        /// <summary>
        /// Gibt das Ende des gewählten Reminders an
        /// </summary>
        public DateTime ReminderDateStop
        {
            get {
                UpdateStartStop();
                return reminderDateStop; 
            }
            set { reminderDateStop = value; }
        }

        private DateTime reminderdateStart;

        /// <summary>
        /// gibt den Start des gewählten Reminders
        /// </summary>
        public DateTime ReminderDateStart
        {
            get {
                UpdateStartStop();
                return reminderdateStart; 
            }
            set { reminderdateStart = value; }
        }

        private void UpdateStartStop()
        {

            if (this.currentTracking == CallJobReminderTracking.ExactDateAndTime)
            {
                this.reminderdateStart = this.reminderDateStop = this.dateTimePickerExt1.Value;
            }
            else if (this.currentTracking == CallJobReminderTracking.OnlyTimeSpan)
            {
                TimeRange range = timeSpanComboBox.SelectedItem as TimeRange;
                if (range == null)
                    throw new Exception("Zeitfenster kann nicht ausgelesen werden");

                this.reminderdateStart = DateTime.Now.Date.Add(range.StartTime);
                this.reminderDateStop = DateTime.Now.Date.Add(range.StopTime);
            }
        }
	
        /// <summary>
        /// Liefert True wenn der gewählte Reminder ein TeamReminder ist
        /// </summary>
        public bool IsTeamReminder
        {
            get
            {
                return this.radioButton2.Checked;
            }
        }

        /// <summary>
        /// Liefert das gewählte ReminderTracking oder legt dieses fest
        /// </summary>
        public CallJobReminderTracking ReminderTracking
        {
            get
            {
                return this.currentTracking;
            }
        }

        private void timeSpanComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            TimeRange range = timeSpanComboBox.SelectedItem as TimeRange;
            if (range != null)
            {
                this.currentTracking = CallJobReminderTracking.OnlyTimeSpan;
                this.reminderdateStart = DateTime.Now.Date.Add(range.StartTime);
                this.reminderDateStop = DateTime.Now.Date.Add(range.StopTime);

                if (!this.isInitialize)
                {

                    OnValueChanged(new CreateReminderDurringEventArgs(
                        this.reminderdateStart,
                        this.reminderDateStop,
                        this.IsTeamReminder,
                        this.ReminderTracking));
                }
            }
        }

        private void dateTimePickerExt1_ValueChanged(object sender, EventArgs e)
        {
            this.currentTracking = CallJobReminderTracking.ExactDateAndTime;
            this.reminderdateStart = this.reminderDateStop = dateTimePickerExt1.Value;

            if (!this.isInitialize)
            {

                OnValueChanged(new CreateReminderDurringEventArgs(
                    this.reminderdateStart,
                    this.reminderDateStop,
                    this.IsTeamReminder,
                    this.ReminderTracking));
            }
        }

        private class TimeRange: IEquatable<TimeRange>    
        {

            public TimeRange(DateTime startTime, DateTime stopTime)
            {
                this.startTime = startTime.TimeOfDay;
                this.stopTime = stopTime.TimeOfDay;
            }

            public TimeRange(string startStop)
            {
                try
                {
                    string start = startStop.Substring(0, startStop.IndexOf('-') - 1).Trim();
                    string stop = startStop.Substring(startStop.IndexOf('-') + 1).Trim();

                    startTime = TimeSpan.Parse(start);
                    stopTime = TimeSpan.Parse(stop);
                }
                catch
                {
                    throw;
                }
            }
            
            private TimeSpan startTime;

            public TimeSpan StartTime
            {
                get { return startTime; }
                set { startTime = value; }
            }

            private TimeSpan stopTime;

            public TimeSpan StopTime
            {
                get { return stopTime; }
                set { stopTime = value; }
            }

            public override string ToString()
            {
                return string.Format(System.Globalization.CultureInfo.CurrentUICulture, "{0:HH:mm} - {1:HH:mm}", new DateTime(startTime.Ticks), new DateTime(stopTime.Ticks));
            }


            #region IEquatable<TimeRange> Member

            public bool Equals(TimeRange other)
            {

                return ((other.startTime == this.startTime) &&
                    (other.stopTime == this.stopTime));
            }

            #endregion
        }

        #region ISupportInitialize Member

        bool isInitialize;
        public void BeginInit()
        {
            this.isInitialize = true;
        }

        public void EndInit()
        {
            this.isInitialize = false;
        }

        #endregion

        protected void OnValueChanged(CreateReminderDurringEventArgs e)
        {
            if (ValueChanged != null)
                ValueChanged(this, e);
        }


        #region IInitializeCall Member

        public void InitializeCall(Call call)
        {
            this.call = call;
            UpdateControls();
            foreach (Control ctl in this.Controls)
            {
                IInitializeCall initializeCallControl = ctl as IInitializeCall;
                if (initializeCallControl != null)
                {
                    initializeCallControl.InitializeCall(call);
                }
            }
        }

        #endregion

        private void CreateReminderDurring_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            //GetReminderTrackingOptions();

            timeSpanComboBox.DataSource = this.timeRanges;

            int maxWidth = 0;
            foreach (TimeRange range in this.timeRanges)
            {
                Size fontSize = TextRenderer.MeasureText(range.ToString(), timeSpanComboBox.Font);
                maxWidth = Math.Max(maxWidth, fontSize.Width);
            }

            timeSpanComboBox.DropDownWidth = maxWidth;
        }
    }

    public delegate void CreateReminderDurringHandler(object sender, CreateReminderDurringEventArgs e);

    public class CreateReminderDurringEventArgs : EventArgs
    {
        
        public CreateReminderDurringEventArgs(
            DateTime start, 
            DateTime stop,
            bool isTeamReminder,
            CallJobReminderTracking callJobReminderTracking)
        {
            this.start = start;
            this.stop = stop;
            this.isTeamReminder = isTeamReminder;
            this.callJobReminderTracking = callJobReminderTracking;
        }

        private DateTime start;
        public DateTime Start
        {
            get { return start; }
        }

        private DateTime stop;
        public DateTime Stop
        {
            get { return stop; }
        }

        private bool isTeamReminder;
        public bool IsTeamReminder
        {
            get { return isTeamReminder; }
        }

        private CallJobReminderTracking callJobReminderTracking;
        public CallJobReminderTracking CallJobReminderTracking
        {
            get { return callJobReminderTracking; }
        }
	
	
	
	
    }

}
