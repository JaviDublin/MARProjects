using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.App.Classes.Phase4Dal.Enumerators;
using Mars.FleetAllocation.DataAccess;
using Mars.FleetAllocation.DataAccess.Entities;

namespace Mars.FleetAllocation.UserControls.Factors
{
    public partial class Revenue : UserControl
    {
        public const string FaoParameterSessionName = "FaoRevenueParameterSessionName";

        public Dictionary<DictionaryParameter, string> SessionStoredParameters
        {
            get { return (Dictionary<DictionaryParameter, string>)Session[FaoParameterSessionName]; }
            set { Session[FaoParameterSessionName] = value; }
        }

        private const string FaoSessionDataGrid = "FaoRevenueSessionDataGrid";

        public string GetUpdatePanelId { get { return ucParameters.UpdatePanelClientId; } }

        protected int RowCount { get { return int.Parse(hfRecordCount.Value); } }


        protected void Page_Load(object sender, EventArgs e)
        {
            ucMaxFactors.GridItemType = typeof(RevenueRow);
            ucMaxFactors.SessionNameForGridData = FaoSessionDataGrid;
            ucMaxFactors.ColumnHeaders = RevenueRow.HeaderRows;
            ucMaxFactors.ColumnFormats = RevenueRow.Formats;
            ucParameters.SessionStoredFaoMinCommSegParameters = SessionStoredParameters;
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            var parameters = ucParameters.GetParameters();
            
            using (var dataAccess = new RevenueDataAccess(parameters))
            {
                var data = dataAccess.GetRevenueData();
                hfRecordCount.Value = data.Count.ToString(CultureInfo.InvariantCulture);
                ucMaxFactors.GridData = data;
            }
            
            upGrid.Update();
        }
    }
}