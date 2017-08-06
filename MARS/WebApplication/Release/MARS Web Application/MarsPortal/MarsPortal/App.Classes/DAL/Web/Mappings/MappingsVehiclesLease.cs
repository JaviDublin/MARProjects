using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.DAL.Data;

namespace App.BLL
{
    public class MappingsVehiclesLease
    {
        #region Methods

        public static void SelectVehiclesLease(int pageSize, int currentPageNumber, string sortExpression, Panel VehiclesLeasePanel, Button buttonFirst,
                                        Button buttonNext, Button buttonPrevious, Button buttonLast, Label labelTotalPages, DropDownList dropDownListPage,
                                        GridView gridviewVehiclesLease, Label labelTotalRecords, UserControl emptyDataTemplate, string country_owner,
                                        string country_rent,string start_date,string model_description)
        {


            //Initialise Connection
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.VehiclesLease_Select_GridView, con);
            
            if (country_owner == "-1" || country_owner == null)
            {
                cmd.Parameters.AddWithValue("@country_owner", System.DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@country_owner", country_owner);
            }

            if (country_rent == "-1" || country_rent == null)
            {
                cmd.Parameters.AddWithValue("@country_rent", System.DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@country_rent", country_rent);
            }

            if (start_date == "-1" || start_date == null)
            {
                cmd.Parameters.AddWithValue("@start_date", System.DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@start_date", Convert.ToDateTime(start_date));
            }

            if (model_description == "-1" || model_description == null)
            {
                cmd.Parameters.AddWithValue("@model_description", System.DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@model_description", model_description);
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
            List<MappingsVehiclesLease.VehiclesLeaseList> results = new List<MappingsVehiclesLease.VehiclesLeaseList>();
            int rowCount = 0;
            using (con)
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    results.Add(new MappingsVehiclesLease.VehiclesLeaseList(reader));
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
                //Show Vehicles Lease panel
                VehiclesLeasePanel.Visible = true;
                //Hide Empty Data Template
                emptyDataTemplate.Visible = false;

                //Databind the gridview
                gridviewVehiclesLease.DataSource = results;
                gridviewVehiclesLease.DataBind();

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
                //Hide Vehicles Lease panel
                VehiclesLeasePanel.Visible = false;
                //Show Empty Data Template
                emptyDataTemplate.Visible = true;
            }
        }

        public static int CheckIfVehicleLeaseExists(string serial)
        {
            //Initialise Connection
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.VehiclesLease_Select_CheckExist, con);
            
            //Set Parameters
            cmd.Parameters.AddWithValue("@serial", serial);

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


        public static int InsertVehicleLease(string serial, string country_owner, string country_rent, string start_date)
        {

            //Initialise Connection
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.VehiclesLease_Insert, con);
            
            //Set Parameters
            cmd.Parameters.AddWithValue("@serial", serial);
            cmd.Parameters.AddWithValue("@country_owner", country_owner);
            cmd.Parameters.AddWithValue("@country_rent", country_rent);
            cmd.Parameters.AddWithValue("@start_date", Convert.ToDateTime(start_date));

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


        public static int DeleteVehicleLease(string serial)
        {

            //Initialise Connection
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.VehiclesLease_Delete, con);
            
            //Set parameters
            cmd.Parameters.AddWithValue("@serial", serial);

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


        public static int UpdateVehicleLease(string serial, string country_owner, string country_rent, string start_date)
        {
            //Initialise Connection
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.VehiclesLease_Update, con);

            //Set Parameters
            cmd.Parameters.AddWithValue("@serial", serial);
            cmd.Parameters.AddWithValue("@country_owner", country_owner);
            cmd.Parameters.AddWithValue("@country_rent", country_rent);
            cmd.Parameters.AddWithValue("@start_date", Convert.ToDateTime(start_date));

            //Execute Command
            using (con)
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            con.Close();
            return 0;
        }

        public static List<VehiclesLeaseList> SelectVehiclesLeaseBySerial(string serial)
        {
            //Initialise Connection
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.VehiclesLease_Select_BySerial, con);
            
            //Set Parameters
            cmd.Parameters.AddWithValue("@serial", serial);

            //Execute Command
            List<VehiclesLeaseList> results = new List<VehiclesLeaseList>();
            using (con)
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    results.Add(new VehiclesLeaseList(reader));
                }
            }
            con.Close();
            return results;
        }

