using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Mars.App.Classes.Phase4Dal.Administration.MappingEntities
{
    public class CountryEntity : IMappingEntity
    {
        public int Id { get; set; }

        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string CountryDw { get; set; }
        public bool Active { get; set; }
        public string ActiveYesNo { get { return (Active ? "Yes" : "No"); } }

        public List<string> GetRowNames()
        {
            var returned = new List<string> { "Id", "Country", "DW Code" , "Country Name", "Active" };

            return returned;
        }
 


       

    }
}