using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.DAL.Data;
using App.AvailabilityTool.CarSearch.Filters; //--added--

namespace App.BLL 
{
    public class AvailabilityCarSearch {
        // altered: 2/4/12 by Gavin
        // alterations : At line around 180, before the GridView databind, further filtering added for MARSV3
        // line 402: method add to return list

        #region Properties
        public static CarFilter TheCarFilter { get; set; }
        #endregion

        #region Enums

        public enum SortDirection : int 
        {
            Ascending = 1,
            Descending = 2
        };


        #endregion

        #region Methods

        public static void GetCarSearchResult(string country, int cms_pool_id, int cms_location_group_id, int ops_region_id, int ops_area_id,
                                                string location, string fleet_name, int car_segment_id, int car_class_id, int car_group_id,
                                                       string operstat_name, string ownarea, string modelcode, int no_rev, string select_by, int pageSize, int currentPageNumber,
                                                            string sortExpression, Panel carSearchPanel, Button buttonFirst, Button buttonNext, Button buttonPrevious,
                                                                Button buttonLast, Label labelTotalPages, DropDownList dropDownListPage, GridView gridviewCarSearch,
                                                                    Label labelTotalRecords, UserControl emptyDataTemplate) {

            pageSize = pageSize == 0 ? 10 : pageSize;
            currentPageNumber = currentPageNumber == 0 ? 1 : currentPageNumber;

            //Initialise command
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.Availability_ReportCarSearch, con);
            
            //Set Parameters
            if (country == "-1") {
                cmd.Parameters.AddWithValue("@country", System.DBNull.Value);
            }
            else 
            {
                cmd.Parameters.AddWithValue("@country", country);
            }

            if (cms_pool_id == -1) 
            {
                cmd.Parameters.AddWithValue("@cms_pool_id", DBNull.Value);
            }
            else {
                cmd.Parameters.AddWithValue("@cms_pool_id", cms_pool_id);
            }

            if (cms_location_group_id == -1) {
                cmd.Parameters.AddWithValue("@cms_location_group_id", System.DBNull.Value);
            }
            else {
                cmd.Parameters.AddWithValue("@cms_location_group_id", cms_location_group_id);
            }

            if (ops_region_id == -1) {
                cmd.Parameters.AddWithValue("@ops_region_id", System.DBNull.Value);
            }
            else {
                cmd.Parameters.AddWithValue("@ops_region_id", ops_region_id);
            }

            if (ops_area_id == -1) {
                cmd.Parameters.AddWithValue("@ops_area_id", System.DBNull.Value);
            }
            else {
                cmd.Parameters.AddWithValue("@ops_area_id", ops_area_id);
            }

            if (location == "-1") {
                cmd.Parameters.AddWithValue("@location", System.DBNull.Value);
            }
            else {
                cmd.Parameters.AddWithValue("@location", location);
            }

            if (fleet_name == "-1") {
                cmd.Parameters.AddWithValue("@fleet_name", System.DBNull.Value);
            }
            else {
                cmd.Parameters.AddWithValue("@fleet_name", fleet_name);
            }

            if (car_segment_id == -1) {
                cmd.Parameters.AddWithValue("@car_segment_id", System.DBNull.Value);
            }
            else {
                cmd.Parameters.AddWithValue("@car_segment_id", car_segment_id);
            }

            if (car_class_id == -1) {
                cmd.Parameters.AddWithValue("@car_class_id", System.DBNull.Value);
            }
            else {
                cmd.Parameters.AddWithValue("@car_class_id", car_class_id);
            }

            if (car_group_id == -1) {
                cmd.Parameters.AddWithValue("@car_group_id", System.DBNull.Value);
            }
            else {
                cmd.Parameters.AddWithValue("@car_group_id", car_group_id);
            }

            if (operstat_name == string.Empty || operstat_name == null) {
                cmd.Parameters.AddWithValue("@operstat_name", System.DBNull.Value);
            }
            else {
                cmd.Parameters.AddWithValue("@operstat_name", operstat_name);
            }

