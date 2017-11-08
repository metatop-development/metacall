using System;
using System.Collections.Generic;
using System.Text;

namespace metatop.Applications.metaCall.DataObjects
{
    public partial class AddressBase
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

        public string DisplaySortName
        {
            get
            {
                StringBuilder displaySortNameTmp = new StringBuilder();

                if (!string.IsNullOrEmpty(this.Nachname))
                {
                    if (displaySortNameTmp.Length > 0) { displaySortNameTmp.Append(" "); }
                    displaySortNameTmp.Append(this.Nachname);
                }

                if (!string.IsNullOrEmpty(this.Vorname))
                {
                    if (displaySortNameTmp.Length > 0) { displaySortNameTmp.Append(", "); }
                    displaySortNameTmp.Append(this.Vorname);
                }

                if (!string.IsNullOrEmpty(this.Titel))
                {
                    if (displaySortNameTmp.Length > 0) { displaySortNameTmp.Append("; "); }
                    displaySortNameTmp.Append(this.Titel);
                }

                return displaySortNameTmp.ToString();

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


        public string GetCompleteAdress()
        {
            StringBuilder sb = new StringBuilder();

            //Zeile 1
            if (!string.IsNullOrEmpty(this.Anrede))
                sb.AppendLine(this.Anrede);

            //Zeile 2
            if (!string.IsNullOrEmpty(this.Vorname))
                sb.Append(this.Vorname);

            if (!string.IsNullOrEmpty(this.Nachname))
            {
                if (sb.Length > 0) sb.Append(" ");
                sb.Append(this.Nachname);
            }


            if (sb.Length > 0) sb.AppendLine();

            if (!string.IsNullOrEmpty(this.Text1))
                sb.AppendLine(this.Text1);

            if (!string.IsNullOrEmpty(this.Text2))
                sb.AppendLine(this.Text2);

            if (!string.IsNullOrEmpty(this.Strasse))
                sb.AppendLine(this.Strasse);

            if (!string.IsNullOrEmpty(this.DisplayResidence))
            {
                //sb.AppendLine(); --> laut Frau Müller !!!
                sb.AppendLine(this.DisplayResidence);
            }
            return sb.ToString();
        }
    }
}
