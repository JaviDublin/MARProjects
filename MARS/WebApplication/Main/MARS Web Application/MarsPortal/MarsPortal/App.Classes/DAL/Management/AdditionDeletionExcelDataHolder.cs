using System;

namespace App.DAL.MarsDataAccess.Management
{
    public struct AdditionDeletionExcelDataHolder
    {
        public DateTime ReportDate;
        public int CalendarWeek;
        public string CountryId;
        public string CountryName;
        public string Pool;
        public int PoolId;
        public string LocationGroup;
        public int LocationGroupId;
        public string CarSegment;
        public int CarSegmentId;
        public string CarClassGroup;
        public int CarClassGroupId;
        public string CarClass;
        public int CarClassId;
        public decimal Amount;
    }
}