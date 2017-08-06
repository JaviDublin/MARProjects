using System;
using System.Collections.Generic;
using System.Linq;
using Mars.App.Classes.BLL.ExtensionMethods;
using Mars.Pooling.HTMLFactories.Abstract;
using Mars.DAL.Pooling.Abstract;
using System.Text;
using Mars.Entities.Pooling;
using App.Classes.DAL.Pooling.Abstract;

namespace Mars.Pooling.HTMLFactories
{
    public class CompHtmlTable : HtmlTable
    {
        public IComparisonRepository _repository;
        public IList<String[]> ABagOfData;
        readonly String DASH = "-", STRINGFORMAT = "00";
        public String[] Dates, Days, Spans;
        public Int32 _width = 800, _bagLength = 30;
        public Enums.DayActualTime Tme;
        public CompHtmlTable(IComparisonRepository r)
        {
            if (r == null) throw new ArgumentNullException("The IComparisonRepository can't be null");
            Filter = new MainFilterEntity();
            _repository = r;
        }

        public override string GetTable()
        {
            if (Tme == Enums.DayActualTime.THREE) popHead();
            else popHeadDays();
            _repository.Filter = Filter;
            ABagOfData = _repository.GetList(Tme);
            return buildTable();
        }
        String buildTable()
        {
            var _sb = new StringBuilder();


            var gmtTimeZone = (Filter.Country == string.Empty || Filter.Country == "United Kingdom");
            var timeZone = string.Format("Timezone: {0}", gmtTimeZone ? "GMT" : "CET");
            var now = DateTime.Now.GetDateAndHourOnlyByCountry(Filter.Country);

            _sb.Append(string.Format(@"<table class='ActualsSurroundTable'>
                            <tr style='vertical-align:top'>
                                <td>
                                    <table class='ActualsTable'>
                                        <tr><th style='border-left: 1px solid black;text-align:left;'>{0}</th></tr>
                                        <tr><td class='head1' style='border-left: 1px solid black;'>&nbsp;</td></tr>", timeZone));
            int count = 0;
            foreach (String[] s in ABagOfData)
            {
                if (s[0] == "Totals")
                    _sb.Append(@"<tr><td class='footer' style='text-align:left;'>" + s[0] + "</td></tr>");
                else
                    _sb.Append(@"<tr class='powerRow' id='powerRowClickable-" + (count++) + "' title='Hover to highlight' onmouseover='OnMouseOverPowerRow(this,event);' onmouseout='OnMouseOutPowerRow(this,event);' OnClick='BalanceClicked(this,event)'><td class='powerSelectableSide'>" + s[0] + "</td></tr>");
            }
            _sb.Append(@"</table>
                            </td>
                            <td>
                                <div class='ActualsScroller' style='width:" + _width + @"px;' id='ActualsScroller_Id'>
                                    <table class='ActualsTable'>
                                        <tr>");
            for (Int32 i = 0; i < Days.Length; i++)
                _sb.Append("<td class='head1' style='text-align:center;' colspan='" + Spans[i] + "'>" + Days[i] + "</td>");
            _sb.Append("</tr><tr>");

            if (!gmtTimeZone)
            {
                //Dates = Dates.Take(Dates.Length - 1).ToArray();
            }

            for (int i = 0; i < Dates.Length; i++)
                _sb.Append("<td class='head1'><div style='width:40px;text-align:center;'>" + Dates[i] + "</div></td>");
            _sb.Append("</tr>");
            count = 0;
            foreach (String[] s in ABagOfData)
            {
                if (s[0] == "Totals")
                {
                    _sb.Append("<tr>");
                    for (Int32 i = 0; i < _bagLength; i++) 
                        _sb.Append("<td class='footer' style='text-align:right;'>" + s[i + 1] + "</td>");
                    _sb.Append("</tr>");
                }
                else
                {
                    _sb.Append("<tr class='powerRow' id='powerRow-" + (count++) + "'>");
                    for (Int32 i = 0; i < _bagLength; i++) 
                        _sb.Append("<td class='powerSelectable'  style='cursor:pointer' onclick='__doPostBack(\"FromCompTable\",\"compTable,"
                            + i + "," + s[0] + "," + now + "," + Tme + "\")'>" + s[i + 1] + "</td>");
                    _sb.Append("</tr>");
                }
            }
            _sb.Append("</table></div></td></tr></table>");
            return _sb.ToString();

        }
        private void popHead()
        {
            var gmtTimeZone = (Filter.Country == string.Empty || Filter.Country == "United Kingdom");
            var timeZone = string.Format("Timezone: {0}", gmtTimeZone ? "GMT" : "CET");
            var hours = 72;

            _bagLength = hours;

            Dates = new String[hours];
            Int32 currentHour = DateTime.Now.GetDateAndHourOnlyByCountry(Filter.Country).Hour;
            for (Int32 i = 0; i < hours; i++)
            {
                Dates[i] = (currentHour).ToString(STRINGFORMAT) + DASH + (currentHour + 1).ToString(STRINGFORMAT);
                currentHour = currentHour > 22 ? 0 : currentHour + 1;
            }
            Days = new String[4];
            Spans = new String[4];
            Days[0] = DateTime.Now.DayOfWeek + ", " + DateTime.Now.ToShortDateString();
            Spans[0] = (24 - DateTime.Now.Hour).ToString();
            Days[1] = DateTime.Now.AddDays(1).DayOfWeek + ", " + DateTime.Now.AddDays(1).ToShortDateString();
            Spans[1] = 24.ToString();
            Days[2] = DateTime.Now.AddDays(2).DayOfWeek + ", " + DateTime.Now.AddDays(2).ToShortDateString();
            Spans[2] = 24.ToString();
            if (DateTime.Now.Hour > 0)
            {
                Days[3] = DateTime.Now.AddDays(3).DayOfWeek + ", " + DateTime.Now.AddDays(3).ToShortDateString();
                Spans[3] = (DateTime.Now.Hour).ToString();
            }
        }
        private void popHeadDays()
        {
            _bagLength = 30;
            Dates = new String[30];
            Spans = new String[30];
            Days = new String[30];
            for (int i = 0; i < 30; i++)
            {
                Dates[i] = DateTime.Now.AddDays(i).ToString("dd/MM");
                Spans[i] = "1";
                Days[i] = DateTime.Now.AddDays(i).DayOfWeek.ToString().ToUpper().Substring(0, 2);
            }
        }
    }
}