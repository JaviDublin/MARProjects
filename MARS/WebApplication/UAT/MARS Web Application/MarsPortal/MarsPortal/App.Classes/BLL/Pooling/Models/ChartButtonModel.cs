using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Classes.BLL.Pooling.Models.Abstract;
using App.Classes.DAL.Pooling.Abstract;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace App.Classes.BLL.Pooling.Models {
    public class ChartButtonModel : ButtonModel2, IChartButtonModel {

        private bool _panelVisible;
        public ChartButtonModel(IButtonModelRepository r)
            : base(r) { _panelVisible = true; }
        public HtmlGenericControl GridViewPanel { get; set; }
        public HtmlGenericControl ChartViewPanel { get; set; }

        public void cxPanels() {
            GridViewPanel.Visible = _panelVisible;
            ChartViewPanel.Visible = !_panelVisible;
            _Button.Text = _panelVisible ? Repository.getLabel(Enums.buttons.SwitchToChart) : Repository.getLabel(Enums.buttons.SwitchToGrid);
            _panelVisible = !_panelVisible;
        }
        public void Initialise(){
            _panelVisible=true;
            cxPanels();
        }
    }
}