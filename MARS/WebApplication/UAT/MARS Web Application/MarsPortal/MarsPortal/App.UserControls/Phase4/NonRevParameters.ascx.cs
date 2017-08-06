using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.App.Classes.Phase4Dal;

using Mars.App.Classes.Phase4Dal.Enumerators;
using Mars.App.Classes.Phase4Dal.NonRev;
using Mars.App.Classes.Phase4Dal.NonRev.Parameters;

namespace Mars.App.UserControls.Phase4
{
    public partial class NonRevParameters : UserControl
    {
        public bool ShowIndividualVehicleFields { set { pnlIndividualVehicleFields.Visible = value; } }
        public bool ShowDateSelectors { set { pnlDateSelectorFields.Visible = value; } }



        public bool SetOverviewView
        {
            set
            {
                pnlDateSelectorFields.Visible = false;
                pnlIndividualVehicleFields.Visible = value;
                dvAdditionalFieldsLabel.Visible = false;
                dvAdditionalFieldsHr.Visible = false;
                dvVehicleFieldsLabel.Visible = value;
                dvVehicleFieldsHr.Visible = value;
            }
        }

        public bool SetReportView
        {
            set
            {
                pnlDateSelectorFields.Visible = value;
                dvAdditionalFieldsLabel.Visible = value;
                dvAdditionalFieldsHr.Visible = value;
                pnlIndividualVehicleFields.Visible = false;
                lblRevenueStatus.Visible = false;
                ddlRevenueStatus.Visible = false;
                lblStatus.Visible = false;
                lbReasonStatus.Visible = false;
                dvVehicleFieldsLabel.Visible = false;
                dvVehicleFieldsHr.Visible = false;
            }
        }

