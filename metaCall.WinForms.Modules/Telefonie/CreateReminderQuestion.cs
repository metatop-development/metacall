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
    public partial class CreateReminderQuestion : UserControl, ISupportInitialize, IInitializeCall
    {

        public event EventHandler<ReminderQuestionChangeEventArgs> ReminderQuestionChanged;

        private string reminderQuestion;

        private string reminderAnswer;

        public CreateReminderQuestion()
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
                OnReminderQuestionChanged(new ReminderQuestionChangeEventArgs("Ja"));
                this.reminderQuestion = "Ja";
                this.reminderAnswerLabel.ForeColor = Color.Blue;
            }

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                OnReminderQuestionChanged(new ReminderQuestionChangeEventArgs("Nein"));
                this.reminderQuestion = "Nein";
                this.reminderAnswerLabel.ForeColor = Color.Black;
            }

        }

        protected void OnReminderQuestionChanged(ReminderQuestionChangeEventArgs e)
        {
            if (ReminderQuestionChanged != null)
                ReminderQuestionChanged(this, e);
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

            if (call != null)
            {
                metatop.Applications.metaCall.DataObjects.ProjectInfo project;
                project = call.CallJob.Project;

                if (project != null)
                {
                    if (project.ReminderDateMax < System.DateTime.Today)
                    {
                        radioButton1.Enabled = false;
                    }
                    else
                    {
                        radioButton1.Enabled = true;
                    }
                }
            }

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