        public static List<FleetSerials> SelectFleetSerialsByCountry(string country)
        {
            //Initialise Connection
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.FleetSerial_Select_ByCountry, con);
            
            //Set Parameters
            cmd.Parameters.AddWithValue("@country", country);

            //Execute Command
            List<FleetSerials> results = new List<FleetSerials>();
            using (con)
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    results.Add(new FleetSerials(reader));
                }
            }
            con.Close();
            return results;        
        }

        public static List<FleetModels> SelectModelsDescription(string country_owner,string country_rent,string start_date)
        {
            //Initialise command
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.VehiclesLease_Select_ModelDescription, con);
                       

            if (country_owner == "-1" || country_owner == null)
            {
                cmd.Parameters.AddWithValue("@country_owner", System.DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@country_owner", country_owner);
            }

            if (country_rent == "-1" || country_rent == null)
            {
                cmd.Parameters.AddWithValue("@country_rent", System.DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@country_rent", country_rent);
            }

            cmd.Parameters.AddWithValue("@start_date", System.DBNull.Value);
            //if (start_date == "-1" || start_date == null)
            //{
            //    cmd.Parameters.AddWithValue("@start_date", System.DBNull.Value);
            //}
            //else
            //{
            //    cmd.Parameters.AddWithValue("@start_date", Convert.ToDateTime(start_date));
            //}

            //Execute Command
            List<FleetModels> results = new List<FleetModels>();
            using (con)
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    results.Add(new FleetModels(reader));
                }
            }

            con.Close();
            return results;

        }

        #endregion

        #region Classes

        public class VehiclesLeaseList
        {
            #region Declarations

            private string _serial;
            private string _plate;
            private string _unit;
            private string _modelDescription;
            private string _country_owner;
            private string _country_rent;
            private DateTime? _startDate;
            
            #endregion

            #region Properties

            public string Serial
            {
                get { return _serial; }
            }

            public string Plate
            {
                get { return _plate; }
            }

            public string Unit
            {
                get { return _unit; }
            }

            public string ModelDescription
            {
                get { return _modelDescription; }
            }

            public string Country_Owner
            {
                get { return _country_owner; }
            }

            public string Country_Rent
            {
                get { return _country_rent; }
            }

            public string StartDate
            {
                get { return (_startDate != null) ? string.Format("{0:dd/MM/yyyy HH:mm:ss}", _startDate) : ""; }
            }

            #endregion

            #region Constructors
            
            public VehiclesLeaseList(SqlDataReader reader)
            {
                if (reader["Serial"] != DBNull.Value)
                {
                    _serial = Convert.ToString(reader["Serial"]);
                }
                if (reader["Plate"] != DBNull.Value)
                {
                    _plate = Convert.ToString(reader["Plate"]);
                }
                if (reader["Unit"] != DBNull.Value)
                {
                    _unit = Convert.ToString(reader["Unit"]);
                }
                if (reader["ModelDescription"] != DBNull.Value)
                {
                    _modelDescription = Convert.ToString(reader["ModelDescription"]);
                }
                if (reader["Country_Owner"] != DBNull.Value)
                {
                    _country_owner = Convert.ToString(reader["Country_Owner"]);
                }
                if (reader["Country_Rent"] != DBNull.Value)
                {
                    _country_rent = Convert.ToString(reader["Country_Rent"]);
                }
                if (reader["StartDate"] != DBNull.Value)
                {
                    _startDate = Convert.ToDateTime(reader["StartDate"]);
                }
                
            }

            #endregion
        }

        public class FleetSerials
        {
            #region Declarations

            private string _serial;
            
            #endregion

            #region Properties

            public string Serial
            {
                get { return _serial; }
            }

            #endregion

            #region Constructors

            public FleetSerials(SqlDataReader reader)
            {
                if (reader["Serial"] != DBNull.Value)
                {
                    _serial = Convert.ToString(reader["Serial"]);
                }
            }

            #endregion
        
        }

        public class FleetModels
        { 
            #region Declarations

            private string _model_description;
            
            #endregion

            #region Properties

            public string ModelDescription
            {
                get { return _model_description; }
            }

            #endregion

            #region Constructors

            public FleetModels(SqlDataReader reader)
            {
                if (reader["ModelDescription"] != DBNull.Value)
                {
                    _model_description = Convert.ToString(reader["ModelDescription"]);
                }
            }

            #endregion
        }

        #endregion

    }
}