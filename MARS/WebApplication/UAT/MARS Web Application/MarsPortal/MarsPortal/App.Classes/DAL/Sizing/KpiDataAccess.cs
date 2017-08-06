using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using App.BLL.ReportEnums.FleetSize;
using App.BLL.TrendLine;
using App.DAL.MarsDataAccess.CsvExportStaticData;
using App.DAL.MarsDataAccess.ParameterAccess;
using Mars.App.Classes.DAL.MarsDBContext;
using App.DAL.MarsDataAccess.Sizing.KpiDataHolders;
using App.Entities.Graphing;
using App.Entities.Graphing.Parameters;
using Mars.App.Classes.DAL.MarsDBContext;
using Mars.App.Classes.DAL.Sizing.GenericSizingDataAccess;
using KpiCalculationType = App.BLL.ReportEnums.FleetSize.KpiCalculationType;

namespace App.DAL.MarsDataAccess.Sizing
{
    internal static class KpiDataAccess
    {
        private const string MaxSeriesName = "Max";
        private const string MinSeriesName = "Min";
        private const string TrendSeriesName = "Trend";
        internal const string KpiSeriesName = "Kpi";

        internal static List<GraphSeries> GetKpiDataNew(Dictionary<string, string> parameters, KpiCalculationType kpiType, FutureTrendDataType futureTrendDataType)
        {
            using (var dataContext = new MarsDBDataContext(MarsConnection.ConnectionString))
            {
                //var rawData = GetRawKpiDataNew(dataContext, parameters, futureTrendDataType);
                var rawData = GenericSizingDataAccess.GetKpiRawData(dataContext, parameters, futureTrendDataType);

                var kpiGraphingData = from gd in rawData
                                      orderby gd.ReportDate ascending
                                      select new KpiGraphDataHolder
                                      {
                                          ReportDate = gd.ReportDate,
                                          Kpi = kpiType == KpiCalculationType.OperationalUtilization ? gd.ExpectedFleet == 0 ? 0 :
                                                                                   gd.Forecast / gd.ExpectedFleet :
                                                kpiType == KpiCalculationType.IdleFleetPercentage ? gd.ExpectedFleet == 0 ? 0 :
                                                                                   (gd.ExpectedFleet - gd.Forecast) / gd.ExpectedFleet :
                                                kpiType == KpiCalculationType.IdleFleet ?
                                                                                   gd.ExpectedFleet - gd.Forecast : 0
                                      };



                return GraphKpiData(kpiGraphingData);
            }
        }

        internal static List<GraphSeries> GetHistoricalKpiData(Dictionary<string, string> parameters, KpiCalculationType kpiType)
        {
            using (var dataContext = new MarsDBDataContext(MarsConnection.ConnectionString))
            {
                var rawData = GetFrozenZoneRawKpiData(dataContext, parameters);

                var groupedData = from fc in rawData
                                  group fc by fc.ReportDate
                                      into g
                                      orderby g.Key ascending
                                      select g;

                var kpiGraphingData = from gd in groupedData
                                      select new KpiGraphDataHolder
                                      {
                                          ReportDate = gd.Key,
                                          Kpi = kpiType == KpiCalculationType.OperationalUtilization ? gd.Sum(d => d.OperationalFleet) == 0 ? 0 :
                                                                              gd.Sum(d => d.OnRent) / gd.Sum(d => d.OperationalFleet) :
                                                kpiType == KpiCalculationType.IdleFleetPercentage ? gd.Sum(d => d.TotalFleet) == 0 ? 0 :
                                                                              gd.Sum(d => d.IdleFleet) / gd.Sum(d => d.TotalFleet) :
                                                kpiType == KpiCalculationType.IdleFleet ? gd.Sum(d => d.IdleFleet) : 0
                                      };

                return GraphKpiData(kpiGraphingData);

            }
        }

