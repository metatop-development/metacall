using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;

using metatop.Applications.metaCall.DataObjects;
using System.Collections;

namespace metatop.Applications.metaCall.BusinessLayer
{
    public static partial class DataFields
    {
        [DataFieldMethod("$Sponsor.Anschrift$",
            DataFieldMethodCategory.Sponsor,
            Description = "liefert die Anschrift (Rechnungsadresse) des Sponsors")]
        public static string GetSponsorAnschrift(DataFieldMethodParameter param)
        {
            if (param == null)
                throw new ArgumentNullException("param");


            object oSponsor;

            if (!param.Parameters.TryGetValue("sponsor", out oSponsor))
                return null;

            Sponsor sponsor = oSponsor as Sponsor;

            string adress = sponsor.GetCompleteAdress();
            return adress.Replace("\n", "");
        }

        [DataFieldMethod("$Sponsor.Anrede$",
            DataFieldMethodCategory.Sponsor,
            Description = "liefert die Anrede des Ansprechpartners eines Sponsors")]
        public static string GetSponsorAnrede(DataFieldMethodParameter param)
        {
            Sponsor sponsor = param.Parameters["sponsor"] as Sponsor;
            Project project = param.Parameters["project"] as Project;

            if (sponsor == null ||
                project == null)
                throw new ArgumentException("param.DataFields doesn't contain Sponsor and/or Project");


            string salutaion = "Sehr geehrte Damen und Herren";
            Language language = project.mwProject.Language;

            IDictionary<string, string> languageSalutations = 
                    param.Business.Addresses.GetSalutaions(language);
            salutaion = languageSalutations[""];

            if (sponsor.ContactPerson != null &&
                sponsor.ContactPerson.Anrede != null)
            {
                string titelName;
                if (sponsor.ContactPerson.Titel == null)
                    titelName = sponsor.ContactPerson.Nachname;
                else
                {
                    if (sponsor.ContactPerson.Titel.Length > 0)
                        titelName = sponsor.ContactPerson.Titel + " " + sponsor.ContactPerson.Nachname;
                    else
                        titelName = sponsor.ContactPerson.Nachname;
                }

                salutaion = param.Business.Addresses.GetSalutaions(language)[sponsor.ContactPerson.Anrede];
                salutaion = string.Format(salutaion, titelName);
            }

            return salutaion;
        }

        [DataFieldMethod("$Verein.Name$",
            DataFieldMethodCategory.Customer,
            Description = "liefert den Namen/Bezeichnung des Vereins")]
        public static string GetVereinName(DataFieldMethodParameter param)
        {

            Customer customer = param.Parameters["customer"] as Customer;

            if (customer == null)
                throw new ArgumentException("param.DataFields doesn't contain Customer");

            return customer.Nachname;
        }

        [DataFieldMethod("$Verein.Abteilung$",
            DataFieldMethodCategory.Customer,
            Description = "liefert die Abteilung des Vereins$")]
        public static string GetVereinAbteilung(DataFieldMethodParameter param)
        {

            Customer customer = param.Parameters["customer"] as Customer;

            if (customer == null)
                throw new ArgumentException("param.DataFields doesn't contain Customer");

            return customer.Abteilung;
        }


        [DataFieldMethod("$Verein.Ansprechpartner$",
            DataFieldMethodCategory.Customer,
            Description = "liefert den Ansprechpartner des Vereins")]
        public static string GetVereinAnsprechpartner(DataFieldMethodParameter param)
        {

            Customer customer = param.Parameters["customer"] as Customer;

            if (customer == null)
                throw new ArgumentException("param.DataFields doesn't contain Customer");

            ContactPerson contactPerson = customer.ContactPerson as ContactPerson;

            if (contactPerson == null)
                return null;

            StringBuilder sb = new StringBuilder();

            if (!string.IsNullOrEmpty(contactPerson.Anrede))
            {
                if (sb.Length > 0) sb.Append(" ");
                sb.Append(contactPerson.Anrede);
            }

            if (!string.IsNullOrEmpty(contactPerson.Titel))
            {
                if (sb.Length > 0) sb.Append(" ");
                sb.Append(contactPerson.Titel);
            }

            if (!string.IsNullOrEmpty(contactPerson.Vorname))
            {
                if (sb.Length > 0) sb.Append(" ");
                sb.Append(contactPerson.Vorname);
            }

            if (!string.IsNullOrEmpty(contactPerson.Nachname))
            {
                if (sb.Length > 0) sb.Append(" ");
                sb.Append(contactPerson.Nachname);
            }

            return sb.ToString();
        }

