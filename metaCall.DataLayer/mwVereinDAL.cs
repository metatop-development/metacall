using System;
using System.Collections.Generic;
using System.Text;

using metatop.Applications.metaCall.DataObjects;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.Common;


namespace metatop.Applications.metaCall.DataAccessLayer
{
    public static class mwVereinDAL
    {
        #region Stored Procedures
        public const string spMwVerein_GetSingle = "dbo.mwVerein_GetSingle";
        #endregion

        public static mwVerein GetVerein(int vereinsNummer)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@Vereinsnummer", vereinsNummer);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spMwVerein_GetSingle, parameters);

            if (dataTable.Rows.Count < 1)
                return null;
            else
                return ConvertToMwVerein(dataTable.Rows[0]);

        }

        private static mwVerein ConvertToMwVerein(DataRow row)
        {

            mwVerein verein = new mwVerein();
            verein.VereinsNummer = (int)row["Vereinsnummer"];
            verein.Bezeichnung = (string)row["Bezeichnung"];

            return verein;

        }

    }
}
