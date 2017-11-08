using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using metatop.Applications.metaCall.DataObjects;
using metatop.Applications.metaCall.BusinessLayer;

using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

using metatop.Applications.metaCall.WinForms.Modules.Telefonie;

namespace metatop.Applications.metaCall.WinForms.Modules
{
    public partial class ReminderEdit : Form
    {
        private CallJobReminder selectedCallJobReminder;
        private string reminderAnswer;

        private List<CenterInfo> centerList = new List<CenterInfo>();
        private List<TeamInfo> teamList = new List<TeamInfo>();
        private List<UserInfo> userList = new List<UserInfo>();
        private List<ProjectInfo> projectInfoList = new List<ProjectInfo>();
        private List<Sponsor> sponsorList = new List<Sponsor>();

        private bool newCallJobReminder;
        private MetaCallPrincipal principal;

        public CallJobReminder SelectedCallJobReminder
        {
            get
            {
                return this.selectedCallJobReminder;
            }
        }

        private string ReminderAnswer
        {
            get
            {
                return this.reminderAnswer;
            }

            set
            {
                this.reminderAnswer = value;
                this.lblReminderAnswer.Text = this.reminderAnswer;
            }
        }

        public ReminderEdit()
        {
            InitializeComponent();
        }

        public ReminderEdit(CallJobReminder callJobReminder)
        {
            InitializeComponent();

            //Bei neuem Reminder Formular im New-Modus aufrufen
            if (callJobReminder == null)
            {
                this.newCallJobReminder = true;
                this.selectedCallJobReminder = new CallJobReminder();
                this.selectedCallJobReminder.CallJobReminderId = Guid.NewGuid();
            }
            else
            {
                this.newCallJobReminder = false;
                this.selectedCallJobReminder = callJobReminder;
            }
        }

        private void InitializeEditReminderMode()
        {
            this.comboBoxProject.Enabled = false;
            this.comboBoxSponsor.Enabled = false;

            if (this.principal == null)
                throw new InvalidOperationException("CurrentPrincipal is not a type of metaCallPrincipal");

            this.Text = "Wiedervorlage bearbeiten";

            this.createReminder1.ReminderDateStart = this.selectedCallJobReminder.ReminderDateStart;
            this.createReminder1.ReminderDateStop = this.selectedCallJobReminder.ReminderDateStop;
            if (this.selectedCallJobReminder.User == null)
                this.createReminder1.SetTeamReminder = true;
            else
                this.createReminder1.SetTeamReminder = false;

            this.createReminder1.ReminderTracking = this.selectedCallJobReminder.ReminderTracking;

            if (!this.principal.IsInRole(MetaCallPrincipal.AdminRoleName))
            {
                this.comboBoxAgent.Enabled = true;
                this.comboBoxCenter.Enabled = false;
                this.comboBoxTeam.Enabled = false;
            }
            else
            {
                this.comboBoxAgent.Enabled = true;
                this.comboBoxCenter.Enabled = true;
                this.comboBoxTeam.Enabled = true;
            }

            if (this.selectedCallJobReminder.ReminderState == CallJobReminderState.Finished)
            {
                this.radioButtonStatusFinished.Checked = true;
            }
            else
            {
                this.radioButtonStatusOpen.Checked = true;
            }

        }

        private void InitializeNewReminderMode()
        {

            this.Text = "Wiedervorlage anlegen";

            this.createReminder1.ReminderDateStart = DateTime.Now;
            this.radioButtonStatusOpen.Checked = true;
            
            if (!this.principal.IsInRole(MetaCallPrincipal.AdminRoleName))
            {
                this.comboBoxAgent.Enabled = true;
                this.comboBoxCenter.Enabled = false;
                this.comboBoxTeam.Enabled = false;
            }
            else
            {
                this.comboBoxAgent.Enabled = true;
                this.comboBoxCenter.Enabled = true;
                this.comboBoxTeam.Enabled = true;
            }

            this.comboBoxProject.Enabled = true;
            this.comboBoxSponsor.Enabled = true;
        }

