using System;
using System.Data.SqlClient;
using System.Text;
using App.DAL.Data;

namespace App.DAL.ExcelExport 
{
    public class DALExportExcel {


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