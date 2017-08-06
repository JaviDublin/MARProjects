using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using App.BLL.VehiclesAbroad.Models.Abstract;
using System.Web.UI.WebControls;
using App.Classes.DAL.Pooling;

namespace App.Classes.BLL.Pooling.Models.Abstract {
    public interface IDateRangeModel {
        ITextFilterModel StartDateModel { get; set; }
        Label StartDateFeedbackLabel { get; set; }
        ITextFilterModel EndDateModel { get; set; }
        Label EndDateFeedbackLabel { get; set; }
        void SetFeedback();
        void SetDates(App.Classes.DAL.Pooling.Abstract.Enums.SelectedDates sd);
        DateTime StartDate { get; }
        DateTime EndDate { get; }
        int getDayRange();
    }
}
