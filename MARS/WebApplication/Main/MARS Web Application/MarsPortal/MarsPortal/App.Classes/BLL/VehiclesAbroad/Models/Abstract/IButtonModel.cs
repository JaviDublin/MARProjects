using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace App.BLL.VehiclesAbroad.Models.Abstract {
    public interface IButtonModel {
        Button _Button { get; set; }
        string _Text { get; set; }
        void buttonClick();
    }
}
