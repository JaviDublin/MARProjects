using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Classes.Entities.Pooling.Abstract;
using Mars.App.Classes.BLL.ExtensionMethods;
using Mars.Pooling.HTMLFactories.Abstract;
using Mars.Entities.Pooling;
using System.Text;
using Mars.DAL.Pooling.Abstract;
using Mars.DAL.Pooling;

namespace Mars.Pooling.HTMLFactories
{

    public class AlertsHtmlTable : HtmlTable
    {

        enum tag { divNextHour, divFollow4Hours, divRestOfDay, Custom };

        static String LINKCSSCLASS = "AlertLink";
        static String URLLINK = @"FleetComparison.aspx?statusType=three&info=";
        const Char DELIMITER = '|';
        HtmlTableRepository<AlertEntity> _repository;

        public DateTime SelectedDate { get; set; }

        public AlertsHtmlTable(HtmlTableRepository<AlertEntity> r)
        {
            if (r == null) throw new ArgumentNullException("The repository can't be null.");
            Filter = new MainFilterEntity();
            _repository = r;
        }

        public override String GetTable()
        {
            _repository.Filter = Filter;
            var sb = new StringBuilder();
            _repository.DateSelected = SelectedDate;

            var l = _repository.GetTable();

            var gmtTimeZone = (Filter.Country == string.Empty || Filter.Country == "United Kingdom");
            //var timeZone = string.Format("Timezone: {0}", gmtTimeZone ? "GMT" : "CET");

            var now = DateTime.Now.GetDateAndHourOnlyByCountry(Filter.Country);

            sb.Append(@"<table class='AlertsOuterTable'><tr><td>");
            sb.Append(@"<table class='AlertsTable'><tr>");
            sb.Append(@"<th id='tdNextHour'>");
            sb.Append("Next Hour (" + now.Hour.ToString("0#") + ":00 - "
                                    + now.AddHours(1).Hour.ToString("0#") + ":00)");
            sb.Append(@"</th></tr>");
            
            sb.Append(@"<tr><td>");
            sb.Append(FillDiv(tag.divNextHour, l));
            sb.Append(@"</td></tr></table>");
            sb.Append(@"</td>");
            sb.Append(@"<td>");
            sb.Append(@"<table class='AlertsTable'><tr>");
            sb.Append(@"<th id='tdFollow4Hours'>");
            sb.Append("Following 4 Hours (" + now.AddHours(1).Hour.ToString("0#") + ":00 - "
                                            + now.AddHours(5).Hour.ToString("0#") + ":00)");
            sb.Append(@"</th></tr>");
            sb.Append(@"<tr><td>");
            sb.Append(FillDiv(tag.divFollow4Hours, l));
            sb.Append(@"</td></tr></table>");
            sb.Append(@"</td>");
            sb.Append(@"<td>");

            sb.Append(@"<table class='AlertsTable'><tr>");
            sb.Append(@"<th id='tdRestOfDay'>");
            sb.Append("Rest of Day (" + now.AddHours(5).Hour.ToString("0#") + ":00 - 00:00)");
            sb.Append(@"</th></tr>");
            sb.Append(@"<tr><td>");
            sb.Append(FillDiv(tag.divRestOfDay, l));
            sb.Append(@"</td></tr></table>");
            sb.Append(@"</td>");
            sb.Append(@"<td>");

            sb.Append(@"<table class='AlertsTable'><tr>");
            sb.Append(@"<th id='tdCustom'>");
            if (SelectedDate.Date > now.Date)
            {
                sb.Append(now.AddDays(1).ToShortDateString() + " - " + SelectedDate.ToShortDateString());
            }
            else
            {
                sb.Append("&nbsp;");
            }
            
            sb.Append(@"</th></tr>");
            sb.Append(@"<tr><td>");
            sb.Append(FillDiv(tag.Custom, l));
            sb.Append(@"</td></tr></table>");


            sb.Append(@"</td></tr></table>");
            return sb.ToString();
        }

        private String FillDiv(tag t, IEnumerable<AlertEntity> l)
        {
            var sb = new StringBuilder();
            sb.Append(@"<div class='AlertTableDiv' id='" + t + "'>");
            foreach (AlertEntity item in l)
            {
                switch (t)
                {
                    case tag.divNextHour: sb.Append(CreateLink(item.NextHour, item.UrlLinkNextHour, t.ToString())); break;
                    case tag.divFollow4Hours: sb.Append(CreateLink(item.Follow4Hours, item.UrlLinkFollow4Hours, t.ToString())); break;
                    case tag.divRestOfDay: sb.Append(CreateLink(item.RestOfDay, item.UrlLinkRestOfDay, t.ToString())); break;
                    case tag.Custom: sb.Append(CreateLink(item.Custom, item.UrlLinkCustom, t.ToString())); break;
                }
                sb.Append(@"<br />");
            }
            sb.Append(@"</div>");
            return sb.ToString();
        }

        private static String CreateLink(String s, String u, String t)
        {
            if (String.IsNullOrEmpty(s)) return String.Empty;
            String[] s1 = s.Split(DELIMITER);
            return "<a class='" + LINKCSSCLASS + "' href='" + URLLINK + s + DELIMITER + t + "'>" + s1[0] + "<span style='color:red'> " + s1[1] + "</span></a>";
        }
    }
}