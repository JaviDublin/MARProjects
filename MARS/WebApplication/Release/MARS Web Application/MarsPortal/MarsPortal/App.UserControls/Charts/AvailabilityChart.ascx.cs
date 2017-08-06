using System.Web.UI.WebControls;
using App.BLL;
using Dundas.Charting.WebControl;
using System.Web.UI;

namespace App.UserControls.Charts {
    public partial class AvailabilityChart : System.Web.UI.UserControl {
        // Alterations added by Gavin 20-4-14 from MarsV3
        // DundasChartAvailability_CommandFiring method added to abort Chart Printing
        // A Javascript function to print screen added and link to this button - line:32 in Page_Load
        // PrintPreview and Save masked to avoid confusion - lines 101 and 115
        // Alteration at 57 to get KPI chart Y axis to start above zero

        private const string JS_PRINT = "jsprintScript";

        #region Properties and Fields
        
        private int _marspage;

        private int _chartType;
        
        public int MARSPage {
            get { return _marspage; }
            set { _marspage = value; }
        }

        public int ChartType {
            get { return _chartType; }
            set { _chartType = value; }
        }
        
        #endregion

        protected void Page_Load(object sender, System.EventArgs e) 
        {
            //ClientScriptManager cs = Page.ClientScript; // Added by Gavin to print entire chart with outer details
            //if (!cs.IsStartupScriptRegistered(JS_PRINT)) {
            //    string s = "ChartToolbar.doPostBack=function(){window.print();};";
            //    cs.RegisterStartupScript(this.GetType(), JS_PRINT, s, true);
            //}
        }
        
        #region Chart Settings

