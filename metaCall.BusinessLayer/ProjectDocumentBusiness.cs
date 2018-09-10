using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using metatop.Applications.metaCall.DataObjects;
using metatop.Applications.metaCall.ServiceAccessLayer;
using System.IO;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Net;
using System.Net.Mail;
using Microsoft.Exchange.WebServices.Data;
using Attachment = System.Net.Mail.Attachment;

namespace metatop.Applications.metaCall.BusinessLayer
{
    public class ProjectDocumentBusiness
    {
        private BusinessLayer.MetaCallBusiness metaCallBusiness;
        static bool mailSent = false;

        internal ProjectDocumentBusiness(MetaCallBusiness metacallBusiness)
        {
            this.metaCallBusiness = metacallBusiness;
        }

        /// <summary>
        /// Erstellt ein neues ProjectDocment auf dem Server
        /// </summary>
        /// <param name="project"></param>
        /// <param name="displayName"></param>
        /// <param name="category"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public ProjectDocument Create(ProjectInfo project, string displayName, DocumentCategory category, string filename, bool packetSelect)
        {
            if (project == null)
            {
                throw new ArgumentNullException("project");
            }

            if (string.IsNullOrEmpty(displayName))
            {
                throw new ArgumentNullException("displayName");
            }

            if (string.IsNullOrEmpty(filename))
            {
                throw new ArgumentNullException("filename");
            }

            ProjectDocument document = new ProjectDocument();
            document.DocumentId = Guid.NewGuid();
            document.Project = project;
            document.DisplayName = displayName;
            document.Category = category;
            document.Filename = filename;
            document.PacketSelect = packetSelect;
            document.DateCreated = DateTime.Now;

            this.metaCallBusiness.ServiceAccess.CreateProjectDocument(document);

            return document;
        }

        /// <summary>
        /// Aktualisiert ein vorhandenes ProjectDokcument auf dem Server
        /// </summary>
        /// <param name="document"></param>
        public void Update(ProjectDocument document)
        {

            if (document == null)
            {
                throw new ArgumentNullException("document");
            }

            if (document.Project == null)
            {
                throw new ArgumentNullException("project");
            }

            if (string.IsNullOrEmpty(document.DisplayName))
            {
                throw new ArgumentNullException("displayName");
            }

            if (string.IsNullOrEmpty(document.Filename))
            {
                throw new ArgumentNullException("filename");
            }

            this.metaCallBusiness.ServiceAccess.UpdateProjectDocument(document);
        }

        /// <summary>
        /// Löscht ein vorhandenes ProjectDokument auf dem Server
        /// </summary>
        /// <param name="document"></param>
        public void Delete(ProjectDocument document)
        {
            this.metaCallBusiness.ServiceAccess.DeleteProjectDocument(document.DocumentId);

            document.DocumentId = Guid.Empty;
            document.DisplayName = null;
            document.DateCreated = DateTime.MinValue;
            document.Filename = null;
            document.Project = null;
        }

        /// <summary>
        /// Liefert eine einzelne Instanz der Klasse ProjectDocument aufgrund der DocumentId
        /// </summary>
        /// <param name="documentId"></param>
        /// <returns></returns>
        public ProjectDocument GetDocument(Guid documentId)
        {
            if (documentId == Guid.Empty)
            {
                throw new ArgumentNullException("documentId");
            }

            return this.metaCallBusiness.ServiceAccess.GetProjectDocument(documentId);
        }

        /// <summary>
        /// Liefert eine Liste von ProjectDocument-Instanzen zu einem gegebenen Projekt
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public List<ProjectDocument> GetDocumentsByProject(ProjectInfo project)
        {
            if (project == null)
            {
                throw new ArgumentNullException("project");
            }

            return new List<ProjectDocument>(this.metaCallBusiness.ServiceAccess.GetProjectDocumentsByProject(project.ProjectId));
        }

        /// <summary>
        /// Liefert eine Liste von ProjectDocument-Instanzen zu 
        /// einem gegebenen Projekt und einer bestimmten Kategorie
        /// </summary>
        /// <param name="project"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        public List<ProjectDocument> GetDocumentsByProjectAndCategory(ProjectInfo project, DocumentCategory category)
        {
            if (project == null)
            {
                throw new ArgumentNullException("project");
            }

            return new List<ProjectDocument>(this.metaCallBusiness.ServiceAccess.GetProjectDocumentsByProjectAndCategory(project.ProjectId, category));
        }

