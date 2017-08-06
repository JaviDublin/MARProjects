using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using App.Classes.Entities.Pooling.Abstract;

namespace App.Classes.BLL.Pooling.Models.Abstract {
    public interface IPoolingGridViewModel {
        GridView GridViewer { get; set; }
        void bind(params string[] dependants);
        IMainFilterEntity MainFilters { get; set; }
        string SortExpression(string sortString);
    }
}
