using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Classes.Entities.Pooling.Abstract;
using System.Web.UI.HtmlControls;
using Mars.DAL.Pooling.Abstract;
using Mars.Pooling.HTMLFactories;
using Mars.Pooling.HTMLFactories.Abstract;
using App.Classes.DAL.Pooling.Abstract;
using Mars.Entities.Pooling;
using Mars.Pooling.Models.Abstract;

namespace Mars.Pooling.Models
{
    public class SiteCompGridModel : ICompGridModel
    {
        public HtmlGenericControl DataTable { get; set; }
        public CompHtmlTable _HtmlTable { get; set; }
        public Enums.Headers Mode { get; set; }
        public SiteCompGridModel()
        {
            _HtmlTable = new HtmlFactory(Enums.HtmlTable.SiteComp).HtmlTable as CompHtmlTable;
        }
        public void Bind(params String[] param)
        {
            int width = 0;
            if (!int.TryParse(param[0], out width)) width = 1000;
            _HtmlTable._width = (int)(width * 0.8);
            _HtmlTable.Tme = Mode == Enums.Headers.threeSiteComparison ? Enums.DayActualTime.THREE : Enums.DayActualTime.THIRTY;
            DataTable.InnerHtml = _HtmlTable.GetTable();
        }
    }
}