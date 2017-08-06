using System;
using System.Collections.Generic;
using App.BLL.ReportEnums.FleetSize;
using App.Entities;
using System.Data.SqlClient;
using App.DAL.Data;

namespace App.DAL.CMSReportType
{
    public class DALcmsReportType
    {
        public List<CMSFleetPlan> CMSFleetPlanGetAll(bool hideScenarios)
        {
            List<CMSFleetPlan> fleetPlans = new List<CMSFleetPlan>();
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.CMSFleetPlanGetAll, con);
            using (con)
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var fleetPlan = new CMSFleetPlan();
                    if (reader["PlanID"] != DBNull.Value)
                        fleetPlan.PlanID = Convert.ToInt32(reader["PlanID"]);

                    if (reader["PlanDescription"] != DBNull.Value)
                        fleetPlan.PlanDescription = reader["PlanDescription"].ToString();

                    if (hideScenarios)
                    {
                        if (fleetPlan.PlanID == 1) fleetPlans.Add(fleetPlan); 
                    }
                    else
                    {
                        fleetPlans.Add(fleetPlan);    
                    }
                    
                }
            }

            return fleetPlans;
        }

        public List<CMSForecastType> CMSForecastTypeGetAll(bool frozenZoneSelected)
        {
            var forecastTypes = new List<CMSForecastType>();
            var con = DBManager.CreateConnection();
            var cmd = DBManager.CreateProcedure(StoredProcedures.CMSForecastTypeGetAll, con);

            using (con)
            {
                con.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var forecastType = new CMSForecastType();
                    if (reader["FCTypeID"] != DBNull.Value)
                        forecastType.FCTypeID = Convert.ToInt32(reader["FCTypeID"]);

                    if (reader["FCType"] != DBNull.Value)
                        forecastType.FCType = reader["FCType"].ToString();

                    if (!frozenZoneSelected || forecastType.FCTypeID != (int)FutureTrendDataType.AlreadyBooked)
                    {
                        forecastTypes.Add(forecastType);        
                    }
                    
                }
            }

            return forecastTypes;
        }

        public List<CMSReportingTimeZone> CMSReportingTimeZoneGetAll()
        {
            List<CMSReportingTimeZone> reportingTimeZoneList = new List<CMSReportingTimeZone>();
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.ReportingTimeZoneGetAll, con);
            using (con)
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var reportingTimeZone = new CMSReportingTimeZone();
                    if (reader["zoneID"] != DBNull.Value)
                        reportingTimeZone.ZoneID = Convert.ToInt32(reader["zoneID"]);

                    if (reader["zoneDescription"] != DBNull.Value)
                        reportingTimeZone.ZoneDescription = reader["zoneDescription"].ToString();

                    reportingTimeZoneList.Add(reportingTimeZone);
                }
            }

            return reportingTimeZoneList;
        }
    }
}