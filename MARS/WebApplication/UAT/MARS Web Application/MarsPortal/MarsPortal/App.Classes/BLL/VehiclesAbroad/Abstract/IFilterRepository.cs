using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.BLL.VehiclesAbroad.Abstract {

    public interface IFilterRepository {
        IList<string> getOperstat();
        IList<string> getMoveType();
    }
}
