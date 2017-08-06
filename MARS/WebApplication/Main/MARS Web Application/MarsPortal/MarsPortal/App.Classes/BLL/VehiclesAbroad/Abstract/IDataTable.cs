using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.BLL.VehiclesAbroad.Abstract {

    public interface IDataTable {

        string getDataTableAsString();
        string getDataTableAsString(string firstColumn, bool clickable = true);
    }
}
