using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.App.Classes.BLL.ExtensionMethods;
using Mars.App.Classes.Phase4Dal.Enumerators;
using Mars.FleetAllocation.DataAccess.Entities;

namespace Mars.FleetAllocation.DataAccess
{
    public class LifecycleHoldingCostDataAccess : BaseDataAccess
    {
        public LifecycleHoldingCostDataAccess(Dictionary<DictionaryParameter, string> parameters)
        {
            Parameters = parameters;
        }

        public List<LifecycleHoldingCostRow> GetLifecycleHoldingCosts()
        {

            if (!Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.StartDate))
            {
                return null;
            }
            var monthSelected = DateTime.Parse(Parameters[DictionaryParameter.StartDate]);

            int month = monthSelected.Month;
            int year = monthSelected.Year;

            var holdingCosts = from lhc in DataContext.LifecycleHoldingCosts
                               where lhc.Year == year && lhc.Month == month
                               select lhc;


            if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CarGroup))
            {
                var carGroupId = int.Parse(Parameters[DictionaryParameter.CarGroup]);
                holdingCosts = holdingCosts.Where(d => d.CarGroupId == carGroupId);
            }
            else if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CarClass))
            {
                var carClassId = int.Parse(Parameters[DictionaryParameter.CarClass]);
                holdingCosts = holdingCosts.Where(d => d.CAR_GROUP.car_class_id == carClassId);
            }
            else if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CarSegment))
            {
                var carSegmentId = int.Parse(Parameters[DictionaryParameter.CarSegment]);
                holdingCosts = holdingCosts.Where(d => d.CAR_GROUP.CAR_CLASS.car_segment_id == carSegmentId);
            }
            else if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.OwningCountry))
            {
                var country = Parameters[DictionaryParameter.OwningCountry];
                holdingCosts = holdingCosts.Where(d => d.CAR_GROUP.CAR_CLASS.CAR_SEGMENT.country == country);
            }

            var holdingCostRows = from lhc in holdingCosts
                select new LifecycleHoldingCostRow(lhc.Month, lhc.Year)
                       {
                           OwningCountry = lhc.CAR_GROUP.CAR_CLASS.CAR_SEGMENT.COUNTRy1.country_description,
                           CarSegment = lhc.CAR_GROUP.CAR_CLASS.CAR_SEGMENT.car_segment1,
                           CarClass = lhc.CAR_GROUP.CAR_CLASS.car_class1,
                           CarGroup = lhc.CAR_GROUP.car_group1,
                           Cost = lhc.Cost
                       };

            var returned = holdingCostRows.ToList();
            return returned;
        }
    }
}