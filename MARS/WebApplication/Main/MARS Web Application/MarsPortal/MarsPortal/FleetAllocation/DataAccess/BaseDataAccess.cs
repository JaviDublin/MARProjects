using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Mars.App.Classes.DAL.MarsDBContext;
using Mars.App.Classes.Phase4Dal.Enumerators;
using Mars.FleetAllocation.DataContext;

namespace Mars.FleetAllocation.DataAccess
{
    public class BaseDataAccess : IDisposable
    {
        protected FaoDataContext DataContext;
        protected Dictionary<DictionaryParameter, string> Parameters;

        public BaseDataAccess(Dictionary<DictionaryParameter, string> parameters = null, FaoDataContext existingDc = null)
        {
            
            DataContext = existingDc ?? new FaoDataContext
                          {
                            Log = new DebugTextWriter(),
                            CommandTimeout = 20000
                          };
            Parameters = parameters;
        }

        public int GetMarsUserId(string employeeId)
        {
            var marsEmployeeId = DataContext.MarsUsers.Single(d => d.EmployeeId == employeeId).MarsUserId;
            return marsEmployeeId;
        }

        public DateTime GetLastHistoryTimestamp()
        {
            var returned = DataContext.FleetHistories.Max(d => d.Timestamp);
            return returned;
        }

        public DateTime MaxCommercialDataDate()
        {
            var maxRevenueDate = DataContext.RevenueByCommercialCarSegments.Max(d => d.MonthDate);
            var maxHoldingCostDate = DataContext.LifecycleHoldingCosts.Max(d => d.MonthDate);

            return maxRevenueDate < maxHoldingCostDate ? maxRevenueDate : maxHoldingCostDate;
        }

        public DateTime MinCommercialDataDate()
        {
            var maxDate = MaxCommercialDataDate();

            var minRevenueDate = DataContext.RevenueByCommercialCarSegments.Where(d => d.MonthDate > maxDate.AddMonths(-3)).Min(d => d.MonthDate);
            var minHoldingCostDate = DataContext.LifecycleHoldingCosts.Where(d => d.MonthDate > maxDate.AddMonths(-3)).Min(d => d.MonthDate);

            return minRevenueDate < minHoldingCostDate ? minRevenueDate : minHoldingCostDate;
        }

        public List<ListItem> GetFaoCountryListItems(List<int> countryIds )
        {
            var countries = from c in DataContext.COUNTRies
                where countryIds.Contains(c.CountryId)
                select new ListItem
                       {
                           Value = c.CountryId.ToString(),
                           Text = c.country_description
                       };
            var returned = countries.ToList();
            return returned;
        }

        public void Dispose()
        {
            DataContext.Dispose();
        }
    }
}