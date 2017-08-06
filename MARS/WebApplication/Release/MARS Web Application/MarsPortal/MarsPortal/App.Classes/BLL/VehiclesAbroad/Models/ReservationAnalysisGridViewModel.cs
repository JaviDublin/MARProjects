using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.BLL.VehiclesAbroad.Models.Abstract;
using System.Web.UI.WebControls;
using App.Entities.VehiclesAbroad;
using App.Classes.DAL.VehiclesAbroad.Abstract;

namespace App.Classes.BLL.VehiclesAbroad.Models {
    public class ReservationAnalysisGridViewModel : IGridviewModel {

        private IReservationAnalysisRepository _repository;

        public ReservationAnalysisGridViewModel(IReservationAnalysisRepository r) {
            _repository = r;
        }
        public GridView GridViewer { get; set; }

        public void bind(params string[] dependants) {
            GridViewer.DataSource = _repository.getDataTable(dependants[0], dependants[1], Convert.ToInt32(dependants[2]));
            GridViewer.DataBind();
        }

        //== Not Used==
        public string SortExpression {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }
        public string SortDirection {
            get { throw new NotImplementedException(); }
        }
        public string getLicense(int index) {
            throw new NotImplementedException();
        }
        public void buttonCSVDownloadClick() {
            throw new NotImplementedException();
        }
        public string getCSVFilename {
            get { throw new NotImplementedException(); }
        }
        public string CSVDirectory {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }
        public ICarFilterEntity CarFilter {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }
        public IFilterEntity Filter {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }
    }
}