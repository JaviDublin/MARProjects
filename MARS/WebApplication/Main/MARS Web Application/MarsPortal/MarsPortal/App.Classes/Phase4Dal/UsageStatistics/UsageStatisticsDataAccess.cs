using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.UI.WebControls;
using Mars.App.Classes.BLL.ExtensionMethods;
using Mars.App.Classes.Phase4Dal.Enumerators;
using Mars.App.Classes.Phase4Dal.UsageStatistics.Entities;

namespace Mars.App.Classes.Phase4Dal.UsageStatistics
{
    public class UsageStatisticsDataAccess : BaseDataAccess
    {

        public UsageStatisticsDataAccess(Dictionary<DictionaryParameter, string> parameters) : base (parameters, null)
        {
            
        }

        public List<PageAreaEnitity> GetMarsPageAreas()
        {
            var rootMenuId = DataContext.RibbonMenus.Single(d => d.ParentId == null).MenuId;
            var baseMenuIds = from rm in DataContext.RibbonMenus
                where rm.ParentId == rootMenuId
                        select new PageAreaEnitity
                       {
                           MenuId = rm.MenuId.HasValue ? rm.MenuId.Value : 0,
                           Description = rm.Description,
                           Selected = true
                       };
            var returned = baseMenuIds.ToList();
            return returned;
        }

        public List<UsageStatisticsDataRow> GetPageUsage(int maxNumber, IEnumerable<int> selectedMenuIds )
        {
            if(!Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.StartDate)
                || !Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.EndDate))
            {
                return null;
            }
            var fromDate = DateTime.Parse(Parameters[DictionaryParameter.StartDate]);
            var toDate = DateTime.Parse(Parameters[DictionaryParameter.EndDate]);

            var statisticsData = from sd in DataContext.StatisticsPageAccesses
                                 join rm in DataContext.RibbonMenus on "~" + sd.Url equals rm.Url
                where sd.AccessedOn >= fromDate
                      && sd.AccessedOn <= toDate
                      && selectedMenuIds.Contains(rm.ParentId.Value)
                group rm by rm.Description
                into groupedData
                orderby groupedData.Count() descending 
                select new UsageStatisticsDataRow
                       {
                           PageName = groupedData.Key,
                           PageRequests = groupedData.Count()
                       };

            var returned = statisticsData.Take(maxNumber).ToList();
            return returned;
        }
    }
}