using System;
using System.Collections.Generic;
using System.Linq;
using Mars.App.Classes.DAL.MarsDBContext;
using App.Entities.Graphing.Parameters;

namespace App.DAL.MarsDataAccess.ParameterAccess
{
    // Altered by Gavin, 28-6-12, for and ITSR that: When selecting France to data wasn't export to Excel

    internal static class AdditionDeletionParameterRestriction
    {
        internal static IQueryable<MARS_CMS_FleetPlanDetail> RestrictForecastByParameters(string country, int scenarioID, IQueryable<MARS_CMS_FleetPlanDetail> resultSet, MarsDBDataContext mdc)
        {
            var fromDate = DateTime.Today;

            resultSet = from r in resultSet
                        join lg in mdc.CMS_LOCATION_GROUPs on r.cms_Location_Group_ID equals lg.cms_location_group_id
                        join fp in mdc.MARS_CMS_FleetPlanEntries on r.fleetPlanEntryID equals fp.PKID // Added by Gavin
                        where lg.CMS_POOL.COUNTRy1.country1 == country && fp.fleetPlanID == scenarioID // Altered by Gavin
                        select r;

            resultSet = resultSet.Where(d => d.targetDate >= fromDate);

            return resultSet;
        }
    }
}