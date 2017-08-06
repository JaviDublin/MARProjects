using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Mars.FleetAllocation.DataAccess.Entities
{
    public class DemandGapOneRow
    {
        public static readonly string[] HeaderRows =
        {
           "Year", "Week", "Month", "Car Group", "Location", "Operational Fleet",  "MinFleet"
           , "Addition and Deletions", "Missing Vehicles", "Reason for Gap", "CA", "CPA"
        };

        public int GetCarGroupId()
        {
            return CarGroupId;
        }

        public int GetLocationId()
        {
            return LocationId;
        }

        public int GetCarSegmentId()
        {
            return CarSegmentId;
        }

        public int GetMonth()
        {
            return MonthNumber;
        }

        public void SetCumulativeAdditions(int value)
        {
            CumulativeAddition = value;
        }

        public void AddVehicles(int value)
        {
            CumulativePlannedAddition += value;
        }

        

        public int Year { get; set; }
        public int WeekNumber { get; set; }
        public int MonthNumber { private get; set; }
        public string MonthName { get { return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(MonthNumber); } }
        public int CarGroupId { private get; set; }
        public int CarSegmentId { private get; set; }
        public int LocationId { private get; set; }

        public string CarGroupName { get; set; }
        public string LocationName { get; set; }

        public int OperationalFleet { get; set; }
        public int MinFleet { get; set; }
        public int AdditionDeletionSum { get; set; }


        public int MissingVehicles {
            get
            {
                var missing = OperationalFleet - MinFleet + CumulativeAddition + CumulativePlannedAddition;

                return missing < 0 ? missing : 0;
            }
        }

        public string ReasonForGap { get; set; }

        public int CumulativeAddition { get; set; }
        public int CumulativePlannedAddition { get; set; }




    }
}