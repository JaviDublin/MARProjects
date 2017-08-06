using System;

namespace App.DAL.MarsDataAccess.Sizing.SiteAndFleetComparisonDataHolders
{
    public struct SiteAndFleetComparisonExcelDataHolder
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
        public decimal ExpectedFleet;
        public decimal Constrained;
        public decimal Unconstrained;
        public decimal Booked;

        public string AdditionalColumns;

    }
}