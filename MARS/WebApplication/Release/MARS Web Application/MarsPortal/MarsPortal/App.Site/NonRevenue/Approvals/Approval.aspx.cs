using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.App.Classes.Phase4Bll;
using Mars.App.Classes.Phase4Bll.Availability;
using Mars.App.Classes.Phase4Dal;
using Mars.App.Classes.Phase4Dal.Availability;
using Mars.App.Classes.Phase4Dal.Enumerators;
using Mars.App.Classes.Phase4Dal.NonRev;
using Mars.App.Classes.Phase4Dal.NonRev.Entities;
using Mars.App.UserControls.Phase4;

namespace Mars.App.Site.NonRevenue.Approvals
{
    public partial class Approval : Page
    {
        private const string ApprovalEntryIdSessionString = "ApprovalEntryIdSessionString";

        public int SelectedTab { get; set; }

        private DateTime ApprovalDateTime
        {
            get { return DateTime.Parse(hfApprovalRequestedDateTime.Value); }
            set { hfApprovalRequestedDateTime.Value = value.ToString(); }
        }

        private List<Tuple<int?, int>> LastEntryIdsApproved
        {
            get { return (List<Tuple<int?, int>>)Session[ApprovalEntryIdSessionString]; }
            set { Session[ApprovalEntryIdSessionString] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                using (var dataAccess = new AvailabilityDataAccess(null))
                {
                    lblLastUpdate.Text = LastUpdatedFromFleetNow.GetLastUpdatedDateTime(dataAccess);
                }
                FillDropdowns();
                ucOverviewGrid.GridData = null;
            }
            lblUploadMessage.Visible = false;
            ucOverviewGrid.Visible = true;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
        }

        protected void ExportToExcel()
        {
            var dataTable = HtmlTableGenerator.GenerateHtmlTableFromGridview(gvHistory);
            Session["ExportData"] = dataTable;
            Session["ExportFileType"] = "xls";
            Session["ExportFileName"] = string.Format("Approval History Export {0}", DateTime.Now.ToShortDateString());
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            SelectedTab = 0;
            LoadVehiclesToApprove();
        }

        protected void btnLoadApproved_Click(object sender, EventArgs e)
        {
            SelectedTab = 1;
            LoadHistory();
        }

        private DateTime ParseMonthSelected()
        {
            var splitInput = ipMonthSelected.Value.Split(' ');

            var month = DateTime.ParseExact(splitInput[0], "MMMM", CultureInfo.CurrentCulture).Month;
            var year = int.Parse(splitInput[1]);

            return new DateTime(year, month, 1);
        }

        private void LoadHistory()
        {
            var owningCountry = ddlApprovedOwningCountry.SelectedValue;
            var locationCountry = ddlApprovedLocationCountry.SelectedValue;
            var selectedMonth = ParseMonthSelected();
            using (var dataAccess = new ApprovalDataAccess(null))
            {
                lblLastUpdate.Text = LastUpdatedFromFleetNow.GetLastUpdatedDateTime(dataAccess);
                upnlUpdatedTime.Update();

                var approvalEntries = dataAccess.GetApprovalEntries(owningCountry, locationCountry, selectedMonth);
                
                gvApprovalHistory.DataSource = approvalEntries;
                
                gvApprovalHistory.DataBind();
            }
            
        }

        protected void gvApprovalHistory_Edit(object sender, CommandEventArgs e)
        {
            SelectedTab = 1;
            if ((e.CommandName == "EditItem"))
            {
                var historyId = int.Parse(e.CommandArgument.ToString());
                pnlHistory.Visible = true;
                List<OverviewGridRow> historyData;
                using (var dataAccess = new ApprovalDataAccess(null))
                {

                    historyData = dataAccess.BuildApprovalList(historyId);

                }
                gvHistory.DataSource = historyData;
                gvHistory.DataBind();
            }
        }


        

        private void LoadVehiclesToApprove()
        {
            var owningCountry = ddlOwningCountry.SelectedValue;
            var locationCountry = ddlLocationCountry.SelectedValue;
            var minDaysNonRev = tbMinDaysNonRev.Text;


            var parameters = new Dictionary<DictionaryParameter, string>
                                   {
                                       {
                                           DictionaryParameter.LocationCountry,
                                           locationCountry
                                       },
                                       {
                                           DictionaryParameter.OwningCountry,
                                           owningCountry
                                       },
                                       {
                                           DictionaryParameter.OperationalStatuses,
                                           NonRevParameters.GetSelectedKeys(lbOperationalStatus.Items)
                                       },
                                       {
                                         DictionaryParameter.FleetTypes,
                                         NonRevParameters.GetSelectedKeys(lbFleet.Items)
                                       },
                                       {
                                           DictionaryParameter.MinDaysNonRev,
                                           minDaysNonRev
                                       },
                                       {
                                           DictionaryParameter.DefleetedVehicles,
                                           "1"
                                       }//Only include IsFleet Vehicles
                                   };

            LastEntryIdsApproved = ucOverviewGrid.LoadGrid(parameters);
            using (var dataAccess = new AvailabilityDataAccess(null))
            {
                lblLastUpdate.Text = LastUpdatedFromFleetNow.GetLastUpdatedDateTime(dataAccess);
            }
            upnlUpdatedTime.Update();

            ApprovalDateTime = DateTime.Now;
        }

 