        void Application_Idle(object sender, EventArgs e)
        {
            if (this.createReminder1.IsTeamReminder == true)
            {
                this.saveButton.Enabled = (
                                            this.comboBoxTeam.SelectedItem != null &&
                                            this.comboBoxProject.SelectedItem != null &&
                                            this.comboBoxSponsor.SelectedItem != null
                                           );
            }
            else
            {
                this.saveButton.Enabled = (
                                            this.comboBoxAgent.SelectedItem != null &&
                                            this.comboBoxProject.SelectedItem != null &&
                                            this.comboBoxSponsor.SelectedItem != null
                                           );
            }
        }

        private void FillToEdit(bool centerRequest)
        {

            User user = null;
           
            // Center wählen
            if (this.selectedCallJobReminder != null)
            {
                if (this.selectedCallJobReminder.User != null)
                {
                    user = MetaCall.Business.Users.GetUser(this.selectedCallJobReminder.User);                
                }
                else
                {
                    /*
                    if (!this.principal.IsInRole(MetaCallPrincipal.AdminRoleName))
                    {
                        user = MetaCall.Business.Users.CurrentUser;
                    }
                     */

                }
            }
       
            
            if (user != null)
            {
                foreach (CenterInfo center in this.centerList)
                {
                    if (center.CenterId.Equals(user.Center.CenterId))
                    {
                        this.comboBoxCenter.SelectedItem = center;
                    }
                }
            }
            else
            {
                if (this.selectedCallJobReminder.Team != null)
                {
                    Team team = MetaCall.Business.Teams.GetTeam(this.selectedCallJobReminder.Team);
                    if (team.Center != null)
                    {
                        foreach (CenterInfo center in this.centerList)
                        {
                            if (center.CenterId.Equals(team.Center.CenterId))
                            {
                                this.comboBoxCenter.SelectedItem = center;
                            }
                        }
                    }
                }
            }

            FillTeamsList((CenterInfo)this.comboBoxCenter.SelectedItem);

            if (this.selectedCallJobReminder != null)
            {
                if (this.selectedCallJobReminder.Team != null)
                {
                    foreach (TeamInfo teamInfo in this.teamList)
                    {
                        if (this.selectedCallJobReminder.Team.TeamId.Equals(teamInfo.TeamId))
                        {
                            this.comboBoxTeam.SelectedItem = teamInfo;
                        }
                    }
                }
            }

            FillUserList((TeamInfo)this.comboBoxTeam.SelectedItem);

            if (this.selectedCallJobReminder != null)
            {
                if (this.selectedCallJobReminder.User != null)
                {
                    foreach (UserInfo userItem in this.userList)
                    {
                        if (this.selectedCallJobReminder.User.UserId.Equals(userItem.UserId))
                        {
                            this.comboBoxAgent.SelectedItem = userItem;
                        }
                    }
                }
            }

            FillProjectList((TeamInfo)this.comboBoxTeam.SelectedItem);

            if (this.selectedCallJobReminder != null)
            {
                if (this.selectedCallJobReminder.Project != null)
                {
                    foreach (ProjectInfo projectInfo in this.projectInfoList)
                    {
                        if (selectedCallJobReminder.Project.ProjectId.Equals(projectInfo.ProjectId))
                        {
                            this.comboBoxProject.SelectedItem = projectInfo;
                        }
                    }
                }
            }

            FillSponsorList((ProjectInfo)this.comboBoxProject.SelectedItem);

            if (this.selectedCallJobReminder != null)
            {
                if (this.selectedCallJobReminder.CallJob != null)
                {
                    foreach (Sponsor sponsor in this.sponsorList)
                    {
                        if (selectedCallJobReminder.CallJob.Sponsor.AddressId.Equals(sponsor.AddressId))
                        {
                            this.comboBoxSponsor.SelectedItem = sponsor;
                        }
                    }
                }
            }
        }

