using System;
using System.Collections.Generic;
using System.Text;

using metatop.Applications.metaCall.DataObjects;

namespace metatop.Applications.metaCall.DataAccessLayer
{
    public static class ActivityDAL
    {
        private const string spActivities_Create = "dbo.Activities_Create";

        public static void CreateActivity(MaDaNet.Common.AppFrameWork.Activities.ActivityBase activity, string contextInfo)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@ActivityId", activity.ActivityId);
            parameters.Add("@UserName", activity.Identity.Name);
            parameters.Add("@ActivityClass", activity.GetType().FullName);
            parameters.Add("@date", activity.Date);
            parameters.Add("@Comment", activity.ToString());
            parameters.Add("@ContextInfo", contextInfo);


            SqlHelper.ExecuteStoredProc(spActivities_Create, parameters);

        }
    }
}
