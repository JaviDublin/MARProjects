using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.DAL.Data;

namespace App.BLL
{
    public class MappingsCMSLocations
    {
        #region Methods

        public static void SelectCMSLocations(int pageSize, int currentPageNumber, string sortExpression, Panel CMSLocationsPanel, Button buttonFirst,
                                                Button buttonNext, Button buttonPrevious, Button buttonLast, Label labelTotalPages, DropDownList dropDownListPage,
                                            GridView gridviewCMSLocations, Label labelTotalRecords, UserControl emptyDataTemplate, string country, int cms_pool_id)
        {

            //Initialise Connection
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.Portal_CMSLocationGroupSelect, con);

            if (country == "-1" || country == null)
            {
                cmd.Parameters.AddWithValue("@country", System.DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@country", country);
            }


            if (cms_pool_id == 0)
            {
                cmd.Parameters.AddWithValue("@cms_pool_id", System.DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@cms_pool_id", cms_pool_id);
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
            List<MappingsCMSLocations.CMSLocations> results = new List<MappingsCMSLocations.CMSLocations>();
            int rowCount = 0;
            using (con)
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    results.Add(new MappingsCMSLocations.CMSLocations(reader));
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
                //Show cms locations panel
                CMSLocationsPanel.Visible = true;
                //Hide Empty Data Template
                emptyDataTemplate.Visible = false;

                //Databind the gridview
                gridviewCMSLocations.DataSource = results;
                gridviewCMSLocations.DataBind();

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
                //Hide cms locations
                CMSLocationsPanel.Visible = false;
                //Show Empty Data Template
                emptyDataTemplate.Visible = true;
            }




        }


        public static int InsertCMSLocationGroup( string cms_location_group_code_dw, string cms_location_group, int cms_pool_id)
        {

            //Initialise Connection
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.Portal_CMSLocationGroupInsert, con);
            

            //Set Parameters
       
            cmd.Parameters.AddWithValue("@cms_location_group_code_dw", cms_location_group_code_dw);
            cmd.Parameters.AddWithValue("@cms_location_group", cms_location_group);
            cmd.Parameters.AddWithValue("@cms_pool_id", cms_pool_id);

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

        public static int UpdateCMSLocationGroup(int cms_location_group_id, string cms_location_group_code_dw, string cms_location_group, int cms_pool_id)
        {

            //Initialise Connection
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.Portal_CMSLocationGroupUpdate, con);

            //Set Parameters
            cmd.Parameters.AddWithValue("@cms_location_group_id", cms_location_group_id);
            cmd.Parameters.AddWithValue("@cms_location_group_code_dw", cms_location_group_code_dw);
            cmd.Parameters.AddWithValue("@cms_location_group", cms_location_group);
            cmd.Parameters.AddWithValue("@cms_pool_id", cms_pool_id);

            //Execute Command
            using (con)
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            con.Close();
            return 0;


        }

        public static int DeleteCMSLocationGroup(int cms_location_group_id)
        {
            //Initialise Connection
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.Portal_CMSLocationGroupDelete, con);
            
            //Set Parameters
            cmd.Parameters.AddWithValue("@cms_location_group_id", cms_location_group_id);

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

        public static List<CMSLocations> SelectCMSLocationGroupByCode(int cms_location_group_id)
        {
            //Initialise Connection
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.Portal_CMSLocationGroupSelectByCode, con);
            
            //Set Parameters
            cmd.Parameters.AddWithValue("@cms_location_group_id", cms_location_group_id);

            //Execute Command
            List<CMSLocations> results = new List<CMSLocations>();
            using (con)
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    results.Add(new CMSLocations(reader));
                }
            }
            con.Close();
            return results;


        }

        public static int CheckCMSLocationGroupExists(int cms_location_group_id)
        {
            //Initialise Connection
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.Portal_CMSLocationGroupCheckExists, con);
            
            //Set Parameters
            cmd.Parameters.AddWithValue("@cms_location_group_id", cms_location_group_id);

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

        #endregion

        #region Classes

        public class CMSLocations
        {
            #region Delcarations
            private int _cms_location_group_id;
            private string _cms_location_group_code_dw;
            private string _cms_location_group;
            private int _cms_pool_id;
            private string _cms_pool;
            private string _country;
            #endregion

            #region Properties
            public int CMS_Location_Group_Id
            {
                get { return _cms_location_group_id; }
            }

            public string CMS_Location_Group_Code_DW
            {
                get { return _cms_location_group_code_dw; }
            }

            public string CMS_Location_group
            {
                get { return _cms_location_group; }
            }

            public int CMS_Pool_Id
            {
                get { return _cms_pool_id; }
            }

            public string CMS_Pool
            {
                get { return _cms_pool; }
            }

            public string Country
            {
                get { return _country; }
            }

            #endregion

            #region Constructors

            public CMSLocations(SqlDataReader reader)
            {
                if (reader["cms_location_group_id"] != DBNull.Value)
                {
                    _cms_location_group_id = int.Parse(reader["cms_location_group_id"].ToString());
                }
                if (reader["cms_location_group_code_dw"] != DBNull.Value)
                {
                    _cms_location_group_code_dw = Convert.ToString(reader["cms_location_group_code_dw"]);
                }
                if (reader["cms_location_group"] != DBNull.Value)
                {
                    _cms_location_group = Convert.ToString(reader["cms_location_group"]);
                }
                if (reader["cms_pool_id"] != DBNull.Value)
                {
                    _cms_pool_id = Convert.ToInt32(reader["cms_pool_id"]);
                }
                if (reader["cms_pool"] != DBNull.Value)
                {
                    _cms_pool = Convert.ToString(reader["cms_pool"]);
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