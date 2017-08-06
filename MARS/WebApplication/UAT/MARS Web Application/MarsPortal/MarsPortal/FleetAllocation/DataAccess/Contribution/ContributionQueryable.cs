using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.FleetAllocation.DataAccess.Contribution.Enties;
using Mars.FleetAllocation.DataContext;

namespace Mars.FleetAllocation.DataAccess.Contribution
{
    public static class ContributionQueryable
    {

        /// <summary>
        /// Returns Revenue and Holding Cost Averaged in Range
        /// </summary>
        /// <param name="dataContext"></param>
        /// <param name="minDate"></param>
        /// <returns></returns>
        public static IQueryable<ContributionEntity> GetContribution(FaoDataContext dataContext, DateTime minDate)
        {

            
            var revenueData = from rev in dataContext.RevenueByCommercialCarSegments
                              where rev.MonthDate >= minDate
                              group rev by new { rev.CarGroupId, rev.LocationId, rev.MonthDate }
                                  into groupedData
                                  //Sum Commercial Car Segment away
                                  select new
                                  {
                                      groupedData.Key.CarGroupId,
                                      groupedData.Key.LocationId,
                                      groupedData.Key.MonthDate,
                                      Revenue = groupedData.Sum(d => d.GrossRevenue)
                                  }
                                    into groupedByDate
                                    //Sum Three months to one
                                      group groupedByDate by new { groupedByDate.CarGroupId, groupedByDate.LocationId }
                                into fullyGrouped
                                select
                                    new
                                    {
                                        fullyGrouped.Key.CarGroupId,
                                        fullyGrouped.Key.LocationId,
                                        Revenue = fullyGrouped.Average(d => d.Revenue)
                                    };
            
            //
            var lifeCycleData = from ld in dataContext.LifecycleHoldingCosts
                                where ld.MonthDate >= minDate
                                group ld by ld.CarGroupId
                                    into fullyGrouped
                                    select new
                                    {
                                        CarGroupId = fullyGrouped.Key,
                                        HoldingCost = fullyGrouped.Average(d => d.Cost)
                                    };
            

            var fullRevData = from rd in revenueData
                              join ld in lifeCycleData  on rd.CarGroupId equals ld.CarGroupId
                              into jLd 
                              from joinedLd in jLd.DefaultIfEmpty()
                              select new ContributionEntity
                              {
                                  LocationId = rd.LocationId,
                                  CarGroupId = rd.CarGroupId,
                                  Revenue = rd.Revenue,
                                  HoldingCost = joinedLd == null ? 0 : joinedLd.HoldingCost,
                              };

            ////
             
            //var distinctRevCarGroups = revenueData.Select(d => d.CarGroupId);
            //var distinctHcCarGroups = lifeCycleData.Select(d => d.CarGroupId);
            //var distinctLocations = revenueData.Select(d => d.LocationId);

            //var fullCarGroupList = distinctRevCarGroups.Union(distinctHcCarGroups).Distinct();

            //var fullList = from fcg in fullCarGroupList
            //    join rd in revenueData on fcg equals rd.CarGroupId
            //    into jRd
            //    from joinedRd in jRd.DefaultIfEmpty()
            //    join hc in lifeCycleData on fcg equals hc.CarGroupId
            //    //into jHc
            //    //from joinedHc in jHc.DefaultIfEmpty()
            //    select new ContributionEntity
            //           {
            //               CarGroupId = fcg,
            //               //LocationId = joinedRd == null 
            //           };

            //var leftJoin = from rd in revenueData
            //                  join ld in lifeCycleData on rd.CarGroupId equals ld.CarGroupId
            //                  into jLd
            //                  from joinedLd in jLd.DefaultIfEmpty()
            //                  select new ContributionEntity
            //                  {
            //                      LocationId = rd.LocationId,
            //                      CarGroupId = rd.CarGroupId,
            //                      Revenue = rd.Revenue,
            //                      HoldingCost = joinedLd == null ? 0 : joinedLd.HoldingCost,
            //                  };

            

            //var rightJoin = from ld in lifeCycleData
            //               where !(from rd in revenueData
            //                      select rd.CarGroupId).Contains(ld.CarGroupId)
                                  
            //               select new ContributionEntity
            //               {
            //                   LocationId = rd.LocationId,
            //                   CarGroupId = rd.CarGroupId,
            //                   Revenue = rd.Revenue,
            //                   HoldingCost = joinedLd == null ? 0 : joinedLd.HoldingCost,
            //               };
            

            var returned = fullRevData;
            return returned;

        }
    }

}