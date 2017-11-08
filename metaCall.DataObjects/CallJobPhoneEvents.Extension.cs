using System;
using System.Collections.Generic;
using System.Text;

namespace metatop.Applications.metaCall.DataObjects
{
    public partial class CallJobPhoneEvent
    {
        public CallJobPhoneEvent() { }

        public CallJobPhoneEvent(Guid callJobId, Guid userId, string eventType, DateTime eventDate,
            string phoneNumber)
        {
            this.CallJobId = callJobId;
            this.UserId = userId;
            this.EventType = eventType;
            this.EventDate = eventDate;
            this.PhoneNumber = phoneNumber;
        }
    }
}
