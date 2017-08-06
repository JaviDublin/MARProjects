using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.App.Classes.BLL.ExtensionMethods;
using Mars.App.Classes.Phase4Dal.Enumerators;
using Mars.FleetAllocation.DataContext;

namespace Mars.FleetAllocation.DataAccess.AdditionPlanDataAccess
{
    public class AdditionPlanMinMaxValueFilter
    {
        public static IQueryable<AdditionPlanMinMaxValue> GetAdditionPlanMinMaxValues(FaoDataContext dc,
                Dictionary<DictionaryParameter, string> parameters)
        {
            var additionPlanMinMax = from ape in dc.AdditionPlanMinMaxValues
                                       select ape;

            if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CarGroup))
            {
                var carGroupId = int.Parse(parameters[DictionaryParameter.CarGroup]);
                additionPlanMinMax = additionPlanMinMax.Where(d => d.CarGroupId == carGroupId);
            }
            else if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CarClass))
            {
                var carClassId = int.Parse(parameters[DictionaryParameter.CarClass]);
                additionPlanMinMax = additionPlanMinMax.Where(d => d.CAR_GROUP.car_class_id == carClassId);
            }
            else if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CarSegment))
            {
                var carSegmentId = int.Parse(parameters[DictionaryParameter.CarSegment]);
                additionPlanMinMax = additionPlanMinMax.Where(d => d.CAR_GROUP.CAR_CLASS.car_segment_id == carSegmentId);
            }
            else if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.OwningCountry))
            {
                var owningCountry = parameters[DictionaryParameter.OwningCountry];
                additionPlanMinMax = additionPlanMinMax.Where(d => d.CAR_GROUP.CAR_CLASS.CAR_SEGMENT.country == owningCountry);
            }


            if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.Location))
            {
                var location = int.Parse(parameters[DictionaryParameter.Location]);
                additionPlanMinMax = additionPlanMinMax.Where(d => d.LOCATION.dim_Location_id == location);
            }
            else if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.LocationGroup)
                    || parameters.ContainsValueAndIsntEmpty(DictionaryParameter.Area))
            {
                if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.LocationGroup))
                {
                    var locationGroupId = int.Parse(parameters[DictionaryParameter.LocationGroup]);
                    additionPlanMinMax = from av in additionPlanMinMax
                                           where av.LOCATION.cms_location_group_id == locationGroupId
                                           select av;
                }
                else
                {
                    var areaId = int.Parse(parameters[DictionaryParameter.Area]);
                    additionPlanMinMax = from av in additionPlanMinMax
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
                    additionPlanMinMax = from av in additionPlanMinMax
                                           where av.LOCATION.CMS_LOCATION_GROUP.cms_pool_id == poolId
                                           select av;
                }
                else
                {
                    var regionId = int.Parse(parameters[DictionaryParameter.Region]);
                    additionPlanMinMax = from av in additionPlanMinMax
                                           where av.LOCATION.OPS_AREA.ops_region_id == regionId
                                           select av;
                }
            }
            else if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.LocationCountry))
            {
                var locationCountry = parameters[DictionaryParameter.LocationCountry];
                additionPlanMinMax = from av in additionPlanMinMax
                                       where av.LOCATION.CMS_LOCATION_GROUP.CMS_POOL.country == locationCountry
                                       select av;
            }
            return additionPlanMinMax;
        }
    }
}