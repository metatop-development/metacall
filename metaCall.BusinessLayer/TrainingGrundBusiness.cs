using System;
using System.Collections.Generic;
using System.Text;

using metatop.Applications.metaCall.ServiceAccessLayer;
using metatop.Applications.metaCall.DataObjects;

namespace metatop.Applications.metaCall.BusinessLayer
{
    public class TrainingGrundBusiness
    {
        MetaCallBusiness metaCallBusiness;

        internal TrainingGrundBusiness(MetaCallBusiness metaCallBusiness)
        {
            this.metaCallBusiness = metaCallBusiness;
        }

        public List<TrainingGrund> GetAllTrainingGrund()
        {
            return new List<TrainingGrund>(metaCallBusiness.ServiceAccess.GetAllTrainingGrund());
        }

        public TrainingGrund GetTrainingGrund(Guid trainingGrundId)
        {
            return metaCallBusiness.ServiceAccess.GetTrainingGrund(trainingGrundId);
        }



    }
}
