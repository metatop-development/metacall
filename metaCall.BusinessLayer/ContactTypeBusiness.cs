using System;
using System.Collections.Generic;
using System.Text;

using metatop.Applications.metaCall.ServiceAccessLayer;
using metatop.Applications.metaCall.DataObjects;

namespace metatop.Applications.metaCall.BusinessLayer
{
    public class ContactTypeBusiness
    {

        MetaCallBusiness metaCallBusiness;

        internal ContactTypeBusiness(MetaCallBusiness metaCallBusiness)
        {
            this.metaCallBusiness = metaCallBusiness;
        }

        public void Create(ContactType contactType)
        {
            if (contactType == null)
                throw new ArgumentNullException();

            metaCallBusiness.ServiceAccess.CreateContactType(contactType);

        }

        public void Create(string displayName)
        {
            ContactType contactType = new ContactType();
            contactType.ContactTypeId = Guid.NewGuid();
            contactType.DisplayName = displayName;

            Create(contactType);
        }

        internal void Update(ContactType contactType)
        {
            if (contactType == null)
                throw new ArgumentNullException("contactType");

            metaCallBusiness.ServiceAccess.UpdateContactType(contactType);
        }

        public void Delete(ContactType contactType)
        {
            if (contactType == null)
                throw new ArgumentNullException("contactType");

            metaCallBusiness.ServiceAccess.DeleteContactType(contactType.ContactTypeId);
        }

        public List<ContactType> ContactTypesSponsoringCallJob
        {
            get
            {
                //Prüfen, ob sich ein Benutzer angemeldet hat
                if (!metaCallBusiness.Users.IsLoggedOn)
                    throw new NoUserLoggedOnException();

                return new List<ContactType>(metaCallBusiness.ServiceAccess.GetAllContactTypesSponsoringCallJob());
            }
        }

        public List<ContactType> ContactTypesDurringCallJob
        {
            get
            {
                //Prüfen, ob sich ein Benutzer angemeldet hat
                if (!metaCallBusiness.Users.IsLoggedOn)
                    throw new NoUserLoggedOnException();

                return new List<ContactType>(metaCallBusiness.ServiceAccess.GetAllContactTypesDurringCallJob());
            }
        }

    }
}
