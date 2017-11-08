using System;
using System.Collections.Generic;
using System.Text;

namespace metatop.Applications.metaCall.DataObjects
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://metatop/Applications/metaCall/DataObjects/DataObjects.xsd")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://metatop/Applications/metaCall/DataObjects/DataObjects.xsd", IsNullable=false)]
    public partial class ProjectDocument: object, System.ComponentModel.INotifyPropertyChanged {

        private System.Guid documentId;

        public System.Guid DocumentId
        {
            get { return documentId; }
            set { documentId = value; }
        }

        private ProjectInfo project;

        public ProjectInfo Project
        {
            get { return project; }
            set { project = value; }
        }

        private string displayName;

        public string DisplayName
        {
            get { return displayName; }
            set { displayName = value; }
        }

        private DocumentCategory category;

        public DocumentCategory Category
        {
            get { return category; }
            set { category = value; }
        }
	

        private string filename;

        public string Filename
        {
            get { return filename; }
            set { filename = value; }
        }

        private bool packetSelect;

        public bool PacketSelect
        {
            get { return packetSelect; }
            set { packetSelect = value; }
        }

        private DateTime dateCreated;

        public DateTime DateCreated
        {
            get { return dateCreated; }
            set { dateCreated = value; }
        }
	
        public event System.ComponentModel.PropertyChangedEventHandler  PropertyChanged;
    }

    public enum DocumentCategory
    {
        Faxangebot,  
        Emailvorlage,
    }

    public partial class DocumentCategoryInfo
    {
        private DocumentCategory category;

        public DocumentCategory Category
        {
            get { return category; }
            set { category = value; }
        }

        private string displayName;

        public string DisplayName
        {
            get { return displayName; }
            set { displayName = value; }
        }

        private string  description;

        public string  Description
        {
            get { return description; }
            set { description = value; }
        }
    }
}
