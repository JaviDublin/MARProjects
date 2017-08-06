using System;
using System.Text;
using App.Classes.DAL.Reservations.Abstract;
using System.Data;
using App.Classes.DAL.Pooling.Abstract;
using App.Classes.Entities.Pooling.Abstract;
using Mars.App.Classes.BLL.ExtensionMethods;
using Mars.Pooling.HTMLFactories.Abstract;

namespace Mars.Pooling.HTMLFactories
{
    public class ActualHtmlTable : HtmlTable
    {

        public IReservationDayActualsRepository Repository;
        public Int32 Condition { get; set; }
        public Int32 NumberColumns { get; set; }
        public Enums.Headers Mode { get; set; }
        public String Width { get; set; }

        public ActualHtmlTable(IReservationDayActualsRepository r)
        {
            Repository = r;
        }
        public override string GetTable()
        {
            int iwidth;
            if (!int.TryParse(Width, out iwidth)) 
                iwidth = 800;
            return buildTable(Condition, NumberColumns, Mode, iwidth);
        }
        string buildTable(int condition, int numberColumns, Enums.Headers mode, int width)
        {
            var sb = new StringBuilder();
            var dataTable = Repository.GetTable(Filter);

            var gmtTimeZone = (Filter.Country == string.Empty || Filter.Country == "United Kingdom");
            var timeZone = string.Format("Timezone: {0}", gmtTimeZone ? "GMT" : "CET");
            int startingHour = gmtTimeZone ? 0 : 1;


            var now = DateTime.Now.GetDateAndHourOnlyByCountry(Filter.Country);

            if (!gmtTimeZone && numberColumns == 72)
            {
                //numberColumns--;
            }

            sb.Append("<table class='ActualsSurroundTable'>");
            sb.Append("<tr style='vertical-align:top'>");
            sb.Append("<td>");

            sb.Append("<table class='ActualsTable'>");
            sb.Append("<tr><th style='border-left: 1px solid black;'>" + timeZone + "</th></tr>");
            sb.Append("<tr><td class='head1' style='border-left: 1px solid black;'>&nbsp;" 
                + (mode == Enums.Headers.threeDayActualStatus ? "" : "<br />&nbsp;") + "</td></tr>");
            sb.Append("<tr title='Click to select'><td class='head2' style='cursor:default'>&nbsp;</td></tr>");

            sb.Append("<tr style='" + ((condition & 1) == 1 ? "" : "display:none") + "' id='trAdditionsDeletions'>");
            sb.Append("<td class='normal_noHover' style='text-align:left'>&nbsp;&nbsp;Additions &amp; Deletions</td>");
            sb.Append("</tr>");


            sb.Append("<tr class='powerRow'><td class='powerSelectableSide' title='Click to expand/collapse' onclick='collapseId(\"trOpenTrips\", -1); collapseId(\"trAdditionsDeletions\", 1); collapseId(\"trAdditionsDeletionsRow\", 1);collapseId(\"trOpenTripsRow\", 1)'>Available</td></tr>");
            sb.Append("<tr style='" + ((condition & 1) == 1 ? "" : "display:none") + "' id='trOpenTrips'>");
            sb.Append("<td class='normal_noHover' style='text-align:left'>&nbsp;&nbsp;Open Trips</td>");
            sb.Append("</tr>");

            sb.Append("<tr class='powerRow'><td class='powerSelectableSide'  title='Click to expand/collapse' onclick='collapseId(\"trOneway\", -1);collapseId(\"trGold\", -1);collapseId(\"trPrepaid\", -1);collapseId(\"trOnewayRow\", -1);collapseId(\"trGoldRow\", -1);collapseId(\"trPrepaidRow\", -1);'>Reservations</td></tr>");
            sb.Append("<tr style='" + ((condition & 2) == 2 ? "" : "display:none") + "' id='trOneway'>");
            sb.Append("<td class='normal_noHover' style='text-align:left'>&nbsp;&nbsp;Oneway</td>");
            sb.Append("</tr>");
            sb.Append("<tr style='" + ((condition & 2) == 2 ? "" : "display:none") + "' id='trGold'>");
            sb.Append("<td class='normal_noHover' style='text-align:left'>&nbsp;&nbsp;Gold</td>");
            sb.Append("</tr>");
            sb.Append("<tr style='" + ((condition & 2) == 2 ? "" : "display:none") + "' id='trPrepaid'>");
            sb.Append("<td class='normal_noHover' style='text-align:left'>&nbsp;&nbsp;Prepaid</td>");
            sb.Append("</tr>");
            //sb.Append("<tr style='" + ((condition & 2) == 2 ? "" : "display:none") + "' id='trPredelivery'>");
            //sb.Append("<td class='normal_noHover'>&nbsp;&nbsp;Predelivery</td>");
            //sb.Append("</tr>");

            sb.Append("<tr class='powerRow'><td class='powerSelectableSide'  title='Click to expand/collapse' onclick='collapseId(\"trCheckInOneway\", -1);collapseId(\"trCheckinReal\", -1);collapseId(\"trLocal\", -1);collapseId(\"trCheckInOnewayRow\", -1);collapseId(\"trCheckinRealRow\", -1);collapseId(\"trLocalRow\", 4)'>Check In (Offset)</td></tr>");

            sb.Append("<tr style='" + ((condition & 4) == 4 ? "" : "display:none") + "' id='trCheckinReal'>");
            sb.Append("<td class='normal_noHover' style='text-align:left'>&nbsp;&nbsp;Check In (Real)</td>");
            sb.Append("</tr>");


            sb.Append("<tr style='" + ((condition & 4) == 4 ? "" : "display:none") + "' id='trLocal'>");
            sb.Append("<td class='normal_noHover' style='text-align:left'>&nbsp;&nbsp;Local (Real)</td>");
            sb.Append("</tr>");

            sb.Append("<tr style='" + ((condition & 4) == 4 ? "" : "display:none") + "' id='trCheckInOneway'>");
            sb.Append("<td class='normal_noHover' style='text-align:left'>&nbsp;&nbsp;Oneway (Real)</td>");
            sb.Append("</tr>");

            sb.Append("<tr class='powerRow'><td class='powerSelectable' style='text-align:left;'>&nbsp;Buffers</td></tr>");
            sb.Append("<tr><td class='footer' style='text-align:left;'>Balance</td></tr>");
            sb.Append("</table>");

            sb.Append("</td>");
            sb.Append("<td>");
            sb.Append("<div class='ActualsScroller' style='width:" + width + "px;' id='ActualsScroller_Id'>");
            sb.Append("<table class='ActualsTable'><tr>");

            sb.Append(mode == Enums.Headers.threeDayActualStatus
                ? getHourStucture(numberColumns, dataTable, startingHour)
                : getDayStructure(numberColumns, dataTable));

            sb.Append("</tr>");

            for (int i = 0; i < numberColumns; i++)
                sb.Append("<td class='head2' onclick=\"__doPostBack('FromFind')\">FIND</td>");
            sb.Append("</tr>");


            sb.Append("<tr style='" + ((condition & 1) == 1 ? "" : "display:none") + "' id='trAdditionsDeletionsRow'>");
            for (int i = 0; i < numberColumns; i++)
                sb.Append("<td class='normal_noHover' >" + dataTable.Rows[(Int32)Enums.ActualsRows.AdditionsDeletions][i] + "</td>");
            sb.Append("</tr>");


            sb.Append("<tr class='powerRow'>");
            for (int i = 0; i < numberColumns; i++)
                sb.Append("<td class='powerSelectable'>" + dataTable.Rows[(Int32)Enums.ActualsRows.Available][i].ToString() + "</td>");
            sb.Append("</tr>");

            sb.Append("<tr style='" + ((condition & 1) == 1 ? "" : "display:none") + "' id='trOpenTripsRow'>");
            for (int i = startingHour; i < numberColumns; i++)
                sb.Append("<td class='normal_noHover' >" + dataTable.Rows[(Int32)Enums.ActualsRows.OpenTrips][i].ToString() + "</td>");
            sb.Append("</tr>");

            sb.Append("<tr class='powerRow'>");
            for (int i = 0; i < numberColumns; i++)
                sb.Append("<td class='powerSelectable click' onclick='__doPostBack(\"FromReservation\",\"Reservations," + i + "," + NumberColumns + "," 
                    + (mode == Enums.Headers.threeDayActualStatus ? now.AddHours(i).Ticks : now.AddDays(i).Ticks) + "\")'>" 
                                    + dataTable.Rows[(Int32)Enums.ActualsRows.Reservations][i] + "</td>");
            sb.Append("</tr>");

            sb.Append("<tr style='" + ((condition & 2) == 2 ? "" : "display:none") + "' id='trOnewayRow'>");
            for (int i = 0; i < numberColumns; i++)
                sb.Append("<td class='normal' onclick='__doPostBack(\"FromReservation\",\"Oneway Reservations," + i + "," + NumberColumns + ","
                    + (mode == Enums.Headers.threeDayActualStatus ? now.AddHours(i).Ticks : now.AddDays(i).Ticks) + "\")'>"
                        + dataTable.Rows[(Int32)Enums.ActualsRows.OnewayReservations][i] +  "</td>");
            sb.Append("</tr>");

            sb.Append("<tr style='" + ((condition & 2) == 2 ? "" : "display:none") + "' id='trGoldRow'>");
            for (int i = 0; i < numberColumns; i++)
                sb.Append("<td class='normal' onclick='__doPostBack(\"FromReservation\",\"Gold Service Reservations," + i + "," + NumberColumns + ","
                    + (mode == Enums.Headers.threeDayActualStatus ? now.AddHours(i).Ticks : now.AddDays(i).Ticks) + "\")'>" 
                        + dataTable.Rows[(Int32)Enums.ActualsRows.Gold][i] + "</td>");
            sb.Append("</tr>");

            sb.Append("<tr style='" + ((condition & 2) == 2 ? "" : "display:none") + "' id='trPrepaidRow'>");
            for (int i = 0; i < numberColumns; i++)
                sb.Append("<td class='normal' onclick='__doPostBack(\"FromReservation\",\"Prepaid Reservations," + i + "," + NumberColumns + ","
                    + (mode == Enums.Headers.threeDayActualStatus ? now.AddHours(i).Ticks : now.AddDays(i).Ticks) + "\")'>"
                    + dataTable.Rows[(Int32)Enums.ActualsRows.Prepaid][i] + "</td>");
            sb.Append("</tr>");

            //sb.Append("<tr style='" + ((condition & 2) == 2 ? "" : "display:none") + "' id='trPredeliveryRow'>");
            //for (int i = startingHour; i < numberColumns; i++)
            //    sb.Append("<td class='normal' >" + dataTable.Rows[(Int32)Enums.ActualsRows.Predelivery][i].ToString() + "</td>");
            //sb.Append("</tr>");

            sb.Append("<tr class='powerRow'>");
            for (int i = 0; i < numberColumns; i++)
                sb.Append("<td class='powerSelectable'>" + dataTable.Rows[(Int32)Enums.ActualsRows.CheckInOffset][i].ToString() + "</td>");
            sb.Append("</tr>");

            sb.Append("<tr style='" + ((condition & 4) == 4 ? "" : "display:none") + "' id='trCheckinRealRow'>");
            for (int i = 0; i < numberColumns; i++)
                sb.Append("<td class='normal_noHover' >" + dataTable.Rows[(Int32)Enums.ActualsRows.CheckIn][i] + "</td>");
            sb.Append("</tr>");

            sb.Append("<tr style='" + ((condition & 4) == 4 ? "" : "display:none") + "' id='trLocalRow'>");
            for (int i = 0; i < numberColumns; i++)
                sb.Append("<td class='normal_noHover' >" + dataTable.Rows[(Int32)Enums.ActualsRows.Local][i] + "</td>");
            sb.Append("</tr>");

            sb.Append("<tr style='" + ((condition & 4) == 4 ? "" : "display:none") + "' id='trCheckInOnewayRow'>");
            for (int i = 0; i < numberColumns; i++)
                sb.Append("<td class='normal_noHover' >" + dataTable.Rows[(Int32)Enums.ActualsRows.OnewayCheckIn][i].ToString() + "</td>");
            sb.Append("</tr>");


            sb.Append("<tr class='powerRow' id='trBuffer'>");
            for (int i = 0; i < numberColumns; i++)
                sb.Append("<td class='normal_noHover' >" + dataTable.Rows[(Int32)Enums.ActualsRows.Buffer][i] + "</td>");
            sb.Append("</tr>");



            sb.Append("<tr>");
            for (int i = 0; i < numberColumns; i++)
                sb.Append("<td class='footer' style='text-align:right' >" + dataTable.Rows[(Int32)Enums.ActualsRows.Balance][i].ToString() + "</td>");
            sb.Append("</tr>");
            sb.Append("</table>");
            sb.Append("</div>");
            sb.Append("</td>");
            sb.Append("</tr>");
            sb.Append("</table>");
            return sb.ToString();
        }
        string getHourStucture(int numberColumns, DataTable dataTable, int startingHour)
        {
            var sb = new StringBuilder();
            var dt = DateTime.Parse(dataTable.Columns[0].ColumnName);

            var ts = new TimeSpan();
            ts = dt.AddHours(numberColumns) - dt;
            int[] colSpan = { 24, 24, 24, 24 };
            colSpan[0] = 24 - dt.Hour;
            colSpan[ts.Days] = 24 - colSpan[0];
            for (int i = 0; i <= ts.Days; i++)
                sb.AppendLine("<th colspan='" + colSpan[i] + "'>" + dt.AddDays(i).DayOfWeek + ", " + dt.AddDays(i).ToShortDateString() + "</th>");
            sb.Append("</tr>");
            sb.Append("<tr>");


            for (int i = 0; i < numberColumns; i++)
            {
                dt = DateTime.Parse(dataTable.Columns[i].ColumnName);
                sb.Append("<td class='head1'>" + dt.Hour.ToString("00") + "-" + dt.AddHours(1).Hour.ToString("00") + "</td>");
            }
            //if (startingHour == 1)
            //{
            //    sb.Append("<td class='head1'>" + dt.AddHours(1).Hour.ToString("00") + "-" + dt.AddHours(2).Hour.ToString("00") + "</td>");
            //}
            return sb.ToString();
        }

