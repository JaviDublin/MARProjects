using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.BLL.VehiclesAbroad.Abstract;


namespace App.UserControls {

    public partial class VehiclesAbroadReservations : System.Web.UI.UserControl {

        public IReservationDetailsRepository ReservationRepository { get; set; }

        protected void Page_Load(object sender, EventArgs e) {

        }

        public void setTable(string resId) {

            if (!string.IsNullOrEmpty(resId) && ReservationRepository != null) {

                var q = ReservationRepository.getDetails(resId);

                tdCpidNbr.InnerHtml = q.CdpidNbr;
                tdCustName.InnerHtml = q.CustName;
                tdFlightNbr.InnerHtml = q.FlightNbr;
                tdGoldType.InnerHtml = q.N1Type;
                tdGrInclGoldUpr.InnerHtml = q.GrInclGoldUpr;
                tdNo1ClubGold.InnerHtml = q.No1ClubGold;
                tdOneWay.InnerHtml = q.Oneway;
                tdPhone.InnerHtml = q.PhoneNbr;
                tdPickUpLoc.InnerHtml = q.ResArrivalTime;
                tdRate.InnerHtml = q.Rate;
                tdRentLoc.InnerHtml = q.RentLoc;
                tdResDays.InnerHtml = q.ResDays;
                tdResNumber.InnerHtml = q.ResIdNumber;
                tdResVehClass.InnerHtml = q.ResVehClass;
                tdRtrnLoc.InnerHtml = q.RtrnLoc;
                tdRtrnTime.InnerHtml = q.RtrnTime;
                tdTaco.InnerHtml = q.Taco;
                remarksText.InnerHtml = q.Remarks;

                UpdatePanelReservations.Visible = true;
            }
        }

        protected void buttonClose_Click(object sender, EventArgs e) {

            UpdatePanelReservations.Visible = false;
        }
    }
}