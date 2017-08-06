using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Classes.BLL.Pooling.Models.Abstract {
    public interface IJavaScriptModel {
        void SetServiceReference(System.Web.UI.Page p);
        void SetJavaScriptService(System.Web.UI.Page p);
    }
}
