using Mars.Pooling.Models.Abstract;
using App.DAL.VehiclesAbroad.Abstract;
using System.Web.UI.WebControls;
using App.Classes.BLL.Pooling.Models;
using Mars.DAL.Pooling.Abstract;
using System;
using System.Linq;
using Mars.Pooling.Entities;
using System.Collections.Generic;

namespace Mars.Pooling.Models {
    public class FilterModel3:FilterModel2,IFilterModel3 {

        public IFilterRepository3 Repository3 { get; set; }
        IList<DropdownEntity> _ddList;

        public FilterModel3(IFilterRepository3 r):base(null){
            if(r==null) throw new ArgumentNullException("The IFilterRepository for the class can't be null.");
           Repository3=r;
            FirstItem=FIRSTITEM;
            _ddList=new List<DropdownEntity>();
        }
        public int GetId() {
            if(SelectedValue=="") return -1;
            return _ddList.Where(p => p.Name==SelectedValue).FirstOrDefault()==null?-1:_ddList.Where(p => p.Name==SelectedValue).FirstOrDefault().Id;
        }
        public override void bind(params string[] dependants) {
            _ddList = Repository3.getList(dependants);
            clear();
            foreach(var item in _ddList)
                FilterDropDownList.Items.Add(item.Name);
            FilterDropDownList.DataBind();
            SetFeedback();
        }
        public override void rebind(int selectedIndex,params string[] dependants) {
            _ddList = Repository3.getList(dependants);
            clear();
            foreach(var item in _ddList)
                FilterDropDownList.Items.Add(item.Name);
            FilterDropDownList.SelectedIndex = selectedIndex;
            FilterDropDownList.DataBind();
            SetFeedback();
        }
        public string GetCode() {
            if(FilterDropDownList==null) return "";
            if(SelectedValue=="") return "";
            return _ddList.Where(p => p.Name==SelectedValue).FirstOrDefault().Code;
        }
    }
}