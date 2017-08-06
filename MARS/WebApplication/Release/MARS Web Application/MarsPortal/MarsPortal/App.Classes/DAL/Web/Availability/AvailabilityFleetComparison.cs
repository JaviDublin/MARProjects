using System;
using System.Data;
using System.Data.SqlClient;
using App.BLL.Workers; // added
using App.DAL.Data;

namespace App.BLL {
    public class AvailabilityFleetComparison {
        // change at line 20 if statement added to check if the car_segment_has been chosen, 
        // if it's null then use stored proc: spReportFleetComparisonCarSegment
        // change at line 212 to use CarSegmentWorker 

        public static DataTable GetFleetComparisonData(string topic, string country, int cms_pool_id, int cms_location_group_id,
                                                        int ops_region_id, int ops_area_id, string location, string fleet_name, int car_segment_id,
                                                            int car_class_id, DateTime start_date, DateTime end_date, int day_of_week, string select_by) {
            DataTable dataTable = new DataTable();
            string sql = string.Empty;

            if(car_segment_id == -1) { // the stored procedure returns all the topics so it has to be altered within this method
                sql = "spReportFleetComparisonCarSegment";
            } else if(car_class_id == -1) {

                // assign the carClass stored procedure to the sql string
                sql = "spReportFleetComparisonCarClass";
            } else {
                switch(topic) {
                    case "TOTAL FLEET":
                        sql = "spReportFleetComparisonTOTAL_FLEET";
                        break;
                    case "CARSALES":
                        sql = "spReportFleetComparisonCARSALES";
                        break;
                    case "CARHOLD L":
                        sql = "spReportFleetComparisonCARHOLD_L";
                        break;
                    case "CARHOLD H":
                        sql = "spReportFleetComparisonCARHOLD_H";
                        break;
                    case "CU":
                        sql = "spReportFleetComparisonCU";
                        break;
                    case "HA":
                        sql = "spReportFleetComparisonHA";
                        break;
                    case "HL":
                        sql = "spReportFleetComparisonHL";
                        break;
                    case "LL":
                        sql = "spReportFleetComparisonLL";
                        break;
                    case "NC":
                        sql = "spReportFleetComparisonNC";
                        break;
                    case "PL":
                        sql = "spReportFleetComparisonPL";
                        break;
                    case "TC":
                        sql = "spReportFleetComparisonTC";
                        break;
                    case "SV":
                        sql = "spReportFleetComparisonSV";
                        break;
                    case "WS (NON RAC)":
                        sql = "spReportFleetComparisonWS_NONRAC";
                        break;
                    case "OPERATIONAL FLEET":
                        sql = "spReportFleetComparisonOPERATIONAL_FLEET";
                        break;
                    case "BD":
                        sql = "spReportFleetComparisonBD";
                        break;
                    case "MM":
                        sql = "spReportFleetComparisonMM";
                        break;
                    case "TW":
                        sql = "spReportFleetComparisonTW";
                        break;
                    case "TB":
                        sql = "spReportFleetComparisonTB";
                        break;
                    case "WS (RAC)":
                        sql = "spReportFleetComparisonWS_RAC";
                        break;
                    case "FS":
                        sql = "spReportFleetComparisonFS";
                        break;
                    case "RL":
                        sql = "spReportFleetComparisonRL";
                        break;
                    case "RP":
                        sql = "spReportFleetComparisonRP";
                        break;
                    case "TN":
                        sql = "spReportFleetComparisonTN";
                        break;
                    case "AVAILABLE FLEET":
                        sql = "spReportFleetComparisonAVAILABLE_FLEET";
                        break;
                    case "RT":
                        sql = "spReportFleetComparisonRT";
                        break;
                    case "SU":
                        sql = "spReportFleetComparisonSU";
                        break;
                    case "GOLD":
                        sql = "spReportFleetComparisonGOLD";
                        break;
                    case "PREDELIVERY":
                        sql = "spReportFleetComparisonPREDELIVERY";
                        break;
                    case "OVERDUE":
                        sql = "spReportFleetComparisonOVERDUE";
                        break;
                    case "ON RENT":
                        sql = "spReportFleetComparisonON_RENT";
                        break;
                    case "UTILISATION":
                        sql = "spReportFleetComparisonUTILISATION";
                        break;
                    case "UTILISATION (incl. Overdue)":
                        sql = "spReportFleetComparisonUTILISATIONINCLOVERDUE";
                        break;
                    case "WS":
                        sql = "spReportFleetComparisonWS";
                        break;
                } // end case
            } // end if 

            SqlConnection myConnection = DBManager.CreateConnection();
            SqlCommand myCommand = DBManager.CreateProcedure(sql, myConnection);

            using(myConnection) {
                if(country == "-1") {
                    myCommand.Parameters.AddWithValue("@country", System.DBNull.Value);
                } else {
                    myCommand.Parameters.AddWithValue("@country", country);
                }

                if(cms_pool_id == -1) {
                    myCommand.Parameters.AddWithValue("@cms_pool_id", System.DBNull.Value);
                } else {
                    myCommand.Parameters.AddWithValue("@cms_pool_id", cms_pool_id);
                }

                if(cms_location_group_id == -1) {
                    myCommand.Parameters.AddWithValue("@cms_location_group_id", System.DBNull.Value);
                } else {
                    myCommand.Parameters.AddWithValue("@cms_location_group_id", cms_location_group_id);
                }

                if(ops_region_id == -1) {
                    myCommand.Parameters.AddWithValue("@ops_region_id", System.DBNull.Value);
                } else {
                    myCommand.Parameters.AddWithValue("@ops_region_id", ops_region_id);
                }

                if(ops_area_id == -1) {
                    myCommand.Parameters.AddWithValue("@ops_area_id", System.DBNull.Value);
                } else {
                    myCommand.Parameters.AddWithValue("@ops_area_id", ops_area_id);
                }

                if(location == "-1") {
                    myCommand.Parameters.AddWithValue("@location", System.DBNull.Value);
                } else {
                    myCommand.Parameters.AddWithValue("@location", location);
                }

                if(fleet_name == "-1") {
                    myCommand.Parameters.AddWithValue("@fleet_name", System.DBNull.Value);
                } else {
                    myCommand.Parameters.AddWithValue("@fleet_name", fleet_name);
                }

                if(car_segment_id == -1) {
                    myCommand.Parameters.AddWithValue("@car_segment_id", System.DBNull.Value);
                } else {
                    myCommand.Parameters.AddWithValue("@car_segment_id", car_segment_id);
                }

                if(car_class_id == -1) {
                    myCommand.Parameters.AddWithValue("@car_class_id", System.DBNull.Value);
                } else {
                    myCommand.Parameters.AddWithValue("@car_class_id", car_class_id);
                }

                start_date = start_date.Date.AddHours(0).AddMinutes(0).AddSeconds(0);
                end_date = end_date.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
                myCommand.Parameters.AddWithValue("@start_date", start_date);
                myCommand.Parameters.AddWithValue("@end_date", end_date);

                if(day_of_week == -1) {
                    myCommand.Parameters.AddWithValue("@day_of_week", System.DBNull.Value);
                } else {
                    myCommand.Parameters.AddWithValue("@day_of_week", day_of_week);
                }

                myCommand.Parameters.AddWithValue("@select_by", select_by);

                SqlDataAdapter myDataAdapter = new SqlDataAdapter();
                myDataAdapter.SelectCommand = myCommand;
                myCommand.CommandTimeout = 600; // cx'd to allow query to run longer

                try { // the fill command is volatile

                    myDataAdapter.Fill(dataTable);

                    if(car_segment_id == -1) {
                        // check for null value and change the data accordingly

                        CarWorker csw = new CarWorker(topic);
                        dataTable = csw.getDataCarSegment("car_segment", dataTable, start_date, end_date, select_by.Contains("PERCENTAGE"));
                    } else if(car_class_id == -1) {
                        // the car_class dropdownlist has been selected so process the return data appropriately

                        CarWorker ccw = new CarWorker(topic);
                        dataTable = ccw.getDataCarClass("car_class", dataTable, start_date, end_date, select_by.Contains("PERCENTAGE"));
                    }
                } catch {
                    // do nothing - an empty datatable will be returned
                }
                return dataTable;
            }
        }
    }
}