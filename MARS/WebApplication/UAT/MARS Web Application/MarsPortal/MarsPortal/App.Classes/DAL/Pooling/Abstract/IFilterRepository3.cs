using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mars.Pooling.Entities;

namespace Mars.DAL.Pooling.Abstract {
    public interface IFilterRepository3 {
        IList<DropdownEntity> getList(params string[] dependants);
    }
}
