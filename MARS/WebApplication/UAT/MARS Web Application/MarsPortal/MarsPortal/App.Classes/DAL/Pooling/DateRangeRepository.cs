using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Classes.DAL.Pooling.Abstract;

namespace App.Classes.DAL.Pooling {
    public class DateRangeRepository : IDateRangeRepository {

        public string getErrorMessage(App.Classes.DAL.Pooling.Abstract.Enums.ErrorMessages error) {
            return getDictionary()[error];
        }
        public IDictionary<App.Classes.DAL.Pooling.Abstract.Enums.ErrorMessages, string> getDictionary() {
            IDictionary<App.Classes.DAL.Pooling.Abstract.Enums.ErrorMessages, string> dic = new Dictionary<App.Classes.DAL.Pooling.Abstract.Enums.ErrorMessages, string>();
            dic.Add(App.Classes.DAL.Pooling.Abstract.Enums.ErrorMessages.startDateBadFormat, "<br />The start date is not in the correct format.");
            dic.Add(App.Classes.DAL.Pooling.Abstract.Enums.ErrorMessages.startDateAfterEndDate, "<br />The start date is after the end date.");
            dic.Add(App.Classes.DAL.Pooling.Abstract.Enums.ErrorMessages.endDateBadFormat, "<br />The end date is not in the correct format.");
            dic.Add(App.Classes.DAL.Pooling.Abstract.Enums.ErrorMessages.endDateBeforeStartDate, "<br />The end date is before the start date.");
            dic.Add(App.Classes.DAL.Pooling.Abstract.Enums.ErrorMessages.startDateBeforeNow, "<br />The start date can't be before today.");
            return dic;
        }

    }
}