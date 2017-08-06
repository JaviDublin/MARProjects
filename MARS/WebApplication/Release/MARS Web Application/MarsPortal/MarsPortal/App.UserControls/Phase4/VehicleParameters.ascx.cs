using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.App.Classes.BLL.ExtensionMethods;
using Mars.App.Classes.Phase4Bll.Parameters;
using Mars.App.Classes.Phase4Dal;

using Mars.App.Classes.Phase4Dal.Enumerators;
using Mars.App.Site.ForeignVehicles;

namespace Mars.App.UserControls.Phase4
{
    public partial class VehicleParameters : UserControl
    {
        public Dictionary<DictionaryParameter, string> SessionStoredAvailabilityParameters { get; set; }

        public const string ParameterSessionName = "ParameterSessionName";

        private ParamaterDropdownListHolder SessionStoredDropDownLists
        {
            get { return (ParamaterDropdownListHolder) Session[ParameterSessionName]; }
            set { Session[ParameterSessionName] = value; }
        }

        public bool ShowLocationLogic
        {
            set { rblLocationLogic.Visible = value; }
        }

        public bool ShowPickUpReturnLogic
        {
            set { rblPickupReturnLogic.Visible = value; }
        }

        public bool ShowUpgradedLogic
        {
            set { rblUpgradedLogic.Visible = value; }
        }

        public bool HideLocationBranch
        {
            set 
            {
                rblCmsOpsLogic.Visible = !value;
                lblPool.Visible = !value;
                lblRegion.Visible = !value;
                lblLocationCountry.Visible = !value;
                ddlLocationCountry.Visible = !value;
                pnlLocationBranch2.Visible = !value;
                pnlLocationBranch3.Visible = !value;
                pnlLocationBranch4.Visible = !value;
                pnlLocationBranch5.Visible = !value;
                lblLocation.Visible = !value;
                ddlLocation.Visible = !value;
                lblQuickLocation.Visible = !value;
                tbQuickLocation.Visible = !value;

            }
        }

        public bool HideCarClass
        {
            set
            {
                ddlCarClass.Visible = !value;
                lblCarClass.Visible = !value;
            }
        }

        public bool HideCarGroup
        {
            set
            {
                ddlCarGroup.Visible = !value;
                lblCarGroup.Visible = !value;
            }
        }


        private bool CmsLogicSelected {get { return (rblCmsOpsLogic.SelectedValue == "Cms"); }}

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CmsOpsLogicSelected();
                if (SessionStoredDropDownLists == null)
                {
                    GenerateNewDropdowns();    
                }
                else
                {
                    RestoreOldDropdowns();
                }
                
                ToggleQuickCarGroup();
            }
            string parameter = Request["__EVENTARGUMENT"];
            if (parameter != null)
            {
                if (parameter.Contains("LocationSingle"))
                {
                    UpdateFromQuickLocation();
                    UpdatePanel();
                }
                if (parameter.Contains("CarGroupSingle"))
                {
                    UpdateFromQuickCarGroup();
                    UpdatePanel();
                }
                
            }

