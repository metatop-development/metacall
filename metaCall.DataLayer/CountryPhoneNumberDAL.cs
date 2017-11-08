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
    public static class CountryPhoneNumberDAL
    {
        #region stored Procedures

        private const string spCountryPhoneNumber_GetSingle = "dbo.CountryPhoneNumber_GetSingle";
        
        #endregion

        /// <summary>
        /// Konvertiert eine DataRow in ein Object CountryPhoneNumber
        /// </summary>
        /// <param name="Row"></param>
        /// <returns></returns>
        private static CountryPhoneNumber ConvertToCountryPhoneNumber(DataRow Row)
        {
            CountryPhoneNumber countryPhoneNumber = new CountryPhoneNumber();

            countryPhoneNumber.Land = (string)Row["LandKurz"];
            countryPhoneNumber.PhoneNumber = (string)Row["Vorwahl"];

            return countryPhoneNumber;
        }

        /// <summary>
        /// Konvertiert eine DataTable in ein Array aus CountryPhoneNumber-Objekten
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        private static CountryPhoneNumber[] ConvertToCountryPhoneNumbers(DataTable dataTable)
        {
            CountryPhoneNumber[] countryPhoneNumbers = new CountryPhoneNumber[dataTable.Rows.Count];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[i];
                countryPhoneNumbers[i] = ConvertToCountryPhoneNumber(row);
            }

            return countryPhoneNumbers;
        }

        /// <summary>
        /// liefert ein CountryPhoneNumber-Objekt anhand des Landes
        /// </summary>
        /// <param name="branchNumber"></param>
        /// <returns></returns>
        public static CountryPhoneNumber GetCountryPhoneNumber(string land)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@LandKurz", land);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spCountryPhoneNumber_GetSingle, parameters);

            if (dataTable.Rows.Count < 1)
                return null;
            else
                return ConvertToCountryPhoneNumber(dataTable.Rows[0]);
        }
    }
}
