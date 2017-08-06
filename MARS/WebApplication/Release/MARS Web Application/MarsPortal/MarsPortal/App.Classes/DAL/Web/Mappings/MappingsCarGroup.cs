using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.DAL.Data;

namespace App.BLL
{
    public class MappingsCarGroup {
        // alterations by Gavin for MarsV3 
        // line 53 note stored proc spPortal_CarGroupSelect has been altered

        // added property by Gavin - if contains value then return unmapped car classes only
        public static string carClass { get; set; }

        #region Methods

        public static void SelectCarGroups(int pageSize, int currentPageNumber, string sortExpression, Panel CarGroupsPanel,
                                            Button buttonFirst, Button buttonNext, Button buttonPrevious, Button buttonLast, Label labelTotalPages, DropDownList dropDownListPage,
                                             GridView gridviewCarGroups, Label labelTotalRecords, UserControl emptyDataTemplate, string country, int car_class_id) 
        {

            //Initialise Connection
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.Portal_CarGroupSelect, con);
            
            if (country == "-1" || country == null) {
                cmd.Parameters.AddWithValue("@country", System.DBNull.Value);
            }
            else {
                cmd.Parameters.AddWithValue("@country", country);
            }

            if (car_class_id == 0) {
                cmd.Parameters.AddWithValue("@car_class_id", System.DBNull.Value);
            }
            else {
                cmd.Parameters.AddWithValue("@car_class_id", car_class_id);
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
            // alterations by Gavin for MarsV3
            if (carClass.Contains("UNMAPPED")) { // Extract the unmapped car class
                cmd.Parameters.AddWithValue("@_carClass", "UNMAPPED"); // heavily coupled!
            }


            //Execute Command
            List<MappingsCarGroup.CarGroup> results = new List<MappingsCarGroup.CarGroup>();
            int rowCount = 0;
            using (con) {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read()) {
                    results.Add(new MappingsCarGroup.CarGroup(reader));
                }

                reader.NextResult();

                while (reader.Read()) {
                    rowCount = Convert.ToInt32(reader["totalCount"]);
                }

            }
            con.Close();

            if (rowCount >= 1) {
                //Show car group panel
                CarGroupsPanel.Visible = true;
                //Hide Empty Data Template
                emptyDataTemplate.Visible = false;

                //Databind the gridview
                gridviewCarGroups.DataSource = results;
                gridviewCarGroups.DataBind();

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
            else 
            {
                //Hide car group panel
                CarGroupsPanel.Visible = false;
                //Show Empty Data Template
                emptyDataTemplate.Visible = true;
            }


        }

        public static int InsertCarGroup(string car_group, string car_group_gold
                            , string car_group_fiveStar
                            , string car_group_PresidentCircle
                            , string car_group_Platinum
                        , int car_class_id, int sort_car_group) 
        {

            //Initialise Connection
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.Portal_CarGroupInsert, con);
            
            //Set Parameters 
            cmd.Parameters.AddWithValue("@car_group", car_group);
            cmd.Parameters.AddWithValue("@car_group_gold", car_group_gold);
            cmd.Parameters.AddWithValue("@car_group_Fivestar", car_group_fiveStar);
            cmd.Parameters.AddWithValue("@car_group_PresidentCircle", car_group_PresidentCircle);
            cmd.Parameters.AddWithValue("@car_group_Platinum", car_group_Platinum);
            cmd.Parameters.AddWithValue("@car_class_id", car_class_id);
            cmd.Parameters.AddWithValue("@sort_car_group", sort_car_group);

            //Execute Command
            int result;
            using (con) {
                con.Open();
                result = Convert.ToInt32(cmd.ExecuteScalar());
            }
            con.Close();
            return result;


        }

        public static int UpdateCarGroup(int car_group_id, string car_group, string car_group_gold
                            , string car_group_fiveStar
                            , string car_group_PresidentCircle
                            , string car_group_Platinum
                        ,    int car_class_id, int sort_car_group) 
        {

            //Initialise Connection
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.Portal_CarGroupUpdate, con);

