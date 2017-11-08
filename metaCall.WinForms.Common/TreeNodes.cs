using System;
using System.Collections.Generic;
using System.Text;

using System.Windows.Forms;
using metatop.Applications.metaCall.BusinessLayer;
using metatop.Applications.metaCall.DataObjects;
using System.ComponentModel;


namespace metatop.Applications.metaCall.WinForms
{

    /// <summary>
    /// TreeNode-Element für die Anzeige von ausgeschiedenen Usern
    /// </summary>
    public class UsersDeletedTreeNode : TreeNode
    {
        /// <summary>
        /// Initialisiert eine neue Instanz der UsersDeletedTreeNode-Klasse
        /// </summary>
        public UsersDeletedTreeNode()
        {
            this.Text = "<Benutzer ausgeschieden>";
        }
    }

    /// <summary>
    /// TreeNode-Element für die Anzeige von User ohne Team
    /// </summary>
    public class UsersWithOutCenterTreeNode : TreeNode
    {
        /// <summary>
        /// Initialisiert eine neue Instanz der UsersWithOutCenterTreeNode-Klasse
        /// </summary>
        public UsersWithOutCenterTreeNode()
        {
            this.Text = "<Benutzer ohne Team>";
        }
    }

    /// <summary>
    /// TreeNode-Element für die Anzeige von ProjectStateInfo-Objekten
    /// </summary>
    public class ProjectStateInfoTreeNode : TreeNode
    {
        /// <summary>
        /// Initialisiert eine neue Instanz der ProjectStateInfoTreeNode-Klasse mit der angegebenen ProjectStateInfo
        /// </summary>
        /// <param name="projectStateInfo"></param>
        public ProjectStateInfoTreeNode(ProjectStateInfo projectStateInfo)
        {
            this.projectStateInfo = projectStateInfo;
            this.Text = projectStateInfo.DisplayName;
        }

        ProjectStateInfo projectStateInfo;
        /// <summary>
        /// Ruft die ProjectStateInfo der ProjectStateInfoTreeNode-Instanz ab
        /// </summary>
        public ProjectStateInfo ProjectState
        {
            get
            {
                return this.projectStateInfo;
            }
        }
    }

    /// <summary>
    /// TreeNode-Element für die Anzeige von ProjectInfo-Objekten
    /// </summary>
    public class ProjectInfoTreeNode : TreeNode
    {
        /// <summary>
        /// Initialisiert eine neue Instanz der ProjectInfoTreeNode-Klasse mit der angegebenen ProjectInfo
        /// </summary>
        /// <param name="project"></param>
        public ProjectInfoTreeNode(ProjectInfo projectInfo)
        {
            this.projectInfo = projectInfo;
            this.Text = projectInfo.Bezeichnung;
        }

        private ProjectInfo projectInfo;
        /// <summary>
        /// Ruft die ProjectInfo der ProjectInfoTreeNode-Instanz ab
        /// </summary>
        public ProjectInfo ProjectInfo
        {
            get { return projectInfo; }
        }
    }

    public class UserInfoTreeNode : TreeNode
    {
        public UserInfoTreeNode(UserInfo user)
        {
            if (user == null)
                throw new ArgumentNullException("User");

            this.user = user;
            this.Text = user.DisplayName;
        }

        private UserInfo user;

        public UserInfo User
        {
            get { return user; }
        }
    }

    /// <summary>
    /// TreeNode-Element für die Anzeige von TeamInfo-Objekten
    /// </summary>
    public class TeamInfoTreeNode : TreeNode
    {
        /// <summary>
        /// Initialisiert eine neue Instanz der TeamInfoTreeNode-Klasse mit der angegebenen TeamInfo
        /// </summary>
        /// <param name="team"></param>
        public TeamInfoTreeNode(TeamInfo team)
        {
            if (team == null)
                throw new ArgumentNullException("team");
            
            this.team = team;
            this.Text = team.Bezeichnung;
            this.ToolTipText = team.Beschreibung;
            this.team.PropertyChanged += new PropertyChangedEventHandler(team_PropertyChanged);
        }

        void team_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Bezeichnung")
                this.Text = this.team.Bezeichnung;

            if (e.PropertyName == "Beschreibung")
                this.ToolTipText = this.team.Beschreibung;
        }

        private TeamInfo team;
        /// <summary>
        /// Ruft die TeamInfo der TeamInfoTreeNde-Instanz ab 
        /// </summary>
        public TeamInfo Team
        {
            get { return team; }
        }
    }


    /// <summary>
    /// TreeNode-Element für die Anzeige von Team-Objekten
    /// </summary>
    public class TeamTreeNode : TreeNode
    {
        /// <summary>
        /// Initialisiert eine neue Instanz der TeamTreeNode-Klasse mit dem angegebenen Team
        /// </summary>
        /// <param name="team"></param>
        public TeamTreeNode(TeamInfo team)
        {
            if (team == null)
                throw new ArgumentNullException("Team");

            this.team = team;
            this.Text = team.Bezeichnung;
        }

        private TeamInfo team;

        /// <summary>
        /// Ruft das Team der TeamTreeNode-Instanz ab
        /// </summary>
        public TeamInfo Team
        {
            get { return team; }
        }
        
    }

    /// <summary>
    /// TreeNode-Element für die Anzeige von nicht zugeordneten Teams
    /// </summary>
    public class TeamsWithoutCenterTreeNode : TreeNode
    {
        /// <summary>
        /// Initialisiert eine neue Instanz der TeamsWithoutCenterTreeNode-Klasse
        /// </summary>
        public TeamsWithoutCenterTreeNode()
        {
            this.Text = "<Team ohne Center>";
        }
    }

    /// <summary>
    /// TreeNode-Element für die Anzeige von deaktiverten Teams
    /// </summary>
    public class TeamsDeletedTreeNode : TreeNode
    {
        /// <summary>
        /// Initialisiert eine neue Instanz der TeamsDeletedTreeNode-Klasse
        /// </summary>
        public TeamsDeletedTreeNode()
        {
            this.Text = "<Teams deaktviert>";
        }
    }

    /// <summary>
    /// TreeNode-Element für die Anzeige von Center-Objekten
    /// </summary>
    public class CenterTreeNode : TreeNode
    {
        /// <summary>
        /// Initialisiert eine neue Instanz der CenterTreeNode-Klasse mit dem angegebenen Center
        /// </summary>
        /// <param name="center"></param>
        public CenterTreeNode(CenterInfo center)
        {
            if (center == null)
                throw new ArgumentNullException("Center");

            this.center = center;
            this.Text = center.Bezeichnung;
        }

        private CenterInfo center;
        /// <summary>
        /// Ruft das Center der CenterTreeNode-Instanz ab
        /// </summary>
        public CenterInfo Center
        {
            get { return center; }
        }
          
    }

    
}
