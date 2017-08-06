using System;
using System.Activities.Expressions;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.App.Classes.Phase4Dal;
using Mars.App.Classes.Phase4Dal.Administration.Sublease;
using Mars.App.Classes.Phase4Dal.Administration.Sublease.Entities;
using Mars.App.Classes.Phase4Dal.Enumerators;

namespace Mars.App.Site.Administration.VehiclesSublease
{
    public partial class SubleasePage : Page
    {
        public const string SubleaseListDataSessionHolder = "SubleaseListDataSessionHolder";

        public const string SubleaseVehicleIdsToUpdateSessionHolder = "SubleaseVehicleIdsToUpdateSessionHolder";

        public List<int> VehicleIdsToUpdate
        {
            get { return (List<int>) Session[SubleaseVehicleIdsToUpdateSessionHolder]; }
            set { Session[SubleaseVehicleIdsToUpdateSessionHolder] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            agSubleasedVehicles.GridItemType = typeof(SubleaseDataRow);
            agSubleasedVehicles.SessionNameForGridData = SubleaseListDataSessionHolder;
            agSubleasedVehicles.ColumnHeaders = SubleaseDataRow.HeaderRows;

            if (!IsPostBack)
            {
                FillCountryDropdowns();
                FillVehicleModels();
                RefreshLeasedVehicles();
                tbStartDate.Text = DateTime.Now.ToShortDateString();
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            btnApplyChangesToVehicles.Enabled = VehicleIdsToUpdate != null;
        }

        private void FillVehicleModels()
        {
            lbModels.Items.Clear();
            using (var dataAccess = new SubleaseDataAccess(null))
            {
                var models = dataAccess.GetSubleasedModels();
                lbModels.Items.AddRange(models.ToArray());
            }
        }

        private void FillCountryDropdowns()
        {
            //List<ListItem> owningCountries, rentingCountries;
            List<ListItem> countries, countriesWithoutAllItem;
            using (var dataAccess = new BaseDataAccess())
            {
                //owningCountries = dataAccess.GetOwningCounries();
                //rentingCountries = dataAccess.GetOwningCounries();
                countries = dataAccess.GetOwningCounries();
                countriesWithoutAllItem = dataAccess.GetOwningCounries();
            }
            countries.RemoveAll(d => d.Value == string.Empty);

            countries.Insert(0, ParameterDataAccess.EmptyItem);
            ddlOwningCountry.Items.AddRange(countries.ToArray());
            ddlRentingCountry.Items.AddRange(countries.ToArray());

            countriesWithoutAllItem.RemoveAll(d => d.Value == string.Empty);
            ddlEditRentingCountry.Items.AddRange(countriesWithoutAllItem.ToArray());
        }

        protected void UpdateDataGrid(object sender, EventArgs e)
        {
            RefreshLeasedVehicles();
        }

        protected void ClearAllSubleasedVehicles(object sender, EventArgs e)
        {
            using (var dataAccess = new SubleaseDataAccess(null))
            {
                dataAccess.TruncateSubleasesTable();
            }
            RefreshLeasedVehicles();
        }

        private void RefreshLeasedVehicles()
        {
            var parameters = new Dictionary<DictionaryParameter, string>();
            if (ddlOwningCountry.SelectedValue != ParameterDataAccess.EmptyItem.Value)
            {
                parameters[DictionaryParameter.OwningCountry] = ddlOwningCountry.SelectedValue;
            }
            var rentingCountry = ddlRentingCountry.SelectedValue;

            var models = new List<string>();
            var selectedModels = (from ListItem v in lbModels.Items where v.Selected select v).ToList();
            if (selectedModels.Any()
                && selectedModels.Count() < lbModels.Items.Count)
            {
                models.AddRange((from ListItem v in lbModels.Items where v.Selected select v.Value).ToArray());
            }


            using (var dataAccess = new SubleaseDataAccess(parameters))
            {
                var leasedVehicles = dataAccess.GetSubleasedVehicles(rentingCountry, models);
                agSubleasedVehicles.GridData = leasedVehicles;
            }
            upnlGrid.Update();
        }

        protected void ShowVehiclePopup(object sender, EventArgs e)
        {
            tbVinResults.Text = string.Empty;
            mpeEditVehicles.Show();
        }

        protected void ParseVins(object sendere, EventArgs e)
        {
            var vinsEntered = new List<string>();
            if (tbVinInput.Text.Contains(','))
            {
                vinsEntered.AddRange(tbVinInput.Text.Split(',').Select(s => s.Trim()).ToList());
            }
            else
            {
                vinsEntered.Add(tbVinInput.Text.Trim());
            }
            using (var dataAccess = new SubleaseDataAccess(null))
            {
                var newSubleases = rbAddVehicles.Checked;
                var vehicleIds = dataAccess.GetVehiclesFromVins(vinsEntered, newSubleases);
                VehicleIdsToUpdate = vehicleIds;
            }
            tbVinResults.Text = string.Format(hfVinResult.Value, VehicleIdsToUpdate.Count, vinsEntered.Count);
            mpeEditVehicles.Show();
            btnApplyChangesToVehicles.Enabled = true;
        }

        protected void AddVehiclesSelected(object sender, EventArgs e)
        {
            ChoiceSwitched(true);
        }

        protected void RemoveVehiclesSelected(object sender, EventArgs e)
        {
            ChoiceSwitched(false);
        }

        private void ChoiceSwitched(bool addVehicles)
        {
            VehicleIdsToUpdate = null;
            ddlEditRentingCountry.Enabled = addVehicles;
            tbStartDate.Enabled = addVehicles;
            btnApplyChangesToVehicles.Text = addVehicles ? "Add Vehicles" : "Remove Subleases";
            tbVinResults.Text = string.Empty;
            mpeEditVehicles.Show();
        }

        protected void UpdateVehicles(object sender, EventArgs e)
        {
            using (var dataAccess = new SubleaseDataAccess(null))
            {
                var newSubleases = rbAddVehicles.Checked;
                var rentingCountry = ddlEditRentingCountry.SelectedValue;
                var startDate = DateTime.Parse(tbStartDate.Text);
                dataAccess.AddOrRemoveSubleasedVehicles(VehicleIdsToUpdate, newSubleases, rentingCountry, startDate);
            }
            
            RefreshLeasedVehicles();
        }
    }
}