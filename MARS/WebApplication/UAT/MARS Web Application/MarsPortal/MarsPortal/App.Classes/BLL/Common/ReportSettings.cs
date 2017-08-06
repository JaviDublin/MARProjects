
namespace App.BLL
{
    public class ReportSettings {


        public enum ReportSettingsTool : int 
        {
            MarsHome = 1,
            Statistics = 2,
            Availability = 3,
            NonRevenue = 4,
            NonRevStartReport = 5,
            NonRevApproval = 6
        }


        public enum OptionLogic : int {
            CMS = 1,
            OPS = 2
        }







    }
}