        string getDayStructure(int numberColumns, DataTable dataTable)
        {
            StringBuilder sb = new StringBuilder();
            DateTime dt = DateTime.Parse(dataTable.Columns[0].ColumnName);
            int[] colSpan = { -1, -1, -1, -1 };
            string[] dates = { "", "", "" };
            int index = 0, noOfDays = numberColumns > 30 ? 30 : numberColumns;
            while (noOfDays > 0)
            {
                colSpan[index] = noOfDays > (DateTime.DaysInMonth(dt.Year, dt.Month) - dt.Day + 1) ? (DateTime.DaysInMonth(dt.Year, dt.Month) - dt.Day + 1) : noOfDays;
                dates[index] = dt.Month.ToString("0#") + "/" + dt.Year;
                noOfDays -= colSpan[index];
                index += 1;
                dt = dt.AddMonths(1);
                dt = new DateTime(dt.Year, dt.Month, 1);
            }
            index = 0;
            while (colSpan[index] > -1)
            {
                sb.AppendLine("<th colspan='" + colSpan[index] + "'>" + dates[index] + "</th>");
                index += 1;
            }
            sb.Append("</tr>");
            sb.Append("<tr>");
            for (int i = 0; i < numberColumns; i++)
            {
                dt = DateTime.Parse(dataTable.Columns[i].ColumnName);
                sb.Append("<td class='head1'" + (i == numberColumns - 1 ? " style='border-right: 1px solid black;text-align:center'>" : " style='text-align:center'>") + dt.DayOfWeek.ToString().ToUpper().Substring(0, 2) + "<br />" + dt.Day.ToString("0#") + "/" + dt.Month.ToString("0#") + "</td>");
            }
            return sb.ToString();
        }
    }
}