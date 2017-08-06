using System;

namespace App.DAL.MarsDataAccess.Sizing.SiteAndFleetComparisonDataHolders
{
    public struct SiteAndFleetComparisonRawDataHolder
    {
        internal DateTime ReportDate;
        internal string Country;
        internal decimal Constrained;
        internal decimal Unconstrained;
        internal decimal Booked;
        internal decimal ExpectedFleet;
        internal int CarClassId;
        internal int LocationGroupId;
        
    }
}