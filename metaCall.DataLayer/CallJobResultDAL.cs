using System;
using System.Collections.Generic;
using System.Text;

using metatop.Applications.metaCall.DataObjects;

using System.Data;
using System.Xml;

namespace metatop.Applications.metaCall.DataAccessLayer
{
    public static class CallJobResultDAL
    {

        #region Stored Procedures
        private const string spCallJobResult_Create = "dbo.CallJobResult_Create";
        private const string spCallJobResult_Update = "dbo.CallJobResult_Update";
        private const string spCallJobResult_Delete = "dbo.CallJobResult_Delete";

        private const string spCallJobResult_GetByCallJobId = "dbo.CallJobResult_GetByCallJobId";
        private const string spCallJobResult_GetSingle = "dbo.CallJobResult_GetSingle";
        #endregion


        #region CRUD-Operations
        public static void CreateCallJobResult(CallJobResult callJobResult)
        {
            IDictionary<string, object> parameters = GetParameters(callJobResult);

            if (callJobResult.GetType() == typeof(CallJobPossibleResult))
            {
                CallJobPossibleResult possibleResult = ((CallJobPossibleResult) callJobResult);
                if (possibleResult.ContactTypesParticipation != null)
                {
                    Guid contactTypeParticipationId = possibleResult.ContactTypesParticipation.ContactTypesParticipationId;
                    parameters.Add("ContactTypeParticipationId", contactTypeParticipationId);

                    bool? secondCallDesired = null;
                    switch (possibleResult.SecondCallDesired)
                    {
                        case SecondCallDesiredChoice.Yes:
                            secondCallDesired = true;
                            break;
                        case SecondCallDesiredChoice.No:
                            secondCallDesired = false;
                            break;
                        default:
                            secondCallDesired = null;
                            break;
                    }   
                    parameters.Add("@SecondCallDesired", secondCallDesired);
                }
            }
            
            SqlHelper.ExecuteStoredProc(spCallJobResult_Create, parameters);
        }

        public static void UpdateCallJobResult(CallJobResult callJobResult)
        {
            IDictionary<string, object> parameters = GetParameters(callJobResult);
            if (callJobResult.GetType() == typeof(CallJobPossibleResult))
            {
                CallJobPossibleResult possibleResult = ((CallJobPossibleResult)callJobResult);
                if (possibleResult.ContactTypesParticipation != null)
                {
                    Guid contactTypeParticipationId = possibleResult.ContactTypesParticipation.ContactTypesParticipationId;
                    parameters.Add("ContactTypeParticipationId", contactTypeParticipationId);
                }

                bool? secondCallDesired = null;
                switch (possibleResult.SecondCallDesired)
                {
                    case SecondCallDesiredChoice.Yes:
                        secondCallDesired = true;
                        break;
                    case SecondCallDesiredChoice.No :
                        secondCallDesired = false;
                        break;
                    default:
                        secondCallDesired = null;
                        break;
                }
                parameters.Add("@SecondCallDesired", secondCallDesired);

            }
            SqlHelper.ExecuteStoredProc(spCallJobResult_Update, parameters);
        }

        public static void DeleteCallJobResult(Guid callJobresultId)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@callJobResultId", callJobresultId);
            parameters.Add("@CurrentUser", Guid.NewGuid());

            SqlHelper.ExecuteStoredProc(spCallJobResult_Delete, parameters);
        }

        #endregion
        
        public static CallJobResult GetCallJobResult(Guid callJobResultId)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@CallJobresultId", callJobResultId);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spCallJobResult_GetSingle, parameters);

