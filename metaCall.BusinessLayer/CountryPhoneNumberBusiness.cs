using System;
using System.Collections.Generic;
using System.Text;

using metatop.Applications.metaCall.ServiceAccessLayer;
using metatop.Applications.metaCall.DataObjects;

namespace metatop.Applications.metaCall.BusinessLayer
{
    public class CountryPhoneNumberBusiness
    {
        MetaCallBusiness metaCallBusiness;

        internal CountryPhoneNumberBusiness(MetaCallBusiness metaCallBusiness)
        {
            this.metaCallBusiness = metaCallBusiness;
        }

        /// <summary>
        /// Liefert die CountryPhoneNumber mit dem angegebenen Land 
        /// </summary>
        /// <param name="land"></param>
        /// <returns></returns>
        public CountryPhoneNumber GetCountryCodeNumber(string land)
        {
            return metaCallBusiness.ServiceAccess.GetCountryPhoneNumber(land);
        }

    }
}
