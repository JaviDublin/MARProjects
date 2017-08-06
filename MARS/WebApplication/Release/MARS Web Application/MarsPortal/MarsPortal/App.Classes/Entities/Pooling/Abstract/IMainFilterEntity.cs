using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Classes.Entities.Pooling.Abstract {
    public interface IMainFilterEntity {
        string Country { get; set; }
        bool CmsLogic { get; set; }
        string PoolRegion { get; set; }
        string LocationGrpArea { get; set; }
        string Branch { get; set; }
        string CarSegment { get; set; }
        string CarClass { get; set; }
        string CarGroup { get; set; }
        string LocAndGoldCar { get; set; }
        string Topic { get; set; }
        bool ExcludeLongterm { get; set; }
    }
}
