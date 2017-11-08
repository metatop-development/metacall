using System;
using System.Collections.Generic;
using System.Text;

using metatop.Applications.metaCall.DataObjects;

namespace metatop.Applications.metaCall.DataAccessLayer
{
    public static class CallJobPhoneEventDAL
    {
        private const string spCallJobPhoneEvents_Create = "dbo.CallJobPhoneEvents_Create";

        public static void CreateCallJobPhoneEvent(CallJobPhoneEvent phoneEvent)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@CallJobId", phoneEvent.CallJobId);
            parameters.Add("@UserId", phoneEvent.UserId);
            parameters.Add("@EventType", phoneEvent.EventType);
            parameters.Add("@EventDate", phoneEvent.EventDate);
            parameters.Add("@PhoneNumber", phoneEvent.PhoneNumber);

            SqlHelper.ExecuteStoredProc(spCallJobPhoneEvents_Create, parameters);
        }

    }
}
