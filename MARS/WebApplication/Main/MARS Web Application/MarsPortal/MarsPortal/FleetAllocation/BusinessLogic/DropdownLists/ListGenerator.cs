using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Mars.FleetAllocation.DataAccess;

namespace Mars.FleetAllocation.BusinessLogic.DropdownLists
{
    public static class ListGenerator
    {
        public static void FillDropdownWithNextYearAndHistry(DropDownList ddl, int historyYears, int futureYears)
        {
            var yearsToAdd = new List<ListItem>();
            var currentYear = DateTime.Now.Year;
            for (int i = currentYear + futureYears; i >= currentYear - historyYears - 1; i--)
            {
                yearsToAdd.Add(new ListItem(i.ToString(CultureInfo.InvariantCulture), i.ToString(CultureInfo.InvariantCulture)));
            }
            ddl.Items.AddRange(yearsToAdd.ToArray());    
        }

        public static void FillDropdownWithFaoCountries(DropDownList ddl, BaseDataAccess bda = null)
        {
            var faoCountries = Properties.Settings.Default.FaoCountries;
            var splitCountryIds = faoCountries.Split(',').Select(int.Parse).ToList();
            List<ListItem> listItems;
            if (bda != null)
            {
                listItems = bda.GetFaoCountryListItems(splitCountryIds);
            }
            else
            {
                using (var dataAccess = new BaseDataAccess())
                {
                    listItems = dataAccess.GetFaoCountryListItems(splitCountryIds);
                }
            }
            ddl.Items.AddRange(listItems.ToArray());
            
        }
    }
}