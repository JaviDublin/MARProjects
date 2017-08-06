using System;
using System.Globalization;

namespace App.BLL
{
    public class Conversions
    {
        public static DateTime ConvertStringToDate(string value)
        {
            DateTime returnDate = DateTime.Now;
            returnDate = (DateTime.ParseExact(value, "ddMMyyyyhhmmss", DateTimeFormatInfo.InvariantInfo));
            return returnDate;
        }
    }
}