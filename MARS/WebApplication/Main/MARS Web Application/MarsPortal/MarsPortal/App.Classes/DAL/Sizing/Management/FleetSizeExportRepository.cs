using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.DAL.Sizing.Management.Abstract;
using System.Text;
using System.Data.SqlClient;
using App.DAL.Data;
using Mars.App.Classes.DAL.MarsDBContext;

namespace Mars.DAL.Sizing.Management {
    public class FleetSizeExportRepository : IFleetSizeExportRepository {

        readonly String ADD="Add",DELETE="Delete",COMMA=",",SPACE=" ";

        public StringBuilder GetData(string country,int scenarioID,int locationGroupID,int carClassGroupID,string fromDate,string toDate,bool isAddition) {
            var sb = new StringBuilder();
            SqlDataReader dr;
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.FleetPlanDetailExport,con);
            using(con) {
                con.Open();
                cmd.Parameters.Add(new SqlParameter("@country",country));
                cmd.Parameters.Add(new SqlParameter("@fleetPlanID",scenarioID));

                if(locationGroupID > 0)
                    cmd.Parameters.Add(new SqlParameter("@locationGroupID",locationGroupID));
                else
                    cmd.Parameters.Add(new SqlParameter("@locationGroupID",DBNull.Value));

                if(carClassGroupID > 0)
                    cmd.Parameters.Add(new SqlParameter("@carClassGroupID",carClassGroupID));
                else
                    cmd.Parameters.Add(new SqlParameter("@carClassGroupID",DBNull.Value));

                cmd.Parameters.Add(new SqlParameter("@dateFrom",DateTime.Parse(fromDate)));

                if(toDate == null)
                    cmd.Parameters.Add(new SqlParameter("@dateTo",DBNull.Value));
                else
                    cmd.Parameters.Add(new SqlParameter("@dateTo",DateTime.Parse(toDate)));

                cmd.Parameters.Add(new SqlParameter("@isAddition",isAddition));

                cmd.CommandTimeout = 999999999;
                dr = cmd.ExecuteReader();

                sb = ParseDataReaderText(dr,isAddition,country);
            }
            return sb;
        }
        StringBuilder ParseDataReaderText(SqlDataReader dr,bool isAddition,string country) {
            var sb = new StringBuilder();
            if(isAddition)
                sb.AppendLine(string.Format(GetCmd(ManualMovementCommands.addTable.ToString()),country));
            else
                sb.AppendLine(String.Format(GetCmd(ManualMovementCommands.delTable.ToString()),country));
            String ic=GetCmd(ManualMovementCommands.insertCmd.ToString());
            while(dr.Read()) {
                var templateString = ic;
                object[] args = new object[dr.FieldCount + 1];

                args[0] = isAddition ? ADD : DELETE;
                for(int col = 0;col < dr.FieldCount - 1;col++) {
                    if(!dr.IsDBNull(col))
                        args[col + 1] = dr.GetValue(col).ToString().Replace(COMMA,SPACE);
                }
                if(!dr.IsDBNull(dr.FieldCount - 1)) {
                    args[args.Length - 1] = dr.GetValue(dr.FieldCount - 1).ToString().Replace(COMMA,SPACE);
                    sb.AppendLine(string.Format(templateString,args));
                }
            }
            if(isAddition) sb.AppendLine(GetCmd(ManualMovementCommands.addCommit.ToString()));
            else sb.AppendLine(GetCmd(ManualMovementCommands.delCommit.ToString()));

            return sb;
        }
        public String GetCmd(String s) {
            using(MarsDBDataContext db = new MarsDBDataContext()) {
                return (from p in db.ManualMovementXCmds where p.Descriptor==s select p.Command).First();
            }
        }
        public void SetCmd(string descriptor,string command) {
            if(String.IsNullOrEmpty(command)) return;
            using(MarsDBDataContext db = new MarsDBDataContext()) {
                ManualMovementXCmd mmxc=(from p in db.ManualMovementXCmds where p.Descriptor==descriptor select p).First();
                mmxc.Command=command;
                db.SubmitChanges();
            }
        }
        public bool IsUserAdmin(string UserId) {
            using(MarsDBDataContext db = new MarsDBDataContext()) {
                Int32? s =(from p in db.MARS_UsersInRoles where p.userId==UserId select p.roleId).FirstOrDefault();
                if(s==null) return false;
                return s==1;
            }
        }
    }
}