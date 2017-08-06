using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mars.App.Classes.Phase4Dal.Administration.MappingEntities
{
    public static class HeadingTranslator
    {
        private static Dictionary<string, string> GetLookupDictionary()
        {
            var lookup = new Dictionary<string, string>
                             {
                                 {"ID", "Id"}, 
                                 {"Country", "CountryCode"},
                                 {"Country Name", "CountryName"},
                                 {"Region", "RegionName"},
                                 {"Area", "AreaName"},
                                 {"Pool", "PoolName"},
                                 {"Location", "LocationCode"},
                                 {"Location Group", "LocationGroupName"},
                                 {"Active", "ActiveYesNo"},
                                 {"DW Code", "CountryDw"},
                                 {"Car Segment", "CarSegmentName"},
                                 {"Car Class", "CarClassName"},
                                 {"Car Group", "CarGroupName"},
                                 {"ADR", "AirportDowntownRailroad"},
                                 {"CAL", "CorporateAgencyLicencee"},
                                 {"Served By", "ServedBy"},
                                 {"Gold", "CarGroupGold"},
                                 {"Five Star", "CarGroupFiveStar"},
                                 {"President Circle", "CarGroupPresidentCircle"},
                                 {"Platinum", "CarGroupPlatinum"},
                                 {"Company Name", "CompanyName"},
                             };
            return lookup;
        }

        public static string TranslateHeader(string headerText)
        {
            var lookup = GetLookupDictionary();
            return lookup[headerText];
        }

        public static string TranslateReverse(string interfaceText)
        {
            var lookup = GetLookupDictionary();
            var returned = lookup.FirstOrDefault(d=> d.Value == interfaceText).Key;
            return returned;
        }
    }
}