using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mars.DAL.Sizing.Abstract {
    public enum FleetPlanOptions {
        // Note resembles the database table Export.Status
        None = 1,
        Running,
        Complete,
        Failed
    }
}