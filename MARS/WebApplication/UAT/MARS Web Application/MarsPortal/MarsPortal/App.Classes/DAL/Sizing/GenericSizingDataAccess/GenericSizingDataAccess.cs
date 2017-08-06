using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using App.BLL.ReportEnums.FleetSize;
using Mars.App.Classes.DAL.MarsDBContext;
using App.DAL.MarsDataAccess.Forecasting.ForecastDataHolders;
using App.DAL.MarsDataAccess.ParameterAccess;
using App.DAL.MarsDataAccess.Sizing.FutureTrendDataHolders;
using App.DAL.MarsDataAccess.Sizing.KpiDataHolders;
using App.DAL.MarsDataAccess.Sizing.SiteAndFleetComparisonDataHolders;
using App.Entities.Graphing.Parameters;
using Mars.App.Classes.DAL.MarsDBContext;
using Mars.App.Classes.DAL.Sizing.GenericSizingHolders;


namespace Mars.App.Classes.DAL.Sizing.GenericSizingDataAccess
{

    /// <summary>
    /// This class is used for ALL Sizing data access, every chart and export to excel raw data should come from here
    /// </summary>
    internal static class GenericSizingDataAccess
    {
        private static IQueryable<MARS_CMS_FORECAST> GetRawSizingCmsData(MarsDBDataContext dc, Dictionary<string, string> parameters)
        {
            var fullCmsDataSet = from cfv in dc.MARS_CMS_FORECASTs
                                 select cfv;
            
            var restrictedCmsData = ForecastParameterRestriction.RestrictMarsCmsForecastByParameters(parameters, fullCmsDataSet, dc);
            
            return restrictedCmsData;
        }

        private static IQueryable<MARS_CMS_NECESSARY_FLEET> GetRawSizingNessFleetData(MarsDBDataContext dc, Dictionary<string, string> parameters)
        {
            var fullNessesaryFleetSet = from nf in dc.MARS_CMS_NECESSARY_FLEETs
                                        select nf;
            var restrictedNessesaryFleetData =
                ForecastParameterRestriction.RestrictMarsCmsNessesaryFleetByParameters(parameters, fullNessesaryFleetSet,
                                                                           dc);
            return restrictedNessesaryFleetData;

            
        }

        
        private static IQueryable<GenericSizingHolder> GetRawSizingCmsAndNessFleetData(MarsDBDataContext dc, Dictionary<string, string> parameters)
        {
            var restrictedCmsData = GetRawSizingCmsData(dc, parameters);
            var restrictedNessesaryFleetData = GetRawSizingNessFleetData(dc, parameters);

            var cmsData = from rd in restrictedCmsData
                          join mnf in restrictedNessesaryFleetData 
                             on new { rd.CAR_CLASS_ID, rd.CMS_LOCATION_GROUP_ID}
                             equals new {mnf.CAR_CLASS_ID , mnf.CMS_LOCATION_GROUP_ID }
                          into joinedData
                          from jd in joinedData.DefaultIfEmpty()
                          
                          select new GenericSizingHolder
                          {
                              Date = rd.REP_DATE,
                              Country = rd.COUNTRY,
                              LocationGroupId = rd.CMS_LOCATION_GROUP_ID,
                              CarClassId = rd.CAR_CLASS_ID,
                              Constrained = rd.CONSTRAINED ?? 0,
                              Unconstrained = rd.UNCONSTRAINED ?? 0,
                              AlreadyBooked = rd.RESERVATIONS_BOOKED ?? 0,
                              NessesaryConstrained = Math.Round((jd.UTILISATION == 0 ? 0 : jd.NONREV_FLEET == 100 ? 0 : 
                                            (rd.CONSTRAINED / jd.UTILISATION * 100) / ((100 - jd.NONREV_FLEET) / 100)) 
                                                        ?? rd.CONSTRAINED ?? 0, 0, MidpointRounding.AwayFromZero),
                              NessesaryUnconstrained = Math.Round((jd.UTILISATION == 0 ? 0 : jd.NONREV_FLEET == 100 ? 0 :
                                            (rd.UNCONSTRAINED / jd.UTILISATION * 100) / ((100 - jd.NONREV_FLEET) / 100))
                                                        ?? rd.UNCONSTRAINED ?? 0, 0, MidpointRounding.AwayFromZero),
                              NessesaryBooked = Math.Round((jd.UTILISATION == 0 ? 0 : jd.NONREV_FLEET == 100 ? 0 :
                                            (rd.RESERVATIONS_BOOKED / jd.UTILISATION * 100) / ((100 - jd.NONREV_FLEET) / 100))
                                                        ?? rd.RESERVATIONS_BOOKED ?? 0, 0, MidpointRounding.AwayFromZero),
                              ExpectedFleet = 0,
                          };
            
            return cmsData;
        }

