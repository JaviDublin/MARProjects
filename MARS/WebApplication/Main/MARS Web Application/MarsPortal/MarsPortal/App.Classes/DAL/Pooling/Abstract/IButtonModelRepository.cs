using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace App.Classes.DAL.Pooling.Abstract {
    public interface IButtonModelRepository {
        string getUri();
        string getLabel();
        string getLabel(Enums.buttons b);
    }
}
