using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using App.DAL.Data;

namespace App.BLL
{
    public class ReportStatistics
    {

        #region "Enums"

        public enum ReportName : int
        {
            PoolingAlerts = 1,
            PoolingStatus = 2,
            PoolingSiteComaprison = 3,
            PoolingFleetComparison = 4,
            PoolingReservations = 5,
            AvailabilityCarSearch = 6,
            AvailabilitySiteComparison = 7,
            AvailabilityFleetComparison = 8,
            AvailabilityFleetStatus = 9,
            AvailabilityHistoricalTrend = 10,
            AvailabilityKPI = 11,
            AvailabilityDownload = 12,
            NonRevCarSearch = 13

        }

        public enum StatisticsType : int
        {
            BySelection = 1,
            ByDate = 2
        }

        public enum SortDirection : int
        {
            Ascending = 1,
            Descending = 2
        }
        #endregion

        #region "Functions"

        public static void InsertStatistics(int reportId, string country, int cms_pool_id, int cms_location_group_id, int ops_region_id, int ops_area_id, string location, string racfId, DateTime reportDate)
        {

            //Initialise command
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.Mars_UsageStatisticsInsert, con);
            
            //Set Parameters
            cmd.Parameters.AddWithValue("@reportId", reportId);
            if ((cms_pool_id == -1))
            {
                cmd.Parameters.AddWithValue("@cms_pool_Id", System.DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@cms_pool_Id", cms_pool_id);
            }
            if ((cms_location_group_id == -1))
            {
                cmd.Parameters.AddWithValue("@cms_location_group_id", System.DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@cms_location_group_id", cms_location_group_id);
            }
            if ((ops_region_id == -1))
            {
                cmd.Parameters.AddWithValue("@ops_region_Id", System.DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@ops_region_Id", ops_region_id);
            }
            if ((ops_area_id == -1))
            {
                cmd.Parameters.AddWithValue("@ops_area_Id", System.DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@ops_area_Id", ops_area_id);
            }
            if ((country == "-1") || (country == null))
            {
                cmd.Parameters.AddWithValue("@country", System.DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@country", country);
            }
            if ((location == "-1") || (country == null))
            {
                cmd.Parameters.AddWithValue("@location", System.DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@location", location);
            }
            if (racfId == null)
            {
                cmd.Parameters.AddWithValue("racfId", System.DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("racfId", racfId);
            }
            cmd.Parameters.AddWithValue("@report_date", reportDate);

            int result = 0;
            using (con)
            {
                con.Open();
                result = Convert.ToInt32(cmd.ExecuteScalar());
            }
            con.Close();

        }

        public static void SelectStatistics(int marsTool, int optionLogic, string country, int cms_pool_id, int cms_location_group_id, string racfId, int ops_region_id, int ops_area_id, string location, DateTime start_date,
        DateTime end_date, int pageSize, int currentPageNumber, string sortExpression, Button buttonFirst, Button buttonNext, Button buttonPrevious, Button buttonLast, Label labelTotalPages, DropDownList dropDownListPage,
        GridView gridviewStatistics, GridView gridviewStatisticsTotals, Label labelTotalRecords, int selectionType, Panel panelResults, Panel panelEmptyData)
        {


            //Initialise command
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = null;

            switch (selectionType)
            {
                case (int)StatisticsType.BySelection:

                    cmd = DBManager.CreateProcedure(StoredProcedures.MARS_UsageStatisticsBySelection, con);
                    
                    break;
                case (int)StatisticsType.ByDate:

                    cmd = DBManager.CreateProcedure(StoredProcedures.MARS_UsageStatisticsByDate, con);
                    
                    break;
            }

            //Set parameters
            cmd.Parameters.AddWithValue("@marsTool", marsTool);
            if ((location == null) || (location == "-1"))
            {
                cmd.Parameters.AddWithValue("@logic", optionLogic);
            }
            else
            {
                cmd.Parameters.AddWithValue("@logic", System.DBNull.Value);
            }

            if (cms_pool_id == -1)
            {
                cmd.Parameters.AddWithValue("@cms_pool_Id", System.DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@cms_pool_Id", cms_pool_id);
            }
            if (cms_location_group_id == -1)
            {
                cmd.Parameters.AddWithValue("@cms_location_group_id", System.DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@cms_location_group_id", cms_location_group_id);
            }
            if (ops_region_id == -1)
            {
                cmd.Parameters.AddWithValue("@ops_region_Id", System.DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@ops_region_Id", ops_region_id);
            }
            if (ops_area_id == -1)
            {
                cmd.Parameters.AddWithValue("@ops_area_Id", System.DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@ops_area_Id", ops_area_id);
            }
            if (country == "-1")
            {
                cmd.Parameters.AddWithValue("@country", System.DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@country", country);
            }
            if (location == "-1")
            {
                cmd.Parameters.AddWithValue("@location", System.DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@location", location);
            }

            if ((racfId == string.Empty || racfId == null))
            {
                cmd.Parameters.AddWithValue("@racfid", System.DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@racfid", racfId);
            }

            start_date = start_date.Date.AddHours(0).AddMinutes(0).AddSeconds(0);
            cmd.Parameters.AddWithValue("@start_Date", start_date);
            end_date = end_date.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
            cmd.Parameters.AddWithValue("@end_Date", end_date);

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

            int rowCount = 0;

            switch (marsTool)
            {


                case (int)ReportSettings.ReportSettingsTool.Availability:
                case 4:

                    var results = new List<ReportStatisticsAvailability>();
                    var resultsTotals = new List<ReportStatisticsAvailability>();

                    var poolingResults = new List<ReportStatisticsPooling>();
                    var poolingTotals = new List<ReportStatisticsPooling>();

                    using (con)
                    {
                        con.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            if (marsTool == (int)ReportSettings.ReportSettingsTool.Availability)
                            {
                                results.Add(new ReportStatisticsAvailability(reader));    
                            }
                            else
                            {
                                poolingResults.Add(new ReportStatisticsPooling(reader));    
                            }
                        }

                        reader.NextResult();

                        while (reader.Read())
                        {
                            if (marsTool == (int)ReportSettings.ReportSettingsTool.Availability)
                            {
                                resultsTotals.Add(new ReportStatisticsAvailability(reader));
                            }
                            else
                            {
                                poolingTotals.Add(new ReportStatisticsPooling(reader));
                            }
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
                        //Show Results Panel / Hide the empty data template
                        panelResults.Visible = true;
                        panelEmptyData.Visible = false;

                        //Databind each gridview
                        if (marsTool == (int)ReportSettings.ReportSettingsTool.Availability)
                        {
                            gridviewStatistics.DataSource = results;
                            gridviewStatistics.DataBind();
                            gridviewStatisticsTotals.DataSource = resultsTotals;
                            gridviewStatisticsTotals.DataBind();

                        }
                        else
                        {
                            gridviewStatistics.DataSource = poolingResults;
                            gridviewStatistics.DataBind();
                            gridviewStatisticsTotals.DataSource = poolingTotals;
                            gridviewStatisticsTotals.DataBind();
                        }
                        

                        



                        //Show total records
                        int totalPages = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(rowCount) / Convert.ToDouble(pageSize)));
                        labelTotalPages.Text = totalPages.ToString();
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
                        //Hide Results Panel / Show the empty data template
                        panelResults.Visible = false;
                        panelEmptyData.Visible = true;

                    }
                    break;
            }

        }
        
        #endregion

        #region Classes

        public class ReportStatisticsAvailability
        {
            #region Properties and Fields
            private string _header;
            private int _fleetStatus;
            private int _historicalTrend;
            private int _siteComparison;
            private int _fleetComparison;
            private int _kpi;
            private int _kpiDownload;

            private int _carSearch;
            public string Header
            {
                get { return _header; }
            }

            public int FleetStatus
            {
                get { return _fleetStatus; }
            }

            public int HistoricalTrend
            {
                get { return _historicalTrend; }
            }

            public int SiteComparison
            {
                get { return _siteComparison; }
            }
            public int FleetComparison
            {
                get { return _fleetComparison; }
            }

            public int KPI
            {
                get { return _kpi; }
            }

            public int KPIDownload
            {
                get { return _kpiDownload; }
            }

            public int CarSearch
            {
                get { return _carSearch; }
            }

            #endregion

            #region Constructors

            public ReportStatisticsAvailability(SqlDataReader reader)
            {
                if (reader["header"] != DBNull.Value)
                {
                    _header = Convert.ToString(reader["header"]);
                }
                if (reader["fleetStatus"] != DBNull.Value)
                {
                    _fleetStatus = Convert.ToInt32(reader["fleetStatus"]);
                }
                if (reader["historicalTrend"] != DBNull.Value)
                {
                    _historicalTrend = Convert.ToInt32(reader["historicalTrend"]);
                }
                if (reader["siteComparison"] != DBNull.Value)
                {
                    _siteComparison = Convert.ToInt32(reader["siteComparison"]);
                }
                if (reader["fleetComparison"] != DBNull.Value)
                {
                    _fleetComparison = Convert.ToInt32(reader["fleetComparison"]);
                }
                if (reader["kpi"] != DBNull.Value)
                {
                    _kpi = Convert.ToInt32(reader["kpi"]);
                }
                if (reader["kpiDownload"] != DBNull.Value)
                {
                    _kpiDownload = Convert.ToInt32(reader["kpiDownload"]);
                }
                if (reader["carSearch"] != DBNull.Value)
                {
                    _carSearch = Convert.ToInt32(reader["carSearch"]);
                }

            }
            #endregion
        }

        public class ReportStatisticsPooling
        {
            #region Properties and Fields

            

            public string Header { get; set; }

            public int Alerts { get; set; }
            public int Status { get; set; }

            public int HistoricalTrend { get; set; }

            public int SiteComparison { get; set; }
            public int FleetComparison { get; set; }

            public int Reservations { get; set; }



            #endregion

            #region Constructors

            public ReportStatisticsPooling(SqlDataReader reader)
            {
                if (reader["header"] != DBNull.Value)
                {
                    Header = Convert.ToString(reader["header"]);
                }


                if (reader["alerts"] != DBNull.Value)
                {
                    Alerts = Convert.ToInt32(reader["alerts"]);
                }

                if (reader["status"] != DBNull.Value)
                {
                   Status = Convert.ToInt32(reader["status"]);
                }

                if (reader["siteComparison"] != DBNull.Value)
                {
                    SiteComparison = Convert.ToInt32(reader["siteComparison"]);
                }
                if (reader["fleetComparison"] != DBNull.Value)
                {
                    FleetComparison = Convert.ToInt32(reader["fleetComparison"]);
                }
                if (reader["reservations"] != DBNull.Value)
                {
                    Reservations = Convert.ToInt32(reader["reservations"]);
                }


            }
            #endregion
        }

        #endregion

    }
}