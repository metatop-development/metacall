using System;
using System.Collections.Generic;
using System.Text;

using metatop.Applications.metaCall.DataObjects;

using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.Common;
using System.ComponentModel;
using System.Threading;

namespace metatop.Applications.metaCall.DataAccessLayer
{
    public class CallJobInfoExtendedDAL
    {
        #region stored Procedures

        private const string spCallJobInfoExtended_GetByProject = "dbo.CallJobInfoExtended_GetByProject";

        #endregion

        private static CallJobInfoExtended ConvertToCallJobInfoExtended(DataRow row)
        {

            string callJobType = "metatop.Applications.metaCall.DataObjects." + (string)row["CallJobType"];
            //            callJobType = "metatop.Applications.metaCall.DataObjects." + (string)row["CallJobType"];
            //string callJobType = "metatop.Applications.metaCall.DataObjects.SponsoringCallJob";


            //Type type = typeof(CallJobInfoExtended).Assembly.GetType(callJobType);
            //CallJobInfoExtended callJobInfoExt = Activator.CreateInstance(type) as CallJobInfoExtended;

            CallJobInfoExtended callJobInfoExt = new CallJobInfoExtended();

            callJobInfoExt.CallJobId = (Guid)row["CallJobId"];
            callJobInfoExt.AddressId = (Guid)row["AddressId"];
            callJobInfoExt.ProjectId = (Guid)row["ProjectId"];
            callJobInfoExt.StartDate = (DateTime)row["StartDate"];
            callJobInfoExt.StopDate = (DateTime)row["StopDate"];
            callJobInfoExt.UserId = (Guid?)SqlHelper.GetNullableDBValue( row["UserId"]);
            callJobInfoExt.TeamId = (Guid?)SqlHelper.GetNullableDBValue( row["TeamId"]);
            callJobInfoExt.IterationCounter = (int)row["CallJobIterationCounter"];
            callJobInfoExt.DialMode = (int)row["DialMode"];
            callJobInfoExt.CallJobGroup = (Guid)row["CallJobGroupId"];
            callJobInfoExt.CallJobType = callJobType;
            callJobInfoExt.AddressSafeActiv = (Boolean)row["AddressSafeActiv"];
            callJobInfoExt.Sponsor = (string)row["Sponsor"];
            callJobInfoExt.Street = (string)SqlHelper.GetNullableDBValue(row["Strasse"]);
            callJobInfoExt.City = (string)SqlHelper.GetNullableDBValue(row["City"]);
            callJobInfoExt.Fon = (string)SqlHelper.GetNullableDBValue(row["Telefon"]);
            callJobInfoExt.StateTerm = (string)SqlHelper.GetNullableDBValue(row["StateTerm"]);
            callJobInfoExt.LastResultDisplayName = (string)SqlHelper.GetNullableDBValue(row["LastResultDisplayName"]);
            callJobInfoExt.DialModeTerm = (string)SqlHelper.GetNullableDBValue(row["DialModeTerm"]);
            callJobInfoExt.CallJobGroupTerm = (string)SqlHelper.GetNullableDBValue(row["CallJobGroupTerm"]);
            callJobInfoExt.UserTerm = (string)SqlHelper.GetNullableDBValue(row["UserTerm"]);
            callJobInfoExt.LastOrderAgent = (string)SqlHelper.GetNullableDBValue(row["LastOrderAgent"]);
            callJobInfoExt.LastContact = (DateTime?)SqlHelper.GetNullableDBValue(row["LastContact"]);
            callJobInfoExt.LastContactAgent = (string)SqlHelper.GetNullableDBValue(row["LastContactAgent"]);
            callJobInfoExt.QuantityOrders = Convert.ToDecimal(SqlHelper.GetNullByNull(row["QuantityOrders"]));
            callJobInfoExt.TotalAmountOrders = Convert.ToDecimal(SqlHelper.GetNullByNull(row["TotalAmountOrders"]));
            callJobInfoExt.RandomSorter = (string)SqlHelper.GetNullableDBValue(row["RandomSorter"]);
            callJobInfoExt.Text1 = (string)SqlHelper.GetNullableDBValue(row["Text1"]);
            callJobInfoExt.CDSource = (string) SqlHelper.GetNullableDBValue(row["CDSource"]);

            /*
            callJobInfoExt.CallJobId = (Guid)row["CallJobId"];
            callJobInfoExt.Sponsor = AddressDAL.GetSponsor((Guid)row["AddressId"]);
            callJobInfoExt.Project = ProjectDAL.GetProjectInfo((Guid)row["ProjectId"]);
            callJobInfoExt.User = UserDAL.GetUserInfo((Guid?)SqlHelper.GetNullableDBValue(row["UserId"]));
            callJobInfoExt.Team = TeamDAL.GetTeamInfo((Guid?)SqlHelper.GetNullableDBValue(row["TeamId"]));
            callJobInfoExt.StartDate = (DateTime)row["StartDate"];
            callJobInfoExt.StopDate = (DateTime)row["StopDate"];
            callJobInfoExt.IterationCounter = (int)row["CallJobIterationCounter"];
            callJobInfoExt.State = (CallJobState)row["State"];
            callJobInfoExt.DialMode = (DialMode)row["DialMode"];
            callJobInfoExt.AddressSafeActiv = (Boolean)row["AddressSafeActiv"];
            if (SqlHelper.GetNullableDBValue(row["CallJobGroupId"]) == null)
                callJobInfoExt.CallJobGroup = null;
            else
                callJobInfoExt.CallJobGroup = CallJobGroupDAL.GetCallJobGroupInfo((Guid)row["CallJobGroupId"]);

            if (callJobInfoExt.GetType() == (typeof(DurringCallJob)))
            {
                DurringCallJob durringCallJob = (DurringCallJob)callJobInfoExt;

                durringCallJob.Invoice = InvoiceDAL.GetInvoice((Guid?)SqlHelper.GetNullableDBValue(row["CallJobId"]));

            }
            */
            ObjectCache.Add(callJobInfoExt.CallJobId, callJobInfoExt, TimeSpan.FromSeconds(20));

            return callJobInfoExt;
        }