        public void LoadChartFeatures() {
            //Settings for all charts except KPI Download
            if (!(_marspage == (int)ReportSettings.ReportSettingsPage.ATKPIDownload)) {
                switch (_marspage) {
                    case (int)(ReportSettings.ReportSettingsPage.ATFleetComparison):
                    case (int)(ReportSettings.ReportSettingsPage.ATSiteComparison):
                    case (int)(ReportSettings.ReportSettingsPage.ATHistoricalTrend):
                    case (int)(ReportSettings.ReportSettingsPage.ATKPI):

                        this.DundasChartAvailability.ChartAreas["Default"].CursorX.UserEnabled = true;
                        // Set restriction on how far the user can zoom in
                        this.DundasChartAvailability.ChartAreas["Default"].AxisX.View.MinSize = 5;
                        //the number of samples per view

                        //Set initial X axis zooming
                        this.DundasChartAvailability.ChartAreas["Default"].AxisX.View.Position = double.NaN;
                        this.DundasChartAvailability.ChartAreas["Default"].AxisX.View.Size = double.NaN;
                        if (_marspage == (int)ReportSettings.ReportSettingsPage.ATKPI) // Alteration Gavin
                            DundasChartAvailability.ChartAreas["Default"].AxisY.StartFromZero = false;

                        this.DundasChartAvailability.CallbackStateContent = CallbackStateContent.All;

                        // Enable AJAXZoomEnabled property.
                        this.DundasChartAvailability.AJAXZoomEnabled = true;

                        break;
                    case (int)ReportSettings.ReportSettingsPage.ATFleetStatus:

                        this.DundasChartAvailability.CallbackStateContent = CallbackStateContent.All;

                        break;

                }


                switch (_marspage) {
                    case (int)(ReportSettings.ReportSettingsPage.ATHistoricalTrend):
                    case (int)(ReportSettings.ReportSettingsPage.ATKPI):
                        this.DundasChartAvailability.ChartAreas["Default"].AxisX.LabelStyle.Format = "d/MM/yyy";
                        break;
                }

                switch (_marspage) {
                    case (int)(ReportSettings.ReportSettingsPage.ATFleetComparison):
                    case (int)(ReportSettings.ReportSettingsPage.ATSiteComparison):
                        this.DundasChartAvailability.Legends["Default"].Enabled = false;
                        break;
                }


                // Javier - This Code was commented:
                //-----------------------------------------------------------------------------------
                //reset Toolbar
                this.DundasChartAvailability.UI.Toolbar.Items.Clear();
                //set toolbar
                this.DundasChartAvailability.UI.Toolbar.Enabled = true;
                this.DundasChartAvailability.UI.Toolbar.Docking = ToolbarDocking.Bottom;
                this.DundasChartAvailability.UI.Toolbar.Placement = ToolbarPlacement.InsideChart;
                //-----------------------------------------------------------------------------------
                
                this.DundasChartAvailability.UI.Toolbar.BorderStyle = ChartDashStyle.NotSet;
                this.DundasChartAvailability.UI.Toolbar.BackColor = System.Drawing.Color.Transparent;

                // Get reference to the chart commands object
                
                CommandCollection commands = this.DundasChartAvailability.UI.Commands;
                Command command = default(Command);

                // Javier - This Code was commented:
                //-----------------------------------------------------------------------------------
                //Add commands

                //Save -- Blanked by Gavin
                //command = commands.FindCommand(ChartCommandType.SaveImage);
                //this.DundasChartAvailability.UI.Toolbar.Items.Add(command);
                //Copy
                command = commands.FindCommand(ChartCommandType.Copy);
                this.DundasChartAvailability.UI.Toolbar.Items.Add(command);
                
                //Separator
                //command = commands.FindCommand(ChartCommandType.Separator);
                //this.DundasChartAvailability.UI.Toolbar.Items.Add(command);

                //Print
                command = commands.FindCommand(ChartCommandType.Print);
                this.DundasChartAvailability.UI.Toolbar.Items.Add(command);

                //Print Preview -- Blanked by Gavin
                //command = commands.FindCommand(ChartCommandType.PrintPreview);
                //this.DundasChartAvailability.UI.Toolbar.Items.Add(command);

                //Separator
                command = commands.FindCommand(ChartCommandType.Separator);
                this.DundasChartAvailability.UI.Toolbar.Items.Add(command);

                command = commands.FindCommand(ChartCommandType.ToggleDataLabels);
                this.DundasChartAvailability.UI.Toolbar.Items.Add(command);

                //Toggle(3D)
                command = commands.FindCommand(ChartCommandType.Toggle3D);
                this.DundasChartAvailability.UI.Toolbar.Items.Add(command);
                
                //Rotate
                command = commands.FindCommand(ChartCommandType.Rotate3DGroup);
                this.DundasChartAvailability.UI.Toolbar.Items.Add(command);

                //-----------------------------------------------------------------------------------

                switch (_marspage) {
                    case (int)(ReportSettings.ReportSettingsPage.ATHistoricalTrend):
                    case (int)(ReportSettings.ReportSettingsPage.ATFleetStatus):
                    case (int)(ReportSettings.ReportSettingsPage.ATKPI):

                        // Javier - This Code was commented:
                        //-----------------------------------------------------------------------------------
                        //'Toggle legend
                        command = commands.FindCommand(ChartCommandType.ToggleLegend);
                        this.DundasChartAvailability.UI.Toolbar.Items.Add(command);
                        //-----------------------------------------------------------------------------------
                        break;
                }

            }


            //Settings for KPI Download Charts
            if (_marspage == (int)ReportSettings.ReportSettingsPage.ATKPIDownload) {
                switch (_chartType) {

                    case (int)Mars.App.Classes.BLL.Common.Charts.ChartType.VehicleStatus:

                        this.DundasChartAvailability.ChartAreas.Clear();
                        this.DundasChartAvailability.ChartAreas.Add("Default");
                        this.DundasChartAvailability.ChartAreas["Default"].CursorX.UserEnabled = false;
                        // Set Chart Area position
                        this.DundasChartAvailability.ChartAreas["Default"].Position.Auto = true;
                        this.DundasChartAvailability.CallbackStateContent = CallbackStateContent.All;
                        // Enable AJAXZoomEnabled property.
                        this.DundasChartAvailability.AJAXZoomEnabled = true;
                        // Enable Toolbar
                        this.DundasChartAvailability.UI.Toolbar.Enabled = true;
                        this.DundasChartAvailability.UI.Toolbar.Docking = ToolbarDocking.Top;
                        this.DundasChartAvailability.UI.Toolbar.Placement = ToolbarPlacement.OutsideChart;
                        // Set the toolbar background color
                        this.DundasChartAvailability.UI.Toolbar.BackColor = System.Drawing.Color.White;

                        // Set the toolbar border style, color and page color
                        this.DundasChartAvailability.UI.Toolbar.BorderSkin.SkinStyle = BorderSkinStyle.Emboss;
                        this.DundasChartAvailability.UI.Toolbar.BorderSkin.FrameBackColor = System.Drawing.Color.Gray;
                        this.DundasChartAvailability.UI.Toolbar.BorderSkin.FrameBorderWidth = 2;
                        this.DundasChartAvailability.UI.Toolbar.BorderSkin.PageColor = System.Drawing.Color.White;

                        //Set X Axis style
                        this.DundasChartAvailability.ChartAreas["Default"].AxisX.LabelsAutoFit = true;
                        this.DundasChartAvailability.ChartAreas["Default"].AxisX.LabelStyle.Font = new System.Drawing.Font("Arial", 7, System.Drawing.FontStyle.Bold);
                        this.DundasChartAvailability.ChartAreas["Default"].AxisX.LineColor = System.Drawing.Color.FromArgb(64, 64, 64, 64);
                        this.DundasChartAvailability.ChartAreas["Default"].AxisX.Interlaced = false;
                        this.DundasChartAvailability.ChartAreas["Default"].AxisX.MajorGrid.LineColor = System.Drawing.Color.FromArgb(64, 64, 64, 64);
                        this.DundasChartAvailability.ChartAreas["Default"].AxisX.LabelStyle.FontAngle = -45;

                        // Set Y axis labels format
                        this.DundasChartAvailability.ChartAreas["Default"].AxisY.LabelStyle.Format = "P0";
                        this.DundasChartAvailability.ChartAreas["Default"].AxisY.LabelStyle.Font = new System.Drawing.Font("Arial", 7, System.Drawing.FontStyle.Bold);
                        this.DundasChartAvailability.ChartAreas["Default"].AxisY.LineColor = System.Drawing.Color.Gray;
                        this.DundasChartAvailability.ChartAreas["Default"].AxisY.Interlaced = true;
                        this.DundasChartAvailability.ChartAreas["Default"].AxisY.InterlacedColor = System.Drawing.Color.FromArgb(20, 20, 20, 20);
                        this.DundasChartAvailability.ChartAreas["Default"].AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(64, 64, 64, 64);

                        //Set Legend style
                        this.DundasChartAvailability.Legends["Default"].AutoFitText = true;
                        this.DundasChartAvailability.Legends["Default"].Docking = LegendDocking.Bottom;

                        break;

                    case (int)Mars.App.Classes.BLL.Common.Charts.ChartType.IdleUnitsOnPeak:

                        this.DundasChartAvailability.CallbackStateContent = CallbackStateContent.All;
                        this.DundasChartAvailability.ChartAreas.Clear();
                        this.DundasChartAvailability.ChartAreas.Add("IdleUnitsOnPeak");
                        this.DundasChartAvailability.ChartAreas["IdleUnitsOnPeak"].CursorX.UserEnabled = false;
                        this.DundasChartAvailability.ChartAreas["IdleUnitsOnPeak"].Position.Auto = true;

                        this.DundasChartAvailability.ChartAreas.Add("OperationalUtilization");
                        this.DundasChartAvailability.ChartAreas["OperationalUtilization"].CursorX.UserEnabled = false;
                        this.DundasChartAvailability.ChartAreas["OperationalUtilization"].Position.Auto = true;

                        this.DundasChartAvailability.Legends["Default"].AutoFitText = true;
                        this.DundasChartAvailability.Legends["Default"].Docking = LegendDocking.Right;


                        foreach (Dundas.Charting.WebControl.ChartArea oChartArea in this.DundasChartAvailability.ChartAreas) {
                            //Set X Axis style
                            oChartArea.AxisX.LabelsAutoFit = true;
                            oChartArea.AxisX.LabelStyle.Font = new System.Drawing.Font("Arial", 7, System.Drawing.FontStyle.Bold);
                            oChartArea.AxisX.LineColor = System.Drawing.Color.FromArgb(64, 64, 64, 64);
                            oChartArea.AxisX.Interlaced = false;
                            oChartArea.AxisX.MajorGrid.LineColor = System.Drawing.Color.FromArgb(64, 64, 64, 64);
                            oChartArea.AxisX.LabelStyle.FontAngle = -45;

                            // Set Y axis labels format
                            oChartArea.AxisY.LabelStyle.Format = "P0";
                            oChartArea.AxisY.LabelStyle.Font = new System.Drawing.Font("Arial", 7, System.Drawing.FontStyle.Bold);
                            oChartArea.AxisY.LineColor = System.Drawing.Color.Gray;
                            oChartArea.AxisY.Interlaced = true;
                            oChartArea.AxisY.InterlacedColor = System.Drawing.Color.FromArgb(20, 20, 20, 20);
                            oChartArea.AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(64, 64, 64, 64);
                        }


                        break;
                    case (int)Mars.App.Classes.BLL.Common.Charts.ChartType.OperationalUtilization:

                        this.DundasChartAvailability.Legends["Default"].AutoFitText = false;
                        this.DundasChartAvailability.Legends["Default"].Docking = LegendDocking.Top;
                        this.DundasChartAvailability.Legends["Default"].DockToChartArea = "Default";
                        this.DundasChartAvailability.Legends["Default"].LegendStyle = LegendStyle.Column;

                        break;
                }
            }
        }
        #endregion

