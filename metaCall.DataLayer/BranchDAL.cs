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
    public static class BranchDAL
    {
        #region stored Procedures
        private const string spBranch_GetSingle = "dbo.Branch_GetSingle";
        private const string spBranch_GetAll = "dbo.Branch_GetAll";
        
        #endregion

        private static Branch ConvertToBranch(DataRow Row)
        {
            Branch branch = new Branch();

            branch.Branchennummer = (int)Row["Branchennummer"];
            branch.Bezeichnung = (string)Row["Bezeichnung"];

            if ((Guid?)SqlHelper.GetNullableDBValue(Row["BranchenGruppenID"]) != null)
            {
                branch.BranchGroup = BranchGroupDAL.GetBranchGroup((Guid)Row["BranchenGruppenID"]);
            }

            return branch;
        }

        private static Branch[] ConvertToBranchs(DataTable dataTable)
        {
            Branch[] branchs = new Branch[dataTable.Rows.Count];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[i];
                branchs[i] = ConvertToBranch(row);
            }

            return branchs;
        }

        public static Branch[] GetAllBranchs()
        {
            DataTable dataTable = SqlHelper.ExecuteDataTable(spBranch_GetAll);
            return ConvertToBranchs(dataTable);
        }

        /// <summary>
        /// liefert die Branche mit der übergebenen Branchennummer oder UnknownBranch zurück.
        /// </summary>
        /// <param name="branchNumber"></param>
        /// <returns></returns>
        public static Branch GetBranch(int? branchNumber)
        {
            if (!branchNumber.HasValue || branchNumber == 0 )
                return Branch.Unknown;

            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@BranchNumber", branchNumber);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spBranch_GetSingle, parameters);

            if (dataTable.Rows.Count < 1)
                return Branch.Unknown;
            else
                return ConvertToBranch(dataTable.Rows[0]);
        }

    }
}
