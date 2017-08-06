using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.App.Classes.BLL.ExtensionMethods;
using Mars.App.Classes.Phase4Dal.Enumerators;
using Mars.FleetAllocation.DataAccess.ParameterFiltering;
using Mars.FleetAllocation.DataContext;

namespace Mars.FleetAllocation.DataAccess.Forecast
{
    public static class ForecastQueryable
    {
        public static IQueryable<MARS_CMS_FORECAST> GetForecast(FaoDataContext dataContext
                    , Dictionary<DictionaryParameter, string> parameters )
        {
            var forecast = from fc in dataContext.MARS_CMS_FORECASTs
                select fc;

            var startDate = parameters.GetDateFromDictionary(DictionaryParameter.ForecastStartDate);
            var endDate = parameters.GetDateFromDictionary(DictionaryParameter.ForecastEndDate);

            forecast = forecast.Where(d => d.REP_DATE >= startDate);

            forecast = forecast.Where(d => d.REP_DATE <= endDate);

            if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CarGroup))
            {
                var carGroupString = parameters[DictionaryParameter.CarGroup];

                if (!carGroupString.Contains(LocationQueryable.Separator))
                {
                    var carGroupId = int.Parse(carGroupString);
                    forecast = forecast.Where(d => d.CAR_CLASS_ID == carGroupId);
                }
                else
                {
                    var splitCarGroupIds = carGroupString.Split(LocationQueryable.Separator.ToCharArray()).Select(int.Parse);
                    forecast = from a in forecast
                                   where splitCarGroupIds.Contains(a.CAR_CLASS_ID)
                                select a;
                }
            }
            else if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CarClass))
            {
                var carClassString = parameters[DictionaryParameter.CarClass];

                if (!carClassString.Contains(LocationQueryable.Separator))
                {
                    var carClassId = int.Parse(carClassString);
                    forecast = forecast.Where(d => d.CAR_GROUP.car_class_id == carClassId);
                }
                else
                {
                    var splitCarClassIds = carClassString.Split(LocationQueryable.Separator.ToCharArray()).Select(int.Parse);
                    forecast = from a in forecast
                                   where splitCarClassIds.Contains(a.CAR_GROUP.car_class_id)
                                   select a;
                }
            }
            else if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CarSegment))
            {
                var carSegmentString = parameters[DictionaryParameter.CarSegment];

                if (!carSegmentString.Contains(LocationQueryable.Separator))
                {
                    var carSegmentId = int.Parse(carSegmentString);
                    forecast = forecast.Where(d => d.CAR_GROUP.CAR_CLASS.car_segment_id == carSegmentId);
                }
                else
                {
                    var splitCarSegmentIds = carSegmentString.Split(LocationQueryable.Separator.ToCharArray()).Select(int.Parse);
                    forecast = from a in forecast
                                   where splitCarSegmentIds.Contains(a.CAR_GROUP.CAR_CLASS.car_segment_id)
                                   select a;
                }
            }
            else if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.OwningCountry))
            {
                var owningCountryString = parameters[DictionaryParameter.OwningCountry];

                if (!owningCountryString.Contains(LocationQueryable.Separator))
                {
                    var carOwningCountryId = owningCountryString;
                    forecast = forecast.Where(d => d.CAR_GROUP.CAR_CLASS.CAR_SEGMENT.country == carOwningCountryId);
                }
                else
                {
                    var splitOwningCountryIds = owningCountryString.Split(LocationQueryable.Separator.ToCharArray());
                    forecast = from a in forecast
                                   where splitOwningCountryIds.Contains(a.CAR_GROUP.CAR_CLASS.CAR_SEGMENT.country)
                                   select a;
                }
            }

            //

            if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.LocationGroup))
            {
                var locationGroupString = parameters[DictionaryParameter.LocationGroup];
                if (!locationGroupString.Contains(LocationQueryable.Separator))
                {
                    var locationGroupId = int.Parse(locationGroupString);
                    forecast = forecast.Where(d => d.CMS_LOCATION_GROUP_ID == locationGroupId);
                }
                else
                {
                    var splitLocationGroupIds = locationGroupString.Split(LocationQueryable.Separator.ToCharArray()).Select(int.Parse);
                    forecast = from a in forecast
                               where splitLocationGroupIds.Contains(a.CMS_LOCATION_GROUP_ID)
                                select a;
                }
            }
            else if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.Pool))
            {
                var poolsString = parameters[DictionaryParameter.Pool];
                if (!poolsString.Contains(LocationQueryable.Separator))
                {
                    var poolId = int.Parse(poolsString);
                    forecast = forecast.Where(d => d.CMS_LOCATION_GROUP.cms_pool_id == poolId);
                }
                else
                {
                    var splitPoolIds = poolsString.Split(LocationQueryable.Separator.ToCharArray()).Select(int.Parse);
                    forecast = from a in forecast
                                   where splitPoolIds.Contains(a.CMS_LOCATION_GROUP.cms_pool_id)
                                   select a;
                }
            }
            else if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.LocationCountry))
            {
                var locationCountryString = parameters[DictionaryParameter.LocationCountry];
                if (!locationCountryString.Contains(LocationQueryable.Separator))
                {
                    var locationCountryId = locationCountryString;
                    forecast = forecast.Where(d => d.CMS_LOCATION_GROUP.CMS_POOL.country == locationCountryId);
                }
                else
                {
                    var splitLocationCountryIds = locationCountryString.Split(LocationQueryable.Separator.ToCharArray());
                    forecast = from a in forecast
                                   where splitLocationCountryIds.Contains(a.CMS_LOCATION_GROUP.CMS_POOL.country)
                                   select a;
                }
            }

            return forecast;
        }
    }
}