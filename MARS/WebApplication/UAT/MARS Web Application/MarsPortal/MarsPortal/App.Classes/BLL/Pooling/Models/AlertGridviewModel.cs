using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;
using Mars.Pooling.HTMLFactories;
using App.Classes.DAL.Pooling.Abstract;

namespace Mars.BLL.Pooling.Models
{
    public class AlertGridviewModel
    {
        public HtmlGenericControl DataTable { get; set; }
        public AlertsHtmlTable _HtmlTable;

        public DateTime SelectedDate
        {
            set
            {
                _HtmlTable.SelectedDate = value;
            }
        }

        public AlertGridviewModel()
        {
            _HtmlTable = new HtmlFactory(Enums.HtmlTable.Alerts).HtmlTable as AlertsHtmlTable;
        }
        public void Bind(params String[] param)
        {
            DataTable.InnerHtml = _HtmlTable.GetTable();
        }
    }
}