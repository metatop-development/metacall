using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using metatop.Applications.metaCall.DataObjects;

namespace metatop.Applications.metaCall.BusinessLayer
{
    public class AddressBusiness
    {
        private MetaCallBusiness metaCallBusiness;

        public AddressBusiness(MetaCallBusiness metaCallBusiness)
        {
            this.metaCallBusiness = metaCallBusiness;
        }

        /// <summary>
        /// Aktualisiert einen Sponsor auf dem Server
        /// Hierbei werden metaware und Adressenpool synchrion gehalten
        /// </summary>
        /// <param name="sponsor"></param>
        /// <remarks>Auftrags- und Rechnungsdaten sind von dem Update nicht
        /// betroffen</remarks>
        public void UpdateSponsor(Sponsor sponsor)
        {
            if (sponsor == null)
            {
                throw new ArgumentNullException("sponsor");
            }

            if (string.IsNullOrEmpty(sponsor.Nachname))
            {
                throw new InvalidOperationException("Der Nachname des Sponsors darf nicht leer sein!");
            }

            this.metaCallBusiness.ServiceAccess.UpdateSponsor(sponsor);
        }

        /// <summary>
        /// Überträgt die Adressen zu einem Projekt aus Metaware nach metaCall
        /// </summary>
        /// <param name="project"></param>
        public void TransferAddresses(Project project)
        {
            if (project == null)
            {
                throw new ArgumentNullException("project");
            }

            this.metaCallBusiness.ServiceAccess.TransferAddressPool(project);
        }

        public int GetFailureByProject(ProjectInfo project)
        {
            if (project == null)
            {
                throw new ArgumentNullException("project");
            }

            return this.metaCallBusiness.ServiceAccess.GetFailureByProject(project);
        }

        public void DeleteFailureByProject(ProjectInfo project)
        {
            if (project == null)
            {
                throw new ArgumentNullException("project");
            }

            this.metaCallBusiness.ServiceAccess.DeleteFailureByProject(project);
        }

        public void BlockCallJobsWithMissingAddresses()
        {
            this.metaCallBusiness.ServiceAccess.BlockCallJobsWithMissingAddresses();
        }

        public List<Sponsor> GetNewSponsorsForTransfer(Project project)
        {
            if (project == null)
            {
                throw new ArgumentNullException("project");
            }

            return new List<Sponsor>(this.metaCallBusiness.ServiceAccess.GetNewSponsorsForTransfer(project));
        }

        /// <summary>
        /// Liefert alle Sponsoren zu einem Projekt
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public List<Sponsor> GetSponsorsByProject(Project project)
        {
            if (project == null)
            {
                throw new ArgumentNullException("project");
            }

            return new List<Sponsor>(this.metaCallBusiness.ServiceAccess.GetSponsorsByProject(project));
        }

        /// <summary>
        /// Liefert die GeoZone anhand von Sponsor und Projekt
        /// </summary>
        /// <param name="sponsor"></param>
        /// <param name="project"></param>
        /// <returns></returns>
        public GeoZone GetGeoZoneByProject(Sponsor sponsor, ProjectInfo project)
        {
            if (sponsor == null)
            {
                throw new ArgumentNullException("sponsor");
            }

            if (project == null)
            {
                throw new ArgumentNullException("project");
            }

            return this.metaCallBusiness.ServiceAccess.GetGeoZone(sponsor, project.ProjectId);
        }

        public string[] Countries
        {
            get
            {
                return new string[] { "D", "A", "CH", "NL", "PL" };
            }
        }

        public IDictionary<string, string> GetSalutaions(Language language)
        {
            IDictionary<string, string> salutations = new Dictionary<string, string>(); 
            switch (language)
            {
                case Language.Invalid:
                    salutations = GetSalutationsDe();
                    break;

                case Language.German:
                    salutations = GetSalutationsDe();
                    break;

                case Language.SwitzerlandItaly:
                    salutations = GetSalutaionsIt();
                    break;

                case Language.SwitzerlandFrench:
                    salutations = GetSalutaionsFr();
                    break;

                case Language.French:
                    salutations = GetSalutaionsFr();
                    break;

                case Language.Italy:
                    salutations = GetSalutaionsIt();
                    break;

                case Language.Netherland:
                    salutations = GetSalutaionsNl();
                    break;

                default:
                    break;
            }

            return salutations;
        }

