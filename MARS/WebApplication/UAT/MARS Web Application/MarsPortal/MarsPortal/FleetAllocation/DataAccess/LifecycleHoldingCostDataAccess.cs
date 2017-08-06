using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
                select new LifecycleHoldingCostRow(lhc.MonthDate)
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

        public List<DataContext.LifecycleHoldingCost> MatchUploadToDatabaseEntities(List<LifecycleHoldingCostRow> uploadedData)
        {
            var returned = new List<DataContext.LifecycleHoldingCost>();
            foreach (var ud in uploadedData)
            {
                var stringCarGroup = ud.CarGroup;
                var carGroup =
                    DataContext.CAR_GROUPs.FirstOrDefault(
                        d => d.car_group1 == stringCarGroup && d.CAR_CLASS.CAR_SEGMENT.country == "GE");

                if (carGroup == null) continue;
                var newDbEnity = new DataContext.LifecycleHoldingCost
                {
                    MonthDate = ud.GetMonthDate(),
                    Month = (byte)ud.GetMonthDate().Month,
                    Year = (short)ud.GetMonthDate().Year,
                    CarGroupId = carGroup.car_group_id,
                    Cost = ud.Cost
                };
                returned.Add(newDbEnity);
            }

            return returned;
        }

        public List<string> GetMissingHoldingCostCarGroups(int countryId)
        {
            var maxDate = DataContext.RevenueByCommercialCarSegments.Max(d => d.MonthDate);


            var minDate = maxDate.AddMonths(-3);

            var expectedCarGroupIdsFromRevenue = from rev in DataContext.RevenueByCommercialCarSegments
                                                where rev.MonthDate >= minDate
                                                    && rev.CAR_GROUP.CAR_CLASS.CAR_SEGMENT.COUNTRy1.CountryId == countryId
                                                select rev.CarGroupId;

            var presentHoldingCosts = from lhc in DataContext.LifecycleHoldingCosts
                                    where lhc.MonthDate >= minDate
                                          && lhc.CAR_GROUP.CAR_CLASS.CAR_SEGMENT.COUNTRy1.CountryId == countryId
                                    select lhc.CarGroupId;

            presentHoldingCosts = presentHoldingCosts.Distinct();

            var missingCarGroups = from eCg in presentHoldingCosts 
                                    join cg in DataContext.CAR_GROUPs on eCg equals cg.car_group_id
                                   where !expectedCarGroupIdsFromRevenue.Contains(eCg)
                                   select cg.car_group1;

            var returned = missingCarGroups.Distinct().ToList();
            return returned;


            /*
         
                         var maxDate = DataContext.RevenueByCommercialCarSegments.Max(d => d.MonthDate);


                var minDate = maxDate.AddMonths(-3);

                var expectedCarGroupIdsFromRevenue = from rev in DataContext.RevenueByCommercialCarSegments
                    where rev.MonthDate >= minDate
                    select rev.CarGroupId;

                var presentHoldingCosts = from lhc in DataContext.LifecycleHoldingCosts
                                        where lhc.MonthDate >= minDate
                                              && lhc.CAR_GROUP.CAR_CLASS.CAR_SEGMENT.COUNTRy1.CountryId == countryId
                                        select lhc.CarGroupId;

                presentHoldingCosts = presentHoldingCosts.Distinct();

                var missingCarGroups = from eCg in presentHoldingCosts 
                                        join cg in DataContext.CAR_GROUPs on eCg equals cg.car_group_id
                                       where !expectedCarGroupIdsFromRevenue.Contains(eCg)
                                       select cg.car_group1;

                var returned = missingCarGroups.Distinct().ToList();
                return returned;
         
             */
        }

        public void UploadDatabaseEntities(List<DataContext.LifecycleHoldingCost> entitiesToUpload)
        {
            var minDate = entitiesToUpload.Min(d => d.MonthDate);
            var entitiesToDelete = DataContext.LifecycleHoldingCosts.Where(d => d.MonthDate >= minDate);
            DataContext.LifecycleHoldingCosts.DeleteAllOnSubmit(entitiesToDelete);
            DataContext.SubmitChanges();

            DataContext.LifecycleHoldingCosts.InsertAllOnSubmit(entitiesToUpload);
            DataContext.SubmitChanges();
        }
    }
}