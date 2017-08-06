using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.BLL.VehiclesAbroad.Models;
using App.BLL.VehiclesAbroad.Factories;
using App.DAL.VehiclesAbroad.Abstract;
using App.Classes.DAL.VehiclesAbroad.Abstract;

namespace App.Classes.BLL.VehiclesAbroad.Models {
    public class UtilisationDataTableModel : DataTableModel {

        private IUtilisationHistoryRepository _repository;
        public UtilisationDataTableModel(IUtilisationHistoryRepository r) {
            _repository = r;
        }
        public override void bind(params string[] dependants) {
            DataTableUtilisationHTMLFactory dtf = new DataTableUtilisationHTMLFactory(_repository.getList(Filter));
            DataTable.InnerHtml = dtf.getDataTableAsString(ColumnHeader);
        }
    }
}