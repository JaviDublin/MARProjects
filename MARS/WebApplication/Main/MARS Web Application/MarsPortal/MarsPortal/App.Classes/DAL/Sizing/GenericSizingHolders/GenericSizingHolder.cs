using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mars.App.Classes.DAL.Sizing.GenericSizingHolders
{
    internal class GenericSizingHolder
    {
        internal DateTime? Date;
        internal string Country;
        internal string CountryName;
        internal int LocationGroupId;
        internal int PoolId;
        internal int CarClassGroupId;
        internal int CarClassId;
        internal int CarSegmentId;

        internal decimal? Constrained;
        internal decimal? Unconstrained;
        internal decimal? AlreadyBooked;

        internal decimal? NessesaryConstrained;
        internal decimal? NessesaryUnconstrained;
        internal decimal? NessesaryBooked;

        internal decimal? ExpectedFleet;

    }
}