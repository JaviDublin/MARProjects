using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.App.Classes.Phase4Bll.Administration;
using Mars.App.Classes.Phase4Dal;
using Mars.App.Classes.Phase4Dal.Administration;
using Mars.App.Classes.Phase4Dal.Enumerators;
using Mars.App.UserControls.Phase4.Administration.Mapping;
using Mars.App.UserControls.Phase4.Administration.Mapping.EntityPopups;

namespace Mars.App.Site.Administration.Mappings
{
    public partial class Mappings : Page
    {
        public const string MappingUpdate = "MappingUpdate";

        protected void Page_Load(object sender, EventArgs e)
        {

            if(!IsPostBack)
            {
                ucCountryParameter.ParameterType = AdminMappingEnum.Country;
                ucPoolParameter.ParameterType = AdminMappingEnum.CmsPool;
                ucLocationGroupParameters.ParameterType = AdminMappingEnum.CmsLocationGroup;
                ucRegionParameters.ParameterType = AdminMappingEnum.OpsRegion;
                ucAreaParameters.ParameterType = AdminMappingEnum.OpsArea;
                ucLocationParameters.ParameterType = AdminMappingEnum.Location;
                ucCarSegmentParameters.ParameterType = AdminMappingEnum.CarSegment;
                ucCarClassParamters.ParameterType = AdminMappingEnum.CarClass;
                ucCarGroupParameters.ParameterType = AdminMappingEnum.CarGroup;
                

                ucCountryGrid.EntityType = AdminMappingEnum.Country;
                ucPoolGrid.EntityType = AdminMappingEnum.CmsPool;
                ucLocationGroupGrid.EntityType = AdminMappingEnum.CmsLocationGroup;
                ucRegionGrid.EntityType = AdminMappingEnum.OpsRegion;
                ucAreaGrid.EntityType = AdminMappingEnum.OpsArea;
                ucLocationGrid.EntityType = AdminMappingEnum.Location;
                ucCarSegmentGrid.EntityType = AdminMappingEnum.CarSegment;
                ucCarClassGrid.EntityType = AdminMappingEnum.CarClass;
                ucCarGroupGrid.EntityType = AdminMappingEnum.CarGroup;

                PopulateDefaultItems();
            }
        }

        private void PopulateDefaultItems()
        {
            using (var dataAccess = new MappingListSelect())
            {
                var countryParams = ucCountryParameter.GetParameters();
                var countries = dataAccess.GetAllCountries(countryParams);

                //var poolParams = ucPoolParameter.GetParameters();
                //var pools = dataAccess.GetPools(poolParams);

                //var locationGroupParams = ucLocationGroupParameters.GetParameters();
                //var locationGroups = dataAccess.GetLocationGroups(locationGroupParams);

                //var regionParams = ucRegionParameters.GetParameters();
                //var regions = dataAccess.GetRegions(regionParams);

                //var areaParams = ucAreaParameters.GetParameters();
                //var areas = dataAccess.GetAreas(areaParams);

                //var locationParams = ucLocationParameters.GetParameters();
                //var locations = dataAccess.GetLocations(locationParams);

                //var carSegmentParams = ucCarSegmentParameters.GetParameters();
                //var segments = dataAccess.GetCarSegments(carSegmentParams);

                //var carClassParams = ucCarClassParamters.GetParameters();
                //var classes = dataAccess.GetCarClasses(carClassParams);

                //var carGroupParams = ucCarGroupParameters.GetParameters();
                //var groups = dataAccess.GetCarGroups(carGroupParams);

                ucCountryGrid.BindGrid(countries);
                //ucPoolGrid.BindGrid(pools);
                //ucLocationGroupGrid.BindGrid(locationGroups);
                //ucRegionGrid.BindGrid(regions);
                //ucAreaGrid.BindGrid(areas);
                //ucLocationGrid.BindGrid(locations);
                //ucCarSegmentGrid.BindGrid(segments);
                //ucCarClassGrid.BindGrid(classes);
                //ucCarGroupGrid.BindGrid(groups);
            }
        }


        protected override bool OnBubbleEvent(object sender, EventArgs args)
        {
            var handled = false;
            if (args is GridViewCommandEventArgs)
            {
                var commandArgs = args as CommandEventArgs;
                if (commandArgs.CommandName == string.Empty) return false;
                var clicked = (AdminMappingEnum) Enum.Parse(typeof (AdminMappingEnum), commandArgs.CommandName);
                int id = int.Parse(commandArgs.CommandArgument.ToString());
                ShowPopup(clicked, id);
                return true;
            }
            if(args is CommandEventArgs)
            {
                var commandArgs = args as CommandEventArgs;
                if(commandArgs.CommandName == EntityParameter.ParamChangedString)
                {
                    var parameterType = (AdminMappingEnum)Enum.Parse(typeof(AdminMappingEnum), commandArgs.CommandArgument.ToString());
                    RefreshData(parameterType);
                    return true;
                }
                if(commandArgs.CommandName == EntityParameter.NewEntityString)
                {
                    var parameterType = (AdminMappingEnum)Enum.Parse(typeof(AdminMappingEnum), commandArgs.CommandArgument.ToString());
                    ShowPopup(parameterType, 0);
                    return true;
                }

                if(commandArgs.CommandName == MappingUpdate)
                {
                    var arguments = (List<string>) commandArgs.CommandArgument;
                    var mappingType = (AdminMappingEnum) Enum.Parse(typeof (AdminMappingEnum), arguments[0]);
                    var message = arguments[1];
                    var parameterEntity = GetParameterControl(mappingType);
                    parameterEntity.SetMessage(message);
                    RefreshData(mappingType);
                    return true;
                }

                if (commandArgs.CommandName == EntityParameter.QuickSelectChangedString)
                {
                    var parameterType = (AdminMappingEnum)Enum.Parse(typeof(AdminMappingEnum), commandArgs.CommandArgument.ToString());
                    RefreshData(parameterType);
                    return true;
                }

            }
            return handled;
        }

