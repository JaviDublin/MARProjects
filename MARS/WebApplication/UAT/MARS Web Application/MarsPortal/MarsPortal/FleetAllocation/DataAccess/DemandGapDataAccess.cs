using System;
using System.Activities.Expressions;
using System.Collections.Generic;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Castle.Components.DictionaryAdapter.Xml;
using Castle.MicroKernel.Registration.Interceptor;
using Mars.App.Classes.Phase4Dal.Availability;
using Mars.App.Classes.Phase4Dal.Enumerators;
using Mars.FleetAllocation.DataAccess.Contribution;
using Mars.FleetAllocation.DataAccess.Entities;
using Mars.FleetAllocation.DataAccess.Entities.Ranking;
using Mars.FleetAllocation.DataAccess.ParameterFiltering;
using Mars.FleetAllocation.BusinessLogic;

namespace Mars.FleetAllocation.DataAccess
{
    public class DemandGapDataAccess : BaseDataAccess
    {
        public DemandGapDataAccess(Dictionary<DictionaryParameter, string> parameters = null)
            : base(parameters)
        {
            
        }

        public List<WeeklyMaxMinValues> CalculateMinMax(int minFleetScenarioId, int maxFleetFactorScenarioId, int weeksToCalculate)
        {
            var additions = from ad in DataContext.ResAdditions
                join isoWeek in DataContext.IsoWeekOfYears on ad.RepDate.Date equals isoWeek.Day.Date
                group new {ad, isoWeek}  by new {ad.CarGrpId, ad.LocId, isoWeek.WeekOfYear, isoWeek.Day.Year}
                into groupedData
                select new
                       {
                           LocationId = groupedData.Key.LocId,
                           CarGroupId = groupedData.Key.CarGrpId,
                           WeekNumber = groupedData.Key.WeekOfYear,
                           Year = groupedData.Key.Year,
                           Additions = groupedData.Sum(d=> d.ad.Value)
                       };

            var deletions = from del in DataContext.ResDeletions
                       join isoWeek in DataContext.IsoWeekOfYears on del.RepDate.Date equals isoWeek.Day.Date
                            group del by new { del.CarGrpId, del.LocId, isoWeek.WeekOfYear, isoWeek.Day.Year }
                           into groupedData
                           select new
                           {
                               LocationId = groupedData.Key.LocId,
                               CarGroupId = groupedData.Key.CarGrpId,
                               WeekNumber = groupedData.Key.WeekOfYear,
                               Year = groupedData.Key.Year,
                               Deletions = groupedData.Sum(d => d.Value)
                           };

            //How many weeks to calculate into the future
            
            var minFleetDataAccess = new MinFleetDataAccess(Parameters, DataContext);
            var minFleet = minFleetDataAccess.CalculateMinNessesaryFleetCommercialSegment(minFleetScenarioId);

            

            var maxFleetDataAccess = new MaxFleetDataAccess(Parameters, DataContext);
            var maxFleet = maxFleetDataAccess.GetMaxFleetSize(maxFleetFactorScenarioId);


            //Build Operational Fleet from the last entry in the Availability History
            //var firstCmsPredictionDate = DataContext.FleetHistories.Max(d => d.Timestamp);
            //Parameters[DictionaryParameter.StartDate] = firstCmsPredictionDate.ToShortDateString();
            var fleetHistory = FleetHistoryQueryable.GetAvailabilityHistory(DataContext, Parameters);

            //Sum FleetType Away, Leaving Date
            var sumFleet = from fh in fleetHistory
                             group fh by
                             new { fh.CarGroupId, fh.LocationId, fh.Timestamp }
                                 into gd
                                 select new
                                 {
                                     gd.Key.CarGroupId,
                                     gd.Key.LocationId,
                                     TotalFleet = gd.Sum(d => d.MaxTotal)
                                 };

            //Sum Date Away
            var totalFleet = from fh in sumFleet
                                   group fh by
                                   new {fh.CarGroupId, fh.LocationId}
                                   into gd
                                   select new
                                    {
                                        gd.Key.CarGroupId,
                                        gd.Key.LocationId,
                                        TotalFleet = gd.Average(d=> d.TotalFleet)
                                    };

           
            var maxDate = DataContext.RevenueByCommercialCarSegments.Max(d => d.MonthDate);
            var minDate = maxDate.AddMonths(-3);

            var contributionData = ContributionQueryable.GetContribution(DataContext, minDate);

         

            var demandGapData = from fh in totalFleet
                join minFs in minFleet on
                    new {fh.CarGroupId, fh.LocationId} equals new {minFs.CarGroupId, minFs.LocationId}
                join maxFs in maxFleet on
                    new {fh.CarGroupId, fh.LocationId} equals new {maxFs.CarGroupId, maxFs.LocationId}

                join adds in additions on new { fh.CarGroupId, fh.LocationId, maxFs.WeekNumber, maxFs.Year }
                    equals new {adds.CarGroupId, adds.LocationId, adds.WeekNumber, adds.Year}
                    into joinedAdds
                from joinedAdditions in joinedAdds.DefaultIfEmpty()
                join dels in deletions on new { fh.CarGroupId, fh.LocationId, maxFs.WeekNumber, maxFs.Year }
                    equals new {dels.CarGroupId, dels.LocationId, dels.WeekNumber, dels.Year}
                    into joinedDels
                from joinedDeletions in joinedDels.DefaultIfEmpty()
                join cd in contributionData on new { fh.CarGroupId, fh.LocationId } equals new { cd.CarGroupId, cd.LocationId }
                into joinedCd
                from joinedContriData in joinedCd.DefaultIfEmpty()
                join cg in DataContext.CAR_GROUPs on fh.CarGroupId equals cg.car_group_id
                join loc in DataContext.LOCATIONs on fh.LocationId equals loc.dim_Location_id
                join weekToMonth in DataContext.IsoWeekToMonths on 
                                    new { maxFs.WeekNumber, maxFs.Year }
                                equals new {WeekNumber = weekToMonth.IsoWeekNumber, Year = (int)weekToMonth.Year}
//                orderby groupedData.Key.Year, groupedData.Key.WeekNumber, loc.location1, cg.car_group1
                select new WeeklyMaxMinValues
                       {
                           Year = maxFs.Year,
                           WeekNumber = maxFs.WeekNumber,
                           MonthNumber = weekToMonth.Month,
                           CarGroupName = cg.car_group1,
                           CarGroupId = fh.CarGroupId,
                           LocationId = fh.LocationId,
                           CarSegmentId = cg.CAR_CLASS.car_segment_id,
                           LocationName = loc.location1,
                           TotalFleet = (int) fh.TotalFleet,
                           AdditionDeletionSum = (joinedAdditions == null ? 0 : joinedAdditions.Additions) 
                                                - (joinedDeletions == null ? 0 : joinedDeletions.Deletions),
                           MinFleet = ((int) minFs.MinNessesaryFleetRequired),
                           MaxFleet = ((int) maxFs.MaxForLocation),
                           ReasonForGap = string.Empty,
                           Contribution = (decimal) ((joinedContriData == null 
                                                        || joinedContriData.HoldingCost == 0
                                                        || joinedContriData.Revenue == 0 ? 0
                                            : (joinedContriData.Revenue / fh.TotalFleet) - (joinedContriData.HoldingCost))),
                           Revenue = joinedContriData == null ? 0 : joinedContriData.Revenue,
                           HoldingCost = joinedContriData == null ? 0 : joinedContriData.HoldingCost
                       };

            var returned = demandGapData.ToList();
            
            return returned;
        }

