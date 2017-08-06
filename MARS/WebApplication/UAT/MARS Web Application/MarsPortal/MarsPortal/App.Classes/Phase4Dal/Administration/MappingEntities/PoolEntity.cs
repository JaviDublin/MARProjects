using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mars.App.Classes.Phase4Dal.Administration.MappingEntities
{
    public class PoolEntity : IMappingEntity
    {
        public int Id { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
        public int CountryId { get; set; }
        public string PoolName { get; set; }
        
        
        public bool Active { get; set; }
        public string ActiveYesNo { get { return (Active ? "Yes" : "No"); } }

        public List<string> GetRowNames()
        {
            var returned = new List<string> { "Id", "Country Name", "Pool", "Active" };

            return returned;
        }
    }
}