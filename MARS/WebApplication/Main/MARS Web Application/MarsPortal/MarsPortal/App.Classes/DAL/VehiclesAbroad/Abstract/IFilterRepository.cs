using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace App.DAL.VehiclesAbroad.Abstract {

    public interface IFilterRepository{
        IList<string> getList(params string[] dependants);
    }
}
