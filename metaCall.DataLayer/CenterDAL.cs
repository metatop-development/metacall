using System;
using System.Collections.Generic;
using System.Text;

using metatop.Applications.metaCall.DataObjects;
using System.Data;
using System.Xml;

namespace metatop.Applications.metaCall.DataAccessLayer
{
    public static class CenterDAL
    {
        #region StoredProcedures
        private const string spCenter_Create = "dbo.Center_Create";
        private const string spCenter_Update = "dbo.Center_Update";
        private const string spCenter_Delete = "dbo.Center_Delete";
        private const string spCenter_GetSingle = "dbo.Center_GetSingle";
        private const string spCenter_GetAllActiveCenters = "dbo.Center_GetAllActiveCenters";
        private const string spCenter_GetCentersByCenterAdmin = "dbo.Center_GetListForUser";

        private const string spCenter_GetByMwCenterNummer = "dbo.Center_GetByMwCenterNummer";
        #endregion

        public static Center GetCenter(Guid? centerId)
        {
            if (!centerId.HasValue)
                return null;

            Center center = ObjectCache.Get<Center>(centerId.Value);

            if (center != null)
                return center;


            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@centerId", centerId);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spCenter_GetSingle, parameters);

            if (dataTable.Rows.Count < 1)
                return null;
            else
                return ConvertToCenter(dataTable.Rows[0]);
        }

        public static Center GetCenter(mwCenter mwCenter)
        {
            if (mwCenter == null)
                throw new ArgumentNullException("mwCenter");

            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@CenterNummer", mwCenter.CenterNummer);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spCenter_GetByMwCenterNummer, parameters);

            if (dataTable.Rows.Count < 1)
                return null;
            else
                return ConvertToCenter(dataTable.Rows[0]);

        }

        public static void CreateCenter(Center center)
        {
            IDictionary<string, object> parameters = GetParameters(center);
            SqlHelper.ExecuteStoredProc(spCenter_Create, parameters);
        }

        public static void CreateCenter(mwCenter mwCenter)
        {
            Center center = new Center();
            center.CenterId = Guid.NewGuid();
            center.Bezeichnung = mwCenter.Bezeichnung;
            center.mwCenter = mwCenter;

            CreateCenter(center);
        }

        public static void UpdateCenter(Center center)
        {
            IDictionary<string, object> parameters = GetParameters(center);
            SqlHelper.ExecuteStoredProc(spCenter_Update, parameters);
        }

        public static void DeleteCenter(Guid centerId)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@centerId", centerId);

            SqlHelper.ExecuteStoredProc(spCenter_Delete, parameters);

        }

        public static CenterInfo[] GetAllCenters()
        {
            DataTable dataTable = SqlHelper.ExecuteDataTable(spCenter_GetAllActiveCenters);
            return ConvertToCenterInfos(dataTable);

        }

        private static IDictionary<string, object> GetParameters(Center center)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();

            string centerAdminsXml = GetCenterAdminXml(center);
            
            parameters.Add("@centerId", center.CenterId);
            parameters.Add("@Bezeichnung", center.Bezeichnung);
            parameters.Add("@CurrentUser", null);
            parameters.Add("@CenterAdminXml", centerAdminsXml);
            parameters.Add("@mwCenterNummer", center.mwCenter == null ? null: (int?) center.mwCenter.CenterNummer);

            return parameters;
        }

        private static Center ConvertToCenter(DataRow row)
        {
            Center center = new Center();
            center.CenterId = (Guid)row["CenterId"];
            center.Bezeichnung = (string) row["Bezeichnung"];
            
            //mwCenter Abrufen und zuweisen
            int? centerNummer = (int?)SqlHelper.GetNullableDBValue(row["mwCenterNummer"]);

            if (centerNummer.HasValue)
                center.mwCenter = mwCenterDAL.GetMwCenter(centerNummer.Value);
            else
                center.mwCenter = null;

            //CenterAdministratoren abrufen
            center.Administratoren = UserDAL.GetCenterAdmins(center);

            ObjectCache.Add(center.CenterId, center, TimeSpan.FromMinutes(20));

            return center;
        }

        private static Center[] ConvertToCenters(DataTable dataTable)
        {
            Center[] centers = new Center[dataTable.Rows.Count];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[i];
                centers[i] = ConvertToCenter(row);
            }

            return centers;
        }

        private static CenterInfo[] ConvertToCenterInfos(DataTable dataTable)
        {
            CenterInfo[] infos = new CenterInfo[dataTable.Rows.Count];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[i];

                infos[i] = ConvertToCenterInfo(row);
                
            }

            return infos;

        }

        private static CenterInfo ConvertToCenterInfo(DataRow row)
        {

            CenterInfo info = new CenterInfo();
            info.CenterId = (Guid)row["CenterId"];
            info.Bezeichnung = (string)row["Bezeichnung"];
            info.mwCenterNummer = (int?)SqlHelper.GetNullableDBValue(row["mwCenterNummer"]);

            ObjectCache.Add(info.CenterId, info, TimeSpan.FromMinutes(20));

            return info;
        }

        private static string GetCenterAdminXml(Center center)
        {
            //Erstellt eine XML-Zeichenfolge der Art
            // <centerAdmins>
            //    <CenterAdmin UserId=".." />
            // </CenterAdmins>
            
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;

            using (XmlWriter writer = XmlWriter.Create(sb, settings))
            {
                writer.WriteStartElement("centerAdmins");

                foreach (UserInfo admin in center.Administratoren)
                {
                    writer.WriteStartElement("centerAdmin");
                    writer.WriteAttributeString("UserId", admin.UserId.ToString());
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
            }

            return sb.ToString();
        }

        public static CenterInfo[] GetCenters(User user)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@UserId", user.UserId);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spCenter_GetCentersByCenterAdmin, parameters);

            return ConvertToCenterInfos(dataTable);
        }


        public static CenterInfo GetCenterInfo(Guid? centerId)
        {
            if (!centerId.HasValue)
                return null;

            CenterInfo centerInfo = ObjectCache.Get<CenterInfo>(centerId.Value);

            if (centerInfo != null)
                return centerInfo;

            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@centerId", centerId);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spCenter_GetSingle, parameters);

            if (dataTable.Rows.Count < 1)
                return null;
            else
                return ConvertToCenterInfo(dataTable.Rows[0]);
        }

        public static CenterInfo GetCenterInfo(mwCenter mwCenter)
        {
            if (mwCenter == null)
                throw new ArgumentNullException("mwCenter");

            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@CenterNummer", mwCenter.CenterNummer);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spCenter_GetByMwCenterNummer, parameters);

            if (dataTable.Rows.Count < 1)
                return null;
            else
                return ConvertToCenterInfo(dataTable.Rows[0]);

        }
    }
}
