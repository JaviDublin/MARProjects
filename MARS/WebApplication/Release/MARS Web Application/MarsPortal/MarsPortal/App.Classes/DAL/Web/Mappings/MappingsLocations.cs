using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.DAL.Data;

namespace App.BLL
{
    public class MappingsLocations {
        // alterations by Gavin for MarsV3 19-4-12
        // altered at line 61 to add Unmapped locations

        // property add to find unmapped 
        public static string Loc { get; set; }

        #region Methods
        public static void SelectLocations(int pageSize, int currentPageNumber, string sortExpression, Panel LocationsPanel, Button buttonFirst,
            Button buttonNext, Button buttonPrevious, Button buttonLast, Label labelTotalPages, DropDownList dropDownListPage,
                GridView gridviewLocations, Label labelTotalRecords, UserControl emptyDataTemplate, string country, int cms_location_group_id, int ops_area_id) 
        {

            //Initialise Connection
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.Portal_LocationSelect, con);
            
            if (country == "-1" || country == null) {
                cmd.Parameters.AddWithValue("@country", System.DBNull.Value);
            }
            else {
                cmd.Parameters.AddWithValue("@country", country);
            }


            if (cms_location_group_id == 0) {
                cmd.Parameters.AddWithValue("@cms_location_group_id", System.DBNull.Value);
            }
            else {
                cmd.Parameters.AddWithValue("@cms_location_group_id", cms_location_group_id);
            }


            if (ops_area_id == 0) {
                cmd.Parameters.AddWithValue("@ops_area_id", System.DBNull.Value);
            }
            else {
                cmd.Parameters.AddWithValue("@ops_area_id", ops_area_id);
            }

            int startRowIndex = ((currentPageNumber - 1) * pageSize) + 1;
            int maximumRows = (currentPageNumber * pageSize);


            cmd.Parameters.AddWithValue("@startRowIndex", startRowIndex);
            cmd.Parameters.AddWithValue("@maximumRows", maximumRows);

            if (sortExpression == null) {
                cmd.Parameters.AddWithValue("@sortExpression", System.DBNull.Value);
            }
            else {
                cmd.Parameters.AddWithValue("@sortExpression", sortExpression);
            }
           //if (Loc.Contains("UNMAPPED")) cmd.Parameters.AddWithValue("@_location", "UNMAPPED"); // heavily coupled!!! 

            //Execute Command
            List<MappingsLocations.Locations> results = new List<MappingsLocations.Locations>();
            int rowCount = 0;
            using (con) {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read()) {
                    results.Add(new MappingsLocations.Locations(reader));
                }

                reader.NextResult();

                while (reader.Read()) {
                    rowCount = Convert.ToInt32(reader["totalCount"]);
                }

            }
            con.Close();



