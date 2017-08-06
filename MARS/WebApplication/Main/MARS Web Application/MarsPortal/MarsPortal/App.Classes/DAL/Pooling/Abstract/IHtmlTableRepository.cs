using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using App.Classes.Entities.Pooling.Abstract;

namespace Mars.DAL.Pooling.Abstract {
    interface IHtmlTableRepository <T> {
        IMainFilterEntity Filter { get; set; }
        IList <T> GetTable(params String [] s);
    }
}
