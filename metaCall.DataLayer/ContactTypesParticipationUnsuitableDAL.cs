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
    public static class ContactTypesParticipationUnsuitableDAL
    {
        #region stored Procedures

        private const string spContactTypesParticipationUnsuitable_GetAll = "dbo.ContactTypesParticipationUnsuitable_GetAll";
        private const string spContactTypesParticipationUnsuitable_GetSingle = "dbo.ContactTypesParticipationUnsuitable_GetSingle";

        #endregion

        private static ContactTypesParticipationUnsuitable ConvertToContactTypesParticipationUnsuitable(DataRow Row)
        {
            ContactTypesParticipationUnsuitable contactTypesParticipationUnsuitable = new ContactTypesParticipationUnsuitable();

            contactTypesParticipationUnsuitable.ContactTypesParticipationUnsuitableId = (Guid)Row["ContactTypesParticipationUnsuitableID"];
            contactTypesParticipationUnsuitable.DisplayName = (string)Row["DisplayName"];

            return contactTypesParticipationUnsuitable;
        }

        private static ContactTypesParticipationUnsuitable[] ConvertToContactTypesParticipationUnsuitable(DataTable dataTable)
        {
            ContactTypesParticipationUnsuitable[] contactTypesParticipationUnsuitable = new ContactTypesParticipationUnsuitable[dataTable.Rows.Count];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[i];
                contactTypesParticipationUnsuitable[i] = ConvertToContactTypesParticipationUnsuitable(row);
            }

            return contactTypesParticipationUnsuitable;
        }

        public static ContactTypesParticipationUnsuitable[] GetAllContactTypesParticipationUnsuitable()
        {
            DataTable dataTable = SqlHelper.ExecuteDataTable(spContactTypesParticipationUnsuitable_GetAll);
            return ConvertToContactTypesParticipationUnsuitable(dataTable);
        }

        internal static ContactTypesParticipationUnsuitable GetContactTypesParticipationUnsuitable(Guid contactTypeParticipationUnsuitableId)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@contactTypeParticipationunsuitableId", contactTypeParticipationUnsuitableId);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spContactTypesParticipationUnsuitable_GetSingle, parameters);

            if (dataTable.Rows.Count < 1)
                return null;
            else
                return ConvertToContactTypesParticipationUnsuitable(dataTable.Rows[0]);
        }

    }
}
