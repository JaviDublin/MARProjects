using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.BLL.VehiclesAbroad.Abstract {

    public interface IDataTableEntity {

        string header { get; set; }
        string rowDefinition { get; set; }
        string theValue { set; get; }
    }
}
