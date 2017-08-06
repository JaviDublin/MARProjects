using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.DAL.Data;

namespace App.BLL
{
    public class MappingsCarClass
    {

        #region Methods


        public static void SelectCarClasses(int pageSize, int currentPageNumber, string sortExpression, Panel CarClassesPanel, Button buttonFirst, Button buttonNext,
                                            Button buttonPrevious, Button buttonLast, Label labelTotalPages, DropDownList dropDownListPage,
                                            GridView gridviewCarClasses, Label labelTotalRecords, UserControl emptyDataTemplate, string country, int car_segment_id)
        {

            //Initialise Connection
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.Portal_CarClassSelect, con);

            if (country == "-1" || country == null)
            {
                cmd.Parameters.AddWithValue("@country", System.DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@country", country);
            }

            if (car_segment_id == 0)
            {
                cmd.Parameters.AddWithValue("@car_segment_id", System.DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@car_segment_id", car_segment_id);
            }

            int startRowIndex = ((currentPageNumber - 1) * pageSize) + 1;
            int maximumRows = (currentPageNumber * pageSize);


            cmd.Parameters.AddWithValue("@startRowIndex", startRowIndex);
            cmd.Parameters.AddWithValue("@maximumRows", maximumRows);

            if (sortExpression == null)
            {
                cmd.Parameters.AddWithValue("@sortExpression", System.DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@sortExpression", sortExpression);
            }

            //Execute Command
            List<MappingsCarClass.CarClass> results = new List<MappingsCarClass.CarClass>();
            int rowCount = 0;
            using (con)
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    results.Add(new MappingsCarClass.CarClass(reader));
                }

                reader.NextResult();

                while (reader.Read())
                {
                    rowCount = Convert.ToInt32(reader["totalCount"]);
                }

            }
            con.Close();



            if (rowCount >= 1)
            {
                //Show car class panel
                CarClassesPanel.Visible = true;
                //Hide Empty Data Template
                emptyDataTemplate.Visible = false;

                //Databind the gridview
                gridviewCarClasses.DataSource = results;
                gridviewCarClasses.DataBind();

                //Display results
                int totalPages = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(rowCount) / Convert.ToDouble(pageSize)));
                labelTotalPages.Text = totalPages.ToString();

                //Show totals label
                labelTotalRecords.Text = rowCount.ToString();

                //Clear items in page drop down list
                dropDownListPage.Items.Clear();
                //Add list of pages to drop down list
                for (int i = 1; i <= Convert.ToInt32(labelTotalPages.Text); i++)
                {
                    dropDownListPage.Items.Add(new ListItem(i.ToString()));
                }
                //Set the selected page
                dropDownListPage.SelectedValue = currentPageNumber.ToString();

                //Set pager buttons depending on page selected

