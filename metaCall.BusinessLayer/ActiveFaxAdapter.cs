using System;
using System.Collections.Generic;
using System.Text;

using metatop.Applications.metaCall.DataObjects;
using System.Drawing.Printing;
using System.Collections;


namespace metatop.Applications.metaCall.BusinessLayer
{
    public static class ActiveFaxAdapter
    {
        public static ArrayList BuildFaxFileName(AddressBase address)
        {
            if (address == null)
                throw new ArgumentNullException("address");

            string faxNumber = address.FaxNummer;

            if (string.IsNullOrEmpty(faxNumber))
                throw new ArgumentNullException("keine Fax-Nummer");

            ArrayList dataTableFax = new ArrayList();
            
            string name = address.DisplayName;

            dataTableFax.Add("@F501 0@");

            //Name des Empfängers (optional)
            if (!string.IsNullOrEmpty(name))
            {
                dataTableFax.Add(string.Format("@F201 {0}@", name));
            }

            //Faxnummer (erforderlich)
            if (!string.IsNullOrEmpty(faxNumber))
            {
                dataTableFax.Add(string.Format("@F211 {0}@", faxNumber));
            }
            return dataTableFax;
        }

        public static ArrayList GetEmptySponsorFaxDatenField()
        {
            ArrayList dataTableFax = new ArrayList();
            dataTableFax.Add(string.Empty);

            return dataTableFax;
        }

        public static ArrayList BuildEmailFileName(AddressBase address,
            string subject,
            string body)
        {
            if (address == null)
                throw new ArgumentNullException("address");

            string email = address.EMail;

            if (string.IsNullOrEmpty(email))
                throw new ArgumentNullException("keine eMail-Adresse");

            StringBuilder sb = new StringBuilder();

            ArrayList dataTableFax = new ArrayList();
            string name = address.DisplayName;

            dataTableFax.Add("@F501 @");

            //Name des Empfängers (optional)
            if (!string.IsNullOrEmpty(name))
            {
                dataTableFax.Add(string.Format("@F201 {0}@", name));
            }
            //Email (erforderlich)
            //if (!string.IsNullOrEmpty(email))
            //{
            //    dataTableFax.Add(string.Format("@F212 {0}@", email.Replace("@","\\@")));
            //}

            //Betreff
            if (!string.IsNullOrEmpty(subject))
            {
                dataTableFax.Add(string.Format("@F307 {0}@", subject));
            }

            //Body
            if (!string.IsNullOrEmpty(body))
            {
                dataTableFax.Add(string.Format("@F603 {0}@", body));
            }

            //Art der Faxnachricht (E=Email)
            dataTableFax.Add("@F213 E@");

            //Email (erforderlich)
            if (!string.IsNullOrEmpty(email))
            {
                dataTableFax.Add(string.Format("@F212 {0}@", email));
            }

            return dataTableFax;
        }
/*
        public static string BuildFaxFileName(AddressBase address)
        {
            if (address == null)
                throw new ArgumentNullException("address");

            string faxNumber = address.FaxNummer;

            if (string.IsNullOrEmpty(faxNumber))
                throw new ArgumentNullException("keine Fax-Nummer");

            StringBuilder sb = new StringBuilder();

            string name = address.DisplayName;

            //Name des Empfängers (optional)
            if (!string.IsNullOrEmpty(name))
                sb.AppendFormat("@F201 {0}@", name);

            //Faxnummer (erforderlich)
            if (!string.IsNullOrEmpty(faxNumber))
                sb.AppendFormat("@F211 {0}@", faxNumber);

            //Art der Faxnachricht (F = Fax)
            sb.AppendFormat("@F213 F@");

            return sb.ToString();


        }
*/

/*
        public static string BuildEmailFileName(AddressBase address, 
            string subject, 
            string body)
        {
            if (address == null)
                throw new ArgumentNullException("address");
            
            string email= address.EMail;

            if (string.IsNullOrEmpty(email))
                throw new ArgumentNullException("keine eMail-Adresse");

            StringBuilder sb = new StringBuilder();

            string name = address.DisplayName;
            
            //Name des Empfängers (optional)
            if (!string.IsNullOrEmpty(name))
                sb.AppendFormat("@F201 {0}@", name);

            //Faxnummer (erforderlich)
            if (!string.IsNullOrEmpty(email))
                sb.AppendFormat("@F212 {0}@", email);

            //Betreff
            if (!string.IsNullOrEmpty(subject))
                sb.AppendFormat("@F307 {0}@", subject);

            //Body
            if (!string.IsNullOrEmpty(body))
                sb.AppendFormat("@F603 {0}@", body);

            //Art der Faxnachricht (E=Email)
            sb.AppendFormat("@F213 E@");

            return sb.ToString();
        }
*/

        public static bool ValidatePrinterName(string printer)
        {
            PrinterSettings settings = new PrinterSettings();
            settings.PrinterName = printer;

            return settings.IsValid;
        }
    }
}
