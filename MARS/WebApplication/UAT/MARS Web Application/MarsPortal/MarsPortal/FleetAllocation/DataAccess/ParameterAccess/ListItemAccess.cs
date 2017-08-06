using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Castle.Core.Internal;
using Mars.App.Classes.Phase4Dal;
using Mars.FleetAllocation.DataContext;

namespace Mars.FleetAllocation.DataAccess.ParameterAccess
{
    public static class ListItemAccess
    {
        public static List<ListItem> GetCommercialCarSegments(FaoDataContext dataContext, bool allSelected = false)
        {
            var listItems = from ccs in dataContext.CommercialCarSegments
                select new ListItem(ccs.Name, ccs.CommercialCarSegmentId.ToString(CultureInfo.InvariantCulture));
            var returned = listItems.ToList();
            if (allSelected)
            {
                returned.ForEach(d => d.Selected = true);
            }
            return returned;
        }

        public static List<ListItem> GetDayOfWeeks(bool allSelected = false)
        {
            var days = from dow in Enum.GetValues(typeof (DayOfWeek))
                .OfType<DayOfWeek>()
                select new ListItem(dow.ToString(), (((int) dow) + 1).ToString())
                       {
                           Selected = allSelected
                       };

            var returned = days.ToList();
            return returned;
        }
    }
}