using System;

namespace App.DAL.MarsDataAccess.Forecasting.ForecastDataHolders
{
    public struct ForecastExcelDataHolder
    {
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

        public decimal OnRent;
        public decimal OnRentLy;
        public decimal Constrained;
        public decimal Unconstrained;
        public decimal Fleet;
        public decimal AlreadyBooked;
        public decimal TopDown;
        public decimal BottomUp1;
        public decimal BottomUp2;
        public decimal Reconciliation;

    }
}