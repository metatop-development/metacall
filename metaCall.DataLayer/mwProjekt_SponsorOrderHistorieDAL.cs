using System;
using System.Collections.Generic;
using System.Text;

using metatop.Applications.metaCall.DataObjects;

using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.Common;

using System.Xml;

namespace metatop.Applications.metaCall.DataAccessLayer
{
    public class mwProjekt_SponsorOrderHistorieDAL
    {
        #region stored Procedures

        private const string spmwProjekt_SponsorOrderHistorie_GetAll = "dbo.mwProjekt_SponsorOrderHistorie_GetAll";
        private const string spmwProjekt_SponsorOrderHistorie_GetAllLastAgent = "dbo.mwProjekt_SponsorOrderHistorie_GetAllLastAgent";

        #endregion

        private static mwProjekt_SponsorOrderHistorie ConvertTomwProjekt_SponsorOrderHistorie(DataRow row)
        {
            mwProjekt_SponsorOrderHistorie mwprojekt_SponsorOrderHistorie = new mwProjekt_SponsorOrderHistorie();
            try
            {
                mwprojekt_SponsorOrderHistorie.AdressenPoolNummer = (int)row["AdressenPoolNummer"];
                mwprojekt_SponsorOrderHistorie.Projektbezeichnung = (string)SqlHelper.GetNullableDBValue(row["Projektbezeichnung"]);
                mwprojekt_SponsorOrderHistorie.Projektjahr = (int)row["Projektjahr"];
                mwprojekt_SponsorOrderHistorie.Projektmonat = (int)row["Projektmonat"];
                mwprojekt_SponsorOrderHistorie.Projektnummer = (int)row["Projektnummer"];
                mwprojekt_SponsorOrderHistorie.Sponsorenpaket = (string)SqlHelper.GetNullableDBValue(row["Sponsorenpaket"]);
                mwprojekt_SponsorOrderHistorie.Stueckzahl = (decimal)row["Stueckzahl"];
                mwprojekt_SponsorOrderHistorie.Agent = (string)SqlHelper.GetNullableDBValue(row["Agent"]);
                mwprojekt_SponsorOrderHistorie.OrderDate = (DateTime?)SqlHelper.GetNullableDBValue(row["OrderDate"]);
                mwprojekt_SponsorOrderHistorie.Quelle = (string)SqlHelper.GetNullableDBValue(row["Quelle"]);
                mwprojekt_SponsorOrderHistorie.OrderState = (string)SqlHelper.GetNullableDBValue(row["OrderState"]);
                mwprojekt_SponsorOrderHistorie.Umsatz = (decimal)row["Umsatz"];
            }
            finally
            {
                
            }
            return mwprojekt_SponsorOrderHistorie;
        }

        private static mwProjekt_SponsorOrderHistorie[] ConvertTomwProjekt_SponsorOrderHistories(DataTable dataTable)
        {
            mwProjekt_SponsorOrderHistorie[] mwprojekt_SponsorOrderHistories = new mwProjekt_SponsorOrderHistorie[dataTable.Rows.Count];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[i];
                mwprojekt_SponsorOrderHistories[i] = ConvertTomwProjekt_SponsorOrderHistorie(row);
            }

            return mwprojekt_SponsorOrderHistories;
        }

        public static mwProjekt_SponsorOrderHistorie[] GetAllmwProjekt_SponsorOrderHistorie(int adressenPoolNummer)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@AdressenPoolNummer", adressenPoolNummer);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spmwProjekt_SponsorOrderHistorie_GetAll, parameters);

            return ConvertTomwProjekt_SponsorOrderHistories(dataTable);
        }

        public static mwProjekt_SponsorOrderHistorie[] GetAllmwProjekt_SponsorOrderHistorieLastAgent(int adressenPoolNummer)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@AdressenPoolNummer", adressenPoolNummer);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spmwProjekt_SponsorOrderHistorie_GetAllLastAgent, parameters);

            return ConvertTomwProjekt_SponsorOrderHistories(dataTable);
        }

        

    }
}
