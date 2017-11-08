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
    public static class ContactTypesParticipationCancellationDAL
    {
        #region stored Procedures

        private const string spContactTypesParticipationCancellation_GetAll = "dbo.ContactTypesParticipationCancellation_GetAll";
        private const string spContactTypesParticipationCancellation_GetSingle = "dbo.ContactTypesParticipationCancellation_GetSingle";

        #endregion

        private static ContactTypesParticipationCancellation ConvertToContactTypesParticipationCancellation(DataRow Row)
        {
            ContactTypesParticipationCancellation contactTypesParticipationCancellation = new ContactTypesParticipationCancellation();

            contactTypesParticipationCancellation.ContactTypesParticipationCancellationId = (Guid)Row["ContactTypesParticipationCancellationID"];
            contactTypesParticipationCancellation.DisplayName = (string)Row["DisplayName"];

            ObjectCache.Add(contactTypesParticipationCancellation.ContactTypesParticipationCancellationId, contactTypesParticipationCancellation, TimeSpan.FromMinutes(30));

            return contactTypesParticipationCancellation;
        }

        private static ContactTypesParticipationCancellation[] ConvertToContactTypesParticipationCancellation(DataTable dataTable)
        {
            ContactTypesParticipationCancellation[] contactTypesParticipationCancellation = new ContactTypesParticipationCancellation[dataTable.Rows.Count];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[i];
                contactTypesParticipationCancellation[i] = ConvertToContactTypesParticipationCancellation(row);
            }

            ObjectCache.Add(allCacheIdentifier, contactTypesParticipationCancellation);


            return contactTypesParticipationCancellation;
        }

        private static Guid allCacheIdentifier = Guid.NewGuid();
        public static ContactTypesParticipationCancellation[] GetAllContactTypesParticipationCancellation()
        {
            ContactTypesParticipationCancellation[] allParticipations = ObjectCache.Get<ContactTypesParticipationCancellation[]>(allCacheIdentifier);

            if (allParticipations != null)
                return allParticipations;
            
            DataTable dataTable = SqlHelper.ExecuteDataTable(spContactTypesParticipationCancellation_GetAll);
            return ConvertToContactTypesParticipationCancellation(dataTable);
        }


        internal static ContactTypesParticipationCancellation GetContactTypesParticipationCancellation(Guid contactTypeParticipationCancellationId)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@contactTypeParticipationCancellationId", contactTypeParticipationCancellationId);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spContactTypesParticipationCancellation_GetSingle, parameters);

            if (dataTable.Rows.Count < 1)
                return null;
            else
                return ConvertToContactTypesParticipationCancellation(dataTable.Rows[0]);
        }
    }
}
