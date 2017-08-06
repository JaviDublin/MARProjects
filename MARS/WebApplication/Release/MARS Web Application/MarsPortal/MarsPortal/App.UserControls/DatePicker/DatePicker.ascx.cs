using System;
using App.BLL.EventArgs;

namespace App.UserControls.DatePicker
{
    public partial class DatePicker : System.Web.UI.UserControl
    {
        internal string FromDate { get; set; }
        internal string ToDate { get; set; }
        public bool FutureDatesOnly { get; set; }
        public bool NextNinetyDayOnly { get; set; }
        public bool GridviewDatePicker { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                tbDate.Text = (NextNinetyDayOnly || FutureDatesOnly) ? 
                    DateTime.Today.AddDays(1).ToShortDateString() : DateTime.Today.ToShortDateString();
                
                RangeValidator1.MinimumValue = (NextNinetyDayOnly || FutureDatesOnly) ? 
                    DateTime.Today.AddDays(1).ToShortDateString() : DateTime.Today.ToShortDateString();
                
                RangeValidator1.MaximumValue = NextNinetyDayOnly ? DateTime.Today.AddDays(91).ToShortDateString() : DateTime.MaxValue.ToShortDateString();

                RangeValidator1.Text = NextNinetyDayOnly ? "Date must be within tomorrow and the next 90 days" : (FutureDatesOnly ? "Historical dates are not allowed" : "");
                
                btnPrevDay.Enabled = (NextNinetyDayOnly || FutureDatesOnly) ? false : true;
            }

            FromDate = tbDate.Text;
            ToDate = tbDate.Text;
             
        }
        protected void DateChanged(object sender, EventArgs e)
        {
            CheckDateRange(0);            
        }

        protected void btnNextDay_Click(object sender, EventArgs e)
        {
            
            CheckDateRange(1);
        }

        protected void btnPrevDay_Click(object sender, EventArgs e)
        {           
            CheckDateRange(-1);
        }

        private void CheckDateRange(int changeDays)
        {

            DateTime tomorrow = DateTime.Today.AddDays(1);
            tbDate.Text = Convert.ToDateTime(tbDate.Text).AddDays(changeDays).ToShortDateString();

            if (NextNinetyDayOnly || FutureDatesOnly)
            {               

                if (Convert.ToDateTime(tbDate.Text).CompareTo(tomorrow) <= 0)
                    btnPrevDay.Enabled = false;
                else
                    btnPrevDay.Enabled = true;

                if (NextNinetyDayOnly)
                {
                    if (Convert.ToDateTime(tbDate.Text).CompareTo(tomorrow.AddDays(90)) >= 0)
                        btnNextDay.Enabled = false;
                    else
                        btnNextDay.Enabled = true;
                }
            }


            RangeValidator1.Validate();

            if (RangeValidator1.IsValid)
            {                
                if (GridviewDatePicker)
                    RaiseBubbleEvent(this, new ParameterChangeEventArgs { date = tbDate.Text });
                else
                    RaiseBubbleEvent(this, new RefreshGraphEventArgs());
            }
        }
    }
}