            SetTransferredParameter();
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            acCarGroup.ContextKey = ddlOwningCountry.SelectedValue;
        }

        private void SetTransferredParameter()
        {
            if (SessionStoredAvailabilityParameters == null) return;

            if (Session[FleetOverview.FleetOverviewSessionTransferName] == null) return;

            if (SessionStoredAvailabilityParameters[DictionaryParameter.LocationCountry] != string.Empty)
            {
                ddlLocationCountry.SelectedValue =
                    SessionStoredAvailabilityParameters[DictionaryParameter.LocationCountry];
                FillDropDowns(true, 0, SessionStoredAvailabilityParameters, false, false);
            }

            if (SessionStoredAvailabilityParameters[DictionaryParameter.OwningCountry] != string.Empty)
            {
                ddlOwningCountry.SelectedValue =
                    SessionStoredAvailabilityParameters[DictionaryParameter.OwningCountry];
            }

            //

            if (SessionStoredAvailabilityParameters[DictionaryParameter.Location] != string.Empty)
            {
                TransferDropdownValue(DictionaryParameter.Location, ddlLocation);
                ddlPool.SelectedValue = SessionStoredAvailabilityParameters[DictionaryParameter.Pool];
                ddlRegion.SelectedValue = SessionStoredAvailabilityParameters[DictionaryParameter.Region];
                ddlArea.SelectedValue = SessionStoredAvailabilityParameters[DictionaryParameter.Area];
                ddlLocationGroup.SelectedValue = SessionStoredAvailabilityParameters[DictionaryParameter.LocationGroup];
            }
            else if (SessionStoredAvailabilityParameters[DictionaryParameter.LocationGroup] != string.Empty
                 || SessionStoredAvailabilityParameters[DictionaryParameter.Area] != string.Empty)
            {
                if (SessionStoredAvailabilityParameters[DictionaryParameter.LocationGroup] != string.Empty)
                {
                    TransferDropdownValue(DictionaryParameter.LocationGroup, ddlLocationGroup);
                    ddlPool.SelectedValue = SessionStoredAvailabilityParameters[DictionaryParameter.Pool];
                }
                else
                {
                    TransferDropdownValue(DictionaryParameter.Area, ddlArea);
                    ddlRegion.SelectedValue = SessionStoredAvailabilityParameters[DictionaryParameter.Region];
                }
                

            }
            else if (SessionStoredAvailabilityParameters[DictionaryParameter.Pool] != string.Empty
                || SessionStoredAvailabilityParameters[DictionaryParameter.Region] != string.Empty)
            {
                if (SessionStoredAvailabilityParameters[DictionaryParameter.Pool] != string.Empty)
                {
                    TransferDropdownValue(DictionaryParameter.Pool, ddlPool);
                }
                else
                {
                    TransferDropdownValue(DictionaryParameter.Region, ddlRegion);
                }

            }
            else
            {
                TransferDropdownValue(DictionaryParameter.OwningCountry, ddlOwningCountry);
            }

            UpdatePanel();

        }

        private void TransferDropdownValue(DictionaryParameter paramType, DropDownList ddl)
        {
            if (SessionStoredAvailabilityParameters.ContainsValueAndIsntEmpty(paramType))
            {
                ddl.SelectedValue = SessionStoredAvailabilityParameters[paramType];
                int paramLevel;
                switch (paramType)
                {
                    case DictionaryParameter.CheckOutLocation:

                    case DictionaryParameter.Location:
                        paramLevel = 3;
                        break;
                    case DictionaryParameter.CheckOutLocationGroup:
                    case DictionaryParameter.LocationGroup:
                    case DictionaryParameter.CheckOutArea:
                    case DictionaryParameter.Area:
                        paramLevel = 2;
                        break;
                    case DictionaryParameter.CheckOutPool:
                    case DictionaryParameter.Pool:
                    case DictionaryParameter.CheckOutRegion:
                    case DictionaryParameter.Region:
                        paramLevel = 1;
                        break;
                    default:
                        paramLevel = 0;
                        break;

                }
                FillDropDowns(true, paramLevel, SessionStoredAvailabilityParameters, false, false);
            }
        }

        public void rblPickupReturnLogic_Changed(object sender, EventArgs e)
        {
            var cmdEvent = new CommandEventArgs("PickupLogicChanged", null);

            RaiseBubbleEvent(this, cmdEvent);
        }

        private void RestoreOldDropdowns()
        {
            var oldDropdowns = SessionStoredDropDownLists;
            UpdateDropdownList(ddlOwningCountry, oldDropdowns.OwningCountry);
            UpdateDropdownList(ddlCarSegment, oldDropdowns.CarSegment);
            UpdateDropdownList(ddlCarClass, oldDropdowns.CarClass);
            UpdateDropdownList(ddlCarGroup, oldDropdowns.CarGroup);
            UpdateDropdownList(ddlLocationCountry, oldDropdowns.LocationCountry);
            UpdateDropdownList(ddlPool, oldDropdowns.Pool);
            UpdateDropdownList(ddlLocationGroup, oldDropdowns.LocationGroup);
            UpdateDropdownList(ddlLocation, oldDropdowns.Location);
        }

        private void UpdateDropdownList(DropDownList toUpdate, DropDownList source)
        {
            foreach (ListItem i in source.Items)
            {
                toUpdate.Items.Add(i);
            }
            toUpdate.SelectedIndex = source.SelectedIndex;
        }


        private void GenerateNewDropdowns()
        {
            using (var generator = new ParamaterListItemGenerator())
            {
                var owningCountries = generator.GenerateList(ParameterType.OwningCountry, null).ToArray();
                var locationCountries = generator.GenerateList(ParameterType.LocationCountry, null).ToArray();
                var emptyList = generator.GenerateEmptyList().ToArray();
                ddlOwningCountry.Items.AddRange(owningCountries);
                ddlOwningCountry.SelectedIndex = 0;
                ddlLocationCountry.Items.AddRange(locationCountries);
                ddlLocationCountry.SelectedIndex = 0;
                ddlCarSegment.Items.AddRange(emptyList);
                ddlCarSegment.SelectedIndex = 0;
                ddlCarClass.Items.AddRange(emptyList);
                ddlCarClass.SelectedIndex = 0;
                ddlCarGroup.Items.AddRange(emptyList);
                ddlCarGroup.SelectedIndex = 0;
                ddlPool.Items.AddRange(emptyList);
                ddlPool.SelectedIndex = 0;
                ddlRegion.Items.AddRange(emptyList);
                ddlRegion.SelectedIndex = 0;
                ddlArea.Items.AddRange(emptyList);
                ddlArea.SelectedIndex = 0;
                ddlLocationGroup.Items.AddRange(emptyList);
                ddlLocationGroup.SelectedIndex = 0;
                ddlLocation.Items.AddRange(emptyList);
                ddlLocation.SelectedIndex = 0;
            }
        }

        private void UpdatePanel()
        {
            RaiseBubbleEvent(this, new CommandEventArgs("ParametersChanged", null));
        }
        
        protected void rblCmsOpsLogic_SelectionChanged(object sender, EventArgs e)
        {
            CmsOpsLogicSelected();
        }

        protected void ParameterChanged(object sender, EventArgs e)
        {
            var senderDropdown = sender as DropDownList;
            if (senderDropdown == null) return;
            
            var sideAndDepth = GetTreeSideAndDepth(senderDropdown.ID);
            bool locationBranch = sideAndDepth.Item1;
            int depth = sideAndDepth.Item2;

            var parameters = BuildParameterDictionary();
            bool clearCms = false;
            bool clearOps = false;
            if (locationBranch && (depth == 1 || depth == 2))       //If a CMS field is slected at depth 
            {
                if (CmsLogicSelected)
                {
                    parameters[DictionaryParameter.Region] = string.Empty;
                    parameters[DictionaryParameter.Area] = string.Empty;
                    clearOps = true;
                }
                else
                {
                    parameters[DictionaryParameter.LocationGroup] = string.Empty;
                    parameters[DictionaryParameter.Pool] = string.Empty;
                    clearCms = true;
                }
            }

            FillDropDowns(locationBranch, depth, parameters, clearCms, clearOps);
            ToggleQuickCarGroup();
            UpdatePanel();
            SetSessionDropdownLists();
        }

        private void SetSessionDropdownLists()
        {

            var dropdownLists = new ParamaterDropdownListHolder
            {
                OwningCountry = ddlOwningCountry,
                CarSegment = ddlCarSegment,
                CarClass = ddlCarClass,
                CarGroup = ddlCarGroup,
                LocationCountry = ddlLocationCountry,
                Pool = ddlPool,
                LocationGroup = ddlLocationGroup,
                Location = ddlLocation
            };
            SessionStoredDropDownLists = dropdownLists;
        }

        public void SetTbQuickLocation(string enteredLocation)
        {
            tbQuickLocation.Text = enteredLocation;
        }

        public void UpdateFromQuickLocation()
        {
            var locationWwd = tbQuickLocation.Text;
            if (locationWwd == string.Empty) return;
            var parameters = BuildParameterDictionary();
            using (var generator = new ParamaterListItemGenerator())
            {
                var country = generator.GetCountry(locationWwd);
                if (country == string.Empty)
                {
                    tbQuickLocation.Text = string.Empty;
                    return;
                }
                    
                var poolId = generator.GetLocationBranchId(ParameterType.Pool, locationWwd);
                var locationGroupId = generator.GetLocationBranchId(ParameterType.LocationGroup, locationWwd);
                var areaId = generator.GetLocationBranchId(ParameterType.Area, locationWwd);
                var regionId = generator.GetLocationBranchId(ParameterType.Region, locationWwd);
                var locationId = generator.GetLocationBranchId(ParameterType.Location, locationWwd);
                parameters[DictionaryParameter.LocationCountry] = country;
                parameters[DictionaryParameter.Pool] = poolId.ToString(CultureInfo.InvariantCulture);
                parameters[DictionaryParameter.LocationGroup] = locationGroupId.ToString(CultureInfo.InvariantCulture);
                parameters[DictionaryParameter.Area] = areaId.ToString(CultureInfo.InvariantCulture);
                parameters[DictionaryParameter.Region] = regionId.ToString(CultureInfo.InvariantCulture);
                parameters[DictionaryParameter.Location] = locationId.ToString(CultureInfo.InvariantCulture);
            }

            FillDropDowns(true, 0, parameters);
            SelectAutoCompleteParameters(parameters);
            tbQuickLocation.Text = string.Empty;
            SetSessionDropdownLists();
        }

        private void UpdateFromQuickCarGroup()
        {
            var carGroup = tbQuickCarGroup.Text;
            if (carGroup == string.Empty) return;
            var country = ddlOwningCountry.SelectedValue;
            var parameters = BuildParameterDictionary();


            using (var generator = new ParamaterListItemGenerator())
            {
                if (!generator.DoesCarGroupExistForCountry(country, carGroup))
                {
                    tbQuickCarGroup.Text = string.Empty;
                    return;
                }

                var carSegment = generator.GetCarBranchId(ParameterType.CarSegment, carGroup, country);
                var carClass = generator.GetCarBranchId(ParameterType.CarClass, carGroup, country);
                var carGroupId = generator.GetCarBranchId(ParameterType.CarGroup, carGroup, country);

                parameters[DictionaryParameter.CarSegment] = carSegment.ToString(CultureInfo.InvariantCulture);
                parameters[DictionaryParameter.CarClass] = carClass.ToString(CultureInfo.InvariantCulture);
                parameters[DictionaryParameter.CarGroup] = carGroupId.ToString(CultureInfo.InvariantCulture);
            }
            FillDropDowns(false, 0, parameters);
            SelectAutoCompleteParameters(parameters);
            tbQuickCarGroup.Text = string.Empty;
            SetSessionDropdownLists();
        }

        public Dictionary<DictionaryParameter, string> BuildParameterDictionary()
        {
            var returned = new Dictionary<DictionaryParameter, string>
                           {
                                 {DictionaryParameter.OwningCountry,ddlOwningCountry.SelectedValue}
                               , {DictionaryParameter.LocationCountry,ddlLocationCountry.SelectedValue}
                               , {DictionaryParameter.Pool,ddlPool.SelectedValue}
                               , {DictionaryParameter.LocationGroup, ddlLocationGroup.SelectedValue}
                               , {DictionaryParameter.Region, ddlRegion.SelectedValue}
                               , {DictionaryParameter.Area, ddlArea.SelectedValue}
                               , {DictionaryParameter.Location, ddlLocation.SelectedValue}
                               , {DictionaryParameter.CarSegment, ddlCarSegment.SelectedValue}
                               , {DictionaryParameter.CarClass, ddlCarClass.SelectedValue}
                               , {DictionaryParameter.ExpectedLocationLogic, rblLocationLogic.SelectedValue}
                               , {DictionaryParameter.UpgradedLogic, rblUpgradedLogic.SelectedValue}
                               
                               , {DictionaryParameter.CarGroup, ddlCarGroup.SelectedValue}
                               , {DictionaryParameter.CmsSelected, (rblCmsOpsLogic.SelectedValue == "Cms").ToString()}
                           };

            return returned;
        }

        private void SelectAutoCompleteParameters(Dictionary<DictionaryParameter, string> parameters)
        {
            SetDropdownList(ddlLocationCountry, parameters[DictionaryParameter.LocationCountry]);
            SetDropdownList(ddlPool, parameters[DictionaryParameter.Pool]);
            SetDropdownList(ddlLocationGroup, parameters[DictionaryParameter.LocationGroup]);
            SetDropdownList(ddlArea, parameters[DictionaryParameter.Area]);
            SetDropdownList(ddlRegion, parameters[DictionaryParameter.Region]);
            SetDropdownList(ddlLocation, parameters[DictionaryParameter.Location]);
            SetDropdownList(ddlCarSegment, parameters[DictionaryParameter.CarSegment]);
            SetDropdownList(ddlCarClass, parameters[DictionaryParameter.CarClass]);
            SetDropdownList(ddlCarGroup, parameters[DictionaryParameter.CarGroup]);
        }

        private void SetDropdownList(DropDownList ddl, string value)
        {
            if (string.IsNullOrEmpty(value)) return;
            ddl.SelectedValue = value;
        }

        /// <summary>
        /// Fills Drop down lists from database
        /// </summary>
        /// <param name="locationBranch">Clears everything below depth for this branch</param>
        /// <param name="depth">Clears everything below this depth</param>
        /// <param name="parameters"></param>
        private void FillDropDowns(bool locationBranch, int depth, Dictionary<DictionaryParameter, string> parameters, bool clearCms = false, bool clearOps = false)
        {
            
            using (var generator = new ParamaterListItemGenerator())
            {
                if (locationBranch)
                {
                    bool locationSelectedOnCms = CmsLogicSelected && depth == 3;
                    bool locationSelectedOnOps = !CmsLogicSelected && depth == 3;
                    
                        if (locationSelectedOnCms)
                        {
                            string areaId = string.Empty;
                            string regionId = string.Empty;
                            if (!string.IsNullOrEmpty(parameters[DictionaryParameter.Location]))
                            {
                                var locationWwd = generator.GetWwdFromLocationId(int.Parse(parameters[DictionaryParameter.Location]));
                                areaId = generator.GetLocationBranchId(ParameterType.Area,
                                    locationWwd).ToString();
                                regionId = generator.GetLocationBranchId(ParameterType.Region,
                                    locationWwd).ToString();
                            }


                            parameters[DictionaryParameter.Area] = areaId;
                            parameters[DictionaryParameter.Region] = regionId;

                        }
                        if (locationSelectedOnOps)
                        {
                            string poolId = string.Empty;
                            string locationGroupId = string.Empty;

                            if (!string.IsNullOrEmpty(parameters[DictionaryParameter.Location]))
                            {
                                var locationWwd =
                                    generator.GetWwdFromLocationId(int.Parse(parameters[DictionaryParameter.Location]));
                                poolId = generator.GetLocationBranchId(ParameterType.Pool,
                                    locationWwd).ToString();
                                locationGroupId = generator.GetLocationBranchId(ParameterType.LocationGroup,
                                    locationWwd).ToString();
                            }
                            parameters[DictionaryParameter.Pool] = poolId;
                            parameters[DictionaryParameter.LocationGroup] = locationGroupId;
                        }
                    

                    var pools = generator.GenerateList(ParameterType.Pool, parameters).ToArray();
                    var locationGroups = generator.GenerateList(ParameterType.LocationGroup, parameters).ToArray();
                    var locations = generator.GenerateList(ParameterType.Location, parameters).ToArray();
                    var regions = generator.GenerateList(ParameterType.Region, parameters).ToArray();
                    var areas = generator.GenerateList(ParameterType.Area, parameters).ToArray();



                    ClearAndPopulateDdl(ddlPool, pools, depth <= 0 || clearCms || locationSelectedOnOps);
                    ClearAndPopulateDdl(ddlRegion, regions, depth <= 0 || clearOps || locationSelectedOnCms);
                    ClearAndPopulateDdl(ddlLocationGroup, locationGroups, depth <= 1 || clearCms || locationSelectedOnOps);
                    ClearAndPopulateDdl(ddlArea, areas, depth <= 1 || clearOps || locationSelectedOnCms);
                    ClearAndPopulateDdl(ddlLocation, locations, depth <= 2);
                    if (locationSelectedOnCms)
                    {
                        SetDropdownList(ddlArea, parameters[DictionaryParameter.Area]);
                        SetDropdownList(ddlRegion, parameters[DictionaryParameter.Region]);
                    }
                    if (locationSelectedOnOps)
                    {
                        SetDropdownList(ddlPool, parameters[DictionaryParameter.Pool]);
                        SetDropdownList(ddlLocationGroup, parameters[DictionaryParameter.LocationGroup]);
                    }

                }
                else
                {
                    var carSegments = generator.GenerateList(ParameterType.CarSegment, parameters).ToArray();
                    var carClass = generator.GenerateList(ParameterType.CarClass, parameters).ToArray();
                    var carGroup = generator.GenerateList(ParameterType.CarGroup, parameters).ToArray();

                    ClearAndPopulateDdl(ddlCarSegment, carSegments, depth <= 0);
                    ClearAndPopulateDdl(ddlCarClass, carClass, depth <= 1);
                    ClearAndPopulateDdl(ddlCarGroup, carGroup, depth <= 2);
                }
            }
        }

        private void ClearAndPopulateDdl(DropDownList ddlToClear, ListItem[] itemsToAdd, bool refreshList)
        {
            if (refreshList)
            {
                ddlToClear.Items.Clear();
                ddlToClear.Items.AddRange(itemsToAdd);
                ddlToClear.SelectedIndex = 0;
            }
        }

        private void ToggleQuickCarGroup()
        {
            var showQuickCarGroup = !string.IsNullOrEmpty(ddlOwningCountry.SelectedValue);

            lblQuickCarGroup.Visible = showQuickCarGroup;
            tbQuickCarGroup.Visible = showQuickCarGroup;
        }

        private void CmsOpsLogicSelected()
        {
            var cmsSelected = rblCmsOpsLogic.SelectedValue == "Cms";

            ddlPool.Visible = cmsSelected;
            ddlLocationGroup.Visible = cmsSelected;
            lblPool.Visible = cmsSelected;
            lblLocationGroup.Visible = cmsSelected;
            
            ddlRegion.Visible = !cmsSelected;
            ddlArea.Visible = !cmsSelected;
            lblRegion.Visible = !cmsSelected;
            lblArea.Visible = !cmsSelected;

            if (!IsPostBack) return;

            if (ddlLocation.SelectedValue == string.Empty)
            {
                var parameters = BuildParameterDictionary();
                FillDropDowns(true, 2, parameters, true, true);
            }
        }

        private Tuple<bool, int> GetTreeSideAndDepth(string id)
        {
            bool locationBranch;
            int depth;
            switch (id)
            {
                case "ddlOwningCountry":
                    locationBranch = false;
                    tbQuickCarGroup.Text = string.Empty;
                    depth = 0;
                    break;
                case "ddlLocationCountry":
                    locationBranch = true;
                    depth = 0;
                    break;
                case "ddlCarSegment":
                    locationBranch = false;
                    depth = 1;
                    break;
                case "ddlPool":
                case "ddlRegion":
                    locationBranch = true;
                    depth = 1;
                    break;
                case "ddlCarClass":
                    locationBranch = false;
                    depth = 2;
                    break;
                case "ddlLocationGroup":
                case "ddlArea":
                    locationBranch = true;
                    depth = 2;
                    break;
                case "ddlCarGroup":
                    locationBranch = false;
                    depth = 3;
                    break;
                case "ddlLocation":
                    locationBranch = true;
                    depth = 3;
                    break;
                default:
                    locationBranch = false;
                    depth = -1;
                    break;
            }
            return new Tuple<bool, int>(locationBranch, depth);
        }
    }
}