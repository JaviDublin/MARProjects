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

            if (!IsPostBack)
            {
                ucScenarioSelection.PopulateCountryDropdown();
                ucScenarioSelection.ControlScenarioType = ScenarioType.MinCommercialSegment;
                LoadScenarios();

            }
        }

        private void LoadScenarios()
        {
            using (var dataAccess = new MinCommercialSegmentDataAccess())
            {
                var scenarioListItems = dataAccess.GetMinCommSegScenarios(ucScenarioSelection.CountryId);
                ucScenarioSelection.LoadScenarios(scenarioListItems);
            }
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
                var scenarioId = ucScenarioSelection.SelectedScenarioId;
                var data = dataAccess.GetMinCommercialSegments(scenarioId);
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
                var scenarioId = ucScenarioSelection.SelectedScenarioId;
                dataAccess.UpdateMinCommercialSegments(newMinCommSegPercent, scenarioId);
                var data = dataAccess.GetMinCommercialSegments(scenarioId);
                hfRecordCount.Value = data.Count.ToString(CultureInfo.InvariantCulture);
                ucMinCommSegGrid.GridData = data;
            }
            ucScenarioSelection.UpdateScenarioStatistics();
            EnableUpdateButton();
            upGrid.Update();
            
        }
        protected override bool OnBubbleEvent(object source, EventArgs args)
        {
            if (args is CommandEventArgs)
            {
                var commandArgs = args as CommandEventArgs;
                if (commandArgs.CommandName == ScenarioCommands.NewBlankMinComScenario.ToString())
                {
                    NewScenario(commandArgs.CommandArgument.ToString());
                    return true;
                }
                if (commandArgs.CommandName == ScenarioCommands.DuplicateMinComScenario.ToString())
                {
                    DuplicateScenario(commandArgs.CommandArgument.ToString());
                    return true;
                }

                if (commandArgs.CommandName == ScenarioCommands.DeleteMinComScenario.ToString())
                {
                    var scenarioId = ucScenarioSelection.SelectedScenarioId;
                    DeleteSecenario(scenarioId);
                    return true;
                }

                if (commandArgs.CommandName == ScenarioCommands.RenameMinComScenario.ToString())
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
            using (var dataAccess = new MinCommercialSegmentDataAccess())
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
            using (var dataAccess = new MinCommercialSegmentDataAccess())
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
            using (var dataAccess = new MinCommercialSegmentDataAccess())
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
            using (var dataAccess = new MinCommercialSegmentDataAccess())
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
    }
}