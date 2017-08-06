using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.BLL.VehiclesAbroad.Models.Abstract;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace App.BLL.VehiclesAbroad.Models {
    public class ButtonModel : IButtonModel {
        public Button _Button { get; set; }

        public string _Text {
            get { return _Button.Text; }
            set { _Button.Text = value; }
        }
        public virtual void buttonClick() {
            // do something
        }
    }
}