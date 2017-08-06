using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.App.Classes.BLL.ExtensionMethods;
using Mars.App.Classes.Phase4Dal;
using Mars.App.Classes.Phase4Dal.Enumerators;
using Mars.FleetAllocation.DataContext;

namespace Mars.FleetAllocation.DataAccess.ParameterFiltering
{
    public static class CommercialSegmentQueryable
    {
        public static IQueryable<CommercialCarSegment> GetCommercialCarSemgents(FaoDataContext dataContext
                        , Dictionary<DictionaryParameter, string> parameters)
        {
            var commCarSeg = from ccs in dataContext.CommercialCarSegments
                select ccs;

            if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CommercialCarSegment))
            {
                var segementsRequested = parameters[DictionaryParameter.CommercialCarSegment];
                var splitSegments = segementsRequested.Split(VehicleFieldRestrictions.Separator.ToCharArray()).Select(int.Parse);

                
                commCarSeg = commCarSeg.Where(d => splitSegments.Contains(d.CommercialCarSegmentId));
            }

            return commCarSeg;
        }
    }
}