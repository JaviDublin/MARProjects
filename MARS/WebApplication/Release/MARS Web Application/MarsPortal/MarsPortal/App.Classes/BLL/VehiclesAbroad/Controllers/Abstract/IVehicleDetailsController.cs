using App.BLL.VehiclesAbroad.Models.Abstract;
using App.BLL.VehiclesAbroad.Models.Filters.Abstract;
using App.Classes.BLL.Pooling.Models.Abstract;

namespace App.BLL.VehiclesAbroad.Controllers.Abstract {

    public interface IVehicleDetailsController : IController {
        IFilterModel2 OperstatModel { get; set; }
        IFilterModel2 MoveTypeModel { get; set; }
        IThreeCascadeFilterModel ThreeCascadeModel { get; set; }
        IFilterModel2 OwnCountryModel { get; set; }
        IFilterModel2 CarSegmentModel { get; set; }
        IFilterModel2 CarClassModel { get; set; }
        IFilterModel2 CarGroupModel { get; set; }
        IFilterModel2 VehiclePredicamentModel { get; set; }
        IFilterModel2 NonRevDaysModel { get; set; }
        void DueCountrySelected();
        void PoolSelected();
        void OwnCountrySelected();
        void CarSegmentSelected();
        void CarClassSelected();
        void GridViewPageSelected(int pageIndex);
        void GridViewSortSelected(string sortExpression, string sortDirection);
        void GridViewSelectRowSelected(int rowIndex);
        IGridviewModel GridViewModel { get; set; }
        void UpdateView();
        IVehicleDetailsUserControlModel VehicleDetailsUserControlModel { get; set; }
        IFilterModel PagerMaxModel { get; set; }
        ITextFilterModel UnitTextBoxModel { get; set; }
        ITextFilterModel LicenseTextBoxModel { get; set; }
        ITextFilterModel ModelTextBoxModel { get; set; }
        ITextFilterModel ModelDescTextBoxModel { get; set; }
        ITextFilterModel VinTextBoxModel { get; set; }
        ITextFilterModel CustNameTextBoxModel { get; set; }
        ITextFilterModel ColourTextBoxModel { get; set; }
        ITextFilterModel MileageTextBoxModel { get; set; }
        IButtonModel FilterGridButtonModel { get; set; }
        IButtonModel ClearFiltersButtonModel { get; set; }
        void filterGridButtonClick();
        void clearFiltersClick();
        void downloadCSVButtonClick();
    }
}
