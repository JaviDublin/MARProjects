using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.App.Classes.DAL.MarsDBContext;

namespace Mars.App.Classes.Phase4Bll.Administration.EnittyBusinessLogic
{
    public static class LocationEntityCheck
    {
        internal const string ServedByLocationDoesntExist = "The served by location entered does not exist on the database";
        internal const string ServedByLocationNotInCountry = "The served by location entered is not in the same country as the selected location";

        internal static bool DoesServedByLocationExist(MarsDBDataContext dataContext, string servedByLocationCode)
        {
            var returned = dataContext.LOCATIONs.Any(d => d.location1 == servedByLocationCode);
            return returned;
        }

        internal static bool IsServedByLocationInSameCountry(MarsDBDataContext dataContext, int selectedLocationId, string servedByLocation)
        {
            var selectedLocationCountry = dataContext.LOCATIONs.Single(d => d.dim_Location_id == selectedLocationId).country;
            var returned = dataContext.LOCATIONs.Any(d => d.location1 == servedByLocation
                                                && d.country == selectedLocationCountry);
            return returned;
        }
    }
}