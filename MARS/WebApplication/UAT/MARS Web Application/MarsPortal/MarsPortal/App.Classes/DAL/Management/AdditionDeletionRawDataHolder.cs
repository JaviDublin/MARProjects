using System;

namespace App.DAL.MarsDataAccess.Sizing.FutureTrendDataHolders {
    internal struct AdditionDeletionRawDataHolder {
        internal DateTime ReportDate;
        public int CalendarWeek;
        internal decimal Amount;
        internal int ScenarioID;
        internal int CarClassId;
        internal int LocationGroupId;
        internal string Country;

    }
}