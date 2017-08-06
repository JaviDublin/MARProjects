using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mars.Entities.Sizing.Abstract {
    public interface IFleetPlanEntity {
        String Message { get; set; }
        String Status { get; set; }
    }
}
