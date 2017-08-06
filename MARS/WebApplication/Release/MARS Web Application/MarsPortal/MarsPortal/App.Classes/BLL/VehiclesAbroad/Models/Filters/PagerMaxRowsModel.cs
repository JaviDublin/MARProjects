using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.BLL.VehiclesAbroad.Models.Abstract;
using System.Web.UI.WebControls;
using App.DAL.VehiclesAbroad.Abstract;

namespace App.BLL.VehiclesAbroad.Models.Filters {
    public class PagerMaxRowsModel : IFilterModel {

        public IFilterRepository Repository { get; set; }
        public string FirstItem { get; set; }
        public DropDownList FilterDropDownList { get; set; }
        public int SelectedIndex { get { throw new NotImplementedException(); } set { throw new NotImplementedException(); } }

        public string SelectedValue {
            get { return FilterDropDownList.SelectedValue.Split(' ')[0]; }
            set { FilterDropDownList.SelectedValue = value; }
        }
        public PagerMaxRowsModel() { }
        public PagerMaxRowsModel(IFilterRepository repository) {
            Repository = repository;
        }
        public void clear() { throw new NotImplementedException(); }
        public void bind(params string[] dependants) {
            var q = Repository.getList();
            FilterDropDownList.Items.Clear(); // cheat!!!!
            foreach (var item in q)
                FilterDropDownList.Items.Add(item);
            FilterDropDownList.DataBind();
        }
        public void rebind(int selectedIndex, params string[] dependants) {
            var q = Repository.getList();
            FilterDropDownList.Items.Clear(); // cheat!!!!
            foreach (var item in q)
                FilterDropDownList.Items.Add(item);
            FilterDropDownList.SelectedIndex = selectedIndex;
            FilterDropDownList.DataBind();
        }
    }
}