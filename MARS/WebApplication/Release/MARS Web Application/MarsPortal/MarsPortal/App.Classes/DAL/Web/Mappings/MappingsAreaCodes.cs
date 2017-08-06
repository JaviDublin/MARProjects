using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.DAL.Data;

namespace App.BLL
{
    public class MappingsAreaCodes
    {

        #region Methods

        public static void SelectAreaCodes(int pageSize, int currentPageNumber, string sortExpression, Panel AreaCodesPanel, Button buttonFirst,
                                            Button buttonNext, Button buttonPrevious, Button buttonLast, Label labelTotalPages, DropDownList dropDownListPage,
                                                GridView gridviewAreaCodes, Label labelTotalRecords, UserControl emptyDataTemplate, string country)
        {


            //Initialise Connection
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.Portal_AreaCodeSelect, con);
            
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
            List<MappingsAreaCodes.AreaCodes> results = new List<MappingsAreaCodes.AreaCodes>();
            int rowCount = 0;
            using (con)
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    results.Add(new MappingsAreaCodes.AreaCodes(reader));
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
                //Show area code panel
                AreaCodesPanel.Visible = true;
                //Hide Empty Data Template
                emptyDataTemplate.Visible = false;

                //Databind the gridview
                gridviewAreaCodes.DataSource = results;
                gridviewAreaCodes.DataBind();

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
                //Hide area code panel
                AreaCodesPanel.Visible = false;
                //Show Empty Data Template
                emptyDataTemplate.Visible = true;
            }
        }



        public static int CheckAreaCodeExists(string ownarea)
        {

            //Initialise Connection
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.Portal_AreaCodeCheckExists, con);
            
            //Set Parameters
            cmd.Parameters.AddWithValue("@ownarea", ownarea);

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


        public static int InsertAreaCode(string ownarea, string country, string area_name, bool opco, bool fleetco, bool carsales, bool licensee)
        {

            //Initialise Connection
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.Portal_AreaCodeInsert, con);
            
            //Set Parameters
            cmd.Parameters.AddWithValue("@ownarea", ownarea);
            cmd.Parameters.AddWithValue("@country", country);
            cmd.Parameters.AddWithValue("@area_name", area_name);
            cmd.Parameters.AddWithValue("@opco", opco);
            cmd.Parameters.AddWithValue("@fleetco", fleetco);
            cmd.Parameters.AddWithValue("@carsales", carsales);
            cmd.Parameters.AddWithValue("@licensee", licensee);


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


        public static int UpdateAreaCode(string ownarea, string country, string area_name, bool opco, bool fleetco, bool carsales, bool licensee)
        {

            //Initialise Connection
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.Portal_AreaCodeUpdate, con);
            
            //Set Parameters
            cmd.Parameters.AddWithValue("@ownarea", ownarea);
            cmd.Parameters.AddWithValue("@country", country);
            cmd.Parameters.AddWithValue("@area_name", area_name);
            cmd.Parameters.AddWithValue("@opco", opco);
            cmd.Parameters.AddWithValue("@fleetco", fleetco);
            cmd.Parameters.AddWithValue("@carsales", carsales);
            cmd.Parameters.AddWithValue("@licensee", licensee);


            //Execute Command
            using (con)
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            con.Close();
            return 0;

        }

        public static int DeleteAreaCode(string ownarea)
        {

            //Initialise Connection
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.Portal_AreaCodeDelete, con);
            
            //Set Parameters
            cmd.Parameters.AddWithValue("@ownarea", ownarea);

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


        public static List<AreaCodes> SelectAreaCodesByOwnArea(string ownarea)
        {
            //Initialise Connection
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.Portal_AreaCodeSelectByOwnArea, con);
            
            //Set Parameters
            cmd.Parameters.AddWithValue("@ownarea", ownarea);

            //Execute Command
            List<AreaCodes> results = new List<AreaCodes>();
            using (con)
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    results.Add(new AreaCodes(reader));
                }
            }
            con.Close();
            return results;
        }



        #endregion

        #region Classes

        public class AreaCodes
        {
            #region Delcarations

            private string _ownarea;
            private string _country;
            private string _area_name;
            private bool _opco;
            private bool _fleetco;
            private bool _carsales;
            private bool _licensee;


            #endregion

            #region Properties
            public string OwnArea
            {
                get { return _ownarea; }
            }

            public string Country
            {
                get { return _country; }
            }

            public string Area_Name
            {
                get { return _area_name; }
            }

            public bool OPCO
            {
                get { return _opco; }
            }

            public bool FleetCo
            {
                get { return _fleetco; }
            }

            public bool CarSales
            {
                get { return _carsales; }
            }

            public bool Licensee
            {
                get { return _licensee; }
            }

            #endregion

            #region Constructors

            public AreaCodes(SqlDataReader reader)
            {
                if (reader["ownarea"] != DBNull.Value)

                    _ownarea = Convert.ToString(reader["ownarea"]);

                if (reader["country"] != DBNull.Value)

                    _country = Convert.ToString(reader["country"]);

                if (reader["area_name"] != DBNull.Value)

                    _area_name = Convert.ToString(reader["area_name"]);

                if (reader["opco"] != DBNull.Value)

                    _opco = Convert.ToBoolean(reader["opco"]);

                if (reader["fleetco"] != DBNull.Value)

                    _fleetco = Convert.ToBoolean(reader["fleetco"]);

                if (reader["carsales"] != DBNull.Value)

                    _carsales = Convert.ToBoolean(reader["carsales"]);

                if (reader["licensee"] != DBNull.Value)

                    _licensee = Convert.ToBoolean(reader["licensee"]);


            }

            #endregion

        }


        #endregion

    }
}