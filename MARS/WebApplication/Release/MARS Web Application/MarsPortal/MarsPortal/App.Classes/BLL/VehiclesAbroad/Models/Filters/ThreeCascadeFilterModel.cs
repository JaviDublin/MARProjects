using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.BLL.VehiclesAbroad.Models.Abstract;
using App.DAL.VehiclesAbroad.Abstract;
using App.BLL.VehiclesAbroad.Models.Filters;
using App.BLL.VehiclesAbroad.Models.Filters.Abstract;
using App.Classes.BLL.Pooling.Models.Abstract;
using App.Classes.BLL.Pooling.Models;

namespace App.BLL.VehiclesAbroad.Models.Filters {
    public class ThreeCascadeFilterModel : IThreeCascadeFilterModel {

        public ThreeCascadeFilterModel(IFilterRepository d, IFilterRepository p, IFilterRepository l) {
            DueCountryModel = new FilterModel2(d);
            PoolModel = new FilterModel2(p);
            LocationGroupModel = new FilterModel2(l);
        }
        public IFilterModel2 DueCountryModel { get; set; }
        public IFilterModel2 PoolModel { get; set; }
        public IFilterModel2 LocationGroupModel { get; set; }
        public void bind(params string[] dependants) {
            DueCountryModel.bind();
            PoolModel.clear();
            LocationGroupModel.clear();
        }
        public void DueCountrySelected() {
            if (DueCountryModel.SelectedIndex <= 0) PoolModel.clear();
            else PoolModel.bind(DueCountryModel.SelectedValue);
            LocationGroupModel.clear();
        }
        public void PoolSelected() {
            if (PoolModel.SelectedIndex <= 0) LocationGroupModel.clear();
            else LocationGroupModel.bind(PoolModel.SelectedValue);
        }
        public void rebind(int d, int p, int l, string due, string pool) {
            DueCountryModel.rebind(d);
            PoolModel.rebind(p, due);
            LocationGroupModel.rebind(l, pool);
        }
    }
}