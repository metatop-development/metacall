using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Principal;

using metatop.Applications.metaCall.DataObjects;

namespace metatop.Applications.metaCall.BusinessLayer
{
    public class MetaCallPrincipal: IPrincipal
    {
        public const string AdminRoleName = "Administratoren";
        public const string CenterAdminRoleName = "CenterAdministratoren";
        public const string TeamLeiterRoleName = "TeamLeiter";
        public const string TelefonAgentRoleName = "TelefonAgent";

        
        private MetaCallIdentity identity;
        public MetaCallPrincipal(User user)
        {
            this.identity = new MetaCallIdentity(user);
        }
        
        #region IPrincipal Member

        public MetaCallIdentity Identity
        {
            get { return this.identity; }
        }

        public bool IsInRole(string role)
        {
            if (role == null)
                throw new ArgumentNullException("role");

            if (identity.User.SecurityGroups == null || identity.User.SecurityGroups.Length == 0)
                return false;

            foreach (SecurityGroup group in identity.User.SecurityGroups)
            {
                if (string.Compare(group.Name, role, StringComparison.InvariantCultureIgnoreCase) == 0)
                    return true;
            }

            return false;
        }

        #endregion

        #region IPrincipal Member

        IIdentity IPrincipal.Identity
        {
            get { return this.identity; }
        }

        #endregion

        public static MetaCallPrincipal Current
        {
            get
            {
                return (MetaCallPrincipal)System.Threading.Thread.CurrentPrincipal;
            }
        }
    }
}