        /// <summary>
        /// Liefert eine Instanz der Klasse DocumentCategory
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public DocumentCategoryInfo GetDocumentCategoryInfo(DocumentCategory category)
        {
            return this.metaCallBusiness.ServiceAccess.GetDocuemntCategoryInfo(category);
        }

        /// <summary>
        /// Liefert eine Liste aller DocumentCategoryInfos
        /// </summary>
        public List<DocumentCategoryInfo> AllDocumentCategoryInfos
        {
            get
            {
                return new List<DocumentCategoryInfo>(this.metaCallBusiness.ServiceAccess.GetAllDocumentCategoryInfos());
            }
        }

        /// <summary>
        /// Ruft einen Druck/eMail oder Faxaufruf asynchron auf
        /// </summary>
        /// <param name="document"></param>
        /// <param name="callJob"></param>
        /// <param name="options"></param>
        /// <param name="printer"></param>
        public void Send(ProjectDocument document, CallJob callJob, SendProjectDocumentOptions options, string printer)
        {
            ProjectDocumentSendMethod sendMethod = new ProjectDocumentSendMethod(SendAsync);
            IAsyncResult result = sendMethod.BeginInvoke(document, callJob, options, printer, new AsyncCallback(this.SendAsyncCallBack), sendMethod);
        }

        /// <summary>
        /// Ruft einen Druck/eMail oder Faxaufruf asynchron auf
        /// </summary>
        /// <param name="document"></param>
        /// <param name="callJob"></param>
        /// <param name="options"></param>
        public void Send(ProjectDocument document, CallJob callJob, SendProjectDocumentOptions options)
        {
            Send(document, callJob, options, null);
        }

        public void Send(ProjectDocument document, ProjectDocument emailTemplate, CallJob callJob, string betreff, string briefanrede, SendProjectDocumentOptions options)
        {
            ProjectDocumentMailMethod mailMethod = new ProjectDocumentMailMethod(MailAsync);
            IAsyncResult result = mailMethod.BeginInvoke(document, emailTemplate, callJob, betreff, briefanrede, options, new AsyncCallback(this.MailAsyncCallBack), mailMethod);
        }

        private delegate void ProjectDocumentMailMethod(
            ProjectDocument document, ProjectDocument emailTemplate, CallJob calljob, string betreff, string briefanrede,
            SendProjectDocumentOptions options);

        private void MailAsyncCallBack(IAsyncResult result)
        {
            //Debugger.Break();
            if (result.IsCompleted)
            {
                LogFaxInformation("Der Mailvorgang wurde abgeschlossen.");
            }

            ProjectDocumentMailMethod method = result.AsyncState as ProjectDocumentMailMethod;
            if (method != null)
            {
                method.EndInvoke(result);
            }
        }        