        private void SaveToObject()
        {

            if (this.selectedCallJobReminder != null)
            {
                this.selectedCallJobReminder.Project = (ProjectInfo)this.comboBoxProject.SelectedItem;
                if (this.selectedCallJobReminder.CallJob == null)
                {
                    CallJob callJob = MetaCall.Business.CallJobs.Get(((Sponsor)this.comboBoxSponsor.SelectedItem).AddressId, this.selectedCallJobReminder.Project.ProjectId);
                    this.selectedCallJobReminder.CallJob = callJob;
                }
                this.selectedCallJobReminder.Address  =  (AddressBase)this.comboBoxSponsor.SelectedItem;

                if (this.createReminder1.IsTeamReminder == true)
                {
                    this.selectedCallJobReminder.User = null;
                    this.selectedCallJobReminder.Team = (TeamInfo)this.comboBoxTeam.SelectedItem;
                }
                else
                {
                    this.selectedCallJobReminder.User = (UserInfo)this.comboBoxAgent.SelectedItem;
                    this.selectedCallJobReminder.Team = (TeamInfo)this.comboBoxTeam.SelectedItem; 
                }

                this.selectedCallJobReminder.ReminderDateStart = this.createReminder1.ReminderDateStart;
                this.selectedCallJobReminder.ReminderDateStop = this.createReminder1.ReminderDateStop;
                this.selectedCallJobReminder.ReminderTracking = this.createReminder1.ReminderTracking;

                if (this.radioButtonStatusFinished.Checked == true)
                {
                    this.selectedCallJobReminder.ReminderState = CallJobReminderState.Finished;
                }
                else
                {
                    this.selectedCallJobReminder.ReminderState = CallJobReminderState.Open;
                }
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
            comboBoxCenter.Items.Clear();
            comboBoxCenter.DisplayMember = "Bezeichnung";
            foreach (CenterInfo center in this.centerList)
            {
                comboBoxCenter.Items.Add(center);
            }
        }

        private void FillTeamsList(CenterInfo center )
        {
            comboBoxTeam.Items.Clear();
            comboBoxTeam.DisplayMember = "Bezeichnung";

            bool currentReminderTeam = false;

            if (center != null)
            {
                try
                {
                    this.teamList = MetaCall.Business.Teams.GetByCenter(center);
                }
                catch (Exception ex)
                {
                    bool rethrow = ExceptionPolicy.HandleException(ex, "UI Exception");
                    if (rethrow)
                        throw;
                }

                foreach (TeamInfo teamInfo in this.teamList)
                {
                    comboBoxTeam.Items.Add(teamInfo);
                    if (this.newCallJobReminder == false && this.selectedCallJobReminder.Team != null)
                    {
                        if (this.selectedCallJobReminder.Team.TeamId.Equals(teamInfo.TeamId))
                            currentReminderTeam = true;
                    }
                    else
                    {
                        currentReminderTeam = true;
                    }
                }

                if (currentReminderTeam == false)
                {
                    this.teamList.Add(this.selectedCallJobReminder.Team);
                    comboBoxTeam.Items.Add(this.selectedCallJobReminder.Team);
                }
            }
        }

        private void FillUserList(TeamInfo teamInfo)
        {
            comboBoxAgent.Items.Clear();
            comboBoxAgent.DisplayMember = "DisplayName";

            bool currentReminderProject = false;

            List<TeamMitglied> teamMitgliedList = new List<TeamMitglied>();
            try
            {
                if (teamInfo != null)
                {
                    teamMitgliedList = MetaCall.Business.Users.GetUsersByTeam(teamInfo.TeamId);
                }

            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "UI Policy");
                if (rethrow)
                    throw;
            }

            if (teamMitgliedList.Count > 0)
            {
                foreach (TeamMitglied teamMitglied in teamMitgliedList)
                {
                    User user = MetaCall.Business.Users.GetUser(teamMitglied.UserId);
                    UserInfo userInfo = MetaCall.Business.Users.GetUserInfo(user);
                    userList.Add(userInfo);
                    comboBoxAgent.Items.Add(userInfo);
                    if (this.newCallJobReminder == false && selectedCallJobReminder.User != null)
                    {
                        if (selectedCallJobReminder.User.UserId.Equals(teamMitglied.UserId))
                            currentReminderProject = true;
                    }
                    else
                    {
                        currentReminderProject = true;
                    }
                }

                if (currentReminderProject == false)
                {
                    User user = MetaCall.Business.Users.GetUser(this.selectedCallJobReminder.User.UserId);
                    UserInfo userInfo = MetaCall.Business.Users.GetUserInfo(user);
                    this.userList.Add(userInfo);
                    comboBoxAgent.Items.Add(userInfo);
                }
            }
            else
            {
                if (this.selectedCallJobReminder.User != null)
                {
                    User user = MetaCall.Business.Users.GetUser(this.selectedCallJobReminder.User.UserId);
                    UserInfo userInfo = MetaCall.Business.Users.GetUserInfo(user);
                    this.userList.Add(userInfo);
                    comboBoxAgent.Items.Add(userInfo);
                }
            }
        }

