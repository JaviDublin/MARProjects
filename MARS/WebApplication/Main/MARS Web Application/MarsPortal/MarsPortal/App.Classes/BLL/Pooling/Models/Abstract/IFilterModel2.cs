using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using App.BLL.VehiclesAbroad.Models.Abstract;
using System.Web.UI.WebControls;

namespace App.Classes.BLL.Pooling.Models.Abstract {
    public interface IFilterModel2 : IFilterModel { // add extra functionality to the Vehicles abroad filtermodel
        Label ErrorLabel { get; set; }
        Label FeedbackLabel { get; set; }
        void SetFeedback();
        //int GetIndex(string s); //attempts to find the index or returns -1
    }
}