        private void MailAsync(ProjectDocument document, ProjectDocument emailTemplate, CallJob callJob, string betreff, string briefanrede,
            SendProjectDocumentOptions options)
        {
            try
            {
                if (document == null)
                {
                    throw new ArgumentNullException("document");
                }

                if (emailTemplate == null)
                {
                    throw new ArgumentNullException("emailTemplate");
                }

                if (callJob == null)
                {
                    throw new ArgumentNullException("calljob");
                }

                if (string.IsNullOrEmpty(callJob.Sponsor.EMail))
                {
                    throw new ArgumentNullException("calljob.Sponsor.Email");
                }

                if (string.IsNullOrEmpty(betreff))
                    throw new ArgumentNullException("betreff");

                if (string.IsNullOrEmpty(briefanrede))
                {
                    throw new ArgumentNullException("briefanrede");
                }

                if (!File.Exists(document.Filename))
                {
                    throw new FileNotFoundException("metacall kann die angegebene Datei nicht finden.", document.Filename);
                }

                if (!File.Exists(emailTemplate.Filename))
                {
                    throw new FileNotFoundException("metacall kann die angegebene Datei nicht finden.", emailTemplate.Filename);
                }

                IDictionary<string, object> logInfos = new Dictionary<string, object>();
                logInfos.Add("Document", document);
                logInfos.Add("Emailvorlage", emailTemplate);
                logInfos.Add("EmailAdresse", callJob.Sponsor.EMail);
                LogFaxInformation("Der Asynchrone Emailversand wurde gestartet.");

                using (MSWordAdapter wordAdapter = new MSWordAdapter())
                {
                    wordAdapter.Open(document.Filename, true, false);
                    wordAdapter.DataFieldTable = ActiveFaxAdapter.GetEmptySponsorFaxDatenField();
 
                    //Parameter vorbereiten
                    Project project = this.metaCallBusiness.Projects.Get(callJob.Project);

                    DataFieldMethodParameter param = new DataFieldMethodParameter(this.metaCallBusiness);
                    param.Parameters.Add("sponsor", callJob.Sponsor);
                    param.Parameters.Add("project", project);
                    param.Parameters.Add("Customer", project.Customer);
                    param.Parameters.Add("User", this.metaCallBusiness.Users.CurrentUser);
                    param.Parameters.Add("ArrayList", wordAdapter.DataFieldTable);
 
                    wordAdapter.ProcessDataFields(param);

                    string filename = null; // "R:\\FaxVorlageTest.pdf";
                    string login = null;
                    string emailpwd = null;

                    filename = wordAdapter.SaveAsAndConvertDocumentToPdf(filename);

                    //wordAdapter.Dispose();

                    //*****************************
                    StringBuilder mailBody = new StringBuilder();

                    using (StreamReader sr = new StreamReader(emailTemplate.Filename))
                    {
                        string lineTmp;

                        while (sr.Peek() >= 0)
                        {
                            lineTmp = sr.ReadLine();

                            if (!String.IsNullOrEmpty(lineTmp))
                            {
                                lineTmp = lineTmp.Replace("$Sponsor.Anrede$", briefanrede);
                                lineTmp = lineTmp.Replace("$Benutzer.Name$", metaCallBusiness.Users.CurrentUser.Vorname + " " + metaCallBusiness.Users.CurrentUser.Nachname);
                                lineTmp = lineTmp.Replace("$Benutzer.ZusatzInfo1$", metaCallBusiness.Users.CurrentUser.AdditionalInfo1);
                                lineTmp = lineTmp.Replace("$Benutzer.ZusatzInfo2$", metaCallBusiness.Users.CurrentUser.AdditionalInfo2);
                                lineTmp = lineTmp.Replace("$Projekt.Bezeichnung$", callJob.Project.BezeichnungRechnung);
                            }

                            mailBody.Append(lineTmp);
                        }
                    }                    
                    
                    ServicePointManager.ServerCertificateValidationCallback = CertificateValidationCallBack;

                    Setting setting = new Setting();
                    setting = metaCallBusiness.Settings.GetSetting();

                    login = setting.DomainEmailLogin + "\\" + metaCallBusiness.Users.CurrentUser.AnmeldungEmail;
                    emailpwd = metaCallBusiness.EncryptionBusiness.DecryptString(metaCallBusiness.Users.CurrentUser.PasswordEmail);

                    ExchangeService service = new ExchangeService(ExchangeVersion.Exchange2007_SP1);
                    service.Credentials = new WebCredentials(login, emailpwd);
                    //service.Url = new Uri("https:/ /MTEXCHANGE01.metatop.local/EWS/Exchange.asmx");
                    service.Url = new Uri("https://mail.metatop.de/EWS/Exchange.asmx");
                    service.Timeout = 300000;

                    EmailMessage m = new EmailMessage(service);

                    m.ToRecipients.Add(callJob.Sponsor.EMail);
                    //m.CcRecipients.Add("maier@madanet.de");
                    m.Subject = betreff;
                    //m.SubjectEncoding = System.Text.Encoding.UTF8;
                    //m.IsBodyHtml = true;
                    m.Body = mailBody.ToString();
                    //m.Body = "Test Hallo Hallo Gruß";
                    //m.BodyEncoding = System.Text.Encoding.UTF8;

                    m.Attachments.AddFileAttachment(project.PraefixMailAttachment + " " + callJob.Project.BezeichnungRechnung + ".pdf", filename);
                    m.SendAndSaveCopy();

                    //*****************************
                    this.metaCallBusiness.DocumentHistory.Create("ProjectDocument", document.DocumentId,
                        options.ToString(), metaCallBusiness.Users.GetUserInfo(metaCallBusiness.Users.CurrentUser),
                        param, "CallJob", callJob.CallJobId);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "UI Policy");
                if (rethrow)
                {
                    throw;
                }
            }             
        }

