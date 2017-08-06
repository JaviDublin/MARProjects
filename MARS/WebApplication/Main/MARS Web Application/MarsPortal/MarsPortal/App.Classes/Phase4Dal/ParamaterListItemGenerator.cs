using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Mars.App.Classes.DAL.MarsDBContext;

using Mars.App.Classes.Phase4Dal.Enumerators;

namespace Mars.App.Classes.Phase4Dal
{
    public class ParamaterListItemGenerator : IDisposable
    {
        private readonly MarsDBDataContext _dataContext;

        public ParamaterListItemGenerator()
        {
            _dataContext = new MarsDBDataContext();
            
        }

        public List<ListItem> GenerateEmptyList()
        {
            var returned = new List<ListItem>();
            returned.Insert(0, ParameterDataAccess.EmptyItem);
            return returned;
        }

        public List<ListItem> GenerateAvailabilityTopicList()
        {
            
            var returned = new List<ListItem>
                           {
                               new ListItem("Total Fleet", AvailabilityTopic.TotalFleet.ToString()),
                               new ListItem("Credit Union - CU", AvailabilityTopic.Cu.ToString()),
                               new ListItem("Hold Admin - HA", AvailabilityTopic.Ha.ToString()),
                               new ListItem("Hold Legal - HL", AvailabilityTopic.Hl.ToString()),
                               new ListItem("Lease Loaner - LL", AvailabilityTopic.Ll.ToString()),
                               new ListItem("No Car Tow - NC", AvailabilityTopic.Nc.ToString()),
                               new ListItem("Retail Pipeline - PL", AvailabilityTopic.Pl.ToString()),
                               new ListItem("Theft Conversion - TC", AvailabilityTopic.Tc.ToString()),
                               new ListItem("Salvage - SV", AvailabilityTopic.Sv.ToString()),
                               new ListItem("Wholesale - WS", AvailabilityTopic.Ws.ToString()),
                               new ListItem("Operational Fleet", AvailabilityTopic.OperationalFleet.ToString()),
                               new ListItem("Body Damage - BD", AvailabilityTopic.Bd.ToString()),
                               new ListItem("Maintenance - MM", AvailabilityTopic.Mm.ToString()),
                               new ListItem("Open Tow - TW", AvailabilityTopic.Tw.ToString()),
                               new ListItem("Turnback - TB", AvailabilityTopic.Tb.ToString()),
                               new ListItem("Fleet Sale - FS", AvailabilityTopic.Fs.ToString()),
                               new ListItem("Reail Lot - RL", AvailabilityTopic.Rl.ToString()),
                               new ListItem("Reail Pending - RP", AvailabilityTopic.Rp.ToString()),
                               new ListItem("Transfer not on Sight - TN", AvailabilityTopic.Tn.ToString()),
                               new ListItem("Available Fleet", AvailabilityTopic.AvailableFleet.ToString()),
                               new ListItem("Idle", AvailabilityTopic.Idle.ToString()),
                               new ListItem("Service Unit - SU", AvailabilityTopic.Su.ToString()),                               
                               new ListItem("Overdue", AvailabilityTopic.Overdue.ToString()),
                               new ListItem("On Rent", AvailabilityTopic.OnRent.ToString()),
                               new ListItem("Utilization", AvailabilityTopic.Utilization.ToString()),
                               new ListItem("Utilization Overdue", AvailabilityTopic.UtilizationInclOverdue.ToString())
                           };

            return returned;
        }

        public string GetCountry(string wwd)
        {
            var country = _dataContext.LOCATIONs.FirstOrDefault(d => d.location1 == wwd);
            if (country == null)
            {
                return string.Empty;
            }
            return country.country;
        }

        public bool DoesCarGroupExistForCountry(string country, string carGroup)
        {
            var carGroupFromDb = from cg in _dataContext.CAR_GROUPs
                where cg.car_group1 == carGroup && cg.CAR_CLASS.CAR_SEGMENT.country == country
                select cg;

            return carGroupFromDb.Any();
        }

