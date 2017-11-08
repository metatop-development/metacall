using System;
using System.Collections.Generic;
using System.Text;

namespace metatop.Applications.metaCall.DataObjects
{
    public partial class CallJobReminderInfo
    {

        public string DisplayStartDate
        {
            get
            {
                if (this.ReminderTracking == CallJobReminderTracking.DateAndTimeSpan ||
                    this.ReminderTracking == CallJobReminderTracking.OnlyTimeSpan)
                {
                    return this.ReminderDateStart.ToString("t");
                }
                else if (this.ReminderTracking == CallJobReminderTracking.Day)
                {
                    return this.ReminderDateStart.ToString("d");
                }
                else if (this.ReminderTracking == CallJobReminderTracking.Week)
                {
                    DateUtilitys.CalendarWeek calenderWeek = DateUtilitys.GetCalendarWeek(this.ReminderDateStart);
                    return string.Format("{0} / {1}", calenderWeek.Week.ToString(), calenderWeek.Year.ToString());
                }
                else if (this.ReminderTracking == CallJobReminderTracking.WeekDay)
                {
                    return this.ReminderDateStart.ToString("dddd");
                }
                else if (this.ReminderTracking == CallJobReminderTracking.ExactDateAndTime)
                {
                    return this.ReminderDateStart.ToString("g");
                }
                else
                {
                    return string.Empty;
                }

            }
        }

        public string DisplayEndDate
        {
            get
            {
                if (this.ReminderTracking == CallJobReminderTracking.DateAndTimeSpan ||
                    this.ReminderTracking == CallJobReminderTracking.OnlyTimeSpan)
                {
                    //return this.ReminderDateStop.ToString("t");
                    return this.ReminderDateStop.ToString("t");
                }
                else if (this.ReminderTracking == CallJobReminderTracking.Day)
                {
                    return string.Empty;
                }
                else if (this.ReminderTracking == CallJobReminderTracking.Week)
                {
                    return string.Empty;
                }
                else if (this.ReminderTracking == CallJobReminderTracking.WeekDay)
                {
                    return string.Empty;
                }
                else if (this.ReminderTracking == CallJobReminderTracking.ExactDateAndTime)
                {
                    return string.Empty;
                }
                else
                    return string.Empty;
            }
        }
    }
}
