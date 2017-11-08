using System;
using System.Collections.Generic;
using System.Text;

using metatop.Applications.metaCall.DataObjects;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.Common;


namespace metatop.Applications.metaCall.DataAccessLayer
{
    public static class mwCenterDAL
    {
        #region Stored Procedures
        private const string spmwCenter_getSingle = "dbo.mwCenter_GetSingle";
        private const string spmwCenter_getAllActive = "dbo.mwCenter_GetAllActive";

        #endregion
        
        public static mwCenter GetMwCenter(int centerNummer)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@CenterNummer", centerNummer);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spmwCenter_getSingle, parameters);

            if (dataTable.Rows.Count < 1)
                return null;
            else
                return ConvertTomwCenter(dataTable.Rows[0]);
        }

        private static mwCenter ConvertTomwCenter(DataRow row)
        {
            mwCenter center = new mwCenter();
            center.CenterNummer = (int) row["CenterNummer"];
            center.Bezeichnung = (string)row["Bezeichnung"];

            return center;
        }

        private static mwCenter[] ConvertTomwCenters(DataTable dataTable)
        {
            mwCenter[] centers = new mwCenter[dataTable.Rows.Count];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[i];
                centers[i] = ConvertTomwCenter(row);
            }

            return centers;
        }

        public static mwCenter[] GetAllActiveMwCenters()
        {
            DataTable dataTable = SqlHelper.ExecuteDataTable(spmwCenter_getAllActive);

            return ConvertTomwCenters(dataTable);
        }
   

    }
}
