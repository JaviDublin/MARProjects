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
    public partial class MinCommercialSegment : UserControl
    {
        public const string FaoCommSegMinParameterSessionName = "FaoCommSegMinParameterSessionName";

        public Dictionary<DictionaryParameter, string> SessionStoredFaoMinCommSegParameters
        {
            get { return (Dictionary<DictionaryParameter, string>)Session[FaoCommSegMinParameterSessionName]; }
            set { Session[FaoCommSegMinParameterSessionName] = value; }
        }

        public string GetUpdatePanelId { get { return ucParameters.UpdatePanelClientId; } }
        

        private const string MinComSegSessionDataGrid = "MinComSegSessionDataGrid";

        protected int RowCount { get { return int.Parse(hfRecordCount.Value); }}

        protected void Page_Load(object sender, EventArgs e)
        {
            ucMinCommSegGrid.GridItemType = typeof(MinCommercialSegmentRow);
            ucMinCommSegGrid.SessionNameForGridData = MinComSegSessionDataGrid;
            ucMinCommSegGrid.ColumnHeaders = MinCommercialSegmentRow.HeaderRows;
            ucMinCommSegGrid.ColumnFormats = MinCommercialSegmentRow.Formats;
            ucParameters.SessionStoredFaoMinCommSegParameters = SessionStoredFaoMinCommSegParameters;

            DisableUpdateButton();
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            SetConfirmText();
        }

        private void DisableUpdateButton()
        {
            btnSubmitChange.Enabled = false;
            btnSubmitChange.ToolTip = hfRefreshDataMessage.Value;
        }

        private void EnableUpdateButton()
        {
            btnSubmitChange.Enabled = true;
            btnSubmitChange.ToolTip = string.Empty;
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            var parameters = ucParameters.GetParameters();
            using (var dataAccess = new FactorsDataAccess(parameters))
            {
                var data = dataAccess.GetMinCommercialSegments();
                hfRecordCount.Value = data.Count.ToString(CultureInfo.InvariantCulture);
                ucMinCommSegGrid.GridData = data;
            }
            EnableUpdateButton();
            upGrid.Update();
        }

        

        private void SetConfirmText()
        {
            cbeChangeConfirm.ConfirmText = string.Format(hfChangeConfirmMessage.Value, RowCount);
        }

        protected void btnSubmitChange_Click(object sender, EventArgs e)
        {
            int rowCount = RowCount;
            if (rowCount == 0) return;
            if (tbNewPercent.Text == string.Empty) return;
            var newMinCommSegPercent = double.Parse(tbNewPercent.Text) / 100;

            var parameters = ucParameters.GetParameters();
            using (var dataAccess = new FactorsDataAccess(parameters))
            {
                dataAccess.UpdateMinCommercialSegments(newMinCommSegPercent);
                var data = dataAccess.GetMinCommercialSegments();
                hfRecordCount.Value = data.Count.ToString(CultureInfo.InvariantCulture);
                ucMinCommSegGrid.GridData = data;
            }
            EnableUpdateButton();
            upGrid.Update();
        }
    }
}