        private void FillProjectList(TeamInfo teamInfo)
        {
            comboBoxProject.Items.Clear();
            comboBoxProject.DisplayMember = "Bezeichnung";

            bool currentReminderProject = false;

            try
            {
                if (teamInfo != null)
                {
                    projectInfoList = MetaCall.Business.Projects.GetByTeam(teamInfo);
                    //projectInfoList.Sort();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "UI Policy");
                if (rethrow)
                    throw;
            }

            if (this.projectInfoList.Count > 0)
            {
                foreach (ProjectInfo projectInfo in this.projectInfoList)
                {
                    comboBoxProject.Items.Add(projectInfo);
                    if (this.newCallJobReminder == false)
                    {
                        if (selectedCallJobReminder.Project.ProjectId.Equals(projectInfo.ProjectId))
                            currentReminderProject = true;
                    }
                    else
                    {
                        currentReminderProject = true;
                    }
                }

                if (currentReminderProject == false)
                {

                    this.projectInfoList.Add(this.selectedCallJobReminder.Project);
                    comboBoxProject.Items.Add(this.selectedCallJobReminder.Project);
                }
            }
            else
            {
                if (this.selectedCallJobReminder.Project != null)
                {
                    this.projectInfoList.Add(this.selectedCallJobReminder.Project);
                    comboBoxProject.Items.Add(this.selectedCallJobReminder.Project);
                }
            }
        }

        private void FillSponsorList(ProjectInfo projectInfo)
        {
            comboBoxSponsor.Items.Clear();
            comboBoxSponsor.DisplayMember = "DisplayName";

            bool currentReminderProject = false;

            try
            {
                if (projectInfo != null && this.newCallJobReminder == true)
                sponsorList = MetaCall.Business.Addresses.GetSponsorsByProject(MetaCall.Business.Projects.Get(projectInfo));
                
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "UI Policy");
                if (rethrow)
                    throw;
            }

            if (this.selectedCallJobReminder != null)
            {
                foreach (Sponsor sponsor in this.sponsorList)
                {
                    comboBoxSponsor.Items.Add(sponsor);
                    if (this.selectedCallJobReminder.CallJob != null)
                    {
                        if (this.selectedCallJobReminder.CallJob.Sponsor.AddressId.Equals(sponsor.AddressId))
                            currentReminderProject = true;
                    }
                    else
                    {
                       // currentReminderProject = true;
                    }
                }

                if (currentReminderProject == false && this.selectedCallJobReminder.CallJob != null)
                {
                    comboBoxSponsor.Items.Add(this.selectedCallJobReminder.CallJob.Sponsor);
                    this.sponsorList.Add(this.selectedCallJobReminder.CallJob.Sponsor);
                }
            }
        }

        private void centerComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox centerComboBox = sender as ComboBox;

            if (sender != null)
            {
                
                FillToEdit(true);
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

        private void comboBoxTeam_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox centerComboBox = sender as ComboBox;

            if (sender != null)
            {
                TeamInfo teamInfo = comboBoxTeam.SelectedItem as TeamInfo;
                if (this.selectedCallJobReminder.Team != null)
                {
                    if (!this.selectedCallJobReminder.Team.TeamId.Equals(teamInfo.TeamId))
                    {
                        this.selectedCallJobReminder.Team = teamInfo;
                        FillToEdit(true);
                    }
                }
                else
                {
                    this.selectedCallJobReminder.Team = teamInfo;
                    FillToEdit(true);
                }

            }
        }

