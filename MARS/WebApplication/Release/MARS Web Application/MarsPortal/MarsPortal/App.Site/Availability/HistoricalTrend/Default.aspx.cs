using System;
using System.Collections.Generic;
using App.BLL;

using Dundas.Charting.WebControl;
using Mars.App.Classes.BLL.Common;

namespace App.AvailabilityTool.HistoricalTrend
{
    public partial class Default : PageBase
    {
        // Update at line 477 for MarsV3 2-5-12 for ReportSettings to display "Custom" instead of date range
        // Update for MarsV3 with no lines in line chart displaying for 1 day trend

        #region "Page Events"

        protected void Page_Init(object sender, System.EventArgs e)
        {
            //Set Chart Page
            this.AvailibilityChartHistoricalTrend.MARSPage = (int)ReportSettings.ReportSettingsPage.ATHistoricalTrend;

            this.AvailibilityChartHistoricalTrend.LoadChartFeatures();
        }

        protected void Page_Load(object sender, System.EventArgs e)
        {

            //Set Page title
            this.Page.Title = "MARS - Availability Tool";

            //Set report settings tool
            this.UserControlReportSettings.MarsTool = (int)ReportSettings.ReportSettingsTool.Availability;
            this.UserControlReportSettings.MarsPage = (int)ReportSettings.ReportSettingsPage.ATHistoricalTrend;


            if (!Page.IsPostBack)
            {


                //Set page informtion on usercontrol
                base.SetPageInformationTitle("ATHistoricalTrend", this.UserControlPageInformation, false);

                //Set Last update / next update labels
                base.SetDataImportUpdateLabel((int)ImportDetails.ImportType.Availability, this.UserControlPageInformation);

                //Check if user as prefences
                CheckReportPreferencesSettings();

            }

            //Set validation group
            this.ModalConfirmHistoricalTrend.ValidationGroup = "GenerateReport";


        }


        protected void CheckReportPreferencesSettings()
        {
            List<ReportPreferences> preferences = base.GetPreferencesSettings();

            if ((preferences != null))
            {
                //There should be only one row
                foreach (ReportPreferences row in preferences)
                {
                    this.UserControlReportSettings.LoadReportSettingsControl(false);
                    this.UserControlReportSettings.SetUserDefaultSettingsAvailabilityTool(row.Logic, row.Country, row.CMS_Pool_Id, row.CMS_Location_Group_Id,
                            row.OPS_Region_Id, row.OPS_Area_Id, row.Car_Segment_Id, row.Car_Class_Id, row.Car_Group_Id, row.UserPreference, row.Location, null,
                                null, null, null, null, null);

                }
            }
            else
            {
                this.UserControlReportSettings.LoadReportSettingsControl(true);
            }

            //Update Update Panel
            this.UpdatePanelHistoricalTrend.Update();

        }
        #endregion

        #region "Click Events"

        protected void ButtonGenerateReport_Click(object sender, System.EventArgs e)
        {
            //Generarte Report
            GenerateReport();
        }


