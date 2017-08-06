using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mars.App.Classes.Phase4Dal.Administration.MappingEntities
{
    public class CarSegmentEntity : IMappingEntity
    {
        public int Id { get; set; }
        public string CountryName { get; set; }
        public int CountryId { get; set; }
        public string CarSegmentName { get; set; }

        public bool Active { get; set; }
        public string ActiveYesNo { get { return (Active ? "Yes" : "No"); } }

        public List<string> GetRowNames()
        {
            var returned = new List<string> { "Id", "Country Name", "Car Segment", "Active" };

            return returned;
        }

    }
}