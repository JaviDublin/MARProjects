using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.BLL.VehiclesAbroad.Models.Abstract;
using System.Web.UI.WebControls;
using App.DAL.VehiclesAbroad.Abstract;
using App.DAL.VehiclesAbroad;
using App.Entities.VehiclesAbroad;
using Mars.App.Classes.DAL.MarsDBContext;
using App.BLL.VehiclesAbroad.Abstract;
using App.BLL.VehiclesAbroad.Concrete;

namespace App.BLL.VehiclesAbroad.Models {
    public class GridviewModel : IGridviewModel {

        GridView _gridViewer;
        IVehicleDetailsRepository _vehicleDetails;
        private string _csvFilename;
        IDictionary<string, string> _sortDic;
        string _sortExpression;
        public ICarFilterEntity CarFilter { get; set; }
        public IFilterEntity Filter { get; set; }
        public string SortExpression {
            get {
                if (string.IsNullOrEmpty(_sortExpression)) return "";
                return _sortExpression;
            }
            set {
                _sortExpression = value;
                if (_sortDic.ContainsKey(value)) _sortDic[value] = _sortDic[value] == "asc" ? "desc" : "asc";
                else _sortDic[value] = "asc"; // set to default
            }
        }
        public string SortDirection {
            get { return _sortExpression == null ? "asc" : _sortDic[_sortExpression]; }
        }
        public string getCSVFilename { get { return _csvFilename; } }
        public string CSVDirectory { get; set; }
        public GridView GridViewer {
            get { return _gridViewer; }
            set { _gridViewer = value; }
        }

        public GridviewModel(IVehicleDetailsRepository vdr) {
            _vehicleDetails = vdr;
            _sortDic = new Dictionary<string, string>();
        }
        public void bind(params string[] dependants) {
            GridViewer.DataSource = (from p in _vehicleDetails.getQueryable(Filter, CarFilter, SortExpression, SortDirection)
                                     select new { p.Lstwwd, Lstdate = p.Lstdate == null ? "" : p.Lstdate.Value.ToShortDateString(), p.Vc, p.Unit, p.License, p.Model, p.Moddesc, p.Duewwd, Duedate = p.Duedate == null ? "" : p.Duedate.Value.ToShortDateString(), p.Duetime, p.Op, p.Mt, p.Driver, p.Doc, p.Lstmlg, p.Nonrev }).ToList();
            GridViewer.DataBind();
        }
        public string getLicense(int index) {
            return GridViewer.Rows[index].Cells[5].Text;
        }
        public void buttonCSVDownloadClick() {
            ICSVDownload<CarSearchDataEntity> csv = new SaveCSVFileVehicleDetails(new CSVGenerator());// can change the CSVGenerator here
            csv.csvList = getCSVList();
            if (csv.downloadFile(CSVDirectory, "CSVFile"))
                _csvFilename = csv.FileName;
            else
                _csvFilename = "error";
        }
        IList<CarSearchDataEntity> getCSVList() {
            IList<CarSearchDataEntity> l = new List<CarSearchDataEntity>();
            var q = (from p in _vehicleDetails.getQueryable(Filter, CarFilter, SortExpression, SortDirection)
                     select p).ToList<ICarSearchDataEntity>();
            foreach (var row in q)
                l.Add((CarSearchDataEntity)row);
            return l;
        }
    }
}