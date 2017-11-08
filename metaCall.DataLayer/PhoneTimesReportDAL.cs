using System;
using System.Collections.Generic;
using System.Text;
using metatop.Applications.metaCall.DataObjects;

using System.Data;
using System.Xml;


namespace metatop.Applications.metaCall.DataAccessLayer
{
    public static class PhoneTimesReportDAL
    {

        private const string spPhoneTimesReport = "dbo.Team_PhoneTime";

        /// <summary>
        /// Liefert einen Telefonzeitenreport
        /// </summary>
        /// <param name="CallJobId"></param>
        /// <returns></returns>
        public static PhoneTimesReport[] GetPhoneTimesReport(Guid teamId, Guid userId, DateTime start, DateTime stop)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            if (teamId == Guid.Empty)
                parameters.Add("@TeamId", null);
            else
                parameters.Add("@TeamId", teamId);
            parameters.Add("@SelectUserId", userId);
            parameters.Add("@Start", start);
            parameters.Add("@Stop", stop);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spPhoneTimesReport, parameters);

            return ConvertToPhoneTimesReports(dataTable);


        }

        private static PhoneTimesReport[] ConvertToPhoneTimesReports(DataTable dataTable)
        {
            PhoneTimesReport[] PhoneTimesReports = new PhoneTimesReport[dataTable.Rows.Count];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[i];
                PhoneTimesReports[i] = ConvertToPhoneTimesReport(row);
            }
            return PhoneTimesReports;
        }

        private static PhoneTimesReport ConvertToPhoneTimesReport(DataRow row)
        {
            PhoneTimesReport PhoneTimesReport = new PhoneTimesReport();

            PhoneTimesReport.Agent = (string)SqlHelper.GetEmptyByNull(row["Agent"]);
            PhoneTimesReport.Start = (DateTime)row["Start"];
            PhoneTimesReport.GewerteteArbeitszeit = (double)SqlHelper.GetNullableDBValue(row["GewerteteArbeitszeit"]);
            PhoneTimesReport.Telefonzeit = (double)SqlHelper.GetNullableDBValue(row["Telefonzeit"]);
            PhoneTimesReport.AnteilTelefonzeit = (double)SqlHelper.GetNullableDBValue(row["AnteilTelefonzeit"]);

            return PhoneTimesReport;
        }
    }
}
