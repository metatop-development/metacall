using System;
using System.Collections.Generic;
using System.Text;

using System.Xml;

using metatop.Applications.metaCall.DataObjects;

using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.Common;
//using System.ComponentModel;


namespace metatop.Applications.metaCall.DataAccessLayer
{
    public class CallJobUnsuitableInfoDAL
    {

        #region stored Procedures
        private const string spCallJobs_GetListUnsuitableByProject = "dbo.CallJobs_GetListUnsuitableByProject";
        private const string spCallJobs_GetListUnsuitableUsersByProject = "dbo.CallJobs_GetListUnsuitableUsersByProject";
        private const string spCallJobs_GetListUnsuitableReasonsByProject = "dbo.CallJobs_GetListUnsuitableReasonsByProject";
        private const string spCallJobs_UpdateUnsuitableAddressChanges = "dbo.CallJobs_UpdateUnsuitableAddressChanges";
        private const string spCallJobs_GetUnsuitableAddressPercentageByProject = "dbo.CallJobs_GetUnsuitableAddressPercentageByProject";
        #endregion

        /// <summary>
        /// Liefert Calljobs eines Projekts die als ungeeignet, Nummer falsch oder Adresse
        /// doppelt gekennzeichnet sind.
        /// </summary>
        /// <param name="project"></param>
        /// <param name="userId"></param>
        /// <param name="contactTypesParticipationUnsuitableId"></param>
        /// <returns></returns>
        public static CallJobUnsuitableInfo[] GetListCallJobsUnsuitableInfoByProject(Project project,
            Guid userId, Guid contactTypesParticipationUnsuitableId)
        {
            // Die Standardparameter (Guid) für die UserId und contactTypesParticipationUnsuitableId
            // sind in den Stored Procedures:
            // Project_GetListCallJobsUnsuitableUsers und
            // CallJobs_GetListUnsuitableUsersByProject
            // als UNION Select hinterlegt
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@ProjectId", project == null ? null : (Guid?)project.ProjectId);
            parameters.Add("@UserId", userId.CompareTo(new Guid("9879A15E-DB1C-46C9-9DE4-74D34B3334C6")) == 0 ? null : (Guid?)userId);
            parameters.Add("@ContactTypesParticipationUnsuitableId", 
                    contactTypesParticipationUnsuitableId.CompareTo(new Guid("544AD687-5B8F-490B-9531-DC06AF0C0895")) == 0 ? 
                    null : (Guid?)contactTypesParticipationUnsuitableId);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spCallJobs_GetListUnsuitableByProject, parameters);

            return ConvertToCallJobUnsuitableInfos(dataTable);
        }

        private static CallJobUnsuitableInfo ConvertToCallJobUnsuitableInfo(DataRow row)
        {
            CallJobUnsuitableInfo cui = new CallJobUnsuitableInfo();

            cui.CallJobId = (Guid)row["CallJobId"];
            cui.Nachname = (string)SqlHelper.GetNullableDBValue(row["Nachname"]);
            cui.Vorname = (string)SqlHelper.GetNullableDBValue(row["Vorname"]);
            cui.Text1 = (string)SqlHelper.GetNullableDBValue(row["Text1"]);
            cui.PLZ = (string)SqlHelper.GetNullableDBValue(row["PLZ"]);
            cui.Ort = (string)SqlHelper.GetNullableDBValue(row["Ort"]);
            cui.Quelle = (string)SqlHelper.GetNullableDBValue(row["Quelle"]);
            cui.DisplayNameContactType = (string)SqlHelper.GetNullableDBValue(row["DisplayNameContactType"]);
            cui.DisplayNameUnsuitableType = (string)SqlHelper.GetNullableDBValue(row["DisplayNameUnsuitableType"]);
            cui.Start = (DateTime)row["Start"];
            cui.Agent = (string)SqlHelper.GetNullableDBValue(row["Agent"]);
            cui.PhoneNumber = (string)SqlHelper.GetNullableDBValue(row["PhoneNumber"]);
            cui.AdresseNichtGeeignet = (Boolean)row["AdresseNichtGeeignet"];
            cui.AddressId = (Guid)row["AddressId"];
            cui.AdressenPoolNummer = (int)row["AdressenPoolNummer"];
            cui.ContactTypesParticipationUnsuitableId = (Guid)row["ContactTypesParticipationUnsuitableId"];
            cui.UserId = (Guid)row["UserId"];
            //wird nicht benötigt, da die gleichen Daten selten aufgerufen werden
            //ObjectCache.Add(cui.CallJobId, cui, TimeSpan.FromSeconds(20));

            return cui;
        }