        protected void GenerateReport()
        {
            //Set default values
            int logic = -1;
            string country = "-1";
            int cms_pool_id = -1;
            string cms_pool = null;
            int cms_location_group_id = -1;
            string cms_location_group = null;
            int ops_region_id = -1;
            string ops_region = null;
            int ops_area_id = -1;
            string ops_area = null;
            int car_segment_id = -1;
            string car_segment = null;
            int car_class_id = -1;
            string car_class = null;
            int car_group_id = -1;
            string car_group = null;
            DateTime availabilityDate = DateTime.Now;
            int dayOfWeekValue = -1;
            string dayOfWeek = null;
            int dateRangeValue = -1;
            string dateRange = null;
            string location = "-1";
            string fleetName = "-1";

            //Pass values from usercontrol
            logic = this.UserControlReportSettings.Logic;
            country = this.UserControlReportSettings.Country;

            switch (logic)
            {

                case (int)ReportSettings.OptionLogic.CMS:
                    cms_pool_id = this.UserControlReportSettings.CMS_Pool_Id;
                    cms_pool = this.UserControlReportSettings.CMS_Pool;
                    cms_location_group_id = this.UserControlReportSettings.CMS_Location_Group_Id;
                    cms_location_group = this.UserControlReportSettings.CMS_Location_Group;

                    break;
                case (int)ReportSettings.OptionLogic.OPS:
                    ops_region_id = this.UserControlReportSettings.OPS_Region_Id;
                    ops_region = this.UserControlReportSettings.OPS_Region;
                    ops_area_id = this.UserControlReportSettings.OPS_Area_Id;
                    ops_area = this.UserControlReportSettings.OPS_Area;

                    break;
            }

            car_segment_id = this.UserControlReportSettings.Car_Segment_Id;
            car_segment = this.UserControlReportSettings.Car_Segment;
            car_class_id = this.UserControlReportSettings.Car_Class_Id;
            car_class = this.UserControlReportSettings.Car_Class;
            car_group_id = this.UserControlReportSettings.Car_Group_Id;
            car_group = this.UserControlReportSettings.Car_Group;

            location = this.UserControlReportSettings.Location;
            fleetName = this.UserControlReportSettings.Fleet_Name;

            availabilityDate = this.UserControlReportSettings.DateAvailability;
            dayOfWeekValue = this.UserControlReportSettings.DayOfWeekValue;
            dayOfWeek = this.UserControlReportSettings.DayOfWeek;
            DateTime startDate = DateTime.Now;
            if (UserControlReportSettings.DateRangeValue.Contains("Custom"))
            {
                startDate = UserControlReportSettings.customStartDate;
                availabilityDate = UserControlReportSettings.customEndDate;
                TimeSpan ts = startDate - availabilityDate;
                dateRangeValue = ts.Days;
            }
            else
            {
                dateRangeValue = Convert.ToInt32(this.UserControlReportSettings.DateRangeValue);
                dateRange = this.UserControlReportSettings.DateRange;
                if (dateRangeValue == -1)
                {
                    startDate = availabilityDate.AddDays(dateRangeValue + 1);
                }
                else
                {
                    startDate = availabilityDate.AddDays(dateRangeValue);
                }
            }

            string availabilityLogic = this.UserControlReportSettings.AvailabilityLogic;
            string grouping_criteria = ReportSettings.getRangeGroupingCriteria(dateRangeValue);

            var onRentType = UserControlReportSettings.OnRentTypeSelected;

            //Set Last Selection Value
            base.SaveLastSelectionToSession(logic, country, cms_pool_id, cms_location_group_id, ops_region_id, ops_area_id, location, car_segment_id, car_class_id, car_group_id);
            SessionHandler.OnRentTypeSelected = onRentType;

            //Load Data
            this.GenerateChart(AvailabilityHistoricalTrend.GetHistoricalTrendData(country, cms_pool_id, cms_location_group_id, ops_region_id, ops_area_id, location, fleetName, car_segment_id, car_class_id, car_group_id,
            startDate, availabilityDate, dayOfWeekValue, grouping_criteria, availabilityLogic, onRentType), dateRangeValue, availabilityLogic);


            //Set report Selection
            this.SetReportSelection(logic, country, cms_pool_id, cms_location_group_id, ops_region_id, ops_area_id, car_segment_id, car_class_id, car_group_id, location,
            startDate, dayOfWeekValue, dateRangeValue);



            //Log Usage Statistics
            base.LogUsageStatistics((int)ReportStatistics.ReportName.AvailabilityHistoricalTrend, country, cms_pool_id, cms_location_group_id, ops_region_id, ops_area_id, location);


            this.UpdatePanelHistoricalTrend.Update();

        }
        #endregion

        #region "Chart Events"


        protected void GenerateChart(System.Data.DataTable dataValues, int dateRangeValue, string logic)
        {
            int rowCount = dataValues.Rows.Count;
            if (rowCount >= 1)
            {
                this.AvailibilityChartHistoricalTrend.Visible = true;
                this.EmptyDataTemplateHistoricalTrend.Visible = false;
            }
            else
            {
                this.EmptyDataTemplateHistoricalTrend.Visible = true;
                this.AvailibilityChartHistoricalTrend.Visible = false;
            }
            Dundas.Charting.WebControl.Chart ChartHistoricalTrend = (Dundas.Charting.WebControl.Chart)this.AvailibilityChartHistoricalTrend.FindControl("DundasChartAvailability");
            ChartHistoricalTrend.Series.Clear();
            ChartHistoricalTrend.Legends.Clear();
            ChartHistoricalTrend.Legends.Add("Default");
            ChartHistoricalTrend.Legends["Default"].LegendStyle = LegendStyle.Column;
            ChartHistoricalTrend.Visible = true;

            //reset chart title
            ChartHistoricalTrend.Titles.Clear();
            Dundas.Charting.WebControl.Title ctitle = new Dundas.Charting.WebControl.Title();
            ctitle.Text = Resources.lang.HistoricalTrend;
            ctitle.Font = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Bold);
            ctitle.Alignment = System.Drawing.ContentAlignment.TopLeft;
            ChartHistoricalTrend.Titles.Add(ctitle);

