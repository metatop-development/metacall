using System;
using System.Collections.Generic;
using System.Text;

using metatop.Applications.metaCall.ServiceAccessLayer;
using metatop.Applications.metaCall.DataObjects;

namespace metatop.Applications.metaCall.BusinessLayer
{
    public class SettingBusiness
    {
        MetaCallBusiness metaCallBusiness;

        internal SettingBusiness(MetaCallBusiness metaCallBusiness)
        {
            this.metaCallBusiness = metaCallBusiness;
        }

        /// <summary>
        /// Liefert die Setting mit der angegebenen Brachennummer oder Branch.Unknown
        /// </summary>
        /// <param name="BranchNumber"></param>
        /// <returns></returns>
        public Setting GetSetting()
        {
            return this.metaCallBusiness.ServiceAccess.GetSetting();
        }

        public void UpdateSettings(Setting setting)
        {
            this.metaCallBusiness.ServiceAccess.UpdateSettings(setting);
        }

    }
}
