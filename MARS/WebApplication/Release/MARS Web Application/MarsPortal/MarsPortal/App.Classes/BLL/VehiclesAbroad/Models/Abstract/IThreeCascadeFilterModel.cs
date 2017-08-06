using System;
using App.BLL.VehiclesAbroad.Models.Abstract;
using App.Classes.BLL.Pooling.Models.Abstract;
namespace App.BLL.VehiclesAbroad.Models.Filters.Abstract {
    public interface IThreeCascadeFilterModel {
        void bind(params string[] dependants);
        IFilterModel2 DueCountryModel { get; set; }
        void DueCountrySelected();
        IFilterModel2 LocationGroupModel { get; set; }
        IFilterModel2 PoolModel { get; set; }
        void PoolSelected();
        void rebind(int d, int p, int l, string due, string pool);
    }
}
