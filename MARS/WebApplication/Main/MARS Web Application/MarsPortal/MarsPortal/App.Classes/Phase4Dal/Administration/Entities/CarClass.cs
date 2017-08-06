namespace Mars.App.Classes.Phase4Dal.Administration.Entities
{
    public class CarClass
    {
        public int CarClassId { get; set; }
        public string CarClassName { get; set; }
        public int? CarSegmentId { get; set; }
        public string CarSegment { get; set; }
        public string Country { get; set; }
        public bool? IsActive { get; set; } 
    }
}