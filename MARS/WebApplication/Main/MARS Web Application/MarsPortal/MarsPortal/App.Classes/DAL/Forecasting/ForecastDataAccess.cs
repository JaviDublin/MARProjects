using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using App.BLL.DynamicLinq;
using App.BLL.ReportEnums.FleetSize;
using App.DAL.MarsDataAccess.CsvExportStaticData;
using App.DAL.MarsDataAccess.Forecasting.ForecastDataHolders;
using App.DAL.MarsDataAccess.ParameterAccess;
using Mars.App.Classes.DAL.MarsDBContext;
using App.Entities.Graphing;
using App.BLL.ExtensionMethods;
using Mars.App.Classes.DAL.Sizing.GenericSizingDataAccess;

namespace App.DAL.MarsDataAccess.Forecasting
{
    internal static class ForecastDataAccess
    {
        internal static List<GraphSeries> GetForecastGraphingData(Dictionary<string, string> parameters, DataType timeZone)
        {
            using (var dataContext = new MarsDBDataContext(MarsConnection.ConnectionString))
            {
                var rawData = timeZone == DataType.DailyChanging
                                      ? GenericSizingDataAccess.GetForecastRawData(parameters, dataContext)
                                      : GetFrozenZoneForecastRawData(parameters, dataContext);

                var groupedData = from rd in rawData
                           group rd by new { RepDate = rd.ReportDate } into g
                           orderby g.Key.RepDate ascending
                           select new ForecastGraphDataHolder
                                      {
                                          ReportDate = g.Key.RepDate,
                                          CurrentOnRent = g.Sum(d => d.OnRent),
                                          OnRentLastYear = g.Sum(d => d.OnRentLy),
                                          ConstrainedForecast = g.Sum(d => d.Constrained),
                                          UnconstrainedForecast = g.Sum(d => d.Unconstrained),
                                          Fleet = g.Sum(d => d.Fleet),
                                          AlreadyBooked = g.Sum(d => d.AlreadyBooked),
                                          OnRentTopDown = g.Sum(d => d.TopDown),
                                          OnRentBottomUpOne = g.Sum(d => d.BottomUp1),
                                          OnRentBottomUpTwo = g.Sum(d => d.BottomUp2),
                                          OnRentReconciliation = g.Sum(d => d.Reconciliation)
                                      };
                dataContext.Log = new DebugTextWriter();
                return GetForecastSeriesData(groupedData, timeZone);
            }
        }

        private static IQueryable<ForecastExcelDataHolder> GetJoinedExcelData(MarsDBDataContext dataContext, IQueryable<ForecastRawDataHolder> rawData)
        {
            return from rd in rawData
                   join lg in dataContext.CMS_LOCATION_GROUPs on rd.LocationGroupId equals lg.cms_location_group_id
                   join cs in dataContext.CAR_GROUPs on rd.CarClassId equals cs.car_group_id
                   select new ForecastExcelDataHolder
                   {
                       ReportDate = rd.ReportDate,
                       CountryId = lg.CMS_POOL.country,
                       CountryName = lg.CMS_POOL.COUNTRy1.country_description,
                       Pool = lg.CMS_POOL.cms_pool1,
                       PoolId = lg.cms_pool_id,
                       LocationGroupId = rd.LocationGroupId,
                       LocationGroup = lg.cms_location_group1,
                       CarSegment = cs.CAR_CLASS.CAR_SEGMENT.car_segment1,
                       CarSegmentId = cs.CAR_CLASS.car_segment_id,
                       CarClassGroup = cs.CAR_CLASS.car_class1,
                       CarClassGroupId = cs.car_class_id,
                       CarClass = cs.car_group1,
                       CarClassId = rd.CarClassId,
                       
                       OnRent = rd.OnRent,
                       OnRentLy = rd.OnRentLy,
                       Constrained = rd.Constrained,
                       Unconstrained = rd.Unconstrained,
                       Fleet = rd.Fleet,
                       AlreadyBooked = rd.AlreadyBooked,
                       TopDown = rd.TopDown,
                       BottomUp1 = rd.BottomUp1,
                       BottomUp2 = rd.BottomUp2,
                       Reconciliation = rd.Reconciliation
                   };
        }

        internal static string GeForecastCsvHeader(int siteGroup, int fleetGroup)
        {
            var header = new StringBuilder();
            header.Append(CsvExportMethods.GetExportHeaders(siteGroup, fleetGroup));

            header.Append(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}\n", CsvExportHeaders.OnRent, CsvExportHeaders.OnRentLy,
                    CsvExportHeaders.Constrained, CsvExportHeaders.Unconstrained, CsvExportHeaders.Fleet, CsvExportHeaders.AlreadyBooked, CsvExportHeaders.TopDown,
                    CsvExportHeaders.BottomUp1, CsvExportHeaders.BottomUp2, CsvExportHeaders.Reconciliation));

