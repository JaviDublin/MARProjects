using System;

namespace App.DAL.MarsDataAccess.Sizing.SupplyAnalysisDataHolders
{
    public class SupplyAnalysisExcelDataHolder
    {
        public DateTime ReportDate;
        public int Week;
        public int Year;
        public string Country;
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

        public decimal Difference;
        public decimal NessesaryFleet;
        public decimal ExpectedFleet;
    }
}