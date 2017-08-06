using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.Entities.Pooling;

using App.Classes.Entities.Pooling.Abstract;

namespace Mars.DAL.Pooling.Abstract {
    public interface IOverdueActualRepository {
        string GetItem(IMainFilterEntity filter, int NumberOfDays);
    }
}