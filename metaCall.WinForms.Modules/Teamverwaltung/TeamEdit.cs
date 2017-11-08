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
    public partial class TeamEdit : Form
    {
        private Team selectedTeam;
        private List<CenterInfo> centerList = new List<CenterInfo>();

        private ProjectState projectState = new  ProjectState();

        private Boolean newTeam;

        ReminderViewInfo rviView;
        UserInfoView uiView;

        public TeamEdit()
        {
            InitializeComponent();
        }

        private ProjectState ProjectState
        {
            set
            {
                if (value == ProjectState.Finished)
                {
                    this.checkBoxActiv.Checked = false;
                    this.checkBoxCompleted.Checked = true;
                }
                else if (value == ProjectState.ReleasedForPhone)
                {
                    this.checkBoxActiv.Checked = true;
                    this.checkBoxCompleted.Checked = false;
                }
                else
                {
                    this.checkBoxActiv.Checked = false;
                    this.checkBoxCompleted.Checked = false;
                }
                this.projectState = value;
            }
            get 
            {
                return this.projectState;
            }
        }

        public TeamEdit(Team team): this()
        {

            this.selectedTeam = team;
            FillCentersList();

            if (team.Bezeichnung == null)
            {
                this.newTeam = true;
            }
            else
            {
                this.newTeam = false;
            }

            Application.Idle += new EventHandler(Application_Idle);

            FillToEdit();

            checkBoxActiv_Click(null,null);

            if (this.newTeam == false)
                rviView = GetReminderViewInfoControl();


            this.tabPageReminder.SuspendLayout();
            this.tabPageReminder.Controls.Clear();

            if (rviView != null)
            {
                this.tabPageReminder.Controls.Add(rviView);
                rviView.Visible = true;
                rviView.Dock = DockStyle.Fill;

                this.tabPageReminder.ResumeLayout();
            }

            if (this.newTeam == false)
                uiView = GetUserInfoViewControl();

            this.tabPageUser.SuspendLayout();
            this.tabPageUser.Controls.Clear();

            if (uiView != null)
            {
                this.tabPageUser.Controls.Add(uiView);
                uiView.Visible = true;
                uiView.Dock = DockStyle.Fill;
                this.tabPageUser.ResumeLayout();
            }

            tabControlUser_Selected(null, null);
        }

        private void SetProjectList()
        {
            this.tabPageProject.SuspendLayout();
            this.tabPageProject.Controls.Clear();

            if (ProjectState != ProjectState.Unknown9)
            {
                ProjectViewInfo pviView = GetProjectViewInfoControl(ProjectState, this.selectedTeam);

                pviView.Visible = true;
                pviView.Dock = DockStyle.Fill;
                this.tabPageProject.Controls.Add(pviView);
            }

            this.tabPageProject.ResumeLayout();
        }

        void Application_Idle(object sender, EventArgs e)
        {
            this.saveButton.Enabled =   (this.TextBoxBezeichnung.Text.Length > 0) &&
                                        (this.TextBoxBeschreibung.Text.Length > 0);
        }

        private void FillToEdit()
        {
            if (this.selectedTeam != null)
            {
                this.TextBoxBezeichnung.Text = this.selectedTeam.Bezeichnung;
                this.TextBoxBeschreibung.Text = this.selectedTeam.Beschreibung;
                this.checkBoxDeleted.Checked = this.selectedTeam.IsDeleted;

                // Center wählen

                if (this.selectedTeam.Center != null)
                {
                    foreach (CenterInfo center in this.centerList)
                    {
                        if (center.CenterId.Equals(this.selectedTeam.Center.CenterId))
                        {
                            this.centerComboBox.SelectedItem = center;
                        }
                    }
                }
            }
        }

        private void SaveToObject()
        {

            if (this.selectedTeam != null)
            {
                this.selectedTeam.Bezeichnung = this.TextBoxBezeichnung.Text;
                this.selectedTeam.Beschreibung = this.TextBoxBeschreibung.Text;
                this.selectedTeam.IsDeleted = this.checkBoxDeleted.Checked;

                // Center
                this.selectedTeam.Center = this.centerComboBox.SelectedItem as CenterInfo;
            }
        }

        private void FillCentersList()
        {
            try
            {
                this.centerList = MetaCall.Business.Centers.Centers;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "UI Policy");
                if (rethrow)
                    throw;
            }

            //Center füllen
            centerComboBox.Items.Clear();
            centerComboBox.DisplayMember = "Bezeichnung";
            foreach (CenterInfo center in this.centerList)
            {
                centerComboBox.Items.Add(center);
            }
            centerComboBox.Items.Insert(0, "<keine Auswahl>");
            centerComboBox.SelectedIndex = 0;
        }

        private void centerComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox centerComboBox = sender as ComboBox;

            if (sender != null)
            {
                CenterInfo center = centerComboBox.SelectedItem as CenterInfo;
            }
        }

        private void UserForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.DialogResult == DialogResult.OK)
            {
                SaveToObject();
            }

            Application.Idle -= new EventHandler(this.Application_Idle);
        }

        private ProjectViewInfo GetProjectViewInfoControl(ProjectState projectState, Team team)
        {
            ProjectViewInfo pviView = (ProjectViewInfo)Activator.CreateInstance(typeof(ProjectViewInfo), new Object[] { projectState , team });

            return pviView;
        }

        private void checkBoxActiv_Click(object sender, EventArgs e)
        {
            ProjectState = ProjectState.ReleasedForPhone;

            SetProjectList();
        }

        private void checkBoxCompleted_Click(object sender, EventArgs e)
        {
            ProjectState = ProjectState.Finished;

            SetProjectList();
        }

        private void tabControlUser_Selected(object sender, TabControlEventArgs e)
        {
            if (this.tabControlUser.TabPages[this.tabControlUser.SelectedIndex].Text.ToString() == "Projekte")
            {
                this.groupBoxSetProjectList.Visible = true;
            }
            else
            {
                this.groupBoxSetProjectList.Visible = false;
            }

        }

        private UserInfoView GetUserInfoViewControl()
        {
            UserInfoView uiView;

            uiView = (UserInfoView)Activator.CreateInstance(typeof(UserInfoView), new Object[] { this.selectedTeam });

            return uiView;
        }


        private ReminderViewInfo GetReminderViewInfoControl()
        {
            ReminderViewInfo rviView;

            TeamInfo teamInfo = MetaCall.Business.Teams.GetTeamInfo(this.selectedTeam);

            rviView = (ReminderViewInfo)Activator.CreateInstance(typeof(ReminderViewInfo), new Object[] { teamInfo, this.tabPageReminder.Width, this.tabPageReminder.Height });

            return rviView;
        }

        private void checkBoxDeleted_Click(object sender, EventArgs e)
        {
            if (this.checkBoxDeleted.Checked == true)
            {
                StringBuilder sb = new StringBuilder();
                int reminderCount = rviView.ReminderCount;

                if (reminderCount == 1)
                    sb.Append("Es ist noch eine Team-Wiedervorlagen vorhanden.");
                else if(reminderCount > 1)
                    sb.AppendFormat("Es sind noch {0} Team-Wiedervorlagen vorhanden.", reminderCount);

                int UserCount = uiView.UserInfoViewCount;


                if (UserCount == 1)
                {
                    if (sb.Length > 0)
                        sb.AppendLine();
                    sb.Append("Es ist noch ein Benutzer diesem Team zugeordnet.");
                } 
                else if (UserCount > 1)
                {
                    if (sb.Length > 0)
                        sb.AppendLine();
                    sb.AppendFormat("Es sind noch {0} Benutzer diesem Team zugeordnet.", UserCount);
                }

                if (sb.Length > 0)
                {
                    sb.AppendLine();
                    sb.Append("Sie können das Team nicht deaktivieren!");
                    this.checkBoxDeleted.Checked = false;
                    MessageBox.Show(sb.ToString());
                }
            }
        }
    }
}