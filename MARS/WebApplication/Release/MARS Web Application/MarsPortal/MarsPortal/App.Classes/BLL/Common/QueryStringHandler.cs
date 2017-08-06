
namespace App.BLL
{
    public class QueryStringHandler
    {
        public enum QueryString : int
        {
            Country = 1,
            CMS_Pool_Id = 2,
            CMS_Location_Group_Code = 3,
            OPS_Region_Id = 4,
            OPS_Area_Id = 5,
            Location = 6,
            Car_Segment_Id = 7,
            Car_Class_Id = 8,
            Car_Group_Id = 9,
            OptionLogic = 10,
            Hour = 11,
            Day = 12,
            Actuals = 13,
            DateValue = 14,
            FleetStatus = 15,
            FleetName = 16,
            Operstat = 17,
            DayOfWeek = 18,
            DateRange = 19
        }

        public static string _country = "c";
        public static string _cms_pool_id = "cp";
        public static string _cms_location_group_code = "clg";
        public static string _ops_region_id = "or";
        public static string _ops_area_id = "oa";
        public static string _location = "l";
        public static string _car_segment_id = "cs";
        public static string _car_class_id = "cc";
        public static string _car_group_id = "cg";
        public static string _option_logic = "ol";
        public static string _hour = "hr";
        public static string _day = "dy";
        public static string _actuals = "a";
        public static string _dateValue = "dv";
        public static string _fleetName = "fn";
        public static string _fleetStatus = "fs";
        public static string _operStat = "os";
        public static string _dayOfWeek = "dow";

        public static string _dateRange = "dr";

        public static string QueryStr(int query)
        {

            string querySt = null;
            switch (query)
            {
                case (int)QueryString.Country:
                    querySt = _country;
                    break;
                case (int)QueryString.CMS_Pool_Id:
                    querySt = _cms_pool_id;
                    break;
                case (int)QueryString.CMS_Location_Group_Code:
                    querySt = _cms_location_group_code;
                    break;
                case (int)QueryString.OPS_Region_Id:
                    querySt = _ops_region_id;
                    break;
                case (int)QueryString.OPS_Area_Id:
                    querySt = _ops_area_id;
                    break;
                case (int)QueryString.Location:
                    querySt = _location;
                    break;
                case (int)QueryString.Car_Segment_Id:
                    querySt = _car_segment_id;
                    break;
                case (int)QueryString.Car_Class_Id:
                    querySt = _car_class_id;
                    break;
                case (int)QueryString.Car_Group_Id:
                    querySt = _car_group_id;
                    break;
                case (int)QueryString.OptionLogic:
                    querySt = _option_logic;
                    break;
                case (int)QueryString.Hour:
                    querySt = _hour;
                    break;
                case (int)QueryString.Day:
                    querySt = _day;
                    break;
                case (int)QueryString.Actuals:
                    querySt = _actuals;
                    break;
                case (int)QueryString.DateValue:
                    querySt = _dateValue;
                    break;
                case (int)QueryString.FleetName:
                    querySt = _fleetName;
                    break;
                case (int)QueryString.FleetStatus:
                    querySt = _fleetStatus;
                    break;
                case (int)QueryString.Operstat:
                    querySt = _operStat;
                    break;
                case (int)QueryString.DayOfWeek:
                    querySt = _dayOfWeek;
                    break;
                case (int)QueryString.DateRange:
                    querySt = _dateRange;
                    break;
            }
            return querySt;

        }
    }
}