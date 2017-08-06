using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.App.Classes.Phase4Dal.Enumerators;
using Mars.FleetAllocation.BusinessLogic.DropdownLists;
using Mars.FleetAllocation.DataAccess;
using Mars.FleetAllocation.DataAccess.AdditionPlanDataAccess;
using Mars.FleetAllocation.DataAccess.AdditionPlanDataAccess.Entities;
using Mars.FleetAllocation.DataAccess.Entities.Output;
using Mars.FleetAllocation.UserControls.Enums;

namespace Mars.FleetAllocation.UserControls
{
    public partial class AdditionPlanHistory : UserControl
    {
        private const string AdditionPlanHistorySessionHolder = "AdditionPlanHistorySessionHolder";
        private const string AdditionPlanHistoryV1WeeklyAdditionsSessionHolder = "AdditionPlanHistoryV1WeeklyAdditionsSessionHolder";
        private const string AdditionPlanHistoryV1MinMaxSessionHolder = "AdditionPlanHistoryV1MinMaxSessionHolder";

        private const string AdditionPlanHistoryV2WeeklyAdditionsSessionHolder = "AdditionPlanHistoryV2WeeklyAdditionsSessionHolder";
        private const string AdditionPlanHistoryV2MinMaxSessionHolder = "AdditionPlanHistoryV2MinMaxSessionHolder";

        private AdditionPlanComparisonTypes ComparisonTypeSelected()
        {
            var returned =
                (AdditionPlanComparisonTypes)
                    Enum.Parse(typeof (AdditionPlanComparisonTypes), rblComparisonType.SelectedValue);
            return returned;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            agAdditionPlanHistory.GridItemType = typeof(AdditionPlanEntity);
            agAdditionPlanHistory.SessionNameForGridData = AdditionPlanHistorySessionHolder;
            agAdditionPlanHistory.ColumnHeaders = AdditionPlanEntity.HeaderRows;

            agV1AdditionPlanEntries.GridItemType = typeof(AdditionEntity);
            agV1AdditionPlanEntries.ColumnFormats = AdditionEntity.Formats;
            agV1AdditionPlanEntries.SessionNameForGridData = AdditionPlanHistoryV1WeeklyAdditionsSessionHolder;
            agV1AdditionPlanEntries.ColumnHeaders = AdditionEntity.HeaderRows;

            agV1AdditionPlanMinMaxValues.GridItemType = typeof(AdditionPlanMinMaxRow);
            agV1AdditionPlanMinMaxValues.SessionNameForGridData = AdditionPlanHistoryV1MinMaxSessionHolder;
            agV1AdditionPlanMinMaxValues.ColumnHeaders = AdditionPlanMinMaxRow.HeaderRows;

            agV2AdditionPlanEntries.GridItemType = typeof(AdditionEntity);
            agV2AdditionPlanEntries.SessionNameForGridData = AdditionPlanHistoryV2WeeklyAdditionsSessionHolder;
            agV2AdditionPlanEntries.ColumnHeaders = AdditionEntity.HeaderRows;
            agV2AdditionPlanEntries.ColumnFormats = AdditionEntity.Formats;

            agV2AdditionPlanMinMaxValues.GridItemType = typeof(AdditionPlanMinMaxRow);
            agV2AdditionPlanMinMaxValues.SessionNameForGridData = AdditionPlanHistoryV2MinMaxSessionHolder;
            agV2AdditionPlanMinMaxValues.ColumnHeaders = AdditionPlanMinMaxRow.HeaderRows;

            btnFilterHistory.Enabled = hfSelectedScenarioBId.Value != string.Empty;
            
            
            if (!IsPostBack)
            {
                ListGenerator.FillDropdownWithFaoCountries(ddlCountry);
                FillAdditionPlanHistory();
                PopulateRadioButtonList();
                ClearGrids();
                SetComparisonType();
                SetScenarioPanels(pnlScenarioA, pnlScenarioB);
            }
        }

        private void ClearGrids()
        {
            agV1AdditionPlanEntries.GridData = null;
            agV1AdditionPlanMinMaxValues.GridData = null;
            agV2AdditionPlanEntries.GridData = null;
            agV2AdditionPlanMinMaxValues.GridData = null;
        }

