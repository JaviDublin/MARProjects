using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.App.Classes.BLL.ExtensionMethods;
using Mars.App.Classes.Phase4Dal.Enumerators;
using Mars.FleetAllocation.DataContext;

namespace Mars.FleetAllocation.DataAccess.AdditionPlanDataAccess
{
    public static class AdditionPlanEntryFilter
    {
        public static IQueryable<AdditionPlanEntry> GetAdditionPlanEntries(FaoDataContext dc,
            Dictionary<DictionaryParameter, string> parameters)
        {
            var additionPlanEntities = from ape in dc.AdditionPlanEntries
                select ape;

            if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CarGroup))
            {
                var carGroupId = int.Parse(parameters[DictionaryParameter.CarGroup]);
                additionPlanEntities = additionPlanEntities.Where(d => d.CarGroupId == carGroupId);
            }
            else if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CarClass))
            {
                var carClassId = int.Parse(parameters[DictionaryParameter.CarClass]);
                additionPlanEntities = additionPlanEntities.Where(d => d.CAR_GROUP.car_class_id == carClassId);
            }
            else if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CarSegment))
            {
                var carSegmentId = int.Parse(parameters[DictionaryParameter.CarSegment]);
                additionPlanEntities = additionPlanEntities.Where(d => d.CAR_GROUP.CAR_CLASS.car_segment_id == carSegmentId);
            }
            else if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.OwningCountry))
            {
                var owningCountry = parameters[DictionaryParameter.OwningCountry];
                additionPlanEntities = additionPlanEntities.Where(d => d.CAR_GROUP.CAR_CLASS.CAR_SEGMENT.country == owningCountry);
            }


            if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.Location))
            {
                var location = int.Parse(parameters[DictionaryParameter.Location]);
                additionPlanEntities = additionPlanEntities.Where(d => d.LOCATION.dim_Location_id == location);
            }
            else if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.LocationGroup)
                    || parameters.ContainsValueAndIsntEmpty(DictionaryParameter.Area))
            {
                if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.LocationGroup))
                {
                    var locationGroupId = int.Parse(parameters[DictionaryParameter.LocationGroup]);
                    additionPlanEntities = from av in additionPlanEntities
                                   where av.LOCATION.cms_location_group_id == locationGroupId
                                   select av;
                }
                else
                {
                    var areaId = int.Parse(parameters[DictionaryParameter.Area]);
                    additionPlanEntities = from av in additionPlanEntities
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
                    additionPlanEntities = from av in additionPlanEntities
                                   where av.LOCATION.CMS_LOCATION_GROUP.cms_pool_id == poolId
                                   select av;
                }
                else
                {
                    var regionId = int.Parse(parameters[DictionaryParameter.Region]);
                    additionPlanEntities = from av in additionPlanEntities
                                   where av.LOCATION.OPS_AREA.ops_region_id == regionId
                                   select av;
                }
            }
            else if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.LocationCountry))
            {
                var locationCountry = parameters[DictionaryParameter.LocationCountry];
                additionPlanEntities = from av in additionPlanEntities
                               where av.LOCATION.CMS_LOCATION_GROUP.CMS_POOL.country == locationCountry
                               select av;
            }
            return additionPlanEntities;
        }
    }
}