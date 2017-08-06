using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.BLL.DynamicLinq;
using Castle.Components.DictionaryAdapter.Xml;
using Mars.App.Classes.Phase4Dal.Reservations;
using Mars.App.Classes.Phase4Dal.Reservations.Entities;

namespace Mars.App.UserControls.Phase4.Reservations
{
    public partial class ReservationDetails : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void SetReservationDetails(ReservationData resData)
        {
            lblExternalId.Text = resData.ExternalId;
            lblCustomer.Text = resData.CustomerName;
            lblTariff.Text = resData.Tariff;
            lblFlightNumber.Text = resData.FlightNumber;
            lblBookingDate.Text = resData.BookedDate.ToShortDateString();
            lblCheckoutLocation.Text = resData.PickupLocation;
            lblCheckinLocation.Text = resData.ReturnLocation;
            lblCheckoutDate.Text = resData.PickupDate.ToShortDateString();
            lblCheckInDate.Text = resData.ReturnDate.ToShortDateString();
            lblReservationLengthInDays.Text = resData.DaysReserved.ToString();
            lblCarGroupReserved.Text = resData.CarGroupReserved;
            lblCarGroupUpgraded.Text = resData.CarGroupUpgraded;
            lblGoldService.Text = resData.GoldService;
            lblN1Type.Text = resData.N1Type;
            lblNeverlost.Text = resData.NeverLost.HasValue && resData.NeverLost.Value ? "Yes" : "No";
            lblRemark.Text = resData.Remark;
            lblComment.Text = resData.Comment;
            hfReservationId.Value = resData.ReservationId.ToString(CultureInfo.InvariantCulture);
        }

        protected void btnSaveComment_Click(object sender, EventArgs e)
        {
            var comment = lblComment.Text;
            var reservationId = int.Parse(hfReservationId.Value);
            using (var dataContext = new ReservationDataAccess(null))
            {
                dataContext.UpdateReservationComment(reservationId, comment);
            }
            RaiseBubbleEvent(this, new CommandEventArgs("RefreshGrid", null));
        }

        
    }
}