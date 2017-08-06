using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.App.Classes.Phase4Bll.Parameters;
using Mars.App.Classes.Phase4Dal;

using Mars.App.Classes.Phase4Dal.Enumerators;

namespace Mars.App.UserControls.Phase4
{
    public partial class MultiSelectVehicleParameters : UserControl
    {
        private const string ParameterSessionName = "MultiParameterSessionName";

        private ParamaterDropdownListHolder SessionStoredDropDownLists
        {
            get { return (ParamaterDropdownListHolder) Session[ParameterSessionName]; }
            set { Session[ParameterSessionName] = value; }
        }


        public bool ShowLocationLogic
        {
            set { rblLocationLogic.Visible = value; }
        }

        public bool ShowUpgradedLogic
        {
            set { rblUpgradedLogic.Visible = value; }
        }

        private bool CmsLogicSelected {get { return (rblCmsOpsLogic.SelectedValue == "Cms"); }}

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CmsOpsLogicSelected();
                GenerateNewDropdowns();
              
                ToggleQuickCarGroup();
            }
            string parameter = Request["__EVENTARGUMENT"];
            if (parameter != null)
            {
                if (parameter == "LocationMultiple CarGroupMultiple")
                {
                    UpdateFromQuickLocation();
                    UpdateFromQuickCarGroup();
                    UpdatePanel();
                    return;
                }                
            }
            
