using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.DAL.VehiclesAbroad.Abstract {
    public interface ICountryDescriptionRepository {
        string getCountryDescription(string countryCode);
    }
}