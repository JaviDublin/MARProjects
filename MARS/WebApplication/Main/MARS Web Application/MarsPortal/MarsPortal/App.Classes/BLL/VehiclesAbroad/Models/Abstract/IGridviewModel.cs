using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using App.DAL.VehiclesAbroad.Abstract;
using App.Entities.VehiclesAbroad;

namespace App.BLL.VehiclesAbroad.Models.Abstract {

    public interface IGridviewModel {
        ICarFilterEntity CarFilter { get; set; }
        IFilterEntity Filter { get; set; }
        GridView GridViewer { get; set; }
        void bind(params string[] dependants);
        string SortExpression { get; set; }
        string SortDirection { get; }
        string getLicense(int index);
        void buttonCSVDownloadClick();
        string getCSVFilename { get; }
        string CSVDirectory { get; set; }
    }
}
