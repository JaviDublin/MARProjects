using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.DataVisualization.Charting;
using App.BLL.Cache;
using App.BLL.EventArgs;
using App.BLL.ExtensionMethods;
using App.BLL.ReportEnums.FleetSize;
using App.DAL.MarsDataAccess.CsvExportStaticData;
using App.DAL.MarsDataAccess.Forecasting;
using App.DAL.MarsDataAccess.ParameterAccess;
using App.DAL.MarsDataAccess.ParameterAccess.DataHolders;
using App.Entities.Graphing;
using App.Entities.Graphing.Parameters;
//using App.UserControls.Parameters;
using Mars.App.Classes.BLL.ExtensionMethods;
using Resources;

namespace App.Reports.Forecasting
{
    public partial class Forecast : System.Web.UI.Page
    {
        private GraphData GraphInformation
        {
            get
            {
                return (GraphData)Session["ForecastGraphInformation"];
            }
            set { Session["ForecastGraphInformation"] = value; }
        }

        private List<LocationGroupHolder> _locationGroupData;

        protected void Page_Init(object sender, EventArgs e)
        {
            _locationGroupData = ParameterCache.GetAllLocationGroups();

            ccForecast.GraphTypesAllowed = new List<SeriesChartType>
                                        {
                                            SeriesChartType.Line,
                                            SeriesChartType.StepLine,
                                            SeriesChartType.Spline,
                                        };

            if (GraphInformation == null)
            {
                GraphInformation = new GraphData
                {
                    IsXValueDate = true,
                    XAxisDateFormat = "dd/MM (ddd)",
                    YAxisNumberFormat = "#,##0",
                    DataPointsXAxisTooltip = "#VALX (day #INDEX)",
                    DataPointsYAxisTooltip = "#VALY{0,0}",
                    LabelFormat = "#,##0",
                    ChartDescription = LocalizedReportPages.ForecastDescription,
                    ChartName = LocalizedReportPages.ForecastReportName,
                    YAxisZoom = "0",
                    ChartLineWidth = 2,
                    ReportParameters = this.GetDefaultParameters()
                };

                GraphInformation.HiddenSeriesNames.Add("Top Down");
                GraphInformation.HiddenSeriesNames.Add("On Rent LY");
                GraphInformation.HiddenSeriesNames.Add("Bottom Up 1");
                GraphInformation.HiddenSeriesNames.Add("Bottom Up 2");
                GraphInformation.HiddenSeriesNames.Add("Reconciliation");
            }

            GeneralParams.ReportTypeControl.ShowTimeZoneParameter = true;
  
            GeneralParams.ParamsHolder = GraphInformation.ReportParameters;
            GeneralParams.SelectedParameters = GraphInformation.SelectedParameters;

            GeneralParams.DynamicReportParametersControl.DateRangePickerControl.DefaultDateRangeValueSelected = "90";

            ccForecast.GraphInformation = GraphInformation;
            GeneralParams.ExportType = 1;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.LoadComplete += PageLoadComplete;
        }

        protected void PageLoadComplete(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(GeneralParams.DynamicReportParametersControl.QuickSelectedValue))
            {
                this.CheckQuickLocationGroupSelected(_locationGroupData, GeneralParams.DynamicReportParametersControl.QuickSelectedValue, GeneralParams.SelectedParameters, GraphInformation.ReportParameters);
                upnlGeneralParameters.Update();
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            GraphInformation.HasReportTypeChanged = GeneralParams.ReportTypeControl.HasReportTypeChanged;

            DataType selectedTimeZoneType;
            Enum.TryParse(GeneralParams.ReportTypeControl.SelectedTimeZone.SelectedValue, out selectedTimeZoneType);

            GeneralParams.DynamicReportParametersControl.DateRangePickerControl.SetDates(selectedTimeZoneType == DataType.FrozenZone ? true : false);

            if (GraphInformation.RefreshData)
            {
                
                var fromDate = DateTime.Parse(GeneralParams.SelectedParameters.ContainsKey(ParameterNames.FromDate) ? GeneralParams.SelectedParameters[ParameterNames.FromDate] : null);
                var toDate = DateTime.Parse(GeneralParams.SelectedParameters.ContainsKey(ParameterNames.ToDate) ? GeneralParams.SelectedParameters[ParameterNames.ToDate] : null);

                var doesDateRangeSpanToday = fromDate.Date < DateTime.Now.Date && toDate.Date > DateTime.Now.Date;

                if(doesDateRangeSpanToday)
                {
                    var selectedParameters = GeneralParams.SelectedParameters;
                    
                    selectedParameters[ParameterNames.ToDate] = DateTime.Now.AddDays(-1).Date.ToString();
                    var pastSeriesData = ForecastDataAccess.GetForecastGraphingData(selectedParameters, DataType.FrozenZone);
                    selectedParameters[ParameterNames.FromDate] = DateTime.Now.Date.ToString();
                    selectedParameters[ParameterNames.ToDate] = toDate.ToString();
                    var futureSeriesData = ForecastDataAccess.GetForecastGraphingData(selectedParameters, DataType.DailyChanging);

                    var count = 0;
                    pastSeriesData[0].Xvalue.AddRange(futureSeriesData[0].Xvalue);
                    foreach (var sd in futureSeriesData)
                    {
                        pastSeriesData[count].Yvalue.AddRange(sd.Yvalue);
                        count++;
                    }
                    selectedParameters[ParameterNames.FromDate] = fromDate.ToString();
                    selectedParameters[ParameterNames.ToDate] = toDate.ToString();
                    GraphInformation.SeriesData = pastSeriesData;
                    GraphInformation.TitleAdditional = GeneralParams.ReportTypeControl.SelectedTimeZone.Items[0].Text 
                                                + " & " + GeneralParams.ReportTypeControl.SelectedTimeZone.Items[1].Text;
                }
                else
                {
                    var seriesData = ForecastDataAccess.GetForecastGraphingData(GeneralParams.SelectedParameters, selectedTimeZoneType);
                    if (selectedTimeZoneType == DataType.FrozenZone) seriesData = seriesData.Where(s => s.SeriesName != "Already Booked").ToList();

                    GraphInformation.SeriesData = seriesData;
                    GraphInformation.TitleAdditional =
                        GeneralParams.ReportTypeControl.SelectedTimeZone.SelectedItem.Text;
                }

                GraphInformation.DataDate = ParameterDataAccess.GetLastDateFromCmsForecast();
                GraphInformation.CalculateYEntriesCount();
                GraphInformation.RefreshData = false;
            }
        }

