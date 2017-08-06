using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.App.Classes.BLL.ExtensionMethods;
using Mars.App.Classes.Phase4Bll.Availability;
using Mars.App.Classes.Phase4Dal.Administration.Membership;
using Mars.App.Classes.Phase4Dal.Availability;
using Mars.App.Classes.Phase4Dal.Enumerators;
using Mars.App.Classes.Phase4Dal.ForeignVehicles;
using Mars.App.Classes.Phase4Dal.ForeignVehicles.Entities;

namespace Mars.App.Site.ForeignVehicles
{
    public partial class FleetOverview : Page
    {
        public const string FleetOverviewSessionTransferName = "FleetOverviewSessionTransferName";

        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {
                using (var dataAccess = new AvailabilityDataAccess(null))
                {
                    lblLastUpdate.Text = LastUpdatedFromFleetNow.GetLastUpdatedDateTime(dataAccess);
                }
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            RefreshGrid();
        }

        private void RefreshGrid()
        {
            
            List<OverviewGridItemHolder> data;
            Dictionary<string, string> distinctLocations;
            //List<CountryHolder> owningCountries;
            
            var mergedParameters = ucParameters.GetParameterDictionary();
            using (var dataAccess = new VehicleOverviewDataAccess(mergedParameters))
            {
                lblLastUpdate.Text = LastUpdatedFromFleetNow.GetLastUpdatedDateTime(dataAccess);

                data = dataAccess.GetForeignVehicleOverviewGrid();
            
                distinctLocations = dataAccess.GetDistinctOwningLocationIds();
                //owningCountries = dataAccess.GetActiveOwningCountryHolders();
            }
            
            ucVehicleOverview.DistinctLocations = distinctLocations;
            ucVehicleOverview.OverviewData = data;

            var countriesThatOwn = data.Select(d => new CountryHolder { CountryDescription = d.CountryName, CountryId = d.CountryId }).Distinct().ToList();

            ucVehicleOverview.OwningCountries = countriesThatOwn;
            pnlOverviewGrid.Visible = true;

            //ucVehicleOverview.AddColumns();
        }


        protected override bool OnBubbleEvent(object sender, EventArgs args)
        {
            var handled = false;

            if (args is GridViewCommandEventArgs)
            {
                var commandArgs = args as GridViewCommandEventArgs;
                var row = commandArgs.CommandName;
                var header = commandArgs.CommandArgument.ToString();
                var parameters = ucParameters.SessionStoredAvailabilityParameters;

                if (header != VehicleOverviewDataAccess.TotalString)
                {
                    parameters[DictionaryParameter.OwningCountry] = header;
                }

                if (row != VehicleOverviewDataAccess.TotalString)
                {


                    if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.LocationGroup)
                        || parameters.ContainsValueAndIsntEmpty(DictionaryParameter.Area))
                    {
                        parameters[DictionaryParameter.Location] = row;
                    }
                    else if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.Pool)
                             || parameters.ContainsValueAndIsntEmpty(DictionaryParameter.Region))
                    {
                        if (parameters[DictionaryParameter.CmsSelected] == true.ToString())
                        {
                            parameters[DictionaryParameter.LocationGroup] = row;
                        }
                        else
                        {
                            parameters[DictionaryParameter.Area] = row;
                        }
                    }
                    else if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.LocationCountry))
                    {
                        if (parameters[DictionaryParameter.CmsSelected] == true.ToString())
                        {
                            parameters[DictionaryParameter.Pool] = row;
                        }
                        else
                        {
                            parameters[DictionaryParameter.Region] = row;
                        }
                    }
                    else
                    {

                        parameters[DictionaryParameter.LocationCountry] = row;
                    }
                }
                


                Session[FleetOverviewSessionTransferName] = true.ToString();
                Response.Redirect(@"~\App.Site\ForeignVehicles\ForeignVehicleSearch.aspx", true);

                handled = true;
                
            }
            

            return handled;
        }




    }
}