            if (ownarea == string.Empty || ownarea == null) {
                cmd.Parameters.AddWithValue("@ownarea", System.DBNull.Value);
            }
            else {
                cmd.Parameters.AddWithValue("@ownarea", ownarea);
            }

            if (modelcode == string.Empty || modelcode == null) {
                cmd.Parameters.AddWithValue("@modelcode", System.DBNull.Value);
            }
            else {
                cmd.Parameters.AddWithValue("@modelcode", modelcode);
            }


            if (no_rev == -1) {
                cmd.Parameters.AddWithValue("@no_rev", System.DBNull.Value);
            }
            else {
                cmd.Parameters.AddWithValue("@no_rev", no_rev);
            }


            //---------------------------------------------------------------------------------
            if (TheCarFilter != null) { // Added by Gavin 2/4/12 to handle extra filtering
                cmd.Parameters.AddWithValue("@license", TheCarFilter.TextBoxLicense.Text.Trim());
                cmd.Parameters.AddWithValue("@unit", TheCarFilter.TextBoxUnit.Text.Trim());
                cmd.Parameters.AddWithValue("@serial", TheCarFilter.TextBoxVin.Text.Trim());
                cmd.Parameters.AddWithValue("@driverName", TheCarFilter.TextBoxName.Text.Trim());
                cmd.Parameters.AddWithValue("@colour", TheCarFilter.TextBoxColour.Text.Trim());
                int no; int.TryParse(TheCarFilter.TextBoxMiles.Text.Trim(), out no);
                cmd.Parameters.AddWithValue("@mileage", no);
                cmd.Parameters.AddWithValue("@model", TheCarFilter.TextBoxModel.Text.Trim());
                cmd.Parameters.AddWithValue("@modelDesc", TheCarFilter.TextBoxModDesc.Text.Trim());
            }
            else {
                cmd.Parameters.AddWithValue("@license", "");
                cmd.Parameters.AddWithValue("@unit", "");
                cmd.Parameters.AddWithValue("@serial", "");
                cmd.Parameters.AddWithValue("@driverName", "");
                cmd.Parameters.AddWithValue("@colour", "");
                cmd.Parameters.AddWithValue("@mileage", 0);
                cmd.Parameters.AddWithValue("@model", "");
                cmd.Parameters.AddWithValue("@modelDesc", "");
            }

            cmd.Parameters.AddWithValue("@racfId", Rad.Security.ApplicationAuthentication.GetGlobalId()); // Added by Gavin 4/4/12 for MarsV3
            //---------------------------------------------------------------------------------

            cmd.Parameters.AddWithValue("@select_by", select_by);

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


            List<CarSearchDetails> results = new List<CarSearchDetails>();
            int rowCount = 0;
            using (con) {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read()) {
                    results.Add(new CarSearchDetails(reader));
                }

                reader.NextResult();
                while (reader.Read()) {
                    rowCount = Convert.ToInt32(reader["totalCount"]);
                }
            }
            con.Close();