            return header.ToString();
        }

        internal static string GetForecastExcelData(Dictionary<string, string> parameters, FutureTrendDataType dataType, int siteGroup, int fleetGroup, DataType timeZone)
        {
            using (var dataContext = new MarsDBDataContext(MarsConnection.ConnectionString))
            {
                var csvData = new StringBuilder();
                var rawData = timeZone == DataType.DailyChanging
                                  ? GenericSizingDataAccess.GetForecastRawData(parameters, dataContext)
                                  : GetFrozenZoneForecastRawData(parameters, dataContext);

                var fullDataSet = GetJoinedExcelData(dataContext, rawData);

                
                var orderedData = fullDataSet.GroupByMany(CsvExportMethods.GetGroupingColumns(siteGroup, fleetGroup)).OrderBy(d => d.Key);

                var keyList = new List<string>();
                foreach (var gr in orderedData)
                {
                    CheckSubGroup(gr, keyList, csvData);
                }
                return csvData.ToString();
            }
        }

        private static void CheckSubGroup(GroupResult gr, ICollection<string> keyList, StringBuilder csvData)
        {

            keyList.Add(gr.Key.ToString());
            if (gr.SubGroups == null)
            {
                var ftData = gr.Items.Cast<ForecastExcelDataHolder>();

                decimal sumOnRent = 0;
                decimal sumOnRentLy = 0;
                decimal sumConstrained = 0;
                decimal sumUnconstrained = 0;
                decimal sumFleet = 0;
                decimal sumAlreadyBooked = 0;
                decimal sumTopDown = 0;
                decimal sumBottomUp1 = 0;
                decimal sumBottomUp2 = 0;
                decimal sumReconciliation = 0;
                
                foreach (var d in ftData)
                {
                    sumOnRent += d.OnRent;
                    sumOnRentLy += d.OnRentLy;
                    sumConstrained += d.Constrained;
                    sumUnconstrained += d.Unconstrained;
                    sumFleet += d.Fleet;
                    sumAlreadyBooked += d.AlreadyBooked;
                    sumTopDown += d.TopDown;
                    sumBottomUp1 += d.BottomUp1;
                    sumBottomUp2 += d.BottomUp2;
                    sumReconciliation += d.Reconciliation;
                }

                var summedColumns = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}\n",
                                        Math.Round(sumOnRent, 0, MidpointRounding.AwayFromZero),
                                        Math.Round(sumOnRentLy, 0, MidpointRounding.AwayFromZero),
                                        Math.Round(sumConstrained, 0, MidpointRounding.AwayFromZero),
                                        Math.Round(sumUnconstrained, 0, MidpointRounding.AwayFromZero),
                                        Math.Round(sumFleet, 0, MidpointRounding.AwayFromZero),
                                        Math.Round(sumAlreadyBooked, 0, MidpointRounding.AwayFromZero),
                                        Math.Round(sumTopDown, 0, MidpointRounding.AwayFromZero),
                                        Math.Round(sumBottomUp1, 0, MidpointRounding.AwayFromZero),
                                        Math.Round(sumBottomUp2, 0, MidpointRounding.AwayFromZero),
                                        Math.Round(sumReconciliation, 0, MidpointRounding.AwayFromZero));

                var listOfKeys = new StringBuilder();
                foreach (var key in keyList)
                {
                    listOfKeys.Append(key + ",");
                }
                csvData.Append(listOfKeys + summedColumns);
            }
            else
            {
                gr.SubGroups.ToList().ForEach(d => CheckSubGroup(d, keyList, csvData));
            }

            keyList.Remove(gr.Key.ToString());
        }

 internal static IQueryable<ForecastRawDataHolder> GetFrozenZoneForecastRawData(Dictionary<string, string> parameters, MarsDBDataContext dataContext)
        {
            var fullDataSet = from fc in dataContext.MARS_CMS_FORECAST_HISTORies
                                  select fc;

            var restrictedData = ForecastParameterRestriction.RestrictHistoricalForecastByParameters(parameters, fullDataSet, dataContext);

            var joinedData = from rd in restrictedData
                                join adj in dataContext.MARS_CMS_FORECAST_ADJUSTMENTs on
                                    new { rd.REP_DATE, rd.CMS_LOCATION_GROUP_ID, rd.CAR_CLASS_ID }
                                    equals new { adj.REP_DATE, adj.CMS_LOCATION_GROUP_ID, adj.CAR_CLASS_ID }
                                    into adj
                                from joinedAdjustment in adj.DefaultIfEmpty()
                                select new ForecastRawDataHolder()
                                {
                                    ReportDate = rd.REP_DATE,
                                    LocationGroupId = rd.CMS_LOCATION_GROUP_ID,
                                    CarClassId = rd.CAR_CLASS_ID,
                                    OnRent = rd.CURRENT_ONRENT ?? 0,
                                    OnRentLy = rd.ONRENT_LY ?? 0,
                                    Constrained = rd.CMS_CONSTRAINED ?? 0,
                                    Unconstrained = rd.CMS_UNCONSTRAINED ?? 0,
                                    Fleet = 0,
                                    AlreadyBooked = 0,
                                    TopDown = joinedAdjustment.ADJUSTMENT_TD ?? 0,
                                    BottomUp1 = joinedAdjustment.ADJUSTMENT_BU1 ?? 0,
                                    BottomUp2 = joinedAdjustment.ADJUSTMENT_BU2 ?? 0,
                                    Reconciliation = joinedAdjustment.ADJUSTMENT_RC ?? 0
                                };
            return joinedData;
        }

        private static List<GraphSeries> GetForecastSeriesData(IEnumerable<ForecastGraphDataHolder> forecastDataFromDatabase, DataType timeZone)
        {
            var dateTimes = new List<object>();
            var currentOnRent = new List<double>();
            var onRentLastYear = new List<double>();
            var constrainedForecast = new List<double>();
            var unconstrainedForecast = new List<double>();
            var alreadyBooked = new List<double>();
            var fleet = new List<double>();
            var onRentTopDown = new List<double>();
            var onRentBottomUpOne = new List<double>();
            var onRentBottomUpTwo = new List<double>();
            var onRentReconciliation = new List<double>();

            foreach (var ft in forecastDataFromDatabase)
            {
                dateTimes.Add(ft.ReportDate);
                double d;
                //d = currentOnRent[currentOnRent.Count - 1];
                if (ft.CurrentOnRent == 0 && currentOnRent.Count != 0)
                {
                    d = currentOnRent[currentOnRent.Count - 1];
                }
                else
                {
                    d = (double)ft.CurrentOnRent;
                }

                currentOnRent.Add(d);
                onRentLastYear.Add((double)ft.OnRentLastYear);
                constrainedForecast.Add((double)ft.ConstrainedForecast);
                unconstrainedForecast.Add((double)ft.UnconstrainedForecast);
                alreadyBooked.Add((double)ft.AlreadyBooked);
                fleet.Add((double) ft.Fleet);
                onRentTopDown.Add((double)ft.OnRentTopDown);
                onRentBottomUpOne.Add((double)ft.OnRentBottomUpOne);
                onRentBottomUpTwo.Add((double)ft.OnRentBottomUpTwo);
                onRentReconciliation.Add((double)ft.OnRentReconciliation);
            }

            var seriesInformation = new List<GraphSeries>()
                        {
                            new GraphSeries("On Rent")
                                {GraphColour = Color.Green, Xvalue = dateTimes, Yvalue = currentOnRent},
                            new GraphSeries("On Rent LY")
                                {GraphColour = Color.DimGray, Xvalue = dateTimes, Yvalue = onRentLastYear},
                            new GraphSeries("Constrained")
                                {GraphColour = Color.MidnightBlue, Xvalue = dateTimes, Yvalue = constrainedForecast},
                            new GraphSeries("Unconstrained")
                                {GraphColour = Color.CornflowerBlue, Xvalue = dateTimes, Yvalue = unconstrainedForecast},
                            new GraphSeries("Fleet")
                                {GraphColour = Color.Black, Xvalue = dateTimes, Yvalue = fleet},
                            new GraphSeries("Already Booked")
                                {GraphColour = Color.YellowGreen, Xvalue = dateTimes, Yvalue = alreadyBooked},
                            new GraphSeries("Top Down")
                                {GraphColour = Color.SandyBrown, Xvalue = dateTimes, Yvalue = onRentTopDown},
                            new GraphSeries("Bottom Up 1")
                                {GraphColour = Color.Orchid, Xvalue = dateTimes, Yvalue = onRentBottomUpOne},
                            new GraphSeries("Bottom Up 2")
                                {GraphColour = Color.Plum, Xvalue = dateTimes, Yvalue = onRentBottomUpTwo},
                            new GraphSeries("Reconciliation")
                                {GraphColour = Color.Crimson, Xvalue = dateTimes, Yvalue = onRentReconciliation}
                        };


            return seriesInformation;
        }
    }
}