using System;
using System.Collections.Generic;
using System.Text;
using metatop.Applications.metaCall.DataObjects;

using System.Data;
using System.Xml;


namespace metatop.Applications.metaCall.DataAccessLayer
{
    public static class ContactReportDAL
    {

        private const string spContactReport_GetSingle = "dbo.ContactReport_GetSingle";

        /// <summary>
        /// Liefert einen Kontaktreport
        /// </summary>
        /// <param name="CallJobId"></param>
        /// <returns></returns>
        public static ContactReport[] GetContactReport(Guid callJobId)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@CallJobId", callJobId);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spContactReport_GetSingle, parameters);

            return ConvertToContactReports(dataTable);


        }

        private static ContactReport[] ConvertToContactReports(DataTable dataTable)
        {
            ContactReport[] contactReports = new ContactReport[dataTable.Rows.Count];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[i];
                contactReports[i] = ConvertToContactReport(row);
            }
            return contactReports;
        }

        private static ContactReport ConvertToContactReport(DataRow row)
        {
            ContactReport contactReport = new ContactReport();

            contactReport.AbsageArt = (string)SqlHelper.GetEmptyByNull(row["AbsageArt"]);
            contactReport.AdressenPoolNummer = (int)row["AdressenpoolNummer"];
            contactReport.AgentNachname = (string)SqlHelper.GetEmptyByNull(row["AgentNachname"]);
            contactReport.AgentVorname = (string)SqlHelper.GetEmptyByNull(row["AgentVorname"]);
            contactReport.Anrede = (string)SqlHelper.GetEmptyByNull(row["Anrede"]);
            contactReport.Antwort = (string)SqlHelper.GetEmptyByNull(row["Antwort"]);
            contactReport.Center = (string)SqlHelper.GetEmptyByNull(row["Center"]);
            contactReport.Fax = (string)SqlHelper.GetEmptyByNull(row["Fax"]);
            contactReport.Kontaktart = (string)SqlHelper.GetEmptyByNull(row["Kontaktart"]);
            contactReport.Land = (string)SqlHelper.GetEmptyByNull(row["Land"]);
            contactReport.Mobil = (string)SqlHelper.GetEmptyByNull(row["Mobil"]);
            contactReport.Nachname = (string)SqlHelper.GetEmptyByNull(row["Nachname"]);
            contactReport.Ort = (string)SqlHelper.GetEmptyByNull(row["Ort"]);
            contactReport.PLZ = (string)SqlHelper.GetEmptyByNull(row["PLZ"]);
            contactReport.Projekt = (string)SqlHelper.GetEmptyByNull(row["Projekt"]);
            contactReport.Projektnummer = (int)row["Projektnummer"];
            contactReport.Start = (DateTime)row["Start"];
            contactReport.Stop = (DateTime)row["Stop"];
            contactReport.Strasse = (string)SqlHelper.GetEmptyByNull(row["Strasse"]);
            contactReport.Telefon = (string)SqlHelper.GetEmptyByNull(row["Telefon"]);
            contactReport.Text1 = (string)SqlHelper.GetEmptyByNull(row["Text1"]);
            contactReport.Text2 = (string)SqlHelper.GetEmptyByNull(row["Text2"]);
            contactReport.Titel = (string)SqlHelper.GetEmptyByNull(row["Titel"]);
            contactReport.Vorname = (string)SqlHelper.GetEmptyByNull(row["Vorname"]);

            return contactReport;
        }
    }
}
