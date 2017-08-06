using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.App.Classes.Phase4Bll.Parameters;
using Mars.App.Classes.Phase4Dal;
using Mars.App.Classes.Phase4Dal.Enumerators;
using Mars.App.Site.ForeignVehicles;

namespace Mars.App.UserControls.Phase4.Reservations
{
    public partial class ReservationVehicleParameters : UserControl
    {
        private const string ParameterSessionName = "ReservationsVehicleParameterSessionName";


        public Dictionary<DictionaryParameter, string> SessionStoredReservationParameters { get; set; }

        private ParamaterDropdownListHolder SessionStoredDropDownLists
        {
            get { return (ParamaterDropdownListHolder)Session[ParameterSessionName]; }
            set { Session[ParameterSessionName] = value; }
        }
        
        private bool CmsLogicSelected { get { return (rblCmsOpsLogic.SelectedValue == "Cms"); } }
        

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

                
            }
            string parameter = Request["__EVENTARGUMENT"];
            if (parameter == "LocationIn")
            {
                UpdateFromQuickLocation(false);
                UpdatePanel();
            }
            if (parameter == "LocationOut")
            {
                UpdateFromQuickLocation(true);
                UpdatePanel();
            }
            if (parameter == "CarGroupSingle")
            {
                UpdateFromQuickCarGroup();
                UpdatePanel();
            }
            SetTransferredParameter();
        }

        
        protected void Page_PreRender(object sender, EventArgs e)
        {
            acCarGroup.ContextKey = ddlCheckOutCountry.SelectedValue;
            ShowCarGroupQuickSelect();
            SetSessionDropdownLists();    
        }

        private void SetTransferredParameter()
        {
            if (SessionStoredReservationParameters == null) return;

            if (Session[ReservationOverview.ReservationOverviewSessionTransferName] == null) return;
            
            if (SessionStoredReservationParameters[DictionaryParameter.LocationCountry] != string.Empty)
            {
                ddlCheckInCountry.SelectedValue =
                    SessionStoredReservationParameters[DictionaryParameter.LocationCountry];
                FillDropDowns(true, 0, SessionStoredReservationParameters, false, false, true);       
            }

            //

            if (SessionStoredReservationParameters[DictionaryParameter.CheckOutLocation] != string.Empty)
            {
                TransferDropdownValue(DictionaryParameter.CheckOutLocation, ddlCheckOutLocation);
            }
            else if (SessionStoredReservationParameters[DictionaryParameter.CheckOutLocationGroup] != string.Empty
                 || SessionStoredReservationParameters[DictionaryParameter.CheckOutArea] != string.Empty)
            {
                if (SessionStoredReservationParameters[DictionaryParameter.CheckOutLocationGroup] != string.Empty)
                {
                    TransferDropdownValue(DictionaryParameter.CheckOutLocationGroup, ddlCheckOutLocationGroup);
                }
                else
                {
                    TransferDropdownValue(DictionaryParameter.CheckOutArea, ddlCheckOutArea);
                }
            }
            else if (SessionStoredReservationParameters[DictionaryParameter.CheckOutPool] != string.Empty
                || SessionStoredReservationParameters[DictionaryParameter.CheckOutRegion] != string.Empty)
            {
                if (SessionStoredReservationParameters[DictionaryParameter.CheckOutPool] != string.Empty)
                {
                    TransferDropdownValue(DictionaryParameter.CheckOutPool, ddlCheckOutPool);
                }
                else
                {
                    TransferDropdownValue(DictionaryParameter.CheckOutRegion, ddlCheckOutRegion);
                }
                
            }
            else
            {
                TransferDropdownValue(DictionaryParameter.CheckOutCountry, ddlCheckOutCountry);
            }
            
            UpdatePanel();
            
        }

        private void TransferDropdownValue(DictionaryParameter paramType, DropDownList ddl)
        {
            if (SessionStoredReservationParameters[paramType] != string.Empty)
            {
                ddl.SelectedValue = SessionStoredReservationParameters[paramType];
                int paramLevel;
                switch (paramType)
                {
                    case DictionaryParameter.CheckOutLocation:
                        paramLevel = 3;
                        break;
                    case DictionaryParameter.CheckOutLocationGroup:
                    case DictionaryParameter.CheckOutArea:
                        paramLevel = 2;
                        break;
                    case DictionaryParameter.CheckOutPool:
                    case DictionaryParameter.CheckOutRegion:
                        paramLevel = 1;
                        break;
                    default:
                        paramLevel = 0;
                        break;

                }
                FillDropDowns(false, paramLevel, SessionStoredReservationParameters, false, false, true);
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
            UpdateDropdownList(ddlCheckInCountry, oldDropdowns.OwningCountry);
            UpdateDropdownList(ddlCheckInPool, oldDropdowns.Pool);
            UpdateDropdownList(ddlCheckInRegion, oldDropdowns.Region);
            UpdateDropdownList(ddlCheckInLocationGroup, oldDropdowns.LocationGroup);
            UpdateDropdownList(ddlCheckInArea, oldDropdowns.Area);
            UpdateDropdownList(ddlCheckInLocation, oldDropdowns.Location);


            UpdateDropdownList(ddlCarSegment, oldDropdowns.CarSegment);
            UpdateDropdownList(ddlCarClass, oldDropdowns.CarClass);
            UpdateDropdownList(ddlCarGroup, oldDropdowns.CarGroup);

            UpdateDropdownList(ddlCheckOutCountry, oldDropdowns.CheckOutCountry);
            UpdateDropdownList(ddlCheckOutPool, oldDropdowns.CheckOutPool);
            UpdateDropdownList(ddlCheckOutRegion, oldDropdowns.CheckOutRegion);
            UpdateDropdownList(ddlCheckOutLocationGroup, oldDropdowns.CheckOutLocationGroup);
            UpdateDropdownList(ddlCheckOutArea, oldDropdowns.CheckOutArea);
            UpdateDropdownList(ddlCheckOutLocation, oldDropdowns.CheckOutLocation);
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
                var owningCountries = generator.GenerateList(ParameterType.CountryCheckOut, null).ToArray();
                var locationCountries = generator.GenerateList(ParameterType.LocationCountry, null).ToArray();
                var emptyList = generator.GenerateEmptyList().ToArray();
                ddlCheckOutCountry.Items.AddRange(owningCountries);
                ddlCheckOutCountry.SelectedIndex = 0;
                ddlCheckInCountry.Items.AddRange(locationCountries);
                ddlCheckInCountry.SelectedIndex = 0;
                ddlCarSegment.Items.AddRange(emptyList);
                ddlCarSegment.SelectedIndex = 0;
                ddlCarClass.Items.AddRange(emptyList);
                ddlCarClass.SelectedIndex = 0;
                ddlCarGroup.Items.AddRange(emptyList);
                ddlCarGroup.SelectedIndex = 0;

                ddlCheckInPool.Items.AddRange(emptyList);
                ddlCheckInPool.SelectedIndex = 0;
                ddlCheckInRegion.Items.AddRange(emptyList);
                ddlCheckInRegion.SelectedIndex = 0;
                ddlCheckInArea.Items.AddRange(emptyList);
                ddlCheckInArea.SelectedIndex = 0;
                ddlCheckInLocationGroup.Items.AddRange(emptyList);
                ddlCheckInLocationGroup.SelectedIndex = 0;
                ddlCheckInLocation.Items.AddRange(emptyList);
                ddlCheckInLocation.SelectedIndex = 0;

                ddlCheckOutPool.Items.AddRange(emptyList);
                ddlCheckOutPool.SelectedIndex = 0;
                ddlCheckOutRegion.Items.AddRange(emptyList);
                ddlCheckOutRegion.SelectedIndex = 0;
                ddlCheckOutArea.Items.AddRange(emptyList);
                ddlCheckOutArea.SelectedIndex = 0;
                ddlCheckOutLocationGroup.Items.AddRange(emptyList);
                ddlCheckOutLocationGroup.SelectedIndex = 0;
                ddlCheckOutLocation.Items.AddRange(emptyList);
                ddlCheckOutLocation.SelectedIndex = 0;
            }
        }

        public void UpdatePanel()
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
            bool owningLocationOnly = sideAndDepth.Item3;

            var parameters = BuildParameterDictionary();
            bool clearCms = false;
            bool clearOps = false;
            if (depth == 1 || depth == 2)       //If a CMS field is slected at depth 
            {
                if (CmsLogicSelected)
                {
                    parameters[DictionaryParameter.Region] = string.Empty;
                    parameters[DictionaryParameter.CheckOutRegion] = string.Empty;
                    parameters[DictionaryParameter.Area] = string.Empty;
                    parameters[DictionaryParameter.CheckOutArea] = string.Empty;
                    clearOps = true;
                }
                else
                {
                    parameters[DictionaryParameter.LocationGroup] = string.Empty;
                    parameters[DictionaryParameter.CheckOutLocationGroup] = string.Empty;
                    parameters[DictionaryParameter.Pool] = string.Empty;
                    parameters[DictionaryParameter.CheckOutPool] = string.Empty;
                    clearCms = true;
                }
            }

            FillDropDowns(locationBranch, depth, parameters, clearCms, clearOps, owningLocationOnly);
            
            UpdatePanel();
            
        }

        private void SetSessionDropdownLists()
        {

            var dropdownLists = new ParamaterDropdownListHolder
            {
                CarSegment = ddlCarSegment,
                CarClass = ddlCarClass,
                CarGroup = ddlCarGroup,
                OwningCountry = ddlCheckInCountry,
                Pool = ddlCheckInPool,
                Region = ddlCheckInRegion,
                LocationGroup = ddlCheckInLocationGroup,
                Area = ddlCheckInArea,
                Location = ddlCheckInLocation,
                CheckOutCountry = ddlCheckOutCountry,
                CheckOutPool = ddlCheckOutPool,
                CheckOutRegion = ddlCheckOutRegion,
                CheckOutLocationGroup = ddlCheckOutLocationGroup,
                CheckOutArea = ddlCheckOutPool,
                CheckOutLocation = ddlCheckOutLocation                
            };
            SessionStoredDropDownLists = dropdownLists;
        }

        private void UpdateFromQuickLocation(bool outLocation)
        {
            var locationWwd = outLocation ? tbQuickCheckOutLocation.Text : tbQuickCheckInLocation.Text;
            var parameters = BuildParameterDictionary();
            using (var generator = new ParamaterListItemGenerator())
            {
                var country = generator.GetCountry(locationWwd);
                if (country == string.Empty)
                {
                    if (outLocation)
                    {
                        tbQuickCheckOutLocation.Text = string.Empty;    
                    }
                    else
                    {
                        tbQuickCheckInLocation.Text = string.Empty;    
                    }
                    
                    return;
                }

                var poolId = generator.GetLocationBranchId(ParameterType.Pool, locationWwd);
                var locationGroupId = generator.GetLocationBranchId(ParameterType.LocationGroup, locationWwd);
                var areaId = generator.GetLocationBranchId(ParameterType.Area, locationWwd);
                var regionId = generator.GetLocationBranchId(ParameterType.Region, locationWwd);
                var locationId = generator.GetLocationBranchId(ParameterType.Location, locationWwd);
                if (outLocation)
                {
                    parameters[DictionaryParameter.OwningCountry] = country;
                    parameters[DictionaryParameter.CheckOutCountry] = country;
                    parameters[DictionaryParameter.CheckOutPool] = poolId.ToString(CultureInfo.InvariantCulture);
                    parameters[DictionaryParameter.CheckOutLocationGroup] = locationGroupId.ToString(CultureInfo.InvariantCulture);
                    parameters[DictionaryParameter.CheckOutArea] = areaId.ToString(CultureInfo.InvariantCulture);
                    parameters[DictionaryParameter.CheckOutRegion] = regionId.ToString(CultureInfo.InvariantCulture);
                    parameters[DictionaryParameter.CheckOutLocation] = locationId.ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    parameters[DictionaryParameter.LocationCountry] = country;
                    parameters[DictionaryParameter.Pool] = poolId.ToString(CultureInfo.InvariantCulture);
                    parameters[DictionaryParameter.LocationGroup] = locationGroupId.ToString(CultureInfo.InvariantCulture);
                    parameters[DictionaryParameter.Area] = areaId.ToString(CultureInfo.InvariantCulture);
                    parameters[DictionaryParameter.Region] = regionId.ToString(CultureInfo.InvariantCulture);
                    parameters[DictionaryParameter.Location] = locationId.ToString(CultureInfo.InvariantCulture);
                }

            }

            FillDropDowns(!outLocation, 0, parameters, false, false, true);
            SelectAutoCompleteParameters(parameters, outLocation);
            if (outLocation)
            {
                tbQuickCheckOutLocation.Text = string.Empty;
            }
            else
            {
                tbQuickCheckInLocation.Text = string.Empty;
            }
            SetSessionDropdownLists();
        }

        private void UpdateFromQuickCarGroup()
        {
            var carGroup = tbQuickCarGroup.Text;
            var country = ddlCheckOutCountry.SelectedValue;
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
            FillDropDowns(false, 1, parameters);
            SelectAutoCompleteParameters(parameters, true);
            tbQuickCarGroup.Text = string.Empty;
            SetSessionDropdownLists();
        }

        public Dictionary<DictionaryParameter, string> BuildParameterDictionary()
        {
            var returned = new Dictionary<DictionaryParameter, string>
                           {
                               {DictionaryParameter.LocationCountry,  ddlCheckInCountry.SelectedValue}
                               , {DictionaryParameter.Pool,             ddlCheckInPool.SelectedValue}
                               , {DictionaryParameter.LocationGroup,    ddlCheckInLocationGroup.SelectedValue}
                               , {DictionaryParameter.Region,           ddlCheckInRegion.SelectedValue}
                               , {DictionaryParameter.Area,             ddlCheckInArea.SelectedValue}
                               , {DictionaryParameter.Location,         ddlCheckInLocation.SelectedValue}

                               , {DictionaryParameter.OwningCountry,    ddlCheckOutCountry.SelectedValue}
                               , {DictionaryParameter.CheckOutCountry,   ddlCheckOutCountry.SelectedValue}
                               , {DictionaryParameter.CheckOutPool,      ddlCheckOutPool.SelectedValue}
                               , {DictionaryParameter.CheckOutLocationGroup,    ddlCheckOutLocationGroup.SelectedValue}
                               , {DictionaryParameter.CheckOutRegion,    ddlCheckOutRegion.SelectedValue}
                               , {DictionaryParameter.CheckOutArea,      ddlCheckOutArea.SelectedValue}
                               , {DictionaryParameter.CheckOutLocation,  ddlCheckOutLocation.SelectedValue}

                               , {DictionaryParameter.CarSegment, ddlCarSegment.SelectedValue}
                               , {DictionaryParameter.CarClass, ddlCarClass.SelectedValue}
                               , {DictionaryParameter.CarGroup, ddlCarGroup.SelectedValue}
                               , {DictionaryParameter.UpgradedLogic, rblUpgradedLogic.SelectedValue}
                               , {DictionaryParameter.CmsSelected, (rblCmsOpsLogic.SelectedValue == "Cms").ToString()}
                           };
            return returned;
        }

        private void SelectAutoCompleteParameters(Dictionary<DictionaryParameter, string> parameters, bool checkOutLogic)
        {
            if (checkOutLogic)
            {
                SetDropdownList(ddlCheckOutCountry, parameters[DictionaryParameter.CheckOutCountry]);
                SetDropdownList(ddlCheckOutPool, parameters[DictionaryParameter.CheckOutPool]);
                SetDropdownList(ddlCheckOutLocationGroup, parameters[DictionaryParameter.CheckOutLocationGroup]);
                SetDropdownList(ddlCheckOutArea, parameters[DictionaryParameter.CheckOutArea]);
                SetDropdownList(ddlCheckOutRegion, parameters[DictionaryParameter.CheckOutRegion]);
                SetDropdownList(ddlCheckOutLocation, parameters[DictionaryParameter.CheckOutLocation]);

                SetDropdownList(ddlCarSegment, parameters[DictionaryParameter.CarSegment]);
                SetDropdownList(ddlCarClass, parameters[DictionaryParameter.CarClass]);
                SetDropdownList(ddlCarGroup, parameters[DictionaryParameter.CarGroup]);
            }
            else
            {
                SetDropdownList(ddlCheckInCountry, parameters[DictionaryParameter.LocationCountry]);
                SetDropdownList(ddlCheckInPool, parameters[DictionaryParameter.Pool]);
                SetDropdownList(ddlCheckInLocationGroup, parameters[DictionaryParameter.LocationGroup]);
                SetDropdownList(ddlCheckInArea, parameters[DictionaryParameter.Area]);
                SetDropdownList(ddlCheckInRegion, parameters[DictionaryParameter.Region]);
                SetDropdownList(ddlCheckInLocation, parameters[DictionaryParameter.Location]);
            }
        }

        private void SetDropdownList(DropDownList ddl, string value)
        {
            if (string.IsNullOrEmpty(value)) return;
            ddl.SelectedValue = value;
        }


        private void SetInvlisibleCmsOpsFields(Dictionary<DictionaryParameter, string> parameters, int depth, ParamaterListItemGenerator generator
                , bool checkOutLogic)
        {
            bool locationSelectedOnCms = CmsLogicSelected && depth == 3;
            bool locationSelectedOnOps = !CmsLogicSelected && depth == 3;
            if (locationSelectedOnCms)
            {
                string areaId = string.Empty;
                string regionId = string.Empty;
                if (!string.IsNullOrEmpty(parameters[checkOutLogic ? DictionaryParameter.CheckOutLocation : DictionaryParameter.Location]))
                {
                    var dpLocation = checkOutLogic ? DictionaryParameter.CheckOutLocation : DictionaryParameter.Location;

                    var locationWwd = generator.GetWwdFromLocationId(int.Parse(parameters[dpLocation]));
                    areaId = generator.GetLocationBranchId(ParameterType.Area, locationWwd).ToString();
                    regionId = generator.GetLocationBranchId(ParameterType.Region, locationWwd).ToString();
                }

                parameters[checkOutLogic ? DictionaryParameter.CheckOutArea : DictionaryParameter.Area] = areaId;
                parameters[checkOutLogic ? DictionaryParameter.CheckOutRegion : DictionaryParameter.Region] = regionId;
            }
            if (locationSelectedOnOps)
            {
                string poolId = string.Empty;
                string locationGroupId = string.Empty;

                if (!string.IsNullOrEmpty(parameters[checkOutLogic ? DictionaryParameter.CheckOutLocation : DictionaryParameter.Location]))
                {
                    var dpLocation = checkOutLogic ? DictionaryParameter.CheckOutLocation : DictionaryParameter.Location;
                    var locationWwd =
                        generator.GetWwdFromLocationId(int.Parse(parameters[dpLocation]));
                    poolId = generator.GetLocationBranchId(ParameterType.Pool, locationWwd).ToString();
                    locationGroupId = generator.GetLocationBranchId(ParameterType.LocationGroup, locationWwd).ToString();
                }
                parameters[checkOutLogic ? DictionaryParameter.CheckOutPool : DictionaryParameter.Pool] = poolId;
                parameters[checkOutLogic ? DictionaryParameter.CheckOutLocationGroup : DictionaryParameter.LocationGroup] = locationGroupId;
            }
        }

        /// <summary>
        /// Fills Drop down lists from database
        /// </summary>
        /// <param name="locationBranch">Clears everything below depth for this branch</param>
        /// <param name="depth">Clears everything below this depth</param>
        /// <param name="parameters"></param>
        private void FillDropDowns(bool locationBranch, int depth, Dictionary<DictionaryParameter, string> parameters
                , bool clearCms = false, bool clearOps = false, bool owningLocationOnly = false)
        {
            bool locationSelectedOnCms = CmsLogicSelected && (depth == 3);
            bool locationSelectedOnOps = !CmsLogicSelected && (depth == 3);

            using (var generator = new ParamaterListItemGenerator())
            {

                if (locationBranch)
                {
                    SetInvlisibleCmsOpsFields(parameters, depth, generator, false);
                    var locationGroups = generator.GenerateList(ParameterType.LocationGroup, parameters).ToArray();
                    var locations = generator.GenerateList(ParameterType.Location, parameters).ToArray();   
                    var areas = generator.GenerateList(ParameterType.Area, parameters).ToArray();
                    var pools = generator.GenerateList(ParameterType.Pool, parameters).ToArray();
                    var regions = generator.GenerateList(ParameterType.Region, parameters).ToArray();

                    ClearAndPopulateDdl(ddlCheckInPool, pools, depth == 0 || clearCms || locationSelectedOnOps);
                    ClearAndPopulateDdl(ddlCheckInRegion, regions, depth == 0 || clearOps || locationSelectedOnCms);
                    ClearAndPopulateDdl(ddlCheckInLocationGroup, locationGroups, depth <= 1 || clearCms || locationSelectedOnOps);
                    ClearAndPopulateDdl(ddlCheckInArea, areas, depth <= 1 || clearOps || locationSelectedOnCms);
                    ClearAndPopulateDdl(ddlCheckInLocation, locations, depth <= 2);
                    
                    if (locationSelectedOnCms)
                    {
                        SetDropdownList(ddlCheckInArea, parameters[DictionaryParameter.Area]);
                        SetDropdownList(ddlCheckInRegion, parameters[DictionaryParameter.Region]);
                    }
                    if (locationSelectedOnOps)
                    {
                        SetDropdownList(ddlCheckInPool, parameters[DictionaryParameter.Pool]);
                        SetDropdownList(ddlCheckInLocationGroup, parameters[DictionaryParameter.LocationGroup]);
                    }

                }
                else
                {
                    SetInvlisibleCmsOpsFields(parameters, depth, generator, true);
                    var carSegments = generator.GenerateList(ParameterType.CarSegment, parameters).ToArray();
                    var carClass = generator.GenerateList(ParameterType.CarClass, parameters).ToArray();
                    var carGroup = generator.GenerateList(ParameterType.CarGroup, parameters).ToArray();

                    ClearAndPopulateDdl(ddlCarSegment, carSegments, depth == 0 );
                    ClearAndPopulateDdl(ddlCarClass, carClass, (depth <= 1 && !owningLocationOnly) || depth == 0);
                    ClearAndPopulateDdl(ddlCarGroup, carGroup, (depth <= 2 && !owningLocationOnly) || depth == 0);

                    var poolsOut = generator.GenerateList(ParameterType.Pool, parameters, true, true).ToArray();
                    var regionsOut = generator.GenerateList(ParameterType.Region, parameters, true, true).ToArray();
                    var locationGroupsOut = generator.GenerateList(ParameterType.LocationGroup, parameters, true, true).ToArray();
                    var areasOut = generator.GenerateList(ParameterType.Area, parameters, true, true).ToArray();
                    var locationsOut = generator.GenerateList(ParameterType.Location, parameters, true, true).ToArray();

                    ClearAndPopulateDdl(ddlCheckOutPool, poolsOut, depth == 0 || clearCms || locationSelectedOnOps);
                    ClearAndPopulateDdl(ddlCheckOutRegion, regionsOut, depth == 0 || clearOps || locationSelectedOnCms);
                    ClearAndPopulateDdl(ddlCheckOutLocationGroup, locationGroupsOut, 
                                ((depth <= 1 || clearCms || locationSelectedOnOps) && owningLocationOnly) || depth == 0);
                    ClearAndPopulateDdl(ddlCheckOutArea, areasOut, 
                                ((depth <= 1 || clearOps || locationSelectedOnCms) && owningLocationOnly) || depth == 0);

                    ClearAndPopulateDdl(ddlCheckOutLocation, locationsOut, ( (depth <= 2 && owningLocationOnly) || depth == 0));
                   

                    if (locationSelectedOnCms)
                    {
                        SetDropdownList(ddlCheckOutArea, parameters[DictionaryParameter.CheckOutArea]);
                        SetDropdownList(ddlCheckOutRegion, parameters[DictionaryParameter.CheckOutRegion]);
                    }
                    if (locationSelectedOnOps)
                    {
                        SetDropdownList(ddlCheckOutPool, parameters[DictionaryParameter.CheckOutPool]);
                        SetDropdownList(ddlCheckOutLocationGroup, parameters[DictionaryParameter.CheckOutLocationGroup]);
                    }
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

        private void ShowCarGroupQuickSelect()
        {
            var showQuickCarGroup = !string.IsNullOrEmpty(ddlCheckOutCountry.SelectedValue);

            lblQuickCarGroup.Visible = showQuickCarGroup;
            tbQuickCarGroup.Visible = showQuickCarGroup;
        }

        private void CmsOpsLogicSelected()
        {
            var cmsSelected = rblCmsOpsLogic.SelectedValue == "Cms";

            ddlCheckInPool.Visible = cmsSelected;
            ddlCheckInLocationGroup.Visible = cmsSelected;
            lblCheckInPool.Visible = cmsSelected;
            lblCheckInLocationGroup.Visible = cmsSelected;

            ddlCheckInRegion.Visible = !cmsSelected;
            ddlCheckInArea.Visible = !cmsSelected;
            lblCheckInRegion.Visible = !cmsSelected;
            lblCheckInArea.Visible = !cmsSelected;

            ddlCheckOutPool.Visible = cmsSelected;
            ddlCheckOutLocationGroup.Visible = cmsSelected;
            lblCheckOutPool.Visible = cmsSelected;
            lblCheckOutLocationGroup.Visible = cmsSelected;

            ddlCheckOutRegion.Visible = !cmsSelected;
            ddlCheckOutArea.Visible = !cmsSelected;
            lblCheckOutRegion.Visible = !cmsSelected;
            lblCheckOutArea.Visible = !cmsSelected;

            if (!IsPostBack) return;

            //if (ddlCheckOutLocation.SelectedValue == string.Empty)
            //{
            //    var parameters = BuildParameterDictionary();
            //    FillDropDowns(false, 2, parameters, true, true);
            //}
        }

        private Tuple<bool, int, bool> GetTreeSideAndDepth(string id)
        {
            bool locationBranch;
            int depth;
            bool owningLocationOnly = false;
            switch (id)
            {
                case "ddlCheckOutCountry":
                    locationBranch = false;
                    tbQuickCarGroup.Text = string.Empty;
                    depth = 0;
                    break;
                case "ddlCheckInCountry":
                    locationBranch = true;
                    depth = 0;
                    break;
                case "ddlCarSegment":
                    locationBranch = false;
                    depth = 1;
                    break;
                case "ddlCheckOutPool":
                case "ddlCheckOutRegion":
                    locationBranch = false;
                    depth = 1;
                    owningLocationOnly = true;
                    break;
                case "ddlCarClass":
                    locationBranch = false;
                    depth = 2;
                    break;
                case "ddlCheckOutLocationGroup":
                case "ddlCheckOutArea":
                    locationBranch = false;
                    depth = 2;
                    owningLocationOnly = true;
                    break;
                case "ddlCarGroup":
                    locationBranch = false;
                    depth = 3;
                    break;
                case "ddlCheckOutLocation":
                    locationBranch = false;
                    depth = 3;
                    owningLocationOnly = true;
                    break;
                case "ddlCheckInPool":
                case "ddlCheckInRegion":
                    locationBranch = true;
                    depth = 1;
                    break;
                case "ddlCheckInLocationGroup":
                case "ddlCheckInArea":
                    locationBranch = true;
                    depth = 2;
                    break;
                case "ddlCheckInLocation":
                    locationBranch = true;
                    depth = 3;
                    break;
                default:
                    locationBranch = false;
                    depth = -1;
                    break;
            }
            return new Tuple<bool, int, bool >(locationBranch, depth, owningLocationOnly);
        }
    }
}