using System;
using System.Collections.Generic;
using System.Text;

using metatop.Applications.metaCall.ServiceAccessLayer;
using metatop.Applications.metaCall.DataObjects;

namespace metatop.Applications.metaCall.BusinessLayer
{
    public class ContactTypesParticipationBusiness
    {

        MetaCallBusiness metaCallBusiness;

        internal ContactTypesParticipationBusiness(MetaCallBusiness metaCallBusiness)
        {
            this.metaCallBusiness = metaCallBusiness;
        }

        public List<ContactTypesParticipation> ContactTypesParticipations
        {
            get
            {
                //Prüfen, ob sich ein Benutzer angemeldet hat
                if (!metaCallBusiness.Users.IsLoggedOn)
                    throw new NoUserLoggedOnException();

                return new List<ContactTypesParticipation>(metaCallBusiness.ServiceAccess.GetAllContactTypesParticipations());
            }
        }
    }
}
