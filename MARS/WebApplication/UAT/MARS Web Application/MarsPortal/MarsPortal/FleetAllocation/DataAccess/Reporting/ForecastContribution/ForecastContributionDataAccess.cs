using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Castle.Components.DictionaryAdapter.Xml;
using Mars.App.Classes.BLL.ExtensionMethods;
using Mars.App.Classes.Phase4Dal.Enumerators;
using Mars.FleetAllocation.DataAccess.AdditionPlanDataAccess;
using Mars.FleetAllocation.DataAccess.Contribution;
using Mars.FleetAllocation.DataAccess.Forecast;
using Mars.FleetAllocation.DataAccess.Forecast.Entities;
using Mars.FleetAllocation.DataAccess.ParameterFiltering;
using Mars.FleetAllocation.DataAccess.Reporting.ForecastContribution.Entities;

namespace Mars.FleetAllocation.DataAccess.Reporting.ForecastContribution
{
    public class ForecastContributionDataAccess : BaseDataAccess
    {
        public ForecastContributionDataAccess(Dictionary<DictionaryParameter, string> parameters) : base(parameters)
        {
            
        }

        private IQueryable<ExpectedFleetHolder> GetExpectedFLeet()
        {
            var country = Parameters[DictionaryParameter.OwningCountry];
            var dbAdditionEntries = from ad in DataContext.ResAdditions
                                    where ad.CAR_GROUP.CAR_CLASS.CAR_SEGMENT.country == country
                                    select ad;

            var dbDeletionEntries = from del in DataContext.ResDeletions
                                    where del.CAR_GROUP.CAR_CLASS.CAR_SEGMENT.country == country
                                    select del;

            if (Parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CarSegment))
            {
                var segmentId = int.Parse(Parameters[DictionaryParameter.CarSegment]);

                dbAdditionEntries = dbAdditionEntries.Where(d => d.CAR_GROUP.CAR_CLASS.car_segment_id == segmentId);
                dbDeletionEntries = dbDeletionEntries.Where(d => d.CAR_GROUP.CAR_CLASS.car_segment_id == segmentId);
            }



            var additions = from ad in dbAdditionEntries
                            group ad by new { ad.LOCATION.cms_location_group_id, ad.CarGrpId, ad.RepDate }
                                into groupedData
                                select new
                                {
                                    LocationGroupId = groupedData.Key.cms_location_group_id,
                                    CarGroupId = groupedData.Key.CarGrpId,
                                    ReportDate = groupedData.Key.RepDate,
                                    Additions = groupedData.Sum(d => d.Value)
                                };

            var deletions = from del in dbDeletionEntries

                            group del by new { del.LOCATION.cms_location_group_id, del.CarGrpId, del.RepDate }
                                into groupedData
                                select new
                                {
                                    LocationGroupId = groupedData.Key.cms_location_group_id,
                                    CarGroupId = groupedData.Key.CarGrpId,
                                    ReportDate = groupedData.Key.RepDate,
                                    Deletions = groupedData.Sum(d => d.Value)
                                };

            var forecast = ForecastQueryable.GetForecast(DataContext, Parameters);
            var monthlyAverageExpected = from fc in forecast
                                         join cmsToLoc in DataContext.CmsToLocationLevelPercents on
                                             new { CarGroupId = fc.CAR_CLASS_ID, LocationGroupId = fc.CMS_LOCATION_GROUP_ID }
                                             equals new { cmsToLoc.CarGroupId, cmsToLoc.LocationGroupId }
                                         group new { fc, cmsToLoc } by new
                                         {
                                             CarGroupId = fc.CAR_CLASS_ID,
                                             cmsToLoc.LocationId
                                            ,
                                             fc.REP_DATE.Month,
                                             fc.REP_DATE.Year
                                         }
                                             into groupedData
                                             select new ExpectedFleetHolder
                                             {
                                                 Year = (short)groupedData.Key.Year,
                                                 Month = (byte)groupedData.Key.Month,
                                                 CarGroupId = groupedData.Key.CarGroupId,
                                                 LocationId = groupedData.Key.LocationId,
                                                 Expected = groupedData.Average(x =>
                                                 ((x.fc.OPERATIONAL_FLEET.HasValue ? x.fc.OPERATIONAL_FLEET.Value : 0)
                                                      + (additions.Where(d => d.ReportDate <= x.fc.REP_DATE
                                                                          && d.LocationGroupId == x.fc.CMS_LOCATION_GROUP_ID
                                                                          && d.CarGroupId == x.fc.CAR_CLASS_ID
                                                          ).Sum(d => (int?)d.Additions) ?? 0)
                                                      - (deletions.Where(d => d.ReportDate <= x.fc.REP_DATE
                                                                          && d.LocationGroupId == x.fc.CMS_LOCATION_GROUP_ID
                                                                          && d.CarGroupId == x.fc.CAR_CLASS_ID
                                                          ).Sum(d => (int?)d.Deletions) ?? 0)
                                                          )
                                                          ),

                                             };
            return monthlyAverageExpected;
        }

