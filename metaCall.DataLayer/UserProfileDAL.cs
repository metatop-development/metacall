using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;

using metatop.Applications.metaCall.DataObjects;
using System.Data;

namespace metatop.Applications.metaCall.DataAccessLayer
{
    public static class UserProfileDAL
    {

        #region Stored Procedures
        private const string spUserProfile_Create = "dbo.UserProfile_Create";
        private const string spUserProfile_Update = "dbo.UserProfile_Update";
        private const string spUserProfile_Delete = "dbo.UserProfile_Delete";
        private const string spUserProfile_GetSingle = "dbo.UserProfile_GetSingle";
        #endregion

        public static void CreateUserProfile(UserProfile userProfile, DbTransaction transaction)
        {
            IDictionary<string, object> parameters = GetParameters(userProfile);
            SqlHelper.ExecuteStoredProc(spUserProfile_Create, parameters, transaction);
        }

        public static void UpdateUserProfile(UserProfile userProfile, DbTransaction transaction)
        {
            IDictionary<string, object> parameters = GetParameters(userProfile);
            SqlHelper.ExecuteStoredProc(spUserProfile_Update, parameters, transaction);
        }

        /// <summary>
        /// Diese Routine ist eigentlich nicht erforderlich, da ein Benutzerprofil immer
        /// in Zusammenhang mit einem Benutzer bestehen muss / kann
        /// </summary>
        /// <param name="userProfileId"></param>
        /// <param name="transcation"></param>
        private static void DeleteUserProfile(Guid userProfileId, DbTransaction transcation)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@userprofileId", userProfileId);
            parameters.Add("@CurrentUser", null);

            SqlHelper.ExecuteStoredProc(spUserProfile_Delete, parameters);
        }

        public static UserProfile GetUserProfile(Guid? userProfileId)
        {
            if (!userProfileId.HasValue)
                return null;

            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@userprofileId", userProfileId);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spUserProfile_GetSingle, parameters);

            if (dataTable.Rows.Count < 1)
                return null;
            else
                return ConvertToUserProfile(dataTable.Rows[0]);

        }

        private static IDictionary<string, object> GetParameters(UserProfile userProfile)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@userprofileId", userProfile.UserProfileId);
            parameters.Add("@Bezeichnung", userProfile.Bezeichnung);
            parameters.Add("@CurrentUser", null);

            return parameters;

        }

        private static UserProfile ConvertToUserProfile(DataRow row)
        {
            UserProfile userProfile = new UserProfile();
            userProfile.UserProfileId = (Guid)row["UserProfileId"];
            userProfile.Bezeichnung = (string)row["Bezeichnung"];

            return userProfile;
        }

    }
}
