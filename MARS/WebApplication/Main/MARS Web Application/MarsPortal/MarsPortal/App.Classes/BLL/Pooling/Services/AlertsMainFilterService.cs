using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.Pooling.Services.Abstract;
using Mars.BLL.Pooling.Models;
using App.Classes.BLL.Pooling.Models.Abstract;

namespace Mars.Pooling.Services
{
    public class AlertsMainFilterService : MainFilterService<AlertGridviewModel>
    {
        public AlertsMainFilterService(AlertGridviewModel gridviewModel, ICmsOpsLogicModel cmsOpsModel, IThreeFilterCascadeModel carModel)
            : base(gridviewModel, cmsOpsModel, carModel) { }
        public override void FillFilter()
        {
            _gridviewModel._HtmlTable.Filter.Country = _cmsOpsModel.CountryFilterModel.SelectedValue;
            _gridviewModel._HtmlTable.Filter.CmsLogic = _cmsOpsModel.isCMS;
            _gridviewModel._HtmlTable.Filter.PoolRegion = _cmsOpsModel.GeneralThreeFilterModel.TopModel.SelectedValue;
            _gridviewModel._HtmlTable.Filter.LocationGrpArea = _cmsOpsModel.GeneralThreeFilterModel.MiddleModel.SelectedValue;
            _gridviewModel._HtmlTable.Filter.Branch = _cmsOpsModel.GeneralThreeFilterModel.BottomModel.SelectedValue;
            _gridviewModel._HtmlTable.Filter.CarSegment = _carModel.TopModel.SelectedValue;
            _gridviewModel._HtmlTable.Filter.CarClass = _carModel.MiddleModel.SelectedValue;
            _gridviewModel._HtmlTable.Filter.CarGroup = _carModel.BottomModel.SelectedValue;
        }
    }
}