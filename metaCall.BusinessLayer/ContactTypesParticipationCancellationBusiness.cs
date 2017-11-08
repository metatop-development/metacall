using System;
using System.Collections.Generic;
using System.Text;

using metatop.Applications.metaCall.ServiceAccessLayer;
using metatop.Applications.metaCall.DataObjects;

namespace metatop.Applications.metaCall.BusinessLayer
{
    public class ContactTypesParticipationCancellationBusiness
    {
        MetaCallBusiness metaCallBusiness;

        internal ContactTypesParticipationCancellationBusiness(MetaCallBusiness metaCallBusiness)
        {
            this.metaCallBusiness = metaCallBusiness;
        }

        public List<ContactTypesParticipationCancellation> ContactTypesParticipationsCancellation
        {
            get
            {
                //Prüfen, ob sich ein Benutzer angemeldet hat
                if (!metaCallBusiness.Users.IsLoggedOn)
                    throw new NoUserLoggedOnException();

                return new List<ContactTypesParticipationCancellation>(metaCallBusiness.ServiceAccess.GetAllContactTypesParticipationCancellation());
            }
        }

    }
}
