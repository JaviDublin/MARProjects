using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.DataVisualization.Charting;
using App.BLL.Cache;
using App.BLL.EventArgs;
using App.BLL.ExtensionMethods;
using App.BLL.ReportEnums.FleetSize;
using App.DAL.MarsDataAccess.ParameterAccess;
using App.DAL.MarsDataAccess.ParameterAccess.DataHolders;
using App.DAL.MarsDataAccess.Sizing;
using App.Entities.Graphing;
using App.Entities.Graphing.Parameters;
using App.UserControls;
using App.UserControls.Parameters;
using Mars.App.Classes.BLL.ExtensionMethods;
using Resources;

namespace App.Reports.Sizing
{
    public partial class KPI : System.Web.UI.Page
    {
        private GraphData GraphInformation
        {
            get
            {
                return (GraphData)Session["KPIGraphInformation"];
            }
            set { Session["KPIGraphInformation"] = value; }
        }

        private List<LocationGroupHolder> _locationGroupData;

        protected void Page_Init(object sender, EventArgs e)
        {
            _locationGroupData = ParameterCache.GetAllLocationGroups();

            ccKPI.GraphTypesAllowed = new List<SeriesChartType>
                                        {
                                            SeriesChartType.Line,
                                            SeriesChartType.StepLine,
                                            SeriesChartType.Spline,
                                            SeriesChartType.Column,
                                            SeriesChartType.Point
                                        };

            if (GraphInformation == null)
            {
                GraphInformation = new GraphData
                {
                    IsXValueDate = true,
                    XAxisDateFormat = "dd/MM (ddd)",
                    AllowNegativeValuesOnYAxis = true,
                    LabelFormat = "0%",
                    YAxisNumberFormat = "0%",
                    ChartDescription = LocalizedReportPages.KpiDescription,
                    DataPointsXAxisTooltip = "#VALX",
                    DataPointsYAxisTooltip = "#VALY{0%}",
                    ChartName = LocalizedReportPages.KpiReportName,
                    YAxisZoom = "75",
                    ReportParameters = this.GetDefaultParameters()
                };

                GraphInformation.ShowLabelSeriesNames.Add(KpiDataAccess.KpiSeriesName);
                GeneralParams.ReportTypeControl.SelectedTimeZone.SelectedIndex = 1;     //Default to Frozen Zone
            }

            GeneralParams.ReportTypeControl.ShowKpiParameter = true;
            GeneralParams.ReportTypeControl.ShowTimeZoneParameter = true;
            GeneralParams.DynamicReportParametersControl.ShowQuickLocationGroupBox = true;
            ccKPI.GraphInformation = GraphInformation;

            GeneralParams.ParamsHolder = GraphInformation.ReportParameters;
            GeneralParams.SelectedParameters = GraphInformation.SelectedParameters;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.LoadComplete += PageLoadComplete;
        }

