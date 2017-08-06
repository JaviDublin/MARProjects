using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.App.Classes.DAL.MarsDBContext;

namespace Mars.App.Classes.Phase4Bll.Administration.EnittyBusinessLogic
{
    public static class CarSegmentEntityCheck
    {
        internal const string SegmentAlreadyExistsForCountry = "A Car Segment with this name already exists in this Country";

        internal static bool DoesSegmentExistForCountry(MarsDBDataContext dataContext, string segmentName, int countryId
                                                        , int segmentId = 0)
        {
            var returned = dataContext.CAR_SEGMENTs.Any(d => d.COUNTRy1.CountryId == countryId 
                && d.car_segment1 == segmentName
                && ((d.car_segment_id != segmentId) || segmentId == 0));
            return returned;
        }
    }
}