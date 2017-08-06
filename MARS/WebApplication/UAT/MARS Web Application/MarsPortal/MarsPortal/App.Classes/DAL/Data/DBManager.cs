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

 








        #endregion

    }

}