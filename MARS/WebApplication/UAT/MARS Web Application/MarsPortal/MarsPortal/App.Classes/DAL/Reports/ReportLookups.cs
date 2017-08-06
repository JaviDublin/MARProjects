using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using App.BLL;
using App.DAL.Data;

namespace App.BLL
{
    public class ReportLookups
    {
        // Property added by Gavin for MarsV3 to be able to search on CAL (Corporation, Agency, Licensee)
        // If CAL is equal to "*" then it has to been reflected in the stored procedure to mean all
        // The CAL property needs to be set before the stored procedures are called
        // The methods are GetCMSPools, GetCMSLocationGroups, GetLocations, GetOPSRegions and GetOPSAreas
        public static string CAL { get; set; }

        #region "Functions"

        public static List<Countries> GetCountries()
        {
            //Initialise command
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.CountriesSelect, con);
           
            //Execute Command
            List<Countries> results = new List<Countries>();
            using (con)
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    results.Add(new Countries(reader));
                }
            }
            con.Close();
            return results;
        }

        public static List<Countries> GetCountriesAll()
        {
            //Initialise command
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.Portal_CountriesSelectAll, con);

            //Execute Command
            List<Countries> results = new List<Countries>();
            using (con)
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    results.Add(new Countries(reader));
                }
            }
            con.Close();
            return results;

        }

        public static List<CMSPools> GetCMSPools(string country)
        {
            //Initialise command
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.CMSPoolsSelect, con);
            
            //Set Parameters
            if (country == "-1")
            {
                cmd.Parameters.AddWithValue("@country", System.DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@country", country);
            }

            //Extra param added to filter on CAL
            cmd.Parameters.AddWithValue("@CAL", CAL ?? "*");

            //Execute Command
            List<CMSPools> results = new List<CMSPools>();
            using (con)
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    results.Add(new CMSPools(reader));
                }
            }
            con.Close();
            return results;

        }

        public static List<CMSPools> GetCMSPoolsWithCountry()
        {
            //Initialise command
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.Portal_CMSPoolSelectWithCountry, con);
            
            //Execute Command
            List<CMSPools> results = new List<CMSPools>();
            using (con)
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    results.Add(new CMSPools(reader));
                }
            }
            con.Close();
            return results;
        }

        public static List<CMSLocationGroups> GetCMSLocationGroups(string country, int cms_pool_id)
        {
            //Initialise command
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.CMSLocationGroupsSelect, con);
            
            //Set Parameters
            if (country == "-1")
            {
                cmd.Parameters.AddWithValue("@country", System.DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@country", country);
            }

            if (cms_pool_id == -1)
            {
                cmd.Parameters.AddWithValue("@cms_pool_id", System.DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@cms_pool_id", cms_pool_id);
            }

            // Parameter to query for the CAL of the Location Group
            cmd.Parameters.AddWithValue("@CAL", CAL ?? "*");

            //Execute Command
            List<CMSLocationGroups> results = new List<CMSLocationGroups>();
            using (con)
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    results.Add(new CMSLocationGroups(reader));
                }
            }
            con.Close();
            return results;


        }

        public static List<CMSLocationGroups> GetCMSLocationGroupsWithCMSPools()
        {
            //Initialise command
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.Portal_CMSLocationGroupSelectWithCMSPool, con);
            
            //Execute Command
            List<CMSLocationGroups> results = new List<CMSLocationGroups>();
            using (con)
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    results.Add(new CMSLocationGroups(reader));
                }
            }
            con.Close();
            return results;
        }

        public static List<Locations> GetLocations(string country, int ops_region_id, int ops_area_id, int cms_pool_id, int cms_location_group_id, int logic)
        {
            //Initialise command
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.LocationsSelect, con);
            
            //Set Parameters

            if (country == "-1")
            {
                cmd.Parameters.AddWithValue("@country", System.DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@country", country);
            }

            // Parameter to query for the CAL of the Location
            cmd.Parameters.AddWithValue("@CAL", CAL ?? "*");

            switch (logic)
            {
                case (int)ReportSettings.OptionLogic.CMS:
                    if (cms_pool_id == -1)
                    {
                        cmd.Parameters.AddWithValue("@cms_pool_id", System.DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@cms_pool_id", cms_pool_id);
                    }

                    if (cms_location_group_id == -1)
                    {
                        cmd.Parameters.AddWithValue("@cms_location_group_id", System.DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@cms_location_group_id", cms_location_group_id);
                    }
                    break;
                case (int)ReportSettings.OptionLogic.OPS:
                    if (ops_region_id == -1)
                    {
                        cmd.Parameters.AddWithValue("@ops_region_id", System.DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@ops_region_id", ops_region_id);
                    }

                    if (ops_area_id == -1)
                    {
                        cmd.Parameters.AddWithValue("@ops_area_id", System.DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@ops_area_id", ops_area_id);
                    }
                    break;
            }

            //Execute Command
            List<Locations> results = new List<Locations>();
            using (con)
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    results.Add(new Locations(reader));
                }
            }
            con.Close();
            return results;


        }

        public static List<OPSRegions> GetOPSRegions(string country)
        {
            //Initialise command
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.OPSRegionsSelect, con);
            
            //Set paramaters
            if (country == "-1")
            {
                cmd.Parameters.AddWithValue("@country", System.DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@country", country);
            }

            //Extra param added to filter on CAL
            cmd.Parameters.AddWithValue("@CAL", CAL ?? "*");

            //Execute Command
            List<OPSRegions> results = new List<OPSRegions>();
            using (con)
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    results.Add(new OPSRegions(reader));
                }
            }

            con.Close();
            return results;

        }

        public static List<OPSRegions> GetOPSRegionsWithCountry()
        {
            //Initialise command
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.Portal_OPSRegionSelectWithCountry, con);

            //Execute Command
            List<OPSRegions> results = new List<OPSRegions>();
            using (con)
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    results.Add(new OPSRegions(reader));
                }
            }

            con.Close();
            return results;

        }

        public static List<OPSAreas> GetOPSAreas(string country, int ops_region_id)
        {
            //Initialise command
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.OPSAreasSelect, con);

            //Set paramaters
            if (country == "-1")
            {
                cmd.Parameters.AddWithValue("@country", System.DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@country", country);
            }

            if (ops_region_id == -1)
            {
                cmd.Parameters.AddWithValue("@ops_region_id", System.DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@ops_region_id", ops_region_id);
            }

            //Extra param added to filter on CAL
            cmd.Parameters.AddWithValue("@CAL", CAL ?? "*");

            //Execute Command
            List<OPSAreas> results = new List<OPSAreas>();
            using (con)
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    results.Add(new OPSAreas(reader));
                }
            }

            con.Close();
            return results;

        }

        public static List<OPSAreas> GetOPSAreasWithOPSRegions()
        {
            //Initialise command
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.Portal_OPSAreaWithOPSRegion, con);
            
            //Execute Command
            List<OPSAreas> results = new List<OPSAreas>();
            using (con)
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    results.Add(new OPSAreas(reader));
                }
            }

            con.Close();
            return results;

        }

        public static List<Fleet> GetFleet()
        {
            //Initialise command
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.FleetLookupSelect, con);
            
            //Execute Command
            List<Fleet> results = new List<Fleet>();
            using (con)
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    results.Add(new Fleet(reader));
                }
            }

            con.Close();
            return results;
        }

        public static List<CarSegments> GetCarSegments(string country)
        {
            //Initialise command
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.CarSegmentsSelect, con);
            
            //Set paramaters
            if (country == "-1")
            {
                cmd.Parameters.AddWithValue("@country", System.DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@country", country);
            }

            //Execute Command
            List<CarSegments> results = new List<CarSegments>();
            using (con)
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    results.Add(new CarSegments(reader));
                }
            }

            con.Close();
            return results;
        }

        public static List<CarSegments> GetCarSegmentsWithCountries()
        {
            //Initialise command
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.Portal_CarSegmentSelectWithCountry, con);
            
            //Execute Command
            List<CarSegments> results = new List<CarSegments>();
            using (con)
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    results.Add(new CarSegments(reader));
                }
            }
            con.Close();
            return results;
        }

        public static List<CarClasses> GetCarClasses(string country, int car_segment_id)
        {
            //Initialise command
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.CarClassesSelect, con);
            
            //Set paramaters
            if (country == "-1")
            {
                cmd.Parameters.AddWithValue("@country", System.DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@country", country);
            }

            if (car_segment_id == -1)
            {
                cmd.Parameters.AddWithValue("@car_segment_id", System.DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@car_segment_id", car_segment_id);
            }

            //Execute Command
            List<CarClasses> results = new List<CarClasses>();
            using (con)
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    results.Add(new CarClasses(reader));
                }
            }

            con.Close();
            return results;

        }

        public static List<CarClasses> GetCarClassesWithCarSegments()
        {
            //Initialise command
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.Portal_CarClassSelectWithCarSegment, con);
           
            //Execute Command
            List<CarClasses> results = new List<CarClasses>();
            using (con)
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    results.Add(new CarClasses(reader));
                }
            }

            con.Close();
            return results;
        }

        public static List<CarGroups> GetCarGroups(string country, int car_segment_id, int car_class_id)
        {
            //Initialise command
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.CarGroupsSelect, con);

            
            //Set paramaters
            if (country == "-1")
            {
                cmd.Parameters.AddWithValue("@country", System.DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@country", country);
            }

            if (car_segment_id == -1)
            {
                cmd.Parameters.AddWithValue("@car_segment_id", System.DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@car_segment_id", car_segment_id);
            }

            if (car_class_id == -1)
            {
                cmd.Parameters.AddWithValue("@car_class_id", System.DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@car_class_id", car_class_id);
            }

            //Execute Command
            List<CarGroups> results = new List<CarGroups>();
            using (con)
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    results.Add(new CarGroups(reader));
                }
            }

            con.Close();
            return results;

        }

        public static List<OperStats> GetOperstats()
        {
            //Initialise command
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.OperstatsSelect, con);

            //Execute Command
            List<OperStats> results = new List<OperStats>();
            using (con)
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    results.Add(new OperStats(reader));
                }
            }

            con.Close();
            return results;
        }

        public static List<OperStats> GetOperstatsSettings()
        {
            //Initialise command
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.OperstatsSelect_Settings, con);

            //Execute Command
            List<OperStats> results = new List<OperStats>();
            using (con)
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    results.Add(new OperStats(reader));
                }
            }

            con.Close();
            return results;
        }

        public static List<ModelCodes> GetModelcodes(string country)
        {
            //Initialise command
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.ModelcodeSelect, con);
            
            //Set paramaters
            if (country == "-1")
            {
                cmd.Parameters.AddWithValue("@country", System.DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@country", country);
            }

            //Execute Command
            List<ModelCodes> results = new List<ModelCodes>();
            using (con)
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    results.Add(new ModelCodes(reader));
                }
            }

            con.Close();
            return results;

        }

        public static List<ModelCodes> GetModelcodesNonRev(string country)
        {
            //Initialise command
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.NonRev_Select_ModelCodes, con);

            //Set paramaters
            if (country == "-1")
            {
                cmd.Parameters.AddWithValue("@country", System.DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@country", country);
            }

            //Execute Command
            List<ModelCodes> results = new List<ModelCodes>();
            using (con)
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    results.Add(new ModelCodes(reader));
                }
            }

            con.Close();
            return results;

        }

        public static List<OwnAreas> GetOwnareas()
        {
            //Initialise command
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.OwnareaSelect, con);
            
            //Execute Command
            List<OwnAreas> results = new List<OwnAreas>();
            using (con)
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    results.Add(new OwnAreas(reader));
                }
            }

            con.Close();
            return results;

        }

        public static List<MovTypes> GetMovTypes()
        {

            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.MovTypeSelect, con);

            List<MovTypes> results = new List<MovTypes>();
            
            using (con)
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    results.Add(new MovTypes(reader));
                }
            }

            con.Close();
            return results;
        }

        public static List<RemarkList> GetRemarksList()
        {
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.RemarkListSelect, con);

            List<RemarkList> results = new List<RemarkList>();

            using (con)
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    results.Add(new RemarkList(reader));
                }
            }

            con.Close();
            return results;
        }

        public static List<DayGroupCodes> GetDayGroupCodes()
        {
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.DayGroupCodeSelect, con);

            List<DayGroupCodes> results = new List<DayGroupCodes>();

            using (con)
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    results.Add(new DayGroupCodes(reader));
                }
            }

            con.Close();
            return results;
        }

        public static List<DayGroupCodes> GetDayGroupCodesFiltered()
        {
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.DayGroupCodeSelect, con);

            cmd.Parameters.AddWithValue("@filtered", 1);

            List<DayGroupCodes> results = new List<DayGroupCodes>();

            using (con)
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    results.Add(new DayGroupCodes(reader));
                }
            }

            con.Close();
            return results;
        }

        #endregion

        #region Classes

        public class Countries
        {
            #region Properties and Fields
            private string _country;
            public string Country
            {
                get { return _country; }
            }
            #endregion

            #region Constructors
            public Countries(SqlDataReader reader)
            {
                if (reader["country"] != DBNull.Value)
                {
                    _country = Convert.ToString(reader["country"]);
                }
            }
            #endregion
        }

        public class CMSPools
        {
            #region Properties and Fields
            private int _cms_Pool_Id;

            private string _cms_Pool;
            public int Cms_Pool_Id
            {
                get { return _cms_Pool_Id; }
            }

            public string Cms_Pool
            {
                get { return _cms_Pool; }
            }
            #endregion

            #region Constructors
            public CMSPools(SqlDataReader reader)
            {
                if (reader["cms_pool_id"] != DBNull.Value)
                {
                    _cms_Pool_Id = Convert.ToInt32(reader["cms_pool_id"]);
                }
                if (reader["cms_pool"] != DBNull.Value)
                {
                    _cms_Pool = Convert.ToString(reader["cms_pool"]);
                }
            }
            #endregion
        }

        public class CMSLocationGroups
        {
            #region Properties and Fields
            private int _cms_Location_Group_Id;

            private string _cms_Location_Group;
            public int Cms_Location_Group_Id
            {
                get { return _cms_Location_Group_Id; }
            }

            public string Cms_Location_Group
            {
                get { return _cms_Location_Group; }
            }
            #endregion

            #region Constructors
            public CMSLocationGroups(SqlDataReader reader)
            {
                if (reader["cms_location_group_id"] != DBNull.Value)
                {
                    _cms_Location_Group_Id = int.Parse(reader["cms_location_group_id"].ToString());
                }
                if (reader["cms_location_group"] != DBNull.Value)
                {
                    _cms_Location_Group = Convert.ToString(reader["cms_location_group"]);
                }
            }
            #endregion
        }

        public class Locations
        {
            #region Properties and Fields
            private string _location;
            private string _real_Location_Name;
            private string _location_Name;

            private string _location_Name_Dw;
            public string Location
            {
                get { return _location; }
            }

            public string Real_Location_Name
            {
                get { return _real_Location_Name; }
            }

            public string Location_Name
            {
                get { return _location_Name; }
            }

            public string Location_Name_Dw
            {
                get { return _location_Name_Dw; }
            }
            
            // Added by Gavin
            public string cal { get; private set; }
            #endregion

            #region Constructors
            public Locations(SqlDataReader reader) {
                if (reader["location"] != DBNull.Value) {
                    _location = Convert.ToString(reader["location"]);
                }
                if (reader["real_location_name"] != DBNull.Value) {
                    _real_Location_Name = Convert.ToString(reader["real_location_name"]);
                }
                if (reader["location_name"] != DBNull.Value) {
                    _location_Name = Convert.ToString(reader["location_name"]);
                }
                if (reader["location_name_dw"] != DBNull.Value) {
                    _location_Name_Dw = Convert.ToString(reader["location_name_dw"]);
                }
                cal = Convert.ToString(reader["cal"]) ?? ""; // -- Altered --
            }
            #endregion
        }

        public class OPSRegions
        {
            #region Properties and Fields
            private int _ops_Region_Id;

            private string _ops_Region;
            public int Ops_Region_Id
            {
                get { return _ops_Region_Id; }
            }

            public string Ops_Region
            {
                get { return _ops_Region; }
            }
            #endregion

            #region Constructors
            public OPSRegions(SqlDataReader reader)
            {
                if (reader["ops_region_id"] != DBNull.Value)
                {
                    _ops_Region_Id = Convert.ToInt32(reader["ops_region_id"]);
                }
                if (reader["ops_region"] != DBNull.Value)
                {
                    _ops_Region = Convert.ToString(reader["ops_region"]);
                }
            }
            #endregion
        }

        public class OPSAreas
        {
            #region Properties and Fields
            private int _ops_Area_Id;

            private string _ops_Area;
            public int Ops_Area_Id
            {
                get { return _ops_Area_Id; }
            }

            public string Ops_Area
            {
                get { return _ops_Area; }
            }
            #endregion

            #region Constructors
            public OPSAreas(SqlDataReader reader)
            {
                if (reader["ops_area_id"] != DBNull.Value)
                {
                    _ops_Area_Id = Convert.ToInt32(reader["ops_area_id"]);
                }
                if (reader["ops_area"] != DBNull.Value)
                {
                    _ops_Area = Convert.ToString(reader["ops_area"]);
                }
            }
            #endregion
        }

        public class Fleet
        {
            #region Properties and Fields
            private string _fleet_Name;
            public string Fleet_Name
            {
                get { return _fleet_Name; }
            }
            #endregion

            #region Constructors
            public Fleet(SqlDataReader reader)
            {
                if (reader["fleet_name"] != DBNull.Value)
                {
                    _fleet_Name = Convert.ToString(reader["fleet_name"]);
                }
            }
            #endregion
        }

        public class CarSegments
        {
            #region Properties and Fields
            private int _car_Segment_Id;

            private string _car_Segment;
            public int Car_Segment_Id
            {
                get { return _car_Segment_Id; }
            }

            public string Car_Segment
            {
                get { return _car_Segment; }
            }
            #endregion

            #region Constructors
            public CarSegments(SqlDataReader reader)
            {
                if (reader["car_segment_id"] != DBNull.Value)
                {
                    _car_Segment_Id = Convert.ToInt32(reader["car_segment_id"]);
                }
                if (reader["car_segment"] != DBNull.Value)
                {
                    _car_Segment = Convert.ToString(reader["car_segment"]);
                }
            }
            #endregion
        }

        public class CarClasses
        {
            #region Properties and Fields
            private int _car_Class_Id;

            private string _car_Class;
            public int Car_Class_Id
            {
                get { return _car_Class_Id; }
            }

            public string Car_class
            {
                get { return _car_Class; }
            }
            #endregion

            #region Constructors
            public CarClasses(SqlDataReader reader)
            {
                if (reader["car_class_id"] != DBNull.Value)
                {
                    _car_Class_Id = Convert.ToInt32(reader["car_class_id"]);
                }
                if (reader["car_class"] != DBNull.Value)
                {
                    _car_Class = Convert.ToString(reader["car_class"]);
                }
            }
            #endregion
        }

        public class CarGroups
        {
            #region Properties and Fields
            private int _car_Group_Id;

            private string _car_Group;
            public int Car_Group_Id
            {
                get { return _car_Group_Id; }
            }

            public string Car_Group
            {
                get { return _car_Group; }
            }
            #endregion

            #region Constructors
            public CarGroups(SqlDataReader reader)
            {
                if (reader["car_group_id"] != DBNull.Value)
                {
                    _car_Group_Id = Convert.ToInt32(reader["car_group_id"]);
                }
                if (reader["car_group"] != DBNull.Value)
                {
                    _car_Group = Convert.ToString(reader["car_group"]);
                }
            }
            #endregion
        }

        public class OperStats
        {
            #region Properties and Fields
            private string _operstat_Name;
            public string OperStat_Name
            {
                get { return _operstat_Name; }
            }


            public string OperStat_Desc { get; set; }
            #endregion

            #region Constructors
            public OperStats(SqlDataReader reader)
            {
                if (reader["operstat_name"] != DBNull.Value)
                {
                    _operstat_Name = Convert.ToString(reader["operstat_name"]);
                }

                if (reader["operstat_desc"] != DBNull.Value)
                {
                    OperStat_Desc = Convert.ToString(reader["operstat_desc"]);
                }
            }
            #endregion
        }

        public class ModelCodes
        {
            #region Properties and Fields
            private string _model;
            public string Model
            {
                get { return _model; }
            }
            #endregion

            #region Constructors
            public ModelCodes(SqlDataReader reader)
            {
                if (reader["model"] != DBNull.Value)
                {
                    _model = Convert.ToString(reader["model"]);
                }
            }

            #endregion
        }

        public class OwnAreas
        {
            #region Properties and Fields
            private string _ownArea;
            public string OwnArea
            {
                get { return _ownArea; }
            }
            #endregion

            #region Constructors

            public OwnAreas(SqlDataReader reader)
            {
                if (reader["ownarea"] != DBNull.Value)
                {
                    _ownArea = Convert.ToString(reader["ownarea"]);
                }
            }

            #endregion
        }

        public class MovTypes
        { 
            #region Properties and Fields
            
            private string _movType;
            
            public string MovType
            {
                get { return _movType; }
            }
            
            #endregion

            #region Constructors

            public MovTypes(SqlDataReader reader)
            {
                if (reader["MovementTypeCode"] != DBNull.Value)
                {
                    _movType = Convert.ToString(reader["MovementTypeCode"]);
                }
            }

            #endregion
        }

        public class RemarkList
        { 
            #region Properties and Fields

            private int _remarkId;
            private string _remarkText;

            public int RemarkId
            {
                get { return _remarkId; }
            }

            public string RemarkText
            {
                get { return _remarkText; }
            }


            
            #endregion

            #region Constructors

            public RemarkList(SqlDataReader reader)
            {
                if (reader["RemarkId"] != DBNull.Value)
                {
                    _remarkId = Convert.ToInt32(reader["RemarkId"]);
                }

                if (reader["RemarkText"] != DBNull.Value)
                {
                    _remarkText = Convert.ToString(reader["RemarkText"]);
                }
            }

            #endregion
        }

        public class DayGroupCodes
        { 
            #region Properties and Fields
            
            private string _dayGroupCode;
            
            public string DayGroupCode
            {
                get { return _dayGroupCode; }
            }
            
            #endregion

            #region Constructors

            public DayGroupCodes(SqlDataReader reader)
            {
                if (reader["DayGroupCode"] != DBNull.Value)
                {
                    _dayGroupCode = Convert.ToString(reader["DayGroupCode"]);
                }
            }

            #endregion
        }

        #endregion

    }
}