            if (rowCount >= 1) {
                //Show reservation panel
                carSearchPanel.Visible = true;
                //Hide Empty Data Template
                emptyDataTemplate.Visible = false;

                //Databind the gridview
                gridviewCarSearch.DataSource = results;
                gridviewCarSearch.DataBind();

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
                //Hide reservation panel
                carSearchPanel.Visible = false;
                //Show Empty Data Template
                emptyDataTemplate.Visible = true;
            }


        }



        public static List<VehicleRemarks> GetVehicleRemarks(string serial) 
        {
            //Initialise command
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.Availability_VehicleRemarksSelect, con);
            
            //Set Parameters
            cmd.Parameters.AddWithValue("@serial", serial);

            //Execute Command
            List<VehicleRemarks> results = new List<VehicleRemarks>();
            using (con) {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read()) {
                    results.Add(new VehicleRemarks(reader));
                }

            }
            con.Close();
            return results;

        }


        public static void SaveVehicleRemark(string serial, string remark, DateTime next_onrent_date) 
        {
            //Initialise command
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.Availability_VehicleRemarksInsertUpdate, con);
            
            cmd.Parameters.AddWithValue("@serial", serial);

            if ((remark == string.Empty || remark == null)) {
                cmd.Parameters.AddWithValue("@remark", System.DBNull.Value);
            }
            else {
                cmd.Parameters.AddWithValue("@remark", remark);
            }

            if ((next_onrent_date == null)) {
                cmd.Parameters.AddWithValue("@next_onrent_date", System.DBNull.Value);
            }
            else {
                DateTime dt = Convert.ToDateTime(next_onrent_date);
                cmd.Parameters.AddWithValue("@next_onrent_date", dt);
            }

            using (con) {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            con.Close();


        }

        #endregion

        // ---- added by Gavin for MarsV3 to return a list of the Search for downloading ----
        public static List<CarSearchDetails> GetCarSearchList(string country, int cms_pool_id, int cms_location_group_id, int ops_region_id, int ops_area_id,
                                                string location, string fleet_name, int car_segment_id, int car_class_id, int car_group_id,
                                                       string operstat_name, string ownarea, string modelcode, int no_rev, string select_by, int pageSize, int currentPageNumber,
                                                            string sortExpression, Panel carSearchPanel, Button buttonFirst, Button buttonNext, Button buttonPrevious,
                                                                Button buttonLast, Label labelTotalPages, DropDownList dropDownListPage, GridView gridviewCarSearch,
                                                                    Label labelTotalRecords, UserControl emptyDataTemplate) {
            List<CarSearchDetails> results = new List<CarSearchDetails>();

            pageSize = pageSize == 0 ? 10 : pageSize;
            currentPageNumber = currentPageNumber == 0 ? 1 : currentPageNumber;

            //Initialise command
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.Availability_ReportCarSearch, con);

            //Set Parameters
            if (country == "-1") {
                cmd.Parameters.AddWithValue("@country", System.DBNull.Value);
            }
            else {
                cmd.Parameters.AddWithValue("@country", country);
            }

            if (cms_pool_id == -1) {
                cmd.Parameters.AddWithValue("@cms_pool_id", System.DBNull.Value);
            }
            else {
                cmd.Parameters.AddWithValue("@cms_pool_id", cms_pool_id);
            }

            if (cms_location_group_id == -1) {
                cmd.Parameters.AddWithValue("@cms_location_group_id", System.DBNull.Value);
            }
            else {
                cmd.Parameters.AddWithValue("@cms_location_group_id", cms_location_group_id);
            }

            if (ops_region_id == -1) {
                cmd.Parameters.AddWithValue("@ops_region_id", System.DBNull.Value);
            }
            else {
                cmd.Parameters.AddWithValue("@ops_region_id", ops_region_id);
            }

            if (ops_area_id == -1) {
                cmd.Parameters.AddWithValue("@ops_area_id", System.DBNull.Value);
            }
            else {
                cmd.Parameters.AddWithValue("@ops_area_id", ops_area_id);
            }

            if (location == "-1") {
                cmd.Parameters.AddWithValue("@location", System.DBNull.Value);
            }
            else {
                cmd.Parameters.AddWithValue("@location", location);
            }

            if (fleet_name == "-1") {
                cmd.Parameters.AddWithValue("@fleet_name", System.DBNull.Value);
            }
            else {
                cmd.Parameters.AddWithValue("@fleet_name", fleet_name);
            }

            if (car_segment_id == -1) {
                cmd.Parameters.AddWithValue("@car_segment_id", System.DBNull.Value);
            }
            else {
                cmd.Parameters.AddWithValue("@car_segment_id", car_segment_id);
            }

            if (car_class_id == -1) {
                cmd.Parameters.AddWithValue("@car_class_id", System.DBNull.Value);
            }
            else {
                cmd.Parameters.AddWithValue("@car_class_id", car_class_id);
            }

            if (car_group_id == -1) {
                cmd.Parameters.AddWithValue("@car_group_id", System.DBNull.Value);
            }
            else {
                cmd.Parameters.AddWithValue("@car_group_id", car_group_id);
            }

            if (operstat_name == string.Empty || operstat_name == null) {
                cmd.Parameters.AddWithValue("@operstat_name", System.DBNull.Value);
            }
            else {
                cmd.Parameters.AddWithValue("@operstat_name", operstat_name);
            }

            if (ownarea == string.Empty || ownarea == null) {
                cmd.Parameters.AddWithValue("@ownarea", System.DBNull.Value);
            }
            else {
                cmd.Parameters.AddWithValue("@ownarea", ownarea);
            }

            if (modelcode == string.Empty || modelcode == null) {
                cmd.Parameters.AddWithValue("@modelcode", System.DBNull.Value);
            }
            else {
                cmd.Parameters.AddWithValue("@modelcode", modelcode);
            }


            if (no_rev == -1) {
                cmd.Parameters.AddWithValue("@no_rev", System.DBNull.Value);
            }
            else {
                cmd.Parameters.AddWithValue("@no_rev", no_rev);
            }


            //---------------------------------------------------------------------------------
            if (TheCarFilter != null) { // Added by Gavin 2/4/12 to handle extra filtering
                cmd.Parameters.AddWithValue("@license", TheCarFilter.TextBoxLicense.Text.Trim());
                cmd.Parameters.AddWithValue("@unit", TheCarFilter.TextBoxUnit.Text.Trim());
                cmd.Parameters.AddWithValue("@serial", TheCarFilter.TextBoxVin.Text.Trim());
                cmd.Parameters.AddWithValue("@driverName", TheCarFilter.TextBoxName.Text.Trim());
                cmd.Parameters.AddWithValue("@colour", TheCarFilter.TextBoxColour.Text.Trim());
                int no; int.TryParse(TheCarFilter.TextBoxMiles.Text.Trim(), out no);
                cmd.Parameters.AddWithValue("@mileage", no);
                cmd.Parameters.AddWithValue("@model", TheCarFilter.TextBoxModel.Text.Trim());
            }
            else {
                cmd.Parameters.AddWithValue("@license", "");
                cmd.Parameters.AddWithValue("@unit", "");
                cmd.Parameters.AddWithValue("@serial", "");
                cmd.Parameters.AddWithValue("@driverName", "");
                cmd.Parameters.AddWithValue("@colour", "");
                cmd.Parameters.AddWithValue("@mileage", 0);
                cmd.Parameters.AddWithValue("@model", "");
            }

            cmd.Parameters.AddWithValue("@racfId", Rad.Security.ApplicationAuthentication.GetGlobalId());
            //---------------------------------------------------------------------------------

            cmd.Parameters.AddWithValue("@select_by", select_by);

            int startRowIndex = 1;

            App.UserControls.Details.CarSearchWorker csw = new App.UserControls.Details.CarSearchWorker(Rad.Security.ApplicationAuthentication.GetGlobalId());
            int maximumRows = Convert.ToInt32(csw.getRowCount());


            cmd.Parameters.AddWithValue("@startRowIndex", startRowIndex);
            cmd.Parameters.AddWithValue("@maximumRows", maximumRows);

            if (sortExpression == null) {
                cmd.Parameters.AddWithValue("@sortExpression", System.DBNull.Value);
            }
            else {
                cmd.Parameters.AddWithValue("@sortExpression", sortExpression);
            }

            int rowCount = 0;
            using (con) {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read()) {
                    results.Add(new CarSearchDetails(reader));
                }

                reader.NextResult();
                while (reader.Read()) {
                    rowCount = Convert.ToInt32(reader["totalCount"]);
                }
            }
            con.Close();
            return results;
        }


        #region Classes

        public class VehicleRemarks 
        {

            #region Declarations

            private string _remark;
            private DateTime? _nextOnRentdate;
            private string _group;
            private string _modelCode;
            private string _model;
            private string _ownarea;
            private string _driverName;
            private DateTime? _blockDate;
            private string _license;
            private string _unit;
            private string _vehicleIdentNbr;
            private int _kilometers;
            private DateTime? _regDate;
            private string _lstwwd;
            private DateTime? _lstdate;
            private string _duewwd;
            private DateTime? _duedate;
            private string _movetype;
            private string _lastDocument;
            private string _operstat;
            private int _nonRev;
            private int _bdDays;
            private string _prevwwd;
            private string _mmDays;
            private string _groupCharged;
            private DateTime? _lsttime;
            private DateTime? _duetime;
            private string _carhold;


            #endregion

            #region Properties
            public string Remark {
                get { return _remark; }
            }

            public string NextOnRentDate {
                get { return (_nextOnRentdate != null) ? string.Format("{0:dd/MM/yyyy}", _nextOnRentdate) : null; }
            }

            public string Group {
                get { return _group; }
            }
            public string ModelCode {
                get { return _modelCode; }
            }
            public string Model {
                get { return _model; }
            }

            public string OwnArea {
                get { return _ownarea; }
            }
            public string DriverName {
                get { return _driverName; }
            }
            public string BlockDate {
                get { return (_blockDate != null) ? string.Format("{0:dd/MM/yyyy}", _blockDate) : null; }
            }
            public string License {
                get { return _license; }
            }
            public string Unit {
                get { return _unit; }
            }
            public string VehicleIdentNbr {
                get { return _vehicleIdentNbr; }
            }
            public string Kilometers {
                get { return _kilometers.ToString(); }
            }
            public string RegDate {
                get { return (_regDate != null) ? string.Format("{0:dd/MM/yyyy}", _regDate) : null; }
            }
            public string Lstwwd {
                get { return _lstwwd; }
            }
            public string LstDate {
                get { return (_lstdate != null) ? string.Format("{0:dd/MM/yyyy}", _lstdate) : null; }
            }
            public string Duewwd {
                get { return _duewwd; }
            }
            public string DueDate {
                get { return (_duedate != null) ? string.Format("{0:dd/MM/yyyy}", _duedate) : null; }
            }
            public string Movetype {
                get { return _movetype; }
            }
            public string lastDocument {
                get { return _lastDocument; }
            }
            public string Operstat {
                get { return _operstat; }
            }
            public string NonRev {
                get { return _nonRev.ToString(); }
            }
            public string BDDays {
                get { return _bdDays.ToString(); }
            }
            public string Prevwwd {
                get { return _prevwwd; }
            }
            public string MMDays {
                get { return _mmDays; }
            }
            public string GroupCharged {
                get { return _groupCharged; }
            }
            public string Lsttime {
                get { return (_lsttime != null) ? string.Format("{0:HH\\:mm}", _lsttime) : null; }
            }
            public string duetime {
                get { return (_duetime != null) ? string.Format("{0:HH\\:mm}", _duetime) : null; }
            }
            public string carhold {
                get { return _carhold; }
            }


            #endregion

            #region Constructors

            public VehicleRemarks(SqlDataReader reader) {

                if (reader["remark"] != DBNull.Value)

                    _remark = Convert.ToString(reader["remark"]);

                if (reader["nextOnRentDate"] != DBNull.Value)

                    _nextOnRentdate = Convert.ToDateTime(reader["nextOnRentDate"]);


                if (reader["group"] != DBNull.Value)

                    _group = Convert.ToString(reader["group"]);

                if (reader["modelcode"] != DBNull.Value)

                    _modelCode = Convert.ToString(reader["modelcode"]);

                if (reader["model"] != DBNull.Value)

                    _model = Convert.ToString(reader["model"]);

                if (reader["ownarea"] != DBNull.Value)

                    _ownarea = Convert.ToString(reader["ownarea"]);

                if (reader["driverName"] != DBNull.Value)

                    _driverName = Convert.ToString(reader["driverName"]);

                if (reader["blockDate"] != DBNull.Value)

                    _blockDate = Convert.ToDateTime(reader["blockDate"]);



                if (reader["license"] != DBNull.Value)

                    _license = Convert.ToString(reader["license"]);

                if (reader["unit"] != DBNull.Value)

                    _unit = Convert.ToString(reader["unit"]);

                if (reader["vehicleIdentNbr"] != DBNull.Value)

                    _vehicleIdentNbr = Convert.ToString(reader["vehicleIdentNbr"]);

                if (reader["kilometers"] != DBNull.Value)

                    _kilometers = Convert.ToInt32(reader["kilometers"]);

                if (reader["regDate"] != DBNull.Value)

                    _regDate = Convert.ToDateTime(reader["regDate"]);


                if (reader["lstwwd"] != DBNull.Value)

                    _lstwwd = Convert.ToString(reader["lstwwd"]);

                if (reader["lstDate"] != DBNull.Value)

                    _lstdate = Convert.ToDateTime(reader["lstDate"]);

                if (reader["duewwd"] != DBNull.Value)

                    _duewwd = Convert.ToString(reader["duewwd"]);

                if (reader["duedate"] != DBNull.Value)

                    _duedate = Convert.ToDateTime(reader["duedate"]);

                if (reader["movetype"] != DBNull.Value)

                    _movetype = Convert.ToString(reader["movetype"]);

                if (reader["lastDocument"] != DBNull.Value)

                    _lastDocument = Convert.ToString(reader["lastDocument"]);

                if (reader["operstat"] != DBNull.Value)

                    _operstat = Convert.ToString(reader["operstat"]);

                if (reader["nonRev"] != DBNull.Value)

                    _nonRev = Convert.ToInt32(reader["nonRev"]);

                if (reader["bdDays"] != DBNull.Value)

                    _bdDays = Convert.ToInt32(reader["bdDays"]);

                if (reader["prevwwd"] != DBNull.Value)

                    _prevwwd = Convert.ToString(reader["prevwwd"]);

                if (reader["mmDays"] != DBNull.Value)

                    _mmDays = Convert.ToString(reader["mmDays"]);

                if (reader["groupCharged"] != DBNull.Value)

                    _groupCharged = Convert.ToString(reader["groupCharged"]);

                if (reader["lsttime"] != DBNull.Value)

                    _lsttime = Convert.ToDateTime(reader["lsttime"]);



                if (reader["duetime"] != DBNull.Value)

                    _duetime = Convert.ToDateTime(reader["duetime"]);


                if (reader["carhold"] != DBNull.Value)

                    _carhold = Convert.ToString(reader["carhold"]);



            }

            #endregion


        }

        public class CarSearchDetails 
        {
            #region Declarations

            private string _vc;
            private string _unit;
            private string _license;
            private string _model;
            private string _lstwwd;
            private DateTime? _lstdate;
            private string _duewwd;
            private DateTime? _duedate;
            private DateTime? _duetime;
            private string _op;
            private string _mt;
            private string _hold;
            private string _nr;
            private string _driver;
            private string _doc;
            private string _lstmlg;
            private string _serial;
            private string _remark;
            private string _erdpassed;


            #endregion

            #region Properties

            public string VC {
                get { return _vc; }
            }
            public string Unit {
                get { return _unit; }
            }
            public string License {
                get { return _license; }
            }
            public string Model {
                get { return _model; }
            }
            public string LSTWWD {
                get { return _lstwwd; }
            }
            public string LSTDate {

                get { return (_lstdate != null) ? string.Format("{0:dd/MM/yyyy}", _lstdate) : null; }

            }
            public string DUEWWD {
                get { return _duewwd; }
            }
            public string DUEDate {
                get { return (_duedate != null) ? string.Format("{0:dd/MM/yyyy}", _duedate) : null; }
            }
            public string DUETime {
                get { return (_duetime != null) ? string.Format("{0:HH\\:mm}", _duetime) : null; }
            }
            public string OP {
                get { return _op; }
            }
            public string MT {
                get { return _mt; }
            }
            public string HOLD {
                get { return _hold; }
            }
            public string NR {
                get { return _nr; }
            }
            public string Driver {
                get { return _driver; }
            }
            public string DOC {
                get { return _doc; }
            }
            public string LSTMLG {
                get { return _lstmlg; }
            }
            public string SERIAL {
                get { return _serial; }
            }
            public string Remark {
                get { return _remark; }
            }
            public string ERDPassed {
                get { return _erdpassed; }
            }
            // --Altered by Gavin --
            public string Modeldesc { get; private set; }
            public string Rc { get; private set; }
            public string LstTime { get; private set; }
            public string iDate { get; private set; }
            public string msoDate { get; private set; }
            public string capDate { get; private set; }
            public string ownArea { get; private set; }
            public string carHold1 { get; private set; }
            public string bdDays { get; private set; }
            public string mmDays { get; private set; }
            public string Resolution { get; private set; }

            #endregion

            #region Constructor

            public CarSearchDetails(SqlDataReader reader) {
                if (reader["vc"] != DBNull.Value)
                    _vc = Convert.ToString(reader["vc"]);

                if (reader["unit"] != DBNull.Value)
                    _unit = Convert.ToString(reader["unit"]);

                if (reader["license"] != DBNull.Value)
                    _license = Convert.ToString(reader["license"]);

                if (reader["model"] != DBNull.Value)
                    _model = Convert.ToString(reader["model"]);

                if (reader["lstwwd"] != DBNull.Value)
                    _lstwwd = Convert.ToString(reader["lstwwd"]);

                if (reader["lstdate"] != DBNull.Value)
                    _lstdate = Convert.ToDateTime(reader["lstdate"]);

                if (reader["duewwd"] != DBNull.Value)
                    _duewwd = Convert.ToString(reader["duewwd"]);

                if (reader["duedate"] != DBNull.Value)
                    _duedate = Convert.ToDateTime(reader["duedate"]);

                if (reader["duetime"] != DBNull.Value)
                    _duetime = Convert.ToDateTime(reader["duetime"]);

                if (reader["op"] != DBNull.Value)
                    _op = Convert.ToString(reader["op"]);

                if (reader["mt"] != DBNull.Value)
                    _mt = Convert.ToString(reader["mt"]);

                if (reader["hold"] != DBNull.Value)
                    _hold = Convert.ToString(reader["hold"]);

                if (reader["nr"] != DBNull.Value)
                    _nr = Convert.ToString(reader["nr"]);

                if (reader["driver"] != DBNull.Value)
                    _driver = Convert.ToString(reader["driver"]);

                if (reader["doc"] != DBNull.Value)
                    _doc = Convert.ToString(reader["doc"]);

                if (reader["lstmlg"] != DBNull.Value)
                    _lstmlg = Convert.ToString(reader["lstmlg"]);

                if (reader["serial"] != DBNull.Value)
                    _serial = Convert.ToString(reader["serial"]);

                if (reader["remark"] != DBNull.Value)
                    _remark = Convert.ToString(reader["remark"]);

                if (reader["erdpassed"] != DBNull.Value)
                    _erdpassed = Convert.ToString(reader["erdpassed"]);

                // added by gavin
                Modeldesc = reader["moddesc"].ToString() ?? "";
                Rc = reader["rc"].ToString() ?? "";
                LstTime = convert2Time(reader["lsttime"].ToString());
                iDate = getShortDate(reader["idate"].ToString());
                msoDate = getShortDate(reader["msodate"].ToString());
                capDate = getShortDate(reader["capdate"].ToString());
                ownArea = reader["ownarea"].ToString() ?? "";
                carHold1 = reader["carhold1"].ToString() ?? "";
                bdDays = reader["bddays"].ToString() ?? "";
                mmDays = reader["mmdays"].ToString() ?? "";
                Resolution = getShortDate(reader["resolution"].ToString());
            }
            #endregion

        #endregion

        #region worker methods
        private string convert2Time(string str) { // converts a string to time by check if its a date and taking the time out
            DateTime dt;
            if (DateTime.TryParse(str, out dt)) return dt.ToShortTimeString(); // succesful DateTime
            return "";
        }
        private string getShortDate(string str) 
        { // gets a short date from a string
            DateTime dt;
            if (DateTime.TryParse(str, out dt)) return dt.ToShortDateString(); // succesful DateTime
            return "";
        }
        #endregion
        }
    }
}
