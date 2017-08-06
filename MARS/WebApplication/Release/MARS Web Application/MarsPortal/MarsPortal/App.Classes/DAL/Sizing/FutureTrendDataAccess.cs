using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Timers;
using App.BLL.DynamicLinq;
using App.BLL.ExtensionMethods;
using App.BLL.ReportEnums.FleetSize;
using App.DAL.MarsDataAccess.CsvExportStaticData;
using App.DAL.MarsDataAccess.ParameterAccess;
using Mars.App.Classes.DAL.MarsDBContext;
using App.DAL.MarsDataAccess.Sizing.FutureTrendDataHolders;
using App.Entities.Graphing;
using App.Entities.Graphing.Parameters;
using Mars.App.Classes.DAL.MarsDBContext;
using Mars.App.Classes.DAL.Sizing.GenericSizingDataAccess;
using Mars.DAL.Sizing;
using App.DAL.MarsDataAccess.Forecasting;

namespace App.DAL.MarsDataAccess.Sizing
{

    internal static class FutureTrendDataAccess
    {

        static String UNKNOWN = "Unknown", DELIMITER = ",";
        static Int32 DAY = 1, WEEK = 7, TIMEOUT = 1800000, HUNDRED = 100;
        static DataType _timeZone;

        internal static List<GraphSeries> GetFutureTrendGraphingData(Dictionary<string, string> parameters, FutureTrendDataType dataType, int scenarioId, DataType timeZone)
        {
            _timeZone = timeZone;
            if (_timeZone == DataType.FrozenZone)
                parameters[ParameterNames.ToDate] = SetFrozenZoneToDateLimit(DateTime.Parse(parameters[ParameterNames.ToDate]));


            using (var dataContext = new MarsDBDataContext(MarsConnection.ConnectionString))
            {
                dataContext.CommandTimeout = TIMEOUT;

                IQueryable<FutureTrendOrSupplyAnalysisRawDataHolder> groupedData;
                
                if (_timeZone == DataType.DailyChanging)
                {
                    //groupedData = new FutureTrendRepository().GetData(parameters, scenarioId);

                    groupedData = GenericSizingDataAccess.GetFutureTrendGroupyByDayData(dataContext, parameters, scenarioId).OrderBy(d=> d.ReportDate);
                }
                else
                {
                    var joinedData = GetRawFrozenZoneFutureTrendData(dataContext, parameters, (byte)scenarioId);
                    groupedData = from fc in joinedData
                                  group fc by new { fc.ReportDate }
                                      into g
                                      orderby g.Key.ReportDate ascending
                                      select new FutureTrendOrSupplyAnalysisRawDataHolder
                                      {
                                          ReportDate = g.Key.ReportDate,
                                          Constrained = g.Sum(c => c.Constrained),
                                          Unconstrained = g.Sum(c => c.Unconstrained),
                                          Booked = g.Sum(c => c.Booked),
                                          NessesaryConstrained = g.Sum(c => c.NessesaryConstrained),
                                          NessesaryUnconstrained = g.Sum(c => c.NessesaryUnconstrained),
                                          NessesaryBooked = g.Sum(c => c.NessesaryBooked),
                                          Expected = g.Sum(c => c.Expected)
                                      };
                }
                string foreCastSeriesName;
                IQueryable<FutureTrendGraphDataHolder> data;
                switch (dataType)
                {
                    case FutureTrendDataType.Constrained:
                        data = from gd in groupedData
                               select new FutureTrendGraphDataHolder
                               {
                                   ReportDate = gd.ReportDate,
                                   Forecast = gd.Constrained,
                                   Nessesary = gd.NessesaryConstrained,
                                   Expected = gd.Expected,
                               };
                        foreCastSeriesName = "Constrained Forecast";
                        break;
                    case FutureTrendDataType.Unconstrained:
                        data = from gd in groupedData
                               select new FutureTrendGraphDataHolder
                               {
                                   ReportDate = gd.ReportDate,
                                   Forecast = gd.Unconstrained,
                                   Nessesary = gd.NessesaryUnconstrained,
                                   Expected = gd.Expected
                               };
                        foreCastSeriesName = "Unconstrained Forecast";
                        break;
                    case FutureTrendDataType.AlreadyBooked:
                        data = from gd in groupedData
                               select new FutureTrendGraphDataHolder
                               {
                                   ReportDate = gd.ReportDate,
                                   Forecast = gd.Booked,
                                   Nessesary = gd.NessesaryBooked,
                                   Expected = gd.Expected
                               };
                        foreCastSeriesName = "Already Booked Forecast";
                        break;
                    default:
                        throw new ArgumentException("Invalid Future Trend Datatype selected");
                }

                return GetFutureTrendSeriesData(data, foreCastSeriesName, dataType);
            }
        }