            //Add a item to reset zoom
            Dundas.Charting.WebControl.Title resetZoomTitle = new Dundas.Charting.WebControl.Title();
            resetZoomTitle.Font = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Bold);
            resetZoomTitle.Alignment = System.Drawing.ContentAlignment.BottomRight;
            resetZoomTitle.Docking = Docking.Bottom;
            resetZoomTitle.ToolTip = Resources.lang.ChartZoomFeature;
            resetZoomTitle.DockOffset = 1;
            resetZoomTitle.MapAreaAttributes = "onclick=\"" + ChartHistoricalTrend.CallbackManager.GetCallbackEventReference("ResetZoom", "ResetZoom") + "\"";
            resetZoomTitle.Text = Resources.lang.ChartZoomText;
            resetZoomTitle.BackImage = "~/App.Images/reset_zoom.png";
            resetZoomTitle.BackImageMode = ChartImageWrapMode.Unscaled;
            resetZoomTitle.BackImageAlign = ChartImageAlign.BottomRight;
            ChartHistoricalTrend.Titles.Add(resetZoomTitle);

            //Declare chart series
            // remove carsales, gold and predelivery from the dataview
            Series series1 = ChartHistoricalTrend.Series.Add("TOTAL FLEET");
            //Series series2 = ChartHistoricalTrend.Series.Add("CARSALES"); // removed to protect the innocent
            Series series3 = ChartHistoricalTrend.Series.Add("CU");
            Series series4 = ChartHistoricalTrend.Series.Add("HA");
            Series series5 = ChartHistoricalTrend.Series.Add("HL");
            Series series6 = ChartHistoricalTrend.Series.Add("LL");
            Series series7 = ChartHistoricalTrend.Series.Add("NC");
            Series series8 = ChartHistoricalTrend.Series.Add("PL");
            Series series9 = ChartHistoricalTrend.Series.Add("TC");
            Series series10 = ChartHistoricalTrend.Series.Add("SV");
            Series series11 = ChartHistoricalTrend.Series.Add("WS");
            Series series12 = ChartHistoricalTrend.Series.Add("OPERATIONAL FLEET");
            Series series13 = ChartHistoricalTrend.Series.Add("BD");
            Series series14 = ChartHistoricalTrend.Series.Add("MM");
            Series series15 = ChartHistoricalTrend.Series.Add("TW");
            Series series16 = ChartHistoricalTrend.Series.Add("TB");
            Series series17 = ChartHistoricalTrend.Series.Add("FS");
            Series series18 = ChartHistoricalTrend.Series.Add("RL");
            Series series19 = ChartHistoricalTrend.Series.Add("RP");
            Series series20 = ChartHistoricalTrend.Series.Add("TN");
            Series series21 = ChartHistoricalTrend.Series.Add("AVAILABLE FLEET");
            Series series22 = ChartHistoricalTrend.Series.Add("RT");
            Series series23 = ChartHistoricalTrend.Series.Add("SU");
            //Series series24 = ChartHistoricalTrend.Series.Add("GOLD");
            //Series series25 = ChartHistoricalTrend.Series.Add("PREDELIVERY");
            Series series26 = ChartHistoricalTrend.Series.Add("OVERDUE");
            Series series27 = ChartHistoricalTrend.Series.Add("ON RENT");

            string xAxisValue = "REP_DATE"; // set to default

            if (dateRangeValue < -30 && dateRangeValue >= -90)
            {
                xAxisValue = "REP_WEEK_OF_YEAR";
            }
            if (dateRangeValue < -90)
            {
                xAxisValue = "REP_MONTH";
            }

            // Initializes a new instance of the DataView class
            System.Data.DataView firstView = new System.Data.DataView(dataValues);

