using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.BLL.VehiclesAbroad.Abstract;

namespace App.Entities.VehiclesAbroad {

    public class DataTableEntity : IDataTableEntity {

        public string header { get; set; }
        public string rowDefinition { get; set; }
        public string theValue { set; get; }
    }
}
