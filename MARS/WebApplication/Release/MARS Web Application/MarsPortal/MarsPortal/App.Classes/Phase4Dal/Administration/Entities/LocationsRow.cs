namespace Mars.App.Classes.Phase4Dal.Administration.Entities
{
    /// <summary>
    /// Author:  Damien Connaghan 
    /// Date: 12/08/2014
    /// Object used by Location Admin Mappings
    /// </summary>
    public class LocationsRow
    {
        public int LocationId { get; set; }
        public string LocnCode { get; set; }
        public string LocnName { get; set; }
        public string Location { get; set; }
        public string Country { get; set; }
        public string CmsPool { get; set; }
        public int? CmsLocnGrpId { get; set; }
        public string CmsLocnGrp { get; set; }
        public int? OpsAreaId { get; set; }
        public string OpsArea { get; set; }
        public int? OpsRegionId { get; set; }
        public string OpsRegion { get; set; }
        public string Apdtrr { get; set; }
        public string Cal { get; set; }
        public string ServedByLocn { get; set; }
        public int? Turnaround { get; set; }
        public bool? IsActive { get; set; }
    }
}