        private static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            // Get the unique identifier for this asynchronous operation.
            String token = (string)e.UserState;

            //if (e.Cancelled)
            //{
            //    Console.WriteLine("[{0}] Send canceled.", token);
            //}
            //if (e.Error != null)
            //{
            //    Console.WriteLine("[{0}] {1}", token, e.Error.ToString());
            //}
            //else
            //{
            //    Console.WriteLine("Message sent.");
            //}
            mailSent = true;
        }

        private static bool CertificateValidationCallBack(object sender,
            System.Security.Cryptography.X509Certificates.X509Certificate certificate,
            System.Security.Cryptography.X509Certificates.X509Chain chain,
            System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            // If the certificate is a valid, signed certificate, return true.
            if (sslPolicyErrors == System.Net.Security.SslPolicyErrors.None)
            {
                return true;
            }

            // If there are errors in the certificate chain, look at each error to determine the cause.
            if ((sslPolicyErrors & System.Net.Security.SslPolicyErrors.RemoteCertificateChainErrors) != 0)
            {
                if (chain != null && chain.ChainStatus != null)
                {
                    foreach (System.Security.Cryptography.X509Certificates.X509ChainStatus status in chain.ChainStatus)
                    {
                        if ((certificate.Subject == certificate.Issuer) &&
                           (status.Status == System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.UntrustedRoot))
                        {
                            // Self-signed certificates with an untrusted root are valid. 
                            continue;
                        }
                        else
                        {
                            if (status.Status != System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.NoError)
                            {
                                // If there are any other errors in the certificate chain, the certificate is invalid,
                                // so the method returns false.
                                return false;
                            }
                        }
                    }
                }

                // When processing reaches this line, the only errors in the certificate chain are 
                // untrusted root errors for self-signed certificates. These certificates are valid
                // for default Exchange server installations, so return true.
                return true;
            }
            else
            {
                // In all other cases, return false.
                return false;
            }
        }

        private delegate void ProjectDocumentSendMethod(ProjectDocument document, CallJob callJob, SendProjectDocumentOptions options, string printer);

        private void SendAsyncCallBack(IAsyncResult result)
        {
            //Debugger.Break();
            if (result.IsCompleted)
            {
                LogFaxInformation("Der Faxvorgang wurde abgeschlossen.");
            }

            ProjectDocumentSendMethod method = result.AsyncState as ProjectDocumentSendMethod;
            if (method != null)
            {
                method.EndInvoke(result);
            }
        }        


