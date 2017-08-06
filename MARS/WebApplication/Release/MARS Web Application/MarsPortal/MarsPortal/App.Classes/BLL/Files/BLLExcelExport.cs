using System.Text;
using App.DAL.ExcelExport;
using Mars.BLL.Sizing;

namespace App.BLL.ExcelExport
{
    public class BLLExcelExport
    {
        DALExportExcel dal = new DALExportExcel();

        public StringBuilder GetFutureTrendExport(string country, int site, int fleet, int forecastType, int timezone,
        int fleetPlan, string toDate, string fromdate)
        {            
            StringBuilder sb = dal.GetFutureTrendExport(country, site, fleet, forecastType, timezone,
            fleetPlan, toDate, fromdate);
            return sb;
        }

        public StringBuilder GetSupplyAnalysisExport(string country, int site, int fleet, int forecastType, int timezone,
        int fleetPlan, string toDate, string fromdate)
        {            
            StringBuilder sb = dal.GetSupplyAnalysisExport(country, site, fleet, forecastType, timezone,
            fleetPlan, toDate, fromdate);
            return sb;
        }

        public StringBuilder GetFleetComparisonExport(string country, int fleetGroup, int cmsPoolId, 
                        int locationGroupId, int topic, string toDate, string fromDate)
        {   
            StringBuilder sb = dal.GetFleetComparisonExport(country, fleetGroup, cmsPoolId,
                        locationGroupId, topic, toDate, fromDate);
            return sb;
        }

        public StringBuilder GetSiteComparisonExport(string country, int site, int carSegmentID, 
            int carClassGroupID, int carClassID, int topic, string toDate, string fromdate)
        {

            StringBuilder sb = dal.GetSiteComparisonExport(country, site, carSegmentID, carClassGroupID, 
                carClassID, topic, toDate, fromdate);
            return sb;
        }

        public StringBuilder GetKPIExport(string country, int carSegmentID, int carClassGroupID, 
            int carClassID, int cmsPoolId, int locationGroupId, int topic, string toDate, string fromDate)
        {
            StringBuilder sb = dal.GetKPIExport(country, carSegmentID,
                        carClassGroupID, carClassID, cmsPoolId,
                        locationGroupId, topic, fromDate, toDate);
            return sb;
        }

        public StringBuilder GetForecastExport(string country, int fleet, int site, string startDate, string endDate)
        {
            StringBuilder sb = dal.GetForecastExport(country, fleet, site, startDate, endDate);
            return sb;
        }

        public StringBuilder GetBenchmarkExport(string country, int fleet, int site, int forecastType, string startDate, string endDate)
        {
            StringBuilder sb = dal.GetBenchmarkExport(country, fleet, site, forecastType, startDate, endDate);
            return sb;
        }

        public StringBuilder GetNecessaryFleetExport(string country)
        {
            StringBuilder sb = dal.GetNecessaryFleetExport(country);
            return sb;
        }
        
        public StringBuilder FleetPlanDetailExport(string country, int scenarioID, int locationGroupID, int carClassGroupID, string fromDate, string toDate, bool isAddition)
        {            
            return new FleetSizeExportLogic().GetFleetPlanExport(country,scenarioID,locationGroupID,carClassGroupID,fromDate,toDate,isAddition);
        }
    }
}