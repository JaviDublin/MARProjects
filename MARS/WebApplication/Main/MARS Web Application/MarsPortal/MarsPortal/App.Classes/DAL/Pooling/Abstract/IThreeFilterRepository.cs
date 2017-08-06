using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using App.DAL.VehiclesAbroad.Abstract;

namespace Mars.DAL.Pooling.Abstract {
    public interface IThreeFilterRepository {
        IFilterRepository3 TopRepository { get; set; }
        IFilterRepository3 MiddleRepository { get; set; }
        IFilterRepository3 BottomRepository { get; set; }
    }
}
