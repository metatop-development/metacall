using System;
using System.Collections.Generic;
using System.Text;

using metatop.Applications.metaCall.ServiceAccessLayer;
using metatop.Applications.metaCall.DataObjects;

namespace metatop.Applications.metaCall.BusinessLayer
{
    public class ContactTypesParticipationUnsuitableBusiness
    {
        MetaCallBusiness metaCallBusiness;

        internal ContactTypesParticipationUnsuitableBusiness(MetaCallBusiness metaCallBusiness)
        {
            this.metaCallBusiness = metaCallBusiness;
        }

        public List<ContactTypesParticipationUnsuitable> ContactTypesParticipationsUnsuitable
        {
            get
            {
                //Prüfen, ob sich ein Benutzer angemeldet hat
                if (!metaCallBusiness.Users.IsLoggedOn)
                    throw new NoUserLoggedOnException();

                return new List<ContactTypesParticipationUnsuitable>(metaCallBusiness.ServiceAccess.GetAllContactTypesParticipationUnsuitable());
            }
        }

    }
}
