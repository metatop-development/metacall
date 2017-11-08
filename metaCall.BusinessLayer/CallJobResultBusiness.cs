using System;
using System.Collections.Generic;
using System.Text;

using metatop.Applications.metaCall.DataObjects;
using metatop.Applications.metaCall.ServiceAccessLayer;

namespace metatop.Applications.metaCall.BusinessLayer
{
    public class CallJobResultBusiness
    {
        private MetaCallBusiness metaCallBusiness;
        
        public CallJobResultBusiness(MetaCallBusiness metaCallBusiness)
        {
            this.metaCallBusiness = metaCallBusiness;
        }

        /// <summary>
        /// Aktualisiert einen CallJobResult auf dem Server
        /// </summary>
        /// <param name="result"></param>
        public void Update(CallJobResult result)
        {
            if (result == null)
                throw new ArgumentNullException("result");

            metaCallBusiness.ServiceAccess.UpdateCallJobResult(result);
        }

        /// <summary>
        /// Erstellt einen CallJobResult auf dem Server
        /// Der CallJobResult-Typ wird ausgewertet
        /// </summary>
        /// <param name="result"></param>
        /// <remarks>
        /// Bei einem CallJobPossibleResult wird der Auftrag oder der Ablehnungsgrund 
        /// mit erstellt
        /// </remarks>
        public void Create(CallJobResult result)
        {
            //TODO: Alle Parameter und Eigenschaften des Results prüfen
            if (result == null)
                throw new ArgumentNullException("result");

            metaCallBusiness.ServiceAccess.CreateCallJobResult(result);

            if (result.GetType() == typeof(CallJobReminderResult))
            {
                Create((CallJobReminderResult)result);
            }

            if (result.GetType() == typeof(CallJobPossibleResult))
                Create((CallJobPossibleResult)result);

            if (result.GetType() == typeof(CallJobUnsuitableResult))
            {
                Create((CallJobUnsuitableResult)result);
            }
        }

        public void CreateDurring(CallJobPossibleResult result)
        {
            metaCallBusiness.ServiceAccess.CreateCallJobDurring(result);
        }

        private void Create(CallJobPossibleResult result)
        {
            Guid participationId = result.ContactTypesParticipation.ContactTypesParticipationId;
            if (participationId.Equals(ContactTypesParticipation.JaId))
            {
                metaCallBusiness.ServiceAccess.CreateCallJobSponsoringOrder(result);
            }
            else if (participationId.Equals(ContactTypesParticipation.NeinId))
            {
                //Wenn ein Zweitanruf gewünscht ist wird dieser CallJob in die Anrufgruppe Zweitanrufe
                //verschoben
                if (result.SecondCallDesired == SecondCallDesiredChoice.Yes)
                {
                    ProjectInfo project = result.CallJob.Project;
                    List<CallJobGroupInfo> callJobGroups = this.metaCallBusiness.CallJobGroups.GetCallJobGroupInfo(project);

                    foreach (CallJobGroupInfo callJobGroup in callJobGroups)
                    {
                        if (callJobGroup.Type == CallJobGroupType.SecondCallList)
                        {
                            result.CallJob.CallJobGroup = callJobGroup;
                            result.CallJob.State = CallJobState.FurtherCall;
                            this.metaCallBusiness.CallJobs.UpdateCallJob(result.CallJob);
                            break;
                        }
                    }
                }
                else
                {
                    metaCallBusiness.ServiceAccess.CreateCallJobSponsoringCancellation(result);
                }
            }
            else if (participationId.Equals(ContactTypesParticipation.WiederVorlageId))
            {
                return;
            }
            else if (participationId.Equals(ContactTypesParticipation.InteresseAngebot))
            {
                return;
            }
            else
            {
                throw new UnknownContactTypeParticipationException();
            }
        }

        private void Create(CallJobReminderResult result)
        {
            return;
        }

        private void Create(CallJobUnsuitableResult result)
        {
            if (result != null)
            {
                this.metaCallBusiness.ServiceAccess.CreateCallJobAddressUnsuitable(result);
            }
        }

        /// <summary>
        /// Liefert den Standardwerbetext für die angegebene Sprache
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public string GetDefaultAdvertisingText(Language language)
        {
            string defaultAdvertisingText = null;
           
            switch (language)
            {
                case Language.SwitzerlandItaly:
                case Language.Italy:
                    defaultAdvertisingText = "come l'indirizzo";
                    break;
                case Language.SwitzerlandFrench:
                case Language.French:
                    defaultAdvertisingText = "comme l'adresse";
                    break;
                case Language.German:
                default:
                    defaultAdvertisingText = "wie Anschrift";
                    break;
            }
            return defaultAdvertisingText;            
        }

        /// <summary>
        /// Liefert eine Instanz der Klasse CallJobResultInfo aufgrund einer Instanz der Klasse CallJobResult
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public CallJobResultInfo GetResultInfo(CallJobResult result)
        {
            if (result == null)
                throw new ArgumentNullException("result");

            CallJobResultInfo info = new CallJobResultInfo();
            info.CallJobResultId = result.CallJobResultId;

            return info;
        }

        /// <summary>
        /// Liefert eine Instanz der Klasse CallJobResult des letzten CallJobResults eines CallJobs
        /// </summary>
        /// <param name="callJob"></param>
        /// <returns></returns>
        public CallJobResult GetLastCallJobResultsByCallJobId(CallJob callJob)
        {
            if (callJob == null)
                throw new ArgumentNullException("CallJob");

            return this.metaCallBusiness.ServiceAccess.GetLastCallJobResultsByCallJobId(callJob); 
        }

        /// <summary>
        /// Liefert eine Instanz der Klasse CallJobResult des letzten CallJobResults einer CallJobId
        /// </summary>
        /// <param name="callJobId"></param>
        /// <returns></returns>
        public CallJobResult GetLastCallJobResultsByCallJobId(Guid callJobId)
        {
            CallJob callJob = metaCallBusiness.ServiceAccess.GetCallJob(callJobId);

            return this.metaCallBusiness.ServiceAccess.GetLastCallJobResultsByCallJobId(callJob);
        }
    }
}