            if (rowCount >= 1) {
                //Show ops locations panel
                LocationsPanel.Visible = true;
                //Hide Empty Data Template
                emptyDataTemplate.Visible = false;

                //Databind the gridview
                gridviewLocations.DataSource = results;
                gridviewLocations.DataBind();

                //Display results
                int totalPages = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(rowCount) / Convert.ToDouble(pageSize)));
                labelTotalPages.Text = totalPages.ToString();

                //Show totals label
                labelTotalRecords.Text = rowCount.ToString();

                //Clear items in page drop down list
                dropDownListPage.Items.Clear();
                //Add list of pages to drop down list
                for (int i = 1; i <= Convert.ToInt32(labelTotalPages.Text); i++) {
                    dropDownListPage.Items.Add(new ListItem(i.ToString()));
                }
                //Set the selected page
                dropDownListPage.SelectedValue = currentPageNumber.ToString();

                //Set pager buttons depending on page selected

                if (currentPageNumber == 1) {
                    buttonPrevious.Enabled = false;
                    buttonPrevious.CssClass = "PagerPreviousInactive";
                    buttonFirst.Enabled = false;
                    buttonFirst.CssClass = "PagerFirstInactive";

                    if ((Convert.ToInt32(labelTotalPages.Text) > 1)) {
                        buttonNext.Enabled = true;
                        buttonNext.CssClass = "PagerNextActive";
                        buttonLast.Enabled = true;
                        buttonLast.CssClass = "PagerLastActive";
                    }
                    else {
                        buttonNext.Enabled = false;
                        buttonNext.CssClass = "PagerNextInactive";
                        buttonLast.Enabled = false;
                        buttonLast.CssClass = "PagerLastInactive";
                    }
                }
                else {
                    buttonPrevious.Enabled = true;
                    buttonPrevious.CssClass = "PagerPreviousActive";
                    buttonFirst.Enabled = true;
                    buttonFirst.CssClass = "PagerFirstActive";

                    if ((currentPageNumber == Convert.ToInt32(labelTotalPages.Text))) {
                        buttonNext.Enabled = false;
                        buttonNext.CssClass = "PagerNextInactive";
                        buttonLast.Enabled = false;
                        buttonLast.CssClass = "PagerLastInactive";
                    }
                    else {
                        buttonNext.Enabled = true;
                        buttonNext.CssClass = "PagerNextActive";
                        buttonLast.Enabled = true;
                        buttonLast.CssClass = "PagerLastActive";

                    }
                }

            }
            else {
                //Hide locations panel
                LocationsPanel.Visible = false;
                //Show Empty Data Template
                emptyDataTemplate.Visible = true;
            }




        }


        public static int InsertLocation(string location, string location_dw, string real_location_name, string location_name, string location_name_dw,
            bool active, string ap_dt_rr, string cal, int cms_location_group_id, int ops_area_id, string served_by_locn, int turnaround_hours) 
        {

            //Initialise Connection
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.Portal_LocationInsert, con);

            //Set Parameters
            cmd.Parameters.AddWithValue("@location", location);
            cmd.Parameters.AddWithValue("@location_dw", location_dw);
            cmd.Parameters.AddWithValue("@real_location_name", real_location_name);
            cmd.Parameters.AddWithValue("@location_name", location_name);
            cmd.Parameters.AddWithValue("@location_name_dw", location_name_dw);
            cmd.Parameters.AddWithValue("@active", active);
            cmd.Parameters.AddWithValue("@ap_dt_rr", ap_dt_rr);
            cmd.Parameters.AddWithValue("@cal", cal);
            cmd.Parameters.AddWithValue("@cms_location_group_id", cms_location_group_id);
            cmd.Parameters.AddWithValue("@ops_area_id", ops_area_id);
            cmd.Parameters.AddWithValue("@served_by_locn", served_by_locn);
            cmd.Parameters.AddWithValue("@turnaround_hours", turnaround_hours);


            //Execute Command
            int result;
            using (con) {
                con.Open();
                result = Convert.ToInt32(cmd.ExecuteScalar());
            }
            con.Close();
            return result;


        }

        public static int UpdateLocation(string location, string location_dw, string real_location_name, string location_name, string location_name_dw,
            bool active, string ap_dt_rr, string cal, int cms_location_group_id, int ops_area_id, string served_by_locn, int turnaround_hours) 
        {

            //Initialise Connection
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.Portal_LocationUpdate, con);

            //Set Parameters
            cmd.Parameters.AddWithValue("@location", location);
            cmd.Parameters.AddWithValue("@location_dw", location_dw);
            cmd.Parameters.AddWithValue("@real_location_name", real_location_name);
            cmd.Parameters.AddWithValue("@location_name", location_name);
            cmd.Parameters.AddWithValue("@location_name_dw", location_name_dw);
            cmd.Parameters.AddWithValue("@active", active);
            cmd.Parameters.AddWithValue("@ap_dt_rr", ap_dt_rr);
            cmd.Parameters.AddWithValue("@cal", cal);
            cmd.Parameters.AddWithValue("@cms_location_group_id", cms_location_group_id);
            cmd.Parameters.AddWithValue("@ops_area_id", ops_area_id);
            cmd.Parameters.AddWithValue("@served_by_locn", served_by_locn);
            cmd.Parameters.AddWithValue("@turnaround_hours", turnaround_hours);

            //Execute Command
            using (con) {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            con.Close();
            return 0;
        }

        public static int DeleteLocation(string location) 
        {
            //Initialise Connection
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.Portal_LocationDelete, con);

            //Set Parameters
            cmd.Parameters.AddWithValue("@location", location);

            //Execute Command
            int result;
            using (con) {
                con.Open();
                result = Convert.ToInt32(cmd.ExecuteScalar());
            }
            con.Close();

            if (result == 0) {
                return 0;
            }
            else {
                //Database constraint
                return -2;
            }


        }

        public static List<Locations> SelectLocationByLocation(string location) 
        {
            //Initialise Connection
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.Portal_LocationSelectByLocation, con);

            //Set Parameters
            cmd.Parameters.AddWithValue("@location", location);

            List<Locations> results = new List<Locations>();
            using (con) {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read()) {
                    results.Add(new Locations(reader));
                }
            }
            con.Close();
            return results;


        }

        public static int CheckLocationExists(string location) 
        {
            //Initialise Connection
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.Portal_LocationCheckExists, con);

            //Set Parameters
            cmd.Parameters.AddWithValue("@location", location);

            //Execute Command
            int result;
            using (con) {
                con.Open();
                result = Convert.ToInt32(cmd.ExecuteScalar());
            }
            con.Close();
            return result;


        }

        #endregion

        #region Classes

        public class Locations {
            #region Delcarations
            private string _location;
            private string _location_dw;
            private string _real_location_name;
            private string _location_name;
            private string _location_name_dw;
            private bool _active;
            private string _ap_dt_rr;
            private string _cal;
            private int _ops_area_id;
            private string _ops_area;
            private int _cms_location_group_id;
            private string _cms_location_group;
            private string _served_by_locn;
            private int _turnaround_hours;

            #endregion

            #region Properties
            public string Location {
                get { return _location; }
            }

            public string Location_DW {
                get { return _location_dw; }
            }

            public string Real_Location_Name {
                get { return _real_location_name; }
            }

            public string Location_Name {
                get { return _location_name; }
            }

            public string Location_Name_DW {
                get { return _location_name_dw; }
            }
            public bool Active {
                get { return _active; }
            }

            public string AP_DT_RR {
                get { return _ap_dt_rr; }
            }

            public string CAL {
                get { return _cal; }
            }

            public int OPS_Area_Id {
                get { return _ops_area_id; }
            }

            public string OPS_Area {
                get { return _ops_area; }
            }

            public int CMS_Location_Group_Id {
                get { return _cms_location_group_id; }
            }

            public string CMS_Location_Group {
                get { return _cms_location_group; }
            }

            public string Served_By_Locn {
                get { return _served_by_locn; }
            }

            public int Turnaround_Hours {
                get { return _turnaround_hours; }
            }

            #endregion

            #region Constructors
            public Locations(SqlDataReader reader) {
                if (reader["location"] != DBNull.Value) {
                    _location = Convert.ToString(reader["location"]);
                }
                if (reader["location_dw"] != DBNull.Value) {
                    _location_dw = Convert.ToString(reader["location_dw"]);
                }
                if (reader["real_location_name"] != DBNull.Value) {
                    _real_location_name = Convert.ToString(reader["real_location_name"]);
                }
                if (reader["location_name"] != DBNull.Value) {
                    _location_name = Convert.ToString(reader["location_name"]);
                }
                if (reader["location_name_dw"] != DBNull.Value) {
                    _location_name_dw = Convert.ToString(reader["location_name_dw"]);
                }
                if (reader["active"] != DBNull.Value) {
                    _active = Convert.ToBoolean(reader["active"]);
                }
                if (reader["ap_dt_rr"] != DBNull.Value) {
                    _ap_dt_rr = Convert.ToString(reader["ap_dt_rr"]);
                }
                if (reader["cal"] != DBNull.Value) {
                    _cal = Convert.ToString(reader["cal"]);
                }
                if (reader["ops_area_id"] != DBNull.Value) {
                    _ops_area_id = Convert.ToInt32(reader["ops_area_id"]);
                }
                if (reader["ops_area"] != DBNull.Value) {
                    _ops_area = Convert.ToString(reader["ops_area"]);
                }
                if (reader["cms_location_group_id"] != DBNull.Value) {
                    _cms_location_group_id = int.Parse(reader["cms_location_group_id"].ToString());
                }
                if (reader["cms_location_group"] != DBNull.Value) {
                    _cms_location_group = Convert.ToString(reader["cms_location_group"]);
                }
                if (reader["served_by_locn"] != DBNull.Value) {
                    _served_by_locn = Convert.ToString(reader["served_by_locn"]);
                }
                if (reader["turnaround_hours"] != DBNull.Value) {
                    _turnaround_hours = Convert.ToInt32(reader["turnaround_hours"]);
                }


            }

            #endregion
        }



        #endregion
    }
}