        protected override bool OnBubbleEvent(object sender, EventArgs args)
        {
            var handled = false;

            if (args is GridViewCommandEventArgs)
            {
                var commandArgs = args as GridViewCommandEventArgs;
                if (commandArgs.CommandName == "ShowVehicle")
                {
                    var vehicleId = int.Parse(commandArgs.CommandArgument.ToString());

                    ucOverviewVehicleHistory.SetVehicleHistoryDetails(vehicleId);
                    ucOverviewVehicle.SetVehicleDetails(vehicleId);

                    mpeNonRevOverview.Show();

                }

                handled = true;
            }
            if (args is CommandEventArgs)
            {
                var commandArgs = args as CommandEventArgs;
                if (commandArgs.CommandName == "KeepPopupOpen")
                {
                    mpeNonRevOverview.Show();
                    handled = true;
                }
                
            }
            if (args is CommandEventArgs)
            {
                var commandArgs = args as CommandEventArgs;
                
                if (commandArgs.CommandName == "Approve Vehicles")
                {
                    var parameters = BuildParameterList();
                    
                    using (var dataAccess = new ApprovalDataAccess(parameters))
                    {
                        var vehiclesApproved = int.Parse(commandArgs.CommandArgument.ToString());
                        var minDaysNonRev = int.Parse(tbMinDaysNonRev.Text);
                        var userId = Rad.Security.ApplicationAuthentication.GetGlobalId();
                        var approvalId = dataAccess.InsertApproval(userId, ApprovalDateTime, vehiclesApproved, minDaysNonRev);
                        dataAccess.InsertApprovalEntries(approvalId, LastEntryIdsApproved);
                        
                    }
                    ucOverviewGrid.Visible = false;
                    lblUploadMessage.Visible = true;
                    handled = true;
                }
                if (commandArgs.CommandName == "IndexChanged")
                {
                    mpeNonRevOverview.Show();
                    handled = true;
                }
                
                if (commandArgs.CommandName == UserControls.Phase4.HelpIcons.ExportToExcel.ExportString)
                {
                    ExportToExcel();
                    handled = true;
                }
            }

            return handled;
        }

        private Dictionary<DictionaryParameter, string> BuildParameterList()
        {
            var parameters = new Dictionary<DictionaryParameter, string>
                             {
                                 {
                                     DictionaryParameter.OwningCountry,
                                     ddlOwningCountry.SelectedValue
                                 },
                                 {
                                     DictionaryParameter.LocationCountry,
                                     ddlLocationCountry.SelectedValue
                                 },
                                 {
                                     DictionaryParameter.OperationalStatuses,
                                     NonRevParameters.GetSelectedKeys(lbOperationalStatus.Items)
                                 },
                                 
                             };
            return parameters;
        }

        private void FillDropdowns()
        {
            var t = DateTime.Now;
            ipMonthSelected.Value = string.Format("{0} {1}", t.ToString("MMMM"), t.Year); ;
            using (var dataAccess = new ApprovalDataAccess(null))
            {
                lbOperationalStatus.Items.AddRange(dataAccess.GetOperationalStatusList().ToArray());
                lbFleet.Items.AddRange(dataAccess.GetFleetTypesList(ModuleType.NonRev).ToArray());
            }

            using (var generator = new ParamaterListItemGenerator())
            {
                var owningCountries = generator.GenerateList(ParameterType.OwningCountry, null);
                var locationCountries = generator.GenerateList(ParameterType.LocationCountry, null);
                ddlOwningCountry.Items.AddRange(owningCountries.ToArray());
                ddlLocationCountry.Items.AddRange(locationCountries.ToArray());
                ddlApprovedOwningCountry.Items.AddRange(owningCountries.ToArray());
                ddlApprovedLocationCountry.Items.AddRange(locationCountries.ToArray());
                ddlApprovedLocationCountry.SelectedIndex = 0;
                ddlApprovedOwningCountry.SelectedIndex = 0;
                ddlOwningCountry.SelectedIndex = 0;
                ddlLocationCountry.SelectedIndex = 0;
            }
        }

    }
}