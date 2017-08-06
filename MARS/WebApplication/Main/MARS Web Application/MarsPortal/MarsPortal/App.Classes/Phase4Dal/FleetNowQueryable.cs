using System.Collections.Generic;
using System.Linq;
using Mars.App.Classes.BLL.ExtensionMethods;
using Mars.App.Classes.DAL.MarsDBContext;
using Mars.App.Classes.Phase4Dal.Enumerators;

namespace Mars.App.Classes.Phase4Dal
{
    public static class FleetNowQueryable
    {

        public static IQueryable<FleetNow> GetFleetNow(IQueryable<FleetNow> fleetNow, Dictionary<DictionaryParameter, string> parameters)
        {
            if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.FleetTypes))
            {
                var selectedFleetTypes = parameters[DictionaryParameter.FleetTypes].Split(',').Select(int.Parse);
                fleetNow = fleetNow.Where(d => selectedFleetTypes.Contains(d.FleetTypeId));
            }

            if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CarGroup))
            {
                var carGroupId = int.Parse(parameters[DictionaryParameter.CarGroup]);
                fleetNow = from av in fleetNow
                               where av.CAR_GROUP.car_group_id == carGroupId
                               select av;
            }
            else if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CarClass))
            {
                var carClassId = int.Parse(parameters[DictionaryParameter.CarClass]);

                fleetNow = from av in fleetNow
                               where av.CAR_GROUP.car_class_id == carClassId
                               select av;
            }
            else if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CarSegment))
            {
                var carSegmentId = int.Parse(parameters[DictionaryParameter.CarSegment]);

                fleetNow = from av in fleetNow
                               where av.CAR_GROUP.CAR_CLASS.car_segment_id == carSegmentId
                               select av;
            }
            else if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.OwningCountry))
            {
                var owningCountry = parameters[DictionaryParameter.OwningCountry];

                fleetNow = from av in fleetNow
                               where av.CAR_GROUP.CAR_CLASS.CAR_SEGMENT.country == owningCountry
                               select av;
            }


            if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.Location))
            {
                var location = int.Parse(parameters[DictionaryParameter.Location]);

                fleetNow = from av in fleetNow
                               where av.LOCATION.dim_Location_id == location
                               select av;
            }
            else if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.LocationGroup)
                    || parameters.ContainsValueAndIsntEmpty(DictionaryParameter.Area))
            {
                if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.LocationGroup))
                {
                    var locationGroupId = int.Parse(parameters[DictionaryParameter.LocationGroup]);

                    fleetNow = from av in fleetNow
                                   where av.LOCATION.cms_location_group_id == locationGroupId
                                   select av;
                }
                else
                {
                    var areaId = int.Parse(parameters[DictionaryParameter.Area]);

                    fleetNow = from av in fleetNow
                                   where av.LOCATION.ops_area_id == areaId
                                   select av;

                }
            }
            else if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.Pool)
                || parameters.ContainsValueAndIsntEmpty(DictionaryParameter.Region))
            {
                if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.Pool))
                {
                    var poolId = int.Parse(parameters[DictionaryParameter.Pool]);

                    fleetNow = from av in fleetNow
                                   where av.LOCATION.CMS_LOCATION_GROUP.cms_pool_id == poolId
                                   select av;
                }
                else
                {
                    var regionId = int.Parse(parameters[DictionaryParameter.Region]);

                    fleetNow = from av in fleetNow
                                   where av.LOCATION.OPS_AREA.ops_region_id == regionId
                                   select av;
                }
            }
            else if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.LocationCountry))
            {
                var locationCountry = parameters[DictionaryParameter.LocationCountry];

                fleetNow = from av in fleetNow
                               where av.LOCATION.country == locationCountry
                               select av;
            }


            return fleetNow;
        }
    }
}