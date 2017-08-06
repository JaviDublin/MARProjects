using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.WebControls.Expressions;
using Castle.Components.DictionaryAdapter.Xml;
using Mars.App.Classes.BLL.ExtensionMethods;
using Mars.App.Classes.Phase4Dal.Enumerators;
using Mars.FleetAllocation.DataAccess.AdditionPlanDataAccess;
using Mars.FleetAllocation.DataAccess.Entities;
using Mars.FleetAllocation.DataAccess.Forecast;
using Mars.FleetAllocation.DataAccess.Forecast.Entities;
using Mars.FleetAllocation.DataAccess.Reporting.ForecastedFleetSize.Entities;

namespace Mars.FleetAllocation.DataAccess.Reporting.ForecastedFleetSize
{
    public class ForecastedFleetSizeDataAccess : BaseDataAccess
    {
        public ForecastedFleetSizeDataAccess(Dictionary<DictionaryParameter, string> parameters) : base(parameters)
        {
            
        }

        public IQueryable<ForecastHolder> GetForecast()
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
            var futureTrend = from fc in forecast
                              join nf in DataContext.MARS_CMS_NECESSARY_FLEETs on
                                  new { fc.CAR_CLASS_ID, fc.CMS_LOCATION_GROUP_ID }
                                  equals new { nf.CAR_CLASS_ID, nf.CMS_LOCATION_GROUP_ID }
                              select new ForecastHolder
                              {
                                  ReportDate = fc.REP_DATE,
                                  CarGroupId = fc.CAR_CLASS_ID,
                                  LocationGroupId = fc.CMS_LOCATION_GROUP_ID,
                                  UnConstrained = (double)fc.UNCONSTRAINED.Value,
                                  Nessesary = nf.UTILISATION == 100 || nf.NONREV_FLEET == 100 ? 0
                                           : (double)((fc.UNCONSTRAINED.Value / (nf.UTILISATION / 100) / (1 - (nf.NONREV_FLEET / 100))) ?? 0),
                                  Expected = ((fc.OPERATIONAL_FLEET.HasValue ? fc.OPERATIONAL_FLEET.Value : 0)
                                       + (additions.Where(d => d.ReportDate <= fc.REP_DATE
                                                           && d.LocationGroupId == fc.CMS_LOCATION_GROUP_ID
                                                           && d.CarGroupId == fc.CAR_CLASS_ID
                                           ).Sum(d => (int?)d.Additions) ?? 0)
                                       - (deletions.Where(d => d.ReportDate <= fc.REP_DATE
                                                           && d.LocationGroupId == fc.CMS_LOCATION_GROUP_ID
                                                           && d.CarGroupId == fc.CAR_CLASS_ID
                                           ).Sum(d => (int?)d.Deletions) ?? 0)
                                           ),
                              };
            return futureTrend;

        }

        public List<SummedByWeekForecastHolder> GetWeeklyForecast()
        {
            var futureTrend = GetForecast();

            var maxOfWeek = from fc in futureTrend
                            join woy in DataContext.IsoWeekOfYears on fc.ReportDate equals woy.Day
                            group new { fc, woy } by new { fc.ReportDate.Year, woy.WeekOfYear, fc.LocationGroupId, fc.CarGroupId }
                                into groupedData
                                select new
                                {
                                    Year = (short)groupedData.Key.Year,
                                    Week = groupedData.Key.WeekOfYear,
                                    ExpectedFleet = groupedData.Max(d => d.fc.Expected.HasValue ? d.fc.Expected.Value : 0),
                                    NessesaryFleet = groupedData.Max(d => d.fc.Nessesary),
                                    Unconstrained = groupedData.Max(d => d.fc.UnConstrained),
                                };

            var summedMaxes = from fc in maxOfWeek
                              group fc by new { fc.Year, fc.Week }
                                  into groupedData
                                  select new SummedByWeekForecastHolder
                                  {
                                      Year = groupedData.Key.Year,
                                      Week = groupedData.Key.Week,
                                      ExpectedFleet = groupedData.Sum(d => d.ExpectedFleet),
                                      NessesaryFleet = groupedData.Sum(d => d.NessesaryFleet),
                                      Unconstrained = groupedData.Sum(d => d.Unconstrained),
                                  };

            var returned = summedMaxes.ToList();
            return returned;
        }

        public List<ForecastedFleetSizeEntity> AttachAdditionPlans(List<SummedByWeekForecastHolder> forecast
                                                                    , int additionPlanId)
        {
            var minMaxData = AdditionPlanMinMaxValueFilter.GetAdditionPlanMinMaxValues(DataContext, Parameters);
            var additionData = AdditionPlanEntryFilter.GetAdditionPlanEntries(DataContext, Parameters);

            var groupedMinMaxData = from apd in minMaxData
                                    where apd.AdditionPlanId == additionPlanId
                                    group apd by new { apd.Week, apd.Year }
                                        into groupedData
                                        select new
                                        {
                                            groupedData.Key.Year,
                                            groupedData.Key.Week,
                                            Max = groupedData.Sum(d => d.MaxFleet),
                                            Min = groupedData.Sum(d => d.MinFleet),
                                        };

            var groupedAdditionData = from apd in additionData
                                      where apd.AdditionPlanId == additionPlanId
                                      group apd by new { apd.Week, apd.Year }
                                          into groupedData
                                          select new
                                          {
                                              groupedData.Key.Year,
                                              groupedData.Key.Week,
                                              Additions = groupedData.Sum(d => d.Additions),
                                          };

            var localAdditionData = groupedAdditionData.ToList();

            var groupedForecast = from fe in forecast
                                  join mmd in groupedMinMaxData on
                                            new { fe.Year, fe.Week }
                                        equals new { mmd.Year, mmd.Week }
                                   into joinedMmd
                                  from jMmd in joinedMmd.DefaultIfEmpty()
                                  orderby fe.Year, fe.Week
                                  select new ForecastedFleetSizeEntity
                                  {
                                      Year = fe.Year,
                                      Week = fe.Week,
                                      ExpectedFleet = fe.ExpectedFleet,
                                      Nessesary = fe.NessesaryFleet,
                                      UnConstrained = fe.Unconstrained,
                                      ExpectedWithAdditionPlan = fe.ExpectedFleet
                                            + localAdditionData.Where(d => d.Year < fe.Year || (d.Year == fe.Year && d.Week <= fe.Week)
                                                            ).Sum(d=> d.Additions),
                                      MaxFleet = jMmd == null ? 0 : jMmd.Max,
                                      MinFleet = jMmd == null ? 0 : jMmd.Min
                                  };

            var returned = groupedForecast.ToList();
            return returned;
        }
    }
}