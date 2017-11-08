using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using metatop.Applications.metaCall.DataObjects;
using metatop.Applications.metaCall.BusinessLayer;

namespace metatop.Applications.metaCall.WinForms.Modules
{
    [ToolboxItem(false)]
    public partial class CallJobEdit : Form
    {
        CallJob callJob;

        List<CallJobStateInfo> callJobStateInfos;
        List<CallJobGroup> callJobGroups;
        List<UserInfo> users;
        
        public CallJobEdit()
        {
            InitializeComponent();
        }


        public CallJobEdit(CallJob callJob, List<CallJobGroup> availableGroups, List<UserInfo> availableUsers):this()
        {
            this.callJob = callJob;
            this.callJobGroups = availableGroups;
            this.users = availableUsers;

            BindCallJobStates();
            BindCallJobGroups();
            BindUsers();
            BindDialModes();

            FillControls();

            Application.Idle += new EventHandler(Application_Idle);
        }

        private void BindUsers()
        {
            this.usersComboBox.DisplayMember = "DisplayName";

            UserInfo us = new UserInfo();
            us.Nachname = "<kein Benutzer>";
            this.usersComboBox.Items.Add(us);

            foreach(UserInfo user in this.users)
            {
                this.usersComboBox.Items.Add(user);
            }
        }

        void Application_Idle(object sender, EventArgs e)
        {
            UpdateUI();
        }

        private void BindDialModes()
        {
            foreach (string dialModeName in Enum.GetNames(typeof(DialMode)))
            {
                this.dialModeComboBox.Items.Add(dialModeName);
            }
        }

        private void BindCallJobGroups()
        {

            this.callJobGroupComboBox.DisplayMember = "DisplayName";
            this.callJobGroupComboBox.DataSource = this.callJobGroups;
        }

        private void BindCallJobStates()
        {
            this.callJobStateInfos = MetaCall.Business.CallJobs.CallJobStates;

            this.statusComboBox.DisplayMember = "DisplayName";
            this.statusComboBox.DataSource = this.callJobStateInfos;
        }

        private void FillControls()
        {
            if (this.callJob == null || this.callJob.CallJobId == Guid.Empty )
            {
                this.Text = "Sponsorenauswahl ändern";
                this.sponsorTextBox.Visible = false;
                this.labelSponsor.Visible = false;
                this.startDateTimePicker.Value = DateTime.Today;
                this.stopDateTimePicker.Value = DateTime.Today;
                this.stopDateTimePicker.Checked = false;
                this.iterationCounterNumericUpDown.Value = 0;
                this.stopDateTimePicker.ShowCheckBox = false;

                this.checkBoxTransferCallJobGroup.Visible = true;
                this.checkBoxTransferDialMode.Visible = true;
                this.checkBoxTransferIterationCounter.Visible = true;
                this.checkBoxTransferStartDate.Visible = true;
                this.checkBoxTransferStatus.Visible = true;
                this.checkBoxTransferStopDate.Visible = true;
                this.checkBoxTransferUser.Visible = true;

                this.label2.Left = 30;
                this.label3.Left = 30;
                this.label4.Left = 30;
                this.label5.Left = 30;
                this.label6.Left = 30;
                this.label7.Left = 30;
                this.label8.Left = 30;
            }
            else
            {
                this.Text = "Sponsor ändern";
                this.sponsorTextBox.Text = this.callJob.Sponsor.DisplayName;
                this.sponsorTextBox.Visible = true;
                this.labelSponsor.Visible = true;
                this.startDateTimePicker.Value = this.callJob.StartDate;
                if (this.callJob.StopDate.Date != DateTime.MaxValue.Date)
                {
                    this.stopDateTimePicker.Checked = true;
                    this.stopDateTimePicker.Value = this.callJob.StopDate;
                }
                else
                {
                    this.stopDateTimePicker.Checked = false;
                }
                this.stopDateTimePicker.ShowCheckBox = true;

                if (this.callJob.State != CallJobState.FirstCall)
                {
                    this.startDateTimePicker.MinDate = this.callJob.StartDate;
                    this.stopDateTimePicker.MinDate = DateTime.Today;
                }
                this.iterationCounterNumericUpDown.Value = this.callJob.IterationCounter;

                foreach (CallJobStateInfo callJobStateInfo in this.callJobStateInfos)
                {
                    if (callJobStateInfo.CallJobStateId == (int)this.callJob.State)
                    {
                        this.statusComboBox.SelectedItem = callJobStateInfo;
                        break;
                    }
                }

                foreach (CallJobGroup callJobGroup in this.callJobGroups)
                {
                    if (callJobGroup.CallJobGroupId.Equals(this.callJob.CallJobGroup.CallJobGroupId))
                    {
                        this.callJobGroupComboBox.SelectedItem = callJobGroup;
                        break;
                    }
                }

                this.dialModeComboBox.SelectedItem = Enum.GetName(typeof(DialMode), this.callJob.DialMode);

                this.checkBoxTransferCallJobGroup.Visible = false;
                this.checkBoxTransferDialMode.Visible = false;
                this.checkBoxTransferIterationCounter.Visible = false;
                this.checkBoxTransferStartDate.Visible = false;
                this.checkBoxTransferStatus.Visible = false;
                this.checkBoxTransferStopDate.Visible = false;
                this.checkBoxTransferUser.Visible = false;

                this.label2.Left = 20;
                this.label3.Left = 20;
                this.label4.Left = 20;
                this.label5.Left = 20;
                this.label6.Left = 20;
                this.label7.Left = 20;
                this.label8.Left = 20;

            }

            if (this.callJob != null && this.callJob.User != null)
            {
                foreach (UserInfo user in this.users)
                {
                    if (user.UserId.Equals(this.callJob.User.UserId))
                    {
                        this.usersComboBox.SelectedItem = user;
                        break;
                    }
                }
            }
            else
            {
                this.usersComboBox.SelectedIndex = 0;
            }


            //solange noch kein richtiger Dialer aktiv ist wird nicht gewählt!!!!
            //TODO: Dialer erlauben
            this.dialModeComboBox.Enabled = false;

        }

