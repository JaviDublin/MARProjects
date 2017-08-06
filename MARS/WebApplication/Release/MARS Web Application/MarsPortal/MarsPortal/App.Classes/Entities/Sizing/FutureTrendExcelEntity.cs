using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mars.Entities.Sizing {
    public class FutureTrendExcelEntity {
        public DateTime REP_DATE;
        public Int32 constrained;
        public Int32 unconstrained;
        public Int32 reservations_booked;
        public Int32 Current_Onrent;
        public Int32 NecessaryConstrained;
        public Int32 NecessaryUnconstrained;
        public Int32 NecessaryBooked;
        public Int32 ExpectedFleet;
        public String country;
        public String Car_Segment;
        public String Car_Class;
        public String Car_Group;
        public String CMS_Pool;
        public String CMS_Location_Group;

        public Int32 Week;
        public Int32 ConstrainedSupply;
        public Int32 UnConstrainedSupply;
        public Int32 BookedSupply;
    }


    public class SupplyAnalysisExcelEntity
    {
        public DateTime REP_DATE;
        public Int32 ConstrainedSupply;
        public Int32 UnConstrainedSupply;
        public Int32 BookedSupply;
        public String country;
        public String Car_Segment;
        public String Car_Class;
        public String Car_Group;
        public String CMS_Pool;
        public String CMS_Location_Group;

        public Int32 Week;
        public Int32 Year;

    }

    
}