        private static IQueryable<GenericSizingHolder> GetFleetSizeFutureTrendData(MarsDBDataContext dc, Dictionary<string, string> parameters, int fleetPlan = 1)
        {
            var fullFleetSizeDataSet = from fsft in dc.FleetSizeFutureTrends
                                       select fsft;

            var restrictedData = ForecastParameterRestriction.RestrictFutureTrendDataByParameters(parameters,
                                                                                                  fullFleetSizeDataSet,
                                                                                                  dc, fleetPlan);

            var futureTrendData = from ftd in restrictedData
                                  //join lg in dc.CMS_LOCATION_GROUPs on ftd.LocGrpId equals lg.cms_location_group_id //Comment to include differnt country Nessesary Fleet Calculations
                                  //join pools in dc.CMS_POOLs on lg.cms_pool_id equals pools.cms_pool_id
                                  //where pools.country == ftd.Country

                                  select new GenericSizingHolder
                                             {
                                                 Date = ftd.TargetDate,
                                                 Country = ftd.Country,
                                                 LocationGroupId = ftd.LocGrpId ,
                                                 CarClassId = ftd.CarGrpId ,
                                                 ExpectedFleet = ftd.ExpectedFleet,
                                             };
            return futureTrendData;
        }

        /// <summary>
        /// Raw Data Access for Forecast Report
        /// </summary>
        internal static IQueryable<ForecastRawDataHolder> GetForecastRawData(Dictionary<string, string> parameters, MarsDBDataContext dc)
        {
            var restrictedData = GetRawSizingCmsData(dc, parameters);

            var joinedData = from rd in restrictedData
                             join adj in dc.MARS_CMS_FORECAST_ADJUSTMENTs on
                                 new { rd.REP_DATE, CMS_LOCATION_GROUP_ID = rd.CMS_LOCATION_GROUP_ID, CAR_CLASS_ID = rd.CAR_CLASS_ID }
                                 equals new { adj.REP_DATE, adj.CMS_LOCATION_GROUP_ID, adj.CAR_CLASS_ID }
                                 into adj
                             from joinedAdjustment in adj.DefaultIfEmpty()
                             select new ForecastRawDataHolder
                             {
                                 ReportDate = rd.REP_DATE,
                                 LocationGroupId = rd.CMS_LOCATION_GROUP_ID,
                                 CarClassId = rd.CAR_CLASS_ID,
                                 OnRent = rd.CURRENT_ONRENT ?? 0,
                                 OnRentLy = rd.ONRENT_LY ?? 0,
                                 Constrained = rd.CONSTRAINED ?? 0,
                                 Unconstrained = rd.UNCONSTRAINED ?? 0,
                                 Fleet = rd.FLEET ?? 0,
                                 AlreadyBooked = rd.RESERVATIONS_BOOKED ?? 0,
                                 TopDown = joinedAdjustment.ADJUSTMENT_TD ?? 0,
                                 BottomUp1 = joinedAdjustment.ADJUSTMENT_BU1 ?? 0,
                                 BottomUp2 = joinedAdjustment.ADJUSTMENT_BU2 ?? 0,
                                 Reconciliation = joinedAdjustment.ADJUSTMENT_RC ?? 0
                             };
            return joinedData;
        }


        internal static IQueryable<MARS_CMS_FORECAST_HISTORY> GetBenchmarkRawData(IDictionary<string, string> parameters, MarsDBDataContext dataContext)
        {

            var fullDataSet = from fc in dataContext.MARS_CMS_FORECAST_HISTORies
                              select fc;

            var restrictedData = ForecastParameterRestriction.RestrictHistoricalForecastByParameters(parameters, fullDataSet, dataContext);

            return restrictedData;
        }

