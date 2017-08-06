using System;
using System.Collections.Generic;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Mars.App.Classes.Phase4Dal.Availability;
using Mars.App.Classes.Phase4Dal.Enumerators;
using Mars.FleetAllocation.DataAccess.Entities;
using Mars.FleetAllocation.DataAccess.ParameterFiltering;

namespace Mars.FleetAllocation.DataAccess
{
    public class DemandGapDataAccess : BaseDataAccess
    {
        public DemandGapDataAccess(Dictionary<DictionaryParameter, string> parameters = null)
            : base(parameters)
        {
            
        }

        public IQueryable<DemandGapOneRow> GetDemandGapStepOne()
        {
            var fleetHistory = FleetHistoryQueryable.GetAvailabilityHistory(DataContext, Parameters);

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

            var demandGapData = from fh in fleetHistory
                join minFs in DataContext.MinNessesaryFleets on
                    new {fh.CarGroupId, fh.LocationId} equals new {minFs.CarGroupId, minFs.LocationId}
                join maxFs in DataContext.MaxFleetSizes on
                    new {fh.CarGroupId, fh.LocationId} equals new {maxFs.CarGroupId, maxFs.LocationId}
                join adds in additions on new {fh.CarGroupId, fh.LocationId, maxFs.WeekNumber}
                    equals new {adds.CarGroupId, adds.LocationId, adds.WeekNumber}
                    into joinedAdds
                from joinedAdditions in joinedAdds.DefaultIfEmpty()
                join dels in deletions on new {fh.CarGroupId, fh.LocationId, maxFs.WeekNumber}
                    equals new {dels.CarGroupId, dels.LocationId, dels.WeekNumber}
                    into joinedDels
                from joinedDeletions in joinedDels.DefaultIfEmpty()

                group new {fh, minFs, maxFs, joinedAdditions, joinedDeletions} by
                    new {fh.CarGroupId, fh.LocationId, maxFs.WeekNumber, maxFs.PeakDay.Year}
                into groupedData
                join cg in DataContext.CAR_GROUPs on groupedData.Key.CarGroupId equals cg.car_group_id
                join loc in DataContext.LOCATIONs on groupedData.Key.LocationId equals loc.dim_Location_id
                join weekToMonth in DataContext.IsoWeekToMonths on 
                                    new { groupedData.Key.WeekNumber, groupedData.Key.Year }
                                equals new {WeekNumber = weekToMonth.IsoWeekNumber, Year = (int)weekToMonth.Year}
                orderby groupedData.Key.Year, groupedData.Key.WeekNumber, loc.location1, cg.car_group1
                select new DemandGapOneRow
                       {
                           Year = groupedData.Key.Year,
                           WeekNumber = groupedData.Key.WeekNumber,
                           MonthNumber = weekToMonth.Month,
                           CarGroupName = cg.car_group1,
                           CarGroupId = groupedData.Key.CarGroupId,
                           LocationId = groupedData.Key.LocationId,
                           CarSegmentId = cg.CAR_CLASS.car_segment_id,
                           LocationName = loc.location1,
                           OperationalFleet = groupedData.Sum(d => d.fh.PeakOperationalFleet),
                           AdditionDeletionSum = groupedData.Sum(d => d.joinedAdditions == null ? 0 : d.joinedAdditions.Additions) - groupedData.Sum(d => d.joinedDeletions == null ? 0 : d.joinedDeletions.Deletions),
                           MinFleet = ((int) groupedData.Sum(d => d.minFs.MinFleet)),
                           ReasonForGap = string.Empty
                                                    
                       };

            var returned = demandGapData;
            //var xx = returned.Take(10).ToList();
            return returned;
        }