        internal static string SetFrozenZoneToDateLimit(DateTime toDate)
        {
            var limitForFrozenZoneData = DateTime.Now.AddDays(WEEK);

            while (limitForFrozenZoneData.DayOfWeek != DayOfWeek.Sunday)
            {
                limitForFrozenZoneData = limitForFrozenZoneData.AddDays(DAY);
            }

            return toDate > limitForFrozenZoneData
                ? limitForFrozenZoneData.ToString()
                : toDate.ToString();
        }

        internal static string GetFutureTrendExcelData(Dictionary<string, string> parameters, FutureTrendDataType dataType, int scenarioId, int siteGroup, int fleetGroup, DataType timeZone)
        {
            var toDate = timeZone == DataType.FrozenZone
                ? SetFrozenZoneToDateLimit(DateTime.Parse(parameters[ParameterNames.ToDate]))
                : parameters[ParameterNames.ToDate];

            var newParameters = new Dictionary<string, string>
                                        {
                                            {ParameterNames.Country, parameters[ParameterNames.Country]},
                                            {ParameterNames.ToDate, toDate},
                                            {ParameterNames.FromDate, parameters[ParameterNames.FromDate]}
                                        };

            using (var dataContext = new MarsDBDataContext(MarsConnection.ConnectionString))
            {
                var joinedData = timeZone == DataType.DailyChanging
                            ? GenericSizingDataAccess.GetFutureTrendRawData(dataContext, parameters, scenarioId)
                            : GetRawFrozenZoneFutureTrendData(dataContext, newParameters, (byte)scenarioId);

                

                return GenerateExcelDataFromRaw(dataContext, joinedData, siteGroup, fleetGroup, dataType);
            }
        }


        private static IEnumerable<FutureTrendExcelDataHolder> GetJoinedExcelRawData(MarsDBDataContext dataContext
                , IQueryable<FutureTrendOrSupplyAnalysisRawDataHolder> joinedData, FutureTrendDataType dataType)
        {
            //var sss = joinedData.Where(d => d.LocationGroupId == 2).ToList();

            var excelData = from jd in joinedData
                   join lg in dataContext.CMS_LOCATION_GROUPs on jd.LocationGroupId equals lg.cms_location_group_id
                   into lgs 
                   from locationGroups in lgs.DefaultIfEmpty()
                   join cs in dataContext.CAR_GROUPs on jd.CarClassId equals cs.car_group_id
                   into cgs
                   from carGroups in cgs.DefaultIfEmpty()
                   join ctr in dataContext.COUNTRies on jd.Country equals ctr.country1                   
                   select new FutureTrendExcelDataHolder
                   {
                       ReportDate = jd.ReportDate,
                       CountryId = jd.Country,
                       CountryName = ctr.country_description,
                       Pool = locationGroups.CMS_POOL.cms_pool1 ?? UNKNOWN,
                       PoolId = locationGroups.cms_pool_id,
                       LocationGroupId = jd.LocationGroupId,
                       LocationGroup = locationGroups.cms_location_group1 ?? UNKNOWN,
                       CarSegment = carGroups.CAR_CLASS.CAR_SEGMENT.car_segment1 ?? UNKNOWN,
                       CarSegmentId = carGroups.CAR_CLASS.car_segment_id,
                       CarClassGroup = carGroups.CAR_CLASS.car_class1 ?? UNKNOWN,
                       CarClassGroupId = carGroups.car_class_id,
                       CarClass = carGroups.car_group1,
                       CarClassId = jd.CarClassId,
                       ExpectedFleet = jd.Expected,
                       Forecast = dataType == FutureTrendDataType.Constrained ? jd.Constrained : dataType == FutureTrendDataType.Unconstrained ? jd.Unconstrained : jd.Booked,
                       NessesaryFleet = dataType == FutureTrendDataType.Constrained ? jd.NessesaryConstrained : dataType == FutureTrendDataType.Unconstrained ? jd.NessesaryUnconstrained : jd.NessesaryBooked
                   };

            return excelData;
        }

