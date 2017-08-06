using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.Pooling.Models;
using App.Classes.BLL.Pooling.Models.Abstract;

namespace Mars.Pooling.Services {
    public class DayActualMainFilterService:MainFilterService<ActualsModel> {
        public DayActualMainFilterService(ActualsModel gridviewModel,ICmsOpsLogicModel cmsOpsModel,IThreeFilterCascadeModel carModel)
            : base(gridviewModel,cmsOpsModel,carModel) { }
        public override void FillFilter() {
            _gridviewModel.MainFilters.Country = _cmsOpsModel.CountryFilterModel.SelectedValue;
            _gridviewModel.MainFilters.CmsLogic = _cmsOpsModel.isCMS;
            _gridviewModel.MainFilters.PoolRegion = _cmsOpsModel.GeneralThreeFilterModel.TopModel.SelectedValue;
            _gridviewModel.MainFilters.LocationGrpArea = _cmsOpsModel.GeneralThreeFilterModel.MiddleModel.SelectedValue;
            _gridviewModel.MainFilters.Branch = _cmsOpsModel.GeneralThreeFilterModel.BottomModel.SelectedValue;
            _gridviewModel.MainFilters.CarSegment = _carModel.TopModel.SelectedValue;
            _gridviewModel.MainFilters.CarClass = _carModel.MiddleModel.SelectedValue;
            _gridviewModel.MainFilters.CarGroup = _carModel.BottomModel.SelectedValue;
        }
    }
}