        private EntityParameter GetParameterControl(AdminMappingEnum entityType)
        {
            switch (entityType)
            {
                case AdminMappingEnum.Country:
                    return ucCountryParameter;
                case AdminMappingEnum.CmsPool:
                    return ucPoolParameter;
                case AdminMappingEnum.CmsLocationGroup:
                    return ucLocationGroupParameters;
                case AdminMappingEnum.OpsRegion:
                    return ucRegionParameters;
                case AdminMappingEnum.OpsArea:
                    return ucAreaParameters;
                case AdminMappingEnum.Location:
                    return ucLocationParameters;
                case AdminMappingEnum.CarSegment:
                    return ucCarSegmentParameters;
                case AdminMappingEnum.CarClass:
                    return ucCarClassParamters;
                case AdminMappingEnum.CarGroup:
                    return ucCarGroupParameters;
                default:
                    throw new ArgumentOutOfRangeException("entityType");
            }
        }

        private void ShowPopup(AdminMappingEnum entityType, int id)
        {
            PopupEntityUserControl uc;
            switch (entityType)
            {
                case AdminMappingEnum.Country:
                    uc = ucCountryPopup;
                    break;
                case AdminMappingEnum.CmsPool:
                    uc = ucPoolPopup;
                    break;
                case AdminMappingEnum.CmsLocationGroup:
                    uc = ucLocationGroupPopup;
                    break;
                case AdminMappingEnum.OpsRegion:
                    uc = ucRegionPopup;
                    break;
                case AdminMappingEnum.OpsArea:
                    uc = ucAreaPopup;
                    break;
                case AdminMappingEnum.Location:
                    uc = ucLocationPopup;
                    break;
                case AdminMappingEnum.CarSegment:
                    uc = ucCarSegmentPopup;
                    break;
                case AdminMappingEnum.CarClass:
                    uc = ucCarClassPopup;
                    break;
                case AdminMappingEnum.CarGroup:
                    uc = ucCarGroupPopup;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("entityType");
            }

            uc.SetValues(id);
            uc.ShowPopup();
        }

        private void RefreshData(AdminMappingEnum dataType)
        {
            using (var dataAccess = new MappingListSelect())
            {
                Dictionary<DictionaryParameter, string> dictionaryParameters;
                switch (dataType)
                {
                    case AdminMappingEnum.Country:
                        dictionaryParameters = ucCountryParameter.GetParameters();
                        var countries = dataAccess.GetAllCountries(dictionaryParameters);
                        ucCountryGrid.BindGrid(countries);
                        break;
                    case AdminMappingEnum.CmsPool:
                        dictionaryParameters = ucPoolParameter.GetParameters();
                        var pools = dataAccess.GetPools(dictionaryParameters);
                        ucPoolGrid.BindGrid(pools);
                        break;
                    case AdminMappingEnum.CmsLocationGroup:
                        dictionaryParameters = ucLocationGroupParameters.GetParameters();
                        var locationGroups = dataAccess.GetLocationGroups(dictionaryParameters);
                        ucLocationGroupGrid.BindGrid(locationGroups);
                        break;
                    case AdminMappingEnum.OpsRegion:
                        dictionaryParameters = ucRegionParameters.GetParameters();
                        var regions = dataAccess.GetRegions(dictionaryParameters);
                        ucRegionGrid.BindGrid(regions);
                        break;
                    case AdminMappingEnum.OpsArea:
                        dictionaryParameters = ucAreaParameters.GetParameters();
                        var areas = dataAccess.GetAreas(dictionaryParameters);
                        ucAreaGrid.BindGrid(areas);
                        break;
                    case AdminMappingEnum.Location:
                        dictionaryParameters = ucLocationParameters.GetParameters();
                        var locations = dataAccess.GetLocations(dictionaryParameters);
                        ucLocationGrid.BindGrid(locations);
                        break;
                    case AdminMappingEnum.CarSegment:
                        dictionaryParameters = ucCarSegmentParameters.GetParameters();
                        var carSegments = dataAccess.GetCarSegments(dictionaryParameters);
                        ucCarSegmentGrid.BindGrid(carSegments);
                        break;
                    case AdminMappingEnum.CarClass:
                        dictionaryParameters = ucCarClassParamters.GetParameters();
                        var carClasses = dataAccess.GetCarClasses(dictionaryParameters);
                        ucCarClassGrid.BindGrid(carClasses);
                        break;
                    case AdminMappingEnum.CarGroup:
                        dictionaryParameters = ucCarGroupParameters.GetParameters();
                        var carGroups = dataAccess.GetCarGroups(dictionaryParameters);
                        ucCarGroupGrid.BindGrid(carGroups);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("dataType");
                }
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {

        }

        
    }
}