        private void PopulateRadioButtonList()
        {
            rblComparisonType.Items.Add(new ListItem("Min Max Values", AdditionPlanComparisonTypes.MinMaxValues.ToString()) );
            rblComparisonType.Items.Add(new ListItem("Additions", AdditionPlanComparisonTypes.Additions.ToString()) { Selected = true });
            

            rblLocationGrouping.Items.Add(new ListItem("Location", LocationLevelGroupings.Location.ToString()) { Selected = true });
            rblLocationGrouping.Items.Add(new ListItem("Location Group", LocationLevelGroupings.LocationGroup.ToString()));
            rblLocationGrouping.Items.Add(new ListItem("Pool", LocationLevelGroupings.Pool.ToString()));
            rblLocationGrouping.Items.Add(new ListItem("Country", LocationLevelGroupings.Country.ToString()));


            rblCarGrouping.Items.Add(new ListItem("Car Group", VehicleLevelGrouping.CarGroup.ToString()) { Selected = true });
            rblCarGrouping.Items.Add(new ListItem("Car Class", VehicleLevelGrouping.CarClass.ToString()));
            rblCarGrouping.Items.Add(new ListItem("Car Segment", VehicleLevelGrouping.CarSegement.ToString()));
        }

        private void FillAdditionPlanHistory()
        {
            using (var dataAccess = new AdditionPlanDataAccess())
            {
                var planData = dataAccess.GetAdditionPlanHistory();
                agAdditionPlanHistory.GridData = planData;
            }
            
        }

        protected void rblComparisonType_SelectionChanged(object sender, EventArgs e)
        {
            SetComparisonType();

            PopulateHistoryTables();
        }

        private void SetComparisonType()
        {
            if (ComparisonTypeSelected() == AdditionPlanComparisonTypes.Additions)
            {
                agV1AdditionPlanMinMaxValues.Visible = false;
                agV2AdditionPlanMinMaxValues.Visible = false;
                rblLocationGrouping.Enabled = true;
                rblCarGrouping.Enabled = true;
            }
            else
            {
                agV1AdditionPlanEntries.Visible = false;
                agV2AdditionPlanEntries.Visible = false;
                rblLocationGrouping.Enabled = false;
                rblCarGrouping.Enabled = false;
            }
        }

        protected void GroupingChanged(object sender, EventArgs e)
        {
            PopulateHistoryTables();
        }

        protected override bool OnBubbleEvent(object source, EventArgs args)
        {
            var handled = false;

            if (args is CommandEventArgs)
            {
                var commandArgs = args as CommandEventArgs;
                if (commandArgs.CommandName == AutoGrid.ViewKeyword)
                {
                    var additionPlanId = int.Parse(commandArgs.CommandArgument.ToString());
                    if (rbAScenario.Checked)
                    {
                        hfSelectedScenarioAId.Value = additionPlanId.ToString();
                    }
                    else
                    {
                        hfSelectedScenarioBId.Value = additionPlanId.ToString();
                    }
                    
                    PopulateHistoryTables();
                    handled = true;
                }

                
            }

            return handled;
        }

        protected void btnRefreshHistory_Click(object sender, EventArgs e)
        {
            FillAdditionPlanHistory();
        }

        protected void btnFilterHistory_Click(object sender, EventArgs e)
        {
            PopulateHistoryTables();
        }


        private void PopulateAdditionTables(AdditionPlanDataAccess dataAccess, LocationLevelGroupings locationGrouping
                    , VehicleLevelGrouping vehicleGrouping, int scenarioId, AutoGrid gridToUpdate, bool scenarioASelected)
        {
            var additionData = dataAccess.GetAdditionPlanEntryEntries(scenarioId, locationGrouping, vehicleGrouping);

            var additionPlan = dataAccess.GetAdditionPlan(scenarioId);

            var bottomLevelGrouping = locationGrouping == LocationLevelGroupings.Location &&
                                      vehicleGrouping == VehicleLevelGrouping.CarGroup;

            gridToUpdate.HideLastColumn = !bottomLevelGrouping;


            if (scenarioASelected)
            {
                rbAScenario.Text = additionPlan.Name;
                lblMaxScenASelected.Text = additionPlan.MaxFleetScenarioName;
                lblMinScenASelected.Text = additionPlan.MinComSegScenarioName;
            }
            else
            {
                rbBScenario.Text = additionPlan.Name;
                lblMaxScenBSelected.Text = additionPlan.MaxFleetScenarioName;
                lblMinScenBSelected.Text = additionPlan.MinComSegScenarioName;
            }
            
            var additionEntities = from ad in additionData
                                   select new AdditionEntity
                                   {
                                       Year = ad.Year,
                                       IsoWeek = ad.IsoWeek,
                                       CarGroupId = ad.CarGroupId,
                                       LocationId = ad.LocationId,
                                       CarGroup = ad.CarGroup,
                                       Location = ad.Location,
                                       Amount = ad.Amount,
                                       Contribution = (double) ad.CpU
                                   };
            gridToUpdate.GridData = additionEntities.ToList();
            gridToUpdate.Visible = true;
        }

