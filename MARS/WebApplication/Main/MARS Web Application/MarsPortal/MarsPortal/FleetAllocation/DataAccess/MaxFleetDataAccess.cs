using System;
using System.Activities.Expressions;
using System.Collections.Generic;
using System.Data.Linq.SqlClient;
using System.Linq;
using System.Web;
using Mars.App.Classes.DAL.MarsDBContext;
using Mars.App.Classes.Phase4Dal.Enumerators;
using Mars.FleetAllocation.DataAccess.Entities;
using Mars.FleetAllocation.DataAccess.Forecast;
using Mars.FleetAllocation.DataContext;

namespace Mars.FleetAllocation.DataAccess
{
    public class MaxFleetDataAccess : BaseDataAccess
    {
        public MaxFleetDataAccess(Dictionary<DictionaryParameter, string> parameters, FaoDataContext dc)
            : base(parameters, dc)
        {
            
        }

        public IQueryable<MaximumFleet> GetMaxFleetSize(int maxFleetFactorsScenarioId)
        {
            var forecast = ForecastQueryable.GetForecast(DataContext, Parameters);

            var forecastBySegment = from fc in forecast
                join woy in DataContext.IsoWeekOfYears on fc.REP_DATE equals woy.Day                
                group fc by new
                            {
                                fc.CMS_LOCATION_GROUP_ID,
                                fc.CAR_GROUP.CAR_CLASS.car_segment_id, //Is Car Group
                                woy.WeekOfYear
                            }
                into groupedData
                select new
                       {
                           groupedData.OrderByDescending(d => d.UNCONSTRAINED).First().REP_DATE,
                           groupedData.Key.CMS_LOCATION_GROUP_ID,
                           groupedData.Key.car_segment_id,
                           groupedData.Key.WeekOfYear
                       };


            var forecastByCarGroup = from fc in forecast
                                     join fbs in forecastBySegment on 
                                        new { fc.REP_DATE, fc.CMS_LOCATION_GROUP_ID, fc.CAR_GROUP.CAR_CLASS.car_segment_id }
                                     equals
                                        new { fbs.REP_DATE, fbs.CMS_LOCATION_GROUP_ID, fbs.car_segment_id}
                                    join cmsLocationLevel in DataContext.CmsToLocationLevelPercents on new
                                            {
                                                CarGroupId = fc.CAR_CLASS_ID,
                                                LocationGroupId = fc.CMS_LOCATION_GROUP_ID
                                            }
                                            equals new
                                                    {
                                                        cmsLocationLevel.CarGroupId,
                                                        cmsLocationLevel.LocationGroupId
                                                    }
                                     join mff in DataContext.MaxFleetFactors.Where(d => d.MaxFleetFactorScenarioId == maxFleetFactorsScenarioId) on new
                                             {
                                                 cmsLocationLevel.LocationId,
                                                 cmsLocationLevel.CarGroupId,
                                                 WeekDay = (byte)fc.REP_DATE.DayOfWeek
                                             }
                                             equals new
                                                    {
                                                        mff.LocationId,
                                                        mff.CarGroupId,
                                                        WeekDay = mff.DayOfWeekId
                                                    }
                                       into joinedMff
                                     from leftJoinedMff in joinedMff.DefaultIfEmpty()
                select new 
                       {
                           cmsLocationLevel.LocationId,
                           CarGroupId = fc.CAR_CLASS_ID,     //Is Car Group
                           WeekNumber = fbs.WeekOfYear,
                           MaxForLocation = ( (double) (fc.UNCONSTRAINED ?? 0)) 
                                * ( cmsLocationLevel.PercentVehiclesAllocated)
                                 * ( 1 + (( 1 - (leftJoinedMff.UtilizationPercentage.HasValue ? leftJoinedMff.UtilizationPercentage.Value : 0) 
                                        * (1 + (leftJoinedMff.NonRevPercentage.HasValue ? leftJoinedMff.NonRevPercentage.Value : 0))) )),
                           PeakDay = fc.REP_DATE,
                           fc.REP_DATE.Year,
                       };

            var maxFleet = from fc in forecastByCarGroup
                           group fc by new {fc.LocationId, fc.CarGroupId, fc.WeekNumber, fc.Year}
                           into groupedData
                           select new MaximumFleet
                            {
                                LocationId = groupedData.Key.LocationId,
                                CarGroupId = groupedData.Key.CarGroupId,
                                WeekNumber = groupedData.Key.WeekNumber,
                                Year = groupedData.Key.Year,
                                MaxForLocation = groupedData.Average(d=> d.MaxForLocation)
                            };

            
            var returned = maxFleet;
            return returned;
        }

    }
}