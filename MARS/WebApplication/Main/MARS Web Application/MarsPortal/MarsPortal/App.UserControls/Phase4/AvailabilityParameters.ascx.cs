using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.App.Classes.Phase4Bll.ForeignVehicles;
using Mars.App.Classes.Phase4Dal;
using Mars.App.Classes.Phase4Dal.Enumerators;
using Mars.App.Classes.Phase4Dal.NonRev;

namespace Mars.App.UserControls.Phase4
{
    public partial class AvailabilityParameters : UserControl
    {
        public const string AvailabilityParameterSessionName = "AvailabilityParameterSessionName";

        public Dictionary<DictionaryParameter, string> SessionStoredAvailabilityParameters
        {
            get { return (Dictionary<DictionaryParameter, string>)Session[AvailabilityParameterSessionName]; }
            set { Session[AvailabilityParameterSessionName] = value; }
        }

        public bool ShowTopics
        {
            set
            {
                ddlTopics.Visible = value;
                lblTopics.Visible = value;
            }
        }


        public bool ShowLocationLogic
        {
            set
            {
                vhParams.ShowLocationLogic = value;
            }
        }

        public bool ShowOperationalStatus
        {
            set
            {
                lblOperationalStatuses.Visible = value;
                lbOperationalStatus.Visible = value;
            }
        }

        public bool ShowExcludeLongtermForOffAirport
        {
            set { cbExcludeLongtermForOffAirport.Visible = value; }
        }

        public bool ShowMovementTypes
        {
            set
            {
                lblMovementTypes.Visible = value;
                lbMovementType.Visible = value;
            }
        }

        public bool ShowValuesAs
        {
            set
            {
                lblShowValuesAs.Visible = value;
                rblShowValuesAs.Visible = value;
            }
        }

        public bool ShowRevenueStatus
        {
            set
            {
                lblRevenueStatus.Visible = value;
                ddlRevenueStatus.Visible = value;
            }
        }

        public bool ShowMinDaysInCountry
        {
            set
            {
                lblMinDaysInCountry.Visible = value;
                tbMinDaysInCountry.Visible = value;
            }
        }

        public bool ShowVehicleFields
        {
            set
            {
                dvVehicleFieldsHr.Visible = value;
                pnlIndividualVehicleFields.Visible = value;
                lblOperationalStatuses.Visible = value;
                lbOperationalStatus.Visible = value;
                lblMovementTypes.Visible = value;
                lbMovementType.Visible = value;
                lblArea.Visible = value;
                acOwningArea.Visible = value;
                lblMinDaysNonRev.Visible = value;
                tbMinDaysNonRev.Visible = value;
                lblOverdue.Visible = value;
                ddlOverdue.Visible = value;
            }
        }

        public bool ShowForeignPredicament
        {
            set
            {
                lblForeignPredicament.Visible = value;
                ddlForeignPredicament.Visible = value;
            }
        }

        public bool ShowAdditionalFields
        {
            set
            {
                dvAdditionalFieldsHr.Visible = value;
                dvAdditionalFieldsLabel.Visible = value;
                pnlDateSelectorFields.Visible = value;
                
            }
        }

        public bool ShowOwningArea
        {
            set
            {
                lblArea.Visible = value;
                acOwningArea.Visible = value;
            }
        }

        public bool ShowOverdue
        {
            set
            {
                lblOverdue.Visible = value;
                ddlOverdue.Visible = value;
            }
        }

        public bool ShowSingleDateOnly
        {
            set
            {
                pnlDateSelectorFields.Visible = !value;
                pnlSingleDate.Visible = value;
            }
        }

        public bool ShowMinDaysNonRev
        {
            set
            {
                lblMinDaysNonRev.Visible = value;
                tbMinDaysNonRev.Visible = value;                
            }
        }

        public bool AllowMultiSelect
        {
            set
            {
                //pnlBasicParams.Visible = value;
                hfMultiParameterOption.Value = value.ToString();
            }
            get { return hfMultiParameterOption.Value == true.ToString(); }
        }

