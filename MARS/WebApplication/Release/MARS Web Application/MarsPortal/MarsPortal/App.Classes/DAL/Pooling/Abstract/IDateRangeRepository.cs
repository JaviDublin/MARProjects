using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Classes.DAL.Pooling.Abstract {
    public interface IDateRangeRepository {
        string getErrorMessage(App.Classes.DAL.Pooling.Abstract.Enums.ErrorMessages error);
        IDictionary<App.Classes.DAL.Pooling.Abstract.Enums.ErrorMessages, string> getDictionary();
    }
}
