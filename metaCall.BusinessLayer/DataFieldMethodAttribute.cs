using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace metatop.Applications.metaCall.BusinessLayer
{
    [global::System.AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class DataFieldMethodAttribute : Attribute
    {
        // See the attribute guidelines at 
        //  http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconusingattributeclasses.asp
        private readonly string dataField;
        private readonly DataFieldMethodCategory category;
        private string description;

        public DataFieldMethodAttribute(string dataField, DataFieldMethodCategory category)
        {
            this.dataField = dataField;
            this.category = category;
        }

        /// <summary>
        /// Name des Datenfelds wie er bei der Abfrage angegeben werden muss
        /// </summary>
        public string DataField
        {
            get
            {
                return this.dataField;
            }
        }

        /// <summary>
        /// Kategorie dem diese Variable zugeordnet ist.
        /// </summary>
        public DataFieldMethodCategory Category
        {
            get
            {
                return this.category;
            }
        }

        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = value;
            }
        }

    }

    public enum DataFieldMethodCategory
    {
        CurrentUser,
        Project, 
        Customer, 
        Sponsor,
        ArrayList
    }

}
