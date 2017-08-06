using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Configuration;
using App.BLL.ExcelExport;
using App.DAL.ExcelExport;
using Mars.DAL.Sizing.Management.Abstract;
using Mars.DAL.Sizing.Management;

namespace Mars.BLL.Sizing {
    
    public class FleetSizeExportLogic {

        public delegate StringBuilder FleetPlanDelegate(string country,int scenarioID,int locationGroupID,int carClassGroupID,string fromDate,string toDate,bool isAddition);
        public const String WEBCONFIGNAME="SizingBugFix5",FALSE="false",TRUE="true",TEST="test",HIDE="hide";
        private IFleetSizeExportRepository _repository;

            public FleetSizeExportLogic() { }
        public FleetSizeExportLogic(IFleetSizeExportRepository r) { _repository=r; }

        public StringBuilder GetFleetPlanExport(string country,int scenarioID,int locationGroupID,int carClassGroupID,string fromDate,string toDate,bool isAddition) {
            return GetFleetPlanExport(ConfigurationManager.AppSettings[WEBCONFIGNAME],country,scenarioID,locationGroupID,carClassGroupID,fromDate,toDate,isAddition);
        }
        public StringBuilder GetFleetPlanExport(String WebConfigKey,string country,int scenarioID,int locationGroupID,int carClassGroupID,string fromDate,string toDate,bool isAddition) {
            switch(WebConfigKey){
                case FALSE:return GetFleetPlanExport(ConfigurationManager.AppSettings[WEBCONFIGNAME],country,scenarioID,locationGroupID,carClassGroupID,fromDate,toDate,isAddition,new DALExportExcel().FleetPlanDetailExport);
                case TEST: _repository=new FleetSizeExportTestRepository(); break;
                default: _repository=new FleetSizeExportRepository();break;
            }
            return GetFleetPlanExport(ConfigurationManager.AppSettings[WEBCONFIGNAME],country,scenarioID,locationGroupID,carClassGroupID,fromDate,toDate,isAddition,_repository.GetData);
        }
        public StringBuilder GetFleetPlanExport(String WebConfigKey,string country,int scenarioID,int locationGroupID,int carClassGroupID,string fromDate,string toDate,bool isAddition,FleetPlanDelegate fpd) {
            return fpd(country,scenarioID,locationGroupID,carClassGroupID,fromDate,toDate,isAddition);
        }
        public static String GetFleetSizeConfig(){return ConfigurationManager.AppSettings[WEBCONFIGNAME];}
    }
}