        private void comboBoxAgent_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox agentComboBox = sender as ComboBox;

            if (sender != null)
            {
                UserInfo userInfo = comboBoxAgent.SelectedItem as UserInfo;
                if (this.selectedCallJobReminder.User != null)
                {
                    if (!this.selectedCallJobReminder.User.UserId.Equals(userInfo.UserId))
                    {
                        this.selectedCallJobReminder.User = userInfo;
                        FillToEdit(true);
                    }
                }
                else
                {
                    this.selectedCallJobReminder.User = userInfo;
                    //this.createReminder1.ReminderUser = userInfo;
                    FillToEdit(true);
                }
            }
        }

        private void createReminder1_ValueChanged(object sender, CreateReminderEventArgs e)
        {
            StringBuilder sb = new StringBuilder();

            switch (e.CallJobReminderTracking)
            {
                case CallJobReminderTracking.ExactDateAndTime:
                    if (e.IsTeamReminder)
                    {
                        sb.AppendFormat("Der Sponsor wird Ihrem Team am {0} um {1} Uhr als Wiedervorlage angeboten",
                            e.Start.ToShortDateString(),
                            e.Start.ToShortTimeString());

                    }
                    else
                    {
                        sb.AppendFormat("Der Sponsor wird Ihnen am {0} um {1} als Wiedervorlage angeboten",
                            e.Start.ToShortDateString(),
                            e.Start.ToShortTimeString());
                    }
                    break;
                case CallJobReminderTracking.Day:
                    break;
                case CallJobReminderTracking.Week:
                    break;
                case CallJobReminderTracking.WeekDay:
                    break;
                case CallJobReminderTracking.OnlyTimeSpan:
                    if (e.IsTeamReminder)
                    {
                        sb.AppendFormat("Der Sponsor wird Ihrem Team täglich zwischen {0} und {1} Uhr als Wiedervorlage angeboten",
                            e.Start.ToShortTimeString(),
                            e.Stop.ToShortTimeString());

                    }
                    else
                    {
                        sb.AppendFormat("Der Sponsor wird Ihnen täglich zwischen {0} und {1} Uhr als Wiedervorlage angeboten",
                                                    e.Start.ToShortTimeString(),
                                                    e.Stop.ToShortTimeString());
                    }
                    break;
                case CallJobReminderTracking.DateAndTimeSpan:
                    break;
                default:
                    break;
            }

            if (sb.Length > 0)
            {
                this.ReminderAnswer = sb.ToString();
            }
            else
            {
                this.ReminderAnswer = string.Empty;
            }
        }

        private void comboBoxProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox projectComboBox = sender as ComboBox;

            if (sender != null)
            {
                ProjectInfo projectInfo = comboBoxProject.SelectedItem as ProjectInfo;
                if (this.selectedCallJobReminder.Project != null)
                {
                    if (!this.selectedCallJobReminder.Project.ProjectId.Equals(projectInfo.ProjectId))
                    {
                        this.selectedCallJobReminder.Project = projectInfo;
                        FillToEdit(true);
                    }
                }
                else
                {
                    this.selectedCallJobReminder.Project = projectInfo;
                    FillToEdit(true);
                }
            }
        }

        private void ReminderEdit_Load(object sender, EventArgs e)
        {
            this.principal = System.Threading.Thread.CurrentPrincipal as MetaCallPrincipal;

            //Bei neuem Reminder Formular im New-Modus aufrufen
            if (this.newCallJobReminder == true)
            {
                InitializeNewReminderMode();
            }
            else
            {
                InitializeEditReminderMode();
            }

            FillCentersList();
            FillToEdit(false);

            Application.Idle += new EventHandler(Application_Idle);
        }

    }
}