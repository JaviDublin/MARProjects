using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using App.BLL.ExcelExport;
using App.BLL.Management;
using App.BLL.ReportParameters;
using App.DAL.MarsDataAccess.ParameterAccess;
using App.DAL.MarsDataAccess.Sizing;
using App.Entities;
using App.DAL.MarsDataAccess;
using App.BLL.EventArgs;
using App.Entities.Graphing.Parameters;
using App.UserControls.DatePicker;
using App.BLL.ExtensionMethods;
using App.BLL.Utilities;
using App.BLL.CMSReportType;
using App.DAL.MarsDataAccess.Management;
using Mars.App.Classes.BLL.ExtensionMethods;


namespace App.UserControls
{
    public partial class ManualMovementTabView : System.Web.UI.UserControl
    {
        private BLLManagement bllManagement = new BLLManagement();
        private BLLReportParameters bllParameters = new BLLReportParameters();
        
        private static readonly string countryDummy = StaticStrings.CountryDummy;

        public bool FutureDatesOnly
        {
            get
            {
                return GeneralParams.FutureDatesOnly;
            }

            set
            {
                GeneralParams.FutureDatesOnly = value;
            }
        }

        private List<ReportParameter> FleetPlanParameters
        {
            get
            {
                return (List<ReportParameter>)Session["FleetPlanParameters"];
            }
            set { Session["FleetPlanParameters"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.mvManualMovement.ActiveViewIndex = 0;
                Page.Init += new EventHandler(Page_Init);
                Page.LoadComplete += new EventHandler(Page_LoadComplete);
            }
            WireEvents();
        }

        void Page_LoadComplete(object sender, EventArgs e)
        {
            PopulateDropDowns();
            PopulateSubControls(null);            
        }

        void Page_Init(object sender, EventArgs e)
        {
            if (FleetPlanParameters == null)
            {
                FleetPlanParameters = new List<ReportParameter>
                {
                    new ReportParameter(0, 0, null, ParameterNames.Country),
                    new ReportParameter(1, 1, ParameterDataAccess.GetLocationGroupParameterListItemsAllCountry, ParameterNames.LocationGroup),
                    new ReportParameter(2, 1, ParameterDataAccess.GetCarClassParameterListItemsAllCountry, ParameterNames.CarClass)
                };
            }
            GeneralParams.DynamicReportParametersControl.ParamsHolder = FleetPlanParameters;
            GeneralParams.ExportType = 4;
        }

        private void PopulateDropDowns()
        {
            System.Web.UI.WebControls.DropDownList ddlCountryList = ((System.Web.UI.WebControls.DropDownList)
                GeneralParams.DynamicReportParametersControl.FindControl("ddlCountry"));
            ddlCountryList.DataTextField = "CountryDescription";
            ddlCountryList.DataValueField = "CountryID";

            Country dummy = new Country();
            dummy.CountryDescription = StaticStrings.SelectCountry;
            dummy.CountryID = countryDummy;

            List<Country> countries = bllManagement.CountryGetAllByRole(this.Page.RadUserId());

            countries.Insert(0, dummy);

            ddlCountryList.DataSource = countries;
            ddlCountryList.DataBind();

            if (countries.Count == 2)
                ddlCountryList.SelectedIndex = 1;
        }

        protected void menuManualMovementSelection_MenuItemClick(object sender, MenuEventArgs e)
        {
            mvManualMovement.ActiveViewIndex = Int32.Parse(e.Item.Value);
            hdnTabIndex.Value = e.Item.Value;
        }

        private void WireEvents()
        {
            mmGridViewActual.RebindGrids += new EventHandler(mmGridViewActual_RebindGrids);
            mmGridViewScenario1.RebindGrids += new EventHandler(mmGridViewActual_RebindGrids);
            mmGridViewScenario2.RebindGrids += new EventHandler(mmGridViewActual_RebindGrids);
            mmGridViewScenario3.RebindGrids += new EventHandler(mmGridViewActual_RebindGrids);
        }

        private void PopulateSubControls(string currentDate)
        {
            var country = ((System.Web.UI.WebControls.DropDownList)GeneralParams.DynamicReportParametersControl.FindControl("ddlCountry")).SelectedItem.Value;

            var ddlLocationGroup = ((System.Web.UI.WebControls.DropDownList)GeneralParams.DynamicReportParametersControl.FindControl("ddlLocation Group")).SelectedItem.Value;
            var locationGroup = 0;
            int.TryParse(ddlLocationGroup, out locationGroup);

            var ddlCarClass = ((System.Web.UI.WebControls.DropDownList)GeneralParams.DynamicReportParametersControl.FindControl("ddlCar Class")).SelectedItem.Value;
            var carClass = 0;
            int.TryParse(ddlCarClass, out carClass);

            var datePicker = (DatePicker.DatePicker)GeneralParams.DynamicReportParametersControl.FindControl("ucDatePicker");
            var dateFrom = (currentDate != null) ? Convert.ToDateTime(currentDate) : Convert.ToDateTime(datePicker.FromDate);
            var dateTo = (currentDate != null) ? Convert.ToDateTime(currentDate) : Convert.ToDateTime(datePicker.ToDate);
            if (country != countryDummy)
            {
                FleetPlanDetailListContainer fleetPlanDetailListContainer = 
                    bllManagement.GetFleetPlanDetailBy(country, locationGroup, carClass, dateFrom, dateTo );


                //for sorting etc
                Session["MovementList"] = fleetPlanDetailListContainer.FleetPlanDetailList;
                              
                var scenarioID = Convert.ToInt32(mmGridViewActual.ScenarioID);
                var fleetPlan = fleetPlanDetailListContainer.FleetPlanEntryList.Find(p => p.ScenarioID == scenarioID);
                mmGridViewActual.FleetPlanID = (fleetPlan != null) ? fleetPlan.FleetPlanID.ToString() : "0";
                var movementListActual = fleetPlanDetailListContainer.FleetPlanDetailList.Where(p => p.ScenarioID == scenarioID).ToList();
                mmGridViewActual.GridView.DataSource = movementListActual;
                mmGridViewActual.GridView.DataBind();
                mmGridViewActual.SetSelectedPopupDate = (currentDate != null) ? currentDate : datePicker.FromDate;

                scenarioID = Convert.ToInt32(mmGridViewScenario1.ScenarioID);
                fleetPlan = fleetPlanDetailListContainer.FleetPlanEntryList.Find(p => p.ScenarioID == scenarioID);
                mmGridViewScenario1.FleetPlanID = (fleetPlan != null) ? fleetPlan.FleetPlanID.ToString() : "0";
                var movementListScenario1 = fleetPlanDetailListContainer.FleetPlanDetailList.Where(p => p.ScenarioID == scenarioID).ToList();
                mmGridViewScenario1.GridView.DataSource = movementListScenario1;
                mmGridViewScenario1.GridView.DataBind();
                mmGridViewScenario1.SetSelectedPopupDate = (currentDate != null) ? currentDate : datePicker.FromDate;

                scenarioID = Convert.ToInt32(mmGridViewScenario2.ScenarioID);
                fleetPlan = fleetPlanDetailListContainer.FleetPlanEntryList.Find(p => p.ScenarioID == scenarioID);
                mmGridViewScenario2.FleetPlanID = (fleetPlan != null) ? fleetPlan.FleetPlanID.ToString() : "0";
                var movementListScenario2 = fleetPlanDetailListContainer.FleetPlanDetailList.Where(p => p.ScenarioID == scenarioID).ToList();
                mmGridViewScenario2.GridView.DataSource = movementListScenario2;
                mmGridViewScenario2.GridView.DataBind();
                mmGridViewScenario2.SetSelectedPopupDate = (currentDate != null) ? currentDate : datePicker.FromDate;

                scenarioID = Convert.ToInt32(mmGridViewScenario3.ScenarioID);
                fleetPlan = fleetPlanDetailListContainer.FleetPlanEntryList.Find(p => p.ScenarioID == scenarioID);
                mmGridViewScenario3.FleetPlanID = (fleetPlan != null) ? fleetPlan.FleetPlanID.ToString() : "0";
                var movementListScenario3 = fleetPlanDetailListContainer.FleetPlanDetailList.Where(p => p.ScenarioID == scenarioID).ToList();
                mmGridViewScenario3.GridView.DataSource = movementListScenario3;
                mmGridViewScenario3.GridView.DataBind();
                mmGridViewScenario3.SetSelectedPopupDate = (currentDate != null) ? currentDate : datePicker.FromDate;

                var locationGroups = bllParameters.LocationGroupGetByCountryID(country);
                mmGridViewActual.CurrentLocationGroupList = locationGroups;
                mmGridViewActual.SetSelectedLocationGroups = ddlLocationGroup;

                mmGridViewScenario1.CurrentLocationGroupList = locationGroups;
                mmGridViewScenario1.SetSelectedLocationGroups = ddlLocationGroup;

                mmGridViewScenario2.CurrentLocationGroupList = locationGroups;
                mmGridViewScenario2.SetSelectedLocationGroups = ddlLocationGroup;

                mmGridViewScenario3.CurrentLocationGroupList = locationGroups;
                mmGridViewScenario3.SetSelectedLocationGroups = ddlLocationGroup;
                
                var carGroupList = bllParameters.CarGroupListGetByCountryID(country);
                mmGridViewActual.CurrentCarClassList = carGroupList;
                mmGridViewActual.SetSelectedCarClass = ddlCarClass;

                mmGridViewScenario1.CurrentCarClassList = carGroupList;
                mmGridViewScenario1.SetSelectedCarClass = ddlCarClass;

                mmGridViewScenario2.CurrentCarClassList = carGroupList;
                mmGridViewScenario2.SetSelectedCarClass = ddlCarClass;

                mmGridViewScenario3.CurrentCarClassList = carGroupList;
                mmGridViewScenario3.SetSelectedCarClass = ddlCarClass;
                
                int index = 0;
                Int32.TryParse(hdnTabIndex.Value, out index);
                mvManualMovement.ActiveViewIndex = index;

            }
        }

        void mmGridViewActual_RebindGrids(object sender, EventArgs e)
        {
            PopulateSubControls(null);
        }

         private  void InitializeGridViews()
        {
            mmGridViewActual.GridView.DataSource = new List<FleetPlanDetail>();
            mmGridViewActual.GridView.DataBind();
            mmGridViewScenario1.GridView.DataSource = new List<FleetPlanDetail>();
            mmGridViewScenario1.GridView.DataBind();
            mmGridViewScenario2.GridView.DataSource = new List<FleetPlanDetail>();
            mmGridViewScenario2.GridView.DataBind();
            mmGridViewScenario3.GridView.DataSource = new List<FleetPlanDetail>();
            mmGridViewScenario3.GridView.DataBind();
        }

        protected override bool OnBubbleEvent(object source, EventArgs args)
        {
            var handled = false;

            if (args is ParameterChangeEventArgs)
            {
                ParameterChangeEventArgs pcea = (ParameterChangeEventArgs)args;
                PopulateSubControls(pcea.date);
                handled = true;
            }

            if (args is ExportToExcelEventArgs)
            {
                ExportToExcelEventArgs exportArgs = (ExportToExcelEventArgs)args;
                if (exportArgs.isTextExport)
                    ExportText(exportArgs.scenarioType, exportArgs.isAddition);
                else
                    ExportExcel(exportArgs.scenarioType, exportArgs.isAddition);
                handled = true;
            }

            return handled;
        }

        private void ExportText(int scenarioID, bool isAddition)
        {
            var country = ((System.Web.UI.WebControls.DropDownList)GeneralParams.DynamicReportParametersControl.FindControl("ddlCountry")).SelectedItem.Value;

            var ddlLocationGroup = ((System.Web.UI.WebControls.DropDownList)GeneralParams.DynamicReportParametersControl.FindControl("ddlLocation Group")).SelectedItem.Value;
            var locationGroupID = 0;
            int.TryParse(ddlLocationGroup, out locationGroupID);

            var ddlCarClass = ((System.Web.UI.WebControls.DropDownList)GeneralParams.DynamicReportParametersControl.FindControl("ddlCar Class")).SelectedItem.Value;
            var carClassGroupID = 0;
            int.TryParse(ddlCarClass, out carClassGroupID);

            var datePicker = (DatePicker.DatePicker)GeneralParams.DynamicReportParametersControl.FindControl("ucDatePicker");
            var fromDate = datePicker.FromDate;

            if (country != countryDummy)
            {
                var bll = new BLLExcelExport();
                var fileNameCountry = ((country == string.Empty) ? "All_Countries" : country);

                Session["ExportData"] = bll.FleetPlanDetailExport(country, scenarioID, locationGroupID, carClassGroupID, fromDate, null, isAddition);
                Session["ExportFileName"] = fileNameCountry + "_" + (ScenarioType)scenarioID + "_" + (isAddition ? "Addition" : "Deletion");
                Session["ExportFileType"] = "txt";

                //Response.Redirect("../ExcelGenerator.aspx?country=" + country +
                //"&fromDate=" + fromDate +
                //"&fleetPlan=" + scenarioID +
                //"&locationGroupId=" + locationGroupID +
                //"&carClassGroupID=" + carClassGroupID +
                //"&isAddition=" + isAddition.ToString() +
                //"&reportType=6");
            }
        }

        private void ExportExcel(int scenarioID, bool isAddition)
        {
            var siteGroup = GeneralParams.ExcelExportControl.SelectedGroupBySite;
            var fleetGroup = GeneralParams.ExcelExportControl.SelectedGroupByFleet;
            var country = ((System.Web.UI.WebControls.DropDownList)GeneralParams.DynamicReportParametersControl.FindControl("ddlCountry")).SelectedItem.Value;

            Session["ExportData"] = ManagementDataAccess.GetAdditionDeletionPlanExcelData(country, scenarioID,
                                            isAddition, int.Parse(siteGroup), int.Parse(fleetGroup));

            Session["ExportFileName"] = "FleetExport";
            //Response.Redirect("~/ExcelGenerator2.aspx");
        }
    }
}