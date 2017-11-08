using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using MaDaNet.Common.AppFrameWork.ApplicationModul;
using metatop.Applications.metaCall.BusinessLayer;
using metatop.Applications.metaCall.DataObjects;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Globalization;

namespace metatop.Applications.metaCall.WinForms.Modules
{
    public partial class UserInfoView : UserControl
    {
        private DataTable dtUsers = new DataTable();
        private Center center;
        private Team team;
        private UserTyp userTyp;

        private UserTyp UserTypInfo
        {
            set { this.userTyp = value; }
            get { return this.userTyp; }
        }

        public int UserInfoViewCount
        {
            get
            {
                return this.bindingSource1.Count; ;
            }
        }

        public UserInfoView()
        {
            InitializeComponent();
        }

        public UserInfoView(Center center): this()
        {
            this.center = center;
            UserTypInfo = UserTyp.Center;
            dtUsers.Locale = CultureInfo.CurrentUICulture;
            bindingSource1.DataSource = dtUsers;

            SetupDataTable();
        }

        public UserInfoView(Team team): this()
        {
            this.team = team;
            UserTypInfo = UserTyp.Team;
            dtUsers.Locale = CultureInfo.CurrentUICulture;
            bindingSource1.DataSource = dtUsers;

            SetupDataTable();

        }

        public UserInfoView(UserTyp usertyp): this()
        {
            UserTypInfo = usertyp;
            dtUsers.Locale = CultureInfo.CurrentUICulture;
            bindingSource1.DataSource = dtUsers;

            SetupDataTable();

        }

        public ToolStripMenuItem[] CreateMainMenuItems()
        {
            return null;
        }


        private void UserInfoView_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;
        }

        private void SetupDataTable()
        {
            MetaCallPrincipal principal = System.Threading.Thread.CurrentPrincipal as MetaCallPrincipal;

            DataTableHelper.AddColumn(this.dtUsers, "Partnernummer", "Partnernummer", typeof(string));
            DataTableHelper.AddColumn(this.dtUsers, "Vorname", "Vorname", typeof(string));
            DataTableHelper.AddColumn(this.dtUsers, "Nachname", "Nachname", typeof(string));
            DataTableHelper.AddColumn(this.dtUsers, "Center", "Center", typeof(string));
            DataTableHelper.AddColumn(this.dtUsers, "Teams", "Teams", typeof(string));
            
            if (principal.IsInRole(MetaCallPrincipal.AdminRoleName))
                DataTableHelper.AddColumn(this.dtUsers, "Roles", "Gruppen", typeof(string));
            else
                DataTableHelper.AddColumn(this.dtUsers, "Roles", "Gruppen", typeof(string), MappingType.Hidden);

            DataTableHelper.AddColumn(this.dtUsers, "User", string.Empty, typeof(User), MappingType.Hidden);
            DataTableHelper.FillGridView(this.dtUsers, this.dataGridViewUsers);

            this.dataGridViewUsers.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewUsers.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewUsers.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewUsers.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewUsers.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewUsers.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            LoadUsersIntoDataTable();
        }

        private void LoadUsersIntoDataTable()
        {
            List<UserInfo> users = new List<UserInfo>();

            if (UserTypInfo == UserTyp.Center)
            {
                users = MetaCall.Business.Centers.GetUsers(this.center);
            }
            else if (UserTypInfo == UserTyp.Team)
            {
                users = MetaCall.Business.Teams.GetUsers(this.team);
            }
            else if (UserTypInfo == UserTyp.WithoutCenter)
            {
                users = MetaCall.Business.Users.UsersWithOutCenter;
            }
            else if (UserTypInfo == UserTyp.Deleted)
            {
                users = MetaCall.Business.Users.UsersDeleted;
            }
            bindingSource1.SuspendBinding();
            try
            {
                dtUsers.Rows.Clear();

                foreach (UserInfo userInfo in users)
                {
                    User user = MetaCall.Business.Users.GetUser(userInfo);

                    object[] objectData = new object[]
                        {
                        user.mwUser == null ? "<unbekannt>" : user.mwUser.PartnerNummer.ToString(),
                        user.Vorname,
                        user.Nachname,
                        user.Center == null ? "<unbekannt>" : user.Center.Bezeichnung,
                        GetTeamsString(user.Teams), 
                        GetRolesString(user.SecurityGroups),
                        user
                        };

                    dtUsers.Rows.Add(objectData);
                }
            }
            finally
            {
                bindingSource1.ResumeBinding();
            }
        }

