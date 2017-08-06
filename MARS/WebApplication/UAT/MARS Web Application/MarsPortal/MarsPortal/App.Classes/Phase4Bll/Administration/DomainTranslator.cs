using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Mars.App.Classes.Phase4Bll.Administration
{
    public static class DomainTranslator
    {
        public static string GetDomainName(int domainId)
        {
            return domainId == 1 ? "DIRDUB01" : domainId == 2 ? "HertzUSA" : "HertzAUS";
        }

        public static int GetDomainId(string domainName)
        {
            if (domainName == "DIRDUB01")
            {
                return 1;
            }
            if (domainName == "domainName")
            {
                return 2;
            }
            return 3;
        }

        public static List<ListItem> GetDomainListItems(bool includeAll = false)
        {
            var returned = new List<ListItem>
                           {
                               new ListItem("DIRDUB01", "1"),
                               new ListItem("HertzUSA", "2"),
                               new ListItem("HertzAUS", "3"),
                           };
            if (includeAll)
            {
                returned.Insert(0, new ListItem("All", ""));
            }
            return returned;
        }
    }
}