            if (dataTable.Rows.Count < 1)
                return null;
            else
                return ConvertToCallJobResult(dataTable.Rows[0]);
        }

        public static CallJobResult GetCallJobResult(Guid? callJobResultId)
        {
            if (!callJobResultId.HasValue)
                return null;
            return GetCallJobResult(callJobResultId.Value);
        }

        public static CallJobResultInfo GetCallJobResultInfo(Guid? callJobResultId)
        {
            if (!callJobResultId.HasValue)
                return null;

            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@CallJobResultId", callJobResultId);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spCallJobResult_GetSingle, parameters);

            if (dataTable.Rows.Count < 1)
                return null;
            else
                return ConvertToCallJobResultInfo(dataTable.Rows[0]);


        }

        public static CallJobResult[] GetCallJobResultsByCallJobId(Guid callJobId)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@CallJobid", callJobId);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spCallJobResult_GetByCallJobId, parameters);

            return ConvertToCallJobResults(dataTable);
        }

        public static CallJobResult GetLastCallJobResultsByCallJobId(Guid callJobId)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@CallJobid", callJobId);
            parameters.Add("@LastResult", 1);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spCallJobResult_GetByCallJobId, parameters);

            if (dataTable.Rows.Count < 1)
                return null;
            else
                return ConvertToCallJobResult(dataTable.Rows[0]);
        }

        private static IDictionary<string, object> GetParameters(CallJobResult result)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("CallJobResultId", result.CallJobResultId);
            parameters.Add("CallJobId", result.CallJob.CallJobId);
            parameters.Add("UserId", result.User.UserId);
            parameters.Add("PhoneNumber", result.PhoneNumber);
            parameters.Add("ContactTypeId", result.ContactType.ContactTypeId);
            parameters.Add("Start", result.Start);
            parameters.Add("Stop", result.Stop);
            parameters.Add("Notice", string.IsNullOrEmpty(result.Notice) ? null: result.Notice);
            parameters.Add("BranchNumber", result.Branch == null? null : (int?) result.Branch.Branchennummer);
            parameters.Add("Category", result.Category);
            parameters.Add("CallJobResultClass", result.GetType().FullName);
            parameters.Add("Currentuser", Guid.NewGuid());

            return parameters;
        }

        private static CallJobResult ConvertToCallJobResult(DataRow row)
        {
            
            string typeName = (string) row["CallJobresultClass"];

            System.Reflection.Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            Type type = null;
            foreach (System.Reflection.Assembly assembly in assemblies)
            {
                type = assembly.GetType(typeName,false, true);
                if (type != null)
                    break;
            }
            if (type == null)
                throw new InvalidCastException("Unknon type of CallJobResult");


            CallJobResult result = Activator.CreateInstance(type) as CallJobResult;
            result.CallJobResultId = (Guid)row["CallJobresultId"];
            result.CallJob = CallJobDAL.GetCallJob((Guid)row["CallJobId"]);
            result.User = UserDAL.GetUser((Guid)row["UserId"]);
            result.ContactType = ContactTypeDAL.GetContactType((Guid)row["ContactTypeId"]);
            result.PhoneNumber = (string)SqlHelper.GetNullableDBValue(row["PhoneNumber"]);
            result.Notice = (string)SqlHelper.GetNullableDBValue(row["Notice"]);
            result.Start = (DateTime?)SqlHelper.GetNullableDBValue(row["Start"]);
            result.Stop = (DateTime?)SqlHelper.GetNullableDBValue(row["Stop"]);
            result.Branch = BranchDAL.GetBranch((int?)SqlHelper.GetNullableDBValue(row["BranchNumber"]));
            result.Category = (CallJobResultCategory)row["Category"];
            
            CallJobPossibleResult possibleResult = result as CallJobPossibleResult;
            if (possibleResult != null)
            {
                possibleResult.ContactTypesParticipation = 
                    ContactTypesParticipationDAL.GetContactTypesParticipation(
                    (Guid?)SqlHelper.GetNullableDBValue(row["ContactTypeParticipationId"]));

                if (possibleResult.ContactTypesParticipation != null)
                {
                    if (possibleResult.ContactTypesParticipation.ContactTypesParticipationId ==
                        new Guid("{657EA0C9-196F-4ECF-8296-D9B837DACA99}"))
                    {
                        possibleResult.ContactTypesParticipationCancellation =
                            ContactTypesParticipationCancellationDAL.GetContactTypesParticipationCancellation((Guid)row["CallJobId"]);
                    }
                }
                bool? secondCallDesiredChoice = (bool?) SqlHelper.GetNullableDBValue(row["SecondCallDesired"]);

                if (!secondCallDesiredChoice.HasValue)
                {
                    possibleResult.SecondCallDesired = SecondCallDesiredChoice.Unset;
                }
                else
                {
                    if (secondCallDesiredChoice.Value)
                        possibleResult.SecondCallDesired = SecondCallDesiredChoice.Yes;
                    else
                        possibleResult.SecondCallDesired = SecondCallDesiredChoice.No;
                }
            }

            CallJobReminderResult reminderResult = result as CallJobReminderResult;
            if (reminderResult != null)
            {
                reminderResult.CallJobReminder = CallJobReminderDAL.GetCallJobReminder(reminderResult.CallJobResultId);
            }

            return result;

        }

        private static CallJobResult[] ConvertToCallJobResults(DataTable dataTable)
        {
            CallJobResult[] results = new CallJobResult[dataTable.Rows.Count];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[i];
                results[i] = ConvertToCallJobResult(row);
            }
            return results;
        }

        private static CallJobResultInfo ConvertToCallJobResultInfo(DataRow row)
        {

            CallJobResultInfo result = new CallJobResultInfo();
            result.CallJobResultId = (Guid)row["CallJobResultId"];

            return result;
        }

        private static CallJobResultInfo[] ConvertToCallJobResultInfos(DataTable dataTable)
        {
            CallJobResultInfo[] results = new CallJobResultInfo[dataTable.Rows.Count];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[i];
                results[i] = ConvertToCallJobResultInfo(row);
            }
            return results;
        }
    
    }
}
