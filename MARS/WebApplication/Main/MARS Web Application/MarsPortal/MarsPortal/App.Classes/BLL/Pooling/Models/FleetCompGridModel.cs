using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.Pooling.Models.Abstract;
using System.Web.UI.HtmlControls;
using Mars.Pooling.HTMLFactories;
using App.Classes.DAL.Pooling.Abstract;

namespace Mars.Pooling.Models {
    public class FleetCompGridModel:ICompGridModel {

        public FleetCompGridModel() {
            _HtmlTable=new HtmlFactory(Enums.HtmlTable.FleetComp).HtmlTable as CompHtmlTable;
        }        
        public void Bind(params string[] param) {
            int width=0;
            if(!int.TryParse(param[0],out width)) width=1000;
            _HtmlTable._width=(int)(width*0.8);
            _HtmlTable.Tme=Mode==Enums.Headers.threeFleetComparison?Enums.DayActualTime.THREE:Enums.DayActualTime.THIRTY;
            DataTable.InnerHtml = _HtmlTable.GetTable();
        }
        public HtmlGenericControl DataTable { get; set; }
        public CompHtmlTable _HtmlTable { get; set; }
        public Enums.Headers Mode { get; set; }
    }
}