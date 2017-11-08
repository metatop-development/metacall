using System;
using System.Collections.Generic;
using System.Text;

using metatop.Applications.metaCall.DataObjects;

namespace metatop.Applications.metaCall.WinForms.Modules.Telefonie
{
    public class ContactTypeChangedEventArgs: EventArgs
    {

        public ContactTypeChangedEventArgs(ContactType contactType)
        {
            this.contactType = contactType;
        }

        private ContactType contactType;
        public ContactType ContactType
        {
            get { return contactType; }
        }
	
    }
}
