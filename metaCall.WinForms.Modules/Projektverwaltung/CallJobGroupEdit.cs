using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using metatop.Applications.metaCall.DataObjects;

namespace metatop.Applications.metaCall.WinForms.Modules
{
    [ToolboxItem(false)]
    public partial class CallJobGroupEdit : Form
    {
        private bool modeAllGroups;
        private CallJobGroup callJobGroup;
        private List<CallJobGroup> currentCallJobGroups; 
        
        public CallJobGroupEdit()
        {
            InitializeComponent();
        }

        public  CallJobGroupEdit(List<CallJobGroup> currentCallJobGroups, List<TeamInfo> availableTeams ):this()
        {
            modeAllGroups = true;
            this.callJobGroup = null;
            this.currentCallJobGroups = currentCallJobGroups;
            FillAvailableTeamsAndUsers(availableTeams);
            FillControlsWithoutCallJobGroups();
 
            Application.Idle += new EventHandler(Application_Idle);
        }

        public CallJobGroupEdit(CallJobGroup callJobGroup, List<TeamInfo> availableTeams):this()
        {

            modeAllGroups = false;
            this.callJobGroup = callJobGroup;
            this.currentCallJobGroups = null;
            FillAvailableTeamsAndUsers(availableTeams);
            FillControls();

            Application.Idle += new EventHandler(Application_Idle);
        }

        void Application_Idle(object sender, EventArgs e)
        {
            UpdateUI();
        }

        private void UpdateUI()
        {
            if (this.callJobGroup == null)
                return;

            if (this.callJobGroup.Type != CallJobGroupType.ManualList)
            {
                //this.displayNameTextBox.ReadOnly = true;
                this.descriptionTextBox.ReadOnly = true;
            }


            this.okButton.Enabled = this.displayNameTextBox.Text.Length > 0 &&
                this.descriptionTextBox.Text.Length > 0;

        }

        private void FillControlsWithoutCallJobGroups()
        {
            this.Text = "Einstellungen für alle Anrufgruppen";
            this.descriptionTextBox.Text = "Die Freigaben werden für alle Anrufgruppen gesetzt.";
            this.descriptionTextBox.Enabled = false;
            this.displayNameTextBox.Enabled = false;
        }

        private void FillControls()
        {
            this.descriptionTextBox.Enabled = true;
            this.displayNameTextBox.Enabled = true;

            CallJobGroupTypeInfo typeInfo = MetaCall.Business.CallJobGroups.GetCallJobGroupTypeInfo(callJobGroup.Type);

            if (!string.IsNullOrEmpty(this.callJobGroup.DisplayName))
            {
                this.Text = string.Format("Anrufgruppe {0} bearbeiten", this.callJobGroup.DisplayName);
            }
            else
            {
                this.Text = "neue Anrufgruppe erstellen";
             }


            this.displayNameTextBox.Text = callJobGroup.DisplayName;
            this.descriptionTextBox.Text = callJobGroup.Description;
            this.typeTextBox.Text = typeInfo.DisplayName;

            //Durchlaufen der Teams und vorbelegen der CheckBoxes
            foreach (TeamInfoTreeNode teamInfoTreeNode in this.teamUserTreeView.Nodes)
            {
                if (Array.Exists<TeamInfo>(this.callJobGroup.Teams, new Predicate<TeamInfo>(
                   delegate(TeamInfo teamInfo)
                   {
                       return teamInfo.TeamId.Equals(teamInfoTreeNode.TeamInfo.TeamId);
                   })))
                {
                    teamInfoTreeNode.Checked = true;

                    foreach (TreeNode treeNode in teamInfoTreeNode.Nodes)
                    {
                        treeNode.Checked = true;
                    }
                }
                else
                {
                    foreach (UserInfoTreeNode userInfoTreeNode in teamInfoTreeNode.Nodes)
                    {
                        if (Array.Exists<UserInfo>(this.callJobGroup.Users, new Predicate<UserInfo>(
                            delegate(UserInfo userInfo){
                                return userInfo.UserId.Equals(userInfoTreeNode.UserInfo.UserId);
                            })))
                        {
                            userInfoTreeNode.Checked = true;
                        }
                    }
                }
            }
        }

        private void FillAvailableTeamsAndUsers(List<TeamInfo> teams)
        {
            foreach (TeamInfo teamInfo in teams)
            {
                TreeNode node = new TeamInfoTreeNode(teamInfo);

                node.Nodes.AddRange(GetUserTreeNodes(teamInfo));


                this.teamUserTreeView.Nodes.Add(node);

            }
            //Alle strukturknoten erweitern
            this.teamUserTreeView.ExpandAll();
        }

