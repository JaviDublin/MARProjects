using System;

namespace Mars.App.Classes.Phase4Bll.ExtensionMethods
{
    public static class DateTimeExtensions
    {
        public static DateTime Next(this DateTime from, DayOfWeek dayOfWeek)
        {
            var start = (int)from.DayOfWeek;
            var target = (int)dayOfWeek;
            if (target <= start)
                target += 7;
            return from.AddDays(target - start);
        }
    }
}