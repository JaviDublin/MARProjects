using System;
using System.Data.SqlClient; 
using App.DAL.Data; 

namespace App.UserControls.Details 
{
    public class CarSearchWorker {
        // Created 3/4/12 by Gavin
        // Used to retrieve and update the number of rows a user can have
        // The default is 500 and set by the database

        #region constants
        private const int ROW_DEFAULT = 500;
        #endregion

        #region Fields
        private string _racfId;
        #endregion

        #region Constructor
        public CarSearchWorker(string racfId) {
            _racfId = racfId;
        }
        #endregion

        #region Public methods
        public string getRowCount() 
        { 
            // gets the rowCount
            return useStoreProc(-1);
        }
        public string setRowCount(string sRowCount) 
        {
            // overloaded accepts string and attempts conversion
            int i; 
            if (!int.TryParse(sRowCount, out i)) return sRowCount; // Failure to convert so return
            return setRowCount(i);
        }
        public string setRowCount(int rowCount) 
        { 
            // inserts or updates database as appropriate
            return useStoreProc(rowCount);
        }
        #endregion

        #region Worker methods
            
        private string useStoreProc(int rowCount)
        { 
            // uses stored proc spPortal_RolesUserCarSearch
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure("spPortal_RolesUserCarSearch", con);
            cmd.Parameters.AddWithValue("@rac", _racfId);
            cmd.Parameters.AddWithValue("@no", rowCount);
               
            using (con) {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                    rowCount = Convert.ToInt32(reader["noRows"]);
            }
            con.Close();
            return rowCount > -1 ? rowCount.ToString() : ROW_DEFAULT.ToString(); // if no entry return default 500
            }
        #endregion
    }
}