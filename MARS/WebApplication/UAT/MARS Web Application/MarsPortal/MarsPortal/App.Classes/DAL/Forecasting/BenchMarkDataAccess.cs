using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using App.BLL.DynamicLinq;
using App.BLL.ExtensionMethods;
using App.DAL.MarsDataAccess.CsvExportStaticData;
using App.DAL.MarsDataAccess.Forecasting.BenchmarkDataHolders;
using App.DAL.MarsDataAccess.ParameterAccess;
using Mars.App.Classes.DAL.MarsDBContext;
using App.Entities.Graphing;
using Mars.App.Classes.DAL.MarsDBContext;
using Mars.App.Classes.DAL.Sizing.GenericSizingDataAccess;

namespace App.DAL.MarsDataAccess.Forecasting
{
    internal static class BenchMarkDataAccess
    {
        internal static List<GraphSeries> GetBenchMarkGraphingData(Dictionary<string, string> parameters, bool constrainedForecast)
        {
            using (var dataContext = new MarsDBDataContext(MarsConnection.ConnectionString))
            {
                var restrictedData = GenericSizingDataAccess.GetBenchmarkRawData(parameters, dataContext);
                var groupedData = from rd in restrictedData
                                  group rd by new { RepDate = rd.REP_DATE }
                                      into g
                                      orderby g.Key.RepDate ascending
                                      select new BenchmarkGraphDataHolder
                                      {
                                          ReportDate = g.Key.RepDate,
                                          CurrentOnRent = g.Sum(d => d.CURRENT_ONRENT) ?? 0,
                                          OnRentLastYear = g.Sum(d => d.ONRENT_LY) ?? 0,
                                          FrozenValue = constrainedForecast ? g.Sum(d => d.CMS_CONSTRAINED) ?? 0 : g.Sum(d => d.CMS_UNCONSTRAINED) ?? 0, // altered by Gavin
                                          Week1 = constrainedForecast ? g.Sum(d => d.CMS_CONSTRAINED_WK1) ?? 0 : g.Sum(d => d.CMS_UNCONSTRAINED_WK1) ?? 0,
                                          Week2 = constrainedForecast ? g.Sum(d => d.CMS_CONSTRAINED_WK2) ?? 0 : g.Sum(d => d.CMS_UNCONSTRAINED_WK2) ?? 0,
                                          Week3 = constrainedForecast ? g.Sum(d => d.CMS_CONSTRAINED_WK3) ?? 0 : g.Sum(d => d.CMS_UNCONSTRAINED_WK3) ?? 0,
                                          Week4 = constrainedForecast ? g.Sum(d => d.CMS_CONSTRAINED_WK4) ?? 0 : g.Sum(d => d.CMS_UNCONSTRAINED_WK4) ?? 0,
                                          Week5 = constrainedForecast ? g.Sum(d => d.CMS_CONSTRAINED_WK5) ?? 0 : g.Sum(d => d.CMS_UNCONSTRAINED_WK5) ?? 0,
                                          Week6 = constrainedForecast ? g.Sum(d => d.CMS_CONSTRAINED_WK6) ?? 0 : g.Sum(d => d.CMS_UNCONSTRAINED_WK6) ?? 0,
                                          Week7 = constrainedForecast ? g.Sum(d => d.CMS_CONSTRAINED_WK7) ?? 0 : g.Sum(d => d.CMS_UNCONSTRAINED_WK7) ?? 0,
                                          Week8 = constrainedForecast ? g.Sum(d => d.CMS_CONSTRAINED_WK8) ?? 0 : g.Sum(d => d.CMS_UNCONSTRAINED_WK8) ?? 0,
                                          TopDown = constrainedForecast ? g.Sum(p => p.ADJUSTMENT_TD) ?? 0 : g.Sum(p => p.ADJUSTMENT_TD) ?? 0
                                      };

                var dateTimes = new List<object>();
                var currentOnRent = new List<double>();
                var onRentLastYear = new List<double>();
                var frozenValue = new List<double>();
                var week1 = new List<double>();
                var week2 = new List<double>();
                var week3 = new List<double>();
                var week4 = new List<double>();
                var week5 = new List<double>();
                var week6 = new List<double>();
                var week7 = new List<double>();
                var week8 = new List<double>();
                var topDown = new List<double>();


                foreach (var ft in groupedData)
                {
                    dateTimes.Add(ft.ReportDate);

                    currentOnRent.AddPreviousIfZero((double)ft.CurrentOnRent);
                    onRentLastYear.AddPreviousIfZero((double)ft.OnRentLastYear);
                    frozenValue.AddPreviousIfZero((double)ft.FrozenValue);
                    week1.AddPreviousIfZero((double)ft.Week1);
                    week2.AddPreviousIfZero((double)ft.Week2);
                    week3.AddPreviousIfZero((double)ft.Week3);
                    week4.AddPreviousIfZero((double)ft.Week4);
                    week5.AddPreviousIfZero((double)ft.Week5);
                    week6.AddPreviousIfZero((double)ft.Week6);
                    week7.AddPreviousIfZero((double)ft.Week7);
                    week8.AddPreviousIfZero((double)ft.Week8);
                    topDown.AddPreviousIfZero((double)ft.TopDown);

                }
                var colourList = new List<Color>();
                var startingColour = Color.Orange;
                colourList.Add(startingColour);

                for (var i = 0; i < 8; i++)
                {
                    colourList.Add(Color.FromArgb(startingColour.R - i * 20, startingColour.G - i * 20, startingColour.B));
                }

                var seriesInformation = new List<GraphSeries>
                        {
                            new GraphSeries("On Rent")
                                {GraphColour = Color.Green, Xvalue = dateTimes, Yvalue = currentOnRent},
                            new GraphSeries("On Rent LY")
                                {GraphColour = Color.DimGray, Xvalue = dateTimes, Yvalue = onRentLastYear},
                            new GraphSeries(constrainedForecast ? "Constrained" : "Unconstrained")
                                {GraphColour = constrainedForecast ? Color.MidnightBlue : Color.CornflowerBlue, 
                                                    Xvalue = dateTimes, Yvalue = frozenValue},
                            new GraphSeries("Week 1")
                                {GraphColour = colourList[7], Xvalue = dateTimes, Yvalue = week1},
                            new GraphSeries("Week 2")
                                {GraphColour = colourList[6], Xvalue = dateTimes, Yvalue = week2},
                            new GraphSeries("Week 3")
                                {GraphColour = colourList[5], Xvalue = dateTimes, Yvalue = week3},
                            new GraphSeries("Week 4")
                                {GraphColour = colourList[4], Xvalue = dateTimes, Yvalue = week4},
                            new GraphSeries("Week 5")
                                {GraphColour = colourList[3], Xvalue = dateTimes, Yvalue = week5},
                            new GraphSeries("Week 6")
                                {GraphColour = colourList[2], Xvalue = dateTimes, Yvalue = week6},
                            new GraphSeries("Week 7")
                                {GraphColour = colourList[1], Xvalue = dateTimes, Yvalue = week7},
                            new GraphSeries("Week 8")
                                {GraphColour = colourList[0], Xvalue = dateTimes, Yvalue = week8},
                            new GraphSeries("Top Down")
                                {GraphColour = Color.SandyBrown, Xvalue = dateTimes, Yvalue = topDown}
                        };
                return seriesInformation;
            }
        }

