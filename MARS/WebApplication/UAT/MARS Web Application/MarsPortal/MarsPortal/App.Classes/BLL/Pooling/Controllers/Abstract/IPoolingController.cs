using System.Web.UI;
using App.Classes.BLL.Pooling.Models.Abstract;
using App.BLL.VehiclesAbroad.Models.Abstract;
using Mars.Pooling.Models;
using App.BLL;

namespace Mars.Pooling.Controllers.Abstract {
    public interface IPoolingController {
        IJavaScriptModel JavascriptModel { get; set; }
        void Initialise(Page p);
        IReservationGridViewModel GridViewModel { get; set; }
        void UpdateView();
        IThreeFilterCascadeModel CarCascadeModel { get; set; }
        ICmsOpsLogicModel CmsOpsModel { get; set; }
        IHeadingModel HeadingModel { get; set; }
        LabelUpdateDBModel LabelUpdateModel { get; set; }
        void CountrySelected();
        void CarSegmentSelected();
        void CarClassSelected();
        void PoolSelected();
        void LocationGroupSelected();
        void CmsLogicSelected();
        void OpsLogicSelected();
        void GridViewPageSelected(int pageIndex);
        void GridViewSortSelected(string sortExpression, string sortDirection);
        void GridViewSelectRowSelected(int rowIndex);
        void UpdateStatistic(ReportStatistics.ReportName reportName);
    }
}
