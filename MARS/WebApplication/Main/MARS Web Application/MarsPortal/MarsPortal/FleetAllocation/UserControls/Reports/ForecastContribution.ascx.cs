using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.FleetAllocation.DataAccess.Reporting.ForecastContribution.Entities;

namespace Mars.FleetAllocation.UserControls.Reports
{
    public partial class ForecastContribution : UserControl
    {
        public const string FaoForecastContributionDataGridSessionHolder = "FaoForecastContributionDataGridSessionHolder";


        protected void Page_Load(object sender, EventArgs e)
        {
            agForecastContribution.GridItemType = typeof(ForecastContributionRow);
            agForecastContribution.SessionNameForGridData = FaoForecastContributionDataGridSessionHolder;
            agForecastContribution.ColumnHeaders = ForecastContributionRow.HeaderRows;
        }

        public void SetContribution(List<ForecastContributionRow> contributionData)
        {
            agForecastContribution.GridData = contributionData;

            var summedForecastData = from fd in contributionData
                                     group fd by new { fd.Year, fd.Month }
                                         into groupedFd
                                         select new TotalContributionRow
                                                {
                                                    Year = groupedFd.Key.Year,
                                                    Month = groupedFd.Key.Month,
                                                    ExpectedContribution = groupedFd.Sum(d => d.CpU * d.Expected),
                                                    ExpectedContributionA = groupedFd.Sum(d => d.CpU * (d.Expected + d.CumulativeAdditionsA)),
                                                    ExpectedContributionB = groupedFd.Sum(d => d.CpU * (d.Expected + d.CumulativeAdditionsB)),
                                                };

            var localSummedList = summedForecastData.ToList();
            ucForecastContributionChart.LoadChart(localSummedList);
            //var xx = summedForecastData.ToList();
        }
    }
}