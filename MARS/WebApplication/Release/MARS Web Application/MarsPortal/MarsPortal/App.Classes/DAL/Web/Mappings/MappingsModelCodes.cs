using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.DAL.Data;

namespace App.BLL
{
    public class MappingsModelCodes
    {
        #region Methods

        public static void SelectModelCodes(int pageSize, int currentPageNumber, string sortExpression, Panel modelCodePanel, Button buttonFirst,
            Button buttonNext, Button buttonPrevious, Button buttonLast, Label labelTotalPages, DropDownList dropDownListPage,
            GridView gridviewModelCodes, Label labelTotalRecords, UserControl emptyDataTemplate, string country)
        {

            //Initialise Connection
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.Portal_ModelCodeSelect, con);
            
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
            List<ModelCodes> results = new List<ModelCodes>();
            int rowCount = 0;
            using (con)
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    results.Add(new ModelCodes(reader));
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
                //Show model codes panel
                modelCodePanel.Visible = true;
                //Hide Empty Data Template
                emptyDataTemplate.Visible = false;

                //Databind the gridview
                gridviewModelCodes.DataSource = results;
                gridviewModelCodes.DataBind();

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
                //Hide model code panel
                modelCodePanel.Visible = false;
                //Show Empty Data Template
                emptyDataTemplate.Visible = true;
            }


        }


        public static int InsertModelCode(string country, string model, bool active)
        {
            //Initialise Connection
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.Portal_ModelCodeInsert, con);
            
            //Set Parameters
            cmd.Parameters.AddWithValue("@country", country);
            cmd.Parameters.AddWithValue("@model", model);
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

        public static int UpdateModelCode(int model_id, string country, string model, bool active)
        {
            //Initialise Connection
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.Portal_ModelCodeUpdate, con);
            
            //Set Parameters
            cmd.Parameters.AddWithValue("@model_id", model_id);
            cmd.Parameters.AddWithValue("@country", country);
            cmd.Parameters.AddWithValue("@model", model);
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

        public static int DeleteModelCode(int model_id)
        {
            //Initialise Connection
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.Portal_ModelCodeDelete, con);
            
            //Set Parameters
            cmd.Parameters.AddWithValue("@model_id", model_id);

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

        public static List<ModelCodes> SelectModelCodeById(int model_id)
        {
            //Initialise Connection
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.Portal_ModelCodeSelectById, con);

            //Set Parameters
            cmd.Parameters.AddWithValue("@model_id", model_id);

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


        #endregion

        #region Classes

        public class ModelCodes
        {

            #region Delcarations
            private int _model_id;
            private string _country;
            private string _model;
            private bool _active;

            #endregion

            #region Properties
            public int Model_Id
            {
                get { return _model_id; }
            }

            public string Country
            {
                get { return _country; }
            }

            public string Model
            {
                get { return _model; }
            }

            public bool Active
            {
                get { return _active; }
            }

            #endregion

            #region Constructors

            public ModelCodes(SqlDataReader reader)
            {
                if (reader["model_id"] != DBNull.Value)
                {
                    _model_id = Convert.ToInt32(reader["model_id"]);
                }
                if (reader["country"] != DBNull.Value)
                {
                    _country = Convert.ToString(reader["country"]);
                }
                if (reader["model"] != DBNull.Value)
                {
                    _model = Convert.ToString(reader["model"]);
                }
                if (reader["active"] != DBNull.Value)
                {
                    _active = Convert.ToBoolean(reader["active"]);
                }

            }

            #endregion

        }


        #endregion
    }
}