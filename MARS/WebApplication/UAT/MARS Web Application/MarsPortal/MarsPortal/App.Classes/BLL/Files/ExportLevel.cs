using System.Collections.Generic;
using App.DAL.ExportLevel;
using App.Entities;

namespace App.BLL.ExportLevel
{
    public class BLLExportLevel
    {
        DalExportLevel dal = new DalExportLevel();

        public List<ExportLevelSite> GetAllExportLevelSite()
        {
            List<ExportLevelSite> siteExportLevels = dal.ExportLevelSiteGetAll();
            return siteExportLevels;
        }

        public List<ExportLevelFleet> GetAllExportLevelFleet()
        {
            List<ExportLevelFleet> fleetExportLevels = dal.ExportLevelFleetGetAll();
            return fleetExportLevels;
        }
    }
}