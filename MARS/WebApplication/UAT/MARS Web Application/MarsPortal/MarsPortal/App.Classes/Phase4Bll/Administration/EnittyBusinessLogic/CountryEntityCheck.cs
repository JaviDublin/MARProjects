using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.App.Classes.DAL.MarsDBContext;

namespace Mars.App.Classes.Phase4Bll.Administration.EnittyBusinessLogic
{
    internal static class CountryEntityCheck
    {
        internal const string DwCodeAlreadyExists = "This Dw Code already exists";
        internal const string CountryCodeAlreadyExists = "This Country Code already exists";

        internal static bool DoesCountryCodeAlreadyExist(MarsDBDataContext dataContext, string countryCode)
        {
            var returned = dataContext.COUNTRies.Any(d => d.country1 == countryCode);
            return returned;
        }

        internal static bool DoesCountryDwAlreadyExist(MarsDBDataContext dataContext, string countryDw, int countryId = 0)
        {
            var existingCountryWithDw = dataContext.COUNTRies.FirstOrDefault(d => d.country_dw == countryDw);
            if(countryId == 0)
            {
                if (existingCountryWithDw != null)
                {
                    return true;        //Another Country alraedy has this DW code
                } 
            }
            else
            {
                var countryDbEntry = dataContext.COUNTRies.Single(d => d.CountryId == countryId);
                if (countryDbEntry.country_dw != countryDw)
                {
                    if (existingCountryWithDw != null)
                    {
                        return true;    //Another Country alraedy has this DW code
                    }
                }
                return false;           //No Update needed
            }
            return false;
        }
        
    }
}