        public IQueryable<AdttionPlanHolder> GetQuerysFromPlan(int queryPlanId)
        {
            var additionData = AdditionPlanEntryFilter.GetAdditionPlanEntries(DataContext, Parameters);
            var additionPlanEntries = from apd in additionData
                                       join wtm in DataContext.IsoWeekToMonths on 
                                                    new {apd.Week, apd.Year} equals new 
                                                        {Week = wtm.IsoWeekNumber, wtm.Year}
                                      where apd.AdditionPlanId == queryPlanId
                                       group apd by new { wtm.Month, apd.Year, apd.CarGroupId, apd.LocationId }
                                           into groupedData
                                           select new AdttionPlanHolder
                                           {
                                               Year = groupedData.Key.Year,
                                               Month = groupedData.Key.Month,
                                               CarGroupId = groupedData.Key.CarGroupId,
                                               LocationId = groupedData.Key.LocationId,
                                               Additions = groupedData.Sum(d => d.Additions),
                                           };
            return additionPlanEntries;
        }

        public List<ForecastContributionRow> GetForecastContribution(int additionPlanA, int additionPlanB)
        {
            var maxDate = DataContext.RevenueByCommercialCarSegments.Max(d => d.MonthDate);
            var minDate = maxDate.AddMonths(-3);

            var monthlyAverageExpected = GetExpectedFLeet();

            var groupedAdditionAData = GetQuerysFromPlan(additionPlanA);


            var groupedAdditionBData = GetQuerysFromPlan(additionPlanB);


            var contributionData = ContributionQueryable.GetContribution(DataContext, minDate);

            var fleetHistory = FleetHistoryQueryable.GetAvailabilityHistory(DataContext, Parameters);

            var sumFleet = from fh in fleetHistory
                           group fh by
                           new { fh.CarGroupId, fh.LocationId, fh.Timestamp }
                               into gd
                               select new
                               {
                                   gd.Key.CarGroupId,
                                   gd.Key.LocationId,
                                   TotalFleet = gd.Sum(d => d.MaxTotal)
                               };

            var totalFleet = from fh in sumFleet
                             group fh by
                             new { fh.CarGroupId, fh.LocationId }
                                 into gd
                                 select new
                                 {
                                     gd.Key.CarGroupId,
                                     gd.Key.LocationId,
                                     TotalFleet = gd.Average(d => d.TotalFleet)
                                 };



            var forecastData = from wae in monthlyAverageExpected
                            join tf in totalFleet on new {wae.CarGroupId, wae.LocationId}
                                                equals new { tf.CarGroupId, tf.LocationId }
                            join cntr in contributionData on new {wae.CarGroupId, wae.LocationId }
                                              equals new { cntr.CarGroupId, cntr.LocationId }
                              into jContri
                            from joinedContri in jContri.DefaultIfEmpty()
                            join cg in DataContext.CAR_GROUPs on wae.CarGroupId equals cg.car_group_id
                            join loc in DataContext.LOCATIONs on wae.LocationId equals loc.dim_Location_id
                            orderby wae.Year, wae.Month
                            select new ForecastContributionRow
                            {
                                Year = wae.Year,
                                Month = wae.Month,
                                CarGroup = cg.car_group1,
                                Location = loc.location1,
                                CpU = (((joinedContri == null ? 0 : joinedContri.Revenue) / tf.TotalFleet) 
                                                    - (joinedContri == null ? 0 : joinedContri.HoldingCost)),
                                Expected = (double) wae.Expected,
                                CumulativeAdditionsA = groupedAdditionAData.Where(d => (d.Year < wae.Year
                                                                                || (d.Year == wae.Year && d.Month <= wae.Month))
                                                                                && d.CarGroupId == wae.CarGroupId
                                                                                && d.LocationId == wae.LocationId
                                                                            ).Sum(d => (int?) d.Additions) ?? 0,
                                CumulativeAdditionsB = groupedAdditionBData.Where(d => (d.Year < wae.Year
                                                                                || (d.Year == wae.Year && d.Month <= wae.Month))
                                                                                && d.CarGroupId == wae.CarGroupId
                                                                                && d.LocationId == wae.LocationId
                                                ).Sum(d => (int?) d.Additions) ?? 0
                            };

            var returned = forecastData.ToList();
            return returned;

        }
    }
}