        internal static string GetKpiCsvHeader(Dictionary<string, string> parameters, KpiCalculationType kpiType, FutureTrendDataType futureTrendDataType)
        {
            var csvHeader = new StringBuilder();
            csvHeader.Append(string.Format("{0}, {1},", CsvExportHeaders.ReportDate, CsvExportHeaders.Country));

            if (!string.IsNullOrEmpty(parameters[ParameterNames.Pool])) csvHeader.Append(CsvExportHeaders.Pool + ",");
            if (!string.IsNullOrEmpty(parameters[ParameterNames.LocationGroup])) csvHeader.Append(CsvExportHeaders.LocationGroup + ",");
            if (!string.IsNullOrEmpty(parameters[ParameterNames.CarSegment])) csvHeader.Append(CsvExportHeaders.CarSegment + ",");
            if (!string.IsNullOrEmpty(parameters[ParameterNames.CarClassGroup])) csvHeader.Append(CsvExportHeaders.CarClassGroup + ",");
            if (!string.IsNullOrEmpty(parameters[ParameterNames.CarClass])) csvHeader.Append(CsvExportHeaders.CarClass + ",");

            switch (kpiType)
            {
                case KpiCalculationType.OperationalUtilization:
                    switch (futureTrendDataType)
                    {
                        case FutureTrendDataType.Constrained:
                            csvHeader.Append(CsvExportHeaders.UltilizationConstrained);
                            break;
                        case FutureTrendDataType.Unconstrained:
                            csvHeader.Append(CsvExportHeaders.UltilizationUnonstrained);
                            break;
                        case FutureTrendDataType.AlreadyBooked:
                            csvHeader.Append(CsvExportHeaders.UltilizationAlreadyBooked);
                            break;
                    }
                    break;
                case KpiCalculationType.IdleFleet:
                    csvHeader.Append(CsvExportHeaders.IdleFleet);
                    break;
                case KpiCalculationType.IdleFleetPercentage:
                    csvHeader.Append(CsvExportHeaders.IdleFleetPercentage);
                    break;
            }

            return csvHeader.ToString();
        }

        internal static string GetKpiExcelData(Dictionary<string, string> parameters, KpiCalculationType kpiType, FutureTrendDataType futureTrendDataType)
        {
            var csvData = new StringBuilder();

            using (var dataContext = new MarsDBDataContext(MarsConnection.ConnectionString))
            {
                var additionalColumns = GenerateAdditionalColumns(parameters, dataContext);

                //var rawData = GetRawKpiDataNew(dataContext, parameters, futureTrendDataType, true);
                var rawData = GenericSizingDataAccess.GetKpiRawData(dataContext, parameters, futureTrendDataType);

                var groupedData = from rw in rawData
                                  select new KpiExcelDataHolder
                                  {
                                      ReportDate = rw.ReportDate,
                                      Country = rw.Country,
                                      Kpi = kpiType == KpiCalculationType.OperationalUtilization ? rw.ExpectedFleet == 0 ? 0 :
                                                                                   (rw.Forecast / rw.ExpectedFleet) * 100 :
                                                kpiType == KpiCalculationType.IdleFleetPercentage ? rw.ExpectedFleet == 0 ? 0 :
                                                                                   (rw.ExpectedFleet - rw.Forecast) * 100 / rw.ExpectedFleet :
                                                kpiType == KpiCalculationType.IdleFleet ?
                                                                                   rw.ExpectedFleet - rw.Forecast : 0
                                  };


                var excelData = from gd in groupedData.OrderBy(d => d.ReportDate).ThenBy(d => d.Country)
                                select string.Format("{0},{1},{2}{3}\n", gd.ReportDate.Value.ToShortDateString(), gd.Country, additionalColumns,
                                                    Math.Round(gd.Kpi, 0, MidpointRounding.AwayFromZero));

                excelData.ToList().ForEach(d => csvData.Append(d.ToString()));

                return csvData.ToString();
            }
        }

