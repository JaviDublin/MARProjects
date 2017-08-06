using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.App.Classes.BLL.ExtensionMethods;
using Mars.App.Classes.Phase4Dal.Enumerators;
using Mars.FleetAllocation.DataContext;

namespace Mars.FleetAllocation.DataAccess.ParameterFiltering
{
    public static class CarGroupQueryable
    {
        public static IQueryable<CAR_GROUP> GetCarGroups(FaoDataContext dataContext
            , Dictionary<DictionaryParameter, string> parameters)
        {

            var carGroups = from cg in dataContext.CAR_GROUPs
                select cg;

            if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CarGroup))
            {
                var carGroupString = parameters[DictionaryParameter.CarGroup];

                if (!carGroupString.Contains(LocationQueryable.Separator))
                {
                    var carGroupId = int.Parse(carGroupString);
                    carGroups = carGroups.Where(d => d.car_group_id == carGroupId);
                }
                else
                {
                    var splitCarGroupIds = carGroupString.Split(LocationQueryable.Separator.ToCharArray()).Select(int.Parse);
                    carGroups = from cg in carGroups
                                where splitCarGroupIds.Contains(cg.car_group_id)
                                select cg;
                }
            }
            else if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CarClass))
            {
                var carClassString = parameters[DictionaryParameter.CarClass];

                if (!carClassString.Contains(LocationQueryable.Separator))
                {
                    var carClassId = int.Parse(carClassString);
                    carGroups = carGroups.Where(d => d.car_class_id == carClassId);
                }
                else
                {
                    var splitCarClassIds = carClassString.Split(LocationQueryable.Separator.ToCharArray()).Select(int.Parse);
                    carGroups = from cg in carGroups
                                where splitCarClassIds.Contains(cg.car_class_id)
                                select cg;
                }
            }
            else if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CarSegment))
            {
                var carSegmentString = parameters[DictionaryParameter.CarSegment];

                if (!carSegmentString.Contains(LocationQueryable.Separator))
                {
                    var carSegmentId = int.Parse(carSegmentString);
                    carGroups = carGroups.Where(d => d.CAR_CLASS.car_segment_id == carSegmentId);
                }
                else
                {
                    var splitCarSegmentIds = carSegmentString.Split(LocationQueryable.Separator.ToCharArray()).Select(int.Parse);
                    carGroups = from cg in carGroups
                                where splitCarSegmentIds.Contains(cg.CAR_CLASS.car_segment_id)
                                select cg;
                }
            }
            else if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.OwningCountry))
            {
                var owningCountryString = parameters[DictionaryParameter.OwningCountry];

                if (!owningCountryString.Contains(LocationQueryable.Separator))
                {
                    var carOwningCountryId = owningCountryString;
                    carGroups = carGroups.Where(d => d.CAR_CLASS.CAR_SEGMENT.country == carOwningCountryId);
                }
                else
                {
                    var splitOwningCountryIds = owningCountryString.Split(LocationQueryable.Separator.ToCharArray());
                    carGroups = from cg in carGroups
                                where splitOwningCountryIds.Contains(cg.CAR_CLASS.CAR_SEGMENT.country)
                                select cg;
                }

            }

            return carGroups;
        }
    }
}