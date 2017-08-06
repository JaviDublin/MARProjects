using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mars.App.Classes.Phase4Dal.Enumerators
{
    public enum ForeignVehiclePredicament
    {
        All,
        IdleInForeignCountry,
        OnRentOwningCountryToForeign,
        TransferOwningCountryToForeign,
        OnRentInOrBetweenForeignCountries,
        TransferInOrBetweenForeignCountries,
        OnRentReturningToOwningCountry,
        TransferReturningToOwningCountry,

    }
}