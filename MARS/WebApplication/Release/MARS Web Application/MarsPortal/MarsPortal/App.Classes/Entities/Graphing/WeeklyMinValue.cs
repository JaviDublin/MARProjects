using System;

namespace App.Entities.Graphing
{
    internal struct WeeklyMinValue
    {
        internal double Value;
        internal DateTime FromDate;
        internal DateTime ToDate;

        internal int WeekOfYear;
    }
}