using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using App.BLL.EventArgs;
using App.BLL.ExportLevel;
using App.Entities;
using App.BLL.CMSReportType;
using Mars.BLL.Sizing;

namespace App.UserControls
{
    public partial class ExportToExcel : System.Web.UI.UserControl
    {
        private readonly BLLExportLevel exportLevelBLL = new BLLExportLevel();
        private readonly BLLcmsReportType bllcmsReportType = new BLLcmsReportType();
        private int exportType;
        private bool displayParameters;

        public int ExportType 
        {
            get{ return exportType; }
            
            set
            {
                switch (value)
                {
                    case 1: //groupings only
                        break;
                    case 2: //grouping fleet only, filtering site only 
                        lblGroupBySite.Visible = false;
                        ddlGroupBySite.Visible = false;
                        break;
                    case 3: //grouping site only, filtering fleet only
                        lblGroupByFleet.Visible = false;
                        ddlGroupByFleet.Visible = false;
                        break;
                    case 4: //text and xls type export
                        ddlGroupByFleet.Enabled = true;
                        ddlGroupBySite.Enabled = true;
                        divExport.Style.Add("width", "500px");          //So when viewed in compatability mode the control doesn't cutoff
                        break;
                    default:// filtering only
                        ddlGroupByFleet.Enabled = false;
                        ddlGroupBySite.Enabled = false;
                        break;
                }

                exportType = value;
            }
        }

        public bool DisplayParameters 
        {
            get { return displayParameters; }
            set
            {
                if (value)
                {
                    pnlExportType.Visible = true;
                    pnlExportParameters.Visible = true;
                    pnlExportParameters.CssClass = "adjustmentdynamicParamsNarrow";
                }

                displayParameters = value;
            }
        }   
                 
        public string SelectedGroupBySite
        {
            get { return ddlGroupBySite.SelectedItem.Value; }
        }

        public string SelectedGroupByFleet
        {
            get { return ddlGroupByFleet.SelectedItem.Value; }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {               
                PopulateDropDownLists();
                SetExportButtonText();
                mmsBtn.Visible=false;
                Page.ClientScript.RegisterClientScriptInclude(this.GetType(),"qwerdsjkhjkE2",@"../../../App.Scripts/Sizing/FleetSizeExport.js");
            }
        }

        private void PopulateDropDownLists()
        {
            //var countryItems = App_Classes.DAL.MarsDataAccess.ParameterDataAccess.GetCountryParameterListItems(null);
            //ddlCountry.Items.Add(new ListItem(lang.All, "All"));
            //ddlCountry.Items.AddRange(countryItems.ToArray());

            ddlGroupBySite.DataTextField = "Description";
            ddlGroupBySite.DataValueField = "ExportLevelSiteID";
            List<ExportLevelSite> sites = exportLevelBLL.GetAllExportLevelSite();
            ddlGroupBySite.DataSource = sites;
            ddlGroupBySite.DataBind();
            ddlGroupBySite.Items.Insert(0, new ListItem("---", "0"));

            ddlGroupByFleet.DataTextField = "Description";
            ddlGroupByFleet.DataValueField = "ExportLevelFleetID";
            List<ExportLevelFleet> fleets = exportLevelBLL.GetAllExportLevelFleet();
            ddlGroupByFleet.DataSource = fleets;
            ddlGroupByFleet.DataBind();
            ddlGroupByFleet.Items.Insert(0, new ListItem("---", "0"));

            ddlScenarioType.DataTextField = "PlanDescription";
            ddlScenarioType.DataValueField = "PlanID";
            var cmsScenarioTypeList = bllcmsReportType.CMSFleetPlanGetAll(false);
            ddlScenarioType.DataSource = cmsScenarioTypeList;
            ddlScenarioType.DataBind();

            ddlAdditionDeletion.Items.Add(new ListItem("Addition", "1"));
            ddlAdditionDeletion.Items.Add(new ListItem("Deletion", "0"));

        }
        
        protected void btnLoadExcel_click(object sender, EventArgs e)
        {
            RaiseBubbleEvent(this, 
                new ExportToExcelEventArgs { 
                    isTextExport = rbExcelExport.Checked ? false : true,
                    isAddition = Convert.ToBoolean(Convert.ToInt32(ddlAdditionDeletion.SelectedItem.Value)),
                    scenarioType = Convert.ToInt32(ddlScenarioType.SelectedItem.Value)
                    } 
                );
        }

        protected void TextExport_CheckedChanged(object sender, EventArgs e)
        {
            ddlGroupByFleet.Enabled = false;
            ddlGroupBySite.Enabled = false;
            String s = FleetSizeExportLogic.GetFleetSizeConfig();
            mmsBtn.Visible=s==FleetSizeExportLogic.FALSE||s==FleetSizeExportLogic.HIDE?false:true;
            SetExportButtonText();
        }

        protected void ExcelExport_CheckedChanged(object sender, EventArgs e)
        {
            ddlGroupByFleet.Enabled = true;
            ddlGroupBySite.Enabled = true;
            mmsBtn.Visible=false;
            SetExportButtonText();
        }

        private void SetExportButtonText()
        {
            btnLoadExcel.Text = rbTextExport.Checked ? "Export To Text" : "Export to Excel";
        }
    }
}