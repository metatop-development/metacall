using System;
using System.Collections.Generic;
using System.Text;

namespace metatop.Applications.metaCall.DataObjects
{
    public partial class ContactPerson
    {

        public string DisplayName
        {
            get
            {
                StringBuilder displayNameTmp = new StringBuilder();
                
                if (!string.IsNullOrEmpty(this.anredeField))
                {
                    if (displayNameTmp.Length > 0) { displayNameTmp.Append(" "); }
                    displayNameTmp.Append(this.anredeField);
                }
                
                if (!string.IsNullOrEmpty(this.titelField))
                {
                    if (displayNameTmp.Length > 0) {displayNameTmp.Append(" ");} 
                    displayNameTmp.Append(this.titelField);
                }

                if (!string.IsNullOrEmpty(this.vornameField)) 
                {
                    if (displayNameTmp.Length > 0) {displayNameTmp.Append(" ");}
                    displayNameTmp.Append(this.vornameField);
                }

                if (!string.IsNullOrEmpty(this.nachnameField)) 
                {
                    if (displayNameTmp.Length > 0) {displayNameTmp.Append(" ");}
                    displayNameTmp.Append(this.nachnameField);
                }
                
                return displayNameTmp.ToString();
            }
        }

    }
}
