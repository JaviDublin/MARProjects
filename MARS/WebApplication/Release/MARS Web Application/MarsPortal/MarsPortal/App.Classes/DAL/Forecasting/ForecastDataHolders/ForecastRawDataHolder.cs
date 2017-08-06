using System;

namespace App.DAL.MarsDataAccess.Forecasting.ForecastDataHolders
{
    internal struct ForecastRawDataHolder
    {
        internal DateTime ReportDate;

        internal decimal OnRent;
        internal decimal OnRentLy;
        internal decimal Constrained;
        internal decimal Unconstrained;
        internal decimal Fleet;
        internal decimal AlreadyBooked;
        internal decimal TopDown;
        internal decimal BottomUp1;
        internal decimal BottomUp2;
        internal decimal Reconciliation;
        
        internal int CarClassId;
        internal int LocationGroupId;
        internal string Country;
    }
}