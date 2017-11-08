using System;
using System.Collections.Generic;
using System.Text;
using metatop.Applications.metaCall.DataObjects;

using System.Data;
using System.Xml;


namespace metatop.Applications.metaCall.DataAccessLayer
{
    public static class ProjectReportDAL
    {

        private const string spProjectReport_GetSingle = "dbo.ProjectReport_GetSingle";
        private const string spProjectReport = "dbo.ProjectReport";

        /// <summary>
        /// Liefert einen Projectreport
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <returns></returns>
        public static ProjectReport[] GetProjectReport(Guid projectId)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@ProjectId", projectId);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spProjectReport_GetSingle, parameters);

            return ConvertToProjectReports(dataTable);


        }

        private static ProjectReport[] ConvertToProjectReports(DataTable dataTable)
        {
            ProjectReport[] projectReports = new ProjectReport[dataTable.Rows.Count];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[i];
                projectReports[i] = ConvertToProjectReport(row);
            }
            return projectReports;
        }

        private static ProjectReport ConvertToProjectReport(DataRow row)
        {
            ProjectReport projectReport = new ProjectReport();

            projectReport.Art = (string)SqlHelper.GetEmptyByNull(row["Art"]);
            projectReport.Projekt = (string)SqlHelper.GetEmptyByNull(row["Projekt"]);
            projectReport.Projektnummer = (int)row["Projektnummer"];
            projectReport.CountCallJobs = (int)row["CountCallJobs"];
            projectReport.DisplayName = (string)SqlHelper.GetEmptyByNull(row["DisplayName"]);
            projectReport.CountCallJobStates = (int)row["CountCallJobStates"];

            return projectReport;
        }

        /// <summary>
        /// Liefert einen ProjectreportDetails
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <returns></returns>
        public static ProjectReportDetail GetProjectReportDetail(Guid projectId, int art)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@ProjectId", projectId);
            parameters.Add("@ArbeitsZeitHeute", art);
            DataSet dataSet = SqlHelper.ExecuteDataSet(spProjectReport, parameters);

            return ConvertToProjectReportsDetail(dataSet);


        }
        
        private static ProjectReportDetail ConvertToProjectReportsDetail(DataSet dataSet)
        {
            ProjectReportDetail prd = new ProjectReportDetail();
            DataTable dtSummen = dataSet.Tables[0];
            DataTable dtDaten = dataSet.Tables[1];
            DataTable dtGeoZonen = dataSet.Tables[2];

            List<ProjectReportDetailSummen> summen = new List<ProjectReportDetailSummen>();
            List<ProjectReportDetailDaten> daten = new List<ProjectReportDetailDaten>();
            List<ProjectReportGeozonen> geoZonen = new List<ProjectReportGeozonen>();

            foreach(DataRow row in dtSummen.Rows)
            {
                summen.Add(ConvertToProjectReportDetailSummen(row));
            }
            prd.Summen = summen;

            foreach (DataRow row in dtDaten.Rows)
            {
                daten.Add(ConvertToProjectReportDetailDaten(row));
            }
            prd.Daten = daten;

            foreach (DataRow row in dtGeoZonen.Rows)
            {
                geoZonen.Add(ConvertToProjectReportDetailGeoZonen(row));
            }
            prd.GeoZonen = geoZonen;
            
            return prd;
        }


        private static ProjectReportDetailSummen ConvertToProjectReportDetailSummen(DataRow row)
        {
            //Projekt, ReminderDateMax, DatumAkquisaAnfang, Sorter, Agent, Stunden, 
            //Umsatz, PaketeGesamt, PaketeBezahlt, PaketeGeozone, PaketeSponsoren 
            ProjectReportDetailSummen prds = new ProjectReportDetailSummen();

            prds.Projekt = (string)SqlHelper.GetEmptyByNull(row["Projekt"]);
            if (row["ReminderDateMax"] != DBNull.Value)
            {
                prds.ReminderDateMax = (DateTime)row["ReminderDateMax"];
            }
            if (row["DatumAkquisaAnfang"] != DBNull.Value)
                prds.DatumAkquisaAnfang = (DateTime)row["DatumAkquisaAnfang"];
            prds.Sorter = (int)row["Sorter"];
            prds.Agent = (string)SqlHelper.GetEmptyByNull(row["Agent"]);
            prds.Stunden = Convert.ToSingle(SqlHelper.GetNullableDBValue(row["Stunden"]));
            prds.Umsatz = Convert.ToSingle(SqlHelper.GetNullableDBValue(row["Umsatz"]));
            prds.PaketeGesamt = Convert.ToSingle(SqlHelper.GetNullableDBValue(row["PaketeGesamt"]));
            prds.PaketeBezahlt = Convert.ToSingle(SqlHelper.GetNullableDBValue(row["PaketeBezahlt"]));
            prds.PaketeGeozone = Convert.ToSingle(SqlHelper.GetNullableDBValue(row["PaketeGeozone"]));
            prds.PaketeSponsoren = Convert.ToSingle(SqlHelper.GetNullableDBValue(row["PaketeSponsoren"]));

            return prds;
        }

        private static ProjectReportDetailDaten ConvertToProjectReportDetailDaten(DataRow row)
        {
            ProjectReportDetailDaten prdd = new ProjectReportDetailDaten();

            //Art, SubArt, Sorter, Agent, DisplayName, Wert, Prozent 
            prdd.Art = (string)SqlHelper.GetEmptyByNull(row["Art"]);
            prdd.SubArt = (string)SqlHelper.GetEmptyByNull(row["SubArt"]);
            prdd.Sorter = (int)row["Sorter"];
            prdd.Agent = (string)SqlHelper.GetEmptyByNull(row["Agent"]);
            prdd.DisplayName = (string)SqlHelper.GetEmptyByNull(row["DisplayName"]);
            prdd.Wert = Convert.ToSingle(SqlHelper.GetNullableDBValue(row["Wert"]));
            prdd.Prozent = Convert.ToSingle(SqlHelper.GetNullableDBValue(row["Prozent"]));

            return prdd;
        }

        private static ProjectReportGeozonen ConvertToProjectReportDetailGeoZonen(DataRow row)
        {
            ProjectReportGeozonen prgz = new ProjectReportGeozonen();
            prgz.Ergebnis = (string)SqlHelper.GetEmptyByNull(row["Ergebnis"]);
            if (SqlHelper.GetNullableDBValue(row["AdressenInZonen"]) != null)
                prgz.AdressenInZonen = (int)SqlHelper.GetNullableDBValue(row["AdressenInZonen"]);
            prgz.Auftraege = (int)SqlHelper.GetNullableDBValue(row["Auftraege"]);

            return prgz;
        }
    }
}
