using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using App.Classes.BLL.Pooling.Models.Abstract;
using Mars.DAL.Pooling.Abstract;

namespace Mars.Pooling.Models.Abstract {
    public interface IFilterModel3:IFilterModel2 {
        int GetId();
        string GetCode();
        IFilterRepository3 Repository3 { get; set; }
    }
}
