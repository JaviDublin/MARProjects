using System;
using System.Collections.Generic;
using System.Data.Linq.SqlClient;
using System.Data.Objects.SqlClient;
using System.Diagnostics;
using System.Linq;
using Mars.App.Classes.DAL.MarsDBContext;
using Mars.FleetAllocation.DataAccess.Entities;
using Mars.FleetAllocation.DataContext;

namespace Mars.FleetAllocation.DataAccess
{
    public class MinFleetDataAccess : BaseDataAccess
    {
        public List<NessesaryFleet> GetMinNessesaryFromDb()
        {
            var minFleet = from mf in DataContext.MinNessesaryFleets
                select new NessesaryFleet
                {
                    LocationId = mf.LocationId,
                    CarGroupId = mf.CarGroupId,
                    CarGroupName = mf.CAR_GROUP.car_group1,
                    LocationName = mf.LOCATION.location1,
                    CommercialCarSegmentNameName = mf.CommercialCarSegment.Name,
                    CommericalCarSegmentId = mf.CommercialCarSegmentId,
                    AverageOnRent = mf.AverageOnRent,
                    MinCommercialSegmentRequired = mf.MinCommercialSegmentRequired,
                    Revenue = mf.Revenue,
                    TotalRevenueByCommercialCarSegment = mf.TotalRevenueByCarGroup
                };

            var returned = minFleet.ToList();
            return returned;
        }

        public List<NessesaryFleet> CalculateMinNessesaryFleetCommercialSegment()
        {
            var nessFleet = CalculateMinNessesaryFleet();

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
                                            join rev in DataContext.RevenueByCommercialCarSegments on new
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
                                                    
               
            select new NessesaryFleet
                       {
                           LocationId = ns.LocationId,
                           CarGroupId = ns.CarGroupId,
                           CarGroupName = cg.car_group1,
                           LocationName = l.location1,
                           CommercialCarSegmentNameName = mcs.CommercialCarSegment.Name,
                           CommericalCarSegmentId = mcs.CommercialCarSegmentId,
                           AverageOnRent = ns.AverageOnRent,
                           MinCommercialSegmentRequired = mcs.Percentage,
                           Revenue = rev.GrossRevenue,
                           //TotalRevenueByCommercialCarSegment = 
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
                select new NessesaryFleet
                       {
                           LocationId = nfcs.LocationId,
                           CarGroupId = nfcs.CarGroupId,
                           CarGroupName = nfcs.CarGroupName,
                           LocationName = nfcs.LocationName,
                           CommercialCarSegmentNameName = nfcs.CommercialCarSegmentNameName,
                           CommericalCarSegmentId = nfcs.CommericalCarSegmentId,
                           AverageOnRent = nfcs.AverageOnRent,
                           MinCommercialSegmentRequired = nfcs.MinCommercialSegmentRequired,
                           Revenue = nfcs.Revenue,
                           TotalRevenueByCommercialCarSegment = nftr.TotalRevenueByCommercialCarSegment
                       };

            var returned = combinedData.ToList();
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
            var sw = new Stopwatch();
            sw.Start();
            var startDate = DateTime.Now.AddDays(-28);
            var countryId = 22; //Germany
            var locationId = 3477;


            var fleetHistory = from fh in DataContext.FleetHistories
                where fh.FleetTypeId == 4
                      && fh.Timestamp >= startDate
                     /* && (
                            fh.LocationId == locationId
                             || fh.LocationId == 4029)*/
                      && fh.LOCATION.COUNTRy1.CountryId == countryId
                select new
                       {
                           fh.Timestamp,
                           fh.CarGroupId,
                           fh.LocationId,
                           fh.PeakOnRent
                       };

            var groupedOnWeek = from fh in fleetHistory
                group fh by new
                            {
                                fh.LocationId,
                                fh.CarGroupId,
                                WeekOfYear = DataContext.GetIsoWeekOfYear(fh.Timestamp)
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