using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.App.Classes.BLL.ExtensionMethods;
using Mars.App.Classes.Phase4Dal.Enumerators;
using Mars.FleetAllocation.DataContext;

namespace Mars.FleetAllocation.DataAccess.ParameterFiltering
{
    public static class CarSegmentQueryable
    {
        public static IQueryable<CAR_SEGMENT> GetCarSegments(FaoDataContext dataContext
            , Dictionary<DictionaryParameter, string> parameters)
        {
            var carSegments = from cs in dataContext.CAR_SEGMENTs
                select cs;

            if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CarSegment))
            {
                var carSegmentId = int.Parse(parameters[DictionaryParameter.CarSegment]);
                carSegments = carSegments.Where(d => d.car_segment_id == carSegmentId);
            }
            else if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.OwningCountry))
            {
                var owningCountry = parameters[DictionaryParameter.OwningCountry];
                carSegments = carSegments.Where(d => d.country == owningCountry);
            }

            return carSegments;
        }
    }
}