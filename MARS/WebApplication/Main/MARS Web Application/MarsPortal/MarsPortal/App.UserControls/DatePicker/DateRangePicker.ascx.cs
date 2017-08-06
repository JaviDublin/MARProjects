using System;
using System.Web.UI.WebControls;
using App.BLL.EventArgs;
using Resources;

namespace App.UserControls.DatePicker
{
    public partial class DateRangePicker : System.Web.UI.UserControl
    {
        internal bool HistoricalDateRange { get; set; }
        internal string DefaultDateRangeValueSelected { get; set; }
        internal string SelectedDateRangeType { get { return ddlDateRange.SelectedValue; } }
        internal string FromDate { get; set; }
        internal string ToDate { get; set; }
        internal bool ShowThisWeekAndNextWeekInDropdown { get; set; }
        public bool FutureDatesOnly { get; set; }
        public bool GridviewDatePicker { get; set; }
        public bool PopupPositionTop { get; set; }

        internal System.Web.UI.WebControls.DropDownList DateRangeControl
        {
            get { return ddlDateRange; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbFromDate.Text))
            {
                tbFromDate.Text = FutureDatesOnly ?  DateTime.Now.AddDays(1).ToShortDateString() : DateTime.Now.ToShortDateString();
                tbToDate.Text = DateTime.Now.ToShortDateString();
            }
            if (!IsPostBack)
            {
                if(!string.IsNullOrEmpty(DefaultDateRangeValueSelected))
                    ddlDateRange.SelectedValue = DefaultDateRangeValueSelected;

                if (PopupPositionTop)
                {
                    ceFromDateExtender.PopupPosition = AjaxControlToolkit.CalendarPosition.TopLeft;
                    ceToDateExtender.PopupPosition = AjaxControlToolkit.CalendarPosition.TopLeft;
                }

                RangeValidator1.MinimumValue = FutureDatesOnly ? DateTime.Today.AddDays(1).ToShortDateString() : DateTime.MinValue.ToShortDateString();
                RangeValidator1.MaximumValue = DateTime.MaxValue.ToShortDateString();
                RangeValidator1.Text = FutureDatesOnly ? "Historical dates are not allowed" : "";                
            }
            SetDates(HistoricalDateRange);
        }

        protected void DateChanged(object sender, EventArgs e)
        {
            CheckDateRange(0); 
        }

        internal void SetToAndFromDateTextBoxes(string fromDate ,string toDate)
        {
            tbFromDate.Text = fromDate;
            tbToDate.Text = toDate;
        }

        private void CheckDateRange(int changeDays)
        {

            DateTime tomorrow = DateTime.Today.AddDays(1);
            tbFromDate.Text = Convert.ToDateTime(tbFromDate.Text).AddDays(changeDays).ToShortDateString();

            RangeValidator1.Validate();

            if (RangeValidator1.IsValid)
            {
                if (!GridviewDatePicker)
                    RaiseBubbleEvent(this, new RefreshGraphEventArgs());
            }
        }

        internal void SetDates(bool historicalRange)
        {
            ddlDateRange.Items.FindByValue("ThisWeek").Enabled = ShowThisWeekAndNextWeekInDropdown;
            ddlDateRange.Items.FindByValue("NextWeek").Enabled = ShowThisWeekAndNextWeekInDropdown;

            lblToDate.Visible = false;
            lblStartDate.Visible = false;
            tbToDate.Visible = false;
            tbFromDate.Visible = false;

            DateTime start, end;

            switch(ddlDateRange.SelectedValue)
            {
                case "Custom":
                    FromDate = tbFromDate.Text;
                    lblToDate.Visible = true;
                    lblStartDate.Visible = true;
                    tbToDate.Visible = true;
                    tbFromDate.Visible = true;
                    lblStartDate.Text = LocalizedParameterControl.DatePickerFromLabel;
                    ToDate = tbToDate.Text;
                    break;
                case "ThisWeek":
                    start = DateTime.Now.AddDays(-(DateTime.Now.DayOfWeek - DayOfWeek.Monday));
                    FromDate = start.ToShortDateString();
                    end = start.AddDays(6);
                    ToDate = end.ToShortDateString();
                    break;
                case "NextWeek":
                    start = DateTime.Now.AddDays(-(DateTime.Now.DayOfWeek - DayOfWeek.Monday) + 7);
                    FromDate = start.ToShortDateString();
                    end = start.AddDays(6);
                    ToDate = end.ToShortDateString();
                    break;
                default:
                    var from = DateTime.Parse(tbFromDate.Text);
                    tbFromDate.Visible = true;
                    lblStartDate.Visible = true;

                    var daysAdded = int.Parse(ddlDateRange.SelectedValue);
                    if (historicalRange)
                    {
                        lblStartDate.Text = LocalizedParameterControl.DatePickerHistoricalFromDate;
                        daysAdded *= -1;
                        FromDate = from.AddDays(daysAdded).ToShortDateString();
                        ToDate = tbFromDate.Text; 
                    }
                    else
                    {
                        lblStartDate.Text = LocalizedParameterControl.DatePickerFromLabel;
                        FromDate = tbFromDate.Text;
                        ToDate = from.AddDays(daysAdded).ToShortDateString();
                    }       
                    break;
            }

        }
    }
}