        private void UpdateUI()
        {
            if (callJob == null || callJob.Project == null || callJob.Project.State == null)
            {
                this.startDateTimePicker.Enabled = true;
                this.stopDateTimePicker.Enabled = true;
                this.iterationCounterNumericUpDown.Enabled = true;
                this.statusComboBox.Enabled = true;
                this.callJobGroupComboBox.Enabled = true;
                this.acceptButton.Enabled = true;
                this.usersComboBox.Enabled = true;
            }
            else
            {
                bool isEditable = callJob.Project.State != ProjectState.Finished;

                switch (this.callJob.State)
                {
                    case CallJobState.Invalid:
                        break;
                    case CallJobState.FirstCall:
                        this.startDateTimePicker.Enabled = true && isEditable;
                        this.stopDateTimePicker.Enabled = true && isEditable;
                        this.iterationCounterNumericUpDown.Enabled = true && isEditable;
                        this.statusComboBox.Enabled = true && isEditable;
                        this.callJobGroupComboBox.Enabled = true && isEditable;
                        this.acceptButton.Enabled = true && isEditable;
                        this.usersComboBox.Enabled = true && isEditable;
                        break;
                    case CallJobState.Waiting:
                    case CallJobState.ReminderCallJob:
                    case CallJobState.FurtherCall:
                        this.startDateTimePicker.Enabled = false;
                        this.stopDateTimePicker.Enabled = true && isEditable;
                        this.iterationCounterNumericUpDown.Enabled = true && isEditable;
                        this.statusComboBox.Enabled = true && isEditable;
                        this.callJobGroupComboBox.Enabled = true && isEditable;
                        this.acceptButton.Enabled = true && isEditable;
                        this.usersComboBox.Enabled = true && isEditable;
                        break;
                    case CallJobState.Cancelled:
                    case CallJobState.Ordered:
                        this.startDateTimePicker.Enabled = false;
                        this.stopDateTimePicker.Enabled = false;
                        this.iterationCounterNumericUpDown.Enabled = false;
                        this.statusComboBox.Enabled = false;
                        this.callJobGroupComboBox.Enabled = false;
                        this.acceptButton.Enabled = false;
                        this.usersComboBox.Enabled = false;
                        break;
                }
            }
        }

        private void statusComboBox_Validating(object sender, CancelEventArgs e)
        {
            CallJobStateInfo stateInfo = this.statusComboBox.SelectedItem as CallJobStateInfo;
            CallJobState state = (CallJobState) stateInfo.CallJobStateId;

            if (stateInfo == null)
                return;

            if (this.callJob != null && this.callJob.CallJobId != Guid.Empty )
            {
                switch (this.callJob.State)
                {
                    case CallJobState.Invalid:
                        e.Cancel = true;
                        break;
                    case CallJobState.FirstCall:
                        break;
                    case CallJobState.Waiting:
                    case CallJobState.ReminderCallJob:
                    case CallJobState.FurtherCall:
                        if (state == CallJobState.Ordered ||
                            state == CallJobState.Cancelled ||
                            state == CallJobState.FirstCall)
                        {
                            e.Cancel = true;
                        }
                        break;
                    case CallJobState.Cancelled:
                        e.Cancel = true;
                        break;
                    case CallJobState.Ordered:
                        e.Cancel = true;
                        break;
                    default:
                        break;
                }
            }


        }

        private void CallJobEdit_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.DialogResult == DialogResult.OK)
            {
                StoreToObject();
            }


