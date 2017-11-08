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
    public static class BranchGroupTimeListDAL
    {
        #region stored Procedures
        private const string spBranchGroupTimeList_GetSingle = "dbo.BranchGroupTimeList_GetSingle";
        private const string spBranchGroupTimeList_GetAllByBranchGroup = "dbo.BranchGroupTimeList_GetAllByBranchGroup";

        #endregion#

        private static BranchGroupTimeList ConvertToBranchGroupTimeList(DataRow Row)
        {
            BranchGroupTimeList branchGroupTimeList = new BranchGroupTimeList();

            branchGroupTimeList.BranchenGruppenTelezeitenID = (Guid)Row["BranchenGruppenTelezeitenID"];
            branchGroupTimeList.BranchenGruppenID = (Guid)Row["BranchenGruppenID"];
            branchGroupTimeList.TelefonTimeStart = (DateTime)Row["TelefonTimeStart"];
            branchGroupTimeList.TelefonTimeEnd = (DateTime)Row["TelefonTimeEnd"];
            branchGroupTimeList.TelefonWeekDay = (int)Row["TelefonWeekDay"];

            return branchGroupTimeList;
        }

        private static BranchGroupTimeList[] ConvertToBranchGroupTimeLists(DataTable dataTable)
        {
            BranchGroupTimeList[] branchGroupTimeLists = new BranchGroupTimeList[dataTable.Rows.Count];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[i];
                branchGroupTimeLists[i] = ConvertToBranchGroupTimeList(row);
            }

            return branchGroupTimeLists;
        }

        public static BranchGroupTimeList[] GetAllBranchGroupTimeLists(Guid branchGroupID)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@BranchGroupID", branchGroupID);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spBranchGroupTimeList_GetAllByBranchGroup, parameters);
            return ConvertToBranchGroupTimeLists(dataTable);
        }

        /// <summary>
        /// liefert die Branchengruppe mit der übergebenen BranchenGruppenID oder UnknownBranchGroup zurück.
        /// </summary>
        /// <param name="branchNumber"></param>
        /// <returns></returns>
        public static BranchGroupTimeList GetBranchGroupTimeList(Guid branchGroupTimeListID)
        {

            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@BranchGroupTimeListID", branchGroupTimeListID);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spBranchGroupTimeList_GetSingle, parameters);

            return ConvertToBranchGroupTimeList(dataTable.Rows[0]);
        }

    }
}
