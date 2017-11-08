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
    public partial class UserForm : Form
    {
        private User selectedUser;
        private List<CenterInfo> centerList = new List<CenterInfo>();
        private List<TeamInfo> teamList = new List<TeamInfo>();
        private List<SecurityGroup> rolesList;
        private List<mwUser> mwUserList;

        private ProjectState projectState = new  ProjectState();

        ReminderViewInfo rviView;

        public UserForm()
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

        public UserForm(User user): this()
        {

            this.selectedUser = user;

            FillCentersList();
            FillSecurityGroupsList();
            FillDialModes();

            Application.Idle += new EventHandler(Application_Idle);

            FillToEdit();

            //Neuen Benutzer identifizieren und das Formular
            // entsprechend anpassen
            if (user.SecurityGroups.Length == 0)
                InitializeNewUserMode();

            checkBoxActiv_Click(null,null);

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

            tabControlUser_Selected(null, null);
        }

        private void SetProjectList()
        {
            this.tabPageProject.SuspendLayout();
            this.tabPageProject.Controls.Clear();

            if (ProjectState != ProjectState.Unknown9)
            {
                ProjectViewInfo pviView = GetProjectViewInfoControl(ProjectState, this.selectedUser);

                pviView.Visible = true;
                pviView.Dock = DockStyle.Fill;
                this.tabPageProject.Controls.Add(pviView);
            }

            this.tabPageProject.ResumeLayout();
        }

        private void InitializeNewUserMode()
        {
            FillMwUserList();
            this.mwUserComboBox.Visible = true;
        }

        private void FillDialModes()
        {
            this.dialModeComboBox.Items.Clear();
            //foreach (string dialMode in Enum.GetNames(typeof(DialMode)))
            DialModeDescription dialModeDescription = new DialModeDescription();
            foreach (DialMode dialMode in Enum.GetValues(typeof(DialMode)))
            {
                if (dialMode != DialMode.Unseeded)
                    this.dialModeComboBox.Items.Add(dialModeDescription.TranslateToDescription(dialMode));
            }
        }

        void Application_Idle(object sender, EventArgs e)
        {
            this.saveButton.Enabled = (this.nachNameTextBox.Text.Length > 0) &&
                (this.userNameTextBox.Text.Length > 0) &&
                (this.rolesCheckedListBox.CheckedItems.Count > 0);
        }

        private DialMode GetCorrectDialModeRange(DialMode dialMode)
        {
            if ((int)dialMode >= 0 && (int)dialMode <= 2)
                return dialMode;
            else
                return DialMode.ManualDialing;
        }

        private void FillToEdit()
        {
            if (this.selectedUser != null)
            {
                this.userNameTextBox.Text = this.selectedUser.UserName;
                this.vornameTextBox.Text = this.selectedUser.Vorname;
                this.nachNameTextBox.Text = this.selectedUser.Nachname;
                this.checkBoxProjectSearchPermit.Checked = this.selectedUser.ProjectSearchPermit;
                this.checkBoxReminderEditPermit.Checked = this.selectedUser.ReminderEditPermit;
                this.checkBoxWorkingTimeEditPermit.Checked = this.selectedUser.WorkingTimeEditPermit;
                this.checkBoxDunning.Checked = this.selectedUser.Dunning;
                this.checkBoxDeleted.Checked = this.selectedUser.IsDeleted;
                this.dialModeComboBox.SelectedIndex = (int)GetCorrectDialModeRange(this.selectedUser.DialMode);
                this.anmeldungEmailTextBox.Text = this.selectedUser.AnmeldungEmail;
                this.additionalInfo1TextBox.Text = this.selectedUser.AdditionalInfo1;
                this.additionalInfo2TextBox.Text = this.selectedUser.AdditionalInfo2;
                if (this.selectedUser.mwUser != null)
                {
                    this.partnerNummerTextBox.Text = this.selectedUser.mwUser.PartnerNummer.ToString ();
                }

                // Center wählen
                if (this.selectedUser.Center != null)
                {
                    foreach (CenterInfo center in this.centerList)
                    {
                        if (center.CenterId.Equals(this.selectedUser.Center.CenterId))
                        {
                            this.centerComboBox.SelectedItem = center;
                        }
                    }
                }

                //Team wählen
                if (this.selectedUser.Teams.Length > 0)
                {
                    TeamAssignInfo selectedTeam = this.selectedUser.Teams[0];
                    foreach (TeamInfo  teamInfo in this.teamList)
                    {
                        if (selectedTeam.Team.TeamId.Equals(teamInfo.TeamId))
                        {
                            this.teamComboBox.SelectedItem = teamInfo;
                            this.isTeamleiterCheckBox.Checked = selectedTeam.IsTeamLeiter;
                        }
                    }
                }

                //SecurityGroups vorselektieren
                foreach (SecurityGroup secGroup in this.rolesList)
                {
                    foreach (SecurityGroup selectedSecurityGroup in this.selectedUser.SecurityGroups)
                    {
                        if (secGroup.SecurityGroupId.Equals(selectedSecurityGroup.SecurityGroupId))
                        {
                            this.rolesCheckedListBox.SetItemChecked(
                                this.rolesCheckedListBox.Items.IndexOf(secGroup), true);
                        }
                    }
                    
                }

                //Aktivieren/ Deaktivieren der Steuerelemente für metaware/metacallUser
                this.vornameTextBox.Enabled = !this.selectedUser.IsMetaWareUser;
                this.nachNameTextBox.Enabled = !this.selectedUser.IsMetaWareUser;
                //Partnernummer ist entweder nicht vergeben oder ist schreibgeschützt
                this.partnerNummerTextBox.Enabled = false;

                UserSignature sig = MetaCall.Business.Users.GetSignature(this.selectedUser);
                if (sig != null)
                {
                    this.signatureFileTextBox.Text = sig.FileName;
                }
            }
        }

        private void SaveToObject()
        {

            if (this.selectedUser != null)
            {
                this.selectedUser.UserName = this.userNameTextBox.Text;
                this.selectedUser.Vorname = this.vornameTextBox.Text;
                this.selectedUser.Nachname = this.nachNameTextBox.Text;
                this.selectedUser.ReminderEditPermit = this.checkBoxReminderEditPermit.Checked;
                this.selectedUser.ProjectSearchPermit = this.checkBoxProjectSearchPermit.Checked;
                this.selectedUser.WorkingTimeEditPermit = this.checkBoxWorkingTimeEditPermit.Checked;
                this.selectedUser.Dunning = this.checkBoxDunning.Checked;
                this.selectedUser.IsDeleted = this.checkBoxDeleted.Checked;
                this.selectedUser.AnmeldungEmail = this.anmeldungEmailTextBox.Text;
                this.selectedUser.AdditionalInfo1 = this.additionalInfo1TextBox.Text;
                this.selectedUser.AdditionalInfo2 = this.additionalInfo2TextBox.Text;

                // Center
                this.selectedUser.Center = this.centerComboBox.SelectedItem as CenterInfo;

                //Team-Zuordnungen
                if (this.teamComboBox.SelectedIndex > 0)
                {
                    TeamAssignInfo assignInfo = new TeamAssignInfo();
                    assignInfo.Team = this.teamComboBox.SelectedItem as TeamInfo;
                    assignInfo.IsTeamLeiter = this.isTeamleiterCheckBox.Checked;
                    this.selectedUser.Teams = new TeamAssignInfo[1];
                    this.selectedUser.Teams[0] = assignInfo;
                }
                else
                {
                    this.selectedUser.Teams = new TeamAssignInfo[0];
                }

                //Berechtigungsgruppen
                this.selectedUser.SecurityGroups = new SecurityGroup[rolesCheckedListBox.CheckedItems.Count];
                for (int i = 0; i < rolesCheckedListBox.CheckedItems.Count; i++)
                {
                    this.selectedUser.SecurityGroups[i] =
                        (SecurityGroup) this.rolesCheckedListBox.CheckedItems[i];
                }

                string filename = this.signatureFileTextBox.Text;
                MetaCall.Business.Users.SetSignature(this.selectedUser, filename);
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

        private void FillTeamsList(CenterInfo center )
        {
            teamComboBox.Items.Clear();
            teamComboBox.DisplayMember = "Bezeichnung";

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
                    teamComboBox.Items.Add(teamInfo);
                }
            }
            teamComboBox.Items.Insert(0, "<keine Auswahl>");
            teamComboBox.SelectedIndex = 0;
        }

        private void FillSecurityGroupsList()
        {
            rolesCheckedListBox.Items.Clear();

            ((ListBox) rolesCheckedListBox).DisplayMember = "Bezeichnung";

            try
            {
                this.rolesList = MetaCall.Business.SecurityGroups.GetAllGroups();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "UI Policy");
                if (rethrow)
                    throw;
            }

            foreach (SecurityGroup role in rolesList)
            {
                rolesCheckedListBox.Items.Add(role);
            }
        }

        private void FillMwUserList()
        {
            mwUserComboBox.Items.Clear();

            try
            {
                mwUserList = MetaCall.Business.Users.GetAllMetaWareUsers();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "UI Policy");
                if (rethrow)
                    throw;
            }

            foreach (mwUser metawareUser in this.mwUserList)
            {
                mwUserComboBox.Items.Add(metawareUser);
            }

            mwUserComboBox.Items.Insert(0, "<kein metware-Benutzer>");
            mwUserComboBox.SelectedIndex = 0;
        }

        private void centerComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox centerComboBox = sender as ComboBox;

            if (sender != null)
            {
                CenterInfo center = centerComboBox.SelectedItem as CenterInfo;
                FillTeamsList(center);
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

        private void mwUserComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

            mwUser metawareUser = this.mwUserComboBox.SelectedItem as mwUser;

            if (metawareUser != null) 
            {
                this.vornameTextBox.Enabled = true;
                this.nachNameTextBox.Enabled = true;

                this.selectedUser.UserId = metawareUser.MemberId;
                this.userNameTextBox.Text = metawareUser.MemberName;
                this.vornameTextBox.Text = metawareUser.Vorname;
                this.nachNameTextBox.Text = metawareUser.Nachname;

                //Auswählen des Centers
                foreach (CenterInfo center in this.centerList)
                {
                    if (center.mwCenterNummer.HasValue &&
                        metawareUser.MwCenter != null &&
                        center.mwCenterNummer.Equals(metawareUser.MwCenter.CenterNummer))
                    {
                        this.centerComboBox.SelectedItem = center;
                    }
                }
            }
            else
            {
                this.vornameTextBox.Enabled = false;
                this.nachNameTextBox.Enabled = false;
            }
        }

        private void selectSignatureFileButton_Click(object sender, EventArgs e)
        {
            this.openFileDialog1.CheckFileExists = true;
            this.openFileDialog1.CheckPathExists = true;
            this.openFileDialog1.DereferenceLinks = true;
            this.openFileDialog1.Multiselect = false;
            this.openFileDialog1.Filter = "Bilddateien (*.bmp, *.jpg, *.tif, *gif)|*.bmp; *.jpg; *.tif; *gif";

            if (this.openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                string filename = this.openFileDialog1.FileName;
                try
                {
                    //Prüfen ob eine Bilddatei gewählt wurde
                    try
                    {
                        Image image = Image.FromFile(filename);
                    }
                    catch
                    {
                        MessageBox.Show("Sie haben keine gültige Bilddatei gewählt");
                        return;
                    }

                    string driveLetter = System.IO.Path.GetPathRoot(filename);

                    if (driveLetter.ToCharArray()[1] != System.IO.Path.VolumeSeparatorChar)
                    {
                        this.signatureFileTextBox.Text = filename;
                    }
                    else
                    {
                        this.signatureFileTextBox.Text = NetServices.GetUniversalName(filename);
                    }
                }
                catch(Win32Exception ex)
                {
                    MessageBox.Show("Sie müssen ein Netzlaufwerk wählen");
                }
            }
        }


        private ProjectViewInfo GetProjectViewInfoControl(ProjectState projectState, User user)
        {
            ProjectViewInfo pviView = (ProjectViewInfo)Activator.CreateInstance(typeof(ProjectViewInfo), new Object[] { projectState , user });

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

        private ReminderViewInfo GetReminderViewInfoControl()
        {
            ReminderViewInfo rviView;

            UserInfo userInfo = MetaCall.Business.Users.GetUserInfo(this.selectedUser);

            rviView = (ReminderViewInfo)Activator.CreateInstance(typeof(ReminderViewInfo), new Object[] { userInfo, null, this.tabPageReminder.Width, this.tabPageReminder.Height });

            return rviView;
        }

        private void checkBoxDeleted_Click(object sender, EventArgs e)
        {
            if (this.checkBoxDeleted.Checked == true)
            {
                int reminderCount = rviView.ReminderCount;
                if (reminderCount > 0)
                {
                    this.checkBoxDeleted.Checked = false;
                    StringBuilder sb = new StringBuilder();
                    if (reminderCount == 1)
                        sb.Append("Es ist noch eine persönliche Wiedervorlagen vorhanden. Sie können den Benutzer nicht löschen!");
                    else
                        sb.AppendFormat("Es sind noch {0} persönliche Wiedervorlagen vorhanden. Sie können den Benutzer nicht löschen!", reminderCount);

                    MessageBox.Show(sb.ToString());
                }
            }
        }

        private void buttonSetPassWord_Click(object sender, EventArgs e)
        {
            string password = AskForPassword();
            if (!string.IsNullOrEmpty(password))
            {
                MetaCall.Business.Users.ChangePassword(this.selectedUser, null, password);
            }
        }

        private string AskForPassword()
        {
            using (EnterPasswordForNewUser dlg = new EnterPasswordForNewUser())
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    return dlg.Password;
                }
            }
            return null;
        }

        private void dialModeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.selectedUser.DialMode = (DialMode)this.dialModeComboBox.SelectedIndex;
        }

        private void passwordEmailButton_Click(object sender, EventArgs e)
        {
            string password = AskForPassword();
            if (!string.IsNullOrEmpty(password))
            {
                this.selectedUser.PasswordEmail = MetaCall.Business.EncryptionBusiness.EncryptString(password);
            }
        }

   
    }
}