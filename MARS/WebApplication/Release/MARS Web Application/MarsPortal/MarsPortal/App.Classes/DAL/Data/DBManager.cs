using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using App.BLL.Utilities;

namespace App.DAL.Data {

    public class DBManager {

        #region "Fields"

        private static readonly string _connectionString = ConfigurationManager.ConnectionStrings["RAD.Properties.Settings.ApplicationDataBase"].ConnectionString;

        #endregion

        #region "Methods"

        /// <summary>
        /// Initialize new database connection
        /// </summary>
        /// <returns>Initializes database connection</returns>
        public static SqlConnection CreateConnection() {
            try {
                SqlConnection con = new SqlConnection(_connectionString);
                return con;
            }
            catch {
                //Log Error
                StackTrace errorStackTrace = new StackTrace(true);
                //ApplicationLog.LogApplicationError(errorStackTrace, ex);
                return null;
            }
        }

        /// <summary>
        /// Create new stored procedure command
        /// </summary>
        /// <param name="storedProcedure">Name of stored procedure</param>
        /// <param name="con">Database connection</param>
        /// <returns>New SqlCommand - Command Type Stored Procedure</returns>
        public static SqlCommand CreateProcedure(string storedProcedure, SqlConnection con) {
            try {
                SqlCommand cmd = new SqlCommand(storedProcedure, con);
                cmd.CommandType = CommandType.StoredProcedure;
                return cmd;
            }
            catch {
                StackTrace errorStackTrace = new StackTrace(true);
                return null;
            }
        }

