using System;
using System.Collections.Generic;
using System.Text;
using metatop.Applications.metaCall.DataObjects;
using System.Data;
using System.Xml;
using System.ComponentModel;
using System.Data.SqlTypes;

namespace metatop.Applications.metaCall.DataAccessLayer
{
    public static class AddressDAL
    {
        #region Stored Procedures
        private const string spProject_TransferAdresses = "dbo.Project_TransferAddresses";
        private const string spAddress_GetFailureByProject = "dbo.Address_GetFailureByProject";
        private const string spAddress_DeleteFailureByProject = "dbo.Address_DeleteFailureByProject";
        private const string spAddress_BlockCallJobsWithMissingAddresses = "dbo.Address_BlockCallJobsWithMissingAddresses";
        private const string spAddresses_GetNewListForTransfer = "dbo.Addresses_GetNewListForTransfer";
        private const string spProject_GetSponsors = "dbo.Address_GetListByProject";
        private const string spAddresses_GetSingle = "dbo.Addresses_GetSingle";
        //private const string spAddresses_GetSingleByVereinsNummer = "dbo.Addresses_GetSingle";
        private const string spAddress_Sponsor_Update = "dbo.Address_Sponsor_Update";
        private const string spAddress_Sponsor_Create = "dbo.Address_Sponsor_Create";
        private const string spInsert_Address_Into_Ausschlussliste = "dbo.InsertAddressIntoAusschlussliste";
        private const string spAddress_GetHistoryNotice = "dbo.Address_GetHistoryNotice";
        private const string spAddresses_GetNoticeAdministration = "dbo.Address_GetNoticeAdministration";
        private const string spGeoZone_GetByProjectAndSponsor = "dbo.GeoZone_GetByProjectAndSponsor";
        // -> dummy falls GeoZone mal separat gespeichert wird // private const string spGeoZone_GetSingle = "dbo.GeoZone_GetSingle";
        private static string spSponsorOrderInfo_GetBySponsorProjectAndCustomer = "dbo.SponsorOrderInfo_GetBySponsorProjectAndCustomer";
        private static string spSponsorTipAddressFromLastProject = "dbo.SponsorTipAddressFromLastProject";
        private static string spAddress_IsTip = "dbo.Address_IsTip";
        #endregion

        #region AdressBase Operations

        public static AddressBase GetAddress(Guid addressId)
        {
            DataTable dataTable = GetAddressTable(addressId);

            if (dataTable.Rows.Count < 1)
            {
                return null;
            }
            else
            {
                string addressType = (string) dataTable.Rows[0]["AddressType"];

                if (addressType == typeof(Sponsor).FullName)
                {
                    return ConvertToSponsor(dataTable.Rows[0]);
                }
                else if (addressType == typeof(Customer).FullName)
                {
                    return ConvertToCustomer(dataTable.Rows[0]);
                }
                else
                {
                    return null;
                }
            }
        }

        private static DataTable GetAddressTable(Guid addressId)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@AddressId", addressId);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spAddresses_GetSingle, parameters);

            return dataTable;
        }

        public static string GetAddress_HistoryNotice(Guid addressId)
        {
            string historyNotice;

            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@AddressId", addressId);
            DataTable dataTable = SqlHelper.ExecuteDataTable(spAddress_GetHistoryNotice, parameters);

            if (dataTable.Rows.Count < 1)
            {
                return null;
            }
            else
            {
                historyNotice = (string)SqlHelper.GetNullableDBValue(dataTable.Rows[0]["Notiz"]);
                return historyNotice;
            }
        }

        public static string GetAddress_NoticeAdministration(Guid addressId)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@AddressId", addressId);
            DataTable dataTable = SqlHelper.ExecuteDataTable(spAddresses_GetNoticeAdministration, parameters);

