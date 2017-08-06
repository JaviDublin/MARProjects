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

namespace Mars.App.UserControls.Phase4.Reservations
{
    public partial class ReservationMultiVehicleParameters : UserControl
    {
        private const string ParameterSessionName = "ReservationsMultiParameterSessionName";

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
                GenerateNewDropdowns();
            }
            string parameter = Request["__EVENTARGUMENT"];
            if (parameter == "LocationInMultiple")
            {
                UpdateFromQuickLocation(false);
                UpdatePanel();
                return;
            }
            if (parameter == "LocationOutMultiple")
            {
                UpdateFromQuickLocation(true);
                UpdatePanel();
                return;
            }
            if (parameter == "CarGroupSingleMultiple")
            {
                UpdateFromQuickCarGroup();
                UpdatePanel();
                return;
            }

            if (!string.IsNullOrEmpty(parameter))
            {
                if (CheckCallbackId(parameter, lbCheckInCountry)) return;
                if (CheckCallbackId(parameter, lbCheckInRegion)) return;
                if (CheckCallbackId(parameter, lbCheckInPool)) return;
                if (CheckCallbackId(parameter, lbCheckInLocationGroup)) return;
                if (CheckCallbackId(parameter, lbCheckInArea)) return;

                if (CheckCallbackId(parameter, lbCheckOutCountry)) return;
                if (CheckCallbackId(parameter, lbCheckOutRegion)) return;
                if (CheckCallbackId(parameter, lbCheckOutPool)) return;
                if (CheckCallbackId(parameter, lbCheckOutLocationGroup)) return;
                if (CheckCallbackId(parameter, lbCheckOutArea)) return;

                if (CheckCallbackId(parameter, lbCarSegment)) return;
                if (CheckCallbackId(parameter, lbCarClass)) return;
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            ShowCarGroupQuickSelect();
        }

        private bool CheckCallbackId(string clientIdCalled, ListBox lb)
        {
            if (!clientIdCalled.Contains(lb.ID)) return false;
            ParameterChanged(lb.ID);
            return true;
        }

        private void SetSessionDropdownLists()
        {
            var dropdownLists = new ParamaterDropdownListHolder
            {
                CarSegmentMultiple = lbCarSegment,
                CarClassMultiple = lbCarClass,
                CarGroupMultiple = lbCarGroup,

                LocationCountryMultiple = lbCheckInCountry,
                PoolMultiple = lbCheckInPool,
                LocationGroupMultiple = lbCheckInLocationGroup,
                AreaMultiple = lbCheckInArea,
                RegionMultiple = lbCheckInRegion,
                LocationMultiple = lbCheckInLocation,

                OwningCountryMultiple = lbCheckOutCountry,
                PoolOutMultiple = lbCheckOutPool,
                LocationGroupOutMultiple = lbCheckOutLocationGroup,
                AreaOutMultiple = lbCheckOutArea,
                RegionOutMultiple = lbCheckOutRegion,
                LocationOutMultiple = lbCheckOutLocation
            };
            SessionStoredDropDownLists = dropdownLists;
        }


        public void rblPickupReturnLogic_Changed(object sender, EventArgs e)
        {
            var cmdEvent = new CommandEventArgs("PickupLogicChanged", null);

            RaiseBubbleEvent(this, cmdEvent);
        }

