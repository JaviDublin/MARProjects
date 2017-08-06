using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.Classes.DAL.Pooling.Abstract {
    public abstract class Enums {
        public enum SelectedDates { startDate, endDate };
        public enum ErrorMessages { startDateBadFormat, endDateBadFormat, startDateAfterEndDate, endDateBeforeStartDate, startDateBeforeNow, endDateBeforeNow }
        public enum Headers { alerts, threeDayActualStatus, thirtyDayActualStatus, threeSiteComparison,thirtySiteComparison, threeFleetComparison,thirtyFleetComparison, reservationDetails };
        public enum buttons { ThreeDayActual, ThirtyDayActual, SwitchToChart, SwitchToGrid };
        public enum ActualsRows { Balance, Available, OpenTrips, Reservations, OnewayReservations, Gold, Prepaid, CheckIn, OnewayCheckIn,CheckInOffset, Local, Buffer, AdditionsDeletions };
        public enum ThreeDayActuals { ZERO = 0, MAXNOOFROWS = 13, MAXNOOFCOLUMNS = 72 };
        public enum DayActualTime { NONE, THREE, THIRTY };
        public enum HtmlTable { Unknown, Alerts, AlertsPopup, Actuals,SiteComp,FleetComp };
    }
}