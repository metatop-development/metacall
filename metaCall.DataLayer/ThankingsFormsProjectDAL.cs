using System;
using System.Collections.Generic;
using System.Text;

using metatop.Applications.metaCall.DataObjects;
using Microsoft.Practices.EnterpriseLibrary.Data;

using System.Data;
using System.Data.Common;


namespace metatop.Applications.metaCall.DataAccessLayer
{
    public static class ThankingsFormsProjectDAL
    {
        #region Stored Procedures

        private const string spThankingsFormsProject_GetAll = "dbo.mwProjekt_Bedankungsformen_GetAll";
        private const string spThankingsFormsProject_GetSingle = "dbo.mwProjekt_Bedankungsformen_GetSingle";
        
        #endregion

        public static ThankingsFormsProject GetThankingFormProject(int id)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@ID", id);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spThankingsFormsProject_GetSingle, parameters);

            if (dataTable.Rows.Count < 1)
                return null;
            else
                return ConvertToThankingFormProject(dataTable.Rows[0]);

        }

        public static ThankingsFormsProject[] GetAllThankingsFormsProject(int projectNummer)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@Projektnummer", projectNummer);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spThankingsFormsProject_GetAll, parameters);

            if (dataTable.Rows.Count < 1)
                return null;
            else
                return ConvertToThankingsFormsProject(dataTable);

        }

        private static ThankingsFormsProject ConvertToThankingFormProject(DataRow row)
        {
            ThankingsFormsProject thankingFormProject = new ThankingsFormsProject();

            thankingFormProject.ID = (int)row["ID"];
            thankingFormProject.Projektnummer = (int)row["Projektnummer"];
            thankingFormProject.BedankungsformDe = (string)row["Bedankungsform_de"];
            thankingFormProject.BedankungsformFr = (string)SqlHelper.GetNullableDBValue(row["Bedankungsform_fr"]);
            thankingFormProject.BedankungsformIt = (string)SqlHelper.GetNullableDBValue(row["Bedankungsform_it"]);
            
            return thankingFormProject;
        }

        private static ThankingsFormsProject[] ConvertToThankingsFormsProject(DataTable dataTable)
        {
            ThankingsFormsProject[] thankingsFormsProject = new ThankingsFormsProject[dataTable.Rows.Count];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[i];
                thankingsFormsProject[i] = ConvertToThankingFormProject(row);
            }

            return thankingsFormsProject;
        }

    }
}