        /// <summary>
        /// Create a stored procedure parameter of type Nullable of Integer
        /// </summary>
        /// <param name="cmd">SQL Command</param>
        /// <param name="parameterName">SQL Parameter Name</param>
        /// <param name="parameterValue">Parameter Value (INT?)</param>
        public static void CreateParameter(SqlCommand cmd, string parameterName, int? parameterValue) {
            if (parameterValue == -1 || parameterValue == null) {
                cmd.Parameters.AddWithValue(parameterName, System.DBNull.Value);
            }
            else {
                cmd.Parameters.AddWithValue(parameterName, parameterValue);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="parameterName"></param>
        /// <param name="parameterValue"></param>
        public static void CreateParameter(SqlCommand cmd, string parameterName, int parameterValue) {
            cmd.Parameters.AddWithValue(parameterName, parameterValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="parameterName"></param>
        /// <param name="parameterValue"></param>
        public static void CreateParameter(SqlCommand cmd, string parameterName, decimal? parameterValue) {
            if (parameterValue == -1 || parameterValue == null) {
                cmd.Parameters.AddWithValue(parameterName, System.DBNull.Value);
            }
            else {
                cmd.Parameters.AddWithValue(parameterName, parameterValue);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="parameterName"></param>
        /// <param name="parameterValue"></param>
        public static void CreateParameter(SqlCommand cmd, string parameterName, decimal parameterValue) {
            cmd.Parameters.AddWithValue(parameterName, parameterValue);
        }

        /// <summary>
        /// Create a stored procedure parameter of type Nullable of Boolean
        /// </summary>
        /// <param name="cmd">SQL Command</param>
        /// <param name="parameterName">SQL Parameter Name</param>
        /// <param name="parameterValue">Parameter Value (Boolean?)</param>
        public static void CreateParameter(SqlCommand cmd, string parameterName, bool? parameterValue) {
            if (parameterValue == null) {
                cmd.Parameters.AddWithValue(parameterName, System.DBNull.Value);
            }
            else {
                cmd.Parameters.AddWithValue(parameterName, parameterValue);
            }
        }

        /// <summary>
        /// Create a stored procedure parameter of type String
        /// </summary>
        /// <param name="cmd">SQL Command</param>
        /// <param name="parameterName">SQL Parameter Name</param>
        /// <param name="parameterValue">Parameter Value (String)</param>
        public static void CreateParameter(SqlCommand cmd, string parameterName, string parameterValue) {
            if (parameterValue == string.Empty || parameterValue == null) {
                cmd.Parameters.AddWithValue(parameterName, System.DBNull.Value);
            }
            else {
                cmd.Parameters.AddWithValue(parameterName, parameterValue);
            }
        }

        /// <summary>
        /// Create a stored procedure parameter of type DateTime 
        /// with the option of including time or selecting end of day or start of day
        /// </summary>
        /// <param name="cmd">SQL Command</param>
        /// <param name="parameterName">SQL Parameter Name</param>
        /// <param name="parameterValue">Parameter Value (DateTime)</param>
        /// <param name="dateFormat">Format of Date (NoTime  = 1 | StartOfDay = 2 | EndOfDay = 3 | IncludeTime = 4)</param>
        public static void CreateParameter(SqlCommand cmd, string parameterName, DateTime parameterValue, int dateFormat) {
            DateTime value = new DateTime();

            switch (dateFormat) {
                case (int)DateFormats.NoTime:
                    value = parameterValue.Date;
                    break;
                case (int)DateFormats.StartOfDay:
                    value = parameterValue.AddHours(0).AddMinutes(0).AddSeconds(0);
                    break;
                case (int)DateFormats.EndOfDay:
                    value = parameterValue.AddHours(23).AddMinutes(59).AddSeconds(59);
                    break;
                case (int)DateFormats.IncludeTime:
                    value = parameterValue;
                    break;

            }

            cmd.Parameters.AddWithValue(parameterName, value);
        }

        /// <summary>
        /// Create a stored procedure parameter of type DateTime
        /// with the option of including startdate and end date parameters
        /// </summary>
        /// <param name="cmd">SQL Command</param>
        /// <param name="startDateParameterName">Start Date SQL Parameter Name</param>
        /// <param name="endDateParameterName">End Date SQL Parameter Name</param>
        /// <param name="startDateParameterValue">Start Date SQL Parameter Value</param>
        /// <param name="endDateParameterValue">End Date SQL Parameter Value</param>
        /// <param name="startDateFormat">Format of Date (NoTime  = 1 | StartOfDay = 2 | EndOfDay = 3 | IncludeTime = 4)</param>
        /// <param name="endDateFormat">Format of Date (NoTime  = 1 | StartOfDay = 2 | EndOfDay = 3 | IncludeTime = 4)</param>
        public static void CreateParameter(SqlCommand cmd, string startDateParameterName, string endDateParameterName,
                                            DateTime startDateParameterValue, DateTime endDateParameterValue, int startDateFormat, int endDateFormat) {
            DateTime startValue = new DateTime();
            DateTime endValue = new DateTime();

            switch (startDateFormat) {
                case (int)DateFormats.NoTime:
                    startValue = startDateParameterValue.Date;
                    break;
                case (int)DateFormats.StartOfDay:
                    startValue = startDateParameterValue.AddHours(0).AddMinutes(0).AddSeconds(0);
                    break;
                case (int)DateFormats.EndOfDay:
                    startValue = startDateParameterValue.AddHours(11).AddMinutes(59).AddSeconds(59);
                    break;
                case (int)DateFormats.IncludeTime:
                    startValue = startDateParameterValue;
                    break;
            }
            switch (endDateFormat) {
                case (int)DateFormats.NoTime:
                    endValue = endDateParameterValue.Date;
                    break;
                case (int)DateFormats.StartOfDay:
                    endValue = endDateParameterValue.AddHours(0).AddMinutes(0).AddSeconds(0);
                    break;
                case (int)DateFormats.EndOfDay:
                    endValue = endDateParameterValue.AddHours(11).AddMinutes(59).AddSeconds(59);
                    break;
                case (int)DateFormats.IncludeTime:
                    endValue = endDateParameterValue;
                    break;
            }

            cmd.Parameters.AddWithValue(startDateParameterName, startValue);
            cmd.Parameters.AddWithValue(endDateParameterName, endValue);

        }

        /// <summary>
        /// Execute a SQL Command that returns a single value
        /// </summary>
        /// <param name="con">Database Connection</param>
        /// <param name="cmd">SQL Command</param>
        /// <returns>Result from stored procedure (INT)</returns>
        public static int? ExecuteCommandScalarInt32(SqlConnection con, SqlCommand cmd) {
            //Execute Command
            int result = (int)ResultCode.Initialized;
            using (con) {
                con.Open();
                result = Convert.ToInt32(cmd.ExecuteScalar());
            }
            con.Close();
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="con"></param>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public static string ExecuteCommandScalarString(SqlConnection con, SqlCommand cmd) {
            //Execute Command
            string result = null;
            using (con) {
                con.Open();
                result = cmd.ExecuteScalar().ToString();
            }
            con.Close();
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="con"></param>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public static DateTime? ExecuteCommandScalarDateTime(SqlConnection con, SqlCommand cmd) {
            //Execute Command
            DateTime? result = null;
            using (con) {
                con.Open();
                result = Convert.ToDateTime(cmd.ExecuteScalar());
            }
            con.Close();
            return result;
        }

        /// <summary>
        /// Execute a SQL Command that does not return any values
        /// </summary>
        /// <param name="con">Database Connection</param>
        /// <param name="cmd">SQL Command</param>
        public static void ExecuteCommandNonQuery(SqlConnection con, SqlCommand cmd) {
            using (con) {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            con.Close();
        }

        #endregion

    }

}