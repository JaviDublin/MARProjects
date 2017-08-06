using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.BLL.VehiclesAbroad.Models.Abstract {
    public interface INonrevModel {
        string getCountryCode(string s);
        string getArgument(string s);
    }
}
