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
    public static class BranchGroupDAL
    {
        #region stored Procedures
        private const string spBranchGroup_GetSingle = "dbo.BranchGroup_GetSingle";
        private const string spBranchGroup_GetAll = "dbo.BranchGroup_GetAll";

        #endregion#

        private static BranchGroup ConvertToBranchGroup(DataRow Row)
        {
            BranchGroup branchGroup = new BranchGroup();

            branchGroup.BranchenGruppenID = (Guid)Row["BranchenGruppenID"];
            branchGroup.BranchenGruppe = (string)Row["BranchenGruppe"];
            branchGroup.Beschreibung =  (string) SqlHelper.GetNullableDBValue(Row["Beschreibung"]);


            branchGroup.BranchGroupTimeList = BranchGroupTimeListDAL.GetAllBranchGroupTimeLists(branchGroup.BranchenGruppenID);
            
            return branchGroup;
        }

        private static BranchGroup[] ConvertToBranchGroups(DataTable dataTable)
        {
            BranchGroup[] branchGroups = new BranchGroup[dataTable.Rows.Count];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[i];
                branchGroups[i] = ConvertToBranchGroup(row);
            }

            return branchGroups;
        }

        public static BranchGroup[] GetAllBranchGroups()
        {
            DataTable dataTable = SqlHelper.ExecuteDataTable(spBranchGroup_GetAll);
            return ConvertToBranchGroups(dataTable);
        }

        /// <summary>
        /// liefert die Branchengruppe mit der übergebenen BranchenGruppenID oder UnknownBranchGroup zurück.
        /// </summary>
        /// <param name="branchNumber"></param>
        /// <returns></returns>
        public static BranchGroup GetBranchGroup(Guid branchGroupID)
        {

            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@BranchGroupID", branchGroupID);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spBranchGroup_GetSingle, parameters);

            return ConvertToBranchGroup(dataTable.Rows[0]);
        }

    }
}
