using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.FleetAllocation.BusinessLogic.DropdownLists;
using Mars.FleetAllocation.DataAccess;
using Mars.FleetAllocation.DataAccess.ScenarioAccess;

namespace Mars.FleetAllocation.UserControls.ScenarioSelection
{
    public partial class ScenarioSelection : UserControl
    {
        

        public ScenarioType ControlScenarioType
        {
            get
            {
                ScenarioType st;
                 Enum.TryParse(hfScenarioType.Value, out st);
                return st;
            }
            set { hfScenarioType.Value = value.ToString(); }
        }

        public int CountryId
        {
            get { return int.Parse(ddlCountry.SelectedValue); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
            lblMessage.Text = string.Empty;
            if (!IsPostBack)
            {

                UpdateScenarioStatistics();
                
            }
            
        }

        public void PopulateCountryDropdown()
        {
            ListGenerator.FillDropdownWithFaoCountries(ddlCountry);
        }

        public int SelectedScenarioId 
        {
            get
            {
                return int.Parse(ddlScenarioSelector.SelectedValue);

            }
            set
            {
                ddlScenarioSelector.SelectedValue = value.ToString();
            }
        }

        public void SetMessage(string message, Color? colour = null)
        {
            lblMessage.Text = message;
            if (colour != null)
            {
                lblMessage.ForeColor = colour.Value;
            }
        }

        public string Description
        {
            get { return tbScenarioDescription.Text; }
            set { tbScenarioDescription.Text = value; }
        }

        public void SetLastScenario()
        {
            ddlScenarioSelector.SelectedIndex = ddlScenarioSelector.Items.Count - 1;
        }

        public void LoadScenarios(List<ListItem> items)
        {
            ddlScenarioSelector.Items.Clear();
            ddlScenarioSelector.Items.AddRange(items.ToArray());
        }

        protected void btnSaveNewBlankScenario_Click(object sender, EventArgs e)
        {
            var commandName = ControlScenarioType == ScenarioType.MinCommercialSegment
                ? ScenarioCommands.NewBlankMinComScenario.ToString()
                : ScenarioCommands.NewBlankMaxFactorScenario.ToString();

            RaiseBubbleEvent(this, new CommandEventArgs(commandName, tbNewScenarioName.Text));
        }

        protected void btnCloneScenario_Click(object sender, EventArgs e)
        {
            var commandName = ControlScenarioType == ScenarioType.MinCommercialSegment
                ? ScenarioCommands.DuplicateMinComScenario.ToString()
                : ScenarioCommands.DuplicateMaxFactorScenario.ToString();

            RaiseBubbleEvent(this, new CommandEventArgs(commandName, tbNewScenarioName.Text));
        }


        protected void btnDeleteScenario_Click(object sender, EventArgs e)
        {
            var commandName = ControlScenarioType == ScenarioType.MinCommercialSegment
                ? ScenarioCommands.DeleteMinComScenario.ToString()
                : ScenarioCommands.DeleteMaxFactorScenario.ToString();

            RaiseBubbleEvent(this, new CommandEventArgs(commandName, null));
        }

        protected void btnConfirmRenameScenario_Click(object sender, EventArgs e)
        {
            var commandName = ControlScenarioType == ScenarioType.MinCommercialSegment
                ? ScenarioCommands.RenameMinComScenario.ToString()
                : ScenarioCommands.RenameMaxFactorScenario.ToString();

            RaiseBubbleEvent(this, new CommandEventArgs(commandName, tbNewScenarioName.Text));
        }

        

        protected void btnNewBlankScenario_Click(object sender, EventArgs e)
        {
            btnCloneScenario.Visible = false;
            btnSaveNewBlankScenario.Visible = true;
            btnConfirmRenameScenario.Visible = false;
            tbNewScenarioName.Text = string.Empty;
            mpeNewName.Show();
        }


        protected void btnCloneScenarioScenario_Click(object sender, EventArgs e)
        {
            btnCloneScenario.Visible = true;
            btnSaveNewBlankScenario.Visible = false;
            btnConfirmRenameScenario.Visible = false;
            tbNewScenarioName.Text = string.Empty;
            mpeNewName.Show();
        }

        protected void ddlScenarioSelector_SelectionChanged(object sender, EventArgs e)
        {
            UpdateScenarioStatistics();
        }

        public void UpdateScenarioStatistics()
        {
            int totalEntries;
            if (ControlScenarioType == ScenarioType.MinCommercialSegment)
            {
                using (var dataAccess = new MinCommercialSegmentDataAccess())
                {
                    totalEntries = dataAccess.CountScenarioEntries(SelectedScenarioId);
                    tbScenarioDescription.Text = dataAccess.GetScenarioDescription(SelectedScenarioId);
                }
            }
            else
            {
                using (var dataAccess = new MaxFleetFactorDataAccess())
                {
                    totalEntries = dataAccess.CountScenarioEntries(SelectedScenarioId);
                    tbScenarioDescription.Text = dataAccess.GetScenarioDescription(SelectedScenarioId);
                }
            }
            
            lblSummary.Text = string.Format("{0:#,0} entries in Scenario", totalEntries);
        }

        protected void btnSaveDescription_Click(object sender, EventArgs e)
        {
            var changedText = tbEditScenarioDescription.Text;
            if (ControlScenarioType == ScenarioType.MinCommercialSegment)
            {
                using (var dataAccess = new MinCommercialSegmentDataAccess())
                {
                    dataAccess.SetScenarioDescription(SelectedScenarioId, changedText);
                }
            }
            else
            {
                using (var dataAccess = new MaxFleetFactorDataAccess())
                {
                    dataAccess.SetScenarioDescription(SelectedScenarioId, changedText);
                }
            }

            tbScenarioDescription.Text = changedText;
        }

        protected void btnUpdateDescription_Click(object sender, EventArgs e)
        {
            tbEditScenarioDescription.Text = tbScenarioDescription.Text;
            mpeEditDescription.Show();
        }

        protected void btnRenameScenario_Click(object sender, EventArgs e)
        {
            btnCloneScenario.Visible = false;
            btnSaveNewBlankScenario.Visible = false;
            btnConfirmRenameScenario.Visible = true;
            tbNewScenarioName.Text = string.Empty;
            mpeNewName.Show();
        }
    }
}