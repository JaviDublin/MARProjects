using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using App.BLL.VehiclesAbroad.Models.Abstract;
using App.BLL.VehiclesAbroad.Abstract;
using App.BLL.VehiclesAbroad.Controllers.Abstract;

namespace App.BLL.VehiclesAbroad.Controllers.Abstract {
    public interface IUtilisationHistoryController : IController {
        IFilterModel CountryFilter { get; set; }
        ITextFilterModel StartDateFilterModel { get; set; }
        ITextFilterModel EndDateFilter { get; set; }
        IDataTableModel DataTableModel { get; set; }
        IChartModel ChartModel { get; set; }
        void UpdateView();
    }
}
