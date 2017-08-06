using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.DAL.Data;

namespace App.BLL
{
    public class MappingsOPSRegions
    {
        #region Methods

        public static void SelectOPSRegions(int pageSize, int currentPageNumber, string sortExpression, Panel OPSRegionsPanel, Button buttonFirst,
                                        Button buttonNext, Button buttonPrevious, Button buttonLast, Label labelTotalPages, DropDownList dropDownListPage,
                                        GridView gridviewOPSRegions, Label labelTotalRecords, UserControl emptyDataTemplate, string country)
        {
            //Initialise Connection
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.Portal_OPSRegionSelect, con);
            
            if (country == "-1" || country == null)
            {
                cmd.Parameters.AddWithValue("@country", System.DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@country", country);
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
            List<MappingsOPSRegions.OPSRegions> results = new List<MappingsOPSRegions.OPSRegions>();
            int rowCount = 0;
            using (con)
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    results.Add(new MappingsOPSRegions.OPSRegions(reader));
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
                //Show ops region panel
                OPSRegionsPanel.Visible = true;
                //Hide Empty Data Template
                emptyDataTemplate.Visible = false;

                //Databind the gridview
                gridviewOPSRegions.DataSource = results;
                gridviewOPSRegions.DataBind();

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
                //Hide ops region panel
                OPSRegionsPanel.Visible = false;
                //Show Empty Data Template
                emptyDataTemplate.Visible = true;
            }


        }

        public static int InsertOPSRegion(string ops_region, string country)
        {
            //Initialise Connection
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.Portal_OPSRegionInsert, con);
            
            //Set Parameters
            cmd.Parameters.AddWithValue("@ops_region", ops_region);
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

        public static int UpdateOPSRegion(int ops_region_id, string ops_region, string country)
        {
            //Initialise Connection
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.Portal_OPSRegionUpdate, con);
            
            //Set Parameters
            cmd.Parameters.AddWithValue("@ops_region_id", ops_region_id);
            cmd.Parameters.AddWithValue("@ops_region", ops_region);
            cmd.Parameters.AddWithValue("@country", country);

            //Execute Command
            using (con)
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            con.Close();
            return 0;
        }

        public static int DeleteOPSRegion(int ops_region_id)
        {
            //Initialise Connection
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.Portal_OPSRegionDelete, con);
            
            //Set Parameters
            cmd.Parameters.AddWithValue("@ops_region_id", ops_region_id);

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

        public static List<OPSRegions> SelectOPSRegionById(int ops_region_id)
        {
            //Initialise Connection
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.Portal_OPSRegionSelectById, con);
            
            //Set Parameters
            cmd.Parameters.AddWithValue("@ops_region_id", ops_region_id);

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

        #endregion

        #region Classes

        public class OPSRegions
        {
            #region Delcarations
            private int _ops_region_id;
            private string _ops_region;
            private string _country;

            #endregion

            #region Properties
            public int OPS_Region_Id
            {
                get { return _ops_region_id; }
            }

            public string OPS_Region
            {
                get { return _ops_region; }
            }

            public string Country
            {
                get { return _country; }
            }

            #endregion

            #region Constructors
            public OPSRegions(SqlDataReader reader)
            {
                if (reader["ops_region_id"] != DBNull.Value)
                {
                    _ops_region_id = Convert.ToInt32(reader["ops_region_id"]);
                }
                if (reader["ops_region"] != DBNull.Value)
                {
                    _ops_region = Convert.ToString(reader["ops_region"]);
                }
                if (reader["country"] != DBNull.Value)
                {
                    _country = Convert.ToString(reader["country"]);
                }
            }
            #endregion
        }



        #endregion

    }
}