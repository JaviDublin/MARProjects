using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using App.BLL.Utilities;
using App.Entities.Graphing.Parameters;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;

namespace App.Entities.Graphing
{
    /// <summary>
    /// Summary description for GenericGraphDataHolder
    /// </summary>
    [Serializable]
    public class GraphData
    {
        public string XAxisTitle { get; set; }
        public string YAxisTitle { get; set; }

        public string TitleDate { get; set; }
        public int YEntriesCount { get; set; }

        public bool ShowingSeriesInformation { get; set; }
        public bool RefreshData { get; set; }

        public bool HasReportTypeChanged { get; set; }
        public bool HaveDatesChanged { get; set; }
        public bool HaveDynamicParametersChanged { get; set; }
        

        public bool UsingCachedGraphingData { get; set; }
        public bool CheckIfCachedDataCanBeUsed { get; set; }
        public DateTime DataDate { get; set; }

        public List<ReportParameter> ReportParameters { get; set; }
        public Dictionary<string, string> SelectedParameters { get; set; }
        private List<GraphSeries> _seriesData;
        private List<GraphSeries> _seriesDataWeekly;                //Only used for Supply Analysis 


        public LocationLogic CmsOpsLogic { get; set; }
        public bool ShowGridData { get; set; }
        public DataTable GridData { get; set; }

        public string Title { get; set; }
        public string TitleAdditional { get; set; }
        public string ChartName { get; set; }
        public string ChartDescription { get; set; }
        public int ChartLineWidth { get; set; }
        public string SeriesDrawingStyle { get; set; }
        public int BranchForDrilldown { get; set; }

        //Config Section
        public bool AllowNegativeValuesOnYAxis { get; set; }
        public bool HideSeriesInfo { get; set; }
        public bool IsXValueDate { get; set; }
        public bool AllowDrillDown { get; set; }
        public bool ShowWeeklyMinimum { get; set; }

        public Color NegativeValuesColour { get; set; }
        public Color LightXAxisLineColour { get; set; }
        public Color DarkXAxisLineColour { get; set; }

        public bool ShowGraphLinkingButton { get; set; }
        public string GraphLinkingText { get; set; }
        public string GraphLinkingPage { get; set; }
        

        public string XAxisDateFormat { get; set; }
        public string YAxisNumberFormat { get; set; }
        public string LabelFormat { get; set; }

        public string DataPointsXAxisTooltip { get; set; }
        public string DataPointsYAxisTooltip { get; set; }

        public string YAxisZoom { get; set; }

        public List<string> HiddenSeriesNames { get; set; }
        public List<string> ShowLabelSeriesNames { get; set; }

        public bool UseWeeklyData { get; set; }

        public List<GraphSeries> SeriesData
        {
            get { return UseWeeklyData ? _seriesDataWeekly : _seriesData; }
            set
            {
                _seriesData = value;    
                foreach (var hsn in HiddenSeriesNames) {
                    _seriesData.Where(d => d.SeriesName == hsn).ForEach(d => d.Displayed = false);
                }
                foreach (var slsn in ShowLabelSeriesNames) {
                    _seriesData.Where(d => d.SeriesName == slsn).ForEach(d => d.ShowLabel = true);
                }
            }
        }

        public List<GraphSeries> WeeklySeriesData
        {
            set
            {
                _seriesDataWeekly = value;    
            }
        }



        public GraphData()
        {
            XAxisTitle = string.Empty;
            YAxisTitle = string.Empty;
            YAxisZoom = "0";
            ChartDescription = string.Empty;
            LabelFormat = "#,###";
            ChartLineWidth = 3;
            LightXAxisLineColour = Color.LightGray;
            DarkXAxisLineColour = Color.Gray;
            NegativeValuesColour = Color.Empty;
            ShowingSeriesInformation = true;
            SelectedParameters = new Dictionary<string, string>();
            HiddenSeriesNames = new List<string>();
            ShowLabelSeriesNames = new List<string>();
            
        }

        public void CalculateYEntriesCount()
        {
            YEntriesCount = SeriesData.Aggregate(0, (current, graphSeries) => graphSeries.Xvalue.Count > current ? graphSeries.Xvalue.Count : current);
        }

        
    }
}