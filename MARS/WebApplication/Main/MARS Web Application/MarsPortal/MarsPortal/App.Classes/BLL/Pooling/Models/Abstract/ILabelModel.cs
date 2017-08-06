using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using App.BLL.VehiclesAbroad.Models.Abstract;
using System.Web.UI.WebControls;

namespace App.Classes.BLL.Pooling.Models.Abstract {
    public interface ILabelModel {
        Label TextLabel { get; set; }
        string Text { get; set; }
        Label ErrorLabel { get; set; }
        string ErrorText { get; set; }
        void Update();
    }
}