        public List<RankingOrderEntitiy> GetFinanceEntities()
        {
            var maxDate = DataContext.RevenueByCommercialCarSegments.Max(d => d.MonthDate);
            

            var minDate = maxDate.AddMonths(-3);


            var spreadData = from rev in DataContext.RevenueByCommercialCarSegments
                          join holdingCost in DataContext.LifecycleHoldingCosts on new {rev.CarGroupId, rev.MonthDate} 
                                                                                equals new {holdingCost.CarGroupId, holdingCost.MonthDate }
                          where rev.MonthDate >= minDate
                          
                          group new {rev, holdingCost } by new { rev.LocationId, rev.CarGroupId }
                              into groupedRev
                              select new
                              {
                                  groupedRev.Key.LocationId,
                                  groupedRev.Key.CarGroupId,
                                  Spread = groupedRev.Sum(d => d.rev.GrossRevenue) - groupedRev.Sum(d=> d.holdingCost.Cost)
                              };

            var totalFromHistory = from fh in DataContext.FleetHistories
                            where fh.LOCATION.COUNTRy1.CountryId == 22
                                  && fh.Timestamp >= minDate
                                  && fh.Timestamp <= maxDate
                                  && fh.CAR_GROUP.CAR_CLASS.CAR_SEGMENT.COUNTRy1.CountryId == 22
                                  && fh.FleetTypeId == 4
                            group fh by new { fh.CarGroupId, fh.LocationId }
                                into groupedFh
                                select new
                                {
                                    groupedFh.Key.LocationId,
                                    groupedFh.Key.CarGroupId,
                                    AvgTotal = groupedFh.Average(d => d.AvgTotal)
                                };

            //Any missing data gets a spread of 0
            var rankingData = from fh in totalFromHistory
                              join rd in spreadData on new { fh.CarGroupId, fh.LocationId }
                                                equals new { rd.CarGroupId, rd.LocationId }
                              into joinedRev
                              from leftJoinedRev in joinedRev.DefaultIfEmpty()
                              join l in DataContext.LOCATIONs on fh.LocationId equals l.dim_Location_id
                              join cg in DataContext.CAR_GROUPs on fh.CarGroupId equals cg.car_group_id
                              select new RankingOrderEntitiy
                              {
                                  CarGroupId = fh.CarGroupId,
                                  LocationId = fh.LocationId,
                                  CarGroup = cg.car_group1,
                                  Location = l.location1,
                                  SpreadPerUnit = leftJoinedRev == null ? 0 : leftJoinedRev.Spread / fh.AvgTotal,
                              };

            var localRanking = rankingData.ToList();
            var rankedList = localRanking.GroupBy(d => d.GetCarGroupId()).SelectMany(g => g.OrderByDescending(d => d.SpreadPerUnit).Select((x, i) =>
                                new RankingOrderEntitiy
                                {
                                    CarGroupId = x.GetCarGroupId(),
                                    LocationId = x.GetLocationId(),
                                    CarGroup = x.CarGroup,
                                    Location = x.Location,
                                    SpreadPerUnit = x.SpreadPerUnit,
                                    Rank = x.SpreadPerUnit == 0 ? 999 : i + 1
                                }));

            var returned = rankedList.ToList();
            return returned;
        }

