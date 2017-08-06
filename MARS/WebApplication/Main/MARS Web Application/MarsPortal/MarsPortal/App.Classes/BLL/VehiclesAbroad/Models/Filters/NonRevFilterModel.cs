using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.DAL.VehiclesAbroad.Abstract;
using App.Classes.BLL.Pooling.Models;

namespace App.BLL.VehiclesAbroad.Models.Filters {
    public class NonRevFilterModel : FilterModel2 {

        public NonRevFilterModel(IFilterRepository filterRepository) : base(filterRepository) { }

        public override string SelectedValue {
            get {
                if (string.IsNullOrEmpty(FilterDropDownList.SelectedValue)) return "0";
                return FilterDropDownList.SelectedValue == FIRSTITEM ? "0" : FilterDropDownList.SelectedValue.Split(' ')[0];
            }
            set { FilterDropDownList.SelectedValue = value; }
        }
    }
}