            // Since the DataView implements IEnumerable, pass the reader directly into
            // the DataBind method with the name of the Columns selected in the query    
            //Set series properties
            int index = 0;
            foreach (Series chartSeries in ChartHistoricalTrend.Series)
            {
                chartSeries.Points.DataBindXY(firstView, xAxisValue, firstView, chartSeries.Name.Replace(" ", "_"));
                if (logic == "PERCENTAGE")
                {
                    foreach (DataPoint p in chartSeries.Points)
                    {
                        //p.YValues[0] = Math.Round(p.YValues[0], 2);
                        p.LabelFormat = "P";
                    }
                }

                chartSeries.Color = Charts.GetColourForColumn(chartSeries.Name, (int)ReportSettings.ReportSettingsTool.Availability);
                switch (chartSeries.Name)
                {
                    case "AVAILABLE FLEET":
                    case "RT":
                    case "SU":
                    //case "GOLD":
                    //case "PREDELIVERY":
                    case "OVERDUE":
                    case "ON RENT":
                        chartSeries.Enabled = true;
                        break;
                    default:
                        chartSeries.Enabled = false;
                        break;
                }

                // Set range column chart type
                chartSeries.Type = SeriesChartType.Line;

                //Set series tooltip
                chartSeries.ToolTip = "#VALX: #VAL, #SERIESNAME";

                // Set the side-by-side drawing style
                chartSeries["DrawSideBySide"] = "false";

                chartSeries.BorderWidth = 2;


                //Set x value type
                if (xAxisValue == "REP_DATE")
                {
                    chartSeries.XValueType = ChartValueTypes.DateTime;
                }
                else
                {
                    chartSeries.XValueType = ChartValueTypes.Int;
                }

                chartSeries.Color = Charts.GetColourForColumn(chartSeries.Name, (int)ReportSettings.ReportSettingsTool.Availability);

                chartSeries.ShowInLegend = false;
                // Add custom legend item with image
                LegendItem legendItem = new LegendItem();
                LegendCell legendCell1 = new LegendCell();
                LegendCell legendCell2 = new LegendCell();

                legendItem.BackImageTranspColor = System.Drawing.Color.Red;
                legendCell1.CellType = LegendCellType.Image;
                if (chartSeries.Enabled == true)
                {
                    legendCell1.Image = "~/App.Images/chk_checked.png";
                }
                else
                {
                    legendCell1.Image = "~/App.Images/chk_unchecked.png";
                }
                legendCell1.ImageTranspColor = System.Drawing.Color.Red;
                legendCell2.CellType = LegendCellType.Text;
                legendCell2.Font = new System.Drawing.Font("Arial", 8, System.Drawing.FontStyle.Regular);
                legendCell2.Text = chartSeries.Name;
                legendCell2.TextColor = chartSeries.Color;
                legendItem.Cells.Add(legendCell1);
                legendItem.Cells.Add(legendCell2);
                legendItem.MapAreaAttributes = "onclick=\"" + ChartHistoricalTrend.CallbackManager.GetCallbackEventReference("LegendClick", index.ToString()) + "\"";

                legendItem.BorderWidth = 0;
                ChartHistoricalTrend.Legends["Default"].CustomItems.Add(legendItem);
                index += 1;
            }

            var yLabelStyle = string.Empty;
            switch (logic)
            {
                case "PERCENTAGE":
                    yLabelStyle = "P";
                    break;
                case "NUMERIC":
                    yLabelStyle = "D";
                    break;
                default:
                    yLabelStyle = "P0";
                    break;
            }
            ChartHistoricalTrend.ChartAreas["Default"].AxisY.LabelStyle.Format = yLabelStyle;




            //Set X Axis style
            ChartHistoricalTrend.ChartAreas["Default"].AxisX.LabelsAutoFit = false;
            ChartHistoricalTrend.ChartAreas["Default"].AxisX.LabelStyle.Font = new System.Drawing.Font("Arial", 7, System.Drawing.FontStyle.Bold);
            ChartHistoricalTrend.ChartAreas["Default"].AxisX.LineColor = System.Drawing.Color.FromArgb(64, 64, 64, 64);
            ChartHistoricalTrend.ChartAreas["Default"].AxisX.Interlaced = false;
            ChartHistoricalTrend.ChartAreas["Default"].AxisX.MajorGrid.LineColor = System.Drawing.Color.FromArgb(64, 64, 64, 64);
            ChartHistoricalTrend.ChartAreas["Default"].AxisX.LabelStyle.FontAngle = -45;

            ChartHistoricalTrend.ChartAreas["Default"].AxisY.LabelStyle.Font = new System.Drawing.Font("Arial", 7, System.Drawing.FontStyle.Bold);
            ChartHistoricalTrend.ChartAreas["Default"].AxisY.LineColor = System.Drawing.Color.Gray;
            ChartHistoricalTrend.ChartAreas["Default"].AxisY.Interlaced = true;
            ChartHistoricalTrend.ChartAreas["Default"].AxisY.InterlacedColor = System.Drawing.Color.FromArgb(20, 20, 20, 20);
            ChartHistoricalTrend.ChartAreas["Default"].AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(64, 64, 64, 64);

