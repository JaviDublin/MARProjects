using System;

namespace App.DAL.MarsDataAccess.Forecasting.ForecastDataHolders
{
    internal class ForecastGraphDataHolder
    {
        internal DateTime ReportDate;
        internal decimal CurrentOnRent;
        internal decimal OnRentLastYear;
        internal decimal ConstrainedForecast;
        internal decimal UnconstrainedForecast;
        internal decimal Fleet;
        internal decimal AlreadyBooked;
        internal decimal OnRentTopDown;
        internal decimal OnRentBottomUpOne;
        internal decimal OnRentBottomUpTwo;
        internal decimal OnRentReconciliation;
    }
}