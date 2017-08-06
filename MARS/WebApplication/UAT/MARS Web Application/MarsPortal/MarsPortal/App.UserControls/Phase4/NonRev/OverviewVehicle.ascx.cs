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
    public partial class OverviewVehicle : UserControl
    {
        private OverviewVehicleDetails VehicleDetails
        {
            get { return Session[OverviewVehicleHistory.OverviewVehicleHistoryDetails] as OverviewVehicleDetails; }
        }

        public bool ShowReasonEntry {set { ucReasonEntry.Visible = value; }}

        public bool ShowHistory
        {
            set
            {
                pnlHistory.Visible = value;
                pnlComment.Visible = !value;
            }
        }

        
        public bool ShowMultiSelectTickBoxes
        {
            set
            {
                ucReasonEntry.Visible = value;
            }
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
            }
        }

        public void SetVehicleDetails(int vehicleId, bool clearActivePeriodEntries = true)
        {
            SetVehicleFields();
            if (VehicleDetails.IsNonRev)
            {
                SetActivePeriodEntries();    
            }

            ucReasonEntry.Disabled = !VehicleDetails.IsNonRev;
            lbCurrentPeriodEntries.SelectedIndex = -1;
            PopulateRemarksListBox();
            ucReasonEntry.ClearFields();     
            
        }

        protected override bool OnBubbleEvent(object source, EventArgs args)
        {
            bool handled = false;

            if (args is CommandEventArgs)
            {
                var commandArgs = args as CommandEventArgs;
                if (commandArgs.CommandName == "NewRemark")
                {
                    AddRemark();
                    RaiseBubbleEvent(this, new CommandEventArgs("RefreshGrid", null));
                    handled = true;
                }
            }

            return handled;
        }

        protected string Substring(object val)
        {
            if (val != null) { return val.ToString().Length > 25 ? val.ToString().Substring(0, 25) + "..." : val.ToString(); }
            return string.Empty;
        }

        private void AddRemark()
        {
            var vehicleId = VehicleDetails.VehicleId;
            var periodEntryId = VehicleDetails.ActivePeriodEntries.Max(d => d.PeriodEntryId);
            var selectedRemarkId = ucReasonEntry.SelectedReasonId;
            if (selectedRemarkId == 1)          //No Remark Selected
            {
                return;
            }

            var expectedResolutionDate = ucReasonEntry.EstimatedResolutionDate;

            if (expectedResolutionDate == null)
            {
                return;
            }
            var remarkText = ucReasonEntry.RemarkEntered;

            using (var dataAccess = new OverviewDataAccess())
            {
                dataAccess.AddRemarkToPeriodEntry(periodEntryId
                        , Rad.Security.ApplicationAuthentication.GetGlobalId()
                        , remarkText, selectedRemarkId, expectedResolutionDate.Value);
            }
            lbCurrentPeriodEntries.SelectedIndex = lbCurrentPeriodEntries.Items.Count - 1;
            SetVehicleDetails(vehicleId);
            PopulateRemarksListBox();
        }

        protected void lbCurrentPeriodEntries_SelectionChanged(object sender, EventArgs e)
        {
            PopulateRemarksListBox();
            RaiseBubbleEvent(this, new CommandEventArgs("KeepPopupOpen", "Keep Popup Open"));
        }


        private void PopulateRemarksListBox()
        {
            if (VehicleDetails.ActivePeriod == null)
            {
                return;
            }

            var periodEntryId = string.IsNullOrEmpty(lbCurrentPeriodEntries.SelectedValue) 
                ? VehicleDetails.ActivePeriod.VechicleNonRevPeriodId 
                : int.Parse(lbCurrentPeriodEntries.SelectedValue);
            
            var remarkEntities =
                VehicleDetails.ActivePeriodEntryRemarks.Where(d => d.PeriodEntryId == periodEntryId).ToList();

            SetRemarks(remarkEntities);
        }

        protected void btnSubmitComment_Click(object sender, EventArgs e)
        {
            using (var dataAccess = new OverviewDataAccess())
            {
                dataAccess.UpdateVehicleComment(VehicleDetails.VehicleId, tbVehicleComment.Text);
                
            }
            RaiseBubbleEvent(this, new CommandEventArgs("RefreshGrid", ""));
        }



        private void SetRemarks(List<NonRevPeriodEntryRemark> remarks )
        {
            rptRemarks.DataSource = remarks;
            rptRemarks.DataBind();
        }

        private void SetActivePeriodEntries()
        {
            var listItems = from pe in VehicleDetails.ActivePeriodEntries
                select new ListItem
                       {
                           Value = pe.PeriodEntryId.ToString(CultureInfo.InvariantCulture),
                           Text = string.Format("{3} {2}: {0} - {1} ({4})", pe.OperationalStatusCode, pe.MovementTypeCode, pe.LastLocationCode
                           , pe.LastChangeDateTime.HasValue ? pe.LastChangeDateTime.Value.ToShortDateString() + " " + pe.LastChangeDateTime.Value.ToShortTimeString() : string.Empty
                           , pe.RemarksEntered)
                       };
            lbCurrentPeriodEntries.Items.Clear();
            lbCurrentPeriodEntries.Items.AddRange(listItems.ToArray());
        }

        private void SetVehicleFields()
        {
            var vd = VehicleDetails;
            
            lblGroup.Text = vd.Group;
            lblModelDescription.Text = vd.ModelDescription;
            lblTasModelCode.Text = vd.TasModelCode;
            lblUnitNumber.Text = vd.UnitNumber.ToString(CultureInfo.InvariantCulture);
            lblVin.Text = vd.Vin;
            lblLastLocation.Text = vd.LastLocation;
            lblOwningCountry.Text = vd.OwningCountry;
            lblLastChangeDateTime.Text = vd.LastChangeDateTimeString;
            lblLocationCountry.Text = vd.LastLocation.Substring(0, 2);
            lblExpectedLocation.Text = vd.ExpectedLocation;
            lblExpectedDateTime.Text = vd.ExpectedDateString;
            //lblNonRevDays.Text = string.Format("{0} {2} {1} {3}", vd.NonRevTimeSpan.Days, vd.NonRevTimeSpan.Hours,
            //    vd.NonRevTimeSpan.Days == 1 ? "Day" : "Days", vd.NonRevTimeSpan.Hours == 1 ? "Hour" : "Hours");
            lblDaysInCountry.Text = vd.InCountryDays.ToString(CultureInfo.InvariantCulture);
            lblNonRevDays.Text = vd.NonRevDays.ToString(CultureInfo.InvariantCulture);
            lblOperationalStatus.Text = vd.OperationalStatus;
            lblMovementType.Text = vd.MovementType;
            lblKilometers.Text = vd.LastMilage.ToString("##,###");
            lblLastDocumentNumber.Text = vd.LastDocumentNumber;
            lblLastDriverName.Text = vd.LastDriverName;
            lblBlockMilage.Text = vd.BlockMilage.HasValue ? vd.BlockMilage.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;
            lblInstallationDate.Text = vd.InstallationDate.HasValue 
                ? vd.InstallationDate.Value.ToShortDateString() 
                : string.Empty;
            lblInstallationMsoDate.Text = vd.InstallationMsoDate.HasValue
                ? vd.InstallationMsoDate.Value.ToShortDateString()
                : string.Empty;
            //lblSaleDate.Text = vd.SaleDate.HasValue ? vd.SaleDate.Value.ToShortDateString() : string.Empty;
            lblHoldFlag.Text = vd.HoldFlag1;
            lblDepreciationStatus.Text = vd.DepreciationStatus;
            lblCarGroupCharged.Text = vd.GroupCharged;
            lblOwningArea.Text = vd.OwningArea.ToString(CultureInfo.InvariantCulture);
            lblPreviousLocationCode.Text = vd.PreviousLocationCode;
            lblDaysInBd.Text = vd.DaysInBd.HasValue ? vd.DaysInBd.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;
            lblDaysInMm.Text = vd.DaysInMm.HasValue ? vd.DaysInMm.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;
            lblBlockDate.Text = vd.BlockDate.HasValue ? vd.BlockDate.Value.ToShortDateString() : string.Empty;
            lblLiscencePlate.Text = vd.LiscencePlate;
            tbVehicleComment.Text = vd.VehicleComment;
            
        }
    }
}