        [DataFieldMethod("$Verein.AnredeKomplett$",
            DataFieldMethodCategory.Customer,
            Description = "liefert die komplette Anrede des Vereins (Name + Abteilunng + Ansprechpartner)")]
        public static string GetVereinAnredeKomplett(DataFieldMethodParameter param)
        {

            Customer customer = param.Parameters["customer"] as Customer;

            if (customer == null)
                throw new ArgumentException("param.DataFields doesn't contain Customer");

            string name, abteilung, ansprechpartner;
            name = GetVereinName(param);
            abteilung = GetVereinAbteilung(param);
            ansprechpartner = GetVereinAnsprechpartner(param);

            StringBuilder sb = new StringBuilder();

            if (!string.IsNullOrEmpty(name))
            {
                if (sb.Length > 0) sb.Append(", ");
                sb.Append(name);
            }
            if (!string.IsNullOrEmpty(abteilung))
            {
                if (sb.Length > 0) sb.Append(", ");
                sb.Append(abteilung);
            }
            if (!string.IsNullOrEmpty(ansprechpartner))
            {
                if (sb.Length > 0) sb.Append(", ");
                sb.Append(ansprechpartner);
            }

            return sb.ToString();
        }

        [DataFieldMethod("$Benutzer.Vorname$",
            DataFieldMethodCategory.CurrentUser,
            Description = "liefert den Vornamen des angemeldeten Benutzers")]
        public static string GetCurrentUserVorname(DataFieldMethodParameter param)
        {

            User user = param.Parameters["user"] as User;

            if (user == null)
                throw new ArgumentException("param.DataFields doesn't contain User");

            return user.Vorname;
        }


        [DataFieldMethod("$Benutzer.Nachname$",
            DataFieldMethodCategory.CurrentUser,
            Description = "liefert den Nachnamen des angemeldeten Benutzers")]
        public static string GetCurrentUserSurname(DataFieldMethodParameter param)
        {

            User user = param.Parameters["user"] as User;

            if (user == null)
                throw new ArgumentException("param.DataFields doesn't contain User");

            return user.Nachname;
        }

        [DataFieldMethod("$Benutzer.Name$",
            DataFieldMethodCategory.CurrentUser,
            Description = "liefert den Vor- und Nachnamen des aktuellen Benutzers")]
        public static string GetCurrentUserName(DataFieldMethodParameter param)
        {

            User user = param.Parameters["user"] as User;

            if (user == null)
                throw new ArgumentException("param.DataFields doesn't contain User");

            StringBuilder sb = new StringBuilder();

            if (!string.IsNullOrEmpty(user.Vorname))
            {
                if (sb.Length > 0) sb.Append(" ");
                sb.Append(user.Vorname);
            }

            if (!string.IsNullOrEmpty(user.Nachname))
            {
                if (sb.Length > 0) sb.Append(" ");
                sb.Append(user.Nachname);
            }
            return sb.ToString();

        }

        [DataFieldMethod("$Benutzer.Unterschrift$",
            DataFieldMethodCategory.CurrentUser,
            Description = "liefert die Unterschrift des aktuellen Benutzers")]
        public static WordImages GetAgentUnterschrift(DataFieldMethodParameter param)
        {
            User user = param.Parameters["user"] as User;

            if (user == null)
                throw new ArgumentException("param.DataFields doesn't contain User");


            UserSignature userSignature = param.Business.Users.GetSignature(user);

            if (userSignature == null)
            {
                return null;
            }
            else
            {
                WordImages wordImages = new WordImages(userSignature.FileName.ToString());
                return wordImages;
            }

            /*
            Image image = null;
            using (Stream stream = new MemoryStream(userSignature.Signature))
            {
                image = Image.FromStream(stream);
            }

            return image;
             */
        }

        [DataFieldMethod("$Projekt.Bedankungsformen$",
            DataFieldMethodCategory.Project,
            Description = "liefert die Bedankungsformen des Projekts als Liste wobei jede Bedankungsform eine Zeile darstellt")]
        public static string GetProjektBedankungsformen(DataFieldMethodParameter param)
        {

            Project project = param.Parameters["project"] as Project;

            if (project == null)
                throw new ArgumentException("param.DataFields doesn't contain Project");

            List<ThankingsFormsProject> thankingForms = param.Business.ThankingsFormsProject.GetThankingsFormsByProject(project);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < thankingForms.Count; i++)
            {
                ThankingsFormsProject thankingForm = thankingForms[i];
                if (sb.Length > 0) sb.AppendLine();
                sb.AppendFormat("{0}", thankingForm.BedankungsformDe);
            }

            return sb.ToString();
        }

