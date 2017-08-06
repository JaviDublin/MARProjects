using System;
using System.Data.SqlClient;
using App.DAL.Data;

namespace App.BLL
{
    public class ImportDetails
    {
        #region "Enums"

        public enum ImportType : int
        {
            Availability = 1,
            Forecasting = 2,
            Sizing = 3,
            Pooling = 4,
            NonRevenue = 5
        }

        #endregion

        #region "Functions"

        /// <summary>
        /// Get the last time data was imported
        /// </summary>
        /// <param name="importTypeId"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static DateTime GetLastDataImportTime(int importTypeId)
        {


            //Initialise command
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = null;

            cmd = DBManager.CreateProcedure(StoredProcedures.GetLastImportTimeByImportType, con);
            
            //Set Parameters
            cmd.Parameters.AddWithValue("@importTypeId", importTypeId);
            //Execute Command
            DateTime result = DateTime.Now;
            using (con)
            {
                con.Open();
                result = Convert.ToDateTime(cmd.ExecuteScalar());
            }

            //close connection and return result
            con.Close();
            return result;



        }

        /// <summary>
        /// Get the next time data is due to be imported
        /// </summary>
        /// <param name="importTypeId"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static DateTime GetNextDataImportTime(int importTypeId)
        {


            //Initialise command
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = null;

            cmd = DBManager.CreateProcedure(StoredProcedures.GetNextImportTimeByImportType, con);
            
            //Set Parameters
            cmd.Parameters.AddWithValue("@importTypeId", importTypeId);
            //Execute Command
            DateTime result = DateTime.Now;
            using (con)
            {
                con.Open();
                result = Convert.ToDateTime(cmd.ExecuteScalar());
            }

            //close connection and return result
            con.Close();
            return result;



        }

        #endregion
    }
}