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
    public static class ContactTypeDAL
    {

        #region stored Procedures

        private const string spContactType_Create = "dbo.ContactType_Create";
        private const string spContactType_Update = "dbo.ContactType_Update";
        private const string spContactType_Delete = "dbo.ContactType_Delete";
        private const string spContactType_GetSingle = "dbo.ContactType_GetSingle";
        private const string spContactTypeSponsoringCallJob_GetAll = "dbo.ContactTypeSponsoringCallJob_GetAll";
        private const string spContactTypeDurringCallJob_GetAll = "dbo.ContactTypeDurringCallJob_GetAll";

        #endregion

        private static ContactType ConvertToContactType(DataRow Row)
        {
            ContactType contactType = new ContactType();

            contactType.ContactTypeId = (Guid)Row["ContactTypeID"];
            contactType.DisplayName = (string)Row["DisplayName"];

            ObjectCache.Add(contactType.ContactTypeId, contactType, TimeSpan.FromMinutes(30));

            return contactType;
        }

        private static ContactType[] ConvertToContactTypes(DataTable dataTable)
        {
            ContactType[] contactTypes = new ContactType[dataTable.Rows.Count];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[i];
                contactTypes[i] = ConvertToContactType(row);
            }

            return contactTypes; 
        }

        public static void CreateContactType(ContactType contactType)
        {
            IDictionary<string, object> parameters = GetParameters(contactType);
            SqlHelper.ExecuteStoredProc(spContactType_Create, parameters);

            ObjectCache.Add(contactType.ContactTypeId, contactType, TimeSpan.FromMinutes(30));
        }

        public static void DeleteContactType(Guid contactTypeId)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@ContactTypeID", contactTypeId);
            parameters.Add("@CurrentUser", null);
            SqlHelper.ExecuteStoredProc(spContactType_Delete, parameters);

            ObjectCache.Remove(contactTypeId);
        }

        private static IDictionary<string, object> GetParameters(ContactType contactType)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@ContactTypeId", contactType.ContactTypeId);
            parameters.Add("@DisplayName", contactType.DisplayName);
            parameters.Add("@CurrentUser", null);

            return parameters;
        }

        public static ContactType GetContactType(Guid contactTypeId)
        {
            ContactType contactType = ObjectCache.Get<ContactType>(contactTypeId);

            if (contactType != null)
                return contactType;

            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@ContactTypeID", contactTypeId);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spContactType_GetSingle, parameters);

            return ConvertToContactType(dataTable.Rows[0]);
        }

        public static ContactType[] GetAllContactTypesSponsoringCallJob()
        {
            DataTable dataTable = SqlHelper.ExecuteDataTable(spContactTypeSponsoringCallJob_GetAll);
            return ConvertToContactTypes(dataTable);
        }

        public static ContactType[] GetAllContactTypesDurringCallJob()
        {
            DataTable dataTable = SqlHelper.ExecuteDataTable(spContactTypeDurringCallJob_GetAll);
            return ConvertToContactTypes(dataTable);
        }

        public static void UpdateContactType(ContactType contactType)
        {
            IDictionary<string, object> parameters = GetParameters(contactType);
            SqlHelper.ExecuteStoredProc(spContactType_Update, parameters);

            ObjectCache.Add(contactType.ContactTypeId, contactType, TimeSpan.FromMinutes(30));
        }
        
    }
}
