using System;
using System.Globalization;
using System.IO;
using System.Web;

namespace App.BLL.Utilities
{
    public class Helper
    {
        public int GetWeekNumber(DateTime date)
        {
            var cultureInfo = CultureInfo.CreateSpecificCulture("no");
            var weekNumber = cultureInfo.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            return weekNumber;
        }


        /// <summary>
        /// Delete all csv files from directory
        /// </summary>
        /// <param name="directory"></param>
        public static void DeleteTempFiles(string directory)
        {

            string[] filePaths = Directory.GetFiles(HttpContext.Current.Server.MapPath(directory), "*.csv");
            foreach (string tempFile in filePaths)
            {


                File.Delete(tempFile);
            }


        }
    }
}