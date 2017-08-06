using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.BLL.VehiclesAbroad.Models.Abstract;
using App.DAL.VehiclesAbroad.Abstract;
using System.Web.UI.HtmlControls;
using App.Entities.VehiclesAbroad;

namespace App.BLL.VehiclesAbroad.Models {
    public class DataTableModel : IDataTableModel {

        public DataTableModel() { }
        IFleetOverviewRepository _repository;
        public DataTableModel(IFleetOverviewRepository r) {
            _repository = r;
        }
        public IFilterEntity Filter { get; set; }
        public HtmlGenericControl DataTable { get; set; }
        public virtual void bind(params string[] dependants) {
            Factories.DataTableHTMLFactory dtf = new Factories.DataTableHTMLFactory(_repository.getList(Filter));
            DataTable.InnerHtml = dtf.getDataTableAsString(ColumnHeader);
        }
        string _colunmHeader = "";
        public string ColumnHeader {
            get { return _colunmHeader; }
            set { _colunmHeader = value; }
        }
    }
}