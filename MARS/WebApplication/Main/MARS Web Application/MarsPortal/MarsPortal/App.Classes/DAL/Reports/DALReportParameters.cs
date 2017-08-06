using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using App.DAL.Data;
using App.Entities;

namespace App.DAL.ReportParameters
{
    public class DALReportParameters
    {
        public List<LocationGroup> LocationGroupGetByCountryID(string countryID)
        {
            var locationGroupList = new List<LocationGroup>();

            var con = DBManager.CreateConnection();
            var cmd = DBManager.CreateProcedure(StoredProcedures.LocationGroupGetByCountryID, con);

            cmd.Parameters.Add(Parameters.CountryID, SqlDbType.VarChar, 3);
            cmd.Parameters[Parameters.CountryID].Value = countryID;
            cmd.Parameters[Parameters.CountryID].Direction = ParameterDirection.Input;

            using (con)
            {
                con.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var locationGroup = new LocationGroup();

                    if (reader["cms_location_group"] != DBNull.Value)
                        locationGroup.LocationGroupName = reader["cms_location_group"].ToString();

                    if (reader["cms_location_group_id"] != DBNull.Value)
                        locationGroup.LocationGroupID = Convert.ToInt32(reader["cms_location_group_id"].ToString());
                    
                    locationGroupList.Add(locationGroup);
                }
            }

            return locationGroupList;
        }

        public List<Country> CountryGetAll()
        {
            var countriesList = new List<Country>();

            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.CMSCountryGetAll, con);
            using (con)
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var country = new Country();
                    if (reader["country"] != DBNull.Value)
                        country.CountryID = reader["country"].ToString();

                    if (reader["country_description"] != DBNull.Value)
                        country.CountryDescription = reader["country_description"].ToString();

                    countriesList.Add(country);
                }
            }

            return countriesList;
        }

        public List<CarGroup> CarGroupListGetByCountryID(string countryID)
        {
            var carGroupList = new List<CarGroup>();

            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.CarGroupGetByCountryID, con);

            cmd.Parameters.Add(Parameters.Country, SqlDbType.VarChar, 3);
            cmd.Parameters[Parameters.Country].Value = countryID;
            cmd.Parameters[Parameters.Country].Direction = ParameterDirection.Input;

            using (con)
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var carGroup = new CarGroup();
                    if (reader["car_group_id"] != DBNull.Value)
                        carGroup.CarGroupID = Convert.ToInt32(reader["car_group_id"]);

                    if (reader["car_group"] != DBNull.Value)
                        carGroup.CarGroupDescription = reader["car_group"].ToString();

                    carGroupList.Add(carGroup);
                }
            }

            return carGroupList;
        }

        public List<CarClass> CarClassGetByCountryID(string countryID)
        {
            var carclassList = new List<CarClass>();

            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.CarClassGetByCountryID, con);

            cmd.Parameters.Add(Parameters.Country, SqlDbType.VarChar, 3);
            cmd.Parameters[Parameters.Country].Value = countryID;
            cmd.Parameters[Parameters.Country].Direction = ParameterDirection.Input;

            using (con)
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var carClass = new CarClass();
                    if (reader["car_class_id"] != DBNull.Value)
                        carClass.CarClassID = Convert.ToInt32(reader["car_class_id"]);

                    if (reader["car_class"] != DBNull.Value)
                        carClass.CarclassDescription = reader["car_class"].ToString();

                    carclassList.Add(carClass);
                }
            }

            return carclassList;
        }
    }
}