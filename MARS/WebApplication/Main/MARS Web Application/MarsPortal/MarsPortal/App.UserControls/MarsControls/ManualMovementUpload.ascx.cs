using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web.UI.WebControls;
using App.BLL.CMSReportType;
using App.BLL.ExtensionMethods;
using App.BLL.Management;
using App.BLL.Utilities;
using App.Entities;
using App.MasterPages;
using Mars.App.Classes.BLL.ExtensionMethods;
using Microsoft.SqlServer.Dts.Runtime;
using System.Web.UI;
using App.Classes.BLL.Pooling.Models.Abstract;
using Mars.Sizing.Models;
using Mars.DAL.Sizing;
using Mars.BLL.Sizing.Models;
using Mars.DAL.Sizing.Abstract;
using App.Classes.DAL.Pooling.Abstract;

namespace App.UserControls
{
    public partial class MovementUpload : System.Web.UI.UserControl
    {
        private readonly BLLManagement bll = new BLLManagement();
        private readonly BLLcmsReportType bllcmsReportType = new BLLcmsReportType();
        private static readonly string countryDummy = StaticStrings.CountryDummy;
        IFleetPlanModel _fplm;
        IFleetPlanModel _fleetPlanLabelModel{
            get{
                if (_fplm==null) _fplm =  new FleetPlanLabelModel(new FleetPlanWebServiceRepository());
                return _fplm;
            }
        }
        protected void Page_Load(object sender, EventArgs e) {
            _fleetPlanLabelModel.TextLabel = lblUpdateStatus;

            if (!IsPostBack) {
                _fplm.Update();
                PopulateDropDowns();
                BindView();
            }
        }

        protected void ddlCountryList_Click(object sender, EventArgs e)
        {
            BindView();
        }


        private void PopulateDropDowns()
        {
            Country dummy = new Country();
            dummy.CountryDescription = StaticStrings.SelectCountry;
            dummy.CountryID = countryDummy;

            ddlCountryList.DataTextField = "CountryDescription";
            ddlCountryList.DataValueField = "CountryID";

            List<Country> countries = bll.CountryGetAllByRole(this.Page.RadUserId());
            countries.Insert(0, dummy);

            ddlCountryList.DataSource = countries;
            ddlCountryList.DataBind();
            
            if (countries.Count == 2)
                ddlCountryList.SelectedIndex = 1;

            ddlFleetPlan.DataTextField = "PlanDescription";
            ddlFleetPlan.DataValueField = "PlanID";
            var cmsFleetPlanList = bllcmsReportType.CMSFleetPlanGetAll(false);
            ddlFleetPlan.DataSource = cmsFleetPlanList;
            ddlFleetPlan.DataBind();

            ddlAdditionDeletion.Items.Add(new ListItem("Addition", "1"));
            ddlAdditionDeletion.Items.Add(new ListItem("Deletion", "0"));
        }


        private void WriteLine(string txt)
        {
            //var sw = File.AppendText("C:\\temp\\LogFile.txt");
            //sw.WriteLine(txt);
            //sw.Close();
        }
        protected void btnUpload_Click(object sender, EventArgs e) {
            WriteLine("Go");
            if(!fupMovementUpload.HasFile) {
                lblStatus.Text = @"Please select a file for uploading.";
                return;
            }
            if (Path.GetExtension(fupMovementUpload.FileName).ToLower() != ".txt") {
                lblStatus.Text = @"You must first select a .txt file for uploading.";
                return;
            }
            string country = ddlCountryList.SelectedItem.Value;
            if(country == countryDummy) {
                lblStatus.Text = @"Please select a country from the drop down menu.";
                return;
            }
            WriteLine("start");
            if(_fplm.GetStatus().Contains(FleetPlanOptions.Running.ToString())) { _fplm.Update(); return; }
            try {
                
                string fp = ddlFleetPlan.SelectedItem.Text;
                string uploadTypeName = ddlAdditionDeletion.SelectedItem.Text;

                string savedFileName = country + "_" + fp + "_" + uploadTypeName + ".txt";

                string path = ConfigAccess.GetFleetPlanUploadLocation();
                string fullSavedFileName = Path.Combine(path,savedFileName);
                WriteLine("1");
                fupMovementUpload.SaveAs(fullSavedFileName);
                WriteLine("2");
                //file uploaded call ssis package
                string SSISpackagePath = ConfigAccess.GetSSISPackagePath();
                WriteLine("3");
                Microsoft.SqlServer.Dts.Runtime.Application app = new Microsoft.SqlServer.Dts.Runtime.Application();
                WriteLine("4");
                using(Package package = app.LoadPackage(SSISpackagePath,null)) 
                {
                    WriteLine("5");
                    //Global Package Variable
                    Variables vars = package.Variables;
                    
                    string SSISpackageVariable = ConfigAccess.GetSSISPackageVariable();
                    WriteLine("6");
                    vars[SSISpackageVariable].Value = savedFileName;
                    
                    Microsoft.SqlServer.Dts.Runtime.DTSExecResult results = package.Execute();
                    WriteLine("7");

                    
                    if(results == DTSExecResult.Success) {
                        //create archive
                        var fleetPlanID = Convert.ToInt32(ddlFleetPlan.SelectedItem.Value);
                        var isAddition = Convert.ToBoolean(Convert.ToInt32(ddlAdditionDeletion.SelectedItem.Value));
                        var user = this.Page.RadUsername() + " (" + this.Page.RadUserId() + ")";
                        bll.ForecastOperationalFleetUpdate(DateTime.Today,fleetPlanID,country);
                        bll.FleetPlanEntryUploadArchiveCreate(user,fupMovementUpload.FileName,savedFileName,fleetPlanID,country,isAddition);

                        lblStatus.Text = @"Upload status: File uploaded! - Processing file status: " + results.ToString();
                        //_fplm.UpdateTables();
                    }
                    else {
                        StringBuilder errorBuilder = new StringBuilder();
                        foreach(DtsError error in package.Errors) {
                            errorBuilder.AppendLine(error.Description);
                            lblStatus.Text = "File uploaded, but error occured during processing, " + results.ToString() + " ,\n " + errorBuilder.ToString();
                        }
                    }
                }
            }
            catch(Exception ex) {
                lblStatus.Text = @"Operation failed. The following error occured: " + ex.Message;
            }
            finally {
                BindView();
            }
        }

        protected void ddlCountryList_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindView();
        }

        private void BindView()
        {
            var country = ddlCountryList.SelectedItem.Value;
            activityLog.BindGridView(country);
        }
    }
}