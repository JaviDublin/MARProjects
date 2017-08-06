using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mars.Entities.Pooling;

namespace Mars.DAL.Pooling.Abstract {
    public interface IDoPostBackRepository {
        IList<ResDetailsHeadingEntity> GetList();
    }
}
