using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data.Common;
using System.Data;
using System.Data.SqlTypes;

namespace metatop.Applications.metaCall.DataAccessLayer
{
    public static class SqlHelper
    {
        private static SqlDatabase database = DatabaseFactory.CreateDatabase() as SqlDatabase;
        private static ParameterCache paramCache= new ParameterCache();


        public static Database Database
        {
            get
            {
                if (SqlHelper.database == null)
                {
                    //string connectionString = GetConnectionString();
                    //SqlHelper.database = new SqlDatabase(connectionString);
                    SqlHelper.database = DatabaseFactory.CreateDatabase() as SqlDatabase;
                }

                return SqlHelper.database;
            }
        }

        public static ParameterCache ParameterCache
        {
            get
            {
                if (SqlHelper.paramCache == null)
                {
                    SqlHelper.paramCache = new ParameterCache();
                }

                return SqlHelper.paramCache;
            }
        }

        public static void ExecuteStoredProc(string spName, IDictionary<string, object> parameters, int timeout)
        {
            DbCommand command = database.GetStoredProcCommand(spName);
            command.CommandTimeout = timeout;
            AddParameters(command, parameters);

            try
            {
                database.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static void ExecuteStoredProc(string spName, IDictionary<string, object> parameters)
        {
            //Aufruf mit default - TimeOut  
            ExecuteStoredProc(spName, parameters, 30);
        }

        public static void ExecuteStoredProc(string spName, IDictionary<string, object> parameters, DbTransaction transaction)
        {
            DbCommand command = database.GetStoredProcCommand(spName, null);

            AddParameters(command, parameters);

            try
            {

                database.ExecuteNonQuery(command, transaction);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static DataTable ExecuteDataTable(string spName)
        {
            return ExecuteDataTable(spName, null);
        }

        public static DataTable ExecuteDataTable(string spName, IDictionary<string, object> parameters)
        {
            DbCommand command  = Database.GetStoredProcCommand(spName);

            AddParameters(command, parameters);

            try
            {
                DataSet dataSet = database.ExecuteDataSet(command);

                return dataSet.Tables[0];
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }

        public static DataTable ExecuteDataTable(string spName, IDictionary<string, object> parameters, int commandTimeOut)
        {
            DbCommand command = Database.GetStoredProcCommand(spName);
            command.CommandTimeout = commandTimeOut;

            AddParameters(command, parameters);

            try
            {
                DataSet dataSet = database.ExecuteDataSet(command);

                return dataSet.Tables[0];
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static DataSet ExecuteDataSet(string spName)
        {
            return ExecuteDataSet(spName, null);
        }

        public static DataSet ExecuteDataSet(string spName, IDictionary<string, object> parameters)
        {
            DbCommand command = Database.GetStoredProcCommand(spName);

            AddParameters(command, parameters);

            try
            {
               return  database.ExecuteDataSet(command);
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        internal static object ExecuteScalar(string spName, IDictionary<string, object> parameters)
        {
            DbCommand command = database.GetStoredProcCommand(spName);
            AddParameters(command, parameters);

            try
            {
                object result = database.ExecuteScalar(command);

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }


        }

        public static object GetEmptyByNull(object value)
        {
            /* 
             * Wenn in der Datenbank der Wert NULL steht wird hier string.empty zurückgegeben
             */

            if (value == DBNull.Value)
                return string.Empty;
            else
                return value.ToString();
        }

        public static object GetNullByNull(object value)
        {
            /*
             * Wenn in der Datenbank der Wert NULL steht wird hier 0 zurückgegeben
             */

            double retDouble = 0;

            if (value == DBNull.Value)
                return retDouble;
            else
                return value;
        }

        public static object GetNullableDBValue(object value)
        {
            // Wenn in der Datenbank der Wert NULL steht, gibt der .NET SQL
            // Server Provider DBNull.Value zurück. Da DBNull.Value jedoch
            // nicht auf andere Typen gecastet werden kann, an dieser Stelle
            // den .NET NULL Wert zurückgeben.
            if (value == DBNull.Value)
                return null;

            // Es ist kein DBNull Wert. Den Wert so zurückgeben wie er ist.
            return value;
        }

        internal static void AddParameters(DbCommand command,
            IDictionary<string, object> parameters)
        {
            paramCache.SetParameters(command, database);

            // Parameter zu dem Befehl nur hinzufügen, wenn Parameter
            // existieren.
            if (parameters != null)
            {
                // Durch alle Schlüssel laufen (es ist ein Wörterbuch).
                foreach (string key in parameters.Keys)
                {
                    // Den vorbereiteten Wert des DataFields zur DataFields
                    // Sammlung des Befehls hinzufügen.
                    object value = PrepareParameter(parameters[key]);
                   // Console.WriteLine(command.CommandText.ToString() + "--" + key + "--" + value);
                    database.SetParameterValue(command, key, value);

                }
            }
        }

        private static object PrepareParameter(object value)
        {
            // NULL durch DBNull.Value ersetzen.
            if (value == null)
                return DBNull.Value;

            //Wenn es sich um einen String handelt, Leerzeichen am Anfang und ende abschneiden
            if (value.GetType() == typeof(string))
            {
                string stringValue = (string)value;

                return stringValue.Trim();
            }

            // DateTime Wertebereiche validieren (nur beim Typ DateTime).
            if (value.GetType() == typeof(DateTime))
            {
                // Entschachteln des DateTime Wertes aus value.
                DateTime dateTime = (DateTime)value;

                // Wenn der Datumswert vor SQL Server datetime Minimum liegt,
                // SqlDateTime.MinValue zurückgeben.
                if (dateTime < SqlDateTime.MinValue.Value)
                    return SqlDateTime.MinValue;

                // Wenn der Datumswert nach SQL Server datetime Maximum liegt,
                // SqlDateTime.MaxValue zurückgeben.
                if (dateTime > SqlDateTime.MaxValue.Value)
                    return SqlDateTime.MaxValue;
            }

            // Keine Veränderung, den Wert so zurückgeben, wie er ist.
            return value;
        }

    }
}
