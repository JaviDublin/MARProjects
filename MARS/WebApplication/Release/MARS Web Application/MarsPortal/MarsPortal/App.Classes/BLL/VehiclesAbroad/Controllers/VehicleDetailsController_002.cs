using System;
using App.BLL.VehiclesAbroad.Controllers.Abstract;
using App.BLL.VehiclesAbroad.Models.Abstract;
using App.BLL.VehiclesAbroad.Models.Filters;
using App.DAL.VehiclesAbroad.Filters;
using App.BLL.VehiclesAbroad.Models;
using App.DAL.VehiclesAbroad;
using App.Entities.VehiclesAbroad;
using App.BLL.Utilities;
using App.BLL.VehiclesAbroad.Models.Filters.Abstract;
using App.Classes.BLL.Pooling.Models;
using App.Classes.BLL.Pooling.Models.Abstract;

namespace App.BLL.VehiclesAbroad.Controllers {

    public class VehicleDetailsController_002 : IVehicleDetailsController {
        public IFilterModel2 OperstatModel { get; set; }
        public IFilterModel2 MoveTypeModel { get; set; }
        public IThreeCascadeFilterModel ThreeCascadeModel { get; set; }
        public IFilterModel2 OwnCountryModel { get; set; }
        public IFilterModel2 CarSegmentModel { get; set; }
        public IFilterModel2 CarClassModel { get; set; }
        public IFilterModel2 CarGroupModel { get; set; }
        public IFilterModel2 VehiclePredicamentModel { get; set; }
        public IFilterModel2 NonRevDaysModel { get; set; }
        public IGridviewModel GridViewModel { get; set; }
        IFilterEntity _filter;
        ICarFilterEntity _carFilter;
        private System.Web.UI.Page _page;
        public IVehicleDetailsUserControlModel VehicleDetailsUserControlModel { get; set; }
        public IFilterModel PagerMaxModel { get; set; }
        public ITextFilterModel UnitTextBoxModel { get; set; }
        public ITextFilterModel LicenseTextBoxModel { get; set; }
        public ITextFilterModel ModelTextBoxModel { get; set; }
        public ITextFilterModel ModelDescTextBoxModel { get; set; }
        public ITextFilterModel VinTextBoxModel { get; set; }
        public ITextFilterModel CustNameTextBoxModel { get; set; }
        public ITextFilterModel ColourTextBoxModel { get; set; }
        public ITextFilterModel MileageTextBoxModel { get; set; }
        public App.BLL.VehiclesAbroad.Models.Abstract.IButtonModel FilterGridButtonModel { get; set; }
        public App.BLL.VehiclesAbroad.Models.Abstract.IButtonModel ClearFiltersButtonModel { get; set; }

