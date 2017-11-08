using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using metatop.Applications.metaCall.DataObjects;


namespace metatop.Applications.metaCall.DataAccessLayer
{
    public static class SecurityGroupDAL
    {
        private const string sp_SecurityGroupCreate = "dbo.SecurityGroup_Create";
        private const string sp_SecurityGroupUpdate = "dbo.SecurityGroup_Update";
        private const string sp_SecurityGroupDelete = "dbo.SecurityGroup_Delete";

        private const string sp_SecurityGroupGetSingle = "dbo.SecurityGroups_GetSingle";
        private const string sp_SecurityGroupsGetAllActive = "dbo.SecurityGroups_GetAllActive";
        private const string sp_SecurityGroupsGetByUser = "dbo.SecurityGroups_GetByUser";

        public static void Create(SecurityGroup securityGroup)
        {
            IDictionary<string, object> parameters = GetParameters(securityGroup);

            SqlHelper.ExecuteStoredProc(sp_SecurityGroupCreate, parameters);

        }

        public static void Update(SecurityGroup securityGroup)
        {
            IDictionary<string, object> parameters = GetParameters(securityGroup);

            SqlHelper.ExecuteStoredProc(sp_SecurityGroupUpdate, parameters);
        }

        public static void Delete(Guid SecurityGroupId)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@SecurityGroupId", SecurityGroupId);

            SqlHelper.ExecuteStoredProc(sp_SecurityGroupDelete, parameters);
        }

        public static SecurityGroup GetSecurityGroup(Guid securityGroupId)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@SecurityGroupId", securityGroupId);

            DataTable dataTable = SqlHelper.ExecuteDataTable(sp_SecurityGroupGetSingle, parameters);

            if (dataTable.Rows.Count < 1)
                return null;
            else
                return ConvertToSecurityGroup(dataTable.Rows[0]);
        }

        public static SecurityGroup[] GetSecurityAllGroups()
        {
            DataTable dataTable = SqlHelper.ExecuteDataTable(sp_SecurityGroupsGetAllActive);

            return ConvertToSecurityGroups(dataTable);
        
        }

        public static SecurityGroup[] GetSecurityGroupsForUser(Guid userId)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@UserId", userId);
            DataTable dataTable = SqlHelper.ExecuteDataTable(sp_SecurityGroupsGetByUser, parameters);

            return ConvertToSecurityGroups(dataTable);

        }

        private static IDictionary<string, object> GetParameters(SecurityGroup securityGroup)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@SecurityGroupId", securityGroup.SecurityGroupId);
            parameters.Add("@Name", securityGroup.Name);
            parameters.Add("@DisplayName", securityGroup.DisplayName);

            return parameters;
        }

        private static SecurityGroup ConvertToSecurityGroup(DataRow row)
        {
            SecurityGroup group = new SecurityGroup();
            group.SecurityGroupId = (Guid)row["SecurityGroupId"];
            group.Name = (string)row["Name"];
            group.DisplayName = (string)row["DisplayName"];

            return group;
        }

        private static SecurityGroup[] ConvertToSecurityGroups(DataTable dataTable)
        {
            SecurityGroup[] groups = new SecurityGroup[dataTable.Rows.Count];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[i];
                groups[i] = ConvertToSecurityGroup(row);
            }

            return groups;
        }

        

    }
}