        public IQueryable<DemandGapVisualization> GetDemandGapStepOneViz()
        {
            var fleetHistory = FleetHistoryQueryable.GetAvailabilityHistory(DataContext, Parameters);

            var additions = from ad in DataContext.ResAdditions
                            join isoWeek in DataContext.IsoWeekOfYears on ad.RepDate.Date equals isoWeek.Day.Date
                            group new { ad, isoWeek } by new { ad.CarGrpId, ad.LocId, isoWeek.WeekOfYear }
                                into groupedData
                                select new
                                {
                                    LocationId = groupedData.Key.LocId,
                                    CarGroupId = groupedData.Key.CarGrpId,
                                    WeekNumber = groupedData.Key.WeekOfYear,
                                    Additions = groupedData.Sum(d => d.ad.Value)
                                };

            var deletions = from del in DataContext.ResDeletions
                            join isoWeek in DataContext.IsoWeekOfYears on del.RepDate.Date equals isoWeek.Day.Date
                            group del by new { del.CarGrpId, del.LocId, isoWeek.WeekOfYear }
                                into groupedData
                                select new
                                {
                                    LocationId = groupedData.Key.LocId,
                                    CarGroupId = groupedData.Key.CarGrpId,
                                    WeekNumber = groupedData.Key.WeekOfYear,
                                    Deletions = groupedData.Sum(d => d.Value)
                                };

            var demandGapData = from fh in fleetHistory
                                join minFs in DataContext.MinNessesaryFleets on
                                    new { fh.CarGroupId, fh.LocationId } equals new { minFs.CarGroupId, minFs.LocationId }
                                join maxFs in DataContext.MaxFleetSizes on
                                    new { fh.CarGroupId, fh.LocationId } equals new { maxFs.CarGroupId, maxFs.LocationId }
                                join adds in additions on new { fh.CarGroupId, fh.LocationId, maxFs.WeekNumber }
                                    equals new { adds.CarGroupId, adds.LocationId, adds.WeekNumber }
                                    into joinedAdds
                                from joinedAdditions in joinedAdds.DefaultIfEmpty()
                                join dels in deletions on new { fh.CarGroupId, fh.LocationId, maxFs.WeekNumber }
                                    equals new { dels.CarGroupId, dels.LocationId, dels.WeekNumber }
                                    into joinedDels
                                from joinedDeletions in joinedDels.DefaultIfEmpty()

                                group new { fh, minFs, maxFs, joinedAdditions, joinedDeletions } by
                                    new { fh.CarGroupId, fh.LocationId, maxFs.WeekNumber, maxFs.PeakDay.Year }
                                    into groupedData
                                    join cg in DataContext.CAR_GROUPs on groupedData.Key.CarGroupId equals cg.car_group_id
                                    join loc in DataContext.LOCATIONs on groupedData.Key.LocationId equals loc.dim_Location_id
                                    orderby groupedData.Key.Year, groupedData.Key.WeekNumber, loc.location1, cg.car_group1

                                    select new DemandGapVisualization
                                           {
                                               CarGroupId = groupedData.Key.CarGroupId,
                                               CarGroupName = cg.car_group1,
                                               LocationId = groupedData.Key.LocationId,
                                               LocationName = loc.location1,
                                               WeekNumber = groupedData.Key.WeekNumber,
                                               OperationalFleet = groupedData.Sum(d => d.fh.PeakOperationalFleet),
                                               MinFleetRequired = groupedData.Sum(d => d.minFs.MinFleet),
                                               MaxFleet = groupedData.Sum(d => d.maxFs.MaxFleet),
                                               Additions = groupedData.Sum(d => d.joinedAdditions == null ? 0 : d.joinedAdditions.Additions),
                                               Deletions = groupedData.Sum(d => d.joinedDeletions == null ? 0 : d.joinedDeletions.Deletions),
                                               ExpectedFleet = groupedData.Sum(d => d.fh.PeakOperationalFleet)
                                                            - groupedData.Sum(d => d.joinedDeletions == null ? 0 : d.joinedDeletions.Deletions)
                                                            + groupedData.Sum(d => d.joinedAdditions == null ? 0 : d.joinedAdditions.Additions)
                                           };
            var returned = demandGapData;
            //var xx = returned.Where(d => d.Additions != 0.0).Take(10).ToList();
            return returned;
        }

        public List<DemandGapOneByCarGroup> GetDemandGapOne(IQueryable<DemandGapVisualization> demandGapVisualization)
        {
            var demandGapOne = from dv in demandGapVisualization
                               where dv.ExpectedFleet < dv.MinFleetRequired
                               group dv by new {dv.CarGroupId, dv.WeekNumber}
                               into groupedData
                               join cg in DataContext.CAR_GROUPs on groupedData.Key.CarGroupId equals cg.car_group_id
                               select new DemandGapOneByCarGroup
                                    {
                                        CarGroupId = groupedData.Key.CarGroupId,
                                        CarGroupName = cg.car_group1,
                                        WeekNumber = groupedData.Key.WeekNumber,
                                        Gap = groupedData.Sum(d=> d.ExpectedFleet - (d.MinFleetRequired))
                                    };
            var returned = demandGapOne.ToList().Where(d=> d.GapRounded != 0).ToList();
            return returned;
        }

        public List<SpreadRow> GetSpreadForCountry()
        {
            return null;
        }

        public List<SpreadRow> GetSpreadForLocation()
        {
            var fleetHistory = FleetHistoryQueryable.GetAvailabilityHistory(DataContext, Parameters);

            
            var holdingCosts = from lhc in DataContext.LifecycleHoldingCosts
                               where lhc.Month == 1
                                && lhc.Year == 2014
                                select lhc;

            var revenue = from rev in DataContext.RevenueByCommercialCarSegments
                          where rev.Month == 1
                          && rev.Year == 2014
                          group rev by new {rev.CarGroupId, rev.LocationId}
                          into groupedData
                        select new
                               {
                                   groupedData.Key.CarGroupId,
                                   groupedData.Key.LocationId,
                                   Revenue = groupedData.Sum(d=> d.GrossRevenue)
                               };

            var groupedHistory = from fh in fleetHistory                
                group fh by new { fh.CarGroupId, fh.LocationId }
                into groupedData
                select new {groupedData.Key.CarGroupId, groupedData.Key.LocationId, TotalFleet = groupedData.Sum(d=> d.AvgTotal)};

            

            var spreadData = from gd in groupedHistory
                join rev in revenue on
                    new {gd.CarGroupId, gd.LocationId}
                    equals new {rev.CarGroupId, rev.LocationId}
                join lc in holdingCosts on
                    gd.CarGroupId equals lc.CarGroupId
                join cg in DataContext.CAR_GROUPs on gd.CarGroupId equals cg.car_group_id
                join l in DataContext.LOCATIONs on gd.LocationId equals l.dim_Location_id
                select new SpreadRow
                       {
                           Pool = l.CMS_LOCATION_GROUP.CMS_POOL.cms_pool1,
                           LocationGroup = l.CMS_LOCATION_GROUP.cms_location_group1,
                           Location = l.location1,
                           CarSegment = cg.CAR_CLASS.CAR_SEGMENT.car_segment1,
                           CarClass = cg.CAR_CLASS.car_class1,
                           CarGroup = cg.car_group1,
                           TotalFleet = gd.TotalFleet,
                           Revenue = rev.Revenue,
                           Cost = lc.Cost
                       };

            var returned = spreadData.ToList().OrderByDescending(d=> d.Spread).ToList();
            return returned;
        }
    }
}