        public List<RankingOrderEntitiy> GetRanking2()
        {
            return null;
            //var revData = from rev in DataContext.RevenueByCommercialCarSegments
            //    where rev.Year == 2014 && rev.Month == 12
            //    group rev by new {rev.LocationId, rev.CarGroupId}
            //    into groupedRev
            //    where groupedRev.Sum(d=> d.GrossRevenue) > 0
            //    select new
            //           {
            //               groupedRev.Key.LocationId, 
            //               groupedRev.Key.CarGroupId, 
            //               GrossRev = groupedRev.Sum(d=> d.GrossRevenue)
            //           };

            ////Last 3 months History
            //var totalData = from fh in DataContext.FleetHistories
            //    where fh.LOCATION.COUNTRy1.CountryId == 22
            //          && fh.Timestamp >= new DateTime(2014, 12, 01)
            //          && fh.Timestamp < new DateTime(2015, 02, 01)
            //          && fh.CAR_GROUP.CAR_CLASS.CAR_SEGMENT.COUNTRy1.CountryId == 22
            //          && fh.FleetTypeId == 4
            //          group fh by new {fh.CarGroupId, fh.LocationId}
            //            into groupedFh
            //    select new
            //           {
            //               groupedFh.Key.LocationId, 
            //               groupedFh.Key.CarGroupId, 
            //               AvgTotal = groupedFh.Average(d => d.AvgTotal)
            //           };



            //var rankingData = from fh in totalData
            //          join rd in revData on new { fh.CarGroupId, fh.LocationId}
            //                            equals new { rd.CarGroupId, rd.LocationId}
            //            into joinedRev
            //            from leftJoinedRev in joinedRev.DefaultIfEmpty()
                        
            //            //join l in DataContext.LOCATIONs on fh.LocationId equals l.dim_Location_id
            //            //join cg in DataContext.CAR_GROUPs on fh.CarGroupId equals cg.car_group_id
            //          select new RankingOrderEntitiy
            //                   {
            //                       CarGroupId = fh.CarGroupId,
            //                       LocationId = fh.LocationId,
            //                       RevPerUnit = leftJoinedRev == null ? 0 : leftJoinedRev.GrossRev / fh.AvgTotal,
            //                       SpreadPerUnit = 0,
            //                       //CarGroup = cg.car_group1,
            //                       //Location = l.location1,
            //                       Rank = 0
            //                   };

            //var localRanking = rankingData.ToList();
            //var rankedList = localRanking.GroupBy(d => d.GetCarGroupId()).SelectMany(g => g.OrderByDescending(d => d.RevPerUnit).Select((x, i) =>
            //                    new RankingOrderEntitiy
            //                    {
            //                        CarGroupId = x.GetCarGroupId(),
            //                        LocationId = x.GetLocationId(),
            //                        //CarGroup = x.CarGroup,
            //                        //Location = x.Location,
            //                        RevPerUnit = x.RevPerUnit,
            //                        Rank = i + 1
            //                    }));


            //var returned = rankedList.ToList();
            //return returned;
        }
    }
}