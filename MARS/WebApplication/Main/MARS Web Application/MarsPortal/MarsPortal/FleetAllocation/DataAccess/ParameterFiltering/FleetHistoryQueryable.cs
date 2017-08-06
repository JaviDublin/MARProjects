using System;
using System.Collections.Generic;
using System.Linq;
using Mars.App.Classes.BLL.ExtensionMethods;
using Mars.App.Classes.Phase4Dal.Enumerators;
using Mars.FleetAllocation.DataContext;

namespace Mars.FleetAllocation.DataAccess.ParameterFiltering
{
    public static class FleetHistoryQueryable
    {
        public static IQueryable<FleetHistory> GetAvailabilityHistory(FaoDataContext dataContext, Dictionary<DictionaryParameter, string> parameters )
        {
            var availability = dataContext.FleetHistories.Select(d => d);
            var startDate = parameters.GetDateFromDictionary(DictionaryParameter.StartDate);
            var endDate = parameters.GetDateFromDictionary(DictionaryParameter.EndDate);


            availability = availability.Where(d => d.Timestamp.Date >= startDate);
            if (endDate != DateTime.MinValue)
            {
                availability = availability.Where(d => d.Timestamp.Date <= endDate);
            }

            if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.FleetTypes))
            {
                var selectedFleetTypes = parameters[DictionaryParameter.FleetTypes].Split(',').Select(byte.Parse);
                availability = availability.Where(d => selectedFleetTypes.Contains(d.FleetTypeId));
            }

            if (parameters.ContainsKey(DictionaryParameter.DayOfWeek) &&
                    parameters[DictionaryParameter.DayOfWeek] != string.Empty)
            {
                DayOfWeek dowEntered;
                var success = Enum.TryParse(parameters[DictionaryParameter.DayOfWeek], out dowEntered);
                if (!success) throw new InvalidCastException("Unable to cast Day of Week");
                availability = availability.Where(d => d.Timestamp.DayOfWeek == dowEntered);
            }

            if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CarGroup))
            {
                var carGroupString = parameters[DictionaryParameter.CarGroup];

                if (!carGroupString.Contains(LocationQueryable.Separator))
                {
                    var carGroupId = int.Parse(carGroupString);
                    availability = availability.Where(d => d.CarGroupId == carGroupId);
                }
                else
                {
                    var splitCarGroupIds = carGroupString.Split(LocationQueryable.Separator.ToCharArray()).Select(int.Parse);
                    availability = from a in availability
                                   where splitCarGroupIds.Contains(a.CarGroupId)
                                select a;
                }
            }
            else if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CarClass))
            {
                var carClassString = parameters[DictionaryParameter.CarClass];

                if (!carClassString.Contains(LocationQueryable.Separator))
                {
                    var carClassId = int.Parse(carClassString);
                    availability = availability.Where(d => d.CAR_GROUP.car_class_id == carClassId);
                }
                else
                {
                    var splitCarClassIds = carClassString.Split(LocationQueryable.Separator.ToCharArray()).Select(int.Parse);
                    availability = from a in availability
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
                    availability = availability.Where(d => d.CAR_GROUP.CAR_CLASS.car_segment_id == carSegmentId);
                }
                else
                {
                    var splitCarSegmentIds = carSegmentString.Split(LocationQueryable.Separator.ToCharArray()).Select(int.Parse);
                    availability = from a in availability
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
                    availability = availability.Where(d => d.CAR_GROUP.CAR_CLASS.CAR_SEGMENT.country == carOwningCountryId);
                }
                else
                {
                    var splitOwningCountryIds = owningCountryString.Split(LocationQueryable.Separator.ToCharArray());
                    availability = from a in availability
                                   where splitOwningCountryIds.Contains(a.CAR_GROUP.CAR_CLASS.CAR_SEGMENT.country)
                                    select a;
                }
            }


            if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.Location))
            {
                var locationString = parameters[DictionaryParameter.Location];

                if (!locationString.Contains(LocationQueryable.Separator))
                {
                    var locationId = int.Parse(locationString);
                    availability = availability.Where(d => d.LocationId == locationId);
                }
                else
                {
                    var splitLocationIds = locationString.Split(LocationQueryable.Separator.ToCharArray()).Select(int.Parse);
                    availability = from a in availability
                                   where splitLocationIds.Contains(a.LocationId)
                                    select a;
                }
            }
            else if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.LocationGroup))
            {
                var locationGroupString = parameters[DictionaryParameter.LocationGroup];
                if (!locationGroupString.Contains(LocationQueryable.Separator))
                {
                    var locationGroupId = int.Parse(locationGroupString);
                    availability = availability.Where(d => d.LOCATION.cms_location_group_id == locationGroupId);
                }
                else
                {
                    var splitLocationGroupIds = locationGroupString.Split(LocationQueryable.Separator.ToCharArray()).Select(int.Parse);
                    availability = from a in availability
                                where splitLocationGroupIds.Contains(a.LOCATION.cms_location_group_id.Value)
                                select a;
                }
            }
            else if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.Pool))
            {
                var poolsString = parameters[DictionaryParameter.Pool];
                if (!poolsString.Contains(LocationQueryable.Separator))
                {
                    var poolId = int.Parse(poolsString);
                    availability = availability.Where(d => d.LOCATION.CMS_LOCATION_GROUP.cms_pool_id == poolId);
                }
                else
                {
                    var splitPoolIds = poolsString.Split(LocationQueryable.Separator.ToCharArray()).Select(int.Parse);
                    availability = from a in availability
                                where splitPoolIds.Contains(a.LOCATION.CMS_LOCATION_GROUP.cms_pool_id)
                                select a;
                }
            }
            else if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.LocationCountry))
            {
                var locationCountryString = parameters[DictionaryParameter.LocationCountry];
                if (!locationCountryString.Contains(LocationQueryable.Separator))
                {
                    var locationCountryId = locationCountryString;
                    availability = availability.Where(d => d.LOCATION.CMS_LOCATION_GROUP.CMS_POOL.country == locationCountryId);
                }
                else
                {
                    var splitLocationCountryIds = locationCountryString.Split(LocationQueryable.Separator.ToCharArray());
                    availability = from a in availability
                                where splitLocationCountryIds.Contains(a.LOCATION.CMS_LOCATION_GROUP.CMS_POOL.country)
                                select a;
                }
            }

            return availability;
        }

    }
}