        private TreeNode[] GetUserTreeNodes(TeamInfo teamInfo)
        {
            List<UserInfo> users = MetaCall.Business.Teams.GetUsers(teamInfo);
            TreeNode[] nodes = new TreeNode[users.Count];


            for (int i = 0; i < users.Count; i++)
            {
                nodes[i] = new UserInfoTreeNode(users[i]);
            }

            return nodes;
        }

        private class TeamInfoTreeNode : TreeNode
        {
            private TeamInfo teamInfo;
            public TeamInfo TeamInfo
            {
                get { return teamInfo; }
            }


            public TeamInfoTreeNode(TeamInfo teamInfo)
            {
                if (teamInfo == null)
                    throw new ArgumentNullException("teamInfo");
                
                
                this.teamInfo = teamInfo;

                this.Text = teamInfo.Bezeichnung;
                this.ToolTipText = teamInfo.Beschreibung;
            }


        }

        private class UserInfoTreeNode : TreeNode
        {
            private UserInfo userInfo;
            public UserInfo UserInfo
            {
                get { return userInfo; }
            }


            public UserInfoTreeNode(UserInfo userInfo)
            {
                if (userInfo == null)
                    throw new ArgumentNullException("userInfo");

                this.userInfo = userInfo;
                this.Text = userInfo.DisplayName;
                this.ToolTipText = userInfo.DisplayName;
            }
        }

        private void teamUserTreeView_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Node is TeamInfoTreeNode)
            {
                if (e.Node.Checked)
                {
                    foreach (TreeNode treeNode in e.Node.Nodes)
                    {
                        treeNode.Checked = true;
                    }
                }
            }
            else if (e.Node is UserInfoTreeNode)
            {
                if (!e.Node.Checked)
                    e.Node.Parent.Checked = false;
            }
        }

        private void CallJobGroupEdit_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (DialogResult == DialogResult.OK)
            {
                SaveToObject();
            }

            Application.Idle -= new EventHandler(this.Application_Idle);
        }

        private void SaveToObject()
        {
            if (!modeAllGroups)
            {
                if (this.callJobGroup == null)
                    return;

                this.callJobGroup.DisplayName = this.displayNameTextBox.Text;
                this.callJobGroup.Description = this.descriptionTextBox.Text;

                List<TeamInfo> teams = new List<TeamInfo>();
                List<UserInfo> users = new List<UserInfo>();

                foreach (TeamInfoTreeNode teamInfoTreeNode in this.teamUserTreeView.Nodes)
                {
                    if (teamInfoTreeNode.Checked)
                        teams.Add(teamInfoTreeNode.TeamInfo);

                    else
                    {
                        foreach (UserInfoTreeNode userInfoTreeNode in teamInfoTreeNode.Nodes)
                        {
                            if (userInfoTreeNode.Checked)
                            {
                                users.Add(userInfoTreeNode.UserInfo);
                            }
                        }
                    }
                }

                this.callJobGroup.Teams = new TeamInfo[teams.Count];
                teams.CopyTo(this.callJobGroup.Teams);

                this.callJobGroup.Users = new UserInfo[users.Count];
                users.CopyTo(this.callJobGroup.Users);
            }
            else
            {
                List<TeamInfo> teams = new List<TeamInfo>();
                List<UserInfo> users = new List<UserInfo>();

                foreach (TeamInfoTreeNode teamInfoTreeNode in this.teamUserTreeView.Nodes)
                {
                    if (teamInfoTreeNode.Checked)
                        teams.Add(teamInfoTreeNode.TeamInfo);

                    else
                    {
                        foreach (UserInfoTreeNode userInfoTreeNode in teamInfoTreeNode.Nodes)
                        {
                            if (userInfoTreeNode.Checked)
                            {
                                users.Add(userInfoTreeNode.UserInfo);
                            }
                        }
                    }
                }

                foreach (var callJobGroup in currentCallJobGroups)
                {
                    callJobGroup.Teams = new TeamInfo[teams.Count];
                    teams.CopyTo(callJobGroup.Teams);

                    callJobGroup.Users = new UserInfo[users.Count];
                    users.CopyTo(callJobGroup.Users);
                }
            }
        }

        private void selectCallJobGroupButton_Click(object sender, EventArgs e)
        {
            foreach (TeamInfoTreeNode teamInfoTreeNode in this.teamUserTreeView.Nodes)
            {
                teamInfoTreeNode.Checked = true;
                foreach (UserInfoTreeNode userInfoTreeNode in teamInfoTreeNode.Nodes)
                {
                    userInfoTreeNode.Checked = true;
                }
            }
        }

        private void delCallJobGroupsButton_Click(object sender, EventArgs e)
        {
            foreach (TeamInfoTreeNode teamInfoTreeNode in this.teamUserTreeView.Nodes)
            {
                teamInfoTreeNode.Checked = false;
                foreach (UserInfoTreeNode userInfoTreeNode in teamInfoTreeNode.Nodes)
                {
                    userInfoTreeNode.Checked = false;
                }
            }

        }
 
    }
}