                if (currentPageNumber == 1)
                {
                    buttonPrevious.Enabled = false;
                    buttonPrevious.CssClass = "PagerPreviousInactive";
                    buttonFirst.Enabled = false;
                    buttonFirst.CssClass = "PagerFirstInactive";

                    if ((Convert.ToInt32(labelTotalPages.Text) > 1))
                    {
                        buttonNext.Enabled = true;
                        buttonNext.CssClass = "PagerNextActive";
                        buttonLast.Enabled = true;
                        buttonLast.CssClass = "PagerLastActive";
                    }
                    else
                    {
                        buttonNext.Enabled = false;
                        buttonNext.CssClass = "PagerNextInactive";
                        buttonLast.Enabled = false;
                        buttonLast.CssClass = "PagerLastInactive";
                    }
                }
                else
                {
                    buttonPrevious.Enabled = true;
                    buttonPrevious.CssClass = "PagerPreviousActive";
                    buttonFirst.Enabled = true;
                    buttonFirst.CssClass = "PagerFirstActive";

                    if ((currentPageNumber == Convert.ToInt32(labelTotalPages.Text)))
                    {
                        buttonNext.Enabled = false;
                        buttonNext.CssClass = "PagerNextInactive";
                        buttonLast.Enabled = false;
                        buttonLast.CssClass = "PagerLastInactive";
                    }
                    else
                    {
                        buttonNext.Enabled = true;
                        buttonNext.CssClass = "PagerNextActive";
                        buttonLast.Enabled = true;
                        buttonLast.CssClass = "PagerLastActive";

                    }
                }

            }
            else
            {
                //Hide car class panel
                CarClassesPanel.Visible = false;
                //Show Empty Data Template
                emptyDataTemplate.Visible = true;
            }


        }


        public static int InsertCarClass(string car_class, int car_segment_id, int sort_car_class)
        {

            //Initialise Connection
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.Portal_CarClassInsert, con);
            
           
            //Set Parameters
            cmd.Parameters.AddWithValue("@car_class", car_class);
            cmd.Parameters.AddWithValue("@car_segment_id", car_segment_id);
            cmd.Parameters.AddWithValue("@sort_car_class", sort_car_class);

            //Execute Command
            int result;
            using (con)
            {
                con.Open();
                result = Convert.ToInt32(cmd.ExecuteScalar());
            }
            con.Close();
            return result;
        }

        public static int UpdateCarClass(int car_class_id, string car_class, int car_segment_id, int sort_car_class)
        {

            //Initialise Connection
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.Portal_CarClassUpdate, con);

            //Set Parameters
            cmd.Parameters.AddWithValue("@car_class_id", car_class_id);
            cmd.Parameters.AddWithValue("@car_class", car_class);
            cmd.Parameters.AddWithValue("@car_segment_id", car_segment_id);
            cmd.Parameters.AddWithValue("@sort_car_class", sort_car_class);

            //Execute Command
            using (con)
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            con.Close();
            return 0;


        }


        public static int DeleteCarClass(int car_class_id)
        {

            //Initialise Connection
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.Portal_CarClassDelete, con);
            
            //Set Parameters
            cmd.Parameters.AddWithValue("@car_class_id", car_class_id);

            //Execute Command
            int result;
            using (con)
            {
                con.Open();
                result = Convert.ToInt32(cmd.ExecuteScalar());
            }
            con.Close();

            if (result == 0)
            {
                return 0;
            }
            else
            {
                //Database constraint
                return -2;
            }


        }

        public static List<CarClass> SelectCarClassById(int car_class_id)
        {

            //Initialise Connection
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.Portal_CarClassSelectById, con);
            
            //Set Parameters
            cmd.Parameters.AddWithValue("@car_class_id", car_class_id);

            //Execute Command
            List<CarClass> results = new List<CarClass>();
            using (con)
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    results.Add(new CarClass(reader));
                }
            }
            con.Close();
            return results;


        }

        #endregion

        #region Classes
        public class CarClass
        {

            #region Delcarations
            private int _car_class_id;
            private string _car_class;
            private int _sort_car_class;
            private int _car_segment_id;
            private string _car_segment;
            private string _country;

            #endregion

            #region Properties
            public int Car_Class_Id
            {
                get { return _car_class_id; }
            }

            public string Car_Class
            {
                get { return _car_class; }
            }

            public int Sort_Car_Class
            {
                get { return _sort_car_class; }
            }

            public int Car_Segment_Id
            {
                get { return _car_segment_id; }
            }

            public string Car_Segment
            {
                get { return _car_segment; }
            }

            public string Country
            {
                get { return _country; }
            }

            #endregion

            #region Constructors

            public CarClass(SqlDataReader reader)
            {
                if (reader["car_class_id"] != DBNull.Value)

                    _car_class_id = Convert.ToInt32(reader["car_class_id"]);

                if (reader["car_class"] != DBNull.Value)

                    _car_class = Convert.ToString(reader["car_class"]);

                if (reader["sort_car_class"] != DBNull.Value)

                    _sort_car_class = Convert.ToInt32(reader["sort_car_class"]);

                if (reader["car_segment_id"] != DBNull.Value)

                    _car_segment_id = Convert.ToInt32(reader["car_segment_id"]);

                if (reader["car_segment"] != DBNull.Value)

                    _car_segment = Convert.ToString(reader["car_segment"]);

                if (reader["country"] != DBNull.Value)

                    _country = Convert.ToString(reader["country"]);


            }

            #endregion

        }


        #endregion
    }
}