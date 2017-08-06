using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Mars.FleetAllocation.DataAccess.Entities
{
    public class WeeklyMaxMinValues
    {
        public static readonly string[] HeaderRows =
        {
           "Year", "Week", "Month", "Car Group", "Location"
           , "Rank From Rev", "Total Fleet", "MinFleet", "MaxFleet"
           , "Addition and Deletions", "Missing Min Vehicles", "Missing Max Vehicles", "Reason for Gap"
           , "CA", "CPA", "CpU", "Rev", "Holding Cost"
        };

        public static readonly string[] Formats =
        {
            string.Empty, string.Empty, string.Empty, string.Empty, string.Empty
            , "#,0", "#,0", "#,0", "#,0"
            , "#,0", "#,0", "#,0", string.Empty
            , "#,0", "#,0", "#,0.000", "#,0.000", "#,0.000"
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

        public int RankFromRevenue { get; set; }
        
        public int TotalFleet { get; set; }
        public int MinFleet { get; set; }
        public int MaxFleet { get; set; }
        public int AdditionDeletionSum { get; set; }


        public int MissingMinVehicles {
            get
            {
                var missing = TotalFleet - MinFleet + CumulativeAddition + CumulativePlannedAddition;

                return missing < 0 ? missing : 0;
            }
        }

        public int MissingMaxVehicles
        {
            get
            {
                var missing = TotalFleet - MaxFleet + CumulativeAddition + CumulativePlannedAddition;

                return missing < 0 ? missing : 0;
            }
        }

        public string ReasonForGap { get; set; }

        public int CumulativeAddition { get; set; }
        public int CumulativePlannedAddition { get; set; }

        public decimal Contribution { get; set; }

        public double Revenue { get; set; }
        public double HoldingCost { get; set; }


    }
}