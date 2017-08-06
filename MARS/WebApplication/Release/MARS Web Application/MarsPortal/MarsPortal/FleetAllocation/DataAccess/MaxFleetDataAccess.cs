using System;
using System.Activities.Expressions;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.App.Classes.DAL.MarsDBContext;
using Mars.FleetAllocation.DataAccess.Entities;
using Mars.FleetAllocation.DataContext;

namespace Mars.FleetAllocation.DataAccess
{
    public class MaxFleetDataAccess : BaseDataAccess
    {        

        public List<MaximumFleet> GetMaxFleetSize()
        {
            var locationGroupId = 36; // Berlin

            var forecastBySegment = from fc in DataContext.MARS_CMS_FORECASTs
                join woy in DataContext.IsoWeekOfYears on fc.REP_DATE equals woy.Day
                                    where fc.COUNTRY == "GE" && fc.CMS_LOCATION_GROUP_ID == locationGroupId
                //where 
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
                                   

            var forecastByCarGroup = from fc in DataContext.MARS_CMS_FORECASTs
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
                                     where fc.COUNTRY == "GE" && fc.CMS_LOCATION_GROUP_ID == locationGroupId
                select new MaximumFleet
                       {
                           LocationGroupId = fc.CMS_LOCATION_GROUP_ID,
                           LocationId = cmsLocationLevel.LocationId,
                           LocationName = cmsLocationLevel.LOCATION.location1,
                           CarGroupId = fc.CAR_CLASS_ID,     //Is Car Group
                           WeekNumber = fbs.WeekOfYear,
                           MaxForLocationGroup = fc.UNCONSTRAINED ?? 0,
                           MaxForLocation = (fc.UNCONSTRAINED ?? 0) * ( (decimal) cmsLocationLevel.PercentVehiclesAllocated),
                           PeakDay = fc.REP_DATE,
                           LocationGroupName = fc.CMS_LOCATION_GROUP.cms_location_group1,
                           CarGroupName = fc.CAR_GROUP.car_group1
                       };


            var returned = forecastByCarGroup.ToList();
            return returned;
        }

    }
}