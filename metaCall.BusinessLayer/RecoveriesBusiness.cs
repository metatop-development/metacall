using System;
using System.Collections.Generic;
using System.Text;


using metatop.Applications.metaCall.DataObjects;
using MaDaNet.Common.AppFrameWork.Activities;


namespace metatop.Applications.metaCall.BusinessLayer
{
    public class RecoveriesBusiness
    {

        private MetaCallBusiness metaCallBusiness;

        internal RecoveriesBusiness(MetaCallBusiness metaCallBusiness)
        {
            this.metaCallBusiness = metaCallBusiness;
        }

        #region Recoveries Summe und Detail

        public RecoveriesSum GetRecoveriesSum_GetByUser(Guid userId, DateTime from, DateTime to, int vertriebabrechnungNummer, int mode)
        {
            return metaCallBusiness.ServiceAccess.GetRecoveriesSum_GetByUser(userId, from, to, vertriebabrechnungNummer, mode);
        }

        public List<RecoveriesDetails> GetRecoveriesDetails_GetByUser(Guid userId, DateTime from, DateTime to, int vertriebabrechnungNummer, int mode)
        {
            return new List<RecoveriesDetails>(metaCallBusiness.ServiceAccess.GetRecoveriesDetails_GetByUser(userId, from, to, vertriebabrechnungNummer, mode));
        }

        public List<SalaryStatementNumbers> GetSalaryStatementNumbers_GetByUser(Guid userId)
        {
            return new List<SalaryStatementNumbers>(metaCallBusiness.ServiceAccess.GetSalaryStatementNumbers_GetByUser(userId));
        }

        #endregion
    }
}
