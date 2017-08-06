using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.FleetAllocation.DataAccess.Reporting.ForecastedFleetSize.Entities;

namespace Mars.FleetAllocation.UserControls
{
    public partial class ForecastedFleetSize : UserControl
    {
        public const string ForecastFleetSizeDataGridASessionHolder = "ForecastFleetSizeDataGridASessionHolder";
        public const string ForecastFleetSizeDataGridBSessionHolder = "ForecastFleetSizeDataGridBSessionHolder";

        public bool ValuesChart
        {
            get { return bool.Parse(hfValues.Value); }
            set { hfValues.Value = value.ToString(); }
        }
        

        protected void Page_Load(object sender, EventArgs e)
        {
            agForecastFleetSizeGridA.GridItemType = typeof(ForecastedFleetSizeEntity);
            agForecastFleetSizeGridA.SessionNameForGridData = ForecastFleetSizeDataGridASessionHolder;
            agForecastFleetSizeGridA.ColumnHeaders = ForecastedFleetSizeEntity.HeaderRows;

            agForecastFleetSizeGridB.GridItemType = typeof(ForecastedFleetSizeEntity);
            agForecastFleetSizeGridB.SessionNameForGridData = ForecastFleetSizeDataGridBSessionHolder;
            agForecastFleetSizeGridB.ColumnHeaders = ForecastedFleetSizeEntity.HeaderRows;
        }

        public void SetGridAndGraph(List<ForecastedFleetSizeEntity> dataForA, List<ForecastedFleetSizeEntity> dataForB)
        {
            ForecastedFleetSizeChart.ValuesChart = ValuesChart;
            ForecastedFleetSizeChart.LoadChart(dataForA, dataForB);
            
            agForecastFleetSizeGridA.GridData = dataForA;
            agForecastFleetSizeGridB.GridData = dataForB;
        }
    }
}