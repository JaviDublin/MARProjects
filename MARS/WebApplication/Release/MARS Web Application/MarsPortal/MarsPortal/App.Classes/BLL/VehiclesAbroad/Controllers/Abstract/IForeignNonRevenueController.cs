using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using App.BLL.VehiclesAbroad.Models.Abstract;

namespace App.BLL.VehiclesAbroad.Controllers.Abstract {
    public interface IForeignNonRevenueController : IController {
        IDataTableModel DataTableModel { get; set; }
        IFilterModel NonRevDaysModel { get; set; }
        IFilterModel OwnCountryModel { get; set; }
        string Condition { get; }
        string OwnCountry { get; }
        string DueCountry { get; }
        void UpdateView();
    }
}
