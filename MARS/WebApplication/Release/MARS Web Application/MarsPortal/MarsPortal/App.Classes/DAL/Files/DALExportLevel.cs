using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using App.DAL.Data;
using App.Entities;

namespace App.DAL.ExportLevel
{
    public class DalExportLevel
    {

        public List<ExportLevelSite> ExportLevelSiteGetAll()
        {
            var exportLevelsites = new List<ExportLevelSite>();

            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.ExportLevelSiteGetAll, con);
            using (con)
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var exportLevelSite = new ExportLevelSite();
                    if (reader["PKID"] != DBNull.Value)
                        exportLevelSite.ExportLevelSiteID = Convert.ToInt32(reader["PKID"]);

                    if (reader["Description"] != DBNull.Value)
                        exportLevelSite.Description = reader["Description"].ToString();
                    
                    exportLevelsites.Add(exportLevelSite);
                }
            }

            return exportLevelsites;
        }

        public List<ExportLevelFleet> ExportLevelFleetGetAll()
        {
            var exportLevelfleets = new List<ExportLevelFleet>();

            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.ExportLevelFleetGetAll, con);
            using (con)
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var exportLevelFleet= new ExportLevelFleet();
                    if (reader["PKID"] != DBNull.Value)
                        exportLevelFleet.ExportLevelFleetID = Convert.ToInt32(reader["PKID"]);

                    if (reader["description"] != DBNull.Value)
                        exportLevelFleet.Description = reader["description"].ToString();

                    exportLevelfleets.Add(exportLevelFleet);
                }
            }

            return exportLevelfleets;
        }
    }
}