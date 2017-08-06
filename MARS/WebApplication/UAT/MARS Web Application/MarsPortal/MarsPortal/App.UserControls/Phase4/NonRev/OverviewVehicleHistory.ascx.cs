using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.App.Classes.Phase4Dal.NonRev;
using Mars.App.Classes.Phase4Dal.NonRev.Entities;

namespace Mars.App.UserControls.Phase4.NonRev
{
    public partial class OverviewVehicleHistory : UserControl
    {
        public const string OverviewVehicleHistoryDetails = "OverviewVehicleHistoryDetails";
        private OverviewVehicleDetails VehicleHistoryDetails
        {
            get { return Session[OverviewVehicleHistoryDetails] as OverviewVehicleDetails; }
            set { Session[OverviewVehicleHistoryDetails] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void SetVehicleHistoryDetails(int vehicleId)
        {
            using (var dataAccess = new OverviewDataAccess())
            {
                VehicleHistoryDetails = dataAccess.GetVehicleHistoryDetails(vehicleId);
            }
            SetHistory();
            
        }

        protected void lbPeriods_SelectionChanged(object sender, EventArgs e)
        {
            
        }

        protected void lbPeriodEntries_SelectionChanged(object sender, EventArgs e)
        {
            
        }

        private void SetHistory()
        {
             tvHistory.Nodes.Clear();
            foreach (var p in VehicleHistoryDetails.Periods.OrderByDescending(d=> d.EnteredNonRevTimestamp))
            {
                var daysNonRevForPeriod = !p.DaysInNonRev.HasValue ? VehicleHistoryDetails.NonRevDays : p.DaysInNonRev.Value;
                var periodNode = new TreeNode(string.Format("{2}Period {0} - {1}"
                                , p.EnteredNonRevLastUpdate.HasValue ? p.EnteredNonRevLastUpdate.Value.ToString("dd/MM/yy") : string.Empty
                                , p.LeftNonRevLastUpdate.HasValue 
                                    ? p.LeftNonRevLastUpdate.Value.ToString("dd/MM/yy") : string.Empty
                                , p.Active ? "Active " : string.Empty
                                , daysNonRevForPeriod
                                , daysNonRevForPeriod == 1 ? "day" : "days"
                                ))
                                 {
                                     SelectAction = TreeNodeSelectAction.None
                                 };
                int periodId = p.VechicleNonRevPeriodId;
                foreach (var pe in VehicleHistoryDetails.PeriodEntries.Where(d => d.PeriodId == periodId).OrderByDescending(d => d.LastChangeDateTime))
                {
                    var periodEntry = new TreeNode(string.Format("{3}: {2} {0} - {1} ", pe.MovementTypeFull
                                                                    , pe.OperationalStatusFull
                                                                    , pe.LastLocationCode
                                                                    , pe.LastChangeDateTime.HasValue ? pe.LastChangeDateTime.Value.ToString("dd/MM/yy HH:mm") : string.Empty
                                                                    ))
                                      {
                                          SelectAction = TreeNodeSelectAction.None
                                      };

                    var periodEntryId = pe.PeriodEntryId;
                    foreach (var per in VehicleHistoryDetails.PeriodEntryRemarks.Where(d => d.PeriodEntryId == periodEntryId))
                    {
                        var remark = new TreeNode(string.Format("{0} {1}: {2} {3}- {4}"
                                                , per.Timestamp.HasValue ? per.Timestamp.Value.ToString("dd/MM/yy HH:mm") : string.Empty
                                                , per.UserId
                                                , per.ExpectedResolutionDate.ToShortDateString()
                                                , per.ReasonText
                                                , per.Remark
                                                ))
                                     {
                                         SelectAction = TreeNodeSelectAction.None
                                     };

                        periodEntry.ChildNodes.Add(remark);
                    }

                    periodNode.ChildNodes.Add(periodEntry);
                }
                tvHistory.Nodes.Add(periodNode);
            }
            
        }
      
    }
}