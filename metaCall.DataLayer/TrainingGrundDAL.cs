using System;
using System.Collections.Generic;
using System.Text;

using metatop.Applications.metaCall.DataObjects;
using Microsoft.Practices.EnterpriseLibrary.Data;

using System.Data;
using System.Data.Common;


namespace metatop.Applications.metaCall.DataAccessLayer
{
    public static class TrainingGrundDAL
    {
        #region Stored Procedures

        private const string spTrainingGrund_GetAll = "dbo.TrainingGrund_GetAll";
        private const string spTrainingGrund_GetSingle = "dbo.TrainingGrund_GetSingle";
        
        #endregion

        public static TrainingGrund GetTrainingGrund(Guid  trainingGrundId)
        {            
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@TrainingGroundId", trainingGrundId);

            DataTable dataTable = SqlHelper.ExecuteDataTable(spTrainingGrund_GetSingle, parameters);

            if (dataTable.Rows.Count < 1)
                return null;
            else
                return ConvertToTrainingGrund(dataTable.Rows[0]);

        }

        public static TrainingGrund[] GetAllTrainingGrund()
        {
            DataTable dataTable = SqlHelper.ExecuteDataTable(spTrainingGrund_GetAll);

            if (dataTable.Rows.Count < 1)
                return null;
            else
                return ConvertToTrainingGrunds(dataTable);

        }

        private static TrainingGrund ConvertToTrainingGrund(DataRow row)
        {
            TrainingGrund trainingGrund = new TrainingGrund();

            trainingGrund.TrainingGrundId = (Guid)row["TrainingGrundId"];
            trainingGrund.TrainingGrundItem = (string)row["TrainingGrundItem"];
            
            return trainingGrund;
        }

        private static TrainingGrund[] ConvertToTrainingGrunds(DataTable dataTable)
        {
            TrainingGrund[] trainingGrunds = new TrainingGrund[dataTable.Rows.Count];

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[i];
                trainingGrunds[i] = ConvertToTrainingGrund(row);
            }

            return trainingGrunds;
        }
    }
}