            //Disable Variable Label Intervals - force showing every X Value
            ChartHistoricalTrend.ChartAreas["Default"].AxisX.Interval = 1;

            // Enable AntiAliasing for either Text and Graphics or just Graphics
            ChartHistoricalTrend.AntiAliasing = AntiAliasing.All;



        }




        #endregion


        #region "Report Selection"

        protected void SetReportSelection(int selectedLogic, string country, int cms_pool_id, int cms_location_group_id, int ops_region_id, int ops_area_id, int car_segment_id, int car_class_id, int car_group_id, string location,
        DateTime start_date, int dayOfWeek, int dateRange)
        {
            //Set Report Selection User Control
            //Logic
            this.UserControlReportSelections.Logic = selectedLogic;

            //Country
            if (country == "-1")
            {
                this.UserControlReportSelections.Country = Resources.lang.ReportSettingsALLSelection;
            }
            else
            {
                this.UserControlReportSelections.Country = country;
            }

            //Select logic
            switch (selectedLogic)
            {

                case (int)ReportSettings.OptionLogic.CMS:
                    //CMS Pool
                    if (cms_pool_id == -1)
                    {
                        this.UserControlReportSelections.CMS_Pool = Resources.lang.ReportSettingsALLSelection;
                    }
                    else
                    {
                        this.UserControlReportSelections.CMS_Pool = this.UserControlReportSettings.CMS_Pool;
                    }
                    //CMS Location Group
                    if (cms_location_group_id == -1)
                    {
                        this.UserControlReportSelections.CMS_Location_Group = Resources.lang.ReportSettingsALLSelection;
                    }
                    else
                    {
                        this.UserControlReportSelections.CMS_Location_Group = this.UserControlReportSettings.CMS_Location_Group;
                    }

                    break;
                case (int)ReportSettings.OptionLogic.OPS:
                    //OPS Region
                    if (ops_region_id == -1)
                    {
                        this.UserControlReportSelections.OPS_Region = Resources.lang.ReportSettingsALLSelection;
                    }
                    else
                    {
                        this.UserControlReportSelections.OPS_Region = this.UserControlReportSettings.OPS_Region;
                    }
                    //OPS Area
                    if (ops_area_id == -1)
                    {
                        this.UserControlReportSelections.OPS_Area = Resources.lang.ReportSettingsALLSelection;
                    }
                    else
                    {
                        this.UserControlReportSelections.OPS_Area = this.UserControlReportSettings.OPS_Area;
                    }
                    break;
            }

            //Location
            if (location == "-1")
            {
                this.UserControlReportSelections.Location = Resources.lang.ReportSettingsALLSelection;
            }
            else
            {
                this.UserControlReportSelections.Location = location;
            }

            //Car Segment
            if (car_segment_id == -1)
            {
                this.UserControlReportSelections.Car_Segment = Resources.lang.ReportSettingsALLSelection;
            }
            else
            {
                this.UserControlReportSelections.Car_Segment = this.UserControlReportSettings.Car_Segment;
            }

            //Car Class
            if (car_class_id == -1)
            {
                this.UserControlReportSelections.Car_Class = Resources.lang.ReportSettingsALLSelection;
            }
            else
            {
                this.UserControlReportSelections.Car_Class = this.UserControlReportSettings.Car_Class;
            }

            //Car Group
            if (car_group_id == -1)
            {
                this.UserControlReportSelections.Car_Group = Resources.lang.ReportSettingsALLSelection;
            }
            else
            {
                this.UserControlReportSelections.Car_Group = this.UserControlReportSettings.Car_Group;
            }


            this.UserControlReportSelections.StartDate = string.Format("{0:dd/MM/yyyy}", start_date);
            this.UserControlReportSelections.DayOfWeek = ReportSettings.GetDayOfWeek(dayOfWeek);
            this.UserControlReportSelections.DateRange = UserControlReportSettings.DateRangeValue.Contains("Custom") ? "Custom" : ReportSettings.GetDateRange(dateRange);

            //Set Report Selection for tool and page
            this.UserControlReportSelections.MarsTool = (int)ReportSettings.ReportSettingsTool.Availability;
            this.UserControlReportSelections.MarsPage = (int)ReportSettings.ReportSettingsPage.ATHistoricalTrend;
            //Display report selection
            this.UserControlReportSelections.ShowReportSelections();
        }

        #endregion
    }
}