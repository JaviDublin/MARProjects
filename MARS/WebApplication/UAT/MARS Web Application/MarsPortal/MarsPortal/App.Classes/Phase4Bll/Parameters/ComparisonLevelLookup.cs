using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.App.Classes.Phase4Dal.Enumerators;
using Mars.App.Classes.BLL.ExtensionMethods;

namespace Mars.App.Classes.Phase4Bll.Parameters
{
    public static class ComparisonLevelLookup
    {
        public static DictionaryParameter GetSiteComparisonTypeFromParameters(Dictionary<DictionaryParameter, string> parameters)
        {
            if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.Location)) return DictionaryParameter.Location;

            if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.LocationGroup)) return DictionaryParameter.Location;
            if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.Area)) return DictionaryParameter.Location;

            if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.Pool)) return DictionaryParameter.LocationGroup;
            if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.Region)) return DictionaryParameter.Area;

            if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.LocationCountry))

            {
                if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CmsSelected)
                    && parameters[DictionaryParameter.CmsSelected] == true.ToString())
                {
                    return DictionaryParameter.Pool;    
                }
                else
                {
                    return DictionaryParameter.Region;
                }
                
            }

            return DictionaryParameter.LocationCountry;
        }

        public static DictionaryParameter GetFleetComparisonTypeFromParameters(Dictionary<DictionaryParameter, string> parameters)
        {
            if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CarGroup)) return DictionaryParameter.CarGroup;

            if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CarClass)) return DictionaryParameter.CarGroup;

            if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CarSegment)) return DictionaryParameter.CarClass;

            if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.OwningCountry)) return DictionaryParameter.CarSegment;

            return DictionaryParameter.OwningCountry;
        }
    }
}