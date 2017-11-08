using System;
using System.Collections.Generic;
using System.Text;

using System.Data.Common;
using System.Data;

using Microsoft.Practices.EnterpriseLibrary.Data;

using metatop.Applications.metaCall.DataObjects;

namespace metatop.Applications.metaCall.DataAccessLayer
{
    public static class mwUserDAL
    {
        #region Stored Porcedures
        private const string spMwUsers_GetAll = "dbo.mwUsers_GetAllActive";
        private const string spmwUser_GetSingle = "dbo.mwUsers_GetSingle";
        #endregion

        public static mwUser[] GetAllmwUsers()
        {
            DataTable dataTable = SqlHelper.ExecuteDataTable(spMwUsers_GetAll);


            if (dataTable.Rows.Count < 1)
                return null;
            else
                return ConvertTomwUsers(dataTable);
        }

        public static mwUser GetmwUser(Guid userId)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@UserId", userId);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spmwUser_GetSingle, parameters);

            if (dataTable.Rows.Count < 1)
                return null;
            else
                return ConvertTomwUser(dataTable.Rows[0]);

        }


        private static mwUser[] ConvertTomwUsers(DataTable dataTable)
        {
            mwUser[] users = new mwUser[dataTable.Rows.Count];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[i];
                users[i] = ConvertTomwUser(row);
            }

            return users;
        }

        private static mwUser ConvertTomwUser(DataRow row)
        {
            mwUser user = new mwUser();

            string vorname = (string)SqlHelper.GetNullableDBValue(row["Vorname"]);

            user.MemberId = (Guid)row["UserOID"];
            user.PartnerNummer = (int)row["Partnernummer"];
            user.Vorname = vorname;
            user.Nachname = (string)row["name"];
            user.MemberName = (string)row["membername"];

            return user;
        }
        
    }
}
