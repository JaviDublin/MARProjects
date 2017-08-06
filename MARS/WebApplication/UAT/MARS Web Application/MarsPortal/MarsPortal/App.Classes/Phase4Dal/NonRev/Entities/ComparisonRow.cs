using System;

namespace Mars.App.Classes.Phase4Dal.NonRev.Entities
{
    public class ComparisonRow
    {
        public string Key { get; set; }
        public int FleetCount { get; set; }
        public int NonRevCount { get; set; }
        public int ReasonsEntered { get; set; }

        public int DaysNonRev { get; set; }
        public DateTime? TimeStamp { get; set; }

        public double PercentNonRev
        {
            get { return NonRevCount == 0 ? 0 : (double)NonRevCount / FleetCount; }
        }

        public double PercentOfTotalFleet { get; set; }
        public double PercentNonRevOfTotalNonRev { get; set; }

        public void CalculatePercentOfTotalNonRev(int totalNonRev)
        {
            PercentNonRevOfTotalNonRev = totalNonRev == 0 ? 0 : (double)NonRevCount / totalNonRev;
        }

        public void CalculatePercentOfTotalFleet(int totalFleet)
        {
            PercentOfTotalFleet = totalFleet == 0 ? 0 : (double)NonRevCount / totalFleet;
        }

        public double PercentNonRevReasonsEntered
        {
            get { return ReasonsEntered == 0 ? 0 : (double)ReasonsEntered / NonRevCount; }
        }
    }
}