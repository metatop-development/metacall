using System;
using System.Collections.Generic;
using System.Text;

namespace metatop.Applications.metaCall.BusinessLayer
{
    public class DataFieldMethodParameter
    {
        private Dictionary<string, object> parameters;
        private MetaCallBusiness metacallBusiness;

        public MetaCallBusiness Business
        {
            get
            {
                return this.metacallBusiness;
            }
        }

        public DataFieldMethodParameter(MetaCallBusiness metacallBusiness)
        {
            this.parameters = new Dictionary<string, object>(
                StringComparer.InvariantCultureIgnoreCase);

            this.metacallBusiness = metacallBusiness;
        }

        public Dictionary<string, object> Parameters
        {
            get
            {
                return this.parameters;
            }
        }
    }


}