            if (!string.IsNullOrEmpty(parameter))
            {
                if (CheckCallbackId(parameter, lbLocationCountry)) return;
                if (CheckCallbackId(parameter, lbOwningCountry)) return;
                if (CheckCallbackId(parameter, lbRegion)) return;
                if (CheckCallbackId(parameter, lbPool)) return;
                if (CheckCallbackId(parameter, lbLocationGroup)) return;
                if (CheckCallbackId(parameter, lbArea)) return;
                if (CheckCallbackId(parameter, lbCarSegment)) return;
                if (CheckCallbackId(parameter, lbCarClass)) return;
            }
        }

        private bool CheckCallbackId(string clientIdCalled, ListBox lb)
        {
            if (!clientIdCalled.Contains(lb.ID)) return false;            
            ParameterChanged(lb.ID);
            return true;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            string selectedOwningCountries = GetJoinedSelectedListBoxItems(lbOwningCountry);
            acCarGroup.ContextKey = selectedOwningCountries;
        }



        //private void RestoreOldDropdowns()
        //{
        //    var oldDropdowns = SessionStoredDropDownLists;
        //    UpdateDropdownList(lbOwningCountry, oldDropdowns.OwningCountry);
        //    UpdateDropdownList(lbCarSegment, oldDropdowns.CarSegment);
        //    UpdateDropdownList(lbCarClass, oldDropdowns.CarClass);
        //    UpdateDropdownList(lbCarGroup, oldDropdowns.CarGroup);
        //    UpdateDropdownList(lbLocationCountry, oldDropdowns.LocationCountry);
        //    UpdateDropdownList(lbPool, oldDropdowns.Pool);
        //    UpdateDropdownList(lbLocationGroup, oldDropdowns.LocationGroup);
        //    UpdateDropdownList(lbLocation, oldDropdowns.Location);
        //}

        private void UpdateDropdownList(ListControl toUpdate, DropDownList source)
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
                var owningCountries = generator.GenerateList(ParameterType.OwningCountry, null, false).ToArray();
                var locationCountries = generator.GenerateList(ParameterType.LocationCountry, null, false).ToArray();
                
                lbOwningCountry.Items.AddRange(owningCountries);
                lbLocationCountry.Items.AddRange(locationCountries);
            }
        }

        private void UpdatePanel()
        {
            upnlParameters.Update();
        }
        
        protected void rblCmsOpsLogic_SelectionChanged(object sender, EventArgs e)
        {
            CmsOpsLogicSelected();
        }

        protected void ParameterChanged(object sender, EventArgs e)
        {
            var senderDropdown = sender as DropDownList;
            if (senderDropdown == null) return;
            var id = senderDropdown.ID;
            ParameterChanged(id);
        }

        protected void ParameterChanged(string id)
        {
            var sideAndDepth = GetTreeSideAndDepth(id);
            if (sideAndDepth == null) return;
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
                OwningCountryMultiple = lbOwningCountry,
                CarSegmentMultiple = lbCarSegment,
                CarClassMultiple = lbCarClass,
                CarGroupMultiple = lbCarGroup,
                LocationCountryMultiple = lbLocationCountry,
                PoolMultiple = lbPool,
                LocationGroupMultiple = lbLocationGroup,
                AreaMultiple = lbArea,
                RegionMultiple = lbRegion,
                LocationMultiple = lbLocation
            };
            SessionStoredDropDownLists = dropdownLists;
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

        private void UpdateFromQuickLocation()
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


                parameters[DictionaryParameter.LocationCountry] = AddAutoCompleteToCountry(parameters[DictionaryParameter.LocationCountry], country);
                parameters[DictionaryParameter.Pool] = AddAutoCompleteToGenericId(parameters[DictionaryParameter.Pool], poolId);
                parameters[DictionaryParameter.LocationGroup] = AddAutoCompleteToGenericId(parameters[DictionaryParameter.LocationGroup], locationGroupId);
                parameters[DictionaryParameter.Area] = AddAutoCompleteToGenericId(parameters[DictionaryParameter.Area], areaId);
                parameters[DictionaryParameter.Region] = AddAutoCompleteToGenericId(parameters[DictionaryParameter.Region], regionId);
                parameters[DictionaryParameter.Location] = AddAutoCompleteToGenericId(parameters[DictionaryParameter.Location], locationId);
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
            var country = GetJoinedSelectedListBoxItems(lbOwningCountry);
            var parameters = BuildParameterDictionary();

            if (carGroup.Contains("-"))
            {
                country = carGroup.Substring(0, 2);
                carGroup = carGroup.Substring(3, carGroup.Length - 3);
                
            }

            using (var generator = new ParamaterListItemGenerator())
            {
                //if (!generator.DoesCarGroupExistForCountry(country, carGroup))
                //{
                //    tbQuickCarGroup.Text = string.Empty;
                //    return;
                //}

                var carSegmentId = generator.GetCarBranchId(ParameterType.CarSegment, carGroup, country);
                var carClassId = generator.GetCarBranchId(ParameterType.CarClass, carGroup, country);
                var carGroupId = generator.GetCarBranchId(ParameterType.CarGroup, carGroup, country);

                parameters[DictionaryParameter.OwningCountry] = AddAutoCompleteToCountry(parameters[DictionaryParameter.OwningCountry], country);
                parameters[DictionaryParameter.CarSegment] = AddAutoCompleteToGenericId(parameters[DictionaryParameter.CarSegment], carSegmentId);
                parameters[DictionaryParameter.CarClass] = AddAutoCompleteToGenericId(parameters[DictionaryParameter.CarClass], carClassId);
                parameters[DictionaryParameter.CarGroup] = AddAutoCompleteToGenericId(parameters[DictionaryParameter.CarGroup], carGroupId);
            }
            FillDropDowns(false, 0, parameters);
            SelectAutoCompleteParameters(parameters);
            tbQuickCarGroup.Text = string.Empty;
            SetSessionDropdownLists();
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

        public Dictionary<DictionaryParameter, string> BuildParameterDictionary()
        {
            string selectedLocationCountries = GetJoinedSelectedListBoxItems(lbLocationCountry);
            string selectedOwningCountries = GetJoinedSelectedListBoxItems(lbOwningCountry); 
            string selectedCarSegments = GetJoinedSelectedListBoxItems(lbCarSegment);
            string selectedPools = GetJoinedSelectedListBoxItems(lbPool);
            string selectedRegions = GetJoinedSelectedListBoxItems(lbRegion);
            string selectedLocationGroups = GetJoinedSelectedListBoxItems(lbLocationGroup);
            string selectedAreas = GetJoinedSelectedListBoxItems(lbArea);
            string selectedLocations = GetJoinedSelectedListBoxItems(lbLocation);
            string selectedCarClasses = GetJoinedSelectedListBoxItems(lbCarClass);
            string selectedCarGroups = GetJoinedSelectedListBoxItems(lbCarGroup);
            
            var returned = new Dictionary<DictionaryParameter, string>
                           {
                                 {DictionaryParameter.OwningCountry, selectedOwningCountries}
                               , {DictionaryParameter.LocationCountry, selectedLocationCountries}
                               , {DictionaryParameter.Pool, selectedPools}
                               , {DictionaryParameter.LocationGroup, selectedLocationGroups}
                               , {DictionaryParameter.Region, selectedRegions}
                               , {DictionaryParameter.Area, selectedAreas}
                               , {DictionaryParameter.Location, selectedLocations}
                               , {DictionaryParameter.CarSegment, selectedCarSegments}
                               , {DictionaryParameter.CarClass, selectedCarClasses}
                               , {DictionaryParameter.ExpectedLocationLogic, rblLocationLogic.SelectedValue}
                               , {DictionaryParameter.UpgradedLogic, rblUpgradedLogic.SelectedValue}
                               , {DictionaryParameter.CarGroup, selectedCarGroups}
                               , {DictionaryParameter.CmsSelected, (rblCmsOpsLogic.SelectedValue == "Cms").ToString()}
                           };

            return returned;
        }

        private void SelectAutoCompleteParameters(Dictionary<DictionaryParameter, string> parameters)
        {
            SetDropdownListMultiple(lbLocationCountry, parameters[DictionaryParameter.LocationCountry]);
            SetDropdownListMultiple(lbPool, parameters[DictionaryParameter.Pool]);
            SetDropdownListMultiple(lbLocationGroup, parameters[DictionaryParameter.LocationGroup]);
            SetDropdownListMultiple(lbArea, parameters[DictionaryParameter.Area]);
            SetDropdownListMultiple(lbRegion, parameters[DictionaryParameter.Region]);
            SetDropdownListMultiple(lbLocation, parameters[DictionaryParameter.Location]);
            SetDropdownListMultiple(lbCarSegment, parameters[DictionaryParameter.CarSegment]);
            SetDropdownListMultiple(lbCarClass, parameters[DictionaryParameter.CarClass]);
            SetDropdownListMultiple(lbCarGroup, parameters[DictionaryParameter.CarGroup]);
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
                    

                    var pools = generator.GenerateList(ParameterType.Pool, parameters, false).ToArray();
                    var locationGroups = generator.GenerateList(ParameterType.LocationGroup, parameters, false).ToArray();
                    var locations = generator.GenerateList(ParameterType.Location, parameters, false).ToArray();
                    var regions = generator.GenerateList(ParameterType.Region, parameters, false).ToArray();
                    var areas = generator.GenerateList(ParameterType.Area, parameters, false).ToArray();



                    ClearAndPopulateLb(lbPool, pools, depth <= 0 || clearCms || locationSelectedOnOps);
                    ClearAndPopulateLb(lbRegion, regions, depth <= 0 || clearOps || locationSelectedOnCms);
                    ClearAndPopulateLb(lbLocationGroup, locationGroups, depth <= 1 || clearCms || locationSelectedOnOps);
                    ClearAndPopulateLb(lbArea, areas, depth <= 1 || clearOps || locationSelectedOnCms);
                    ClearAndPopulateLb(lbLocation, locations, depth <= 2);
                    if (locationSelectedOnCms)
                    {
                        SetDropdownListMultiple(lbRegion, parameters[DictionaryParameter.Region]);
                        SetDropdownListMultiple(lbArea, parameters[DictionaryParameter.Area]);
                        
                    }
                    if (locationSelectedOnOps)
                    {
                        SetDropdownListMultiple(lbRegion, parameters[DictionaryParameter.Pool]);
                        SetDropdownListMultiple(lbLocationGroup, parameters[DictionaryParameter.LocationGroup]);
                    }

                }
                else
                {
                    var carSegments = generator.GenerateList(ParameterType.CarSegment, parameters, false).ToArray();
                    var carClass = generator.GenerateList(ParameterType.CarClass, parameters, false).ToArray();
                    var carGroup = generator.GenerateList(ParameterType.CarGroup, parameters, false).ToArray();

                    ClearAndPopulateLb(lbCarSegment, carSegments, depth <= 0);
                    ClearAndPopulateLb(lbCarClass, carClass, depth <= 1);
                    ClearAndPopulateLb(lbCarGroup, carGroup, depth <= 2);
                }
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

        private void ToggleQuickCarGroup()
        {
            var showQuickCarGroup = !string.IsNullOrEmpty(lbOwningCountry.SelectedValue);

            lblQuickCarGroup.Visible = showQuickCarGroup;
            tbQuickCarGroup.Visible = showQuickCarGroup;
        }

        private void CmsOpsLogicSelected()
        {
            var cmsSelected = rblCmsOpsLogic.SelectedValue == "Cms";

            lblPool.Visible = cmsSelected;
            lbLocationGroup.Visible = cmsSelected;
            lbPool.Visible = cmsSelected;
            lblLocationGroup.Visible = cmsSelected;
            
            lbRegion.Visible = !cmsSelected;
            lbArea.Visible = !cmsSelected;
            lblRegion.Visible = !cmsSelected;
            lblArea.Visible = !cmsSelected;

            if (!IsPostBack) return;

            if (lbLocation.SelectedValue == string.Empty)
            {
                var parameters = BuildParameterDictionary();
                FillDropDowns(true, 2, parameters, true, true);
            }
        }

        private bool HasParameterChanged(ListBox lbOld, ListBox lbNew)
        {
            
            var oldItems = GetJoinedSelectedListBoxItems(lbOld);
            var newItems = GetJoinedSelectedListBoxItems(lbNew);
            return oldItems != newItems;
        }

        /// <summary>
        /// Finds the position of the control that updated
        /// </summary>
        /// <param name="id"></param>
        /// <returns>null if nothing changed</returns>
        private Tuple<bool, int> GetTreeSideAndDepth(string id)
        {
            var parameters = SessionStoredDropDownLists;
            bool locationBranch;
            int depth;
            switch (id)
            {
                case "lbOwningCountry":
                    if (parameters != null && !HasParameterChanged(parameters.OwningCountryMultiple, lbOwningCountry))
                    {
                        return null;
                    }
                    
                    locationBranch = false;
                    tbQuickCarGroup.Text = string.Empty;
                    depth = 0;
                    
                    break;
                case "lbLocationCountry":
                    if (parameters != null && !HasParameterChanged(parameters.LocationCountryMultiple, lbLocationCountry))
                    {
                        return null;
                    }
                    locationBranch = true;
                    depth = 0;
                    break;
                case "lbCarSegment":
                    if (parameters != null && !HasParameterChanged(parameters.CarClassMultiple, lbCarSegment))
                    {
                        return null;
                    }
                    locationBranch = false;
                    depth = 1;
                    break;
                case "lbPool":
                    if (parameters != null && !HasParameterChanged(parameters.PoolMultiple, lbPool))
                    {
                        return null;
                    }
                    locationBranch = true;
                    depth = 1;
                    break;
                case "lbRegion":
                    if (parameters != null && !HasParameterChanged(parameters.RegionMultiple, lbRegion))
                    {
                        return null;
                    }
                    locationBranch = true;
                    depth = 1;
                    break;
                case "lbCarClass":
                    if (parameters != null && !HasParameterChanged(parameters.CarClassMultiple, lbCarClass))
                    {
                        return null;
                    }
                    locationBranch = false;
                    depth = 2;
                    break;
                case "lbLocationGroup":
                    if (parameters != null && !HasParameterChanged(parameters.LocationGroupMultiple, lbLocationGroup))
                    {
                        return null;
                    }
                    locationBranch = true;
                    depth = 2;
                    break;
                case "lbArea":
                    if (parameters != null && !HasParameterChanged(parameters.AreaMultiple, lbArea))
                    {
                        return null;
                    }
                    locationBranch = true;
                    depth = 2;
                    break;
                case "lbCarGroup":
                    if (parameters != null && !HasParameterChanged(parameters.CarGroupMultiple, lbCarGroup))
                    {
                        return null;
                    }
                    locationBranch = false;
                    depth = 3;
                    break;
                case "lbLocation":
                    if (parameters != null && !HasParameterChanged(parameters.LocationMultiple, lbLocation))
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
            return new Tuple<bool, int>(locationBranch, depth);
        }
    }
}