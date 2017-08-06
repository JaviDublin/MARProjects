using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using App.Classes.Entities.Pooling.Abstract;
using App.Classes.DAL.Pooling.Abstract;

namespace Mars.DAL.Pooling.Abstract {
    public interface IComparisonRepository {
        IMainFilterEntity Filter { get; set; }
        IList<String[]> GetList(Enums.DayActualTime Tme);
    }
}
