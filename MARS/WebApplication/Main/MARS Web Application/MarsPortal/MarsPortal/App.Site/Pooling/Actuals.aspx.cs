using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.App.Classes.Phase4Dal.Enumerators;
using Mars.App.Classes.Phase4Dal.Pooling;
using Mars.App.Classes.Phase4Dal.Pooling.Entities;

namespace Mars.App.Site.Pooling
{
    public partial class Actuals : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void LoadReport(object sender, EventArgs e)
        {
            var parameters = ucAvailabilityParameters.GetParameterDictionary();

            var startDate = DateTime.Parse(parameters[DictionaryParameter.StartDate]);
            startDate = startDate.AddHours(DateTime.Now.Hour);
            parameters[DictionaryParameter.StartDate] = startDate.ToString();
            parameters[DictionaryParameter.ReservationCheckOutInDateLogic] = true.ToString();
            var toDateExact = DateTime.Now.AddHours(DayActualsDataAccess.HoursForActuals);
            var toDate = toDateExact.Date.AddHours(toDateExact.Hour + 1);
            parameters[DictionaryParameter.EndDate] = toDate.ToString();

            List<DayActualsRow> gridData;

            using (var dataAccess = new DayActualsDataAccess(parameters))
            {
                var overdueAccess = new OverdueDataAccess(parameters, dataAccess.ShareDataContext());
                var overdueOpenTrips = overdueAccess.GetOpenTripsDue();
                var overdueCollections = overdueAccess.GetCollections();
                
                gridData = dataAccess.GetActualsRows();
                ucOverdueGrid.SetOverdueValues(overdueCollections, overdueOpenTrips);
            }

            ucActualsGrid.PopulateGrid(gridData);
        }
    }
}