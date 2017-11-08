using System;
using System.Collections.Generic;
using System.Text;

using metatop.Applications.metaCall.ServiceAccessLayer;
using metatop.Applications.metaCall.DataObjects;

namespace metatop.Applications.metaCall.BusinessLayer
{
    public class mwProjekt_SponsorOrderHistorieBusiness
    {
        MetaCallBusiness metaCallBusiness;

        internal mwProjekt_SponsorOrderHistorieBusiness(MetaCallBusiness metaCallBusiness)
        {
            this.metaCallBusiness = metaCallBusiness;
        }

        public List<mwProjekt_SponsorOrderHistorie> GetmwProjekt_SponsorOrderHistorie(int adressenPoolNummer)
        {
            //Prüfen, ob sich ein Benutzer angemeldet hat
            if (!metaCallBusiness.Users.IsLoggedOn)
                throw new NoUserLoggedOnException();

            return new List<mwProjekt_SponsorOrderHistorie>(metaCallBusiness.ServiceAccess.GetAllmwProjekt_SponsorOrderHistorie(adressenPoolNummer));
        }

        public List<mwProjekt_SponsorOrderHistorie> GetAllmwProjekt_SponsorOrderHistorieLastAgent(int adressenPoolNummer)
        {
            return new List<mwProjekt_SponsorOrderHistorie>(metaCallBusiness.ServiceAccess.GetAllmwProjekt_SponsorOrderHistorieLastAgent(adressenPoolNummer));
        }
    }
}