        internal static string GetFrozenZoneKpiExcelData(Dictionary<string, string> parameters, KpiCalculationType kpiType, FutureTrendDataType futureTrendDataType)
        {
            var csvData = new StringBuilder();

            using (var dataContext = new MarsDBDataContext(MarsConnection.ConnectionString))
            {
                var additionalColumns = GenerateAdditionalColumns(parameters, dataContext);
                var rawData = GetFrozenZoneRawKpiData(dataContext, parameters);

                var groupedData = from rw in rawData
                                  group rw by new { rw.ReportDate, rw.Country } into gd
                                  join c in dataContext.COUNTRies on gd.Key.Country equals c.country1
                                  select new KpiExcelDataHolder
                                             {
                                                 ReportDate = gd.Key.ReportDate,
                                                 Country = c.country_description,
                                                 Kpi = kpiType == KpiCalculationType.OperationalUtilization ? gd.Sum(d => d.OperationalFleet) == 0 ? 0 :
                                                                    gd.Sum(d => d.OnRent) * 100 / gd.Sum(d => d.OperationalFleet) :
                                                      kpiType == KpiCalculationType.IdleFleetPercentage ? gd.Sum(d => d.TotalFleet) == 0 ? 0 :
                                                                                    gd.Sum(d => d.IdleFleet) * 100 / gd.Sum(d => d.TotalFleet) :
                                                      kpiType == KpiCalculationType.IdleFleet ? gd.Sum(d => d.IdleFleet) : 0
                                             };

                var excelData = from gd in groupedData.OrderBy(d => d.ReportDate).ThenBy(d => d.Country)
                                select string.Format("{0},{1},{2}{3}\n", gd.ReportDate.Value.ToShortDateString(), gd.Country, additionalColumns,
                                                    Math.Round(gd.Kpi, 0, MidpointRounding.AwayFromZero));

                excelData.ToList().ForEach(d => csvData.Append(d.ToString()));
                return csvData.ToString();
            }
        }

        private static string GenerateAdditionalColumns(IDictionary<string, string> parameters, MarsDBDataContext dataContext)
        {
            var additionalColumns = new StringBuilder();

            if (!string.IsNullOrEmpty(parameters[ParameterNames.Pool]))
            {
                additionalColumns.Append((from cc in dataContext.CMS_POOLs
                                          where cc.cms_pool_id == int.Parse(parameters[ParameterNames.Pool])
                                          select cc.cms_pool1).First() + ",");
            }

            if (!string.IsNullOrEmpty(parameters[ParameterNames.LocationGroup]))
            {
                additionalColumns.Append((from cc in dataContext.CMS_LOCATION_GROUPs
                                          where cc.cms_location_group_id == int.Parse(parameters[ParameterNames.LocationGroup])
                                          select cc.cms_location_group1).First() + ",");
            }

            if (!string.IsNullOrEmpty(parameters[ParameterNames.CarSegment]))
            {
                additionalColumns.Append((from cc in dataContext.CAR_SEGMENTs
                                          where cc.car_segment_id == int.Parse(parameters[ParameterNames.CarSegment])
                                          select cc.car_segment1).First() + ",");
            }

            if (!string.IsNullOrEmpty(parameters[ParameterNames.CarClassGroup]))
            {
                additionalColumns.Append((from cc in dataContext.CAR_CLASSes
                                          where cc.car_class_id == int.Parse(parameters[ParameterNames.CarClassGroup])
                                          select cc.car_class1).First() + ",");
            }

            if (!string.IsNullOrEmpty(parameters[ParameterNames.CarClass]))
            {
                additionalColumns.Append((from cc in dataContext.CAR_GROUPs
                                          where cc.car_group_id == int.Parse(parameters[ParameterNames.CarClass])
                                          select cc.car_group1).First() + ",");
            }

            return additionalColumns.ToString();
        }

