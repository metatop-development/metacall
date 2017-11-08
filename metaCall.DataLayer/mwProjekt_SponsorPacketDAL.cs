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
    public static class mwProjekt_SponsorPacketDAL
    {
        #region stored Procedures

        private const string spmwProjekt_SponsorPacketProject_GetAll = "dbo.mwProjekt_SponsorPacketProject_GetAll";
        private const string spmwProjekt_SponsorPacketProject_GetSingle = "dbo.mwProjekt_SponsorPacketProject_GetSingle";

        #endregion

        public static mwProjekt_SponsorPacket GetmwProjekt_SponsorPacket(int projekteSponsorenpaketNummer)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@ProjekteSponsorenpaketNummer", projekteSponsorenpaketNummer);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spmwProjekt_SponsorPacketProject_GetSingle, parameters);

            if (dataTable.Rows.Count < 1)
                return null;
            else
                return ConvertTomwProjekt_SponsorPacket(dataTable.Rows[0]);

        }

        public static mwProjekt_SponsorPacket[] GetAllmwProjekt_SponsorPackets(int projectNummer)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@Projektnummer", projectNummer);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spmwProjekt_SponsorPacketProject_GetAll, parameters);

            if (dataTable.Rows.Count < 1)
                return null;
            else
                return ConvertTomwProjekt_SponsorPackets(dataTable);

        }

        private static mwProjekt_SponsorPacket ConvertTomwProjekt_SponsorPacket(DataRow row)
        {
            mwProjekt_SponsorPacket mwprojekt_SponsorPacket = new mwProjekt_SponsorPacket();

            mwprojekt_SponsorPacket.ProjekteSponsorenpaketNummer = (int)row["ProjekteSponsorenpaketNummer"];
            mwprojekt_SponsorPacket.Bezeichnung = (string)row["Bezeichnung"];
            mwprojekt_SponsorPacket.BetragNetto = (decimal)row["Betrag_Netto"];
            mwprojekt_SponsorPacket.FaxText1_de = (string)SqlHelper.GetNullableDBValue(row["FaxText1_de"]);
            mwprojekt_SponsorPacket.FaxText2_de = (string)SqlHelper.GetNullableDBValue(row["FaxText2_de"]);
            mwprojekt_SponsorPacket.FaxText1_it = (string)SqlHelper.GetNullableDBValue(row["FaxText1_it"]);
            mwprojekt_SponsorPacket.FaxText2_it = (string)SqlHelper.GetNullableDBValue(row["FaxText2_it"]);
            mwprojekt_SponsorPacket.FaxText1_fr = (string)SqlHelper.GetNullableDBValue(row["FaxText1_fr"]);
            mwprojekt_SponsorPacket.FaxText2_fr = (string)SqlHelper.GetNullableDBValue(row["FaxText2_fr"]);

            return mwprojekt_SponsorPacket;
        }

        private static mwProjekt_SponsorPacket[] ConvertTomwProjekt_SponsorPackets(DataTable dataTable)
        {
            mwProjekt_SponsorPacket[] mwprojekt_SponsorPackets = new mwProjekt_SponsorPacket[dataTable.Rows.Count];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[i];
                mwprojekt_SponsorPackets[i] = ConvertTomwProjekt_SponsorPacket(row);
            }

            return mwprojekt_SponsorPackets;
        }
    }
}
