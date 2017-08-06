using System;
using System.Data;
using System.Data.SqlClient;
using App.DAL.Data;

namespace App.BLL
{
    public class AvailabilityFleetStatus
    {

        public static DataTable GetFleetStatusData(string country, int cms_pool_id, int cms_location_group_id, int ops_region_id,
                                                    int ops_area_id, string location, string fleet_name, int car_segment_id, int car_class_id,
                                                        int car_group_id, DateTime start_date, DateTime end_date, int day_of_week, string userName)
        {

            DataTable dataTable = new DataTable();
            string sql = "spReportFleetStatus";
            SqlConnection myConnection = DBManager.CreateConnection();
            SqlCommand myCommand = DBManager.CreateProcedure(sql, myConnection);
            
            using (myConnection)
            {

                if (country == "-1")
                {
                    myCommand.Parameters.AddWithValue("@country", System.DBNull.Value);
                }
                else
                {
                    myCommand.Parameters.AddWithValue("@country", country);
                }

                if (cms_pool_id == -1)
                {
                    myCommand.Parameters.AddWithValue("@cms_pool_id", System.DBNull.Value);
                }
                else
                {
                    myCommand.Parameters.AddWithValue("@cms_pool_id", cms_pool_id);
                }

                if (cms_location_group_id == -1)
                {
                    myCommand.Parameters.AddWithValue("@cms_location_group_id", System.DBNull.Value);
                }
                else
                {
                    myCommand.Parameters.AddWithValue("@cms_location_group_id", cms_location_group_id);
                }

                if (ops_region_id == -1)
                {
                    myCommand.Parameters.AddWithValue("@ops_region_id", System.DBNull.Value);
                }
                else
                {
                    myCommand.Parameters.AddWithValue("@ops_region_id", ops_region_id);
                }

                if (ops_area_id == -1)
                {
                    myCommand.Parameters.AddWithValue("@ops_area_id", System.DBNull.Value);
                }
                else
                {
                    myCommand.Parameters.AddWithValue("@ops_area_id", ops_area_id);
                }

                if (location == "-1")
                {
                    myCommand.Parameters.AddWithValue("@location", System.DBNull.Value);
                }
                else
                {
                    myCommand.Parameters.AddWithValue("@location", location);
                }

                if (fleet_name == "-1")
                {
                    myCommand.Parameters.AddWithValue("@fleet_name", System.DBNull.Value);
                }
                else
                {
                    myCommand.Parameters.AddWithValue("@fleet_name", fleet_name);
                }

                if (car_segment_id == -1)
                {
                    myCommand.Parameters.AddWithValue("@car_segment_id", System.DBNull.Value);
                }
                else
                {
                    myCommand.Parameters.AddWithValue("@car_segment_id", car_segment_id);
                }

                if (car_class_id == -1)
                {
                    myCommand.Parameters.AddWithValue("@car_class_id", System.DBNull.Value);
                }
                else
                {
                    myCommand.Parameters.AddWithValue("@car_class_id", car_class_id);
                }

                if (car_group_id == -1)
                {
                    myCommand.Parameters.AddWithValue("@car_group_id", System.DBNull.Value);
                }
                else
                {
                    myCommand.Parameters.AddWithValue("@car_group_id", car_group_id);
                }

                start_date = start_date.Date.AddHours(0).AddMinutes(0).AddSeconds(0);
                end_date = end_date.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
                myCommand.Parameters.AddWithValue("@start_date", start_date);
                myCommand.Parameters.AddWithValue("@end_date", end_date);

                if (day_of_week == -1)
                {
                    myCommand.Parameters.AddWithValue("@day_of_week", System.DBNull.Value);
                }
                else
                {
                    myCommand.Parameters.AddWithValue("@day_of_week", day_of_week);
                }

                
                myCommand.Parameters.AddWithValue("@userName", userName);

                SqlDataAdapter myDataAdapter = new SqlDataAdapter();
                myDataAdapter.SelectCommand = myCommand;
                myDataAdapter.Fill(dataTable);

                myConnection.Close();

                return dataTable;
            }

        }

    }
}