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
        public const string AUF_DIE_VEREINSAUSSCHUSSLISTE = "{F7D2E268-2462-47CE-AA57-D294BD66D4FD}";

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
