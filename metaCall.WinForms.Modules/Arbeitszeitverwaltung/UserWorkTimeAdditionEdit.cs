using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;

using metatop.Applications.metaCall.DataObjects;
using metatop.Applications.metaCall.BusinessLayer;

using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace metatop.Applications.metaCall.WinForms.Modules
{
    public partial class UserWorkTimeAdditionEdit : Form
    {
        private WorkTimeAdditions selectedWorkTimeAdditions;
        private List<WorkTimeAdditionItems> workTimeAdditionItems = new List<WorkTimeAdditionItems>();
        private Boolean EditTime;

        public UserWorkTimeAdditionEdit()
        {
            InitializeComponent();
        }

        public UserWorkTimeAdditionEdit(WorkTimeAdditions workTimeAdditions): this()
        {
            MetaCallPrincipal principal = System.Threading.Thread.CurrentPrincipal as MetaCallPrincipal;


            this.checkBoxConfirmed.Enabled = principal.IsInRole(MetaCallPrincipal.AdminRoleName);
            this.selectedWorkTimeAdditions = workTimeAdditions;

            EditTime = CheckOfAutoTime(this.selectedWorkTimeAdditions.Start, this.selectedWorkTimeAdditions.Stop);

            FillWorkTimeAdditionItemsList();

            Application.Idle += new EventHandler(Application_Idle);

            FillToEdit();
        }

        private Boolean CheckOfAutoTime(DateTime? checkDate1, DateTime? checkDate2)
        {
            Boolean IsAutoTime = false;

            if (checkDate1 != null)
            {
                DateTime testTime;
                testTime = (DateTime)checkDate1;

                if (testTime.Second != 0 && testTime.Millisecond != 0)
                {
                    IsAutoTime = true;
                }
                else
                {
                    if (checkDate2 != null)
                    {
                        testTime = (DateTime)checkDate2;

                        if (testTime.Second != 0 && testTime.Millisecond != 0)
                        {
                            IsAutoTime = true;
                        }
                    }
                }
            }
            this.dateTimePickerWorkDate.Enabled = !IsAutoTime;
            this.maskedTextBoxTo.Enabled = !IsAutoTime;
            this.MaskedTextBoxFrom.Enabled = !IsAutoTime;
            return !IsAutoTime;

        }

        void Application_Idle(object sender, EventArgs e)
        {
            string tmp;
            
            int FromHour;
            int FromMinute;

            int ToHour;
            int ToMinute;

            tmp = this.MaskedTextBoxFrom.Text.Substring(0, 2).Trim();
            if (tmp.Length < 2)
                FromHour = 0;
            else
                FromHour = int.Parse(tmp);

            if (this.MaskedTextBoxFrom.Text.Length != 5)
                tmp = "";
            else
                tmp = this.MaskedTextBoxFrom.Text.Substring(3, 2).Trim();

            if (tmp.Length < 2)
                FromMinute = 60;
            else
                FromMinute = int.Parse(tmp);


            tmp = this.maskedTextBoxTo.Text.Substring(0, 2).Trim();
            if (tmp.Length < 2)
                ToHour = 0;
            else
                ToHour = int.Parse(tmp);

            if (this.maskedTextBoxTo.Text.Length != 5)
                tmp = "";
            else
                tmp = this.maskedTextBoxTo.Text.Substring(3, 2).Trim();

            if (tmp.Length < 2)
                ToMinute = 60;
            else
                ToMinute = int.Parse(tmp);

            this.saveButton.Enabled =   (this.dateTimePickerWorkDate.Value != null) &&
                                        (this.ComboBoxUserWorkTimeItem.SelectedItem != null) &&
                                        (FromHour > 0) && (FromHour < 24) &&
                                        (ToHour > 0) && (ToHour < 24) &&
                                        (FromMinute >= 0) && (FromMinute < 60) &&
                                        (ToMinute >= 0) && (ToMinute < 60);

        }

        private void FillToEdit()
        {
            if (this.selectedWorkTimeAdditions != null)
            {
                if (this.selectedWorkTimeAdditions.Start != null)
                {
                    DateTime selectedStart = (DateTime)this.selectedWorkTimeAdditions.Start;
                    this.dateTimePickerWorkDate.Value = selectedStart.Date;
                    this.MaskedTextBoxFrom.Text = string.Format("{0:00}{1:00}", selectedStart.Hour, selectedStart.Minute);
                }
                else
                {
                    this.MaskedTextBoxFrom.Text = null;
                    this.dateTimePickerWorkDate.Value = DateTime.Today;
                }


                if (this.selectedWorkTimeAdditions.Stop != null)
                {
                    DateTime stop = (DateTime)this.selectedWorkTimeAdditions.Stop;
                    this.maskedTextBoxTo.Text = string.Format("{0:00}{1:00}", stop.Hour, stop.Minute);
                }
                else
                    this.maskedTextBoxTo.Text = null;

                if (this.selectedWorkTimeAdditions.Notice != null)
                    this.textBoxNotice.Text = this.selectedWorkTimeAdditions.Notice.ToString();
                else
                    this.textBoxNotice.Text = string.Empty;

                this.checkBoxConfirmed.Checked = this.selectedWorkTimeAdditions.Confirmed;

                // WorkTimeAdditionItem wählen
                if (this.selectedWorkTimeAdditions.WorkTimeAdditionItemType != null)
                {
                    foreach (WorkTimeAdditionItems workTimeAdditionItem in this.workTimeAdditionItems)
                    {
                        if (workTimeAdditionItem.WorkTimeAdditionItemId.Equals(this.selectedWorkTimeAdditions.WorkTimeAdditionItemType))
                        {
                            this.ComboBoxUserWorkTimeItem.SelectedItem = workTimeAdditionItem;
                        }
                    }
                }
            }
        }

        private void SaveToObject()
        {

            if (this.selectedWorkTimeAdditions != null)
            {
                if (EditTime == true)
                {
                    DateTime start = new DateTime(
                        this.dateTimePickerWorkDate.Value.Year,
                        this.dateTimePickerWorkDate.Value.Month,
                        this.dateTimePickerWorkDate.Value.Day,
                        int.Parse(this.MaskedTextBoxFrom.Text.Substring(0, 2)),
                        int.Parse(this.MaskedTextBoxFrom.Text.Substring(3, 2)),
                        0);

                    DateTime stop = new DateTime(
                        this.dateTimePickerWorkDate.Value.Year,
                        this.dateTimePickerWorkDate.Value.Month,
                        this.dateTimePickerWorkDate.Value.Day,
                        int.Parse(this.maskedTextBoxTo.Text.Substring(0, 2)),
                        int.Parse(this.maskedTextBoxTo.Text.Substring(3, 2)),
                        0);


                    this.selectedWorkTimeAdditions.Start = start;
                    this.selectedWorkTimeAdditions.Stop = stop;
                    TimeSpan timeSpan = stop.Subtract(start);

                    this.selectedWorkTimeAdditions.Duration = timeSpan.TotalSeconds;
                }
                this.selectedWorkTimeAdditions.Notice = this.textBoxNotice.Text.ToString();
                this.selectedWorkTimeAdditions.Confirmed = this.checkBoxConfirmed.Checked;
                this.selectedWorkTimeAdditions.WorkTimeAdditionItemType = ((WorkTimeAdditionItems)this.ComboBoxUserWorkTimeItem.SelectedItem as WorkTimeAdditionItems).WorkTimeAdditionItemId;
            }
        }

        private void FillWorkTimeAdditionItemsList()
        {
            try
            {
                DateTime from = new DateTime(2004, 1, 1);
                DateTime to = new DateTime(2021, 1, 1);

                this.workTimeAdditionItems = MetaCall.Business.Users.WorkTimeAdditionItems_GetAllByUser(this.selectedWorkTimeAdditions.User.UserId, from, to);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "UI Policy");
                if (rethrow)
                    throw;
            }

            //WorkTimeAdditionItems füllen
            ComboBoxUserWorkTimeItem.Items.Clear();
            ComboBoxUserWorkTimeItem.DisplayMember = "Bezeichnung";
            foreach (WorkTimeAdditionItems workTimeAdditionItems in this.workTimeAdditionItems)
            {
                ComboBoxUserWorkTimeItem.Items.Add(workTimeAdditionItems);
            }
        }

        private void ComboBoxUserWorkTimeItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBoxUserWorkTimeItem = sender as ComboBox;

            if (sender != null)
            {
                WorkTimeAdditionItems workTimeAdditionItem = this.ComboBoxUserWorkTimeItem.SelectedItem as WorkTimeAdditionItems;
            }
        }

        private void UserForm_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            SaveToObject();
            int count = MetaCall.Business.Users.mwWorkTime_Test(this.selectedWorkTimeAdditions);

            if (count > 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("Sie haben versucht doppelte Arbeitszeiten einzugeben!");
                MessageBox.Show(sb.ToString(),"Zeitüberprüfung");
            }
            else
            {
                this.DialogResult = DialogResult.OK;
                Application.Idle -= new EventHandler(this.Application_Idle);
                this.Close();
            }
        }
    }
}