using System.Text;
using App.DAL.ExcelExport;
using Mars.BLL.Sizing;

namespace App.BLL.ExcelExport
{
    public class BLLExcelExport
    {
        readonly DALExportExcel _dal = new DALExportExcel();





        public StringBuilder GetNecessaryFleetExport(string country)
        {
            StringBuilder sb = _dal.GetNecessaryFleetExport(country);
            return sb;
        }
        
        public StringBuilder FleetPlanDetailExport(string country, int scenarioID, int locationGroupID, int carClassGroupID, string fromDate, string toDate, bool isAddition)
        {            
            return new FleetSizeExportLogic().GetFleetPlanExport(country,scenarioID,locationGroupID,carClassGroupID,fromDate,toDate,isAddition);
        }
    }
}