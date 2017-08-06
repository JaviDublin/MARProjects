using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mars.App.Classes.Phase4Dal.Administration.MappingEntities
{
    public class LocationEntity : IMappingEntity
    {
        public int Id { get; set; }
        public string CountryName { get; set; }
        public int CountryId { get; set; }
        public string CountryCode { get; set; }
        public string PoolName { get; set; }
        public int PoolId { get; set; }
        public string LocationGroupName { get; set; }
        public int LocationGroupId { get; set; }
        public string RegionName { get; set; }
        public int RegionId { get; set; }
        public string AreaName { get; set; }
        public int AreaId { get; set; }
        public string LocationCode { get; set; }
        public string LocationFullName { get; set; }
        public string AirportDowntownRailroad { get; set; }
        public string CorporateAgencyLicencee { get; set; }
        public string ServedBy { get; set; }
        public int TurnaroundHours { get; set; }

        public int? CompanyId { get; set; }
        public string CompanyName { get; set; }


        public bool Active { get; set; }
        public string ActiveYesNo { get { return (Active ? "Yes" : "No"); } }

        public List<string> GetRowNames()
        {
            var returned = new List<string> { "Id", "Location", "Country Name", "Pool", "Location Group"
                , "Region", "Area", "CAL", "ADR",  "Served By", "Company Name", "Active" };

            return returned;
        }

    }
}