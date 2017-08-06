
namespace App.BLL
{
    public class ReportPreferences
    {

        private int _logic;
        private string _country;
        private int _cms_pool_id;
        private int _cms_location_group_id;
        private int _ops_region_id;
        private int _ops_area_id;
        private string _location;
        private int _car_segment_id;
        private int _car_class_id;
        private int _car_group_id;
        private int _userPreference;
        private int? _actuals;
        private string _dateValue;
        private string _fleetName;
        private string _operstat;
        private int? _fleetStatus;
        private int? _dayOfWeek;

        private int? _dateRange;
        public int Logic
        {
            get { return _logic; }
        }

        public string Country
        {
            get { return _country; }
        }

        public int CMS_Pool_Id
        {
            get { return _cms_pool_id; }
        }

        public int CMS_Location_Group_Id
        {
            get { return _cms_location_group_id; }
        }

        public int OPS_Region_Id
        {
            get { return _ops_region_id; }
        }

        public int OPS_Area_Id
        {
            get { return _ops_area_id; }
        }

        public string Location
        {
            get { return _location; }
        }

        public int Car_Segment_Id
        {
            get { return _car_segment_id; }
        }

        public int Car_Class_Id
        {
            get { return _car_class_id; }
        }

        public int Car_Group_Id
        {
            get { return _car_group_id; }
        }

        public int UserPreference
        {
            get { return _userPreference; }
        }

        public int? Actuals
        {
            get { return _actuals; }
        }

        public string DateValue
        {
            get { return _dateValue; }
        }

        public string FleetName
        {
            get { return _fleetName; }
        }

        public string Operstat
        {
            get { return _operstat; }
        }

        public int? FleetStatus
        {
            get { return _fleetStatus; }
        }

        public int? DayOfWeek
        {
            get { return _dayOfWeek; }
        }

        public int? DateRange
        {
            get { return _dateRange; }
        }

        public ReportPreferences(int logic, string country, int cms_pool_id, int cms_location_group_id, int ops_region_id,
                                  int ops_area_id, int car_segment_id, int car_class_id, int car_group_id, int userPreference, string location,
                                     int? actuals, string dateValue, string fleetname, string operstat, int? fleetStatus, int? dayOfWeek, int? dateRange)
        {
            _logic = logic;
            _country = country;
            _cms_pool_id = cms_pool_id;
            _cms_location_group_id = cms_location_group_id;
            _ops_region_id = ops_region_id;
            _ops_area_id = ops_area_id;
            _location = location;
            _car_segment_id = car_segment_id;
            _car_class_id = car_class_id;
            _car_group_id = car_group_id;
            _userPreference = userPreference;
            _actuals = actuals;
            _dateValue = dateValue;
            _fleetName = fleetname;
            _operstat = operstat;
            _fleetStatus = fleetStatus;
            _dayOfWeek = DayOfWeek;
            _dateRange = dateRange;
        }
    }
}