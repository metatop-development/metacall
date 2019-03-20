using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace metatop.Applications.metaCall.DataObjects
{
    public partial class CallJobInfoExtended
    {
        private string projectTermField;
        private int projektJahrField;
        private int projektMonatField;

        public string ProjectTerm
        {
            get
            {
                return this.projectTermField;
            }
            set
            {
                this.projectTermField = value;
                this.RaisePropertyChanged("projectTermField");
            }
        }

        public int ProjektJahr
        {
            get
            {
                return this.projektJahrField;
            }
            set
            {
                this.projektJahrField = value;
                this.RaisePropertyChanged("projectJahrField");
            }
        }

        public int ProjektMonat
        {
            get
            {
                return this.projektMonatField;
            }
            set
            {
                this.projektMonatField = value;
                this.RaisePropertyChanged("projectMonatField");
            }
        }
    }
}
