using System;
using System.Collections.Generic;
using System.Text;

using metatop.Applications.metaCall.DataObjects;
using metatop.Applications.metaCall.ServiceAccessLayer;
using System.Xml;
using System.Drawing;

namespace metatop.Applications.metaCall.BusinessLayer
{
    public class DocumentHistoryBusiness
    {

        private MetaCallBusiness metaCallBusiness;
        
        public DocumentHistoryBusiness(MetaCallBusiness metaCallBusiness)
        {
            this.metaCallBusiness = metaCallBusiness;
        }

        public DocumentHistory Create(string documentType, 
            Guid documentId, 
            string sendOption, 
            UserInfo user,
            DataFieldMethodParameter dataFields)
        {
            return Create(documentType, documentId, sendOption, user, dataFields, null, Guid.Empty);
        }

        public DocumentHistory Create(string documentType,
            Guid documentId,
            string sendOption,
            UserInfo user,
            DataFieldMethodParameter dataFields,
            string referencedType,
            Guid referencedId)
        {

            DocumentHistory item = new DocumentHistory();
            item.DocumentHistoryId = Guid.NewGuid();
            item.DocumentId = documentId;
            item.DocumentType = documentType;
            item.DataFields = CreateDataFieldString(dataFields);
            item.ReferencedId = referencedId == Guid.Empty ? null: (Guid?)referencedId;
            item.ReferencedType = referencedType;
            item.SendDate = DateTime.Now;
            item.SendOption = sendOption;
            item.SendUser = user;

            this.metaCallBusiness.ServiceAccess.CreateDocumentHistoryItem(item);


            return item;

        }

        private string CreateDataFieldString(DataFieldMethodParameter dataFields)
        {
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings  setting = new XmlWriterSettings();
            setting.OmitXmlDeclaration = true;

            using (XmlWriter writer = XmlWriter.Create(sb, setting))
            {
                writer.WriteStartElement("DataFieldMethodParameters");

                    foreach (string key in dataFields.Parameters.Keys)
                    {
                        writer.WriteStartElement("DataFieldMethodParameter");
                        
                        writer.WriteAttributeString("key", key);


                        object data = dataFields.Parameters[key];

                        writer.WriteAttributeString("type", data.GetType().Name);
                        
                        if (data.GetType() == typeof(string))
                            writer.WriteAttributeString("data", ((string)data));
                        else if (data.GetType() == typeof(Image))
                            writer.WriteAttributeString("data", "<Image>");
                        else if (data.GetType() == typeof(User))
                        {
                            writer.WriteAttributeString("data", ((User)data).UserId.ToString());
                        }
                        else if (data.GetType() == typeof(Project))
                        {
                            writer.WriteAttributeString("data", ((Project)data).ProjectId.ToString());
                        }
                        else if (data.GetType() == typeof(ProjectInfo))
                        {
                            writer.WriteAttributeString("data", ((ProjectInfo)data).ProjectId.ToString());
                        }
                        else if (data.GetType() == typeof(Sponsor))
                        {
                            writer.WriteAttributeString("data", ((Sponsor)data).AddressId.ToString());
                        }
                        else if (data.GetType() == typeof(Customer))
                        {
                            writer.WriteAttributeString("data", ((Customer)data).AddressId.ToString());
                        }
                        else
                            writer.WriteAttributeString("data", "unknown");

                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
            }

            return sb.ToString();
        }

        public List<DocumentHistory> GetHistoryByCallJob(CallJob callJob)
        {
            if (callJob == null)
                throw new ArgumentNullException("callJob");

            return new List<DocumentHistory>(this.metaCallBusiness.ServiceAccess.GetDocumentHistoryItemsByCallJob(callJob));
        }

    }
}