        private void GenerateNewDropdowns()
        {
            using (var generator = new ParamaterListItemGenerator())
            {
                var owningCountries = generator.GenerateList(ParameterType.OwningCountry, null, false).ToArray();
                var locationCountries = generator.GenerateList(ParameterType.LocationCountry, null, false).ToArray();

                lbCheckOutCountry.Items.AddRange(owningCountries);
                lbCheckInCountry.Items.AddRange(locationCountries);
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

        protected void ParameterChanged(string id)
        {
            var sideAndDepth = GetTreeSideAndDepth(id);
            if (sideAndDepth == null) return;
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
            
            SetSessionDropdownLists();

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
                    parameters[DictionaryParameter.OwningCountry] = AddAutoCompleteToCountry(parameters[DictionaryParameter.OwningCountry], country);
                    parameters[DictionaryParameter.CheckOutCountry] = AddAutoCompleteToCountry(parameters[DictionaryParameter.CheckOutCountry], country);
                    parameters[DictionaryParameter.CheckOutPool] = AddAutoCompleteToGenericId(parameters[DictionaryParameter.CheckOutPool], poolId);
                    parameters[DictionaryParameter.CheckOutLocationGroup] = AddAutoCompleteToGenericId(parameters[DictionaryParameter.CheckOutLocationGroup], locationGroupId);
                    parameters[DictionaryParameter.CheckOutArea] = AddAutoCompleteToGenericId(parameters[DictionaryParameter.CheckOutArea], areaId);
                    parameters[DictionaryParameter.CheckOutRegion] = AddAutoCompleteToGenericId(parameters[DictionaryParameter.CheckOutRegion], regionId);
                    parameters[DictionaryParameter.CheckOutLocation] = AddAutoCompleteToGenericId(parameters[DictionaryParameter.CheckOutLocation], locationId);

                }
                else
                {
                    parameters[DictionaryParameter.LocationCountry] = AddAutoCompleteToCountry(parameters[DictionaryParameter.LocationCountry], country);
                    parameters[DictionaryParameter.Pool] = AddAutoCompleteToGenericId(parameters[DictionaryParameter.Pool], poolId);
                    parameters[DictionaryParameter.LocationGroup] = AddAutoCompleteToGenericId(parameters[DictionaryParameter.LocationGroup], locationGroupId);
                    parameters[DictionaryParameter.Area] = AddAutoCompleteToGenericId(parameters[DictionaryParameter.Area], areaId);
                    parameters[DictionaryParameter.Region] = AddAutoCompleteToGenericId(parameters[DictionaryParameter.Region], regionId);
                    parameters[DictionaryParameter.Location] = AddAutoCompleteToGenericId(parameters[DictionaryParameter.Location], locationId);
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
            var country = GetJoinedSelectedListBoxItems(lbCheckOutCountry);
            var parameters = BuildParameterDictionary();

            if (carGroup.Contains("-"))
            {
                country = carGroup.Substring(0, 2);
                carGroup = carGroup.Substring(3, carGroup.Length - 3);

            }

            using (var generator = new ParamaterListItemGenerator())
            {
                var carSegmentId = generator.GetCarBranchId(ParameterType.CarSegment, carGroup, country);
                var carClassId = generator.GetCarBranchId(ParameterType.CarClass, carGroup, country);
                var carGroupId = generator.GetCarBranchId(ParameterType.CarGroup, carGroup, country);

                parameters[DictionaryParameter.OwningCountry] = AddAutoCompleteToCountry(parameters[DictionaryParameter.OwningCountry], country);
                parameters[DictionaryParameter.CarSegment] = AddAutoCompleteToGenericId(parameters[DictionaryParameter.CarSegment], carSegmentId);
                parameters[DictionaryParameter.CarClass] = AddAutoCompleteToGenericId(parameters[DictionaryParameter.CarClass], carClassId);
                parameters[DictionaryParameter.CarGroup] = AddAutoCompleteToGenericId(parameters[DictionaryParameter.CarGroup], carGroupId);
            }
            FillDropDowns(false, 0, parameters);
            SelectAutoCompleteParameters(parameters, true);
            tbQuickCarGroup.Text = string.Empty;
            SetSessionDropdownLists();
        }

        private string AddAutoCompleteToGenericId(string currentSelected, int newId)
        {
            var seperator = Properties.Settings.Default.Seperator;
            if (currentSelected == string.Empty) return newId.ToString();
            if (currentSelected.Contains(seperator))
            {
                var splitValues = currentSelected.Split(seperator.ToCharArray()).Select(int.Parse).ToList();
                if (splitValues.Contains(newId)) return currentSelected;
            }
            var returned = currentSelected + seperator + newId;
            return returned;
        }

        private string AddAutoCompleteToCountry(string currentSelected, string newCountry)
        {
            var seperator = Properties.Settings.Default.Seperator;
            if (currentSelected == string.Empty) return newCountry;
            if (currentSelected.Contains(seperator))
            {
                var splitCountries = currentSelected.Split(seperator.ToCharArray());
                if (splitCountries.Contains(newCountry)) return currentSelected;
            }
            var returned = currentSelected + seperator + newCountry;
            return returned;
        }

        public Dictionary<DictionaryParameter, string> BuildParameterDictionary()
        {


            var returned = new Dictionary<DictionaryParameter, string>
                           {
                               {DictionaryParameter.LocationCountry,  GetJoinedSelectedListBoxItems(lbCheckInCountry)}
                               , {DictionaryParameter.Pool,           GetJoinedSelectedListBoxItems(lbCheckInPool)}
                               , {DictionaryParameter.LocationGroup,    GetJoinedSelectedListBoxItems(lbCheckInLocationGroup)}
                               , {DictionaryParameter.Region,           GetJoinedSelectedListBoxItems(lbCheckInRegion)}
                               , {DictionaryParameter.Area,             GetJoinedSelectedListBoxItems(lbCheckInArea)}
                               , {DictionaryParameter.Location,         GetJoinedSelectedListBoxItems(lbCheckInLocation)}

                               , {DictionaryParameter.OwningCountry,    GetJoinedSelectedListBoxItems(lbCheckOutCountry)}
                               , {DictionaryParameter.CheckOutCountry,   GetJoinedSelectedListBoxItems(lbCheckOutCountry)}
                               , {DictionaryParameter.CheckOutPool,      GetJoinedSelectedListBoxItems(lbCheckOutPool)}
                               , {DictionaryParameter.CheckOutLocationGroup,    GetJoinedSelectedListBoxItems(lbCheckOutLocationGroup)}
                               , {DictionaryParameter.CheckOutRegion,    GetJoinedSelectedListBoxItems(lbCheckOutRegion)}
                               , {DictionaryParameter.CheckOutArea,      GetJoinedSelectedListBoxItems(lbCheckOutArea)}
                               , {DictionaryParameter.CheckOutLocation,  GetJoinedSelectedListBoxItems(lbCheckOutLocation)}

                               , {DictionaryParameter.CarSegment, GetJoinedSelectedListBoxItems(lbCarSegment)}
                               , {DictionaryParameter.CarClass, GetJoinedSelectedListBoxItems(lbCarClass)}
                               , {DictionaryParameter.CarGroup, GetJoinedSelectedListBoxItems(lbCarGroup)}
                               , {DictionaryParameter.UpgradedLogic, rblUpgradedLogic.SelectedValue}
                               , {DictionaryParameter.CmsSelected, (rblCmsOpsLogic.SelectedValue == "Cms").ToString()}
                           };
            return returned;
        }

        private string GetJoinedSelectedListBoxItems(ListBox lb)
        {
            string returned = (from ListItem v in lb.Items where v.Selected select v).Aggregate(string.Empty, (current, v) => current + (v.Value + ","));
            if (returned.Length > 0)
            {
                returned = returned.Remove(returned.Length - 1);
            }

            return returned;
        }

        private void SelectAutoCompleteParameters(Dictionary<DictionaryParameter, string> parameters, bool checkOutLogic)
        {
            if (checkOutLogic)
            {
                SetDropdownListMultiple(lbCheckOutCountry, parameters[DictionaryParameter.CheckOutCountry]);
                SetDropdownListMultiple(lbCheckOutPool, parameters[DictionaryParameter.CheckOutPool]);
                SetDropdownListMultiple(lbCheckOutLocationGroup, parameters[DictionaryParameter.CheckOutLocationGroup]);
                SetDropdownListMultiple(lbCheckOutArea, parameters[DictionaryParameter.CheckOutArea]);
                SetDropdownListMultiple(lbCheckOutRegion, parameters[DictionaryParameter.CheckOutRegion]);
                SetDropdownListMultiple(lbCheckOutLocation, parameters[DictionaryParameter.CheckOutLocation]);

                SetDropdownListMultiple(lbCarSegment, parameters[DictionaryParameter.CarSegment]);
                SetDropdownListMultiple(lbCarClass, parameters[DictionaryParameter.CarClass]);
                SetDropdownListMultiple(lbCarGroup, parameters[DictionaryParameter.CarGroup]);
            }
            else
            {
                SetDropdownListMultiple(lbCheckInCountry, parameters[DictionaryParameter.LocationCountry]);
                SetDropdownListMultiple(lbCheckInPool, parameters[DictionaryParameter.Pool]);
                SetDropdownListMultiple(lbCheckInLocationGroup, parameters[DictionaryParameter.LocationGroup]);
                SetDropdownListMultiple(lbCheckInArea, parameters[DictionaryParameter.Area]);
                SetDropdownListMultiple(lbCheckInRegion, parameters[DictionaryParameter.Region]);
                SetDropdownListMultiple(lbCheckInLocation, parameters[DictionaryParameter.Location]);
            }
        }

        private void SetDropdownListMultiple(ListBox lb, string value)
        {
            if (string.IsNullOrEmpty(value)) return;
            var seperator = Properties.Settings.Default.Seperator;
            var splitItems = value.Split(seperator.ToCharArray());
            foreach (var li in lb.Items.Cast<ListItem>().Where(li => splitItems.Contains(li.Value)))
            {
                li.Selected = true;
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

                    var locationGroups = generator.GenerateList(ParameterType.LocationGroup, parameters, false).ToArray();
                    var locations = generator.GenerateList(ParameterType.Location, parameters, false).ToArray();
                    var areas = generator.GenerateList(ParameterType.Area, parameters, false).ToArray();
                    var pools = generator.GenerateList(ParameterType.Pool, parameters, false).ToArray();
                    var regions = generator.GenerateList(ParameterType.Region, parameters, false).ToArray();

                    ClearAndPopulateLb(lbCheckInPool, pools, depth == 0 || clearCms || locationSelectedOnOps);
                    ClearAndPopulateLb(lbCheckInRegion, regions, depth == 0 || clearOps || locationSelectedOnCms);
                    ClearAndPopulateLb(lbCheckInLocationGroup, locationGroups, depth <= 1 || clearCms || locationSelectedOnOps);
                    ClearAndPopulateLb(lbCheckInArea, areas, depth <= 1 || clearOps || locationSelectedOnCms);
                    ClearAndPopulateLb(lbCheckInLocation, locations, depth <= 2);

                }
                else
                {
                    SetInvlisibleCmsOpsFields(parameters, depth, generator, true);

                    var carSegments = generator.GenerateList(ParameterType.CarSegment, parameters, false).ToArray();
                    var carClass = generator.GenerateList(ParameterType.CarClass, parameters, false).ToArray();
                    var carGroup = generator.GenerateList(ParameterType.CarGroup, parameters, false).ToArray();

                    ClearAndPopulateLb(lbCarSegment, carSegments, depth == 0);
                    ClearAndPopulateLb(lbCarClass, carClass, (depth <= 1 && !owningLocationOnly) || depth == 0);
                    ClearAndPopulateLb(lbCarGroup, carGroup, (depth <= 2 && !owningLocationOnly) || depth == 0);

                    var poolsOut = generator.GenerateList(ParameterType.Pool, parameters, false, true).ToArray();
                    var regionsOut = generator.GenerateList(ParameterType.Region, parameters, false, true).ToArray();
                    var locationGroupsOut = generator.GenerateList(ParameterType.LocationGroup, parameters, false, true).ToArray();
                    var areasOut = generator.GenerateList(ParameterType.Area, parameters, false, true).ToArray();
                    var locationsOut = generator.GenerateList(ParameterType.Location, parameters, false, true).ToArray();

                    ClearAndPopulateLb(lbCheckOutPool, poolsOut, depth == 0 || clearCms || locationSelectedOnOps);
                    ClearAndPopulateLb(lbCheckOutRegion, regionsOut, depth == 0 || clearOps || locationSelectedOnCms);
                    ClearAndPopulateLb(lbCheckOutLocationGroup, locationGroupsOut, 
                                ((depth <= 1 || clearCms || locationSelectedOnOps) && owningLocationOnly) || depth == 0);
                    ClearAndPopulateLb(lbCheckOutArea, areasOut, 
                                ((depth <= 1 || clearOps || locationSelectedOnCms) && owningLocationOnly) || depth == 0);

                    ClearAndPopulateLb(lbCheckOutLocation, locationsOut, ((depth <= 2 && owningLocationOnly) || depth == 0));
                }
            }
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
        }

        private void ClearAndPopulateLb(ListBox lbToClear, ListItem[] itemsToAdd, bool refreshList)
        {
            if (refreshList)
            {
                lbToClear.Items.Clear();
                lbToClear.Items.AddRange(itemsToAdd);
                lbToClear.SelectedIndex = -1;
            }
        }

        private void ShowCarGroupQuickSelect()
        {
            var showQuickCarGroup = !string.IsNullOrEmpty(lbCheckOutCountry.SelectedValue);

            lblQuickCarGroup.Visible = showQuickCarGroup;
            tbQuickCarGroup.Visible = showQuickCarGroup;
        }

        private void CmsOpsLogicSelected()
        {
            var cmsSelected = rblCmsOpsLogic.SelectedValue == "Cms";

            lblCheckOutPool.Visible = cmsSelected;
            lblCheckInPool.Visible = cmsSelected;

            lbCheckOutLocationGroup.Visible = cmsSelected;
            lbCheckInLocationGroup.Visible = cmsSelected;

            lbCheckOutPool.Visible = cmsSelected;
            lbCheckInPool.Visible = cmsSelected;
            lblCheckOutLocationGroup.Visible = cmsSelected;
            lblCheckInLocationGroup.Visible = cmsSelected;

            lbCheckOutRegion.Visible = !cmsSelected;
            lbCheckInRegion.Visible = !cmsSelected;
            lbCheckOutArea.Visible = !cmsSelected;
            lbCheckInArea.Visible = !cmsSelected;
            lblCheckInRegion.Visible = !cmsSelected;
            lblCheckOutRegion.Visible = !cmsSelected;
            lblCheckOutArea.Visible = !cmsSelected;
            lblCheckInArea.Visible = !cmsSelected;

            if (!IsPostBack) return;

            if (lbCheckOutLocation.SelectedValue == string.Empty)
            {
                var parameters = BuildParameterDictionary();
                FillDropDowns(true, 2, parameters, true, true);
            }
            if (lbCheckInLocation.SelectedValue == string.Empty)
            {
                var parameters = BuildParameterDictionary();
                FillDropDowns(false, 2, parameters, true, true);
            }
        }

        private bool HasParameterChanged(ListBox lbOld, ListBox lbNew)
        {
            var oldItems = GetJoinedSelectedListBoxItems(lbOld);
            var newItems = GetJoinedSelectedListBoxItems(lbNew);
            return oldItems != newItems;
        }

        private Tuple<bool, int, bool> GetTreeSideAndDepth(string id)
        {
            var parameters = SessionStoredDropDownLists;
            bool locationBranch;
            int depth;
            bool owningLocationOnly = false;
            switch (id)
            {
                case "lbCheckOutCountry":

                    if (parameters != null && !HasParameterChanged(parameters.OwningCountryMultiple, lbCheckOutCountry))
                    {
                        return null;
                    }

                    locationBranch = false;
                    tbQuickCarGroup.Text = string.Empty;
                    depth = 0;
                    break;

                case "lbCarSegment":
                    if (parameters != null && !HasParameterChanged(parameters.CarSegmentMultiple, lbCarSegment))
                    {
                        return null;
                    }

                    locationBranch = false;
                    depth = 1;
                    break;
                case "lbCheckOutPool":
                    if (parameters != null && !HasParameterChanged(parameters.PoolMultiple, lbCheckOutPool))
                    {
                        return null;
                    }

                    locationBranch = false;
                    depth = 1;
                    owningLocationOnly = true;
                    break;
                case "lbCheckOutRegion":
                    if (parameters != null && !HasParameterChanged(parameters.RegionMultiple, lbCheckOutRegion))
                    {
                        return null;
                    }

                    locationBranch = false;
                    depth = 1;
                    owningLocationOnly = true;
                    break;
                case "lbCarClass":
                    if (parameters != null && !HasParameterChanged(parameters.CarClassMultiple, lbCarClass))
                    {
                        return null;
                    }

                    locationBranch = false;
                    depth = 2;
                    break;
                case "lbCheckOutLocationGroup":
                    if (parameters != null && !HasParameterChanged(parameters.LocationGroupMultiple, lbCheckOutLocationGroup))
                    {
                        return null;
                    }
                    locationBranch = false;
                    depth = 2;
                    owningLocationOnly = true;
                    break;
                case "lbCheckOutArea":
                    if (parameters != null && !HasParameterChanged(parameters.AreaMultiple, lbCheckOutArea))
                    {
                        return null;
                    }

                    locationBranch = false;
                    depth = 2;
                    owningLocationOnly = true;
                    break;
                case "lbCarGroup":
                    if (parameters != null && !HasParameterChanged(parameters.CarGroupMultiple, lbCarGroup))
                    {
                        return null;
                    }
                    locationBranch = false;
                    depth = 3;
                    break;
                case "lbCheckOutLocation":
                    if (parameters != null && !HasParameterChanged(parameters.LocationMultiple, lbCheckOutLocation))
                    {
                        return null;
                    }
                    locationBranch = false;
                    depth = 3;
                    owningLocationOnly = true;
                    break;

                case "lbCheckInCountry":
                    if (parameters != null && !HasParameterChanged(parameters.LocationCountryMultiple, lbCheckInCountry))
                    {
                        return null;
                    }
                    locationBranch = true;
                    depth = 0;
                    break;
                case "lbCheckInPool":
                    if (parameters != null && !HasParameterChanged(parameters.PoolMultiple, lbCheckInPool))
                    {
                        return null;
                    }

                    locationBranch = true;
                    depth = 1;
                    break;

                case "lbCheckInRegion":
                    if (parameters != null && !HasParameterChanged(parameters.RegionMultiple, lbCheckInRegion))
                    {
                        return null;
                    }

                    locationBranch = true;
                    depth = 1;
                    break;
                case "lbCheckInLocationGroup":
                    if (parameters != null && !HasParameterChanged(parameters.LocationGroupMultiple, lbCheckInLocationGroup))
                    {
                        return null;
                    }
                    locationBranch = true;
                    depth = 2;
                    
                    break;
                case "lbCheckInArea":
                    if (parameters != null && !HasParameterChanged(parameters.AreaMultiple, lbCheckInArea))
                    {
                        return null;
                    }

                    
                    locationBranch = true;
                    depth = 2;
                    break;
                case "lbCheckInLocation":
                    if (parameters != null && !HasParameterChanged(parameters.LocationMultiple, lbCheckInLocation))
                    {
                        return null;
                    }

                    
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