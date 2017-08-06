using System;

namespace App.DAL.MarsDataAccess.Forecasting.BenchmarkDataHolders {
    public struct BenchmarkExcelDataHolder {
        public DateTime ReportDate;
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

        public decimal CurrentOnRent;
        public decimal OnRentLastYear;
        public decimal FrozenValue;
        public decimal Week1;
        public decimal Week2;
        public decimal Week3;
        public decimal Week4;
        public decimal Week5;
        public decimal Week6;
        public decimal Week7;
        public decimal Week8;
        public decimal TopDown;
    }
}