        private static CallJobUnsuitableInfo[] ConvertToCallJobUnsuitableInfos(DataTable dataTable)
        {
            CallJobUnsuitableInfo[] cuis = new CallJobUnsuitableInfo[dataTable.Rows.Count];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[i];
                cuis[i] = ConvertToCallJobUnsuitableInfo(row);
            }

            return cuis;
        }


        public static CallJobUnsuitableInfoUser[] GetListCallJobsUnsuitableInfoUsersByProject(Project project)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@ProjectId", project == null ? null : (Guid?)project.ProjectId);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spCallJobs_GetListUnsuitableUsersByProject, parameters);

            return ConvertToCallJobUnsuitableInfosUsers(dataTable);
        }

        private static CallJobUnsuitableInfoUser ConvertToCallJobUnsuitableInfoUser(DataRow row)
        {
            CallJobUnsuitableInfoUser cuiu = new CallJobUnsuitableInfoUser();

            cuiu.UserId = (Guid)row["UserId"];
            cuiu.DisplayName = (string)row["DisplayName"];

            //wird nicht benötigt, da die gleichen Daten selten aufgerufen werden
            //ObjectCache.Add(cui.CallJobId, cui, TimeSpan.FromSeconds(20));

            return cuiu;
        }

        private static CallJobUnsuitableInfoUser[] ConvertToCallJobUnsuitableInfosUsers(DataTable dataTable)
        {
            CallJobUnsuitableInfoUser[] cuius = new CallJobUnsuitableInfoUser[dataTable.Rows.Count];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[i];
                cuius[i] = ConvertToCallJobUnsuitableInfoUser(row);
            }

            return cuius;
        }

        public static CallJobUnsuitableInfoReason[] GetListCallJobsUnsuitableInfoReasonsByProject(Project project)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@ProjectId", project == null ? null : (Guid?)project.ProjectId);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spCallJobs_GetListUnsuitableReasonsByProject, parameters);

            return ConvertToCallJobUnsuitableInfosReasons(dataTable);
        }

        private static CallJobUnsuitableInfoReason ConvertToCallJobUnsuitableInfoReason(DataRow row)
        {
            CallJobUnsuitableInfoReason cuir = new CallJobUnsuitableInfoReason();

            cuir.ContactTypesParticipationUnsuitableId = (Guid)row["ContactTypesParticipationUnsuitableId"];
            cuir.DisplayName = (string)row["DisplayName"];

            //wird nicht benötigt, da die gleichen Daten selten aufgerufen werden
            //ObjectCache.Add(cui.CallJobId, cui, TimeSpan.FromSeconds(20));

            return cuir;
        }

        private static CallJobUnsuitableInfoReason[] ConvertToCallJobUnsuitableInfosReasons(DataTable dataTable)
        {
            CallJobUnsuitableInfoReason[] cuirs = new CallJobUnsuitableInfoReason[dataTable.Rows.Count];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[i];
                cuirs[i] = ConvertToCallJobUnsuitableInfoReason(row);
            }

            return cuirs;
        }

        public static void UpdateCallJobsUnsuitableAddressChanges(CallJobUnsuitableAddressChanges[] callJobAddressChanges)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@AddressChangesXml", GetAddressChangesXml(callJobAddressChanges));

            SqlHelper.ExecuteStoredProc(spCallJobs_UpdateUnsuitableAddressChanges, parameters);

        }

        private static object GetAddressChangesXml(CallJobUnsuitableAddressChanges[] callJobAddressChanges)
        {
            //Erstellt eine XML-Zeichenfolge der Art
            // <centerAdmins>
            //    <CenterAdmin UserId=".." />
            // </CenterAdmins>

            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;

            using (XmlWriter writer = XmlWriter.Create(sb, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("Addresses");

                foreach (CallJobUnsuitableAddressChanges callJobAddressChange in callJobAddressChanges)
                {
                    writer.WriteStartElement("Address");
                    writer.WriteAttributeString("AdressenPoolNummer", callJobAddressChange.AdressenPoolNummer.ToString());
                    writer.WriteAttributeString("AdresseNichtGeeignet", Convert.ToInt16(callJobAddressChange.AdresseNichtGeeignet).ToString());
                    writer.WriteAttributeString("ContactTypesParticipationUnsuitableId", callJobAddressChange.ContactTypesParticipationUnsuitableId.ToString());
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
            }

            return sb.ToString();
        }

        public static double GetUnsuitableAddressPercentageByProject(Project project)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@ProjectId", project == null ? null : (Guid?)project.ProjectId);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spCallJobs_GetUnsuitableAddressPercentageByProject, parameters);

            DataRow row = dataTable.Rows[0];

            return Convert.ToDouble(row["AnteilBestaetigteAdressen"]);
        }

    }
}
