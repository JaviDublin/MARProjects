using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.DAL.Data;

namespace App.BLL
{
    public class MappingsCountry
    {
        #region Methods

        public static void SelectCountry(int pageSize, int currentPageNumber, string sortExpression, Panel CountryPanel, Button buttonFirst,
                                        Button buttonNext, Button buttonPrevious, Button buttonLast, Label labelTotalPages, DropDownList dropDownListPage,
                                        GridView gridviewCountry, Label labelTotalRecords, UserControl emptyDataTemplate)
        {
            //Initialise Connection
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.Portal_CountrySelect, con);
            
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
            List<MappingsCountry.CountryList> results = new List<MappingsCountry.CountryList>();
            int rowCount = 0;
            using (con)
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    results.Add(new MappingsCountry.CountryList(reader));
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
                //Show country panel
                CountryPanel.Visible = true;
                //Hide Empty Data Template
                emptyDataTemplate.Visible = false;

                //Databind the gridview
                gridviewCountry.DataSource = results;
                gridviewCountry.DataBind();

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
                //Hide country panel
                CountryPanel.Visible = false;
                //Show Empty Data Template
                emptyDataTemplate.Visible = true;
            }
        }

        public static int CheckIfCountryExists(string country)
        {
            //Initialise Connection
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.Portal_CountryCheckExists, con);
            
            //Set Parameters
            cmd.Parameters.AddWithValue("@country", country);

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


        public static int InsertCountry(string country, string country_dw, string country_description, bool active)
        {

            //Initialise Connection
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.Portal_CountryInsert, con);

            //Set Parameters
            cmd.Parameters.AddWithValue("@country", country);
            cmd.Parameters.AddWithValue("@country_dw", country_dw);
            cmd.Parameters.AddWithValue("@country_description", country_description);
            cmd.Parameters.AddWithValue("@active", active);

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


        public static int DeleteCountry(string country)
        {
            //Initialise Connection
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.Portal_CountryDelete, con);
            
            //Set parameters
            cmd.Parameters.AddWithValue("@country", country);

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


        public static int UpdateCountry(string country, string country_dw, string country_description, bool active)
        {

            //Initialise Connection
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.Portal_CountryUpdate, con);

            //Set Parameters
            cmd.Parameters.AddWithValue("@country", country);
            cmd.Parameters.AddWithValue("@country_dw", country_dw);
            cmd.Parameters.AddWithValue("@country_description", country_description);
            cmd.Parameters.AddWithValue("@active", active);

            //Execute Command
            using (con)
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            con.Close();
            return 0;



        }

        public static List<CountryList> SelectCountryByCountry(string country)
        {
            //Initialise Connection
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.Portal_CountrySelectByCountry, con);
            
            //Set Parameters
            cmd.Parameters.AddWithValue("@country", country);

            //Execute Command
            List<CountryList> results = new List<CountryList>();
            using (con)
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    results.Add(new CountryList(reader));
                }
            }
            con.Close();
            return results;
        }


        #endregion

        #region Classes
        public class CountryList
        {
            #region Delcarations
            private string _country;
            private string _country_dw;
            private string _country_description;
            private bool _active;

            #endregion

            #region Properties

            public string Country
            {
                get { return _country; }
            }

            public string Country_DW
            {
                get { return _country_dw; }
            }

            public string Country_Description
            {
                get { return _country_description; }
            }

            public bool Active
            {
                get { return _active; }
            }

            #endregion

            #region Constructors
            public CountryList(SqlDataReader reader)
            {
                if (reader["country"] != DBNull.Value)
                {
                    _country = Convert.ToString(reader["country"]);
                }
                if (reader["country_dw"] != DBNull.Value)
                {
                    _country_dw = Convert.ToString(reader["country_dw"]);
                }
                if (reader["country_description"] != DBNull.Value)
                {
                    _country_description = Convert.ToString(reader["country_description"]);
                }
                if (reader["active"] != DBNull.Value)
                {
                    _active = Convert.ToBoolean(reader["active"]);
                }
            }
        }

            #endregion

        #endregion
    }
}