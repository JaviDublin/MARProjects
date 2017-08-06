using System;

namespace App.DAL.MarsDataAccess.Sizing.KpiDataHolders
{
    internal struct KpiRawDataHolderOld
    {
        internal DateTime? ReportDate;
        internal decimal OnRent;
        internal decimal OperationalFleet;
        internal decimal IdleFleet;
        internal decimal TotalFleet;
        internal decimal AvailableFleet;

        internal string Country;

        internal decimal Forecast;
    }

    internal struct KpiRawDataHolder
    {
        internal DateTime? ReportDate;
        internal decimal Forecast;
        internal decimal ExpectedFleet;

        internal string Country;

    }
}