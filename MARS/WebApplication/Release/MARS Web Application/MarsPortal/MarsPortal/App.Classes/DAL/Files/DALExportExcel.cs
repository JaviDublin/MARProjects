using System;
using System.Data.SqlClient;
using System.Text;
using App.DAL.Data;

namespace App.DAL.ExcelExport 
{
    public class DALExportExcel {
        public StringBuilder GetFutureTrendExport(string country, int site, int fleet, int forecastType, int timezone,
        int fleetPlan, string toDate, string fromdate) {
            var sb = new StringBuilder();
            SqlDataReader dr;
            try {
                SqlConnection con = DBManager.CreateConnection();
                SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.GetFutureTrendExportData, con);
                using (con) {
                    con.Open();
                    cmd.Parameters.Add(new SqlParameter("@country", country));
                    cmd.Parameters.Add(new SqlParameter("@fleet", fleet));
                    cmd.Parameters.Add(new SqlParameter("@site", site));
                    cmd.Parameters.Add(new SqlParameter("@forecastType", forecastType));
                    cmd.Parameters.Add(new SqlParameter("@timezone", timezone));
                    cmd.Parameters.Add(new SqlParameter("@fleetPlan", fleetPlan));
                    cmd.Parameters.Add(new SqlParameter("@startDate", DateTime.Parse(fromdate)));
                    cmd.Parameters.Add(new SqlParameter("@endDate", DateTime.Parse(toDate)));
                    cmd.CommandTimeout = 999999999;
                    dr = cmd.ExecuteReader();

                    sb = ParseDataReader(dr);
                }

            }
            catch {
            }
            return sb;
        }