            if (dataTable.Rows.Count < 1)
            {
                return null;
            }
            else
            {
                return (string)SqlHelper.GetNullableDBValue(dataTable.Rows[0]["NotizVerwaltung"]);
            }
        }

        private static void FillAddressBase(AddressBase addressBase, DataRow row)
        {
            addressBase.AddressId = (Guid)row["AddressId"];
            addressBase.AdressenPoolNummer = (int)row["AdressenPoolNummer"];
            addressBase.Anrede = (string)SqlHelper.GetNullableDBValue(row["Anrede"]);
            addressBase.DateCreated = (DateTime)row["DateCreated"];
            addressBase.DateLastUsed = (DateTime)row["DateLastUsed"];
            addressBase.FaxNummer = (string)SqlHelper.GetNullableDBValue(row["Fax"]);
            addressBase.Land = (string)SqlHelper.GetNullableDBValue(row["Land"]);
            addressBase.MobilNummer = (string)SqlHelper.GetNullableDBValue(row["Mobil"]);
            addressBase.Nachname = (string)row["Nachname"];
            addressBase.Ort = (string)SqlHelper.GetNullableDBValue(row["Ort"]);
            addressBase.PLZ = (string)SqlHelper.GetNullableDBValue(row["PLZ"]);
            addressBase.Strasse = (string)SqlHelper.GetNullableDBValue(row["Strasse"]);
            addressBase.TelefonNummer = (string)SqlHelper.GetNullableDBValue(row["Telefon"]);
            addressBase.Text1 = (string)SqlHelper.GetNullableDBValue(row["Text1"]);
            addressBase.Text2 = (string)SqlHelper.GetNullableDBValue(row["Text2"]);
            addressBase.Vorname = (string)SqlHelper.GetNullableDBValue(row["Vorname"]);
            addressBase.ContactPerson = ConvertToContactPerson(row);
            addressBase.EMail = (string)SqlHelper.GetNullableDBValue(row["EMail"]);
            addressBase.Zusatz = (string)SqlHelper.GetNullableDBValue(row["Zusatz"]);
            addressBase.Webadresse = (string) SqlHelper.GetNullableDBValue(row["Webadresse"]);
            
            return;
        }
        private static ContactPerson ConvertToContactPerson(DataRow row)
        {
            ContactPerson contactPerson = new ContactPerson();

            contactPerson.Anrede = (string)SqlHelper.GetNullableDBValue(row["AnsprechpartnerAnrede"]);
            contactPerson.Nachname = (string)SqlHelper.GetNullableDBValue(row["AnsprechpartnerNachname"]);
            contactPerson.Titel = (string)SqlHelper.GetNullableDBValue(row["AnsprechpartnerTitel"]);
            contactPerson.Vorname = (string)SqlHelper.GetNullableDBValue(row["AnsprechpartnerVorname"]);

            return contactPerson;
        }
        #endregion

        #region Sponsor Operations
        /// <summary>
        /// Aktualisiert einen Sponsor auf dem Server 
        /// Die Tabellen metawareadressenPool..tblAdressenPool sowie
        /// metaware2004..tblSponsoren sind betroffen
        /// </summary>
        /// <param name="sponsor">Sponsor der aktualisiert werden soll</param>
        public static void UpdateSponsor(Sponsor sponsor)
        {
            IDictionary<string, object> parameters = GetParameters(sponsor);
            SqlHelper.ExecuteStoredProc(spAddress_Sponsor_Update, parameters);
        }

        private static IDictionary<string, object> GetParameters(Sponsor sponsor)
        {
            IDictionary<string, object> parameters = GetParameters((AddressBase)sponsor);
            
            int? branchNumber  = null;

            if (sponsor.Branch != null)
            {
                branchNumber = (int?)SqlHelper.GetNullableDBValue(sponsor.Branch.Branchennummer);
                if (branchNumber == 0)
                {
                    branchNumber = null;
                }
            }

            Guid? branchGroupID = null;
            if (sponsor.BranchGroup != null)
            {
                branchGroupID = (Guid?)SqlHelper.GetNullableDBValue(sponsor.BranchGroup.BranchenGruppenID);
            }

            parameters.Add("@BranchenNummer", branchNumber);
            parameters.Add("@BranchenGruppenID", branchGroupID);
            parameters.Add("@Bank", sponsor.Bank);
            parameters.Add("@Bankleitzahl", sponsor.BankNumber);
            parameters.Add("@KontoNummer", sponsor.AccountNumber);
            parameters.Add("@Addition_Phone1", sponsor.Additions.Phone1);
            parameters.Add("@Addition_Phone2", sponsor.Additions.Phone2);
            parameters.Add("@Addition_Phone3", sponsor.Additions.Phone3);
            parameters.Add("@SponsorUrkunde1", sponsor.SponsorenUrkunde1);
            parameters.Add("@SponsorUrkunde2", sponsor.SponsorenUrkunde2);

            return parameters;
        }

        private static IDictionary<string, object> GetParameters(AddressBase address)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@AddressId", address.AddressId);
            parameters.Add("@AdressenPoolNummer", address.AdressenPoolNummer);
            parameters.Add("@Anrede", address.Anrede);
            parameters.Add("@Titel", address.Titel);
            parameters.Add("@Nachname", address.Nachname);
            parameters.Add("@Vorname", address.Vorname);
            parameters.Add("@Text1", address.Text1);
            parameters.Add("@text2", address.Text2);
            parameters.Add("@Strasse", address.Strasse);
            parameters.Add("@Land", address.Land);
            parameters.Add("@PLZ", address.PLZ);
            parameters.Add("@Ort", address.Ort);
            parameters.Add("@Telefon", address.TelefonNummer);
            parameters.Add("@Fax", address.FaxNummer);
            parameters.Add("@Mobil", address.MobilNummer);
            parameters.Add("@ContactpersonsXml", GetContactPersonsXml(address));
            parameters.Add("@Email", address.EMail);
            parameters.Add("@Webadresse", address.Webadresse);

            return parameters;
        }

        private static string GetContactPersonsXml(AddressBase address)
        {
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;

            using (XmlWriter writer = XmlWriter.Create(sb, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("ContactPersons");


                if (address.ContactPerson != null)
                {
                    writer.WriteStartElement("ContactPerson");
                    writer.WriteAttributeString("Anrede", address.ContactPerson.Anrede);
                    writer.WriteAttributeString("Titel", address.ContactPerson.Titel);
                    writer.WriteAttributeString("Nachname", address.ContactPerson.Nachname);
                    writer.WriteAttributeString("Vorname", address.ContactPerson.Vorname);
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }

            return sb.ToString();
        }
        
        public static Sponsor GetSponsor(Guid addressId)
        {
            DataTable dataTable = GetAddressTable(addressId);

            return dataTable.Rows.Count < 1 ? null : ConvertToSponsor(dataTable.Rows[0]);
        }

        public static Sponsor[] GetSponsorsByProject(Guid projectId)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@ProjectId", projectId);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spProject_GetSponsors, parameters, 0);

            return ConvertToSponsors(dataTable);
        }

        private static Sponsor ConvertToSponsor(DataRow row)
        {
            Sponsor sponsor = new Sponsor();
            FillAddressBase(sponsor, row);

            //Abrufen der Branche
            int? branchNumber = (int?) SqlHelper.GetNullableDBValue(row["Branchennummer"]);

            sponsor.Branch = BranchDAL.GetBranch(branchNumber);
            object branchGroupNumber = SqlHelper.GetNullableDBValue(row["BranchenGruppenID"]);
            if (branchGroupNumber != null)
            {
                sponsor.BranchGroup = BranchGroupDAL.GetBranchGroup(new Guid((string)branchGroupNumber));
//                sponsor.BranchGroup = BranchGroupDAL.GetBranchGroup((Guid)branchGroupNumber);
            }

            sponsor.Bank = (string)SqlHelper.GetNullableDBValue(row["Bank"]);
            sponsor.BankNumber = (string)SqlHelper.GetNullableDBValue(row["Bankleitzahl"]);
            sponsor.AccountNumber = (string)SqlHelper.GetNullableDBValue(row["KontoNummer"]);
            sponsor.Additions = new SponsorAdditions();
            sponsor.Additions.Phone1 = (string) SqlHelper.GetNullableDBValue(row["Phone1"]);
            sponsor.Additions.Phone2 = (string) SqlHelper.GetNullableDBValue(row["Phone2"]);
            sponsor.Additions.Phone3 = (string) SqlHelper.GetNullableDBValue(row["Phone3"]);
            sponsor.SponsorenUrkunde1 = (string) SqlHelper.GetNullableDBValue(row["SponsorUrkunde1"]);
            sponsor.SponsorenUrkunde2 = (string)SqlHelper.GetNullableDBValue(row["SponsorUrkunde2"]);

            return sponsor;
        }

        private static Sponsor[] ConvertToSponsors(DataTable dataTable)
        {
            Sponsor[] sponsors = new Sponsor[dataTable.Rows.Count];

            int totalRows = dataTable.Rows.Count;

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[i];
                sponsors[i] = ConvertToSponsor(row);
            }

            return sponsors;
        }
        #endregion

        #region CustomerOperations
        /// <summary>
        /// Liefert ein Customer-Objekt zurück
        /// </summary>
        /// <param name="Vereinsnummer">Vereinsnumer aus der Metaware-Anwendung</param>
        /// <returns></returns>
        public static Customer GetCustomer(int Vereinsnummer)
        {
            return null;
        }

        /// <summary>
        /// Liefert ein Customer-Objekt zurück
        /// </summary>
        /// <param name="addressId">AddressId aus der metaCall-Anwendung</param>
        /// <returns></returns>
        public static Customer GetCustomer(Guid? addressId)
        {
            if (!addressId.HasValue)
            {
                return null;
            }

            DataTable dataTable = GetAddressTable(addressId.Value);

            return dataTable.Rows.Count < 1 ? null : ConvertToCustomer(dataTable.Rows[0]);
        }

        private static Customer ConvertToCustomer(DataRow row)
        {
            Customer customer = new Customer();
            FillAddressBase(customer, row);

            customer.Vereinsnummer = (int)row["Vereinsnummer"];
            customer.Abteilung = (string) SqlHelper.GetNullableDBValue(row["Abteilung"]);

            return customer;
        }    
        #endregion

        #region GeoZone Operations
        public static GeoZone GetGeoZone(Sponsor sponsor, Guid projectId)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@ProjectId", projectId);
            parameters.Add("@AddressId", sponsor.AddressId);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spGeoZone_GetByProjectAndSponsor, parameters);

            return dataTable.Rows.Count < 1 ? null : ConvertToGeoZone(dataTable.Rows[0]);
        }

        public static GeoZone ConvertToGeoZone(DataRow row)
        {
            GeoZone geoZone = new GeoZone();

            if (SqlHelper.GetNullableDBValue(row["GeoZone"]) == null)
            {
                geoZone.Zone = 99;
            }
            else
            {
                geoZone.Zone = (int)row["GeoZone"];
            }

            return geoZone;
        }
        #endregion

        #region SponsorOrderInfo
        public static SponsorOrderInfo[] GetAllSponsorOrderInfos(Sponsor sponsor, Guid projectId)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@AddressId", sponsor.AddressId);
            parameters.Add("@ProjectId", projectId);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spSponsorOrderInfo_GetBySponsorProjectAndCustomer, parameters);

            return ConvertToSponsorOrderInfos(dataTable);
        }

        private static SponsorOrderInfo[] ConvertToSponsorOrderInfos(DataTable dataTable)
        {
            SponsorOrderInfo[] infos = new SponsorOrderInfo[dataTable.Rows.Count];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[i];
                infos[i] = ConvertToSponsorOrderInfo(row);
            }

            return infos;
            
        }

        private static SponsorOrderInfo ConvertToSponsorOrderInfo(DataRow row)
        {
            SponsorOrderInfo info = new SponsorOrderInfo();

            int? projectYear = (int?)SqlHelper.GetNullableDBValue(row["ProjectYear"]);
            int? projectMonth = (int?)SqlHelper.GetNullableDBValue(row["ProjectMonth"]);
            string projectOrderState;
            if(SqlHelper.GetNullableDBValue(row["OrderState"])==null)
                projectOrderState = "beauftragt";
            else
                projectOrderState = (string)row["OrderState"];

            info.ProjectNumber = (int)row["ProjectNumber"];
            info.ProjectYear = projectYear.HasValue ? projectYear.Value : -1;
            info.ProjectMonth = projectMonth.HasValue ? projectMonth.Value : -1;
            info.OrderNumber = (int)row["OrderNumber"];
            info.OrderState = projectOrderState;
            //info.OrderState = (string)row["OrderState"];
            info.Ranking = (int)row["Ranking"];
            return info;
        }
        #endregion

        public static void TransferAddressPool(Project project)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@ProjectId", project.ProjectId);
            parameters.Add("@TargetAddressType", typeof(Sponsor).FullName);
            parameters.Add("@CurrentUser", Guid.NewGuid());
            parameters.Add("@ProjectStopDate", DateTime.MaxValue);
            parameters.Add("@AddressStartUpStatus", ProjectSponsorAssignState.Open);

            DataTable dataTable =  SqlHelper.ExecuteDataTable(spProject_TransferAdresses, parameters, 0);

            if (project.Sponsors == null)
            {
                project.Sponsors = new Sponsor[0];
            }

            //sichern der existierenden Sponsoren
            List<Sponsor> sponsors = new List<Sponsor>(project.Sponsors);

            sponsors.AddRange(ConvertToSponsors(dataTable));

            project.Sponsors = sponsors.ToArray();

        }

        public static int GetFailureByProject(ProjectInfo project)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@ProjectId", project.ProjectId);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spAddress_GetFailureByProject, parameters);

            return (int)dataTable.Rows[0][0];
        }

        public static void DeleteFailureByProject(ProjectInfo project)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@ProjectId", project.ProjectId);

            SqlHelper.ExecuteStoredProc(spAddress_DeleteFailureByProject, parameters);

            return;
        }

        public static void BlockCallJobsWithMissingAddresses()
        {
            SqlHelper.ExecuteStoredProc(spAddress_BlockCallJobsWithMissingAddresses, null);
            return;
        }

        public static Sponsor[] GetNewListForTransfer(Project project)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@ProjectId", project.ProjectId);


            DataTable dataTable = SqlHelper.ExecuteDataTable(spAddresses_GetNewListForTransfer, parameters);

            return ConvertToSponsors(dataTable);
        }

        /// <summary>
        /// Erstellt einen neuen Sponsor auf dem Server
        /// </summary>
        /// <param name="sponsor"></param>
        public static void CreateSponsor(Sponsor sponsor, Project project)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@ProjectId", project.ProjectId);
            parameters.Add("@ProjectStopDate", DateTime.MaxValue);
            parameters.Add("@AddressStartUpStatus", (int)ProjectSponsorAssignState.Open);
            parameters.Add("@AddressId", sponsor.AddressId);
            parameters.Add("@AdressenPoolNummer", sponsor.AdressenPoolNummer);
            parameters.Add("@AddressType", sponsor.GetType().FullName);
            parameters.Add("@Bank", sponsor.Bank);
            parameters.Add("@Bankleitzahl", sponsor.BankNumber);
            parameters.Add("@KontoNummer", sponsor.AccountNumber);
            parameters.Add("@Addition_Phone1", sponsor.Additions.Phone1);
            parameters.Add("@Addition_Phone2", sponsor.Additions.Phone2);
            parameters.Add("@Addition_Phone3", sponsor.Additions.Phone3);
            parameters.Add("@SponsorUrkunde1", sponsor.SponsorenUrkunde1);
            parameters.Add("@SponsorUrkunde2", sponsor.SponsorenUrkunde2);

            SqlHelper.ExecuteStoredProc(spAddress_Sponsor_Create, parameters);
        }

        public static void InsertAddressIntoAusschlussliste(int Adressenpoolnummer, int Projektnummer)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@AusschlussTyp", "V");
            parameters.Add("@AdressenPoolNummer", Adressenpoolnummer);
            parameters.Add("@ReferenzNummer", Projektnummer);
            parameters.Add("@Grund", "metacall - Telefonat");

            SqlHelper.ExecuteStoredProc(spInsert_Address_Into_Ausschlussliste, parameters);
        }

        #region Tip-Adresse
        public static Boolean GetTipAddressLastProject(Sponsor sponsor, ProjectInfo projectInfo)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@AddressId", sponsor.AddressId);
            parameters.Add("@ProjectId", projectInfo.ProjectId);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spSponsorTipAddressFromLastProject, parameters);

            return dataTable.Rows.Count < 1 ? false : true;
        }

        public static Boolean GetAddress_IsTip(int adressenPoolNummer)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@AdressenPoolNummer", adressenPoolNummer);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spAddress_IsTip, parameters);

            return dataTable.Rows.Count < 1 ? false : true;
        }
        #endregion
    }
}
