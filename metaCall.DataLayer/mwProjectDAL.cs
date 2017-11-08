using System;
using System.Collections.Generic;
using System.Text;

using metatop.Applications.metaCall.DataObjects;
using Microsoft.Practices.EnterpriseLibrary.Data;

using System.Data;
using System.Data.Common;


namespace metatop.Applications.metaCall.DataAccessLayer
{
    public static class mwProjectDAL
    {

        #region Stored Procedures
        private const string spMwProjects_GetSingle = "dbo.mwProjekt_GetSingle";
        private const string spMwprojects_GetProjectsForTransfer = "dbo.mwProjekt_GetListeForTransfer";

        private const string spmwProjekt_ProjectCounts = "dbo.mwProjekt_ProjectCounts";
        private const string spmwProjekt_OrdersByProject = "dbo.mwProjekt_OrdersByProject";
        #endregion

        public static mwProject GetMwProject(int projektNummer)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@Projektnummer", projektNummer);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spMwProjects_GetSingle, parameters);

            if (dataTable.Rows.Count < 1)
                return null;
            else
                return ConvertToMwProject(dataTable.Rows[0]);

        }

        public static mwProject[] GetAllProjectsForTransfer(CenterInfo center, int statusKennung)
        {
            if (center == null)
                throw new ArgumentNullException("center");

            if (!center.mwCenterNummer.HasValue)
                throw new InvalidOperationException("center must have a valid mwCenter object");
            
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@CenterNummer", center.mwCenterNummer);
            parameters.Add("@StatusKennung", statusKennung);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spMwprojects_GetProjectsForTransfer, parameters);

            return ConvertToMwProjects(dataTable);

        }

        private static mwProject ConvertToMwProject(DataRow row)
        {
            mwProject project = new mwProject();
            project.Projektnummer = (int)row["Projektnummer"];
            project.Bezeichnung = (string)row["Bezeichnung"];
            project.BezeichnungRechnung = (string)row["BezeichnungRechnung"];
            project.ProjektMonat = ((int)row["ProjektMonat"]).ToString();
            project.ProjektJahr = ((int)row["ProjektJahr"]).ToString();
            project.MitgliederVerein = (int?)SqlHelper.GetNullableDBValue(row["Mitglieder_Verein"]);
            project.MitgliederAbteilung = (int?)SqlHelper.GetNullableDBValue(row["Mitglieder_Abteilung"]);
            project.AnzahlProjekte = (int?)SqlHelper.GetNullableDBValue(row["AnzahlProjekte"]);
            project.Language = (Language)row["Sprachennummer"];

            project.mwVerein = mwVereinDAL.GetVerein((int)row["Vereinsnummer"]);

            return project;
        }

        private static mwProject[] ConvertToMwProjects(DataTable dataTable)
        {
            mwProject[] projects = new mwProject[dataTable.Rows.Count];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[i];
                projects[i] = ConvertToMwProject(row);
            }

            return projects;
        }

        #region Projektstatistik

        public static ProjectCounts[] GetAllProjectCounts(Guid? userId, Guid projectId)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@UserId", userId);
            parameters.Add("@ProjectId", projectId);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spmwProjekt_ProjectCounts, parameters);

            return ConvertToProjectCounts(dataTable);
        }

        public static ProjectCounts[] GetAllProjectCounts(Guid userId)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@UserId", userId);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spmwProjekt_ProjectCounts, parameters);

            return ConvertToProjectCounts(dataTable);

        }

        private static ProjectCounts[] ConvertToProjectCounts(DataTable dataTable)
        {
            ProjectCounts[] projectCounts = new ProjectCounts[dataTable.Rows.Count];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[i];
                projectCounts[i] = ConvertToProjectCount(row);
            }

            return projectCounts;
        }

        private static ProjectCounts ConvertToProjectCount(DataRow row)
        {
            ProjectCounts projectCount = new ProjectCounts();

            projectCount.Projektnummer = (int)row["Projektnummer"];
            projectCount.Bezeichnung = (string)row["Bezeichnung"];
            projectCount.Projektjahr = (string)row["Projektjahr"];
            projectCount.Count = (double)row["Count"];

            return projectCount;
        }

        public static ProjectOrders[] GetAllProjectOrders(Guid userId, Guid projectId)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@UserId", userId);
            parameters.Add("@ProjectId", projectId);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spmwProjekt_OrdersByProject, parameters);

            return ConvertToProjectOrders(dataTable);

        }

        private static ProjectOrders[] ConvertToProjectOrders(DataTable dataTable)
        {
            ProjectOrders[] projectOrders = new ProjectOrders[dataTable.Rows.Count];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[i];
                projectOrders[i] = ConvertToProjectOrder(row);
            }

            return projectOrders;
        }

        private static ProjectOrders ConvertToProjectOrder(DataRow row)
        {
            ProjectOrders projectOrder = new ProjectOrders();

            projectOrder.OrderNumber = (int)row["OrderNumber"];
            projectOrder.OrderDate = (DateTime)row["OrderDate"];
            projectOrder.PaymentTarget = (DateTime?)SqlHelper.GetNullableDBValue(row["PaymentTarget"]);
            projectOrder.Sponsor = (string)row["Sponsor"];
            projectOrder.Count = (double)row["Count"];
            projectOrder.State = (string)row["State"];

            return projectOrder;
        }


        #endregion
    }
}
