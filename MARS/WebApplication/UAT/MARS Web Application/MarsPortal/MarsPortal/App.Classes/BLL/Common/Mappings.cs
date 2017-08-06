
namespace App.BLL
{
    public class Mappings
    {

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