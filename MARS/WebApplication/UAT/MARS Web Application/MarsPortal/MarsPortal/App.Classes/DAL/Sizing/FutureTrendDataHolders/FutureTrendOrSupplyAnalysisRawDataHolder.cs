using System;

namespace App.DAL.MarsDataAccess.Sizing.FutureTrendDataHolders
{
    public struct FutureTrendOrSupplyAnalysisRawDataHolder
    {
        internal DateTime ReportDate;
        internal decimal Constrained;
        internal decimal Unconstrained;
        internal decimal Booked;
        internal decimal Utilization;
        internal decimal NonRevFleet;
        internal decimal OnRent;
        internal decimal NessesaryConstrained;
        internal decimal NessesaryUnconstrained;
        internal decimal NessesaryBooked;
        internal decimal Expected;

        internal int CarClassId;
        internal int LocationGroupId;
        internal string Country;
    }
}