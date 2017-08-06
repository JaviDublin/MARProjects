using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Classes.DAL.Pooling.Abstract {
    public interface IHeaderRepository {
        string getHeader(App.Classes.DAL.Pooling.Abstract.Enums.Headers header);
    }
}