        /// <summary>
        /// Druck/eMail oder Faxaufruf
        /// </summary>
        /// <param name="document"></param>
        /// <param name="callJob"></param>
        /// <param name="options"></param>
        /// <param name="printer"></param>
        private void SendAsync(ProjectDocument document, CallJob callJob, SendProjectDocumentOptions options, string printer)
        {
            try
            {
                if (document == null)
                {
                    throw new ArgumentNullException("document");
                }

                if (callJob == null)
                {
                    throw new ArgumentNullException("calljob");
                }

                if (!File.Exists(document.Filename))
                {
                    throw new FileNotFoundException("metacall kann die angegebene Datei nicht finden.", document.Filename);
                }
#if DEBUG
                if (options == SendProjectDocumentOptions.SendFax)
                {
                    callJob.Sponsor.FaxNummer = "071188023380";
                }
                else if (options == SendProjectDocumentOptions.SendMail)
                {
                    // callJob.Sponsor.EMail = "frohna@madanet.de";
                }
#endif
                IDictionary<string, object> logInfos = new Dictionary<string, object>();
                logInfos.Add("Document", document);
                LogFaxInformation("Der Asynchrone Faxversand wurde gestartet.");

                using (MSWordAdapter wordAdapter = new MSWordAdapter())
                {
                    wordAdapter.Open(document.Filename, true, false);

                    if (options != SendProjectDocumentOptions.PrintOut)
                    {
                        if (options == SendProjectDocumentOptions.SendMail)
                        {
                            string projectName;
                            projectName = callJob.Project.BezeichnungRechnung.ToString();
                            //filename = ActiveFaxAdapter.BuildEmailFileName(callJob.Sponsor, "Angebot metatop", "Anbei das gewünschte Angebot");
                            wordAdapter.DataFieldTable = ActiveFaxAdapter.BuildEmailFileName(callJob.Sponsor, "Unterstützung / " 
                                + projectName, "Anbei das gewünschte Angebot für " + projectName);
                        }
                        else if (options == SendProjectDocumentOptions.SendFax)
                        {
                            //filename = ActiveFaxAdapter.BuildFaxFileName(callJob.Sponsor);
                            wordAdapter.DataFieldTable = ActiveFaxAdapter.BuildFaxFileName(callJob.Sponsor);
                        }

                        printer = Properties.Settings.Default.ActiveFaxPrinterName;
#if DEBUG
                        wordAdapter.DataFieldTable = ActiveFaxAdapter.GetEmptySponsorFaxDatenField();
                        printer = "hp LaserJet 1320 PCL 5";
#else
                        if (string.IsNullOrEmpty(printer))
                        {
                            printer = "ActiveFax";
                        }
#endif
                        if (!ActiveFaxAdapter.ValidatePrinterName(printer))
                        {
                            throw new InvalidOperationException("ActiveFax-Drucker kann nicht gefunden werden.");
                        }
                    }

                    //Parameter vorbereiten
                    Project project = this.metaCallBusiness.Projects.Get(callJob.Project);

                    DataFieldMethodParameter param = new DataFieldMethodParameter(this.metaCallBusiness);
                    param.Parameters.Add("sponsor", callJob.Sponsor);
                    param.Parameters.Add("project", project);
                    param.Parameters.Add("Customer", project.Customer);
                    param.Parameters.Add("User", this.metaCallBusiness.Users.CurrentUser);
                    param.Parameters.Add("ArrayList", wordAdapter.DataFieldTable);

                    wordAdapter.ProcessDataFields(param);

                    string filename = null;

                    wordAdapter.PrintDocument(printer, filename);

                    this.metaCallBusiness.DocumentHistory.Create("ProjectDocument", document.DocumentId,
                        options.ToString(), metaCallBusiness.Users.GetUserInfo(metaCallBusiness.Users.CurrentUser),
                        param, "CallJob", callJob.CallJobId);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "UI Policy");
                if (rethrow)
                {
                    throw;
                }
            }  
        }

        private void LogFaxInformation(string message)
        {
            LogFaxInformation(message, null);
        }

        private void LogFaxInformation(string message, IDictionary<string, object> logInformations)
        {
            if (!Logger.IsLoggingEnabled())
            {
                return;
            }

            IDictionary<string, object> logInfos = new Dictionary<string, object>();

            logInfos.Add("user", metaCallBusiness.Users.CurrentUser);
            logInfos.Add("Session", Environment.GetEnvironmentVariable("SESSIONNAME"));

            IDictionary<string, string> sysInfos = metaCallBusiness.GetSystemInformation();
            foreach (KeyValuePair<string, string> sysInfo in sysInfos)
            {
                logInfos.Add(sysInfo.Key, sysInfo.Value);
            }
            
            if ((logInformations != null) && logInformations.Count >0)
            {
                foreach (KeyValuePair<string, object> logInformation in logInformations)
                {
                    logInfos.Add(logInformation);
                }
            }

            LogEntry logEntry = new LogEntry(message, "Fax", 20, 2001, System.Diagnostics.TraceEventType.Information, "Faxmeldungen", logInfos);
            Logger.Write(logEntry);
        }
    }

    public enum SendProjectDocumentOptions
    {
        SendMail, 
        SendFax,
        PrintOut,
    }
}
