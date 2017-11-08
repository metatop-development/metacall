using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace metatop.Applications.metaCall.WinForms.Modules.Telefonie
{
    [ToolboxItem(false)]
    public partial class CreateReminderDurringQuestion : UserControl, ISupportInitialize, IInitializeCall
    {

        public event EventHandler<ReminderQuestionDurringChangeEventArgs> ReminderQuestionDurringChanged;

        private string reminderQuestion;

        private string reminderAnswer;

        public CreateReminderDurringQuestion()
        {
            InitializeComponent();
            radioButton2.Checked = true;
        }

        public int FormHeight
        {
            get { return 70; }
        }

        public string ReminderQuestion
        {
            get { return reminderQuestion;}
        }

        public string ReminderAnswer
        {
            get
            {
                return this.reminderAnswer;
            }

            set
            {
                this.reminderAnswer = value;
                this.reminderAnswerLabel.Text = this.reminderAnswer;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                OnReminderQuestionDurringChanged(new ReminderQuestionDurringChangeEventArgs("Ja"));
                this.reminderQuestion = "Ja";
                this.reminderAnswerLabel.ForeColor = Color.Blue;
            }

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                OnReminderQuestionDurringChanged(new ReminderQuestionDurringChangeEventArgs("Nein"));
                this.reminderQuestion = "Nein";
                this.reminderAnswerLabel.ForeColor = Color.Black;
            }

        }

        protected void OnReminderQuestionDurringChanged(ReminderQuestionDurringChangeEventArgs e)
        {
            if (ReminderQuestionDurringChanged != null)
                ReminderQuestionDurringChanged(this, e);
        }

        #region ISupportInitialize Member

        bool isInitializing;
        public void BeginInit()
        {
            this.isInitializing = true;
        }

        public void EndInit()
        {
            this.isInitializing = false;
        }

        #endregion

        #region IInitializeCall Member

        public void InitializeCall(metatop.Applications.metaCall.DataObjects.Call call)
        {
            if (this.isInitializing) return;

            radioButton2.Checked = true;
            this.reminderQuestion = "Nein";
            this.ReminderAnswer = string.Empty;
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
    }
}
