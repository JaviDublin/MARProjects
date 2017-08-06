using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using App.DAL.VehiclesAbroad.Abstract;
using System.Web.UI.HtmlControls;
using App.Entities.VehiclesAbroad;

namespace App.BLL.VehiclesAbroad.Models.Abstract {
    public interface IDataTableModel {
        IFilterEntity Filter { get; set; }
        HtmlGenericControl DataTable { get; set; }
        void bind(params string[] dependants);
        string ColumnHeader { get; set; }
    }
}
