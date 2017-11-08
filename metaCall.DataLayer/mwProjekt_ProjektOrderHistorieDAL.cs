using System;
using System.Collections.Generic;
using System.Text;

using metatop.Applications.metaCall.DataObjects;

using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.Common;
using System.Xml;

namespace metatop.Applications.metaCall.DataAccessLayer
{
    public class mwProjekt_ProjektOrderHistorieDAL
    {
        #region stored Procedures

        private const string spmwProjekt_ProjektOrderHistorie_GetAll = "dbo.mwProjekt_ProjekOrderHistorie_GetAll";

        private const string spmwOrder_Historie_GetByUser = "dbo.mwOrder_Historie_GetByUser";

        #endregion

        private static mwProjekt_ProjektOrderHistorie ConvertTomwProjekt_ProjektOrderHistorie(DataRow Row)
        {
            mwProjekt_ProjektOrderHistorie mwprojekt_ProjektOrderHistorie = new mwProjekt_ProjektOrderHistorie();

            mwprojekt_ProjektOrderHistorie.AdressenPoolNummer = (int)Row["AdressenPoolNummer"];
            mwprojekt_ProjektOrderHistorie.Projektnummer = (int)Row["Projektnummer"];
            mwprojekt_ProjektOrderHistorie.Sponsor= (string)Row["Sponsor"];
            mwprojekt_ProjektOrderHistorie.Stueckzahl = (decimal)Row["Stueckzahl"];
            mwprojekt_ProjektOrderHistorie.UserOID = (Guid)Row["UserOID"];
            mwprojekt_ProjektOrderHistorie.Umsatz = (decimal)Row["Umsatz"];

            return mwprojekt_ProjektOrderHistorie;
        }

        private static mwProjekt_ProjektOrderHistorie[] ConvertTomwProjekt_ProjektOrderHistories(DataTable dataTable)
        {
            mwProjekt_ProjektOrderHistorie[] mwprojekt_ProjektOrderHistories = new mwProjekt_ProjektOrderHistorie[dataTable.Rows.Count];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[i];
                mwprojekt_ProjektOrderHistories[i] = ConvertTomwProjekt_ProjektOrderHistorie(row);
            }

            return mwprojekt_ProjektOrderHistories;
        }

        public static mwProjekt_ProjektOrderHistorie[] getAllmwProjekt_ProjektOrderHistorie(int projectNummer)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@Projektnummer", projectNummer);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spmwProjekt_ProjektOrderHistorie_GetAll, parameters);

            return ConvertTomwProjekt_ProjektOrderHistories(dataTable);
        }


        public static OrderHistory[] GetOrderHistoy_GetByUser(Guid userId, DateTime from, DateTime to)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@UserId", userId);
            parameters.Add("@From", from);
            parameters.Add("@To", to);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spmwOrder_Historie_GetByUser, parameters);

            return ConvertToOrderHistories(dataTable);

        }

        private static OrderHistory[] ConvertToOrderHistories(DataTable dataTable)
        {
            OrderHistory[] orderHistories = new OrderHistory[dataTable.Rows.Count];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[i];
                orderHistories[i] = ConvertToOrderHistory(row);
            }

            return orderHistories;
        }

        private static OrderHistory ConvertToOrderHistory(DataRow Row)
        {
            OrderHistory orderHistory = new OrderHistory();

            orderHistory.OrderNumber = (int)Row["OrderNumber"];
            orderHistory.OrderDate = (DateTime)Row["OrderDate"];
            orderHistory.OrderCount = (double)Row["OrderCount"];
            orderHistory.Umsatz = (double)Row["Umsatz"];
            orderHistory.Sponsor = (string)Row["Sponsor"];
            orderHistory.Customer = (string)Row["Customer"];
            orderHistory.OrderState = (string)Row["OrderState"];
            orderHistory.UserName = (string)Row["UserName"];
            orderHistory.SponsorOrder = (bool)Row["SponsorOrder"];

            return orderHistory;
        }


    }
}
