using System;
using System.Collections.Generic;
using System.Text;

using metatop.Applications.metaCall.DataObjects;

namespace metatop.Applications.metaCall.WinForms.Modules.Telefonie
{
    public class ReminderQuestionChangeEventArgs: EventArgs
    {
        public ReminderQuestionChangeEventArgs(String reminderQuestion)
        {
            this.reminderQuestion = reminderQuestion;
        }

        private string reminderQuestion;

        public string ReminderQuestion
        {
            get { return reminderQuestion; }
        }
	
    }
}
