using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.Pooling.Services.Abstract;
using App.Classes.BLL.Pooling.Models.Abstract;

namespace Mars.Pooling.Services
{
    public class ReservationFilterService : FilterService<IReservationGridViewModel>, IFilterService
    {

        IDictionary<string, string> _filterValues;
        public ReservationFilterService(IReservationGridViewModel gridviewModel, IDictionary<string, string> filterValues)
            : base(gridviewModel)
        {
            if (filterValues == null) throw new ArgumentNullException("The filterValues argument can not be null");
            _filterValues = filterValues;
        }
        public override void FillFilter()
        {
            _gridviewModel.MainFilters.Country = _filterValues["Country"];
            _gridviewModel.MainFilters.CmsLogic = Convert.ToBoolean(_filterValues["CmsOps"]);
            //GridViewModel.MainFilters.PoolRegion = CmsOpsModel.GeneralThreeFilterModel.TopModel.SelectedValue;
            //GridViewModel.MainFilters.LocationGrpArea = CmsOpsModel.GeneralThreeFilterModel.MiddleModel.SelectedValue;
            //GridViewModel.MainFilters.Branch = CmsOpsModel.GeneralThreeFilterModel.BottomModel.SelectedValue;
            //GridViewModel.MainFilters.CarSegment = CarCascadeModel.TopModel.SelectedValue;
            //GridViewModel.MainFilters.CarClass = CarCascadeModel.MiddleModel.SelectedValue;
            //GridViewModel.MainFilters.CarGroup = CarCascadeModel.BottomModel.SelectedValue;
        }
    }
}