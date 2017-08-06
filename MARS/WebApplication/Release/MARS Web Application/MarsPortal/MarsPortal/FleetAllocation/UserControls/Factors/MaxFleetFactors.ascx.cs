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
    public partial class NonRevenuePercent : UserControl
    {
        public const string FaoMaxFactorParameterSessionName = "FaoMaxFactorParameterSessionName";

        public Dictionary<DictionaryParameter, string> SessionStoredFaoMinCommSegParameters
        {
            get { return (Dictionary<DictionaryParameter, string>)Session[FaoMaxFactorParameterSessionName]; }
            set { Session[FaoMaxFactorParameterSessionName] = value; }
        }

        private const string FaoMaxFactorSessionDataGrid = "FaoMaxFactorSessionDataGrid";

        public string GetUpdatePanelId { get { return ucParameters.UpdatePanelClientId; } }

        protected int RowCount { get { return int.Parse(hfRecordCount.Value); } }
        

        protected void Page_Load(object sender, EventArgs e)
        {
            ucMaxFactors.GridItemType = typeof(MaxFleetFactorRow);
            ucMaxFactors.SessionNameForGridData = FaoMaxFactorSessionDataGrid;
            ucMaxFactors.ColumnHeaders = MaxFleetFactorRow.HeaderRows;
            ucMaxFactors.ColumnFormats = MaxFleetFactorRow.Formats;
            ucParameters.SessionStoredFaoMinCommSegParameters = SessionStoredFaoMinCommSegParameters;


            DisableUpdateButton();
        }
        protected void Page_PreRender(object sender, EventArgs e)
        {
            SetConfirmText();
        }

        private void SetConfirmText()
        {
            cbeChangeNonRevConfirm.ConfirmText = string.Format(hfNonRevUpdate.Value, RowCount);
            cbeChangeUtilizationConfirm.ConfirmText = string.Format(hfUltilizationUpdate.Value, RowCount);
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            var parameters = ucParameters.GetParameters();
            using (var dataAccess = new FactorsDataAccess(parameters))
            {
                var data = dataAccess.GetMaxFleetFactors();
                hfRecordCount.Value = data.Count.ToString(CultureInfo.InvariantCulture);
                ucMaxFactors.GridData = data;
            }
            EnableUpdateButton();
            upGrid.Update();
        }

        private void UpdateFactors(double? nonRevPercent, double? utilizationPercent)
        {
            int rowCount = RowCount;
            if (rowCount == 0) return;


            var parameters = ucParameters.GetParameters();
            using (var dataAccess = new FactorsDataAccess(parameters))
            {
                dataAccess.UpdateMaxFleetFactors(nonRevPercent, utilizationPercent);
                var data = dataAccess.GetMaxFleetFactors();
                hfRecordCount.Value = data.Count.ToString(CultureInfo.InvariantCulture);
                ucMaxFactors.GridData = data;
            }
            EnableUpdateButton();
            upGrid.Update();
        }

        private void DisableUpdateButton()
        {
            btnSubmitNonRev.Enabled = false;
            btnSubmitNonRev.ToolTip = hfRefreshDataMessage.Value;
            btnSubmitUtilization.Enabled = false;
            btnSubmitUtilization.ToolTip = hfRefreshDataMessage.Value;
        }

        private void EnableUpdateButton()
        {
            btnSubmitNonRev.Enabled = true;
            btnSubmitNonRev.ToolTip = string.Empty;
            btnSubmitUtilization.Enabled = true;
            btnSubmitUtilization.ToolTip = string.Empty;
        }

        protected void btnSubmitNonRev_Click(object sender, EventArgs e)
        {
            if (tbNewNonRev.Text == string.Empty) return;
            var newMinCommSegPercent = double.Parse(tbNewNonRev.Text) / 100;
            UpdateFactors(newMinCommSegPercent, null);
        }

        protected void btnSubmitUtilization_Click(object sender, EventArgs e)
        {
            if (tbNewUtilization.Text == string.Empty) return;
            var minUtilization = double.Parse(tbNewUtilization.Text) / 100;
            UpdateFactors(null, minUtilization);
        }
    }
}