using System;
using System.Collections.Generic;
using System.Text;

using metatop.Applications.metaCall.DataObjects;
using metatop.Applications.metaCall.ServiceAccessLayer;

namespace metatop.Applications.metaCall.BusinessLayer
{
    public class CallJobInfoUnsuitableBusiness
    {
        private MetaCallBusiness metaCallBusiness;

        internal CallJobInfoUnsuitableBusiness(MetaCallBusiness metaCallBusiness)
        {
            this.metaCallBusiness = metaCallBusiness;
        }


        /// <summary>
        /// Liefert Calljobs (CallJobUnsuitableInfo) eines Projekts die als ungeeignet, Nummer falsch 
        /// oder Adresse doppelt gekennzeichnet sind.
        /// </summary>
        /// <param name="project"></param>
        /// <param name="userId"></param>
        /// <param name="contactTypesParticipationUnsuitableId"></param>
        /// <returns></returns>
        public List<CallJobUnsuitableInfo> GetListCallJobsUnsuitableInfoByProject(Project project, 
            Guid userId, Guid contactTypesParticipationUnsuitableId)
        {
            if (project == null)
                throw new ArgumentNullException("project");

            return new List<CallJobUnsuitableInfo>(metaCallBusiness.ServiceAccess.GetListCallJobsUnsuitableInfoByProject(
                project, userId, contactTypesParticipationUnsuitableId));
        }

        /// <summary>
        /// Liefert UserIds eines Projekts für User die eine Adresse als ungeeignet gekennzeichnet
        /// haben
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public CallJobUnsuitableInfoUser[] GetListCallJobsUnsuitableInfoUsersByProject(Project project)
        {
            if (project == null)
                throw new ArgumentNullException("project");

            return metaCallBusiness.ServiceAccess.GetListCallJobsUnsuitableInfoUsersByProject(project);
        }

        /// <summary>
        /// Liefert contactTypeParticipationUnsuitableIds für Gründe die bei ungeeigneten Adressen des 
        /// Projekts angegeben wurden.
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public CallJobUnsuitableInfoReason[] GetListCallJobsUnsuitableInfoReasonsByProject(Project project)
        {
            if (project == null)
                throw new ArgumentNullException("project");

            return metaCallBusiness.ServiceAccess.GetListCallJobsUnsuitableInfoReasonsByProject(project);
        }

        public void UpdateCallJobsUnsuitableAddressChanges(CallJobUnsuitableAddressChanges[] callJobAddressChanges)
        {
            if (callJobAddressChanges == null)
                throw new ArgumentNullException("callJobAddressChanges");

            metaCallBusiness.ServiceAccess.UpdateCallJobsUnsuitableAddressChanges(callJobAddressChanges);
            
        }

        /// <summary>
        /// Liefert den Prozentsatz der bestätigten ungeeigneten Adressen aus allen ungeeigneten
        /// Adressen pro Projekt
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public double GetUnsuitableAddressPercentageByProject(Project project)
        {
            if (project == null)
                throw new ArgumentNullException("project");

            return metaCallBusiness.ServiceAccess.GetUnsuitableAddressPercentageByProject(project);
        }
    }
}
