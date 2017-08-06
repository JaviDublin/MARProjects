using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.BLL.VehiclesAbroad.Models.Abstract;
using App.DAL.VehiclesAbroad.Abstract;
using System.Web.UI.WebControls;

namespace App.BLL.VehiclesAbroad.Models.Filters {
    public class FilterModel : IFilterModel {

        public static string FIRSTITEM = "***All***";
        public IFilterRepository Repository { get; set; }
        public string FirstItem { get; set; }
        public DropDownList FilterDropDownList { get; set; }

        public FilterModel(IFilterRepository filterRepository) {
            Repository = filterRepository;
            FirstItem = FIRSTITEM;
        }
        public int SelectedIndex {
            get { return FilterDropDownList.SelectedIndex; }
            set { FilterDropDownList.SelectedIndex = value; }
        }
        public virtual string SelectedValue {
            get { return FilterDropDownList.SelectedValue == FIRSTITEM ? "" : FilterDropDownList.SelectedValue; }
            set { FilterDropDownList.SelectedValue = value; }
        }
        public void clear() {
            FilterDropDownList.Items.Clear();
            if (!string.IsNullOrEmpty(FirstItem))
                FilterDropDownList.Items.Add(FirstItem);
        }
        public virtual void bind(params string[] dependants) {
            var q = Repository.getList(dependants);
            clear();
            foreach (var item in q)
                FilterDropDownList.Items.Add(item);
            FilterDropDownList.DataBind();
        }
        public virtual void rebind(int selectedIndex, params string[] dependants) {
            var q = Repository.getList(dependants);
            clear();
            foreach (var item in q)
                FilterDropDownList.Items.Add(item);
            FilterDropDownList.SelectedIndex = selectedIndex;
            FilterDropDownList.DataBind();
        }
    }
}