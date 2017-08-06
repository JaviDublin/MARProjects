using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.App.Classes.Phase4Dal.Enumerators;

namespace Mars.App.Classes.Phase4Bll.ForeignVehicles
{
    public static class PredicamentTranslation
    {
        public static string GetAvailabilityTopicShortDescription(ForeignVehiclePredicament predicament)
        {
            switch (predicament)
            {
                case ForeignVehiclePredicament.All:
                    return "All";
                case ForeignVehiclePredicament.OnRentOwningCountryToForeign:
                    return "On Rent - Owning Country To Foreign Country";
                case ForeignVehiclePredicament.TransferOwningCountryToForeign:
                    return "Transfer - Owning Country To Foreign Country";
                case ForeignVehiclePredicament.IdleInForeignCountry:
                    return "Idle - In Foreign Country";
                case ForeignVehiclePredicament.OnRentInOrBetweenForeignCountries:
                    return "On Rent - In or Between Foreign Countries";
                case ForeignVehiclePredicament.TransferInOrBetweenForeignCountries:
                    return "Transfer - In or Between Foreign Countries";
                case ForeignVehiclePredicament.OnRentReturningToOwningCountry:
                    return "On Rent - Returning To Owning Country";
                case ForeignVehiclePredicament.TransferReturningToOwningCountry:
                    return "Transfer - Returning To Owning Country";
                default:
                    throw new ArgumentOutOfRangeException("predicament");
            }
        }
    }
}