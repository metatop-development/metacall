using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Principal;

using metatop.Applications.metaCall.DataObjects;

namespace metatop.Applications.metaCall.BusinessLayer
{
    public class MetaCallIdentity: IIdentity
    {
        private User user;

        public MetaCallIdentity(User user)
        {

            if (user == null)
                throw new ArgumentNullException("user");

            this.user = user;
        }

        #region IIdentity Member

        public string AuthenticationType
        {
            get { return "METACALL"; }
        }

        public bool IsAuthenticated
        {
            get { return true; }
        }

        public string Name
        {
            get 
            {
                return this.user.UserName; 
            }
        }

        #endregion

        public User User
        {
            get
            {
                return this.user;
            }
        }
    }
}
