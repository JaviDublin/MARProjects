using System;
using System.Collections.Generic;
using System.Data.Linq.SqlClient;
using System.Data.Objects.SqlClient;
using System.Diagnostics;
using System.Linq;
using Mars.App.Classes.DAL.MarsDBContext;
using Mars.App.Classes.Phase4Dal.Enumerators;
using Mars.FleetAllocation.DataAccess.Entities;
using Mars.FleetAllocation.DataAccess.ParameterFiltering;
using Mars.FleetAllocation.DataContext;

namespace Mars.FleetAllocation.DataAccess
{
    public class MinFleetDataAccess : BaseDataAccess
    {
        public MinFleetDataAccess(Dictionary<DictionaryParameter, string> parameters, FaoDataContext dc)
            : base(parameters, dc)
        {
            
        }

 

        public IQueryable<NessesaryFleet> CalculateMinNessesaryFleetCommercialSegment(int scenarioId)
        {
            var nessFleet = CalculateMinNessesaryFleet();

            var maxRevMonth = DataContext.RevenueByCommercialCarSegments.Max(d => d.MonthDate);
            var minRevMonth = maxRevMonth.AddMonths(-3);

            var revData = from rd in DataContext.RevenueByCommercialCarSegments
                          where rd.MonthDate >= minRevMonth && rd.MonthDate >= maxRevMonth
                          group rd by new {rd.CarGroupId, rd.LocationId, rd.CommercialCarSegmentId}
                          into groupedData
                        select new
                               {
                                   groupedData.Key.CarGroupId,
                                   groupedData.Key.LocationId,
                                   groupedData.Key.CommercialCarSegmentId,
                                   GrossRevenue = groupedData.Average(d=> d.GrossRevenue)
                               };

            
            var nessFleetCommercialSegment = from ns in nessFleet
                                             join l in DataContext.LOCATIONs on ns.LocationId equals l.dim_Location_id
                                             join cg in DataContext.CAR_GROUPs on ns.CarGroupId equals cg.car_group_id
                                            join mcs in DataContext.MinCommercialSegments on
                                                new {
                                                        CarSegmentId = cg.CAR_CLASS.car_segment_id,
                                                        ns.LocationId
                                                    }
                                                equals 
                                                new {
                                                        mcs.CarSegmentId,
                                                        mcs.LocationId
                                                    }
                                            join rev in revData on new
                                                    {
                                                        CarGroupId = cg.car_group_id,
                                                        ns.LocationId,
                                                        mcs.CommercialCarSegmentId
                                                    }
                                                    equals new
                                                    {
                                                        rev.CarGroupId,
                                                        rev.LocationId,
                                                        rev.CommercialCarSegmentId
                                                    }
                                             where mcs.MinCommercialSegmentScenarioId == scenarioId
                                             //where rev.Month == 12 && rev.Year == 2014
                                    select new NessesaryFleet
                                               {
                                                   LocationId = ns.LocationId,
                                                   CarGroupId = ns.CarGroupId,
                                                   //CarGroupName = cg.car_group1,
                                                   //LocationName = l.location1,
                                                   //CommercialCarSegmentNameName = mcs.CommercialCarSegment.Name,
                                                   CommericalCarSegmentId = mcs.CommercialCarSegmentId,
                                                   AverageOnRent = ns.AverageOnRent,
                                                   MinCommercialSegmentRequired = mcs.Percentage,
                                                   Revenue = rev.GrossRevenue,
                                               };

            

            var nessFleetWithTotalRevenue = CalculateTotalRevenue(nessFleetCommercialSegment);
            

            var combinedData = from nfcs in nessFleetCommercialSegment
                join nftr in nessFleetWithTotalRevenue on new
                                    {
                                        nfcs.LocationId,
                                        nfcs.CarGroupId
                                    } equals new
                                    {
                                        nftr.LocationId,
                                        nftr.CarGroupId
                                    }
                                    select new
                                           {
                                               nfcs.LocationId,
                                               nfcs.CarGroupId,
                                               nfcs.CommericalCarSegmentId,
                                               MinNessesaryFleetRequired = nfcs.AverageOnRent * 
                                                        nfcs.MinCommercialSegmentRequired * 
                                                        (nftr.TotalRevenueByCommercialCarSegment == 0 ? 0 : nfcs.Revenue / nftr.TotalRevenueByCommercialCarSegment) 
                                           };

            var nessData = from cd in combinedData
                           group cd by new {cd.LocationId, cd.CarGroupId}
                           into groupedData
                           select new NessesaryFleet
                            {
                                CarGroupId = groupedData.Key.CarGroupId,
                                LocationId = groupedData.Key.LocationId,
                                MinNessesaryFleetRequired = groupedData.Sum(d=> d.MinNessesaryFleetRequired)
                            };


            var returned = nessData;
            return returned;
        }

        private IEnumerable<NessesaryFleet> CalculateTotalRevenue(IQueryable<NessesaryFleet> listOfNessFleet)
        {
            var totalData = from nf in listOfNessFleet
                group nf by new {nf.LocationId, nf.CarGroupId}
                into groupedData
                 select new NessesaryFleet
                       {
                           LocationId = groupedData.Key.LocationId,
                           CarGroupId = groupedData.Key.CarGroupId,
                           TotalRevenueByCommercialCarSegment = groupedData.Sum(d => d.Revenue)
                       };

            
            return totalData;
        }
        

        public IQueryable<NessesaryFleet> CalculateMinNessesaryFleet()
        {
            var fleetHistory = FleetHistoryQueryable.GetAvailabilityHistory(DataContext, Parameters);

            

            var groupedOnWeek = from fh in fleetHistory
                                join week in DataContext.IsoWeekOfYears on fh.Timestamp equals week.Day
                group fh by new
                            {
                                fh.LocationId,
                                fh.CarGroupId,
                                week.WeekOfYear
                            }
                into groupedData
                select new
                       {
                           groupedData.Key.CarGroupId,
                           groupedData.Key.LocationId,
                           OnRent = groupedData.Max(d=> d.PeakOnRent),
                           //DayOfWeek = groupedData.Key.DayOfWeek
                       };

            var averagedForMonth = from gow in groupedOnWeek
                                    group gow by new
                                              {
                                                  gow.LocationId,
                                                  gow.CarGroupId,
                                              }
                                    into groupedData
                                    select new NessesaryFleet
                                           {
                                               LocationId = groupedData.Key.LocationId,
                                               CarGroupId = groupedData.Key.CarGroupId,
                                               AverageOnRent = groupedData.Average(d=> d.OnRent)
                                           };

            return averagedForMonth;
            
        }
    }
}