using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace metatop.Applications.metaCall.WinForms.Modules
{
    [ToolboxItem(true)]
    public partial class SelectionPeriod : UserControl
    {
        private TimeMode timeMode = TimeMode.FromTo;
        private string lastMonthMasked;

        public SelectionPeriod()
        {
            InitializeComponent();
            SetDefaultDate();
        }

        public event SelectionPeriodChangedEventHandler SelectionChanged;

        private  void OnSelectionPeriodChanged(SelectionPeriodEventArgs e)
        {
            if (SelectionChanged != null)
                SelectionChanged(e);
        }

        public DateTime SelectionFrom
        {
            set
            {
                this.fromDateTimePicker.Value = value;
                int year = this.fromDateTimePicker.Value.Year;
                int month = this.fromDateTimePicker.Value.Month;

                this.monthMaskedTextBox.Text = string.Format("{0:00}{1:0000}", month, year);
                UpdateUI();

            }
            get
            {
                if (this.timeMode == TimeMode.FromTo)
                {
                    return this.fromDateTimePicker.Value;
                }
                else
                {
                    int month = int.Parse(this.monthMaskedTextBox.Text.Substring(0, 2));
                    int year = int.Parse(this.monthMaskedTextBox.Text.Substring(3, 4));

                    if (year == 2007 && month == 5)
                        return new DateTime(year, month, 6);
                    else
                        return new DateTime(year, month, 1);

                }
            }
        }

        public DateTime SelectionTo
        {
            set
            {
                this.toDateTimePicker.Value = value;
                UpdateUI();
            }
            get
            {
                if (this.timeMode == TimeMode.FromTo)
                {
                    return this.toDateTimePicker.Value;
                }
                else
                {
                    int month = int.Parse(this.monthMaskedTextBox.Text.Substring(0, 2));
                    int year = int.Parse(this.monthMaskedTextBox.Text.Substring(3, 4));

                    return new DateTime(year, month, DateTime.DaysInMonth(year, month));
                }
            }
        }

        private enum TimeMode
        {
            FromTo,
            Month,
        }

        private void SetTimeMode(object sender, EventArgs e)
        {

            if (this.timeModeFromTo.Checked)
                this.timeMode = TimeMode.FromTo;
            else if (this.timeModeMonth.Checked)
                this.timeMode = TimeMode.Month;

            OnSelectionPeriodChanged(new SelectionPeriodEventArgs(true));
            
            UpdateUI();
        }

        private void SetDefaultDate()
        {

            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;

            if (year == 2007 && month == 5)
                SelectionFrom = new DateTime(year, month, 6);
            else
                SelectionFrom = new DateTime(year, month, 1);

            SelectionTo = new DateTime(year, month, DateTime.DaysInMonth(year, month));

            this.lastMonthMasked = this.monthMaskedTextBox.Text;

            SetTimeMode(null, EventArgs.Empty);
            UpdateUI();
        }

        private void UpdateUI()
        {
            this.fromDateTimePicker.Enabled = (this.timeMode == TimeMode.FromTo);
            this.toDateTimePicker.Enabled = (this.timeMode == TimeMode.FromTo);
            this.monthMaskedTextBox.Enabled = (this.timeMode == TimeMode.Month);
        }

        private void fromDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            OnSelectionPeriodChanged(new SelectionPeriodEventArgs(true));
        }

        private void toDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            OnSelectionPeriodChanged(new SelectionPeriodEventArgs(true));
        }

        private void monthMaskedTextBox_Validated(object sender, EventArgs e)
        {
            try
            {
                int month = int.Parse(this.monthMaskedTextBox.Text.Substring(0, 2));
                int year = int.Parse(this.monthMaskedTextBox.Text.Substring(3, 4));

                if (year < 2007 || (year == 2007 && month < 5))
                {
                    year = 2007;

                    if (month < 5)
                        month = 5;

                    this.monthMaskedTextBox.Text = month.ToString() + year.ToString();
                }

                OnSelectionPeriodChanged(new SelectionPeriodEventArgs(true));
            }
            catch
            {
                this.monthMaskedTextBox.Text = this.lastMonthMasked;
            }
        }

        private void SelectionPeriod_Load(object sender, EventArgs e)
        {
            SetDefaultDate();
        }
    }

    public delegate void SelectionPeriodChangedEventHandler(SelectionPeriodEventArgs e);

    public class SelectionPeriodEventArgs : EventArgs
    {
        private Boolean changed;

        public SelectionPeriodEventArgs(Boolean changed)
        {
            this.changed = changed;
        }

        public Boolean Changed
        {
            get { return this.changed; }
        }
    }
}