        protected void PageLoadComplete(object sender, EventArgs e)
        {
            DataType selectedTimeZoneType;
            Enum.TryParse(GeneralParams.ReportTypeControl.SelectedTimeZone.SelectedValue, out selectedTimeZoneType);
            GeneralParams.ReportTypeControl.ShowForecastTypeParameter = selectedTimeZoneType != DataType.FrozenZone;
            

            if (!string.IsNullOrEmpty(GeneralParams.DynamicReportParametersControl.QuickSelectedValue))
            {
                this.CheckQuickLocationGroupSelected(_locationGroupData, GeneralParams.DynamicReportParametersControl.QuickSelectedValue, GeneralParams.SelectedParameters, GraphInformation.ReportParameters);
                upnlGeneralParameters.Update();
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            DataType selectedTimeZoneType;
            Enum.TryParse(GeneralParams.ReportTypeControl.SelectedTimeZone.SelectedValue, out selectedTimeZoneType);
            FutureTrendDataType futureTrendDataType;
            Enum.TryParse(GeneralParams.ReportTypeControl.SelectedForecastType.SelectedValue, out futureTrendDataType);
            
            
            GeneralParams.DynamicReportParametersControl.DateRangePickerControl.SetDates(selectedTimeZoneType == DataType.FrozenZone);
            if (GraphInformation.RefreshData)
            {
                var fromDate = DateTime.Parse(GeneralParams.SelectedParameters.ContainsKey(ParameterNames.FromDate) ? GeneralParams.SelectedParameters[ParameterNames.FromDate] : null);
                var toDate = DateTime.Parse(GeneralParams.SelectedParameters.ContainsKey(ParameterNames.ToDate) ? GeneralParams.SelectedParameters[ParameterNames.ToDate] : null);

                var kpiTypeSelection = int.Parse(GeneralParams.ReportTypeControl.SelectedKpi.Value);
                var kpiShowPercentage = GeneralParams.ReportTypeControl.ShowKpiAsPercentage;


                var kpiType = kpiTypeSelection == 1
                              ? KpiCalculationType.OperationalUtilization
                              : kpiTypeSelection == 2 && kpiShowPercentage
                                    ? KpiCalculationType.IdleFleetPercentage
                                    : KpiCalculationType.IdleFleet;
                

                if (!kpiShowPercentage && kpiTypeSelection == 2)
                {
                    GraphInformation.LabelFormat = "#,##0";
                    GraphInformation.DataPointsYAxisTooltip = "#VALY{0}";
                    GraphInformation.YAxisNumberFormat = "#,##0";
                }
                else
                {
                    GraphInformation.LabelFormat = "0%";
                    GraphInformation.DataPointsYAxisTooltip = "#VALY{0%}";
                    GraphInformation.YAxisNumberFormat = "0%";
                }

                var doesDateRangeSpanToday = fromDate.Date < DateTime.Now.Date && toDate.Date > DateTime.Now.Date;
                string timeZoneTitle;
                if (doesDateRangeSpanToday)
                {
                    var selectedParameters = GeneralParams.SelectedParameters;

                    selectedParameters[ParameterNames.ToDate] = DateTime.Now.AddDays(-1).Date.ToString();
                    var pastSeriesData = KpiDataAccess.GetHistoricalKpiData(selectedParameters, kpiType);
                    selectedParameters[ParameterNames.FromDate] = DateTime.Now.Date.ToString();
                    selectedParameters[ParameterNames.ToDate] = toDate.ToString();
                    var futureSeriesData = KpiDataAccess.GetKpiDataNew(selectedParameters, kpiType, futureTrendDataType);

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
                    timeZoneTitle = string.Format("{0} ({2}) & {1} ", GeneralParams.ReportTypeControl.SelectedTimeZone.Items[0].Text,
                                        GeneralParams.ReportTypeControl.SelectedTimeZone.Items[1].Text,
                                        GeneralParams.ReportTypeControl.SelectedForecastType.SelectedItem.Text);
                }
                else
                {
                    GraphInformation.SeriesData = selectedTimeZoneType == DataType.FrozenZone
                            ? KpiDataAccess.GetHistoricalKpiData(GeneralParams.SelectedParameters, kpiType)
                            : KpiDataAccess.GetKpiDataNew(GeneralParams.SelectedParameters, kpiType, futureTrendDataType);
                    
                    timeZoneTitle = GeneralParams.ReportTypeControl.SelectedTimeZone.SelectedItem.Text;
                }
                KpiDataAccess.RecalculateTrendMinMaxLines(GraphInformation.SeriesData);

                GraphInformation.DataDate = ParameterDataAccess.GetLastDateFromCmsForecast();

               
                GraphInformation.TitleAdditional = string.Format("{0} - {1} {2}",
                    timeZoneTitle,
                    GeneralParams.ReportTypeControl.SelectedKpi.Text, 
                    kpiShowPercentage ? "%" : "");
                GraphInformation.CalculateYEntriesCount();

                GraphInformation.RefreshData = false;
            }
        }

        protected void btnRefreshChart_Click(object sender, EventArgs e)
        {
            GraphInformation.RefreshData = true;
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
                var kpiTypeSelection = int.Parse(GeneralParams.ReportTypeControl.SelectedKpi.Value);
                var kpiShowPercentage = GeneralParams.ReportTypeControl.ShowKpiAsPercentage;

                FutureTrendDataType futureTrendDataType;
                Enum.TryParse(GeneralParams.ReportTypeControl.SelectedForecastType.SelectedValue, out futureTrendDataType);
                DataType selectedTimeZoneType;
                Enum.TryParse(GeneralParams.ReportTypeControl.SelectedTimeZone.SelectedValue, out selectedTimeZoneType);

                GeneralParams.DynamicReportParametersControl.DateRangePickerControl.SetDates(selectedTimeZoneType == DataType.FrozenZone);

                var kpiType = kpiTypeSelection == 1
                      ? KpiCalculationType.OperationalUtilization
                      : kpiTypeSelection == 2 && kpiShowPercentage
                            ? KpiCalculationType.IdleFleetPercentage
                            : KpiCalculationType.IdleFleet;

                var fromDate = DateTime.Parse(GeneralParams.SelectedParameters.ContainsKey(ParameterNames.FromDate) ? GeneralParams.SelectedParameters[ParameterNames.FromDate] : null);
                var toDate = DateTime.Parse(GeneralParams.SelectedParameters.ContainsKey(ParameterNames.ToDate) ? GeneralParams.SelectedParameters[ParameterNames.ToDate] : null);

                var doesDateRangeSpanToday = fromDate.Date < DateTime.Now.Date && toDate.Date > DateTime.Now.Date;


                var csvData = new StringBuilder();
                csvData.AppendLine(KpiDataAccess.GetKpiCsvHeader(GeneralParams.SelectedParameters, kpiType, futureTrendDataType));
                

                if (doesDateRangeSpanToday)
                {
                    var selectedParameters = GeneralParams.SelectedParameters;

                    selectedParameters[ParameterNames.ToDate] = DateTime.Now.AddDays(-1).Date.ToString();
                    csvData.Append(KpiDataAccess.GetFrozenZoneKpiExcelData(selectedParameters, kpiType, futureTrendDataType));
                    selectedParameters[ParameterNames.FromDate] = DateTime.Now.Date.ToString();
                    selectedParameters[ParameterNames.ToDate] = toDate.ToString();
                    csvData.Append(KpiDataAccess.GetKpiExcelData(selectedParameters, kpiType, futureTrendDataType));
                    selectedParameters[ParameterNames.FromDate] = fromDate.ToString();
                    selectedParameters[ParameterNames.ToDate] = toDate.ToString();
                }
                else
                {
                    csvData.Append(selectedTimeZoneType == DataType.DailyChanging
                                       ? KpiDataAccess.GetKpiExcelData(GeneralParams.SelectedParameters, kpiType,
                                                                       futureTrendDataType)
                                       : KpiDataAccess.GetFrozenZoneKpiExcelData(GeneralParams.SelectedParameters,
                                                                                 kpiType, futureTrendDataType));
                }

                Session["ExportData"] = csvData.ToString();
                Session["ExportFileName"] = "KpiExport";

                handled = true;
            }

            return handled;
        }

    }
}