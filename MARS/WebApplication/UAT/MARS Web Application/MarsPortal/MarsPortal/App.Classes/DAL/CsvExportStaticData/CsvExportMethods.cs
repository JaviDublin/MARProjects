using System.Collections.Generic;
using System.Text;

namespace App.DAL.MarsDataAccess.CsvExportStaticData {
    internal static class CsvExportMethods {
        internal static string GetExportHeaders(int siteGroup, int fleetGroup, bool weeklyGrouping = false, bool showCalendarWeek = false) {
            var header = new StringBuilder();
            var firstHeader = weeklyGrouping ? CsvExportHeaders.Week : CsvExportHeaders.ReportDate;

            var yearHeader = weeklyGrouping ? CsvExportHeaders.Year + "," : "";

            var calendarWeekHeader = showCalendarWeek ? CsvExportHeaders.CalendarWeek + "," : "";

            header.Append(string.Format("{0}{1},{2}{3},", yearHeader, firstHeader, calendarWeekHeader, CsvExportHeaders.Country));

            if(siteGroup > 0) {
                header.Append(CsvExportHeaders.Pool + ",");
            }
            if(siteGroup > 1) {
                header.Append(CsvExportHeaders.LocationGroup + ",");
            }
            if(fleetGroup > 0) {
                header.Append(CsvExportHeaders.CarSegment + ",");
            }
            if(fleetGroup > 1) {
                header.Append(CsvExportHeaders.CarClassGroup + ",");
            }
            if(fleetGroup > 2) {
                header.Append(CsvExportHeaders.CarClass + ",");
            }
            return header.ToString();
        }

        internal static string[] GetGroupingColumns(int siteGroup, int fleetGroup, bool weeklyGrouping = false, bool showCalendarWeek = false) {
            var groupedList = new List<string>();


            if(weeklyGrouping) {
                groupedList.Add("Year");
                groupedList.Add("Week");
            } else {
                groupedList.Add("ReportDate");
            }

            if(showCalendarWeek) groupedList.Add("CalendarWeek");

            groupedList.Add("CountryName");

            if(siteGroup > 0) groupedList.Add("Pool");
            if(siteGroup > 1) groupedList.Add("LocationGroup");
            if(fleetGroup > 0) groupedList.Add("CarSegment");
            if(fleetGroup > 1) groupedList.Add("CarClassGroup");
            if(fleetGroup > 2) groupedList.Add("CarClass");
            return groupedList.ToArray();
        }
    }
}