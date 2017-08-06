using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace App.Classes.BLL.Pooling.Models.Abstract {
    public interface IBrowserParamsModel {
        HiddenField BrowserHeight { get; set; }
        HiddenField BrowserWidth { get; set; }
        void SetJavaScript(System.Web.UI.Page p);
        int getBrowserHeight();
        int getBrowserWidth();
    }
}
