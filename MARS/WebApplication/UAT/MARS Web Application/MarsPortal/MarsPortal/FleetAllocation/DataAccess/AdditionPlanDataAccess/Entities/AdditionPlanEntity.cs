using System;
using Mars.FleetAllocation.UserControls;

namespace Mars.FleetAllocation.DataAccess.AdditionPlanDataAccess.Entities
{
    public class AdditionPlanEntity
    {
        public static readonly string[] HeaderRows =
        {
            "Name", "Date Created", "Applied", "Min ComSeg Scenario", "Max Fleet Scenario", 
             AutoGrid.ViewKeyword
        };

        public string GetMinComSegScenarioDescription()
        {
            return MinComSegScenarioDescription;
        }

        public string GetMaxFleetScenarioDescription()
        {
            return MaxFleetScenarioDescription;
        }

        public string Name { get; set; }
         
        public DateTime CreatedDate { private get; set; }

        public string CreatedDateString
        {
            get { return CreatedDate.ToString("dd/MM/yyyy HH:mm"); }
        }
        
        //public string CreatedBy { get; set; }
        public string Applied { get; set; }
        private string CurrentDateString
        {
            get { return CurrentDate.ToString("dd/MMM/yyyy"); }
        }

        public string MinComSegScenarioName { get; set; }
        public string MaxFleetScenarioName { get; set; }
        public string MinComSegScenarioDescription { private get; set; }
        public string MaxFleetScenarioDescription { private get; set; }

        private string RevenueRange
        {
            get { return string.Format("{0:MMM yyyy} - {1:MMM yyyy}", StartRevenueDate, EndRevenueDate); }
        }

        public DateTime GetStartRevenue()
        {
            return StartRevenueDate;
        }
        public DateTime StartRevenueDate { private get; set; }
        public DateTime GetEndRevenue()
        {
            return EndRevenueDate;
        }
        public DateTime EndRevenueDate { private get; set; }

        public DateTime GetCurrentDate()
        {
            return CurrentDate;
        }
        public DateTime CurrentDate { private get; set; }


        public int WeeksCalculated { private get; set; }

        public int GetWeeksCalculated()
        {
            return WeeksCalculated;
        }
        public int ViewParameterId { get; set; }
    }
}