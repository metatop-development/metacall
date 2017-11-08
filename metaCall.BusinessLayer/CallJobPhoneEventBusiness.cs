using System;
using System.Collections.Generic;
using System.Text;

using metatop.Applications.metaCall.DataObjects;
using metatop.Applications.metaCall.ServiceAccessLayer;
using System.Threading;

namespace metatop.Applications.metaCall.BusinessLayer
{
    public class CallJobPhoneEventBusiness
    {
        private MetaCallBusiness metaCallBusiness;
        private delegate void CreateDelegate(CallJobPhoneEvent phoneEvent);

        internal CallJobPhoneEventBusiness(MetaCallBusiness metaCallBusiness)
        {
            this.metaCallBusiness = metaCallBusiness;
        }

        public void Create(CallJobPhoneEvent phoneEvent)
        {
            this.metaCallBusiness.ServiceAccess.CreateCallJobPhoneEvent(phoneEvent);
        }

        public void CreateAsync(CallJobPhoneEvent phoneEvent)
        {
            CreateDelegate del = new CreateDelegate(this.metaCallBusiness.ServiceAccess.CreateCallJobPhoneEvent);
            del.BeginInvoke(phoneEvent, null, null);
        }
    }
}
