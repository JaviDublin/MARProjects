using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using App.BLL.VehiclesAbroad.Models.Abstract;
using App.Classes.DAL.Pooling.Abstract;
using System.Web.UI;

namespace App.Classes.BLL.Pooling.Models.Abstract {
    public interface IButtonModel2 : IButtonModel {
        string getRedirectUrl(); // set via the repository
        void setLabel();
        IButtonModelRepository Repository { get; set; }
    }
}
