using System;
using System.Collections.Generic;
using System.Text;
using metatop.Applications.metaCall.DataObjects;
using System.Data;
using System.Data.SqlTypes;
using System.Xml;
using System.ComponentModel;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace metatop.Applications.metaCall.DataAccessLayer
{
    public static class CallJobDAL
    {
       #region Stored Procedures
        private const string spCallJob_Delete = "dbo.CallJobs_Delete";
        private const string spCallJob_Update = "dbo.CallJobs_Update";
        private const string spCallJob_GetByProjectAndSponsor = "dbo.CallJobs_GetByProjectAndAddress";
        private const string spCallJob_GetSingle = "dbo.CallJob_GetSingle";
        private const string spCallJob_GetSingleBySponsorproject = "dbo.CallJob_GetSingleBySponsorproject";
        private const string spCallJob_GetListByProject ="dbo.CallJob_GetListByProject";
        private const string spCallJob_GetListByUserAndProject = "dbo.CallJob_GetListByUserAndProject";
        private const string spCallJob_GetLastAddressContact = "dbo.CallJob_GetLastAddressContact";

        private const string spCallJobDurring_GetListByUser = "dbo.CallJobDurring_GetListByUser";
        private const string spDurringInfo_GetByUser = "dbo.DurringInfo_GetByUser";
        private const string spDurringLevelInfo_GetByUser = "dbo.DurringLevelInfo_GetByUser";
        private const string spDurringCreate = "dbo.DurringCreate";
        private const string sp_mwDurringUpdate = "dbo.mwDurringUpdate";

        private const string spCallJobs_ActivityTimeItemCreate = "dbo.CallJobs_ActivityTimeItemCreate";
        private const string spCallJobs_ActivityTimeItemUpdate = "dbo.CallJobs_ActivityTimeItemUpdate";
        private const string spCallJobs_ActivityTimeItemCreate_MetaWare = "CallJobs_ActivityTimeItemCreate_MetaWare";
        private const string spCallJobs_ActivityTimeItemUpdate_MetaWare = "CallJobs_ActivityTimeItemUpdate_MetaWare";

        private const string spCallJobs_CreateByAddressReleaseInfos = "dbo.CallJobs_CreateByAddressReleaseInfos";

        private const string spmwOrder_Create = "dbo.mwOrder_Create";
        private const string spCallJobs_SponsoringOrdersCreate = "dbo.CallJobs_SponsoringOrdersCreate";
        private const string spCallJobs_SponsoringCancellationCreate = "dbo.CallJobs_SponsoringCancellationCreate";
        private const string spCallJobs_AddressUnsuitableCreate = "dbo.CallJobs_AddressUnsuitableCreate";
        private const string spCallJobInfoExtended_GetByAddressSearch = "dbo.CallJobInfoExtended_GetByAddressSearch";

        private const string spCallJobState_GetSingle = "dbo.CallJobState_GetSingle";
        private const string spCallJobState_GetAll = "dbo.CallJobState_GetAll";

        private const string spCallJobs_SponsoringOrdersDistribution = "dbo.CallJobs_SponsoringOrdersDistribution";

        private const string spCallJobs_UpdateDialModeByProject = "dbo.CallJobs_UpdateDialModeByProject";

        private const string spCallJobs_Unused_Transfer = "dbo.CallJobs_Unused_Transfer";
        private const string spCallJobs_Unused_Transfer_Count = "dbo.CallJobs_Unused_Transfer_Count";

        #endregion

        #region CallJobs

        public static CallJobInfoExtended[] GetListCallJobInfoExtendedByAddressSearch(string addressSearch)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@AddressSearch", addressSearch);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spCallJobInfoExtended_GetByAddressSearch, parameters);

            CallJobInfoExtended[] callJobInfoExt = ConvertToListCallJobInfoExtendedAddress(dataTable);

            return callJobInfoExt;
        }

        private static CallJobInfoExtended[] ConvertToListCallJobInfoExtendedAddress(DataTable dataTable)
        {
            CallJobInfoExtended[] callJobInfoExt = new CallJobInfoExtended[dataTable.Rows.Count];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[i];
                CallJobInfoExtended callJobInfoExtended = CallJobInfoExtendedDAL.ConvertToCallJobInfoExtended(row);
                callJobInfoExtended.ProjectTerm = (string)row["ProjectTerm"];
                callJobInfoExtended.ProjektJahr = (int)row["ProjektJahr"];
                callJobInfoExtended.ProjektMonat = (int)row["ProjektMonat"];
                callJobInfoExt[i] = callJobInfoExtended;
            }

            return callJobInfoExt;
        }

        /// <summary>
        /// Gibt Adressen zum Telefonieren frei und erstellt die 
        /// dafür erforedrlichen CallJobs
        /// </summary>
        /// <param name="infoList"></param>
        public static void CreateCallJobsByAddressReleaseInfos(AddressReleaseInfo[] infoList)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@AddressInfoXml", GetReleaseInfoXml(infoList));

            SqlHelper.ExecuteStoredProc(spCallJobs_CreateByAddressReleaseInfos, parameters);

        }

        /// <summary>
        /// Aktualisiert einen CallJob auf dem Server
        /// </summary>
        /// <param name="callJob"></param>
        public static void UpdateCallJob(CallJob callJob)
        {
            IDictionary<string, object> parameters = GetParameters(callJob);
            SqlHelper.ExecuteStoredProc(spCallJob_Update, parameters);

            ObjectCache.Add(callJob.CallJobId, callJob, TimeSpan.FromSeconds(20));
        }
        
        /// <summary>
        /// Löscht einen CallJob auf dem Server
        /// </summary>
        /// <param name="callJobId"></param>
        public static void DeleteCallJob(Guid callJobId)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@CallJobId", callJobId);
            parameters.Add("@CurrentUser", Guid.NewGuid());
            SqlHelper.ExecuteStoredProc(spCallJob_Delete, parameters);

            ObjectCache.Remove(callJobId);
        }

        /// <summary>
        /// Liefert einen CallJob aufgrund der angegebenen CallJobId
        /// </summary>
        /// <param name="callJobId"></param>
        /// <returns></returns>
        public static CallJob GetCallJob(Guid callJobId)
        {

            CallJob callJob = null;

            callJob = ObjectCache.Get<CallJob>(callJobId);

            if (callJob != null)
                return callJob;
            
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@CallJobId", callJobId);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spCallJob_GetSingle, parameters);

            if (dataTable.Rows.Count < 1)
            {
                return null;
            }
            else
            {
                return ConvertToCallJob(dataTable.Rows[0]);
            }
            
        }

        /// <summary>
        /// Liefert einen CallJob anhand der AddressId und der ProjectId
        /// </summary>
        /// <param name="addressId"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public static CallJob GetCallJob(Guid addressId, Guid projectId)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@AddressId", addressId);
            parameters.Add("@ProjectId", projectId);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spCallJob_GetSingleBySponsorproject, parameters);

            if (dataTable.Rows.Count < 1)
            {
                return null;
            }
            else
            {
                return ConvertToCallJob(dataTable.Rows[0]);
            }

        }

        /// <summary>
        /// Liefert eine Liste von Mahnungs-CallJobs für den angegebenen User
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static DurringCallJob[] GetDurringCallJobsByUser(User user)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@UserId", user.UserId);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spCallJobDurring_GetListByUser, parameters);

            return (DurringCallJob[])ConvertToCallJobs(dataTable);
        }

        /// <summary>
        /// Liefert eine Liste von Sponsor-CallJobs für das angegebene Project (synchron)
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public static SponsoringCallJob[] GetSponsoringCallJobsByProject(Project project)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@ProjectId", project.ProjectId);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spCallJob_GetListByProject, parameters);

            return (SponsoringCallJob[]) ConvertToCallJobs(dataTable);
        }

        /// <summary>
        /// Liefert eine Liste von Sponsor-CallJobs für das angegebene Project (asynchron)
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public static SponsoringCallJob[] GetSponsoringCallJobsByProject(Project project,
            AsyncOperation asyncOp,
            SendOrPostCallback reportProgressDelegate)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@ProjectId", project.ProjectId);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spCallJob_GetListByProject, parameters);

            CallJob[] callJobs = ConvertToCallJobs(dataTable, asyncOp, reportProgressDelegate);

            SponsoringCallJob[] sponsoringCallJobs = new SponsoringCallJob[callJobs.Length];

            Array.Copy(callJobs, sponsoringCallJobs, callJobs.Length);

            return sponsoringCallJobs;
        }

        /// <summary>
        /// Liefert einen neuen leeren Sponsoring CallJob
        /// </summary>
        /// <returns></returns>
        public static CallJob GetNewSponsoringCallJob()
        {
            string callJobType = "metatop.Applications.metaCall.DataObjects.SponsoringCallJob";

            Type type = typeof(CallJob).Assembly.GetType(callJobType);
            CallJob callJob = Activator.CreateInstance(type) as CallJob;
            return callJob;
        }
        
        public static CallJob[] GetCallJobsByUserAndProject(UserInfo user, ProjectInfo project, string expression, bool isAdminMode)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@ProjectId", project == null ? null : (Guid?) project.ProjectId);
            parameters.Add("@UserId", user.UserId);
            parameters.Add("@Expression", expression);
            parameters.Add("@AdminMode", isAdminMode);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spCallJob_GetListByUserAndProject, parameters);

            return ConvertToCallJobs(dataTable);
        }

        public static CallJob[] GetCallJobsByUserAndProject(UserInfo user, ProjectInfo project, 
            string expression, bool isAdminMode, bool excludeRefusals)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@ProjectId", project == null ? null : (Guid?) project.ProjectId);
            parameters.Add("@UserId", user.UserId);
            parameters.Add("@Expression", expression);
            parameters.Add("@AdminMode", isAdminMode);
            parameters.Add("@excludeRefusals", excludeRefusals);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spCallJob_GetListByUserAndProject, parameters);

            return ConvertToCallJobs(dataTable);
        }

        private static IDictionary<string, object> GetParameters(CallJob callJob)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@CallJobId", callJob.CallJobId);
            parameters.Add("@StartDate", callJob.StartDate);
            parameters.Add("@StopDate", callJob.StopDate);
            parameters.Add("@State", callJob.State);
            parameters.Add("@IterationCounter", callJob.IterationCounter);
            parameters.Add("@AddressSafeActiv", callJob.AddressSafeActiv);

            
            if (callJob.User == null)
                parameters.Add("@UserId", null);
            else
                parameters.Add("@UserId", callJob.User.UserId);
            
            if (callJob.Team == null)
                parameters.Add("@TeamId", null);
            else
                parameters.Add("@TeamId", callJob.Team.TeamId);

            if (callJob.CallJobGroup == null)
                parameters.Add("@CallJobGroupId", null);
            else
                parameters.Add("@CallJobGroupId", callJob.CallJobGroup.CallJobGroupId);

            parameters.Add("@DialMode", callJob.DialMode);

            return parameters;
        }

        private static CallJob ConvertToCallJob(DataRow row)
        {

            string callJobType = "metatop.Applications.metaCall.DataObjects." + (string)row["CallJobType"];
//            callJobType = "metatop.Applications.metaCall.DataObjects." + (string)row["CallJobType"];
            //string callJobType = "metatop.Applications.metaCall.DataObjects.SponsoringCallJob";

            
            Type type = typeof(CallJob).Assembly.GetType(callJobType);
            CallJob callJob = Activator.CreateInstance(type) as CallJob;

            callJob.CallJobId = (Guid) row["CallJobId"];
            callJob.Sponsor = AddressDAL.GetSponsor((Guid) row["AddressId"]);
            callJob.Project = ProjectDAL.GetProjectInfo((Guid)row["ProjectId"]);
            callJob.User = UserDAL.GetUserInfo((Guid?)SqlHelper.GetNullableDBValue( row["UserId"]));
            callJob.Team = TeamDAL.GetTeamInfo((Guid?) SqlHelper.GetNullableDBValue(row["TeamId"]));
            callJob.StartDate = (DateTime)row["StartDate"];
            callJob.StopDate = (DateTime)row["StopDate"];
            callJob.IterationCounter = (int)row["CallJobIterationCounter"];
            callJob.State = (CallJobState)row["State"];
            callJob.DialMode = (DialMode)row["DialMode"];
            callJob.AddressSafeActiv = (Boolean)row["AddressSafeActiv"];
            if (SqlHelper.GetNullableDBValue(row["CallJobGroupId"]) == null)
                callJob.CallJobGroup = null;
            else
                callJob.CallJobGroup = CallJobGroupDAL.GetCallJobGroupInfo((Guid)row["CallJobGroupId"]);

            if (callJob.GetType() == (typeof(DurringCallJob)))
            {
                DurringCallJob durringCallJob = (DurringCallJob)callJob;

                durringCallJob.Invoice = InvoiceDAL.GetInvoice((Guid?)SqlHelper.GetNullableDBValue(row["CallJobId"]));
                
            }

            ObjectCache.Add(callJob.CallJobId, callJob, TimeSpan.FromSeconds(20));

            return callJob;
        }
        
        private static CallJob[] ConvertToCallJobs(DataTable dataTable)
        {
            CallJob[] callJobs = new CallJob[dataTable.Rows.Count];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[i];
                callJobs[i] = ConvertToCallJob(row);
            }

            return callJobs;
        }

        private static CallJob[] ConvertToCallJobs(DataTable dataTable, 
            AsyncOperation asyncOp,
            SendOrPostCallback reportProgressDelegate)
        {
            CallJob[] callJobs = new CallJob[dataTable.Rows.Count];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[i];
                callJobs[i] = ConvertToCallJob(row);

                GetCallJobsProgressChangedEventArgs e = new GetCallJobsProgressChangedEventArgs(
                    callJobs[i],
                    dataTable.Rows.Count,
                    i, 
                    (int)(((float)i / (float)dataTable.Rows.Count) * 100),
                    asyncOp.UserSuppliedState);

                asyncOp.Post(reportProgressDelegate, e);


                // Yield the rest of this time slice.
                Thread.Sleep(0);
            }

            return callJobs;
        }
        
        private static string GetReleaseInfoXml(AddressReleaseInfo[] infoList)
        {
            //Erstellt eine XML-Zeichenfolge der Art
            // <centerAdmins>
            //    <CenterAdmin UserId=".." />
            // </CenterAdmins>

            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;

            using (XmlWriter writer = XmlWriter.Create(sb, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("ReleaseInfos");


                foreach (AddressReleaseInfo info in infoList)
                {
                    string userId = info.PreferredUser == null ? null : info.PreferredUser.UserId.ToString();
                    string teamId = info.PreferredTeam == null ? null : info.PreferredTeam.TeamId.ToString();

                    if (info.StartDate < SqlDateTime.MinValue.Value)
                        info.StartDate = SqlDateTime.MinValue.Value;
                    if (info.StartDate > SqlDateTime.MaxValue.Value)
                        info.StartDate = SqlDateTime.MaxValue.Value;

                    if (info.StopDate < SqlDateTime.MinValue.Value)
                        info.StopDate = SqlDateTime.MinValue.Value;
                    if (info.StopDate > SqlDateTime.MaxValue.Value)
                        info.StopDate = SqlDateTime.MaxValue.Value;

                    writer.WriteStartElement("ReleaseInfo");
                    writer.WriteAttributeString("AddressId", info.Sponsor.AddressId.ToString());
                    writer.WriteAttributeString("ProjectId", info.Project.ProjectId.ToString());
                    writer.WriteAttributeString("StartDate", info.StartDate.ToString("yyyyMMdd HH:mm:ss"));
                    writer.WriteAttributeString("StopDate", info.StopDate.ToString("yyyyMMdd HH:mm:ss"));
                    writer.WriteAttributeString("UserId", userId);
                    writer.WriteAttributeString("TeamId", teamId);
                    writer.WriteAttributeString("UserCreated", Guid.NewGuid().ToString());
                    writer.WriteAttributeString("State", ((int)CallJobState.FirstCall).ToString());
                    writer.WriteAttributeString("CallJobIterationCounter", "0");
                    writer.WriteAttributeString("DialMode", ((int)info.Project.DialMode).ToString());
                    writer.WriteAttributeString("CallJobGroupId", info.CallJobGroup.CallJobGroupId.ToString());
                    writer.WriteEndElement();


                }

                writer.WriteEndElement();
            }

            return sb.ToString();
        }

        #endregion

        #region CallJobStates
        public static CallJobStateInfo GetCallJobStateInfo(CallJobState callJobState)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@CallJobStateId", (int)callJobState);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spCallJobState_GetSingle, parameters);

            if (dataTable.Rows.Count < 1)
                return null;
            else
                return ConvertToCallJobStateInfo(dataTable.Rows[0]);
        }

        public static CallJobStateInfo[] GetCallJobStateInfos()
        {
            DataTable dataTable = SqlHelper.ExecuteDataTable(spCallJobState_GetAll);

            return ConvertToCallJobStateInfos(dataTable);
        }

        private static CallJobStateInfo ConvertToCallJobStateInfo(DataRow row)
        {
            CallJobStateInfo callJobStateInfo = new CallJobStateInfo();
            callJobStateInfo.CallJobStateId = (int)row["CallJobStateId"];
            callJobStateInfo.DisplayName = (string)row["DisplayName"];
            callJobStateInfo.Description = (string)row["Description"];

            return callJobStateInfo;
        }

        private static CallJobStateInfo[] ConvertToCallJobStateInfos(DataTable dataTable)
        {
            CallJobStateInfo[] callJobStateInfos = new CallJobStateInfo[dataTable.Rows.Count];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[i];
                callJobStateInfos[i] = ConvertToCallJobStateInfo(row);
            }

            return callJobStateInfos;
        }
        #endregion

        #region ActivityTimeItems
        public static void CreateCallJobActivityTimeItem(CallJobActivityTimeItem callJobActivityTimeItem)
        {
            callJobActivityTimeItem.UploadMetaWare = 1;
            IDictionary<string, object> parameters;
            try
            {
                // Try-Block zum speichern in metaWare
                parameters = GetParameters(callJobActivityTimeItem);
                SqlHelper.ExecuteStoredProc(spCallJobs_ActivityTimeItemCreate_MetaWare, parameters);
            }
            catch (Exception ex)
            {
                // wenn ein Fehler auftritt muss UploadMetaWare-Flag auf false gesetzt werden.
                callJobActivityTimeItem.UploadMetaWare = 0;
                Logger.Write(ex.Message, "CreateCallJobActivityTimeItem--" + callJobActivityTimeItem.ActivityTimeId.ToString(), 99, 2, System.Diagnostics.TraceEventType.Transfer, "Fehler-MetaWare-Upload");
            }
            finally
            {
                //in metaCall soll der Eintrag auf jedenfall gespeichert werden.
                parameters = GetParameters(callJobActivityTimeItem);
                SqlHelper.ExecuteStoredProc(spCallJobs_ActivityTimeItemCreate, parameters);
            }
        }

        public static void UpdateCallJobActivityTimeItem(CallJobActivityTimeItem callJobActivityTimeItem)
        {
            callJobActivityTimeItem.UploadMetaWare = 1;
            IDictionary<string, object> parameters;
            try
            {
                // Try-Block zum speichern in metaWare
                parameters = GetParameters(callJobActivityTimeItem);
                SqlHelper.ExecuteStoredProc(spCallJobs_ActivityTimeItemUpdate_MetaWare, parameters);
            }
            catch
            {
                // wenn ein Fehler auftritt muss UploadMetaWare-Flag auf false gesetzt werden.
                callJobActivityTimeItem.UploadMetaWare = 0;
            }
            finally
            {
                //in metaCall soll der Eintrag auf jedenfall gespeichert werden.
                parameters = GetParameters(callJobActivityTimeItem);
                SqlHelper.ExecuteStoredProc(spCallJobs_ActivityTimeItemUpdate, parameters);
            }
        }

        private static IDictionary<string, object> GetParameters(CallJobActivityTimeItem callJobActivityTimeItem)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@ActivityTimeId", callJobActivityTimeItem.ActivityTimeId);
            parameters.Add("@UserId", callJobActivityTimeItem.User.UserId);
            parameters.Add("@CallJobId", callJobActivityTimeItem.CallJob.CallJobId);
            parameters.Add("@Start", callJobActivityTimeItem.Start);
            parameters.Add("@Stop", callJobActivityTimeItem.Stop);
            parameters.Add("@Duration", callJobActivityTimeItem.Duration);
            parameters.Add("@ActivityTimeItemType", callJobActivityTimeItem.ActivityTimeItemType);
            parameters.Add("@StartActivityId", callJobActivityTimeItem.StartActivityId);
            parameters.Add("@StopActivityId", callJobActivityTimeItem.StopActivityId);
            parameters.Add("@UploadMetaWare", callJobActivityTimeItem.UploadMetaWare);

            return parameters;
        }
        #endregion

        #region CallJob_Durring
        public static void CreateDurring(CallJobPossibleResult result)
        {
            DurringCallJob callJob = (DurringCallJob)result.CallJob;
            mwUser mwUser = result.mwUser;

            DateTime? faelligAm =  callJob.Invoice.FaelligAm;

            //Wenn Der Benutzer einen Reminder erstellt hat, wird die fälligkeit auf 
            //den Reminder gelegt
            if (result.Reminder != null)
            {
                faelligAm = result.Reminder.ReminderDateStart;
            }

            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@Mahndatum", faelligAm);
            parameters.Add("@ProjekteRechnungsnummer", callJob.Invoice.ProjekteRechnungsnummer);
            parameters.Add("@Notice", result.Notice);
            parameters.Add("@Bearbeiter", mwUser.Nachname);

            SqlHelper.ExecuteStoredProc(sp_mwDurringUpdate, parameters);

        }

