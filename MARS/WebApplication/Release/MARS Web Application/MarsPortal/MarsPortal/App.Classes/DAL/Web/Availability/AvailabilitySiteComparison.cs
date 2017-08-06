using System;
using System.Data;
using System.Data.SqlClient;
using App.DAL.Data;

namespace App.BLL
{
    public class AvailabilitySiteComparison
    {
        public static DataTable GetSiteComparisonData(string topic, int logic, string country, int cms_pool_id, int cms_location_group_id, int ops_region_id,
                                                        int ops_area_id, string fleet_name, int car_segment_id, int car_class_id,
                                                            int car_group_id, DateTime start_date, DateTime end_date, int day_of_week, string select_by)
        {

            DataTable dataTable = new DataTable();
            string sql = string.Empty;

            switch (topic)
            {
                case "TOTAL FLEET":
                    sql = "spReportSiteComparisonTOTAL_FLEET";
                    break;
                case "CARSALES":
                    sql = "spReportSiteComparisonCARSALES";
                    break;
                case "CARHOLD L":
                    sql = "spReportSiteComparisonCARHOLD_L";
                    break;
                case "CARHOLD H":
                    sql = "spReportSiteComparisonCARHOLD_H";
                    break;
                case "CU":
                    sql = "spReportSiteComparisonCU";
                    break;
                case "HA":
                    sql = "spReportSiteComparisonHA";
                    break;
                case "HL":
                    sql = "spReportSiteComparisonHL";
                    break;
                case "LL":
                    sql = "spReportSiteComparisonLL";
                    break;
                case "NC":
                    sql = "spReportSiteComparisonNC";
                    break;
                case "PL":
                    sql = "spReportSiteComparisonPL";
                    break;
                case "TC":
                    sql = "spReportSiteComparisonTC";
                    break;
                case "SV":
                    sql = "spReportSiteComparisonSV";
                    break;
                case "WS (NON RAC)":
                    sql = "spReportSiteComparisonWS_NONRAC";
                    break;
                case "OPERATIONAL FLEET":
                    sql = "spReportSiteComparisonOPERATIONAL_FLEET";
                    break;
                case "BD":
                    sql = "spReportSiteComparisonBD";
                    break;
                case "MM":
                    sql = "spReportSiteComparisonMM";
                    break;
                case "TW":
                    sql = "spReportSiteComparisonTW";
                    break;
                case "TB":
                    sql = "spReportSiteComparisonTB";
                    break;
                case "WS (RAC)":
                    sql = "spReportSiteComparisonWS_RAC";
                    break;
                case "FS":
                    sql = "spReportSiteComparisonFS";
                    break;
                case "RL":
                    sql = "spReportSiteComparisonRL";
                    break;
                case "RP":
                    sql = "spReportSiteComparisonRP";
                    break;
                case "TN":
                    sql = "spReportSiteComparisonTN";
                    break;
                case "AVAILABLE FLEET":
                    sql = "spReportSiteComparisonAVAILABLE_FLEET";
                    break;
                case "RT":
                    sql = "spReportSiteComparisonRT";
                    break;
                case "SU":
                    sql = "spReportSiteComparisonSU";
                    break;
                case "GOLD":
                    sql = "spReportSiteComparisonGOLD";
                    break;
                case "PREDELIVERY":
                    sql = "spReportSiteComparisonPREDELIVERY";
                    break;
                case "OVERDUE":
                    sql = "spReportSiteComparisonOVERDUE";
                    break;
                case "ON RENT":
                    sql = "spReportSiteComparisonON_RENT";
                    break;
                case "UTILISATION":
                    sql = "spReportSiteComparisonUTILISATION";
                    break;
                case "UTILISATION (incl. Overdue)":
                    sql = "spReportSiteComparisonUTILISATIONINCLOVERDUE";
                    break;
                case "WS":
                    sql = "spReportSiteComparisonWS";
                    break;
            }

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

                myCommand.Parameters.AddWithValue("@select_by", select_by);

                if (country == "-1" & cms_pool_id == -1 & cms_location_group_id == -1 & ops_region_id == -1 & ops_area_id == -1)
                {
                    myCommand.Parameters.AddWithValue("@grouping_criteria", "COUNTRY");
                }
                else if (cms_pool_id == -1 & cms_location_group_id == -1 & ops_region_id == -1 & ops_area_id == -1)
                {
                    if (logic == (int)ReportSettings.OptionLogic.CMS)
                    {
                        myCommand.Parameters.AddWithValue("@grouping_criteria", "CMS_POOL");
                    }
                    else
                    {
                        myCommand.Parameters.AddWithValue("@grouping_criteria", "OPS_REGION");
                    }
                }
                else if (logic == (int)ReportSettings.OptionLogic.CMS & cms_location_group_id == -1)
                {
                    myCommand.Parameters.AddWithValue("@grouping_criteria", "CMS_LOCATION_GROUP");
                }
                else if (logic == (int)ReportSettings.OptionLogic.OPS & ops_area_id == -1)
                {
                    myCommand.Parameters.AddWithValue("@grouping_criteria", "OPS_AREA");
                }
                else
                {
                    myCommand.Parameters.AddWithValue("@grouping_criteria", "LOCATION");
                }


                SqlDataAdapter myDataAdapter = new SqlDataAdapter();
                myDataAdapter.SelectCommand = myCommand;
                myDataAdapter.Fill(dataTable);

                myConnection.Close();

                return dataTable;
            }

        }



    }
}