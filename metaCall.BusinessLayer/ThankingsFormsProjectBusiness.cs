using System;
using System.Collections.Generic;
using System.Text;

using metatop.Applications.metaCall.ServiceAccessLayer;
using metatop.Applications.metaCall.DataObjects;

namespace metatop.Applications.metaCall.BusinessLayer
{
    public class ThankingsFormsProjectBusiness
    {
        MetaCallBusiness metaCallBusiness;

        internal ThankingsFormsProjectBusiness(MetaCallBusiness metaCallBusiness)
        {
            this.metaCallBusiness = metaCallBusiness;
        }

        public List<ThankingsFormsProject> GetThankingsFormsByProject(int projektnummer)
        {
            //Prüfen, ob sich ein Benutzer angemeldet hat
            if (!metaCallBusiness.Users.IsLoggedOn)
                throw new NoUserLoggedOnException();

            return new List<ThankingsFormsProject>(metaCallBusiness.ServiceAccess.GetAllThankingsFormsProject(projektnummer));
        }
        public List<ThankingsFormsProject> GetThankingsFormsByProject(Project project)
        {

            if (project == null)
                throw new ArgumentNullException("project");

            if (project.mwProject == null)
                throw new NoMetaWareProjectException("Das angegebene Projekt ist kein metaware-Projekt");

            int projektNummer = project.mwProject.Projektnummer;

            return GetThankingsFormsByProject(projektNummer);
            
        }


    }
}