#endregion

        #region CallJob_SponsoringOrders
        public static void CreateSponsoringOrder(CallJobPossibleResult result)
        {
            CallJob callJob = result.CallJob;
            Sponsor sponsor = callJob.Sponsor;
            ProjectInfo project = callJob.Project;
            User user = result.User;
            mwUser mwUser = result.mwUser;

            if (!project.mwProjektNummer.HasValue)
                new InvalidOperationException("Cannot create an order for a nonmetaware-project.");

            string werbeText = result.AdvertisingText;
            DateTime? zahlungsZiel = result.PaymentTarget;

            int? orderNumber ;

            string sponsorenPaketXml = GetSponsorenPaketXml(result);
            string thankingForms = GetBedankungsFormenXml(result.ThankingsFormsProject);

            CreatemwOrder(project.mwProjektNummer.Value,
                sponsor.AdressenPoolNummer,
                mwUser.PartnerNummer,
                DateTime.Today,
                result.Branch.Branchennummer,
                mwUser.MemberName,
                result.Notice,
                werbeText,
                zahlungsZiel,
                sponsorenPaketXml,
                thankingForms,
                mwUser.MemberName,
                sponsor.ContactPerson.Anrede,
                sponsor.ContactPerson.Titel,
                sponsor.ContactPerson.Nachname,
                sponsor.ContactPerson.Vorname,
                sponsor.EMail,
                sponsor.Bank,
                sponsor.BankNumber,
                sponsor.AccountNumber,
                sponsor.MobilNummer,
                sponsor.EinzugsermaechtigungAusgabe,
                sponsor.SponsorenUrkunde1,
                sponsor.SponsorenUrkunde2,
                sponsor.Additions.Phone3,
                out orderNumber);

            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@CallJobsSponsoringOrderId", Guid.NewGuid());
            parameters.Add("@CallJobId", callJob.CallJobId);
            parameters.Add("@Auftragsnummer", orderNumber);
            parameters.Add("@SponsorPaketNumber", result.mwProjekt_SponsorPacket.ProjekteSponsorenpaketNummer);
            parameters.Add("@SponsorPaketCount", result.SponsorPacketCount);
            parameters.Add("@PaymentTarget", result.PaymentTarget);
            parameters.Add("@ThankingForms", thankingForms);

            SqlHelper.ExecuteStoredProc(spCallJobs_SponsoringOrdersCreate, parameters);

        }

        private static string GetBedankungsFormenXml(ThankingsFormsProject[] thankingsFormsProject)
        {

            //Erstellt eine XML-Zeichenfolge der Art
            // <centerAdmins>
            //    <CenterAdmin UserId=".." />
            // </CenterAdmins>

            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;

            using (XmlWriter writer = XmlWriter.Create(sb, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("ThankingForms");

                foreach (ThankingsFormsProject thankingForm in thankingsFormsProject)
                {
                    writer.WriteStartElement("ThankingForm");
                    writer.WriteAttributeString("ThankingFormNumber", thankingForm.ID.ToString());
                    writer.WriteEndElement();
                    
                }

                writer.WriteEndElement();
            }

            return sb.ToString();
        }

        private static string GetSponsorenPaketXml(CallJobPossibleResult result)
        {
            //Erstellt eine XML-Zeichenfolge der Art
            // <centerAdmins>
            //    <CenterAdmin UserId=".." />
            // </CenterAdmins>

            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;

            using (XmlWriter writer = XmlWriter.Create(sb, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("SponsorPackages");

                writer.WriteStartElement("SponsorPackage");
                writer.WriteAttributeString("SponsorpackageNumber", result.mwProjekt_SponsorPacket.ProjekteSponsorenpaketNummer.ToString());
                writer.WriteAttributeString("CountOfPackages", result.SponsorPacketCount.ToString());
                writer.WriteEndElement();


                writer.WriteEndElement();
            }

            return sb.ToString();
        }
        #endregion

        #region CallJob_SponsoringCancellations
        public static void CreateSponsoringCancellation(CallJobPossibleResult result)
        {
            CallJob callJob = result.CallJob;

            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@CallJobSponsoringCancellationId", Guid.NewGuid());
            parameters.Add("@CallJobId", callJob.CallJobId);
            parameters.Add("@ContactTypesParticipationCancellationId", result.ContactTypesParticipationCancellation.ContactTypesParticipationCancellationId);
            parameters.Add("@StoreTipAddress", 0);
            parameters.Add("@StoreSecondCall", 0);

            SqlHelper.ExecuteStoredProc(spCallJobs_SponsoringCancellationCreate, parameters);
        }
        #endregion

        #region CallJobAddressUnsuitable
        public static void CreateAddressUnsuitable(CallJobUnsuitableResult result)
        {
            CallJob callJob = result.CallJob;

            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@CallJobSponsoringUnsuitableId", Guid.NewGuid());
            parameters.Add("@CallJobId", callJob.CallJobId);
            parameters.Add("@ContactTypesParticipationUnsuitableId", result.ContactTypesParticipationUnsuitable.ContactTypesParticipationUnsuitableId);

            SqlHelper.ExecuteStoredProc(spCallJobs_AddressUnsuitableCreate, parameters);
        }
        #endregion

        #region UpdateCallJobsDialModeByProject

        public static void UpdateCallJobsDialModeByProject(Project project, DialMode dialMode)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@ProjectId", project == null ? null : (Guid?)project.ProjectId);
            parameters.Add("@DialMode", (int)dialMode);

            //DataTable dataTable = SqlHelper.ExecuteDataTable(spDurringCreate, parameters);
            SqlHelper.ExecuteStoredProc(spCallJobs_UpdateDialModeByProject, parameters, 120);

        }

        #endregion

        #region Auftragsverteilung

        /// <summary>
        /// Liefert eine Auflistung der Auftragsverteilung pro Stunde
        /// </summary>
        /// <param name="centerId"></param>
        /// <param name="teamId"></param>
        /// <param name="userId"></param>
        /// <param name="projectId"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static SponsoringOrdersDistribution[] GetSponsoringOrdersDistribution(
                                Guid? centerId,
                                Guid? teamId,
                                Guid? userId,
                                Guid? projectId,
                                DateTime from,
                                DateTime to)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@Start", from);
            parameters.Add("@Stop", to);
            parameters.Add("@UserId", userId);
            parameters.Add("@CenterId", centerId);
            parameters.Add("@TeamId", teamId);
            parameters.Add("@ProjectId", projectId);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spCallJobs_SponsoringOrdersDistribution, parameters);

            return ConvertToSponsoringOrdersDistributions(dataTable);
        }

        private static SponsoringOrdersDistribution[] ConvertToSponsoringOrdersDistributions(DataTable dataTable)
        {
            SponsoringOrdersDistribution[] distributions = new SponsoringOrdersDistribution[dataTable.Rows.Count];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[i];
                distributions[i] = ConvertToSponsoringOrdersDistribution(row);
            }

            return distributions;
        }

        private static SponsoringOrdersDistribution ConvertToSponsoringOrdersDistribution(DataRow row)
        {
            SponsoringOrdersDistribution distribution = new SponsoringOrdersDistribution();
            distribution.Hour = (string)row["Hour"];
            distribution.Count = (double)row["Count"];
            distribution.CountOrder = (double)row["OrderCount"];

            return distribution;
        }

        public static DateTime? GetLastAddressContact(int adressenPoolNummer, Guid projectId)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@AdressenPoolNummer", adressenPoolNummer);
            parameters.Add("@ProjectId", projectId);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spCallJob_GetLastAddressContact, parameters);

            if (dataTable.Rows.Count < 1)
                return null;
            else
            {
                DataRow row = dataTable.Rows[0];

                return (DateTime)row["LastContact"];
            }
         }

        #endregion

        #region mwOrder
        public static void CreatemwOrder(
            int projektNummer,
            int adressenPoolNummer,
            int partnerNummer,
            DateTime auftragsDatum,
            int branchenNummer,
            string auftragsBearbeiter,
            string notiz,
            string werbetext,
            DateTime? zahlungsZiel,
            string sponsorenPaketXml,
            string bedankungsFormenXml,
            string currentUser,
            string ap_Anrede,
            string ap_Titel,
            string ap_Name,
            string ap_Vorname,
            string eMail,
            string Bank,
            string Bankleitzahl,
            string Kontonummer,
            string MobilNummer,
            int EinzugsermaechtigungAusgabe,
            string SponsorUrkunde1,
            string SponsorUrkunde2,
            string Alternativnummer,
            out int? orderNumber
            )

        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@ProjektNummer", projektNummer);
            parameters.Add("@adressenPoolNummer", adressenPoolNummer);
            parameters.Add("@AuftragsDatum", auftragsDatum);
            parameters.Add("@BranchenNummer", branchenNummer);
            parameters.Add("@Auftragsbearbeiter", auftragsBearbeiter);
            parameters.Add("@Notiz", notiz);
            parameters.Add("@Werbetext", werbetext);
            parameters.Add("@Zahlungsziel", zahlungsZiel);
            parameters.Add("@Verkaeufer1", partnerNummer); //HACK
            parameters.Add("@SponsorenPaketeXml", sponsorenPaketXml);
            parameters.Add("@BedankungsformenXml", bedankungsFormenXml);
            parameters.Add("@currentUSer", currentUser);
            parameters.Add("@AP_Anrede", ap_Anrede);
            parameters.Add("@AP_Titel", ap_Titel);
            parameters.Add("@AP_Name", ap_Name);
            parameters.Add("@AP_Vorname", ap_Vorname);

            parameters.Add("@eMail", eMail);
            parameters.Add("@Bank", Bank);
            parameters.Add("@Bankleitzahl", Bankleitzahl);
            parameters.Add("@Kontonummer", Kontonummer);
            parameters.Add("@MobilNummer", MobilNummer);
            parameters.Add("@EinzugsermaechtigungAusgabe", EinzugsermaechtigungAusgabe);
            parameters.Add("@SponsorUrkunde1", SponsorUrkunde1);
            parameters.Add("@SponsorUrkunde2", SponsorUrkunde2);
            parameters.Add("@Alternativnummer", Alternativnummer);

            DataTable dataTable = new DataTable();
            try
            {
                dataTable = SqlHelper.ExecuteDataTable(spmwOrder_Create, parameters);
            }
            catch (Exception ex)
            {
                Logger.Write(ex.Message, "CreatemwOrder--branch--" + branchenNummer.ToString() + "--" + projektNummer.ToString() + "--" + adressenPoolNummer + "--", 88, 3, System.Diagnostics.TraceEventType.Transfer, "Fehler-Auftrag-MetaWare");
                throw;
            }
            finally
            {
                if (dataTable.Rows.Count < 1)
                    orderNumber = null;
                else
                    orderNumber = (int)dataTable.Rows[0][0];
            }
            
        }

        #endregion

        #region DurringLevelInfo

        public static DurringLevelInfo[] GetDurringLevelInfoByUser(User user)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@UserId", user.UserId);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spDurringLevelInfo_GetByUser, parameters);

            return ConvertToDurringLevelInfos(dataTable);

        }

        private static DurringLevelInfo[] ConvertToDurringLevelInfos(DataTable dataTable)
        {
            DurringLevelInfo[] infos = new DurringLevelInfo[dataTable.Rows.Count];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[i];
                infos[i] = ConvertToDurringLevelInfo(row);
            }

            return infos;
        }

        private static DurringLevelInfo ConvertToDurringLevelInfo(DataRow row)
        {
            DurringLevelInfo info = new DurringLevelInfo();
            info.Mahnstufe2 = (int)row["Mahnstufe2"];

            return info;
        }

        #endregion

        #region DurringCreate

        public static void DurringsCreate(User user)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@Partnernummer", user.mwUser.PartnerNummer);

            //DataTable dataTable = SqlHelper.ExecuteDataTable(spDurringCreate, parameters);
            SqlHelper.ExecuteStoredProc(spDurringCreate, parameters, 120);

        }

        #endregion

        #region DurringInfo

        public static DurringInfo[] GetDurringInfosByUser(User user, int mahnstufe2)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@UserId", user.UserId);
            parameters.Add("@Mahnstufe2", mahnstufe2);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spDurringInfo_GetByUser, parameters);

            return ConvertToDurringInfos(dataTable);
        }

        private static DurringInfo[] ConvertToDurringInfos(DataTable dataTable)
        {
            DurringInfo[] infos = new DurringInfo[dataTable.Rows.Count];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[i];
                infos[i] = ConvertToDurringInfo(row);
            }

            return infos;

        }

        private static DurringInfo ConvertToDurringInfo(DataRow row)
        {
            DurringInfo info = new DurringInfo();

            info.ProjekteRechnungsnummer = (int)row["ProjekteRechnungsnummer"];
            info.AnsprechpartnerText = (string)SqlHelper.GetNullableDBValue(row["AnsprechpartnerText"]);
            info.BetragOffen = (decimal)row["BetragOffen"];
            info.Brutto = (decimal)row["Brutto"];
            info.Mahnstufe2 = (int)row["Mahnstufe2"];
            info.Mahnungsdatum = (DateTime)row["Mahnungsdatum"];
            info.ProjektText = (string)SqlHelper.GetNullableDBValue(row["ProjektText"]);
            info.Rechnungsdatum = (DateTime)row["Rechnungsdatum"];
            info.Rechnungsnummer = (int)row["Rechnungsnummer"];
            info.Sponsortext = (string)SqlHelper.GetNullableDBValue(row["Sponsortext"]);
            info.Stueckzahl = (decimal)row["Stueckzahl"];
            info.Telefonnumer = (string)SqlHelper.GetNullableDBValue(row["Telefonnummer"]);
            if (SqlHelper.GetNullableDBValue(row["Wiedervorlage"])!=null)
                info.Wiedervorlage = (DateTime) row["Wiedervorlage"];
            info.Prio = (string) SqlHelper.GetNullableDBValue(row["Prio"]);
            if (SqlHelper.GetNullableDBValue(row["Aktionsdatum"]) != null)
                info.Aktionsdatum = (DateTime) row["Aktionsdatum"];
            info.BemerkungMahnungsaktion = (string)SqlHelper.GetNullableDBValue(row["BemerkungMahnungsaktion"]);
            info.BearbeiterMahnungsaktion = (string)SqlHelper.GetNullableDBValue(row["BearbeiterMahnungsaktion"]);
            info.CallJobId = (Guid)SqlHelper.GetNullableDBValue(row["CallJobId"]);

            return info;
        }
 
        #endregion

        #region TransferUnusedCallJobs

        public static void TransferUnusedCallJobs(Guid sourceProjectId, Guid targetProjectId)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@ProjectIdSource", sourceProjectId);
            parameters.Add("@ProjectIdTarget", targetProjectId);

            SqlHelper.ExecuteStoredProc(spCallJobs_Unused_Transfer, parameters, 120);

        }

        public static int TransferUnusedCallJobsCount(Guid sourceProjectId)
        {
            int numberOfCallJobs;

            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@ProjectIdSource", sourceProjectId);

            DataTable dataTable = new DataTable();
            try
            {
                dataTable = SqlHelper.ExecuteDataTable(spCallJobs_Unused_Transfer_Count, parameters, 120);
            }
            catch 
            {
                throw;
            }
            finally
            {
                if (dataTable.Rows.Count < 1)
                    numberOfCallJobs = 0;
                else
                    numberOfCallJobs = (int)dataTable.Rows[0][0];
            }
            return numberOfCallJobs;

        }
        #endregion
    }
}