        private IDictionary<string, string> GetSalutaionsFr()
        {
            IDictionary<string, string> salutations = new Dictionary<string, string>();
            salutations.Add("Frau", "Chère Madame {0}");
            salutations.Add("Herr", "Cher Monsieur {0}");
            salutations.Add("", "Mesdames, Messieurs");

            return salutations;
        }

        private IDictionary<string, string> GetSalutaionsIt()
        {
            return GetSalutationsDe();
        }

        private IDictionary<string, string> GetSalutaionsNl()
        {
            IDictionary<string, string> salutations = new Dictionary<string, string>();
            salutations.Add("Frau", "Geachte mevrouw {0}");
            salutations.Add("Herr", "Geachte heer {0}");
            salutations.Add("", "Geachte dames en heren");

            return salutations;
        }
        private IDictionary<string, string> GetSalutationsDe()
        {
            IDictionary<string, string> salutations= new Dictionary<string, string>();
            salutations.Add("Frau", "Sehr geehrte Frau {0}");
            salutations.Add("Herr", "Sehr geehrter Herr {0}");
            salutations.Add("", "Sehr geehrte Damen und Herren");

            return salutations;
        }

        /// <summary>
        /// Erstellt einen neuen Sponsor auf dem Server
        /// </summary>
        /// <param name="sponsor"></param>
        public void CreateSponsor(Sponsor sponsor, Project project)
        {
            if (sponsor == null)
            {
                throw new ArgumentNullException("sponsor");
            }

            if (project == null)
            {
                throw new ArgumentNullException("project");
            }

            this.metaCallBusiness.ServiceAccess.CreateSponsor(sponsor, project);
        }

