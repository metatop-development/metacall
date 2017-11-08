using System;
using System.Collections.Generic;
using System.Text;

using System.Data;

using metatop.Applications.metaCall.DataObjects;
using System.Xml;

namespace metatop.Applications.metaCall.DataAccessLayer
{
    public static class RecoveriesDAL
    {
        #region StoredProcedures

        private static string spmwRecoveries_SUM_GetByUser = "dbo.mwRecoveries_SUM_GetByUser";
        private static string spmwRecoveries_DETAILS_GetByUser = "dbo.mwRecoveries_DETAILS_GetByUser";
        private static string spmwSalaryStatementNumbers_GetByUser = "dbo.mwSalaryStatementNumbers_GetByUser";

        #endregion

        public static RecoveriesDetails[] GetRecoveriesDetails_GetByUser(Guid userId, DateTime from, DateTime to, int vertriebabrechnungNummer, int mode)
        {

            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@UserId", userId);
            parameters.Add("@From", from);
            parameters.Add("@To", to);
            parameters.Add("@VertriebabrechnungNummer", vertriebabrechnungNummer);
            parameters.Add("@Mode", mode);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spmwRecoveries_DETAILS_GetByUser, parameters);

            return ConvertToRecoveriesDetails(dataTable);
        }

        private static RecoveriesDetails[] ConvertToRecoveriesDetails(DataTable dataTable)
        {
            RecoveriesDetails[] recoveriesDetails = new RecoveriesDetails[dataTable.Rows.Count];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[i];
                recoveriesDetails[i] = ConvertToRecoveriesDetail(row);
            }
            
            return recoveriesDetails;
        }

        private static RecoveriesDetails ConvertToRecoveriesDetail(DataRow row)
        {

            RecoveriesDetails recoveriesDetails = new RecoveriesDetails();

            recoveriesDetails.InvoiceNumber = (int)row["InvoiceNumber"];
            recoveriesDetails.State = (string)row["State"];
            recoveriesDetails.PaymentDate = (DateTime?)SqlHelper.GetNullableDBValue(row["PaymentDate"]);
            recoveriesDetails.OrderDate = (DateTime?)SqlHelper.GetNullableDBValue(row["OrderDate"]);
            recoveriesDetails.Currency = (string)(row["Currency"]);
            recoveriesDetails.Count = (double)row["Count"];
            recoveriesDetails.Value = (double)row["Value"];
            recoveriesDetails.Paid = (double)(row["Paid"]);
            recoveriesDetails.Project = (string)(row["Project"]);
            recoveriesDetails.Sponsor = (string)(row["Sponsor"]);

            return recoveriesDetails;
        }

        public static RecoveriesSum GetRecoveriesSum_GetByUser(Guid userId, DateTime from, DateTime to, int vertriebabrechnungNummer, int mode)
        {

            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@UserId", userId);
            parameters.Add("@From", from);
            parameters.Add("@To", to);
            parameters.Add("@VertriebabrechnungNummer", vertriebabrechnungNummer);
            parameters.Add("@Mode", mode);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spmwRecoveries_SUM_GetByUser, parameters);

            return ConvertToRecoveriesSum(dataTable.Rows[0]);
        }

        private static RecoveriesSum ConvertToRecoveriesSum(DataRow row)
        {

            RecoveriesSum recoveriesSum = new RecoveriesSum();

            recoveriesSum.AmountBrutto = (double)SqlHelper.GetNullByNull(row["AmountBrutto"]);
            recoveriesSum.AmountBrutto_Canceled = (double)SqlHelper.GetNullByNull(row["AmountBrutto_Canceled"]);
            recoveriesSum.Count = (double)SqlHelper.GetNullByNull(row["Count"]);
            recoveriesSum.Count_Canceled = (double)SqlHelper.GetNullByNull(row["Count_Canceled"]);
            recoveriesSum.Paid = (double)SqlHelper.GetNullByNull(row["Paid"]);
            recoveriesSum.Paid_Candeled = (double)SqlHelper.GetNullByNull(row["Paid_Canceled"]);

            return recoveriesSum;
        }

        public static SalaryStatementNumbers[] GetSalaryStatementNumbers_GetByUser(Guid userId)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@UserId", userId);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spmwSalaryStatementNumbers_GetByUser, parameters);

            return ConvertToSalaryStatementNumbers(dataTable);
        }

        private static SalaryStatementNumbers[] ConvertToSalaryStatementNumbers(DataTable dataTable)
        {
            SalaryStatementNumbers[] salaryStatementNumbers = new SalaryStatementNumbers[dataTable.Rows.Count];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[i];
                salaryStatementNumbers[i] = ConvertToSalaryStatementNumber(row);
            }

            return salaryStatementNumbers;
        }

        private static SalaryStatementNumbers ConvertToSalaryStatementNumber(DataRow row)
        {

            SalaryStatementNumbers salaryStatementNumber = new SalaryStatementNumbers();

            salaryStatementNumber.SalaryStatementNumber = (int)row["SalaryStatementNumber"];
            salaryStatementNumber.SalaryStatementDate = (DateTime)row["SalaryStatementDate"];

            return salaryStatementNumber;
        }
    
    
    
    }
}