        [DataFieldMethod("$Projekt.Sponsorenpakete1$",
            DataFieldMethodCategory.Project,
            Description = "liefert die erste Zeile der Sponsorenpakete des Projekts wobei jedes Sponsorpaket eine Zeile darstellt")]
        public static string GetProjektSponsorenpakete1(DataFieldMethodParameter param)
        {

            Project project = param.Parameters["project"] as Project;

            if (project == null)
                throw new ArgumentException("param.DataFields doesn't contain Project");

            List<mwProjekt_SponsorPacket> sponsorPackets = param.Business.SponsorPacketBusiness;
            //List<ThankingsFormsProject> thankingForms = param.Business.ThankingsFormsProject.GetThankingsFormsByProject(project);
            StringBuilder sb = new StringBuilder();
            /*for (int i = 0; i < thankingForms.Count; i++)
            {
                ThankingsFormsProject thankingForm = thankingForms[i];
                if (sb.Length > 0) sb.AppendLine();
                sb.AppendFormat("{0}", thankingForm.BedankungsformDe);
            }*/
            if (sponsorPackets.Count > 0)
            {
                mwProjekt_SponsorPacket sponsorPacket = sponsorPackets[0];
                sb.AppendFormat("___\t{0}\tzu je {1} € zzgl. Mwst." ,sponsorPacket.FaxText1_de,Convert.ToInt32(sponsorPacket.BetragNetto).ToString() );
            }

            return sb.ToString();
        }

        [DataFieldMethod("$Projekt.Sponsorenpakete2$",
            DataFieldMethodCategory.Project,
            Description = "liefert die zweite Zeile der Sponsorenpakete des Projekts wobei jedes Sponsorpaket eine Zeile darstellt")]
        public static string GetProjektSponsorenpakete2(DataFieldMethodParameter param)
        {

            Project project = param.Parameters["project"] as Project;

            if (project == null)
                throw new ArgumentException("param.DataFields doesn't contain Project");

            List<mwProjekt_SponsorPacket> sponsorPackets = param.Business.SponsorPacketBusiness;
            //List<ThankingsFormsProject> thankingForms = param.Business.ThankingsFormsProject.GetThankingsFormsByProject(project);
            StringBuilder sb = new StringBuilder();
            /*for (int i = 0; i < thankingForms.Count; i++)
            {
                ThankingsFormsProject thankingForm = thankingForms[i];
                if (sb.Length > 0) sb.AppendLine();
                sb.AppendFormat("{0}", thankingForm.BedankungsformDe);
            }*/
            if (sponsorPackets.Count > 0)
            {
                mwProjekt_SponsorPacket sponsorPacket = sponsorPackets[0];
                sb.AppendFormat("{0}", sponsorPacket.FaxText2_de);
            }

            return sb.ToString();
        }

        [DataFieldMethod("$Projekt.Sponsorenpakete3$",
            DataFieldMethodCategory.Project,
            Description = "liefert die zweite Zeile der Sponsorenpakete des Projekts wobei jedes Sponsorpaket eine Zeile darstellt")]
        public static string GetProjektSponsorenpakete3(DataFieldMethodParameter param)
        {

            Project project = param.Parameters["project"] as Project;

            if (project == null)
                throw new ArgumentException("param.DataFields doesn't contain Project");

            List<mwProjekt_SponsorPacket> sponsorPackets = param.Business.SponsorPacketBusiness;
            StringBuilder sb = new StringBuilder();
            /*for (int i = 0; i < thankingForms.Count; i++)
            {
                ThankingsFormsProject thankingForm = thankingForms[i];
                if (sb.Length > 0) sb.AppendLine();
                sb.AppendFormat("{0}", thankingForm.BedankungsformDe);
            }*/
            if (sponsorPackets.Count > 1)
            {
                mwProjekt_SponsorPacket sponsorPacket = sponsorPackets[1];
                sb.AppendFormat("___\t{0}\tzu je {1} € zzgl. Mwst.", sponsorPacket.FaxText1_de, Convert.ToInt32(sponsorPacket.BetragNetto).ToString());
            }

            return sb.ToString();
        }

        [DataFieldMethod("$Projekt.Sponsorenpakete4$",
            DataFieldMethodCategory.Project,
            Description = "liefert die zweite Zeile der Sponsorenpakete des Projekts wobei jedes Sponsorpaket eine Zeile darstellt")]
        public static string GetProjektSponsorenpakete4(DataFieldMethodParameter param)
        {

            Project project = param.Parameters["project"] as Project;

            if (project == null)
                throw new ArgumentException("param.DataFields doesn't contain Project");

            List<mwProjekt_SponsorPacket> sponsorPackets = param.Business.SponsorPacketBusiness;
            //List<ThankingsFormsProject> thankingForms = param.Business.ThankingsFormsProject.GetThankingsFormsByProject(project);
            StringBuilder sb = new StringBuilder();
            /*for (int i = 0; i < thankingForms.Count; i++)
            {
                ThankingsFormsProject thankingForm = thankingForms[i];
                if (sb.Length > 0) sb.AppendLine();
                sb.AppendFormat("{0}", thankingForm.BedankungsformDe);
            }*/
            if (sponsorPackets.Count > 1)
            {
                mwProjekt_SponsorPacket sponsorPacket = sponsorPackets[1];
                sb.AppendFormat("{0}", sponsorPacket.FaxText2_de);
            }

            return sb.ToString();
        }

