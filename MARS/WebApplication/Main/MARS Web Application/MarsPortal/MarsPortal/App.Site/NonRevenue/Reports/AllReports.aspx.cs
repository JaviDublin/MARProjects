using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.App.Classes.Phase4Bll.Availability;
using Mars.App.Classes.Phase4Dal.Availability;
using Mars.App.Classes.Phase4Dal.Enumerators;
using Mars.App.Classes.Phase4Dal.NonRev;
using Mars.App.Classes.Phase4Dal.NonRev.Entities;
using ComparisonDataAccess = Mars.App.Classes.Phase4Dal.NonRev.ComparisonDataAccess;
using HistoricalTrendDataAccess = Mars.App.Classes.Phase4Dal.NonRev.HistoricalTrendDataAccess;

namespace Mars.App.Site.NonRevenue.Reports
{
    public partial class AllReports : Page
    {
        public int SelectedTab
        {
            get { return int.Parse(hfSelectedTab.Value); }
        }

        public int SelectedSubTab
        {
            get { return int.Parse(hfSelectedSubTab.Value); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                using (var dataAccess = new AvailabilityDataAccess(null))
                {
                    lblLastUpdate.Text = LastUpdatedFromFleetNow.GetLastUpdatedDateTime(dataAccess);
                }
            }
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            var mergedParameters = nrParams.GetParameterDictionary();

            var reportTypeSelected = GetReportType();

            switch (reportTypeSelected)
            {
                case ReportType.SiteComparison:
                case ReportType.FleetComparison:
                case ReportType.StatusComparison:
                    PopulateComparisonReport(mergedParameters, reportTypeSelected);
                    break;
                case ReportType.Ageing:
                    PopulateAgeingReport(mergedParameters);

                    break;
                case ReportType.HistoricalTrend:
                case ReportType.ReasonHistory:
                    PopulateHistoricalTrend(mergedParameters, reportTypeSelected);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private ReportType GetReportType()
        {
            if (SelectedTab == 0)
            {
                return ReportType.SiteComparison;
            }
            if (SelectedTab == 1)
            {
                return ReportType.FleetComparison;
            }
            if (SelectedTab == 2)
            {
                return ReportType.StatusComparison;
            }
            if (SelectedTab == 3)
            {
                return ReportType.Ageing;
            }
            if (SelectedTab == 4)
            {
                return ReportType.HistoricalTrend;
            }

            return ReportType.ReasonHistory;
        }


        private void PopulateHistoricalTrend(Dictionary<DictionaryParameter, string> parameters, ReportType report)
        {
            using (var dataAccess = new HistoricalTrendDataAccess(parameters))
            {
                
                lblLastUpdate.Text = LastUpdatedFromFleetNow.GetLastUpdatedDateTime(dataAccess);
                upnlUpdatedTime.Update();
                
                if (report == ReportType.HistoricalTrend)
                {
                    var trendData = dataAccess.GetHistoricalTrendEntries();
                    ucHistoricalTrendGrid.LoadGrid(trendData);
                    ucHistoricalTrendChart.LoadChart(trendData);
                    upHistoricalTrendGrid.Update();
                    upHistoricalTrendChart.Update();
                }
                if (report == ReportType.ReasonHistory)
                {
                    var historyData = dataAccess.GetHistoryEntries();
                    ucReasonHistory.LoadGrid(historyData);
                    upReasonHistory.Update();
                }
            }
            
        }

        private void PopulateComparisonReport(Dictionary<DictionaryParameter, string> parameters, ReportType report)
        {
            using (var dataAccess = new ComparisonDataAccess(parameters))
            {
                lblLastUpdate.Text = LastUpdatedFromFleetNow.GetLastUpdatedDateTime(dataAccess);
                upnlUpdatedTime.Update();

                List<ComparisonRow> comparisonData;
                switch (report)
                {
                    case ReportType.SiteComparison:
                            comparisonData = dataAccess.GetComparisonEntries(true);
                            CalculateTotals(comparisonData);

                            ucSiteCompChart.LoadChart(comparisonData, report);
                            ucSiteCompGrid.LoadGrid(comparisonData);
                            upSiteCompGrid.Update();
                            upSiteCompChart.Update();
                        break;
                    case ReportType.FleetComparison:
                            comparisonData = dataAccess.GetComparisonEntries(false);
                            CalculateTotals(comparisonData);
                            ucFleetCompChart.LoadChart(comparisonData, report);
                            ucFleetCompGrid.LoadGrid(comparisonData);
                            upFleetCompGrid.Update();
                            upFleetCompChart.Update();
                        break;
                    case ReportType.StatusComparison:
                            comparisonData = dataAccess.GetComparisonByStatusEntries();
                            CalculateTotals(comparisonData);
                            
                            ucAgeCompGrid.LoadGrid(comparisonData);
                            ucAgeCompChart.LoadChart(comparisonData, report);
                            upAgeCompGrid.Update();
                            upAgeCompChart.Update();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("report");
                }
            }
        }

        private void CalculateTotals(List<ComparisonRow> comparisonData)
        {
            var totalNonRev = comparisonData.Sum(d => d.NonRevCount);
            var totalFleet = comparisonData.Sum(d => d.FleetCount);

            comparisonData.ForEach(d => d.CalculatePercentOfTotalNonRev(totalNonRev));
            comparisonData.ForEach(d => d.CalculatePercentOfTotalFleet(totalFleet));

            var totalRow = new ComparisonRow
            {
                Key = NonRevBaseDataAccess.TotalKeyName,
                FleetCount = comparisonData.Where(d => d != null).Sum(d => d.FleetCount),
                DaysNonRev = comparisonData.Where(d => d != null).Sum(d => d.DaysNonRev),
                NonRevCount = comparisonData.Where(d => d != null).Sum(d => d.NonRevCount),
                PercentNonRevOfTotalNonRev = comparisonData.Where(d => d != null).Sum(d => d.PercentNonRevOfTotalNonRev),
                ReasonsEntered = comparisonData.Where(d => d != null).Sum(d => d.ReasonsEntered)
            };


            comparisonData.Add(totalRow);
        }

        private void PopulateAgeingReport(Dictionary<DictionaryParameter, string> parameters)
        {
            List<AgeingRow> ageingData;
            using (var dataAccess = new AgeingDataAccess(parameters))
            {
                ageingData = dataAccess.GetAgeingEntries();
            }
            ucAgeingGrid.LoadGrid(ageingData);
            ucAgeingChart.LoadChart(ageingData);
            upAgeingGrid.Update();
            upAgeingChart.Update();
        }
    }
}