        private void PopulateMinMaxTables(AdditionPlanDataAccess dataAccess, int scenarioId, AutoGrid gridToUpdate)
        {
            var minMaxValues = dataAccess.GetAdditionPlanMinMaxRows(scenarioId);

            var additionPlan = dataAccess.GetAdditionPlan(scenarioId);

            if (rbAScenario.Checked)
            {
                rbAScenario.Text = additionPlan.Name;
                lblMaxScenASelected.Text = additionPlan.MaxFleetScenarioName;
                lblMinScenASelected.Text = additionPlan.MinComSegScenarioName;
            }
            else
            {
                rbBScenario.Text = additionPlan.Name;
                lblMaxScenBSelected.Text = additionPlan.MaxFleetScenarioName;
                lblMinScenBSelected.Text = additionPlan.MinComSegScenarioName;
            }
            
            gridToUpdate.GridData = minMaxValues.ToList();
            gridToUpdate.Visible = true;
        }

        protected void SelectedScenarioChanged(object sender, EventArgs e)
        {
            if (rbAScenario.Checked)
            {
                SetScenarioPanels(pnlScenarioA, pnlScenarioB);
            }
            else
            {
                SetScenarioPanels(pnlScenarioB, pnlScenarioA);
            }
        }

        private void SetScenarioPanels(Panel activePanel, Panel inactivePanel)
        {
            activePanel.BorderStyle = BorderStyle.Inset;

            inactivePanel.BorderStyle = BorderStyle.None;
            
        }

        private void PopulateHistoryTables()
        {
            var parameters = FaoParameter.GetParameters();

            using (var dataAccess = new AdditionPlanDataAccess(parameters))
            {
                if (ComparisonTypeSelected() == AdditionPlanComparisonTypes.Additions)
                {
                    var locationGrouping = (LocationLevelGroupings)Enum.Parse(typeof(LocationLevelGroupings), rblLocationGrouping.SelectedValue);
                    var vehicleGrouping = (VehicleLevelGrouping)Enum.Parse(typeof(VehicleLevelGrouping), rblCarGrouping.SelectedValue);


                    if (hfSelectedScenarioAId.Value != string.Empty)
                    {
                        var scenarioId = int.Parse(hfSelectedScenarioAId.Value);
                        PopulateAdditionTables(dataAccess, locationGrouping, vehicleGrouping, scenarioId,
                            agV1AdditionPlanEntries, true);
                    }
                    if (hfSelectedScenarioBId.Value != string.Empty)
                    {
                        var scenarioId = int.Parse(hfSelectedScenarioBId.Value);
                        PopulateAdditionTables(dataAccess, locationGrouping, vehicleGrouping, scenarioId,
                            agV2AdditionPlanEntries, false);
                    }
                }
                else
                {
                    if (hfSelectedScenarioAId.Value != string.Empty)
                    {
                        var scenarioId = int.Parse(hfSelectedScenarioAId.Value);
                        PopulateMinMaxTables(dataAccess, scenarioId, agV1AdditionPlanMinMaxValues);
                    }
                    if (hfSelectedScenarioBId.Value != string.Empty)
                    {
                        var scenarioId = int.Parse(hfSelectedScenarioAId.Value);
                        PopulateMinMaxTables(dataAccess, scenarioId, agV2AdditionPlanMinMaxValues);
                    }

                }
            }
        }
    }
}