        private string GetRolesString(SecurityGroup[] securityGroup)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < securityGroup.Length; i++)
            {
                if (sb.Length > 0) sb.Append("; ");
                sb.Append(securityGroup[i].DisplayName);
            }

            return sb.ToString();
        }

        private string GetTeamsString(TeamAssignInfo[] teamInfo)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < teamInfo.Length; i++)
            {
                if (sb.Length > 0) sb.Append("; ");
                sb.Append(teamInfo[i].Team.Bezeichnung);
            }

            return sb.ToString();
        }

        private void Reload()
        {
            LoadUsersIntoDataTable();
        }

        private void UserNew()
        {
            User newUser = new User();
            newUser.UserId = Guid.NewGuid();
            newUser.SecurityGroups = new SecurityGroup[0];
            newUser.Teams = new TeamAssignInfo[0];

            using (UserForm dlg = new UserForm(newUser))
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string password = AskForPassword();
                        if (!string.IsNullOrEmpty(password))
                        {
                            MetaCall.Business.Users.Create(newUser, password);
                        }
                        else
                        {
                            MessageBox.Show("Der Benutzer konnte nicht erstellt werden, da Sie kein Passwort eingegeben haben!");
                        }
                    }
                    catch (Exception ex)
                    {
                        bool rethrow = ExceptionPolicy.HandleException(ex, "UI Policy");
                        if (rethrow)
                            throw;
                    }
                }
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

        private void UserUpdate()
        {
            if (this.SelectedUser != null)
            {
                using (UserForm dlg = new UserForm(SelectedUser))
                {
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            MetaCall.Business.Users.Update(SelectedUser);
                        }
                        catch (Exception ex)
                        {
                            bool rethrow = ExceptionPolicy.HandleException(ex, "UI Policy");
                            if (rethrow)
                                throw;
                        }
                    }
                }
            }
        }

        private void UserDelete()
        {
            if (this.SelectedUser != null)
            {
                MessageBoxOptions options = CultureInfo.CurrentCulture.TextInfo.IsRightToLeft ?
                    MessageBoxOptions.RtlReading : 0;

                if (this.SelectedUser.UserId.Equals(MetaCall.Business.Users.CurrentUser.UserId))
                {
                    MessageBox.Show("Der aktuell angemeldete Benutzer kann nicht gelöscht werden!");
                    return;
                }

                string msg = string.Format("Möchten Sie den Benutzer {0} wirklich löschen?", SelectedUser.DisplayName);

                if (MessageBox.Show(msg, Application.ProductName, MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question, MessageBoxDefaultButton.Button2, options) == DialogResult.Yes)
                {
                    try
                    {
                        MetaCall.Business.Users.Delete(this.SelectedUser);
                    }
                    catch (Exception ex)
                    {
                        bool rethrow = ExceptionPolicy.HandleException(ex, "UI Policy");
                        if (rethrow)
                            throw;
                    }
                }
            }
        }

        private void editToolStripButton_Click(object sender, EventArgs e)
        {
            UserUpdate();
            Reload();
        }

        public User SelectedUser
        {
            get
            {
                if (dataGridViewUsers.CurrentRow == null)
                    return null;

                DataRowView currentRowView =
                    (DataRowView)dataGridViewUsers.CurrentRow.DataBoundItem;

                if (currentRowView == null || currentRowView.Row == null)
                    return null;

                return (User)
                    currentRowView.Row.ItemArray[
                    currentRowView.Row.ItemArray.Length - 1];
            }
        }

        private void newToolStripButton_Click(object sender, EventArgs e)
        {
            UserNew();
            Reload();
        }

        private void deleteToolStripButton_Click(object sender, EventArgs e)
        {
            UserDelete();
            Reload();
        }

        private void dataGridViewUsers_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                this.dataGridViewUsers.Rows[e.RowIndex].Selected = true;
                UserUpdate();
            }
        }
    }
}
