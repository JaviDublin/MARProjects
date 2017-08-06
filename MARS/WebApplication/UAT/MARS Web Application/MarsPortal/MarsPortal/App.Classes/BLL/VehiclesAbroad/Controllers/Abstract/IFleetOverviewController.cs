using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using App.BLL.VehiclesAbroad.Models.Abstract;
using System.Web.UI.HtmlControls;
using App.BLL.VehiclesAbroad.Models.Filters.Abstract;
using App.Classes.BLL.Pooling.Models.Abstract;

namespace App.BLL.VehiclesAbroad.Controllers.Abstract {
    public interface IFleetOverviewController : IController {
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
        void UpdateView();
        IDataTableModel DataTableModel { get; set; }
    }
}
