using System;
using System.Collections.Generic;
using System.Text;

using MaDaNet.Common.AppFrameWork.Activities;

namespace metatop.Applications.metaCall.BusinessLayer.Activities
{
    public class metaCallLogger: ActivityLogger
    {
        private MetaCallBusiness metaCallBusiness;

        public metaCallLogger(MetaCallBusiness metaCallBusiness)
        {
            this.metaCallBusiness = metaCallBusiness;
        }

        protected override void StoreActivity(ActivityBase activity, string contextInfo)
        {
            base.StoreActivity(activity, contextInfo);

            if (activity == null)
                return;

            metaCallBusiness.ServiceAccess.CreateActivity(activity, contextInfo);

        }
    }
}