        /// <summary>
        /// Überprüft ob es eine Korrekte eMail-Adresse ist.
        /// </summary>
        /// <param name="eMail"></param>
        /// <returns></returns>
        private bool CheckEMail(string eMail) 
        {
            Regex eMailRegex = new Regex("^[\\w\\.\\-]+@([\\w\\-]+\\.)*[\\w\\-]{2,63}\\.[a-zA-Z]{2,4}$");

            if (eMailRegex.IsMatch(eMail))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #region PhoneNumber von ungültigen Zeichen bereinigen
        /// <summary>
        /// Extrahiert nur Zahlen aus einen String, fügt diese zusammen und gibt einen
        /// String zurück.
        /// "+" wird in 00 umgewandelt
        /// </summary>
        /// <param name="pn"></param>
        /// <returns></returns>
        public string ClearPhoneNumber(string pn)
        {
            char[] cpn = pn.ToCharArray();
            pn = "";

            for (int i = 0; i <= cpn.GetUpperBound(0); i++)
            {

                if (cpn[i].ToString() == "0")
                    pn = pn + cpn[i];
                if (cpn[i].ToString() == "1")
                    pn = pn + cpn[i];
                if (cpn[i].ToString() == "2")
                    pn = pn + cpn[i];
                if (cpn[i].ToString() == "3")
                    pn = pn + cpn[i];
                if (cpn[i].ToString() == "4")
                    pn = pn + cpn[i];
                if (cpn[i].ToString() == "5")
                    pn = pn + cpn[i];
                if (cpn[i].ToString() == "6")
                    pn = pn + cpn[i];
                if (cpn[i].ToString() == "7")
                    pn = pn + cpn[i];
                if (cpn[i].ToString() == "8")
                    pn = pn + cpn[i];
                if (cpn[i].ToString() == "9")
                    pn = pn + cpn[i];
                if (cpn[i].ToString() == "+")
                    pn = pn + "00";

            }
            return pn;
        }
        #endregion

        /// <summary>
        /// Prüft einen Sponsor, ob für diesen alle Angaben vorhanden sind, 
        /// um einen Auftrag zu erstellen.
        /// </summary>
        /// <param name="sponsor"></param>
        /// <param name="wrongFields">eine Liste mit Feldern, die fehlerhafte oder keine Daten enthalten</param>
        /// <returns>wahr wenn alle Angaben vorhanden sind. Ansonsten Falsch</returns>
        public bool IsSponsorValidForOrdering(Sponsor sponsor,  out List<string> wrongFields )
        {
            wrongFields = new List<string>();

            if (string.IsNullOrEmpty(sponsor.Nachname))
            {
                wrongFields.Add("Nachname");
            }

            /*
             * if (string.IsNullOrEmpty(sponsor.Vorname))
                wrongFields.Add("Vorname");
             */

            if (string.IsNullOrEmpty(sponsor.Strasse))
            {
                wrongFields.Add("Strasse");
            }

            if (string.IsNullOrEmpty(sponsor.Land))
            {
                wrongFields.Add("Land");
            }

            if (string.IsNullOrEmpty(sponsor.PLZ))
            {
                wrongFields.Add("Postleitzahl");
            }

            if (string.IsNullOrEmpty(sponsor.Ort))
            {
                wrongFields.Add("Ort");
            }

            if (string.IsNullOrEmpty(sponsor.TelefonNummer))
            {
                wrongFields.Add("Telefonnummer");
            }
            else
            {
                if (ClearPhoneNumber(sponsor.TelefonNummer).Length < 4)
                {
                    wrongFields.Add("Telefonnummer");
                }
            }

            if (string.IsNullOrEmpty(sponsor.SponsorenUrkunde1))
            {
                wrongFields.Add("Sponsorurkunde Zeile 1");
            }

            if (string.IsNullOrEmpty(sponsor.EMail))
            {
                wrongFields.Add("eMail-Adresse");
            }
            else
            {
                if (!sponsor.EMail.Equals("kein eMail vorhanden", StringComparison.CurrentCultureIgnoreCase))
                {
                    if (CheckEMail(sponsor.EMail) == false)
                    {
                        wrongFields.Add("eMail-Adresse");
                    }
                }
            }

            if (sponsor.ContactPerson != null)
            {
                if (string.IsNullOrEmpty(sponsor.ContactPerson.Anrede))
                {
                    wrongFields.Add("Ansprechpartner Anrede");
                }

                if (string.IsNullOrEmpty(sponsor.ContactPerson.Vorname))
                {
                    wrongFields.Add("Ansprechpartner Vorname");
                }
                else
                {
                    if (sponsor.ContactPerson.Vorname.Length < 3)
                    {
                        if (sponsor.ContactPerson.Vorname.Length > 1)
                        {
                            if (sponsor.ContactPerson.Vorname.Substring(1, 1) != ".")
                            {
                                wrongFields.Add("Ansprechpartner Vorname");
                            }
                        }
                        else
                        {
                            wrongFields.Add("Ansprechpartner Vorname");
                        }
                    }
                }

                if (string.IsNullOrEmpty(sponsor.ContactPerson.Nachname))
                {
                    wrongFields.Add("Ansprechpartner Nachname");
                }
            }
            else
            {
                wrongFields.Add("Ansprechpartner");
            }

            return (wrongFields.Count == 0);
        }

        /// <summary>
        /// Liefert den Customer des Projektes zurück
        /// </summary>
        /// <param name="projectInfo"></param>
        /// <returns></returns>
        public Customer GetCustomer(ProjectInfo projectInfo)
        {
            if (projectInfo == null)
            {
                throw new ArgumentNullException("projectInfo");
            }

            return this.metaCallBusiness.ServiceAccess.GetCustomer(projectInfo);
        }

        /// <summary>
        /// Liefert true zurück wenn der Sponsor im Vorgängerprojekt ausgewählt hat das er dieses mal mitmachen würde.
        /// </summary>
        /// <param name="sponsor"></param>
        /// <param name="projectInfo"></param>
        /// <returns></returns>
        public Boolean GetTipAddressLastProject(Sponsor sponsor, ProjectInfo projectInfo)
        {
            if (sponsor == null)
            {
                throw new ArgumentNullException("Sponsor");
            }

            if (projectInfo == null)
            {
                throw new ArgumentNullException("Projektinfo");
            }

            return this.metaCallBusiness.ServiceAccess.GetTipAddressLastProject(sponsor, projectInfo);
        }

        public Boolean GetAddress_IsTip(int adressenPoolNummer)
        {
            return this.metaCallBusiness.ServiceAccess.GetAddress_IsTip(adressenPoolNummer);
        }

        public string GetAddress_HistoryNotice(Guid addressId)
        {
            return this.metaCallBusiness.ServiceAccess.GetAddress_HistoryNotice(addressId);
        }
    }
}
