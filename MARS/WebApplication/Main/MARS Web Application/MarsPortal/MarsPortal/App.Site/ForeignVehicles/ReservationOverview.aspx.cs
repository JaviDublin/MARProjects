using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.App.Classes.BLL.ExtensionMethods;
using Mars.App.Classes.Phase4Dal.Enumerators;
using Mars.App.Classes.Phase4Dal.ForeignVehicles;
using Mars.App.Classes.Phase4Dal.ForeignVehicles.Entities;
using Mars.App.Classes.Phase4Dal.Reservations;

namespace Mars.App.Site.ForeignVehicles
{
    public partial class ReservationOverview : Page
    {
        public const string ReservationOverviewSessionTransferName = "ReservationOverviewSessionTransferName";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                using (var dataAccess = new ReservationDataAccess(null))
                {
                    lblLastUpdate.Text = dataAccess.GetLastGwdRequest();
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

            var mergedParameters = ucResParameters.GetParameterDictionary();
            using (var dataAccess = new ReservationOverviewDataAccess(mergedParameters))
            {
                lblLastUpdate.Text = dataAccess.GetLastGwdRequest();

                data = dataAccess.GetReservationOverviewGrid();

                distinctLocations = dataAccess.GetDistinctPickupLocationIds();
            }

            ucVehicleOverview.DistinctLocations = distinctLocations;
            ucVehicleOverview.OverviewData = data;
            var activeCountries = data.Select(d => new CountryHolder { CountryDescription = d.CountryName, CountryId = d.CountryId }).Distinct().ToList();
            ucVehicleOverview.OwningCountries = activeCountries;
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
                var parameters = ucResParameters.SessionStoredReservationParameters;

                if (header != VehicleOverviewDataAccess.TotalString)
                {
                    parameters[DictionaryParameter.LocationCountry] = header;    
                }

                if (row != VehicleOverviewDataAccess.TotalString)
                {


                    if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CheckOutLocationGroup)
                        || parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CheckOutArea))
                    {
                        parameters[DictionaryParameter.CheckOutLocation] = row;
                    }
                    else if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CheckOutPool)
                             || parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CheckOutRegion))
                    {
                        if (parameters[DictionaryParameter.CmsSelected] == true.ToString())
                        {
                            parameters[DictionaryParameter.CheckOutLocationGroup] = row;
                        }
                        else
                        {
                            parameters[DictionaryParameter.CheckOutArea] = row;
                        }
                    }
                    else if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.OwningCountry))
                    {
                        if (parameters[DictionaryParameter.CmsSelected] == true.ToString())
                        {
                            parameters[DictionaryParameter.CheckOutPool] = row;
                        }
                        else
                        {
                            parameters[DictionaryParameter.CheckOutRegion] = row;
                        }
                    }
                    else
                    {
                        parameters[DictionaryParameter.OwningCountry] = row;
                        parameters[DictionaryParameter.CheckOutCountry] = row;
                    }
                }


                Session[ReservationOverviewSessionTransferName] = true.ToString();
                Response.Redirect(@"~\App.Site\ForeignVehicles\ReservationDetails.aspx", true);

                handled = true;
            }


            return handled;
        }


    }
}