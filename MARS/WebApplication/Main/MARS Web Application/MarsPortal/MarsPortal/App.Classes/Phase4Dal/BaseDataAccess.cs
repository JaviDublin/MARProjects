using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Mars.App.Classes.DAL.MarsDBContext;
using Mars.App.Classes.Phase4Bll.Administration;
using Mars.App.Classes.Phase4Dal.Enumerators;
using Mars.App.Classes.Phase4Dal.ForeignVehicles;
using Mars.App.Classes.Phase4Dal.NonRev.Parameters;
using Rad.Security;

namespace Mars.App.Classes.Phase4Dal
{
    public class BaseDataAccess : IDisposable
    {
        protected MarsDBDataContext DataContext;
        protected Dictionary<DictionaryParameter, string> Parameters { get; set; }

        public const string TotalKeyName = "Total";
        public const string NotFoundKeyName = "Not Found";

        public BaseDataAccess()
        {
            DataContext = new MarsDBDataContext();
        }

        protected BaseDataAccess(Dictionary<DictionaryParameter, string> parameters, MarsDBDataContext dbc)
        {
            Parameters = parameters;
            DataContext = dbc ?? new MarsDBDataContext();
            //DataContext.Log = new DebugTextWriter();
            DataContext.CommandTimeout = 60;
        }

        public void Dispose()
        {
            DataContext.Dispose();
        }

        public DateTime GetLastFleetNowUpdate()
        {
            if (!DataContext.FleetNows.Any()) return DateTime.MinValue;
            var lastUpdate = DataContext.FleetNows.Max(d => d.Timestamp);
            return lastUpdate;

        }

        public List<ListItem> GetOwningAreasList()
        {
            var returned = NonRevParameterDataAccess.GetOwningAreas(DataContext);

            return returned;
        }


        public List<ListItem> GetOperationalStatusList()
        {
            var returned = NonRevParameterDataAccess.GetOperationalStatuses(DataContext);
            returned.ForEach(d => d.Selected = true);
            return returned;
        }

        public List<ListItem> GetFleetTypesList(ModuleType modType)
        {
            var returned = NonRevParameterDataAccess.GetFleetTypes(DataContext);
            switch (modType)
            {
                case ModuleType.Availability:
                    returned.ForEach(d => d.Selected =
                        d.Text.ToLower() != "licensee"
                        && d.Text.ToLower() != "inactive"
                        && d.Text.ToLower() != "undefined"
                        && d.Text.ToLower() != "firefly"
                        && d.Text.ToLower() != "hertz on demand"
                        && d.Text.ToLower() != "sold");
                    break;
                case ModuleType.NonRev:
                    returned.ForEach(d => d.Selected =
                        d.Text.ToLower() != "licensee"
                        && d.Text.ToLower() != "inactive"
                        && d.Text.ToLower() != "sold");
                    break;
                default:
                    throw new ArgumentOutOfRangeException("modType");
            }
            var loggedOnEmployee = ApplicationAuthentication.GetEmployeeId();
            if (loggedOnEmployee == string.Empty) //Special case for Itdemo
            {
                loggedOnEmployee = ApplicationAuthentication.GetGlobalId();
            }

            var marsUserEntity = DataContext.MarsUsers.FirstOrDefault(d => d.EmployeeId == loggedOnEmployee);

            if (marsUserEntity != null)
            {
                if (marsUserEntity.CompanyType.CompanyTypeName == CompanyTypeEnum.Licensee.ToString())
                {
                    var item = returned.SingleOrDefault(d => d.Text.ToLower() == "licensee");
                    if (item != null)
                    {
                        item.Selected = true;
                    }
                    
                }
            }

    return returned;
        }

        public List<ListItem> GetMovementTypesList()
        {
            var returned = NonRevParameterDataAccess.GetMovementTypes(DataContext);
            returned.ForEach(d => d.Selected = true);
            return returned;
        }

        public List<ListItem> GetDayOfWeeks()
        {
            var weekDays = Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>()
                            .Select(dow => new ListItem(dow.ToString(), ((int) dow).ToString(CultureInfo.InvariantCulture)))
                            .ToList();
            return weekDays.ToList();


        }

        public List<ListItem> GetRemarkReasonsList()
        {
            var returned = NonRevParameterDataAccess.GetRemarkReasons(DataContext);
            return returned;
        }

        public string GetCountryFromId(string countryCode)
        {
            var returned  = DataContext.COUNTRies.Single(d => d.country1 == countryCode).country_description;
            return returned;
        }

        public List<ListItem> GetOwningCounries()
        {
            var countries = ParameterDataAccess.GetOwningCountryListItems(DataContext);
            return countries;
        }

        public Dictionary<string, string> GetCountryDescriptionDictionary()
        {
            var cntryDictionary = from c in DataContext.COUNTRies
                select new {ID = c.country1, Value = c.country_description};

            var returned = cntryDictionary.ToDictionary(d => d.ID, d=> d.Value);
            returned.Add(VehicleOverviewDataAccess.TotalString, VehicleOverviewDataAccess.TotalString);
            return returned;
        }

        public string GetPoolFromId(int poolId)
        {
            var returned = DataContext.CMS_POOLs.Single(d => d.cms_pool_id == poolId).cms_pool1;
            return returned;
        }

        public string GetRegionFromId(int regionId)
        {
            var returned = DataContext.OPS_REGIONs.Single(d => d.ops_region_id == regionId).ops_region1;
            return returned;
        }

        public string GetAreaFromId(int areaId)
        {
            var returned = DataContext.OPS_AREAs.Single(d => d.ops_area_id == areaId).ops_area1;
            return returned;
        }

        public string GetLocationGroupFromId(int locationGroupId)
        {
            var returned = DataContext.CMS_LOCATION_GROUPs.Single(d => d.cms_location_group_id == locationGroupId).cms_location_group1;
            return returned;
        }

        public string GetLocationFromId(int locationId)
        {
            var returned = DataContext.LOCATIONs.Single(d => d.dim_Location_id == locationId).location1;
            return returned;
        }

        public string GetCarSegmentFromId(int segmentId)
        {
            var returned = DataContext.CAR_SEGMENTs.Single(d => d.car_segment_id == segmentId).car_segment1;
            return returned;
        }

        public string GetCarClassFromId(int classId)
        {
            var returned = DataContext.CAR_CLASSes.Single(d => d.car_class_id == classId).car_class1;
            return returned;
        }

        public string GetCarGroupFromId(int groupId)
        {
            var returned = DataContext.CAR_GROUPs.Single(d => d.car_group_id == groupId).car_group1;
            return returned;
        }

        public List<string> GetFleetTypenamesFromIds(List<int> fleetTypeIds)
        {
            var returned = DataContext.VehicleFleetTypes.Where(d => fleetTypeIds.Contains(d.VehicleFleetTypeId)).Select(d=> d.FleetTypeName).ToList();
            return returned;
        }

    }
}