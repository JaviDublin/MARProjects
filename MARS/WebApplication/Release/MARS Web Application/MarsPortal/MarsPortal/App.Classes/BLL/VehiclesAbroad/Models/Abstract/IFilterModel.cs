using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Web.UI.WebControls;
using App.DAL.VehiclesAbroad.Abstract;

namespace App.BLL.VehiclesAbroad.Models.Abstract
{
    public interface IFilterModel
    {
        IFilterRepository Repository { get; set; }
        string FirstItem { get; set; }
        DropDownList FilterDropDownList { get; set; }
        int SelectedIndex { get; set; }
        string SelectedValue { get; set; }
        void clear();
        void bind(params string[] dependants);
        void rebind(int selectedIndex, params string[] dependants);
    }
}
