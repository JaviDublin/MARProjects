using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.App.Classes.DAL.MarsDBContext;
using Mars.App.Classes.DAL.Pooling.SharedDataAccess;
using Mars.Entities.Reservations.Abstract;

namespace App.UserControls.Pooling
{
    public partial class ReservationDetailsModal : UserControl
    {

        //public void SetTable(IReservationDetailsEntity Entity)
        //{
        //    string rsArrivalTime = Entity.RS_ARRIVAL_TIME == null ? "" : Entity.RS_ARRIVAL_TIME.Value.ToShortTimeString();
        //    string rtrnTime = Entity.RTRN_TIME == null ? "" : Entity.RTRN_TIME.Value.ToShortTimeString();
        //    spanResId.InnerHtml = Entity.RES_ID_NBR;
        //    tdResId.InnerHtml = Entity.RES_ID_NBR;
        //    tdCheckOutLoc.InnerHtml = Entity.RENT_LOC;
        //    tdCheckOutDate.InnerHtml = Entity.RS_ARRIVAL_DATE == null ? "" : Entity.RS_ARRIVAL_DATE.Value.ToShortDateString() + " " + rsArrivalTime;
        //    tdResLoc.InnerHtml = Entity.RES_LOC;
        //    tdCheckInLoc.InnerHtml = Entity.RTRN_LOC;
        //    tdCheckInDate.InnerHtml = Entity.RTRN_DATE == null ? "" : Entity.RTRN_DATE.Value.ToShortDateString() + " " + rtrnTime;
        //    tdDays.InnerHtml = Entity.RES_DAYS.ToString();
        //    tdGroup.InnerHtml = Entity.GR_INCL_GOLDUPGR;
        //    tdGroupGold.InnerHtml = Entity.GR_INCL_GOLDUPGR;
        //    tdCustomer.InnerHtml = Entity.CUST_NAME;
        //    tdPhone.InnerHtml = Entity.PHONE;
        //    tdCdp.InnerHtml = Entity.CDPID_NBR;
        //    tdGoldNbr.InnerHtml = Entity.NO1_CLUB_GOLD;
        //    tdGoldStatus.InnerHtml = Entity.GR_INCL_GOLDUPGR;
        //    tdFltNbr.InnerHtml = Entity.FLIGHT_NBR;
        //    tdTaco.InnerHtml = Entity.TACO;
        //    tdNeverlost.InnerHtml = Entity.NEVERLOST;
        //    tdPrepaid.InnerHtml = Entity.PREPAID;
        //    tdResDate.InnerHtml = Entity.DATE_SOLD == null ? "" : Entity.DATE_SOLD.Value.ToShortDateString();
        //    tdRemarks.InnerHtml = Entity.REMARKS;
        //    tdTariff.InnerHtml = Entity.RATE_QUOTED;
        //    Visible = true;
        //}

        public void SetReservationTable(IReservationDetailsEntity Entity)
        {
            var goldGroupName = ReservationsDataAccess.LookupGoldLevel(Entity.RES_ID_NBR);

            string rsArrivalTime = Entity.RS_ARRIVAL_TIME == null ? "" : Entity.RS_ARRIVAL_TIME.Value.ToShortTimeString();
            string rtrnTime = Entity.RTRN_TIME == null ? "" : Entity.RTRN_TIME.Value.ToShortTimeString();

            lblResIDValue.Text = Entity.RES_ID_NBR;
            lblCheckOutValue.Text = Entity.RENT_LOC;
            lblCheckOutDateValue.Text = Entity.RS_ARRIVAL_DATE == null ? "" : Entity.RS_ARRIVAL_DATE.Value.ToShortDateString() + " " + rsArrivalTime;

            lblResLocValue.Text = Entity.RES_LOC;



            lblCheckInValue.Text = Entity.RTRN_LOC;
            lblCheckInDateValue.Text = Entity.RTRN_DATE == null ? "" : Entity.RTRN_DATE.Value.ToShortDateString() + " " + rtrnTime;
            lblDaysValue.Text = Entity.RES_DAYS.ToString();
            lblGroupValue.Text = Entity.ReservedCarGroup;
            lblGroupGoldValue.Text = Entity.GR_INCL_GOLDUPGR;
            lblCustomerValue.Text = Entity.CUST_NAME;
            lblPhoneValue.Text = Entity.PHONE;
            lblCDPValue.Text = Entity.CDPID_NBR;
            lblGoldNumberValue.Text = Entity.NO1_CLUB_GOLD;
            lblGoldStatusValue.Text = goldGroupName;
            lblFlightNumberValue.Text = Entity.FLIGHT_NBR;
            lblTacoValue.Text = Entity.TACO;
            lblNeverLostValue.Text = Entity.NEVERLOST;
            lblPrepaidValue.Text = Entity.PREPAID;
            lblResDateValue.Text = Entity.DATE_SOLD == null ? "" : Entity.DATE_SOLD.Value.ToShortDateString();
            lblRemarksValue.Text = Entity.REMARKS;
            lblTariffValue.Text = Entity.RATE_QUOTED;
            Visible = true;
        }



        protected void Page_Load(object sender, EventArgs e)
        {
        }
        protected void ImageButtonClose_Click(object sender, ImageClickEventArgs e)
        {
            Visible = false;
        }
    }
}