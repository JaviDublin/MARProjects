using System;

namespace App.DAL.MarsDataAccess.Sizing.KpiDataHolders
{
    internal struct KpiGraphDataHolder
    {
        internal DateTime? ReportDate;
        internal Decimal? Kpi;
        internal Decimal Max;
        internal Decimal Min;
    }
}