        [DataFieldMethod("$Projekt.Sponsorenpakete5$",
            DataFieldMethodCategory.Project,
            Description = "liefert die zweite Zeile der Sponsorenpakete des Projekts wobei jedes Sponsorpaket eine Zeile darstellt")]
        public static string GetProjektSponsorenpakete5(DataFieldMethodParameter param)
        {

            Project project = param.Parameters["project"] as Project;

            if (project == null)
                throw new ArgumentException("param.DataFields doesn't contain Project");

            List<mwProjekt_SponsorPacket> sponsorPackets = param.Business.SponsorPacketBusiness;
            //List<ThankingsFormsProject> thankingForms = param.Business.ThankingsFormsProject.GetThankingsFormsByProject(project);
            StringBuilder sb = new StringBuilder();
            /*for (int i = 0; i < thankingForms.Count; i++)
            {
                ThankingsFormsProject thankingForm = thankingForms[i];
                if (sb.Length > 0) sb.AppendLine();
                sb.AppendFormat("{0}", thankingForm.BedankungsformDe);
            }*/
            if (sponsorPackets.Count > 2)
            {
                mwProjekt_SponsorPacket sponsorPacket = sponsorPackets[2];
                sb.AppendFormat("___\t{0}\tzu je {1} € zzgl. Mwst.", sponsorPacket.FaxText1_de, Convert.ToInt32(sponsorPacket.BetragNetto).ToString());
            }

            return sb.ToString();
        }

        [DataFieldMethod("$Projekt.Sponsorenpakete6$",
            DataFieldMethodCategory.Project,
            Description = "liefert die zweite Zeile der Sponsorenpakete des Projekts wobei jedes Sponsorpaket eine Zeile darstellt")]
        public static string GetProjektSponsorenpakete6(DataFieldMethodParameter param)
        {

            Project project = param.Parameters["project"] as Project;

            if (project == null)
                throw new ArgumentException("param.DataFields doesn't contain Project");

            List<mwProjekt_SponsorPacket> sponsorPackets = param.Business.SponsorPacketBusiness;
            //List<ThankingsFormsProject> thankingForms = param.Business.ThankingsFormsProject.GetThankingsFormsByProject(project);
            StringBuilder sb = new StringBuilder();
            /*for (int i = 0; i < thankingForms.Count; i++)
            {
                ThankingsFormsProject thankingForm = thankingForms[i];
                if (sb.Length > 0) sb.AppendLine();
                sb.AppendFormat("{0}", thankingForm.BedankungsformDe);
            }*/
            if (sponsorPackets.Count > 2)
            {
                mwProjekt_SponsorPacket sponsorPacket = sponsorPackets[2];
                sb.AppendFormat("{0}", sponsorPacket.FaxText2_de);
            }

            return sb.ToString();
        }

        [DataFieldMethod("$Projekt.Bezeichnung$",
            DataFieldMethodCategory.Project,
            Description = "liefert Bezeichnung des Projekts")]
        public static string GetProjektBezeichnung(DataFieldMethodParameter param)
        {
            Project project = param.Parameters["project"] as Project;

            if (project == null)
                throw new ArgumentException("param.DataFields doesn't contain Project");


            return project.BezeichnungRechnung;

        }

        [DataFieldMethod("$Sponsor.Faxdaten$",
            DataFieldMethodCategory.ArrayList,
            Description = "liefert die ActivFaxdaten")]
        public static string GetSponsorFaxdaten(DataFieldMethodParameter param)
        {
            ArrayList dataFieldTable = param.Parameters["ArrayList"] as ArrayList;

            if (dataFieldTable == null)
                throw new ArgumentException("param.DataFields doesn't contain ArrayList dataFieldTable");

            StringBuilder sbDataFieldTable = new StringBuilder();

            foreach (string fieldTable in dataFieldTable)
            {
                if (sbDataFieldTable.Length > 0)
                {
                    sbDataFieldTable.AppendLine();
                }
                sbDataFieldTable.Append(fieldTable);
            }

            return sbDataFieldTable.ToString();

        }        

    }
}