        public VehicleDetailsController_002() {
            OperstatModel = new FilterModel2(new OperstatRepository());
            MoveTypeModel = new FilterModel2(new MoveTypeRepository());
            ThreeCascadeModel = new ThreeCascadeFilterModel(new FleetEuropeActualDueCountriesRepository(), new PoolRepository(), new LocationRepository());
            OwnCountryModel = new FilterModel2(new FleetActualEuropeOwnCountriesRepository());
            CarSegmentModel = new FilterModel2(new CarSegmentRepository());
            CarClassModel = new FilterModel2(new CarClassRepository());
            CarGroupModel = new FilterModel2(new CarGroupRepository());
            VehiclePredicamentModel = new FilterModel2(new VehiclePredicamentRepository());
            NonRevDaysModel = new NonRevFilterModel(new NonrevRepository());
            GridViewModel = new GridviewModel(new VehicleDetailsRepository());
            _filter = new FilterEntity();
            _carFilter = new CarFilterEntity();
            VehicleDetailsUserControlModel = new VehicleDetailsUserControlModel(new VehicleDetailsRepository());
            PagerMaxModel = new PagerMaxRowsModel(new GridViewMaxRowsRepository());
            UnitTextBoxModel = new TextFilterModel();
            LicenseTextBoxModel = new TextFilterModel();
            ModelTextBoxModel = new TextFilterModel();
            ModelDescTextBoxModel = new TextFilterModel();
            VinTextBoxModel = new TextFilterModel();
            CustNameTextBoxModel = new TextFilterModel();
            ColourTextBoxModel = new TextFilterModel();
            MileageTextBoxModel = new TextFilterModel();
            FilterGridButtonModel = new ButtonModel();
            ClearFiltersButtonModel = new ButtonModel();
        }
        public void Initialise(System.Web.UI.Page p) {
            _page = p;
            if (p.IsPostBack) return;
            if (_page.Session["IFleetOverviewController"] != null)
                repopulateFilters();
            else {
                ThreeCascadeModel.bind();
                OwnCountryModel.bind();
                CarSegmentModel.clear();
                CarClassModel.clear();
                CarGroupModel.clear();
                VehiclePredicamentModel.bind();
                VehiclePredicamentModel.SelectedIndex = 3;
                if (_page.Session["IForeignNonRevenueController"] != null) fromNonRev();
            }
            OperstatModel.bind();
            MoveTypeModel.bind();
            NonRevDaysModel.FirstItem = "";
            NonRevDaysModel.bind();
            PagerMaxModel.bind();
            UpdateView();
        }
        private void fromNonRev() {
            IForeignNonRevenueController fnr = (IForeignNonRevenueController)_page.Session["IForeignNonRevenueController"];
            ThreeCascadeModel.DueCountryModel.SelectedValue = fnr.DueCountry;
            OwnCountryModel.SelectedValue = fnr.OwnCountry;
            VehiclePredicamentModel.SelectedIndex = 7;
            OperstatModel.FilterDropDownList.SelectedValue = fnr.Condition == "Rent" ? "RT" : fnr.Condition;
            NonRevDaysModel.SelectedIndex = fnr.NonRevDaysModel.SelectedIndex;
            //_page.Session["IForeignNonRevenueController"] = null;
        }
        private void repopulateFilters() {
            IFleetOverviewController foc = (IFleetOverviewController)_page.Session["IFleetOverviewController"];
            ThreeCascadeModel.rebind(foc.ThreeCascadeModel.DueCountryModel.SelectedIndex, foc.ThreeCascadeModel.PoolModel.SelectedIndex, foc.ThreeCascadeModel.LocationGroupModel.SelectedIndex, foc.ThreeCascadeModel.DueCountryModel.SelectedValue, foc.ThreeCascadeModel.PoolModel.SelectedValue);
            OwnCountryModel.rebind(foc.OwnCountryModel.SelectedIndex);
            CarSegmentModel.rebind(foc.CarSegmentModel.SelectedIndex, OwnCountryModel.SelectedValue);
            CarClassModel.rebind(foc.CarClassModel.SelectedIndex, OwnCountryModel.SelectedValue, CarSegmentModel.SelectedValue);
            CarGroupModel.rebind(foc.CarClassModel.SelectedIndex, OwnCountryModel.SelectedValue, CarSegmentModel.SelectedValue, CarClassModel.SelectedValue);
            VehiclePredicamentModel.rebind(foc.VehiclePredicamentModel.SelectedIndex);
            //_page.Session["IFleetOverviewController"] = null;
        }
        public void DueCountrySelected() {
            ThreeCascadeModel.DueCountrySelected();
            UpdateView();
        }
        public void PoolSelected() {
            ThreeCascadeModel.PoolSelected();
            UpdateView();
        }
        public void OwnCountrySelected() {
            if (OwnCountryModel.SelectedIndex <= 0) CarSegmentModel.clear();
            else CarSegmentModel.bind(OwnCountryModel.SelectedValue);
            CarClassModel.clear();
            CarGroupModel.clear();
            UpdateView();
        }
        public void CarSegmentSelected() {
            if (CarSegmentModel.SelectedIndex <= 0) CarClassModel.clear();
            else CarClassModel.bind(OwnCountryModel.SelectedValue, CarSegmentModel.SelectedValue);
            CarGroupModel.clear();
            UpdateView();
        }
        public void CarClassSelected() {
            if (CarClassModel.SelectedIndex <= 0) CarGroupModel.clear();
            else CarGroupModel.bind(OwnCountryModel.SelectedValue, CarSegmentModel.SelectedValue, CarClassModel.SelectedValue);
            UpdateView();
        }
        public void UpdateView() {
            _filter.CarClass = CarClassModel.SelectedValue;
            _filter.CarGroup = CarGroupModel.SelectedValue;
            _filter.CarSegment = CarSegmentModel.SelectedValue;
            _filter.DueCountry = ThreeCascadeModel.DueCountryModel.SelectedValue;
            _filter.OwnCountry = OwnCountryModel.SelectedValue;
            _filter.VehiclePredicament = VehiclePredicamentModel.SelectedIndex;
            _filter.nonRev = Convert.ToInt32(NonRevDaysModel.SelectedValue == "" ? "0" : NonRevDaysModel.SelectedValue);
            _filter.Operstat = OperstatModel.SelectedValue;
            _filter.MoveType = MoveTypeModel.SelectedValue;
            _filter.Pool = ThreeCascadeModel.PoolModel.SelectedValue;
            _filter.Location = ThreeCascadeModel.LocationGroupModel.SelectedValue;
            setFeedback();
            GridViewModel.Filter = _filter;
            GridViewModel.CarFilter = _carFilter;
            GridViewModel.GridViewer.PageSize = Convert.ToInt32(PagerMaxModel.SelectedValue == "" ? "10" : PagerMaxModel.SelectedValue);
            GridViewModel.bind();
        }
        void setFeedback() {
            OperstatModel.SetFeedback();
            MoveTypeModel.SetFeedback();
            VehiclePredicamentModel.SetFeedback();
            NonRevDaysModel.SetFeedback();
            ThreeCascadeModel.DueCountryModel.SetFeedback();
            ThreeCascadeModel.PoolModel.SetFeedback();
            ThreeCascadeModel.LocationGroupModel.SetFeedback();
            OwnCountryModel.SetFeedback();
            CarSegmentModel.SetFeedback();
            CarClassModel.SetFeedback();
            CarGroupModel.SetFeedback();
        }
        public void GridViewPageSelected(int pageIndex) {
            GridViewModel.GridViewer.PageIndex = pageIndex;
            UpdateView();
        }
        public void GridViewSortSelected(string sortExpression, string sortDirection) {
            //if (GridViewModel.SortExpression == sortExpression)
            //    GridViewModel.SortDirection = sortDirection.Contains("Ascending") ? "DESC" : "ASC";
            //else {
            //    GridViewModel.SortExpression = sortExpression;
            //    GridViewModel.SortDirection = sortDirection;
            //}
            GridViewModel.SortExpression = sortExpression;
            UpdateView();
        }
        public void GridViewSelectRowSelected(int rowIndex) {
            VehicleDetailsUserControlModel.bind(GridViewModel.getLicense(rowIndex));
        }
        public void filterGridButtonClick() {
            fillCarFilter();
            UpdateView();
        }
        public void clearFiltersClick() {
            clearCarFilters();
            fillCarFilter();
            UpdateView();
        }
        public void downloadCSVButtonClick() {
            string tempDirectory = Mars.Properties.Settings.Default.TempDirectory;
            Helper.DeleteTempFiles(tempDirectory);
            GridViewModel.CSVDirectory = _page.Server.MapPath(tempDirectory);
            GridViewModel.buttonCSVDownloadClick();
            App.Classes.BLL.Workers.VehiclesAbroadWorker.downloadFile(GridViewModel.getCSVFilename, tempDirectory, _page);
        }
        //helper
        void fillCarFilter() {
            _carFilter.Unit = UnitTextBoxModel.Text;
            _carFilter.License = LicenseTextBoxModel.Text;
            _carFilter.Model = ModelTextBoxModel.Text;
            _carFilter.ModelDesc = ModelDescTextBoxModel.Text;
            _carFilter.Vin = VinTextBoxModel.Text;
            _carFilter.Name = CustNameTextBoxModel.Text;
            _carFilter.Colour = ColourTextBoxModel.Text;
            _carFilter.Mileage = MileageTextBoxModel.Text;
        }
        void clearCarFilters() {
            UnitTextBoxModel.Text = "";
            LicenseTextBoxModel.Text = "";
            ModelTextBoxModel.Text = "";
            ModelDescTextBoxModel.Text = "";
            VinTextBoxModel.Text = "";
            CustNameTextBoxModel.Text = "";
            ColourTextBoxModel.Text = "";
            MileageTextBoxModel.Text = "";
        }
    }
}