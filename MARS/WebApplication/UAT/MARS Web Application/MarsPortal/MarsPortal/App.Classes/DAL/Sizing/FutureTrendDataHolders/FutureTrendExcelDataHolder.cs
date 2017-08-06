using System;

namespace App.DAL.MarsDataAccess.Sizing.FutureTrendDataHolders {
    public struct FutureTrendExcelDataHolder {
        public DateTime ReportDate;
        public string CountryId;
        public string CountryName;
        public string Pool;
        public int? PoolId;
        public string LocationGroup;
        public int? LocationGroupId;
        public string CarSegment;
        public int? CarSegmentId;
        public string CarClassGroup;
        public int? CarClassGroupId;
        public string CarClass;
        public int? CarClassId;
        public decimal Forecast;
        public decimal NessesaryFleet;
        public decimal ExpectedFleet;
    }
}