        private static CallJobInfoExtended[] ConvertToListCallJobInfoExtended(DataTable dataTable,
        AsyncOperation asyncOp,
        SendOrPostCallback reportProgressDelegate)
        {
            CallJobInfoExtended[] callJobInfoExt = new CallJobInfoExtended[dataTable.Rows.Count];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[i];
                callJobInfoExt[i] = ConvertToCallJobInfoExtended(row);

                GetCallJobInfoExtendedProgressChangedEventArgs e = new GetCallJobInfoExtendedProgressChangedEventArgs(
                    callJobInfoExt[i],
                    dataTable.Rows.Count,
                    i,
                    (int)(((float)i / (float)dataTable.Rows.Count) * 100),
                    asyncOp.UserSuppliedState);

                asyncOp.Post(reportProgressDelegate, e);


                // Yield the rest of this time slice.
                Thread.Sleep(0);
            }

            return callJobInfoExt;
        }

        
        /// <summary>
        /// Liefert eine Liste von CallJobInfoExtended für das angegebene Project (asynchron)
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public static CallJobInfoExtended[] GetListCallJobInfoExtendedByProject(Project project,
            AsyncOperation asyncOp,
            SendOrPostCallback reportProgressDelegate)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@ProjectId", project.ProjectId);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spCallJobInfoExtended_GetByProject, parameters);

            CallJobInfoExtended[] callJobInfoExt = ConvertToListCallJobInfoExtended(dataTable, asyncOp, reportProgressDelegate);

            //SponsoringCallJob[] sponsoringCallJobs = new SponsoringCallJob[callJobs.Length];
            //Array.Copy(callJobs, sponsoringCallJobs, callJobs.Length);

            return callJobInfoExt;
        }
    }
}
