using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.App.Classes.BLL.ExtensionMethods;
using Mars.App.Classes.Phase4Dal.Enumerators;
using Mars.FleetAllocation.DataAccess.ParameterFiltering;
using Mars.FleetAllocation.DataContext;

namespace Mars.FleetAllocation.DataAccess.AdditionDeletion
{
    public static class AdditionDeletionQueryable
    {
        public static IQueryable<ResAddition> GetAdditions(FaoDataContext dataContext, Dictionary<DictionaryParameter, string> parameters)
        {
            var additions = from ra in dataContext.ResAdditions
                        select ra;

            if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.ForecastStartDate))
            {
                var forecastStartDate = DateTime.Parse(parameters[DictionaryParameter.ForecastStartDate]);
                additions = additions.Where(d => d.RepDate >= forecastStartDate);
            }

            if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.ForecastEndDate))
            {
                var forecastEndDate = DateTime.Parse(parameters[DictionaryParameter.ForecastEndDate]);
                additions = additions.Where(d => d.RepDate >= forecastEndDate);
            }

            if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CarGroup))
            {
                var carGroupString = parameters[DictionaryParameter.CarGroup];

                var carGroupId = int.Parse(carGroupString);
                additions = additions.Where(d => d.CarGrpId == carGroupId);

            }
            else if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CarClass))
            {
                var carClassString = parameters[DictionaryParameter.CarClass];


                var carClassId = int.Parse(carClassString);
                additions = additions.Where(d => d.CAR_GROUP.car_class_id == carClassId);

            }
            else if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CarSegment))
            {
                var carSegmentString = parameters[DictionaryParameter.CarSegment];
                var carSegmentId = int.Parse(carSegmentString);
                additions = additions.Where(d => d.CAR_GROUP.CAR_CLASS.car_segment_id == carSegmentId);

            }
            else if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.OwningCountry))
            {
                var owningCountryString = parameters[DictionaryParameter.OwningCountry];
                var carOwningCountryId = owningCountryString;
                additions = additions.Where(d => d.CAR_GROUP.CAR_CLASS.CAR_SEGMENT.country == carOwningCountryId);
            }

            //
            if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.Location))
            {
                var locationString = parameters[DictionaryParameter.Location];
                var locationId = int.Parse(locationString);
                additions = additions.Where(d => d.LocId == locationId);
            }
            else if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.LocationGroup))
            {
                var locationGroupString = parameters[DictionaryParameter.LocationGroup];
                var locationGroupId = int.Parse(locationGroupString);
                additions = additions.Where(d => d.LOCATION.cms_location_group_id == locationGroupId);
            }
            else if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.Pool))
            {
                var poolsString = parameters[DictionaryParameter.Pool];
                
                var poolId = int.Parse(poolsString);
                additions = additions.Where(d => d.LOCATION.CMS_LOCATION_GROUP.cms_pool_id == poolId);
                
            }
            else if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.LocationCountry))
            {
                var locationCountryString = parameters[DictionaryParameter.LocationCountry];

                var locationCountryId = locationCountryString;
                additions = additions.Where(d => d.LOCATION.CMS_LOCATION_GROUP.CMS_POOL.country == locationCountryId);                
            }

            return additions;
        }

        public static IQueryable<ResDeletion> GetDeletions(FaoDataContext dataContext, Dictionary<DictionaryParameter, string> parameters)
        {
            var deletions = from ra in dataContext.ResDeletions
                            select ra;

            if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.ForecastStartDate))
            {
                var forecastStartDate = DateTime.Parse(parameters[DictionaryParameter.ForecastStartDate]);
                deletions = deletions.Where(d => d.RepDate >= forecastStartDate);
            }

            if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.ForecastEndDate))
            {
                var forecastEndDate = DateTime.Parse(parameters[DictionaryParameter.ForecastEndDate]);
                deletions = deletions.Where(d => d.RepDate >= forecastEndDate);
            }

            if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CarGroup))
            {
                var carGroupString = parameters[DictionaryParameter.CarGroup];

                var carGroupId = int.Parse(carGroupString);
                deletions = deletions.Where(d => d.CarGrpId == carGroupId);

            }
            else if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CarClass))
            {
                var carClassString = parameters[DictionaryParameter.CarClass];


                var carClassId = int.Parse(carClassString);
                deletions = deletions.Where(d => d.CAR_GROUP.car_class_id == carClassId);

            }
            else if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CarSegment))
            {
                var carSegmentString = parameters[DictionaryParameter.CarSegment];
                var carSegmentId = int.Parse(carSegmentString);
                deletions = deletions.Where(d => d.CAR_GROUP.CAR_CLASS.car_segment_id == carSegmentId);

            }
            else if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.OwningCountry))
            {
                var owningCountryString = parameters[DictionaryParameter.OwningCountry];
                var carOwningCountryId = owningCountryString;
                deletions = deletions.Where(d => d.CAR_GROUP.CAR_CLASS.CAR_SEGMENT.country == carOwningCountryId);
            }

            //
            if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.Location))
            {
                var locationString = parameters[DictionaryParameter.Location];
                var locationId = int.Parse(locationString);
                deletions = deletions.Where(d => d.LocId == locationId);
            }
            else if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.LocationGroup))
            {
                var locationGroupString = parameters[DictionaryParameter.LocationGroup];
                var locationGroupId = int.Parse(locationGroupString);
                deletions = deletions.Where(d => d.LOCATION.cms_location_group_id == locationGroupId);
            }
            else if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.Pool))
            {
                var poolsString = parameters[DictionaryParameter.Pool];

                var poolId = int.Parse(poolsString);
                deletions = deletions.Where(d => d.LOCATION.CMS_LOCATION_GROUP.cms_pool_id == poolId);

            }
            else if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.LocationCountry))
            {
                var locationCountryString = parameters[DictionaryParameter.LocationCountry];

                var locationCountryId = locationCountryString;
                deletions = deletions.Where(d => d.LOCATION.CMS_LOCATION_GROUP.CMS_POOL.country == locationCountryId);
            }

            return deletions;
        }
    }
}