            Application.Idle -= new EventHandler(this.Application_Idle);
        }

        private void StoreToObject()
        {

            if (this.callJob == null)
                return;

            CallJobGroupInfo callJobGroupInfo = MetaCall.Business.CallJobGroups.Get(this.callJobGroupComboBox.SelectedItem as CallJobGroup);

            if (this.callJob.CallJobId != Guid.Empty)
            {
                this.callJob.StartDate = this.startDateTimePicker.Value;
                
                if (this.stopDateTimePicker.Checked == true)
                    this.callJob.StopDate = this.stopDateTimePicker.Value;
                else
                    this.callJob.StopDate = DateTime.MaxValue;

                this.callJob.IterationCounter = (int)this.iterationCounterNumericUpDown.Value;
                this.callJob.State = (CallJobState)((CallJobStateInfo)this.statusComboBox.SelectedItem).CallJobStateId;
                this.callJob.DialMode = (DialMode)Enum.Parse(typeof(DialMode), (string)this.dialModeComboBox.SelectedItem);
                this.callJob.CallJobGroup = callJobGroupInfo;
                this.callJob.User = this.usersComboBox.SelectedItem as UserInfo;
            }
            else
            {
                if (this.checkBoxTransferStartDate.Checked == true)
                    this.callJob.StartDate = this.startDateTimePicker.Value;
                else
                    this.callJob.StartDate = DateTime.MinValue.Date;

                if (this.checkBoxTransferStopDate.Checked == true)
                    this.callJob.StopDate = this.stopDateTimePicker.Value;
                else
                    this.callJob.StopDate = DateTime.MinValue.Date;

                if (this.checkBoxTransferIterationCounter.Checked == true)
                    this.callJob.IterationCounter = (int)this.iterationCounterNumericUpDown.Value;
                else
                    this.callJob.IterationCounter = Int16.MaxValue;

                if (this.checkBoxTransferStatus.Checked == true)
                    this.callJob.State = (CallJobState)((CallJobStateInfo)this.statusComboBox.SelectedItem).CallJobStateId;
                else
                {
                    this.callJob.State = CallJobState.Unseeded;
                }

                if (this.checkBoxTransferDialMode.Checked == true)
                    this.callJob.DialMode = (DialMode)Enum.Parse(typeof(DialMode), (string)this.dialModeComboBox.SelectedItem);
                else
                {
                    this.callJob.DialMode = DialMode.Unseeded;
                }

                if (this.checkBoxTransferCallJobGroup.Checked == true)
                    this.callJob.CallJobGroup = callJobGroupInfo;
                else
                    this.callJob.CallJobGroup = null;

                if (this.checkBoxTransferUser.Checked == true)
                    this.callJob.User = this.usersComboBox.SelectedItem as UserInfo;
                else
                    this.callJob.User = null;

            }
        }

        private void startDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            this.checkBoxTransferStartDate.Checked = this.checkBoxTransferStartDate.Visible;
        }

        private void stopDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            this.checkBoxTransferStopDate.Checked = this.checkBoxTransferStopDate.Visible;
        }

        private void statusComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.checkBoxTransferStatus.Checked = this.checkBoxTransferStatus.Visible;
        }

        private void iterationCounterNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            this.checkBoxTransferIterationCounter.Checked = this.checkBoxTransferIterationCounter.Visible;
        }

        private void dialModeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.checkBoxTransferDialMode.Checked = this.checkBoxTransferDialMode.Visible;
        }

        private void callJobGroupComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.checkBoxTransferCallJobGroup.Checked = this.checkBoxTransferCallJobGroup.Visible;
        }

        private void usersComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.checkBoxTransferUser.Checked = this.checkBoxTransferUser.Visible;
        }

        public Boolean TransferSelectedItems(CallJobTransferItem callJobTransferItem)
        {
            switch (callJobTransferItem)
            {
                case CallJobTransferItem.CallJobGroup:
                    return this.checkBoxTransferCallJobGroup.Checked;
                case CallJobTransferItem.DialMode:
                    return this.checkBoxTransferDialMode.Checked;
                case CallJobTransferItem.IterationCounter:
                    return this.checkBoxTransferIterationCounter.Checked;
                case CallJobTransferItem.StartDateTime:
                    return this.checkBoxTransferStartDate.Checked;
                case CallJobTransferItem.Status:
                    return this.checkBoxTransferStatus.Checked;
                case CallJobTransferItem.StopDateTime:
                    return this.checkBoxTransferStopDate.Checked;
                case CallJobTransferItem.User:
                    return this.checkBoxTransferUser.Checked;
                default:
                    return false;
            }
        }

        public enum CallJobTransferItem
        {
            StartDateTime,
            StopDateTime,
            Status,
            IterationCounter,
            DialMode,
            CallJobGroup,
            User
        };
    }
}