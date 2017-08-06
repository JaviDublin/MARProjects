using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App.UserControls.DatePicker
{
    public partial class DatePickerTextBoxForm : System.Web.UI.UserControl
    {
        private string _validationGroup = "GenerateReport";
        private string _errorImageSrc = "~/App.Images/error.png";
        private DateTime _startDate = DateTime.Now;
        private bool _setDefaultDate = true;

        private DateTime _defaultDate = DateTime.Now;
        public string ValidationGroup
        {
            get { return _validationGroup; }
            set { _validationGroup = value; }
        }

        public string ErrorImageSrc
        {
            get { return _errorImageSrc; }
            set { _errorImageSrc = value; }
        }

        public string StartDate
        {
            get { return _startDate.ToString(); }
            set { _startDate = Convert.ToDateTime(value); TextBoxDatePicker.Text = StartDate.Substring(0, 10); }
        }

        public bool SetDefaultDate
        {
            get { return _setDefaultDate; }
            set { _setDefaultDate = value; }
        }

        public DateTime DefaultDate
        {
            get { return _defaultDate; }
            set { _defaultDate = value; }
        }
        public bool Enabled
        {
            get { return TextBoxDatePicker.Enabled; }
            set
            {
                ImageCalendar.Enabled = value; ImageCalendar.ImageUrl = value ? @"~/App.Images/calendar.png" : "";
                TextBoxDatePicker.Enabled = value;
            }
        }

        public new bool Visible
        {
            get { return TextBoxDatePicker.Visible; }
            set { TextBoxDatePicker.Visible = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (_setDefaultDate)
                {
                    if (!(_startDate == null))
                    {
                        this.TextBoxDatePicker.Text = string.Format("{0:dd/MM/yyyy}", _startDate);
                    }
                    else
                    {
                        this.TextBoxDatePicker.Text = string.Format("{0:dd/MM/yyyy}", DateTime.Now);
                    }
                }

            }

            //Set Validator Properties
            this.CustomValidatorDate.ValidationGroup = _validationGroup;
            this.CustomValidatorDate.ToolTip = Resources.lang.DatePickerError;
            this.CustomValidatorDate.Text = "<img src='" + this.Page.ResolveUrl(_errorImageSrc) + "'>";

        }

        public void SetDateDefault()
        {
            if (!(_defaultDate == null))
            {
                this.TextBoxDatePicker.Text = string.Format("{0:dd/MM/yyyy}", _defaultDate);
            }
            else
            {
                this.TextBoxDatePicker.Text = string.Empty;
            }
        }
        public DateTime getDate()
        { // Return the date on of what's in the textbox as DateTime
            return Convert.ToDateTime(TextBoxDatePicker.Text);
        }
    }
}