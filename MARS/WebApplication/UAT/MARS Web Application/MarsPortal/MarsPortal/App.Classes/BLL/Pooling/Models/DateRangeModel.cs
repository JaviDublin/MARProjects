using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Classes.BLL.Pooling.Models.Abstract;
using App.BLL.VehiclesAbroad.Models.Abstract;
using System.Web.UI.WebControls;
using App.BLL.VehiclesAbroad.Models.Filters;
using App.Classes.DAL.Pooling;
using App.Classes.DAL.Pooling.Abstract;

namespace App.Classes.BLL.Pooling.Models {
    public class DateRangeModel : IDateRangeModel {

        IDateRangeRepository _repository;
        public DateRangeModel(IDateRangeRepository r) {
            _repository = r;
            StartDateModel = new TextFilterModel();
            EndDateModel = new TextFilterModel();
        }
        public Label EndDateFeedbackLabel { get; set; }
        public Label StartDateFeedbackLabel { get; set; }
        public ITextFilterModel StartDateModel { get; set; }
        public ITextFilterModel EndDateModel { get; set; }

        public void SetFeedback() {
            EndDateFeedbackLabel.Font.Bold = true;
           // EndDateFeedbackLabel.ForeColor = System.Drawing.Color.Blue;
            EndDateFeedbackLabel.Text = EndDateModel.Text;
            StartDateFeedbackLabel.Font.Bold = true;
            //StartDateFeedbackLabel.ForeColor = System.Drawing.Color.Blue;
            StartDateFeedbackLabel.Text = StartDateModel.Text;
        }
        public void SetDates(App.Classes.DAL.Pooling.Abstract.Enums.SelectedDates dateSelected) {
            StartDateModel.ErrorText = "";
            EndDateModel.ErrorText = "";
            if (dateSelected == App.Classes.DAL.Pooling.Abstract.Enums.SelectedDates.startDate) {
                try {
                    if (getDayRange() < 0) {
                        StartDateModel.Text = EndDateModel.Text;
                        StartDateModel.ErrorText = _repository.getErrorMessage(App.Classes.DAL.Pooling.Abstract.Enums.ErrorMessages.startDateAfterEndDate);
                    }
                    if (StartDate.Date < DateTime.Now.Date) {
                        StartDateModel.Text = DateTime.Now.ToShortDateString();
                        StartDateModel.ErrorText = _repository.getErrorMessage(App.Classes.DAL.Pooling.Abstract.Enums.ErrorMessages.startDateBeforeNow);
                    }
                }
                catch (FormatException) {
                    StartDateModel.Text = DateTime.Now.ToShortDateString();
                    StartDateModel.ErrorText = _repository.getErrorMessage(App.Classes.DAL.Pooling.Abstract.Enums.ErrorMessages.startDateBadFormat);
                }
            }
            else {
                try {
                    if (getDayRange() < 0) {
                        EndDateModel.Text = StartDateModel.Text;
                        EndDateModel.ErrorText = _repository.getErrorMessage(App.Classes.DAL.Pooling.Abstract.Enums.ErrorMessages.endDateBeforeStartDate);
                    }
                }
                catch (FormatException) {
                    EndDateModel.Text = DateTime.Now.ToShortDateString();
                    EndDateModel.ErrorText = _repository.getErrorMessage(App.Classes.DAL.Pooling.Abstract.Enums.ErrorMessages.endDateBadFormat);
                }
            }
        }
        public DateTime StartDate {
            get { return getDate(StartDateModel.Text); }
        }
        public DateTime EndDate {
            get { return getDate(EndDateModel.Text); }
        }
        public int getDayRange() {
            TimeSpan ts = EndDate - StartDate;
            return ts.Days;
        }
        DateTime getDate(string date) {
            if (string.IsNullOrEmpty(date)) return DateTime.Now.Date;
            DateTime dt = new DateTime();
            if (DateTime.TryParse(date, out dt)) return dt;
            throw new FormatException();
        }
    }
}