        #region Event Handlers

        public event CommandEventHandler ChartCallBack;


        #endregion

        #region Events

        protected void DundasChartAvailability_Callback(object sender, System.Web.UI.WebControls.CommandEventArgs e) {

            switch (_marspage) {
                case (int)(ReportSettings.ReportSettingsPage.ATHistoricalTrend):
                case (int)(ReportSettings.ReportSettingsPage.ATKPI):
                    // Handle the legend item click
                    if (e.CommandName == "LegendClick") {
                        // Get the index of the legend item that was clicked
                        int index = int.Parse(e.CommandArgument.ToString());

                        // Legend item result
                        LegendItem legendItem = this.DundasChartAvailability.Legends["Default"].CustomItems[index];

                        // Get the series item that was selected
                        Series selectedSeries = this.DundasChartAvailability.Series[index];

                        if (selectedSeries.Enabled) {
                            selectedSeries.Enabled = false;
                            legendItem.Cells[0].Image = "~/App.Images/chk_unchecked.png";
                            legendItem.Cells[0].ImageTranspColor = System.Drawing.Color.Red;

                        }
                        else {
                            selectedSeries.Enabled = true;
                            legendItem.Cells[0].Image = "~/App.Images/chk_checked.png";
                            legendItem.Cells[0].ImageTranspColor = System.Drawing.Color.Red;
                        }

                    }

                    break;

                case (int)ReportSettings.ReportSettingsPage.ATFleetStatus:
                    //Raise custom event from parent page
                    if (ChartCallBack != null) {
                        ChartCallBack(this, e);
                    }


                    break;
            }

            switch (_marspage) {

                case (int)(ReportSettings.ReportSettingsPage.ATFleetComparison):
                case (int)(ReportSettings.ReportSettingsPage.ATSiteComparison):
                case (int)(ReportSettings.ReportSettingsPage.ATHistoricalTrend):
                case (int)(ReportSettings.ReportSettingsPage.ATKPI):

                    if (e.CommandName == "ResetZoom") {
                        this.DundasChartAvailability.ChartAreas["Default"].AxisX.View.Position = double.NaN;
                        this.DundasChartAvailability.ChartAreas["Default"].AxisX.View.Size = double.NaN;


                    }

                    break;
            }


        }

        #endregion
    }
}