        public bool ShowDayGrouping
        {
            set
            {
                lblDayGrouping.Visible = value;
                rblDayGrouping.Visible = value;
            }
        }

        public bool PercentageOption
        {
            set
            {
                rblPercentOrValues.Visible = value;
                lblPercentOrValues.Visible = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            vhParams.SessionStoredAvailabilityParameters = SessionStoredAvailabilityParameters;
            if (!IsPostBack)
            {
                Page.DataBind();
                PopulateRadioButtonLists();
                GetDefaultParameters();
                tbSingleDate.Text = pnlSingleDate.Visible ? DateTime.Now.ToShortDateString() : string.Empty;
            }

            string parameter = Request["__EVENTARGUMENT"];
            if (parameter == "BasicParametersChanged")
            {
                if (AllowMultiSelect)
                {
                    vhParams.Visible = !vhParams.Visible;
                    vhMultiParams.Visible = !vhMultiParams.Visible;
                }
                //UpdatePanel();
            }
        }

        private void GetDefaultParameters()
        {
            tbFromDate.Text = DateTime.Now.ToShortDateString();
            tbToDate.Text = DateTime.Now.ToShortDateString();
            using (var dataAccess = new ParamaterListItemGenerator())
            {
                ddlTopics.Items.AddRange(dataAccess.GenerateAvailabilityTopicList().ToArray());
            }

            using (var dataAccess = new BaseDataAccess())
            {
                lbFleet.Items.AddRange(dataAccess.GetFleetTypesList(ModuleType.Availability).ToArray());
                var daysOfWeek = dataAccess.GetDayOfWeeks();
                daysOfWeek.Insert(0, new ListItem("All", string.Empty));
                ddlDayOfWeek.Items.AddRange(daysOfWeek.ToArray());
                lbMovementType.Items.AddRange(dataAccess.GetMovementTypesList().ToArray());
                lbOperationalStatus.Items.AddRange(dataAccess.GetOperationalStatusList().ToArray());

            }
            AddPredicamentDropDownValues();

            if (SessionStoredAvailabilityParameters != null)
            {
                ExtractParametersFromSession();
            }
            else
            {
                rblPercentOrValues.Items[2].Selected = true;
                rblDayGrouping.Items[0].Selected = true;
                rblShowValuesAs.Items[0].Selected = true;
            }


        }

        private void AddPredicamentDropDownValues()
        {
            var predicaments = Enum.GetValues(typeof(ForeignVehiclePredicament)).Cast<ForeignVehiclePredicament>();

            foreach (var p in predicaments)
            {
                ddlForeignPredicament.Items.Add(
                    new ListItem(PredicamentTranslation.GetAvailabilityTopicShortDescription(p), p.ToString()));
            }
        }

        private void ExtractParametersFromSession()
        {
            var savedParams = SessionStoredAvailabilityParameters;
            if (savedParams[DictionaryParameter.FleetTypes] != string.Empty)
            {
                var fleetTypes = savedParams[DictionaryParameter.FleetTypes].Split(',');

                foreach (ListItem li in lbFleet.Items)
                {
                    li.Selected = fleetTypes.Contains(li.Value);
                }
            }

            if (savedParams[DictionaryParameter.StartDate] != string.Empty)
            {
                tbFromDate.Text = savedParams[DictionaryParameter.StartDate];
            }

            if (savedParams[DictionaryParameter.EndDate] != string.Empty)
            {
                tbToDate.Text = savedParams[DictionaryParameter.EndDate];
            }

            if (savedParams[DictionaryParameter.PercentageCalculation] != string.Empty)
            {
                rblPercentOrValues.SelectedValue = savedParams[DictionaryParameter.PercentageCalculation];
            }


            if (savedParams[DictionaryParameter.AvailabilityKeyGrouping] != string.Empty)
            {
                rblShowValuesAs.SelectedValue = savedParams[DictionaryParameter.AvailabilityKeyGrouping];
            }


            if (savedParams[DictionaryParameter.AvailabilityDayGrouping] != string.Empty)
            {
                rblDayGrouping.SelectedValue = savedParams[DictionaryParameter.AvailabilityDayGrouping];
            }

            if (savedParams[DictionaryParameter.MinDaysNonRev] != string.Empty)
            {
                tbMinDaysNonRev.Text = savedParams[DictionaryParameter.MinDaysNonRev];
            }

            if (savedParams[DictionaryParameter.ForeignVehiclePredicament] != string.Empty)
            {
                ddlForeignPredicament.SelectedValue = savedParams[DictionaryParameter.ForeignVehiclePredicament];
            }
            
        }

        public void SimulateQuickSelectLocation(string enteredCode)
        {
            vhParams.SetTbQuickLocation(enteredCode);
            vhParams.UpdateFromQuickLocation();
        }


        public void TransferParameters(Dictionary<DictionaryParameter, string> transParams)
        {
            var selectedOps = transParams[DictionaryParameter.OperationalStatuses].Split(',');
            foreach (ListItem i in lbOperationalStatus.Items)
            {
                i.Selected = selectedOps.Contains(i.Value);
            }

            var selectedMoveTypes = transParams[DictionaryParameter.MovementTypes].Split(',');
            foreach (ListItem i in lbMovementType.Items)
            {
                i.Selected = selectedMoveTypes.Contains(i.Value);
            }

            var selectedFleetTypes = transParams[DictionaryParameter.FleetTypes].Split(',');
            foreach (ListItem i in lbFleet.Items)
            {
                i.Selected = selectedFleetTypes.Contains(i.Value);
            }

            if (transParams[DictionaryParameter.ExcludeOverdue] != string.Empty)
            {
                ddlOverdue.SelectedValue = transParams[DictionaryParameter.ExcludeOverdue];
            }
        }

        private void PopulateRadioButtonLists()
        {
            rblPercentOrValues.Items.Add(new ListItem("Values", PercentageDivisorType.Values.ToString()));
            rblPercentOrValues.Items.Add(new ListItem("% Total Fleet", PercentageDivisorType.TotalFleet.ToString()));
            rblPercentOrValues.Items.Add(new ListItem("% Operational Fleet", PercentageDivisorType.OperationalFleet.ToString()));
            rblPercentOrValues.Items.Add(new ListItem("% Available Fleet", PercentageDivisorType.AvailableFleet.ToString()));

            rblShowValuesAs.Items.Add(new ListItem("Average", AvailabilityGrouping.Average.ToString()));
            rblShowValuesAs.Items.Add(new ListItem("Peak", AvailabilityGrouping.Peak.ToString()));
            rblShowValuesAs.Items.Add(new ListItem("Trough", AvailabilityGrouping.Trough.ToString()));
            rblShowValuesAs.Items.Add(new ListItem("Max", AvailabilityGrouping.Max.ToString()));
            rblShowValuesAs.Items.Add(new ListItem("Min", AvailabilityGrouping.Min.ToString()));
            

            rblShowValuesAs.Items[0].Attributes.CssStyle.Add("margin-right", "10px");
            rblShowValuesAs.Items[2].Attributes.CssStyle.Add("margin-right", "10px");

            rblDayGrouping.Items.Add(new ListItem("Average", AvailabilityGrouping.Average.ToString()));
            rblDayGrouping.Items.Add(new ListItem("Max", AvailabilityGrouping.Max.ToString()));
            rblDayGrouping.Items.Add(new ListItem("Min", AvailabilityGrouping.Min.ToString()));    
        }

        public Dictionary<DictionaryParameter, string> GetParameterDictionary()
        {
            var parameters = vhParams.Visible ? vhParams.BuildParameterDictionary() : vhMultiParams.BuildParameterDictionary();
            var availableParameters = BuildParameterDictionary();
            Dictionary<DictionaryParameter, string> mergedParameters;
            try
            {
                mergedParameters = parameters.Concat(availableParameters).ToDictionary(d => d.Key, d => d.Value);
            }
            catch
            {
                throw new Exception("Mixed Parameter types!");
            }
            SessionStoredAvailabilityParameters = mergedParameters;
            return mergedParameters;
        }

        private Dictionary<DictionaryParameter, string> BuildParameterDictionary()
        {


            var endDate = string.Empty;
            if (tbToDate.Text != string.Empty)
            {
                endDate = DateTime.Parse(tbToDate.Text).ToShortDateString();
            }
            string startDate = string.Empty;
            if (pnlSingleDate.Visible && tbSingleDate.Text != string.Empty)
            {
                startDate = DateTime.Parse(tbSingleDate.Text).ToShortDateString();
            }
            else
            {
                if (tbFromDate.Text != string.Empty)
                {
                    if (ddlDateRangeDuration.SelectedValue == string.Empty)       //Between selected
                    {
                        startDate = DateTime.Parse(tbFromDate.Text).ToShortDateString();
                    }
                    else
                    {
                        if (ddlDateRangeDuration.SelectedValue == "0")              //Today Selected
                        {
                            startDate = DateTime.Now.ToShortDateString();
                            endDate = DateTime.Now.ToShortDateString();
                        }
                        else            // Dropdown list used
                        {
                            int daysPrevious = int.Parse(ddlDateRangeDuration.SelectedValue);
                            startDate = DateTime.Parse(tbToDate.Text).AddDays(-daysPrevious).ToShortDateString();
                        }

                    }
                }
            }
            string predicament = string.Empty;
            if (ddlForeignPredicament.Visible)
            {
                var parsedPredicament = Enum.Parse(typeof (ForeignVehiclePredicament), ddlForeignPredicament.SelectedValue);
                predicament = parsedPredicament.ToString();
            }

            var returned = new Dictionary<DictionaryParameter, string>
                           {
                               {DictionaryParameter.FleetTypes, GetSelectedKeys(lbFleet.Items)}
                               , {DictionaryParameter.DayOfWeek, ddlDayOfWeek.SelectedValue}
                               , {DictionaryParameter.PercentageCalculation, rblPercentOrValues.SelectedValue}
                               , {DictionaryParameter.StartDate, startDate}
                               , {DictionaryParameter.LicencePlate, acLicencePlate.SelectedText}
                               , {DictionaryParameter.Vin, acVin.SelectedText}
                               , {DictionaryParameter.UnitNumber, acUnitNumber.SelectedText}
                               , {DictionaryParameter.DriverName, acDriverName.SelectedText}
                               , {DictionaryParameter.Colour, acColour.SelectedText}
                               , {DictionaryParameter.ModelDescription, acModelDesc.SelectedText}
                               , {DictionaryParameter.EndDate, endDate}
                               , {DictionaryParameter.ExcludeOverdue, ddlOverdue.SelectedValue}
                               , {DictionaryParameter.OwningArea, acOwningArea.SelectedText}
                               , {DictionaryParameter.AvailabilityKeyGrouping, rblShowValuesAs.SelectedValue}
                               , {DictionaryParameter.AvailabilityDayGrouping, rblDayGrouping.SelectedValue}
                               , {DictionaryParameter.MinDaysNonRev, tbMinDaysNonRev.Text}
                               , {DictionaryParameter.MinDaysInCountry, tbMinDaysInCountry.Text}
                               , {DictionaryParameter.OperationalStatuses, GetSelectedKeys(lbOperationalStatus.Items)}
                               , {DictionaryParameter.MovementTypes, GetSelectedKeys(lbMovementType.Items)}
                               , {DictionaryParameter.ForeignVehiclePredicament, predicament}
                               , {DictionaryParameter.ExcludeAirportForLongtermRentals, cbExcludeLongtermForOffAirport.Checked.ToString()}
                           };
            
            return returned;
        }

        public static string GetSelectedKeys(ListItemCollection lic, bool skipAll = true)
        {
            
            var selectedItems = (from ListItem li in lic where li.Selected select li.Value).ToList();
            if(skipAll)
            {
                if (selectedItems.Count == lic.Count) return string.Empty;    
            }
            
            var returned = string.Join(",", selectedItems);
            return returned;
        }

        public void cbBasicParams_Checked(object sender, EventArgs e)
        {
            //ucMultiParameters.Visible = !cbBasicParams.Checked;
            //ucSingleParameters.Visible = cbBasicParams.Checked;
            //upnlParams.Update();
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