        /// <summary>
        /// Raw data access for Fleet and Site Comparisons
        /// </summary>
        internal static IQueryable<SiteAndFleetComparisonRawDataHolder> GetRawSiteAndFleetComparisonData(MarsDBDataContext dc, Dictionary<string, string> parameters,int fleetPlan = 1)
        {
            var cmsData = GetRawSizingCmsAndNessFleetData(dc, parameters);
            var fleetSizingData = GetFleetSizeFutureTrendData(dc, parameters, fleetPlan);

            var validLocationGroupIds = cmsData.Select(d => d.LocationGroupId).Distinct().ToList();


            //fleetSizingData = fleetSizingData.Where(d => validLocationGroupIds.Contains(d.LocationGroupId));

            //var totalJoinData = cmsData.Union(fleetSizingData);

            var finalData2 = from td in cmsData
                             join fsd in fleetSizingData on new { td.Date, td.Country, td.CarClassId, td.LocationGroupId }
                                                    equals new { fsd.Date, fsd.Country, fsd.CarClassId, fsd.LocationGroupId }
                            into fjd
                             from joinFleetData in fjd.DefaultIfEmpty()
                             select new SiteAndFleetComparisonRawDataHolder
                             {
                                 ReportDate = td.Date.Value,
                                    Country = td.Country,
                                    CarClassId = td.CarClassId,
                                    LocationGroupId = td.LocationGroupId,
                                    Constrained = td.Constrained ?? 0,
                                    Unconstrained = td.Unconstrained ?? 0,
                                    Booked = td.AlreadyBooked ?? 0,
                                 ExpectedFleet = joinFleetData.ExpectedFleet ?? 0, 
                             };


            //var finalData = from td in totalJoinData
            //                select new SiteAndFleetComparisonRawDataHolder
            //                {
            //                    ReportDate = td.Date.Value,
            //                    Country = td.Country,
            //                    CarClassId = td.CarClassId,
            //                    LocationGroupId = td.LocationGroupId,
            //                    Constrained = td.Constrained ?? 0,
            //                    Unconstrained = td.Unconstrained ?? 0,
            //                    Booked = td.AlreadyBooked ?? 0,
            //                    ExpectedFleet = td.ExpectedFleet ?? 0,                                
            //                };

            return finalData2;
        }


        /// <summary>
        /// Raw Data used for Future Trend or Suuply Analysis Export to Excel
        /// </summary>
        public static IQueryable<FutureTrendOrSupplyAnalysisRawDataHolder> GetFutureTrendRawData(MarsDBDataContext dc, Dictionary<string, string> parameters, int fleetPlan = 1)
        {
            
            var cmsData = GetRawSizingCmsAndNessFleetData(dc, parameters);
            var fleetSizingData = GetFleetSizeFutureTrendData(dc, parameters, fleetPlan);

            //var validLocationGroupIds = cmsData.Select(d => d.LocationGroupId).Distinct().ToList();
            //fleetSizingData = fleetSizingData.Where(d => validLocationGroupIds.Contains(d.LocationGroupId));

            //fleetSizingData = from fsd in fleetSizingData
            //                  join validLgs in cmsData.Select(d=> d.LocationGroupId).Distinct() on fsd.LocationGroupId equals validLgs
            //                  select fsd;

            
            //var totalJoinData = cmsData.Union(fleetSizingData);

            var finalData2 = from td in cmsData
                             join fsd in fleetSizingData on new { td.Date, td.Country, td.CarClassId, td.LocationGroupId }
                                                    equals new { fsd.Date, fsd.Country, fsd.CarClassId, fsd.LocationGroupId }
                            into fjd
                             from joinFleetData in fjd.DefaultIfEmpty()
                            select new FutureTrendOrSupplyAnalysisRawDataHolder
                            {
                                ReportDate = td.Date.Value,
                                Country = td.Country,
                                CarClassId = td.CarClassId,
                                LocationGroupId = td.LocationGroupId,
                                Constrained = td.Constrained ?? 0,
                                Unconstrained = td.Unconstrained ?? 0,
                                Booked = td.AlreadyBooked ?? 0,
                                Expected = joinFleetData.ExpectedFleet ?? 0,
                                NessesaryConstrained = td.NessesaryConstrained ?? 0,
                                NessesaryUnconstrained = td.NessesaryUnconstrained ?? 0,
                                NessesaryBooked = td.NessesaryBooked ?? 0
                            };

            //var finalData = from td in totalJoinData
            //                select new FutureTrendOrSupplyAnalysisRawDataHolder
            //                {
            //                    ReportDate = td.Date.Value,
            //                    Country = td.Country,
            //                    CarClassId = td.CarClassId,
            //                    LocationGroupId = td.LocationGroupId,
            //                    Constrained = td.Constrained ?? 0,
            //                    Unconstrained = td.Unconstrained ?? 0,
            //                    Booked = td.AlreadyBooked ?? 0,
            //                    Expected = td.ExpectedFleet ?? 0,
            //                    NessesaryConstrained = td.NessesaryConstrained ?? 0,
            //                    NessesaryUnconstrained = td.NessesaryUnconstrained ?? 0,
            //                    NessesaryBooked = td.NessesaryBooked ?? 0
            //                };

            return finalData2;
        }