        private static string GenerateExcelDataFromRaw(MarsDBDataContext dataContext, IQueryable<FutureTrendOrSupplyAnalysisRawDataHolder> joinedRawData,
                                                    int siteGroup, int fleetGroup, FutureTrendDataType dataType)
        {
            var csvData = new StringBuilder();

            var fullDataSet = GetJoinedExcelRawData(dataContext, joinedRawData, dataType);
            

            var forecastHeader = string.Empty;
            switch (dataType)
            {
                case FutureTrendDataType.Constrained:
                    forecastHeader = CsvExportHeaders.Constrained;
                    break;
                case FutureTrendDataType.Unconstrained:
                    forecastHeader = CsvExportHeaders.Unconstrained;
                    break;
                case FutureTrendDataType.AlreadyBooked:
                    forecastHeader = CsvExportHeaders.AlreadyBooked;
                    break;
            }

            csvData.Append(CsvExportMethods.GetExportHeaders(siteGroup, fleetGroup));
            csvData.Append(string.Format("{0},{1},{2}\n", forecastHeader, CsvExportHeaders.NessesaryFleet, CsvExportHeaders.ExpectedFleet));
            
            var orderedData = fullDataSet.GroupByMany(CsvExportMethods.GetGroupingColumns(siteGroup, fleetGroup)).OrderBy(d => d.Key);

            var keyList = new List<string>();
            foreach (var gr in orderedData)
            {
                CheckSubGroup(gr, keyList, csvData);
            }
            return csvData.ToString();
        }

        private static void CheckSubGroup(GroupResult gr, ICollection<string> keyList, StringBuilder csvData)
        {
            keyList.Add(gr.Key.ToString());
            if (gr.SubGroups == null)
            {
                var ftData = gr.Items.Cast<FutureTrendExcelDataHolder>();

                decimal sumForecast = 0;
                decimal sumNessFleet = 0;
                decimal sumExpectedFleet = 0;
                foreach (var d in ftData)
                {
                    sumForecast += d.Forecast;
                    sumNessFleet += d.NessesaryFleet;
                    sumExpectedFleet += d.ExpectedFleet;
                }

                var str = string.Format("{0},{1},{2}\n",
                                        Math.Round(sumForecast, 0, MidpointRounding.AwayFromZero),
                                        Math.Round(sumNessFleet, 0, MidpointRounding.AwayFromZero),
                                        Math.Round(sumExpectedFleet, 0, MidpointRounding.AwayFromZero));

                var listOfKeys = new StringBuilder();
                foreach (var key in keyList)
                {
                    listOfKeys.Append(key + DELIMITER);
                }
                csvData.Append(listOfKeys + str);
            }
            else
            {
                gr.SubGroups.ToList().ForEach(d => CheckSubGroup(d, keyList, csvData));
            }

            keyList.Remove(gr.Key.ToString());
        }

        private static List<GraphSeries> GetFutureTrendSeriesData(IQueryable<FutureTrendGraphDataHolder> futureTrendDataFromDatabase, string forecastSeriesName, FutureTrendDataType dataType)
        {
            var dateTimes = new List<object>();
            var expected = new List<double>();
            var nessesary = new List<double>();
            var forecast = new List<double>();

            foreach (var ft in futureTrendDataFromDatabase)
            {
                dateTimes.Add(ft.ReportDate);
                if (ft.Expected > 0) expected.Add((double)ft.Expected);
                nessesary.Add((double)ft.Nessesary);
                forecast.Add((double)ft.Forecast);
            }

            Color forecastColour;
            switch (dataType)
            {
                case FutureTrendDataType.Constrained:
                    forecastColour = Color.MidnightBlue;
                    break;
                case FutureTrendDataType.Unconstrained:
                    forecastColour = Color.CornflowerBlue;
                    break;
                case FutureTrendDataType.AlreadyBooked:
                    forecastColour = Color.YellowGreen;
                    break;
                default:
                    forecastColour = Color.Black;
                    break;
            }

            var seriesInformation = new List<GraphSeries>()
                                        {
                                            new GraphSeries("Expected")
                                                {GraphColour = Color.DarkGreen, Xvalue = dateTimes, Yvalue = expected},
                                            new GraphSeries("Necessary")
                                                {GraphColour = Color.Brown, Xvalue = dateTimes, Yvalue = nessesary},
                                            new GraphSeries(forecastSeriesName)
                                                {GraphColour = forecastColour, Xvalue = dateTimes, Yvalue = forecast}
                                        };

            return seriesInformation;
        }


