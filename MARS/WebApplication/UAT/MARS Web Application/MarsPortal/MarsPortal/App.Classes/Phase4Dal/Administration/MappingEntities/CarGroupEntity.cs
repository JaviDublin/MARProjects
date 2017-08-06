using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mars.App.Classes.Phase4Dal.Administration.MappingEntities
{
    public class CarGroupEntity : IMappingEntity
    {
        public int Id { get; set; }
        public string CountryName { get; set; }
        public int CountryId { get; set; }
        public string CarSegmentName { get; set; }
        public int CarSegmentId { get; set; }
        public string CarClassName { get; set; }
        public int CarClassId { get; set; }
        public string CarGroupName { get; set; }
        public string CarGroupGold { get; set; }
        public string CarGroupFiveStar { get; set; }
        public string CarGroupPresidentCircle { get; set; }
        public string CarGroupPlatinum { get; set; }


        public bool Active { get; set; }
        public string ActiveYesNo { get { return (Active ? "Yes" : "No"); } }

        public List<string> GetRowNames()
        {
            var returned = new List<string> {"Id", "Country Name", "Car Group", "Car Segment", "Car Class"
                , "Gold", "Five Star", "President Circle", "Platinum", "Active"};

            return returned;
        }
    }
}