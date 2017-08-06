using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.BLL.VehiclesAbroad.Models.Abstract;
using System.Web.UI.WebControls;

namespace App.BLL.VehiclesAbroad.Models.Filters {
    public class TextFilterModel : ITextFilterModel {
        public TextBox _TextBox { get; set; }
        public string Text {
            get { return _TextBox.Text; }
            set { _TextBox.Text = value; }
        }
        public Label ErrorLabel { get; set; }

        public string ErrorText {
            get { return ErrorLabel.Text; }
            set {
                if (ErrorLabel != null) {
                    ErrorLabel.ForeColor = System.Drawing.Color.Red;
                    ErrorLabel.Text = value;
                }
            }
        }
    }
}