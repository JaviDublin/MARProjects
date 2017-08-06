using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using App.DAL.Data;

namespace App.BLL
{
    public class Roles
    {

        #region Methods
        public static List<RoleDetails> SelectRoles()
        {


            //Initialise Connection
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.Portal_RolesSelectAll, con);
           

            //Execute Command
            List<RoleDetails> results = new List<RoleDetails>();
            using (con)
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    results.Add(new RoleDetails(reader));
                }
            }
            con.Close();
            return results;

        }

        public static List<RoleDescriptions> SelectRoleDescriptions()
        {


            //Initialise Connection
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.Portal_RolesSelectDescriptions, con);
            
            //Execute Command
            List<RoleDescriptions> results = new List<RoleDescriptions>();
            using (con)
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    results.Add(new RoleDescriptions(reader));
                }
            }
            con.Close();
            return results;

        }

        public static List<RoleDetails> SelectUserRoles(string racfId)
        {

            //Initialise Connection
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.Portal_RolesSelectForUser, con);
            
            //Set Parameters
            cmd.Parameters.AddWithValue("@racfId", racfId);

            //Execute Command
            List<RoleDetails> results = new List<RoleDetails>();
            using (con)
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    results.Add(new RoleDetails(reader));
                }
            }
            con.Close();
            return results;

        }

        public static int DeleteUserRoles(string racfId)
        {

            //Initialise Connection
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.Portal_UserDeleteRoles, con);
            
            //Set Parameters
            cmd.Parameters.AddWithValue("@racfId", racfId);
            using (con)
            {
                con.Open();
                cmd.ExecuteScalar();
            }
            con.Close();
            return 0;


        }

        #endregion

        #region Classes

        public class RoleDescriptions
        {
            #region Properties and Fields
            private string _roleName;

            private string _description;
            public string RoleName
            {
                get { return _roleName; }
            }
            public string Description
            {
                get { return _description; }
            }
            #endregion


            #region Constructors

            public RoleDescriptions(SqlDataReader reader)
            {
                if (reader["roleName"] != DBNull.Value)
                {
                    _roleName = Convert.ToString(reader["rolename"]);
                }
                if (reader["description"] != DBNull.Value)
                {
                    _description = Convert.ToString(reader["description"]);
                }
            }

            #endregion
        }


        public class RoleDetails
        {
            #region Properties and Fields
            private string _roleId;

            private string _roleName;
            public string RoleId
            {
                get { return _roleId; }
            }

            public string RoleName
            {
                get { return _roleName; }
            }
            #endregion


            #region Constructors
            public RoleDetails(SqlDataReader reader)
            {
                if (reader["roleId"] != DBNull.Value)
                {
                    _roleId = Convert.ToString(reader["roleId"]);
                }
                if (reader["rolename"] != DBNull.Value)
                {
                    _roleName = Convert.ToString(reader["rolename"]);
                }
            }

            #endregion
        }

        #endregion

    }
}