        public bool ShowKciOperstatGrouping
        {
            set
            {
                lblKciOperstatGrouping.Visible = value;
                rblKciOperstatGrouping.Visible = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page.DataBind();
                using (var dataAccess = new BaseDataAccess())
                {
                    lbFleet.Items.AddRange(dataAccess.GetFleetTypesList(ModuleType.NonRev).ToArray());
                    lbMovementType.Items.AddRange(dataAccess.GetMovementTypesList().ToArray());
                    lbOperationalStatus.Items.AddRange(dataAccess.GetOperationalStatusList().ToArray());
                    //lbOwningArea.Items.AddRange(dataAccess.GetOwningAreasList().Take(500).ToArray());
                }
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {

        }

        public Dictionary<DictionaryParameter, string> GetParameterDictionary()
        {
            var parameters = vhParams.BuildParameterDictionary();
            var nonRevParams = BuildParameterDictionary();
            Dictionary<DictionaryParameter, string> mergedParameters;
            try
            {
                mergedParameters = parameters.Concat(nonRevParams).ToDictionary(d => d.Key, d => d.Value);
            }
            catch
            {
                throw new Exception("Mixed Parameter types!");
            }
            return mergedParameters;
        }

        protected void ddlRevenueStatus_SelectionChanged(object sender, EventArgs e)
        {
            acVin.WebServiceUrl = ddlRevenueStatus.SelectedIndex == 1 ? @"~/AutoComplete.asmx/VehcileAutoCompleteVin"
                                                                    : @"~/AutoComplete.asmx/VehcileAutoCompleteVinAll";
            acLicencePlate.WebServiceUrl = ddlRevenueStatus.SelectedIndex == 1 ? @"~/AutoComplete.asmx/VehcileAutoCompleteLicencePlate"
                                                                            : @"~/AutoComplete.asmx/VehcileAutoCompleteLicencePlateAll";
            acUnitNumber.WebServiceUrl = ddlRevenueStatus.SelectedIndex == 1 ? @"~/AutoComplete.asmx/VehcileAutoCompleteUnitNumber"
                                                                        : @"~/AutoComplete.asmx/VehcileAutoCompleteUnitNumberAll";
            acDriverName.WebServiceUrl = ddlRevenueStatus.SelectedIndex == 1 ? @"~/AutoComplete.asmx/VehcileAutoCompleteDriverName"
                                                                        : @"~/AutoComplete.asmx/VehcileAutoCompleteDriverNameAll";
            acColour.WebServiceUrl = ddlRevenueStatus.SelectedIndex == 1 ? @"~/AutoComplete.asmx/VehcileAutoCompleteModelDescription"
                                                                : @"~/AutoComplete.asmx/VehcileAutoCompleteModelDescriptionNameAll";

            lblMinDaysNonRev.Visible = ddlRevenueStatus.SelectedIndex == 1;
            tbMinDaysNonRev.Visible = ddlRevenueStatus.SelectedIndex == 1;

        }

        public Dictionary<DictionaryParameter, string> BuildParameterDictionary()
        {
            string startDate;
            if (tbToDate.Text == string.Empty || ddlDateRangeDuration.SelectedValue == "")
            {
                startDate = tbToDate.Text == string.Empty
                ? DateTime.Parse(tbSingleReportDate.Text).ToShortDateString()
                : DateTime.Parse(tbFromDate.Text).ToShortDateString();    
            }
            else
            {
                int daysPrevious = int.Parse(ddlDateRangeDuration.SelectedValue);
                startDate = DateTime.Parse(tbToDate.Text).AddDays(-daysPrevious).ToShortDateString();
            }
            
            var endDate = string.Empty;
            if (tbToDate.Text != string.Empty)
            {
                endDate = DateTime.Parse(tbToDate.Text).ToShortDateString();
            }

            var nonRevOnly = ddlRevenueStatus.SelectedValue == string.Empty;
            var minDaysNonRev = tbMinDaysNonRev.Text;
            var returned = new Dictionary<DictionaryParameter, string>
                           {
                               {DictionaryParameter.FleetTypes, GetSelectedKeys(lbFleet.Items)}
                               , {DictionaryParameter.MovementTypes, GetSelectedKeys(lbMovementType.Items)}
                               , {DictionaryParameter.OperationalStatuses, GetSelectedKeys(lbOperationalStatus.Items)}
                               , {DictionaryParameter.OwningArea, acOwningArea.SelectedText}
                               , {DictionaryParameter.LicencePlate, acLicencePlate.SelectedText}
                               , {DictionaryParameter.Vin, acVin.SelectedText}
                               , {DictionaryParameter.UnitNumber, acUnitNumber.SelectedText}
                               , {DictionaryParameter.DriverName, acDriverName.SelectedText}
                               , {DictionaryParameter.Colour, acColour.SelectedText}
                               , {DictionaryParameter.ModelDescription, acModelDesc.SelectedText}
                               , {DictionaryParameter.StartDate, startDate}
                               , {DictionaryParameter.EndDate, endDate}
                               , {DictionaryParameter.DefleetedVehicles, lbFleetStatus.SelectedValue}
                               , {DictionaryParameter.NoReason, lbReasonStatus.SelectedValue}
                               , {DictionaryParameter.MinDaysNonRev, minDaysNonRev}
                               , {DictionaryParameter.NonRevOnly, nonRevOnly.ToString()}
                               , {
                                   rblKciOperstatGrouping.SelectedValue == "Kci"
                                       ? DictionaryParameter.KciGrouping
                                       : DictionaryParameter.OperationalStatusGrouping,
                                   true.ToString()
                               }
                           };

            return returned;
        }

        public static string GetSelectedKeys(ListItemCollection lic)
        {
            var selectedItems = (from ListItem li in lic where li.Selected select li.Value).ToList();
            
            var returned = string.Join(",", selectedItems);
            return returned;
        }

        protected override bool OnBubbleEvent(object sender, EventArgs args)
        {
            var handled = false;

            if (args is CommandEventArgs)
            {
                var commandArgs = args as CommandEventArgs;

                if (commandArgs.CommandName == "ParametersChanged")
                {
                    upnlParameters.Update();
                }
            }
            return handled;
        }

    }
}