using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace App.Classes.BLL.Pooling.Models.Abstract {
    public interface IChartButtonModel:IButtonModel2 {
        HtmlGenericControl GridViewPanel { get; set; }
        HtmlGenericControl ChartViewPanel { get; set; }
        void cxPanels();
        void Initialise();
    }
}