        public string GetWwdFromLocationId(int locationId)
        {
            var returned = _dataContext.LOCATIONs.First(d => d.dim_Location_id == locationId).location1;
            return returned;
        }

        public int GetLocationBranchId(ParameterType typeRequested, string wwd)
        {
            var locationWwd = _dataContext.LOCATIONs.First(d => d.location1 == wwd);
            switch (typeRequested)
            {
                case ParameterType.Pool:
                    return locationWwd.CMS_LOCATION_GROUP.cms_pool_id;
                case ParameterType.LocationGroup:
                    return locationWwd.CMS_LOCATION_GROUP.cms_location_group_id;
                case ParameterType.Area:
                    return locationWwd.ops_area_id;
                case ParameterType.Region:
                    return locationWwd.OPS_AREA.OPS_REGION.ops_region_id;
                case ParameterType.Location:
                    return locationWwd.dim_Location_id;
                default:
                    throw new ArgumentOutOfRangeException("typeRequested");
            }
        }


        public int GetCarBranchId(ParameterType typeRequested, string carGroupName, string country)
        {
            var carGroup = _dataContext.CAR_GROUPs.First(d => d.car_group1 == carGroupName 
                                    && d.CAR_CLASS.CAR_SEGMENT.country == country);
            switch (typeRequested)
            {
                case ParameterType.CarSegment:
                    return carGroup.CAR_CLASS.car_segment_id;
                case ParameterType.CarClass:
                    return carGroup.car_class_id;
                case ParameterType.CarGroup:
                    return carGroup.car_group_id;
                default:
                    throw new ArgumentOutOfRangeException("typeRequested");
            }
        }

        public List<ListItem> GenerateList(ParameterType typeRequested, Dictionary<DictionaryParameter, string> parameters
                    , bool addAllItem = true, bool checkOutLogic = false)
        {
            List<ListItem> returned;
            switch (typeRequested)
            {
                case ParameterType.OwningCountry:
                    returned = ParameterDataAccess.GetOwningCountryListItems(_dataContext);
                    break;
                case ParameterType.CountryCheckOut:
                    returned = ParameterDataAccess.GetReservationOutCountryListItems(_dataContext);
                    break;
                case ParameterType.LocationCountry:
                    returned = ParameterDataAccess.GetLocationCountryListItems(_dataContext);
                    break;
                case ParameterType.Pool:
                    returned = ParameterDataAccess.GetPoolListItems(parameters, _dataContext, checkOutLogic);
                    break;
                case ParameterType.LocationGroup:
                    returned = ParameterDataAccess.GetLocationGroupListItems(parameters, _dataContext, checkOutLogic);
                    break;
                case ParameterType.Area:
                    returned = ParameterDataAccess.GetAreaListItems(parameters, _dataContext, checkOutLogic);
                    break;
                case ParameterType.Region:
                    returned = ParameterDataAccess.GetRegionListItems(parameters, _dataContext, checkOutLogic);
                    break;
                case ParameterType.Location:
                    returned = ParameterDataAccess.GetLocationListItems(parameters, _dataContext, checkOutLogic);
                    break;
                case ParameterType.CarSegment:
                    returned = ParameterDataAccess.GetCarSegmentListItems(parameters, _dataContext);
                    break;
                case ParameterType.CarClass:
                    returned = ParameterDataAccess.GetCarClassListItems(parameters, _dataContext);
                    break;
                case ParameterType.CarGroup:
                    returned = ParameterDataAccess.GetCarCarGroupListItems(parameters, _dataContext);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("typeRequested");
            }
            returned.RemoveAll(d => d.Value == string.Empty);
            if (addAllItem)
            {
                
                returned.Insert(0, new ListItem("***All***", string.Empty));        
            }
            
            return returned;
        }

        public void Dispose()
        {
            _dataContext.Dispose();
        }
    }
}