        protected override bool OnBubbleEvent(object source, EventArgs args)
        {
            var handled = false;

            if (args is RefreshGraphEventArgs)
            {
                GraphInformation.RefreshData = true;
                handled = true;
            }

            if (args is ExportToExcelEventArgs)
            {
                DataType selectedTimeZoneType;
                Enum.TryParse(GeneralParams.ReportTypeControl.SelectedTimeZone.SelectedValue, out selectedTimeZoneType);
                GeneralParams.DynamicReportParametersControl.DateRangePickerControl.SetDates(selectedTimeZoneType == DataType.FrozenZone ? true : false);
                FutureTrendDataType futureTrendDataType;
                Enum.TryParse(GeneralParams.ReportTypeControl.SelectedForecastType.SelectedValue, out futureTrendDataType);
                var siteGroup = int.Parse(GeneralParams.ExcelExportControl.SelectedGroupBySite);
                var fleetGroup = int.Parse(GeneralParams.ExcelExportControl.SelectedGroupByFleet);

                
                var fromDate = DateTime.Parse(GeneralParams.SelectedParameters.ContainsKey(ParameterNames.FromDate) ? GeneralParams.SelectedParameters[ParameterNames.FromDate] : null);
                var toDate = DateTime.Parse(GeneralParams.SelectedParameters.ContainsKey(ParameterNames.ToDate) ? GeneralParams.SelectedParameters[ParameterNames.ToDate] : null);

                var doesDateRangeSpanToday = fromDate.Date < DateTime.Now.Date && toDate.Date > DateTime.Now.Date;


                var csvData = new StringBuilder();
                
                csvData.Append(ForecastDataAccess.GeForecastCsvHeader(siteGroup, fleetGroup));


                if (doesDateRangeSpanToday)
                {
                    var selectedParameters = GeneralParams.SelectedParameters;

                    selectedParameters[ParameterNames.ToDate] = DateTime.Now.AddDays(-1).Date.ToString();
                    csvData.Append(ForecastDataAccess.GetForecastExcelData(selectedParameters,
                                                                      futureTrendDataType, siteGroup,
                                                                      fleetGroup, DataType.FrozenZone));
                    selectedParameters[ParameterNames.FromDate] = DateTime.Now.Date.ToString();
                    selectedParameters[ParameterNames.ToDate] = toDate.ToString();
                    csvData.Append(ForecastDataAccess.GetForecastExcelData(selectedParameters,
                                                  futureTrendDataType, siteGroup,
                                                  fleetGroup, DataType.DailyChanging));
                    selectedParameters[ParameterNames.FromDate] = fromDate.ToString();
                    selectedParameters[ParameterNames.ToDate] = toDate.ToString();
                }
                else
                {
                    csvData.Append(ForecastDataAccess.GetForecastExcelData(GeneralParams.SelectedParameters,
                                                                      futureTrendDataType, siteGroup,
                                                                      fleetGroup, selectedTimeZoneType));
                }

                

                Session["ExportData"] = csvData.ToString();
                Session["ExportFileName"] = "ForecastExport";

                handled = true;
            }

            return handled;
        }

  
    }

}