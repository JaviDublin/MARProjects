using System;

namespace Mars.App.Classes.Phase4Dal.Pooling.Enums
{
    public static class DayActualTypeTranslator
    {
        public static string GetTypeName(DayActualRowType type)
        {
            switch (type)
            {
                case DayActualRowType.Available:
                    return "Available";
                case DayActualRowType.Reservations:
                    return "Reservations";
                case DayActualRowType.CheckIns:
                    return "Check Ins";
                case DayActualRowType.Buffers:
                    return "Buffers";
                case DayActualRowType.Balance:
                    return "Balance";
                case DayActualRowType.AdditionDeletion:
                    return "Additions Deletions";
                default:
                    throw new ArgumentOutOfRangeException("type");
            }
        }
    }
}