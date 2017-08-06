using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using Mars.Pooling.Models.Abstract;

namespace App.Classes.BLL.Pooling.Models.Abstract {
    public interface ICmsOpsLogicModel {
        RadioButton CMSRadioButton { get; set; }
        RadioButton OPSRadioButton { get; set; }
        Label TopLabel0 { get; set; }
        Label TopLabel1 { get; set; }
        Label BottomLabel0 { get; set; }
        Label BottomLabel1 { get; set; }
        IThreeFilterCascadeModel GeneralThreeFilterModel { get; set; }
        IFilterModel3 CountryFilterModel { get; set; }
        void Initialise(Page p);
        bool isOPS { get; }
        bool isCMS { get; }
        void SetToOps();
        void SetToCms();
    }
}
