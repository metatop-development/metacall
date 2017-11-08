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
    public static class StatisticsDAL
    {
        #region stored Procedures

        private const string spCallJobStatistics_GetSingleByProjectId = "dbo.CallJobStatistics_GetSingleByProjectId";
        private const string spCallJobStatistics_GetSingleByCallJobGroupId = "dbo.CallJobStatistics_GetSingleByCallJobGroupId";

        #endregion

        private static CallJobStatistics ConvertToCallJobStatistics(DataRow Row)
        {
            CallJobStatistics callJobStatistics = new CallJobStatistics();

            callJobStatistics.InWork = (int)Row["InWork"];
            callJobStatistics.Total = (int)Row["Total"];
            callJobStatistics.FirstCall = (int) Row["FirstCall"];
            callJobStatistics.Waiting = (int)Row["Waiting"]; ;
            callJobStatistics.Cancelled = (int)Row["Cancelled"]; ;
            callJobStatistics.Ordered = (int)Row["Ordered"]; ;
            callJobStatistics.ReminderCallJob = (int)Row["ReminderCallJob"]; ;
            callJobStatistics.FurtherCall = (int)Row["FurtherCall"]; ;
            callJobStatistics.Unsuitable = (int)Row["UnSuitable"]; ;

            return callJobStatistics;
        }

        /// <summary>
        /// liefert die Statistik mit der übergebenen ProjectInfo zurück.
        /// </summary>
        /// <param name="ProjectInfo"></param>
        /// <returns></returns>
        public static CallJobStatistics GetCallJobStatistics(ProjectInfo projectInfo, Guid? userId, int resultType)
        {

            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@ProjectId", projectInfo.ProjectId);
            parameters.Add("@CurrentUserId", userId);
            parameters.Add("resultType", resultType);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spCallJobStatistics_GetSingleByProjectId, parameters);

            return ConvertToCallJobStatistics(dataTable.Rows[0]);
        }

        public static CallJobStatistics GetCallJobStatistics(CallJobGroupInfo callJobGroup, Guid? userId, int resultType)
        {

            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@CallJobGroupId", callJobGroup.CallJobGroupId);
            parameters.Add("@CurrentUserId", userId);
            parameters.Add("resultType", resultType);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spCallJobStatistics_GetSingleByCallJobGroupId, parameters);

            return ConvertToCallJobStatistics(dataTable.Rows[0]);
        }

    }
}
