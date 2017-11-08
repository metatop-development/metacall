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
    public static class ContactTypesParticipationDAL
    {
        #region stored Procedures

        private const string spContactTypesParticipation_GetAll = "dbo.ContactTypesParticipation_GetAll";
        private const string spContactTypesParticipation_GetSingle = "dbo.ContactTypesParticipation_GetSingle";

        #endregion

        private static ContactTypesParticipation ConvertToContactTypesParticipation(DataRow Row)
        {
            ContactTypesParticipation contactTypesParticipation = new ContactTypesParticipation();

            contactTypesParticipation.ContactTypesParticipationId = (Guid)Row["ContactTypesParticipationID"];
            contactTypesParticipation.DisplayName = (string)Row["DisplayName"];

            return contactTypesParticipation;
        }

        private static ContactTypesParticipation[] ConvertToContactTypesParticipations(DataTable dataTable)
        {
            ContactTypesParticipation[] contactTypesParticipations = new ContactTypesParticipation[dataTable.Rows.Count];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[i];
                contactTypesParticipations[i] = ConvertToContactTypesParticipation(row);
            }

            return contactTypesParticipations;
        }

        public static ContactTypesParticipation[] GetAllContactTypesParticipation()
        {
            DataTable dataTable = SqlHelper.ExecuteDataTable(spContactTypesParticipation_GetAll);
            return ConvertToContactTypesParticipations(dataTable);
        }

        public static ContactTypesParticipation GetContactTypesParticipation(Guid? contactTypeParticipationId)
        {
            if (!contactTypeParticipationId.HasValue)
                return null;

            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@ContactTypeParticipationId", contactTypeParticipationId);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spContactTypesParticipation_GetSingle, parameters);

            if (dataTable.Rows.Count < 1)
                return null;
            else
                return ConvertToContactTypesParticipation(dataTable.Rows[0]);
            
        }
    }
}
