
namespace App.BLL
{
    public class Mappings
    {

        public enum Type : int
        {
            Country = 1,
            AreaCode = 2,
            CMSPools = 3,
            CMSLocationGroups = 4,
            OPSRegions = 5,
            OPSAreas = 6,
            Locations = 7,
            CarSegment = 8,
            CarClass = 9,
            CarGroup = 10,
            ModelCodes = 11
        }

        public enum SortDirection : int
        {
            Ascending = 1,
            Descending = 2
        }

        public enum Mode : int
        {
            Insert = 1,
            Edit = 2,
            Delete = 3
        }
    }
}