            //Set Parameters 
            cmd.Parameters.AddWithValue("@car_group_id", car_group_id);
            cmd.Parameters.AddWithValue("@car_group", car_group);
            cmd.Parameters.AddWithValue("@car_group_gold", car_group_gold);
            cmd.Parameters.AddWithValue("@car_group_Fivestar", car_group_fiveStar);
            cmd.Parameters.AddWithValue("@car_group_PresidentCircle", car_group_PresidentCircle);
            cmd.Parameters.AddWithValue("@car_group_Platinum", car_group_Platinum);
            cmd.Parameters.AddWithValue("@car_class_id", car_class_id);
            cmd.Parameters.AddWithValue("@sort_car_group", sort_car_group);

            //Execute Command
            using (con) {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            con.Close();
            return 0;


        }

        public static List<CarGroup> SelectCarGroupById(int car_group_id) 
        {
            //Initialise Connection
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.Portal_CarGroupSelectById, con);
            
            //Set Parameters 
            cmd.Parameters.AddWithValue("@car_group_id", car_group_id);

            //Execute Command
            List<CarGroup> results = new List<CarGroup>();
            using (con) {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read()) {
                    results.Add(new CarGroup(reader));
                }
            }
            con.Close();
            return results;

        }

        public static int DeleteCarGroup(int car_group_id) 
        {
            //Initialise Connection
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.Portal_CarGroupDelete, con);
            
          
            //Set Parameters 
            cmd.Parameters.AddWithValue("@car_group_id", car_group_id);

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

        #endregion

        #region Classes

        public class CarGroup {
            #region Delcarations
            private int _car_group_id;
            private string _car_group;
            private string _car_group_gold;
            private int _sort_car_group;
            private int _car_class_id;
            private string _car_class;
            private string _country;

            #endregion

            #region Properties
            public int Car_Group_Id {
                get { return _car_group_id; }
            }

            public string Car_Group {
                get { return _car_group; }
            }

            public string Car_Group_Gold {
                get { return _car_group_gold; }
            }

            public string Car_Group_Fivestar { get; set; }
            public string Car_Group_PresidentCircle { get; set; }
            public string Car_Group_Platinum { get; set; }

            public int Sort_Car_Group {
                get { return _sort_car_group; }
            }

            public int Car_Class_Id {
                get { return _car_class_id; }
            }

            public string Car_Class {
                get { return _car_class; }
            }

            public string Country {
                get { return _country; }
            }

            

            #endregion

            #region Constructors

            public CarGroup(SqlDataReader reader) {
                if (reader["car_group_id"] != DBNull.Value) {
                    _car_group_id = Convert.ToInt32(reader["car_group_id"]);
                }
                if (reader["car_group"] != DBNull.Value) {
                    _car_group = Convert.ToString(reader["car_group"]);
                }
                if (reader["car_group_gold"] != DBNull.Value) {
                    _car_group_gold = Convert.ToString(reader["car_group_gold"]);
                }
                if (reader["sort_car_group"] != DBNull.Value) {
                    _sort_car_group = Convert.ToInt32(reader["sort_car_group"]);
                }
                if (reader["car_class_id"] != DBNull.Value) {
                    _car_class_id = Convert.ToInt32(reader["car_class_id"]);
                }
                if (reader["car_class"] != DBNull.Value) {
                    _car_class = Convert.ToString(reader["car_class"]);
                }
                if (reader["country"] != DBNull.Value) {
                    _country = Convert.ToString(reader["country"]);
                }
                if (reader["car_group_fivestar"] != DBNull.Value)
                {
                    Car_Group_Fivestar = Convert.ToString(reader["car_group_fivestar"]);
                }
                if (reader["car_group_presidentCircle"] != DBNull.Value)
                {
                    Car_Group_PresidentCircle = Convert.ToString(reader["car_group_presidentCircle"]);
                }
                if (reader["car_group_platinum"] != DBNull.Value)
                {
                    Car_Group_Platinum = Convert.ToString(reader["car_group_platinum"]);
                }

            }

            #endregion
        }



        #endregion
    }
}