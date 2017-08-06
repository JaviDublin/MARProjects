using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.App.Classes.Phase4Dal.NonRev;
using Mars.App.Classes.Phase4Dal.NonRev.Entities;

namespace Mars.App.UserControls.Phase4.NonRev
{
    public partial class MultiReasonEntry : UserControl
    {
        private List<int> SelectedVehicleIds
        {
            get { return Session["NonRevMultiReasonVehicleIds"] as List<int>; }
            set { Session["NonRevMultiReasonVehicleIds"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void SetVehicleDetails(List<int> vehicleIds)
        {
            SelectedVehicleIds = vehicleIds;
            ucReasonEntry.ClearFields();
            using (var dataAccess = new OverviewDataAccess())
            {
                var vehicleData = dataAccess.GetMultiRemarkVehicleEntries(vehicleIds);
                gvMultiReasonList.DataSource = vehicleData;
                gvMultiReasonList.DataBind();
            }
        }

        protected void ibClose_Click(object sender, EventArgs e)
        {
            SelectedVehicleIds = null;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            var selectedIds = SelectedVehicleIds;
            
            var selectedRemarkId = ucReasonEntry.SelectedReasonId;
            if (selectedRemarkId == 1)          //No Remark Selected
            {
                ucReasonEntry.SetMultipleEntryNoReasonMessage();
                return;
            }

            var expectedResolutionDate = ucReasonEntry.EstimatedResolutionDate;

            if (expectedResolutionDate == null)
            {
                ucReasonEntry.SetMultipleEntryInvalidDateMessage();
                return;
            }
            var remarkText = ucReasonEntry.RemarkEntered;

            using (var dataAccess = new OverviewDataAccess())
            {
                dataAccess.AddRemarkToManyEntries(selectedIds
                        , Rad.Security.ApplicationAuthentication.GetGlobalId()
                        , remarkText, selectedRemarkId, expectedResolutionDate.Value);
            }
            RaiseBubbleEvent(this, new CommandEventArgs("RefreshGrid", null));
        }
    }
}