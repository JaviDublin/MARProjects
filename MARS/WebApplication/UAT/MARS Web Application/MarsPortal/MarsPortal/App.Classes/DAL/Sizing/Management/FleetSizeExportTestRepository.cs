using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using Mars.DAL.Sizing.Management.Abstract;

namespace Mars.DAL.Sizing.Management {
    public class FleetSizeExportTestRepository : IFleetSizeExportRepository {
        static String TESTLINE="Test,Test,Test,Test,Line Number=";
        static Int32 LOOPSTART=1,LOOPCOUNT=11;
        public StringBuilder GetData(string country,int scenarioID,int locationGroupID,int carClassGroupID,string fromDate,string toDate,bool isAddition) {
            StringBuilder sb = new StringBuilder();
            for(int i=LOOPSTART;i<LOOPCOUNT;i++) sb.AppendLine(TESTLINE+i);
            return sb;
        }
        public string GetCmd(string s) {
            throw new NotImplementedException();
        }
        public void SetCmd(string descriptor,string command) {
            throw new NotImplementedException();
        }
        public bool IsUserAdmin(string UserId) {
            throw new NotImplementedException();
        }
    }
}