using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mars.App.Classes.BLL.ExtensionMethods
{
    public static class DateTimeExtensions
    {
        public static DateTime GetDateAndHourOnlyByCountry(this DateTime dt, string country)
        {
            var gmtTimeZone = (country == string.Empty || country == "United Kingdom");
            var now = gmtTimeZone ? dt : dt.AddHours(1);
            now =
                now.AddMinutes(-now.Minute)
                    .AddSeconds(-now.Second)
                    .AddMilliseconds(-now.Millisecond);
            return now;

        }
    }
}