        internal static IQueryable<FutureTrendOrSupplyAnalysisRawDataHolder> GetRawFrozenZoneFutureTrendData(MarsDBDataContext dataContext, Dictionary<string, string> parameters, byte scenarioId)
        {
            var country = parameters.ContainsKey(ParameterNames.Country) ? parameters[ParameterNames.Country] : null;
            var fromDate = DateTime.Parse(parameters.ContainsKey(ParameterNames.FromDate) ? parameters[ParameterNames.FromDate] : null);
            var toDate = DateTime.Parse(parameters.ContainsKey(ParameterNames.ToDate) ? parameters[ParameterNames.ToDate] : null);

            var fullDataSet = from fc in dataContext.MARS_CMS_FORECAST_HISTORies
                              select fc;

            var restrictedData = ForecastParameterRestriction.RestrictHistoricalForecastByParameters(parameters, fullDataSet, dataContext);

            var joinedData = from rd in restrictedData
                             join nf in dataContext.MARS_CMS_NECESSARY_FLEETs on
                                 new { rd.CMS_LOCATION_GROUP_ID, rd.CAR_CLASS_ID }
                                 equals
                                 new { nf.CMS_LOCATION_GROUP_ID, nf.CAR_CLASS_ID }
                                 into nessearyFleetDetails
                             from joinednessFleetDetails in nessearyFleetDetails.DefaultIfEmpty()
                             join fps in dataContext.FleetPlanSummary(country == String.Empty ? null : country, scenarioId, fromDate, toDate) on
                                 new { date = (DateTime?)rd.REP_DATE, classId = rd.CAR_CLASS_ID, locGroupId = rd.CMS_LOCATION_GROUP_ID }
                                 equals
                                 new { date = fps.targetDate, classId = fps.CAR_CLASS_ID.Value, locGroupId = fps.LOCATION_GROUP_ID.Value }
                             into fleetDetails
                             from joinedFleetDetails in fleetDetails.DefaultIfEmpty()
                             select new FutureTrendOrSupplyAnalysisRawDataHolder
                             {
                                 ReportDate = rd.REP_DATE,
                                 Constrained = rd.CMS_CONSTRAINED_FROZEN ?? 0,
                                 Unconstrained = rd.CMS_UNCONSTRAINED_FROZEN ?? 0,
                                 Booked = 0,
                                 Utilization = joinednessFleetDetails.UTILISATION ?? 0,
                                 NonRevFleet = joinednessFleetDetails.NONREV_FLEET ?? 0,
                                 OnRent = rd.CURRENT_ONRENT ?? 0,
                                 NessesaryConstrained = joinednessFleetDetails.UTILISATION == 0 ? 0 : (rd.CMS_CONSTRAINED_FROZEN / joinednessFleetDetails.UTILISATION * HUNDRED) + (joinednessFleetDetails.NONREV_FLEET * rd.CMS_CONSTRAINED_FROZEN / HUNDRED)
                                            ?? rd.CMS_CONSTRAINED_FROZEN ?? 0,
                                 NessesaryUnconstrained = joinednessFleetDetails.UTILISATION == 0 ? 0 : (rd.CMS_UNCONSTRAINED_FROZEN / joinednessFleetDetails.UTILISATION * HUNDRED) + (joinednessFleetDetails.NONREV_FLEET * rd.CMS_UNCONSTRAINED_FROZEN / HUNDRED)
                                            ?? rd.CMS_UNCONSTRAINED_FROZEN ?? 0,
                                 NessesaryBooked = 0,
                                 Expected = rd.OPERATIONAL_FLEET ?? 0 + joinedFleetDetails.amount.GetValueOrDefault(0),
                                 LocationGroupId = rd.CMS_LOCATION_GROUP_ID,
                                 CarClassId = rd.CAR_CLASS_ID,
                                 Country = rd.COUNTRY
                             };

            return joinedData;
        }
    }
}