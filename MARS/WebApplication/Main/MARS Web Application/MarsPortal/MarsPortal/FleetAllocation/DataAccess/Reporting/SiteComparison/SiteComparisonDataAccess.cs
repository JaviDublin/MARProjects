using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.App.Classes.BLL.ExtensionMethods;
using Mars.App.Classes.Phase4Dal.Enumerators;
using Mars.FleetAllocation.DataAccess.AdditionPlanDataAccess;
using Mars.FleetAllocation.DataAccess.Reporting.SiteComparison.Entities;

namespace Mars.FleetAllocation.DataAccess.Reporting.SiteComparison
{
    public class SiteComparisonDataAccess : BaseDataAccess
    {
        public SiteComparisonDataAccess(Dictionary<DictionaryParameter, string> parameters)
            : base(parameters)
        {

        }

        public List<SiteComparisonEntity> GetComparisonEntities(int additionPlanId, int weeksInFuture)
        {
            var additions = AdditionPlanEntryFilter.GetAdditionPlanEntries(DataContext, Parameters);


            var currentDate = DataContext.FleetHistories.Max(d => d.Timestamp);
            var futureTarget = currentDate.AddDays(weeksInFuture * 7);
            var targetWeek = DataContext.IsoWeekOfYears.First(d => d.Day == futureTarget).WeekOfYear;
            var targetYear = futureTarget.Year;


            additions = additions.Where(d => d.AdditionPlanId == additionPlanId
                            && ((d.Week <= targetWeek && d.Year == targetYear)
                              || d.Year < targetYear));


            IQueryable<SiteComparisonEntity> comparisonEntites;

            if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.Location)
                || Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.LocationGroup))
            {
                comparisonEntites = from mmd in additions
                    group mmd by new {LocGrouping = mmd.LOCATION.location1, mmd.Week, mmd.Year}
                    into groupedData
                    select new SiteComparisonEntity
                           {
                               Location = groupedData.Key.LocGrouping,
                               Year = groupedData.Key.Year,
                               Week = groupedData.Key.Week,
                               Additions = groupedData.Sum(d => d.Additions),
                           };
            }
            else if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.Pool))
            {
                comparisonEntites = from mmd in additions
                    group mmd by
                        new {LocGrouping = mmd.LOCATION.CMS_LOCATION_GROUP.cms_location_group1, mmd.Week, mmd.Year}
                    into groupedData
                    select new SiteComparisonEntity
                           {
                               Location = groupedData.Key.LocGrouping,
                               Year = groupedData.Key.Year,
                               Week = groupedData.Key.Week,
                               Additions = groupedData.Sum(d => d.Additions),
                           };
            }
            else if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.LocationCountry))
            {
                comparisonEntites = from mmd in additions
                    group mmd by
                        new {LocGrouping = mmd.LOCATION.CMS_LOCATION_GROUP.CMS_POOL.cms_pool1, mmd.Week, mmd.Year}
                    into groupedData
                    select new SiteComparisonEntity
                           {
                               Location = groupedData.Key.LocGrouping,
                               Year = groupedData.Key.Year,
                               Week = groupedData.Key.Week,
                               Additions = groupedData.Sum(d => d.Additions),
                           };
            }
            else
            {
                comparisonEntites = from mmd in additions
                    group mmd by
                        new
                        {
                            LocGrouping = mmd.LOCATION.CMS_LOCATION_GROUP.CMS_POOL.COUNTRy1.country_description,
                            mmd.Week,
                            mmd.Year
                        }
                    into groupedData
                    select new SiteComparisonEntity
                           {
                               Location = groupedData.Key.LocGrouping,
                               Year = groupedData.Key.Year,
                               Week = groupedData.Key.Week,
                               Additions = groupedData.Sum(d => d.Additions),
                           };

            }

            var returned = comparisonEntites.ToList();
            return returned;
        }
    }
}