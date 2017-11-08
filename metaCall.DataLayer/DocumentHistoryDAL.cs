using System;
using System.Collections.Generic;
using System.Text;

using metatop.Applications.metaCall.DataObjects;
using System.Data;

namespace metatop.Applications.metaCall.DataAccessLayer
{
    public static class DocumentHistoryDAL
    {

        #region Stored Procedures
        public const string spDocumentsHistory_Create = "dbo.DocumentHistory_Create";
        public const string spDocumentsHistory_GetListByCallJobId = "dbo.DocumentHistory_GetByCallJob";
        #endregion


        /// <summary>
        /// Erstellt einen neuen Eintrag in der Tabelle tblDocumentHistory 
        /// aufgrund einer Instanz der Klasse DocumentHistory
        /// </summary>
        /// <param name="documentHistoryItem"></param>
        public static void CreateDocumentHistoryItem(DocumentHistory documentHistoryItem)
        {
            IDictionary<string, object> parameters = GetParameters(documentHistoryItem);

            SqlHelper.ExecuteStoredProc(spDocumentsHistory_Create, parameters);

        }

        private static IDictionary<string, object> GetParameters(DocumentHistory documentHistoryItem)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@DocumentHistoryId", documentHistoryItem.DocumentHistoryId);
            parameters.Add("@ReferencedId", documentHistoryItem.ReferencedId);
            parameters.Add("@ReferencedType", documentHistoryItem.ReferencedType);
            parameters.Add("@DocumentType", documentHistoryItem.DocumentType);
            parameters.Add("@DocumentId", documentHistoryItem.DocumentId);
            parameters.Add("@SendOption", documentHistoryItem.SendOption);
            parameters.Add("@SendDate", documentHistoryItem.SendDate);
            parameters.Add("@SendUserId", documentHistoryItem.SendUser.UserId);
            parameters.Add("@DataFields", documentHistoryItem.DataFields);

            return parameters;
        }            


        /// <summary>
        /// Liefert eine Liste von DocumentHistory-Instanzen aufgrund eines CallJobs
        /// </summary>
        /// <param name="callJob"></param>
        /// <returns></returns>
        public static DocumentHistory[] GetDocumentHistoryItems(CallJob callJob)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@@CallJobId", callJob.CallJobId);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spDocumentsHistory_GetListByCallJobId, parameters);

            return ConvertToDocumentHistoryItems(dataTable);
        }

        private static DocumentHistory[] ConvertToDocumentHistoryItems(DataTable dataTable)
        {
            DocumentHistory[] documentHistoryItems = new DocumentHistory[dataTable.Rows.Count];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[i];

                documentHistoryItems[i] = ConvertToDocumentHistoryItem(row);
            }

            return documentHistoryItems;

        }

        private static DocumentHistory ConvertToDocumentHistoryItem(DataRow row)
        {
            DocumentHistory documentHistoryItem = new DocumentHistory();
            documentHistoryItem.DocumentHistoryId = (Guid) row["DocumentHistoryId"];

            documentHistoryItem.ReferencedId = (Guid?) SqlHelper.GetNullableDBValue(row["ReferencedId"]);
            documentHistoryItem.ReferencedType = (string)SqlHelper.GetNullableDBValue(row["ReferencedType"]);
            documentHistoryItem.DocumentType = (string)row["DocumentType"];
            documentHistoryItem.DocumentId = (Guid)row["DocumentId"];
            documentHistoryItem.SendOption = (string)row["SendOption"];
            documentHistoryItem.SendDate = (DateTime)row["SendDate"];
            documentHistoryItem.SendUser = UserDAL.GetUserInfo((Guid?)row["SendUser"]);
            documentHistoryItem.DataFields = (string)SqlHelper.GetNullableDBValue(row["DataFields"]);

            return documentHistoryItem;


        }



    }
}
