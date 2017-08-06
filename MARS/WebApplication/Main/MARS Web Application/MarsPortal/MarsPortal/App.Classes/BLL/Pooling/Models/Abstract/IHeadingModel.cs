using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace App.Classes.BLL.Pooling.Models.Abstract {
    public interface IHeadingModel {
        Label HeadingLabel { get; set; }
        string Text { get; }
        void setText(App.Classes.DAL.Pooling.Abstract.Enums.Headers h);
    }
}
