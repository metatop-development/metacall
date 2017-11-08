using System;
using System.Collections.Generic;
using System.Text;

namespace metatop.Applications.metaCall.DataObjects
{
    public partial class Customer
    {
        public string DisplayName
        {
            get
            {
                StringBuilder displayNameTmp = new StringBuilder();

                if (!string.IsNullOrEmpty(this.Anrede))
                {
                    if (displayNameTmp.Length > 0) { displayNameTmp.Append(" "); }
                    displayNameTmp.Append(this.Anrede);
                }

                if (!string.IsNullOrEmpty(this.Titel))
                {
                    if (displayNameTmp.Length > 0) { displayNameTmp.Append(" "); }
                    displayNameTmp.Append(this.Titel);
                }

                if (!string.IsNullOrEmpty(this.Vorname))
                {
                    if (displayNameTmp.Length > 0) { displayNameTmp.Append(" "); }
                    displayNameTmp.Append(this.Vorname);
                }

                if (!string.IsNullOrEmpty(this.Nachname))
                {
                    if (displayNameTmp.Length > 0) { displayNameTmp.Append(" "); }
                    displayNameTmp.Append(this.Nachname);
                }

                return displayNameTmp.ToString();
            }
        }

        public string DisplayResidence
        {
            get
            {
                StringBuilder displayResidenceTmp = new StringBuilder();

                if (!string.IsNullOrEmpty(this.Land))
                {
                    if (displayResidenceTmp.Length > 0) { displayResidenceTmp.Append(" "); }
                    displayResidenceTmp.Append(this.Land);
                }

                if (!string.IsNullOrEmpty(this.PLZ))
                {
                    if (displayResidenceTmp.Length > 0) { displayResidenceTmp.Append("-"); }
                    displayResidenceTmp.Append(this.PLZ);
                }

                if (!string.IsNullOrEmpty(this.Ort))
                {
                    if (displayResidenceTmp.Length > 0) { displayResidenceTmp.Append(" "); }
                    displayResidenceTmp.Append(this.Ort);
                }

                return displayResidenceTmp.ToString();

            }

        }



    }
}
