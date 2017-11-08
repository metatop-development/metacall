using System;
using System.Collections.Generic;
using System.Text;

using metatop.Applications.metaCall.ServiceAccessLayer;
using metatop.Applications.metaCall.DataObjects;

namespace metatop.Applications.metaCall.BusinessLayer
{
    public class mwProjekt_SponsorPacketBusiness
    {
        MetaCallBusiness metaCallBusiness;

        internal mwProjekt_SponsorPacketBusiness(MetaCallBusiness metaCallBusiness)
        {
            this.metaCallBusiness = metaCallBusiness;
        }

        public List<mwProjekt_SponsorPacket> mwProjekt_SponsorPackets(int projektnummer)
        {
            //Prüfen, ob sich ein Benutzer angemeldet hat
            if (!metaCallBusiness.Users.IsLoggedOn)
                throw new NoUserLoggedOnException();

            return new List<mwProjekt_SponsorPacket>(metaCallBusiness.ServiceAccess.GetAllmwProjekt_SponsorPacket(projektnummer));
        }

    }
}
