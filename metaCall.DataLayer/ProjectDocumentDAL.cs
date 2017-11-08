using System;
using System.Collections.Generic;
using System.Text;
using metatop.Applications.metaCall.DataObjects;
using System.Data;

namespace metatop.Applications.metaCall.DataAccessLayer
{
    public static class ProjectDocumentDAL
    {
        #region StoredProcedures
        private const string sp_ProjectDocument_Create = "dbo.ProjectDocument_Create";
        private const string sp_ProjectDocument_Update = "dbo.ProjectDocument_Update";
        private const string sp_ProjectDocument_Delete = "dbo.ProjectDocument_Delete";
        private const string sp_ProjectDocument_GetSingle = "dbo.ProjectDocument_GetSingle";
        private const string sp_ProjectDocument_GetListByProject = "dbo.ProjectDocument_GetListByProject";
        private const string sp_ProjectDocument_GetListByProjectAndCategory = "dbo.ProjectDocument_GetListByProjectAndCategory";
        #endregion

        #region CRUD Methods
        public static void CreateDocument(ProjectDocument document)
        {
            IDictionary<string, object> parameters = GetParameters(document);

            SqlHelper.ExecuteStoredProc(sp_ProjectDocument_Create, parameters);

        }

        public static void UpdateDocument(ProjectDocument document)
        {
            IDictionary<string, object> parameters = GetParameters(document);

            SqlHelper.ExecuteStoredProc(sp_ProjectDocument_Update, parameters);

        }

        public static void DeleteDocument(Guid documentId)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@DocumentId", documentId);

            SqlHelper.ExecuteStoredProc(sp_ProjectDocument_Delete, parameters);
        }
        #endregion

        #region SELECT's
        public static ProjectDocument GetProjectDocument(Guid documentId)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@DocumentId", documentId);

            DataTable dataTable = SqlHelper.ExecuteDataTable(sp_ProjectDocument_GetSingle, parameters);

            if (dataTable.Rows.Count < 1)
                return null;
            else
                return ConvertToProjectDocument(dataTable.Rows[0]);
        }

        public static ProjectDocument[] GetProjectDocumentsByProject(Guid projectId)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@ProjectId", projectId);

            DataTable dataTable = SqlHelper.ExecuteDataTable(sp_ProjectDocument_GetListByProject, parameters);

            return ConvertToProjectDocuments(dataTable);
        }

        public static ProjectDocument[] GetProjectDocumentsByProjectAndCategory(Guid projectId, DocumentCategory category)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@ProjectId", projectId);
            parameters.Add("@Category", (int)category);

            DataTable dataTable = SqlHelper.ExecuteDataTable(sp_ProjectDocument_GetListByProjectAndCategory, parameters);

            return ConvertToProjectDocuments(dataTable);
        }        
        #endregion

        #region helpers
        private static ProjectDocument ConvertToProjectDocument(DataRow row)
        {
            ProjectDocument document = new ProjectDocument();
            
            Guid projectId = (Guid) row["ProjectId"];

            document.DocumentId = (System.Guid)row["DocumentId"];
            document.Project = ProjectDAL.GetProjectInfo(projectId);
            document.Category = (DocumentCategory)row["Category"];
            document.DisplayName = (string)row["DisplayName"];
            document.Filename = (string)row["Filename"];
            document.PacketSelect = (bool)row["PacketSelect"];
            document.DateCreated = (DateTime)row["DateCreated"];

            return document;
        }

        private static ProjectDocument[] ConvertToProjectDocuments(DataTable dataTable)
        {
            ProjectDocument[] documents = new ProjectDocument[dataTable.Rows.Count];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[i];

                documents[i] = ConvertToProjectDocument(row);
            }

            return documents;
        }

        private static IDictionary<string, object> GetParameters(ProjectDocument document)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@DocumentId", document.DocumentId);
            parameters.Add("@ProjectId", document.Project.ProjectId);
            parameters.Add("@DisplayName", document.DisplayName);
            parameters.Add("@Category", (int)document.Category);
            parameters.Add("@Filename", document.Filename);
            parameters.Add("@PacketSelect", document.PacketSelect);
            parameters.Add("@DateCreated", document.DateCreated);

            return parameters;
        }
        #endregion

        #region DocumentCategoryInfo
        private const string spDocumentCategory_GetSingle = "dbo.ProjectCategory_GetSingle";
        private const string spDocumentCategory_GetAll = "dbo.ProjectCategory_GetAll";

        public static DocumentCategoryInfo GetDocumentCategoryInfo(DocumentCategory category)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@CategoryId", (int)category);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spDocumentCategory_GetSingle, parameters);

            if (dataTable.Rows.Count < 1)
                return null;
            else
                return ConvertToDocumentCategoryInfo(dataTable.Rows[0]);
        }

        public static DocumentCategoryInfo[] GetAllDocumentCategoryInfos()
        {
            DataTable dataTable = SqlHelper.ExecuteDataTable(spDocumentCategory_GetAll);

            return ConvertToDocumentCategoryInfos(dataTable);
        }

        private static DocumentCategoryInfo ConvertToDocumentCategoryInfo(DataRow row)
        {
            DocumentCategoryInfo info = new DocumentCategoryInfo();

            info.Category = (DocumentCategory)row["CategoryId"];
            info.DisplayName = (string)row["DisplayName"];
            info.Description = (string)row["Description"];

            return info;
        }

        private static DocumentCategoryInfo[] ConvertToDocumentCategoryInfos(DataTable dataTable)
        {
            DocumentCategoryInfo[] infos = new DocumentCategoryInfo[dataTable.Rows.Count];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[i];

                infos[i] = ConvertToDocumentCategoryInfo(row);
            }

            return infos;
        }


        
        #endregion
    }
}