        private static string GeForecastCsvHeader(int siteGroup, int fleetGroup, bool constrainedForecast)
        {
            var header = new StringBuilder();

            header.Append(CsvExportMethods.GetExportHeaders(siteGroup, fleetGroup));

            if (constrainedForecast)
            {
                header.Append(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}\n", CsvExportHeaders.OnRent, CsvExportHeaders.OnRentLy,
                        CsvExportHeaders.Constrained, CsvExportHeaders.ConstrainedWeek1, CsvExportHeaders.ConstrainedWeek2, CsvExportHeaders.ConstrainedWeek3, CsvExportHeaders.ConstrainedWeek4,
                        CsvExportHeaders.ConstrainedWeek5, CsvExportHeaders.ConstrainedWeek6, CsvExportHeaders.ConstrainedWeek7, CsvExportHeaders.ConstrainedWeek8
                        , CsvExportHeaders.TopDown));
            }
            else
            {
                header.Append(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}\n", CsvExportHeaders.OnRent, CsvExportHeaders.OnRentLy,
                        CsvExportHeaders.Unconstrained, CsvExportHeaders.UnconstrainedWeek1, CsvExportHeaders.UnconstrainedWeek2, CsvExportHeaders.UnconstrainedWeek3, CsvExportHeaders.UnconstrainedWeek4,
                        CsvExportHeaders.UnconstrainedWeek5, CsvExportHeaders.UnconstrainedWeek6, CsvExportHeaders.UnconstrainedWeek7, CsvExportHeaders.UnconstrainedWeek8
                        , CsvExportHeaders.TopDown));
            }


            return header.ToString();
        }

        internal static string GetBenchmarkCsvData(Dictionary<string, string> parameters, bool constrainedForecast, int siteGroup, int fleetGroup)
        {
            using (var dataContext = new MarsDBDataContext(MarsConnection.ConnectionString))
            {
                var restrictedData = GenericSizingDataAccess.GetBenchmarkRawData(parameters, dataContext);

                var fullDataSet = GetJoinedExcelData(dataContext, restrictedData, constrainedForecast);
                var csvData = new StringBuilder();

                csvData.Append(GeForecastCsvHeader(siteGroup, fleetGroup, constrainedForecast));

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
                var ftData = gr.Items.Cast<BenchmarkExcelDataHolder>();

                decimal sumOnRent = 0;
                decimal sumOnRentLy = 0;
                decimal sumFrozenValue = 0;
                decimal sumWeek1 = 0;
                decimal sumWeek2 = 0;
                decimal sumWeek3 = 0;
                decimal sumWeek4 = 0;
                decimal sumWeek5 = 0;
                decimal sumWeek6 = 0;
                decimal sumWeek7 = 0;
                decimal sumWeek8 = 0;
                decimal sumTopDown = 0;
                foreach (var d in ftData)
                {
                    sumOnRent += d.CurrentOnRent;
                    sumOnRentLy += d.OnRentLastYear;
                    sumFrozenValue += d.FrozenValue;
                    sumWeek1 += d.Week1;
                    sumWeek2 += d.Week2;
                    sumWeek3 += d.Week3;
                    sumWeek4 += d.Week4;
                    sumWeek5 += d.Week5;
                    sumWeek6 += d.Week6;
                    sumWeek7 += d.Week7;
                    sumWeek8 += d.Week8;
                    sumTopDown += d.TopDown;
                }

                var str = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}\n",
                                        sumOnRent.Round(),
                                        sumOnRentLy.Round(),
                                        sumFrozenValue.Round(),
                                        sumWeek1.Round(),
                                        sumWeek2.Round(),
                                        sumWeek3.Round(),
                                        sumWeek4.Round(),
                                        sumWeek5.Round(),
                                        sumWeek6.Round(),
                                        sumWeek7.Round(),
                                        sumWeek8.Round(),
                                        sumTopDown.Round());

                var listOfKeys = new StringBuilder();
                foreach (var key in keyList)
                {
                    listOfKeys.Append(key + ",");
                }
                csvData.Append(listOfKeys + str);
            }
            else
            {
                gr.SubGroups.ToList().ForEach(d => CheckSubGroup(d, keyList, csvData));
            }

            keyList.Remove(gr.Key.ToString());
        }

        private static IEnumerable<BenchmarkExcelDataHolder> GetJoinedExcelData(MarsDBDataContext dataContext, IQueryable<MARS_CMS_FORECAST_HISTORY> rawData, bool constrainedForecast)
        {
            var returned = from rd in rawData
                           join lg in dataContext.CMS_LOCATION_GROUPs on rd.CMS_LOCATION_GROUP_ID equals lg.cms_location_group_id
                           join cs in dataContext.CAR_GROUPs on rd.CAR_CLASS_ID equals cs.car_group_id
                           select new BenchmarkExcelDataHolder
                           {
                               ReportDate = rd.REP_DATE,
                               CountryId = lg.CMS_POOL.country,
                               CountryName = lg.CMS_POOL.COUNTRy1.country_description,
                               Pool = lg.CMS_POOL.cms_pool1,
                               PoolId = lg.cms_pool_id,
                               LocationGroupId = rd.CMS_LOCATION_GROUP_ID,
                               LocationGroup = lg.cms_location_group1,
                               CarSegment = cs.CAR_CLASS.CAR_SEGMENT.car_segment1,
                               CarSegmentId = cs.CAR_CLASS.car_segment_id,
                               CarClassGroup = cs.CAR_CLASS.car_class1,
                               CarClassGroupId = cs.car_class_id,
                               CarClass = cs.car_group1,
                               CarClassId = rd.CAR_CLASS_ID,

                               CurrentOnRent = rd.CURRENT_ONRENT ?? 0,
                               OnRentLastYear = rd.ONRENT_LY ?? 0,
                               FrozenValue = constrainedForecast ? rd.CMS_CONSTRAINED ?? 0 : rd.CMS_UNCONSTRAINED ?? 0,
                               Week1 = constrainedForecast ? rd.CMS_CONSTRAINED_WK1 ?? 0 : rd.CMS_UNCONSTRAINED_WK1 ?? 0,
                               Week2 = constrainedForecast ? rd.CMS_CONSTRAINED_WK2 ?? 0 : rd.CMS_UNCONSTRAINED_WK2 ?? 0,
                               Week3 = constrainedForecast ? rd.CMS_CONSTRAINED_WK3 ?? 0 : rd.CMS_UNCONSTRAINED_WK3 ?? 0,
                               Week4 = constrainedForecast ? rd.CMS_CONSTRAINED_WK4 ?? 0 : rd.CMS_UNCONSTRAINED_WK4 ?? 0,
                               Week5 = constrainedForecast ? rd.CMS_CONSTRAINED_WK5 ?? 0 : rd.CMS_UNCONSTRAINED_WK5 ?? 0,
                               Week6 = constrainedForecast ? rd.CMS_CONSTRAINED_WK6 ?? 0 : rd.CMS_UNCONSTRAINED_WK6 ?? 0,
                               Week7 = constrainedForecast ? rd.CMS_CONSTRAINED_WK7 ?? 0 : rd.CMS_UNCONSTRAINED_WK7 ?? 0,
                               Week8 = constrainedForecast ? rd.CMS_CONSTRAINED_WK8 ?? 0 : rd.CMS_UNCONSTRAINED_WK8 ?? 0,
                               TopDown = rd.ADJUSTMENT_TD ?? 0
                           };

            //var data = returned.ToList();
            //foreach(var bh in data)
            //{
            //    if(bh.CurrentOnRent == 0)
            //    {
            //        var localBh = bh;
            //        var previousDaysData = data.FirstOrDefault(d => 
            //                d.ReportDate == localBh.ReportDate.AddDays(-1)
            //                && d.CountryId == localBh.CountryId
            //                && d.LocationGroupId == localBh.LocationGroupId
            //                && d.CarClassId == localBh.CarClassId
            //            );
            //        if(previousDaysData.CountryId == null)
            //        {
            //            var ss = 0;
            //        }
            //        else
            //        {

            //            var ww = 0;
            //        }
            //    }
            //}
            return returned;
        }

    }
}