using System;
using App.Classes.DAL.Pooling.Abstract;
using App.Classes.Entities.Pooling.Abstract;
using Mars.Entities.Pooling;
using Mars.Pooling.HTMLFactories.Abstract;
using App.Classes.DAL.Reservations;
using Mars.DAL.Pooling.Abstract;
using Mars.DAL.Pooling;

namespace Mars.Pooling.HTMLFactories
{
    public class HtmlFactory : IHtmlFactory
    {
        
        public HtmlTable HtmlTable { get; set; }
        public DateTime SelectedDate;

        public HtmlFactory(Enums.HtmlTable en)
        {
            
            switch (en)
            {
                case Enums.HtmlTable.Alerts: 
                    HtmlTable = new AlertsHtmlTable(new AlertsRepository()); 
                    break;
                //case Enums.HtmlTable.AlertsPopup: HtmlTable=new AlertsHtmlPopup(new AlertsPopupRepository()); break;
                case Enums.HtmlTable.Actuals: HtmlTable = new ActualHtmlTable(new DayActualRepository(new ReservationActualsJavascriptRepository())); break;
                case Enums.HtmlTable.SiteComp: HtmlTable = new CompHtmlTable(new SiteComparisonRepository()); break;
                case Enums.HtmlTable.FleetComp: HtmlTable = new CompHtmlTable(new FleetComparisonRepository()); break;
            }
        }
        public String GetHTML()
        {
            return HtmlTable.GetTable();
        }
    }
}