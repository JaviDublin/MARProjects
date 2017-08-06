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
    public class MaxFleetFactorQueryable
    {
        public static IQueryable<MaxFleetFactor> GetCommercialCarSemgents(FaoDataContext dataContext
                        , Dictionary<DictionaryParameter, string> parameters)
        {
            var maxFleetFactors = from mff in dataContext.MaxFleetFactors
                             select mff;

            if (parameters.ContainsKey(DictionaryParameter.DayOfWeek) &&
                    parameters[DictionaryParameter.DayOfWeek] != string.Empty)
            {
                var dayOfWeek = int.Parse(parameters[DictionaryParameter.DayOfWeek]);
                maxFleetFactors = maxFleetFactors.Where(d => d.DayOfWeekId == dayOfWeek);
            }


            return maxFleetFactors;
        }
    }
}