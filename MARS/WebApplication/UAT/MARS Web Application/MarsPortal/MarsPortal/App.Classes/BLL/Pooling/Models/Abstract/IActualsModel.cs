using System.Web.UI.HtmlControls;
using App.Classes.Entities.Pooling.Abstract;
using System.Web.UI.WebControls;
using System.Web.UI;
using App.BLL.VehiclesAbroad.Models.Abstract;
using App.Classes.DAL.Reservations.Abstract;
using App.Classes.DAL.Pooling.Abstract;

namespace Mars.Pooling.Models.Abstract {
    public interface IActualsModel {
        Enums.Headers Mode { get; set; }
        HiddenField GridChartHidden { get; set; }
        IFilterModel ColumnMaster { get; set; }
        HtmlGenericControl _HtmlControl { get; set; }
        IMainFilterEntity MainFilters { get; set; }
        void bind(params string[] dependants);
        void findClicked();
        void setJavascript(Page p);
    }
}
