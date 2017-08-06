using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Classes.DAL.Reservations.Queryables.SortRepository.Abstract {
    public interface ISortRepository {
        string getValue(string key);
    }
}