        private static List<GraphSeries> GraphKpiData(IQueryable<KpiGraphDataHolder> kpiGraphingData)
        {
            var dateTimes = new List<object>();
            var kpi = new List<double>();
            var max = new List<double>();
            var trend = new List<double>();
            var min = new List<double>();



            if (kpiGraphingData.Any())
            {

                foreach (var ft in kpiGraphingData)
                {
                    if (ft.Kpi.HasValue)
                    {
                        kpi.Add((double)ft.Kpi);
                    }
                    else
                    {
                        kpi.Add(0);
                    }

                    if (kpi.Count > 1 && kpi[kpi.Count - 2] == 0)
                    {
                        kpi[kpi.Count - 2] = kpi[kpi.Count - 1];
                    }

                    dateTimes.Add(ft.ReportDate);
                }

                var maxValue = kpi.Max();
                var minValue = kpi.Min();
                foreach (var t in kpi)
                {
                    max.Add(maxValue);
                    min.Add(minValue);
                }



                var trendLine = TrendLineCalculator.CalculateLinearRegression(kpi.ToArray());
                trend = trendLine.GetTrendLineYPoints();
            }


            var seriesInformation = new List<GraphSeries>()
                                        {
                                            new GraphSeries(MaxSeriesName)
                                                {GraphColour = Color.MediumSeaGreen, Xvalue = dateTimes, Yvalue = max},
                                            new GraphSeries(TrendSeriesName)
                                                {GraphColour = Color.DeepSkyBlue, Xvalue = dateTimes, Yvalue = trend},
                                            new GraphSeries(MinSeriesName)
                                                {GraphColour = Color.Red, Xvalue = dateTimes, Yvalue = min},
                                            new GraphSeries(KpiSeriesName)
                                                {GraphColour = Color.Blue, Xvalue = dateTimes, Yvalue = kpi}
                                        };

            return seriesInformation;
        }


        internal static IQueryable<KpiRawDataHolderOld> GetFrozenZoneRawKpiData(MarsDBDataContext dataContext, Dictionary<string, string> parameters)
        {
            var fullDataSet = from fc in dataContext.MARS_CMS_FORECAST_HISTORies
                              select fc;

            var restrictedData = ForecastParameterRestriction.RestrictHistoricalForecastByParameters(parameters, fullDataSet, dataContext);

            var joinedData = from rd in restrictedData
                             select new KpiRawDataHolderOld
                             {
                                 OnRent = rd.CURRENT_ONRENT ?? 0,
                                 OperationalFleet = rd.OPERATIONAL_FLEET ?? 0 + rd.FLEET_MOVEMENT_FROZEN ?? 0,
                                 IdleFleet = (rd.TOTAL_FLEET - rd.CURRENT_ONRENT) ?? 0,
                                 TotalFleet = rd.TOTAL_FLEET ?? 0,
                                 ReportDate = rd.REP_DATE,
                                 Country = rd.COUNTRY
                             };

            return joinedData;
        }

        internal static void RecalculateTrendMinMaxLines(List<GraphSeries> graphData)
        {

            var maxSeries = graphData.First(d => d.SeriesName == MaxSeriesName).Yvalue;
            var minSeries = graphData.First(d => d.SeriesName == MinSeriesName).Yvalue;
            var kpiSeries = graphData.First(d => d.SeriesName == KpiSeriesName).Yvalue;
            if (kpiSeries.Count == 0) return;
            var maxValue = kpiSeries.Max();
            var minValue = kpiSeries.Min();

            for (var i = 0; i < maxSeries.Count; i++)
            {
                maxSeries[i] = maxValue;
                minSeries[i] = minValue;
            }

            var trendLine = TrendLineCalculator.CalculateLinearRegression(kpiSeries.ToArray());
            var trend = trendLine.GetTrendLineYPoints();
            graphData.First(d => d.SeriesName == TrendSeriesName).Yvalue = trend;
        }
    }

}