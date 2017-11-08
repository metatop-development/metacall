using System;
using System.Collections.Generic;
using System.Text;

using metatop.Applications.metaCall.ServiceAccessLayer;
using metatop.Applications.metaCall.DataObjects;

namespace metatop.Applications.metaCall.BusinessLayer
{
    public class mwProjekt_ProjektOrderHistorieBusiness
    {
        MetaCallBusiness metaCallBusiness;

        internal mwProjekt_ProjektOrderHistorieBusiness(MetaCallBusiness metaCallBusiness)
        {
            this.metaCallBusiness = metaCallBusiness;
        }

        public List<mwProjekt_ProjektOrderHistorie> GetmwProjekt_ProjektOrderHistorie(int projektNummer)
        {
            //Prüfen, ob sich ein Benutzer angemeldet hat
            if (!metaCallBusiness.Users.IsLoggedOn)
                throw new NoUserLoggedOnException();

            return new List<mwProjekt_ProjektOrderHistorie>(metaCallBusiness.ServiceAccess.GetAllmwProjekt_ProjektOrderHistorie(projektNummer));
        }

        public List<OrderHistory> GetOrderHistoy_GetByUser(Guid userId, DateTime from, DateTime to)
        {
            return new List<OrderHistory>(metaCallBusiness.ServiceAccess.GetOrderHistoy_GetByUser(userId, from, to));
        }
    }
}
