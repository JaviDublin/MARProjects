using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.BLL.VehiclesAbroad.Controllers;
using App.BLL.VehiclesAbroad.Controllers.Abstract;

namespace MarsV2.VehiclesAbroad {
    public partial class VehicleDetails2 : System.Web.UI.Page {

        IVehicleDetailsController Controller {
            get {
                if (Session["IVehicleDetailsController"] == null) Session["IVehicleDetailsController"] = new VehicleDetailsController_002();
                return (IVehicleDetailsController)Session["IVehicleDetailsController"];
            }
        }
        protected void Page_Load(object sender, EventArgs e) {
            Controller.OperstatModel.FilterDropDownList = DropDownListOperstat;
            Controller.OperstatModel.FeedbackLabel = labelOperstat;
            Controller.MoveTypeModel.FilterDropDownList = DropDownListMoveType;
            Controller.MoveTypeModel.FeedbackLabel = labelMoveType;
            Controller.ThreeCascadeModel.DueCountryModel.FilterDropDownList = DropDownListDueCountry;
            Controller.ThreeCascadeModel.DueCountryModel.FeedbackLabel = labelDueCountry;
            Controller.ThreeCascadeModel.PoolModel.FilterDropDownList = DropDownListPool;
            Controller.ThreeCascadeModel.PoolModel.FeedbackLabel = labelPool;
            Controller.ThreeCascadeModel.LocationGroupModel.FilterDropDownList = DropDownListLocationGroup;
            Controller.ThreeCascadeModel.LocationGroupModel.FeedbackLabel = labelLocationGroup;
            Controller.OwnCountryModel.FilterDropDownList = DropDownListOwnCountry;
            Controller.OwnCountryModel.FeedbackLabel = labelOwnCounty;
            Controller.CarSegmentModel.FilterDropDownList = DropDownListCarSegment;
            Controller.CarSegmentModel.FeedbackLabel = labelCarSegment;
            Controller.CarClassModel.FilterDropDownList = DropDownListCarClass;
            Controller.CarClassModel.FeedbackLabel = labelCarClass;
            Controller.CarGroupModel.FilterDropDownList = DropDownListCarGroup;
            Controller.CarGroupModel.FeedbackLabel = labelCarGroup;
            Controller.VehiclePredicamentModel.FilterDropDownList = DropDownListVehiclePredicament;
            Controller.VehiclePredicamentModel.FeedbackLabel = labelVehiclePredicament;
            Controller.NonRevDaysModel.FilterDropDownList = dropDownListNonRev;
            Controller.NonRevDaysModel.FeedbackLabel = labelNonRev;
            Controller.GridViewModel.GridViewer = GridViewVehicleDetails;
            Controller.VehicleDetailsUserControlModel.VehiclesAbroadDetailsModal = VehiclesAbroadDetailsUserControl;
            Controller.PagerMaxModel.FilterDropDownList = DropDownListPagerMaxRows;
            Controller.UnitTextBoxModel._TextBox = TextBoxUnit;
            Controller.LicenseTextBoxModel._TextBox = TextBoxLicense;
            Controller.ModelTextBoxModel._TextBox = TextBoxModel;
            Controller.ModelDescTextBoxModel._TextBox = TextBoxModelDescription;
            Controller.VinTextBoxModel._TextBox = TextBoxVin;
            Controller.CustNameTextBoxModel._TextBox = TextBoxCustomerName;
            Controller.ColourTextBoxModel._TextBox = TextBoxColour;
            Controller.MileageTextBoxModel._TextBox = TextBoxMileage;
            Controller.FilterGridButtonModel._Button = ButtonFilterGrid;
            Controller.ClearFiltersButtonModel._Button = ButtonClearFilters;
            Controller.Initialise(Page);
        }
        protected void DropDownListDueCountry_SelectedIndexChanged(object sender, EventArgs e) {
            Controller.DueCountrySelected();
        }
        protected void DropDownListPool_SelectedIndexChanged(object sender, EventArgs e) {
            Controller.PoolSelected();
        }
        protected void DropDownListOwnCountry_SelectedIndexChanged(object sender, EventArgs e) {
            Controller.OwnCountrySelected();
        }
        protected void DropDownListCarSegment_SelectedIndexChanged(object sender, EventArgs e) {
            Controller.CarSegmentSelected();
        }
        protected void DropDownListCarClass_SelectedIndexChanged(object sender, EventArgs e) {
            Controller.CarClassSelected();
        }
        protected void GridViewVehicleDetails_SelectedIndexChanging(object sender, GridViewSelectEventArgs e) {
            Controller.GridViewSelectRowSelected(e.NewSelectedIndex);
        }
        protected void GridViewVehicleDetails_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            Controller.GridViewPageSelected(e.NewPageIndex);
        }
        protected void GridViewVehicleDetails_Sorting(object sender, GridViewSortEventArgs e) {
            Controller.GridViewSortSelected(e.SortExpression, e.SortDirection.ToString());
        }
        protected void UpdateController(object sender, EventArgs e) {
            Controller.UpdateView();
        }
        protected void ButtonFilterGrid_Click(object sender, EventArgs e) {
            Controller.filterGridButtonClick();
        }
        protected void ButtonClearFilters_Click(object sender, EventArgs e) {
            Controller.clearFiltersClick();
        }
        protected void ButtonDownloadCSV_Click(object sender, EventArgs e) {
            Controller.downloadCSVButtonClick();
        }
    }
}