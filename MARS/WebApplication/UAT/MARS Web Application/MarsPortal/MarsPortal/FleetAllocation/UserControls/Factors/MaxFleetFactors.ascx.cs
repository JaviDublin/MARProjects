using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.App.Classes.Phase4Dal.Enumerators;
using Mars.FleetAllocation.DataAccess;
using Mars.FleetAllocation.DataAccess.Entities;
using Mars.FleetAllocation.DataAccess.ScenarioAccess;

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

            if (!IsPostBack)
            {
                ucScenarioSelection.PopulateCountryDropdown();
                ucScenarioSelection.ControlScenarioType = ScenarioType.MaxFleetFactor;
                LoadScenarios();

            }
        }

        private void LoadScenarios()
        {
            using (var dataAccess = new MaxFleetFactorDataAccess())
            {
                var scenarioListItems = dataAccess.GetMaxFleetFactorScenarios(ucScenarioSelection.CountryId);
                ucScenarioSelection.LoadScenarios(scenarioListItems);
            }
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
                var scenarioId = ucScenarioSelection.SelectedScenarioId;
                var data = dataAccess.GetMaxFleetFactors(scenarioId);
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
                var scenarioId = ucScenarioSelection.SelectedScenarioId;
                dataAccess.UpdateMaxFleetFactors(nonRevPercent, utilizationPercent, scenarioId);
                var data = dataAccess.GetMaxFleetFactors(scenarioId);
                hfRecordCount.Value = data.Count.ToString(CultureInfo.InvariantCulture);
                ucMaxFactors.GridData = data;
            }
            EnableUpdateButton();
            upGrid.Update();
        }

        protected override bool OnBubbleEvent(object source, EventArgs args)
        {
            if (args is CommandEventArgs)
            {
                var commandArgs = args as CommandEventArgs;
                if (commandArgs.CommandName == ScenarioCommands.NewBlankMaxFactorScenario.ToString())
                {
                    NewScenario(commandArgs.CommandArgument.ToString());
                    return true;
                }
                if (commandArgs.CommandName == ScenarioCommands.DuplicateMaxFactorScenario.ToString())
                {
                    DuplicateScenario(commandArgs.CommandArgument.ToString());
                    return true;
                }

                if (commandArgs.CommandName == ScenarioCommands.DeleteMaxFactorScenario.ToString())
                {
                    var scenarioId = ucScenarioSelection.SelectedScenarioId;
                    DeleteSecenario(scenarioId);
                    return true;
                }

                if (commandArgs.CommandName == ScenarioCommands.RenameMaxFactorScenario.ToString())
                {
                    RenameScenario(commandArgs.CommandArgument.ToString());
                    return true;
                }


            }
            return false;
        }

        private void RenameScenario(string newName)
        {
            var scenarioId = ucScenarioSelection.SelectedScenarioId;
            string result;
            using (var dataAccess = new MaxFleetFactorDataAccess())
            {
                result = dataAccess.RenameScenario(scenarioId, newName);
            }
            if (result == string.Empty)
            {
                LoadScenarios();
                ucScenarioSelection.SelectedScenarioId = scenarioId;
            }
            else
            {
                ucScenarioSelection.SetMessage(result);
            }

        }

        private void NewScenario(string scenarioName)
        {
            string result;
            using (var dataAccess = new MaxFleetFactorDataAccess())
            {
                result = dataAccess.CreateNewScenario(scenarioName, ucScenarioSelection.CountryId);
            }
            if (result == string.Empty)
            {
                LoadScenarios();
                ucScenarioSelection.SetLastScenario();
                ucScenarioSelection.UpdateScenarioStatistics();
            }
            else
            {
                ucScenarioSelection.SetMessage(result);
            }

        }

        private void DuplicateScenario(string scenarioName)
        {
            var scenarioId = ucScenarioSelection.SelectedScenarioId;
            string result;
            using (var dataAccess = new MaxFleetFactorDataAccess())
            {
                result = dataAccess.CloneExistingScenario(scenarioId, scenarioName, ucScenarioSelection.CountryId);
            }
            if (result == string.Empty)
            {
                LoadScenarios();
                ucScenarioSelection.SetLastScenario();
                ucScenarioSelection.UpdateScenarioStatistics();
            }
            else
            {
                ucScenarioSelection.SetMessage(result);
            }
        }

        private void DeleteSecenario(int scenarioId)
        {
            string result;
            using (var dataAccess = new MaxFleetFactorDataAccess())
            {
                result = dataAccess.DeleteScenario(scenarioId);
            }
            if (result == string.Empty)
            {
                ucScenarioSelection.SetMessage("Scenario Deleted", Color.Red);
                LoadScenarios();
                ucScenarioSelection.UpdateScenarioStatistics();
            }
            else
            {
                ucScenarioSelection.SetMessage(result, Color.Red);
            }
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