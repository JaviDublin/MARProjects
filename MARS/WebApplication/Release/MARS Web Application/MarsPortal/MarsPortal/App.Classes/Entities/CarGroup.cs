
namespace App.Entities
{
    public class CarGroup
    {
        public int CarGroupID { get; set; }
        public string CarGroupDescription { get; set; }
        public int CarClassID { get; set; }
        public int CarSortClass { get; set; }
        public int SortCarGroup { get; set; }
        public string CarGroupGold { get; set; }
    }
}