        public StringBuilder GetSupplyAnalysisExport(string country, int site, int fleet, int forecastType, int timezone,
        int fleetPlan, string toDate, string fromdate) {
            var sb = new StringBuilder();
            SqlDataReader dr;
            try {
                SqlConnection con = DBManager.CreateConnection();
                SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.GetSupplyAnalysisExportData, con);
                using (con) {
                    con.Open();
                    cmd.Parameters.Add(new SqlParameter("@country", country));
                    cmd.Parameters.Add(new SqlParameter("@fleet", fleet));
                    cmd.Parameters.Add(new SqlParameter("@site", site));
                    cmd.Parameters.Add(new SqlParameter("@forecastType", forecastType));
                    cmd.Parameters.Add(new SqlParameter("@timezone", timezone));
                    cmd.Parameters.Add(new SqlParameter("@fleetPlan", fleetPlan));
                    cmd.Parameters.Add(new SqlParameter("@startDate", DateTime.Parse(fromdate)));
                    cmd.Parameters.Add(new SqlParameter("@endDate", DateTime.Parse(toDate)));
                    cmd.CommandTimeout = 999999999;
                    dr = cmd.ExecuteReader();

                    sb = ParseDataReader(dr);
                }

            }
            catch {
            }
            return sb;
        }

        public StringBuilder GetFleetComparisonExport(string country, int fleet, int cmsPoolId,
                        int locationGroupId, int topic, string toDate, string fromdate) {
            var sb = new StringBuilder();
            SqlDataReader dr;
            try {
                SqlConnection con = DBManager.CreateConnection();
                SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.GetFleetComparisonExportData, con);
                using (con) {
                    con.Open();
                    cmd.Parameters.Add(new SqlParameter("@country", country));
                    cmd.Parameters.Add(new SqlParameter("@fleet", fleet));
                    cmd.Parameters.Add(new SqlParameter("@pool", cmsPoolId));
                    cmd.Parameters.Add(new SqlParameter("@locationGroup", locationGroupId));
                    cmd.Parameters.Add(new SqlParameter("@topic", topic));
                    cmd.Parameters.Add(new SqlParameter("@startDate", DateTime.Parse(fromdate)));
                    cmd.Parameters.Add(new SqlParameter("@endDate", DateTime.Parse(toDate)));
                    cmd.CommandTimeout = 999999999;
                    dr = cmd.ExecuteReader();

                    sb = ParseDataReader(dr);
                }

            }
            catch {

            }
            return sb;
        }

        public StringBuilder GetSiteComparisonExport(string country, int site, int carSegmentID, int carClassGroupID,
            int carClassID, int topic, string toDate, string fromdate) {
            var sb = new StringBuilder();
            SqlDataReader dr;
            try {
                SqlConnection con = DBManager.CreateConnection();
                SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.GetSiteComparisonExportData, con);
                using (con) {
                    con.Open();
                    cmd.Parameters.Add(new SqlParameter("@country", country));
                    cmd.Parameters.Add(new SqlParameter("@site", site));

                    cmd.Parameters.Add(new SqlParameter("@CarSegment", carSegmentID));
                    cmd.Parameters.Add(new SqlParameter("@CarClassGroup", carClassGroupID));
                    cmd.Parameters.Add(new SqlParameter("@CarClass", carClassID));

                    cmd.Parameters.Add(new SqlParameter("@topic", topic));
                    cmd.Parameters.Add(new SqlParameter("@startDate", DateTime.Parse(fromdate)));
                    cmd.Parameters.Add(new SqlParameter("@endDate", DateTime.Parse(toDate)));
                    cmd.CommandTimeout = 999999999;
                    dr = cmd.ExecuteReader();

                    sb = ParseDataReader(dr);
                }

            }
            catch {

            }
            return sb;
        }

        public StringBuilder GetKPIExport(string country, int carSegmentID, int carClassGroupID,
            int carClassID, int cmsPoolId, int locationGroupId, int topic, string fromDate, string toDate) {
            var sb = new StringBuilder();
            SqlDataReader dr;
            try {
                SqlConnection con = DBManager.CreateConnection();
                SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.GetKPIExportData, con);
                using (con) {
                    con.Open();
                    cmd.Parameters.Add(new SqlParameter("@country", country));

                    cmd.Parameters.Add(new SqlParameter("@CarSegment", carSegmentID));
                    cmd.Parameters.Add(new SqlParameter("@CarClassGroup", carClassGroupID));
                    cmd.Parameters.Add(new SqlParameter("@CarClass", carClassID));

                    cmd.Parameters.Add(new SqlParameter("@pool", cmsPoolId));
                    cmd.Parameters.Add(new SqlParameter("@locationGroup", locationGroupId));

                    cmd.Parameters.Add(new SqlParameter("@topic", topic));

                    cmd.Parameters.Add(new SqlParameter("@startDate", DateTime.Parse(fromDate)));
                    cmd.Parameters.Add(new SqlParameter("@endDate", DateTime.Parse(toDate)));
                    cmd.CommandTimeout = 999999999;
                    dr = cmd.ExecuteReader();

                    sb = ParseDataReader(dr);
                }

            }
            catch {

            }
            return sb;
        }

        public StringBuilder GetNecessaryFleetExport(string country) {
            var sb = new StringBuilder();
            SqlDataReader dr;
            try {
                SqlConnection con = DBManager.CreateConnection();
                SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.GetNecessaryFleetExportData, con);
                using (con) {
                    con.Open();
                    cmd.Parameters.Add(new SqlParameter("@country", country));
                    cmd.CommandTimeout = 999999999;
                    dr = cmd.ExecuteReader();

                    sb = ParseDataReader(dr);
                }

            }
            catch {

            }
            return sb;
        }

        public StringBuilder GetBenchmarkExport(string country, int fleet, int site, int forecastType, string startDate, string endDate) {
            var sb = new StringBuilder();
            SqlDataReader dr;
            try {
                SqlConnection con = DBManager.CreateConnection();
                SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.GetBenchmarkExportData, con);
                using (con) {
                    con.Open();
                    cmd.Parameters.Add(new SqlParameter("@country", country));
                    cmd.Parameters.Add(new SqlParameter("@fleet", fleet));
                    cmd.Parameters.Add(new SqlParameter("@site", site));
                    cmd.Parameters.Add(new SqlParameter("@forecastType", forecastType));
                    cmd.Parameters.Add(new SqlParameter("@startDate", DateTime.Parse(startDate)));
                    cmd.Parameters.Add(new SqlParameter("@endDate", DateTime.Parse(endDate)));
                    cmd.CommandTimeout = 999999999;
                    dr = cmd.ExecuteReader();

                    sb = ParseDataReader(dr);
                }

            }
            catch {

            }
            return sb;
        }

        public StringBuilder GetForecastExport(string country, int fleet, int site, string startDate, string endDate) {
            var sb = new StringBuilder();
            SqlDataReader dr;
            try {
                SqlConnection con = DBManager.CreateConnection();
                SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.GetForecastExportData, con);
                using (con) {
                    con.Open();
                    cmd.Parameters.Add(new SqlParameter("@country", country));
                    cmd.Parameters.Add(new SqlParameter("@fleet", fleet));
                    cmd.Parameters.Add(new SqlParameter("@site", site));
                    cmd.Parameters.Add(new SqlParameter("@startDate", DateTime.Parse(startDate)));
                    cmd.Parameters.Add(new SqlParameter("@endDate", DateTime.Parse(endDate)));
                    cmd.CommandTimeout = 999999999;
                    dr = cmd.ExecuteReader();

                    sb = ParseDataReader(dr);
                }

            }
            catch {

            }
            return sb;
        }

        public StringBuilder FleetPlanDetailExport(string country, int scenarioID, int locationGroupID, int carClassGroupID, string fromDate, string toDate, bool isAddition) {
            var sb = new StringBuilder();
            SqlDataReader dr;
            try {
                SqlConnection con = DBManager.CreateConnection();
                SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.FleetPlanDetailExport, con);
                using (con) {
                    con.Open();
                    cmd.Parameters.Add(new SqlParameter("@country", country));
                    cmd.Parameters.Add(new SqlParameter("@fleetPlanID", scenarioID));

                    if (locationGroupID > 0)
                        cmd.Parameters.Add(new SqlParameter("@locationGroupID", locationGroupID));
                    else
                        cmd.Parameters.Add(new SqlParameter("@locationGroupID", DBNull.Value));

                    if (carClassGroupID > 0)
                        cmd.Parameters.Add(new SqlParameter("@carClassGroupID", carClassGroupID));
                    else
                        cmd.Parameters.Add(new SqlParameter("@carClassGroupID", DBNull.Value));

                    cmd.Parameters.Add(new SqlParameter("@dateFrom", DateTime.Parse(fromDate)));

                    if (toDate == null)
                        cmd.Parameters.Add(new SqlParameter("@dateTo", DBNull.Value));
                    else
                        cmd.Parameters.Add(new SqlParameter("@dateTo", DateTime.Parse(toDate)));

                    cmd.Parameters.Add(new SqlParameter("@isAddition", isAddition));

                    cmd.CommandTimeout = 999999999;
                    dr = cmd.ExecuteReader();

                    sb = ParseDataReaderText(dr, isAddition, country);
                }

            }
            catch {
            }
            return sb;
        }

        private StringBuilder ParseDataReader(SqlDataReader dr) {
            var sb = new StringBuilder();
            //Add Header          
            for (int count = 0; count < dr.FieldCount; count++) {
                if (dr.GetName(count) != null)
                    sb.Append(dr.GetName(count));
                if (count < dr.FieldCount - 1) {
                    sb.Append(",");
                }
            }
            sb.AppendLine("");

            //Append Data
            while (dr.Read()) {
                for (int col = 0; col < dr.FieldCount - 1; col++) {
                    if (!dr.IsDBNull(col))
                        sb.Append(dr.GetValue(col).ToString().Replace(",", " "));
                    sb.Append(",");
                }
                if (!dr.IsDBNull(dr.FieldCount - 1)) {
                    sb.Append(dr.GetValue(
                        dr.FieldCount - 1).ToString().Replace(",", " "));
                    sb.AppendLine("");
                }
            }

            return sb;
        }

        private StringBuilder ParseDataReaderText(SqlDataReader dr, bool isAddition, string country) {
            var sb = new StringBuilder();
            //Add Header

            if (isAddition)
                sb.Append(string.Format("delete from cm_fleet_add_detail where location_code like '{0}%';", country));
            else
                sb.Append("Insert Statement");


            sb.AppendLine("");

            //Append Data
            while (dr.Read()) {
                var templateString = "insert into cm_fleet_{0}_detail (LOCATION_CODE, CAR_CLASS_CODE, {0}_DATE, {0}_COUNT) values ('{1}', '{2}', to_date('{3}', 'MM/DD/YYYY'), {4});";
                object[] args = new object[dr.FieldCount + 1];

                args[0] = isAddition ? "Add" : "Delete";
                for (int col = 0; col < dr.FieldCount - 1; col++) {
                    if (!dr.IsDBNull(col))
                        args[col + 1] = dr.GetValue(col).ToString().Replace(",", " ");
                }
                if (!dr.IsDBNull(dr.FieldCount - 1)) {
                    args[args.Length - 1] = dr.GetValue(dr.FieldCount - 1).ToString().Replace(",", " ");
                    sb.Append(string.Format(templateString, args));
                    sb.AppendLine("");
                }
            }

            if (isAddition)
                sb.AppendLine("Commit;");

            return sb;
        }
    }
}