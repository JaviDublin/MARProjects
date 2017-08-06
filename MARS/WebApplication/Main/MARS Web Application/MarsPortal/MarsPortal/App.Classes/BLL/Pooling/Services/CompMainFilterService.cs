using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.Pooling.Models.Abstract;
using App.Classes.BLL.Pooling.Models.Abstract;

namespace Mars.Pooling.Services {
    public class CompMainFilterService:MainFilterWithTopicService<ICompGridModel> {
        public CompMainFilterService(ICompGridModel gridviewModel,ICmsOpsLogicModel cmsOpsModel,IThreeFilterCascadeModel carModel,IFilterModel2 topicFilter)
            : base(gridviewModel,cmsOpsModel,carModel,topicFilter) { }
        public override void FillFilter() {
            _gridviewModel._HtmlTable.Filter.Country = _cmsOpsModel.CountryFilterModel.SelectedValue;
            _gridviewModel._HtmlTable.Filter.CmsLogic = _cmsOpsModel.isCMS;
            _gridviewModel._HtmlTable.Filter.PoolRegion = _cmsOpsModel.GeneralThreeFilterModel.TopModel.SelectedValue;
            _gridviewModel._HtmlTable.Filter.LocationGrpArea = _cmsOpsModel.GeneralThreeFilterModel.MiddleModel.SelectedValue;
            _gridviewModel._HtmlTable.Filter.Branch = _cmsOpsModel.GeneralThreeFilterModel.BottomModel.SelectedValue;
            _gridviewModel._HtmlTable.Filter.CarSegment = _carModel.TopModel.SelectedValue;
            _gridviewModel._HtmlTable.Filter.CarClass = _carModel.MiddleModel.SelectedValue;
            _gridviewModel._HtmlTable.Filter.CarGroup = _carModel.BottomModel.SelectedValue;
            _gridviewModel._HtmlTable.Filter.Topic=_topicFilter.SelectedValue;
        }
    }
}