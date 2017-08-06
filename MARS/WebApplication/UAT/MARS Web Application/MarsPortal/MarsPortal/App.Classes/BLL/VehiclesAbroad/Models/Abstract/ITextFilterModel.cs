using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace App.BLL.VehiclesAbroad.Models.Abstract {
    public interface ITextFilterModel {
        TextBox _TextBox { get; set; }
        string Text { get; set; }
        Label ErrorLabel { get; set; }
        string ErrorText { get; set; }
    }
}