        /// <summary>
        /// Returns the same Data as the Raw version, but grouped by Date for Graphing
        /// </summary>
        public static IQueryable<FutureTrendOrSupplyAnalysisRawDataHolder> GetFutureTrendGroupyByDayData(MarsDBDataContext dc, Dictionary<string, string> parameters, int fleetPlan = 1)
        {
            var returned = from rd in GetFutureTrendRawData(dc, parameters, fleetPlan)
                          group rd by rd.ReportDate
                          into groupedData
                          select new FutureTrendOrSupplyAnalysisRawDataHolder
                                     {
                                         ReportDate = groupedData.Key,
                                         Constrained = groupedData.Sum(d => d.Constrained),
                                         Unconstrained = groupedData.Sum(d => d.Unconstrained),
                                         Booked = groupedData.Sum(d => d.Booked),
                                         Expected = groupedData.Sum(d => d.Expected),
                                         NessesaryConstrained = groupedData.Sum(d => d.NessesaryConstrained),
                                         NessesaryUnconstrained = groupedData.Sum(d => d.NessesaryUnconstrained),
                                         NessesaryBooked = groupedData.Sum(d => d.NessesaryBooked),
                                     };
            return returned;
        }


        public static IQueryable<KpiRawDataHolder> GetKpiRawData(MarsDBDataContext dc, Dictionary<string, string> parameters
                    ,FutureTrendDataType futureTrendDataType, bool excelRequest = false)
        {
            var cmsData = GetRawSizingCmsAndNessFleetData(dc, parameters);
            var fleetData = GetFleetSizeFutureTrendData(dc, parameters);

            IQueryable<KpiRawDataHolder> cmsGroupedData;
            IQueryable<KpiRawDataHolder> fleetGroupedData;
            IQueryable<KpiRawDataHolder> combinedData;

            if (parameters[ParameterNames.Country] == string.Empty && excelRequest)
            {
                cmsGroupedData = from cd in cmsData
                                 group cd by new { cd.Date, cd.Country }
                                     into gd
                                     select new KpiRawDataHolder
                                     {
                                         ReportDate = gd.Key.Date,
                                         Forecast = gd.Sum(d => futureTrendDataType == FutureTrendDataType.Constrained ? d.Constrained ?? 0 :
                                                                 futureTrendDataType == FutureTrendDataType.Unconstrained ? d.Unconstrained ?? 0 :
                                                                 futureTrendDataType == FutureTrendDataType.AlreadyBooked ? d.AlreadyBooked ?? 0 : 0),
                                         Country = gd.Key.Country
                                     };
                fleetGroupedData = from fd in cmsData
                                   group fd by new { fd.Date, fd.Country }
                                       into gd
                                       select new KpiRawDataHolder
                                       {
                                           ReportDate = gd.Key.Date,
                                           ExpectedFleet = gd.Sum(d => d.ExpectedFleet) ?? 0,
                                           Country = gd.Key.Country
                                       };
                combinedData = from cd in cmsGroupedData
                               join fd in fleetGroupedData
                                   on new { cd.ReportDate, cd.Country } equals new { fd.ReportDate, fd.Country }
                               select new KpiRawDataHolder
                               {
                                   ReportDate = cd.ReportDate,
                                   Forecast = cd.Forecast,
                                   ExpectedFleet = fd.ExpectedFleet,
                                   Country = cd.Country
                               };
            }
            else
            {
                cmsGroupedData = from cd in cmsData
                                 group cd by cd.Date
                                     into gd
                                     select new KpiRawDataHolder
                                     {
                                         ReportDate = gd.Key,
                                         Forecast = gd.Sum(d => futureTrendDataType == FutureTrendDataType.Constrained ? d.Constrained ?? 0 :
                                                                 futureTrendDataType == FutureTrendDataType.Unconstrained ? d.Unconstrained ?? 0 :
                                                                 futureTrendDataType == FutureTrendDataType.AlreadyBooked ? d.AlreadyBooked ?? 0 : 0),
                                         Country = gd.First().Country
                                     };
                fleetGroupedData = from fd in fleetData
                                   group fd by fd.Date
                                       into gd
                                       select new KpiRawDataHolder
                                       {
                                           ReportDate = gd.Key,
                                           ExpectedFleet = gd.Sum(d => d.ExpectedFleet) ?? 0,
                                           Country = gd.First().Country
                                       };

                combinedData = from cd in cmsGroupedData
                               join fd in fleetGroupedData
                                   on cd.ReportDate equals fd.ReportDate
                               select new KpiRawDataHolder
                               {
                                   ReportDate = cd.ReportDate,
                                   Forecast = cd.Forecast,
                                   ExpectedFleet = fd.ExpectedFleet,
                                   Country = cd.Country
                               };
            }

            return combinedData;
        }
    }
}